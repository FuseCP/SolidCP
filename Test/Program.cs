using SolidCP.Server.Client;
using SolidCP.Providers;
using System.ServiceModel.Security;
using SolidCP.Web.Client;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using (var client = new AutoDiscovery() { Url = "http://localhost:9900" }) {
	Console.WriteLine($"Server Path: {client.GetServerFilePath()}");
}

using (var client = new SolidCP.Server.Client.OperatingSystem() { Url = "http://localhost:9900/ws" }) {
	client.SoapHeader = new ServiceProviderSettingsSoapHeader() {
		Settings = new string[] { "Hello", "Howdy" }
	};
	client.Credentials.Password = "+uxnDOdf55yuH6iZYXgYAxsfIBw=";
	Console.WriteLine($"C:\\GitHub exists: {client.DirectoryExists("C:\\GitHub")}");
}

Console.ReadKey();