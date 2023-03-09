#if Client
using System;
using System.IO;
using System.Data;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.ServiceProcess;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Management;
using System.Collections.Specialized;
using Microsoft.Web.PlatformInstaller;
using Microsoft.Web.Services3;
using Microsoft.Win32;
using SolidCP.Providers.Utils;
using SolidCP.Server.Code;
using SolidCP.Server.Utils;
using SolidCP.Providers;
using SolidCP.Server.WPIService;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IWindowsServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IWindowsServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessionsResponse")]
        TerminalSession[] GetTerminalServicesSessions();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessionsResponse")]
        System.Threading.Tasks.Task<TerminalSession[]> GetTerminalServicesSessionsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSessionResponse")]
        void CloseTerminalServicesSession(int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSessionResponse")]
        System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcesses", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcessesResponse")]
        WindowsProcess[] GetWindowsProcesses();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcesses", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcessesResponse")]
        System.Threading.Tasks.Task<WindowsProcess[]> GetWindowsProcessesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcess", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcessResponse")]
        void TerminateWindowsProcess(int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcess", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcessResponse")]
        System.Threading.Tasks.Task TerminateWindowsProcessAsync(int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServices", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServicesResponse")]
        WindowsService[] GetWindowsServices();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServices", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServicesResponse")]
        System.Threading.Tasks.Task<WindowsService[]> GetWindowsServicesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatus", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatusResponse")]
        void ChangeWindowsServiceStatus(string id, WindowsServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatus", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatusResponse")]
        System.Threading.Tasks.Task ChangeWindowsServiceStatusAsync(string id, WindowsServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsResponse")]
        WPIProduct[] GetWPIProducts(string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsResponse")]
        System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFilteredResponse")]
        WPIProduct[] GetWPIProductsFiltered(string filter);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFilteredResponse")]
        System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsFilteredAsync(string filter);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductByIdResponse")]
        WPIProduct GetWPIProductById(string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductByIdResponse")]
        System.Threading.Tasks.Task<WPIProduct> GetWPIProductByIdAsync(string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabsResponse")]
        WPITab[] GetWPITabs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabsResponse")]
        System.Threading.Tasks.Task<WPITab[]> GetWPITabsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeedsResponse")]
        void InitWPIFeeds(string feedUrls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeedsResponse")]
        System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywordsResponse")]
        WPIKeyword[] GetWPIKeywords();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywordsResponse")]
        System.Threading.Tasks.Task<WPIKeyword[]> GetWPIKeywordsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependenciesResponse")]
        WPIProduct[] GetWPIProductsWithDependencies(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependenciesResponse")]
        System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/InstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/InstallWPIProductsResponse")]
        void InstallWPIProducts(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/InstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/InstallWPIProductsResponse")]
        System.Threading.Tasks.Task InstallWPIProductsAsync(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/CancelInstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/CancelInstallWPIProductsResponse")]
        void CancelInstallWPIProducts();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/CancelInstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/CancelInstallWPIProductsResponse")]
        System.Threading.Tasks.Task CancelInstallWPIProductsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIStatus", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIStatusResponse")]
        string GetWPIStatus();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIStatus", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIStatusResponse")]
        System.Threading.Tasks.Task<string> GetWPIStatusAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogFileDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogFileDirectoryResponse")]
        string WpiGetLogFileDirectory();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogFileDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogFileDirectoryResponse")]
        System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogsInDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogsInDirectoryResponse")]
        SettingPair[] WpiGetLogsInDirectory(string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogsInDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogsInDirectoryResponse")]
        System.Threading.Tasks.Task<SettingPair[]> WpiGetLogsInDirectoryAsync(string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNames", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNamesResponse")]
        List<string> GetLogNames();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNames", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNamesResponse")]
        System.Threading.Tasks.Task<List<string>> GetLogNamesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesResponse")]
        List<SystemLogEntry> GetLogEntries(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesResponse")]
        System.Threading.Tasks.Task<List<SystemLogEntry>> GetLogEntriesAsync(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPagedResponse")]
        SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPagedResponse")]
        System.Threading.Tasks.Task<SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ClearLog", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ClearLogResponse")]
        void ClearLog(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ClearLog", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ClearLogResponse")]
        System.Threading.Tasks.Task ClearLogAsync(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystem", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystemResponse")]
        void RebootSystem();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystem", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystemResponse")]
        System.Threading.Tasks.Task RebootSystemAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetMemory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetMemoryResponse")]
        Memory GetMemory();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetMemory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetMemoryResponse")]
        System.Threading.Tasks.Task<Memory> GetMemoryAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommand", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommandResponse")]
        string ExecuteSystemCommand(string path, string args);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommand", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommandResponse")]
        System.Threading.Tasks.Task<string> ExecuteSystemCommandAsync(string path, string args);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class WindowsServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IWindowsServer
    {
        public TerminalSession[] GetTerminalServicesSessions()
        {
            return (TerminalSession[])Invoke("SolidCP.Server.WindowsServer", "GetTerminalServicesSessions");
        }

        public async System.Threading.Tasks.Task<TerminalSession[]> GetTerminalServicesSessionsAsync()
        {
            return await InvokeAsync<TerminalSession[]>("SolidCP.Server.WindowsServer", "GetTerminalServicesSessions");
        }

        public void CloseTerminalServicesSession(int sessionId)
        {
            Invoke("SolidCP.Server.WindowsServer", "CloseTerminalServicesSession", sessionId);
        }

        public async System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "CloseTerminalServicesSession", sessionId);
        }

        public WindowsProcess[] GetWindowsProcesses()
        {
            return (WindowsProcess[])Invoke("SolidCP.Server.WindowsServer", "GetWindowsProcesses");
        }

        public async System.Threading.Tasks.Task<WindowsProcess[]> GetWindowsProcessesAsync()
        {
            return await InvokeAsync<WindowsProcess[]>("SolidCP.Server.WindowsServer", "GetWindowsProcesses");
        }

        public void TerminateWindowsProcess(int pid)
        {
            Invoke("SolidCP.Server.WindowsServer", "TerminateWindowsProcess", pid);
        }

        public async System.Threading.Tasks.Task TerminateWindowsProcessAsync(int pid)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "TerminateWindowsProcess", pid);
        }

        public WindowsService[] GetWindowsServices()
        {
            return (WindowsService[])Invoke("SolidCP.Server.WindowsServer", "GetWindowsServices");
        }

        public async System.Threading.Tasks.Task<WindowsService[]> GetWindowsServicesAsync()
        {
            return await InvokeAsync<WindowsService[]>("SolidCP.Server.WindowsServer", "GetWindowsServices");
        }

        public void ChangeWindowsServiceStatus(string id, WindowsServiceStatus status)
        {
            Invoke("SolidCP.Server.WindowsServer", "ChangeWindowsServiceStatus", id, status);
        }

        public async System.Threading.Tasks.Task ChangeWindowsServiceStatusAsync(string id, WindowsServiceStatus status)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "ChangeWindowsServiceStatus", id, status);
        }

        public WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {
            return (WPIProduct[])Invoke("SolidCP.Server.WindowsServer", "GetWPIProducts", tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId)
        {
            return await InvokeAsync<WPIProduct[]>("SolidCP.Server.WindowsServer", "GetWPIProducts", tabId, keywordId);
        }

        public WPIProduct[] GetWPIProductsFiltered(string filter)
        {
            return (WPIProduct[])Invoke("SolidCP.Server.WindowsServer", "GetWPIProductsFiltered", filter);
        }

        public async System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsFilteredAsync(string filter)
        {
            return await InvokeAsync<WPIProduct[]>("SolidCP.Server.WindowsServer", "GetWPIProductsFiltered", filter);
        }

        public WPIProduct GetWPIProductById(string productdId)
        {
            return (WPIProduct)Invoke("SolidCP.Server.WindowsServer", "GetWPIProductById", productdId);
        }

        public async System.Threading.Tasks.Task<WPIProduct> GetWPIProductByIdAsync(string productdId)
        {
            return await InvokeAsync<WPIProduct>("SolidCP.Server.WindowsServer", "GetWPIProductById", productdId);
        }

        public WPITab[] GetWPITabs()
        {
            return (WPITab[])Invoke("SolidCP.Server.WindowsServer", "GetWPITabs");
        }

        public async System.Threading.Tasks.Task<WPITab[]> GetWPITabsAsync()
        {
            return await InvokeAsync<WPITab[]>("SolidCP.Server.WindowsServer", "GetWPITabs");
        }

        public void InitWPIFeeds(string feedUrls)
        {
            Invoke("SolidCP.Server.WindowsServer", "InitWPIFeeds", feedUrls);
        }

        public async System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "InitWPIFeeds", feedUrls);
        }

        public WPIKeyword[] GetWPIKeywords()
        {
            return (WPIKeyword[])Invoke("SolidCP.Server.WindowsServer", "GetWPIKeywords");
        }

        public async System.Threading.Tasks.Task<WPIKeyword[]> GetWPIKeywordsAsync()
        {
            return await InvokeAsync<WPIKeyword[]>("SolidCP.Server.WindowsServer", "GetWPIKeywords");
        }

        public WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            return (WPIProduct[])Invoke("SolidCP.Server.WindowsServer", "GetWPIProductsWithDependencies", products);
        }

        public async System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products)
        {
            return await InvokeAsync<WPIProduct[]>("SolidCP.Server.WindowsServer", "GetWPIProductsWithDependencies", products);
        }

        public void InstallWPIProducts(string[] products)
        {
            Invoke("SolidCP.Server.WindowsServer", "InstallWPIProducts", products);
        }

        public async System.Threading.Tasks.Task InstallWPIProductsAsync(string[] products)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "InstallWPIProducts", products);
        }

        public void CancelInstallWPIProducts()
        {
            Invoke("SolidCP.Server.WindowsServer", "CancelInstallWPIProducts");
        }

        public async System.Threading.Tasks.Task CancelInstallWPIProductsAsync()
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "CancelInstallWPIProducts");
        }

        public string GetWPIStatus()
        {
            return (string)Invoke("SolidCP.Server.WindowsServer", "GetWPIStatus");
        }

        public async System.Threading.Tasks.Task<string> GetWPIStatusAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.WindowsServer", "GetWPIStatus");
        }

        public string WpiGetLogFileDirectory()
        {
            return (string)Invoke("SolidCP.Server.WindowsServer", "WpiGetLogFileDirectory");
        }

        public async System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.WindowsServer", "WpiGetLogFileDirectory");
        }

        public SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            return (SettingPair[])Invoke("SolidCP.Server.WindowsServer", "WpiGetLogsInDirectory", Path);
        }

        public async System.Threading.Tasks.Task<SettingPair[]> WpiGetLogsInDirectoryAsync(string Path)
        {
            return await InvokeAsync<SettingPair[]>("SolidCP.Server.WindowsServer", "WpiGetLogsInDirectory", Path);
        }

        public List<string> GetLogNames()
        {
            return (List<string>)Invoke("SolidCP.Server.WindowsServer", "GetLogNames");
        }

        public async System.Threading.Tasks.Task<List<string>> GetLogNamesAsync()
        {
            return await InvokeAsync<List<string>>("SolidCP.Server.WindowsServer", "GetLogNames");
        }

        public List<SystemLogEntry> GetLogEntries(string logName)
        {
            return (List<SystemLogEntry>)Invoke("SolidCP.Server.WindowsServer", "GetLogEntries", logName);
        }

        public async System.Threading.Tasks.Task<List<SystemLogEntry>> GetLogEntriesAsync(string logName)
        {
            return await InvokeAsync<List<SystemLogEntry>>("SolidCP.Server.WindowsServer", "GetLogEntries", logName);
        }

        public SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            return (SystemLogEntriesPaged)Invoke("SolidCP.Server.WindowsServer", "GetLogEntriesPaged", logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows)
        {
            return await InvokeAsync<SystemLogEntriesPaged>("SolidCP.Server.WindowsServer", "GetLogEntriesPaged", logName, startRow, maximumRows);
        }

        public void ClearLog(string logName)
        {
            Invoke("SolidCP.Server.WindowsServer", "ClearLog", logName);
        }

        public async System.Threading.Tasks.Task ClearLogAsync(string logName)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "ClearLog", logName);
        }

        public void RebootSystem()
        {
            Invoke("SolidCP.Server.WindowsServer", "RebootSystem");
        }

        public async System.Threading.Tasks.Task RebootSystemAsync()
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "RebootSystem");
        }

        public Memory GetMemory()
        {
            return (Memory)Invoke("SolidCP.Server.WindowsServer", "GetMemory");
        }

        public async System.Threading.Tasks.Task<Memory> GetMemoryAsync()
        {
            return await InvokeAsync<Memory>("SolidCP.Server.WindowsServer", "GetMemory");
        }

        public string ExecuteSystemCommand(string path, string args)
        {
            return (string)Invoke("SolidCP.Server.WindowsServer", "ExecuteSystemCommand", path, args);
        }

        public async System.Threading.Tasks.Task<string> ExecuteSystemCommandAsync(string path, string args)
        {
            return await InvokeAsync<string>("SolidCP.Server.WindowsServer", "ExecuteSystemCommand", path, args);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class WindowsServer : SolidCP.Web.Client.ClientBase<IWindowsServer, WindowsServerAssemblyClient>, IWindowsServer
    {
        public TerminalSession[] GetTerminalServicesSessions()
        {
            return base.Client.GetTerminalServicesSessions();
        }

        public async System.Threading.Tasks.Task<TerminalSession[]> GetTerminalServicesSessionsAsync()
        {
            return await base.Client.GetTerminalServicesSessionsAsync();
        }

        public void CloseTerminalServicesSession(int sessionId)
        {
            base.Client.CloseTerminalServicesSession(sessionId);
        }

        public async System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId)
        {
            await base.Client.CloseTerminalServicesSessionAsync(sessionId);
        }

        public WindowsProcess[] GetWindowsProcesses()
        {
            return base.Client.GetWindowsProcesses();
        }

        public async System.Threading.Tasks.Task<WindowsProcess[]> GetWindowsProcessesAsync()
        {
            return await base.Client.GetWindowsProcessesAsync();
        }

        public void TerminateWindowsProcess(int pid)
        {
            base.Client.TerminateWindowsProcess(pid);
        }

        public async System.Threading.Tasks.Task TerminateWindowsProcessAsync(int pid)
        {
            await base.Client.TerminateWindowsProcessAsync(pid);
        }

        public WindowsService[] GetWindowsServices()
        {
            return base.Client.GetWindowsServices();
        }

        public async System.Threading.Tasks.Task<WindowsService[]> GetWindowsServicesAsync()
        {
            return await base.Client.GetWindowsServicesAsync();
        }

        public void ChangeWindowsServiceStatus(string id, WindowsServiceStatus status)
        {
            base.Client.ChangeWindowsServiceStatus(id, status);
        }

        public async System.Threading.Tasks.Task ChangeWindowsServiceStatusAsync(string id, WindowsServiceStatus status)
        {
            await base.Client.ChangeWindowsServiceStatusAsync(id, status);
        }

        public WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {
            return base.Client.GetWPIProducts(tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId)
        {
            return await base.Client.GetWPIProductsAsync(tabId, keywordId);
        }

        public WPIProduct[] GetWPIProductsFiltered(string filter)
        {
            return base.Client.GetWPIProductsFiltered(filter);
        }

        public async System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsFilteredAsync(string filter)
        {
            return await base.Client.GetWPIProductsFilteredAsync(filter);
        }

        public WPIProduct GetWPIProductById(string productdId)
        {
            return base.Client.GetWPIProductById(productdId);
        }

        public async System.Threading.Tasks.Task<WPIProduct> GetWPIProductByIdAsync(string productdId)
        {
            return await base.Client.GetWPIProductByIdAsync(productdId);
        }

        public WPITab[] GetWPITabs()
        {
            return base.Client.GetWPITabs();
        }

        public async System.Threading.Tasks.Task<WPITab[]> GetWPITabsAsync()
        {
            return await base.Client.GetWPITabsAsync();
        }

        public void InitWPIFeeds(string feedUrls)
        {
            base.Client.InitWPIFeeds(feedUrls);
        }

        public async System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls)
        {
            await base.Client.InitWPIFeedsAsync(feedUrls);
        }

        public WPIKeyword[] GetWPIKeywords()
        {
            return base.Client.GetWPIKeywords();
        }

        public async System.Threading.Tasks.Task<WPIKeyword[]> GetWPIKeywordsAsync()
        {
            return await base.Client.GetWPIKeywordsAsync();
        }

        public WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            return base.Client.GetWPIProductsWithDependencies(products);
        }

        public async System.Threading.Tasks.Task<WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products)
        {
            return await base.Client.GetWPIProductsWithDependenciesAsync(products);
        }

        public void InstallWPIProducts(string[] products)
        {
            base.Client.InstallWPIProducts(products);
        }

        public async System.Threading.Tasks.Task InstallWPIProductsAsync(string[] products)
        {
            await base.Client.InstallWPIProductsAsync(products);
        }

        public void CancelInstallWPIProducts()
        {
            base.Client.CancelInstallWPIProducts();
        }

        public async System.Threading.Tasks.Task CancelInstallWPIProductsAsync()
        {
            await base.Client.CancelInstallWPIProductsAsync();
        }

        public string GetWPIStatus()
        {
            return base.Client.GetWPIStatus();
        }

        public async System.Threading.Tasks.Task<string> GetWPIStatusAsync()
        {
            return await base.Client.GetWPIStatusAsync();
        }

        public string WpiGetLogFileDirectory()
        {
            return base.Client.WpiGetLogFileDirectory();
        }

        public async System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync()
        {
            return await base.Client.WpiGetLogFileDirectoryAsync();
        }

        public SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            return base.Client.WpiGetLogsInDirectory(Path);
        }

        public async System.Threading.Tasks.Task<SettingPair[]> WpiGetLogsInDirectoryAsync(string Path)
        {
            return await base.Client.WpiGetLogsInDirectoryAsync(Path);
        }

        public List<string> GetLogNames()
        {
            return base.Client.GetLogNames();
        }

        public async System.Threading.Tasks.Task<List<string>> GetLogNamesAsync()
        {
            return await base.Client.GetLogNamesAsync();
        }

        public List<SystemLogEntry> GetLogEntries(string logName)
        {
            return base.Client.GetLogEntries(logName);
        }

        public async System.Threading.Tasks.Task<List<SystemLogEntry>> GetLogEntriesAsync(string logName)
        {
            return await base.Client.GetLogEntriesAsync(logName);
        }

        public SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            return base.Client.GetLogEntriesPaged(logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows)
        {
            return await base.Client.GetLogEntriesPagedAsync(logName, startRow, maximumRows);
        }

        public void ClearLog(string logName)
        {
            base.Client.ClearLog(logName);
        }

        public async System.Threading.Tasks.Task ClearLogAsync(string logName)
        {
            await base.Client.ClearLogAsync(logName);
        }

        public void RebootSystem()
        {
            base.Client.RebootSystem();
        }

        public async System.Threading.Tasks.Task RebootSystemAsync()
        {
            await base.Client.RebootSystemAsync();
        }

        public Memory GetMemory()
        {
            return base.Client.GetMemory();
        }

        public async System.Threading.Tasks.Task<Memory> GetMemoryAsync()
        {
            return await base.Client.GetMemoryAsync();
        }

        public string ExecuteSystemCommand(string path, string args)
        {
            return base.Client.ExecuteSystemCommand(path, args);
        }

        public async System.Threading.Tasks.Task<string> ExecuteSystemCommandAsync(string path, string args)
        {
            return await base.Client.ExecuteSystemCommandAsync(path, args);
        }
    }
}
#endif