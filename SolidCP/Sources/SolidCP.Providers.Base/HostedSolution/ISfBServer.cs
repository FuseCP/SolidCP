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
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.HostedSolution
{
    public interface ISfBServer
    {
        string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice);
        string GetOrganizationTenantId(string organizationId);
        bool DeleteOrganization(string organizationId, string sipDomain);

        bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan);
        SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn);
        bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser);
        bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan);
        bool DeleteUser(string userUpn);

        SfBFederationDomain[] GetFederationDomains(string organizationId);
        bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn);
        bool RemoveFederationDomain(string organizationId, string domainName);

        void ReloadConfiguration();

        string[] GetPolicyList(SfBPolicyType type, string name);
    }
}
