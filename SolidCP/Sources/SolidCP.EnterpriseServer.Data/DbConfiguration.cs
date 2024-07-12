#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Reflection;
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
		static System.Data.Entity.Core.Common.DbProviderServices mySqlProviderServices = null;

		static ImmutableHashSet<DbType> LoadedDatabaseProviders = ImmutableHashSet<DbType>.Empty;

		public static void InitDatabaseProvider(DbType dbType)
		{
			if (!LoadedDatabaseProviders.Contains(dbType))
			{
				var count = LoadedDatabaseProviders.Count;
				if (ImmutableInterlocked.Update(ref LoadedDatabaseProviders, providers => providers.Add(dbType)) &&
					count < LoadedDatabaseProviders.Count) {
					SetConfiguration(new DbConfiguration());
				}
			}
		}

		void SetProvidersMsSql()
		{
			SetProviderServices("Microsoft.Data.SqlClient", MicrosoftSqlProviderServices.Instance);
			SetProviderFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
		}
		
		void SetProvidersMySql()
		{
			mySqlProviderServices = mySqlProviderServices ??= new MySqlProviderServices();
			SetProviderServices("MySql.Data.MySqlClient", mySqlProviderServices);
			SetProviderFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
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
		}

		void SetProvidersPostgreSql()
		{
			SetProviderServices("Npgsql", NpgsqlServices.Instance);
			SetProviderFactory("Npgsql", NpgsqlFactory.Instance);
		}
		public DbConfiguration()
		{
			SetDatabaseInitializer<MsSqlDbContext>(null);
			SetDatabaseInitializer<MySqlDbContext>(null);
			SetDatabaseInitializer<MariaDbDbContext>(null);
			SetDatabaseInitializer<SqliteDbContext>(null);
			SetDatabaseInitializer<PostgreSqlDbContext>(null);
			SetDatabaseInitializer<Context.DbContextBase>(null);

			if (LoadedDatabaseProviders.Contains(DbType.SqlServer)) SetProvidersMsSql();
			if (LoadedDatabaseProviders.Contains(DbType.MySql) || LoadedDatabaseProviders.Contains(DbType.MariaDb))
			{
				SetProvidersMySql();
			}
			if (LoadedDatabaseProviders.Contains(DbType.Sqlite))
			{
				SetProvidersSqlite();
			}
			if (LoadedDatabaseProviders.Contains(DbType.PostgreSql)) SetProvidersPostgreSql();
		}
	}
}
#endif