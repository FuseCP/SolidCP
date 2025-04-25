#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using EF=System.Data.Entity.Core.Common;
using System.Reflection;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Spatial;
using System.Security;
using System.Security.Permissions;

namespace SolidCP.EnterpriseServer.Data;

public class GenericDbProviderServices: EF.DbProviderServices
{
	public string TypeName
	{
		get
		{
			switch (DbType)
			{
				case DbType.SqlServer:
					return "System.Data.Entity.SqlServer.MicrosoftSqlProviderServices, Microsoft.EntityFramework.SqlServer"; ;
				case DbType.MySql:
				case DbType.MariaDb:
					return "MySql.Data.MySqlClient.MySqlProviderServices, MySql.Data.EntityFramework";
				case DbType.Sqlite:
				case DbType.SqliteFX:
					return "System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6";
				case DbType.PostgreSql:
					return "Npgsql.NpgsqlServices, EntityFramework6.Npgsql";
				default:
					throw new NotSupportedException($"DbType {DbType} is not supported.");
			}
		}
	}
	public DbType DbType { get; set; }
	public GenericDbProviderServices(DbType dbType)
	{
		DbType = dbType;
	}
	EF.DbProviderServices instance = null;
	public EF.DbProviderServices Instance
	{
		get
		{
			if (instance == null)
			{
				var type = Type.GetType(TypeName);
				if (DbType == DbType.MySql || DbType == DbType.MariaDb)
				{
					instance = Activator.CreateInstance(type) as EF.DbProviderServices;
				}
				else
				{
					var instanceField = type.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
					var instanceProperty = type.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
					instance = (instanceField?.GetValue(null) ?? instanceProperty?.GetValue(null)) as EF.DbProviderServices;
				}
			}
			return instance;
		}
	}

