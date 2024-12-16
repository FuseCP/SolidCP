using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public class Utils
	{
		public static string GetDistributiveLocationInfo(string ccode, string cversion)
		{
			var service = Installer.Current.InstallerWebService;
			
			var info = service.GetReleaseFileInfo(ccode, cversion);
			
			if (info == null)
			{
				Log.WriteInfo("Component code: {0}; Component version: {1};", ccode, cversion);
				
				throw new ServiceComponentNotFoundException("Seems that the Service has no idea about the component requested.");
			}
			
			return info.FullFilePath;
		}
	}
}
