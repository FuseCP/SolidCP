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
using System.Threading;
using System.Xml;
using SolidCP.UniversalInstaller.Core;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.Setup
{
	/// <summary>
	/// Provides methods for rolling back after an error.
	/// </summary>
	public sealed class RollBack
	{
		private static XmlDocument script;

		static RollBack()
		{
			script = new XmlDocument();
			XmlElement root = script.CreateElement("currentScenario");
			script.AppendChild(root);
		}

		private static XmlNode RootElement
		{
			get
			{
				return script.ChildNodes[0];
			}
		}
		
		internal static  XmlNodeList Actions
		{
			get
			{
				return RootElement.ChildNodes;
			}
		}

		internal static void RegisterRegistryAction(string key)
		{
			XmlElement action = script.CreateElement("registry");
			RootElement.AppendChild(action); 
			action.SetAttribute("key", key);
		}

		internal static void RegisterDirectoryAction(string path)
		{
			XmlElement action = script.CreateElement("directory");
			RootElement.AppendChild(action); 
			action.SetAttribute("path", path);
		}

		internal static void RegisterDirectoryBackupAction(string source, string destination)
		{
			XmlElement action = script.CreateElement("directoryBackup");
			RootElement.AppendChild(action);
			action.SetAttribute("source", source);
			action.SetAttribute("destination", destination);
		}

		internal static void RegisterDatabaseAction(string connectionString, string name)
		{
			XmlElement action = script.CreateElement("database");
			RootElement.AppendChild(action); 
			action.SetAttribute("connectionString", connectionString);
			action.SetAttribute("name", name);
		}

		internal static void RegisterDatabaseBackupAction(string connectionString, string name, string bakFile, string position)
		{
			XmlElement action = script.CreateElement("databaseBackup");
			RootElement.AppendChild(action);
			action.SetAttribute("connectionString", connectionString);
			action.SetAttribute("name", name);
			action.SetAttribute("bakFile", bakFile);
			action.SetAttribute("position", position);
		}


        internal static void RegisterDatabaseUserAction(string connectionString, string username)
        {
            XmlElement action = script.CreateElement("databaseUser");
            RootElement.AppendChild(action);
            action.SetAttribute("connectionString", connectionString);
            action.SetAttribute("username", username);
        }

		internal static void RegisterWebSiteAction(string siteId)
		{
			XmlElement action = script.CreateElement("webSite");
			RootElement.AppendChild(action); 
			action.SetAttribute("siteId", siteId);
		}

		internal static void RegisterIIS7WebSiteAction(string siteId)
		{
			XmlElement action = script.CreateElement("IIS7WebSite");
			RootElement.AppendChild(action);
			action.SetAttribute("siteId", siteId);
		}

		internal static void RegisterVirtualDirectoryAction(string siteId, string name)
		{
			XmlElement action = script.CreateElement("virtualDirectory");
			RootElement.AppendChild(action); 
			action.SetAttribute("siteId", siteId);
			action.SetAttribute("name", name);
		}

		internal static void RegisterUserAccountAction(string domain, string userName)
		{
			XmlElement action = script.CreateElement("userAccount");
			RootElement.AppendChild(action); 
			action.SetAttribute("name", userName);
			action.SetAttribute("domain", domain);
		}

		internal static void RegisterApplicationPool(string name)
		{
			XmlElement action = script.CreateElement("applicationPool");
			RootElement.AppendChild(action); 
			action.SetAttribute("name", name);
		}

		internal static void RegisterIIS7ApplicationPool(string name)
		{
			XmlElement action = script.CreateElement("IIS7ApplicationPool");
			RootElement.AppendChild(action);
			action.SetAttribute("name", name);
		}

		internal static void RegisterStopApplicationPool(string name)
		{
			XmlElement action = script.CreateElement("stopApplicationPool");
			RootElement.AppendChild(action);
			action.SetAttribute("name", name);
		}

		internal static void RegisterStopIIS7ApplicationPool(string name)
		{
			XmlElement action = script.CreateElement("stopIIS7ApplicationPool");
			RootElement.AppendChild(action);
			action.SetAttribute("name", name);
		}

		internal static void RegisterConfigAction(string componentId, string name)
		{
			XmlElement action = script.CreateElement("config");
			RootElement.AppendChild(action);
			action.SetAttribute("key", componentId);
			action.SetAttribute("name", name);
		}

		internal static void RegisterWindowsService(string path, string name)
		{
			XmlElement action = script.CreateElement("WindowsService");
			RootElement.AppendChild(action);
			action.SetAttribute("path", path);
			action.SetAttribute("name", name);
		}

		internal static void ProcessAction(XmlNode action)
		{
			switch(action.Name)
			{
				case "registry":
					DeleteRegistryKey(XmlUtils.GetXmlAttribute(action, "key"));
					break;
				case "directory":
					DeleteDirectory(XmlUtils.GetXmlAttribute(action, "path"));
					break;
				case "directoryBackup":
					RestoreDirectory(
						XmlUtils.GetXmlAttribute(action, "source"),
						XmlUtils.GetXmlAttribute(action, "destination"));
					break;
				case "database":
					DeleteDatabase(
						XmlUtils.GetXmlAttribute(action, "connectionString"),
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "databaseBackup":
					RestoreDatabase(
						XmlUtils.GetXmlAttribute(action, "connectionString"),
						XmlUtils.GetXmlAttribute(action, "name"),
						XmlUtils.GetXmlAttribute(action, "bakFile"),
						XmlUtils.GetXmlAttribute(action, "position"));
					break;
                case "databaseUser":
                    DeleteDatabaseUser(
						XmlUtils.GetXmlAttribute(action, "connectionString"),
						XmlUtils.GetXmlAttribute(action, "username"));
                    break;
				case "webSite":
					DeleteWebSite(
						XmlUtils.GetXmlAttribute(action, "siteId"));
					break;
				case "IIS7WebSite":
					DeleteIIS7WebSite(
						XmlUtils.GetXmlAttribute(action, "siteId"));
					break;
				case "virtualDirectory":
					DeleteVirtualDirectory(
						XmlUtils.GetXmlAttribute(action, "siteId"),
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "userAccount":
					DeleteUserAccount(
						XmlUtils.GetXmlAttribute(action, "domain"),
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "applicationPool":
					DeleteApplicationPool(
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "IIS7ApplicationPool":
					DeleteIIS7ApplicationPool(
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "config":
					DeleteComponentSettings(
						XmlUtils.GetXmlAttribute(action, "key"),
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "stopApplicationPool":
					StartApplicationPool(
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "stopIIS7ApplicationPool":
					StartIIS7ApplicationPool(
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
				case "WindowsService":
					UnregisterWindowsService(
						XmlUtils.GetXmlAttribute(action, "path"),
						XmlUtils.GetXmlAttribute(action, "name"));
					break;
			}
		}

		private static void DeleteComponentSettings(string id, string name)
		{
			try
			{
				Log.WriteStart("Deleting component settings");
				Log.WriteInfo(string.Format("Deleting \"{0}\" settings", name));
				XmlNode node = AppConfig.GetComponentConfig(id);
				if (node != null)
				{
					XmlUtils.RemoveXmlNode(node);
					AppConfig.SaveConfiguration();
				}
				Log.WriteEnd("Deleted component settings");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Component settings delete error", ex);
				throw;
			}
		}

		private static void DeleteRegistryKey(string subkey)
		{
			try
			{
				Log.WriteStart("Deleting registry key");
				Log.WriteInfo(string.Format("Deleting registry key \"{0}\"", subkey));
				RegistryUtils.DeleteRegistryKey(subkey);
				Log.WriteEnd("Deleted registry key");
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Registry key delete error", ex);
				throw;
			}
		}

		private static void DeleteDirectory(string path)
		{
			try
			{
				Log.WriteStart("Deleting directory");
				Log.WriteInfo(string.Format("Deleting directory \"{0}\"", path));

				if(FileUtils.DirectoryExists(path))
				{
					FileUtils.DeleteDirectory(path);
					Log.WriteEnd("Deleted directory");
				}
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Directory delete error", ex);
				throw;
			}
		}

		private static void RestoreDirectory(string targetDir, string zipFile)
		{
			try
			{
				Log.WriteStart("Restoring directory");
				Log.WriteInfo(string.Format("Restoring directory \"{0}\"", targetDir));
				FileUtils.UnzipFile(targetDir, zipFile);
				Log.WriteStart("Restored directory");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Directory restore error", ex);
				throw;
			}
		}


		private static void DeleteDatabase(string connectionString, string name)
		{
			try
			{
				Log.WriteStart("Deleting database");
				Log.WriteInfo(string.Format("Deleting database \"{0}\"", name));
				if ( DatabaseUtils.DatabaseExists(connectionString, name) )
				{
					DatabaseUtils.DeleteDatabase(connectionString, name);
					Log.WriteEnd("Deleted database");
				}
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database delete error", ex);
				throw;
			}
		}

		private static void RestoreDatabase(string connectionString, string name, string bakFile, string position)
		{
			try
			{
				Log.WriteStart("Restoring database");
				Log.WriteInfo(string.Format("Restoring database \"{0}\"", name));
				if (DatabaseUtils.DatabaseExists(connectionString, name))
				{
					DatabaseUtils.RestoreDatabase(connectionString, name, bakFile, position);
					Log.WriteEnd("Restored database");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database restore error", ex);
				throw;
			}
		}


        private static void DeleteDatabaseUser(string connectionString, string username)
        {
            try
            {
                Log.WriteStart("Deleting database user");
                Log.WriteInfo(string.Format("Deleting database user \"{0}\"", username));
                if (DatabaseUtils.UserExists(connectionString, username))
                {
                    DatabaseUtils.DeleteUser(connectionString, username);
                    Log.WriteEnd("Deleted database user");
                }
            }
            catch (Exception ex)
            {
				if (Utils.IsThreadAbortException(ex))
					return;

                Log.WriteError("Database user delete error", ex);
                throw;
            }
        }
		
		private static void DeleteWebSite(string siteId)
		{
			try
			{
				Log.WriteStart("Deleting web site");
				Log.WriteInfo(string.Format("Deleting web site \"{0}\"", siteId));
				if ( WebUtils.SiteIdExists(siteId) )
				{
					WebUtils.DeleteSite(siteId);
					Log.WriteEnd("Deleted web site");
				}
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				throw;
			}
		}

		private static void DeleteIIS7WebSite(string siteId)
		{
			try
			{
				Log.WriteStart("Deleting web site");
				Log.WriteInfo(string.Format("Deleting web site \"{0}\"", siteId));
				if (WebUtils.IIS7SiteExists(siteId))
				{
					WebUtils.DeleteIIS7Site(siteId);
					Log.WriteEnd("Deleted web site");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				throw;
			}
		}

		private static void DeleteVirtualDirectory(string siteId, string name)
		{
			try
			{
				Log.WriteStart("Deleting virtual directory");
				Log.WriteInfo(string.Format("Deleting virtual directory \"{0}\" for the site \"{1}\"", name, siteId));
				if ( WebUtils.VirtualDirectoryExists(siteId, name) )
				{
					WebUtils.DeleteVirtualDirectory(siteId, name);
					Log.WriteEnd("Deleted  virtual directory");
				}
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Virtual directory delete error", ex);
				throw;
			}
		}

		private static void DeleteUserAccount(string domain, string user)
		{
			try
			{
				Log.WriteStart("Deleting user account");
				Log.WriteInfo(string.Format("Deleting user account \"{0}\"", user));
				if ( SecurityUtils.UserExists(domain, user) )
				{
					SecurityUtils.DeleteUser(domain, user);
					Log.WriteEnd("Deleted  user account");
				}
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("User account delete error", ex);
				throw;
			}
		}

		private static void DeleteApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Deleting application pool");
				Log.WriteInfo(string.Format("Deleting application pool \"{0}\"", name));
				if (WebUtils.ApplicationPoolExists(name))
				{
					int count = WebUtils.GetApplicationPoolSitesCount(name);
					if (count > 0)
					{
						Log.WriteEnd("Application pool is not empty");
					}
					else
					{
						WebUtils.DeleteApplicationPool(name);
						Log.WriteEnd("Deleted  application pool");
					}
				}
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool delete error", ex);
				throw;
			}
		}
		private static void DeleteIIS7ApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Deleting application pool");
				Log.WriteInfo(string.Format("Deleting application pool \"{0}\"", name));
				if (WebUtils.IIS7ApplicationPoolExists(name))
				{
					int count = WebUtils.GetIIS7ApplicationPoolSitesCount(name);
					if (count > 0)
					{
						Log.WriteEnd("Application pool is not empty");
					}
					else
					{
						WebUtils.DeleteIIS7ApplicationPool(name);
						Log.WriteEnd("Deleted  application pool");
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool delete error", ex);
				throw;
			}
		}

		private static void StartApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Starting IIS application pool");
				Log.WriteInfo(string.Format("Starting \"{0}\"", name));
				WebUtils.StartApplicationPool(name);
				Log.WriteEnd("Started application pool");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool start error", ex);
				throw;
			}
		}

		private static void StartIIS7ApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Starting IIS application pool");
				Log.WriteInfo(string.Format("Starting \"{0}\"", name));
				WebUtils.StartIIS7ApplicationPool(name);
				Log.WriteEnd("Started application pool");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool start error", ex);
				throw;
			}
		}

		private static void UnregisterWindowsService(string path, string serviceName)
		{
			try
			{
				Log.WriteStart(string.Format("Unregistering \"{0}\" Windows service", serviceName));
				try
				{
					Utils.StopService(serviceName);
				}
				catch (Exception ex)
				{
					if (!Utils.IsThreadAbortException(ex))
						Log.WriteError("Windows service stop error", ex);
				}
				Utils.RunProcess(path, "/u");
				Log.WriteEnd("Unregistered Windows service");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Windows service error", ex);
				throw;
			}
		}
	}
}
