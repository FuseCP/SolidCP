using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data
{

	public class MySqlDbContext : GenericDbContext<MySqlProvider>
	{
#if NetCore

		public MySqlDbContext(Data.DbContext context) : this(new DbOptions<Context.DbContextBase>(context)) { }

		public MySqlDbContext(DbContextOptions<Context.DbContextBase> options) : base(options)
		{
			if (options is DbOptions<Context.DbContextBase> opts)
			{
				DbType = DbType.MySql;
				InitSeedData = opts.InitSeedData;
			}
		}

		/*public GenericDbContext(DbContextOptions<GenericDbContext<TProvider>> options) :
			base(new DbOptions<Context.DbContextBase>(
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts) ? opts.DbType : DbType.Unknown,
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts2) ? opts2.ConnectionString : "")) { }
		*/
		public MySqlDbContext(string connectionString, bool initSeedData = false) :
			base(new DbOptions<Context.DbContextBase>(DbType.MySql, connectionString, initSeedData)) { }
#elif NetFX
		public MySqlDbContext(Data.DbContext context) : base(context)
		{
			DbType = DbType.MySql;
			InitSeedData = context.InitSeedData;
		}
#endif
	}

	public class MsSqlDbContext: GenericDbContext<MsSqlProvider>
	{
#if NetCore

		public MsSqlDbContext(Data.DbContext context) : this(new DbOptions<Context.DbContextBase>(context)) { }

		public MsSqlDbContext(DbContextOptions<Context.DbContextBase> options) : base(options)
		{
			if (options is DbOptions<Context.DbContextBase> opts)
			{
				DbType = DbType.SqlServer;
				InitSeedData = opts.InitSeedData;
			}
		}

		/*public GenericDbContext(DbContextOptions<GenericDbContext<TProvider>> options) :
			base(new DbOptions<Context.DbContextBase>(
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts) ? opts.DbType : DbType.Unknown,
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts2) ? opts2.ConnectionString : "")) { }
		*/
		public MsSqlDbContext(string connectionString, bool initSeedData = false) :
			base(new DbOptions<Context.DbContextBase>(DbType.SqlServer, connectionString, initSeedData)) { }
#elif NetFX
		public MsSqlDbContext(Data.DbContext context) : base(context)
		{
			DbType = DbType.SqlServer;
			InitSeedData = context.InitSeedData;
		}
#endif
	}

	public class PostgreSqlDbContext : GenericDbContext<PostgreSqlProvider>
	{
#if NetCore

		public PostgreSqlDbContext(Data.DbContext context) : this(new DbOptions<Context.DbContextBase>(context)) { }

		public PostgreSqlDbContext(DbContextOptions<Context.DbContextBase> options) : base(options)
		{
			if (options is DbOptions<Context.DbContextBase> opts)
			{
				DbType = DbType.PostgreSql;
				InitSeedData = opts.InitSeedData;
			}
		}

		/*public GenericDbContext(DbContextOptions<GenericDbContext<TProvider>> options) :
			base(new DbOptions<Context.DbContextBase>(
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts) ? opts.DbType : DbType.Unknown,
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts2) ? opts2.ConnectionString : "")) { }
		*/
		public PostgreSqlDbContext(string connectionString, bool initSeedData = false) :
			base(new DbOptions<Context.DbContextBase>(DbType.PostgreSql, connectionString, initSeedData))
		{ }
#elif NetFX
		public PostgreSqlDbContext(Data.DbContext context) : base(context)
		{
			DbType = DbType.PostgreSql;
			InitSeedData = context.InitSeedData;
		}
#endif
	}

	public class SqliteDbContext : GenericDbContext<SqliteProvider>
	{
#if NetCore

		public SqliteDbContext(Data.DbContext context) : this(new DbOptions<Context.DbContextBase>(context)) { }

		public SqliteDbContext(DbContextOptions<Context.DbContextBase> options) : base(options)
		{
			if (options is DbOptions<Context.DbContextBase> opts)
			{
				DbType = DbType.Sqlite;
				InitSeedData = opts.InitSeedData;
			}
		}

		/*public GenericDbContext(DbContextOptions<GenericDbContext<TProvider>> options) :
			base(new DbOptions<Context.DbContextBase>(
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts) ? opts.DbType : DbType.Unknown,
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts2) ? opts2.ConnectionString : "")) { }
		*/
		public SqliteDbContext(string connectionString, bool initSeedData = false) :
			base(new DbOptions<Context.DbContextBase>(DbType.Sqlite, connectionString, initSeedData))
		{ }
#elif NetFX
		public SqliteDbContext(Data.DbContext context) : base(context)
		{
			DbType = DbType.Sqlite;
			InitSeedData = context.InitSeedData;
		}
#endif
	}

	public class MariaDbDbContext : GenericDbContext<MariaDbProvider>
	{
#if NetCore

		public MariaDbDbContext(Data.DbContext context) : this(new DbOptions<Context.DbContextBase>(context)) { }

		public MariaDbDbContext(DbContextOptions<Context.DbContextBase> options) : base(options)
		{
			if (options is DbOptions<Context.DbContextBase> opts)
			{
				DbType = DbType.MariaDb;
				InitSeedData = opts.InitSeedData;
			}
		}

		/*public GenericDbContext(DbContextOptions<GenericDbContext<TProvider>> options) :
			base(new DbOptions<Context.DbContextBase>(
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts) ? opts.DbType : DbType.Unknown,
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts2) ? opts2.ConnectionString : "")) { }
		*/
		public MariaDbDbContext(string connectionString, bool initSeedData = false) :
			base(new DbOptions<Context.DbContextBase>(DbType.MariaDb, connectionString, initSeedData))
		{ }
#elif NetFX
		public MariaDbDbContext(Data.DbContext context) : base(context)
		{
			DbType = DbType.MariaDb;
			InitSeedData = context.InitSeedData;
		}
#endif
	}

	public class GenericDbContext<TProvider>: Context.DbContextBase where TProvider: IDbProvider
	{
#if NetCore

		public GenericDbContext(Data.DbContext context) : this(new DbOptions<Context.DbContextBase>(context)) { }

		public GenericDbContext(DbContextOptions<Context.DbContextBase> options) : base(options)
		{
			if (options is DbOptions<Context.DbContextBase> opts)
			{
				DbType = opts.DbType;
				InitSeedData = opts.InitSeedData;
			}
		}

		/*public GenericDbContext(DbContextOptions<GenericDbContext<TProvider>> options) :
			base(new DbOptions<Context.DbContextBase>(
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts) ? opts.DbType : DbType.Unknown,
				(options is Data.Extensions.DbOptions<GenericDbContext<TProvider>> opts2) ? opts2.ConnectionString : "")) { }
		*/
		public GenericDbContext(DbType dbType, string connectionString, bool initSeedData = false):
			base(new DbOptions<Context.DbContextBase>(dbType, connectionString, initSeedData)) { }
#elif NetFX
		public GenericDbContext(Data.DbContext context) : base(context)
		{
			DbType = context.DbType;
			InitSeedData = context.InitSeedData;
		}
#endif
	}
}