using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if !NETFRAMEWORK
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data
{
	public static class IQueryableExtensions
	{
		public static int ExecuteDelete<TEntity>(this IQueryable<TEntity> query, DbSet<TEntity> set) where TEntity: class
		{
#if NETFRAMEWORK
			set.RemoveRange(query);
			return set.BaseContext.SaveChanges();
#else
			return query.ExecuteDelete();
#endif
		}
	}
}
