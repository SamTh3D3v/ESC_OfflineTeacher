using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;

namespace SyncLibrary
{   
    public interface ISyncService
    {
        // Defines the first method to call when using SessionMode
        
        void Initialize(string scopeName);
        
        DbSyncScopeDescription GetScopeDescription();
        
        void BeginSession(SyncProviderPosition position);
        
        SyncBatchParameters GetKnowledge();
        
        GetChangesParameters GetChanges(uint batchSize, SyncKnowledge destinationKnowledge);
        
        SyncSessionStatistics ApplyChanges(ConflictResolutionPolicy resolutionPolicy, ChangeBatch sourceChanges, object changeData);
        
        bool HasUploadedBatchFile(string batchFileid, string remotePeerId);
        
        void UploadBatchFile(string batchFileid, byte[] batchFile,
            string remotePeerId);
        
        byte[] DownloadBatchFile(string batchFileId);
        
        void EndSession();
        //Indicates the last method to call when use SessionMode        
        void Cleanup();
    }
    
    public class SyncBatchParameters
    {        
        public SyncKnowledge DestinationKnowledge;
        
        public uint BatchSize;
    }

   
    public class GetChangesParameters
    {
        
        public object DataRetriever;
        
        public ChangeBatch ChangeBatch;
    }
    
    public class WebSyncFaultException
    {
        private string _message;
        private Exception _innerException;

        public WebSyncFaultException(string message, Exception innerException)
        {
            this._message = message;
            this._innerException = innerException;
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
        
        public Exception InnerException
        {
            get
            {
                return _innerException;
            }

            set
            {
                _innerException = value;
            }
        }
    }
}
