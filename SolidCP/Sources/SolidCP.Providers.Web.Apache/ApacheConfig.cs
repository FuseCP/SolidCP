using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SolidCP.Providers.Web.Apache
{

	public struct SingleSetting {
		public string Name;
		public string Value;
		public ApacheConfig Parent;
		public override string ToString() {
			int parents = 0;
			var p = Parent;
			while (p != null) {
				parents++;
				p = p.Parent;
			}
			var ident = new string(Enumerable.Repeat(' ', parents*3).ToArray());
			return $"{ident}{Name}={Value}";
		}
	}

	public class ApacheConfig: KeyedCollection<string, SingleSetting>
	{
		public ApacheConfig Parent;

		protected override string GetKeyForItem(SingleSetting setting) => setting.Name;

		public string this[string key] {
			get {
				if (Contains(key)) return base[key].Value;
				else return string.Empty;
			}
			set {
				if (string.IsNullOrEmpty(value)) {
					if (Contains(key)) Remove(key);
				} else if (Contains(key)) {
					var setting = base[key];
					setting.Value = value;
				} else {
					Add(new SingleSetting { Name = key, Value = value, Parent = this });
				}
			}
		}

		public void Parse(string configuration) {
			var matches = Regex.Matches(configuration, @"^\s*(?<key>.*?)\s*=\s*(?<setting>.*?)\s*$", RegexOptions.Multiline);
			foreach (Match match in matches) {
				var key = match.Groups["key"].Value;
				var setting = match.Groups["setting"].Value;
				this[key] = setting;
			}
		}

		public override string ToString() {
			return string.Join(Environment.NewLine,
				this.Items
					.Select(setting => setting.ToString())
					.ToArray());
		}
	}
}
