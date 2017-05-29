using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;
using AutoMapper;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core.Security.Authentication.Principals;
using SolidCP.WebDav.Core.Security.Cryptography;
using SolidCP.WebDavPortal.App_Start;
using SolidCP.WebDavPortal.Controllers;
using SolidCP.WebDavPortal.CustomAttributes;
using SolidCP.WebDavPortal.DependencyInjection;
using SolidCP.WebDavPortal.HttpHandlers;
using SolidCP.WebDavPortal.Mapping;
using SolidCP.Server.Utils;

namespace SolidCP.WebDavPortal
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Log.WriteStart("Application_Start");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new AccessTokenHandler());

            DependencyResolver.SetResolver(new NinjectDependecyResolver());

            AutoMapperPortalConfiguration.Configure();
            
            Mapper.AssertConfigurationIsValid();

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
               typeof(PhoneNumberAttribute),
               typeof(RegularExpressionAttributeAdapter));

            Log.WriteEnd("Application_Start");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            Server.ClearError();

            int statusCode;

            if (lastError.GetType() == typeof (HttpException))
                statusCode = ((HttpException) lastError).GetHttpCode();
            else
                statusCode = 500;

            var contextWrapper = new HttpContextWrapper(Context);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");
            routeData.Values.Add("statusCode", statusCode);
            routeData.Values.Add("exception", lastError);
            routeData.Values.Add("isAjaxRequet", contextWrapper.Request.IsAjaxRequest());

            IController controller = new ErrorController();
            var requestContext = new RequestContext(contextWrapper, routeData);
            controller.Execute(requestContext);
            Response.End();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var s = HttpContext.Current.Request;
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            Log.WriteStart("Application_PostAuthenticateRequest");

            if (!IsOwaRequest())
            {
                Log.WriteInfo("Try get HttpContext ...");
                var contextWrapper = new HttpContextWrapper(Context);

                Log.WriteInfo("Try get Auth-Cookie ...");
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                Log.WriteInfo("Try get Auth-Servive ...");
                var authService = DependencyResolver.Current.GetService<IAuthenticationService>();

                Log.WriteInfo("Try get Crypto-Service ...");
                var cryptography = DependencyResolver.Current.GetService<ICryptography>();

                if (authCookie != null)
                {
                    Log.WriteInfo("Found Auth-Cookie!");
                    Log.WriteInfo("Try to Decrpyt ...");
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    Log.WriteInfo("Try to get UserData from Auth-Cookie");
                    var serializer = new JavaScriptSerializer();
                    var principalSerialized = serializer.Deserialize<ScpPrincipal>(authTicket.UserData);

                    Log.WriteInfo("Try to Login ...");
                    authService.LogIn(principalSerialized.Login, cryptography.Decrypt(principalSerialized.EncryptedPassword));

                    if (!contextWrapper.Request.IsAjaxRequest())
                    {
                        SetAuthenticationExpirationTicket();
                    }
                }
                else
                {
                    Log.WriteWarning("Auth-Cookie is null");
                }
            }
            else
            {
                Log.WriteInfo("Is OWA Request!");
            }

            Log.WriteEnd("Application_PostAuthenticateRequest");
        }



        private bool IsOwaRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/owa");
        }

        public static void SetAuthenticationExpirationTicket()
        {
            Log.WriteStart("SetAuthenticationExpirationTicket");

            var expirationDateTimeInUtc = DateTime.UtcNow.AddMinutes(FormsAuthentication.Timeout.TotalMinutes).AddSeconds(1);
            var authenticationExpirationTicketCookie = new HttpCookie(WebDavAppConfigManager.Instance.AuthTimeoutCookieName);
            
            authenticationExpirationTicketCookie.Value = expirationDateTimeInUtc.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("F0");
            authenticationExpirationTicketCookie.HttpOnly = false; 
            authenticationExpirationTicketCookie.Secure = FormsAuthentication.RequireSSL;

            HttpContext.Current.Response.Cookies.Add(authenticationExpirationTicketCookie);

            Log.WriteEnd("SetAuthenticationExpirationTicket");
        }
    }
}