using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#if NetCore
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
#endif

namespace SolidCP.EnterpriseServer.Data
{
	public static class MigrationBuilderExtension
	{
		struct Segment
		{
			public int Start, Length;
			public bool HasSpecialCommand;
		}

#if NetCore
		public static OperationBuilder<SqlOperation> SafeSql(this MigrationBuilder builder,
			string query, bool suppressTransaction = false)
			=> builder.Sql(SafeSql(query), suppressTransaction);
#endif
		public static string SafeSql(string query)
		{
			List<Segment> segments = new();
			char stringDelimiter = ' ';
			bool isString = false, isDashComment = false, isCComment = false;

			int i = 0;
			int start = 0, length = 0;
			char pre = ' ';
			string preIdent = "";
			List<char> identifier = new();
			bool hasSpecialCommand = false;

			void ParseIdent()
			{
				if (identifier.Count > 0 || i >= query.Length)
				{
					var ident = new string(identifier.ToArray());
					if (ident.Equals("GO", StringComparison.OrdinalIgnoreCase) || i >= query.Length)
					{
						int end;
						if (i >= query.Length) end = query.Length - 1;
						else end = query.LastIndexOf('\n', i - 3);
						
						if (end == -1) end = 0;
						length = end - start + 1;

						segments.Add(new Segment() {
							Start = start,
							Length = length,
							HasSpecialCommand = hasSpecialCommand
						});

						if (i < query.Length) start = query.IndexOf('\n', i);
						if (start == -1) start = query.Length;
						hasSpecialCommand = false;
					}
					else if ((preIdent.Equals("CREATE", StringComparison.OrdinalIgnoreCase) ||
						preIdent.Equals("AlTER", StringComparison.OrdinalIgnoreCase)) &&
						(ident.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase) ||
						ident.Equals("VIEW", StringComparison.OrdinalIgnoreCase) ||
						ident.Equals("PROCEDURE", StringComparison.OrdinalIgnoreCase) ||
						ident.Equals("TRIGGER", StringComparison.OrdinalIgnoreCase)))
					{
						hasSpecialCommand = true;
					}
					preIdent = ident;
					identifier.Clear();
				}
			}

			foreach (char ch in query)
			{
				if (!isCComment && !isDashComment)
				{
					if (ch == '\'')
					{
						if (stringDelimiter == ' ')
						{
							isString = true;
							stringDelimiter = '\'';
						}
						else if (stringDelimiter == '\'')
						{
							isString = false;
							stringDelimiter = ' ';
						}
					}
					if (ch == '"')
					{
						if (stringDelimiter == ' ')
						{
							isString = true;
							stringDelimiter = '"';
						}
						else if (stringDelimiter == '"')
						{
							isString = false;
							stringDelimiter = ' ';
						}
					}
				}
				if (!isString)
				{
					if (ch == '-' && pre == '-') isDashComment = true;
					else if (isDashComment && ch == '\n') isDashComment = false;
					else if (ch == '*' && pre == '/')
					{
						isCComment = true;
					}
					else if (isCComment && ch == '/' && pre == '*') isCComment = false;
				}
				if (!isString && !isCComment && !isDashComment)
				{
					if (char.IsLetterOrDigit(ch) || ch == '[')
					{
						identifier.Add(ch);
					}
					else
					{
						ParseIdent();
					}
				}
				pre = ch;
				i++;
			}
			ParseIdent();

			segments.Reverse();
			var str = new StringBuilder(query);

			foreach (var segment in segments)
			{
				var cmd = str.ToString(segment.Start, segment.Length)
					.Trim();
				if (segment.HasSpecialCommand)
				{
					str.Remove(segment.Start, segment.Length);
					str.Insert(segment.Start, Environment.NewLine);
					str.Insert(segment.Start, "'");
					str.Insert(segment.Start, cmd.Replace("'", "''"));
					str.Insert(segment.Start, "EXECUTE sp_executesql N'");
					str.Insert(segment.Start, Environment.NewLine);
				}
				else
				{
					str.Remove(segment.Start, segment.Length);
					if (cmd != "")
					{
						str.Insert(segment.Start, Environment.NewLine);
						str.Insert(segment.Start, cmd);
						str.Insert(segment.Start, Environment.NewLine);
					}
				}
			}

			//File.WriteAllText(@"C:\GitHub\test.sql", str.ToString().Trim());

			return str.ToString().Trim();
		}
	}
}