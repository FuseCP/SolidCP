﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.ComponentModel.Design;

namespace SolidCP.Providers.Web.Apache
{

	public partial class ConfigSection : KeyedCollection<string, ConfigSection.Setting>
	{
		public struct Setting
		{
			public string Name;
			public string Value;
			public bool IsComment;
			public bool IsSection => Section != null;
			public ConfigSection Section = null;
			public ConfigSection Parent;

			public Setting() { }

			public override string ToString()
			{
				var w = new StringWriter();
				Write(w);
				return w.ToString();
			}
			public void Write(TextWriter w)
			{
				if (IsSection) Section.Write(w);
				else if (IsComment)
				{
					w.Write("# "); w.WriteLine(Value);
				}
				else
				{
					w.Write(Parent.Ident);
					w.Write(Name);
					w.Write(" = ");
					w.Write(Value);
				}
			}
		}

		public string Name;
		public string Argument;
		public ConfigSection Parent;

		string ident = null;
		public string Ident
		{
			get
			{
				if (ident == null)
				{
					if (Parent == null) ident = "";
					else ident = $"{Parent.Ident}   ";
				}
				return ident;
			}
		}

		protected override string GetKeyForItem(Setting setting)
		{
			int index = -1;
			if (setting.IsComment || setting.IsSection) index = IndexOf(setting);
			if (index == -1) index = Count;
			return setting.IsComment ? $"_comment{index}" :
				setting.IsSection ? $"_section{index}" : setting.Name;
		}
		public string this[string key]
		{
			get
			{
				if (Contains(key)) return base[key].Value;
				else return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (Contains(key)) Remove(key);
				}
				else if (Contains(key))
				{
					var setting = base[key];
					setting.Value = value.Trim();
				}
				else
				{
					Add(new Setting { Name = key, Value = value.Trim(), Parent = this, IsComment = false });
				}
			}
		}

		struct LineInfo
		{
			public string Text;
			public int LineNumber;
			public Match? Match;
		} 
		IEnumerable<LineInfo> ParseLines(string configuration)
		{
			var reader = new StringReader(configuration);
			string line;
			int lineno = 1;
			StringBuilder sb = new StringBuilder();
			while ((line = reader.ReadLine()) != null)
			{
				if (line.EndsWith("\\"))
				{
					sb.Append(line.Substring(0, line.Length - 1));
				} else
				{
					if (sb.Length > 0) {
						sb.Append(line);
						line = sb.ToString();
						sb.Clear();
					}
					yield return new LineInfo()
					{
						LineNumber = lineno,
						Text = line,
						Match = Regex.Match(line, @"(?:^[ \t]*(?<key>[a-zA-Z0-9_]*?)[ \t]*=[ \t]*(?<setting>.*?)[ \t]*$)|(?:^[ \t]*<(?<section>/?[a-zA-Z0-9_]+)[ \t]*(?<argument>[^>\n]*?)>[ \t]*$)|(?:^#[ \t]*(?<comment>.*?)[ \t]*$)|(?:^(?<other>.*?)$)", RegexOptions.Singleline)
					};
				}
				lineno++;
			}
		}
		public void Parse(string configuration)
		{
			var lines = ParseLines(configuration);

			var sections = new Stack<ConfigSection>();
			var section = this;

			foreach (LineInfo line in lines)
			{
				var match = line.Match;

				if (match.Groups["key"].Success)
				{
					var key = match.Groups["key"].Value;
					var setting = match.Groups["setting"].Value;
					section[key] = setting;
				}
				else if (match.Groups["section"].Success)
				{
					var name = match.Groups["section"].Value;
					if (name.StartsWith("/"))
					{
						if (name.Substring(1) == section.Name)
						{
							section = sections.Pop();
						}
						else throw new NotSupportedException($"Parse exception on line {line.LineNumber}, expecting </{section.Name}>");
					}
					else
					{
						var parent = section;
						sections.Push(parent);
						switch (name)
						{
							case nameof(AuthnProviderAlias): section = new AuthnProviderAlias(); break;
							case nameof(AuthzProviderAlias): section = new AuthzProviderAlias(); break;
							case nameof(Directory): section = new Directory(); break;
							case nameof(DirectoryMatch): section = new DirectoryMatch(); break;
							case nameof(Else): section = new Else(); break;
							case nameof(ElseIf): section = new ElseIf(); break;
							case nameof(Files): section = new Files(); break;
							case nameof(FilesMatch): section = new FilesMatch(); break;
							case nameof(If): section = new If(); break;
							case nameof(IfDefine): section = new IfDefine(); break;
							case nameof(IfDirective): section = new IfDirective(); break;
							case nameof(IfFile): section = new IfFile(); break;
							case nameof(IfModule): section = new IfModule(); break;
							case nameof(IfSection): section = new IfSection(); break;
							case nameof(IfVersion): section = new IfVersion(); break;
							case nameof(Limit): section = new Limit(); break;
							case nameof(LimitExcept): section = new LimitExcept(); break;
							case nameof(Location): section = new Location(); break;
							case nameof(LocationMatch): section = new LocationMatch(); break;
							case nameof(Macro): section = new Macro(); break;
							case nameof(MDomainSet): section = new MDomainSet(); break;
							case nameof(Proxy): section = new Proxy(); break;
							case nameof(ProxyMatch): section = new ProxyMatch(); break;
							case nameof(RequireAll): section = new RequireAll(); break;
							case nameof(RequireAny): section = new RequireAny(); break;
							case nameof(RequireNone): section = new RequireNone(); break;
							case nameof(VirtualHost): section = new VirtualHost(); break;
							default:
								throw new NotSupportedException($"Invalid section {name} on line {line.LineNumber}.");
						}
						section.Parent = parent;
						section.Argument = match.Groups["argument"].Value;
						parent.Add(new Setting() { Parent = parent, IsComment = false, Name = section.Name, Section = section, Value = section.Argument });
					}
				}
				else if (match.Groups["comment"].Success)
				{
					section.Add(new Setting() { Value = match.Groups["comment"].Value, IsComment = true, Parent = section });
				}
				else
				{
					throw new NotSupportedException($"Parse error at line {line.LineNumber}. Could not parse the line {match.Value}.");
				}
			}
		}

