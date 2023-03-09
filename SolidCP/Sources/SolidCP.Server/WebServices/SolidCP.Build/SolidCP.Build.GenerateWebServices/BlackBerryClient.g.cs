#if Client
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using Microsoft.Web.Services3;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IBlackBerry", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IBlackBerry
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUserResponse")]
        ResultObject CreateBlackBerryUser(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUserResponse")]
        System.Threading.Tasks.Task<ResultObject> CreateBlackBerryUserAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUserResponse")]
        ResultObject DeleteBlackBerryUser(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUserResponse")]
        System.Threading.Tasks.Task<ResultObject> DeleteBlackBerryUserAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStats", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStatsResponse")]
        BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStats", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStatsResponse")]
        System.Threading.Tasks.Task<BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTime", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTimeResponse")]
        ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTime", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTimeResponse")]
        System.Threading.Tasks.Task<ResultObject> SetActivationPasswordWithExpirationTimeAsync(string primaryEmailAddress, string password, int time);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPassword", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPasswordResponse")]
        ResultObject SetEmailActivationPassword(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPassword", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPasswordResponse")]
        System.Threading.Tasks.Task<ResultObject> SetEmailActivationPasswordAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDevice", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDeviceResponse")]
        ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDevice", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDeviceResponse")]
        System.Threading.Tasks.Task<ResultObject> DeleteDataFromBlackBerryDeviceAsync(string primaryEmailAddress);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class BlackBerryAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IBlackBerry
    {
        public ResultObject CreateBlackBerryUser(string primaryEmailAddress)
        {
            return (ResultObject)Invoke("SolidCP.Server.BlackBerry", "CreateBlackBerryUser", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> CreateBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.BlackBerry", "CreateBlackBerryUser", primaryEmailAddress);
        }

        public ResultObject DeleteBlackBerryUser(string primaryEmailAddress)
        {
            return (ResultObject)Invoke("SolidCP.Server.BlackBerry", "DeleteBlackBerryUser", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.BlackBerry", "DeleteBlackBerryUser", primaryEmailAddress);
        }

        public BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress)
        {
            return (BlackBerryUserStatsResult)Invoke("SolidCP.Server.BlackBerry", "GetBlackBerryUserStats", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<BlackBerryUserStatsResult>("SolidCP.Server.BlackBerry", "GetBlackBerryUserStats", primaryEmailAddress);
        }

        public ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time)
        {
            return (ResultObject)Invoke("SolidCP.Server.BlackBerry", "SetActivationPasswordWithExpirationTime", primaryEmailAddress, password, time);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetActivationPasswordWithExpirationTimeAsync(string primaryEmailAddress, string password, int time)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.BlackBerry", "SetActivationPasswordWithExpirationTime", primaryEmailAddress, password, time);
        }

        public ResultObject SetEmailActivationPassword(string primaryEmailAddress)
        {
            return (ResultObject)Invoke("SolidCP.Server.BlackBerry", "SetEmailActivationPassword", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetEmailActivationPasswordAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.BlackBerry", "SetEmailActivationPassword", primaryEmailAddress);
        }

        public ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress)
        {
            return (ResultObject)Invoke("SolidCP.Server.BlackBerry", "DeleteDataFromBlackBerryDevice", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteDataFromBlackBerryDeviceAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.BlackBerry", "DeleteDataFromBlackBerryDevice", primaryEmailAddress);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class BlackBerry : SolidCP.Web.Client.ClientBase<IBlackBerry, BlackBerryAssemblyClient>, IBlackBerry
    {
        public ResultObject CreateBlackBerryUser(string primaryEmailAddress)
        {
            return base.Client.CreateBlackBerryUser(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> CreateBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await base.Client.CreateBlackBerryUserAsync(primaryEmailAddress);
        }

        public ResultObject DeleteBlackBerryUser(string primaryEmailAddress)
        {
            return base.Client.DeleteBlackBerryUser(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await base.Client.DeleteBlackBerryUserAsync(primaryEmailAddress);
        }

        public BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress)
        {
            return base.Client.GetBlackBerryUserStats(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(string primaryEmailAddress)
        {
            return await base.Client.GetBlackBerryUserStatsAsync(primaryEmailAddress);
        }

        public ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time)
        {
            return base.Client.SetActivationPasswordWithExpirationTime(primaryEmailAddress, password, time);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetActivationPasswordWithExpirationTimeAsync(string primaryEmailAddress, string password, int time)
        {
            return await base.Client.SetActivationPasswordWithExpirationTimeAsync(primaryEmailAddress, password, time);
        }

        public ResultObject SetEmailActivationPassword(string primaryEmailAddress)
        {
            return base.Client.SetEmailActivationPassword(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetEmailActivationPasswordAsync(string primaryEmailAddress)
        {
            return await base.Client.SetEmailActivationPasswordAsync(primaryEmailAddress);
        }

        public ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress)
        {
            return base.Client.DeleteDataFromBlackBerryDevice(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteDataFromBlackBerryDeviceAsync(string primaryEmailAddress)
        {
            return await base.Client.DeleteDataFromBlackBerryDeviceAsync(primaryEmailAddress);
        }
    }
}
#endif