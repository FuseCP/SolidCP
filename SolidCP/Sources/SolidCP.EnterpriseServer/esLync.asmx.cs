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
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esLync
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class esLync : WebService
    {

        [WebMethod]
        public LyncUserResult CreateLyncUser(int itemId, int accountId, int lyncUserPlanId)
        {
            return LyncController.CreateLyncUser(itemId, accountId, lyncUserPlanId);
        }

        [WebMethod]
        public ResultObject DeleteLyncUser(int itemId, int accountId)
        {
            return LyncController.DeleteLyncUser(itemId, accountId);
        }

        [WebMethod]
        public LyncUsersPagedResult GetLyncUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return LyncController.GetLyncUsersPaged(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        [WebMethod]
        public List<LyncUser> GetLyncUsersByPlanId(int itemId, int planId)
        {
            return LyncController.GetLyncUsersByPlanId(itemId, planId);
        }

        [WebMethod]
        public IntResult GetLyncUserCount(int itemId)
        {
            return LyncController.GetLyncUsersCount(itemId);
        }


        #region Lync User Plans
        [WebMethod]
        public List<LyncUserPlan> GetLyncUserPlans(int itemId)
        {
            return LyncController.GetLyncUserPlans(itemId);
        }

        [WebMethod]
        public LyncUserPlan GetLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return LyncController.GetLyncUserPlan(itemId, lyncUserPlanId);
        }

        [WebMethod]
        public int AddLyncUserPlan(int itemId, LyncUserPlan lyncUserPlan)
        {
            return LyncController.AddLyncUserPlan(itemId, lyncUserPlan);
        }

        [WebMethod]
        public int UpdateLyncUserPlan(int itemId, LyncUserPlan lyncUserPlan)
        {
            return LyncController.UpdateLyncUserPlan(itemId, lyncUserPlan);
        }


        [WebMethod]
        public int DeleteLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return LyncController.DeleteLyncUserPlan(itemId, lyncUserPlanId);
        }

        [WebMethod]
        public int SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return LyncController.SetOrganizationDefaultLyncUserPlan(itemId, lyncUserPlanId);
        }

        [WebMethod]
        public LyncUser GetLyncUserGeneralSettings(int itemId, int accountId)
        {
            return LyncController.GetLyncUserGeneralSettings(itemId, accountId);
        }

        [WebMethod]
        public LyncUserResult SetLyncUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return LyncController.SetLyncUserGeneralSettings(itemId, accountId, sipAddress, lineUri);
        }


        [WebMethod]
        public LyncUserResult SetUserLyncPlan(int itemId, int accountId, int lyncUserPlanId)
        {
            return LyncController.SetUserLyncPlan(itemId, accountId, lyncUserPlanId);
        }

        [WebMethod]
        public LyncFederationDomain[] GetFederationDomains(int itemId)
        {
            return LyncController.GetFederationDomains(itemId);
        }

        [WebMethod]
        public LyncUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return LyncController.AddFederationDomain(itemId, domainName, proxyFqdn);
        }

        [WebMethod]
        public LyncUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return LyncController.RemoveFederationDomain(itemId, domainName);
        }

        #endregion

        [WebMethod]
        public string[] GetPolicyList(int itemId, LyncPolicyType type, string name)
        {
            return LyncController.GetPolicyList(itemId, type, name);
        }

    }
}
