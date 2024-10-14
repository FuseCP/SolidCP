using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using SolidCP.EnterpriseServer.Code;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Data;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif


namespace SolidCP.Tests
{
	[TestClass]
	public class DbContextTests
	{
		const string ConnectionString = "DbType=Sqlite;Data Source=App_Data\\SolidCP.Test.sqlite";

		static readonly object Lock = new object();

		[ClassInitialize]
		public static void InitSqliteDb(TestContext context)
		{
			lock (Lock)
			{
				var appDir = AppDomain.CurrentDomain.BaseDirectory;
				var dbFile = Path.Combine(appDir, "App_Data", "SolidCP.Test.sqlite");
				var dbPath = Path.GetDirectoryName(dbFile);
				if (!Directory.Exists(dbPath)) Directory.CreateDirectory(dbPath);
				if (File.Exists(dbFile)) File.Delete(dbFile);
				File.WriteAllText(dbFile, "");
				DatabaseUtils.ExecuteSql(ConnectionString, DatabaseUtils.InstallScriptSqlite());
			}
		}

		[TestMethod]
		public void TestDbAccess()
		{
			using (var db = new EnterpriseServer.Data.DbContext(ConnectionString))
			{
				var providers = db.Providers.ToArray();
				Assert.IsTrue(providers.Length > 0);
			}
		}

		[TestMethod]
		public void TestDynamicLike()
		{
			using (var db = new EnterpriseServer.Data.DbContext(ConnectionString))
			{
				var columnName = "ProviderName";
				var columnValue = "%S";
				var providersStatic = db.Providers.Where(p => DbFunctions.Like(p.ProviderName, columnValue));
				var config = new ParsingConfig { ResolveTypesBySimpleName = true };
				var providersDynamic = db.Providers.Where(DynamicFunctions.ColumnLike(db.Providers, columnName, columnValue));
				var nstatic = providersStatic.Count();
				var ndynamic = providersDynamic.Count();
				Assert.AreEqual(nstatic, ndynamic);
			}
		}

	}
}
