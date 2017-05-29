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
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.RemoteDesktopServices;
using SolidCP.EnterpriseServer.Base.RDS;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esRemoteDesktopServices : System.Web.Services.WebService
    {

        [WebMethod]
        public RdsCollection GetRdsCollection(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollection(collectionId);
        }

        [WebMethod]
        public RdsCollectionSettings GetRdsCollectionSettings(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionSettings(collectionId);
        }

        [WebMethod]
        public List<RdsCollection> GetOrganizationRdsCollections(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsCollections(itemId);
        }

        [WebMethod]
        public int AddRdsCollection(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.AddRdsCollection(itemId, collection);
        }

        [WebMethod]
        public ResultObject EditRdsCollection(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.EditRdsCollection(itemId, collection);
        }

        [WebMethod]
        public ResultObject EditRdsCollectionSettings(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.EditRdsCollectionSettings(itemId, collection);
        }

        [WebMethod]
        public RdsCollectionPaged GetRdsCollectionsPaged(int itemId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return RemoteDesktopServicesController.GetRdsCollectionsPaged(itemId, filterColumn, filterValue, sortColumn,
                startRow, maximumRows);
        }

        [WebMethod]
        public ResultObject RemoveRdsCollection(int itemId, RdsCollection collection)
        {
            return RemoteDesktopServicesController.RemoveRdsCollection(itemId, collection);
        }

        [WebMethod]
        public RdsServersPaged GetRdsServersPaged(string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {
            return RemoteDesktopServicesController.GetRdsServersPaged(filterColumn, filterValue, sortColumn, startRow,
                maximumRows);
        }

        [WebMethod]
        public RdsServersPaged GetFreeRdsServersPaged(int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return RemoteDesktopServicesController.GetFreeRdsServersPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public RdsServersPaged GetOrganizationRdsServersPaged(int itemId, int? collectionId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsServersPaged(itemId, collectionId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public RdsServersPaged GetOrganizationFreeRdsServersPaged(int itemId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return RemoteDesktopServicesController.GetOrganizationFreeRdsServersPaged(itemId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public RdsServer GetRdsServer(int rdsSeverId)
        {
            return RemoteDesktopServicesController.GetRdsServer(rdsSeverId);
        }

        [WebMethod]
        public ResultObject SetRDServerNewConnectionAllowed(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            return RemoteDesktopServicesController.SetRDServerNewConnectionAllowed(itemId, newConnectionAllowed, rdsSeverId);
        }

        [WebMethod]
        public List<RdsServer> GetCollectionRdsServers(int collectionId)
        {
            return RemoteDesktopServicesController.GetCollectionRdsServers(collectionId);
        }

        [WebMethod]
        public List<RdsServer> GetOrganizationRdsServers(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsServers(itemId);
        }

        [WebMethod]
        public ResultObject AddRdsServer(RdsServer rdsServer)
        {
            return RemoteDesktopServicesController.AddRdsServer(rdsServer);
        }

        [WebMethod]
        public ResultObject AddRdsServerToCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            return RemoteDesktopServicesController.AddRdsServerToCollection(itemId, rdsServer, rdsCollection);
        }

        [WebMethod]
        public ResultObject AddRdsServerToOrganization(int itemId, int serverId)
        {
            return RemoteDesktopServicesController.AddRdsServerToOrganization(itemId, serverId);
        }

        [WebMethod]
        public ResultObject RemoveRdsServer(int rdsServerId)
        {
            return RemoteDesktopServicesController.RemoveRdsServer(rdsServerId);
        }

        [WebMethod]
        public ResultObject RemoveRdsServerFromCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            return RemoteDesktopServicesController.RemoveRdsServerFromCollection(itemId, rdsServer, rdsCollection);
        }

        [WebMethod]
        public ResultObject RemoveRdsServerFromOrganization(int itemId, int rdsServerId)
        {
            return RemoteDesktopServicesController.RemoveRdsServerFromOrganization(itemId, rdsServerId);
        }

        [WebMethod]
        public ResultObject UpdateRdsServer(RdsServer rdsServer)
        {
            return RemoteDesktopServicesController.UpdateRdsServer(rdsServer);
        }

        [WebMethod]
        public List<OrganizationUser> GetRdsCollectionUsers(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionUsers(collectionId);
        }

        [WebMethod]
        public ResultObject SetUsersToRdsCollection(int itemId, int collectionId, List<OrganizationUser> users)
        {
            return RemoteDesktopServicesController.SetUsersToRdsCollection(itemId, collectionId, users);
        }

        [WebMethod]
        public List<RemoteApplication> GetCollectionRemoteApplications(int itemId, string collectionName)
        {
            return RemoteDesktopServicesController.GetCollectionRemoteApplications(itemId, collectionName);
        }

        [WebMethod]
        public List<StartMenuApp> GetAvailableRemoteApplications(int itemId, string collectionName)
        {
            return RemoteDesktopServicesController.GetAvailableRemoteApplications(itemId, collectionName);
        }

        [WebMethod]
        public ResultObject AddRemoteApplicationToCollection(int itemId, RdsCollection collection, RemoteApplication application)
        {
            return RemoteDesktopServicesController.AddRemoteApplicationToCollection(itemId, collection, application);
        }

        [WebMethod]
        public ResultObject RemoveRemoteApplicationFromCollection(int itemId, RdsCollection collection, RemoteApplication application)
        {
            return RemoteDesktopServicesController.RemoveRemoteApplicationFromCollection(itemId, collection, application);
        }

        [WebMethod]
        public ResultObject SetRemoteApplicationsToRdsCollection(int itemId, int collectionId, List<RemoteApplication> remoteApps)
        {
            return RemoteDesktopServicesController.SetRemoteApplicationsToRdsCollection(itemId, collectionId, remoteApps);
        }

        [WebMethod]
        public int GetOrganizationRdsUsersCount(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsUsersCount(itemId);
        }

        [WebMethod]
        public int GetOrganizationRdsServersCount(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsServersCount(itemId);
        }

        [WebMethod]
        public int GetOrganizationRdsCollectionsCount(int itemId)
        {
            return RemoteDesktopServicesController.GetOrganizationRdsCollectionsCount(itemId);
        }        

        [WebMethod]
        public List<string> GetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp)
        {
            return RemoteDesktopServicesController.GetApplicationUsers(itemId, collectionId, remoteApp);
        }

        [WebMethod]
        public ResultObject SetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp, List<string> users)
        {
            return RemoteDesktopServicesController.SetApplicationUsers(itemId, collectionId, remoteApp, users);
        }

        [WebMethod]
        public List<RdsUserSession> GetRdsUserSessions(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsUserSessions(collectionId);
        }

        [WebMethod]
        public ResultObject LogOffRdsUser(int itemId, string unifiedSessionId, string hostServer)
        {
            return RemoteDesktopServicesController.LogOffRdsUser(itemId, unifiedSessionId, hostServer);
        }

        [WebMethod]
        public List<string> GetRdsCollectionSessionHosts(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionSessionHosts(collectionId);
        }

        [WebMethod]
        public RdsServerInfo GetRdsServerInfo(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.GetRdsServerInfo(itemId, fqdnName);
        }

        [WebMethod]
        public string GetRdsServerStatus(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.GetRdsServerStatus(itemId, fqdnName);
        }

        [WebMethod]
        public ResultObject ShutDownRdsServer(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.ShutDownRdsServer(itemId, fqdnName);
        }

        [WebMethod]
        public ResultObject RestartRdsServer(int? itemId, string fqdnName)
        {
            return RemoteDesktopServicesController.RestartRdsServer(itemId, fqdnName);
        }

        [WebMethod]
        public List<OrganizationUser> GetRdsCollectionLocalAdmins(int collectionId)
        {
            return RemoteDesktopServicesController.GetRdsCollectionLocalAdmins(collectionId);
        }

        [WebMethod]
        public ResultObject SaveRdsCollectionLocalAdmins(OrganizationUser[] users, int collectionId)
        {
            return RemoteDesktopServicesController.SaveRdsCollectionLocalAdmins(users, collectionId);
        }

        [WebMethod]
        public ResultObject InstallSessionHostsCertificate(RdsServer rdsServer)
        {
            return RemoteDesktopServicesController.InstallSessionHostsCertificate(rdsServer);
        }

        [WebMethod]
        public RdsCertificate GetRdsCertificateByServiceId(int serviceId)
        {
            return RemoteDesktopServicesController.GetRdsCertificateByServiceId(serviceId);
        }

        [WebMethod]
        public RdsCertificate GetRdsCertificateByItemId(int? itemId)
        {
            return RemoteDesktopServicesController.GetRdsCertificateByItemId(itemId);
        }

        [WebMethod]
        public ResultObject AddRdsCertificate(RdsCertificate certificate)
        {
            return RemoteDesktopServicesController.AddRdsCertificate(certificate);
        }

        [WebMethod]
        public List<ServiceInfo> GetRdsServices()
        {
            return RemoteDesktopServicesController.GetRdsServices();
        }

        [WebMethod]
        public string GetRdsSetupLetter(int itemId, int? accountId)
        {
            return RemoteDesktopServicesController.GetRdsSetupLetter(itemId, accountId);
        }

        [WebMethod]
        public int SendRdsSetupLetter(int itemId, int? accountId, string to, string cc)
        {
            return RemoteDesktopServicesController.SendRdsSetupLetter(itemId, accountId, to, cc);
        }

        [WebMethod]
        public RdsServerSettings GetRdsServerSettings(int serverId, string settingsName)
        {
            return RemoteDesktopServicesController.GetRdsServerSettings(serverId, settingsName);
        }

        [WebMethod]
        public int UpdateRdsServerSettings(int serverId, string settingsName, RdsServerSettings settings)
        {
            return RemoteDesktopServicesController.UpdateRdsServerSettings(serverId, settingsName, settings);
        }

        [WebMethod]
        public ResultObject ShadowSession(int itemId, string sessionId, bool control, string fqdName)
        {
            return RemoteDesktopServicesController.ShadowSession(itemId, sessionId, control, fqdName);
        }

        [WebMethod]
        public ResultObject ImportCollection(int itemId, string collectionName)
        {
            return RemoteDesktopServicesController.ImportCollection(itemId, collectionName);
        }

        [WebMethod]
        public int GetRemoteDesktopServiceId(int itemId)
        {
            return RemoteDesktopServicesController.GetRemoteDesktopServiceId(itemId);
        }

        [WebMethod]
        public ResultObject SendMessage(RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            return RemoteDesktopServicesController.SendMessage(recipients, text, itemId, rdsCollectionId, userName);
        }

        [WebMethod]
        public List<RdsMessage> GetRdsMessagesByCollectionId(int rdsCollectionId)
        {
            return RemoteDesktopServicesController.GetRdsMessagesByCollectionId(rdsCollectionId);
        }
    }
}
