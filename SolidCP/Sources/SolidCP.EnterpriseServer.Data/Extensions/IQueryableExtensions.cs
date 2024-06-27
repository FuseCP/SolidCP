using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;
#if !NETFRAMEWORK
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data
{
	public static class IQueryableExtensions
	{
#if NETFRAMEWORK
		public static int ExecuteDelete<TEntity>(this IQueryable<TEntity> query) where TEntity: class
		{
			return query.Delete();
		}
#endif
		public static int ExecuteUpdate<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateFactory) where TEntity: class {
			return query.Update(updateFactory);
		}
	}
}
