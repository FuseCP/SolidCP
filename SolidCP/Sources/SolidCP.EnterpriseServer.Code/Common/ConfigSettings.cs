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
using System.Configuration;
using Microsoft.Win32;
using SolidCP.Providers.OS;
using SolidCP.Web.Services;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for ConfigSettings.
	/// </summary>
	public class ConfigSettings
	{

		const string EnterpriseServerRegistryPath = "SOFTWARE\\SolidCP\\EnterpriseServer";

		private static string GetKeyFromRegistry(string Key)
		{
			string value = string.Empty;

			if (!string.IsNullOrEmpty(Key))
			{
				RegistryKey root = Registry.LocalMachine;
				RegistryKey rk = root.OpenSubKey(EnterpriseServerRegistryPath);
				if (rk != null)
				{
					value = (string)rk.GetValue(Key, null);
					rk.Close();
				}
			}
			return value;
		}

		public static string ConnectionString => Data.DbSettings.ConnectionString;
		public static string SpecificConnectionString => Data.DbSettings.NativeConnectionString;

		static string cryptoKey = null;
		public static string CryptoKey
		{
			get
			{
				if (cryptoKey == null)
				{
					string key;
					if (OSInfo.IsNetFX)
					{
						key = ConfigurationManager.AppSettings["SolidCP.AltCryptoKey"];
					}
					else
					{
						key = Web.Services.Configuration.AltCryptoKey;
					}

					string value = string.Empty;

					if (OSInfo.IsWindows) value = GetKeyFromRegistry(key);

					if (!string.IsNullOrEmpty(value))
						cryptoKey = value;
					else
					{
						if (OSInfo.IsNetFX)
						{
							cryptoKey = ConfigurationManager.AppSettings["SolidCP.CryptoKey"];
						}
						else
						{
							cryptoKey = Web.Services.Configuration.CryptoKey;
						}
					}
				}
				return cryptoKey;
			}
		}


		static bool? encryptionEnabled = null;
		public static bool EncryptionEnabled
		{
			get
			{
				if (encryptionEnabled == null)
				{
					if (OSInfo.IsNetFX)
					{
						encryptionEnabled = (ConfigurationManager.AppSettings["SolidCP.EncryptionEnabled"] != null)
						? bool.Parse(ConfigurationManager.AppSettings["SolidCP.EncryptionEnabled"]) : true;
					} else
					{
						encryptionEnabled = Web.Services.Configuration.EncryptionEnabled;
					}
				}
				return encryptionEnabled.Value;
			}
		}


		static string dataProviderType = null;
		public static string DataProviderType
		{
			get
			{
				if (dataProviderType == null)
				{
					if (OSInfo.IsNetFX)
					{
						dataProviderType = ConfigurationManager.AppSettings["SolidCP.EnterpriseServer.DataProvider"];
					}
					else
					{
						dataProviderType = Web.Services.Configuration.DataProviderType;
					}
				}
				return dataProviderType;
			}
		}

		static string webApplicationPath = null;
		public static string WebApplicationsPath
		{
			get
			{
				if (webApplicationPath == null)
				{
					if (OSInfo.IsNetFX)
					{
						webApplicationPath = ConfigurationManager.AppSettings["SolidCP.EnterpriseServer.WebApplicationsPath"];
					}
					else
					{
						webApplicationPath = Web.Services.Configuration.WebApplicationsPath;
					}
				}
				if (webApplicationPath.StartsWith("~")) webApplicationPath = Web.Services.Server.MapPath(webApplicationPath);

				return webApplicationPath;
			}
		}

		public static string BackupsPath
		{
			get
			{
				SystemSettings settings = new SystemController().GetSystemSettingsInternal(
					SystemSettings.BACKUP_SETTINGS,
					false
				);

				return settings["BackupsPath"];
			}
		}

		#region Communication
		static int? serverRequestTimeout = null;
		public static int ServerRequestTimeout
		{
			get
			{
				if (serverRequestTimeout == null)
				{
					if (OSInfo.IsNetFX)
					{
						serverRequestTimeout = Utils.ParseInt(
							ConfigurationManager.AppSettings["SolidCP.EnterpriseServer.ServerRequestTimeout"], -1);
					}
					else
					{
						serverRequestTimeout = Web.Services.Configuration.ServerRequestTimeout ?? -1;
					}
				}
				return serverRequestTimeout.Value;
			}
		}
		#endregion
	}
}
