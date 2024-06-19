using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Data.Extensions
{
	internal class TestDbContext: MsSqlDbContext
	{
		public TestDbContext() : base(new DbContext()) { }
	}
}
