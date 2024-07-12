#if !NETFRAMEWORK
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;


namespace SolidCP.EnterpriseServer.Data
{
	internal class Program
	{

		public static void Main(string[] args)
		{
			//Console.ReadKey();
			//Console.WriteLine("SolidCP.EnterpriseServer.Data");

			if (args.Length < 2) return;

			var connectionString = args[0];

			using (var db = new DbContext(connectionString, DbType.SqlServer))
			{
				if (args.Length >= 3)
				{
					var type = Type.GetType(args[1]);
					Console.WriteLine(type.Name);
					var entityType = ((Context.DbContextBase)db.BaseContext).Model.FindEntityType(type);
					string entityData = null;

					if (args.Length == 3)
					{
						int indent = int.Parse(args[2]);
						entityData = (new Scaffolding.Scaffold()).GetEntityData(entityType, db, indent).ToString();
					}
					else if (args.Length == 4)
					{
						int indent = int.Parse(args[2]);
						ModelCodeGenerationOptions options = new ModelCodeGenerationOptions();
						options.ProjectDir = Environment.CurrentDirectory;
						options.ContextName = "DbContextBase";
						options.ContextDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\Configuration\Sources"));
						options.ConnectionString = connectionString;
						var templateFile = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"CodeTemplates\EFCore\bla.t4"));
						entityData = Scaffolding.Scaffold.GetEntityDatasFromSeparateProcess(entityType, options, templateFile, indent);
					}
					Console.Write(Scaffolding.Scaffold.Escape(entityData));
					Console.WriteLine();
				} else if (args.Length == 2)
				{
					var entityTypes = ((Context.DbContextBase)db.BaseContext).Model.GetEntityTypes()
						.Where(e => !e.IsSimpleManyToManyJoinEntityType());
					var scaffolder = new Scaffolding.Scaffold();
					var str = new StringBuilder();
					int indent = int.Parse(args[1]);
					foreach (var entityType in entityTypes)
					{
						str.AppendLine(entityType.Name);
						var data = scaffolder.GetEntityData(entityType, db, indent);
						str.Append(Scaffolding.Scaffold.Escape(data.ToString()));
						str.AppendLine();
						str.AppendLine();
					}
					Console.WriteLine(str.ToString());
				}
			}
			Console.ReadKey();
		}
	}
}
#else
namespace SolidCP.EnterpriseServer.Data
{
	public class Program {
		public static void Main(string[] args) {
		}
	}
}
#endif