#if !NETFRAMEWORK && !NETSTANDARD
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SolidCP.Providers.OS;

namespace SolidCP.EnterpriseServer.Data.Scaffolding
{
	public class Scaffold
	{
		class OSShell : Shell
		{
			public OSShell() { Redirect = false; }
			public override string ShellExe => "cmd";
		}
		public static string Echo(string msg) => msg;
		public string GetEnttityDataAsString(IEntityType entityType, DbContext db, int ident) => GetEntityData(entityType, db, ident).ToString();

		static StringBuilder ToLiteral(string input)
		{
			var literal = new StringBuilder();
			literal.Append("\"");
			foreach (var c in input)
			{
				switch (c)
				{
					case '\"': literal.Append("\\\""); break;
					case '\\': literal.Append(@"\\"); break;
					case '\0': literal.Append(@"\0"); break;
					case '\a': literal.Append(@"\a"); break;
					case '\b': literal.Append(@"\b"); break;
					case '\f': literal.Append(@"\f"); break;
					case '\n': literal.Append(@"\n"); break;
					case '\r': literal.Append(@"\r"); break;
					case '\t': literal.Append(@"\t"); break;
					case '\v': literal.Append(@"\v"); break;
					default:
						// ASCII printable character
						if (c >= 0x20 && c <= 0x7e || Char.GetUnicodeCategory(c) != UnicodeCategory.Control)
						{
							literal.Append(c);
							// As UTF16 escaped character
						}
						else
						{
							literal.Append(@"\u");
							literal.Append(((int)c).ToString("x4"));
						}
						break;
				}
			}
			literal.Append("\"");
			return literal;
		}

		StringBuilder AppendString(string txt, int indent)
		{
			var str = new StringBuilder();
			if (txt == null)
			{
				str.Append("null");
				return str;
			} else if (txt == "")
			{
				str.Append("\"\"");
				return str;
			}
			const int columns = 80;
			int index = 0;
			bool first = true;
			while (index < txt.Length)
			{
				if (!first)
				{
					str.AppendLine(" +");
					for (int i = 0; i < indent; i++) str.Append(" ");
				}
				first = false;
				var len = Math.Min(columns, txt.Length - index);
				var line = txt.Substring(index, len);
				str.Append(ToLiteral(line));
				index += columns;
			}
			return str;
		}
		public StringBuilder GetEntityData(IEntityType entityType, DbContext db, int indent)
		{
			const int columns = 5;
			const int tabSize = 4;

			var writer = new StringBuilder();
			try
			{
				var entityClrType = entityType.ClrType;
				if (entityClrType == null) return writer;
				var defaultEntityData = Activator.CreateInstance(entityClrType);
				var setType = typeof(DbSet<>).MakeGenericType(entityClrType);
				var setMethod = db.BaseContext.GetType().GetMethod("Set", BindingFlags.Public | BindingFlags.Instance, new Type[0]);
				var setGenericMethod = setMethod.MakeGenericMethod(entityClrType);
				var set = (IQueryable)setGenericMethod.Invoke(db.BaseContext, new object[0]);
				var firstRecord = true;
				foreach (var entity in set)
				{
					if (!firstRecord) writer.AppendLine(",");
					firstRecord = false;

					int col = 0;
					for (int i = 0; i < indent; i++) writer.Append(" ");
					writer.Append("new ");
					writer.Append(entityClrType.Name);
					writer.Append("() { ");
					bool omit = true;
					foreach (var prop in entityType.GetProperties())
					{
						if (!omit)
						{
							if (col++ < columns) writer.Append(", ");
							else
							{
								writer.AppendLine(",");
								for (int i = 0; i < indent + tabSize; i++) writer.Append(" ");
								col = 0;
							}
						}
						omit = false;
						var rp = entityClrType.GetProperty(prop.Name);
						var val = rp.GetValue(entity);
						var defaultVal = rp.GetValue(defaultEntityData);

						if (val != null && val.Equals(defaultVal) ||
							val == null && defaultVal == null)
						{
							omit = true;
							continue;
						}

						writer.Append(prop.Name);
						writer.Append(" = ");

						if (val == null)
						{
								writer.Append("null");
						}
						else if (prop.ClrType == typeof(string))
						{
							var str = (string)val;
							if (str.Any(ch => Char.GetUnicodeCategory(ch) == UnicodeCategory.Control))
							{
								str = str.Replace("\"", "\"\"");
								writer.AppendLine();
								writer.Append("@\"");
								writer.Append(str);
								writer.AppendLine("\"");
								for (int i = 0; i < indent + tabSize; i++) writer.Append(" ");
							}
							else writer.Append(AppendString((string)val, indent + tabSize));
						}
						else if (prop.ClrType == typeof(byte) || prop.ClrType == typeof(sbyte) ||
							prop.ClrType == typeof(Int16) || prop.ClrType == typeof(UInt16) ||
							prop.ClrType == typeof(Int32) || prop.ClrType == typeof(UInt32) ||
							prop.ClrType == typeof(Int64) || prop.ClrType == typeof(UInt64) ||
							prop.ClrType == typeof(decimal) || prop.ClrType == typeof(float) ||
							prop.ClrType == typeof(double) ||
							prop.ClrType == typeof(byte?) || prop.ClrType == typeof(sbyte?) ||
							prop.ClrType == typeof(Int16?) || prop.ClrType == typeof(UInt16?) ||
							prop.ClrType == typeof(Int32?) || prop.ClrType == typeof(UInt32?) ||
							prop.ClrType == typeof(Int64?) || prop.ClrType == typeof(UInt64?) ||
							prop.ClrType == typeof(decimal?) || prop.ClrType == typeof(float?) ||
							prop.ClrType == typeof(double?)
							)
						{
							writer.Append(val.ToString());
						}
						else if (prop.ClrType == typeof(Guid) || prop.ClrType == typeof(Guid?))
						{
							writer.Append("new Guid(\"");
							writer.Append(val.ToString());
							writer.Append("\")");
						}
						else if (prop.ClrType == typeof(DateTime) || prop.ClrType == typeof(DateTime?))
						{
							writer.Append("DateTime.Parse(\"");
							writer.Append(((DateTime)val).ToUniversalTime().ToString("O"));
							writer.Append("\").ToUniversalTime()");
						}
						else if (prop.ClrType == typeof(TimeSpan) || prop.ClrType == typeof(TimeSpan?))
						{
							writer.Append("TimeSpan.Parse(\"");
							writer.Append(val.ToString());
							writer.Append("\")");
						}
						else if (prop.ClrType == typeof(byte[]))
						{
							writer.Append("Convert.FromBase64String(@\"");
							writer.Append(Convert.ToBase64String((byte[])val));
							writer.Append("\")");
						}
						else if (prop.ClrType == typeof(bool) || prop.ClrType == typeof(bool?))
						{
							writer.Append(val.ToString().ToLower());
						}
						else
						{
							writer.Append("\""); writer.Append(val.ToString()); writer.Append("\"");
						}
					}
					writer.Append(" }");
				}
			}
			catch (Exception ex)
			{
			}
			return writer;
		}

