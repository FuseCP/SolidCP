using SolidCP.EnterpriseServer.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Tests
{
	[TestClass]
	public class SafeSqlTest
	{
		[TestMethod]
		public void TestSafeSql()
		{
			var sql = @"CREATE FUNCTION
GO

'CREATE FUNCTION'
'GO'

GO

CREATE VIEW
GO;

CREATE VIEW
[GO]

CREATE VIEW
GO
-- CREATE VIEW
GO
/* CREATE VIEW */
GO
";
			var safesql = MigrationBuilderExtension.SafeSql(sql);

			var sql2 = MigrationBuilderExtension.SafeSql(DatabaseUtils.InstallScript("InitialCreate_StoredProcedures.sql"));

			Debug.WriteLine(safesql);
		}

	}
}
