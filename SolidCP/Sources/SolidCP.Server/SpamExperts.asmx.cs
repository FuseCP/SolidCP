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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.Filters;
using SolidCP.Server.Utils;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for MailServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class SpamExperts : HostingServiceProviderWebService, ISpamExperts
    {
        private ISpamExperts SpamExpertsProvider
        {
            get { return (ISpamExperts)Provider; }
        }
        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            return SpamExpertsProvider.AddDomainFilter(domain, password, email, destinations);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            return SpamExpertsProvider.AddEmailFilter(name, domain, password);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult DeleteDomainFilter(string domain)
        {
            return SpamExpertsProvider.DeleteDomainFilter(domain);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult DeleteEmailFilter(string email)
        {
            return SpamExpertsProvider.DeleteEmailFilter(email);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            return SpamExpertsProvider.SetDomainFilterDestinations(name, destinations);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            return SpamExpertsProvider.SetDomainFilterUser(domain, password, email);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            return SpamExpertsProvider.SetDomainFilterUserPassword(name, password);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            return SpamExpertsProvider.SetEmailFilterUserPassword(email, password);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            return SpamExpertsProvider.AddDomainFilterAlias(domain,alias);
        }

        [WebMethod, SoapHeader("settings")]
        public SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            return SpamExpertsProvider.DeleteDomainFilterAlias(domain, alias);
        }
    }
}