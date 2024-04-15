using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SolidCP.Providers.OS
{
    public class QueryStringDictionary: OrderedNameDictionary<string?>
    {

        public QueryStringDictionary(): base() { }
        public QueryStringDictionary(string url) : base()
        {
            if (!string.IsNullOrEmpty(url))
            {
                int i = url.IndexOf('?');
                if (i >= 0) QueryString = url.Substring(i + 1);
            }
        }

        public virtual string QueryString
        {
            get
            {
                var str = new StringBuilder();
                foreach (var par in this)
                {
                    if (str.Length > 0) str.Append("&");
                    str.Append(Uri.EscapeDataString(par.Key));
                    if ((string)par.Value != "")
                    {
                        str.Append("=");
                        str.Append(Uri.EscapeDataString(par.Value));
                    }
                }
                return str.ToString();
            }
            set
            {
                Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.StartsWith("?")) value = value.Substring(1);
                    foreach (var par in value.Split('&', ';'))
                    {
                        var tokens = par.Split('=');
                        if (tokens.Length > 1) Add(Uri.UnescapeDataString(tokens[0].Trim()), Uri.UnescapeDataString(tokens[1].Trim()));
                        else Add(Uri.UnescapeDataString(tokens[0].Trim()), "");
                    }
                }
            }
        }

        public override string ToString() => QueryString;
    }
}
