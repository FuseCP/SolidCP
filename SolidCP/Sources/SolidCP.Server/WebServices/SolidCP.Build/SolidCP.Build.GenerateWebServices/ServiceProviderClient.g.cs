#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IServiceProvider", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IServiceProvider
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/Install", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/InstallResponse")]
        string[] Install();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/Install", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/InstallResponse")]
        System.Threading.Tasks.Task<string[]> InstallAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/GetProviderDefaultSettings", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/GetProviderDefaultSettingsResponse")]
        SettingPair[] GetProviderDefaultSettings();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/GetProviderDefaultSettings", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/GetProviderDefaultSettingsResponse")]
        System.Threading.Tasks.Task<SettingPair[]> GetProviderDefaultSettingsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/Uninstall", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/UninstallResponse")]
        void Uninstall();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/Uninstall", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/UninstallResponse")]
        System.Threading.Tasks.Task UninstallAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/IsInstalled", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/IsInstalledResponse")]
        bool IsInstalled();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/IsInstalled", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/IsInstalledResponse")]
        System.Threading.Tasks.Task<bool> IsInstalledAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/ChangeServiceItemsState", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/ChangeServiceItemsStateResponse")]
        void ChangeServiceItemsState(SoapServiceProviderItem[] items, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/ChangeServiceItemsState", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/ChangeServiceItemsStateResponse")]
        System.Threading.Tasks.Task ChangeServiceItemsStateAsync(SoapServiceProviderItem[] items, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/DeleteServiceItems", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/DeleteServiceItemsResponse")]
        void DeleteServiceItems(SoapServiceProviderItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/DeleteServiceItems", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/DeleteServiceItemsResponse")]
        System.Threading.Tasks.Task DeleteServiceItemsAsync(SoapServiceProviderItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsDiskSpace", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsDiskSpaceResponse")]
        ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(SoapServiceProviderItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsDiskSpace", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsDiskSpaceResponse")]
        System.Threading.Tasks.Task<ServiceProviderItemDiskSpace[]> GetServiceItemsDiskSpaceAsync(SoapServiceProviderItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsBandwidth", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsBandwidthResponse")]
        ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(SoapServiceProviderItem[] items, DateTime since);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsBandwidth", ReplyAction = "http://smbsaas/solidcp/server/IServiceProvider/GetServiceItemsBandwidthResponse")]
        System.Threading.Tasks.Task<ServiceProviderItemBandwidth[]> GetServiceItemsBandwidthAsync(SoapServiceProviderItem[] items, DateTime since);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class ServiceProviderAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IServiceProvider
    {
        public string[] Install()
        {
            return (string[])Invoke("SolidCP.Server.ServiceProvider", "Install");
        }

        public async System.Threading.Tasks.Task<string[]> InstallAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.ServiceProvider", "Install");
        }

        public SettingPair[] GetProviderDefaultSettings()
        {
            return (SettingPair[])Invoke("SolidCP.Server.ServiceProvider", "GetProviderDefaultSettings");
        }

        public async System.Threading.Tasks.Task<SettingPair[]> GetProviderDefaultSettingsAsync()
        {
            return await InvokeAsync<SettingPair[]>("SolidCP.Server.ServiceProvider", "GetProviderDefaultSettings");
        }

        public void Uninstall()
        {
            Invoke("SolidCP.Server.ServiceProvider", "Uninstall");
        }

        public async System.Threading.Tasks.Task UninstallAsync()
        {
            await InvokeAsync("SolidCP.Server.ServiceProvider", "Uninstall");
        }

        public bool IsInstalled()
        {
            return (bool)Invoke("SolidCP.Server.ServiceProvider", "IsInstalled");
        }

        public async System.Threading.Tasks.Task<bool> IsInstalledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.ServiceProvider", "IsInstalled");
        }

        public void ChangeServiceItemsState(SoapServiceProviderItem[] items, bool enabled)
        {
            Invoke("SolidCP.Server.ServiceProvider", "ChangeServiceItemsState", items, enabled);
        }

        public async System.Threading.Tasks.Task ChangeServiceItemsStateAsync(SoapServiceProviderItem[] items, bool enabled)
        {
            await InvokeAsync("SolidCP.Server.ServiceProvider", "ChangeServiceItemsState", items, enabled);
        }

        public void DeleteServiceItems(SoapServiceProviderItem[] items)
        {
            Invoke("SolidCP.Server.ServiceProvider", "DeleteServiceItems", items);
        }

        public async System.Threading.Tasks.Task DeleteServiceItemsAsync(SoapServiceProviderItem[] items)
        {
            await InvokeAsync("SolidCP.Server.ServiceProvider", "DeleteServiceItems", items);
        }

        public ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(SoapServiceProviderItem[] items)
        {
            return (ServiceProviderItemDiskSpace[])Invoke("SolidCP.Server.ServiceProvider", "GetServiceItemsDiskSpace", items);
        }

        public async System.Threading.Tasks.Task<ServiceProviderItemDiskSpace[]> GetServiceItemsDiskSpaceAsync(SoapServiceProviderItem[] items)
        {
            return await InvokeAsync<ServiceProviderItemDiskSpace[]>("SolidCP.Server.ServiceProvider", "GetServiceItemsDiskSpace", items);
        }

        public ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(SoapServiceProviderItem[] items, DateTime since)
        {
            return (ServiceProviderItemBandwidth[])Invoke("SolidCP.Server.ServiceProvider", "GetServiceItemsBandwidth", items, since);
        }

        public async System.Threading.Tasks.Task<ServiceProviderItemBandwidth[]> GetServiceItemsBandwidthAsync(SoapServiceProviderItem[] items, DateTime since)
        {
            return await InvokeAsync<ServiceProviderItemBandwidth[]>("SolidCP.Server.ServiceProvider", "GetServiceItemsBandwidth", items, since);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class ServiceProvider : SolidCP.Web.Client.ClientBase<IServiceProvider, ServiceProviderAssemblyClient>, IServiceProvider
    {
        public string[] Install()
        {
            return base.Client.Install();
        }

        public async System.Threading.Tasks.Task<string[]> InstallAsync()
        {
            return await base.Client.InstallAsync();
        }

        public SettingPair[] GetProviderDefaultSettings()
        {
            return base.Client.GetProviderDefaultSettings();
        }

        public async System.Threading.Tasks.Task<SettingPair[]> GetProviderDefaultSettingsAsync()
        {
            return await base.Client.GetProviderDefaultSettingsAsync();
        }

        public void Uninstall()
        {
            base.Client.Uninstall();
        }

        public async System.Threading.Tasks.Task UninstallAsync()
        {
            await base.Client.UninstallAsync();
        }

        public bool IsInstalled()
        {
            return base.Client.IsInstalled();
        }

        public async System.Threading.Tasks.Task<bool> IsInstalledAsync()
        {
            return await base.Client.IsInstalledAsync();
        }

        public void ChangeServiceItemsState(SoapServiceProviderItem[] items, bool enabled)
        {
            base.Client.ChangeServiceItemsState(items, enabled);
        }

        public async System.Threading.Tasks.Task ChangeServiceItemsStateAsync(SoapServiceProviderItem[] items, bool enabled)
        {
            await base.Client.ChangeServiceItemsStateAsync(items, enabled);
        }

        public void DeleteServiceItems(SoapServiceProviderItem[] items)
        {
            base.Client.DeleteServiceItems(items);
        }

        public async System.Threading.Tasks.Task DeleteServiceItemsAsync(SoapServiceProviderItem[] items)
        {
            await base.Client.DeleteServiceItemsAsync(items);
        }

        public ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(SoapServiceProviderItem[] items)
        {
            return base.Client.GetServiceItemsDiskSpace(items);
        }

        public async System.Threading.Tasks.Task<ServiceProviderItemDiskSpace[]> GetServiceItemsDiskSpaceAsync(SoapServiceProviderItem[] items)
        {
            return await base.Client.GetServiceItemsDiskSpaceAsync(items);
        }

        public ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(SoapServiceProviderItem[] items, DateTime since)
        {
            return base.Client.GetServiceItemsBandwidth(items, since);
        }

        public async System.Threading.Tasks.Task<ServiceProviderItemBandwidth[]> GetServiceItemsBandwidthAsync(SoapServiceProviderItem[] items, DateTime since)
        {
            return await base.Client.GetServiceItemsBandwidthAsync(items, since);
        }
    }
}
#endif