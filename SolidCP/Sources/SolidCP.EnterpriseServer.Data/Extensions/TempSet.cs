using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace SolidCP.EnterpriseServer.Data.Extensions
{

	public class TempSet: IQueryable<Entities.TempId>, IDisposable
	{
		private Guid Scope = Guid.NewGuid();

		private DbContext Context;

		public TempSet(IEnumerable<int> ids, DbContext context)
		{

		}
		public void Dispose()
		{

		}
	}

	public static class EnumerableExtensions
	{
	}
}
