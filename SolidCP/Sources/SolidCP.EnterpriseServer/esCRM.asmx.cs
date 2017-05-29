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
using System.ComponentModel;
using System.Web.Services;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using Microsoft.Web.Services3;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    
    public class esCRM : WebService
    {

        [WebMethod]
        public OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode)
        {
            return CRMController.CreateOrganization(organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName, userId, collation, baseLanguageCode);
        }

        [WebMethod]
        public StringArrayResultObject GetCollation(int packageId)
        {            
            return CRMController.GetCollation(packageId);            
        }

        [WebMethod]
        public StringArrayResultObject GetCollationByServiceId(int serviceId)
        {
            return CRMController.GetCollationByServiceId(serviceId);            
        }

        [WebMethod]
        public CurrencyArrayResultObject GetCurrency(int packageId)
        {
            return CRMController.GetCurrency(packageId);
        }

        [WebMethod]
        public CurrencyArrayResultObject GetCurrencyByServiceId(int serviceId)
        {
            return CRMController.GetCurrencyByServiceId(serviceId);
        }
      
        [WebMethod]
        public ResultObject DeleteCRMOrganization(int organizationId)
        {
            return CRMController.DeleteOrganization(organizationId);
        }

        [WebMethod]
        public OrganizationUsersPagedResult GetCRMUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email,
            int startRow, int maximumRows)
        {
            return CRMController.GetCRMUsers(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        [WebMethod]
        public IntResult GetCRMUserCount(int itemId, string name, string email, int CALType)
        {
            return CRMController.GetCRMUsersCount(itemId, name, email, CALType);
        }

        [WebMethod]
        public UserResult CreateCRMUser(OrganizationUser user, int packageId, int itemId, Guid businessUnitOrgId, int CALType)
        {
            return CRMController.CreateCRMUser(user, packageId, itemId, businessUnitOrgId, CALType);
        }


        [WebMethod]
        public CRMBusinessUnitsResult GetBusinessUnits(int itemId, int packageId)
        {
            return CRMController.GetCRMBusinessUnits(itemId, packageId);
        }


        [WebMethod]
        public CrmRolesResult GetCrmRoles(int itemId, int accountId, int packageId)
        {
            return CRMController.GetCRMRoles(itemId, accountId, packageId);
        }

        [WebMethod]
        public ResultObject SetUserRoles(int itemId, int accountId, int packageId, Guid[] roles)
        {
            return CRMController.SetUserRoles(itemId, accountId, packageId, roles);
        }

        [WebMethod]
        public ResultObject SetUserCALType(int itemId, int accountId, int packageId, int CALType)
        {
            return CRMController.SetUserCALType(itemId, accountId, packageId, CALType);
        }

        [WebMethod]
        public ResultObject ChangeUserState(int itemId, int accountId, bool disable)
        {
            return CRMController.ChangeUserState(itemId, accountId, disable);
        }


        [WebMethod]
        public CrmUserResult GetCrmUser(int itemId, int accountId)
        {
            return CRMController.GetCrmUser(itemId, accountId);
        }

        [WebMethod]
        public ResultObject SetMaxDBSize(int itemId, int packageId, long maxSize)
        {
            return CRMController.SetMaxDBSize(itemId, packageId, maxSize);
        }

        [WebMethod]
        public long GetDBSize(int itemId, int packageId)
        {
            return CRMController.GetDBSize(itemId, packageId);
        }

        [WebMethod]
        public long GetMaxDBSize(int itemId, int packageId)
        {
            return CRMController.GetMaxDBSize(itemId, packageId);
        }

        [WebMethod]
        public int[] GetInstalledLanguagePacks(int packageId)
        {
            return CRMController.GetInstalledLanguagePacks(packageId);
        }

        [WebMethod]
        public int[] GetInstalledLanguagePacksByServiceId(int serviceId)
        {
            return CRMController.GetInstalledLanguagePacksByServiceId(serviceId);
        }
        
    }
}
