#if !Client
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
using System.ServiceModel.Activation;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IWindowsServer
    {
        [WebMethod]
        [OperationContract]
        TerminalSession[] GetTerminalServicesSessions();
        [WebMethod]
        [OperationContract]
        void CloseTerminalServicesSession(int sessionId);
        [WebMethod]
        [OperationContract]
        WindowsProcess[] GetWindowsProcesses();
        [WebMethod]
        [OperationContract]
        void TerminateWindowsProcess(int pid);
        [WebMethod]
        [OperationContract]
        WindowsService[] GetWindowsServices();
        [WebMethod]
        [OperationContract]
        void ChangeWindowsServiceStatus(string id, WindowsServiceStatus status);
        [WebMethod]
        [OperationContract]
        WPIProduct[] GetWPIProducts(string tabId, string keywordId);
        [WebMethod]
        [OperationContract]
        WPIProduct[] GetWPIProductsFiltered(string filter);
        [WebMethod]
        [OperationContract]
        WPIProduct GetWPIProductById(string productdId);
        [WebMethod]
        [OperationContract]
        WPITab[] GetWPITabs();
        [WebMethod]
        [OperationContract]
        void InitWPIFeeds(string feedUrls);
        [WebMethod]
        [OperationContract]
        WPIKeyword[] GetWPIKeywords();
        [WebMethod]
        [OperationContract]
        WPIProduct[] GetWPIProductsWithDependencies(string[] products);
        [WebMethod]
        [OperationContract]
        void InstallWPIProducts(string[] products);
        [WebMethod]
        [OperationContract]
        void CancelInstallWPIProducts();
        [WebMethod]
        [OperationContract]
        string GetWPIStatus();
        [WebMethod]
        [OperationContract]
        string WpiGetLogFileDirectory();
        [WebMethod]
        [OperationContract]
        SettingPair[] WpiGetLogsInDirectory(string Path);
        [WebMethod]
        [OperationContract]
        List<string> GetLogNames();
        [WebMethod]
        [OperationContract]
        List<SystemLogEntry> GetLogEntries(string logName);
        [WebMethod]
        [OperationContract]
        SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        void ClearLog(string logName);
        [WebMethod]
        [OperationContract]
        void RebootSystem();
        [WebMethod]
        [OperationContract]
        Memory GetMemory();
        [WebMethod]
        [OperationContract]
        string ExecuteSystemCommand(string path, string args);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WindowsServer : SolidCP.Server.WindowsServer, IWindowsServer
    {
        public new TerminalSession[] GetTerminalServicesSessions()
        {
            return base.GetTerminalServicesSessions();
        }

        public new void CloseTerminalServicesSession(int sessionId)
        {
            base.CloseTerminalServicesSession(sessionId);
        }

        public new WindowsProcess[] GetWindowsProcesses()
        {
            return base.GetWindowsProcesses();
        }

        public new void TerminateWindowsProcess(int pid)
        {
            base.TerminateWindowsProcess(pid);
        }

        public new WindowsService[] GetWindowsServices()
        {
            return base.GetWindowsServices();
        }

        public new void ChangeWindowsServiceStatus(string id, WindowsServiceStatus status)
        {
            base.ChangeWindowsServiceStatus(id, status);
        }

        public new WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {
            return base.GetWPIProducts(tabId, keywordId);
        }

        public new WPIProduct[] GetWPIProductsFiltered(string filter)
        {
            return base.GetWPIProductsFiltered(filter);
        }

        public new WPIProduct GetWPIProductById(string productdId)
        {
            return base.GetWPIProductById(productdId);
        }

        public new WPITab[] GetWPITabs()
        {
            return base.GetWPITabs();
        }

        public new void InitWPIFeeds(string feedUrls)
        {
            base.InitWPIFeeds(feedUrls);
        }

        public new WPIKeyword[] GetWPIKeywords()
        {
            return base.GetWPIKeywords();
        }

        public new WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            return base.GetWPIProductsWithDependencies(products);
        }

        public new void InstallWPIProducts(string[] products)
        {
            base.InstallWPIProducts(products);
        }

        public new void CancelInstallWPIProducts()
        {
            base.CancelInstallWPIProducts();
        }

        public new string GetWPIStatus()
        {
            return base.GetWPIStatus();
        }

        public new string WpiGetLogFileDirectory()
        {
            return base.WpiGetLogFileDirectory();
        }

        public new SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            return base.WpiGetLogsInDirectory(Path);
        }

        public new List<string> GetLogNames()
        {
            return base.GetLogNames();
        }

        public new List<SystemLogEntry> GetLogEntries(string logName)
        {
            return base.GetLogEntries(logName);
        }

        public new SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            return base.GetLogEntriesPaged(logName, startRow, maximumRows);
        }

        public new void ClearLog(string logName)
        {
            base.ClearLog(logName);
        }

        public new void RebootSystem()
        {
            base.RebootSystem();
        }

        public new Memory GetMemory()
        {
            return base.GetMemory();
        }

        public new string ExecuteSystemCommand(string path, string args)
        {
            return base.ExecuteSystemCommand(path, args);
        }
    }
}
#endif