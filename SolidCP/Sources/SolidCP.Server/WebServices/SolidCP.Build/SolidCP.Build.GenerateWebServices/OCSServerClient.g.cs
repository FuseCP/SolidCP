#if Client
using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IOCSServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IOCSServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/CreateUserResponse")]
        string CreateUser(string userUpn, string userDistinguishedName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/CreateUserResponse")]
        System.Threading.Tasks.Task<string> CreateUserAsync(string userUpn, string userDistinguishedName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/GetUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/GetUserGeneralSettingsResponse")]
        OCSUser GetUserGeneralSettings(string instanceId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/GetUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/GetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<OCSUser> GetUserGeneralSettingsAsync(string instanceId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/SetUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/SetUserGeneralSettingsResponse")]
        void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/SetUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/SetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetUserGeneralSettingsAsync(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/DeleteUserResponse")]
        void DeleteUser(string instanceId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/DeleteUserResponse")]
        System.Threading.Tasks.Task DeleteUserAsync(string instanceId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/SetUserPrimaryUri", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/SetUserPrimaryUriResponse")]
        void SetUserPrimaryUri(string instanceId, string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSServer/SetUserPrimaryUri", ReplyAction = "http://smbsaas/solidcp/server/IOCSServer/SetUserPrimaryUriResponse")]
        System.Threading.Tasks.Task SetUserPrimaryUriAsync(string instanceId, string userUpn);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OCSServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IOCSServer
    {
        public string CreateUser(string userUpn, string userDistinguishedName)
        {
            return (string)Invoke("SolidCP.Server.OCSServer", "CreateUser", userUpn, userDistinguishedName);
        }

        public async System.Threading.Tasks.Task<string> CreateUserAsync(string userUpn, string userDistinguishedName)
        {
            return await InvokeAsync<string>("SolidCP.Server.OCSServer", "CreateUser", userUpn, userDistinguishedName);
        }

        public OCSUser GetUserGeneralSettings(string instanceId)
        {
            return (OCSUser)Invoke("SolidCP.Server.OCSServer", "GetUserGeneralSettings", instanceId);
        }

        public async System.Threading.Tasks.Task<OCSUser> GetUserGeneralSettingsAsync(string instanceId)
        {
            return await InvokeAsync<OCSUser>("SolidCP.Server.OCSServer", "GetUserGeneralSettings", instanceId);
        }

        public void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            Invoke("SolidCP.Server.OCSServer", "SetUserGeneralSettings", instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }

        public async System.Threading.Tasks.Task SetUserGeneralSettingsAsync(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            await InvokeAsync("SolidCP.Server.OCSServer", "SetUserGeneralSettings", instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }

        public void DeleteUser(string instanceId)
        {
            Invoke("SolidCP.Server.OCSServer", "DeleteUser", instanceId);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string instanceId)
        {
            await InvokeAsync("SolidCP.Server.OCSServer", "DeleteUser", instanceId);
        }

        public void SetUserPrimaryUri(string instanceId, string userUpn)
        {
            Invoke("SolidCP.Server.OCSServer", "SetUserPrimaryUri", instanceId, userUpn);
        }

        public async System.Threading.Tasks.Task SetUserPrimaryUriAsync(string instanceId, string userUpn)
        {
            await InvokeAsync("SolidCP.Server.OCSServer", "SetUserPrimaryUri", instanceId, userUpn);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OCSServer : SolidCP.Web.Client.ClientBase<IOCSServer, OCSServerAssemblyClient>, IOCSServer
    {
        public string CreateUser(string userUpn, string userDistinguishedName)
        {
            return base.Client.CreateUser(userUpn, userDistinguishedName);
        }

        public async System.Threading.Tasks.Task<string> CreateUserAsync(string userUpn, string userDistinguishedName)
        {
            return await base.Client.CreateUserAsync(userUpn, userDistinguishedName);
        }

        public OCSUser GetUserGeneralSettings(string instanceId)
        {
            return base.Client.GetUserGeneralSettings(instanceId);
        }

        public async System.Threading.Tasks.Task<OCSUser> GetUserGeneralSettingsAsync(string instanceId)
        {
            return await base.Client.GetUserGeneralSettingsAsync(instanceId);
        }

        public void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            base.Client.SetUserGeneralSettings(instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }

        public async System.Threading.Tasks.Task SetUserGeneralSettingsAsync(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            await base.Client.SetUserGeneralSettingsAsync(instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }

        public void DeleteUser(string instanceId)
        {
            base.Client.DeleteUser(instanceId);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string instanceId)
        {
            await base.Client.DeleteUserAsync(instanceId);
        }

        public void SetUserPrimaryUri(string instanceId, string userUpn)
        {
            base.Client.SetUserPrimaryUri(instanceId, userUpn);
        }

        public async System.Threading.Tasks.Task SetUserPrimaryUriAsync(string instanceId, string userUpn)
        {
            await base.Client.SetUserPrimaryUriAsync(instanceId, userUpn);
        }
    }
}
#endif