using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
	public class Configuration
	{
		public const int KB = 1024;
		public const int MB = 1024 * 1024;
		public const int MaxReceivedMessageSize = 32 * MB;
		public const int MaxBufferSize = 32 * MB;
		public const int MaxBytesPerRead = 4 * KB;
		public const int MaxDepth = 1024;
		public const int MaxArrayLength = 1 * MB;
		public const int MaxStringContentLength = 16 * MB;
		public const int MaxNameTableCharCount = 16 * MB;

		public const bool AllowInsecureHttp = PolicyAttribute.AllowInsecureHttp;

		public static int? HttpPort = null;
		public static int? HttpsPort = null;
		public static int? NetTcpPort = null;
		public static string HttpHost = null;
		public static string HttpsHost = null;
		public static string NetTcpHost = null;
		public static StoreLocation StoreLocation = StoreLocation.LocalMachine;
		public static StoreName StoreName = StoreName.My;
		public static X509FindType FindType = X509FindType.FindBySubjectName;
		public static string Name = null;
		public static string CertificateFile = null;
		public static string CertificatePassword = null;
		public static string Password;
		public static string KeyFile = null;
		public static string ProbingPaths = "";
		public static string AllowedHosts = "0.0.0.0";
		public static bool IsLocalService = false;
		public static TraceLevel TraceLevel = TraceLevel.Off;
		public static X509Certificate2 Certificate = null;
		public static string DataProviderType = null;
		public static string WebApplicationsPath = null;
		public static int? ServerRequestTimeout = null;
		public static string ConnectionString = null;
		public static string ProviderName = null;
		public static bool AlwaysUseEntityFramework = false;
		public static string AltConnectionString = null;
		public static string AltProviderName = null;
		public static string CryptoKey = null;
		public static string AltCryptoKey = null;
		public static bool? EncryptionEnabled = null;
		public static string ExposeWebServices = null;
		public static bool IsPortal = false;
	}
}
