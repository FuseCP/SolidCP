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
using SolidCP.EnterpriseServer;
using SolidCP.Providers.OS;

namespace SolidCP.Tests;

public class EnterpriseServer : IDisposable
{
	// Create a temporal clone of the EnterpriseServer website
	const bool CreateClone = false;
	const string DatabaseName = "SolidCPTest";
	const DbType dbType = DbType.SqlServer;
	public const string SysadminPassword = "123456";
	public const string AssemblyUrl = "assembly://SolidCP.EnterpriseServer";
	public static void InitAssemblyLoader()
	{
#if NETFRAMEWORK
		Web.Clients.AssemblyLoader.Init(@"..\SolidCP.EnterpriseServer\bin;..\SolidCP.EnterpriseServer\bin\Code;..\SolidCP.EnterpriseServer\bin\netstandard", "none", true);
#else
		//Web.Clients.AssemblyLoader.Init(@"..\SolidCP.EnterpriseServer\bin_dotnet;..\SolidCP.EnterpriseServer\bin\netstandard", "none", true);
		Web.Services.Configuration.ProbingPaths = @"..\..\..\..\SolidCP.EnterpriseServer\bin_dotnet;..\..\..\..\SolidCP.EnterpriseServer\bin\netstandard";
		Web.Services.AssemblyLoaderNetCore.Init();

        var eserver = Assembly.Load("SolidCP.EnterpriseServer");
        if (eserver != null)
        {
            // init password validator
            var validatorType = eserver.GetType("SolidCP.EnterpriseServer.UsernamePasswordValidator");
            var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
            init.Invoke(null, new object[0]);
        }
#endif
	}

	static object Lock = new object();

	static string path = null;

	public static string EnterpriseServerPath
	{
		get
		{
			var exepath = IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			var esserver = IO.Path.GetFullPath(IO.Path.Combine(exepath, "..", "..", "..", "..", "SolidCP.EnterpriseServer"));
			return esserver;
		}
	}
	public static string Path {
		get {
			string tmpPath = null;
			bool mustClone = false;
			if (CreateClone)
			{
				lock (Lock)
				{
					if (path != null) return path;
					path = IO.Path.Combine(IO.Path.GetTempPath(), "SolidCP", "SolidCP.EnterpriseServer.Tests", Guid.NewGuid().ToString());
					mustClone = true;
					tmpPath = path;
				}
			} else path = EnterpriseServerPath;

			if (mustClone) CloneTo(tmpPath);
			return path;
		}
	}

	public static void Clone()
	{
		if (CreateClone) Console.WriteLine($"Cloning EnterpriseServer to {Path}");
	}

	public static void CloneTo(string path)
	{
		DeleteDirectory(IO.Path.GetDirectoryName(path));

		var exepath = IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
		var esserver = IO.Path.GetFullPath(IO.Path.Combine(exepath, "..", "..", "..", "..", "SolidCP.EnterpriseServer"));

		Console.WriteLine($"Cloning {IO.Path.GetFileName(EnterpriseServerPath)} ...");
		SolidCP.Providers.Utils.FileUtils.CopyDirectory(esserver, path);
	}

	public void Dispose() => Delete();

	public static void Delete()
	{
		if (CreateClone)
		{
			string tmpPath = null;
			bool mustDelete = false;
			if (path != null)
			{
				tmpPath = path;
				path = null;
				mustDelete = true;
			}
			if (mustDelete) DeleteDirectory(tmpPath);
		}
	}

	static void DeleteDirectory(string dir) => Directory.Delete($@"\\?\{dir}", true);
	
	public static string SetupDatabase(DbType dbType = DbType.SqlServer)
	{
		string connectionString;
		if (dbType == DbType.SqlServer) connectionString = SetupLocalDb();
		else if (dbType == DbType.Sqlite) connectionString = SetupSqliteDb();
		else throw new NotSupportedException($"Database type {dbType} is not supported");

		ConfigureDatabase(connectionString);

		return connectionString;
	}
	static int localDbStarted = 0;
	public static void StartLocalDB()
	{
		if (Interlocked.Exchange(ref localDbStarted, 1) == 0)
		{
			var shell = Shell.Standard.Clone;
			shell.Redirect = true;
			shell.Exec("SqlLocalDB start");
		}
	}
	public static string SetupLocalDb()
	{
		var connectionString = $"DbType=SqlServer;Data Source=(localdb)\\.;Database={DatabaseName};Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";
		var masterConnectionString = "DbType=SqlServer;Data Source=(localdb)\\.;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

		StartLocalDB();

		if (DatabaseUtils.DatabaseExists(masterConnectionString, DatabaseName))
		{
			DatabaseUtils.DeleteDatabase(masterConnectionString, DatabaseName);
		}

		DatabaseUtils.InstallFreshDatabase(masterConnectionString, DatabaseName, null, null);

		DatabaseUtils.SetServerAdminPassword(masterConnectionString, DatabaseName,
			CryptoUtils.Encrypt(SysadminPassword));

		return sqlServerConnectionString = connectionString;
	}

	public static string SetupSqliteDb() {
		var dbfile = IO.Path.Combine(Path, "App_Data", $"{DatabaseName}.sqlite");
		var dir = IO.Path.GetDirectoryName(dbfile);
		if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

		var connectionString = DatabaseUtils.BuildSqliteConnectionString(dbfile);
	
		if (DatabaseUtils.DatabaseExists(connectionString, DatabaseName))
		{
			DatabaseUtils.DeleteDatabase(connectionString, DatabaseName);
		}

		DatabaseUtils.InstallFreshDatabase(connectionString, DatabaseName, null, null);

		DatabaseUtils.SetServerAdminPassword(connectionString, DatabaseName,
			CryptoUtils.Encrypt(SysadminPassword));

		return sqliteConnectionString = connectionString;
	}

	public static void ConfigureDatabase(string connectionString) 
	{
		/*var espath = Path;
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
		*/
		CurrentConnectionString = connectionString;
		Environment.SetEnvironmentVariable("SOLIDCP_CONNECTIONSTRING", connectionString);
	}

	public static string sqlServerConnectionString = null;
	public static string sqliteConnectionString = null;
	public static string CurrentConnectionString { get; set; }

	public static string SqlServerConnectionString => sqlServerConnectionString ??= SetupDatabase(DbType.SqlServer);
	public static string SqliteConnectionString => sqliteConnectionString ??= SetupDatabase(DbType.Sqlite);

	public static string ConnectionString(DbType dbType = DbType.SqlServer) =>
		dbType == DbType.SqlServer ? SqlServerConnectionString :
		dbType == DbType.Sqlite ? SqliteConnectionString :
		throw new NotSupportedException($"Database type {dbType} is not supported");

	public static void DeleteDatabases()
	{
		if (sqlServerConnectionString != null &&
			DatabaseUtils.DatabaseExists(sqlServerConnectionString, DatabaseName))
			DatabaseUtils.DeleteDatabase(sqlServerConnectionString, DatabaseName);
		try {
			if (sqliteConnectionString != null &&
				DatabaseUtils.DatabaseExists(sqliteConnectionString, DatabaseName))
				DatabaseUtils.DeleteDatabase(sqliteConnectionString, DatabaseName);
		} catch { }
	}

	public static void SetupEmbeddedEnterpriseServer()
	{
		SolidCP.Web.Clients.AssemblyLoader.ProbingPaths = @"..\SolidCP.EnterpriseServer\bin;..\SolidCP.EnterpriseServer\bin\Code;..\SolidCP.EnterpriseServer\bin\netstandard";
	}
}
