#if NETCOREAPP
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query;
#else
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



#if NETFRAMEWORK
namespace System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
#else
namespace Microsoft.EntityFrameworkCore;
#endif


public static partial class EstrellasDeEsperanzaEntityFrameworkExtensions
{

#if NETFRAMEWORK
	internal static DbContext GetDbContext(this ObjectContext context)
	{
		var property = context.GetType().GetProperty("InterceptionContext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		var interceptionContext = property.GetValue(context, null) as DbInterceptionContext;

		if (interceptionContext == null)
		{
			return null;
		}

		var dbContext = interceptionContext.DbContexts.FirstOrDefault();

		if (dbContext == null)
		{
			dbContext = new DbContext(context, false);
		}

		return dbContext;
	}

	internal static ObjectQuery<T> GetObjectQuery<T>(this IQueryable<T> query)
	{
		// CHECK for ObjectQuery
		var objectQuery = query as ObjectQuery<T>;
		if (objectQuery != null)
		{
			return objectQuery;
		}

		// CHECK for DbQuery
		var dbQuery = query as DbQuery<T>;

		if (dbQuery == null)
		{
			var internalQueryProperty = query.GetType().GetProperty("InternalQuery", BindingFlags.NonPublic | BindingFlags.Instance);

			if (internalQueryProperty == null)
			{
				// Check if a InnerQuery on AutoMapper 3erd party library with field Inner.
				var innerField = query.GetType().GetField("inner", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);

				if (innerField != null)
				{
					var innerQuery = innerField.GetValue(query) as IQueryable<T>;

					if (innerQuery != null && query != innerQuery)
					{
						var innerObjectQuery = innerQuery.GetObjectQuery();
						return innerObjectQuery;
					}
				}

				// CHECK if a InnerQuery exists
				var innerQueryProperty = query.GetType().GetProperty("InnerQuery", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

				if (innerQueryProperty != null)
				{
					var innerQuery = innerQueryProperty.GetValue(query, null) as IQueryable<T>;

					if (innerQuery != null && query != innerQuery)
					{
						var innerObjectQuery = innerQuery.GetObjectQuery();
						return innerObjectQuery;
					}
				}

				throw new Exception("Cannot determine DbContext.");
			}

			var internalQuery = internalQueryProperty.GetValue(query, null);
			var objectQueryContextProperty = internalQuery.GetType().GetProperty("ObjectQuery", BindingFlags.Public | BindingFlags.Instance);
			var objectQueryContext = objectQueryContextProperty.GetValue(internalQuery, null);

			objectQuery = objectQueryContext as ObjectQuery<T>;

			return objectQuery;
		}

		{
			var internalQueryProperty = dbQuery.GetType().GetProperty("InternalQuery", BindingFlags.NonPublic | BindingFlags.Instance);
			var internalQuery = internalQueryProperty.GetValue(dbQuery, null);
			var objectQueryContextProperty = internalQuery.GetType().GetProperty("ObjectQuery", BindingFlags.Public | BindingFlags.Instance);
			var objectQueryContext = objectQueryContextProperty.GetValue(internalQuery, null);

			objectQuery = objectQueryContext as ObjectQuery<T>;

			return objectQuery;
		}
	}


#endif

	internal static DbContext GetDbContext<T>(this IQueryable<T> source)
	{
#if NETCOREAPP
            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = (QueryCompiler)compilerField.GetValue(source.Provider);

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = (RelationalQueryContextFactory)queryContextFactoryField.GetValue(compiler);

            object stateManagerDynamic;

            var dependenciesProperty = typeof(RelationalQueryContextFactory).GetProperty("Dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
            //var dependenciesField = typeof(RelationalQueryContextFactory).GetField("_dependencies", BindingFlags.NonPublic | BindingFlags.Instance);

            var dependencies = dependenciesProperty?.GetValue(queryContextFactory);// ??
                //dependenciesField.GetValue(queryContextFactory);

            var stateManagerField = typeof(DbContext).Assembly.GetType("Microsoft.EntityFrameworkCore.Query.QueryContextDependencies").GetProperty("StateManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            stateManagerDynamic = stateManagerField.GetValue(dependencies);

			IStateManager stateManager = stateManagerDynamic as IStateManager;

            if (stateManager == null)
            {
                stateManager = ((dynamic)stateManagerDynamic).Value;
            }

            return stateManager.Context;
#else
		return source.GetObjectQuery().Context.GetDbContext();
#endif
	}
}