#if NetCore
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System.Reflection;

namespace SolidCP.EnterpriseServer.Data
{
	internal class Test
	{

		public void Do(IEntityType EntityType, ModelCodeGenerationOptions Options, IServiceProvider services)
		{
			var annotationCodeGenerator = services.GetRequiredService<IAnnotationCodeGenerator>();
			var code = services.GetRequiredService<ICSharpHelper>();

			var annotations = EntityType.GetDataAnnotations(annotationCodeGenerator);
			var coreAnnotations = annotations.Where(a => a.Type.FullName.Contains("EntityFrameworkCore"));
			var commonAnnotations = annotations.Except(coreAnnotations);

			var match = Regex.Match("", @$"^using\s+{Regex.Escape(EntityType.Name)}\s*=.*?$", RegexOptions.Multiline);

			var scp = Assembly.Load("SolidCP.EnterpriseServer.Data.Core");
			var scaffoldType = scp.GetType("SolidCP.EnterpriseServer.Data.Scaffolding.Scaffold, SolidCP.EnterpriseServer.Data.Core");
			var getEntityDataMethod = scaffoldType.GetMethod("GetEntityData");
			var entityData = (StringBuilder)getEntityDataMethod.Invoke(null, new object[] { EntityType, Options, 12 });

			var sb = new StringBuilder();
			var firstProperty = true;
			foreach (var property in EntityType.GetProperties())
			{
				var propertyFluentApiCalls = property.GetFluentApiCalls(annotationCodeGenerator)
					?.FilterChain(c => !(Options.UseDataAnnotations && c.IsHandledByDataAnnotations)
						&& !(c.Method == "IsRequired" && Options.UseNullableReferenceTypes && !property.ClrType.IsValueType));
				if (propertyFluentApiCalls == null)
				{
					continue;
				}
			}
		}

		public static void GetData(IEntityType EntityType, ModelCodeGenerationOptions Options, IServiceProvider services, int indent)
		{
			using (var db = new DbContext(Options.ConnectionString, DbType.SqlServer))
			{
				var entityType = EntityType.ClrType;
				var setType = typeof(DbSet<>).MakeGenericType(entityType);
				var set = (IQueryable)Activator.CreateInstance(setType, db.BaseContext);
				foreach (var entity in set)
				{
					for (int i = 0; i < indent; i++) Console.Write(" ");
					Console.Write("new ");
					Console.Write(EntityType.ClrType.Name);
					Console.Write("() { ");
					bool first = true;
					foreach (var prop in EntityType.GetProperties())
					{
						if (!first) Console.Write(", ");
						first = false;
						var rp = entityType.GetProperty(prop.Name);
						var val = rp.GetValue(entity);
						Console.Write(prop.Name);
						Console.Write(" = ");
						if (val == null) Console.Write("null");
						else if (prop.ClrType == typeof(string))
						{
							Console.Write("\"");
							var txt = ((string)val).Replace(@"\", @"\\").Replace("\"", @"\""");
							Console.Write(txt);
							Console.Write("\"");
						}
						else if (prop.ClrType == typeof(byte) || prop.ClrType == typeof(sbyte) ||
							prop.ClrType == typeof(Int16) || prop.ClrType == typeof(UInt16) ||
							prop.ClrType == typeof(Int32) || prop.ClrType == typeof(UInt32) ||
							prop.ClrType == typeof(Int64) || prop.ClrType == typeof(UInt64) ||
							prop.ClrType == typeof(decimal) || prop.ClrType == typeof(float) ||
							prop.ClrType == typeof(double))
						{
							Console.Write(val.ToString());
						}
						else if (prop.ClrType == typeof(Guid))
						{
							Console.Write("new Guid(\"");
							Console.Write(val.ToString());
							Console.Write("\")");
						} else if (prop.ClrType == typeof(DateTime))
						{
							Console.Write("DateTime.Parse(");
							Console.Write(((DateTime)val).ToLongDateString());
							Console.Write("\")");
						} else if (prop.ClrType == typeof(TimeSpan))
						{
							Console.Write("TimeSpan.Parse(\"");
							Console.Write(val.ToString());
							Console.Write("\")");
						} else if (prop.ClrType == typeof(byte[]))
						{
							Console.Write("Convert.FromBase64String(\"");
							Console.Write(Convert.ToBase64String((byte[])val));
							Console.Write("\")");
						} else
						{
							Console.Write("\""); Console.Write(val.ToString()); Console.Write("\"");
						}
					}
					Console.WriteLine("},");
				}
			}
		}

		static ConcurrentDictionary<string, ServerVersion> serverVersions = new ConcurrentDictionary<string, ServerVersion>();
		public void Setup(DbContextOptionsBuilder builder)
		{
			DbContext context = null;
			switch (context.DbType)
			{
				case DbType.SqlServer:
					builder.UseSqlServer(context.ConnectionString);
					break;
				case DbType.Sqlite:
					builder.UseSqlite(context.ConnectionString);
					break;
				case DbType.MySql:
				case DbType.MariaDb:
					ServerVersion serverVersion = serverVersions.GetOrAdd(context.ConnectionString, connectionString => ServerVersion.AutoDetect(connectionString));
					builder.UseMySql(context.ConnectionString, serverVersion);
					break;
				case DbType.PostgreSql:
					builder.UseNpgsql(context.ConnectionString);
					break;
				default: throw new NotSupportedException("This DB type is not supported");
			}
		}

	}
}
#endif