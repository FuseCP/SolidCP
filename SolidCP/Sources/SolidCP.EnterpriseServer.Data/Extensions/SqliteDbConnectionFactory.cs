#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Data.Entity.Infrastructure;

namespace SolidCP.EnterpriseServer.Data
{
	public class SqliteConnectionFactory : IDbConnectionFactory
	{
		public DbConnection CreateConnection(string connectionString)
		{
			return new SqliteConnection(connectionString);
		}
	}
}
#endif