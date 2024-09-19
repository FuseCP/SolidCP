using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Tests
{
	[TestClass]
	public class DbContextTestsSQLServer
	{

		[TestMethod]
		public void TestGetSearchObject()
		{
			using (var db = new DataProvider("DbType=SqlServer;Server=(local)\\SQLExpress;Database=SolidCP;uid=sa;pwd=Password12;TrustServerCertificate=true"))
			{
				db.AlwaysUseEntityFramework = true;
				var set = db.GetSearchObject(1, 1, null, "%test%", 0, 0, "", 0, 15, null, null, false, true);
			}
		}

	}
}
