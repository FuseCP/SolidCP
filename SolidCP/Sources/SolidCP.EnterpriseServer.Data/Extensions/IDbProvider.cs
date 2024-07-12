using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Data
{
	public interface IDbProvider
	{
		DbType DbType { get; }
	}

	public class MsSqlProvider : IDbProvider
	{
		public DbType DbType => DbType.SqlServer;
	}

	public class MySqlProvider : IDbProvider
	{
		public DbType DbType => DbType.MySql;
	}
	public class MariaDbProvider : IDbProvider
	{
		public DbType DbType => DbType.MariaDb;
	}
	public class PostgreSqlProvider : IDbProvider
	{
		public DbType DbType => DbType.PostgreSql;
	}
	public class SqliteProvider : IDbProvider
	{
		public DbType DbType => DbType.Sqlite;
	}

}
