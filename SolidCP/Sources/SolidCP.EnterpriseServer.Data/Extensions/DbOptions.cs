#if NetCore
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace SolidCP.EnterpriseServer.Data
{

	public class DbOptions<T>: DbContextOptions<T> where T: Microsoft.EntityFrameworkCore.DbContext
	{
		public DbType DbType { get; private set; }
		public string ConnectionString { get; private set; }
		public bool InitSeedData { get; private set; }
		public DbOptions(DbType dbType, string connectionString = null, bool initSeedData = false)
		{
			DbType = dbType;
			ConnectionString = connectionString;
			InitSeedData = initSeedData;
		}
		public DbOptions(DbContext context) {
			DbType = context.DbType;
			ConnectionString = context.NativeConnectionString;
			InitSeedData = context.InitSeedData;
		}
	}
}
#endif