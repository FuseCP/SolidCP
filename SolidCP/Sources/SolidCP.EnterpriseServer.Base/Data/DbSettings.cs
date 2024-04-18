using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Win32;
using SolidCP.Providers.OS;

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
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {

                    string ConnectionKey;
                    if (OSInfo.IsNetFX)
                    {
                        ConnectionKey = ConfigurationManager.AppSettings["SolidCP.AltConnectionString"];
                    }
                    else
                    {
                        ConnectionKey = Web.Services.Configuration.AltConnectionString;
                    }

                    string value = string.Empty;

                    if (!string.IsNullOrEmpty(ConnectionKey) && OSInfo.IsWindows)
                    {
                        value = GetKeyFromRegistry(ConnectionKey);
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
                            connectionString = Web.Services.Configuration.ConnectionString;
                        }
                    }
                }
                return connectionString;
            }
        }

        public static string SpecificConnectionString =>
            Regex.Replace(DbSettings.ConnectionString, @"(?:;|^)\s*Flavor\s*=[^;$]*", "", RegexOptions.IgnoreCase);
    }
}