		public static string Escape(string txt) => txt.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n");
		public static string Unescape(string txt) => txt.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\\\", "\\");

		const bool Prefetch = true;

		static Dictionary<string, string> entityTypes = new Dictionary<string, string>();

		static void ParseData(string data)
		{
			entityTypes.Clear();
			var tokens = Regex.Matches(data, @"(?<=^|\n\r?\n)(?<type>[^\r\n]*)\r?\n(?<data>.*?)\r?\n\r?\n", RegexOptions.Singleline);
			foreach (Match match in tokens)
			{
				var type = Regex.Match(match.Groups["type"].Value.Trim(), @"(?<=\.|^)[^.]*?$").Value;
				var ed = Unescape(match.Groups["data"].Value);
				if (!entityTypes.ContainsKey(type))
				{
					//Console.WriteLine($"Added type {type}");
					entityTypes.Add(type, ed);
				}
				else entityTypes[type] = ed;
			}
		}

		public static string GetEntityDatasFromSeparateProcess(IEntityType entityType, ModelCodeGenerationOptions options, string templateFile, int indent, bool debug = false)
		{
			string entityData = "";
			var typeName = Regex.Match(entityType.Name, @"(?<=\.|^)[^.]*?$").Value;

			if (entityTypes.TryGetValue(typeName, out entityData))
			{
				if (!string.IsNullOrEmpty(entityData))
					Console.WriteLine($"EntityData for {typeName} found.");
				return entityData;
			}
			var dataFile = Path.Combine(Path.GetTempPath(), "SolidCP.EntityData.txt");
			var dataInfo = new FileInfo(dataFile);
			//Console.WriteLine($"TemplateFile: {templateFile}");
			//Console.WriteLine($"ProjectDir: {options.ProjectDir}");
			//Console.WriteLine($"ContextDir: {options.ContextDir}");
			//Console.WriteLine($"CurrentDir: {Environment.CurrentDirectory}");
			var contextDir = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(templateFile), options.ContextDir));
			var fileInfos = contextDir.EnumerateFiles("*.cs")
				.Where(fi => !fi.Name.Contains(options.ContextName));
			if (dataInfo.Exists &&
				dataInfo.LastAccessTime > DateTime.Now.AddMinutes(-2) &&
				!fileInfos.All(fi => fi.LastWriteTime >= dataInfo.LastWriteTime))
			{
				ParseData(File.ReadAllText(dataFile));
			}
			else
			{
				Console.WriteLine("Fetch Entity Data from Database...");

				var dll = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;

				string cmd;
				if (Prefetch) cmd = $"dotnet \"{dll}\" \"{options.ConnectionString}\" {indent}";
				else cmd = $"dotnet \"{dll}\" \"{options.ConnectionString}\" \"{entityType.ClrType.AssemblyQualifiedName}\" {indent}";
				if (debug) Console.WriteLine(cmd);
				var output = new OSShell().Exec(cmd).Output().Result;
				if (debug) Console.WriteLine(output);
				File.WriteAllText(dataFile, output);
				ParseData(output);
			}

			if (entityTypes.TryGetValue(typeName, out entityData))
			{
				if (!string.IsNullOrEmpty(entityData))
					Console.WriteLine($"EntityData for {typeName} found.");
				return entityData;
			}
			return entityData;

			Console.WriteLine($"Type not found {typeName}.");

			return "";
		}
	}
}
#endif