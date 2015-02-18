using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Data;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization;

namespace ServiceInterface
{
    [ServiceContract(SessionMode = SessionMode.Required)] // Important attribute
    [ServiceKnownType(typeof(SyncIdFormatGroup))]
    [ServiceKnownType(typeof(DbSyncContext))]
    [ServiceKnownType(typeof(SyncSchema))]
    [ServiceKnownType(typeof(WebSyncFaultException))]
    [ServiceKnownType(typeof(SyncBatchParameters))]
    [ServiceKnownType(typeof(GetChangesParameters))]
    public interface ISyncService
    {
        // Defines the first method to call when using SessionMode
        [OperationContract(IsInitiating = true)]
        void Initialize(string scopeName);
        [OperationContract]
        DbSyncScopeDescription GetScopeDescription();
        [OperationContract]
        void BeginSession(SyncProviderPosition position);
        [OperationContract]
        SyncBatchParameters GetKnowledge();
        [OperationContract]
        GetChangesParameters GetChanges(uint batchSize,SyncKnowledge destinationKnowledge);
        [OperationContract]
        SyncSessionStatistics ApplyChanges(ConflictResolutionPolicy resolutionPolicy, ChangeBatch sourceChanges, object changeData);
        [OperationContract]
        bool HasUploadedBatchFile(string batchFileid, string remotePeerId);
        [OperationContract]
        void UploadBatchFile(string batchFileid, byte[] batchFile,
            string remotePeerId);
        [OperationContract]
        byte[] DownloadBatchFile(string batchFileId);
        [OperationContract]
        void EndSession();
        //Indicates the last method to call when use SessionMode
        [OperationContract(IsTerminating = true)]
        void Cleanup();
    }

    [DataContract]
    public class SyncBatchParameters
    {
        [DataMember]
        public SyncKnowledge DestinationKnowledge;

        [DataMember]
        public uint BatchSize;
    }

    [DataContract]
    [KnownType(typeof(DataSet))]
    public class GetChangesParameters
    {
        [DataMember]
        public object DataRetriever;

        [DataMember]
        public ChangeBatch ChangeBatch;
    }

    [DataContract]
    public class WebSyncFaultException
    {
        private string _message;
        private Exception _innerException;

        public WebSyncFaultException(string message, Exception innerException)
        {
            this._message = message;
            this._innerException = innerException;
        }
        [DataMember]
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

        [DataMember]
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