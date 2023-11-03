#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesOCS", Namespace = "http://tempuri.org/")]
    public interface IesOCS
    {
        [OperationContract(Action = "http://tempuri.org/IesOCS/CreateOCSUser", ReplyAction = "http://tempuri.org/IesOCS/CreateOCSUserResponse")]
        SolidCP.Providers.ResultObjects.OCSUserResult CreateOCSUser(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOCS/CreateOCSUser", ReplyAction = "http://tempuri.org/IesOCS/CreateOCSUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OCSUserResult> CreateOCSUserAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOCS/DeleteOCSUser", ReplyAction = "http://tempuri.org/IesOCS/DeleteOCSUserResponse")]
        SolidCP.Providers.Common.ResultObject DeleteOCSUser(int itemId, string instanceId);
        [OperationContract(Action = "http://tempuri.org/IesOCS/DeleteOCSUser", ReplyAction = "http://tempuri.org/IesOCS/DeleteOCSUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteOCSUserAsync(int itemId, string instanceId);
        [OperationContract(Action = "http://tempuri.org/IesOCS/GetOCSUsersPaged", ReplyAction = "http://tempuri.org/IesOCS/GetOCSUsersPagedResponse")]
        SolidCP.Providers.ResultObjects.OCSUsersPagedResult GetOCSUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOCS/GetOCSUsersPaged", ReplyAction = "http://tempuri.org/IesOCS/GetOCSUsersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OCSUsersPagedResult> GetOCSUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOCS/GetOCSUserCount", ReplyAction = "http://tempuri.org/IesOCS/GetOCSUserCountResponse")]
        SolidCP.Providers.ResultObjects.IntResult GetOCSUserCount(int itemId, string name, string email);
        [OperationContract(Action = "http://tempuri.org/IesOCS/GetOCSUserCount", ReplyAction = "http://tempuri.org/IesOCS/GetOCSUserCountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetOCSUserCountAsync(int itemId, string name, string email);
        [OperationContract(Action = "http://tempuri.org/IesOCS/GetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOCS/GetUserGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.OCSUser GetUserGeneralSettings(int itemId, string instanceId);
        [OperationContract(Action = "http://tempuri.org/IesOCS/GetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOCS/GetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OCSUser> GetUserGeneralSettingsAsync(int itemId, string instanceId);
        [OperationContract(Action = "http://tempuri.org/IesOCS/SetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOCS/SetUserGeneralSettingsResponse")]
        void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence);
        [OperationContract(Action = "http://tempuri.org/IesOCS/SetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOCS/SetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetUserGeneralSettingsAsync(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esOCSAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesOCS
    {
        public SolidCP.Providers.ResultObjects.OCSUserResult CreateOCSUser(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.OCSUserResult>("SolidCP.EnterpriseServer.esOCS", "CreateOCSUser", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OCSUserResult> CreateOCSUserAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.OCSUserResult>("SolidCP.EnterpriseServer.esOCS", "CreateOCSUser", itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteOCSUser(int itemId, string instanceId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOCS", "DeleteOCSUser", itemId, instanceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteOCSUserAsync(int itemId, string instanceId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOCS", "DeleteOCSUser", itemId, instanceId);
        }

        public SolidCP.Providers.ResultObjects.OCSUsersPagedResult GetOCSUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.ResultObjects.OCSUsersPagedResult>("SolidCP.EnterpriseServer.esOCS", "GetOCSUsersPaged", itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OCSUsersPagedResult> GetOCSUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.OCSUsersPagedResult>("SolidCP.EnterpriseServer.esOCS", "GetOCSUsersPaged", itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetOCSUserCount(int itemId, string name, string email)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esOCS", "GetOCSUserCount", itemId, name, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetOCSUserCountAsync(int itemId, string name, string email)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esOCS", "GetOCSUserCount", itemId, name, email);
        }

        public SolidCP.Providers.HostedSolution.OCSUser GetUserGeneralSettings(int itemId, string instanceId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OCSUser>("SolidCP.EnterpriseServer.esOCS", "GetUserGeneralSettings", itemId, instanceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OCSUser> GetUserGeneralSettingsAsync(int itemId, string instanceId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OCSUser>("SolidCP.EnterpriseServer.esOCS", "GetUserGeneralSettings", itemId, instanceId);
        }

        public void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            Invoke("SolidCP.EnterpriseServer.esOCS", "SetUserGeneralSettings", itemId, instanceId, enabledForFederation, enabledForPublicIMConnectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }

        public async System.Threading.Tasks.Task SetUserGeneralSettingsAsync(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOCS", "SetUserGeneralSettings", itemId, instanceId, enabledForFederation, enabledForPublicIMConnectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esOCS : SolidCP.Web.Client.ClientBase<IesOCS, esOCSAssemblyClient>, IesOCS
    {
        public SolidCP.Providers.ResultObjects.OCSUserResult CreateOCSUser(int itemId, int accountId)
        {
            return base.Client.CreateOCSUser(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OCSUserResult> CreateOCSUserAsync(int itemId, int accountId)
        {
            return await base.Client.CreateOCSUserAsync(itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteOCSUser(int itemId, string instanceId)
        {
            return base.Client.DeleteOCSUser(itemId, instanceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteOCSUserAsync(int itemId, string instanceId)
        {
            return await base.Client.DeleteOCSUserAsync(itemId, instanceId);
        }

        public SolidCP.Providers.ResultObjects.OCSUsersPagedResult GetOCSUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return base.Client.GetOCSUsersPaged(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OCSUsersPagedResult> GetOCSUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return await base.Client.GetOCSUsersPagedAsync(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetOCSUserCount(int itemId, string name, string email)
        {
            return base.Client.GetOCSUserCount(itemId, name, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetOCSUserCountAsync(int itemId, string name, string email)
        {
            return await base.Client.GetOCSUserCountAsync(itemId, name, email);
        }

        public SolidCP.Providers.HostedSolution.OCSUser GetUserGeneralSettings(int itemId, string instanceId)
        {
            return base.Client.GetUserGeneralSettings(itemId, instanceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OCSUser> GetUserGeneralSettingsAsync(int itemId, string instanceId)
        {
            return await base.Client.GetUserGeneralSettingsAsync(itemId, instanceId);
        }

        public void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            base.Client.SetUserGeneralSettings(itemId, instanceId, enabledForFederation, enabledForPublicIMConnectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }

        public async System.Threading.Tasks.Task SetUserGeneralSettingsAsync(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            await base.Client.SetUserGeneralSettingsAsync(itemId, instanceId, enabledForFederation, enabledForPublicIMConnectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }
    }
}
#endif