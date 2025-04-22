using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using Newtonsoft.Json;

using SolidCP.EnterpriseServer.Data;
using SolidCP.Providers.Utils;

namespace SolidCP.Tests;

public class TestWebSite : IDisposable
{
	const string EnterpriseServerPath = @"..\..\..\..\SolidCP.EnterpriseServer";
	const DbType dbType = DbType.SqlServer;
	public const string SysadminPassword = "123456";

	static object Lock = new object();

	static string path = null;
	public static string Path {
		get {
			string tmpPath = null;
			bool mustClone = false;
			lock (Lock) {
				if (path != null) return path;
				path = IO.Path.Combine(IO.Path.GetTempPath(), "SolidCP", "SolidCP.EnterpriseServer.Tests", Guid.NewGuid().ToString());
				mustClone = true;
				tmpPath = path;
			}
			if (mustClone) CloneTo(tmpPath);
			return path;
		}
	}

	public static void Clone() => Console.WriteLine($"Cloning EnterpriseServer to {Path}");
	public static void CloneTo(string path)
	{
		var exepath = IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
		var esserver = IO.Path.GetFullPath(IO.Path.Combine(exepath, "..", "..", "..", "..", "SolidCP.EnterpriseServer"));

		Console.WriteLine($"Cloning {IO.Path.GetFileName(EnterpriseServerPath)} ...");
		FileUtils.CopyDirectory(esserver, path);
	}

	public void Dispose() => Delete();

	public static void Delete()
	{
		string tmpPath = null;
		bool mustDelete = false;
		if (path != null)
		{
			path = null;
			mustDelete = true;
			tmpPath = path;
		}
		if (mustDelete) DeleteDirectory(tmpPath);
	}

	static void DeleteDirectory(string dir) => Directory.Delete(dir, true);
	
	public static string SetupDatabase(DbType dbType = DbType.SqlServer)
	{
		string connectionString;
		if (dbType == DbType.SqlServer) connectionString = SetupLocalDB();
		else if (dbType == DbType.Sqlite) connectionString = SetupSqliteDB();
		else throw new NotSupportedException($"Database type {dbType} is not supported");

		ConfigureDatabase(connectionString);

		return connectionString;
	}
	public static string SetupLocalDB()
	{
		const string DatabaseName = "SolidCPTest";
		var connectionString = $"DbType=SqlServer;Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={DatabaseName};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";
		var masterConnectionString = "DbType=SqlServer;Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";

		Console.Write("Waiting for SQL Server LocalDB to start");
		while (!DatabaseUtils.CheckSqlConnection(masterConnectionString))
		{
			Console.Write(".");
			System.Threading.Thread.Sleep(1000);
		}

		if (DatabaseUtils.DatabaseExists(masterConnectionString, DatabaseName))
		{
			DatabaseUtils.DeleteDatabase(masterConnectionString, DatabaseName);
		}

		DatabaseUtils.InstallFreshDatabase(masterConnectionString, DatabaseName, null, null);

		DatabaseUtils.SetServerAdminPassword(connectionString, DatabaseName, SysadminPassword);

		return connectionString;
	}

	public static string SetupSqliteDB() {
		const string DatabaseName = "SolidCPTest";
		var connectionString = DatabaseUtils.BuildSqliteConnectionString(DatabaseName, Path);
	
		if (DatabaseUtils.DatabaseExists(connectionString, DatabaseName))
		{
			DatabaseUtils.DeleteDatabase(connectionString, DatabaseName);
		}

		DatabaseUtils.InstallFreshDatabase(connectionString, DatabaseName, null, null);

		DatabaseUtils.SetServerAdminPassword(connectionString, DatabaseName, SysadminPassword);

		return connectionString;
	}

	public static void ConfigureDatabase(string connectionString) 
	{
		var espath = Path;
		var webConfigPath = IO.Path.Combine(espath, "Web.config");
		var appsettingsPath = IO.Path.Combine(espath, "appsettings.json");

		// Configure appsettings.json
		dynamic appSettings = JsonConvert.DeserializeObject(File.ReadAllText(appsettingsPath));
		appSettings.EnterpriseServer.ConnectionString = connectionString;
		File.WriteAllText(appsettingsPath, JsonConvert.SerializeObject(appSettings, Formatting.Indented));

		// Configure Web.config
		var webConfig = XElement.Parse(File.ReadAllText(webConfigPath));
		var connectionStringElement = webConfig.Element("connectionStrings")
				.Elements("add")
				.FirstOrDefault(e => e.Attribute("name").Value == "EnterpriseServer");
		connectionStringElement.SetAttributeValue("connectionString", connectionString);
		File.WriteAllText(webConfigPath, webConfig.ToString());
	}

}
