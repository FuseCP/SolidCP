#if NETCOREAPP

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SolidCP.Web.Services;
using SolidCP.Web.Clients;
public class Program
{

	public static void Main(string[] args)
	{
		Configuration.IsPortal = true;
		Server.UseWebForms = app =>
		{
			app.UseWebForms();
			AssemblyLoader.Init(Configuration.ProbingPaths, Configuration.ExposeWebServices, true);
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