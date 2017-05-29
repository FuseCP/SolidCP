using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SolidCP.WebDavPortal.DependencyInjection.Providers
{
    public class HttpSessionStateProvider : Provider<HttpSessionState>
    {
        protected override HttpSessionState CreateInstance(IContext context)
        {
            return HttpContext.Current.Session;
        }
    }
}