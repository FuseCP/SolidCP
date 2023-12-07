using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace SolidCP.Providers.Web.LetsEncrypt
{
	public class Program
	{

		// this program is an update script that updates the wcf server certificate in web.config for use with win-acme.
		public static void Main(string[] args)
		{

			Console.WriteLine("Update WCF Certificate v1.0");

			if (args.Length == 0)
			{
				Console.WriteLine(@"Usage: wcfcert storename certthumb");
			}

			if (args.Length != 2)
			{
				Console.WriteLine("Error: Invalid number of arguments.");
				return;
			}

			var storename = args[0];
			var certthumb = args[1];

			var rootPath = Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
			var webConfigFileName = Path.Combine(rootPath, "web.config");
			XDocument wdoc = null;
			using (var webConfigFile = File.OpenRead(webConfigFileName))
			{
				wdoc = XDocument.Load(webConfigFile);
			}

			var root = wdoc.Element("configuration");
			var serviceModel = root.Element("system.serviceModel");
			if (serviceModel == null)
			{
				serviceModel = new XElement("system.serviceModel");
				root.Add(serviceModel);
			}
			var behaviors = serviceModel.Element("behaviors");
			if (behaviors == null)
			{
				behaviors = new XElement("behaviors");
				serviceModel.Add(behaviors);
			}
			var serviceBehaviors = behaviors.Element("serviceBehaviors");
			if (serviceBehaviors == null)
			{
				serviceBehaviors = new XElement("serviceBehaviors");
				behaviors.Add(serviceBehaviors);
			}
			var behavior = serviceBehaviors.Element("behavior");
			if (behavior == null)
			{
				behavior = new XElement("behavior");
				serviceBehaviors.Add(behavior);
			}
			var serviceCredentials = behavior.Element("serviceCredentials");
			if (serviceCredentials == null)
			{
				serviceCredentials = new XElement("serviceCredentials");
				behavior.Add(serviceCredentials);
			}
			var serviceCertificate = serviceCredentials.Element("serviceCertificate");
			if (serviceCertificate == null)
			{
				serviceCertificate = new XElement("serviceCertificate");
				serviceCredentials.Add(serviceCertificate);
			}
			serviceCertificate.SetAttributeValue("storeLocation", "LocalMachine");
			serviceCertificate.SetAttributeValue("storeName", storename);
			serviceCertificate.SetAttributeValue("x509FindType", "FindByThumbprint");
			serviceCertificate.SetAttributeValue("findValue", certthumb);

			using (var webConfigFile = new FileStream(webConfigFileName, FileMode.Create, FileAccess.Write))
			{
				wdoc.Save(webConfigFile);
			}
		}
	}
}
