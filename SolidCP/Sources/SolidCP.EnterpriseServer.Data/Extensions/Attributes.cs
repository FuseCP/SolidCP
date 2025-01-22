using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Data
{

#if Attributes
	public class DbContextAttribute: Attribute
	{
		public DbContextAttribute(Type type) { }
	}
	public class MigrationAttribute: Attribute
	{
		public MigrationAttribute(string id) { }
	}
#endif
}
