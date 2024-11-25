using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Linq.Expressions;

#if NETFRAMEWORK
namespace System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query;
namespace Microsoft.EntityFrameworkCore;
#endif

public static partial class EstrellasDeEsperanzaEntityFrameworkExtensions
{
	public const string SqliteProviderCore = "Microsoft.EntityFrameworkCore.Sqlite";
	public const string SqlProviderCore = "Microsoft.EntityFrameworkCore.SqlServer";
	public const string MySqlProviderCore = "Pomelo.EntityFrameworkCore.MySql";
	public const string PostgreSqlProviderCore = "Npgsql.EntityFrameworkCore";

	const string StringLiterals = "(?:[^\"']*?(?:'(?:''|[^'])*'|\"[^\"]*\"))*[^\"']*?";
#if NETCOREAPP
	[GeneratedRegex($"^({StringLiterals})SELECT", RegexOptions.Singleline)]
	private static partial Regex InjectInsertRegex();
	[GeneratedRegex($"^(?<params>{StringLiterals})SELECT", RegexOptions.Singleline)]
	private static partial Regex InjectInsertSqliteRegex();
	[GeneratedRegex(@"(?:^|\n)\.param\s+set\s+(?<name>[@a-zA-Z0-9_]+)\s+(?<value>(?:'(?:[^']|'')*')|(?:""[^""]*"")|(?:.*?)(?=\s*\r?\n))", RegexOptions.Singleline)]
	private static partial Regex ParseParamsSqliteRegex();
#else
	static readonly Regex injectInsertRegex = new Regex($"^({StringLiterals})SELECT", RegexOptions.Singleline);
	static readonly Regex injectInsertSqliteRegex = new Regex($"^(?<params>{StringLiterals})SELECT", RegexOptions.Singleline);
	static readonly Regex parseParamsSqliteRegex = new Regex(@"(?:^|\n)\.param\s+set\s+(?<name>[@a-zA-Z0-9_]+)\s+(?<value>(?:'(?:[^']|'')*')|(?:""[^""]*"")|(?:.*?)(?=\s*\r?\n))", RegexOptions.Singleline);
	private static Regex InjectInsertRegex() => injectInsertRegex;
	private static Regex InjectInsertSqliteRegex() => injectInsertSqliteRegex;
	private static Regex ParseParamsSqliteRegex() => parseParamsSqliteRegex;
#endif
	public enum DatabaseType { Unknown, SqlServer, Sqlite, MySql, MariaDb, PostgreSql, Oracle, Other };
	public class QueryContext<EntityType> where EntityType : class
	{
		public IQueryable<EntityType> Query;
		public DbContext Context;
		public DatabaseType Type = DatabaseType.Unknown;

