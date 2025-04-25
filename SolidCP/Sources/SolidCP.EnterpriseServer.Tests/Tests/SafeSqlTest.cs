using SolidCP.EnterpriseServer.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolidCP.Tests
{
	[TestClass]
	public class SafeSqlTest
	{
		public TestContext TestContext { get; set; }

		public string ReplaceWhiteSpace(string txt)
		{
			return Regex.Replace(txt.Trim(), @"\s+", " ", RegexOptions.Singleline);
		}
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
			int count = 0;
			var safesql = MigrationBuilderExtension.SafeSql(sql, ref count);

			Debug.WriteLine(safesql);

			var correctSafeSql = @"PRINT 'Command 4'
EXECUTE sp_executesql N'CREATE FUNCTION'
GO

'CREATE FUNCTION'
'GO'
GO

PRINT 'Command 3'
EXECUTE sp_executesql N'CREATE VIEW'
GO;

PRINT 'CREATE VIEW'
EXECUTE sp_executesql N'CREATE VIEW
[GO]

CREATE VIEW'
GO

-- CREATE VIEW
GO

/* CREATE VIEW */
GO";
			Assert.AreEqual(ReplaceWhiteSpace(safesql), ReplaceWhiteSpace(correctSafeSql));

		}

	}
}
