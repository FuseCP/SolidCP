using System.Reflection;
using SolidCP.Providers.OS;

namespace SolidCP.Server
{
	public class PreloadSpecificAssemblies
	{
		public static void Init()
		{
			if (OSInfo.IsWindows)
			{
				if (OSInfo.WindowsVersion >= WindowsVersion.WindowsServer2012)
				{
					Assembly.Load("System.Management.Automation, Version=3.0.0");
				} else
				{
					Assembly.Load("System.Management.Automation, Version=1.0.0");
				}
			}
		}
	}
}
