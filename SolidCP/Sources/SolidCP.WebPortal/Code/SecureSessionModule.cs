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
using System.Web;
using System.Web.Security;
using System.Web.Caching;
using System.Configuration;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Globalization;
using System.Text;

namespace SolidCP.WebPortal
{
    public class SecureSessionModule : IHttpModule
    {
        public const string DEFAULT_PAGE = "~/Default.aspx";
        public const string PAGE_ID_PARAM = "pid";

        private static string _ValidationKey = null;

        public void Init(HttpApplication app)
        {
            // Initialize validation key if not already initialized 
            if (_ValidationKey == null)
                _ValidationKey = GetValidationKey();

            // Register handlers for BeginRequest and EndRequest events 
            app.BeginRequest += new EventHandler(OnBeginRequest);
            app.EndRequest += new EventHandler(OnEndRequest);
        }

        public void Dispose() { }

        void OnBeginRequest(Object sender, EventArgs e)
        {
            // Look for an incoming cookie named "ASP.NET_SessionID" 
            HttpRequest request = ((HttpApplication)sender).Request;
            HttpCookie cookie = GetCookie(request, "ASP.NET_SessionId");
            HttpCookie authCookie = request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null)
            {
                // Throw an exception if the cookie lacks a MAC 
                if (cookie.Value.Length <= 24)
                {
                    if ((authCookie != null))
                    {
                        SolidCP.Portal.PortalUtils.UserSignOut();
                    } 
                    return;
                }

                // Separate the session ID and the MAC 
                string id = cookie.Value.Substring(0, 24);
                string mac1 = cookie.Value.Substring(24);

                // Generate a new MAC from the session ID and requestor info 
                string mac2 = GetSessionIDMac(id, request.UserHostAddress,
                    request.UserAgent, _ValidationKey);

                // Throw an exception if the MACs don't match 
                if (String.CompareOrdinal(mac1, mac2) != 0)
                {
                    SolidCP.Portal.PortalUtils.UserSignOut();
                }

                // Strip the MAC from the cookie before ASP.NET sees it 
                cookie.Value = id;
            }
        }

        void OnEndRequest(Object sender, EventArgs e)
        {
            // Look for an outgoing cookie named "ASP.NET_SessionID" 
            HttpRequest request = ((HttpApplication)sender).Request;
            HttpCookie cookie = GetCookie( request, "ASP.NET_SessionId");

            if (cookie != null)
            {
                // Add a MAC 
                cookie.Value += GetSessionIDMac(cookie.Value,
                    request.UserHostAddress, request.UserAgent,
                    _ValidationKey);
            }
        }

        private string GetValidationKey()
        {
            string key = ConfigurationManager.AppSettings["SessionValidationKey"];
            if (key == null || key == String.Empty)
                throw new InvalidSessionException
                    ("SessionValidationKey missing");
            return key;
        }

        private HttpCookie GetCookie(HttpRequest request, string name)
        {
            HttpCookieCollection cookies = request.Cookies;
            return FindCookie(cookies, name);
        }

        private HttpCookie GetCookie(HttpResponse response, string name)
        {
            HttpCookieCollection cookies = response.Cookies;
            return FindCookie(cookies, name);
        }

        private HttpCookie FindCookie(HttpCookieCollection cookies,
            string name)
        {
            int count = cookies.Count;

            for (int i = 0; i < count; i++)
            {
                if (String.Compare(cookies[i].Name, name, true,
                    CultureInfo.InvariantCulture) == 0)
                    return cookies[i];
            }

            return null;
        }

        private string GetSessionIDMac(string id, string ip,
            string agent, string key)
        {
            StringBuilder builder = new StringBuilder(id, 512);
            builder.Append(ip);
            builder.Append(agent);

            using (HMACSHA1 hmac = new HMACSHA1
                (Encoding.UTF8.GetBytes(key)))
            {
                return Convert.ToBase64String(hmac.ComputeHash
                   (Encoding.UTF8.GetBytes(builder.ToString())));
            }
        }
    }

    [Serializable]
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException() :
            base("Session cookie is invalid") { }

        public InvalidSessionException(string message) :
            base(message) { }

        public InvalidSessionException(string message,
            Exception inner)
            : base(message, inner) { }

        protected InvalidSessionException(SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }
}
