#if NETFRAMEWORK
using System;
using System.Collections.Generic;
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
		static MySqlProviderServices mySqlProviderServices = new MySqlProviderServices();

		public DbConfiguration()
		{
			SetDatabaseInitializer<MsSqlDbContext>(null);
			SetDatabaseInitializer<MySqlDbContext>(null);
			SetDatabaseInitializer<MariaDbDbContext>(null);
			SetDatabaseInitializer<SqliteDbContext>(null);
			SetDatabaseInitializer<PostgreSqlDbContext>(null);
			SetDatabaseInitializer<Context.DbContextBase>(null);

			// MS SQL
			SetProviderServices("Microsoft.Data.SqlClient", MicrosoftSqlProviderServices.Instance);
			SetProviderFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
			
			// MySQL & MariaDB
			SetProviderServices("MySql.Data.MySqlClient", mySqlProviderServices);
			SetProviderFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);

			// SQLite
			var providerServicesType = Type.GetType("System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
			var instanceProperty = providerServicesType.GetField("Instance",
				BindingFlags.NonPublic | BindingFlags.Static);
			Activator.CreateInstance(providerServicesType);
			SetProviderServices("System.Data.SQLite", (System.Data.Entity.Core.Common.DbProviderServices)instanceProperty.GetValue(null));
			SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
			SetProviderFactory("System.Data.SQLite.EF6", System.Data.SQLite.EF6.SQLiteProviderFactory.Instance);

			// PostgreSQL
			SetProviderServices("Npgsql", NpgsqlServices.Instance);
			SetProviderFactory("Npgsql", NpgsqlFactory.Instance);
		}
	}
}
#endif