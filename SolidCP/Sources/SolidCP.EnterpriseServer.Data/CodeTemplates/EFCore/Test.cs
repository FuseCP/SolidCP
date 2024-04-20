#if NetCore
using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace SolidCP.EnterpriseServer.Data.CodeTemplates.EFCore
{
    public class Test
    {

        public void Do(IEntityType EntityType, ModelCodeGenerationOptions Options, IServiceProvider services)
        {
            var annotationCodeGenerator = services.GetRequiredService<IAnnotationCodeGenerator>();
            var code = services.GetRequiredService<ICSharpHelper>();

            var annotations = EntityType.GetDataAnnotations(annotationCodeGenerator);
            var coreAnnotations = annotations.Where(a => a.Type.FullName.Contains("EntityFrameworkCore"));
            var commonAnnotations = annotations.Except(coreAnnotations);

			var match = Regex.Match("", @$"^using\s+{Regex.Escape(EntityType.Name)}\s*=.*?$", RegexOptions.Multiline);

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

		public void Setup(DbContextOptionsBuilder builder)
        {
            DbContext context = null;
            switch (context.Flavor)
            {
                case DbFlavor.MsSql:
                    builder.UseSqlServer(context.ConnectionString);
                    break;
                case DbFlavor.SqlLite:
                    builder.UseSqlite(context.ConnectionString);
                    break;
                case DbFlavor.MySql:
                    builder.UseMySql(context.ConnectionString, ServerVersion.AutoDetect(context.ConnectionString));
                    break;
                case DbFlavor.MariaDb:
                    builder.UseMariaDB(context.ConnectionString);
                    break;
                case DbFlavor.PostgreSql:
                    builder.UseNpgsql(context.ConnectionString);
                    break;
                default: throw new NotSupportedException("This DB flavor is not supported");
            }
        }

    }
}
#endif