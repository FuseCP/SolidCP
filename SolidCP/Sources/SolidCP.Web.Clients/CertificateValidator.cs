using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Client
{
	public class CertificateValidator
	{
		private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
		{
#if DEBUG
			//return true;
			return error == SslPolicyErrors.None;
#else
			return error == SslPolicyErrors.None;
#endif
		}

		public static void Init()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
			ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
		} 
	}
}
