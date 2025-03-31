#if NETCOREAPP

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SolidCP.Web.Services;
using SolidCP.Web.Clients;
public class Program
{

	public static void Main(string[] args)
	{
		Configuration.IsPortal = true;
		Server.ConfigureApp = app =>
		{
			app.UseWebForms(options => options.AddHandleExtensions(".less"));
			AssemblyLoader.Init(Configuration.ProbingPaths, Configuration.ExposeWebServices, true);
		};
		Server.ConfigureServices = services =>
		{
			var es = Assembly.Load("SolidCP.EnterpriseServer");
			if (es != null)
			{
				var initializer = es.GetType("SolidCP.EnterpriseServer.Code.Initializer");
				var init = initializer.GetMethod("Init");
				init?.Invoke(null, new[] { services });
			}
		};
		StartupCore.Init(args);
		
		/*var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		//builder.Services.AddRazorPages();
		//builder.Services.AddControllersWithViews();

		var app = builder.Build();

		app.UseStaticFiles();

		//app.UseAuthorization();

		app.UseWebForms();

		//app.MapDefaultControllerRoute();

		app.Run();*/

	}
}
#endif