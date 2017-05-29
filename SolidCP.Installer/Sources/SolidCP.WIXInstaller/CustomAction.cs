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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Xml;

using Microsoft.Deployment.WindowsInstaller;

using SolidCP.Setup;
using SolidCP.Setup.Internal;
using SolidCP.WIXInstaller.Common;
using SolidCP.WIXInstaller.Common.Util;

namespace SolidCP.WIXInstaller
{
    public class CustomActions
    {
        public static List<string> SysDb = new List<string> { "tempdb", "master", "model", "msdb" };
        public const string SQL_AUTH_WINDOWS = "Windows Authentication";
        public const string SQL_AUTH_SERVER = "SQL Server Authentication";

        #region CustomActions
        [CustomAction]
        public static ActionResult OnScpa(Session Ctx)
        {
            PopUpDebugger();
            Func<Hashtable, string, string> GetParam = (s, x) => Utils.GetStringSetupParameter(s, x);
            Ctx.AttachToSetupLog();
            Log.WriteStart("OnScpa");

            try
            {
                var Hash = Ctx.CustomActionData.ToNonGenericDictionary() as Hashtable;
                var Scpa = new SolidCP.Setup.Actions.ConfigureStandaloneServerAction();
                Scpa.ServerSetup = new SetupVariables { };
                Scpa.EnterpriseServerSetup = new SetupVariables { };
                Scpa.PortalSetup = new SetupVariables { };
                Scpa.EnterpriseServerSetup.ServerAdminPassword = GetParam(Hash, "ServerAdminPassword");
                Scpa.EnterpriseServerSetup.PeerAdminPassword = Scpa.EnterpriseServerSetup.ServerAdminPassword;
                Scpa.PortalSetup.InstallerFolder = GetParam(Hash, "InstallerFolder");
                Scpa.PortalSetup.ComponentId = WiXSetup.GetComponentID(Scpa.PortalSetup);
                AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = WiXSetup.GetFullConfigPath(Scpa.PortalSetup) });
                Scpa.PortalSetup.EnterpriseServerURL = GetParam(Hash, "EnterpriseServerUrl");
                Scpa.PortalSetup.WebSiteIP = GetParam(Hash, "PortalWebSiteIP");
                Scpa.ServerSetup.WebSiteIP = GetParam(Hash, "ServerWebSiteIP");
                Scpa.ServerSetup.WebSitePort = GetParam(Hash, "ServerWebSitePort");
                Scpa.ServerSetup.ServerPassword = GetParam(Hash, "ServerPassword");
                var Make = Scpa as SolidCP.Setup.Actions.IInstallAction;
                Make.Run(null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.ToString());
            }
            Log.WriteEnd("OnScpa");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult OnServerPrepare(Session Ctx)
        {
            PopUpDebugger();

            Ctx.AttachToSetupLog();
            Log.WriteStart("OnServerPrepare");
            GetPrepareScript(Ctx).Run();
            Log.WriteEnd("OnServerPrepare");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult OnEServerPrepare(Session Ctx)
        {
            PopUpDebugger();

            Ctx.AttachToSetupLog();
            Log.WriteStart("OnEServerPrepare");
            GetPrepareScript(Ctx).Run();
            Log.WriteEnd("OnEServerPrepare");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult OnPortalPrepare(Session Ctx)
        {
            PopUpDebugger();

            Ctx.AttachToSetupLog();
            Log.WriteStart("OnPortalPrepare");
            GetPrepareScript(Ctx).Run();
            Log.WriteEnd("OnPortalPrepare");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult OnSchedulerPrepare(Session Ctx)
        {
            PopUpDebugger();
            Ctx.AttachToSetupLog();
            Log.WriteStart("OnSchedulerPrepare");
            try
            {
                var ServiceName = Global.Parameters.SchedulerServiceName;
                var ServiceFile = string.Empty;
                ManagementClass WmiService = new ManagementClass("Win32_Service");
                foreach (var MObj in WmiService.GetInstances())
                {
                    if (MObj.GetPropertyValue("Name").ToString().Equals(ServiceName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        ServiceFile = MObj.GetPropertyValue("PathName").ToString();
                        break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(ServiceName) && !string.IsNullOrWhiteSpace(ServiceFile))
                {
                    var CtxVars = new SetupVariables() { ServiceName = ServiceName, ServiceFile = ServiceFile };
                    var Script = new BackupSchedulerScript(CtxVars);
                    Script.Actions.Add(new InstallAction(ActionTypes.UnregisterWindowsService)
                    {
                        Name = ServiceName,
                        Path = ServiceFile,
                        Description = "Removing Windows service...",
                        Log = string.Format("- Remove {0} Windows service", ServiceName)
                    });
                    Script.Context.ServiceName = Global.Parameters.SchedulerServiceName;
                    Script.Run();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.ToString());
            }
            Log.WriteEnd("OnSchedulerPrepare");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult MaintenanceServer(Session session)
        {
            PopUpDebugger();
            var Result = ActionResult.Success;
            Log.WriteStart("MaintenanceServer");
            Result = ProcessInstall(session, WiXInstallType.MaintenanceServer);
            Log.WriteEnd("MaintenanceServer");
            return Result;
        }
        [CustomAction]
        public static ActionResult MaintenanceEServer(Session session)
        {
            PopUpDebugger();
            var Result = ActionResult.Success;
            Log.WriteStart("MaintenanceEServer");
            Result = ProcessInstall(session, WiXInstallType.MaintenanceEnterpriseServer);
            Log.WriteEnd("MaintenanceEServer");
            return Result;
        }
        [CustomAction]
        public static ActionResult MaintenancePortal(Session session)
        {
            PopUpDebugger();
            var Result = ActionResult.Success;
            Log.WriteStart("MaintenancePortal");
            Result = ProcessInstall(session, WiXInstallType.MaintenancePortal);
            Log.WriteEnd("MaintenancePortal");
            return Result;
        }
        [CustomAction]
        public static ActionResult PreFillSettings(Session session)
        {
            Func<string, bool> HaveInstalledComponents = (string CfgFullPath) =>
            {
                var ComponentsPath = "//components";
                return File.Exists(CfgFullPath) ? BackupRestore.HaveChild(CfgFullPath, ComponentsPath) : false;
            };
            Func<IEnumerable<string>, string> FindMainConfig = (IEnumerable<string> Dirs) =>
            {
                // Looking into directories with next priority: 
                // Previous installation directory and her backup, "SolidCP" directories on fixed drives and their backups.
                // The last chance is an update from an installation based on previous installer version that installed to "Program Files".
                // Regular directories.
                foreach (var Dir in Dirs)
                {
                    var Result = Path.Combine(Dir, BackupRestore.MainConfig);
                    if (HaveInstalledComponents(Result))
                    {
                        return Result;
                    }
                    else
                    {
                        var ComponentNames = new string[] { Global.Server.ComponentName, Global.EntServer.ComponentName, Global.WebPortal.ComponentName };
                        foreach (var Name in ComponentNames)
                        {
                            var Backup = BackupRestore.Find(Dir, Global.DefaultProductName, Name);
                            if (Backup != null && HaveInstalledComponents(Backup.BackupMainConfigFile))
                                return Backup.BackupMainConfigFile;
                        }
                    }
                }
                // Looking into platform specific Program Files.
                {
                    var InstallerMainCfg = "SolidCP.Installer.exe.config";
                    var InstallerName = "SolidCP Installer";
                    var PFolderType = Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles;
                    var PFiles = Environment.GetFolderPath(PFolderType);
                    var Result = Path.Combine(PFiles, InstallerName, InstallerMainCfg);
                    if (HaveInstalledComponents(Result))
                        return Result;
                }
                return null;
            };
            Action<Session, SetupVariables> VersionGuard = (Session SesCtx, SetupVariables CtxVars) =>
            {
                var Current = SesCtx["ProductVersion"];
                var Found = string.IsNullOrWhiteSpace(CtxVars.Version) ? "0.0.0" : CtxVars.Version;
                if ((new Version(Found) > new Version(Current)) && !CtxVars.InstallerType.ToLowerInvariant().Equals("msi"))
                    throw new InvalidOperationException("New version must be greater than previous always.");
            };
            Func<string, string> NormalizeDir = (string Dir) =>
            {
                string nds = Path.DirectorySeparatorChar.ToString();
                string ads = Path.AltDirectorySeparatorChar.ToString();
                var Result = Dir.Trim();
                if (!Result.EndsWith(nds) && !Result.EndsWith(ads))
                {
                    if (Result.Contains(nds))
                        Result += nds;
                    else
                        Result += ads;
                }
                return Result;
            };
            try
            {
                var Ctx = session;
                Ctx.AttachToSetupLog();

                PopUpDebugger();

                Log.WriteStart("PreFillSettings");

                TryApllyNewPassword(Ctx, "PI_SERVER_PASSWORD");
                TryApllyNewPassword(Ctx, "PI_ESERVER_PASSWORD");
                TryApllyNewPassword(Ctx, "PI_PORTAL_PASSWORD");

                var SCP = Ctx["SCP_INSTALL_DIR"];
                var DirList = new List<string>();
                DirList.Add(SCP);
                DirList.AddRange(from Drive in DriveInfo.GetDrives()
                                 where Drive.DriveType == DriveType.Fixed
                                 select Path.Combine(Drive.RootDirectory.FullName, Global.DefaultProductName));
                var CfgPath = FindMainConfig(DirList);
                if (!string.IsNullOrWhiteSpace(CfgPath))
                {
                    try
                    {
                        var EServerUrl = string.Empty;
                        AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = CfgPath });
                        var CtxVars = new SetupVariables();
                        CtxVars.ComponentId = WiXSetup.GetComponentID(CfgPath, Global.Server.ComponentCode);
                        if (!string.IsNullOrWhiteSpace(CtxVars.ComponentId))
                        {
                            AppConfig.LoadComponentSettings(CtxVars);
                            VersionGuard(Ctx, CtxVars);

                            SetProperty(Ctx, "COMPFOUND_SERVER_ID", CtxVars.ComponentId);
                            SetProperty(Ctx, "COMPFOUND_SERVER_MAIN_CFG", CfgPath);

                            SetProperty(Ctx, "PI_SERVER_IP", CtxVars.WebSiteIP);
                            SetProperty(Ctx, "PI_SERVER_PORT", CtxVars.WebSitePort);
                            SetProperty(Ctx, "PI_SERVER_HOST", CtxVars.WebSiteDomain);
                            SetProperty(Ctx, "PI_SERVER_LOGIN", CtxVars.UserAccount);
                            SetProperty(Ctx, "PI_SERVER_DOMAIN", CtxVars.UserDomain);

                            SetProperty(Ctx, "PI_SERVER_INSTALL_DIR", CtxVars.InstallFolder);
                            SetProperty(Ctx, "SCP_INSTALL_DIR", NormalizeDir(new DirectoryInfo(CtxVars.InstallFolder).Parent.FullName));

                            Ctx["SERVER_ACCESS_PASSWORD"] = string.Empty;
                            Ctx["SERVER_ACCESS_PASSWORD_CONFIRM"] = string.Empty;

                            var HaveAccount = SecurityUtils.UserExists(CtxVars.UserDomain, CtxVars.UserAccount);
                            bool HavePool = Tool.AppPoolExists(CtxVars.ApplicationPool);

                            Ctx["COMPFOUND_SERVER"] = (HaveAccount && HavePool) ? YesNo.Yes : YesNo.No;
                        }
                        CtxVars.ComponentId = WiXSetup.GetComponentID(CfgPath, Global.EntServer.ComponentCode);
                        if (!string.IsNullOrWhiteSpace(CtxVars.ComponentId))
                        {
                            AppConfig.LoadComponentSettings(CtxVars);
                            VersionGuard(Ctx, CtxVars);

                            SetProperty(Ctx, "COMPFOUND_ESERVER_ID", CtxVars.ComponentId);
                            SetProperty(Ctx, "COMPFOUND_ESERVER_MAIN_CFG", CfgPath);

                            SetProperty(Ctx, "PI_ESERVER_IP", CtxVars.WebSiteIP);
                            SetProperty(Ctx, "PI_ESERVER_PORT", CtxVars.WebSitePort);
                            SetProperty(Ctx, "PI_ESERVER_HOST", CtxVars.WebSiteDomain);
                            SetProperty(Ctx, "PI_ESERVER_LOGIN", CtxVars.UserAccount);
                            SetProperty(Ctx, "PI_ESERVER_DOMAIN", CtxVars.UserDomain);
                            EServerUrl = string.Format("http://{0}:{1}", CtxVars.WebSiteIP, CtxVars.WebSitePort);

                            SetProperty(Ctx, "PI_ESERVER_INSTALL_DIR", CtxVars.InstallFolder);
                            SetProperty(Ctx, "SCP_INSTALL_DIR", NormalizeDir(new DirectoryInfo(CtxVars.InstallFolder).Parent.FullName));

                            var ConnStr = new SqlConnectionStringBuilder(CtxVars.DbInstallConnectionString);
                            SetProperty(Ctx, "DB_CONN", ConnStr.ToString());
                            SetProperty(Ctx, "DB_SERVER", ConnStr.DataSource);
                            SetProperty(Ctx, "DB_AUTH", ConnStr.IntegratedSecurity ? SQL_AUTH_WINDOWS : SQL_AUTH_SERVER);
                            if (!ConnStr.IntegratedSecurity)
                            {
                                SetProperty(Ctx, "DB_LOGIN", ConnStr.UserID);
                                SetProperty(Ctx, "DB_PASSWORD", ConnStr.Password);
                            }
                            ConnStr = new SqlConnectionStringBuilder(CtxVars.ConnectionString);
                            SetProperty(Ctx, "DB_DATABASE", ConnStr.InitialCatalog);
                            SetProperty(Ctx, "PI_ESERVER_CONN_STR", ConnStr.ToString());
                            SetProperty(Ctx, "PI_ESERVER_CRYPTO_KEY", CtxVars.CryptoKey);

                            try
                            {
                                var SqlQuery = string.Format("USE [{0}]; SELECT [dbo].[Users].[Password] FROM [dbo].[Users] WHERE [dbo].[Users].[UserID] = 1;", ConnStr.InitialCatalog);
                                using (var Reader = SqlUtils.ExecuteSql(CtxVars.DbInstallConnectionString, SqlQuery).CreateDataReader())
                                {
                                    if (Reader.Read())
                                    {
                                        var Hash = Reader[0].ToString();
                                        var Password = IsEnctyptionEnabled(string.Format(@"{0}\Web.config", CtxVars.InstallationFolder)) ? Utils.Decrypt(CtxVars.CryptoKey, Hash) : Hash;
                                        Ctx["SERVERADMIN_PASSWORD"] = Password;
                                        Ctx["SERVERADMIN_PASSWORD_CONFIRM"] = Password;
                                    }
                                }
                            }
                            catch
                            {
                                // Nothing to do.
                            }

                            var HaveAccount = SecurityUtils.UserExists(CtxVars.UserDomain, CtxVars.UserAccount);
                            bool HavePool = Tool.AppPoolExists(CtxVars.ApplicationPool);

                            Ctx["COMPFOUND_ESERVER"] = (HaveAccount && HavePool) ? YesNo.Yes : YesNo.No;
                        }
                        CtxVars.ComponentId = WiXSetup.GetComponentID(CfgPath, Global.WebPortal.ComponentCode);
                        if (!string.IsNullOrWhiteSpace(CtxVars.ComponentId))
                        {
                            AppConfig.LoadComponentSettings(CtxVars);
                            VersionGuard(Ctx, CtxVars);

                            SetProperty(Ctx, "COMPFOUND_PORTAL_ID", CtxVars.ComponentId);
                            SetProperty(Ctx, "COMPFOUND_PORTAL_MAIN_CFG", CfgPath);

                            SetProperty(Ctx, "PI_PORTAL_IP", CtxVars.WebSiteIP);
                            SetProperty(Ctx, "PI_PORTAL_PORT", CtxVars.WebSitePort);
                            SetProperty(Ctx, "PI_PORTAL_HOST", CtxVars.WebSiteDomain);
                            SetProperty(Ctx, "PI_PORTAL_LOGIN", CtxVars.UserAccount);
                            SetProperty(Ctx, "PI_PORTAL_DOMAIN", CtxVars.UserDomain);
                            if (!SetProperty(Ctx, "PI_ESERVER_URL", CtxVars.EnterpriseServerURL))
                                if (!SetProperty(Ctx, "PI_ESERVER_URL", EServerUrl))
                                    SetProperty(Ctx, "PI_ESERVER_URL", Global.WebPortal.DefaultEntServURL);

                            SetProperty(Ctx, "PI_PORTAL_INSTALL_DIR", CtxVars.InstallFolder);
                            SetProperty(Ctx, "SCP_INSTALL_DIR", NormalizeDir(new DirectoryInfo(CtxVars.InstallFolder).Parent.FullName));

                            var HaveAccount = SecurityUtils.UserExists(CtxVars.UserDomain, CtxVars.UserAccount);
                            bool HavePool = Tool.AppPoolExists(CtxVars.ApplicationPool);

                            Ctx["COMPFOUND_PORTAL"] = (HaveAccount && HavePool) ? YesNo.Yes : YesNo.No;
                        }
                        CtxVars.ComponentId = WiXSetup.GetComponentID(CfgPath, Global.Scheduler.ComponentCode);
                        var HaveComponentId = !string.IsNullOrWhiteSpace(CtxVars.ComponentId);
                        var HaveSchedulerService = ServiceController.GetServices().Any(x => x.DisplayName.Equals(Global.Parameters.SchedulerServiceName, StringComparison.CurrentCultureIgnoreCase));
                        var EServerConnStr = Ctx["PI_ESERVER_CONN_STR"];
                        var HaveConn = !string.IsNullOrWhiteSpace(EServerConnStr);
                        if (HaveComponentId || HaveSchedulerService || HaveConn)
                        {
                            Ctx["COMPFOUND_SCHEDULER"] = YesNo.No;
                            try
                            {
                                SqlConnectionStringBuilder ConnInfo = null;
                                if (HaveComponentId)
                                {
                                    AppConfig.LoadComponentSettings(CtxVars);
                                    VersionGuard(Ctx, CtxVars);
                                    SetProperty(Ctx, "COMPFOUND_SCHEDULER_ID", CtxVars.ComponentId);
                                    SetProperty(Ctx, "COMPFOUND_SCHEDULER_MAIN_CFG", CfgPath);
                                    SetProperty(Ctx, "PI_SCHEDULER_CRYPTO_KEY", CtxVars.CryptoKey);
                                    ConnInfo = new SqlConnectionStringBuilder(CtxVars.ConnectionString);                                    
                                }
                                else if (HaveConn)
                                {                                    
                                    SetProperty(Ctx, "PI_SCHEDULER_CRYPTO_KEY", Ctx["PI_ESERVER_CRYPTO_KEY"]);
                                    ConnInfo = new SqlConnectionStringBuilder(EServerConnStr);
                                }
                                if(ConnInfo != null)
                                {
                                    SetProperty(Ctx, "PI_SCHEDULER_DB_SERVER", ConnInfo.DataSource);
                                    SetProperty(Ctx, "PI_SCHEDULER_DB_DATABASE", ConnInfo.InitialCatalog);
                                    SetProperty(Ctx, "PI_SCHEDULER_DB_LOGIN", ConnInfo.UserID);
                                    SetProperty(Ctx, "PI_SCHEDULER_DB_PASSWORD", ConnInfo.Password);
                                }
                                Ctx["COMPFOUND_SCHEDULER"] = HaveSchedulerService? YesNo.Yes : YesNo.No;
                            }
                            catch (Exception ex)
                            {
                                Log.WriteError("Detection of existing Scheduler Service failed.", ex);
                            }                                
                        }
                    }
                    catch (InvalidOperationException ioex)
                    {
                        Log.WriteError(ioex.ToString());
                        var Text = new Record(1);
                        Text.SetString(0, ioex.Message);
                        Ctx.Message(InstallMessage.Error, Text);
                        return ActionResult.Failure;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.ToString());
                return ActionResult.Failure;
            }
            Log.WriteEnd("PreFillSettings");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult InstallWebFeatures(Session session)
        {
            PopUpDebugger();

            var Ctx = session;
            Ctx.AttachToSetupLog();
            Log.WriteStart("InstallWebFeatures");
            var Result = ActionResult.Success;
            var Scheduled = Ctx.GetMode(InstallRunMode.Scheduled);
            var Components = new List<string>();
            var InstallIis = false;
            var InstallAspNet = false;
            var InstallNetFx3 = false;
            if (Scheduled)
            {
                InstallIis = Ctx.CustomActionData["PI_PREREQ_IIS_INSTALL"] != YesNo.No;
                InstallAspNet = Ctx.CustomActionData["PI_PREREQ_ASPNET_INSTALL"] != YesNo.No;
                InstallNetFx3 = Ctx.CustomActionData["PI_PREREQ_NETFX_INSTALL"] != YesNo.No;
            }
            else
            {
                InstallIis = Ctx["PI_PREREQ_IIS_INSTALL"] != YesNo.No;
                InstallAspNet = Ctx["PI_PREREQ_ASPNET_INSTALL"] != YesNo.No;
                InstallNetFx3 = Ctx["PI_PREREQ_NETFX_INSTALL"] != YesNo.No;
            }
            if (InstallIis)
                Components.AddRange(Tool.GetWebRoleComponents());
            if (InstallAspNet)
                Components.AddRange(Tool.GetWebDevComponents());
            if (InstallNetFx3)
                Components.AddRange(Tool.GetNetFxComponents());
            if (Scheduled)
            {
                Action<int, int> ProgressReset = (int Total, int Mode) =>
                {
                    using (var Tmp = new Record(4))
                    {
                        Tmp.SetInteger(1, 0);
                        Tmp.SetInteger(2, Total);
                        Tmp.SetInteger(3, 0);
                        Tmp.SetInteger(4, Mode);
                        Ctx.Message(InstallMessage.Progress, Tmp);
                    }
                };
                Action<int> ProgressIncrement = (int Value) =>
                {
                    using (var Tmp = new Record(2))
                    {
                        Tmp.SetInteger(1, 2);
                        Tmp.SetInteger(2, Value);
                        Ctx.Message(InstallMessage.Progress, Tmp);
                    }
                };
                Action<string> ProgressText = (string Text) =>
                {
                    using (var Tmp = new Record(2))
                    {
                        Tmp.SetString(1, "CA_InstallWebFeaturesDeferred");
                        Tmp.SetString(2, Text);
                        Ctx.Message(InstallMessage.ActionStart, Tmp);
                    }
                };
                var Frmt = "Installing web component the {0} a {1} of {2}";
                Log.WriteStart("InstallWebFeatures: components");
                ProgressReset(Components.Count + 1, 0);
                ProgressText("Installing necessary web components ...");
                var Msg = new StringBuilder();
                try
                {
                    InstallToolDelegate InstallTool = Tool.GetInstallTool();
                    if (InstallTool == null)
                        throw new ApplicationException("Install tool for copmonents is not found.");                    
                    for (int i = 0; i < Components.Count; i ++)
                    {
                        var Component = Components[i];
                        ProgressText(string.Format(Frmt, Component, i + 1, Components.Count));
                        ProgressIncrement(1);
                        Msg.AppendLine(InstallTool(Component));
                    }
                    if (InstallAspNet)
                        Tool.PrepareAspNet();                    
                    Log.WriteInfo("InstallWebFeatures: done.");
                }
                catch (Exception ex)
                {
                    Log.WriteError(string.Format("InstallWebFeatures: fail - {0}.", ex.ToString()));
                    Result = ActionResult.Failure;
                }
                finally
                {
                    ProgressReset(Components.Count * 3 + 1, 0);
                    if (Msg.Length > 0)
                        Log.WriteInfo(string.Format("InstallWebFeatures Tool Log: {0}.", Msg.ToString()));
                    Log.WriteEnd("InstallWebFeatures: components"); 
                }                               
            }
            else
            {
                Log.WriteStart("InstallWebFeatures: prepare");
                using (var ProgressRecord = new Record(2))
                {     
                    ProgressRecord.SetInteger(1, 3);
                    ProgressRecord.SetInteger(2, Components.Count);
                    Ctx.Message(InstallMessage.Progress, ProgressRecord);
                }
                Log.WriteEnd("InstallWebFeatures: prepare");
            }
            Log.WriteEnd("InstallWebFeatures");
            return Result;
        }        
        // Install.
        [CustomAction]
        public static ActionResult OnServerInstall(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.InstallServer);
        }
        [CustomAction]
        public static ActionResult OnEServerInstall(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.InstallEnterpriseServer);
        }
        [CustomAction]
        public static ActionResult OnPortalInstall(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.InstallPortal);
        }
        [CustomAction]
        public static ActionResult OnSchedulerInstall(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.InstallScheduler);
        }
        // Remove.
        [CustomAction]
        public static ActionResult OnServerRemove(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.RemoveServer);
        }
        [CustomAction]
        public static ActionResult OnEServerRemove(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.RemoveEnterpriseServer);
        }
        [CustomAction]
        public static ActionResult OnPortalRemove(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.RemovePortal);
        }
        [CustomAction]
        public static ActionResult OnSchedulerRemove(Session session)
        {
            PopUpDebugger();
            return ProcessInstall(session, WiXInstallType.RemoveScheduler);
        }
        // Other.
        [CustomAction]
        public static ActionResult SetEServerUrlUI(Session session)
        {
            var Ctx = session;
            Ctx["PI_ESERVER_URL"] = string.Format("http://{0}:{1}/", Ctx["PI_ESERVER_IP"], Ctx["PI_ESERVER_PORT"]);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult RecapListUI(Session session)
        {
            const string F_SCP = "SolidCP";
            const string F_Server = "ServerFeature";
            const string F_EServer = "EnterpriseServerFeature";
            const string F_Portal = "PortalFeature";
            const string F_Scheduler = "SchedulerServiceFeature";
            const string F_WDPosrtal = "WebDavPortalFeature";
            var S_Install = new List<string> { "Copy SolidCP Server files", "Add SolidCP Server website" };
            var ES_Install = new List<string> { "Copy SolidCP Enterprise Server files", "Install SolidCP database and updates", "Add SolidCP Enterprise Server website" };
            var P_Install = new List<string> { "Copy SolidCP Portal files", "Add SolidCP Enterprise Portal website" };
            var SCH_Install = new List<string> { "Copy SolidCP Scheduler Service files", "Install Scheduler Service Windows Service" };
            var WDP_Install = new List<string> { "Copy SolidCP WebDav Portal files" };
            var S_Uninstall = new List<string> { "Delete SolidCP Server files", "Remove SolidCP Server website" };
            var ES_Uninstall = new List<string> { "Delete SolidCP Enterprise Server files", "Keep SolidCP database and updates", "Remove SolidCP Enterprise Server website" };
            var P_Uninstall = new List<string> { "Delete SolidCP Portal files", "Remove SolidCP Enterprise Portal website" };
            var SCH_Uninstall = new List<string> { "Delete SolidCP Scheduler Service files", "Remove Scheduler Service Windows Service" };
            var WDP_Uninstall = new List<string> { "Delete SolidCP WebDav Portal files" };
            var RecapList = new List<string>();
            var EmptyList = new List<string>();
            var Ctx = session;
            RecapListReset(Ctx);
            foreach (var Feature in Ctx.Features)
            {
                switch (Feature.Name)
                {
                    case F_SCP:
                        break;
                    case F_Server:
                        RecapList.AddRange(Feature.RequestState == InstallState.Local ? S_Install : /*S_Uninstall*/ EmptyList);
                        break;
                    case F_EServer:
                        RecapList.AddRange(Feature.RequestState == InstallState.Local ? ES_Install : /*ES_Uninstall*/ EmptyList);
                        break;
                    case F_Portal:
                        RecapList.AddRange(Feature.RequestState == InstallState.Local ? P_Install : /*P_Uninstall*/ EmptyList);
                        break;
                    case F_Scheduler:
                        RecapList.AddRange(Feature.RequestState == InstallState.Local ? SCH_Install : /*SCH_Uninstall*/ EmptyList);
                        break;
                    case F_WDPosrtal:
                        RecapList.AddRange(Feature.RequestState == InstallState.Local ? WDP_Install : /*WDP_Uninstall*/ EmptyList);
                        break;
                    default:
                        break;
                }
            }
            RecapListAdd(Ctx, RecapList.ToArray());
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult DatabaseConnectionValidateUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidateDbNameUI(Ctx, out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult SchedulerValidateUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidateCryptoKeyUI(Ctx, out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult ServerAdminValidateUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidatePasswordUI(Ctx, "SERVERADMIN", out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult ServerValidateADUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidateADUI(Ctx, "PI_SERVER", out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult EServerValidateADUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidateADUI(Ctx, "PI_ESERVER", out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult PortalValidateADUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidateADUI(Ctx, "PI_PORTAL", out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult ServerAccessValidateUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidatePasswordUI(Ctx, "SERVER_ACCESS", out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult ServerAccessValidateMtnUI(Session session)
        {
            var Ctx = session;
            bool Valid = true;
            string Msg;
            ValidationReset(Ctx);
            Valid = ValidateEqualPasswordUI(Ctx, "SERVER_ACCESS", out Msg);
            ValidationMsg(Ctx, Msg);
            ValidationStatus(Ctx, Valid);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult FillDomainListUI(Session Ctx)
        {
            try
            {
                var Ctrls = new[]{ new ComboBoxCtrl(Ctx, "PI_SERVER_DOMAIN"),
                                   new ComboBoxCtrl(Ctx, "PI_ESERVER_DOMAIN"),
                                   new ComboBoxCtrl(Ctx, "PI_PORTAL_DOMAIN") };
                using (var f = Forest.GetCurrentForest())
                {
                    foreach (Domain d in f.Domains)
                        foreach (var Ctrl in Ctrls)
                            Ctrl.AddItem(d.Name);
                }
            }
            catch
            {
                // Nothing to do.
            }
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult SqlServerListUI(Session session)
        {
            var Ctx = session;
            var SrvList = new ComboBoxCtrl(Ctx, "DB_SERVER");
            foreach (var Srv in GetSqlInstances())
                SrvList.AddItem(Srv);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult DbListUI(Session session)
        {
            string tmp;
            var Ctrl = new ComboBoxCtrl(session, "DB_SELECT");
            if (Adapter.CheckConnectionInfo(session["DB_CONN"], out tmp))
                foreach (var Db in GetDbList(ConnStr: session["DB_CONN"], ForbiddenNames: SysDb))
                {
                    Ctrl.AddItem(Db);
                    session["DB_SELECT"] = Db; // Adds available DBs to installer log and selects latest.
                }
            else
                session["DB_SELECT"] = "";
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult CheckConnectionSchedulerUI(Session Ctx)
        {
            PopUpDebugger();
            Ctx.AttachToSetupLog();
            Log.WriteStart("CheckConnectionSchedulerUI");
            var Result = default(bool);
            var Msg = default(string);
            var DbServer = Ctx["PI_SCHEDULER_DB_SERVER"];
            var DbUser = Ctx["PI_SCHEDULER_DB_LOGIN"];
            var DbPass = Ctx["PI_SCHEDULER_DB_PASSWORD"];
            var DbBase = Ctx["PI_SCHEDULER_DB_DATABASE"];
            if (new string[] { DbServer, DbUser, DbPass, DbBase }.Any(x => string.IsNullOrWhiteSpace(x)))
                Msg = "Please check all fields again.";
            else
                Result = Adapter.CheckSql(new SetupVariables { InstallConnectionString = GetConnectionString(DbServer, DbBase, DbUser, DbPass) }, out Msg) == CheckStatuses.Success;
            Ctx["DB_CONN_CORRECT"] = Result ? YesNo.Yes : YesNo.No;
            Ctx["DB_CONN_MSG"] = string.Format(Result ? "Success. {0}" : "Error. {0}", Msg);
            Log.WriteEnd("CheckConnectionSchedulerUI");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult CheckConnectionUI(Session session)
        {
            string Msg = default(string);
            string ConnStr = session["DB_AUTH"].Equals(SQL_AUTH_WINDOWS) ? GetConnectionString(session["DB_SERVER"], "master") :
                                                                           GetConnectionString(session["DB_SERVER"], "master", session["DB_LOGIN"], session["DB_PASSWORD"]);
            var Result = Adapter.CheckSql(new SetupVariables { InstallConnectionString = ConnStr }, out Msg) == CheckStatuses.Success;
            session["DB_CONN_CORRECT"] = Result ? YesNo.Yes : YesNo.No;
            session["DB_CONN"] = Result ? ConnStr : "";
            session["DB_CONN_MSG"] = string.Format(Result? "Success. {0}" : "Error. {0}", Msg);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult GetDbSkip(Session Ctx)
        {
            PopUpDebugger();
            Ctx.AttachToSetupLog();
            Log.WriteStart("GetDbSkip");
            var ConnStr = Ctx["DB_CONN"];
            var DbName = Ctx["DB_DATABASE"];
            var Tmp = Adapter.SkipCreateDb(ConnStr, DbName) ? YesNo.Yes : YesNo.No;
            Log.WriteInfo("DB_SKIP_CREATE is " + Tmp);
            Ctx["DB_SKIP_CREATE"] = Tmp;
            Log.WriteEnd("GetDbSkip");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult PrereqCheck(Session session)
        {
            var SecMsg = "You do not have the appropriate permissions to perform this operation. Make sure you are running the application from the local disk and you have local system administrator privileges.";
            var NetMsg = "Microsoft .NET {0} is {1}.";
            Action<string> ShowMsg = (string Text) =>
            {
                using(var Rec = new Record(0))
                {
                    Rec.SetString(0, Text);
                    session.Message(InstallMessage.Error, Rec);
                }
            };
            Action<Session, string, string> FxLog = (Session SesCtx, string Ver, string StrVer) =>
            {
                if (YesNo.Get(session[Ver]) == YesNo.Yes)
                    AddLog(SesCtx, string.Format(NetMsg, StrVer, "available"));
                else
                    AddLog(SesCtx, string.Format(NetMsg, StrVer, "not available"));
            };
            if (!Adapter.CheckSecurity() || !Adapter.IsAdministrator())
            {
                ShowMsg(SecMsg);
                return ActionResult.Failure;
            }            
            string Msg;
            var Ctx = Tool.GetSetupVars(session);
            var ros = Adapter.CheckOS(Ctx, out Msg);
            AddLog(session, Msg);
            var riis = Adapter.CheckIIS(Ctx, out Msg);
            AddLog(session, Msg);
            var raspnet = Adapter.CheckASPNET(Ctx, out Msg);
            AddLog(session, Msg);
            session[Prop.REQ_OS] = ros == CheckStatuses.Success ? YesNo.Yes : YesNo.No;
            session[Prop.REQ_IIS] = riis == CheckStatuses.Success ? YesNo.Yes : YesNo.No; ;
            session[Prop.REQ_ASPNET] = raspnet == CheckStatuses.Success ? YesNo.Yes : YesNo.No; ;
            FxLog(session, Prop.REQ_NETFRAMEWORK20, "2.0");
            FxLog(session, Prop.REQ_NETFRAMEWORK35, "3.5");
            FxLog(session, Prop.REQ_NETFRAMEWORK40FULL, "4.0");
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult PrereqCheckUI(Session session)
        {
            var ListView = new ListViewCtrl(session, "REQCHECKLIST");
            AddCheck(ListView, session, Prop.REQ_NETFRAMEWORK20);
            AddCheck(ListView, session, Prop.REQ_NETFRAMEWORK35);
            AddCheck(ListView, session, Prop.REQ_NETFRAMEWORK40FULL);
            AddCheck(ListView, session, Prop.REQ_OS);
            AddCheck(ListView, session, Prop.REQ_IIS);
            AddCheck(ListView, session, Prop.REQ_ASPNET);
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult FillIpListUI(Session session)
        {
            PopUpDebugger();
            var Ctrls = new[]{ new ComboBoxCtrl(session, "PI_SERVER_IP"),
                                new ComboBoxCtrl(session, "PI_ESERVER_IP"),
                                new ComboBoxCtrl(session, "PI_PORTAL_IP") };
            foreach (var Ip in GetIpList())
                foreach (var Ctrl in Ctrls)
                    Ctrl.AddItem(Ip);
            return ActionResult.Success;
        }
        #endregion
        private static string GetConnectionString(string serverName, string databaseName, string login = null, string password = null)
        {
            return SqlUtils.BuildDbServerConnectionString(serverName, databaseName, login, password);
        }
        private static void AddCheck(ListViewCtrl view, Session session, string PropertyID)
        {
            view.AddItem(YesNo.Get(session[PropertyID]) != YesNo.No, session[PropertyID + "_TITLE"]);
        }
        static IList<string> GetSqlInstances()
        {
            var Result = new List<string>();
            using (var Src = SqlDataSourceEnumerator.Instance.GetDataSources())
            {
                foreach (DataRow Row in Src.Rows)
                {
                    var Instance = Row["InstanceName"].ToString();
                    Result.Add((string.IsNullOrWhiteSpace(Instance) ? "" : (Instance + "\\")) + Row["ServerName"].ToString());
                }
            }
            return Result;
        }
        static IEnumerable<string> GetDbList(string ConnStr, IList<string> ForbiddenNames = null)
        {
            using (var Conn = new SqlConnection(ConnStr))
            {
                Conn.Open();
                var Cmd = Conn.CreateCommand();
                Cmd.CommandText = "SELECT name FROM master..sysdatabases";
                if (ForbiddenNames != null && ForbiddenNames.Count > 0)
                    Cmd.CommandText += string.Format(" WHERE name NOT IN ({0})", string.Join(", ", ForbiddenNames.Select(x => string.Format("'{0}'", x))));
                var Result = Cmd.ExecuteReader();
                while (Result.Read())
                    yield return Result["name"].ToString();
            }
        }
        static IEnumerable<string> GetIpList()
        {
            foreach (var Ni in NetworkInterface.GetAllNetworkInterfaces())
                if (Ni.OperationalStatus == OperationalStatus.Up && (Ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                                                     Ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                                                                     Ni.NetworkInterfaceType == NetworkInterfaceType.Loopback))
                    foreach (var IpInfo in Ni.GetIPProperties().UnicastAddresses)
                        if (IpInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            yield return IpInfo.Address.ToString();
        }
        internal static void AddLog(Session Ctx, string Msg)
        {
            AddTo(Ctx, "PI_PREREQ_LOG", Msg);
        }
        internal static void AddTo(Session Ctx, string TextProp, string Msg)
        {
            if (!string.IsNullOrWhiteSpace(Msg))
            {
                string tmp = Ctx[TextProp];
                if (string.IsNullOrWhiteSpace(tmp))
                    Ctx[TextProp] = Msg;
                else
                    Ctx[TextProp] = tmp + Environment.NewLine + Msg;
            }
        }
        internal static void ValidationReset(Session Ctx)
        {
            Ctx["VALIDATE_OK"] = "0";
            Ctx["VALIDATE_MSG"] = "Error occurred.";
        }
        internal static void ValidationStatus(Session Ctx, bool Value)
        {
            Ctx["VALIDATE_OK"] = Value ? YesNo.Yes : YesNo.No;
        }
        internal static void ValidationMsg(Session Ctx, string Msg)
        {
            AddTo(Ctx, "VALIDATE_MSG", Msg);
        }
        internal static bool PasswordValidate(string Password, string Confirm, out string Msg)
        {
            Msg = string.Empty;
            bool Result = false;
            if (string.IsNullOrWhiteSpace(Password))
                Msg = "Empty password.";
            else if (Password != Confirm)
                Msg = "Password does not match the confirm password. Type both passwords again.";
            else
                Result = true;
            return Result;
        }
        internal static bool PasswordValidateEqual(string Password, string Confirm, out string Msg)
        {
            Msg = string.Empty;
            bool Result = false;
            if (Password != Confirm)
                Msg = "Password does not match the confirm password. Type both passwords again.";
            else
                Result = true;
            return Result;
        }
        internal static bool ValidatePasswordUI(Session Ctx, string Ns, out string Msg)
        {
            string p1 = Ctx[Ns + "_PASSWORD"];
            string p2 = Ctx[Ns + "_PASSWORD_CONFIRM"];
            return PasswordValidate(p1, p2, out Msg);
        }
        internal static bool ValidateEqualPasswordUI(Session Ctx, string Ns, out string Msg)
        {
            string p1 = Ctx[Ns + "_PASSWORD"];
            string p2 = Ctx[Ns + "_PASSWORD_CONFIRM"];
            return PasswordValidateEqual(p1, p2, out Msg);
        }
        internal static bool ValidateADDomainUI(Session Ctx, string Ns, out string Msg)
        {
            bool Result = default(bool);
            bool check = Ctx[Ns + "_CREATE_AD"] == YesNo.Yes;
            string name = Ctx[Ns + "_DOMAIN"];
            if (check && string.IsNullOrWhiteSpace(name))
            {
                Result = false;
                Msg = "The domain can't be empty.";
            }
            else
            {
                Result = true;
                Msg = string.Empty;
            }
            return Result;
        }
        internal static bool ValidateADLoginUI(Session Ctx, string Ns, out string Msg)
        {
            bool Result = default(bool);
            string name = Ctx[Ns + "_LOGIN"];
            if (string.IsNullOrWhiteSpace(name))
            {
                Result = false;
                Msg = "The login can't be empty.";
            }
            else
            {
                Result = true;
                Msg = string.Empty;
            }
            return Result;
        }
        internal static bool ValidateADUI(Session Ctx, string Ns, out string Msg)
        {
            bool Result = true;
            if (!ValidateADDomainUI(Ctx, Ns, out Msg))
                Result = false;
            else if (!ValidateADLoginUI(Ctx, Ns, out Msg))
                Result = false;
            else if (!ValidatePasswordUI(Ctx, Ns, out Msg))
                Result = false;
            return Result;
        }
        internal static bool ValidateDbNameUI(Session Ctx, out string Msg)
        {
            Msg = string.Empty;
            var Result = true;
            string DbName = Ctx["DB_DATABASE"];
            if (string.IsNullOrWhiteSpace(DbName))
            {
                Result = false;
                Msg = "The database name can't be empty.";
            }
            return Result;
        }
        internal static bool ValidateCryptoKeyUI(Session Ctx, out string Msg)
        {
            Msg = string.Empty;
            var Result = true;
            string DbName = Ctx["PI_SCHEDULER_CRYPTO_KEY"];
            if (string.IsNullOrWhiteSpace(DbName))
            {
                Result = false;
                Msg = "The crypto key can't be empty.";
            }
            return Result;
        }
        internal static void RecapListReset(Session Ctx)
        {
            Ctx["CUSTOM_INSTALL_TEXT"] = string.Empty;
        }
        internal static void RecapListAdd(Session Ctx, params string[] Msgs)
        {
            foreach (var Msg in Msgs)
                AddTo(Ctx, "CUSTOM_INSTALL_TEXT", Msg); ;
        }
        private static ActionResult ProcessInstall(Session Ctx, WiXInstallType InstallType)
        {
            IWiXSetup Install = null;
            try
            {
                Ctx.AttachToSetupLog();
                switch (InstallType)
                {
                    case WiXInstallType.InstallServer:
                        Install = ServerSetup.Create(Ctx.CustomActionData, SetupActions.Install);
                        break;
                    case WiXInstallType.RemoveServer:
                        Install = ServerSetup.Create(Ctx.CustomActionData, SetupActions.Uninstall);
                        break;
                    case WiXInstallType.MaintenanceServer:
                        Install = ServerSetup.Create(Ctx.CustomActionData, SetupActions.Setup);
                        break;
                    case WiXInstallType.InstallEnterpriseServer:
                        Install = EServerSetup.Create(Ctx.CustomActionData, SetupActions.Install);
                        break;
                    case WiXInstallType.RemoveEnterpriseServer:
                        Install = EServerSetup.Create(Ctx.CustomActionData, SetupActions.Uninstall);
                        break;
                    case WiXInstallType.MaintenanceEnterpriseServer:
                        Install = EServerSetup.Create(Ctx.CustomActionData, SetupActions.Setup);
                        break;
                    case WiXInstallType.InstallPortal:
                        Install = PortalSetup.Create(Ctx.CustomActionData, SetupActions.Install);
                        break;
                    case WiXInstallType.RemovePortal:
                        Install = PortalSetup.Create(Ctx.CustomActionData, SetupActions.Uninstall);
                        break;
                    case WiXInstallType.MaintenancePortal:
                        Install = PortalSetup.Create(Ctx.CustomActionData, SetupActions.Setup);
                        break;
                    case WiXInstallType.InstallScheduler:
                        Install = SchedulerSetup.Create(Ctx.CustomActionData, SetupActions.Install);
                        break;
                    case WiXInstallType.RemoveScheduler:
                        Install = SchedulerSetup.Create(Ctx.CustomActionData, SetupActions.Uninstall);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                Install.Run();
            }
            catch (WiXSetupException we)
            {
                Ctx.Log("Expected exception: " + we.ToString());
                return ActionResult.Failure;
            }
            catch (Exception ex)
            {
                Ctx.Log(ex.ToString());
                return ActionResult.Failure;
            }
            return ActionResult.Success;
        }
        [Conditional("DEBUG")]
        private static void PopUpDebugger()
        {
            Debugger.Launch();
        }
        private static void TryApllyNewPassword(Session Ctx, string Id)
        {
            var Pass = Ctx[Id];
            if (string.IsNullOrWhiteSpace(Pass))
            {
                Pass = Guid.NewGuid().ToString();
                Ctx[Id] = Pass;
                Ctx[Id + "_CONFIRM"] = Pass;
                Log.WriteInfo("New password was applied to " + Id);
            }
        }
        private static string GetProperty(Session Ctx, string Property)
        {
            if (Ctx.CustomActionData.ContainsKey(Property))
                return Ctx.CustomActionData[Property];
            else
                return string.Empty;
        }
        private static bool SetProperty(Session CtxSession, string Prop, string Value)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                CtxSession[Prop] = Value;
                return true;
            }
            return false;
        }
        private static SetupScript GetPrepareScript(Session Ctx)
        {
            var CtxVars = new SetupVariables();
            WiXSetup.FillFromSession(Ctx.CustomActionData, CtxVars);
            AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = GetProperty(Ctx, "MainConfig") });
            CtxVars.IISVersion = Tool.GetWebServerVersion();
            CtxVars.ComponentId = GetProperty(Ctx, "ComponentId");
            CtxVars.Version = AppConfig.GetComponentSettingStringValue(CtxVars.ComponentId, Global.Parameters.Release);
            CtxVars.SpecialBaseDirectory = Directory.GetParent(GetProperty(Ctx, "MainConfig")).FullName;
            CtxVars.FileNameMap = new Dictionary<string, string>();
            CtxVars.FileNameMap.Add(new FileInfo(GetProperty(Ctx, "MainConfig")).Name, BackupRestore.MainConfig);
            SetupScript Result = new ExpressScript(CtxVars);
            Result.Actions.Add(new InstallAction(ActionTypes.StopApplicationPool) { SetupVariables = CtxVars });
            Result.Actions.Add(new InstallAction(ActionTypes.Backup) { SetupVariables = CtxVars });
            Result.Actions.Add(new InstallAction(ActionTypes.DeleteDirectory) { SetupVariables = CtxVars, Path = CtxVars.InstallFolder });
            return Result;
        }
        private static bool IsEnctyptionEnabled(string Cfg)
        {
            var doc = new XmlDocument();
            doc.Load(Cfg);
            string xPath = "configuration/appSettings/add[@key=\"SolidCP.EncryptionEnabled\"]";
            XmlElement encryptionNode = doc.SelectSingleNode(xPath) as XmlElement;
            bool encryptionEnabled = false;
            if (encryptionNode != null)
                bool.TryParse(encryptionNode.GetAttribute("value"), out encryptionEnabled);
            return encryptionEnabled;
        }
    }
    public static class SessionExtension
    {
        public static void AttachToSetupLog(this Session Ctx)
        {
            WiXSetup.InstallLogListener(new WiXLogListener(Ctx));
            WiXSetup.InstallLogListener(new InMemoryStringLogListener("WIX CA IN MEMORY"));
            WiXSetup.InstallLogListener(new WiXLogFileListener());
        }
    }
    public static class StringExtension
    {
        public static string Repeat(this string Src, ulong Count)
        {
            var Result = new StringBuilder();
            for (ulong i = 0; i < Count; i++)
                Result.Append(Src);
            return Result.ToString();
        }
    }
    internal enum WiXInstallType: byte
    {
        InstallServer,
        InstallEnterpriseServer,
        InstallPortal,
        InstallScheduler,
        RemoveServer,
        RemoveEnterpriseServer,
        RemovePortal,
        RemoveScheduler,
        MaintenanceServer,
        MaintenanceEnterpriseServer,
        MaintenancePortal
    }    
}
