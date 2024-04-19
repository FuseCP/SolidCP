#if NetCore
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace SolidCP.EnterpriseServer.Data.Extensions
{

	public class DbOptions<T>: DbContextOptions<T> where T: Microsoft.EntityFrameworkCore.DbContext
	{
		public DbFlavor Flavor { get; private set; }
		public string ConnectionString { get; private set; }
		public DbOptions(Data.DbContext context) {
			Flavor = context.Flavor;
			ConnectionString = context.ConnectionString;
		}
	}
}
#endif