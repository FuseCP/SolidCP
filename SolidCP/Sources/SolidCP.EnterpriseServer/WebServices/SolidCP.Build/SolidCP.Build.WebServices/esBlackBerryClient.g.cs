#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesBlackBerry", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesBlackBerry
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/CreateBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/CreateBlackBerryUserResponse")]
        SolidCP.Providers.Common.ResultObject CreateBlackBerryUser(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/CreateBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/CreateBlackBerryUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateBlackBerryUserAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteBlackBerryUserResponse")]
        SolidCP.Providers.Common.ResultObject DeleteBlackBerryUser(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteBlackBerryUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteBlackBerryUserAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUsersPagedResponse")]
        SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult GetBlackBerryUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUsersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult> GetBlackBerryUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserCountResponse")]
        SolidCP.Providers.ResultObjects.IntResult GetBlackBerryUserCount(int itemId, string name, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserCountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetBlackBerryUserCountAsync(int itemId, string name, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserStats", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserStatsResponse")]
        SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult GetBlackBerryUserStats(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserStats", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/GetBlackBerryUserStatsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteDataFromBlackBerryDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteDataFromBlackBerryDeviceResponse")]
        SolidCP.Providers.Common.ResultObject DeleteDataFromBlackBerryDevice(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteDataFromBlackBerryDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/DeleteDataFromBlackBerryDeviceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteDataFromBlackBerryDeviceAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetEmailActivationPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetEmailActivationPasswordResponse")]
        SolidCP.Providers.Common.ResultObject SetEmailActivationPassword(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetEmailActivationPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetEmailActivationPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEmailActivationPasswordAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetActivationPasswordWithExpirationTime", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetActivationPasswordWithExpirationTimeResponse")]
        SolidCP.Providers.Common.ResultObject SetActivationPasswordWithExpirationTime(int itemId, int accountId, string password, int time);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetActivationPasswordWithExpirationTime", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBlackBerry/SetActivationPasswordWithExpirationTimeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetActivationPasswordWithExpirationTimeAsync(int itemId, int accountId, string password, int time);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esBlackBerryAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesBlackBerry
    {
        public SolidCP.Providers.Common.ResultObject CreateBlackBerryUser(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "CreateBlackBerryUser", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateBlackBerryUserAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "CreateBlackBerryUser", itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteBlackBerryUser(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "DeleteBlackBerryUser", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteBlackBerryUserAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "DeleteBlackBerryUser", itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult GetBlackBerryUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult>("SolidCP.EnterpriseServer.esBlackBerry", "GetBlackBerryUsersPaged", itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult> GetBlackBerryUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult>("SolidCP.EnterpriseServer.esBlackBerry", "GetBlackBerryUsersPaged", itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetBlackBerryUserCount(int itemId, string name, string email)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esBlackBerry", "GetBlackBerryUserCount", itemId, name, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetBlackBerryUserCountAsync(int itemId, string name, string email)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esBlackBerry", "GetBlackBerryUserCount", itemId, name, email);
        }

        public SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult GetBlackBerryUserStats(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult>("SolidCP.EnterpriseServer.esBlackBerry", "GetBlackBerryUserStats", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult>("SolidCP.EnterpriseServer.esBlackBerry", "GetBlackBerryUserStats", itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteDataFromBlackBerryDevice(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "DeleteDataFromBlackBerryDevice", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteDataFromBlackBerryDeviceAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "DeleteDataFromBlackBerryDevice", itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SetEmailActivationPassword(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "SetEmailActivationPassword", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEmailActivationPasswordAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "SetEmailActivationPassword", itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SetActivationPasswordWithExpirationTime(int itemId, int accountId, string password, int time)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "SetActivationPasswordWithExpirationTime", itemId, accountId, password, time);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetActivationPasswordWithExpirationTimeAsync(int itemId, int accountId, string password, int time)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esBlackBerry", "SetActivationPasswordWithExpirationTime", itemId, accountId, password, time);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esBlackBerry : SolidCP.Web.Client.ClientBase<IesBlackBerry, esBlackBerryAssemblyClient>, IesBlackBerry
    {
        public SolidCP.Providers.Common.ResultObject CreateBlackBerryUser(int itemId, int accountId)
        {
            return base.Client.CreateBlackBerryUser(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateBlackBerryUserAsync(int itemId, int accountId)
        {
            return await base.Client.CreateBlackBerryUserAsync(itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteBlackBerryUser(int itemId, int accountId)
        {
            return base.Client.DeleteBlackBerryUser(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteBlackBerryUserAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteBlackBerryUserAsync(itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult GetBlackBerryUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return base.Client.GetBlackBerryUsersPaged(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult> GetBlackBerryUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return await base.Client.GetBlackBerryUsersPagedAsync(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetBlackBerryUserCount(int itemId, string name, string email)
        {
            return base.Client.GetBlackBerryUserCount(itemId, name, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetBlackBerryUserCountAsync(int itemId, string name, string email)
        {
            return await base.Client.GetBlackBerryUserCountAsync(itemId, name, email);
        }

        public SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult GetBlackBerryUserStats(int itemId, int accountId)
        {
            return base.Client.GetBlackBerryUserStats(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(int itemId, int accountId)
        {
            return await base.Client.GetBlackBerryUserStatsAsync(itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteDataFromBlackBerryDevice(int itemId, int accountId)
        {
            return base.Client.DeleteDataFromBlackBerryDevice(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteDataFromBlackBerryDeviceAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteDataFromBlackBerryDeviceAsync(itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SetEmailActivationPassword(int itemId, int accountId)
        {
            return base.Client.SetEmailActivationPassword(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEmailActivationPasswordAsync(int itemId, int accountId)
        {
            return await base.Client.SetEmailActivationPasswordAsync(itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SetActivationPasswordWithExpirationTime(int itemId, int accountId, string password, int time)
        {
            return base.Client.SetActivationPasswordWithExpirationTime(itemId, accountId, password, time);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetActivationPasswordWithExpirationTimeAsync(int itemId, int accountId, string password, int time)
        {
            return await base.Client.SetActivationPasswordWithExpirationTimeAsync(itemId, accountId, password, time);
        }
    }
}
#endif