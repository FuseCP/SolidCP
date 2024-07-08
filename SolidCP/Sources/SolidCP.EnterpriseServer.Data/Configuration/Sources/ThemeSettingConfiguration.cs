// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
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
            new ThemeSetting() { ThemeId = 1, SettingsName = "Style", PropertyName = "Light", PropertyValue = "light-theme" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "Style", PropertyName = "Dark", PropertyValue = "dark-theme" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "Style", PropertyName = "Semi Dark", PropertyValue = "semi-dark" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "Style", PropertyName = "Minimal", PropertyValue = "minimal-theme" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#0727d7", PropertyValue = "headercolor1" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#23282c", PropertyValue = "headercolor2" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#e10a1f", PropertyValue = "headercolor3" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#157d4c", PropertyValue = "headercolor4" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#673ab7", PropertyValue = "headercolor5" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#795548", PropertyValue = "headercolor6" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#d3094e", PropertyValue = "headercolor7" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-header", PropertyName = "#ff9800", PropertyValue = "headercolor8" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#6c85ec", PropertyValue = "sidebarcolor1" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#5b737f", PropertyValue = "sidebarcolor2" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#408851", PropertyValue = "sidebarcolor3" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#230924", PropertyValue = "sidebarcolor4" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#903a85", PropertyValue = "sidebarcolor5" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#a04846", PropertyValue = "sidebarcolor6" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#a65314", PropertyValue = "sidebarcolor7" },
            new ThemeSetting() { ThemeId = 1, SettingsName = "color-Sidebar", PropertyName = "#1f0e3b", PropertyValue = "sidebarcolor8" }
        });
        #endregion

    }
}
