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

﻿using System.Web.Services;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esOCS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class esOCS : WebService
    {

        [WebMethod]
        public OCSUserResult CreateOCSUser(int itemId, int accountId)
        {
            return OCSController.CreateOCSUser(itemId, accountId);
        }

        [WebMethod]
        public ResultObject DeleteOCSUser(int itemId, string instanceId)
        {
            return OCSController.DeleteOCSUser(itemId, instanceId);
        }

        [WebMethod]
        public OCSUsersPagedResult GetOCSUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email,
            int startRow, int maximumRows)
        {
            return OCSController.GetOCSUsers(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);            
        }

        [WebMethod]
        public IntResult GetOCSUserCount(int itemId, string name, string email)
        {
            return OCSController.GetOCSUsersCount(itemId, name, email);            
        }

        [WebMethod]
        public OCSUser GetUserGeneralSettings(int itemId, string instanceId)
        {
            return OCSController.GetUserGeneralSettings(itemId, instanceId);
        }

        [WebMethod]
        public void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            OCSController.SetUserGeneralSettings(itemId, instanceId, enabledForFederation, enabledForPublicIMConnectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);    
        }
    }

}
