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
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Server
{
    
    /// <summary>
    /// 
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class CRM : HostingServiceProviderWebService, ICRM
    {              
        private ICRM CrmProvider
        {
            get { return (ICRM)Provider; }
        }


        [WebMethod, SoapHeader("settings")]
        public OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {                                  
           return CrmProvider.CreateOrganization(organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        [WebMethod, SoapHeader("settings")] 
        public string[] GetSupportedCollationNames()
        {
            return CrmProvider.GetSupportedCollationNames();
        }
       
        [WebMethod, SoapHeader("settings")]
        public Currency[] GetCurrencyList()
        {
            return CrmProvider.GetCurrencyList();
        }

        [WebMethod, SoapHeader("settings")]
        public int[] GetInstalledLanguagePacks()
        {
            return CrmProvider.GetInstalledLanguagePacks();
        }

                
        [WebMethod, SoapHeader("settings")]
        public ResultObject DeleteOrganization(Guid orgId)
        {
            return CrmProvider.DeleteOrganization(orgId);
        }

        [WebMethod, SoapHeader("settings")]
        public UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return CrmProvider.CreateCRMUser(user, orgName, organizationId, baseUnitId, CALType);
        }

        [WebMethod, SoapHeader("settings")]
        public CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName)
        {
            return CrmProvider.GetOrganizationBusinessUnits(organizationId, orgName);
        }

        [WebMethod, SoapHeader("settings")]
        public CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId)
        {
            return CrmProvider.GetAllCrmRoles(orgName, businessUnitId);
        }
        
        [WebMethod, SoapHeader("settings")]
        public CrmRolesResult GetCrmUserRoles(string orgName, Guid userId)
        {
            return CrmProvider.GetCrmUserRoles(orgName, userId);
        }
        
        [WebMethod, SoapHeader("settings")]
        public ResultObject SetUserRoles(string orgName, Guid userId, Guid []roles)
        {
            return CrmProvider.SetUserRoles(orgName, userId, roles);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetUserCALType(string orgName, Guid userId, int CALType)
        {
            return CrmProvider.SetUserCALType(orgName, userId, CALType);
        }
        
        [WebMethod, SoapHeader("settings")]
        public CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return CrmProvider.GetCrmUserByDomainName(domainName, orgName);
        }


        [WebMethod, SoapHeader("settings")]
        public CrmUserResult GetCrmUserById(Guid crmUserId, string orgName)
        {
            return CrmProvider.GetCrmUserById(crmUserId, orgName);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId)
        {
            return CrmProvider.ChangeUserState(disable, orgName, crmUserId);
        }

        [WebMethod, SoapHeader("settings")]
        public long GetDBSize(Guid organizationId)
        {
            return CrmProvider.GetDBSize(organizationId);
        }

        [WebMethod, SoapHeader("settings")]
        public long GetMaxDBSize(Guid organizationId)
        {
            return CrmProvider.GetMaxDBSize(organizationId);
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetMaxDBSize(Guid organizationId, long maxSize)
        {
            return CrmProvider.SetMaxDBSize(organizationId, maxSize);
        }
    
    }
}
