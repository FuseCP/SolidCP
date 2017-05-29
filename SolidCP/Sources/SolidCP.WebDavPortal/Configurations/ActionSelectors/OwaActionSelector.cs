using System.Linq;
using System.Web.Http.Controllers;

namespace SolidCP.WebDavPortal.Configurations.ActionSelectors
{
    public class OwaActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            if (controllerContext.Request.Headers.Contains("X-WOPI-Override"))
            {
                var matchingHeaders = controllerContext.Request.Headers.GetValues("X-WOPI-Override");
                var headerValue = (matchingHeaders == null) ? "" : (matchingHeaders.FirstOrDefault() ?? "");

                if (!string.IsNullOrEmpty(headerValue))
                {
                    controllerContext.RouteData.Values["action"] = headerValue;
                }
            }

            return base.SelectAction(controllerContext);
        }
    }
}