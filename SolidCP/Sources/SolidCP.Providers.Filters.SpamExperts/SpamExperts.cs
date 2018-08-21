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

using SolidCP.Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace SolidCP.Providers.Filters
{
    public class SpamExperts : HostingServiceProviderBase, ISpamExperts
    {
        public SpamExperts()
        {
        }

        public override bool IsInstalled()
        {
            return true;
        }

        private string emptyPassword => Guid.NewGuid().ToString();

        private string Url => ProviderSettings["SpamExpertsUrl"];

        private string User => ProviderSettings["SpamExpertsUser"];

        private string Password => ProviderSettings["SpamExpertsPassword"];


        private SpamExpertsResult CheckSuccess(string result)
        {
            if (result == null) return SpamExpertsResult.None;

            if (result.Contains("SUCCESS")) return new SpamExpertsResult(SpamExpertsStatus.Success, result);
            if (result.StartsWith("[") && result.EndsWith("]")) return new SpamExpertsResult(SpamExpertsStatus.Success, result);
            if (result.StartsWith("{") && result.EndsWith("}")) return new SpamExpertsResult(SpamExpertsStatus.Success, result);

            if (result.Contains("already exists")) return new SpamExpertsResult(SpamExpertsStatus.AlreadyExists, result);
            if (result.Contains("Unable to find")) return new SpamExpertsResult(SpamExpertsStatus.NotFound, result);
            if (result.Contains("No such")) return new SpamExpertsResult(SpamExpertsStatus.NotFound, result);

            if (result.StartsWith("EXCEPTION:")) return new SpamExpertsResult(SpamExpertsStatus.Error, result);
            if (result.StartsWith("ERROR:")) return new SpamExpertsResult(SpamExpertsStatus.Error, result);

            return new SpamExpertsResult(SpamExpertsStatus.Success, result);
        }

        private SpamExpertsResult ExecCommand(string command, params string[] param)
        {
            Log.WriteStart("ExecCommand {0}", command);

            UriBuilder uri = new UriBuilder();
            uri.Scheme = "https";
            uri.Host = Url;

            string path = "/api/" + command + "/";
            int paramCount = param.Length / 2;
            for (int i = 0; i < paramCount; i++)
            {
                string name = param[i * 2];
                string val = param[i * 2 + 1];

                path += name + "/" + HttpUtility.UrlEncode(val) + "/";

                if (name != "password")
                    Log.WriteInfo("{0}={1}", name, val);

            }
            uri.Path = path;

            string result = string.Empty;
            try
            {
                System.Net.WebClient Client = new WebClient();

                Client.Credentials = new NetworkCredential(User, Password);

                using (Stream strm = Client.OpenRead(uri.Uri))
                {
                    StreamReader sr = new StreamReader(strm);
                    result = sr.ReadToEnd();
                }

                if (result == null) result = "";

                Log.WriteInfo("result = {0}", result);
            }
            catch (Exception exc)
            {
                result = "EXCEPTION:" + exc.ToString();
                Log.WriteWarning(result);
            }

            Log.WriteEnd("ExecCommand");
            return CheckSuccess(result);
        }

        public SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            SpamExpertsResult result;

            Log.WriteStart("AddDomain");

            if (String.IsNullOrEmpty(password))
                password = emptyPassword;

            if ((destinations == null) || (destinations.Length == 0))
            {
                result = ExecCommand("domain/add", "domain", domain);
            }
            else
            {
                string list = "[\"" + String.Join("\",\"", destinations) + "\"]";
                result = ExecCommand("domain/add", "domain", domain, "destinations", list);
            }

            result = ExecCommand("domainuser/add", "domain", domain, "password", password, "email", email);

            Log.WriteEnd("AddDomain");

            return result;
        }

        public SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            SpamExpertsResult result;

            Log.WriteStart("SetDomainUser");

            if (String.IsNullOrEmpty(password))
                password = emptyPassword;

            result = ExecCommand("domainuser/add", "domain", domain, "password", password, "email", email);

            Log.WriteEnd("SetDomainUser");

            return result;
        }



        public SpamExpertsResult DeleteDomainFilter(string domain)
        {
            Log.WriteStart("DeleteDomain");

            var result = ExecCommand("domainuser/remove", "username", domain);
            result = ExecCommand("domain/remove", "domain", domain);

            Log.WriteEnd("DeleteDomain");

            return result;
        }

        public SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            Log.WriteStart("AddEmail");

            if (String.IsNullOrEmpty(password))
                password = emptyPassword;

            var result = ExecCommand("emailusers/add", "username", name, "password", password, "domain", domain);

            Log.WriteEnd("AddEmail");

            return result;
        }

        public SpamExpertsResult DeleteEmailFilter(string email)
        {
            Log.WriteStart("DeleteEmail");

            var result = ExecCommand("emailusers/remove", "username", email);

            Log.WriteEnd("DeleteEmail");

            return result;
        }

        public SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            Log.WriteStart("SetEmailPassword");

            if (String.IsNullOrEmpty(password))
                password = emptyPassword;

            var result = ExecCommand("emailusers/setpassword", "username", email, "password", password);

            Log.WriteEnd("SetEmailPassword");

            return result;
        }

        public SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            Log.WriteStart("SetDomainUserPassword");

            if (String.IsNullOrEmpty(password))
                password = emptyPassword;

            Log.WriteEnd("SetDomainUserPassword");

            var result = ExecCommand("domainuser/setpassword", "username", name, "password", password);

            return result;
        }

        public SpamExpertsResult SetDomainFilterDestinations(string domain, string[] destinations)
        {
            if (destinations == null) destinations = new string[] { };

            string list = "[\"" + String.Join("\",\"", destinations) + "\"]";
            var result = ExecCommand("domain/edit", "domain", domain, "destinations", list);
            return result;
        }

        public SpamExpertsResult SetDomainFilterAdminContact(string domain, string email)
        {
            SpamExpertsResult res = null;

            Log.WriteStart("SetDomainAdminContact");

            res = ExecCommand("domainadmincontact/set", "domain", domain, "email", email);

            Log.WriteEnd("SetDomainAdminContact");

            return res;
        }

        public SpamExpertsResult SetDomainFilterContact(string domain, string email)
        {
            SpamExpertsResult res = null;

            Log.WriteStart("SetDomainAdminContact");

            res = ExecCommand("domaincontact/set", "domain", domain, "email", email);

            Log.WriteEnd("SetDomainAdminContact");

            return res;
        }



    }
}
