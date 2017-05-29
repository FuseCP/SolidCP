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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.ServiceProcess;
using System.DirectoryServices;
using System.Linq;
using SolidCP.Setup.Web;
using Microsoft.Win32;

namespace SolidCP.Setup
{
	/// <summary>
	/// Utils class.
	/// </summary>
	public sealed class Utils
	{
		public const string AspNet40RegistrationToolx64 = @"Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe";
		public const string AspNet40RegistrationToolx86 = @"Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private Utils()
		{
		}

		#region Resources

		/// <summary>
		/// Get resource stream from assembly by specified resource name.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		/// <returns>Resource stream.</returns>
		public static Stream GetResourceStream(string resourceName)
		{
			Assembly asm = typeof(Utils).Assembly;
			Stream ret = asm.GetManifestResourceStream(resourceName);
			return ret;
		}
		#endregion

		#region Crypting

		/// <summary>
		/// Computes the SHA1 hash value
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string ComputeSHA1(string plainText)
		{
			// Convert plain text into a byte array.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			HashAlgorithm hash = new SHA1Managed();
			// Compute hash value of our plain text with appended salt.
			byte[] hashBytes = hash.ComputeHash(plainTextBytes);
			// Return the result.
			return Convert.ToBase64String(hashBytes);
		}

		public static string CreateCryptoKey(int len)
		{
			byte[] bytes = new byte[len];
			new RNGCryptoServiceProvider().GetBytes(bytes);

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				sb.Append(string.Format("{0:X2}", bytes[i]));
			}

			return sb.ToString();
		}

