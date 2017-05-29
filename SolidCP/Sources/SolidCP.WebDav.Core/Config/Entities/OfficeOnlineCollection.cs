using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SolidCP.EnterpriseServer;
using SolidCP.WebDavPortal.WebConfigSections;

namespace SolidCP.WebDav.Core.Config.Entities
{
    public class OfficeOnlineCollection : AbstractConfigCollection, IReadOnlyCollection<OfficeOnlineElement>
    {
        private readonly IList<OfficeOnlineElement> _officeExtensions;

        public OfficeOnlineCollection()
        {
            NewFilePath = ConfigSection.OfficeOnline.CobaltNewFilePath;
            CobaltFileTtl = ConfigSection.OfficeOnline.CobaltFileTtl;
            _officeExtensions = ConfigSection.OfficeOnline.Cast<OfficeOnlineElement>().ToList();
        }

        public bool IsEnabled {
            get
            {
                return GetWebdavSystemSettigns().GetValueOrDefault(EnterpriseServer.SystemSettings.WEBDAV_OWA_ENABLED_KEY, false);
            }
        }
        public string Url
        {
            get
            {
                return GetWebdavSystemSettigns().GetValueOrDefault(EnterpriseServer.SystemSettings.WEBDAV_OWA_URL, string.Empty);
            }
        }

        private SystemSettings GetWebdavSystemSettigns()
        {
            return ScpContext.Services.Organizations.GetWebDavSystemSettings() ?? new SystemSettings();
        }

        public string NewFilePath { get; private set; }
        public int CobaltFileTtl { get; private set; }

        public IEnumerator<OfficeOnlineElement> GetEnumerator()
        {
            return _officeExtensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _officeExtensions.Count; }
        }

        public bool Contains(string extension)
        {
            return _officeExtensions.Any(x=>x.Extension == extension);
        }
    }
}