using System.Configuration;
using SolidCP.WebDavPortal.WebConfigSections;

namespace SolidCP.WebDav.Core.Config.Entities
{
    public abstract class AbstractConfigCollection
    {
        protected WebDavExplorerConfigurationSettingsSection ConfigSection;

        protected AbstractConfigCollection()
        {
            ConfigSection =
                (WebDavExplorerConfigurationSettingsSection)
                    ConfigurationManager.GetSection(WebDavExplorerConfigurationSettingsSection.SectionName);
        }
    }
}