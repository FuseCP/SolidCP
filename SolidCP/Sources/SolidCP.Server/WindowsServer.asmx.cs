// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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
using SolidCP.Web.Services;
using System.ComponentModel;
using System.ServiceProcess;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Management;
using System.Collections.Specialized;
using Microsoft.Web.PlatformInstaller;
using Microsoft.Win32;
using SolidCP.Providers.Utils;
using SolidCP.Server.Code;
using SolidCP.Server.Utils;
using SolidCP.Providers;
using SolidCP.Server.WPIService;







namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for WindowsServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class WindowsServer : System.Web.Services.WebService
    {
        #region Terminal connections
        [WebMethod]
        public TerminalSession[] GetTerminalServicesSessions()
        {
            try
            {
                Log.WriteStart("GetTerminalServicesSessions");
                List<TerminalSession> sessions = new List<TerminalSession>();
                string ret = FileUtils.ExecuteSystemCommand("qwinsta", "");

                // parse returned string
                StringReader reader = new StringReader(ret);
                string line = null;
				int lineIndex = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    /*if (line.IndexOf("USERNAME") != -1 )
                        continue;*/
					//
					if (lineIndex == 0)
					{
						lineIndex++;
						continue;
					}

                    Regex re = new Regex(@"(\S+)\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection matches = re.Matches(line);

                    // add row to the table
                    string username = matches[1].Value.Trim();
                    if (Regex.IsMatch(username, "^[0-9]*$"))
                    {
                        username = "";
                    }

                    if (username != "")
                    {
                        TerminalSession session = new TerminalSession();
						//
                        session.SessionId = Int32.Parse(matches[2].Value.Trim());
                        session.Username = username;
                        session.Status = matches[3].Value.Trim();

                        sessions.Add(session);
                    }
					//
					lineIndex++;
                }
                reader.Close();

                Log.WriteEnd("GetTerminalServicesSessions");
                return sessions.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetTerminalServicesSessions", ex);
                throw;
            }
        }

        [WebMethod]
        public void CloseTerminalServicesSession(int sessionId)
        {
            try
            {
                Log.WriteStart("CloseTerminalServicesSession");
                FileUtils.ExecuteSystemCommand("rwinsta", sessionId.ToString());
                Log.WriteEnd("CloseTerminalServicesSession");
            }
            catch (Exception ex)
            {
                Log.WriteError("CloseTerminalServicesSession", ex);
                throw;
            }
        }
        #endregion

        #region Windows Processes
        [WebMethod]
        public OSProcess[] GetWindowsProcesses()
        {
            try
            {
                Log.WriteStart("GetWindowsProcesses");

                List<OSProcess> winProcesses = new List<OSProcess>();

                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objProcesses = wmi.ExecuteQuery(
                    "SELECT * FROM Win32_Process");

                foreach (ManagementObject objProcess in objProcesses)
                {
                    int pid = Int32.Parse(objProcess["ProcessID"].ToString());
                    string name = objProcess["Name"].ToString();

                    // get user info
                    string[] methodParams = new String[2];
                    objProcess.InvokeMethod("GetOwner", (object[])methodParams);
                    string username = methodParams[0];

                    OSProcess winProcess = new OSProcess();
                    winProcess.Pid = pid;
                    winProcess.Name = name;
                    winProcess.Username = username;
                    winProcess.MemUsage = Int64.Parse(objProcess["WorkingSetSize"].ToString());

                    winProcesses.Add(winProcess);
                }

                Log.WriteEnd("GetWindowsProcesses");
                return winProcesses.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWindowsProcesses", ex);
                throw;
            }
        }

        [WebMethod]
        public void TerminateWindowsProcess(int pid)
        {
            try
            {
                Log.WriteStart("TerminateWindowsProcess");

                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.Id == pid)
                        process.Kill();
                }

                Log.WriteEnd("TerminateWindowsProcess");
            }
            catch (Exception ex)
            {
                Log.WriteError("TerminateWindowsProcess", ex);
                throw;
            }
        }
        #endregion

        #region Windows Services
        [WebMethod]
        public OSService[] GetWindowsServices()
        {
            try
            {
                Log.WriteStart("GetWindowsServices");
                List<OSService> winServices = new List<OSService>();

                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    OSService winService = new OSService();
                    winService.Id = service.ServiceName;
                    winService.Name = service.DisplayName;
                    winService.CanStop = service.CanStop;
                    winService.CanPauseAndContinue = service.CanPauseAndContinue;

                    OSServiceStatus status = OSServiceStatus.ContinuePending;
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.ContinuePending: status = OSServiceStatus.ContinuePending; break;
                        case ServiceControllerStatus.Paused: status = OSServiceStatus.Paused; break;
                        case ServiceControllerStatus.PausePending: status = OSServiceStatus.PausePending; break;
                        case ServiceControllerStatus.Running: status = OSServiceStatus.Running; break;
                        case ServiceControllerStatus.StartPending: status = OSServiceStatus.StartPending; break;
                        case ServiceControllerStatus.Stopped: status = OSServiceStatus.Stopped; break;
                        case ServiceControllerStatus.StopPending: status = OSServiceStatus.StopPending; break;
                    }
                    winService.Status = status;

                    winServices.Add(winService);
                }

                Log.WriteEnd("GetWindowsServices");
                return winServices.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWindowsServices", ex);
                throw;
            }
        }

        [WebMethod]
        public void ChangeWindowsServiceStatus(string id, OSServiceStatus status)
        {
            try
            {
                Log.WriteStart("ChangeWindowsServiceStatus");
                // get all services
                ServiceController[] services = ServiceController.GetServices();

                // find required service
                foreach (ServiceController service in services)
                {
                    if (String.Compare(service.ServiceName, id, true) == 0)
                    {
                        if (status == OSServiceStatus.Paused
                            && service.Status == ServiceControllerStatus.Running)
                            service.Pause();
                        else if (status == OSServiceStatus.Running
                            && service.Status == ServiceControllerStatus.Stopped)
                            service.Start();
                        else if (status == OSServiceStatus.Stopped
                            && ((service.Status == ServiceControllerStatus.Running) ||
                                (service.Status == ServiceControllerStatus.Paused)))
                            service.Stop();
                        else if (status == OSServiceStatus.ContinuePending
                            && service.Status == ServiceControllerStatus.Paused)
                            service.Continue();
                    }
                }
                Log.WriteEnd("ChangeWindowsServiceStatus");
            }
            catch (Exception ex)
            {
                Log.WriteError("ChangeWindowsServiceStatus", ex);
                throw;
            }
        }
        #endregion

        #region Web Platform Installer

      

        private string Linkify(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            //" qweqwe http://www.helicontech.com/zoo/feed/  asdasdasd"
            Regex link =new Regex("(http[^\\s,]+)(?<![.,])");

            return link.Replace(value,"<a href=\"$1\" target=\"_blank\">$1</a>");
        }


        private WPIProduct ProductToWPIProduct(Product product)
        {
            WPIProduct p = new WPIProduct();
            p.ProductId = product.ProductId;
            p.Summary = product.Summary;
            p.LongDescription = Linkify(product.LongDescription);
            p.Published = product.Published;
            p.Author = product.Author;
            p.AuthorUri = (product.AuthorUri != null) ? product.AuthorUri.ToString() : ""; 
            p.Title = product.Title;
            p.Link = (product.Link != null) ? product.Link.ToString() : "";
            p.Version = product.Version;

            if (product.Installers.Count > 0)
            {
                if (product.Installers[0].EulaUrl != null)
                {
                    p.EulaUrl = product.Installers[0].EulaUrl.ToString();
                    
                }

                if (product.Installers[0].InstallerFile != null)
                {
                    if (product.Installers[0].InstallerFile.InstallerUrl != null)
                    {
                        p.DownloadedLocation = product.Installers[0].InstallerFile.InstallerUrl.ToString();
                    }
                    p.FileSize = product.Installers[0].InstallerFile.FileSize;
                }

            }

            if (product.IconUrl != null)
            {
                p.Logo = product.IconUrl.ToString();
            }

            p.IsInstalled = product.IsInstalled(true);

            return p;
        }

        private void CheckHostingPackagesUpgrades(IList<WPIProduct> products)
        {
            foreach (WPIProduct product in products)
            {
                string hostingPackageName = product.ProductId;
                CheckProductForUpdate(hostingPackageName, product);
            }
        }

        private void CheckProductForUpdate(string hostingPackageName, WPIProduct product)
        {
            string installedHostingPackageVersion = GetInstalledHostingPackageVersion(hostingPackageName);
            if (!string.IsNullOrEmpty(installedHostingPackageVersion))
            {
                //6
                //3.0.90.383
                if (CompareVersions(product.Version, installedHostingPackageVersion) > 0)
                {
                    product.IsUpgrade = true;
                    product.IsInstalled = false;
                }
                
            }
        }

        private decimal CompareVersions(string newVersion, string oldVersion)
        {
            try
            {
                var version1 = new Version(newVersion);
                var version2 = new Version(oldVersion);

                return version1.CompareTo(version2);

            }
            catch (ArgumentException)
            {
                return String.Compare(newVersion,oldVersion, StringComparison.Ordinal);

            }
        }

        private string GetInstalledHostingPackageVersion(string hostingPackageName)
        {
            string installedVersion = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Helicon\\Zoo", hostingPackageName + "Version", string.Empty) as string;
            if (string.IsNullOrEmpty(installedVersion))
            {
                installedVersion = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Helicon\\Zoo", hostingPackageName + "Version", string.Empty) as string;
            }

            return installedVersion;
        }


        [WebMethod]
        public WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {


            try
            {
                Log.WriteStart("GetWPIProducts");
                List<WPIProduct> wpiProducts = new List<WPIProduct>();


                WpiHelper wpi = GetWpiFeed();

                string feedLocation = null;
                if (tabId != null)
                {
                    Tab tab = wpi.GetTab(tabId);
                    ICollection<string> feeds = tab.FeedList;
                    feedLocation = feeds.GetEnumerator().Current;
                }

                List<Product> products = wpi.GetProductsToInstall(feedLocation, keywordId);

                if (products != null)
                {


                    foreach (Product product in products)
                    {
                        if (null != product && !product.IsApplication)
                        {
                            wpiProducts.Add(ProductToWPIProduct(product));

                        }
                    }

                    // check upgrades for Hosting Packages (ZooPackage keyword)
                    if (WPIKeyword.HOSTING_PACKAGE_KEYWORD == keywordId)
                    {
                        CheckHostingPackagesUpgrades(wpiProducts);
                    }


                }

               

                Log.WriteEnd("GetWPIProducts");
                return wpiProducts.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWPIProducts", ex);
                throw;
            }
        }


        [WebMethod]
        public WPIProduct[] GetWPIProductsFiltered(string filter)
        {


            try
            {
                Log.WriteStart("GetWPIProductsFiltered");
                List<WPIProduct> wpiProducts = new List<WPIProduct>();


                WpiHelper wpi = GetWpiFeed();

                List<Product> products = wpi.GetProductsFiltered( filter);

                if (products != null)
                {


                    foreach (Product product in products)
                    {
                        if (null != product && !product.IsApplication)
                        {
                            wpiProducts.Add(ProductToWPIProduct(product));

                        }
                    }

                }



                Log.WriteEnd("GetWPIProductsFiltered");
                return wpiProducts.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWPIProductsFiltered", ex);
                throw;
            }
        }

        [WebMethod]
        public WPIProduct GetWPIProductById(string productdId)
        {
            try
            {
                Log.WriteStart("GetWPIProductById");
                WpiHelper wpi = GetWpiFeed();

                Product product = wpi.GetWPIProductById(productdId);
                WPIProduct wpiProduct = ProductToWPIProduct(product);

                if (wpiProduct.ProductId == "HeliconZooModule")
                {
                    /*null string = HeliconZooModule in registry*/
                    CheckProductForUpdate("", wpiProduct);
                }

                Log.WriteEnd("GetWPIProductById");
                return wpiProduct;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWPIProductById", ex);
                throw;
            }
        }

        [WebMethod]
        public WPITab[] GetWPITabs()
        {
            try
            {
                Log.WriteStart("GetWPITabs");

                WpiHelper wpi = GetWpiFeed();

                List<WPITab> result = new List<WPITab>();

                foreach (Tab tab in wpi.GetTabs())
                {
                    result.Add(new WPITab(tab.Id, tab.Name));
                }


                Log.WriteEnd("GetWPITabs");

                return result.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWPITabs", ex);
                throw;
            }
        }

       
        static private string[] _feeds = new string[]{};

        [WebMethod]
        public void InitWPIFeeds(string feedUrls)
        {
            if (string.IsNullOrEmpty(feedUrls))
            {
                throw new Exception("Empty feed list");
            }

            string[] newFeeds = feedUrls.Split(';');

            if (newFeeds.Length == 0)
            {
                throw new Exception("Empty feed list");
            }
            if (!ArraysEqual<string>(newFeeds, _feeds))
            {
                Log.WriteInfo("InitWPIFeeds - new value: " + feedUrls);

                //Feeds settings have been channged
                _feeds = newFeeds;
                wpi = null;

            }
        }


        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        [WebMethod]
        public WPIKeyword[] GetWPIKeywords()
        {
            try
            {
                Log.WriteStart("GetWPIKeywords");

                WpiHelper wpi = GetWpiFeed();

                List<WPIKeyword> result = new List<WPIKeyword>();

                result.Add(new WPIKeyword("", "All"));

                foreach (Keyword keyword in wpi.GetKeywords())
                {
                    if (!wpi.IsKeywordApplication(keyword))
                    {
                        result.Add(new WPIKeyword(keyword.Id, keyword.Text));
                    }

                }


                Log.WriteEnd("GetWPIKeywords");

                return result.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWPIKeywords", ex);
                throw;
            }
        }


        [WebMethod]
        public WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            try
            {
                Log.WriteStart("GetWPIProductsWithDependencies");

                WpiHelper wpi = GetWpiFeed();

                List<WPIProduct> result = new List<WPIProduct>();
                foreach (Product product in wpi.GetProductsToInstallWithDependencies(products))
                {
                    result.Add(ProductToWPIProduct(product));
                }

                Log.WriteEnd("GetWPIProductsWithDependencies");

                return result.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWPIProductsWithDependencies", ex);
                throw;
            }
        }

        static Process _WpiServiceExe = null;

        [WebMethod]
        public void InstallWPIProducts(string[] products)
        {
            try
            {
                Log.WriteStart("InstallWPIProducts");

                StartWpiService();

                RegisterWpiService();
                
                WPIServiceContract client = new WPIServiceContract();

                client.Initialize(_feeds);
                client.BeginInstallation(products);

                


               
                Log.WriteEnd("InstallWPIProducts");
            }
            catch (Exception ex)
            {
                Log.WriteError("InstallWPIProducts", ex);
                throw;
            }
        }

     
        private void StartWpiService()
        {
            string binFolder = HttpContext.Current.Server.MapPath("/bin/");
            string workingDirectory = Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "Temp\\zoo.wpi");

            //string newUserProfile = Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "Temp\\zoo.wpi");
            //string newAppData = Path.Combine(newUserProfile, "Roaming");
            //string newLocalAppData = Path.Combine(newUserProfile, "Local");
            //try
            //{
            //    Directory.CreateDirectory(newUserProfile);
            //    Directory.CreateDirectory(newAppData);
            //    Directory.CreateDirectory(newLocalAppData);
            //}
            //catch (Exception)
            //{
            //    //throw;
            //}


            Process wpiServiceExe = new Process();
            wpiServiceExe.StartInfo = new ProcessStartInfo(Path.Combine(binFolder, "SolidCP.Server.WPIService.exe"));
            wpiServiceExe.StartInfo.WorkingDirectory = workingDirectory;
            wpiServiceExe.StartInfo.UseShellExecute = false;
            wpiServiceExe.StartInfo.LoadUserProfile = true;
            wpiServiceExe.StartInfo.EnvironmentVariables["MySqlPassword"] = "";
            //wpiServiceExe.StartInfo.EnvironmentVariables["UserProfile"] = newUserProfile;
            //wpiServiceExe.StartInfo.EnvironmentVariables["LocalAppData"] = newLocalAppData;
            //wpiServiceExe.StartInfo.EnvironmentVariables["AppData"] = newAppData;
            if (wpiServiceExe.Start())
            {
                _WpiServiceExe = wpiServiceExe;
            }
        }

        [WebMethod]
        public void CancelInstallWPIProducts()
        {
            try
            {
                Log.WriteStart("CancelInstallWPIProducts");

                KillWpiService();


                Log.WriteEnd("CancelInstallWPIProducts");
            }
            catch (Exception ex)
            {
                Log.WriteError("CancelInstallWPIProducts", ex);
                throw;
            }
        }

        private void KillWpiService()
        {
            //kill own service
            if (_WpiServiceExe != null && !_WpiServiceExe.HasExited)
            {
                _WpiServiceExe.Kill();
                _WpiServiceExe = null;
            }
            else
            {
                //find SolidCP.Server.WPIService.exe
                Process[] wpiservices = Process.GetProcessesByName("SolidCP.Server.WPIService");
                foreach (Process p in wpiservices)
                {
                    p.Kill();
                }
            }
        }

        [WebMethod]
        public string GetWPIStatus()
        {
            try
            {
                Log.WriteStart("GetWPIStatus");

                RegisterWpiService();
                
                WPIServiceContract client = new WPIServiceContract();

                string status = client.GetStatus();

                Log.WriteEnd("GetWPIStatus");

                return status; //OK
            }
            catch (Exception ex)
            {
                // done or error

                if (_WpiServiceExe == null || _WpiServiceExe.HasExited)
                {
                    // reset WpiHelper for refresh status
                    wpi = null;
                    return ""; //OK
                }

                Log.WriteError("GetWPIStatus", ex);

                return ex.ToString();
            }
        }

        [WebMethod]
        public string WpiGetLogFileDirectory()
        {
            try
            {
                Log.WriteStart("WpiGetLogFileDirectory");

                RegisterWpiService();
                
                WPIServiceContract client = new WPIServiceContract();

                string result = client.GetLogFileDirectory();

                Log.WriteEnd("WpiGetLogFileDirectory");

                return result; //OK
            }
            catch (Exception ex)
            {

                Log.WriteError("WpiGetLogFileDirectory", ex);

                //throw;
                return string.Empty;
            }
        }

        [WebMethod]
        public SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            try
            {
                Log.WriteStart("WpiGetLogsInDirectory");

                ArrayList result = new ArrayList();

                string[] filePaths = Directory.GetFiles(Path);
                foreach (string filePath in filePaths)
                {
                    using (StreamReader streamReader = new StreamReader(filePath))
                    {
                        string fileContent = SecurityElement.Escape(StringUtils.CleanupASCIIControlCharacters(streamReader.ReadToEnd()));
                        result.Add(new SettingPair(filePath, fileContent));
                    }
                    
                }

                Log.WriteEnd("WpiGetLogFileDirectory");

                return (SettingPair[])result.ToArray(typeof(SettingPair)); //OK
            }
            catch (Exception ex)
            {

                Log.WriteError("WpiGetLogFileDirectory", ex);

                //throw;
                return null;
            }
        }

        
      
     

        static WpiHelper wpi = null;
        WpiHelper GetWpiFeed()
        {
            if (_feeds.Length == 0)
            {
                throw new Exception("Empty feed list");
            }

            if (null == wpi)
            {
                wpi = new WpiHelper(_feeds);
            }
            return wpi;
        }

        private static object _lockRegisterWpiService = new object();
        private void RegisterWpiService()
        {
            lock (_lockRegisterWpiService)
            {
                
                
                try
                {
                    ChannelServices.RegisterChannel(new TcpChannel(), true);
                }
                catch (System.Exception)
                {
                    //ignor
                }

                if (null == RemotingConfiguration.IsWellKnownClientType(typeof(WPIServiceContract)))
                {
                    RemotingConfiguration.RegisterWellKnownClientType(typeof(WPIServiceContract), string.Format("tcp://localhost:{0}/WPIServiceContract", WPIServiceContract.PORT));
                }

                try
                {
                    WPIServiceContract client = new WPIServiceContract();
                    client.Ping();
                }
                catch (Exception)
                {
                    //unable to connect 
                    //try to restart service
                    KillWpiService();
                    //StartWpiService();
                }

               
             

            }

            
        }
        #endregion GetWPIProducts


        #region Event Viewer
        [WebMethod]
        public List<string> GetLogNames()
        {
            List<string> logs = new List<string>();
            EventLog[] eventLogs = EventLog.GetEventLogs();
            foreach (EventLog eventLog in eventLogs)
            {
                logs.Add(eventLog.Log);
            }
            return logs;
        }

        [WebMethod]
        public List<SystemLogEntry> GetLogEntries(string logName)
        {
            SystemLogEntriesPaged result = new SystemLogEntriesPaged();
            List<SystemLogEntry> entries = new List<SystemLogEntry>();

            if (String.IsNullOrEmpty(logName))
                return entries;
			
            EventLog log = new EventLog(logName);
            EventLogEntryCollection logEntries = log.Entries;
            int count = logEntries.Count;

            // iterate in reverse order
            for (int i = count - 1; i >= 0; i--)
                entries.Add(CreateLogEntry(logEntries[i], false));

            return entries;
        }

        [WebMethod]
        public SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            SystemLogEntriesPaged result = new SystemLogEntriesPaged();
            List<SystemLogEntry> entries = new List<SystemLogEntry>();

            if (String.IsNullOrEmpty(logName))
            {
                result.Count = 0;
                result.Entries = new SystemLogEntry[] { };
                return result;
            }
			
            EventLog log = new EventLog(logName);
            EventLogEntryCollection logEntries = log.Entries;
            int count = logEntries.Count;
            result.Count = count;

            // iterate in reverse order
            startRow = count - 1 - startRow;
            int endRow = startRow - maximumRows + 1;
            if (endRow < 0)
                endRow = 0;

            for (int i = startRow; i >= endRow; i--)
                entries.Add(CreateLogEntry(logEntries[i], true));

            result.Entries = entries.ToArray();

            return result;
        }

        [WebMethod]
        public void ClearLog(string logName)
        {
			EventLog log = new EventLog(logName);
			log.Clear();
        }

        private SystemLogEntry CreateLogEntry(EventLogEntry logEntry, bool includeMessage)
        {
            SystemLogEntry entry = new SystemLogEntry();
            switch (logEntry.EntryType)
            {
                case EventLogEntryType.Error: entry.EntryType = SystemLogEntryType.Error; break;
                case EventLogEntryType.Warning: entry.EntryType = SystemLogEntryType.Warning; break;
                case EventLogEntryType.Information: entry.EntryType = SystemLogEntryType.Information; break;
                case EventLogEntryType.SuccessAudit: entry.EntryType = SystemLogEntryType.SuccessAudit; break;
                case EventLogEntryType.FailureAudit: entry.EntryType = SystemLogEntryType.FailureAudit; break;
            }

            entry.Created = logEntry.TimeGenerated;
            entry.Source = logEntry.Source;
            entry.Category = logEntry.Category;
            entry.EventID = logEntry.InstanceId;
            entry.UserName = logEntry.UserName;
            entry.MachineName = logEntry.MachineName;

            if (includeMessage)
                entry.Message = logEntry.Message;

            return entry;
        }
        #endregion

        #region Reboot
        [WebMethod]
        public void RebootSystem()
        {
            try
            {
                Log.WriteStart("RebootSystem");
                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objOses = wmi.ExecuteQuery("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject objOs in objOses)
                {
                    objOs.Scope.Options.EnablePrivileges = true;
                    objOs.InvokeMethod("Reboot", null);
                }
                Log.WriteEnd("RebootSystem");
            }
            catch (Exception ex)
            {
                Log.WriteError("RebootSystem", ex);
                throw;
            }
        }
        #endregion

        #region OS informations
        [WebMethod]
        public Memory GetMemory()
        {            
            try
            {
                Log.WriteStart("GetMemory");
                Memory memory = new Memory();
                
                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objOses = wmi.ExecuteQuery("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject objOs in objOses)
                {
                    memory.FreePhysicalMemoryKB = UInt64.Parse(objOs["FreePhysicalMemory"].ToString());
                    memory.TotalVisibleMemorySizeKB = UInt64.Parse(objOs["TotalVisibleMemorySize"].ToString());
                    memory.TotalVirtualMemorySizeKB = UInt64.Parse(objOs["TotalVirtualMemorySize"].ToString());
                    memory.FreeVirtualMemoryKB = UInt64.Parse(objOs["FreeVirtualMemory"].ToString());
                }
                Log.WriteEnd("GetMemory");
                return memory;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetMemory", ex);
                throw;
            }
        }
        #endregion

        #region System Commands
        [WebMethod]
        public string ExecuteSystemCommand(string path, string args)
        {
            try
            {
                Log.WriteStart("ExecuteSystemCommand");
                string result = FileUtils.ExecuteSystemCommand(path, args);
                Log.WriteEnd("ExecuteSystemCommand");
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError("ExecuteSystemCommand", ex);
                throw;
            }
        }
        #endregion
    }

 
}
