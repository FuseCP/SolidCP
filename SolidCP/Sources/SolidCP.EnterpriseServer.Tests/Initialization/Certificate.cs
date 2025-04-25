using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace SolidCP.Tests
{
	public class Certificate
	{
		public const string Password = "123456";
		public const string CertFile = "localhost.pfx";

		public static string CertFilePath {
			get {
				var testdllpath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
				var testprojpath = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", ".."));
				var certfile = Path.Combine(testprojpath, "Initialization", CertFile);
				return certfile;
			}
		}
		public static void Install(string certfile, string password)
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadWrite);
			X509Certificate2 cert = new X509Certificate2(certfile, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
			if (!store.Certificates.OfType<X509Certificate2>()
				.Any(c => c.Thumbprint == cert.Thumbprint))
			{
				store.Add(cert);
			}
			store.Close();
		}

		public static void Remove(string certfile, string password)
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadWrite);
			X509Certificate2 cert = new X509Certificate2(certfile, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
			var storecert = store.Certificates.OfType<X509Certificate2>()
				.FirstOrDefault(c => c.Thumbprint == cert.Thumbprint);
			if (storecert != null) store.Remove(storecert);
			store.Close();
		}

		public static void Install() => Install(CertFilePath, Password);

		public static void Remove() => Remove(CertFilePath, Password);
	}
}
