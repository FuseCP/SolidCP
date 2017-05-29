using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using SolidCP.WebDavPortal.DependencyInjection;
using SolidCP.WebDavPortal.Models;
using SolidCP.WebDavPortal.UI.Routes;

namespace SolidCP.WebDavPortal.CustomAttributes
{
    public class LdapAuthorizationAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(AccountRouteNames.Login, null);
        }
    }
}