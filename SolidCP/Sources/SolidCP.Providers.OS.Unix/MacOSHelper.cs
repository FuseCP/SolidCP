using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SolidCP.Providers.OS
{
	public static class MacOSHelper
	{
		// Constants for host_statistics
		const int HOST_CPU_LOAD_INFO = 3;
		const int HOST_CPU_LOAD_INFO_COUNT = (int)(CPU_STATE_MAX);

		const int CPU_STATE_USER = 0;
		const int CPU_STATE_SYSTEM = 1;
		const int CPU_STATE_IDLE = 2;
		const int CPU_STATE_NICE = 3;
		const int CPU_STATE_MAX = 4;

		[StructLayout(LayoutKind.Sequential)]
		struct host_cpu_load_info
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = CPU_STATE_MAX)]
			public uint[] cpu_ticks;
		}

		[DllImport("/usr/lib/libSystem.dylib")]
		static extern int host_statistics(IntPtr host, int flavor, IntPtr cpuLoadInfo, ref int count);

		[DllImport("/usr/lib/libSystem.dylib")]
		static extern IntPtr mach_host_self();

		public static short GetProcessorTotalProcessorTimeMac()
		{
			var (idle1, total1) = GetCpuStatsMac();
			Thread.Sleep(1000);
			var (idle2, total2) = GetCpuStatsMac();

			int idleDiff = (int)idle2 - (int)idle1;
			int totalDiff = (int)total2 - (int)total1;

			int idlePercent = 100 * idleDiff / totalDiff;

			return (short)(100 - idlePercent);
		}

		static (uint idleTicks, uint totalTicks) GetCpuStatsMac()
		{
			var cpuInfo = new host_cpu_load_info { cpu_ticks = new uint[CPU_STATE_MAX] };
			int count = HOST_CPU_LOAD_INFO_COUNT;
			int size = Marshal.SizeOf<host_cpu_load_info>();
			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(cpuInfo, ptr, false);

			int result = host_statistics(mach_host_self(), HOST_CPU_LOAD_INFO, ptr, ref count);
			if (result != 0)
				throw new Exception("host_statistics failed.");

			cpuInfo = Marshal.PtrToStructure<host_cpu_load_info>(ptr);
			Marshal.FreeHGlobal(ptr);

			uint user = cpuInfo.cpu_ticks[CPU_STATE_USER];
			uint system = cpuInfo.cpu_ticks[CPU_STATE_SYSTEM];
			uint idle = cpuInfo.cpu_ticks[CPU_STATE_IDLE];
			uint nice = cpuInfo.cpu_ticks[CPU_STATE_NICE];

			uint total = user + system + idle + nice;

			return (idle, total);
		}
	}
}
