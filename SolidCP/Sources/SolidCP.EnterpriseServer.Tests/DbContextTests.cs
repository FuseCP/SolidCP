using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Tests
{
	public class DbContextTests
	{
		const string ConnectionString = "DbType=MsSql;Server=(local);Database=SolidCPFresh;uid=sa;pwd=Password12;TrustServerCertificate=true;Connection Timeout=300;command timeout=300";
		public void TestDbAccess()
		{
			using (var db = new Data.DbContext(ConnectionString))
			{
				var providers = db.Providers.ToArray();
				Assert.IsTrue(providers.Length > 0);
			}
		}
	}
}
