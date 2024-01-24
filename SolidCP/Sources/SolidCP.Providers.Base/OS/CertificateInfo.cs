using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace SolidCP.Providers.OS
{
	public class CertificateInfo
	{
		public StoreLocation Location { get; set; }
		public StoreName Name { get; set; }
		public X509FindType FindType { get; set; }
		public string FindValue { get; set; }

		public byte[] File { get; set; }
		public string Password { get; set; }
		
		public string LetsEncryptHosts { get; set; }
		public string LetsEncryptEmail { get; set; }
	}
}
