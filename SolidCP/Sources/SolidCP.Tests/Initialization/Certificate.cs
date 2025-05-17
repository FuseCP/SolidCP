using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Net;
using System.IO;

namespace SolidCP.Tests
{
	public class Certificate
	{
		public const string Password = "123456";
		public const string CertFile = "localhost.pfx";

		public static string CertFilePath {
			get {
				var certfile = Path.Combine(Paths.Test, "Initialization", CertFile);
				var asm = Assembly.GetExecutingAssembly();
				var resxs = asm.GetManifestResourceNames();
				var localhostPfx = resxs.FirstOrDefault(r => r.EndsWith(CertFile));
				var stream = asm.GetManifestResourceStream(localhostPfx);
				if (stream == null)
					throw new FileNotFoundException($"Resource {localhostPfx} not found in assembly {asm.FullName}");
				using (var file = File.Create(certfile))
				using (stream)
					stream.CopyTo(file);
				
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

		public static void InstallLocalhostIntoMy()
		{
			var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			var mystore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadWrite);
			mystore.Open(OpenFlags.ReadWrite);
			var certs = store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", true);
			foreach (var cert in certs)
			{
				if (!mystore.Certificates.OfType<X509Certificate2>()
					.Any(c => c.Thumbprint == cert.Thumbprint))
				{
					try
					{
						mystore.Add(cert);
					}
					catch { }
				}
			}

			mystore.Close();
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

		public static void TrustAll()
		{
			// Always trust certificates
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			Web.Clients.ClientBase.TrustAllCertificates = true;
			InstallLocalhostIntoMy();
		}
	}
}
