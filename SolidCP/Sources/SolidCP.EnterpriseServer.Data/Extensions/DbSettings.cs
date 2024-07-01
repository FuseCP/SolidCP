using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Win32;
using SolidCP.Providers.OS;
using System.Data;

namespace SolidCP.EnterpriseServer.Data
{
	public class DbSettings
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


        static string connectionString = null;

		static string ConnectionStringNetCore
		{
			get
			{
				if (string.IsNullOrEmpty(connectionString))
				{

					string connectionKey = null;
					if (OSInfo.IsNetFX)
					{
						//connectionKey = ConfigurationManager.AppSettings["SolidCP.AltConnectionString"];
					}
					else
					{
						connectionKey = Web.Services.Configuration.AltConnectionString;
					}

					string value = string.Empty;

					if (!string.IsNullOrEmpty(connectionKey) && OSInfo.IsWindows)
					{
						value = GetKeyFromRegistry(connectionKey);
					}

					if (!string.IsNullOrEmpty(value))
					{
						connectionString = value;
					}
					else
					{
						if (OSInfo.IsNetFX)
						{
							//connectionString = ConfigurationManager.ConnectionStrings["EnterpriseServer"].ConnectionString;
						}
						else
						{
							connectionString = Web.Services.Configuration.ConnectionString;

						}
					}
				}
				return connectionString;
			}
		}
		static string ConnectionStringNetFX
		{
			get
			{
				if (string.IsNullOrEmpty(connectionString))
				{

					string connectionKey = null;
					if (OSInfo.IsNetFX)
					{
						connectionKey = ConfigurationManager.AppSettings["SolidCP.AltConnectionString"];
					}
					else
					{
						//connectionKey = Web.Services.Configuration.AltConnectionString;
					}

					string value = string.Empty;

					if (!string.IsNullOrEmpty(connectionKey) && OSInfo.IsWindows)
					{
						value = GetKeyFromRegistry(connectionKey);
					}

					if (!string.IsNullOrEmpty(value))
					{
						connectionString = value;
					}
					else
					{
						if (OSInfo.IsNetFX)
						{
							connectionString = ConfigurationManager.ConnectionStrings["EnterpriseServer"].ConnectionString;
						}
						else
						{
							//connectionString = Web.Services.Configuration.ConnectionString;

						}
					}
				}
				return connectionString;
			}
		}

		static string providerName = null;
		static string ProviderNameNetCore
		{
			get
			{
				if (string.IsNullOrEmpty(providerName))
				{

					string providerKey = null;
					if (OSInfo.IsNetFX)
					{
						//providerKey = ConfigurationManager.AppSettings["SolidCP.AltProviderName"];
					}
					else
					{
						providerKey = Web.Services.Configuration.AltProviderName;
					}

					string value = string.Empty;

					if (!string.IsNullOrEmpty(providerKey) && OSInfo.IsWindows)
					{
						value = GetKeyFromRegistry(providerKey);
					}

					if (!string.IsNullOrEmpty(value))
					{
						providerName = value;
					}
					else
					{
						if (OSInfo.IsNetFX)
						{
							//providerName = ConfigurationManager.ConnectionStrings["EnterpriseServer"].ProviderName;
						}
						else
						{
							providerName = Web.Services.Configuration.ProviderName;

						}
					}
				}
				return providerName;
			}
		}
		static string ProviderNameNetFX
		{
			get
			{
				if (string.IsNullOrEmpty(providerName))
				{

					string providerKey = null;
					if (OSInfo.IsNetFX)
					{
						providerKey = ConfigurationManager.AppSettings["SolidCP.AltProviderName"];
					}
					else
					{
						//providerKey = Web.Services.Configuration.AltProviderName;
					}

					string value = string.Empty;

					if (!string.IsNullOrEmpty(providerKey) && OSInfo.IsWindows)
					{
						value = GetKeyFromRegistry(providerKey);
					}

					if (!string.IsNullOrEmpty(value))
					{
						providerName = value;
					}
					else
					{
						if (OSInfo.IsNetFX)
						{
							providerName = ConfigurationManager.ConnectionStrings["EnterpriseServer"].ProviderName;
						}
						else
						{
							//providerName = Web.Services.Configuration.ProviderName;

						}
					}
				}
				return connectionString;
			}
		}

		public static string ConnectionString => OSInfo.IsNetFX ? ConnectionStringNetFX : ConnectionStringNetCore;
		public static string ProviderName => OSInfo.IsNetFX ? ProviderNameNetFX : ProviderNameNetCore;
		public static string NativeConnectionString => GetNativeConnectionString(ConnectionString);

        public static string GetNativeConnectionString(string connectionString)
        {
            return Regex.Replace(connectionString, @$"^\s*{nameof(DbType)}\s*=[^;$]*;|;\s*{nameof(DbType)}\s*=[^;$]*", "", RegexOptions.IgnoreCase);
		}
		public static DbType GetDbType(string connectionString)
        {
            DbType dbType = DbType.Unknown;
            var dbTypeName = Regex.Match(connectionString, @$"(?<=(?:;|^)\s*{nameof(DbType)}\s*=\s*)[^;$]*", RegexOptions.IgnoreCase)?.Value.Trim();
			if (!string.IsNullOrEmpty(dbTypeName) && !Enum.TryParse<DbType>(dbTypeName, true, out dbType)) dbType = DbType.Other;
            return dbType;
		}

        static DbType dbType = DbType.Unknown;
        public static DbType DbType => dbType != DbType.Unknown ? dbType : (dbType = GetDbType(ConnectionString));

		public static bool AlwaysUseEntityFrameworkNetFX =>
			string.Equals(ConfigurationManager.AppSettings["SolidCP.AlwaysUseEntityFramework"], "true", StringComparison.OrdinalIgnoreCase);
		public static bool AlwaysUseEntityFrameworkNetCore => Web.Services.Configuration.AlwaysUseEntityFramework;

		public static bool AlwaysUseEntityFramework => OSInfo.IsNetFX ? AlwaysUseEntityFrameworkNetFX : AlwaysUseEntityFrameworkNetCore;
	}
}
