using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code
{
	// For use with LINQPad
	public class TestDbContext: Data.SqlServerDbContext
	{
		public TestDbContext() : base(new Data.DbContext()) { }
		public TestDbContext(string connectionString) : base(new Data.DbContext(connectionString)) { }
	}
}
