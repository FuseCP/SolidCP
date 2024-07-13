using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data
{
	public static class DynamicFunctions
	{
		public static Expression<Func<T, bool>> ColumnLike<T>(IEnumerable<T> items, string columnName, string likeExpression)
		{
			var param = Expression.Parameter(typeof(T));
			var property = Expression.Property(param, columnName);
			var likeExpressionParameter = Expression.Constant(likeExpression);
#if NETFRAMEWORK
			var type = typeof(DbFunctions);
			var likeMethod = type.GetMethod("Like", new[] { typeof(string), typeof(string) });
			// TODO use variable, not constant.
			var call = Expression.Call(null, likeMethod, property, likeExpressionParameter);
#else
			// Get the Like Method from EF.Functions
			var efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new[] { typeof(DbFunctions), typeof(string), typeof(string) },
				null);

			var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
			// Сall the method with all the required arguments
			var call = Expression.Call(efLikeMethod, efFunctions, property, likeExpressionParameter);
#endif
			return Expression.Lambda<Func<T, bool>>(call, param);
		}
	}

}
