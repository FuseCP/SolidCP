using System;
using System.Collections.Generic;
using SolidCP.Providers.OS;

#if !NETFRAMEWORK
using CoreWCF;
using CoreWCF.Channels;
using Microsoft.AspNetCore.Builder;
#else
using System.Web;
#endif

namespace SolidCP.Web.Services
{
	public static class Server
	{
#if !NETFRAMEWORK
		public static string WebRoot { get; set; } = null;
		public static string ContentRoot { get; set; } = null;
		public static string MapPath(string path) => path.Replace("~", ContentRoot);
		public static string UserHostAddress
		{
			get
			{
				OperationContext context = OperationContext.Current;
				if (context == null) return "127.0.0.1";
				MessageProperties prop = context.IncomingMessageProperties;
				RemoteEndpointMessageProperty endpoint =
						 prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
				string ip = endpoint?.Address ?? "127.0.0.1";
				return ip;
			}
		}

		public static Action<WebApplication> UseWebForms = null;
#else
		public static string UserHostAddress => System.Web.HttpContext.Current.Request.UserHostAddress;
		public static string MapPath(string path) => System.Web.Hosting.HostingEnvironment.MapPath(path);
#endif

		public static readonly Dictionary<string, object> Cache = new Dictionary<string, object>();
	}
}
