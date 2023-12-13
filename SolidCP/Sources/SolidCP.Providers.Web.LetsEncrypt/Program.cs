using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace SolidCP.Providers.Web.LetsEncrypt
{
	public static class MyExtensions
	{
		public static XElement Element(this XElement parent, XName name, bool createIfNotFound)
		{
			var childNode = parent.Element(name);
			{
				if (childNode != null)
				{
					return childNode;
				}
			};
			//
			if (createIfNotFound.Equals(true))
			{
				childNode = new XElement(name);
				parent.Add(childNode);
			}
			//
			return childNode;
		}
	}

	public class Program
	{

		// this program is an update script that updates the wcf server certificate in web.config for use with win-acme.
		public static void Main(string[] args)
		{

			Console.WriteLine("Update WCF Certificate v1.1");

			if (args.Length != 3)
			{
				Console.WriteLine("Error: Invalid number of arguments.\n" +
					"Usage: wcfcert webrootpath storename certthumb");
				Console.ReadKey();
				return;
			}

			var rootPath = args[0];
			var storename = args[1];
			var certthumb = args[2];

			var webConfigFileName = Path.Combine(rootPath, "web.config");
			XDocument wdoc = null;
			using (var webConfigFile = File.OpenRead(webConfigFileName))
			{
				wdoc = XDocument.Load(webConfigFile);
			}

			var root = wdoc.Root;
			var serviceModel = root.Element("system.serviceModel", true);
			var behaviors = serviceModel.Element("behaviors", true);
			var serviceBehaviors = behaviors.Element("serviceBehaviors", true);
			var behavior = serviceBehaviors.Element("behavior", true);
			var serviceCredentials = behavior.Element("serviceCredentials", true);
			var serviceCertificate = serviceCredentials.Element("serviceCertificate", true);
			serviceCertificate.SetAttributeValue("storeLocation", "LocalMachine");
			serviceCertificate.SetAttributeValue("storeName", storename);
			serviceCertificate.SetAttributeValue("x509FindType", "FindByThumbprint");
			serviceCertificate.SetAttributeValue("findValue", certthumb);

			using (var webConfigFile = new FileStream(webConfigFileName, FileMode.Create, FileAccess.Write))
			{
				wdoc.Save(webConfigFile);

				Console.WriteLine($"Set WCF certificate to LocalMachine, {storename} with Thumprint {certthumb} in {webConfigFileName}");
			}
		}
	}
}
