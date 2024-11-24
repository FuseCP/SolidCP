using System;
using System.Runtime.InteropServices;

namespace SolidCP.Web.Services;

internal static class NativeMethods
{
	internal const string AspNetCoreModuleDll = "aspnetcorev2_inprocess.dll";

	[DllImport("kernel32.dll")]
	private static extern IntPtr GetModuleHandle(string lpModuleName);

	public static bool IsAspNetCoreModuleLoaded()
	{
		return GetModuleHandle(AspNetCoreModuleDll) != IntPtr.Zero;
	}
}