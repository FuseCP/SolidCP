using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using SolidCP.EnterpriseServer.Base.RDS;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.RemoteDesktopServices;

namespace SolidCP.EnterpriseServer
{
    public class RemoteDesktopServicesHelpers
    {
        public static string GetFormattedCollectionName(string displayName, string organizationId)
        {
            return string.Format("{0}-{1}", organizationId, displayName.Replace(" ", "_"));
        }

        public static string EvaluateMailboxTemplate(string template, Organization org, int? accountId, int itemId)
        {
            OrganizationUser user = null;

            if (accountId.HasValue)
            {
                user = OrganizationController.GetAccount(itemId, accountId.Value);
            }

            Hashtable items = new Hashtable();
            items["Organization"] = org;

            if (user != null)
            {
                items["account"] = user;
            }

            return PackageController.EvaluateTemplate(template, items);
        }

        public static RdsCollectionSettings GetDefaultCollectionSettings()
        {
            return new RdsCollectionSettings
            {
                DisconnectedSessionLimitMin = 0,
                ActiveSessionLimitMin = 0,
                IdleSessionLimitMin = 0,
                BrokenConnectionAction = BrokenConnectionActionValues.Disconnect.ToString(),
                AutomaticReconnectionEnabled = true,
                TemporaryFoldersDeletedOnExit = true,
                TemporaryFoldersPerSession = true,
                ClientDeviceRedirectionOptions = string.Join(",", new List<string>
                    {
                        ClientDeviceRedirectionOptionValues.AudioVideoPlayBack.ToString(),
                        ClientDeviceRedirectionOptionValues.AudioRecording.ToString(),
                        ClientDeviceRedirectionOptionValues.SmartCard.ToString(),
                        ClientDeviceRedirectionOptionValues.Clipboard.ToString(),
                        ClientDeviceRedirectionOptionValues.Drive.ToString(),
                        ClientDeviceRedirectionOptionValues.PlugAndPlayDevice.ToString()
                    }.ToArray()),
                ClientPrinterRedirected = true,
                ClientPrinterAsDefault = true,
                RDEasyPrintDriverEnabled = true,
                MaxRedirectedMonitors = 16,
                EncryptionLevel = EncryptionLevel.ClientCompatible.ToString(),
                SecurityLayer = SecurityLayerValues.Negotiate.ToString(),
                AuthenticateUsingNLA = true
            };
        }

