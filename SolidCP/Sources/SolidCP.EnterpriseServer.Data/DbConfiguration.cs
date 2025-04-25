#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using Npgsql;

namespace SolidCP.EnterpriseServer.Data
{
	public class DbConfiguration : System.Data.Entity.DbConfiguration
	{
		// We do not support PostgreSql in EF6, because it's buggy
		const bool EF6SupportPostgreSql = false;
#if MultipleEF6Providers
		public const bool OnlySupportOneEF6DbType = false;
#else
		public const bool OnlySupportOneEF6DbType = true;
#endif
		static int initialized = 0;
		public static void InitDatabaseProviders(DbType dbType, bool? initAllProviders = null)
		{
			if (Interlocked.CompareExchange(ref initialized, 1, 0) == 0)
			{
				var configuration = new DbConfiguration(dbType, initAllProviders);
				SetConfiguration(configuration);
			}
		}

		public static void InitAllDatabaseProviders() => InitDatabaseProviders(DbType.Unknown, true);
		void SetProvidersSqlServer()
		{
			SetProviderServices("Microsoft.Data.SqlClient", MicrosoftSqlProviderServices.Instance);
			SetProviderFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
			//SetProviderServices("Microsoft.Data.SqlClient", new GenericDbProviderServices(DbType.SqlServer));
			//SetProviderFactory("Microsoft.Data.SqlClient", new GenericDbProviderFactory(DbType.SqlServer));
		}
		static System.Data.Entity.Core.Common.DbProviderServices mySqlProviderServices = null;
		void SetProvidersMySql()
		{
			mySqlProviderServices = mySqlProviderServices ??= new MySqlProviderServices();
			SetProviderServices("MySql.Data.MySqlClient", mySqlProviderServices);
			SetProviderFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);

			//SetProviderServices("MySql.Data.MySqlClient", new GenericDbProviderServices(DbType.MySql));
			//SetProviderFactory("MySql.Data.MySqlClient", new GenericDbProviderFactory(DbType.MySql));
		}
		void SetProvidersSqlite()
		{
			var providerServicesType = Type.GetType("System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
			var instanceProperty = providerServicesType.GetField("Instance",
			BindingFlags.NonPublic | BindingFlags.Static);
			Activator.CreateInstance(providerServicesType);
			SetProviderServices("System.Data.SQLite", (System.Data.Entity.Core.Common.DbProviderServices)instanceProperty.GetValue(null));
			SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
			SetProviderFactory("System.Data.SQLite.EF6", System.Data.SQLite.EF6.SQLiteProviderFactory.Instance);

			//SetProviderServices("System.Data.SQLite", new GenericDbProviderServices(DbType.Sqlite));
			//SetProviderFactory("System.Data.SQLite", new GenericDbProviderFactory(DbType.Sqlite));
			//SetProviderFactory("System.Data.SQLite.EF6", new GenericDbProviderFactory(DbType.Sqlite, true));
		}
		void SetProvidersPostgreSql()
		{
			if (EF6SupportPostgreSql)
			{
				SetProviderServices("Npgsql", NpgsqlServices.Instance);
				SetProviderFactory("Npgsql", NpgsqlFactory.Instance);

				//SetProviderServices("Npgsql", new GenericDbProviderServices(DbType.PostgreSql));
				//SetProviderFactory("Npgsql", new GenericDbProviderFactory(DbType.PostgreSql));
			}
		}
		public DbConfiguration(DbType dbType, bool? initAllProviders = false)
		{
			SetDatabaseInitializer<SqlServerDbContext>(null);
			SetDatabaseInitializer<MySqlDbContext>(null);
			SetDatabaseInitializer<MariaDbDbContext>(null);
			SetDatabaseInitializer<SqliteDbContext>(null);
			SetDatabaseInitializer<PostgreSqlDbContext>(null);
			SetDatabaseInitializer<Context.DbContextBase>(null);

			var initAll = initAllProviders ?? !OnlySupportOneEF6DbType;

			if (initAll || dbType == DbType.SqlServer)
				SetProvidersSqlServer();
			if (initAll || dbType == DbType.MySql || dbType == DbType.MariaDb)
				SetProvidersMySql();
			if (initAll || dbType == DbType.Sqlite || dbType == DbType.SqliteFX)
				SetProvidersSqlite();
			if (initAll || dbType == DbType.PostgreSql)
				SetProvidersPostgreSql();
		}
	}
}
#endif