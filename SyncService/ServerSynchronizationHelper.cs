using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;
using System.Configuration;

namespace SyncService
{
    public class ServerSynchronizationHelper
    {
        string conString =ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        /// <summary>
        /// Configure the SqlSyncprovider.  Note that this method assumes you have a direct 
        /// conection to the server as this is more of a design time use case vs. runtime 
        /// use case.  We think of provisioning the server as something that occurs before 
        /// an application is deployed whereas provisioning the client is somethng that 
        /// happens during runtime (on intitial sync) after the application is deployed.
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public SqlSyncProvider ConfigureSqlSyncProvider(string scopeName)
        {

            SqlSyncProvider provider = new SqlSyncProvider();
            provider.ScopeName = scopeName;
            provider.Connection = new SqlConnection(conString);

            //create a new scope description and add the appropriate tables to this scope
            DbSyncScopeDescription scopeDesc = new DbSyncScopeDescription(scopeName);

            //class to be used to provision the scope defined above
            SqlSyncScopeProvisioning serverConfig =
                new SqlSyncScopeProvisioning((SqlConnection)provider.Connection);

            //determine if this scope already exists on the server and if not go ahead 
            //and provision
            if (!serverConfig.ScopeExists(scopeName))
            {

                //note that it is important to call this after the tables have been added 
                //to the scope
                serverConfig.PopulateFromScopeDescription(scopeDesc);

                //indicate that the base table already exists and does not need to be created
                serverConfig.SetCreateTableDefault(DbSyncCreationOption.Skip);

                //provision the server
                serverConfig.Apply();
            }


            return provider;
        }



    }
}