        public static RdsServerSettings GetDefaultGpoSettings()
        {
            var defaultSettings = UserController.GetUserSettings(SecurityContext.User.UserId, UserSettings.RDS_POLICY);
            var settings = new RdsServerSettings();

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.LOCK_SCREEN_TIMEOUT,
                PropertyValue = defaultSettings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_VALUE],
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_RUN_COMMAND,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.REMOVE_RUN_COMMAND_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.REMOVE_RUN_COMMAND_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_POWERSHELL_COMMAND,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.REMOVE_POWERSHELL_COMMAND_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.REMOVE_POWERSHELL_COMMAND_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.HIDE_C_DRIVE,
                PropertyValue = defaultSettings[RdsServerSettings.HIDE_C_DRIVE_VALUE],
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.HIDE_C_DRIVE_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.HIDE_C_DRIVE_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_SHUTDOWN_RESTART,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.REMOVE_SHUTDOWN_RESTART_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.REMOVE_SHUTDOWN_RESTART_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.DISABLE_TASK_MANAGER,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.DISABLE_TASK_MANAGER_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.DISABLE_TASK_MANAGER_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.CHANGE_DESKTOP_DISABLED,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.CHANGE_DESKTOP_DISABLED_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.CHANGE_DESKTOP_DISABLED_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.SCREEN_SAVER_DISABLED,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.SCREEN_SAVER_DISABLED_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.SCREEN_SAVER_DISABLED_USERS])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION_Users])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION_Users])
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.DISABLE_CMD,
                PropertyValue = "",
                ApplyAdministrators = Convert.ToBoolean(defaultSettings[RdsServerSettings.DISABLE_CMD_ADMINISTRATORS]),
                ApplyUsers = Convert.ToBoolean(defaultSettings[RdsServerSettings.DISABLE_CMD_USERS])
            });

            return settings;
        }

        public static RdsServerSettings GetEmptyGpoSettings()
        {
            var defaultSettings = UserController.GetUserSettings(SecurityContext.User.UserId, UserSettings.RDS_POLICY);
            var settings = new RdsServerSettings();

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.LOCK_SCREEN_TIMEOUT,
                PropertyValue = defaultSettings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_VALUE]
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_RUN_COMMAND,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_POWERSHELL_COMMAND,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.HIDE_C_DRIVE,
                PropertyValue = defaultSettings[RdsServerSettings.HIDE_C_DRIVE_VALUE]
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_SHUTDOWN_RESTART,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.DISABLE_TASK_MANAGER,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.CHANGE_DESKTOP_DISABLED,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.SCREEN_SAVER_DISABLED,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION,
                PropertyValue = ""
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.DISABLE_CMD,
                PropertyValue = ""
            });

            return settings;
        }

        public static string GetSettingsXml(RdsServerSettings settings)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement nodeProps = doc.CreateElement("properties");

            if (settings != null)
            {
                foreach (var setting in settings.Settings)
                {
                    XmlElement nodeProp = doc.CreateElement("property");
                    nodeProp.SetAttribute("name", setting.PropertyName);
                    nodeProp.SetAttribute("value", setting.PropertyValue);
                    nodeProp.SetAttribute("applyUsers", setting.ApplyUsers ? "1" : "0");
                    nodeProp.SetAttribute("applyAdministrators", setting.ApplyAdministrators ? "1" : "0");
                    nodeProps.AppendChild(nodeProp);
                }
            }

            return nodeProps.OuterXml;
        }

        public static int GetRemoteDesktopServiceID(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.RDS);
        }        

        public static RemoteDesktopServices GetRemoteDesktopServices(int serviceId)
        {
            var rds = new RemoteDesktopServices();
            ServiceProviderProxy.Init(rds, serviceId);

            return rds;
        }        

        public static RdsServer FillRdsServerData(RdsServer server)
        {
            var serverIp = GetServerIp(server.FqdName);

            if (serverIp != null)
            {
                server.Address = serverIp.ToString();
            }
            else
            {
                server.Address = "";
            }

            return server;
        }

        public static System.Net.IPAddress GetServerIp(string hostname, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var address = GetServerIps(hostname);

            return address.FirstOrDefault(x => x.AddressFamily == addressFamily);
        }

        public static RdsCollectionSettings ParseCollectionSettings(List<RdsCollectionSetting> settings)
        {
            var collectionSettings = new RdsCollectionSettings();
            var properties = typeof(RdsCollectionSettings).GetProperties().Where(p => p.Name.ToLower() != "id" && p.Name.ToLower() != "rdscollectionid");            

            foreach (var prop in properties)
            {
                var values = settings.Where(s => s.PropertyName.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase));

                if (values.Count() == 1)
                {
                    switch(prop.Name.ToLower())
                    {
                        case "brokenconnectionaction":
                            prop.SetValue(collectionSettings, ((BrokenConnectionActionValues)values.First().PropertyValue).ToString(), null);
                            break;
                        case "clientdeviceredirectionoptions":
                            prop.SetValue(collectionSettings, ((ClientDeviceRedirectionOptionValues)values.First().PropertyValue).ToString(), null);
                            break;
                        case "encryptionlevel":
                            prop.SetValue(collectionSettings, ((EncryptionLevel)values.First().PropertyValue).ToString(), null);
                            break;
                        case "securitylayer":
                            prop.SetValue(collectionSettings, ((SecurityLayerValues)values.First().PropertyValue).ToString(), null);
                            break;
                        default:
                            prop.SetValue(collectionSettings, Convert.ChangeType(values.First().PropertyValue, prop.PropertyType), null);
                            break;
                    }                    
                }
            }

            return collectionSettings;
        }

        public static void FillSessionHosts(IEnumerable<string> sessionHosts, IEnumerable<RdsServer> existingSessionHosts, int collectionId, int itemId)
        {
            var domainName = string.Format(".{0}", IPGlobalProperties.GetIPGlobalProperties().DomainName);

            foreach (var sessionHost in sessionHosts)
            {
                var existingSessionHost = existingSessionHosts.FirstOrDefault(e => e.FqdName.Equals(sessionHost, StringComparison.InvariantCultureIgnoreCase));
                int serverId = -1;

                if (existingSessionHost == null)
                {
                    var serverName = sessionHost.Replace(domainName, "");
                    serverId = DataProvider.AddRDSServer(serverName, sessionHost, "");                    
                }
                else
                {
                    serverId = existingSessionHost.Id;
                }

                DataProvider.AddRDSServerToOrganization(itemId, serverId);
                DataProvider.AddRDSServerToCollection(serverId, collectionId);
            }
        }

        private static IEnumerable<System.Net.IPAddress> GetServerIps(string hostname)
        {
            try
            {
                var address = Dns.GetHostAddresses(hostname);
                return address;
            }
            catch
            {
            }

            return new List<System.Net.IPAddress>();
        }               
    }
}
