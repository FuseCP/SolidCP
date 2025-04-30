#if NETCOREAPP
using Microsoft.Extensions.DependencyInjection;

namespace SolidCP.EnterpriseServer.Code
{
	public static class Initializer
	{
		public static void Init(IServiceCollection services)
		{
			services.AddHostedService<ScheduleWorker>();
		}
	}
}
#endif