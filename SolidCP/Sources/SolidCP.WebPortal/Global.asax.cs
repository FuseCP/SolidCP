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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Security.Principal;

namespace SolidCP.WebPortal
{
    public class Global : System.Web.HttpApplication
    {
		protected void Application_PostAuthorizeRequest(Object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated == true && Request.RawUrl.IndexOf("WebResource.axd") == -1)
            {
				FormsAuthenticationTicket authTicket = (FormsAuthenticationTicket)Context.Items[FormsAuthentication.FormsCookieName];

				string roleName = String.Empty;

				if (authTicket == null)
				{
					HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
					if (authCookie != null)
					{
						authTicket = FormsAuthentication.Decrypt(authCookie.Value);
						Context.Items[FormsAuthentication.FormsCookieName] = authTicket;

						int index = authTicket.UserData.IndexOf(Environment.NewLine);

						if (index > -1)
							roleName = authTicket.UserData.Substring(index + Environment.NewLine.Length);
					}
				}

				string[] roles = null;

                switch (roleName)
                {
                    case "Administrator":
                        roles = new string[] { "Administrator", "PlatformHelpdesk", "PlatformCSR", "Reseller", "ResellerCSR", "ResellerHelpdesk", "User" };
                        break;
                    case "Reseller":
                        roles = new string[] { "Reseller", "ResellerCSR", "ResellerHelpdesk", "User" };
                        break;
                    case "PlatformCSR":
                        roles = new string[] { "PlatformCSR", "ResellerCSR", "ResellerHelpdesk", "User" };
                        break;
                    case "PlatformHelpdesk":
                        roles = new string[] { "PlatformHelpdesk", "ResellerHelpdesk", "User" };
                        break;
                    case "ResellerCSR":
                        roles = new string[] { "ResellerCSR", "User" };
                        break;
                    case "ResellerHelpdesk":
                        roles = new string[] { "ResellerHelpdesk", "User" };
                        break;
                    default:
                        roles = new string[] { "User" };
                        break;
                }

				HttpContext.Current.User = new GenericPrincipal(HttpContext.Current.User.Identity, roles);
            }

        }

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
