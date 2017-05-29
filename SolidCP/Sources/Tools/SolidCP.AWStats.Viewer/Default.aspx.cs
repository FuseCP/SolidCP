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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using System.IO;
using System.Net;

namespace SolidCP.AWStats.Viewer
{
    public partial class Default : System.Web.UI.Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
            string username = Request["username"];
            string password = Request["password"];

            if (Request.IsAuthenticated)
            {
                string identity = Context.User.Identity.Name;
                string domain = identity.Split('=')[0];
                
                if (String.Compare(Request["config"], domain, true) != 0)
                {
                    FormsAuthentication.SignOut();
                    domain = Request["domain"];
                    if (!String.IsNullOrEmpty(domain)
                        && !String.IsNullOrEmpty(username)
                        && !String.IsNullOrEmpty(password))
                    {
                        // perform login
                        txtUsername.Text = username;
                        txtDomain.Text = domain;
                        Login(domain, username, password);
                    }
                    else
                    {
                        Response.Redirect(Request.Url.AbsolutePath);
                    }
                }
                Response.Clear();

                string queryParams = Request.Url.Query;

                string awStatsUrl = AWStatsUrl;
                if (awStatsUrl.IndexOf(":") == -1)
                {
                    string appUrl = Request.Url.ToString();
                    appUrl = appUrl.Substring(0, appUrl.LastIndexOf("/"));
                    awStatsUrl = appUrl + awStatsUrl;
                }

                string awStatsPage = GetWebDocument(awStatsUrl + queryParams);

                // replace links
                awStatsPage = awStatsPage.Replace(AWStatsScript, Request.Url.AbsolutePath);
                Response.Write(awStatsPage);

                Response.End();
            }
            else
            {
                lblMessage.Visible = false;               
            
                if (!IsPostBack)
                {
                    string domain = Request["domain"];

                    if (String.IsNullOrEmpty(domain))
                        domain = Request["config"];

                    txtDomain.Text = domain;

                    if (!String.IsNullOrEmpty(username))
                        txtUsername.Text = username;

                    // check for autologin
                    if (!String.IsNullOrEmpty(domain)
                        && !String.IsNullOrEmpty(username)
                        && !String.IsNullOrEmpty(password))
                    {
                        // perform login
                        Login(domain, username, password);
                    }
                }
            }  
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

			// perform login
			Login(txtDomain.Text.Trim(), txtUsername.Text.Trim(), txtPassword.Text);
        }

		private void Login(string domain, string username, string password)
		{
			// check user
			AuthenticationResult result =
				AuthenticationProvider.Instance.AuthenticateUser(domain, username, password);

			if (result == AuthenticationResult.OK)
			{
				FormsAuthentication.SetAuthCookie(domain + "=" + username, false);
				Response.Redirect(Request.Url.AbsolutePath + "?config=" + domain);
			}

			// show error message
			lblMessage.Text = ConfigurationManager.AppSettings["AWStats.Message.DomainNotFound"];
			if (result == AuthenticationResult.WrongUsername)
				lblMessage.Text = ConfigurationManager.AppSettings["AWStats.Message.WrongUsername"];
			else if (result == AuthenticationResult.WrongPassword)
				lblMessage.Text = ConfigurationManager.AppSettings["AWStats.Message.WrongPassword"];

			lblMessage.Visible = true;
		}

        #region Private Helpers
        private string GetWebDocument(string url)
        {
            HttpWebResponse result = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                string lang = Request.Headers["Accept-Language"];
                req.Headers["Accept-Language"] = lang;

                string username = ConfigurationManager.AppSettings["AWStats.Username"];
                if (username != null && username != "")
                {
                    string password = ConfigurationManager.AppSettings["AWStats.Password"];
                    string domain = null;
                    int sepIdx = username.IndexOf("\\");
                    if (sepIdx != -1)
                    {
                        domain = username.Substring(0, sepIdx);
                        username = username.Substring(sepIdx + 1);
                    }

                    req.Credentials = new NetworkCredential(username, password, domain);
                }
                else
                {
                    req.Credentials = CredentialCache.DefaultNetworkCredentials;
                }

                result = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                string respEnc = !String.IsNullOrEmpty(result.ContentEncoding) ? result.ContentEncoding : "utf-8";
                Encoding encode = System.Text.Encoding.GetEncoding(respEnc);

                StreamReader sr = new StreamReader(ReceiveStream, encode);

                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);

                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    sb.Append(str);
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (WebException ex)
            {
                Response.Write("Error while opening '" + url + "': " + ex.Message);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }

            return sb.ToString();
        }

        private string AWStatsUrl
        {
            get { return ConfigurationManager.AppSettings["AWStats.URL"]; }
        }

        private string AWStatsScript
        {
            get
            {
                int idx = AWStatsUrl.LastIndexOf("/");
                return (idx == -1) ? AWStatsUrl : AWStatsUrl.Substring(idx + 1);
            }
        }
        #endregion
    }
}
