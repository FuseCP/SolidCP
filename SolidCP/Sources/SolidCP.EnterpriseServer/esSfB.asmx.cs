// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
    /// Summary description for esSfB
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class esSfB : WebService
    {

        [WebMethod]
        public SfBUserResult CreateSfBUser(int itemId, int accountId, int sfbUserPlanId)
        {
            return SfBController.CreateSfBUser(itemId, accountId, sfbUserPlanId);
        }

        [WebMethod]
        public ResultObject DeleteSfBUser(int itemId, int accountId)
        {
            return SfBController.DeleteSfBUser(itemId, accountId);
        }

        [WebMethod]
        public SfBUsersPagedResult GetSfBUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return SfBController.GetSfBUsersPaged(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        [WebMethod]
        public List<SfBUser> GetSfBUsersByPlanId(int itemId, int planId)
        {
            return SfBController.GetSfBUsersByPlanId(itemId, planId);
        }

        [WebMethod]
        public IntResult GetSfBUserCount(int itemId)
        {
            return SfBController.GetSfBUsersCount(itemId);
        }


        #region SfB User Plans
        [WebMethod]
        public List<SfBUserPlan> GetSfBUserPlans(int itemId)
        {
            return SfBController.GetSfBUserPlans(itemId);
        }

        [WebMethod]
        public SfBUserPlan GetSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return SfBController.GetSfBUserPlan(itemId, sfbUserPlanId);
        }

        [WebMethod]
        public int AddSfBUserPlan(int itemId, SfBUserPlan sfbUserPlan)
        {
            return SfBController.AddSfBUserPlan(itemId, sfbUserPlan);
        }

        [WebMethod]
        public int UpdateSfBUserPlan(int itemId, SfBUserPlan sfbUserPlan)
        {
            return SfBController.UpdateSfBUserPlan(itemId, sfbUserPlan);
        }


        [WebMethod]
        public int DeleteSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return SfBController.DeleteSfBUserPlan(itemId, sfbUserPlanId);
        }

        [WebMethod]
        public int SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return SfBController.SetOrganizationDefaultSfBUserPlan(itemId, sfbUserPlanId);
        }

        [WebMethod]
        public SfBUser GetSfBUserGeneralSettings(int itemId, int accountId)
        {
            return SfBController.GetSfBUserGeneralSettings(itemId, accountId);
        }

        [WebMethod]
        public SfBUserResult SetSfBUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return SfBController.SetSfBUserGeneralSettings(itemId, accountId, sipAddress, lineUri);
        }


        [WebMethod]
        public SfBUserResult SetUserSfBPlan(int itemId, int accountId, int sfbUserPlanId)
        {
            return SfBController.SetUserSfBPlan(itemId, accountId, sfbUserPlanId);
        }

        [WebMethod]
        public SfBFederationDomain[] GetFederationDomains(int itemId)
        {
            return SfBController.GetFederationDomains(itemId);
        }

        [WebMethod]
        public SfBUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return SfBController.AddFederationDomain(itemId, domainName, proxyFqdn);
        }

        [WebMethod]
        public SfBUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return SfBController.RemoveFederationDomain(itemId, domainName);
        }

        #endregion

        [WebMethod]
        public string[] GetPolicyList(int itemId, SfBPolicyType type, string name)
        {
            return SfBController.GetPolicyList(itemId, type, name);
        }

    }
}
