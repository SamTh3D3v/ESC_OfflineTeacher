﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ESC_OfflineTeacher.SGS.Helper {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SGS.Helper.IOfflineService")]
    public interface IOfflineService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOfflineService/GetUserId", ReplyAction="http://tempuri.org/IOfflineService/GetUserIdResponse")]
        object GetUserId(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOfflineService/GetUserId", ReplyAction="http://tempuri.org/IOfflineService/GetUserIdResponse")]
        System.Threading.Tasks.Task<object> GetUserIdAsync(string username);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IOfflineServiceChannel : ESC_OfflineTeacher.SGS.Helper.IOfflineService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OfflineServiceClient : System.ServiceModel.ClientBase<ESC_OfflineTeacher.SGS.Helper.IOfflineService>, ESC_OfflineTeacher.SGS.Helper.IOfflineService {
        
        public OfflineServiceClient() {
        }
        
        public OfflineServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OfflineServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OfflineServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OfflineServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public object GetUserId(string username) {
            return base.Channel.GetUserId(username);
        }
        
        public System.Threading.Tasks.Task<object> GetUserIdAsync(string username) {
            return base.Channel.GetUserIdAsync(username);
        }
    }
}
