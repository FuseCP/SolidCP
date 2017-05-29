using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SolidCP.WebDav.Core.Config.WebConfigSections;

namespace SolidCP.WebDav.Core.Config.Entities
{
    public class OwaSupportedBrowsersCollection : AbstractConfigCollection, IReadOnlyDictionary<string, int>
    {
        private readonly IDictionary<string, int> _browsers;

        public OwaSupportedBrowsersCollection()
        {
            _browsers = ConfigSection.OwaSupportedBrowsers.Cast<OwaSupportedBrowsersElement>().ToDictionary(x => x.Browser, y => y.Version);
        }

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return _browsers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _browsers.Count; }
        }

        public bool ContainsKey(string browser)
        {
            return _browsers.ContainsKey(browser);
        }

        public bool TryGetValue(string browser, out int version)
        {
            return _browsers.TryGetValue(browser, out version);
        }

        public int this[string browser]
        {
            get { return ContainsKey(browser) ? _browsers[browser] : 0; }
        }

        public IEnumerable<string> Keys
        {
            get { return _browsers.Keys; }
        }

        public IEnumerable<int> Values
        {
            get { return _browsers.Values; }
        } 
    }
}