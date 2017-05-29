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
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

using System.Security.Principal;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Threading;

using SolidCP.Installer.Core;
using SolidCP.Installer.Configuration;
using System.Xml;
using System.Data;

namespace SolidCP.Installer.Common
{
	public class UserDecisionEventArgs : EventArgs
	{
		public bool Accepted { get; set; }
		public string UserMessage { get; set; }
	}

	public static class Utils
	{
		public static void FixConfigurationSectionDefinition()
		{
			var appConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Concat(AppConfigManager.AppConfigFileNameWithoutExtension, ".config"));
			//
			try
			{
				var configXmlDoc = new XmlDocument();
				//
				configXmlDoc.Load(appConfigPath);
				//
				var sectionDef = configXmlDoc.SelectSingleNode("//configSections/section[@name='installer']");
				//
				if (sectionDef == null)
				{
					return;
				}
				//
				var typeDefString = sectionDef.Attributes["type"].Value;
				//
				if (typeDefString.EndsWith("SolidCP.Installer.Core") == false)
				{
					sectionDef.Attributes["type"].Value = "SolidCP.Installer.Configuration.InstallerSection, SolidCP.Installer.Core";
					//
					configXmlDoc.Save(appConfigPath);
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError("Failed to fix configuration section definition. Exception: {0}", ex);
			}
		}

		#region DB

