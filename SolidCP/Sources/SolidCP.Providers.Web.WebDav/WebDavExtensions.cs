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
using Microsoft.Web.Administration;

namespace SolidCP.Providers.Web.Extensions
{
    public static class WebDavExtensions
    {
        public static WebDavFolderRule ToWebDavFolderRule(this ConfigurationElement element)
        {
            var result = new WebDavFolderRule();

            if(!string.IsNullOrEmpty(element["users"].ToString()))
            {
                var users = element["users"].ToString().Split(',');

                foreach (var user in users)
                {
                    result.Users.Add(user.Trim());
                }
            }

            if (!string.IsNullOrEmpty(element["roles"].ToString()))
            {
                var roles = element["roles"].ToString().Split(',');

                foreach (var role in roles)
                {
                    result.Roles.Add(role.Trim());
                }
            }

            if (!string.IsNullOrEmpty(element["path"].ToString()))
            {
                var pathes = element["path"].ToString().Split(',');

                foreach (var path in pathes)
                {
                    result.Pathes.Add(path.Trim());
                }
            }

            var access = (int)element["access"] ;

            result.Write = (access & (int)WebDavAccess.Write) == (int)WebDavAccess.Write;
            result.Read = (access & (int)WebDavAccess.Read) == (int)WebDavAccess.Read;
            result.Source = (access & (int)WebDavAccess.Source) == (int)WebDavAccess.Source;

            return result;
        }

        public static bool ExistsWebDavRule(this ConfigurationElementCollection collection, WebDavFolderRule settings)
        {
            return collection.FindWebDavRule(settings) != null;
        }

        public static ConfigurationElement FindWebDavRule(this ConfigurationElementCollection collection, WebDavFolderRule settings)
        {
            return collection.FirstOrDefault(x =>
            {
                var s = x["users"].ToString();
                if (settings.Users.Any()
                    && x["users"].ToString() != string.Join(", ", settings.Users.ToArray()))
                {
                    return false;
                }

                if (settings.Roles.Any()
                    && x["roles"].ToString() != string.Join(", ", settings.Roles.ToArray()))
                {
                    return false;
                }

                if (settings.Pathes.Any()
                    && x["path"].ToString() != string.Join(", ", settings.Pathes.ToArray()))
                {
                    return false;
                }

                //if ((int)x["access"] != settings.AccessRights)
                //{
                //    return false;
                //}

                return true;
            });
        }
    }
}
