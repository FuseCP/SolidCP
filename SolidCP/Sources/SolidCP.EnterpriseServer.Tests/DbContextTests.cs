using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer.Data;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Tests
{
	[TestClass]
	public class DbContextTests
	{
		const string ConnectionString = "DbType=MsSql;Server=(local);Database=SolidCPFresh;uid=sa;pwd=Password12;TrustServerCertificate=true;Connection Timeout=300";
		[TestMethod]
		public void TestDbAccess()
		{
			using (var db = new Data.DbContext(ConnectionString))
			{
				var providers = db.Providers.ToArray();
				Assert.IsTrue(providers.Length > 0);
			}
		}

		[TestMethod]
		public void TestDynamicLike()
		{
			using (var db = new Data.DbContext(ConnectionString))
			{
				var columnName = "ProviderName";
				var columnValue = "%S";
				var providersStatic = db.Providers.Where(p => DbFunctions.Like(p.ProviderName, columnValue));
				var config = new ParsingConfig { ResolveTypesBySimpleName = true };
				var providersDynamic = db.Providers.Where(DynamicFunctions.ColumnLike<Data.Entities.Provider>(columnName, columnValue));
				var nstatic = providersStatic.Count();
				var ndynamic = providersDynamic.Count();
				Assert.AreEqual(nstatic, ndynamic);
			}
		}
	}
}
