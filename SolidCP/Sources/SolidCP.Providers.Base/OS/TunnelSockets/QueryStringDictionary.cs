using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SolidCP.Providers.OS
{
    public class QueryStringDictionary: OrderedStringDictionary
    {

        public QueryStringDictionary(): base() { }
        public QueryStringDictionary(string url): base()
        {
            int i = url.IndexOf('?');
            if (i >= 0) QueryString = url.Substring(i + 1);
            else Clear();
        }

        public static string QueryEncode(string value) => WebUtility.UrlEncode(value.Replace("%", "%25"))
            .Replace("?", "%3F")
            .Replace("&", "%26")
            .Replace("=", "%3D")
            .Replace(";", "%3B");

        public virtual string QueryString
        {
            get
            {
                var str = new StringBuilder();
                foreach (var par in this)
                {
                    if (str.Length > 0) str.Append("&");
                    str.Append(QueryEncode(par.Key));
                    if ((string)par.Value != "")
                    {
                        str.Append("=");
                        str.Append(QueryEncode(par.Value));
                    }
                }
                return str.ToString();
            }
            set
            {
                Clear();
                if (value.StartsWith("?")) value = value.Substring(1);
                foreach (var par in value.Split('&', ';'))
                {
                    var tokens = par.Split('=');
                    if (tokens.Length > 1) Add(WebUtility.UrlDecode(tokens[0].Trim()), WebUtility.UrlDecode(tokens[1].Trim()));
                    else Add(WebUtility.UrlDecode(tokens[0].Trim()), "");
                }
            }
        }

        public override string ToString() => QueryString;
    }
}
