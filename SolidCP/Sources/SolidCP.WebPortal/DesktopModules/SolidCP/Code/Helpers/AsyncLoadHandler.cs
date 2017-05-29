using SolidCP.EnterpriseServer;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Web;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;

namespace SolidCP.Portal
{
    public class AsyncLoadHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.
            Context = context;
            context.Response.ContentType = "text/plain";
            context.Response.Write(GetData());
        }

        #endregion

        private int AccountId
        {
            get
            {
                return Convert.ToInt32(Context.Request.Params["accountId"]);
            }
        }

        private HttpContext Context
        {
            get;
            set;
        }

        private int ItemId
        {
            get
            {
                return Convert.ToInt32(Context.Request.Params["itemId"]);
            }
        }

        private string MethodName
        {
            get
            {
                return Context.Request.Params["method"];
            }
        }

        public AsyncLoadHandler()
        {
        }

        private string GetData()
        {
            string methodName = MethodName;

            if (methodName == "Litigation")
            {
                return GetLitigationImage();
            }
            if (methodName == "Archiving")
            {
                return GetArchivingImage();
            }
            if (methodName == "Mailflow")
            {
                return GetMailflowImage();
            }
            if (methodName == "Capacity")
            {
                return GetCapacityImage();
            }

            if (methodName == "ServerVersion")
            {
                return GetServerVersion();
            }
            if (methodName == "WebSiteStatus")
            {
                return GetWebSiteStatus();
            }
            if (methodName == "AppPoolStatus")
            {
                return GetAppPoolStatus();
            }
            if (methodName == "IsSSLEnabled")
            {
                return IsSSLEnabled();
            }
            if (methodName == "IsRedirection")
            {
                return IsRedirection();
            }
            if (methodName == "GetSSLExpDate")
            {
                return GetSSLExpDate();
            }

            return GetExchangeImage("blank16.gif");
        }

        private string GetServerVersion()
        {
            string serverVersion;
            try
            {
                serverVersion = ES.Services.Servers.GetServerVersion(ItemId);
            }
            catch (Exception exception)
            {
                serverVersion = "Server unavailable";
            }
            return serverVersion;
        }

        #region Set Directory
        private string GetWebSiteImage(string imageUrl)
        {
            return string.Concat("App_Themes/Default/Images/", imageUrl);
        }

        private string GetExchangeImage(string imageUrl)
        {
            return string.Concat("App_Themes/Default/Images/Exchange/", imageUrl);
        }

        private string GetExchangeIcon(string imageUrl)
        {
            return string.Concat("App_Themes/Default/Icons/", imageUrl);
        }
        #endregion

        #region Mail
        private string GetArchivingImage()
        {
            string result = string.Empty;
            ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(ItemId, AccountId);
            if (account != null && account.EnableArchiving)
            {
                result = "<span class=\"fa fa-archive fa-lg text-success\"></span>";
            }
            return result;
        }

        private string GetCapacityImage()
        {
            string result = string.Empty;
            ExchangeMailboxStatistics mailboxStatistics = ES.Services.ExchangeServer.GetMailboxStatistics(ItemId, AccountId);
            if (mailboxStatistics != null && mailboxStatistics.MaxSize > (long)0 && (double)mailboxStatistics.TotalSize / (double)mailboxStatistics.MaxSize > 0.9)
            {
                result = "<span class=\"fa fa-hdd-o fa-lg text-success\"></span>";
            }
            if (mailboxStatistics != null && mailboxStatistics.MaxSize < (long)0 && (double)mailboxStatistics.TotalSize / (double)mailboxStatistics.MaxSize > 0.9)
            {
                result = "<span class=\"fa fa-hdd-o fa-lg text-danger\"></span>";
            }
            return result;
        }

        private string GetLitigationImage()
        {
            string result = string.Empty;
            ExchangeMailbox mailboxGeneralSettings = ES.Services.ExchangeServer.GetMailboxGeneralSettings(ItemId, AccountId);
            if (mailboxGeneralSettings != null && mailboxGeneralSettings.EnableLitigationHold)
            {
                result = "<span class=\"fa fa-gavel fa-lg text-danger\" ></span>";
            }
            return result;
        }

        private string GetMailflowImage()
        {
            string result = string.Empty;
            ExchangeMailbox mailboxMailFlowSettings = ES.Services.ExchangeServer.GetMailboxMailFlowSettings(ItemId, AccountId);
            if (mailboxMailFlowSettings != null && mailboxMailFlowSettings.EnableForwarding)
            {
                result = "<span class=\"fa fa-arrow-circle-o-right fa-lg text-success\" ></span>";
            }
            return result;
        }
        #endregion

        #region Web
        private string GetWebSiteStatus()
        {

            string result = string.Empty;

            ServerState siteState = ES.Services.WebServers.GetSiteState(ItemId);
            try
            {
                if (siteState == ServerState.Started)
                {
                    result = "<span class=\"fa fa-play-circle-o fa-lg text-success\"></span>";
                }
                if (siteState == ServerState.Paused)
                {
                    result = "<span class=\"fa fa-pause-circle-o fa-lg text-warning\"></span>";
                }
                if (siteState == ServerState.Stopped)
                {
                    result = "<span class=\"fa fa-stop-circle-o fa-lg text-danger\"></span>";
                }

            }
            catch (Exception exception)
            {
                result = "<span class=\"fa fa-exclamation-triangle fa-lg text-danger\"></span>";
            }
            return result;
        }

        private string GetAppPoolStatus()
        {
            string result = string.Empty;
            try
            {
                AppPoolState appPoolState = ES.Services.WebServers.GetAppPoolState(ItemId);
                if (appPoolState == AppPoolState.Started)
                {
                    result = "<span class=\"fa fa-play-circle-o fa-lg text-success\"></span>";
                }
                if (appPoolState == AppPoolState.Stopped)
                {
                    result = "<span class=\"fa fa-stop-circle-o fa-lg text-danger\"></span>";
                }
            }
            catch (Exception exception)
            {
                result = "<span class=\"fa fa-exclamation-triangle fa-lg text-danger\"></span>";
            }
            return result;
        }

        protected string IsRedirection()
        {
            string result;
            try
            {
                if (!string.IsNullOrEmpty(ES.Services.WebServers.GetWebSite(ItemId).HttpRedirect))
                {
                    result = "<span class=\"fa fa-check fa-lg text-success\"></span>";
                }
                else
                {
                    result = "";
                }
                return result;
            }
            catch (Exception exception)
            {
                result = "";
            }
            return result;
        }
        #endregion

        #region Security
        protected string GetSSLExpDate()
        {
            string result;
            try
            {
                SSLCertificate[] certificatesForSite = null;
                certificatesForSite = ES.Services.WebServers.GetCertificatesForSite(ItemId);
                if (certificatesForSite.Length != 0)
                {
                    DateTime expiryDate = certificatesForSite[0].ExpiryDate;
                    DateTime datenow = DateTime.Today;
                    DateTime dateTime = new DateTime();
                    TimeSpan ts = expiryDate - DateTime.Now;
                    int diffInDays = ts.Days;
                    var SSLgood = (expiryDate - DateTime.Now).TotalDays > 30;
                    var SSLexpiring = (expiryDate - DateTime.Now).TotalDays < 30;
                    var SSLexpired = DateTime.Now > expiryDate;
                    // Success
                    if (SSLgood)
                    {
                        dateTime = certificatesForSite[0].ExpiryDate;
                        result = "<i class=\"fa fa-clock-o fa-lg text-success\"></i><span class=\"text-success\" > " + dateTime.ToShortDateString() + "</span>";
                        return result;
                    }
                    // Warning Less than 30 Days
                    else if (SSLexpiring)
                    {
                        dateTime = certificatesForSite[0].ExpiryDate;
                        result = "<i class=\"fa fa-clock-o fa-lg text-warning\"></i><span class=\"text-warning\" title=\"Expiring on " + dateTime.ToShortDateString() + "\" > " + diffInDays + " Days Left</span>";
                        return result;
                    }
                    // Danger Expired
                    else if (SSLexpired)
                    {
                        dateTime = certificatesForSite[0].ExpiryDate;
                        result = "<i class=\"fa fa-clock-o fa-lg text-danger\"></i><span class=\"text-danger\" title=\"Expired on " + dateTime.ToShortDateString() + "\" ><strong> EXPIRED</strong></span>";
                        return result;
                    }
                    else
                    {
                        // No SSL
                        result = "";
                    }
                }
                else
                {
                    // No SSL
                    result = "<span class=\"text-danger\" >NO SSL</span>";
                }
                return result;
            }
            catch (Exception exception)
            {
                result = "";
            }
            return result;
        }

        protected string IsSSLEnabled()
        {
            string result;
            try
            {
                if (ES.Services.WebServers.GetCertificatesForSite(ItemId).Length == 1)
                {
                    result = "<span class=\"fa fa-lock fa-lg text-success\"></span>";
                }
                else
                {
                    result = "<span class=\"fa fa-unlock fa-lg text-danger\"></span>";
                }

                return result;
            }
            catch (Exception exception)
            {
                result = "";
            }
            return result;
        }
        #endregion

    }
}