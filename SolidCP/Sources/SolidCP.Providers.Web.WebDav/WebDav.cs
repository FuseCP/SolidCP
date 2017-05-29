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
using System.Text;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using Microsoft.Web.Administration;
using SolidCP.Providers.Web.Extensions;

namespace SolidCP.Providers.Web
{
    public class WebDav : IWebDav
    {
        #region Fields

        protected WebDavSetting _Setting;

        #endregion

        public WebDav(WebDavSetting setting)
        {
            _Setting = setting;
        }

        public void CreateWebDavRule(string organizationId, string folder, WebDavFolderRule rule)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();

                ConfigurationSection authoringRulesSection = config.GetSection("system.webServer/webdav/authoringRules", string.Format("{0}/{1}/{2}", _Setting.Domain, organizationId, folder));

                ConfigurationElementCollection authoringRulesCollection = authoringRulesSection.GetCollection();

                ConfigurationElement addElement = authoringRulesCollection.CreateElement("add");

                if (rule.Users.Any())
                {
                    addElement["users"] = string.Join(", ", rule.Users.Select(x => x.ToString()).ToArray());
                }

                if (rule.Roles.Any())
                {
                    addElement["roles"] = string.Join(", ", rule.Roles.Select(x => x.ToString()).ToArray());
                }

                if (rule.Pathes.Any())
                {
                    addElement["path"] = string.Join(", ", rule.Pathes.ToArray());
                }

                addElement["access"] = rule.AccessRights;
                authoringRulesCollection.Add(addElement);

                serverManager.CommitChanges();
            }
        }
        public bool DeleteWebDavRule(string organizationId, string folder, WebDavFolderRule rule)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();

                ConfigurationSection authoringRulesSection = config.GetSection("system.webServer/webdav/authoringRules", string.Format("{0}/{1}/{2}", _Setting.Domain, organizationId, folder));

                ConfigurationElementCollection authoringRulesCollection = authoringRulesSection.GetCollection();

                var toDeleteRule = authoringRulesCollection.FindWebDavRule(rule);

                if (toDeleteRule != null)
                {
                    authoringRulesCollection.Remove(toDeleteRule);
                    serverManager.CommitChanges();
                    return true;
                }
                return false;
            }
        }

        public bool SetFolderWebDavRules(string organizationId, string folder, WebDavFolderRule[] newRules)
        {
            try
            {
                if (DeleteAllWebDavRules(organizationId, folder))
                {
                    if (newRules != null)
                    {
                        foreach (var rule in newRules)
                        {
                            CreateWebDavRule(organizationId, folder, rule);
                        }
                    }

                    return true;
                }
            }
            catch {  }
            return false;
        }

        public WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder)
        {
            var rules = new List<WebDavFolderRule>();
            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    Configuration config = serverManager.GetApplicationHostConfiguration();

                    ConfigurationSection authoringRulesSection = config.GetSection("system.webServer/webdav/authoringRules", string.Format("{0}/{1}/{2}", _Setting.Domain, organizationId, folder));

                    ConfigurationElementCollection authoringRulesCollection = authoringRulesSection.GetCollection();


                    foreach (var rule in authoringRulesCollection)
                    {
                        rules.Add(rule.ToWebDavFolderRule());
                    }
                }
            }
            catch { }

            return rules.ToArray();
        }


        public bool DeleteAllWebDavRules(string organizationId, string folder)
        {
            try
            {
                using (ServerManager serverManager = new ServerManager())
                {

                    Configuration config = serverManager.GetApplicationHostConfiguration();

                    config.RemoveLocationPath(string.Format("{0}/{1}/{2}", _Setting.Domain, organizationId, folder));
                    serverManager.CommitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
