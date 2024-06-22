using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code
{
	// For use with LINQPad
	public class TestDbContext: Data.MsSqlDbContext
	{
		public TestDbContext() : base(new Data.DbContext()) { }

		public DataProvider Db = new DataProvider();
	}
}
