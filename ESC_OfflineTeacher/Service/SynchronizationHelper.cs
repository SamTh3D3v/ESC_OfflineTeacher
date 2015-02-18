using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data.SqlServerCe;
using Microsoft.Synchronization.Data;
using System.Threading;

namespace ESC_OfflineTeacher.Service
{
    class SyncResults
    {
        private SyncOperationStatistics stats;

        public SyncOperationStatistics Stats
        {
            get { return stats; }
            set { stats = value; }
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public SyncResults(string message,
            SyncOperationStatistics stats)
        {
            this.message = message;
            this.stats = stats;
        }
    }

    class SynchronizationHelper
    {
        public void SynchronizeAsync(
            System.ComponentModel.BackgroundWorker worker,
            System.ComponentModel.DoWorkEventArgs e)
        {

            SyncResults results;
            results = SynchronizeProductsEmployees();
            worker.ReportProgress(0, results);
            results = SynchronizeEmployeeSales();
            worker.ReportProgress(0, results);
        }




        public SyncResults SynchronizeEtudiantProfMat()
        {
            SyncResults results = null;
            // Create the SQL CE Sync Provider for the given scope name
            SqlCeSyncProvider localProvider =
                ConfigureCESyncProvider("EtudiantProfMat");
            // Create the remote provider for the given scope name
            RelationalProviderProxy destinationProxy =
                new RelationalProviderProxy("EtudiantProfMat",
                    Properties.Settings.Default.ServiceUrl);
            // Synchronize and collect results
            results = new SyncResults("EtudiantProfMat",
                SynchronizeProviders(localProvider, destinationProxy,
                SyncDirectionOrder.Download));
            destinationProxy.Dispose();
            localProvider.Dispose();
            return results;
        }

        public SyncResults SynchronizeNote()
        {
            SyncResults results = null;
            // Create the SQL CE Sync Provider for the given scope name
            SqlCeSyncProvider localProvider =
                ConfigureCESyncProvider("Note");
            // Create the remote provider for the given scope name
            RelationalProviderProxy destinationProxy =
                new RelationalProviderProxy("Note",
                    Properties.Settings.Default.ServiceUrl);
            // Synchronize and collect results
            results = new SyncResults("Note",
                SynchronizeProviders(localProvider,
                destinationProxy, SyncDirectionOrder.UploadAndDownload));
            destinationProxy.Dispose();
            localProvider.Dispose();
            return results;
        }


        /// <summary>
        /// Utility function that configures a CE provider
        /// </summary>
        /// <param name="sqlCeConnection"></param>
        /// <returns></returns>
        private SqlCeSyncProvider ConfigureCESyncProvider(string scopeName)
        {
            SqlCeConnection sqlCeConnection = new SqlCeConnection(
                Properties.Settings.Default.DBConnection);
            SqlCeSyncProvider provider = new SqlCeSyncProvider();
            //Set the scope name
            provider.ScopeName = scopeName;

            //Set the connection.
            provider.Connection = sqlCeConnection;

            //Thats it. We are done configuring the CE provider.
            return provider;
        }

        /// <summary>
        /// Utility function that will create a SyncOrchestrator and 
        /// synchronize the two passed in providers
        /// </summary>
        /// <param name="localProvider">Local store provider</param>
        /// <param name="remoteProvider">Remote store provider</param>
        /// <returns></returns>
        private SyncOperationStatistics SynchronizeProviders(
            KnowledgeSyncProvider localProvider, KnowledgeSyncProvider remoteProvider,
            SyncDirectionOrder direction)
        {
            SyncOrchestrator orchestrator = new SyncOrchestrator();
            orchestrator.LocalProvider = localProvider;
            orchestrator.RemoteProvider = remoteProvider;
            orchestrator.Direction = direction;
            //Check to see if any provider is a SqlCe provider and if it needs schema
            CheckIfProviderNeedsSchema(localProvider as SqlCeSyncProvider);
            SyncOperationStatistics stats = orchestrator.Synchronize();
            return stats;
        }

        /// <summary>
        /// Check to see if the passed in CE provider needs Schema from server
        /// </summary>
        /// <param name="localProvider"></param>
        private void CheckIfProviderNeedsSchema(SqlCeSyncProvider localProvider)
        {

            if (localProvider != null)
            {
                SqlCeConnection ceConn = (SqlCeConnection)localProvider.Connection;
                SqlCeSyncScopeProvisioning ceConfig =
                    new SqlCeSyncScopeProvisioning(ceConn);

                string scopeName = localProvider.ScopeName;

                //if the scope does not exist in this store
                if (!ceConfig.ScopeExists(scopeName))
                {
                    //create a reference to the server proxy
                    RelationalProviderProxy serverProxy =
                        new RelationalProviderProxy(scopeName,
                            Properties.Settings.Default.ServiceUrl);

                    //retrieve the scope description from the server
                    DbSyncScopeDescription scopeDesc = serverProxy.GetScopeDescription();
                    serverProxy.Dispose();

                    //use scope description from server to intitialize the client
                    ceConfig.PopulateFromScopeDescription(scopeDesc);
                    ceConfig.Apply();
                }
            }
        }
    }
}
