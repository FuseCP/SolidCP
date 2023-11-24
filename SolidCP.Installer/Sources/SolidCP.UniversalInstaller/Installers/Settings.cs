using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public class ServerSettings
	{
		public string Urls { get; set; }
		public string ServerUser { get; set; }
		public string ServerUserPassword { get; set; }
		public string ServerPassword { get; set; }

	}

	public class EnterpriseServerSettings
	{
		public string Urls { get; set; }
		public string EnterpriseUser { get; set; }
		public string EnterpriseUserPassword { get; set; }
		public string DatabaseServer { get; set; }
		public string DatabaseUser { get;}
		public string DatabasePassword { get; set; }
	}

	public class WebPortalSettings
	{
		public string Urls { get; set; }
		public string EnterpriseServerUrl { get; set; }
		public string PortalUser { get; set; }
		public string PortalUserPassword { get;}
	}

}
