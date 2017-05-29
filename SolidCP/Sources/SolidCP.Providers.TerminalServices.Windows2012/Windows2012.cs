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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Text;
using System.Reflection;
using Microsoft.Win32;
using SolidCP.Providers.HostedSolution;
using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using SolidCP.Providers.OS;
using SolidCP.Providers.Common;

using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Xml;
using SolidCP.EnterpriseServer.Base.RDS;


namespace SolidCP.Providers.RemoteDesktopServices
{
    public class Windows2012 : HostingServiceProviderBase, IRemoteDesktopServices
    {
        #region Constants

        private const string CapPath = @"RDS:\GatewayServer\CAP";
        private const string RapPath = @"RDS:\GatewayServer\RAP";
        private const string Computers = "Computers";
        private const string AdDcComputers = "Domain Controllers";
        private const string Users = "users";
        private const string Admins = "Admins";        
        private const string RdsGroupFormat = "rds-{0}-{1}";
        private const string RdsModuleName = "RemoteDesktopServices";
        private const string AddNpsString = "netsh nps add np name=\"\"{0}\"\" policysource=\"1\" processingorder=\"{1}\" conditionid=\"0x3d\" conditiondata=\"^5$\" conditionid=\"0x1fb5\" conditiondata=\"{2}\" conditionid=\"0x1e\" conditiondata=\"UserAuthType:(PW|CA)\" profileid=\"0x1005\" profiledata=\"TRUE\" profileid=\"0x100f\" profiledata=\"TRUE\" profileid=\"0x1009\" profiledata=\"0x7\" profileid=\"0x1fe6\" profiledata=\"0x40000000\"";
        private const string SCPAdministratorsGroupDescription = "SCP RDS Collection Adminstrators";
        private const string RdsCollectionUsersGroupDescription = "SCP RDS Collection Users";
        private const string RdsCollectionComputersGroupDescription = "SCP RDS Collection Computers";
        private const string RdsServersOU = "RDSServersOU";
        private const string RdsServersRootOU = "RDSRootServersOU";
        private const string RDSHelpDeskComputerGroup = "SolidCP-RDSHelpDesk-Computer";        
        private const string RDSHelpDeskGroup = "SCP-HelpDeskAdministrators";
        private const string RDSHelpDeskGroupDescription = "SCP Help Desk Administrators";
        private const string LocalAdministratorsGroupName = "Administrators";
        private const string RDSHelpDeskRdRapPolicyName = "RDS-HelpDesk-RDRAP";
        private const string RDSHelpDeskRdCapPolicyName = "RDS-HelpDesk-RDCAP";
        private const string ScreenSaverGpoKey = @"HKCU\Software\Policies\Microsoft\Windows\Control Panel\Desktop";
        private const string ScreenSaverValueName = "ScreenSaveActive";
        private const string ScreenSaverTimeoutGpoKey = @"HKCU\Software\Policies\Microsoft\Windows\Control Panel\Desktop";
        private const string ScreenSaverTimeoutValueName = "ScreenSaveTimeout";
        private const string RemoveRestartGpoKey = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string RemoveRestartGpoValueName = "NoClose";
        private const string RemoveRunGpoKey = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string RemoveRunGpoValueName = "NoRun";
        private const string DisableTaskManagerGpoKey = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string DisableTaskManagerGpoValueName = "DisableTaskMgr";
        private const string HideCDriveGpoKey = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string HideCDriveGpoValueName = "NoDrives";
        private const string RDSSessionGpoKey = @"HKCU\Software\Policies\Microsoft\Windows NT\Terminal Services";
        private const string RDSSessionGpoValueName = "Shadow";
        private const string DisableCmdGpoKey = @"HKCU\Software\Policies\Microsoft\Windows\System";
        private const string DisableCmdGpoValueName = "DisableCMD";
        private const string DisallowRunParentKey = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string DisallowRunKey = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun";
        private const string DisallowRunValueName = "DisallowRun";

        #endregion

        #region Properties

        internal string PrimaryDomainController
        {
            get
            {
                return ProviderSettings["PrimaryDomainController"];
            }
        }

        private string RootOU
        {
            get
            {
                return ProviderSettings["RootOU"];
            }
        }

        private string ComputersRootOU
        {
            get
            {
                return ProviderSettings["ComputersRootOU"];
            }
        }

        private string CentralNpsHost
        {
            get
            {
                return ProviderSettings["CentralNPS"];
            }
        }

        private IEnumerable<string> Gateways
        {
            get
            {
                return ProviderSettings["GWServrsList"].Split(';').Select(x => string.IsNullOrEmpty(x) ? x : x.Trim());
            }
        }

        private bool CentralNps
        {
            get
            {
                return Convert.ToBoolean(ProviderSettings["UseCentralNPS"]);
            }
        }

        private string RootDomain
        {
            get
            {
                return ServerSettings.ADRootDomain;
            }
        }

        private string ConnectionBroker
        {
            get
            {
                return ProviderSettings["ConnectionBroker"];
            }
        }

        #endregion

        #region HostingServiceProvider methods

        public override bool IsInstalled()
        {            
            Server.Utils.OS.WindowsVersion version = SolidCP.Server.Utils.OS.GetVersion();
            return version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012 || version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012R2;
        }

        public override string[] Install()
        {            
            Runspace runSpace = null;
            PSObject feature = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                if (!IsFeatureInstalled("Desktop-Experience", runSpace))
                {
                    feature = AddFeature(runSpace, "Desktop-Experience", true, false);                    
                }

                if (!IsFeatureInstalled("NET-Framework-Core", runSpace))
                {
                    feature = AddFeature(runSpace, "NET-Framework-Core", true, false);
                }

                if (!IsFeatureInstalled("NET-Framework-45-Core", runSpace))
                {
                    feature = AddFeature(runSpace, "NET-Framework-45-Core", true, false);                    
                }
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return new string[]{};
        }

        public bool CheckRDSServerAvaliable(string hostname)
        {
            bool result = false;
            var ping = new Ping();
            var reply = ping.Send(hostname, 1000);

            if (reply.Status == IPStatus.Success)
            {
                result = true;
            }

            return result;
        }

        #endregion

        #region RDS Collections

        public void SendMessage(List<RdsMessageRecipient> recipients, string text)
        {
            new RdsMessenger().SendMessage(recipients, text, PrimaryDomainController);
        }

        public List<string> GetRdsCollectionSessionHosts(string collectionName)
        {
            var result = new List<string>();
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Get-RDSessionHost");
                cmd.Parameters.Add("CollectionName", collectionName);                
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);
                object[] errors;

                var hosts = runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors);

