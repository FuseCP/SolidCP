using SolidCP.Server.Client;
using SolidCP.Providers;
using System.ServiceModel.Security;
using SolidCP.Web.Client;
using System;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using (var client = new AutoDiscovery() { Url = "http://localhost:9900" })
{
	Console.WriteLine($"Server Path: {client.GetServerFilePath()}");
}

using (var client = new SolidCP.Server.Client.OperatingSystem() { Url = "http://localhost:9900" })
{
	client.SoapHeader = new ServiceProviderSettingsSoapHeader()
	{
		Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
	};
	client.Credentials.Password = "+uxnDOdf55yuH6iZYXgYAxsfIBw=";
	Console.WriteLine($"C:\\GitHub exists: {client.DirectoryExists("C:\\GitHub")}");
}

using (var client = new SolidCP.Server.Client.OperatingSystem() { Url = "http://localhost:9900/basic" })
{
	client.SoapHeader = new ServiceProviderSettingsSoapHeader()
	{
		Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
	};
	client.Credentials.Password = "+uxnDOdf55yuH6iZYXgYAxsfIBw=";
	Console.WriteLine($"C:\\GitHub exists: {client.DirectoryExists("C:\\GitHub")}");
}

using (var client = new SolidCP.Server.Client.OperatingSystem() { Url = "http://localhost:9900/ws" })
{
	client.SoapHeader = new ServiceProviderSettingsSoapHeader()
	{
		Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
	};
	client.Credentials.Password = "+uxnDOdf55yuH6iZYXgYAxsfIBw=";
	Console.WriteLine($"C:\\GitHub exists: {client.DirectoryExists("C:\\GitHub")}");
}

using (var client = new SolidCP.Server.Client.OperatingSystem() { Url = "http://localhost:9900/net" })
{
	client.SoapHeader = new ServiceProviderSettingsSoapHeader()
	{
		Settings = new string[] { "Provider:ProviderType=SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022", "Provider:ProviderName=Windows2022" }
	};
	client.Credentials.Password = "+uxnDOdf55yuH6iZYXgYAxsfIBw=";
	Console.WriteLine($"C:\\GitHub exists: {client.DirectoryExists("C:\\GitHub")}");
}
