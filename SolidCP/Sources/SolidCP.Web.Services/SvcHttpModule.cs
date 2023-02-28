#if NETFRAMEWORK
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SolidCP.Web.Services
{
	internal class SvcHttpModule : IHttpModule
	{
		public void Dispose() { }

		public void Init(HttpApplication context)
		{
			context.PostResolveRequestCache += MapHandler;
		}

		public void MapHandler(object sender, EventArgs args) {
			var context = (HttpApplication)sender;
			var svc = DictionaryVirtualPathProvider.Current.GetObject(context.Context.Request.Path);
			if (svc is SvcFile)
			{
				var factory = (IHttpHandlerFactory)Activator.CreateInstance(
						Type.GetType("System.ServiceModel.Activation.ServiceHttpHandlerFactory, System.ServiceModel.Activation, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = 31bf3856ad364e35"));
				context.Context.RemapHandler(factory.GetHandler(context.Context, context.Request.HttpMethod, context.Request.RawUrl, context.Request.Path));
			}
		}
	}
}
#endif