		public static string Encrypt(string key, string str)
		{
			if (str == null)
				return str;

			// We are now going to create an instance of the 
			// Rihndael class.
			RijndaelManaged RijndaelCipher = new RijndaelManaged();
			byte[] plainText = System.Text.Encoding.Unicode.GetBytes(str);
			byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
			PasswordDeriveBytes secretKey = new PasswordDeriveBytes(key, salt);
			ICryptoTransform encryptor = RijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));

			// encode
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainText, 0, plainText.Length);
			cryptoStream.FlushFinalBlock();
			byte[] cipherBytes = memoryStream.ToArray();

			// Close both streams
			memoryStream.Close();
			cryptoStream.Close();

			// Return encrypted string
			return Convert.ToBase64String(cipherBytes);
		}

        public static string Decrypt(string key, string Base64String)
        {
            var RijndaelCipher = new RijndaelManaged();
            byte[] secretText = Convert.FromBase64String(Base64String);
            byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
            var secretKey = new PasswordDeriveBytes(key, salt);
            var decryptor = RijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            var MemStream = new MemoryStream();
            var DecryptoStream = new CryptoStream(MemStream, decryptor, CryptoStreamMode.Write);
            DecryptoStream.Write(secretText, 0, secretText.Length);
            DecryptoStream.FlushFinalBlock();
            var Result = MemStream.ToArray();
            MemStream.Close();
            DecryptoStream.Close();
            return Encoding.Unicode.GetString(Result);
        }

		public static string GetRandomString(int length)
		{
			string ptrn = "abcdefghjklmnpqrstwxyz0123456789";
			StringBuilder sb = new StringBuilder();

			byte[] randomBytes = new byte[4];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);

			// Convert 4 bytes into a 32-bit integer value.
			int seed = (randomBytes[0] & 0x7f) << 24 |
						randomBytes[1] << 16 |
						randomBytes[2] << 8 |
						randomBytes[3];


			Random rnd = new Random(seed);

			for (int i = 0; i < length; i++)
				sb.Append(ptrn[rnd.Next(ptrn.Length - 1)]);

			return sb.ToString();
		}

		#endregion

		#region Setup

		public static Hashtable GetSetupParameters(object obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			Hashtable args = obj as Hashtable;
			if (args == null)
				throw new ArgumentNullException("obj");

			return args;
		}

		public static object GetSetupParameter(Hashtable args, string paramName)
		{
			if (args == null)
				throw new ArgumentNullException("args");
			//
			if (args.ContainsKey(paramName) == false)
			{
				return String.Empty;
			}
			//
			return args[paramName];
		}

		public static string GetStringSetupParameter(Hashtable args, string paramName)
		{
			object obj = GetSetupParameter(args, paramName);
			if (obj == null)
				return null;
			if (!(obj is string))
				throw new Exception(string.Format("Invalid type of '{0}' parameter", paramName));
			return obj as string;
		}

		public static int GetInt32SetupParameter(Hashtable args, string paramName)
		{
			object obj = GetSetupParameter(args, paramName);
			if (!(obj is int))
				throw new Exception(string.Format("Invalid type of '{0}' parameter", paramName));
			return (int)obj;
		}

		public static Version GetVersionSetupParameter(Hashtable args, string paramName)
		{
			object obj = GetSetupParameter(args, paramName);
			if (!(obj is Version))
				throw new Exception(string.Format("Invalid type of '{0}' parameter", paramName));
			return obj as Version;
		}


		public static string ReplaceScriptVariable(string str, string variable, string value)
		{
			Regex re = new Regex("\\$\\{" + variable + "\\}+", RegexOptions.IgnoreCase);
			return re.Replace(str, value);
		}

		#endregion

		#region Type convertions

		/// <summary>
		/// Converts string to int
		/// </summary>
		/// <param name="value">String containing a number to convert</param>
		/// <param name="defaultValue">Default value</param>
		/// <returns>
		///The Int32 number equivalent to the number contained in value.
		/// </returns>
		public static int ParseInt(string value, int defaultValue)
		{
			if (value != null && value.Length > 0)
			{
				try
				{
					return Int32.Parse(value);
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// ParseBool
		/// </summary>
		/// <param name="value">EventData</param>
		/// <param name="defaultValue">Dafault value</param>
		/// <returns>bool</returns>
		public static bool ParseBool(string value, bool defaultValue)
		{
			if (value != null)
			{
				try
				{
					return bool.Parse(value);
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Converts string to decimal
		/// </summary>
		/// <param name="value">String containing a number to convert</param>
		/// <param name="defaultValue">Default value</param>
		/// <returns>The Decimal number equivalent to the number contained in value.</returns>
		public static decimal ParseDecimal(string value, decimal defaultValue)
		{
			if (value != null && !string.IsNullOrEmpty(value))
			{
				try
				{
					return Decimal.Parse(value);
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Converts string to double 
		/// </summary>
		/// <param name="value">String containing a number to convert</param>
		/// <param name="defaultValue">Default value</param>
		/// <returns>The double number equivalent to the number contained in value.</returns>
		public static double ParseDouble(string value, double defaultValue)
		{
			if (value != null)
			{
				try
				{
					return double.Parse(value);
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			return defaultValue;
		}

		#endregion

		#region DB

		/// <summary>
		/// Converts db value to string
		/// </summary>
		/// <param name="val">EventData</param>
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
		/// <param name="val">EventData</param>
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
		/// <param name="val">EventData</param>
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
		/// <param name="val">EventData</param>
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
		/// <param name="val">EventData</param>
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
		/// <param name="val">EventData</param>
		/// <returns>DateTime</returns>
		public static DateTime GetDbDateTime(object val)
		{
			DateTime ret = DateTime.MinValue;
			if ((val != null) && (val != DBNull.Value))
				ret = (DateTime)val;
			return ret;
		}

		#endregion

		#region Exceptions
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
		#endregion

		#region Windows Firewall
		public static bool IsWindowsFirewallEnabled()
		{
			int ret = RegistryUtils.GetRegistryKeyInt32Value(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile", "EnableFirewall");
			return (ret == 1);
		}
		public static bool IsWindowsFirewallExceptionsAllowed()
		{
			int ret = RegistryUtils.GetRegistryKeyInt32Value(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile", "DoNotAllowExceptions");
			return (ret != 1);
		}
		public static void OpenWindowsFirewallPort(string name, string port)
		{
			string path = Path.Combine(Environment.SystemDirectory, "netsh.exe");
			string arguments = string.Format("firewall set portopening tcp {0} \"{1}\" enable", port, name);
			RunProcess(path, arguments);
		}
        public static void OpenWindowsFirewallPortAdv(string RuleName, string Port)
        {
            string tool = Path.Combine(Environment.SystemDirectory, "netsh.exe");
            string args = string.Format("advfirewall firewall add rule name=\"{0}\" dir=in action=allow protocol=tcp localport={1}", RuleName, Port);
            RunProcess(tool, args);
        }
		#endregion

		#region Processes & Services
		public static int RunProcess(string path, string arguments)
		{
			Process process = null;
			try
			{
				ProcessStartInfo info = new ProcessStartInfo(path, arguments);
				info.WindowStyle = ProcessWindowStyle.Hidden;
				process = Process.Start(info);
				process.WaitForExit();
				return process.ExitCode;
			}
			finally
			{
				if (process != null)
				{
					process.Close();
				}
			}
		}

		public static void StartService(string serviceName)
		{
			ServiceController sc = new ServiceController(serviceName);
			// Start the service if the current status is stopped.
			if (sc.Status == ServiceControllerStatus.Stopped)
			{
				// Start the service, and wait until its status is "Running".
				sc.Start();
				sc.WaitForStatus(ServiceControllerStatus.Running);
			}
		}

		public static void StopService(string serviceName)
		{
			ServiceController sc = new ServiceController(serviceName);
			// Stop the service if the current status is not stopped.
			if (sc.Status != ServiceControllerStatus.Stopped &&
				sc.Status != ServiceControllerStatus.StopPending)
			{
				// Stop the service, and wait until its status is "Running".
				sc.Stop();
				sc.WaitForStatus(ServiceControllerStatus.Stopped);
			}
		}
		#endregion


		#region I/O
		public static string GetSystemDrive()
		{
			return Path.GetPathRoot(Environment.SystemDirectory);
		}
		#endregion

		public static bool IsWebDeployInstalled()
		{
			// TO-DO: Implement Web Deploy detection (x64/x86)
			var isInstalled = false;
			//
			try
			{
				var msdeployRegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\IIS Extensions\MSDeploy\2");
				//
				var keyValue = msdeployRegKey.GetValue("Install");
				// We have found the required key in the registry hive
				if (keyValue != null && keyValue.Equals(1))
				{
					isInstalled = true;
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Could not retrieve Web Deploy key from the registry", ex);
			}
			//
			return isInstalled;
		}

		public static bool IsWin64()
		{
			return (IntPtr.Size == 8);
		}

		public static void ShowConsoleErrorMessage(string format, params object[] args)
		{
			Console.WriteLine(String.Format(format, args));
		}

		public static string ResolveAspNet40RegistrationToolPath_Iis6(SetupVariables setupVariables)
		{
			// By default we fallback to the corresponding tool version based on the platform bitness
			var util = Environment.Is64BitOperatingSystem ? AspNet40RegistrationToolx64 : AspNet40RegistrationToolx86;
			// Choose appropriate tool version for IIS 6
			if (setupVariables.IISVersion.Major == 6)
			{
				// Change to x86 tool version on x64 w/ "Enable32bitAppOnWin64" flag enabled
				if (Environment.Is64BitOperatingSystem == true && Utils.IIS32Enabled())
				{
					util = AspNet40RegistrationToolx86;
				}
			}
			// Build path to the tool
			return Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), util);
		}

		/// <summary>
		/// Beware: Web site component-dependent logic
		/// </summary>
		/// <param name="setupVariables"></param>
		/// <returns></returns>
		public static string ResolveAspNet40RegistrationToolPath_Iis7(SetupVariables setupVariables)
		{
			// By default we fallback to the corresponding tool version based on the platform bitness
			var util = Environment.Is64BitOperatingSystem ? AspNet40RegistrationToolx64 : AspNet40RegistrationToolx86;
			// Choose appropriate tool version for IIS 7
            if (setupVariables.IISVersion.Major >= 7 && setupVariables.SetupAction == SetupActions.Update)
			{
				// Evaluate app pool settings on x64 platform only when update is running
				if (Environment.Is64BitOperatingSystem == true)
				{
					// Change to x86 tool version if the component's app pool is in WOW64 mode
					using (var srvman = new Microsoft.Web.Administration.ServerManager())
					{
						// Retrieve the component's app pool
						var appPoolObj = srvman.ApplicationPools[setupVariables.WebApplicationPoolName];
						// We are 
						if (appPoolObj == null)
						{
							throw new ArgumentException(String.Format("Could not find '{0}' web application pool", setupVariables.WebApplicationPoolName), "appPoolObj");
						}
						// Check app pool mode
						else if (appPoolObj.Enable32BitAppOnWin64 == true)
						{
							util = AspNet40RegistrationToolx86;
						}
					}
				}
			}
			// Build path to the tool
			return Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), util);
		}

		public static bool CheckAspNet40Registered(SetupVariables setupVariables)
		{
            var regkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\ASP.NET\\4.0.30319.0");
            return (regkey != null);
		}

		public static string ExecAspNetRegistrationToolCommand(SetupVariables setupVariables, string arguments)
		{
			//
			var util = (setupVariables.IISVersion.Major == 6) ? Utils.ResolveAspNet40RegistrationToolPath_Iis6(setupVariables) : Utils.ResolveAspNet40RegistrationToolPath_Iis7(setupVariables);
			//
			// Create a specific process start info set to redirect its standard output for further processing
			ProcessStartInfo info = new ProcessStartInfo(util)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				Arguments = arguments
			};
			//
			Log.WriteInfo(String.Format("Starting aspnet_regiis.exe {0}", info.Arguments));
			//
			var process = default(Process);
			//
			var psOutput = String.Empty;
			//
			try
			{
				// Start the process
				process = Process.Start(info);
				// Read the output
				psOutput = process.StandardOutput.ReadToEnd();
				// Wait for the completion
				process.WaitForExit();
			}
			catch (Exception ex)
			{
				Log.WriteError("Could not execute ASP.NET Registration Tool command", ex);
			}
			finally
			{
				if (process != null)
					process.Close();
			}
			// Trace output data for troubleshooting purposes
			Log.WriteInfo(psOutput);
			//
			Log.WriteInfo(String.Format("Finished aspnet_regiis.exe {0}", info.Arguments));
			//
			return psOutput;
		}

		public static void RegisterAspNet40(Setup.SetupVariables setupVariables)
		{
			// Run ASP.NET Registration Tool command
			ExecAspNetRegistrationToolCommand(setupVariables, arguments: (setupVariables.IISVersion.Major == 6) ? "-ir -enable" : "-ir");
		}

		public static WebExtensionStatus GetAspNetWebExtensionStatus_Iis6(SetupVariables setupVariables)
		{
			WebExtensionStatus status = WebExtensionStatus.Allowed;
			if (setupVariables.IISVersion.Major == 6)
			{
				status = WebExtensionStatus.NotInstalled;
				string path;
				if (Utils.IsWin64() && !Utils.IIS32Enabled())
				{
					//64-bit
					path = Path.Combine(OS.GetWindowsDirectory(), @"Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll");
				}
				else
				{
					//32-bit
					path = Path.Combine(OS.GetWindowsDirectory(), @"Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll");
				}
				path = path.ToLower();
				using (DirectoryEntry iis = new DirectoryEntry("IIS://LocalHost/W3SVC"))
				{
					PropertyValueCollection values = iis.Properties["WebSvcExtRestrictionList"];
					for (int i = 0; i < values.Count; i++)
					{
						string val = values[i] as string;
						if (!string.IsNullOrEmpty(val))
						{
							string strVal = val.ToString().ToLower();

							if (strVal.Contains(path))
							{
								if (strVal[0] == '1')
								{
									status = WebExtensionStatus.Allowed;
								}
								else
								{
									status = WebExtensionStatus.Prohibited;
								}
								break;
							}
						}
					}
				}
			}
			return status;
		}

		public static void EnableAspNetWebExtension_Iis6()
		{
			Log.WriteStart("Enabling ASP.NET Web Service Extension");
			//
			var webExtensionName = (Utils.IsWin64() && Utils.IIS32Enabled()) ? "ASP.NET v4.0.30319 (32-bit)" : "ASP.NET v4.0.30319";
			//
			using (DirectoryEntry iisService = new DirectoryEntry("IIS://LocalHost/W3SVC"))
			{
				iisService.Invoke("EnableWebServiceExtension", webExtensionName);
				iisService.CommitChanges();
			}
			//
			Log.WriteEnd("Enabled ASP.NET Web Service Extension");
		}

		public static bool IIS32Enabled()
		{
			bool enabled = false;
			using (DirectoryEntry obj = new DirectoryEntry("IIS://LocalHost/W3SVC/AppPools"))
			{
				object objProperty = GetObjectProperty(obj, "Enable32bitAppOnWin64");
				if (objProperty != null)
				{
					enabled = (bool)objProperty;
				}
			}
			return enabled;
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

		public static void OpenFirewallPort(string name, string port, Version iisVersion)
		{
            bool iis7 = (iisVersion.Major >= 7);
			if (iis7)
			{
				if (Utils.IsWindowsFirewallEnabled() && Utils.IsWindowsFirewallExceptionsAllowed())
                {
                    Log.WriteStart(String.Format("Opening port {0} in windows firewall", port));
                    Utils.OpenWindowsFirewallPortAdv(name, port);
                    Log.WriteEnd("Opened port in windows firewall");
                    InstallLog.AppendLine(String.Format("- Opened port {0} in Windows Firewall", port));
                }
			}
			else
			{
				if (Utils.IsWindowsFirewallEnabled() &&
					Utils.IsWindowsFirewallExceptionsAllowed())
				{
					//SetProgressText("Opening port in windows firewall...");

					Log.WriteStart(String.Format("Opening port {0} in windows firewall", port));

					Utils.OpenWindowsFirewallPort(name, port);

					//update log
					Log.WriteEnd("Opened port in windows firewall");
					InstallLog.AppendLine(String.Format("- Opened port {0} in Windows Firewall", port));
				}
			}
		}

		public static string[] GetApplicationUrls(string ip, string domain, string port, string virtualDir)
		{
			List<string> urls = new List<string>();

			// IP address, [port] and [virtualDir]
			string url = ip;
			if (String.IsNullOrEmpty(domain))
			{
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			// domain, [port] and [virtualDir]
			if (!String.IsNullOrEmpty(domain))
			{
				url = domain;
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			return urls.ToArray();
		}
	}
}
