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
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Providers.HostedSolution
{
    public interface ICRM
    {
        OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName,
                    int baseLanguageCode,
                    string ou,
                    string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol,
                    string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail,                  
                    string organizationCollation,
                    long maxSize);

        string[] GetSupportedCollationNames();

        Currency[] GetCurrencyList();

        int[] GetInstalledLanguagePacks();
       
        ResultObject DeleteOrganization(Guid orgId);

        UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType);

        CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName);

        CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId);

        CrmRolesResult GetCrmUserRoles(string orgName, Guid userId);

        ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles);

        ResultObject SetUserCALType(string orgName, Guid userId, int CALType);

        CrmUserResult GetCrmUserByDomainName(string domainName, string orgName);

        CrmUserResult GetCrmUserById(Guid crmUserId, string orgName);

        ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId);

        long GetDBSize(Guid organizationId);

        long GetMaxDBSize(Guid organizationId);

        ResultObject SetMaxDBSize(Guid organizationId, long maxSize);

    }
}
