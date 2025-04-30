using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using SolidCP.EnterpriseServer.Code;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Data;


namespace SolidCP.Tests
{
	[TestClass]
	public class Database
	{
		public string ConnectionString(DbType dbtype) => EnterpriseServer.ConnectionString(dbtype);

		[TestMethod]
		[DataRow(DbType.SqlServer)]
		[DataRow(DbType.Sqlite)]
		public void TestDbAccess(DbType dbtype)
		{
			using (var db = new SolidCP.EnterpriseServer.Data.DbContext(ConnectionString(dbtype)))
			{
				var providers = db.Providers.ToArray();
				Assert.IsTrue(providers.Length > 0);
			}
		}

		[TestMethod]
		[DataRow(DbType.SqlServer)]
		[DataRow(DbType.Sqlite)]
		public void TestDynamicLike(DbType dbtype)
		{
			using (var db = new SolidCP.EnterpriseServer.Data.DbContext(ConnectionString(dbtype)))
			{
				var columnName = "ProviderName";
				var columnValue = "%S";
#if NETFRAMEWORK
				var providersStatic = db.Providers.Where(p => DbFunctions.Like(p.ProviderName, columnValue));
#else
				var providersStatic = db.Providers.Where(p => EF.Functions.Like(p.ProviderName, columnValue));
#endif
				var config = new ParsingConfig { ResolveTypesBySimpleName = true };
				var providersDynamic = db.Providers.Where(DynamicFunctions.ColumnLike(db.Providers, columnName, columnValue));
				var nstatic = providersStatic.Count();
				var ndynamic = providersDynamic.Count();
				Assert.AreEqual(nstatic, ndynamic);
			}
		}

		[TestMethod]
		[DataRow(DbType.SqlServer)]
		[DataRow(DbType.Sqlite)]
		public void TestGetSearchObjectEF(DbType dbtype)
		{
			using (var db = new DataProvider(ConnectionString(dbtype)))
			{
				db.AlwaysUseEntityFramework = true;
				var set = db.GetSearchObject(1, 1, null, "%test%", 0, 0, "", 0, 15, null, null, false, true);
			}
		}

		[TestMethod]
		[DataRow(DbType.SqlServer)]
		[DataRow(DbType.Sqlite)]
		public void TestGetSearchObjectStoredProcedure(DbType dbtype)
		{
			using (var db = new DataProvider(ConnectionString(dbtype)))
			{
				db.AlwaysUseEntityFramework = false;
				var set = db.GetSearchObject(1, 1, null, "%test%", 0, 0, "", 0, 15, null, null, false, true);
			}
		}

	}
}