                foreach (var host in hosts)
                {
                    result.Add(RdsRunspaceExtensions.GetPSObjectProperty(host, "SessionHost").ToString());
                }
            }            
            finally
            {
                runspace.CloseRunspace();
            }

            return result;
        }

        public bool AddRdsServersToDeployment(RdsServer[] servers)
        {
            var result = true;
            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                foreach (var server in servers)
                {                    
                    if (!ExistRdsServerInDeployment(runSpace, server))
                    {
                        AddRdsServerToDeployment(runSpace, server);
                    }
                }
            }
            catch (AmbiguousMatchException)
            {
                result = false;
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return result;
        }

        public bool CreateCollection(string organizationId, RdsCollection collection)
        {            
            var result = true;

            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();
                Log.WriteWarning("Creating Collection");
                var existingServers = GetServersExistingInCollections(runSpace);
                existingServers = existingServers.Select(x => x.ToUpper()).Intersect(collection.Servers.Select(x => x.FqdName.ToUpper())).ToList();                

                if (existingServers.Any())
                {                                        
                    throw new Exception(string.Format("Server{0} {1} already added to another collection", existingServers.Count == 1 ? "" : "s", string.Join(" ,", existingServers.ToArray())));
                }                

                foreach (var server in collection.Servers)
                {
                    //If server will restart it will not be added to collection
                    //Do not install feature here                        

                    if (!ExistRdsServerInDeployment(runSpace, server))
                    {                        
                        AddRdsServerToDeployment(runSpace, server);                        
                    }
                }

                Log.WriteWarning("powershell: New-RDSessionCollection");
                Command cmd = new Command("New-RDSessionCollection");
                cmd.Parameters.Add("CollectionName", collection.Name);
                cmd.Parameters.Add("SessionHost", collection.Servers.Select(x => x.FqdName).ToArray());
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

                if (!string.IsNullOrEmpty(collection.Description))
                {
                    cmd.Parameters.Add("CollectionDescription", collection.Description);
                }

                var collectionPs = runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController).FirstOrDefault();                

                if (collectionPs == null)
                {
                    throw new Exception("Collection not created");
                }

                Log.WriteWarning("powershell: New-RDSessionCollection\tSuccessfully");

                EditRdsCollectionSettingsInternal(collection, runSpace);

                Log.WriteWarning("Settings edited");
                var orgPath = GetOrganizationPath(organizationId);
                Log.WriteWarning(string.Format("orgPath: {0}", orgPath));
                CheckOrCreateAdGroup(GetComputerGroupPath(organizationId, collection.Name), orgPath, GetComputersGroupName(collection.Name), RdsCollectionComputersGroupDescription);
                CheckOrCreateHelpDeskComputerGroup();
                string helpDeskGroupSamAccountName = CheckOrCreateAdGroup(GetHelpDeskGroupPath(RDSHelpDeskGroup), GetRootOUPath(), RDSHelpDeskGroup, RDSHelpDeskGroupDescription);
                string groupName = GetLocalAdminsGroupName(collection.Name);
                string groupPath = GetGroupPath(organizationId, collection.Name, groupName);
                string localAdminsGroupSamAccountName = CheckOrCreateAdGroup(groupPath, GetOrganizationPath(organizationId), groupName, SCPAdministratorsGroupDescription);
                CheckOrCreateAdGroup(GetUsersGroupPath(organizationId, collection.Name), orgPath, GetUsersGroupName(collection.Name), RdsCollectionUsersGroupDescription);                

                var capPolicyName = GetPolicyName(organizationId, collection.Name, RdsPolicyTypes.RdCap);
                var rapPolicyName = GetPolicyName(organizationId, collection.Name, RdsPolicyTypes.RdRap);

                foreach (var gateway in Gateways)
                {
                    CreateHelpDeskRdCapForce(runSpace, gateway);
                    CreateHelpDeskRdRapForce(runSpace, gateway);

                    if (!CentralNps)
                    {
                        CreateRdCapForce(runSpace, gateway, capPolicyName, collection.Name, new List<string> { GetUsersGroupName(collection.Name) });
                    }

                    CreateRdRapForce(runSpace, gateway, rapPolicyName, collection.Name, new List<string> { GetUsersGroupName(collection.Name) });
                }

                if (CentralNps)
                {
                    CreateCentralNpsPolicy(runSpace, CentralNpsHost, capPolicyName, collection.Name, organizationId);
                }

                //add user group to collection
                AddUserGroupsToCollection(runSpace, collection.Name, new List<string> { GetUsersGroupName(collection.Name) });                

                //add session servers to group
                foreach (var rdsServer in collection.Servers)
                {
                    AddComputerToCollectionAdComputerGroup(organizationId, collection.Name, rdsServer);
                    MoveSessionHostToCollectionOU(rdsServer.Name, collection.Name, organizationId);                    
                    AddAdGroupToLocalAdmins(runSpace, rdsServer.FqdName, helpDeskGroupSamAccountName);
                    AddAdGroupToLocalAdmins(runSpace, rdsServer.FqdName, localAdminsGroupSamAccountName);
                }

                string collectionComputersPath = GetComputerGroupPath(organizationId, collection.Name);
                CreatePolicy(runSpace, organizationId, string.Format("{0}-administrators", collection.Name),
                    new DirectoryEntry(GetGroupPath(organizationId, collection.Name, GetLocalAdminsGroupName(collection.Name))), new DirectoryEntry(collectionComputersPath), collection.Name);
                CreateUsersPolicy(runSpace, organizationId, string.Format("{0}-users", collection.Name), new DirectoryEntry(GetUsersGroupPath(organizationId, collection.Name))
                    , new DirectoryEntry(collectionComputersPath), collection.Name);
                CreateHelpDeskPolicy(runSpace, new DirectoryEntry(GetHelpDeskGroupPath(RDSHelpDeskGroup)), new DirectoryEntry(collectionComputersPath), organizationId, collection.Name);
            }                   
            finally
            {
                runSpace.CloseRunspace();
            }

            return result;
        }        

        public void EditRdsCollectionSettings(RdsCollection collection)
        {            
            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                if (collection.Settings != null)
                {
                    var errors = EditRdsCollectionSettingsInternal(collection, runSpace);

                    if (errors.Count > 0)
                    {
                        throw new Exception(string.Format("Settings not setted:\r\n{0}", string.Join("r\\n\\", errors.ToArray())));
                    }
                }
            }
            finally
            {
                runSpace.CloseRunspace();
            }
        }

        public List<RdsUserSession> GetRdsUserSessions(string collectionName)
        {
            Runspace runSpace = null;
            var result = new List<RdsUserSession>();

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();
                result = GetRdsUserSessionsInternal(collectionName, runSpace);                              
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return result;
        }

        public void LogOffRdsUser(string unifiedSessionId, string hostServer)
        {            
            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();
                object[] errors;
                Command cmd = new Command("Invoke-RDUserLogoff");
                cmd.Parameters.Add("HostServer", hostServer);
                cmd.Parameters.Add("UnifiedSessionID", unifiedSessionId);
                cmd.Parameters.Add("Force", true);

                runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors);

                if (errors != null && errors.Length > 0)
                {
                    throw new Exception(string.Join("r\\n\\", errors.Select(e => e.ToString()).ToArray()));
                }
            }            
            finally
            {
                runSpace.CloseRunspace();
            }
        }

        public List<string> GetServersExistingInCollections()
        {
            Runspace runSpace = null;
            List<string> existingServers = new List<string>();

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();
                existingServers = GetServersExistingInCollections(runSpace);
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return existingServers;
        }

        public RdsCollection GetCollection(string collectionName)
        {
            RdsCollection collection =null;

            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();
                collection = runSpace.GetCollection(collectionName, ConnectionBroker, PrimaryDomainController);
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return collection;
        }

        public bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            var result = true;

            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Remove-RDSessionCollection");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);
                cmd.Parameters.Add("Force", true);

                runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

                DeleteGpo(runSpace, string.Format("{0}-administrators", collectionName));
                DeleteGpo(runSpace, string.Format("{0}-users", collectionName));
                DeleteHelpDeskPolicy(runSpace, collectionName);
                var capPolicyName = GetPolicyName(organizationId, collectionName, RdsPolicyTypes.RdCap);
                var rapPolicyName = GetPolicyName(organizationId, collectionName, RdsPolicyTypes.RdRap);

                foreach (var gateway in Gateways)
                {
                    if (!CentralNps)
                    {
                        RemoveRdCap(runSpace, gateway, capPolicyName);
                    }

                    RemoveRdRap(runSpace, gateway, rapPolicyName);
                }

                if (CentralNps)
                {
                    RemoveNpsPolicy(runSpace, CentralNpsHost, capPolicyName);
                }

                foreach(var server in servers)
                {
                    RemoveGroupFromLocalAdmin(server.FqdName, server.Name, GetLocalAdminsGroupName(collectionName), runSpace);
                    RemoveComputerFromCollectionAdComputerGroup(organizationId, collectionName, server);
                    MoveRdsServerToTenantOU(server.Name, organizationId);
                }

                ActiveDirectoryUtils.DeleteADObject(GetComputerGroupPath(organizationId, collectionName));
                ActiveDirectoryUtils.DeleteADObject(GetUsersGroupPath(organizationId, collectionName));
                ActiveDirectoryUtils.DeleteADObject(GetGroupPath(organizationId, collectionName, GetLocalAdminsGroupName(collectionName)));
                ActiveDirectoryUtils.DeleteADObject(GetCollectionOUPath(organizationId, string.Format("{0}-OU", collectionName)));
            }
            catch (AmbiguousMatchException)
            {
                result = false;
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return result;
        }        

        public bool SetUsersInCollection(string organizationId, string collectionName, List<string> users)
        {
            var result = true;

            try
            {
                var usersGroupName = GetUsersGroupName(collectionName);
                var usersGroupPath = GetUsersGroupPath(organizationId, collectionName);
                SetUsersToCollectionAdGroup(collectionName, organizationId, users, usersGroupName, usersGroupPath);
            }
            catch (Exception e)
            {
                result = false;
                Log.WriteWarning(e.ToString());
            }

            return result;
        }

        public void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server)
        {
            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                if (!ExistRdsServerInDeployment(runSpace, server))
                {
                    AddRdsServerToDeployment(runSpace, server);
                }

                Command cmd = new Command("Add-RDSessionHost");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("SessionHost", server.FqdName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

                runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

                CheckOrCreateHelpDeskComputerGroup();

                foreach(var gateway in Gateways)
                {
                    CreateHelpDeskRdCapForce(runSpace, gateway);
                    CreateHelpDeskRdRapForce(runSpace, gateway);
                }

                string helpDeskGroupSamAccountName = CheckOrCreateAdGroup(GetHelpDeskGroupPath(RDSHelpDeskGroup), GetRootOUPath(), RDSHelpDeskGroup, RDSHelpDeskGroupDescription);
                string groupName = GetLocalAdminsGroupName(collectionName);
                string groupPath = GetGroupPath(organizationId, collectionName, groupName);
                string localAdminsGroupSamAccountName = CheckOrCreateAdGroup(groupPath, GetOrganizationPath(organizationId), groupName, SCPAdministratorsGroupDescription);

                AddAdGroupToLocalAdmins(runSpace, server.FqdName, LocalAdministratorsGroupName);
                AddAdGroupToLocalAdmins(runSpace, server.FqdName, helpDeskGroupSamAccountName);
                AddComputerToCollectionAdComputerGroup(organizationId, collectionName, server);
            }            
            finally
            {
                runSpace.CloseRunspace();
            }
        }

        public void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            foreach (var server in servers)
            {
                AddSessionHostServerToCollection(organizationId, collectionName, server);
            }
        }

        public void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId)
        {
            foreach(var server in servers)
            {
                MoveSessionHostToCollectionOU(server.Name, collectionName, organizationId);
            }
        }

        public void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server)
        {
            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Remove-RDSessionHost");
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);
                cmd.Parameters.Add("SessionHost", server.FqdName);
                cmd.Parameters.Add("Force", true);
                
                runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

                RemoveGroupFromLocalAdmin(server.FqdName, server.Name, GetLocalAdminsGroupName(collectionName), runSpace);
                RemoveComputerFromCollectionAdComputerGroup(organizationId, collectionName, server);
                MoveRdsServerToTenantOU(server.Name, organizationId);
            }
            finally
            {
                runSpace.CloseRunspace();
            }
        }

        public void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            foreach (var server in servers)
            {
                RemoveSessionHostServerFromCollection(organizationId, collectionName, server);
            }
        }

        #endregion

        public void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server)
        {
            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Set-RDSessionHost");
                cmd.Parameters.Add("SessionHost", server.FqdName);
                cmd.Parameters.Add("NewConnectionAllowed", newConnectionAllowed);

                runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);
            }
            catch (AmbiguousMatchException)
            {

            }
            finally
            {
                runSpace.CloseRunspace();
            }
        }

        #region Remote Applications

        public string[] GetApplicationUsers(string collectionName, string applicationName)
        {
            Runspace runspace = null;
            List<string> result = new List<string>();

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Get-RDRemoteApp");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

                if (!string.IsNullOrEmpty(applicationName))
                {
                    cmd.Parameters.Add("Alias", applicationName);
                }

                var application = runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController).FirstOrDefault();

                if (application != null)
                {
                    var users = (string[])(RdsRunspaceExtensions.GetPSObjectProperty(application, "UserGroups"));

                    if (users != null)
                    {
                        result.AddRange(users);
                    }
                }
            }
            finally
            {
                runspace.CloseRunspace();
            }

            return result.ToArray();
        }

        public bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users)
        {
            Runspace runspace = null;
            bool result = true;

            try
            {
                Log.WriteWarning(string.Format("App alias: {0}\r\nCollection Name:{2}\r\nUsers: {1}", remoteApp.Alias, string.Join("; ", users), collectionName));
                runspace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Set-RDRemoteApp");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);
                cmd.Parameters.Add("DisplayName", remoteApp.DisplayName);
                cmd.Parameters.Add("UserGroups", users);
                cmd.Parameters.Add("Alias", remoteApp.Alias);
                cmd.Parameters.Add("CommandLineSetting", remoteApp.CommandLineSettings.ToString());

                if (remoteApp.CommandLineSettings == CommandLineSettings.Require)
                {                    
                    cmd.Parameters.Add("RequiredCommandLine", remoteApp.RequiredCommandLine);
                }

                object[] errors;

                runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors).FirstOrDefault();

                if (errors.Any())
                {
                    Log.WriteWarning(string.Format("{0} adding users errors: {1}", remoteApp.DisplayName, string.Join("\r\n", errors.Select(e => e.ToString()).ToArray())));
                }
                else
                {
                    Log.WriteWarning(string.Format("{0} users added successfully", remoteApp.DisplayName));
                }
            }
            catch(Exception)
            {
                result = false;
            }
            finally
            {
                runspace.CloseRunspace();
            }

            return result;
        }

        public List<StartMenuApp> GetAvailableRemoteApplications(string collectionName)
        {
            var startApps = new List<StartMenuApp>();

            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Get-RDAvailableApp");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

                var remoteApplicationsPS = runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

                if (remoteApplicationsPS != null)
                {
                    startApps.AddRange(remoteApplicationsPS.Select(CreateStartMenuAppFromPsObject));
                }
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return startApps;
        }

        public List<RemoteApplication> GetCollectionRemoteApplications(string collectionName)
        {
            var remoteApps = new List<RemoteApplication>();

            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Get-RDRemoteApp");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

                var remoteAppsPs = runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

                if (remoteAppsPs != null)
                {
                    remoteApps.AddRange(remoteAppsPs.Select(CreateRemoteApplicationFromPsObject));
                }
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return remoteApps;
        }

        public bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            var result = false;

            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("New-RDRemoteApp");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);
                cmd.Parameters.Add("Alias", remoteApp.Alias);
                cmd.Parameters.Add("DisplayName", remoteApp.DisplayName);
                cmd.Parameters.Add("FilePath", remoteApp.FilePath);
                cmd.Parameters.Add("ShowInWebAccess", remoteApp.ShowInWebAccess);
                cmd.Parameters.Add("CommandLineSetting", remoteApp.CommandLineSettings.ToString());

                if (remoteApp.CommandLineSettings == CommandLineSettings.Require)
                {
                    cmd.Parameters.Add("RequiredCommandLine", remoteApp.RequiredCommandLine);
                }

                runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

                result = true;
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return result;
        }

        public bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps)
        {
            var result = true;

            foreach (var remoteApp in remoteApps)
            {
                result = AddRemoteApplication(collectionName, remoteApp) && result;
            }

            return result;
        }

        public bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            var result = false;

            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Remove-RDRemoteApp");
                cmd.Parameters.Add("CollectionName", collectionName);
                cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);
                cmd.Parameters.Add("Alias", remoteApp.Alias);
                cmd.Parameters.Add("Force", true);

                runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return result;
        }
        
        #endregion

        #region Gateaway (RD CAP | RD RAP)

        internal void CreateCentralNpsPolicy(Runspace runSpace, string centralNpshost, string policyName, string collectionName, string organizationId)
        {
            var showCmd = new Command("netsh nps show np");

            var showResult = runSpace.ExecuteRemoteShellCommand(centralNpshost, showCmd, PrimaryDomainController);
            var processingOrders = showResult.Where(x => Convert.ToString(x).ToLower().Contains("processing order")).Select(x => Convert.ToString(x));
            var count = 0;

            foreach(var processingOrder in processingOrders)
            {
                var order = Convert.ToInt32(processingOrder.Remove(0, processingOrder.LastIndexOf("=") + 1).Replace(" ", ""));

                if (order > count)
                {
                    count = order;
                }
            }

            var userGroupAd = ActiveDirectoryUtils.GetADObject(GetUsersGroupPath(organizationId, collectionName));
            var userGroupSid = (byte[])ActiveDirectoryUtils.GetADObjectProperty(userGroupAd, "objectSid");
            var addCmdString = string.Format(AddNpsString, policyName.Replace(" ", "_"), count + 1, ConvertByteToStringSid(userGroupSid));
            Command addCmd = new Command(addCmdString);

            var result = runSpace.ExecuteRemoteShellCommand(centralNpshost, addCmd, PrimaryDomainController);
        }

        internal void RemoveNpsPolicy(Runspace runSpace, string centralNpshost, string policyName)
        {
            var removeCmd = new Command(string.Format("netsh nps delete np {0}", policyName.Replace(" ", "_")));

            var removeResult = runSpace.ExecuteRemoteShellCommand(centralNpshost, removeCmd, PrimaryDomainController);
        }

        internal void CreateRdCapForce(Runspace runSpace, string gatewayHost, string policyName, string collectionName, List<string> groups)
        {
            //New-Item -Path "RDS:\GatewayServer\CAP" -Name "Allow Admins" -UserGroups "Administrators@." -AuthMethod 1
            //Set-Item -Path "RDS:\GatewayServer\CAP\Allow Admins\SessionTimeout" -Value 480 -SessionTimeoutAction 0

            if (ItemExistsRemote(runSpace, gatewayHost, Path.Combine(CapPath, policyName)))
            {
                RemoveRdCap(runSpace, gatewayHost, policyName);
            }

            var userGroupParametr = string.Format("@({0})",string.Join(",", groups.Select(x => string.Format("\"{0}@{1}\"", x, RootDomain)).ToArray()));

            Command rdCapCommand = new Command("New-Item");
            rdCapCommand.Parameters.Add("Path", string.Format("\"{0}\"", CapPath));
            rdCapCommand.Parameters.Add("Name", string.Format("\"{0}\"", policyName));
            rdCapCommand.Parameters.Add("UserGroups", userGroupParametr);
            rdCapCommand.Parameters.Add("AuthMethod", 1);

            runSpace.ExecuteRemoteShellCommand(gatewayHost, rdCapCommand, PrimaryDomainController, RdsModuleName);
        }

        private void CreateHelpDeskRdCapForce(Runspace runSpace, string gatewayHost)
        {                        
            if (ItemExistsRemote(runSpace, gatewayHost, Path.Combine(CapPath, RDSHelpDeskRdCapPolicyName)))
            {
                return;
            }

            var userGroupParameter = string.Format("@({0})", string.Format("\"{0}@{1}\"", RDSHelpDeskGroup, RootDomain));

            Command rdCapCommand = new Command("New-Item");
            rdCapCommand.Parameters.Add("Path", string.Format("\"{0}\"", CapPath));
            rdCapCommand.Parameters.Add("Name", string.Format("\"{0}\"", RDSHelpDeskRdCapPolicyName));
            rdCapCommand.Parameters.Add("UserGroups", userGroupParameter);
            rdCapCommand.Parameters.Add("AuthMethod", 1);

            runSpace.ExecuteRemoteShellCommand(gatewayHost, rdCapCommand, PrimaryDomainController, RdsModuleName);
        }

        private void CreateHelpDeskRdRapForce(Runspace runSpace, string gatewayHost)
        {
            if (ItemExistsRemote(runSpace, gatewayHost, Path.Combine(RapPath, RDSHelpDeskRdRapPolicyName)))
            {
                return;
            }

            var userGroupParameter = string.Format("@({0})", string.Format("\"{0}@{1}\"", RDSHelpDeskGroup, RootDomain));
            var computerGroupParameter = string.Format("\"{0}@{1}\"", RDSHelpDeskComputerGroup, RootDomain);

            Command rdRapCommand = new Command("New-Item");
            rdRapCommand.Parameters.Add("Path", string.Format("\"{0}\"", RapPath));
            rdRapCommand.Parameters.Add("Name", string.Format("\"{0}\"", RDSHelpDeskRdRapPolicyName));
            rdRapCommand.Parameters.Add("UserGroups", userGroupParameter);
            rdRapCommand.Parameters.Add("ComputerGroupType", 1);
            rdRapCommand.Parameters.Add("ComputerGroup", computerGroupParameter);

            object[] errors;

            for (int i = 0; i < 3; i++)
            {                
                runSpace.ExecuteRemoteShellCommand(gatewayHost, rdRapCommand, PrimaryDomainController, out errors, RdsModuleName);

                if (errors == null || !errors.Any())
                {
                    Log.WriteWarning("RD RAP Added Successfully");
                    break;
                }
                else
                {
                    Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                }
            }
        }

        internal void RemoveRdCap(Runspace runSpace, string gatewayHost, string name)
        {
            RemoveItemRemote(runSpace, gatewayHost, string.Format(@"{0}\{1}", CapPath, name), RdsModuleName);
        }

        internal void CreateRdRapForce(Runspace runSpace, string gatewayHost, string policyName, string collectionName, List<string> groups)
        {            
            //New-Item -Path "RDS:\GatewayServer\RAP" -Name "Allow Connections To Everywhere" -UserGroups "Administrators@." -ComputerGroupType 1
            //Set-Item -Path "RDS:\GatewayServer\RAP\Allow Connections To Everywhere\PortNumbers" -Value 3389,3390

            if (ItemExistsRemote(runSpace, gatewayHost, Path.Combine(RapPath, policyName)))
            {
                RemoveRdRap(runSpace, gatewayHost, policyName);
            }
            
            var userGroupParametr = string.Format("@({0})", string.Join(",", groups.Select(x => string.Format("\"{0}@{1}\"", x, RootDomain)).ToArray()));
            var computerGroupParametr = string.Format("\"{0}@{1}\"", GetComputersGroupName(collectionName), RootDomain);

            Command rdRapCommand = new Command("New-Item");
            rdRapCommand.Parameters.Add("Path", string.Format("\"{0}\"", RapPath));
            rdRapCommand.Parameters.Add("Name", string.Format("\"{0}\"", policyName));
            rdRapCommand.Parameters.Add("UserGroups", userGroupParametr);            
            rdRapCommand.Parameters.Add("ComputerGroupType", 1);
            rdRapCommand.Parameters.Add("ComputerGroup", computerGroupParametr);            

            object[] errors;

            for (int i = 0; i < 3; i++)
            {
                Log.WriteWarning(string.Format("Adding RD RAP ... {0}\r\nGateway Host\t{1}\r\nUser Group\t{2}\r\nComputer Group\t{3}", i + 1, gatewayHost, userGroupParametr, computerGroupParametr));
                runSpace.ExecuteRemoteShellCommand(gatewayHost, rdRapCommand, PrimaryDomainController, out errors, RdsModuleName);

                if (errors == null || !errors.Any())
                {
                    Log.WriteWarning("RD RAP Added Successfully");
                    break;
                }
                else
                {
                    Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                }
            }            
        }        

        internal void RemoveRdRap(Runspace runSpace, string gatewayHost, string name)
        {
            RemoveItemRemote(runSpace, gatewayHost, string.Format(@"{0}\{1}", RapPath, name), RdsModuleName);
        }

        #endregion

        #region Local Admins

        public void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string collectionName, string organizationId)
        {
            Runspace runspace = null;            

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();
                var index = ServerSettings.ADRootDomain.LastIndexOf(".");
                var domainName = ServerSettings.ADRootDomain;
                string groupName = GetLocalAdminsGroupName(collectionName);
                string groupPath = GetGroupPath(organizationId, collectionName, groupName);
                string helpDeskGroupSamAccountName = CheckOrCreateAdGroup(GetHelpDeskGroupPath(RDSHelpDeskGroup), GetRootOUPath(), RDSHelpDeskGroup, RDSHelpDeskGroupDescription);
                string localAdminsGroupSamAccountName = CheckOrCreateAdGroup(groupPath, GetOrganizationPath(organizationId), groupName, SCPAdministratorsGroupDescription);

                if (index > 0)
                {
                    domainName = ServerSettings.ADRootDomain.Substring(0, index);
                }

                foreach (var hostName in hosts)
                {                                                     
                    AddAdGroupToLocalAdmins(runspace, hostName, helpDeskGroupSamAccountName);
                    AddAdGroupToLocalAdmins(runspace, hostName, localAdminsGroupSamAccountName);

                    SetUsersToCollectionAdGroup(collectionName, organizationId, users, GetLocalAdminsGroupName(collectionName), groupPath);
                }                
            }
            finally
            {
                runspace.CloseRunspace();
            }                       
        }

        public List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName)
        {
            string groupName = GetLocalAdminsGroupName(collectionName);
            return GetUsersToCollectionAdGroup(collectionName, groupName, organizationId);
        }                     
        
        private void RemoveGroupFromLocalAdmin(string fqdnName, string hostName, string groupName, Runspace runspace)
        {            
            var scripts = new List<string>
            {
                string.Format("$LocalAdminsName = (Get-WMIObject -Class Win32_Group -Filter \"LocalAccount=TRUE and SID='S-1-5-32-544'\").name"),
                string.Format("$GroupObj = [ADSI]\"WinNT://{0}/$LocalAdminsName\"", hostName),
                string.Format("$GroupObj.Remove(\"WinNT://{0}/{1}\")", ServerSettings.ADRootDomain, RDSHelpDeskGroup),
                string.Format("$GroupObj.Remove(\"WinNT://{0}/{1}\")", ServerSettings.ADRootDomain, groupName)
            };

            object[] errors = null;
            runspace.ExecuteRemoteShellCommand(fqdnName, scripts, PrimaryDomainController, out errors);
        }
        
        #endregion

        #region GPO   
     
        public void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings)
        {
            string administratorsGpo = string.Format("{0}-administrators", collectionName);
            string usersGpo = string.Format("{0}-users", collectionName);
            Runspace runspace = null;            

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();                
                string collectionComputersPath = GetComputerGroupPath(organizationId, collectionName);

                CreatePolicy(runspace, organizationId, string.Format("{0}-administrators", collectionName),
                    new DirectoryEntry(GetGroupPath(organizationId, collectionName, GetLocalAdminsGroupName(collectionName))), new DirectoryEntry(collectionComputersPath), collectionName);
                CreateUsersPolicy(runspace, organizationId, string.Format("{0}-users", collectionName),
                    new DirectoryEntry(GetUsersGroupPath(organizationId, collectionName)), new DirectoryEntry(collectionComputersPath), collectionName);
                CreateHelpDeskPolicy(runspace, new DirectoryEntry(GetHelpDeskGroupPath(RDSHelpDeskGroup)), new DirectoryEntry(collectionComputersPath), organizationId, collectionName);                
                RemoveRegistryValue(runspace, ScreenSaverGpoKey, administratorsGpo);
                RemoveRegistryValue(runspace, ScreenSaverGpoKey, usersGpo);                
                RemoveRegistryValue(runspace, RemoveRestartGpoKey, administratorsGpo);
                RemoveRegistryValue(runspace, RemoveRestartGpoKey, usersGpo);
                RemoveRegistryValue(runspace, DisableTaskManagerGpoKey, administratorsGpo);
                RemoveRegistryValue(runspace, DisableTaskManagerGpoKey, usersGpo);
                RemoveRegistryValue(runspace, DisableCmdGpoKey, usersGpo);
                RemoveRegistryValue(runspace, DisableCmdGpoKey, administratorsGpo);
                RemoveRegistryValue(runspace, DisallowRunKey, usersGpo);
                RemoveRegistryValue(runspace, DisallowRunParentKey, usersGpo);
                RemoveRegistryValue(runspace, DisallowRunKey, administratorsGpo);
                RemoveRegistryValue(runspace, DisallowRunParentKey, administratorsGpo);

                var setting = serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.SCREEN_SAVER_DISABLED));
                SetRegistryValue(setting, runspace, ScreenSaverGpoKey, administratorsGpo, usersGpo, ScreenSaverValueName, "0", "string");

                setting = serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.REMOVE_SHUTDOWN_RESTART));
                SetRegistryValue(setting, runspace, RemoveRestartGpoKey, administratorsGpo, usersGpo, RemoveRestartGpoValueName, "1", "DWord");

                setting = serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.REMOVE_RUN_COMMAND));
                SetRegistryValue(setting, runspace, RemoveRunGpoKey, administratorsGpo, usersGpo, RemoveRunGpoValueName, "1", "DWord");

                setting = serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.DISABLE_TASK_MANAGER));
                SetRegistryValue(setting, runspace, DisableTaskManagerGpoKey, administratorsGpo, usersGpo, DisableTaskManagerGpoValueName, "1", "DWord");

                setting = serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.HIDE_C_DRIVE));
                SetRegistryValue(setting, runspace, HideCDriveGpoKey, administratorsGpo, usersGpo, HideCDriveGpoValueName, setting.PropertyValue, "DWord");

                setting = serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.DISABLE_CMD));
                SetRegistryValue(setting, runspace, DisableCmdGpoKey, administratorsGpo, usersGpo, DisableCmdGpoValueName, "1", "DWord");

                setting = serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.LOCK_SCREEN_TIMEOUT));
                double result;

                if (setting != null && !string.IsNullOrEmpty(setting.PropertyValue) && double.TryParse(setting.PropertyValue, out result))
                {                    
                    SetRegistryValue(setting, runspace, ScreenSaverTimeoutGpoKey, administratorsGpo, usersGpo, ScreenSaverTimeoutValueName, setting.PropertyValue, "string");                                    
                }

                SetRdsSessionHostPermissions(runspace, serverSettings, usersGpo, administratorsGpo);
                SetPowershellPermissions(runspace, serverSettings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.REMOVE_POWERSHELL_COMMAND)), usersGpo, administratorsGpo);
            }
            finally
            {
                runspace.CloseRunspace();
            }
        }

        private void CheckPolicySecurityFiltering(Runspace runspace, string gpoName, DirectoryEntry collectionComputersEntry)
        {
            var scripts = new List<string>{
                string.Format("Get-GPPermissions -Name {0} -TargetName {1} -TargetType group", gpoName, string.Format("'{0}'", ActiveDirectoryUtils.GetADObjectProperty(collectionComputersEntry, "sAMAccountName").ToString()))
            };

            object[] errors = null;
            runspace.ExecuteRemoteShellCommand(PrimaryDomainController, scripts, PrimaryDomainController, out errors);

            if (errors != null && errors.Any())
            {
                scripts = new List<string>{
                    string.Format("Set-GPPermissions -Name {0} -PermissionLevel gpoapply -TargetName {1} -TargetType group", gpoName, string.Format("'{0}'", ActiveDirectoryUtils.GetADObjectProperty(collectionComputersEntry, "sAMAccountName").ToString()))
                };
            }

            runspace.ExecuteRemoteShellCommand(PrimaryDomainController, scripts, PrimaryDomainController, out errors);
        }

        private void SetPowershellPermissions(Runspace runspace, RdsServerSetting setting, string usersGpo, string administratorsGpo)
        {
            if (setting != null)
            {
                SetRegistryValue(setting, runspace, DisallowRunParentKey, administratorsGpo, usersGpo, DisallowRunValueName, "1", "Dword");

                if (setting.ApplyAdministrators)
                {
                    SetRegistryValue(runspace, DisallowRunKey, administratorsGpo, "powershell.exe", "string");
                }

                if (setting.ApplyUsers)
                {
                    SetRegistryValue(runspace, DisallowRunKey, usersGpo, "powershell.exe", "string");
                }
            }
        }

        private void SetRdsSessionHostPermissions(Runspace runspace, RdsServerSettings settings, string usersGpo, string administratorsGpo)
        {
            var viewSetting = settings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION));
            var controlSetting = settings.Settings.FirstOrDefault(s => s.PropertyName.Equals(RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION));

            if (viewSetting == null || controlSetting == null)
            {
                return;
            }

            if (controlSetting.ApplyUsers)
            {
                SetRegistryValue(runspace, RDSSessionGpoKey, usersGpo, "2", RDSSessionGpoValueName, "DWord");
            }
            else if (viewSetting.ApplyUsers)
            {
                SetRegistryValue(runspace, RDSSessionGpoKey, usersGpo, "4", RDSSessionGpoValueName, "DWord");
            }
            else
            {
                SetRegistryValue(runspace, RDSSessionGpoKey, usersGpo, "3", RDSSessionGpoValueName, "DWord");
            }

            if (controlSetting.ApplyAdministrators)
            {
                SetRegistryValue(runspace, RDSSessionGpoKey, administratorsGpo, "2", RDSSessionGpoValueName, "DWord");
            }
            else if (viewSetting.ApplyAdministrators)
            {
                SetRegistryValue(runspace, RDSSessionGpoKey, administratorsGpo, "4", RDSSessionGpoValueName, "DWord");
            }
            else
            {
                SetRegistryValue(runspace, RDSSessionGpoKey, administratorsGpo, "3", RDSSessionGpoValueName, "DWord");
            }
        }
   
        private void RemoveRegistryValue(Runspace runspace, string key, string gpoName)
        {
            Command cmd = new Command("Remove-GPRegistryValue");
            cmd.Parameters.Add("Name", gpoName);
            cmd.Parameters.Add("Key", string.Format("\"{0}\"", key));

            Collection<PSObject> result = runspace.ExecuteRemoteShellCommand(PrimaryDomainController, cmd, PrimaryDomainController);
        }

        private void SetRegistryValue(RdsServerSetting setting, Runspace runspace, string key, string administratorsGpo, string usersGpo, string valueName, string value, string type)
        {
            if (setting == null)
            {
                return;
            }

            if (setting.ApplyAdministrators)
            {
                SetRegistryValue(runspace, key, administratorsGpo, value, valueName, type);
            }

            if (setting.ApplyUsers)
            {
                SetRegistryValue(runspace, key, usersGpo, value, valueName, type);
            }
        }

        private void SetRegistryValue(Runspace runspace, string key, string gpoName, string value, string type)
        {
            Command cmd = new Command("Set-GPRegistryValue");
            cmd.Parameters.Add("Name", gpoName);
            cmd.Parameters.Add("Key", string.Format("\"{0}\"", key));
            cmd.Parameters.Add("Value", value);            
            cmd.Parameters.Add("Type", type);

            Collection<PSObject> result = runspace.ExecuteRemoteShellCommand(PrimaryDomainController, cmd, PrimaryDomainController);
        }

        private void SetRegistryValue(Runspace runspace, string key, string gpoName, string value, string valueName, string type)
        {
            Command cmd = new Command("Set-GPRegistryValue");
            cmd.Parameters.Add("Name", gpoName);
            cmd.Parameters.Add("Key", string.Format("\"{0}\"", key));
            cmd.Parameters.Add("Value", value);
            cmd.Parameters.Add("ValueName", valueName);
            cmd.Parameters.Add("Type", type);

            Collection<PSObject> result = runspace.ExecuteRemoteShellCommand(PrimaryDomainController, cmd, PrimaryDomainController);
        }

        private void CreateHelpDeskPolicy(Runspace runspace, DirectoryEntry entry, DirectoryEntry collectionComputersEntry, string organizationId, string collectionName)
        {
            string gpoName = string.Format("{0}-HelpDesk", collectionName);
            string gpoId = GetPolicyId(runspace, gpoName);

            if (string.IsNullOrEmpty(gpoId))
            {
                gpoId = CreateAndLinkPolicy(runspace, gpoName, organizationId, collectionName);
                SetPolicyPermissions(runspace, gpoName, entry, collectionComputersEntry);
                SetRegistryValue(runspace, RDSSessionGpoKey, gpoName, "2", RDSSessionGpoValueName, "DWord");
            }
            else
            {
                CheckPolicySecurityFiltering(runspace, gpoName, collectionComputersEntry);
            }
        }

        private string CreateUsersPolicy(Runspace runspace, string organizationId, string gpoName, DirectoryEntry entry, DirectoryEntry collectionComputersEntry, string collectionName)
        {
            string gpoId = CreatePolicy(runspace, organizationId, gpoName, entry, collectionComputersEntry, collectionName);
            ExcludeAdminsFromUsersPolicy(runspace, gpoId, collectionName);
            return gpoId;
        }

        private string CreatePolicy(Runspace runspace, string organizationId, string gpoName, DirectoryEntry entry, DirectoryEntry collectionComputersEntry, string collectionName)
        {
            string gpoId = GetPolicyId(runspace, gpoName);

            if (string.IsNullOrEmpty(gpoId))
            {
                gpoId = CreateAndLinkPolicy(runspace, gpoName, organizationId, collectionName);
                SetPolicyPermissions(runspace, gpoName, entry, collectionComputersEntry);
            }
            else
            {
                CheckPolicySecurityFiltering(runspace, gpoName, collectionComputersEntry);
            }

            return gpoId;
        }

        private void DeleteHelpDeskPolicy(Runspace runspace, string collectionName)
        {
            string gpoName = string.Format("{0}-HelpDesk", collectionName);
            DeleteGpo(runspace, gpoName);
        }

        private void DeleteGpo(Runspace runspace, string gpoName)
        {
            Command cmd = new Command("Remove-GPO");
            cmd.Parameters.Add("Name", gpoName);

            Collection<PSObject> result = runspace.ExecuteRemoteShellCommand(PrimaryDomainController, cmd, PrimaryDomainController);
        }

        private void ExcludeAdminsFromUsersPolicy(Runspace runspace, string gpoId, string collectionName)
        {
            var scripts = new List<string>
            {
                string.Format("$adgpo = [ADSI]\"{0}\"", GetGpoPath(gpoId)),
                string.Format("$rule = New-Object System.DirectoryServices.ActiveDirectoryAccessRule([System.Security.Principal.NTAccount]\"{0}\\{1}\",\"ExtendedRight\",\"Deny\",[GUID]\"edacfd8f-ffb3-11d1-b41d-00a0c968f939\")",
                    RootDomain.Split('.').First(), GetLocalAdminsGroupName(collectionName)),
                string.Format("$acl = $adgpo.ObjectSecurity"),
                string.Format("$acl.AddAccessRule($rule)"),
                string.Format("$adgpo.CommitChanges()")
            };

            object[] errors = null;
            runspace.ExecuteRemoteShellCommand(PrimaryDomainController, scripts, PrimaryDomainController, out errors);
        }

        private void SetPolicyPermissions(Runspace runspace, string gpoName, DirectoryEntry entry, DirectoryEntry collectionComputersEntry)
        {
            var scripts = new List<string>
            {
                string.Format("Set-GPPermissions -Name {0} -Replace -PermissionLevel None -TargetName 'Authenticated Users' -TargetType group", gpoName),
                string.Format("Set-GPPermissions -Name {0} -PermissionLevel gpoapply -TargetName {1} -TargetType group", gpoName, string.Format("'{0}'", ActiveDirectoryUtils.GetADObjectProperty(entry, "sAMAccountName").ToString())),
                string.Format("Set-GPPermissions -Name {0} -PermissionLevel gpoapply -TargetName {1} -TargetType group", gpoName, string.Format("'{0}'", ActiveDirectoryUtils.GetADObjectProperty(collectionComputersEntry, "sAMAccountName").ToString()))
            };

            object[] errors = null;
            runspace.ExecuteRemoteShellCommand(PrimaryDomainController, scripts, PrimaryDomainController, out errors);
        }

        private string CreateAndLinkPolicy(Runspace runspace, string gpoName, string organizationId, string collectionName)
        {
            string gpoId = null;

            try
            {
                var entry = new DirectoryEntry(GetOrganizationPath(organizationId));
                var distinguishedName = string.Format("\"{0}\"", ActiveDirectoryUtils.GetADObjectProperty(entry, "DistinguishedName"));

                Command cmd = new Command("New-GPO");
                cmd.Parameters.Add("Name", gpoName);

                Collection<PSObject> result = runspace.ExecuteRemoteShellCommand(PrimaryDomainController, cmd, PrimaryDomainController);

                if (result != null && result.Count > 0)
                {
                    PSObject gpo = result[0];
                    gpoId = ((Guid)RdsRunspaceExtensions.GetPSObjectProperty(gpo, "Id")).ToString("B");

                }

                cmd = new Command("New-GPLink");
                cmd.Parameters.Add("Name", gpoName);
                cmd.Parameters.Add("Target", distinguishedName);

                runspace.ExecuteRemoteShellCommand(PrimaryDomainController, cmd, PrimaryDomainController);
            }
            catch (Exception)
            {
                gpoId = null;
                throw;
            }

            return gpoId;
        }

        private string GetPolicyId(Runspace runspace, string gpoName)
        {
            string gpoId = null;

            try
            {
                Command cmd = new Command("Get-GPO");
                cmd.Parameters.Add("Name", gpoName);

                Collection<PSObject> result = runspace.ExecuteRemoteShellCommand(PrimaryDomainController, cmd, PrimaryDomainController);

                if (result != null && result.Count > 0)
                {
                    PSObject gpo = result[0];
                    gpoId = ((Guid)RdsRunspaceExtensions.GetPSObjectProperty(gpo, "Id")).ToString("B");
                }
            }
            catch (Exception)
            {
                gpoId = null;

                throw;
            }

            return gpoId;
        }

        #endregion

        #region Shadow Session

        public void ShadowSession(string sessionId, string fqdName, bool control)
        {
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();

                var scripts = new List<string>
                {
                    string.Format("mstsc /Shadow:{0}{1} /v:{2}", sessionId, control ? " /Control" : "", fqdName)
                };

                object[] errors = null;
                runspace.ExecuteShellCommand(scripts, out errors);
            }
            finally
            {
                runspace.CloseRunspace();
            }
        }

        #endregion

        #region RDS Help Desk

        private string GetHelpDeskGroupPath(string groupName)
        {
            StringBuilder sb = new StringBuilder();

            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, groupName);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }                

        private void CheckOrCreateHelpDeskComputerGroup()
        {
            if (!ActiveDirectoryUtils.AdObjectExists(GetHelpDeskGroupPath(RDSHelpDeskComputerGroup)))
            {
                ActiveDirectoryUtils.CreateGroup(GetRootOUPath(), RDSHelpDeskComputerGroup);
            }
        }

        private string CheckOrCreateAdGroup(string groupPath, string rootPath, string groupName, string description)
        {            
            DirectoryEntry groupEntry = null;

            if (!ActiveDirectoryUtils.AdObjectExists(groupPath))
            {
                ActiveDirectoryUtils.CreateGroup(rootPath, groupName);
                groupEntry = ActiveDirectoryUtils.GetADObject(groupPath);

                if (groupEntry.Properties.Contains("Description"))
                {
                    groupEntry.Properties["Description"][0] = description;
                }
                else
                {
                    groupEntry.Properties["Description"].Add(description);
                }

                groupEntry.CommitChanges();
            }

            if (groupEntry == null)
            {
                groupEntry = ActiveDirectoryUtils.GetADObject(groupPath);
            }

            return ActiveDirectoryUtils.GetADObjectProperty(groupEntry, "sAMAccountName").ToString();
        }

        private void AddAdGroupToLocalAdmins(Runspace runspace, string hostName, string samAccountName)
        {                                    
            var scripts = new List<string>
            {
                string.Format("$LocalAdminsName = (Get-WMIObject -Class Win32_Group -Filter \"LocalAccount=TRUE and SID='S-1-5-32-544'\").name"),
                string.Format("$GroupObj = [ADSI]\"WinNT://{0}/$LocalAdminsName\"", hostName),
                string.Format("$GroupObj.Add(\"WinNT://{0}/{1}\")", ServerSettings.ADRootDomain, samAccountName)
            };
            
            object[] errors = null;
            runspace.ExecuteRemoteShellCommand(hostName, scripts, PrimaryDomainController, out errors);                 
        }

        #endregion

        #region SSL

        public void InstallCertificate(byte[] certificate, string password, List<string> hostNames)
        {
            Runspace runspace = null;

            try
            {
                var guid = Guid.NewGuid();                
                var x509Cert = new X509Certificate2(certificate, password, X509KeyStorageFlags.Exportable);                                
                var filePath = SaveCertificate(certificate, guid);
                runspace = RdsRunspaceExtensions.OpenRunspace();

                foreach (var hostName in hostNames)
                {                    
                    var destinationPath = string.Format("\\\\{0}\\c$\\{1}.pfx", hostName, guid);
                    var errors = CopyCertificateFile(runspace, filePath, destinationPath);

                    if (!errors.Any())
                    {
                        RemoveCertificate(runspace, hostName, x509Cert.Thumbprint);                        
                        errors = ImportCertificate(runspace, hostName, password, string.Format("c:\\{0}.pfx", guid), x509Cert.Thumbprint);
                    }

                    DeleteCertificateFile(destinationPath, runspace);

                    if (errors.Any())
                    {
                        Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                        throw new Exception(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                    }
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            finally
            {
                runspace.CloseRunspace();
            }
        }  
 
        private void RemoveCertificate(Runspace runspace, string hostName, string thumbprint)
        {
            var scripts = new List<string>
            {
                string.Format("Remove-Item -Path cert:\\LocalMachine\\My\\{0}", thumbprint)
            };

            object[] errors = null;
            runspace.ExecuteRemoteShellCommand(hostName, scripts, PrimaryDomainController, out errors);
        }

        private object[] ImportCertificate(Runspace runspace, string hostName, string password, string certificatePath, string thumbprint)
        {
            var scripts = new List<string>
            {                
                string.Format("$mypwd = ConvertTo-SecureString -String {0} -Force –AsPlainText", password),
                string.Format("Import-PfxCertificate –FilePath \"{0}\" cert:\\localMachine\\my -Password $mypwd", certificatePath),
                string.Format("$cert = Get-Item cert:\\LocalMachine\\My\\{0}", thumbprint),
                string.Format("$path = (Get-WmiObject -class \"Win32_TSGeneralSetting\" -Namespace root\\cimv2\\terminalservices -Filter \"TerminalName='RDP-tcp'\").__path"),
                string.Format("Set-WmiInstance -Path $path -argument @{0}", string.Format("{{SSLCertificateSHA1Hash=\"{0}\"}}", thumbprint))
            };

            object[] errors = null;
            runspace.ExecuteRemoteShellCommand(hostName, scripts, PrimaryDomainController, out errors);

            return errors;
        }

        private string SaveCertificate(byte[] certificate, Guid guid)
        {
            var filePath = string.Format("{0}{1}.pfx", Path.GetTempPath(), guid);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllBytes(filePath, certificate);

            return filePath;
        }
     
        private object[] CopyCertificateFile(Runspace runspace, string filePath, string destinationPath)
        {                                  
            var scripts = new List<string>
            {
                string.Format("Copy-Item \"{0}\" -Destination \"{1}\" -Force", filePath, destinationPath)
            };

            object[] errors = null;
            runspace.ExecuteShellCommand(scripts, out errors);            

            return errors;
        }

        private object[] DeleteCertificateFile(string destinationPath, Runspace runspace)
        {
            var scripts = new List<string>
            {
                string.Format("Remove-Item -Path \"{0}\" -Force", destinationPath)
            };

            object[] errors = null;
            runspace.ExecuteShellCommand(scripts, out errors);

            return errors;
        }

        #endregion

        #region Import Collection

        public ImportedRdsCollection GetExistingCollection(string collectionName)
        {
            Runspace runspace = null;
            ImportedRdsCollection result;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();
                var collection = runspace.GetCollection(collectionName, ConnectionBroker, PrimaryDomainController);
                result = new ImportedRdsCollection
                {
                    CollectionName = collection.Name,
                    Description = collection.Description
                };
                
                if (collection == null)
                {
                    throw new NullReferenceException(string.Format("Collection \"{0}\" not found", collectionName));
                }

                object[] errors;
                result.CollectionSettings = runspace.GetCollectionSettings(collectionName, ConnectionBroker, PrimaryDomainController, out errors).ToList();

                if (errors.Any())
                {
                    throw new Exception(string.Format("Collection not imported:\r\n{0}", string.Join("r\\n\\", errors.Select(e => e.ToString()).ToArray())));
                }

                result.SessionHosts = runspace.GetSessionHosts(collectionName, ConnectionBroker, PrimaryDomainController, out errors);

                if (errors.Any())
                {
                    throw new Exception(string.Format("Collection not imported:\r\n{0}", string.Join("r\\n\\", errors.Select(e => e.ToString()).ToArray())));
                }

                result.UserGroups = runspace.GetCollectionUserGroups(collectionName, ConnectionBroker, PrimaryDomainController, out errors).ToList();

                if (errors.Any())
                {
                    throw new Exception(string.Format("Collection not imported:\r\n{0}", string.Join("r\\n\\", errors.Select(e => e.ToString()).ToArray())));
                }
            }
            finally
            {
                runspace.CloseRunspace();
            }

            return result;
        }

        public void ImportCollection(string organizationId, RdsCollection collection, List<string> users)
        {            
            Runspace runSpace = null;

            try
            {
                Log.WriteStart(string.Format("Starting collection import: {0}", collection.Name));
                runSpace = RdsRunspaceExtensions.OpenRunspace();                                                
                var orgPath = GetOrganizationPath(organizationId);
                CheckOrCreateAdGroup(GetComputerGroupPath(organizationId, collection.Name), orgPath, GetComputersGroupName(collection.Name), RdsCollectionComputersGroupDescription);
                CheckOrCreateHelpDeskComputerGroup();
                string helpDeskGroupSamAccountName = CheckOrCreateAdGroup(GetHelpDeskGroupPath(RDSHelpDeskGroup), GetRootOUPath(), RDSHelpDeskGroup, RDSHelpDeskGroupDescription);
                string groupName = GetLocalAdminsGroupName(collection.Name);
                string groupPath = GetGroupPath(organizationId, collection.Name, groupName);
                string localAdminsGroupSamAccountName = CheckOrCreateAdGroup(groupPath, GetOrganizationPath(organizationId), groupName, SCPAdministratorsGroupDescription);
                CheckOrCreateAdGroup(GetUsersGroupPath(organizationId, collection.Name), orgPath, GetUsersGroupName(collection.Name), RdsCollectionUsersGroupDescription);

                var capPolicyName = GetPolicyName(organizationId, collection.Name, RdsPolicyTypes.RdCap);
                var rapPolicyName = GetPolicyName(organizationId, collection.Name, RdsPolicyTypes.RdRap);

                foreach (var gateway in Gateways)
                {
                    CreateHelpDeskRdCapForce(runSpace, gateway);
                    CreateHelpDeskRdRapForce(runSpace, gateway);

                    if (!CentralNps)
                    {
                        CreateRdCapForce(runSpace, gateway, capPolicyName, collection.Name, new List<string> { GetUsersGroupName(collection.Name) });
                    }

                    CreateRdRapForce(runSpace, gateway, rapPolicyName, collection.Name, new List<string> { GetUsersGroupName(collection.Name) });
                }

                if (CentralNps)
                {
                    CreateCentralNpsPolicy(runSpace, CentralNpsHost, capPolicyName, collection.Name, organizationId);
                }

                //add user group to collection
                AddUserGroupsToCollection(runSpace, collection.Name, new List<string> { GetUsersGroupName(collection.Name) });

                //add session servers to group
                foreach (var rdsServer in collection.Servers)
                {
                    MoveSessionHostToCollectionOU(rdsServer.Name, collection.Name, organizationId);
                    AddAdGroupToLocalAdmins(runSpace, rdsServer.FqdName, helpDeskGroupSamAccountName);
                    AddAdGroupToLocalAdmins(runSpace, rdsServer.FqdName, localAdminsGroupSamAccountName);

                    Log.WriteStart(string.Format("Adding server {0} to AD Group, OrgId: {1}", rdsServer.Name, organizationId));
                    AddComputerToCollectionAdComputerGroup(organizationId, collection.Name, rdsServer);
                    Log.WriteStart(string.Format("Server {0} added to AD Group, OrgId: {1}", rdsServer.Name, organizationId));
                }

                string collectionComputersPath = GetComputerGroupPath(organizationId, collection.Name);
                CreatePolicy(runSpace, organizationId, string.Format("{0}-administrators", collection.Name),
                    new DirectoryEntry(GetGroupPath(organizationId, collection.Name, GetLocalAdminsGroupName(collection.Name))), new DirectoryEntry(collectionComputersPath), collection.Name);
                CreateUsersPolicy(runSpace, organizationId, string.Format("{0}-users", collection.Name), new DirectoryEntry(GetUsersGroupPath(organizationId, collection.Name))
                    , new DirectoryEntry(collectionComputersPath), collection.Name);
                CreateHelpDeskPolicy(runSpace, new DirectoryEntry(GetHelpDeskGroupPath(RDSHelpDeskGroup)), new DirectoryEntry(collectionComputersPath), organizationId, collection.Name);

                SetUsersInCollection(organizationId, collection.Name, users);

                Log.WriteStart(string.Format("Collection imported: {0}", collection.Name));
            }
            finally
            {
                runSpace.CloseRunspace();
            }
        }

        #endregion

        private void AddRdsServerToDeployment(Runspace runSpace, RdsServer server)
        {
            Command cmd = new Command("Add-RDserver");
            cmd.Parameters.Add("Server", server.FqdName);
            cmd.Parameters.Add("Role", "RDS-RD-SERVER");
            cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

            runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);
        }   

        private bool ExistRdsServerInDeployment(Runspace runSpace, RdsServer server)
        {
            Command cmd = new Command("Get-RDserver");
            cmd.Parameters.Add("Role", "RDS-RD-SERVER");
            cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

            var serversPs = runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

            if (serversPs != null)
            {
                foreach (var serverPs in serversPs)
                {
                    var serverName = Convert.ToString(RdsRunspaceExtensions.GetPSObjectProperty(serverPs, "Server"));

                    if (string.Compare(serverName, server.FqdName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetUsersToCollectionAdGroup(string collectionName, string organizationId, IEnumerable<string> users, string groupName, string groupPath)
        {            
            var orgPath = GetOrganizationPath(organizationId);
            var orgEntry = ActiveDirectoryUtils.GetADObject(orgPath);
            var groupUsers = ActiveDirectoryUtils.GetGroupObjects(groupName, "user", orgEntry);
            
            foreach (string userPath in groupUsers)
            {
                ActiveDirectoryUtils.RemoveObjectFromGroup(userPath, groupPath);                
            }          
            
            foreach (var user in users)
            {                
                var userPath = GetUserPath(organizationId, user);                

                if (ActiveDirectoryUtils.AdObjectExists(userPath))
                {                    
                    var userObject = ActiveDirectoryUtils.GetADObject(userPath);
                    var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(userObject, "sAMAccountName");                                        
                    ActiveDirectoryUtils.AddObjectToGroup(userPath, groupPath);                    
                }                
            }
        }

        private List<string> GetUsersToCollectionAdGroup(string collectionName, string groupName, string organizationId)
        {
            var users = new List<string>();    
            var orgPath = GetOrganizationPath(organizationId);
            var orgEntry = ActiveDirectoryUtils.GetADObject(orgPath);

            foreach (string userPath in ActiveDirectoryUtils.GetGroupObjects(groupName, "user", orgEntry))
            {
                var userObject = ActiveDirectoryUtils.GetADObject(userPath);
                var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(userObject, "sAMAccountName");

                users.Add(samName);
            }

            return users;
        }        

        private void AddUserGroupsToCollection(Runspace runSpace, string collectionName, List<string> groups)
        {
            Command cmd = new Command("Set-RDSessionCollectionConfiguration");
            cmd.Parameters.Add("CollectionName", collectionName);
            cmd.Parameters.Add("UserGroup", groups);
            cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

            runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController).FirstOrDefault();
        }

        private void AddComputerToCollectionAdComputerGroup(string organizationId, string collectionName, RdsServer server)
        {            
            var computerGroupName = GetComputersGroupName( collectionName);            
            var computerObject = GetComputerObject(server.Name);

            Log.WriteWarning(string.Format("ComputerGroupName: {0}\r\nServerName: {1}\r\nCollectionName: {2}", computerGroupName, server.Name, collectionName));

            if (computerObject != null)
            {                
                var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(computerObject, "sAMAccountName");
                Log.WriteWarning(string.Format("sAMAccountName: {0}\r\nPath: {1}\r\n", samName, computerObject.Path));

                if (!ActiveDirectoryUtils.IsComputerInGroup(samName, computerGroupName))
                {
                    string groupPath = GetComputerGroupPath(organizationId, collectionName);
                    Log.WriteWarning(string.Format("ComputerGroupPath: {0}", groupPath));
                    ActiveDirectoryUtils.AddObjectToGroup(computerObject.Path, groupPath);
                    Log.WriteWarning(string.Format("{0} added to {1}", computerObject.Path, groupPath));
                }

                if (!ActiveDirectoryUtils.IsComputerInGroup(samName, RDSHelpDeskComputerGroup))
                {
                    string helpDeskGroupPath = GetHelpDeskGroupPath(RDSHelpDeskComputerGroup);
                    Log.WriteWarning(string.Format("HelpDeskGroupPath: {0}", helpDeskGroupPath));
                    ActiveDirectoryUtils.AddObjectToGroup(computerObject.Path, helpDeskGroupPath);
                    Log.WriteWarning(string.Format("{0} added to {1}", computerObject.Path, helpDeskGroupPath));
                }
            }
            else
            {
                Log.WriteWarning(string.Format("ComputerObject IS NULL"));
            }

            SetRDServerNewConnectionAllowed("yes", server);
        }

        private void RemoveComputerFromCollectionAdComputerGroup(string organizationId, string collectionName, RdsServer server)
        {            
            var computerGroupName = GetComputersGroupName(collectionName);
            var computerObject = GetComputerObject(server.Name);

            if (computerObject != null)
            {                
                var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(computerObject, "sAMAccountName");

                if (ActiveDirectoryUtils.IsComputerInGroup(samName, computerGroupName))
                {
                    ActiveDirectoryUtils.RemoveObjectFromGroup(computerObject.Path, GetComputerGroupPath(organizationId, collectionName));
                }

                if (ActiveDirectoryUtils.AdObjectExists(GetHelpDeskGroupPath(RDSHelpDeskComputerGroup)))
                {
                    if (ActiveDirectoryUtils.IsComputerInGroup(samName, RDSHelpDeskComputerGroup))
                    {
                        ActiveDirectoryUtils.RemoveObjectFromGroup(computerObject.Path, GetHelpDeskGroupPath(RDSHelpDeskComputerGroup));
                    }
                }
            }
        }

        public bool AddSessionHostFeatureToServer(string hostName)
        {
            bool installationResult = false;
            Runspace runSpace = null;

            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();
                var feature = AddFeature(runSpace, hostName, "RDS-RD-Server", true, true);
                installationResult = (bool)RdsRunspaceExtensions.GetPSObjectProperty(feature, "Success");

                if (!IsFeatureInstalled(hostName, "Desktop-Experience", runSpace))
                {
                    feature = AddFeature(runSpace, hostName, "Desktop-Experience", true, false);
                }

                if (!IsFeatureInstalled(hostName, "NET-Framework-Core", runSpace))
                {
                    feature = AddFeature(runSpace, hostName, "NET-Framework-Core", true, false);
                }
            }            
            finally
            {
                runSpace.CloseRunspace();
            }

            return installationResult;
        }

        private void CheckOrCreateComputersRoot(string computersRootPath)
        {
            if (ActiveDirectoryUtils.AdObjectExists(computersRootPath) && !ActiveDirectoryUtils.AdObjectExists(GetRdsServersGroupPath()))
            {                
                //ActiveDirectoryUtils.CreateGroup(computersRootPath, RdsServersRootOU);
                ActiveDirectoryUtils.CreateOrganizationalUnit(RdsServersRootOU, computersRootPath);
            }
        }

        public void MoveSessionHostToRdsOU(string hostName)
        {
            if (!string.IsNullOrEmpty(ComputersRootOU))
            {
                CheckOrCreateComputersRoot(GetComputersRootPath());
            }

            var computerObject = GetComputerObject(hostName);

            if (computerObject != null)
            {
                var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(computerObject, "sAMAccountName");

                if (!ActiveDirectoryUtils.IsComputerInGroup(samName, RdsServersRootOU))
                {
                    DirectoryEntry group = new DirectoryEntry(GetRdsServersGroupPath());
                    computerObject.MoveTo(group);
                    group.CommitChanges();
                }
            } 
        }

        public void MoveSessionHostToCollectionOU(string hostName, string collectionName, string organizationId)
        {
            if (!string.IsNullOrEmpty(ComputersRootOU))
            {
                CheckOrCreateComputersRoot(GetComputersRootPath());
            }

            var computerObject = GetComputerObject(hostName);
            string collectionOUName = string.Format("{0}-OU", collectionName);
            string collectionOUPath = GetCollectionOUPath(organizationId, collectionOUName);

            if (!ActiveDirectoryUtils.AdObjectExists(collectionOUPath))
            {
                ActiveDirectoryUtils.CreateOrganizationalUnit(collectionOUName, GetOrganizationPath(organizationId));
            }

            if (computerObject != null)
            {
                var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(computerObject, "sAMAccountName");

                if (!ActiveDirectoryUtils.IsComputerInGroup(samName, collectionOUName))
                {
                    DirectoryEntry group = new DirectoryEntry(collectionOUPath);
                    computerObject.MoveTo(group);
                    group.CommitChanges();
                }
            }
        }

        public void MoveRdsServerToTenantOU(string hostName, string organizationId)
        {
            var tenantComputerGroupPath = GetTenantComputerGroupPath(organizationId);

            if (!ActiveDirectoryUtils.AdObjectExists(tenantComputerGroupPath))
            {
                ActiveDirectoryUtils.CreateOrganizationalUnit(RdsServersOU, GetOrganizationPath(organizationId));
            }

            hostName = hostName.ToLower().Replace(string.Format(".{0}", ServerSettings.ADRootDomain.ToLower()), "");            
            var rootComputerPath = GetRdsServerPath(hostName);
            var tenantComputerPath = GetTenantComputerPath(hostName, organizationId);            

            if (!string.IsNullOrEmpty(ComputersRootOU))
            {                
                CheckOrCreateComputersRoot(GetComputersRootPath());
            }            
            
            var computerObject = GetComputerObject(hostName);

            if (computerObject != null)
            {
                var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(computerObject, "sAMAccountName");

                if (!ActiveDirectoryUtils.IsComputerInGroup(samName, RdsServersOU))
                {
                    DirectoryEntry group = new DirectoryEntry(tenantComputerGroupPath);
                    computerObject.MoveTo(group);
                    group.CommitChanges();
                }
            } 
        }

        public void RemoveRdsServerFromTenantOU(string hostName, string organizationId)
        {
            var tenantComputerGroupPath = GetTenantComputerGroupPath(organizationId);
            hostName = hostName.ToLower().Replace(string.Format(".{0}", ServerSettings.ADRootDomain.ToLower()), "");                                    

            if (!string.IsNullOrEmpty(ComputersRootOU))
            {
                CheckOrCreateComputersRoot(GetComputersRootPath());
            }            

            if (!ActiveDirectoryUtils.AdObjectExists(tenantComputerGroupPath))
            {
                ActiveDirectoryUtils.CreateOrganizationalUnit(RdsServersOU, GetOrganizationPath(organizationId));
            }
            
            var computerObject = GetComputerObject(hostName);
            
            if (computerObject != null)
            {
                var samName = (string)ActiveDirectoryUtils.GetADObjectProperty(computerObject, "sAMAccountName");
                
                if (ActiveDirectoryUtils.AdObjectExists(GetComputersRootPath()) && !string.IsNullOrEmpty(ComputersRootOU) && !ActiveDirectoryUtils.IsComputerInGroup(samName, RdsServersRootOU))
                {
                    DirectoryEntry group = new DirectoryEntry(GetRdsServersGroupPath());
                    computerObject.MoveTo(group);
                    group.CommitChanges();
                }
            }
        }

        public bool CheckSessionHostFeatureInstallation(string hostName)
        {
            bool isInstalled = false;

            Runspace runSpace = null;
            try
            {
                runSpace = RdsRunspaceExtensions.OpenRunspace();

                Command cmd = new Command("Get-WindowsFeature");
                cmd.Parameters.Add("Name", "RDS-RD-Server");

                var feature = runSpace.ExecuteRemoteShellCommand(hostName, cmd, PrimaryDomainController).FirstOrDefault();

                if (feature != null)
                {
                    isInstalled = (bool)RdsRunspaceExtensions.GetPSObjectProperty(feature, "Installed");
                }
            }
            finally
            {
                runSpace.CloseRunspace();
            }

            return isInstalled;
        }

        public bool CheckServerAvailability(string hostName)
        {
            var ping = new Ping();

            var ipAddress = GetServerIp(hostName);

            if (ipAddress == null)
            {
                return false;
            }

            var reply = ping.Send(ipAddress, 3000);

            return reply != null && reply.Status == IPStatus.Success;
        }
        
        #region Helpers

        private static string ConvertByteToStringSid(Byte[] sidBytes)
        {
            StringBuilder strSid = new StringBuilder();
            strSid.Append("S-");
            try
            {
                // Add SID revision.
                strSid.Append(sidBytes[0].ToString());
                // Next six bytes are SID authority value.
                if (sidBytes[6] != 0 || sidBytes[5] != 0)
                {
                    string strAuth = String.Format
                        ("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
                        (Int16)sidBytes[1],
                        (Int16)sidBytes[2],
                        (Int16)sidBytes[3],
                        (Int16)sidBytes[4],
                        (Int16)sidBytes[5],
                        (Int16)sidBytes[6]);
                    strSid.Append("-");
                    strSid.Append(strAuth);
                }
                else
                {
                    Int64 iVal = (Int32)(sidBytes[1]) +
                        (Int32)(sidBytes[2] << 8) +
                        (Int32)(sidBytes[3] << 16) +
                        (Int32)(sidBytes[4] << 24);
                    strSid.Append("-");
                    strSid.Append(iVal.ToString());
                }

                // Get sub authority count...
                int iSubCount = Convert.ToInt32(sidBytes[7]);
                int idxAuth = 0;
                for (int i = 0; i < iSubCount; i++)
                {
                    idxAuth = 8 + i * 4;
                    UInt32 iSubAuth = BitConverter.ToUInt32(sidBytes, idxAuth);
                    strSid.Append("-");
                    strSid.Append(iSubAuth.ToString());
                }
            }
            catch
            {
                return "";
            }
            return strSid.ToString();
        }

        private StartMenuApp CreateStartMenuAppFromPsObject(PSObject psObject)
        {
            var remoteApp = new StartMenuApp
            {
                DisplayName = Convert.ToString(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "DisplayName")),
                FilePath = Convert.ToString(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "FilePath")),
                FileVirtualPath = Convert.ToString(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "FileVirtualPath"))
            };

            remoteApp.Alias = remoteApp.DisplayName;

            return remoteApp;
        }

        private RemoteApplication CreateRemoteApplicationFromPsObject(PSObject psObject)
        {
            var remoteApp = new RemoteApplication
            {
                DisplayName = Convert.ToString(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "DisplayName")),
                FilePath = Convert.ToString(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "FilePath")),
                Alias = Convert.ToString(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "Alias")),
                ShowInWebAccess = Convert.ToBoolean(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "ShowInWebAccess")),
                Users = null
            };

            var requiredCommandLine = RdsRunspaceExtensions.GetPSObjectProperty(psObject, "RequiredCommandLine");
            remoteApp.RequiredCommandLine = requiredCommandLine == null ? null : requiredCommandLine.ToString();
            var commandLineSettings = RdsRunspaceExtensions.GetPSObjectProperty(psObject, "CommandLineSetting");

            if (commandLineSettings != null)
            {
                remoteApp.CommandLineSettings = (CommandLineSettings)Enum.Parse(typeof(CommandLineSettings), commandLineSettings.ToString());
            }

            var users = (string[])(RdsRunspaceExtensions.GetPSObjectProperty(psObject, "UserGroups"));

            if (users != null && users.Any())
            {
                remoteApp.Users = users;
            }
            else
            {
                remoteApp.Users = null;
            }
            
            return remoteApp;
        }

        internal IPAddress GetServerIp(string hostname, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var address = GetServerIps(hostname);

            return address.FirstOrDefault(x => x.AddressFamily == addressFamily);
        }

        internal IEnumerable<IPAddress> GetServerIps(string hostname)
        {
            try
            {
                var address = Dns.GetHostAddresses(hostname);
                return address;
            }
            catch
            {
            }

            return new List<IPAddress>();
        }

        internal void RemoveItem(Runspace runSpace, string path)
        {
            Command rdRapCommand = new Command("Remove-Item");
            rdRapCommand.Parameters.Add("Path", path);
            rdRapCommand.Parameters.Add("Force", true);
            rdRapCommand.Parameters.Add("Recurse", true);

            runSpace.ExecuteShellCommand(rdRapCommand, false, PrimaryDomainController);
        }

        internal void RemoveItemRemote(Runspace runSpace, string hostname, string path, params string[] imports)
        {
            Command rdRapCommand = new Command("Remove-Item");
            rdRapCommand.Parameters.Add("Path", string.Format("\"{0}\"", path));
            rdRapCommand.Parameters.Add("Force", "");
            rdRapCommand.Parameters.Add("Recurse", "");

            runSpace.ExecuteRemoteShellCommand(hostname, rdRapCommand, PrimaryDomainController, imports);
        }

        private string GetPolicyName(string organizationId, string collectionName, RdsPolicyTypes policyType)
        {
            string policyName = string.Format("{0}-", collectionName);

            switch (policyType)
            {
                case RdsPolicyTypes.RdCap:
                {
                    policyName += "RDCAP";
                    break;
                }
                case RdsPolicyTypes.RdRap:
                {
                    policyName += "RDRAP";
                    break;
                }
            }

            return policyName;
        }

        private string GetComputersGroupName(string collectionName)
        {
            return string.Format(RdsGroupFormat, collectionName, Computers.ToLowerInvariant());
        }

        private string GetUsersGroupName(string collectionName)
        {
            return string.Format(RdsGroupFormat, collectionName, Users.ToLowerInvariant());
        }

        private string GetLocalAdminsGroupName(string collectionName)
        {
            return string.Format(RdsGroupFormat, collectionName, Admins.ToLowerInvariant());
        }

        internal string GetComputerGroupPath(string organizationId, string collection)
        {
            StringBuilder sb = new StringBuilder();
            
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, GetComputersGroupName(collection));
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }        

        internal string GetUsersGroupPath(string organizationId, string collection)
        {
            StringBuilder sb = new StringBuilder();
            
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, GetUsersGroupName(collection));
            AppendOUPath(sb, organizationId);            
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetGroupPath(string organizationId, string collectionName, string groupName)
        {
            StringBuilder sb = new StringBuilder();

            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, groupName);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetCollectionOUPath(string organizationId, string collectionName)
        {
            StringBuilder sb = new StringBuilder();

            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendOUPath(sb, collectionName);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetUserPath(string organizationId, string loginName)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, loginName);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetRootOUPath()
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);                        
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetOrganizationPath(string organizationId)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private DirectoryEntry GetComputerObject(string computerName)
        {
            DirectorySearcher deSearch = new DirectorySearcher
            {
                Filter = string.Format("(&(objectCategory=computer)(name={0}))", computerName)
            };
            
            SearchResult results = deSearch.FindOne();
            
            return results.GetDirectoryEntry();
        }        

        private string GetTenantComputerPath(string objName, string organizationId)
        {
            StringBuilder sb = new StringBuilder();
            
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, objName);
            AppendOUPath(sb, RdsServersOU);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetComputersRootPath()
        {
            StringBuilder sb = new StringBuilder();
            
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendOUPath(sb, ComputersRootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetRdsServersGroupPath()
        {
            StringBuilder sb = new StringBuilder();

            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendOUPath(sb, RdsServersRootOU);
            AppendOUPath(sb, ComputersRootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetRdsServerPath(string name)
        {
            StringBuilder sb = new StringBuilder();

            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, name);
            AppendOUPath(sb, RdsServersRootOU);
            AppendOUPath(sb, ComputersRootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetRootPath()
        {
            StringBuilder sb = new StringBuilder();

            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendDomainPath(sb, RootDomain);        

            return sb.ToString();
        }

        private string GetGpoPath(string gpoId)
        {
            StringBuilder sb = new StringBuilder();

            AppendProtocol(sb);
            AppendCNPath(sb, gpoId);
            AppendCNPath(sb, "Policies");
            AppendCNPath(sb, "System");
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        internal string GetTenantComputerGroupPath(string organizationId)
        {
            StringBuilder sb = new StringBuilder();
            
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendOUPath(sb, RdsServersOU);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }        

        private static void AppendCNPath(StringBuilder sb, string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
                return;

            sb.Append("CN=").Append(organizationId).Append(",");
        }

        private void AppendDomainController(StringBuilder sb)
        {
            sb.Append(PrimaryDomainController + "/");
        }

        private static void AppendProtocol(StringBuilder sb)
        {
            sb.Append("LDAP://");
        }

        private static void AppendOUPath(StringBuilder sb, string ou)
        {
            if (string.IsNullOrEmpty(ou))
                return;

            string path = ou.Replace("/", "\\");
            string[] parts = path.Split('\\');
            for (int i = parts.Length - 1; i != -1; i--)
                sb.Append("OU=").Append(parts[i]).Append(",");
        }

        private static void AppendDomainPath(StringBuilder sb, string domain)
        {
            if (string.IsNullOrEmpty(domain))
                return;

            string[] parts = domain.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append("DC=").Append(parts[i]);

                if (i < (parts.Length - 1))
                    sb.Append(",");
            }
        }

        #endregion

        #region Windows Feature PowerShell

        internal bool IsFeatureInstalled(string featureName, Runspace runSpace)
        {
            bool isInstalled = false;
            Command cmd = new Command("Get-WindowsFeature");
            cmd.Parameters.Add("Name", featureName);
            var feature = runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController).FirstOrDefault();

            if (feature != null)
            {
                isInstalled = (bool)RdsRunspaceExtensions.GetPSObjectProperty(feature, "Installed");
            }

            return isInstalled;
        }

        internal bool IsFeatureInstalled(string hostName, string featureName, Runspace runSpace)
        {
            bool isInstalled = false;            
            Command cmd = new Command("Get-WindowsFeature");
            cmd.Parameters.Add("Name", featureName);
            var feature = runSpace.ExecuteRemoteShellCommand(hostName, cmd, PrimaryDomainController).FirstOrDefault();
            
            if (feature != null)
            {
                isInstalled = (bool)RdsRunspaceExtensions.GetPSObjectProperty(feature, "Installed");
            }            

            return isInstalled;
        }

        internal PSObject AddFeature(Runspace runSpace, string featureName, bool includeAllSubFeature = true, bool restart = false)
        {
            Command cmd = new Command("Add-WindowsFeature");
            cmd.Parameters.Add("Name", featureName);

            if (includeAllSubFeature)
            {
                cmd.Parameters.Add("IncludeAllSubFeature", true);
            }

            if (restart)
            {
                cmd.Parameters.Add("Restart", true);
            }

            return runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController).FirstOrDefault();
        }

        internal PSObject AddFeature(Runspace runSpace, string hostName, string featureName, bool includeAllSubFeature = true, bool restart = false)
        {                       
            Command cmd = new Command("Add-WindowsFeature");
            cmd.Parameters.Add("Name", featureName);

            if (includeAllSubFeature)
            {
                cmd.Parameters.Add("IncludeAllSubFeature", "");
            }
            
            if (restart)
            {
                cmd.Parameters.Add("Restart", "");
            }

            return runSpace.ExecuteRemoteShellCommand(hostName, cmd, PrimaryDomainController).FirstOrDefault();            
        }

        #endregion

        #region PowerShell integration                 

        /// <summary>
        /// Returns the identity of the object from the shell execution result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal string GetResultObjectIdentity(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectIdentity");
            if (result == null)
                throw new ArgumentNullException("result", "Execution result is not specified");

            if (result.Count < 1)
                throw new ArgumentException("Execution result is empty", "result");

            if (result.Count > 1)
                throw new ArgumentException("Execution result contains more than one object", "result");

            PSMemberInfo info = result[0].Members["Identity"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain Identity property", "result");

            string ret = info.Value.ToString();
            Log.WriteEnd("GetResultObjectIdentity");
            return ret;
        }

        internal string GetResultObjectDN(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectDN");
            if (result == null)
                throw new ArgumentNullException("result", "Execution result is not specified");

            if (result.Count < 1)
                throw new ArgumentException("Execution result does not contain any object");

            if (result.Count > 1)
                throw new ArgumentException("Execution result contains more than one object");

            PSMemberInfo info = result[0].Members["DistinguishedName"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain DistinguishedName property", "result");

            string ret = info.Value.ToString();
            Log.WriteEnd("GetResultObjectDN");
            return ret;
        }

        internal bool ItemExists(Runspace runSpace, string path)
        {
            Command testPathCommand = new Command("Test-Path");
            testPathCommand.Parameters.Add("Path", path);

            var testPathResult = runSpace.ExecuteShellCommand(testPathCommand, false, PrimaryDomainController).First();

            var result = Convert.ToBoolean(testPathResult.ToString());

            return result;
        }

        internal bool ItemExistsRemote(Runspace runSpace, string hostname,string path)
        {
            Command testPathCommand = new Command("Test-Path");
            testPathCommand.Parameters.Add("Path", string.Format("\"{0}\"", path));

            var testPathResult = runSpace.ExecuteRemoteShellCommand(hostname, testPathCommand, PrimaryDomainController, RdsModuleName).First();

            var result = Convert.ToBoolean(testPathResult.ToString());

            return result;
        }

        internal List<string> GetServersExistingInCollections(Runspace runSpace)
        {
            var existingHosts = new List<string>();
            //var scripts = new List<string>();
            //scripts.Add(string.Format("$sessions = Get-RDSessionCollection -ConnectionBroker {0}", ConnectionBroker));
            //scripts.Add(string.Format("foreach($session in $sessions){{Get-RDSessionHost $session.CollectionName -ConnectionBroker {0}|Select SessionHost}}", ConnectionBroker));
            //object[] errors;

            //var sessionHosts = ExecuteShellCommand(runSpace, scripts, out errors);

            //foreach(var host in sessionHosts)
            //{
            //    var sessionHost = GetPSObjectProperty(host, "SessionHost");

            //    if (sessionHost != null)
            //    {
            //        existingHosts.Add(sessionHost.ToString());
            //    }
            //}

            return existingHosts;
        }

        internal List<string> EditRdsCollectionSettingsInternal(RdsCollection collection, Runspace runspace)
        {
            object[] errors;
            Command cmd = new Command("Set-RDSessionCollectionConfiguration");
            cmd.Parameters.Add("CollectionName", collection.Name);            
            cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);

            if (string.IsNullOrEmpty(collection.Settings.ClientDeviceRedirectionOptions))
            {
                collection.Settings.ClientDeviceRedirectionOptions = "None";
            }

            var properties = collection.Settings.GetType().GetProperties();

            foreach(var prop in properties)
            {
                if (prop.Name.ToLower() != "id" && prop.Name.ToLower() != "rdscollectionid")
                {
                    var value = prop.GetValue(collection.Settings, null);

                    if (value != null)
                    {
                        cmd.Parameters.Add(prop.Name, value);
                    }
                }
            }

            runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors);

            if (errors != null)
            {
                return errors.Select(e => e.ToString()).ToList();
            }

            return new List<string>();
        }

        internal List<RdsUserSession> GetRdsUserSessionsInternal(string collectionName, Runspace runSpace)
        {
            var result = new List<RdsUserSession>();
            var scripts = new List<string>();
            scripts.Add(string.Format("Get-RDUserSession -ConnectionBroker {0} - CollectionName {1} | ft CollectionName, Username, UnifiedSessionId, SessionState, HostServer", ConnectionBroker, collectionName));            
            object[] errors;
            Command cmd = new Command("Get-RDUserSession");
            cmd.Parameters.Add("CollectionName", collectionName);
            cmd.Parameters.Add("ConnectionBroker", ConnectionBroker);
            var userSessions = runSpace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors);            
            var properties = typeof(RdsUserSession).GetProperties();            

            foreach(var userSession in  userSessions)
            {
                object collectionNameObj = RdsRunspaceExtensions.GetPSObjectProperty(userSession, "CollectionName");
                object domainName = RdsRunspaceExtensions.GetPSObjectProperty(userSession, "DomainName");
                object hostServer = RdsRunspaceExtensions.GetPSObjectProperty(userSession, "HostServer");
                object sessionState = RdsRunspaceExtensions.GetPSObjectProperty(userSession, "SessionState");
                object unifiedSessionId = RdsRunspaceExtensions.GetPSObjectProperty(userSession, "UnifiedSessionId");
                object samAccountName = RdsRunspaceExtensions.GetPSObjectProperty(userSession, "UserName");

                var session = new RdsUserSession
                {
                    CollectionName = collectionNameObj != null ? collectionNameObj.ToString() : "",
                    DomainName = domainName != null ? domainName.ToString() : "",
                    HostServer = hostServer != null ? hostServer.ToString() : "",
                    SessionState = sessionState != null ? sessionState.ToString() : "",
                    UnifiedSessionId = unifiedSessionId != null ? unifiedSessionId.ToString() : "",
                    SamAccountName = samAccountName != null ? samAccountName.ToString() : ""
                };                                
                                
                session.IsVip = false;
                session.UserName = GetUserFullName(session.DomainName, session.SamAccountName, runSpace);
                result.Add(session);
            }

            return result;
        }

        private string GetUserFullName(string domain, string userName, Runspace runspace)
        {
            Command cmd = new Command("Get-WmiObject");
            cmd.Parameters.Add("Class", "win32_useraccount");
            cmd.Parameters.Add("Filter", string.Format("Domain = '{0}' AND Name = '{1}'", domain, userName));
            var names = runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController);

            if (names.Any())
            {
                return names.First().Members["FullName"].Value.ToString();
            }

            return "";
        }

        #endregion

        #region Server Info

        public RdsServerInfo GetRdsServerInfo(string serverName)
        {
            var result = new RdsServerInfo();
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();
                result = GetServerInfo(runspace, serverName);
                result.Status = GetRdsServerStatus(runspace, serverName);

            }
            finally
            {
                runspace.CloseRunspace();
            }

            return result;
        }

        public string GetRdsServerStatus(string serverName)
        {
            string result = "";
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();                
                result = GetRdsServerStatus(runspace, serverName);

            }
            finally
            {
                runspace.CloseRunspace();
            }

            return result;
        }

        public void ShutDownRdsServer(string serverName)
        {            
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();
                var command = new Command("Stop-Computer");
                command.Parameters.Add("ComputerName", serverName);
                command.Parameters.Add("Force", true);
                object[] errors = null;

                runspace.ExecuteShellCommand(command, false, PrimaryDomainController, out errors);

                if (errors.Any())
                {
                    Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                    throw new Exception(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                }
            }
            finally
            {
                runspace.CloseRunspace();
            }
        }

        public void RestartRdsServer(string serverName)
        {
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();
                var command = new Command("Restart-Computer");
                command.Parameters.Add("ComputerName", serverName);
                command.Parameters.Add("Force", true);
                object[] errors = null;

                runspace.ExecuteShellCommand(command, false, PrimaryDomainController, out errors);

                if (errors.Any())
                {
                    Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                    throw new Exception(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                }
            }
            finally
            {
                runspace.CloseRunspace();
            }
        }

        private RdsServerInfo GetServerInfo(Runspace runspace, string serverName)
        {
            var result = new RdsServerInfo();
            Command cmd = new Command("Get-WmiObject");
            cmd.Parameters.Add("Class", "Win32_Processor");
            cmd.Parameters.Add("ComputerName", serverName);

            object[] errors = null;
            var psProcInfo = runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors).First();

            if (errors.Any())
            {
                Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                return result;
            }

            cmd = new Command("Get-WmiObject");
            cmd.Parameters.Add("Class", "Win32_OperatingSystem");
            cmd.Parameters.Add("ComputerName", serverName);

            var psMemoryInfo = runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors).First();

            if (errors.Any())
            {
                Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                return result;
            }

            result.NumberOfCores = Convert.ToInt32(RdsRunspaceExtensions.GetPSObjectProperty(psProcInfo, "NumberOfCores"));
            result.MaxClockSpeed = Convert.ToInt32(RdsRunspaceExtensions.GetPSObjectProperty(psProcInfo, "MaxClockSpeed"));
            result.LoadPercentage = Convert.ToInt32(RdsRunspaceExtensions.GetPSObjectProperty(psProcInfo, "LoadPercentage"));
            result.MemoryAllocatedMb = Math.Round(Convert.ToDouble(RdsRunspaceExtensions.GetPSObjectProperty(psMemoryInfo, "TotalVisibleMemorySize")) / 1024, 1);
            result.FreeMemoryMb = Math.Round(Convert.ToDouble(RdsRunspaceExtensions.GetPSObjectProperty(psMemoryInfo, "FreePhysicalMemory")) / 1024, 1);
            result.Drives = GetRdsServerDriveInfo(runspace, serverName).ToArray();

            return result;
        }

        private string GetRdsServerStatus (Runspace runspace, string serverName)
        {
            Log.WriteWarning(string.Format("CheckServerAvailability started"));        

            if (CheckServerAvailability(serverName))
            {
                Log.WriteWarning(string.Format("Pending reboot check started"));        

                if (CheckPendingReboot(runspace, serverName))
                {
                    Log.WriteWarning(string.Format("Pending reboot check finished"));        

                    return "Online - Pending Reboot";
                }

                Log.WriteWarning(string.Format("Pending reboot check finished"));        

                return "Online";
            }
            else
            {
                return "Unavailable";
            }
        }

        private List<RdsServerDriveInfo> GetRdsServerDriveInfo(Runspace runspace, string serverName)
        {
            var result = new List<RdsServerDriveInfo>();
            Command cmd = new Command("Get-WmiObject");
            cmd.Parameters.Add("Class", "Win32_LogicalDisk");
            cmd.Parameters.Add("Filter", "DriveType=3");
            cmd.Parameters.Add("ComputerName", serverName);
            object[] errors = null;
            var psDrives = runspace.ExecuteShellCommand(cmd, false, PrimaryDomainController, out errors);

            if (errors.Any())
            {
                Log.WriteWarning(string.Join("\r\n", errors.Select(e => e.ToString()).ToArray()));
                return result;
            }            

            foreach (var psDrive in psDrives)
            {
                var driveInfo = new RdsServerDriveInfo()
                {
                    VolumeName = RdsRunspaceExtensions.GetPSObjectProperty(psDrive, "VolumeName").ToString(),
                    DeviceId = RdsRunspaceExtensions.GetPSObjectProperty(psDrive, "DeviceId").ToString(),
                    SizeMb = Math.Round(Convert.ToDouble(RdsRunspaceExtensions.GetPSObjectProperty(psDrive, "Size")) / 1024 / 1024, 1),
                    FreeSpaceMb = Math.Round(Convert.ToDouble(RdsRunspaceExtensions.GetPSObjectProperty(psDrive, "FreeSpace")) / 1024 / 1024, 1)
                };

                result.Add(driveInfo);
            }

            return result;
        }

        private bool CheckRDSServerAvaliability(string serverName)
        {            
            var ping = new Ping();
            var reply = ping.Send(serverName, 1000);

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }

            return false;
        }

        private bool CheckPendingReboot(Runspace runspace, string serverName)
        {            
            if (CheckPendingReboot(runspace, serverName, @"HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing", "RebootPending"))
            {
                return true;
            }

            if (CheckPendingReboot(runspace, serverName, @"HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", "RebootRequired"))
            {
                return true;
            }

            if (CheckPendingReboot(runspace, serverName, @"HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager", "PendingFileRenameOperations"))
            {
                return true;
            }

            return false;
        }

        private bool CheckPendingReboot(Runspace runspace, string serverName, string registryPath, string registryKey)
        {
            Command cmd = new Command("Get-ItemProperty");
            cmd.Parameters.Add("Path", string.Format("\"{0}\"", registryPath));
            cmd.Parameters.Add("Name", registryKey);
            cmd.Parameters.Add("ErrorAction", "SilentlyContinue");

            var feature = runspace.ExecuteRemoteShellCommand(serverName, cmd, PrimaryDomainController).FirstOrDefault();

            if (feature != null)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}

