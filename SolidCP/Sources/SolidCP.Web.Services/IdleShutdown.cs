#if NETCOREAPP
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.Web.Services;

public class IdleShutdownService : BackgroundService
{
	private readonly IHostApplicationLifetime Lifetime;
	private static DateTime LastRequestTime = DateTime.UtcNow;
	public static readonly TimeSpan DefaultIdleTimeout = TimeSpan.FromMinutes(5); // Set your idle timeout
	public static TimeSpan IdleTimeout = DefaultIdleTimeout; // Set your idle timeout
	public ISystemdNotifier SystemdNotifier { get; set; }

	public IdleShutdownService(IHostApplicationLifetime lifetime, ISystemdNotifier notifier)
	{
		Lifetime = lifetime;
		SystemdNotifier = notifier;
		Lifetime.ApplicationStarted.Register(() => SystemdNotifier.Notify(ServiceState.Ready));
	}

	public static void UpdateLastRequestTime()
	{
		LastRequestTime = DateTime.UtcNow;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var idleTime = DateTime.UtcNow - LastRequestTime;
			if (idleTime > IdleTimeout)
			{
				Console.WriteLine("Idle timeout reached. Shutting down.");
				SystemdNotifier.Notify(ServiceState.Stopping);
				Lifetime.StopApplication();
				break;
			}

			await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
		}
	}
}

public class ActivityTrackingMiddleware
{
	private readonly RequestDelegate Next;

	public ActivityTrackingMiddleware(RequestDelegate next)
	{
		Next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		IdleShutdownService.UpdateLastRequestTime();
		await Next(context);
		IdleShutdownService.UpdateLastRequestTime();
	}
}

public static class ApplicationBuilderExtensions
{
	public static void UseIdleTimeout(this IApplicationBuilder app, TimeSpan idleTimeout = default)
	{
		if (idleTimeout != default) IdleShutdownService.IdleTimeout = idleTimeout;
		app.UseMiddleware<ActivityTrackingMiddleware>();
	}
}
#endif