	protected override EF.DbCommandDefinition CreateDbCommandDefinition(EF.DbProviderManifest providerManifest, DbCommandTree commandTree)
	{
		return Instance.CreateCommandDefinition(providerManifest, commandTree);
	}
	protected override string GetDbProviderManifestToken(DbConnection connection)
	{
		return Instance.GetProviderManifestToken(connection);
	}
	protected override EF.DbProviderManifest GetDbProviderManifest(string manifestToken)
	{
		return Instance.GetProviderManifest(manifestToken);
	}
	public override DbConnection CloneDbConnection(DbConnection connection)
	{
		return Instance.CloneDbConnection(connection);
	}
	public override DbConnection CloneDbConnection(DbConnection connection, DbProviderFactory factory)
	{
		return Instance.CloneDbConnection(connection, factory);
	}
	protected override DbCommand CloneDbCommand(DbCommand fromDbCommand)
	{
		var type = Instance.GetType();
		var cloneDbCommand = type.GetMethod(nameof(CloneDbCommand), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		return cloneDbCommand.Invoke(Instance, new[] { fromDbCommand }) as DbCommand;
	}
	public override EF.DbCommandDefinition CreateCommandDefinition(DbCommand prototype)
	{
		return Instance.CreateCommandDefinition(prototype);
	}
	protected override void DbCreateDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
	{
		var type = Instance.GetType();
		var dbCreateDatabase = type.GetMethod(nameof(DbCreateDatabase), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		dbCreateDatabase.Invoke(Instance, new object[] { connection, commandTimeout, storeItemCollection });
	}
	protected override string DbCreateDatabaseScript(string providerManifestToken, StoreItemCollection storeItemCollection)
	{
		var type = Instance.GetType();
		var dbCreateDatabaseScript = type.GetMethod(nameof(DbCreateDatabaseScript), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		return dbCreateDatabaseScript.Invoke(Instance, new object[] { providerManifestToken, storeItemCollection }) as string;
	}
	protected override bool DbDatabaseExists(DbConnection connection, int? commandTimeout, Lazy<StoreItemCollection> storeItemCollection)
	{
		var type = Instance.GetType();
		var dbDatabaseExists = type.GetMethod(nameof(DbDatabaseExists), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		return (bool)dbDatabaseExists.Invoke(Instance, new object[] { connection, commandTimeout, storeItemCollection });
	}
	protected override bool DbDatabaseExists(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
	{
		var type = Instance.GetType();
		var dbDatabaseExists = type.GetMethod(nameof(DbDatabaseExists), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		return (bool)dbDatabaseExists.Invoke(Instance, new object[] { connection, commandTimeout, storeItemCollection });
	}
	protected override void DbDeleteDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
	{
		var type = Instance.GetType();
		var dbDeleteDatabase = type.GetMethod(nameof(DbDeleteDatabase), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		dbDeleteDatabase.Invoke(Instance, new object[] { connection, commandTimeout, storeItemCollection });
	}
	[Obsolete]
	protected override DbSpatialServices DbGetSpatialServices(string manifestToken)
	{
		var type = Instance.GetType();
		var dbGetSpatialServices = type.GetMethod(nameof(DbGetSpatialServices), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		return dbGetSpatialServices.Invoke(Instance, new object[] { manifestToken }) as DbSpatialServices;
	}
	public override bool Equals(object obj)
	{
		return Instance.Equals(obj);
	}
	protected override DbSpatialDataReader GetDbSpatialDataReader(DbDataReader fromReader, string manifestToken)
	{
		var type = Instance.GetType();
		var getDbSpatialDataReader = type.GetMethod(nameof(GetDbSpatialDataReader), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		return getDbSpatialDataReader.Invoke(Instance, new object[] { fromReader, manifestToken }) as DbSpatialDataReader;
	}
	public override int GetHashCode()
	{
		return Instance.GetHashCode();
	}
	public override object GetService(Type type, object key)
	{
		return Instance.GetService(type, key);
	}
	public override IEnumerable<object> GetServices(Type type, object key)
	{
		//if (instance == null && type.FullName == "System.Data.Entity.Infrastructure.Interception.IDbInterceptor") return Enumerable.Empty<object>();
		return Instance.GetServices(type, key);
	}
	public override void RegisterInfoMessageHandler(DbConnection connection, Action<string> handler)
	{
		Instance.RegisterInfoMessageHandler(connection, handler);
	}
	protected override void SetDbParameterValue(DbParameter parameter, TypeUsage parameterType, object value)
	{
		var type = Instance.GetType();
		var setDbParameterValue = type.GetMethod(nameof(SetDbParameterValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		setDbParameterValue.Invoke(Instance, new[] { parameter, parameterType, value });
	}
	public override string ToString()
	{
		return Instance.ToString();
	}
}

public class GenericDbProviderFactory: DbProviderFactory
{
	public DbType DbType { get; set; }
	public bool SqliteEF6 { get; set; } = false;

	public GenericDbProviderFactory(DbType dbType, bool slqiteEF6 = false)
	{
		DbType = dbType;
		SqliteEF6 = slqiteEF6;
	}
	public string TypeName
	{
		get
		{
			switch (DbType)
			{
				case DbType.SqlServer: return "Microsoft.Data.SqlClient.SqlClientFactory, Microsoft.Data.SqlClient"; ;
				case DbType.MySql:
				case DbType.MariaDb:
					return "MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data";
				case DbType.Sqlite:
				case DbType.SqliteFX:
					if (SqliteEF6) return "System.Data.SQLite.EF6.SQLiteProviderFactory, System.Data.SQLite.EF6";
					else return "System.Data.SQLite.SQLiteFactory, System.Data.SQLite";
				case DbType.PostgreSql:
					return "Npgsql.NpgsqlFactory, Npgsql";
				default:
					throw new NotSupportedException($"DbType {DbType} is not supported.");
			}
		}
	}
	public DbProviderFactory Instance
	{
		get {
			var type = Type.GetType(TypeName);
			var instanceField = type.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			var instanceProperty = type.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			return (DbProviderFactory)(instanceField?.GetValue(null) ?? instanceProperty?.GetValue(null));
		}
	}
	public override bool CanCreateDataSourceEnumerator => Instance.CanCreateDataSourceEnumerator;
	public override DbCommand CreateCommand()
	{
		return Instance.CreateCommand();
	}
	public override DbCommandBuilder CreateCommandBuilder()
	{
		return Instance.CreateCommandBuilder();
	}
	public override DbConnection CreateConnection()
	{
		return Instance.CreateConnection();
	}
	public override DbConnectionStringBuilder CreateConnectionStringBuilder()
	{
		return Instance.CreateConnectionStringBuilder();
	}
	public override DbDataAdapter CreateDataAdapter()
	{
		return Instance.CreateDataAdapter();
	}
	public override DbDataSourceEnumerator CreateDataSourceEnumerator()
	{
		return Instance.CreateDataSourceEnumerator();
	}
	public override DbParameter CreateParameter()
	{
		return Instance.CreateParameter();
	}
	public override CodeAccessPermission CreatePermission(PermissionState state)
	{
		return Instance.CreatePermission(state);
	}
	public override bool Equals(object obj)
	{
		return Instance.Equals(obj);
	}
	public override int GetHashCode()
	{
		return Instance.GetHashCode();
	}
	public override string ToString()
	{
		return Instance.ToString();
	}
}
#endif