		public IEnumerable<ConfigSection> Sections => this
			.Where(setting => setting.IsSection)
			.Select(setting => setting.Section);
		public void Add(ConfigSection section)
		{
			Add(new Setting() { Parent = this, IsComment = false, Name = section.Name, Section = section });
		}
		public void Remove(ConfigSection section)
		{
			var setting = this.FirstOrDefault(setting => setting.Section == section);
			Remove(setting);
		}

		public override string ToString()
		{
			var w = new StringWriter();
			Write(w);
			return w.ToString();
		}

		public virtual void Write(TextWriter w)
		{
			if (Parent != null)
			{
				w.Write(Ident); w.Write("<"); w.Write(Name);
				if (!string.IsNullOrEmpty(Argument))
				{
					w.Write(" ");
					w.Write(Argument);
				}
				w.WriteLine(">");
			}
			foreach (var item in Items)
			{
				item.Write(w);
			}
			if (Parent != null)
			{
				w.Write(Ident); w.Write("</"); w.Write(Name); w.WriteLine(">");
			}
		}
	}

	// Sections:

	// Enclose a group of directives that represent an extension of a base authentication provider and referenced by the specified alias
	// <AuthnProviderAlias baseProvider Alias> ... </AuthnProviderAlias>
	public class AuthnProviderAlias: ConfigSection {
		public AuthnProviderAlias() { Name = nameof(AuthnProviderAlias); }
	}
	// Enclose a group of directives that represent an extension of a base authorization provider and referenced by the specified alias
	// <AuthzProviderAlias baseProvider Alias Require-Parameters> ... </AuthzProviderAlias> 
	public class AuthzProviderAlias : ConfigSection
	{
		public AuthzProviderAlias() { Name = nameof(AuthzProviderAlias); }
	}
	// Enclose a group of directives that apply only to the named file-system directory, sub-directories, and their contents.
	// <Directory directory-path> ... </Directory>
	public class Directory : ConfigSection
	{
		public Directory() { Name = nameof(Directory); }
	}

	// Enclose directives that apply to the contents of file-system directories matching a regular expression.
	// <DirectoryMatch regex> ... </DirectoryMatch>
	public class DirectoryMatch : ConfigSection
	{
		public DirectoryMatch() { Name = nameof(DirectoryMatch); }
	}

	// Contains directives that apply only if the condition of a previous <If> or <ElseIf> section is not satisfied by a request at runtime
	// <Else> ... </Else>
	public class Else : ConfigSection
	{
		public Else() { Name = nameof(Else); }
	}

	// Contains directives that apply only if a condition is satisfied by a request at runtime while the condition of a previous <If> or <ElseIf> section is not satisfied
	// <ElseIf expression> ... </ElseIf>
	public class ElseIf : ConfigSection
	{
		public ElseIf() { Name = nameof(ElseIf); }
	}

	// Contains directives that apply to matched filenames
	// <Files filename> ... </Files>
	public class Files : ConfigSection
	{
		public Files() { Name = nameof(Files); }
	}

	// Contains directives that apply to regular-expression matched filenames
	// <FilesMatch regex> ... </FilesMatch>
	public class FilesMatch : ConfigSection
	{
		public FilesMatch() { Name = nameof(FilesMatch); }
	}

	// Contains directives that apply only if a condition is satisfied by a request at runtime
	// <If expression> ... </If>
	public class If : ConfigSection
	{
		public If() { Name = nameof(If); }
	}

	// Encloses directives that will be processed only if a test is true at startup
	// <IfDefine [!]parameter-name> ...     </IfDefine>
	public class IfDefine : ConfigSection
	{
		public IfDefine() { Name = nameof(IfDefine); }
	}

	// Encloses directives that are processed conditional on the presence or absence of a specific directive
	// <IfDirective [!]directive-name> ...     </IfDirective>
	public class IfDirective : ConfigSection
	{
		public IfDirective() { Name = nameof(IfDirective); }
	}

	// Encloses directives that will be processed only if file exists at startup
	// <IfFile [!]filename> ...     </IfFile>
	public class IfFile : ConfigSection
	{
		public IfFile() { Name = nameof(IfFile); }
	}

	// Encloses directives that are processed conditional on the presence or absence of a specific module
	// <IfModule [!]module-file|module-identifier> ...     </IfModule>
	public class IfModule : ConfigSection
	{
		public IfModule() { Name = nameof(IfModule); }
	}

	// Encloses directives that are processed conditional on the presence or absence of a specific section directive
	// <IfSection [!]section-name> ...     </IfSection>
	public class IfSection : ConfigSection
	{
		public IfSection() { Name = nameof(IfSection); }
	}

	// contains version dependent configuration
	// <IfVersion [[!]operator] version> ... </IfVersion>
	public class IfVersion : ConfigSection
	{
		public IfVersion() { Name = nameof(IfVersion); }
	}

	// Restrict enclosed access controls to only certain HTTP methods
	// <Limit method [method] ... > ...     </Limit>
	public class Limit : ConfigSection
	{
		public Limit() { Name = nameof(Limit); }
	}

	// Restrict access controls to all HTTP methods except the named ones
	// <LimitExcept method [method] ... > ...     </LimitExcept>
	public class LimitExcept : ConfigSection
	{
		public LimitExcept() { Name = nameof(LimitExcept); }
	}

	// Applies the enclosed directives only to matching URLs
	// <Location     URL-path|URL> ... </Location>
	public class Location : ConfigSection
	{
		public Location() { Name = nameof(Location); }
	}

	// Applies the enclosed directives only to regular-expression matching URLs
	// <LocationMatch     regex> ... </LocationMatch>
	public class LocationMatch : ConfigSection
	{
		public LocationMatch() { Name = nameof(LocationMatch); }
	}

	// Define a configuration file macro
	// <Macro name [par1 .. parN]> ... </Macro>
	public class Macro : ConfigSection
	{
		public Macro() { Name = nameof(Macro); }
	}

	// Container for directives applied to the same managed domains.
	// <MDomainSet dns-name [ other-dns-name... ]>...</MDomainSet>
	public class MDomainSet : ConfigSection
	{
		public MDomainSet() { Name = nameof(MDomainSet); }
	}

	// Container for directives applied to proxied resources
	// <Proxy wildcard-url> ...</Proxy>
	public class Proxy : ConfigSection
	{
		public Proxy() { Name = nameof(Proxy); }
	}

	// Container for directives applied to regular-expression-matched proxied resources
	// <ProxyMatch regex> ...</ProxyMatch>
	public class ProxyMatch : ConfigSection
	{
		public ProxyMatch() { Name = nameof(ProxyMatch); }
	}

	// Enclose a group of authorization directives of which none must fail and at least one must succeed for the enclosing directive to succeed.
	// <RequireAll> ... </RequireAll>
	public class RequireAll : ConfigSection
	{
		public RequireAll() { Name = nameof(RequireAll); }
	}

	// Enclose a group of authorization directives of which one must succeed for the enclosing directive to succeed.
	// <RequireAny> ... </RequireAny>
	public class RequireAny : ConfigSection
	{
		public RequireAny() { Name = nameof(RequireAny); }
	}

	// Enclose a group of authorization directives of which none must succeed for the enclosing directive to not fail.
	// <RequireNone> ... </RequireNone>
	public class RequireNone : ConfigSection
	{
		public RequireNone() { Name = nameof(RequireNone); }
	}

	// Contains directives that apply only to a specific hostname or IP address
	// <VirtualHost addr[:port] [addr[:port]] ...> ... </VirtualHost>
	public class VirtualHost : ConfigSection
	{
		public VirtualHost() { Name = nameof(VirtualHost); }
	}


}