using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SolidCP.WebDav.Core.Config.WebConfigSections;
using SolidCP.WebDavPortal.WebConfigSections;

namespace SolidCP.WebDav.Core.Config.Entities
{
    public class FilesToIgnoreCollection : AbstractConfigCollection, IReadOnlyCollection<FilesToIgnoreElement>
    {
        private readonly IList<FilesToIgnoreElement> _filesToIgnore;

        public FilesToIgnoreCollection()
        {
            _filesToIgnore = ConfigSection.FilesToIgnore.Cast<FilesToIgnoreElement>().ToList();
        }

        public IEnumerator<FilesToIgnoreElement> GetEnumerator()
        {
            return _filesToIgnore.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _filesToIgnore.Count; }
        }

        public bool Contains(string name)
        {
            return _filesToIgnore.Any(x => x.Name == name);
        }
    }
}