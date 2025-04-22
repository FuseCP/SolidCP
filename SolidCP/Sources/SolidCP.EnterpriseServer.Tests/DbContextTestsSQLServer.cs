using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.Tests
{
	[TestClass]
	public class DbContextTestsSQLServer
	{
		static readonly object Lock = new object();

		public static string ConnectionString = null;

		[ClassInitialize]
		public static void InitSqlServerDb(TestContext context)
		{
			lock (Lock)
			{
				if (ConnectionString == null) ConnectionString = TestWebSite.SetupDatabase(DbType.SqlServer);
			}
		}

		[TestMethod]
		public void TestGetSearchObject()
		{
			using (var db = new DataProvider(ConnectionString))
			{
				db.AlwaysUseEntityFramework = true;
				var set = db.GetSearchObject(1, 1, null, "%test%", 0, 0, "", 0, 15, null, null, false, true);
			}
		}

		[TestMethod]
		public void TestGetSearchTableByColumns()
		{
			using (var db = new DataProvider(ConnectionString))
			{
				db.AlwaysUseEntityFramework = true;
				//var set = db.GetSearchTableByColumns(1, 1, null, "%test%", 0, 0, "", 0, 15, null, null, false, true);
			}
		}

	}
}
