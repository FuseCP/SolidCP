using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SolidCP.WebDav.Core.Config.WebConfigSections;
using SolidCP.WebDavPortal.WebConfigSections;

namespace SolidCP.WebDav.Core.Config.Entities
{
    public class OpenerCollection : AbstractConfigCollection, IReadOnlyCollection<OpenerElement>
    {
        private readonly IList<OpenerElement> _targetBlankMimeTypeExtensions;

        public OpenerCollection()
        {
            _targetBlankMimeTypeExtensions = ConfigSection.TypeOpener.Cast<OpenerElement>().ToList();
        }

        public IEnumerator<OpenerElement> GetEnumerator()
        {
            return _targetBlankMimeTypeExtensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _targetBlankMimeTypeExtensions.Count; }
        }

        public bool Contains(string extension)
        {
            return _targetBlankMimeTypeExtensions.Any(x => x.Extension == extension);
        }
    }
}