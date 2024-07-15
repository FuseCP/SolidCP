#if NetCore

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace SolidCP.EnterpriseServer.Data.Extensions
{
	public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<Context.DbContextBase>,
		IDesignTimeDbContextFactory<SqlServerDbContext>,
		IDesignTimeDbContextFactory<MySqlDbContext>,
		IDesignTimeDbContextFactory<MariaDbDbContext>,
		IDesignTimeDbContextFactory<PostgreSqlDbContext>,
		IDesignTimeDbContextFactory<SqliteDbContext>
	{

		public Context.DbContextBase CreateDbContext(string[] args)
		{
			const string DefaultConnectionString = "DbType=SqlServer; Server=(local); Database=SolidCPFresh; uid=sa; pwd=Password12; TrustServerCertificate=true; Connection Timeout=300; command timeout=300";

			var connectionString = args.Length > 0 ? args[0] : DefaultConnectionString;

			var dbType = DbSettings.GetDbType(connectionString);
			connectionString = DbSettings.GetNativeConnectionString(connectionString);
			Console.WriteLine($"DbType: {dbType}");
			Console.WriteLine($"Using connection string: {connectionString}");

			Microsoft.EntityFrameworkCore.DbContext db;
			switch (dbType)
			{
				default:
				case DbType.SqlServer: return new SqlServerDbContext(connectionString, true);
				case DbType.MySql: return new MySqlDbContext(connectionString, true);
				case DbType.MariaDb: return new MariaDbDbContext(connectionString, true);
				case DbType.Sqlite: return new SqliteDbContext(connectionString, true);
				case DbType.PostgreSql: return new PostgreSqlDbContext(connectionString, true);
			}
		}

		SqlServerDbContext IDesignTimeDbContextFactory<SqlServerDbContext>.CreateDbContext(string[] args)
		{
			return (SqlServerDbContext)CreateDbContext(args);
		}
		MySqlDbContext IDesignTimeDbContextFactory<MySqlDbContext>.CreateDbContext(string[] args)
		{
			return (MySqlDbContext)CreateDbContext(args);
		}
		MariaDbDbContext IDesignTimeDbContextFactory<MariaDbDbContext>.CreateDbContext(string[] args)
		{
			return (MariaDbDbContext)CreateDbContext(args);
		}
		SqliteDbContext IDesignTimeDbContextFactory<SqliteDbContext>.CreateDbContext(string[] args)
		{
			return (SqliteDbContext)CreateDbContext(args);
		}
		PostgreSqlDbContext IDesignTimeDbContextFactory<PostgreSqlDbContext>.CreateDbContext(string[] args)
		{
			return (PostgreSqlDbContext)CreateDbContext(args);
		}
	}
}
#endif