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
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mail;


namespace SEPlugin
{
    public enum ExecResult
    {
        Success = 0,
        Error = 1,
        AlreadyExists = 2,
        NotFound = 3,
        None = 4
    }

    public class SE
    {
        public static string DefaultEmptyPass = "123123";

        private static ExecResult CheckSuccess(string result)
        {
            if (result == null) return ExecResult.None;

            if (result.Length >= 10)
            {
                if (result.Substring(0, 10) == "EXCEPTION:") return ExecResult.Error;
            }
            if (result.Contains("SUCCESS")) return ExecResult.Success;
            if (result.Contains("already exists")) return ExecResult.AlreadyExists;
            if (result.Contains("Unable to find")) return ExecResult.NotFound;
            if (result.Contains("No such")) return ExecResult.NotFound;

            return ExecResult.Error;
        }

        private static string ExecCommand(string command, params string[] param)
        {
            string commandInfo = "exec command "+command;

            UriBuilder uri = new UriBuilder();
            uri.Scheme = Config.Instance.schema;
            uri.Host = Config.Instance.url;
            //uri.UserName = Config.Instance.user;
            //uri.Password = Config.Instance.password;

            string path = "/api/" + command + "/";
            int paramCount = param.Length / 2;
            for (int i=0;i<paramCount;i++)
            {
                string name = param[i * 2];
                string val = param[i * 2 + 1];

                path += name + "/" + HttpUtility.UrlEncode(val) + "/";

                if ((name!="password")||(Config.Instance.ExtendedLog))
                {
                    commandInfo += " " + name + "='" + val + "'";
                }
                    
            }
            uri.Path = path;

            Log.Write(commandInfo);

            string result = string.Empty;
            try
            {
                System.Net.WebClient Client = new WebClient();

                Client.Credentials = new NetworkCredential(Config.Instance.user, Config.Instance.password);

                using (Stream strm = Client.OpenRead( uri.Uri ))
                {
                    StreamReader sr = new StreamReader(strm);
                    result = sr.ReadToEnd();
                }

                if (result == null) result = "";

                Log.Write(result);
            }
            catch(Exception exc)
            {
                result = "EXCEPTION:" + exc.ToString();
                Log.WriteError(exc);
            }


            // send error message
            if (CheckSuccess(result)==ExecResult.Error)
            {
                SendMail(Config.Instance.ErrorMailSubject,
                        String.Format(Config.Instance.ErrorMailBody, commandInfo, result)
                    );
            }

            return result;
        }

        static public ExecResult AddDomain(string domain, string password, string email, string[] destinations, out string result)
        {
            Log.Write("AddDomain");

            if (String.IsNullOrEmpty(password))
                password = DefaultEmptyPass;

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

            return CheckSuccess(result);
        }

        static public ExecResult AddDomain(string domain, string password, string email, string[] destinations)
        {
            string result;
            return AddDomain(domain, password, email, destinations, out result);
        }


        static public ExecResult DeleteDomain(string domain, out string result)
        {
            Log.Write("DeleteDomain");

            result = ExecCommand("domainuser/remove", "username", domain);
            result = ExecCommand("domain/remove", "domain", domain);

            return CheckSuccess(result);
        }

        static public ExecResult DeleteDomain(string name)
        {
            string result;
            return DeleteDomain(name, out result);
        }

        static public ExecResult AddEmail(string name, string domain, string password, out string result)
        {
            Log.Write("AddEmail");

            if (String.IsNullOrEmpty(password))
                password = DefaultEmptyPass;

            result = ExecCommand("emailusers/add", "username", name, "password", password, "domain", domain);

            return CheckSuccess(result);
        }

        static public ExecResult AddEmail(string name, string domain, string password)
        {
            string result;
            return AddEmail(name, domain, password, out result);
        }

        static public ExecResult DeleteEmail(string email, out string result)
        {
            Log.Write("DeleteEmail");

            result = ExecCommand("emailusers/remove", "username", email);

            return CheckSuccess(result);
        }

        static public ExecResult DeleteEmail(string email)
        {
            string result;
            return DeleteEmail(email, out result);
        }

        static public ExecResult SetEmailPassword(string email, string password, out string result)
        {
            Log.Write("SetEmailPassword");

            if (String.IsNullOrEmpty(password))
                password = DefaultEmptyPass;

            result = ExecCommand("emailusers/setpassword", "username", email, "password", password);

            return CheckSuccess(result);
        }

        static public ExecResult SetEmailPassword(string email, string password)
        {
            string result;
            return SetEmailPassword(email, password, out result);
        }

        static public ExecResult SetDomainUserPassword(string name, string password)
        {
            Log.Write("SetDomainUserPassword");

            if (String.IsNullOrEmpty(password))
                password = DefaultEmptyPass;

            string result = ExecCommand("domainuser/setpassword", "username", name, "password", password);

            return CheckSuccess(result);
        }

        static public ExecResult SetDomainDestinations(string name, string[] destinations, out string result)
        {
            result = "";
            if (destinations.Length == 0) return ExecResult.None;

            string list = "[\"" + String.Join("\",\"", destinations) + "\"]";
            result = ExecCommand("domain/edit", "domain", name, "destinations", list);
            return CheckSuccess(result);
        }

        static public ExecResult SetDomainDestinations(string name, string[] destinations)
        {
            string result;
            return SetDomainDestinations(name, destinations, out result);
        }


        static private String SendMail(String subject, String Message)
        {
            try
            {
                if (String.IsNullOrEmpty(Config.Instance.MailServer))
                    return "";

                SmtpClient client = new SmtpClient(Config.Instance.MailServer, 25);
                client.Send(Config.Instance.MailFrom, Config.Instance.MailTo, subject, Message);
                return "";
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                return ex.Message;
            }

        }

    }
}
