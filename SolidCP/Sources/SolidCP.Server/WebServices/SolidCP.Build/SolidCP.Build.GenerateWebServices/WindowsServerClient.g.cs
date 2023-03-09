#if Client
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IWindowsServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IWindowsServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessionsResponse")]
        SolidCP.Server.TerminalSession[] GetTerminalServicesSessions();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetTerminalServicesSessionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.TerminalSession[]> GetTerminalServicesSessionsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSessionResponse")]
        void CloseTerminalServicesSession(int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/CloseTerminalServicesSessionResponse")]
        System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcesses", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcessesResponse")]
        SolidCP.Server.WindowsProcess[] GetWindowsProcesses();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcesses", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsProcessesResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WindowsProcess[]> GetWindowsProcessesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcess", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcessResponse")]
        void TerminateWindowsProcess(int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcess", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/TerminateWindowsProcessResponse")]
        System.Threading.Tasks.Task TerminateWindowsProcessAsync(int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServices", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServicesResponse")]
        SolidCP.Server.WindowsService[] GetWindowsServices();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServices", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWindowsServicesResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WindowsService[]> GetWindowsServicesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatus", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatusResponse")]
        void ChangeWindowsServiceStatus(string id, SolidCP.Server.WindowsServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatus", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ChangeWindowsServiceStatusResponse")]
        System.Threading.Tasks.Task ChangeWindowsServiceStatusAsync(string id, SolidCP.Server.WindowsServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProducts(string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFilteredResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(string filter);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsFilteredResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(string filter);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductByIdResponse")]
        SolidCP.Server.WPIProduct GetWPIProductById(string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabsResponse")]
        SolidCP.Server.WPITab[] GetWPITabs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPITabsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeedsResponse")]
        void InitWPIFeeds(string feedUrls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/InitWPIFeedsResponse")]
        System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywordsResponse")]
        SolidCP.Server.WPIKeyword[] GetWPIKeywords();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIKeywordsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependenciesResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetWPIProductsWithDependenciesResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products);
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
        SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogsInDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/WpiGetLogsInDirectoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNames", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNamesResponse")]
        System.Collections.Generic.List<string> GetLogNames();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNames", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogNamesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetLogNamesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesResponse")]
        System.Collections.Generic.List<SolidCP.Server.SystemLogEntry> GetLogEntries(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Server.SystemLogEntry>> GetLogEntriesAsync(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPagedResponse")]
        SolidCP.Server.SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetLogEntriesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ClearLog", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ClearLogResponse")]
        void ClearLog(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ClearLog", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ClearLogResponse")]
        System.Threading.Tasks.Task ClearLogAsync(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystem", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystemResponse")]
        void RebootSystem();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystem", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/RebootSystemResponse")]
        System.Threading.Tasks.Task RebootSystemAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetMemory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetMemoryResponse")]
        SolidCP.Server.Memory GetMemory();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/GetMemory", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/GetMemoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.Memory> GetMemoryAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommand", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommandResponse")]
        string ExecuteSystemCommand(string path, string args);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommand", ReplyAction = "http://smbsaas/solidcp/server/IWindowsServer/ExecuteSystemCommandResponse")]
        System.Threading.Tasks.Task<string> ExecuteSystemCommandAsync(string path, string args);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class WindowsServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IWindowsServer
    {
        public SolidCP.Server.TerminalSession[] GetTerminalServicesSessions()
        {
            return (SolidCP.Server.TerminalSession[])Invoke("SolidCP.Server.WindowsServer", "GetTerminalServicesSessions");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.TerminalSession[]> GetTerminalServicesSessionsAsync()
        {
            return await InvokeAsync<SolidCP.Server.TerminalSession[]>("SolidCP.Server.WindowsServer", "GetTerminalServicesSessions");
        }

        public void CloseTerminalServicesSession(int sessionId)
        {
            Invoke("SolidCP.Server.WindowsServer", "CloseTerminalServicesSession", sessionId);
        }

        public async System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "CloseTerminalServicesSession", sessionId);
        }

        public SolidCP.Server.WindowsProcess[] GetWindowsProcesses()
        {
            return (SolidCP.Server.WindowsProcess[])Invoke("SolidCP.Server.WindowsServer", "GetWindowsProcesses");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WindowsProcess[]> GetWindowsProcessesAsync()
        {
            return await InvokeAsync<SolidCP.Server.WindowsProcess[]>("SolidCP.Server.WindowsServer", "GetWindowsProcesses");
        }

        public void TerminateWindowsProcess(int pid)
        {
            Invoke("SolidCP.Server.WindowsServer", "TerminateWindowsProcess", pid);
        }

        public async System.Threading.Tasks.Task TerminateWindowsProcessAsync(int pid)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "TerminateWindowsProcess", pid);
        }

        public SolidCP.Server.WindowsService[] GetWindowsServices()
        {
            return (SolidCP.Server.WindowsService[])Invoke("SolidCP.Server.WindowsServer", "GetWindowsServices");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WindowsService[]> GetWindowsServicesAsync()
        {
            return await InvokeAsync<SolidCP.Server.WindowsService[]>("SolidCP.Server.WindowsServer", "GetWindowsServices");
        }

        public void ChangeWindowsServiceStatus(string id, SolidCP.Server.WindowsServiceStatus status)
        {
            Invoke("SolidCP.Server.WindowsServer", "ChangeWindowsServiceStatus", id, status);
        }

        public async System.Threading.Tasks.Task ChangeWindowsServiceStatusAsync(string id, SolidCP.Server.WindowsServiceStatus status)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "ChangeWindowsServiceStatus", id, status);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {
            return (SolidCP.Server.WPIProduct[])Invoke("SolidCP.Server.WindowsServer", "GetWPIProducts", tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.Server.WindowsServer", "GetWPIProducts", tabId, keywordId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(string filter)
        {
            return (SolidCP.Server.WPIProduct[])Invoke("SolidCP.Server.WindowsServer", "GetWPIProductsFiltered", filter);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(string filter)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.Server.WindowsServer", "GetWPIProductsFiltered", filter);
        }

        public SolidCP.Server.WPIProduct GetWPIProductById(string productdId)
        {
            return (SolidCP.Server.WPIProduct)Invoke("SolidCP.Server.WindowsServer", "GetWPIProductById", productdId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(string productdId)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct>("SolidCP.Server.WindowsServer", "GetWPIProductById", productdId);
        }

        public SolidCP.Server.WPITab[] GetWPITabs()
        {
            return (SolidCP.Server.WPITab[])Invoke("SolidCP.Server.WindowsServer", "GetWPITabs");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync()
        {
            return await InvokeAsync<SolidCP.Server.WPITab[]>("SolidCP.Server.WindowsServer", "GetWPITabs");
        }

        public void InitWPIFeeds(string feedUrls)
        {
            Invoke("SolidCP.Server.WindowsServer", "InitWPIFeeds", feedUrls);
        }

        public async System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls)
        {
            await InvokeAsync("SolidCP.Server.WindowsServer", "InitWPIFeeds", feedUrls);
        }

        public SolidCP.Server.WPIKeyword[] GetWPIKeywords()
        {
            return (SolidCP.Server.WPIKeyword[])Invoke("SolidCP.Server.WindowsServer", "GetWPIKeywords");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync()
        {
            return await InvokeAsync<SolidCP.Server.WPIKeyword[]>("SolidCP.Server.WindowsServer", "GetWPIKeywords");
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            return (SolidCP.Server.WPIProduct[])Invoke("SolidCP.Server.WindowsServer", "GetWPIProductsWithDependencies", products);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.Server.WindowsServer", "GetWPIProductsWithDependencies", products);
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

        public SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            return (SolidCP.Providers.SettingPair[])Invoke("SolidCP.Server.WindowsServer", "WpiGetLogsInDirectory", Path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(string Path)
        {
            return await InvokeAsync<SolidCP.Providers.SettingPair[]>("SolidCP.Server.WindowsServer", "WpiGetLogsInDirectory", Path);
        }

        public System.Collections.Generic.List<string> GetLogNames()
        {
            return (System.Collections.Generic.List<string>)Invoke("SolidCP.Server.WindowsServer", "GetLogNames");
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetLogNamesAsync()
        {
            return await InvokeAsync<System.Collections.Generic.List<string>>("SolidCP.Server.WindowsServer", "GetLogNames");
        }

        public System.Collections.Generic.List<SolidCP.Server.SystemLogEntry> GetLogEntries(string logName)
        {
            return (System.Collections.Generic.List<SolidCP.Server.SystemLogEntry>)Invoke("SolidCP.Server.WindowsServer", "GetLogEntries", logName);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Server.SystemLogEntry>> GetLogEntriesAsync(string logName)
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Server.SystemLogEntry>>("SolidCP.Server.WindowsServer", "GetLogEntries", logName);
        }

        public SolidCP.Server.SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            return (SolidCP.Server.SystemLogEntriesPaged)Invoke("SolidCP.Server.WindowsServer", "GetLogEntriesPaged", logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Server.SystemLogEntriesPaged>("SolidCP.Server.WindowsServer", "GetLogEntriesPaged", logName, startRow, maximumRows);
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

        public SolidCP.Server.Memory GetMemory()
        {
            return (SolidCP.Server.Memory)Invoke("SolidCP.Server.WindowsServer", "GetMemory");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.Memory> GetMemoryAsync()
        {
            return await InvokeAsync<SolidCP.Server.Memory>("SolidCP.Server.WindowsServer", "GetMemory");
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
        public SolidCP.Server.TerminalSession[] GetTerminalServicesSessions()
        {
            return base.Client.GetTerminalServicesSessions();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.TerminalSession[]> GetTerminalServicesSessionsAsync()
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

        public SolidCP.Server.WindowsProcess[] GetWindowsProcesses()
        {
            return base.Client.GetWindowsProcesses();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WindowsProcess[]> GetWindowsProcessesAsync()
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

        public SolidCP.Server.WindowsService[] GetWindowsServices()
        {
            return base.Client.GetWindowsServices();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WindowsService[]> GetWindowsServicesAsync()
        {
            return await base.Client.GetWindowsServicesAsync();
        }

        public void ChangeWindowsServiceStatus(string id, SolidCP.Server.WindowsServiceStatus status)
        {
            base.Client.ChangeWindowsServiceStatus(id, status);
        }

        public async System.Threading.Tasks.Task ChangeWindowsServiceStatusAsync(string id, SolidCP.Server.WindowsServiceStatus status)
        {
            await base.Client.ChangeWindowsServiceStatusAsync(id, status);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {
            return base.Client.GetWPIProducts(tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId)
        {
            return await base.Client.GetWPIProductsAsync(tabId, keywordId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(string filter)
        {
            return base.Client.GetWPIProductsFiltered(filter);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(string filter)
        {
            return await base.Client.GetWPIProductsFilteredAsync(filter);
        }

        public SolidCP.Server.WPIProduct GetWPIProductById(string productdId)
        {
            return base.Client.GetWPIProductById(productdId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(string productdId)
        {
            return await base.Client.GetWPIProductByIdAsync(productdId);
        }

        public SolidCP.Server.WPITab[] GetWPITabs()
        {
            return base.Client.GetWPITabs();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync()
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

        public SolidCP.Server.WPIKeyword[] GetWPIKeywords()
        {
            return base.Client.GetWPIKeywords();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync()
        {
            return await base.Client.GetWPIKeywordsAsync();
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            return base.Client.GetWPIProductsWithDependencies(products);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products)
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

        public SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            return base.Client.WpiGetLogsInDirectory(Path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(string Path)
        {
            return await base.Client.WpiGetLogsInDirectoryAsync(Path);
        }

        public System.Collections.Generic.List<string> GetLogNames()
        {
            return base.Client.GetLogNames();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetLogNamesAsync()
        {
            return await base.Client.GetLogNamesAsync();
        }

        public System.Collections.Generic.List<SolidCP.Server.SystemLogEntry> GetLogEntries(string logName)
        {
            return base.Client.GetLogEntries(logName);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Server.SystemLogEntry>> GetLogEntriesAsync(string logName)
        {
            return await base.Client.GetLogEntriesAsync(logName);
        }

        public SolidCP.Server.SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            return base.Client.GetLogEntriesPaged(logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows)
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

        public SolidCP.Server.Memory GetMemory()
        {
            return base.Client.GetMemory();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.Memory> GetMemoryAsync()
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