#if NETCOREAPP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting.Systemd;

namespace SolidCP.EnterpriseServer.Code;


public class ScheduleWorker: BackgroundService
{
	public bool IsSystemd = false;
	public bool IsWindowsService = false;
	public static bool Collect = false;
	public static bool RunAsService = false;
	public bool IsService => RunAsService || IsWindowsService || IsSystemd;

	public ILogger<ScheduleWorker> Log;

	public ScheduleWorker(ILogger<ScheduleWorker> logger, IHostLifetime lifetime)
	{
		IsSystemd = lifetime is SystemdLifetime;
		IsWindowsService = lifetime is WindowsServiceLifetime;
		Log = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (!IsService) return;

		Log.LogInformation("Scheduler Service started...");

		await Task.Delay(5000, stoppingToken);

		int runs = 0;

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				using (var scheduler = new Scheduler())
				{
					scheduler.Start();
				}
				if (Collect && ++runs >= 10)
				{
					runs = 0;
					GC.Collect();
				}
			}
			catch { }

			await Task.Delay(5000, stoppingToken);
		}
	}
}
#endif