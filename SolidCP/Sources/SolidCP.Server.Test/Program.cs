using SolidCP.Server.Client;
using SolidCP.Providers;
using System.ServiceModel.Security;
using SolidCP.Web.Client;
using System;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using (var client = new AutoDiscovery() { Url = "https://localhost:9007/basic" })
{
	Console.WriteLine($"Server Path: {client.GetServerFilePath()}");
}

using (var client = new SolidCP.Server.Client.OperatingSystem() { Url = "https://localhost:9007/basic" })
{
	client.SoapHeader = new ServiceProviderSettingsSoapHeader()
	{
		Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
	};
	client.Credentials.Password = "aWs7wiWmcyph0oYjIRyMBP2yQZQ=";
	Console.WriteLine($"C:\\GitHub exists: {client.DirectoryExists("C:\\GitHub")}");
}

using (var client = new SolidCP.Server.Client.OperatingSystem() { Url = "https://localhost:9007/basic" })
{
	client.SoapHeader = new ServiceProviderSettingsSoapHeader()
	{
		Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
	};
	client.Credentials.Password = "aWs7wiWmcyph0oYjIRyMBP2yQZQ=";
	Console.WriteLine("Log Names:");
	foreach (var log in client.GetLogNames())
	{
		Console.WriteLine(log);
	}
}

