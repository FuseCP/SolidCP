using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

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
				int parents = 0;
				var p = Parent;
				while (p != null)
				{
					parents++;
					p = p.Parent;
				}
				var ident = new string(Enumerable.Repeat(' ', parents * 3).ToArray());
				return $"{ident}{Name} = {Value}";
			}
			public void Write(StreamWriter w)
			{
				w.WriteLine(ToString());
			}
		}

		public ConfigSection Parent;

		protected override string GetKeyForItem(Setting setting) => setting.IsComment ? $"comment {IndexOf(setting)}" : 
			setting.IsSection ? $"section {IndexOf(setting)}" : setting.Name;

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

		public void Parse(string configuration)
		{
			var matches = Regex.Matches(configuration, @"^\s*(?<key>.*?)\s*=\s*(?<setting>.*?)\s*$", RegexOptions.Multiline);
			foreach (Match match in matches)
			{
				var key = match.Groups["key"].Value;
				var setting = match.Groups["setting"].Value;
				this[key] = setting;
			}
		}

		public override string ToString()
		{
			return string.Join(Environment.NewLine,
				this.Items
					.Select(setting => setting.ToString())
					.ToArray());
		}



	}
}
