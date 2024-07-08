using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class ThemeSettingConfiguration: EntityTypeConfiguration<ThemeSetting>
{
    public override void Configure() {

        #region Seed Data
        HasData(() => new ThemeSetting[] {
            new ThemeSetting() { PropertyName = "Light", PropertyValue = "light-theme", SettingsName = "Style", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "Dark", PropertyValue = "dark-theme", SettingsName = "Style", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "Semi Dark", PropertyValue = "semi-dark", SettingsName = "Style", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "Minimal", PropertyValue = "minimal-theme", SettingsName = "Style", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#0727d7", PropertyValue = "headercolor1", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#23282c", PropertyValue = "headercolor2", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#e10a1f", PropertyValue = "headercolor3", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#157d4c", PropertyValue = "headercolor4", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#673ab7", PropertyValue = "headercolor5", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#795548", PropertyValue = "headercolor6", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#d3094e", PropertyValue = "headercolor7", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#ff9800", PropertyValue = "headercolor8", SettingsName = "color-header", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#6c85ec", PropertyValue = "sidebarcolor1", SettingsName = "color-Sidebar", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#5b737f", PropertyValue = "sidebarcolor2", SettingsName = "color-Sidebar", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#408851", PropertyValue = "sidebarcolor3", SettingsName = "color-Sidebar", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#230924", PropertyValue = "sidebarcolor4", SettingsName = "color-Sidebar", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#903a85", PropertyValue = "sidebarcolor5", SettingsName = "color-Sidebar", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#a04846", PropertyValue = "sidebarcolor6", SettingsName = "color-Sidebar", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#a65314", PropertyValue = "sidebarcolor7", SettingsName = "color-Sidebar", ThemeId = 1 },
            new ThemeSetting() { PropertyName = "#1f0e3b", PropertyValue = "sidebarcolor8", SettingsName = "color-Sidebar", ThemeId = 1 }
        });
        #endregion
    }
}
