using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32;

using SolidCP.Setup;

namespace SolidCP.WIXInstaller.Common
{
    public delegate string InstallToolDelegate (params string[] Components);
    internal static class Tool
    {
        public const int MINIMUM_WEBSERVER_MAJOR_VERSION = 6;
        public static SetupVariables GetSetupVars(Session Ctx)
        {
            return new SetupVariables
                {
                    SetupAction = SetupActions.Install,
                    IISVersion = Global.IISVersion
                };
        }
        public static Version GetWebServerVersion()
        {
            var WebServerKey = "SOFTWARE\\Microsoft\\InetStp";
            RegistryKey Key = Registry.LocalMachine.OpenSubKey(WebServerKey);
            if (Key == null)
                return new Version(0,0);
            var Major = int.Parse(Key.GetValue("MajorVersion", 0).ToString());
            var Minor = int.Parse(Key.GetValue("MinorVersion", 0).ToString());
            return new Version(Major, Minor);
        }
        public static bool GetIsWebRoleInstalled()
        {
            var WebServer = GetWebServerVersion();
            return WebServer.Major >= Tool.MINIMUM_WEBSERVER_MAJOR_VERSION;
        }
        public static bool GetIsWebFeaturesInstalled()
        {
            bool Result = false;
            var LMKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            Result |= CheckAspNetRegValue(LMKey);
            LMKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            Result |= CheckAspNetRegValue(LMKey);
            return Result;
        }
        public static bool AppPoolExists(string Pool)
        {
            if (GetWebServerVersion().Major > MINIMUM_WEBSERVER_MAJOR_VERSION)
                return WebUtils.IIS7ApplicationPoolExists(Pool);
            else
                return WebUtils.ApplicationPoolExists(Pool);
        }
        public static bool CheckAspNetRegValue(RegistryKey BaseKey)
        {
            var WebComponentsKey = "SOFTWARE\\Microsoft\\InetStp\\Components";
            var AspNet = "ASPNET";
            var AspNet45 = "ASPNET45";
            RegistryKey Key = BaseKey.OpenSubKey(WebComponentsKey);
            if (Key == null)
                return false;
            var Value = int.Parse(Key.GetValue(AspNet, 0).ToString());
            if (Value != 1)
                Value = int.Parse(Key.GetValue(AspNet45, 0).ToString());
            return Value == 1;
        }
        public static string[] GetWebRoleComponents()
        {
            string[] Result = null;
            var OSV = Global.OSVersion;
            switch (OSV)
            {
                case OS.WindowsVersion.WindowsServer2008:
                        Result = new[] 
                        { 
                            "Web-Server",
                            "Web-Common-Http",
                            "Web-Default-Doc",
                            "Web-App-Dev",
                            "Web-ISAPI-Ext",
                            "Web-ISAPI-Filter",
                            "Web-Net-Ext",
                            "Web-Mgmt-Console",                            
                            "Web-Filtering",
                            "Web-Security",
                            "Web-Static-Content"
                        };
                    break;
                case OS.WindowsVersion.WindowsServer2008R2:
                case OS.WindowsVersion.WindowsServer2012:
                case OS.WindowsVersion.WindowsServer2012R2:
                case OS.WindowsVersion.WindowsServer2016:
                case OS.WindowsVersion.Windows7:
                case OS.WindowsVersion.Windows8:
                case OS.WindowsVersion.Windows10:
                    Result = new[]
                        { 
                            "IIS-WebServerRole",
                            "IIS-WebServer",                                            
                            "IIS-CommonHttpFeatures",
                            "IIS-DefaultDocument",
                            "IIS-ApplicationDevelopment",
                            "IIS-ISAPIExtensions",
                            "IIS-ISAPIFilter",
                            "IIS-NetFxExtensibility",
                            "IIS-ManagementConsole",                            
                            "IIS-RequestFiltering",
                            "IIS-Security",
                            "IIS-StaticContent"                
                        };
                    break;
            }
            return Result;
        }
        public static string[] GetWebDevComponents()
        {
            string[] Result = null;
            var OSV = Global.OSVersion;
            switch (OSV)
            {
                case OS.WindowsVersion.WindowsServer2008:
                        Result = new[]
                        { 
                            "Web-App-Dev",
                            "Web-ISAPI-Ext",
                            "Web-ISAPI-Filter",
                            "Web-Net-Ext",
                            "Web-Asp-Net"
                        };
                    break;
                case OS.WindowsVersion.Windows7:
                case OS.WindowsVersion.WindowsServer2008R2:
                    Result = new[]
                        {
                            "IIS-ApplicationDevelopment",
                            "IIS-ISAPIExtensions",
                            "IIS-ISAPIFilter",
                            "IIS-NetFxExtensibility",
                            "IIS-ASPNET"
                        };
                    break;
                case OS.WindowsVersion.WindowsServer2012:
                case OS.WindowsVersion.WindowsServer2012R2:
                case OS.WindowsVersion.WindowsServer2016:
                case OS.WindowsVersion.Windows8:
                case OS.WindowsVersion.Windows10:
                    Result = new[]
                        { 
                            "IIS-ApplicationDevelopment",
                            "IIS-ISAPIExtensions",
                            "IIS-ISAPIFilter",
                            "IIS-NetFxExtensibility",
                            "IIS-ASPNET",
                            "IIS-ASPNET45"
                        };
                    break;
            }
            return Result;
        }
        public static string [] GetNetFxComponents()
        {
            string[] Result = null;
            var OSV = Global.OSVersion;
            switch (OSV)
            {
                case OS.WindowsVersion.WindowsServer2008:
                        Result = new[] { "NET-Framework" };
                    break;
                case OS.WindowsVersion.WindowsServer2008R2:
                case OS.WindowsVersion.WindowsServer2012:
                case OS.WindowsVersion.WindowsServer2012R2:
                case OS.WindowsVersion.WindowsServer2016:
                case OS.WindowsVersion.Windows7:
                case OS.WindowsVersion.Windows8:
                case OS.WindowsVersion.Windows10:
                    Result = new[] { "NetFx3" };
                    break;
            }
            return Result;
        }
        public static string PrepareAspNet()
        {
            var Cmd = string.Format(@"Microsoft.NET\Framework{0}\v4.0.30319\aspnet_regiis.exe", Environment.Is64BitOperatingSystem ? "64" : "" );
            return RunTool(Path.Combine(OS.GetWindowsDirectory(), Cmd), "-i -enable");
        }
        public static InstallToolDelegate GetInstallTool()
        {
            InstallToolDelegate Result = null;
            var OSV = Global.OSVersion;
            switch (OSV)
            {
                case OS.WindowsVersion.WindowsServer2008:
                        Result = InstallWebViaServerManagerCmd;
                    break;
                case OS.WindowsVersion.WindowsServer2008R2:
                case OS.WindowsVersion.Windows7:
                        Result = InstallWebViaDism;
                    break;
                case OS.WindowsVersion.WindowsServer2012:
                case OS.WindowsVersion.WindowsServer2012R2:
                case OS.WindowsVersion.WindowsServer2016:
                case OS.WindowsVersion.Windows8:
                case OS.WindowsVersion.Windows10:
                    Result = InstallWebViaDismEx;
                    break;
            }
            return Result;
        }
        private static string InstallWebViaDism(params string[] Features)
        {
            var Params = string.Format("/NoRestart /Online /Enable-Feature {0}",
                                       string.Join(" ", Features.Select(
                                           Feature => string.Format("/FeatureName:{0}", Feature)
                                       )));
            return RunTool(Path.Combine(OS.GetWindowsDirectory(), @"SysNative\dism.exe"), Params);
        }
        private static string InstallWebViaDismEx(params string[] Features)
        {
            var Params = string.Format("/NoRestart /Online /Enable-Feature {0}",
                                       string.Join(" ", Features.Select(
                                           Feature => string.Format("/FeatureName:{0} /All", Feature)
                                       )));
            return RunTool(Path.Combine(OS.GetWindowsDirectory(), @"SysNative\dism.exe"), Params);
        }
        private static string InstallWebViaServerManagerCmd(params string[] Features)
        {
            var Params = string.Format("-install {0}",
                                       string.Join(" ", Features)
                                       );
            return RunTool(Path.Combine(OS.GetWindowsDirectory(), @"SysNative\servermanagercmd.exe"), Params);
        }
        private static string RunTool(string Name, string Params)
        {
            var ToolProcessInfo = new ProcessStartInfo
            {
                FileName = Name,
                Arguments = Params,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            using (var ToolProcess = Process.Start(ToolProcessInfo))
            {
                var Result = ToolProcess.StandardOutput.ReadToEnd();
                ToolProcess.WaitForExit();
                return Result;
            }
        }
    }
}
