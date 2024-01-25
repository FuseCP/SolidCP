using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace SolidCP.UniversalInstaller.Core
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				var stores = CertificateStoreInfo.GetStores();
				foreach (var store in stores)
				{
					Console.WriteLine(store.Name);
					Console.WriteLine(store.Location);
				}
			}
			else if (args.Length == 4)
			{
				StoreName name;
				StoreLocation location;
				X509FindType findType;
				string findValue = args[3];

				if (Enum.TryParse<StoreName>(args[0], out name) &&
					Enum.TryParse<StoreLocation>(args[1], out location) &&
					Enum.TryParse<X509FindType>(args[2], out findType))
				{
					if (CertificateStoreInfo.ExistsDirect(location, name, findType, findValue))
					{
						Console.WriteLine("Certificate found");
						Environment.Exit(0);
					}
					else
					{
						Console.WriteLine("Certificate not found.");
						Environment.Exit(-1);
					}
				}
				else
				{
					Console.WriteLine("Invalid parameters.");
					Environment.Exit(-2);
				}

			}
		}
	}
}
