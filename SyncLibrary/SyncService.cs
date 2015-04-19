using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;

namespace SyncLibrary
{
    public class SyncService : ISyncService
    {
        protected bool isProxyToCompactDatabase;
        protected SqlSyncProvider sqlProvider;
        protected DirectoryInfo sessionBatchingDirectory = null;
        protected Dictionary<string, string> batchIdToFileMapper;
        int batchCount = 0;
        public void Initialize(string scopeName)
        {
            ServerSynchronizationHelper helper = new ServerSynchronizationHelper();
            this.sqlProvider = helper.ConfigureSqlSyncProvider(scopeName);
            this.batchIdToFileMapper = new Dictionary<string, string>();
        }
        public DbSyncScopeDescription GetScopeDescription()
        {
            Log("GetSchema: {0}", this.sqlProvider.Connection.ConnectionString);

            DbSyncScopeDescription scopeDesc =
                SqlSyncDescriptionBuilder.GetDescriptionForScope(
                this.sqlProvider.ScopeName, (SqlConnection)this.sqlProvider.Connection);
            return scopeDesc;
        }
        public void BeginSession(Microsoft.Synchronization.SyncProviderPosition position)
        {
            Log("*****************************************************************");
            Log("******************** New Sync Session ***************************");
            Log("*****************************************************************");
            Log("BeginSession: ScopeName: {0}, Position: {1}",
                this.sqlProvider.ScopeName, position);
            //Clean the mapper for each session.
            this.batchIdToFileMapper = new Dictionary<string, string>();

            this.sqlProvider.BeginSession(position, null/*SyncSessionContext*/);
            this.batchCount = 0;
        }
        public SyncBatchParameters GetKnowledge()
        {
            Log("GetSyncBatchParameters: {0}", this.sqlProvider.Connection.ConnectionString);
            SyncBatchParameters destParameters = new SyncBatchParameters();
            this.sqlProvider.GetSyncBatchParameters(out destParameters.BatchSize,
                out destParameters.DestinationKnowledge);
            return destParameters;
        }
        public GetChangesParameters GetChanges(uint batchSize,
                Microsoft.Synchronization.SyncKnowledge destinationKnowledge)
        {
            Log("GetChangeBatch: {0}", this.sqlProvider.Connection.ConnectionString);
            GetChangesParameters changesWrapper = new GetChangesParameters();
            changesWrapper.ChangeBatch = this.sqlProvider.GetChangeBatch(batchSize,
                                    destinationKnowledge, out changesWrapper.DataRetriever);

            DbSyncContext context = changesWrapper.DataRetriever as DbSyncContext;
            //Check to see if data is batched
            if (context != null && context.IsDataBatched)
            {
                Log("GetChangeBatch: Data Batched. Current Batch #:{0}", ++this.batchCount);
                //Dont send the file location info. Just send the file name
                string fileName = new FileInfo(context.BatchFileName).Name;
                this.batchIdToFileMapper[fileName] = context.BatchFileName;
                context.BatchFileName = fileName;
            }
            return changesWrapper;
        }
        public Microsoft.Synchronization.SyncSessionStatistics ApplyChanges(
            Microsoft.Synchronization.ConflictResolutionPolicy resolutionPolicy,
            Microsoft.Synchronization.ChangeBatch sourceChanges, object changeData)
        {
            Log("ProcessChangeBatch: {0}", this.sqlProvider.Connection.ConnectionString);

            DbSyncContext dataRetriever = changeData as DbSyncContext;

            if (dataRetriever != null && dataRetriever.IsDataBatched)
            {
                string remotePeerId = dataRetriever.MadeWithKnowledge.ReplicaId.ToString();
                //Data is batched. The client should have uploaded this file to us prior to 
                //calling ApplyChanges.
                //So look for it.
                //The Id would be the DbSyncContext.BatchFileName which is just the batch 
                //file name without the complete path
                string localBatchFileName = null;
                if (!this.batchIdToFileMapper.TryGetValue(dataRetriever.BatchFileName,
                    out localBatchFileName))
                {
                    //Service has not received this file. Throw exception
                    throw new Exception("No batch file uploaded for id " +
                            dataRetriever.BatchFileName,
                            null);
                }
                dataRetriever.BatchFileName = localBatchFileName;
            }

            SyncSessionStatistics sessionStatistics = new SyncSessionStatistics();
            this.sqlProvider.ProcessChangeBatch(resolutionPolicy, sourceChanges,
                changeData,
                new SyncCallbacks(), sessionStatistics);
            return sessionStatistics;
        }
        public bool HasUploadedBatchFile(String batchFileId, string remotePeerId)
        {
            this.CheckAndCreateBatchingDirectory(remotePeerId);

            //The batchFileId is the fileName without the path information in it.
            FileInfo fileInfo = new FileInfo(Path.Combine(
                this.sessionBatchingDirectory.FullName,
                batchFileId));
            if (fileInfo.Exists && !this.batchIdToFileMapper.ContainsKey(batchFileId))
            {
                //If file exists but is not in the memory id to location mapper 
                //then add it to the mapping
                this.batchIdToFileMapper.Add(batchFileId, fileInfo.FullName);
            }
            //Check to see if the proxy has already uploaded this file to the service
            return fileInfo.Exists;
        }
        public void UploadBatchFile(string batchFileId, byte[] batchContents,
            string remotePeerId)
        {
            Log("UploadBatchFile: {0}", this.sqlProvider.Connection.ConnectionString);
            try
            {
                if (HasUploadedBatchFile(batchFileId, remotePeerId))
                {
                    //Service has already received this file. So dont save it again.
                    return;
                }

                //Service hasnt seen the file yet so save it.
                String localFileLocation = Path.Combine(
                    sessionBatchingDirectory.FullName, batchFileId);
                FileStream fs = new FileStream(localFileLocation,
                    FileMode.Create, FileAccess.Write);
                using (fs)
                {
                    fs.Write(batchContents, 0, batchContents.Length);
                }
                //Save this Id to file location mapping in the mapper object
                this.batchIdToFileMapper[batchFileId] = localFileLocation;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to save batch file.", e);
            }
        }
        public byte[] DownloadBatchFile(string batchFileId)
        {
            try
            {
                Log("DownloadBatchFile: {0}",
                    this.sqlProvider.Connection.ConnectionString);
                Stream localFileStream = null;

                string localBatchFileName = null;

                if (!this.batchIdToFileMapper.TryGetValue(batchFileId,
                    out localBatchFileName))
                {
                    throw new Exception("Unable to retrieve batch file for id."
                            + batchFileId, null);
                }

                using (localFileStream = new FileStream(localBatchFileName,
                    FileMode.Open, FileAccess.Read))
                {
                    byte[] contents = new byte[localFileStream.Length];
                    localFileStream.Read(contents, 0, contents.Length);
                    return contents;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to read batch file for id " +
                        batchFileId, e);
            }
        }
        public void EndSession()
        {
            Log("EndSession: {0}", this.sqlProvider.Connection.ConnectionString);
            Log("*****************************************************************");
            Log("******************** End Sync Session ***************************");
            Log("*****************************************************************");
            this.sqlProvider.EndSession(null);
            Log("");
        }
        public void Cleanup()
        {
            this.sqlProvider = null;
            //Delete all file in the temp session directory
            if (sessionBatchingDirectory != null)
            {
                sessionBatchingDirectory.Refresh();

                if (sessionBatchingDirectory.Exists)
                {
                    try
                    {
                        sessionBatchingDirectory.Delete(true);
                    }
                    catch
                    {
                        //Ignore 
                    }
                }
            }
        }
        protected void Log(string p, params object[] paramArgs)
        {
            Console.WriteLine(p, paramArgs);
        }
        private void CheckAndCreateBatchingDirectory(string remotePeerId)
        {
            //Check to see if we have temp directory for this session.
            if (sessionBatchingDirectory == null)
            {
                //Generate a unique Id for the directory
                //We use the peer id of the store enumerating the changes so that the 
                //local temp directory is same for a given source
                //across sync sessions. This enables us to restart a failed sync by not 
                //downloading already received files.
                string sessionDir = Path.Combine(this.sqlProvider.BatchingDirectory,
                            "WebSync_" + remotePeerId);
                sessionBatchingDirectory = new DirectoryInfo(sessionDir);
                //Create the directory if it doesnt exist.
                if (!sessionBatchingDirectory.Exists)
                {
                    sessionBatchingDirectory.Create();
                }
            }
        }

    }
}
