using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFRAMEWORK
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data
{

#if NETFRAMEWORK
	public class EF
	{
		public static EF Instance = new EF();
		public static EF Functions => Instance;

		public bool Like(string searchString, string likeExpression) => DbFunctions.Like(searchString, likeExpression);
	}
#endif

}
