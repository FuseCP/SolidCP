using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core.CustomTypeProviders;
#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer
{
	public static class DynamicFunctions
	{
		public static Expression<Func<T, bool>> ColumnLike<T>(IEnumerable<T> items, string columnName, string likeExpression)
		{
			var param = Expression.Parameter(typeof(T));
			var prop = Expression.Property(param, columnName);
			var type = typeof(DbFunctions);
			var likeMethod = type.GetMethod("Like", new Type[] { typeof(string), typeof(string) });
			var call = Expression.Call(null, likeMethod, prop, Expression.Constant(likeExpression));
			return Expression.Lambda<Func<T, bool>>(call, param);
		}
	}

}
