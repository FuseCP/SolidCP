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
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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
}
#endif