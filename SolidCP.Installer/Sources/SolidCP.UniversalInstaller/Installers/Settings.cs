using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{

	public class CommonSettings
	{
		public string Urls { get; set; }
		public string CertificateStoreName { get; set; }
		public string CertificateStoreLocation { get; set; }
		public string CertificateFindValue { get; set; }
		public string CertificateFindType { get; set; }
		public string CertificateFile { get; set; }
		public string CertificatePassword { get; set; }
		public string LetsEncryptCertificateDomains { get; set; }
		public string LetsEncryptCertificateEmail { get; set; }
	}
	public class ServerSettings: CommonSettings
	{
		public string ServerPassword { get; set; }
	}

	public class EnterpriseServerSettings: CommonSettings
	{
		public string EnterpriseUser { get; set; }
		public string EnterpriseUserPassword { get; set; }
		public string DatabaseServer { get; set; }
		public string DatabaseUser { get; set; }
		public string DatabasePassword { get; set; }
	}

	public class WebPortalSettings: CommonSettings
	{
		public string EnterpriseServerUrl { get; set; }
		public string PortalUser { get; set; }
		public string PortalUserPassword { get; set; }
	}
}
