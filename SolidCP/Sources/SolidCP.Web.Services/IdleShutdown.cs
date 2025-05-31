#if NETCOREAPP
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
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

	public IdleShutdownService(IHostApplicationLifetime lifetime)
	{
		Lifetime = lifetime;
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