		/// <summary>
		/// Converts db value to string
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>string</returns>
		public static string GetDbString(object val)
		{
			string ret = string.Empty;
			if ((val != null) && (val != DBNull.Value))
				ret = (string)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to short
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>short</returns>
		public static short GetDbShort(object val)
		{
			short ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (short)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to int
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>int</returns>
		public static int GetDbInt32(object val)
		{
			int ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (int)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to bool
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>bool</returns>
		public static bool GetDbBool(object val)
		{
			bool ret = false;
			if ((val != null) && (val != DBNull.Value))
				ret = Convert.ToBoolean(val);
			return ret;
		}

		/// <summary>
		/// Converts db value to decimal
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>decimal</returns>
		public static decimal GetDbDecimal(object val)
		{
			decimal ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (decimal)val;
			return ret;
		}


		/// <summary>
		/// Converts db value to datetime
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>DateTime</returns>
		public static DateTime GetDbDateTime(object val)
		{
			DateTime ret = DateTime.MinValue;
			if ((val != null) && (val != DBNull.Value))
				ret = (DateTime)val;
			return ret;
		}

		#endregion

		public static void ShowConsoleErrorMessage(string format, params object[] args)
		{
			Console.WriteLine(String.Format(format, args));
		}

		public static bool CheckForInstalledComponent(string componentCode)
		{
			bool ret = false;
			List<string> installedComponents = new List<string>();
			foreach (ComponentConfigElement componentConfig in AppConfigManager.AppConfiguration.Components)
			{
				string code = componentConfig.Settings[Global.Parameters.ComponentCode].Value;
				installedComponents.Add(code);
				if (code == componentCode)
				{
					ret = true;
					break;
				}
			}
			//
			if (componentCode == "standalone")
			{
				if (installedComponents.Contains("server") ||
                    installedComponents.Contains("WebDavPortal") ||
                    installedComponents.Contains("enterprise server") ||
					installedComponents.Contains("portal") ||
                    installedComponents.Contains("WebDavPortal"))
					ret = true;
			}
			//
			return ret;
		}

		public static bool IsThreadAbortException(Exception ex)
		{
			Exception innerException = ex;
			while (innerException != null)
			{
				if (innerException is System.Threading.ThreadAbortException)
					return true;
				innerException = innerException.InnerException;
			}

			string str = ex.ToString();
			return str.Contains("System.Threading.ThreadAbortException");
		}

		public static bool IsSecurityException(Exception ex)
		{
			if (ex is System.Security.SecurityException)
				return true;

			Exception innerException = ex;
			while (innerException != null)
			{
				if (innerException is System.Security.SecurityException)
					return true;
				innerException = innerException.InnerException;
			}

			string str = ex.ToString();
			return str.Contains("System.Security.SecurityException");
		}

		public static bool IsWin64()
		{
			return (IntPtr.Size == 8);
		}

		public static void SetObjectProperty(DirectoryEntry oDE, string name, object value)
		{
			if (value != null)
			{
				if (oDE.Properties.Contains(name))
				{
					oDE.Properties[name][0] = value;
				}
				else
				{
					oDE.Properties[name].Add(value);
				}
			}
		}

		public static object GetObjectProperty(DirectoryEntry entry, string name)
		{
			if (entry.Properties.Contains(name))
				return entry.Properties[name][0];
			else
				return null;
		}

		public static bool IIS32Enabled()
		{
			bool enabled = false;
			DirectoryEntry obj = new DirectoryEntry("IIS://LocalHost/W3SVC/AppPools");
			object objProperty = GetObjectProperty(obj, "Enable32bitAppOnWin64");
			if (objProperty != null)
			{
				enabled = (bool)objProperty;
			}
			return enabled;
		}

		public static void CheckWin64(EventHandler<UserDecisionEventArgs> callback)
		{
			if (IsWin64())
			{
				Log.WriteInfo("x64 detected");
				string check = AppConfigManager.AppConfiguration.GetStringSetting(ConfigKeys.Settings_IIS64);
				if (check == "False")
					return;

				//ignore win64 check on IIS7
                if (Global.IISVersion.Major >= 7)
					return;

				if (!IIS32Enabled())
				{
					Log.WriteInfo("IIS 32-bit mode disabled");

					var args = new UserDecisionEventArgs
						{
							UserMessage = "SolidCP Installer has detected that Microsoft Internet Information Services (IIS) " +
						"are running in 64-bit mode. It is recommended to switch IIS to 32-bit mode to " +
						"enable compatibility with 32-bit applications. " +
						"Please note that 64-bit web applications will not work in 32-bit mode.\n" +
						"Do you want SolidCP Installer to switch IIS to 32-bit mode?"
						};

					if (callback != null)
					{
						callback(null, args);
					}

					if (AppConfigManager.AppConfiguration.Settings[ConfigKeys.Settings_IIS64] != null)
						AppConfigManager.AppConfiguration.Settings[ConfigKeys.Settings_IIS64].Value = "False";
					else
						AppConfigManager.AppConfiguration.Settings.Add(ConfigKeys.Settings_IIS64, "False");
					//
					AppConfigManager.SaveConfiguration(false);

					if (args.Accepted)
					{
						EnableIIS32();
					}
				}
				else
				{
					Log.WriteInfo("IIS 32-bit mode enabled");
				}
			}
			else
			{
				Log.WriteInfo("x86 detected");
			}
		}

		private static void EnableIIS32()
		{
			Log.WriteStart("Enabling IIS 32-bit mode");
			DirectoryEntry obj = new DirectoryEntry("IIS://LocalHost/W3SVC/AppPools");
			SetObjectProperty(obj, "Enable32bitAppOnWin64", true);
			obj.CommitChanges();
			Log.WriteEnd("Enabled IIS 32-bit mode");


			Log.WriteStart("Starting aspnet_regiis -i");
			string path = Path.Combine(OS.GetWindowsDirectory(), @"Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe");
			ProcessStartInfo info = new ProcessStartInfo(path, "-i");
			//info.WindowStyle = ProcessWindowStyle.Minimized;
			Process process = Process.Start(info);
			process.WaitForExit();
			Log.WriteEnd("Finished aspnet_regiis -i");

			Log.WriteStart("Enabling Web Service Extension");
			DirectoryEntry iis = new DirectoryEntry("IIS://LocalHost/W3SVC");
			iis.Invoke("EnableWebServiceExtension", "ASP.NET v2.0.50727 (32-bit)");
			iis.CommitChanges();
			Log.WriteEnd("Enabled Web Service Extension");
		}

		/// <summary>
		/// Check security permissions
		/// </summary>
		public static bool CheckSecurity()
		{
			try
			{
				PermissionSet set = new PermissionSet(PermissionState.Unrestricted);
				set.Demand();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool IsAdministrator()
		{
			WindowsIdentity user = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(user);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		private static Mutex mutex = null;

		public static void SaveMutex()
		{
			GC.KeepAlive(mutex);
		}

		public static bool IsNewInstance()
		{
			//check only one instance
			bool createdNew = true;
			mutex = new Mutex(true, "SolidCP Installer", out createdNew);
			return createdNew;
		}

        public static string GetDistributiveLocationInfo(string ccode, string cversion)
        {
            var service = ServiceProviderProxy.GetInstallerWebService();
            //
            DataSet ds = service.GetReleaseFileInfo(ccode, cversion);
            //
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Log.WriteInfo("Component code: {0}; Component version: {1};", ccode, cversion);
                //
                throw new ServiceComponentNotFoundException("Seems that the Service has no idea about the component requested.");
            }
            //
            DataRow row = ds.Tables[0].Rows[0];
            //
            return row["FullFilePath"].ToString();
        }
	}
}