		public QueryContext(IQueryable<EntityType> query, DbContext context = null)
		{
			Query = query;
			Context = context ??= query.GetDbContext();
#if NETCOREAPP
			switch (context?.Database.ProviderName)
			{
				case SqlProviderCore: Type = DatabaseType.SqlServer; break;
				case SqliteProviderCore: Type = DatabaseType.Sqlite; break;
				case MySqlProviderCore: Type = DatabaseType.MySql; break;
				case PostgreSqlProviderCore: Type = DatabaseType.PostgreSql; break;
				default: Type = DatabaseType.Other; break;
			}
#endif
			var set = Context.Set<EntityType>();
			if (set == null) throw new InvalidOperationException("Insert: EntityType must be an entity type.");
		}
		public string Table
		{
			get
			{
#if NETCOREAPP
				var entityType = Context.Model.FindEntityType(typeof(EntityType));
				return entityType.GetSchemaQualifiedTableName();
#else
		return "";
#endif
			}
		}
		public IEnumerable<System.Reflection.PropertyInfo> Properties
		{
			get
			{
				var callexp = Query.Expression as MethodCallExpression;
				while (callexp != null &&
					callexp.Method.Name != "Select" && callexp.Method.Name != "Join" && callexp.Method.Name != "SelectMany")
				{
					callexp = callexp.Object as MethodCallExpression;
				}

				if (callexp == null) throw new InvalidOperationException("No Select, Join, or SelectMany found in query.");

				var lambda = callexp.Arguments
					.OfType<UnaryExpression>()
					.Where(ue => ue.Operand.NodeType == ExpressionType.Lambda)
					.Select(ue => ue.Operand as LambdaExpression)
					.LastOrDefault();

				var memberInitExp = lambda.Body as MemberInitExpression;
				var newexp = memberInitExp?.NewExpression;

				if (newexp == null || newexp.Constructor.DeclaringType != typeof(EntityType))
					throw new InvalidOperationException($"new expression is not of type {typeof(EntityType).Name}");

				return memberInitExp.Bindings
					.OfType<MemberAssignment>()
					.Select(mem => mem.Member)
					.OfType<System.Reflection.PropertyInfo>();
			}
		}
		public IEnumerable<string> Columns
		{
			get
			{
#if NETCOREAPP
				var entityType = Context.Model.FindEntityType(typeof(EntityType));
				return Properties.Select(p => entityType.GetProperty(p.Name)?.GetColumnName());
#else
				return Enumerable.Empty<string>();					
#endif
			}
		}

		public string QuerySql
		{
			get
			{
#if NETCOREAPP
				return Query.ToQueryString();
#else
		return "";		
#endif
			}
		}
		public int Insert(string quotLeft, string quotRight)
		{
			var sql = new StringBuilder();
			sql.Append("INSERT INTO ");
			sql.Append(Table);
			sql.Append(" (");
			// Column info
			bool first = true;
			foreach (var property in Columns)
			{
				if (!first) sql.Append(", ");
				first = false;
				sql.Append(quotLeft);
				sql.Append(property);
				sql.Append(quotRight);
			}
			sql.AppendLine(")");
			string sqltext;
			if (Type != DatabaseType.Sqlite) {
				sql.Append("SELECT");
				sqltext = InjectInsertRegex().Replace(QuerySql, $"$1{sql}", 1);
			} else
			{
				sql.Append("SELECT");
				string partext = "";
				sqltext = sql.ToString();
				sqltext = InjectInsertSqliteRegex().Replace(QuerySql, match =>
				{
					partext = match.Groups["params"].Success ? match.Groups["params"].Value : "";
					return sqltext;
				}, 1);
				var pars = ParseParamsSqliteRegex().Matches(partext); 
				sql = new StringBuilder(sqltext);
				foreach (Match param in pars)
				{
					sql.Replace(param.Groups["name"].Value, param.Groups["value"].Value);
				}
				sqltext = sql.ToString();
			}

#if NETCOREAPP
			return Context.Database.ExecuteSqlRaw(sqltext);
#else
			return Context.Database.ExecuteSqlCommand(sqltext);
#endif
		}
	}
	public static int ExecuteInsert<EntityType>(this IQueryable<EntityType> query, DbContext? context = null) where EntityType : class
    {

		var queryContext = new QueryContext<EntityType>(query, context);

#if NETCOREAPP
		switch (queryContext.Type)
        {
			case DatabaseType.SqlServer:
			case DatabaseType.Oracle:
				return queryContext.Insert("[", "]");
			case DatabaseType.PostgreSql:
				return queryContext.Insert("\"", "\"");
			case DatabaseType.Sqlite:
				return queryContext.Insert("\"", "\"");
			case DatabaseType.MySql:
			case DatabaseType.MariaDb:
				return queryContext.Insert("`", "`");
			case DatabaseType.Other:
			case DatabaseType.Unknown:
            default:
				break;
        }
#else
#endif
		var set = queryContext.Context.Set<EntityType>();
		set.AddRange(query);
		return queryContext.Context.SaveChanges();
	}
}