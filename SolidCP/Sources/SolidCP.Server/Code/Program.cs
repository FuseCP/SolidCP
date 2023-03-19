#if !NETFRAMEWORK
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SolidCP.Server
{

	public static class Program
	{

		public static void Main(string[] args)
		{
			SolidCP.Web.Services.CoreWebServicesApp.Init(args);
		}
	}
}

#endif