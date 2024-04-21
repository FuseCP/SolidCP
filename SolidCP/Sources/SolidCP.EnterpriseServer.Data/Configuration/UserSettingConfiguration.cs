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

public partial class UserSettingConfiguration: Extensions.EntityTypeConfiguration<UserSetting>
{

    public UserSettingConfiguration(): base() { }
    public UserSettingConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {

#if NetCore
        HasOne(d => d.User).WithMany(p => p.UserSettings).HasConstraintName("FK_UserSettings_Users");
#else
        HasRequired(d => d.User).WithMany(p => p.UserSettings);
#endif

#region Seed Data
        HasData(
            new UserSetting() { UserId = 1, SettingsName = "AccountSummaryLetter", PropertyName = "CC", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "AccountSummaryLetter", PropertyName = "EnableLetter", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "AccountSummaryLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "AccountSummaryLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Account Summary " +
                "Information</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-color:" +
                " ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; fo" +
                "nt-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid" +
                " 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { fo" +
                "nt-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; co" +
                "lor: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font" +
                "-size: 1.3em; color: ##1F4978; }\r\n        .Summary TABLE { border: solid 1px ##e" +
                "5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-s" +
                "ize: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD " +
                "{ padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; fo" +
                "nt-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: n" +
                "ormal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\">" +
                "</a>\r\n<div class=\"Header\">\r\n\tHosting Account Information\r\n</div>\r\n\r\n<ad:if test=" +
                "\"#Signup#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n\r\n<p>\r\nNew user account has bee" +
                "n created and below you can find its summary information.\r\n</p>\r\n\r\n<h1>Control P" +
                "anel URL</h1>\r\n<table>\r\n    <thead>\r\n        <tr>\r\n            <th>Control Panel" +
                " URL</th>\r\n            <th>Username</th>\r\n            <th>Password</th>\r\n       " +
                " </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n            <td><a href=\"http:/" +
                "/panel.HostingCompany.com\">http://panel.HostingCompany.com</a></td>\r\n           " +
                " <td>#user.Username#</td>\r\n            <td>#user.Password#</td>\r\n        </tr>\r\n" +
                "    </tbody>\r\n</table>\r\n</ad:if>\r\n\r\n<h1>Hosting Spaces</h1>\r\n<p>\r\n    The follow" +
                "ing hosting spaces have been created under your account:\r\n</p>\r\n<ad:foreach coll" +
                "ection=\"#Spaces#\" var=\"Space\" index=\"i\">\r\n<h2>#Space.PackageName#</h2>\r\n<table>\r" +
                "\n\t<tbody>\r\n\t\t<tr>\r\n\t\t\t<td class=\"Label\">Hosting Plan:</td>\r\n\t\t\t<td>\r\n\t\t\t\t<ad:if " +
                "test=\"#not(isnull(Plans[Space.PlanId]))#\">#Plans[Space.PlanId].PlanName#<ad:else" +
                ">System</ad:if>\r\n\t\t\t</td>\r\n\t\t</tr>\r\n\t\t<ad:if test=\"#not(isnull(Plans[Space.PlanI" +
                "d]))#\">\r\n\t\t<tr>\r\n\t\t\t<td class=\"Label\">Purchase Date:</td>\r\n\t\t\t<td>\r\n\t\t\t\t#Space.P" +
                "urchaseDate#\r\n\t\t\t</td>\r\n\t\t</tr>\r\n\t\t<tr>\r\n\t\t\t<td class=\"Label\">Disk Space, MB:</t" +
                "d>\r\n\t\t\t<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.D" +
                "iskspace\" /></td>\r\n\t\t</tr>\r\n\t\t<tr>\r\n\t\t\t<td class=\"Label\">Bandwidth, MB/Month:</t" +
                "d>\r\n\t\t\t<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.B" +
                "andwidth\" /></td>\r\n\t\t</tr>\r\n\t\t<tr>\r\n\t\t\t<td class=\"Label\">Maximum Number of Domai" +
                "ns:</td>\r\n\t\t\t<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota" +
                "=\"OS.Domains\" /></td>\r\n\t\t</tr>\r\n\t\t<tr>\r\n\t\t\t<td class=\"Label\">Maximum Number of S" +
                "ub-Domains:</td>\r\n\t\t\t<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]" +
                "#\" quota=\"OS.SubDomains\" /></td>\r\n\t\t</tr>\r\n\t\t</ad:if>\r\n\t</tbody>\r\n</table>\r\n</ad" +
                ":foreach>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nIf you have any questions regarding y" +
                "our hosting account, feel free to contact our support department at any time.\r\n<" +
                "/p>\r\n\r\n<p>\r\nBest regards,<br />\r\nSolidCP.<br />\r\nWeb Site: <a href=\"https://soli" +
                "dcp.com\">https://solidcp.com</a><br />\r\nE-Mail: <a href=\"mailto:support@solidcp." +
                "com\">support@solidcp.com</a>\r\n</p>\r\n</ad:if>\r\n\r\n<ad:template name=\"NumericQuota\"" +
                ">\r\n\t<ad:if test=\"#space.Quotas.ContainsKey(quota)#\">\r\n\t\t<ad:if test=\"#space.Quot" +
                "as[quota].QuotaAllocatedValue isnot -1#\">#space.Quotas[quota].QuotaAllocatedValu" +
                "e#<ad:else>Unlimited</ad:if>\r\n\t<ad:else>\r\n\t\t0\r\n\t</ad:if>\r\n</ad:template>\r\n\r\n</di" +
                "v>\r\n</body>\r\n</html>" },
            new UserSetting() { UserId = 1, SettingsName = "AccountSummaryLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "AccountSummaryLetter", PropertyName = "Subject", PropertyValue = "<ad:if test=\"#Signup#\">SolidCP  account has been created for<ad:else>SolidCP  ac" +
                "count summary for</ad:if> #user.FirstName# #user.LastName#" },
            new UserSetting() { UserId = 1, SettingsName = "AccountSummaryLetter", PropertyName = "TextBody", PropertyValue = "=================================\r\n   Hosting Account Information\r\n=============" +
                "====================\r\n<ad:if test=\"#Signup#\">Hello #user.FirstName#,\r\n\r\nNew user" +
                " account has been created and below you can find its summary information.\r\n\r\nCon" +
                "trol Panel URL: https://panel.solidcp.com\r\nUsername: #user.Username#\r\nPassword: " +
                "#user.Password#\r\n</ad:if>\r\n\r\nHosting Spaces\r\n==============\r\nThe following hosti" +
                "ng spaces have been created under your account:\r\n\r\n<ad:foreach collection=\"#Spac" +
                "es#\" var=\"Space\" index=\"i\">\r\n=== #Space.PackageName# ===\r\nHosting Plan: <ad:if t" +
                "est=\"#not(isnull(Plans[Space.PlanId]))#\">#Plans[Space.PlanId].PlanName#<ad:else>" +
                "System</ad:if>\r\n<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">Purchase Date: " +
                "#Space.PurchaseDate#\r\nDisk Space, MB: <ad:NumericQuota space=\"#SpaceContexts[Spa" +
                "ce.PackageId]#\" quota=\"OS.Diskspace\" />\r\nBandwidth, MB/Month: <ad:NumericQuota s" +
                "pace=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Bandwidth\" />\r\nMaximum Number " +
                "of Domains: <ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS." +
                "Domains\" />\r\nMaximum Number of Sub-Domains: <ad:NumericQuota space=\"#SpaceContex" +
                "ts[Space.PackageId]#\" quota=\"OS.SubDomains\" />\r\n</ad:if>\r\n</ad:foreach>\r\n\r\n<ad:i" +
                "f test=\"#Signup#\">If you have any questions regarding your hosting account, feel" +
                " free to contact our support department at any time.\r\n\r\nBest regards,\r\nSolidCP.\r" +
                "\nWeb Site: https://solidcp.com\">\r\nE-Mail: support@solidcp.com\r\n</ad:if><ad:templ" +
                "ate name=\"NumericQuota\"><ad:if test=\"#space.Quotas.ContainsKey(quota)#\"><ad:if t" +
                "est=\"#space.Quotas[quota].QuotaAllocatedValue isnot -1#\">#space.Quotas[quota].Qu" +
                "otaAllocatedValue#<ad:else>Unlimited</ad:if><ad:else>0</ad:if></ad:template>" },
            new UserSetting() { UserId = 1, SettingsName = "BandwidthXLST", PropertyName = "Transform", PropertyValue = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<xsl:stylesheet version=\"1.0\" xmlns:xsl=" +
                "\"http://www.w3.org/1999/XSL/Transform\">\r\n<xsl:template match=\"/\">\r\n  <html>\r\n  <" +
                "body>\r\n  <img alt=\"Embedded Image\" width=\"299\" height=\"60\" src=\"data:image/png;b" +
                "ase64,iVBORw0KGgoAAAANSUhEUgAAASsAAAA8CAYAAAA+AJwjAAAACXBIWXMAAAsTAAALEwEAmpwYAA" +
                "A4G2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTX" +
                "BDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeD" +
                "p4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMTQgNzkuMTUxNDgxLCAyMDEzLzAzLzEzLTEyOjA5Oj" +
                "E1ICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMD" +
                "IvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogIC" +
                "AgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgIC" +
                "AgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG" +
                "1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgIC" +
                "AgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgIC" +
                "AgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW" +
                "50IyIKICAgICAgICAgICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCi" +
                "AgICAgICAgICAgIHhtbG5zOmV4aWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vZXhpZi8xLjAvIj4KICAgIC" +
                "AgICAgPHhtcDpDcmVhdG9yVG9vbD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC94bXA6Q3JlYX" +
                "RvclRvb2w+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC" +
                "94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMTYtMDMtMDFUMTQ6NTE6NT" +
                "grMDE6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE2LTAzLT" +
                "AxVDE0OjUxOjU4KzAxOjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8ZGM6Zm9ybWF0PmltYW" +
                "dlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3" +
                "A6Q29sb3JNb2RlPgogICAgICAgICA8eG1wTU06SW5zdGFuY2VJRD54bXAuaWlkOjZhNTdmMWYyLTgyZj" +
                "YtMjk0MS1hYjFmLTNkOWQ0YjdmMTY2YjwveG1wTU06SW5zdGFuY2VJRD4KICAgICAgICAgPHhtcE1NOk" +
                "RvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE" +
                "1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2YT" +
                "U3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD" +
                "4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgIC" +
                "AgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RX" +
                "Z0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbn" +
                "N0YW5jZUlEPnhtcC5paWQ6NmE1N2YxZjItODJmNi0yOTQxLWFiMWYtM2Q5ZDRiN2YxNjZiPC9zdEV2dD" +
                "ppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE2LTAzLTAxVDE0OjUwOj" +
                "QzKzAxOjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD" +
                "5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgIC" +
                "AgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3" +
                "Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgIC" +
                "AgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgIC" +
                "AgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgIC" +
                "A8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZj" +
                "pDb2xvclNwYWNlPjY1NTM1PC9leGlmOkNvbG9yU3BhY2U+CiAgICAgICAgIDxleGlmOlBpeGVsWERpbW" +
                "Vuc2lvbj4yOTk8L2V4aWY6UGl4ZWxYRGltZW5zaW9uPgogICAgICAgICA8ZXhpZjpQaXhlbFlEaW1lbn" +
                "Npb24+NjA8L2V4aWY6UGl4ZWxZRGltZW5zaW9uPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC" +
                "9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCi" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIA" +
                "ogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCi" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIA" +
                "ogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/Pq+oh5kAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAH" +
                "UwAADqYAAAOpgAABdvkl/FRgAALU1JREFUeNrsnXmcnFWV97/neZ6q6jXd2XcI+76DoqACKgq4gPsAgg" +
                "vIoDjovOq4oDMqCu7buOAuKg4iyCCCG4uyCI4CYQlJCIGErJ1O70tVPc+95/3j3uqurtTWSTpErZPP80" +
                "lVddXz3PV3zzn3d86Vh+YtoSABIP7/QGTsNf5z/OfF74VxUbYVG4TM6e1i9dw9Oeb7D2A0gO5eCIPir4" +
                "VAB3AAsAQ4ApgDzAfaAQP0ARuAx/21GlgH5KglxoKWKV0UQv8gbNoMYehqYP1lDFjrXieJu0eSQBxD03" +
                "Quu+cqPv7IDaxpnVXz8WkgLG1PEaZK1NdVAev/N4Ap1wa+D7cYg0XH+5kdL1+iyoJUilYRDLuXKLA+jj" +
                "ElYxggVmVuFNEZhiQV2myyMqu/i2+e+CbeeeHXYLDLjbcoKuoEgVCmvuKJnwvqx3U2B81N0NrsxvtUi9" +
                "XyQAHjz1c//2B8zgHRszxmWoAjgVOBFwInAqk6f7sK+A1wC3Av0E9DGtKQf1h5NsHqROBtwBs9aE1W9g" +
                "UuAf4VuBn4NvA7r0Q0pCEN+QeTYOofIagEpJM4UBEQmQ9cAdwBvHU7gaoUcM8Efg18Gdij0a0NaUhDsy" +
                "qVVmAezr8027tIImBPoElFrBXpmDbUG62fPueKKI73ygfpryM8f0pQ0WlazwfeA9zV6N6GNOSfG6yagR" +
                "OA4/z/ewILLEz3JlgIiKhigpDFXWsYaGm76rK3Xf4SNXyFwcGFBFOq0B0N/C9wDnBro4sb0pB/PrBKAa" +
                "9T1XOAk9RpVaWO/bH7mSBk4ZZ1jASpK8993w9vufWs11zNmuGFxPmJuyBTI9OBa4CzG4DVkIb8Y0i9Ks" +
                "4rcDtvP7VwhoVW622+4mt8q1wJk5iujtnXveW937nh1jNe823WDC0hl4VUalfVrRP4qTcLG9KQhvyDa1" +
                "bNwGeA86zjQW0j5SgT2SjNgeufXPG+8z/xlV+d9cZP0TVyIHEOUmF5vtOEG+oAIvcDDwNP+U/TwN6oHg" +
                "gcj0jbJDSsn4CeDKyhmNukCkHAFJukDWlIQ3YBWO2j8B0LJxd/WI42JkUkxCQImdW3Jf/AXod87OYTX3" +
                "0GvfFLGRp2oBBXZRX0IfwAkRsQWQ5sLcHCEJiO6gFY+1KUC4CFNWso7AX6IRJzKarjBNIggP5R6Ol3Zu" +
                "lOIv81pCEN2bVg9RxVvVodo7ymJlUQE4TMHOwhH4a/uOiSq3qfWnDg29iypfDXauX4I4F8hDD8CyJxpd" +
                "sD3e7SezH2Oqz9KI6nVQ2sAN6O1Ru9KTuuWcUx5PIQBQ2wasjUiiqB2kY77EywEjhU4XvlgKoWWIlamn" +
                "LD9qcnvO6Gx/Y49HwdGJhLHEP10JJrCIN/h2jzZLoeeAz0AhLzFPABqvvfIuDVWHsHkEPE0fj7B+szTX" +
                "euhL48nd5M7QQSoAmY4etRb4HEK7s9QN4D+hZ/5dkNCLJ1VqSD8d1kW1K/EBjymnZBZuPCsLJMjJYpfH" +
                "8A6K3VaPWUWyrXQXx/tQPlFtiUL0ePqBJHKUbSLZXGWlg0LjqANmBmUf/NwG1oTaY/A//8QV/dLX6xz/" +
                "nxVq2Tip+vO2kYpHFRJsMT1IiJRputVsdSsJqr8CUcYJXtqDIlj4EhK5JvHx1JNnfM/d53XvaOocSYkx" +
                "nqr7Xa/JIouhQJurerCUSGgI+TJAL8R41vvwTHCVszBlbW7kqfVRPwXOA04CBVnQ/M8oOiwE/b3t2HvL" +
                "9HgoufXAU8ATwI/L5kou+6lVCETUnMwjAiI1ININ4FXOgHsymZcM3Abf47BfmQ16g3lgz6woS/AfgYVV" +
                "T0LmPKxgUCpETYmrj53FG53K3AfwJn4OJWS4EsA/xaRT4wZ7CbWw48kQ+/4WPQt7GwGx4Ac3FulqOAuR" +
                "id48dDh/+bLbqXbCdIFBat9YzH1v4Z+C3QRxjA6CgEAplMIT7vdcB7/T2SnQRWTX5R3eDrXhp23OPBdC" +
                "1wJy7ud7AiWCl8SN2kHnuCVl53RoDlwM+shH8SbO8+fZtzZ/zH9WsfPfRFV7H+yQWg1Zr4UZAPYG03xt" +
                "TSviqMOgPWZrH2Sqw9FOGMaj44rC4G1mCsC+DcdUA1A7hC4UIDUxGtmil6PRM4rOj9Qzhm/4/rUCimRL" +
                "PKqhJVB6sDcQHslaS7BAiWAAv8VU6WVypLBGwyCXl1yFZRLRGh2xhsENAeBOXKnvHl3rvKbVYH1shApk" +
                "0fmb8/2XDMN9oJ+k7i+K24sLGpXiTxmuuhuDjcS4F7gI/7xQziZNw94riThz+LCvlW4C/Af+NC6BKAqE" +
                "jdfami7y50qhT/vy1iLQM+C1wjQmyCkD16NrFq+jy6Oue2MTj4Imf+VQSDmICPI6wiZyCbhc5Ot+LUE/" +
                "kdhG6k9Y+CWhD6UK7A6otBmipq/8ICVJ2fSmSXmH8C7QrfS+DMZ8MrJi5Q/LtAi8A3y03ggKlB0IKq02" +
                "0NFshUXhyGatymv6TItVb7gUp20ajqmLpST9l7rMUCLUFQungn3gytAtQyNHeoj3v3PIwP/svl0Lse0t" +
                "EMrLkaa87g2XWTnoBwDcjrCcI7SYybS05GvcWUepbKNhM4DeE04JOI/BdgI+sKOE2Qj+CUwQmdUka7ul" +
                "HgvQJPmyBkWn6YhV1reHrmQl777qtZvvDgU9j09DyCsNpCfguB3Oye5tFwNOuAasH8ykueAH2j0NfjUr" +
                "YYO/4M5W8Yez2OuV7pBrM9GBZ8DZuoJ8VMGbDsyA0SxKMYCQirOE4VLk7gTH12HfiRwOXAbQorS5t0VH" +
                "VKnVsB0GsNsyprz1Ibc3cYtMmr0med+RfVecsQ6LcWAzRX1w63/a1NZGNrJ78/8AQYHQAJMlj7FRJ7xs" +
                "6p1Q6rvbMQvolyAlZ7HIo/24UqWZaEjwKPY+zPIq89vdjCi4rbT4sgqmia/UKQN4tfUdpzwzw+Z29uO+" +
                "hkvvT8N7Fy0eGwcfUhhFGHA5KyYgmC69EgS1J05/wgkclz8oO/JVKLlmm09uF+rnvZW8GmoGsdpNPFf8" +
                "4C12NMNbDqw5hWovBHwGKvIa4Alno/z6N1NWJumD8uOoYXb3qM2dkBRsJ0pQkyLxA5dzfZZ5yh8Bbgw6" +
                "VAstUmqAqpKR6no9bSNMkJXwF36uEHbuPtHrSWRCefNioAhqwbk5lJlL85n+Vviw/hitddBlvWggSnku" +
                "i57EZ4ABxIkpxJYr9PKirNM7e7ANZl5JM/ROr68awCJJkKo0PhfnFO0Cw4msI+PZu5/pBT+PD7roEnV8" +
                "CGJyGKFpJUXafXEOp9YwnAxpahiCTTzpVfuYhpSb5sOaYDx6y8jw/+yxeheRrkhiauBMpjWLMRZX6FZz" +
                "8FHEliXuHfH1H0t6e9/f5b4Eaq7byM9HLzIa/ikK4VfHjp9fS3zao0yA+J4LDdiBRxlFcWTLFtHLJrku" +
                "P1W4sJAppEdtQCqpWpo7XU/h9SS6K63WlGAmDYm5CpOrWPJAiIkjxsXgdJHpCTnJ90N6PJqBxDGHzf+Y" +
                "B3ywxLB6McGVllD3FOtwlaVEkG0GGE9wn0CaASsGCwm0fm7sGvjz0LnlwO2SHnS7I6o2pnKA+QJKvGHG" +
                "KFJ0ybx7m//296O2fRp1pWs3o6jHjN7deS9A5w2blfAo0gN1J8oz5EViJlwSqPtQlB8G9liycswYHxhb" +
                "j8WN/zoFXeZ5YfYr/+9TSbfFlT0AP8vnFdg0XB5VDsBYakvjCogoXeAXTWqb7P8/6ArkIZR9SQqBJO8X" +
                "JfuPugNaTCaLu0K3GXAt8BHmGi472AKSng/ybYwMCoWvKqpAPZoTqMqiWQsC64SYDEWkhiiJNmJFiCbA" +
                "cYTMqFIEULeN2/O9y7RnrYbUWOjBQ9BEdZmDCoSgbTNSh3F77Tkh/m4VlL+PfXfoJlS46G7jUQRsVmfj" +
                "VZgYhOaMsow1tv/QLvuf0qels7SYLyWn5kElbPWcSZj/4G87OI/3z9p12JTVwoeS+wscLEXY3qxVj7+j" +
                "pa5hXAS4GfAJ8E1mxr0/TyzSNex4KRHg7sW89gqqnc2N6/roEmcrW4Z/UDo0H93gz1cZovFWs/gOPH1J" +
                "pvqfGOEkbVYph6sCogiQEGjZm0dlXgRnm6wfW4q87fqQPkHfTHFMowai2pOqAg8b4ugmbAdpLk5tbdzF" +
                "YB/oRINyLGA0muzPwqLFrTgA5UFdV24EBEFtX5vCavrfZMEkQHUUaRSWdvSYBOlDQidY52OSBSOF5Lal" +
                "6CycZP2rG/hEmWZTMXs2yPY2DjKqdpmHhc564+MfMTTTcLzTM445HfkU01EQcpqg3j0FrWdy7gVY/8Fl" +
                "Hl8pf/P/JhCtSA28FYWeGn04E31wGmBckAbwc5Fpcn6+6Jetowf9vreax96DqO27yCnlQzwcRyZ3DkxV" +
                "pyWwgXCWSlaFJPzqTnPuNyil1S4+tRAdWdlmAwqmN59UtkD6+JzfL/p/wgU8b5YeIXiE2Mc5wMjitT+H" +
                "xtaXnzqqS3w3fliGRaP5L7toxVsW5mlPbPQcAiHKdJfNnn+PoO+LILjqu03Ps0R3ybCSBadUYqBoEgA2" +
                "SnkcvNdxtPNUv+CEHwPpClBAz5xX2kjr51K6a1GZS5oG9DeTeOkFlN0mzPzp/wQUTuRrV5kr+MvTVwJK" +
                "rvQMsT0EvaZI9IXYdVhAdFHxBk+QSzUAKa4xwM9ri+LKYbqCY10DiY8P1MK5fedDkzR/sYzrRQz3orqv" +
                "S2dnLi6r+QJ3SDIT9cKPCyCrWZu50L6hGIXANcjMtG6gsRQu8zfO6485ib7eOA3nUMpzLl5lcteSJvNb" +
                "u9fgz1GlIo3GNcmuiA8ozqdopIl4Uv2Yno/ULgtcAxis7ymlqzX3UL27vKOHenIAUmecEhM+g/G1anlf" +
                "7Om9bLC5rOqFpS2+G78mX9T+B0xtnQxcqb4PLyf8ahj6JM8FUdBbxT0aP9YtJW5AMrrVuuaMHuBTYL/F" +
                "Lh83m1uVDEVg0/A6xaJclDkk+TJE1IzSExCPwXmfTvJqnsJhQoIIEMgWwln/8PjHkOyAvqcA/MYjxxQH" +
                "06fTr9F4Lg4R2gAN2OSR7C6PW4SI5q0hxZdEYNM3mZogMyQUNVbLkCioAxXTW4UhO1DYk4fP1jTMsO0d" +
                "PcSb3DNwlCrIT8+KfvZOWsvfnkKz4CSQ6S5C6S5G+IHLPzzGVZDPLdIn+WH40xT87ej82ZaRxkE4xmKr" +
                "lqqsnJBj1e0fu2t8sTlFDllgg5qWieFLl5EL+R0gd0C5Dz2oZ34eyj8FGrnOEH7WRBpBS8Zha93l8cS/" +
                "s9wA+BTyrkjI6j33b4vp7jr0rSTZGqVMQbvMwq7wLm1Vm/TIlTf6HAkeo2Tf7NKj3VOtj4g5KIE4gTS5" +
                "wkNYnIyuOEwS/J5twJOM1Nk7S1AxjJQT4HiEHpqq7/jYG8TopLoYDJttHc7AhPOrmfjw0wq/eSJH/GRX" +
                "ZUExNZrT4u1anCE8hvRgRrrSNzZnOlKLea8Yyh5eQwb5K52K0kYTjdgglCJuvFCNWwV8869u9aTWgsl7" +
                "/8QyTWbMCYj6J8B5E5Xj0uuDtM0QIdTdLimofIlxDZBPx1DJyHt/ChEy/m27//FHv1byQbpYtV3aE67n" +
                "uAwLXi0jAP+t9t8Gp/xq+Yz+CIeoUwhRDY7M2sCEglqhsT9G+UUOOs/0HGE3SdRqXE4+bUYkV/lqDHTa" +
                "G7KsJlyPiQIDME3qtoLr/9u2K1TKLBUpcz8JkE+152PJV3ALxBHAt9pLoZaEnUQj4P+VhJYq1ClC4sjK" +
                "sIUoq1447yyRKYVSExDuysfhXltirrTxoX2vK4c6Xo5NaOOIa2NgdYVicHVqOjYK2i+nQ9v4gmnuJV9m" +
                "Gbyj5JLeTjsTO9Jqh2biJVCoU4GLVHoHonCAR5muJcsb9nujfZch5cYj9hR/zruFBQRRhOtyBRhtOX/w" +
                "HUcuVpl5GNMrcyMvBC4vxJSLCfdxw+g4slK8Rk7Ys7YefISZiI+yLyaUReOWYiqNLbMh2xFqzFjC8wZh" +
                "Jq9R5MJLOWKATE477lMXNryE/KwJ84N2SdWWR9nz2D45DdZ9DHh60ZjERokgmhI6LKfxiXonpXSAB6vs" +
                "KfE9UfB+riB7cDsrSev6s3OQPkFQLvZuee5nSqHwO20qJnFdSq06ySJCQxYQ0tR4HNY6Ev1rqQsukd3g" +
                "CfREuN+yH/5K/a3zf+eZPdiOjth6Fhigd/7ZEg0NLsDyiVdD0/iSwalKrZJU3SWcaPRT6IIMp4x/aEEj" +
                "6K5eEqYDUD9GWo3ulWgJinpi3k2M3LiNSQSDjD+xVcwj2XU31Pdf6CJ3B8qM1e+1gJ9FgJBntbOnnF8t" +
                "8TBxE/P/481ki4Ot8+dzWj/WCS8h3guF4HA+9EeTWqi+roqJeCvhfkyrHBE+dZNW0+84e2ABY73h5Pbu" +
                "dECMtoJuVMr1l13CsRuFnhi3m1dw3bhIigABL7inA6u1ZagAv87qeaKeYcCYSKvkG3NVV3hmSqbgiId5" +
                "c4M3AOSTKrhmaVTFAOVKGn1wHIrJnufTUXi6o/PDV05M4gcA4AraOVrD/Et16wKt6JGxn15ZqMGSkwPA" +
                "pRFJIO96ynK6M6siosUQcc+cIHeYmYMdrHnl0rWdM8fXwncEwvsF9G9YWIVCLvnY2x12D1EeI+rnjexS" +
                "TGcvbq2+hLtz1pJHjSP3Nf5zPj+QqnAC8u2ug0XnN4FLhHkT90tc1++uTV946c/eBNfPM5b+Te/U/hgc" +
                "5FMG0uDG8Fm5Rr0GUol2DtVah+HtVT6+isd5HYW7D6sLtdlg8e/y7+8L/vojnJkQvTBdBfZpUu3A7Tsy" +
                "URcKb3G50XIDdZlNjtaJ2CyoI679MD9Hue05CfWIWsCGnvgppH7Z0ngL0E9ldYMZVRSL4P9gHq9V/mvE" +
                "8vV6TNF9L3NAOBjqf1qSlGwdgxsGolNk11+I+2rcWWHmfWLZwHUboyYIUhDI24hJLGeNOsDj1UPP0nMU" +
                "V7xXW1bi+5PBNM1slr2q8mNs+tZwGPVFlfrjZFn+ynbsdkjIPRl27m+PVLufie7/HB138buldBbtDtkL" +
                "l6/BZrbgZ5QxWz5/2g52EUhnv53KHnEcQxb157O0kQ0ZXuyAu6zIPVtaiGCudalyrkOK99HOyvNwBbRf" +
                "WWOIi+tWrW4nvPevx3XHL/tVx54vmsWHAYf150FLTOhMEtlXrvEVRPw9jvA+fXaLhFKOeAPop1kdRic9" +
                "w253BO3vAAGFMgta7AkUs/xLMvHcC31GVhWOsmkx4LmqnDP/RTgV+KyJO+8Xr94iVeU2q3bvv6uep2TY" +
                "+scc92328rprrSis4RZFEdX70duCaApYj0+3oPFgF+B5C2qgco/AvwppoqrSpGLeTHHOzU9FlVAoaePl" +
                "K5hHQqRbk4UwUknSLu6ibe2FUailZdrIXWFmhrqd9nJQJJ/CKgnWC7bPkm0FNJzAV+PNSS/kjRZVrdKf" +
                "AcHBelp4CngSq9mTZacgO8+s9Xcf+sfdm01/Oh7xnIDTvQslyGSY4E2b9C+78Zq/eRJN8gicEGfOaQ8x" +
                "iJWlgw0sXrNtxLLAHDUTNd6WlEmhjgR6jeYF2uon8VaCsiws/E8ajOjWzy3dEo86UVMxc8/va//g8t2R" +
                "/xuRe8nXUz9uTugz1Zf7Cr3ApigYuIk2nAWVVXGOH1WP06qmudVhtxxTFv5VXP3MNo1Iz1/Zeofk0dJe" +
                "CE3QCw5qvqhcBHHU2opmmUFfiEeBpApUHkfYEAS9UFS1+t1Q/qaKI+DtoOAhWgzBSZGH5TBgp+E7qsGL" +
                "kamiXACoVfWbfjeEktm86gBbCS7QIrVUhHpPKGBcuW0TQ4ignDMlaVEFjL1plt9MzppEbI27Zeh2zOmY" +
                "6pSaX4/nKltCx11WtSZH59KlJ4sNyjLGO51VvUHcH18Pg2kzIcNrNP10pOXf1XfrPHETy0YSk37Hk8A4" +
                "uOgp6nIY6fwJjzUa4F2aPCKPk8IoME4Y+dM3GUrx18Pox2syk9g6FMB0f0Luc1G++nPwzZmu5gIGwaFL" +
                "XvV/iTwjcEFpVkhhBRe6EoJwdq/l9vc8dNW5qFd9/zPfJByPe2rmbN9D2469g3OdNwsNuBq4iz8VVyaP" +
                "w2jD0A5OBqpgzoiS7NBqAJKWv5wiFv5PzHbiCRgLQITVG0Ma/6JmPtx63q6d5Uol5le3JGT13yXCBE1T" +
                "CRYlBOVonwtUikEnG0RKcXgCcT9P3G6m+pzKhv2oWm8SJUqyHEgKJfRCQXBcHYeQI16qhW7eWJcpZUOQ" +
                "fAWMUaH26TmNCZWTUntpkwoVMR0XCWeWu6SSVKrqO1rPKjIgSq2HRq+8AjFLdhtvvKisioPuad1QuK1c" +
                "uS1K+XKNyicJ/x/BxVQz5qYumMFg7qfoKXrV3Kfvs8yLpZ+3LVYWdipy+B7qfuI47fgOoPgIPKzKdmkG" +
                "/jeI0/xCqMbAIJ+cpB50OQYV7vcp5omc/Glnm8Zt3tnLB1JStbZzEaRr8S1S1WuVZgDx2br2N12Be4Dr" +
                "goUP3hpvbZBGp5z90/ZFPrNA7bsor75x3M3w49A3rWuNUv78N2jO3D2stQvb4qI99yMsK1boBZFMtQEj" +
                "OYxCQEpIKAWJVEdV0o8vaUyHOt6tF+06DdoNOAWZ6Ok4hz2C7wjttR6mfBTwasFvp7bqJMdoISeUKVES" +
                "PO5msKIxzBRMs+eVSN4zapbrTKGuCQ3WCQ17KH+gUeyqtiVEkFbpExZSZ8IMKITRw3xDJqHFVkYWUz0G" +
                "KsdTvmcbyVOBlCgrYanThe3iggGskyb3MfKWOJ09U3EwUwgThflZnw8Uy/QCQVfJohIt0YM+zoxLtRWo" +
                "ixOS0PRQrrVPV3wFt0W+dkQaYLfAF4ucBgcZhkZA1D6VYezkzjuese4PQn/8zcnqfoap/HN444B9Kd9z" +
                "Ow6dUol6F6XhlHXBNwFcqewJdB+gNriEa3EFjDpsx0vrbvORCmWdE0h70Xbuata25men6Qvqj5PtC3qA" +
                "s4nlYmrVQauAq3e3irlYCnO+eQMjHvuO8aXtHSzmcF7jz4FFi/jjEintPPfkli7sHRGyrZGftgbSeqW5" +
                "GQllw/lzx0DUPp1rHMmIM+PW4kQhIE96vq/UVmSgDMSQcSWDCBc1TPAtICWXVgtciDViEZ2gIm7g5ahU" +
                "7UHo3wsjpGWrpI46mliK9FPbkRJVZLZypDWsIxwPLgxEASE6uOBxvXjqbZVVlLayXq0wCJUYjVkrOWaa" +
                "kUTUGILUmTNBDnyXnntvdDV62DEYtRK+QTJZ90ESdbaoBVhOPzQSikRmLmbh0ilVjiVEhYIzGlCYRpI3" +
                "nyqYhcJkKMReEIgc+pW6BMeRuQAOUTwC9cEr7dS6US1Ts0Ch+JVDVRuMHnOioy9Lax/5+PyzT5DsqQ8i" +
                "Jr6G+aRm9zwAvW/oXIxCzofYZ1bfP51pFvfoKRgQsweh1qLhR4lW47gf4Ll8/6CuBXQKIS0GxyRLHLCP" +
                "rItL15ZOYRrGydz+ce+QbNJsdokL7DKh9zQLdtyf2u4pdwjuWNokoSRDzROZd9+zZz6LI/cef8k2BwyF" +
                "NHbTEMXI/q86lMHt0X1XmgWwu7v93N02ky+YJiPT8U6QASC+Ssxe/PFLhieYGNeYV04AwMq/qM+lXcr+" +
                "5/qXPXa7q1ernCO+sAiXq9BbZ01m/MjjjukBQ/X0gFIUVZ0GQSIXzP+lxI0FSx37grN4qxus26mgrCid" +
                "XW6rwtYxVjrZJYgsQGUWykDjNwXiYxNMWGKDGkjCWJgjqidMCKkIoNc3qH6WlvIpsKCa0epPCSOjrjcO" +
                "AXuxVQCYglZwL5lGL6C9SFu1T1ToWTKjvaFeAcx9zl/bjE89uqymrpbXLnoT5/w4MID4JN+NZRF8SMDt" +
                "1MfuRuQY4OrX2lwLEKx447euU4kJ8g+hDCDSh3A/c7mzygLRlB4mEeb9+HDxx6MR99/AdMS4bJE37Xom" +
                "egvLQc0AocoOh7repHKMTNqeXp9g5OWv9/LL//x/xhz1MI4/7SHy4XW5n0502AxcBjVlyO6IL/TJ1v6i" +
                "pvCtkKyrVR+Fps7FdHkwRFaQ4jAhH68jnaUmkyQVA+tKnYNDEJidXelIQ3KnouLgK/mt9Z67QdW4ufk0" +
                "0ShpPY7UYVzWQBWqIUmSAqwFUo8HdzeqxFpVDHxCQMxTFGFSmpY1NoaQ5Txb6lfFWwUr/25QwSm6A5MY" +
                "GtBVbC4enEzGjLxj1WIJlkMjwTBgRWmTGYJZeOyEXhbBOQiNYkxM7z43y3OS9MlH4C/n00jG7DKJFxtl" +
                "OfwqcoA1aUOPvVaT+HAz8CvoU7NaP89klTB5E1vOyZezm2exn3zDyUHxx6Xp9m+24XuCtSOlH2R+3egW" +
                "VvXFD1vghHgOyr6FsE1qg7nfkeYKmKrG+Lh3R52x4MBilajMEEMqzwac8lKssdU3iHOhrB2HZ5NkixZK" +
                "ibJYPrwEY0bXsI62ZVtlKZ4R54Ew0jkEoMRi1WrYu2d2BWi/B2mLGWBAsKI+qOLjOqDCcxo/VMNp8Ujo" +
                "A26k8vDj6vVTXntCAYtcRqsdZ6wvG2j8hZQ6KWACGUYGEozNf6yjDVUmu2pwOYY1S7RtWOgVS5BHuxKt" +
                "bELt5JwrZAKiZ5HPNZWbUQG5piI81xIrb2buABolxpguBSRUe3C3wDIbCQSswMRc8SrSuLc8RkDcCp08" +
                "KWC9xgkV/kMuGDhdWieGL/QZWvAJeWpzJM+HQxLj3u2QK/VvgDjlnex/ixUh1AKglCq8jmBUOb15/d5z" +
                "JuXH3wOcT5kTgw8ZZAzRbgHnGDNwPajNIOmhLnK5utjoT4DG6bXBVoNVlEbbGasNS6NC4nVWjEDtzJPS" +
                "vGtUAlG0IiacjnSW8LVj2489bm1tq9MaKkY4N6R611nw/U0TEnK5waIH9D6FdQfO4lVa3rHCRxSkGrUX" +
                "2N1M5pFRb5vIZrfPdoxZ5krL3T1lDFCr4rl5xRTzbK9Cr3jZl4CMRUSi1AnmHgDKP20YIGW18d7clW5b" +
                "jqmpWiqiJqNTRmuClv+o3o4jqs+gvURXH8Bpd6O/Jza32FbjBM9G9aG7BvYPXMprw5us52embS8CPchr" +
                "KW7Y8OKNRjk59n62woKxMJnhLYikwc/qVHcX1W0aOAF9ZR6gDYG+Xd6vxYee/IHvW7THMLq6egK4ZSLZ" +
                "8Mg/Qvz151M4mE/GavlxObPNkgg7isMsb7wkao45y7XJh2avZ4BoheHLHvpEqgr/BCq3pVsdM1DwTGEs" +
                "aWTLKNBjwNqk66In8BpBMXuBqoxUes1hOLtg/wS3Ha4//5+qeoz04rBGfPRjnRYPerQ7MaLQKpWoHW8w" +
                "SuQuSDonqHuubSIp9bYRxEOJZOM+jZRu3FNe6b9YNzilUqQYTuRDUnlUNjUri4wTXiOGIjTPQrFtcxEO" +
                "cDPcuqfkzRqPrzXTjZ/N4tpG1+QFQ3R2IOrdNbcyxu1zgpMnBM5R8okdGwqP8j0MnYkD2ieIO4LhMNE8" +
                "lnbBjcuRP8k7boqqi0RclEf8gGhQ+gXCfo4jpCcQqfZzyitxd/WFSDo0T5ZiJhb09Tx52vXHsH71hxAx" +
                "8++lLuWvQiWkY2MZntUhVh8fAmQpNgVIqDvZf6YkqFZBJHej9M/0Q/m9OKUnFpamKdpyK1qANhAaxSxp" +
                "JYS+AOFxim/sNFW4Dj/TXVshHo8q29SpmYYLqM7I/Iz0AfwOWmGvUrYSERXxMwX9E2hUMt7FdH9H1WfJ" +
                "D3VPtzBfrV9Xc1XtdC4McIS72FMODrWPBJNfs6NgN7JXBMPRkGAmtILKiJSCUjgyjrAtXJ1DmgvvClHX" +
                "NiA4qsHecraT0/I50jn2uK4lqUip0l5VaG+60LJfgfb+7VhVjluqBET5kLvBPkThUhG0YcMLCau8zzyI" +
                "bNtNrRugBaVBlp6uTSB77AnOxWhqMJIQKbFM1SmT80U50P6eFijShIDJlsTLqEFKciC2xQ1bdiCif9WA" +
                "kIkwTr/RSK9AOP4VIk707yGJAXEVCWKzpYwyEPSgZ4nr92hv9ik0jwV2fxTvkoX+2vOTXngnIM1eIIJ1" +
                "nURCGdDLPXwBNsTc3JB9i/iNYM5dq14ur0jI1kmQ0CQmNrEmOLwbRlMMdoW5pcU4rATm1fBhUqcK8qr7" +
                "Kq91hnd49fqKc/ll4TdsLUus0QSq59FJoV2Nw0nTev+hUXPv4jbNjEaNhUVz6rOExxcPcjpEyOXBBisc" +
                "XlMBbiMZ3Sm4hFl6hqW6EuBiFtoT0/RDpvycTJhCs05iVRYsIocdvIpVcqNmsDY58OjKVwlcSB/p6dc/" +
                "z2zpRbnE2hKHofldNAT2kZVG2yi85S7MadWDS1070cWAVpZue7eOWGaxkNW8Blmn2G3U9uDYyuiPJmMk" +
                "DlKh8IzcN50tkYDWTXg5V7pD6k8EoLVxvImyIHiS25tGBQu2u9Ue636gLOrbXF10xr7f7WWsQkrG2ZxT" +
                "mrbuWCZT8iiZrIBZmqgBWoJdu2gLc88XMWDq1lWCIStWOXURtYa1OF5+m2ZfVZp9z7tMnyZNM8lrUcSJ" +
                "MZHTsdzF/7RIk9NRUbKl1RYldGxm4MjFK4SuROxvhfu4V8A7cZQpHP6pNsz0GvO6bpfL26El5LSa/LB1" +
                "Is3wbu28n1ML4tu6tMZasCA6kORsJ2QmvXAB9kN6IHIKxH/BiVSeuQWnDLtHjAslMIWLUccL0o56vqm6" +
                "3qbcVaVgkIoV7Hsdj3WOxDFpt27yeARbuF2WOeNGtY0zqTNz71Gy5Y/mPyqeaKJ9sAZFMtPGft7bTFww" +
                "yHrQToeN5e95U5xSagqo6bZa7cgVV3CnWiMCM/zMrmfbiz8yQ68v2I1eLrg2J0jhioeFkeCmM7kMobUn" +
                "kHYCXdbICPA1/dDYblT/xEKZWbcBskG3dBGVYAF5XRLmrtYHaUrKW1DjcojeLfgMuksTMB62PAR6gest" +
                "QWS4rZuS6O7b2bbNgMcA1wNu4osWdbbgdew7acyWbqO0AiU+xHbhnOk8klUwZY9WZO/DkuNvDVqno28G" +
                "It3V1xntrfAb8A3l5BU+4AFk+Y0GpY3zyds1bfSqLCDw853wUWl1NHW+dw+oNfZkn/Bja2TifY9juHle" +
                "YaG88RZgHiQGSDFaEz6WNDpoO/TjuW+bmNGJngmjoVeEsNF1oM+sex6SMQiGVa0s9oOGH8DgGX+oFxmn" +
                "eiHzTljlMnA7gUzD/H5T+vpEFdjTu95R04Ht28KdCm/tdrOMvL/D2c5DhNbce4XgmcCbzXT9D9trMua3" +
                "F8vU/7vqwaPhNLmvnZbp7T+yfumfES9hxdhZXwWtzO75uAFwCH4pz8u4L1v8E/+zYcV3JgO5SYiUbY2B" +
                "wTmodyLjSjKUVgdq4CKdd3zt4GVtRCQStSR/wZ9zuptuFY58co+jxgCS4/1WyQV6pyM8JjuHxFvkYTQO" +
                "Xbqlw0scaCoLTHQ9wy93jiMDVmOxfHKCZhmkO2Psb87FbibY9sbxPhRlFerCX3dnaBArJCRY5sSYazub" +
                "CZby9+F081LWFOvqsYrPbH8Vv2qtF293hQGyl0VIByRP/9vHDLb6lC/luCc/LP8Ndi3A6l4ngy7ZPwc4" +
                "mf6H24PNqFMxjWA6twu2DLmdx5cEf7chzkJ9BCxpMvBl57LU181OfNoSb/+Ub/zDW4Xb91OL5QJTkOl4" +
                "poqMREKgT2PgncUfT5ybhc/r1lJlnon1VNizoQl4X2cN8fc3y5W/3rwr7YFsbpNKtwIVurGd+gWQC8mJ" +
                "LklEXa3wrg7kgTskErt805naUdz6HFDJdqgXu7+cMMXGzoXoynsV6AixqZjO8zxOXiKvCyuvzrbl+n5Z" +
                "Scq1AiBzN+IEc5ukTKP+MmHF1pvOIOMBhty5DPRJP2gdlQSPzcsSLkMiGjqcjN3u0Aq6LvaiswQx3PZz" +
                "Zwh6KdKPfgsh6Uk6cVTlX0iUKvJtY9QVDassMVl5cA6IsiSDcTbtsGpwC3VtJY1Pm8bggxb8oFmfgHiy" +
                "7imaY9mZ70YscXkhMYD5GpJW8Hvj9B1ZIMGR3lklWXY7cryRoZP+HNJMAqwNEJ8lO0Erf5gVlIs9ru27" +
                "i4B0Y90BQ0nkF2u3DYqm1YWCwyvr6FNXLIt2t+R9pXEWbmh1naeQg/2PNSZuS7J/PztDfLJpP9KfBadO" +
                "5ZaVCjJOmQoY6mSe8QVgOrHU2gP+yvZ8Dtvila64SlJcBZqvrZwgeJFvYToS/TUkdjmLE85+Lo222CjB" +
                "3mWI5GIUDaZm8bjtrinyy8gE2ZhXQmfQWgynjz4NI6TaA7gJ9u+wxLazK4I+35rA2wKjJU4z0loPX3Jl" +
                "pUp2Gm4Ah1QYkDGAnbCHTSptEOAeWzBf9TsTMY7cTyFXp+s1c1t8kQWqSVXYTbvn7Ujq09MqnRVRyLq3" +
                "Cmoq+a+KwSvdXmzU1zX3vXyraDGQqn0WYGWy1BQY2/EDiC+uLVutj1O2gNacg/veyUyHgXM+XoA6pqVX" +
                "WZqqX0GjMzYW+FzyjM2Qm2wgtU+awqwUQ+2Dj7yqJE1vyiOz27c3Nm/qczNvtdS3Az7qy+L3s/Tb2BtV" +
                "8t8Z+MI78m9KZmceOCc8nYbGN0NaQhu5tmpWNG3Nj7Pymc55nPJd8ce306jvfzTmoHm1aSF6nq9xWtGP" +
                "1uEKbHw/pwx5E/39i0+Nz2ZOCCHQDpq4Arq7WEkZDBaBryd+OyaUhD/gk0K5cszpI1CUbtGLtd4deq+r" +
                "Sy7T8mvOK1uC31Ayf56DTwOpyDe+/K0CE0WYMJghsf7jg2P5jqPCOl8fbW+X9webxMLf9EqKYxshrSkN" +
                "0JrBTGclWXhNX0KnxRKRvyMjF8R/U0XBjC+3A7iNVOImnBHXjwWRy5ripQqcCcONv/VOt+X1o27fA3ti" +
                "RDC2XyIR7WA+rFlBxL3pCGNOTvwAwUgdhYskniTz7ZBgS+j+MhvbaO2+0NfA53tNHdwJ9xAbcDHizacA" +
                "7w43GBwZ21EUbojIfYkGl//52zTt0rUnNWpPFks1l0+XJ9vjFUGtKQv1OwKjYFK0gCvAdHMDuoztvt6a" +
                "9zPEgNebNrGnU6wBXBiDA9zjIctn/9R3te+NdVrYffNDu3sTXA1gtWimP4fgoX39eQhjTk79kMrEPWAW" +
                "/F5QbanrIVkt/VDVQKdMZDzDfJL/4644SvLWs/6oud8dZFIUk9QKW4kKGzgZc3gKohDfkH0qzqkPv95P" +
                "8pVM9ZvSPimFqWlMkzkur4441zjvvdvTNedN3M3JbD0jaLrYx3hfCDPwP3eo1qqDE0GtKQfz6wAsdLeg" +
                "2OqnDUVD0kY/OkrfnBDbNP+8Btc886bUZu8zOtycBtiBQnMhRvWvYyHrO2ml2QZrchDWnI7g9W4AJLz8" +
                "Sl1XjHFNy/N0A/CXxjS3pOLq9cNy3u/ZkJouTv5wi7hjSkIZVkV5/tthaXnP+1wJ920j0HcDuPL1d3mG" +
                "kuY3OkNMlaCZJGFzekIQ3NanslD9zgweoFwBuB09k2YVotWYpLV3szLmfTaKM7G9KQBlhNhXQDv8Ttvu" +
                "2HO3nmRBzNYT7juZEKNtxmnG/pIVzysKX+HnGjGxvSkH98+f8DACeR8Z8W+T8oAAAAAElFTkSuQmCC\" " +
                "/>\r\n  <h2>Bandwidth Report</h2>\r\n  <table border=\"1\">\r\n    <tr bgcolor=\"#66ccff\"" +
                ">\r\n\t\t<th>PackageID</th>\r\n        <th>QuotaValue</th>\r\n        <th>Diskspace</th>" +
                "\r\n        <th>UsagePercentage</th>\r\n        <th>PackageName</th>\r\n        <th>Pa" +
                "ckagesNumber</th>\r\n        <th>StatusID</th>\r\n        <th>UserID</th>\r\n      <th" +
                ">Username</th>\r\n        <th>FirstName</th>\r\n        <th>LastName</th>\r\n        <" +
                "th>FullName</th>\r\n        <th>RoleID</th>\r\n        <th>Email</th>\r\n        <th>U" +
                "serComments</th> \r\n    </tr>\r\n    <xsl:for-each select=\"//Table1\">\r\n    <tr>\r\n\t<" +
                "td><xsl:value-of select=\"PackageID\"/></td>\r\n        <td><xsl:value-of select=\"Qu" +
                "otaValue\"/></td>\r\n        <td><xsl:value-of select=\"Diskspace\"/></td>\r\n        <" +
                "td><xsl:value-of select=\"UsagePercentage\"/>%</td>\r\n        <td><xsl:value-of sel" +
                "ect=\"PackageName\"/></td>\r\n        <td><xsl:value-of select=\"PackagesNumber\"/></t" +
                "d>\r\n        <td><xsl:value-of select=\"StatusID\"/></td>\r\n        <td><xsl:value-o" +
                "f select=\"UserID\"/></td>\r\n      <td><xsl:value-of select=\"Username\"/></td>\r\n    " +
                "    <td><xsl:value-of select=\"FirstName\"/></td>\r\n        <td><xsl:value-of selec" +
                "t=\"LastName\"/></td>\r\n        <td><xsl:value-of select=\"FullName\"/></td>\r\n       " +
                " <td><xsl:value-of select=\"RoleID\"/></td>\r\n        <td><xsl:value-of select=\"Ema" +
                "il\"/></td>\r\n        <td><xsl:value-of select=\"UserComments\"/></td>\r\n    </tr>\r\n " +
                "   </xsl:for-each>\r\n  </table>\r\n  </body>\r\n  </html>\r\n</xsl:template>\r\n</xsl:sty" +
                "lesheet>" },
            new UserSetting() { UserId = 1, SettingsName = "BandwidthXLST", PropertyName = "TransformContentType", PropertyValue = "test/html" },
            new UserSetting() { UserId = 1, SettingsName = "BandwidthXLST", PropertyName = "TransformSuffix", PropertyValue = ".htm" },
            new UserSetting() { UserId = 1, SettingsName = "DiskspaceXLST", PropertyName = "Transform", PropertyValue = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<xsl:stylesheet version=\"1.0\" xmlns:xsl=" +
                "\"http://www.w3.org/1999/XSL/Transform\">\r\n<xsl:template match=\"/\">\r\n  <html>\r\n  <" +
                "body>\r\n  <img alt=\"Embedded Image\" width=\"299\" height=\"60\" src=\"data:image/png;b" +
                "ase64,iVBORw0KGgoAAAANSUhEUgAAASsAAAA8CAYAAAA+AJwjAAAACXBIWXMAAAsTAAALEwEAmpwYAA" +
                "A4G2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTX" +
                "BDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeD" +
                "p4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMTQgNzkuMTUxNDgxLCAyMDEzLzAzLzEzLTEyOjA5Oj" +
                "E1ICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMD" +
                "IvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogIC" +
                "AgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgIC" +
                "AgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG" +
                "1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgIC" +
                "AgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgIC" +
                "AgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW" +
                "50IyIKICAgICAgICAgICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCi" +
                "AgICAgICAgICAgIHhtbG5zOmV4aWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vZXhpZi8xLjAvIj4KICAgIC" +
                "AgICAgPHhtcDpDcmVhdG9yVG9vbD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC94bXA6Q3JlYX" +
                "RvclRvb2w+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC" +
                "94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMTYtMDMtMDFUMTQ6NTE6NT" +
                "grMDE6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE2LTAzLT" +
                "AxVDE0OjUxOjU4KzAxOjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8ZGM6Zm9ybWF0PmltYW" +
                "dlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3" +
                "A6Q29sb3JNb2RlPgogICAgICAgICA8eG1wTU06SW5zdGFuY2VJRD54bXAuaWlkOjZhNTdmMWYyLTgyZj" +
                "YtMjk0MS1hYjFmLTNkOWQ0YjdmMTY2YjwveG1wTU06SW5zdGFuY2VJRD4KICAgICAgICAgPHhtcE1NOk" +
                "RvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE" +
                "1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2YT" +
                "U3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD" +
                "4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgIC" +
                "AgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RX" +
                "Z0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbn" +
                "N0YW5jZUlEPnhtcC5paWQ6NmE1N2YxZjItODJmNi0yOTQxLWFiMWYtM2Q5ZDRiN2YxNjZiPC9zdEV2dD" +
                "ppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE2LTAzLTAxVDE0OjUwOj" +
                "QzKzAxOjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD" +
                "5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgIC" +
                "AgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3" +
                "Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgIC" +
                "AgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgIC" +
                "AgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgIC" +
                "A8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZj" +
                "pDb2xvclNwYWNlPjY1NTM1PC9leGlmOkNvbG9yU3BhY2U+CiAgICAgICAgIDxleGlmOlBpeGVsWERpbW" +
                "Vuc2lvbj4yOTk8L2V4aWY6UGl4ZWxYRGltZW5zaW9uPgogICAgICAgICA8ZXhpZjpQaXhlbFlEaW1lbn" +
                "Npb24+NjA8L2V4aWY6UGl4ZWxZRGltZW5zaW9uPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC" +
                "9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCi" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIA" +
                "ogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCi" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIA" +
                "ogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgIC" +
                "AgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/Pq+oh5kAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAH" +
                "UwAADqYAAAOpgAABdvkl/FRgAALU1JREFUeNrsnXmcnFWV97/neZ6q6jXd2XcI+76DoqACKgq4gPsAgg" +
                "vIoDjovOq4oDMqCu7buOAuKg4iyCCCG4uyCI4CYQlJCIGErJ1O70tVPc+95/3j3uqurtTWSTpErZPP80" +
                "lVddXz3PV3zzn3d86Vh+YtoSABIP7/QGTsNf5z/OfF74VxUbYVG4TM6e1i9dw9Oeb7D2A0gO5eCIPir4" +
                "VAB3AAsAQ4ApgDzAfaAQP0ARuAx/21GlgH5KglxoKWKV0UQv8gbNoMYehqYP1lDFjrXieJu0eSQBxD03" +
                "Quu+cqPv7IDaxpnVXz8WkgLG1PEaZK1NdVAev/N4Ap1wa+D7cYg0XH+5kdL1+iyoJUilYRDLuXKLA+jj" +
                "ElYxggVmVuFNEZhiQV2myyMqu/i2+e+CbeeeHXYLDLjbcoKuoEgVCmvuKJnwvqx3U2B81N0NrsxvtUi9" +
                "XyQAHjz1c//2B8zgHRszxmWoAjgVOBFwInAqk6f7sK+A1wC3Av0E9DGtKQf1h5NsHqROBtwBs9aE1W9g" +
                "UuAf4VuBn4NvA7r0Q0pCEN+QeTYOofIagEpJM4UBEQmQ9cAdwBvHU7gaoUcM8Efg18Gdij0a0NaUhDsy" +
                "qVVmAezr8027tIImBPoElFrBXpmDbUG62fPueKKI73ygfpryM8f0pQ0WlazwfeA9zV6N6GNOSfG6yagR" +
                "OA4/z/ewILLEz3JlgIiKhigpDFXWsYaGm76rK3Xf4SNXyFwcGFBFOq0B0N/C9wDnBro4sb0pB/PrBKAa" +
                "9T1XOAk9RpVaWO/bH7mSBk4ZZ1jASpK8993w9vufWs11zNmuGFxPmJuyBTI9OBa4CzG4DVkIb8Y0i9Ks" +
                "4rcDtvP7VwhoVW622+4mt8q1wJk5iujtnXveW937nh1jNe823WDC0hl4VUalfVrRP4qTcLG9KQhvyDa1" +
                "bNwGeA86zjQW0j5SgT2SjNgeufXPG+8z/xlV+d9cZP0TVyIHEOUmF5vtOEG+oAIvcDDwNP+U/TwN6oHg" +
                "gcj0jbJDSsn4CeDKyhmNukCkHAFJukDWlIQ3YBWO2j8B0LJxd/WI42JkUkxCQImdW3Jf/AXod87OYTX3" +
                "0GvfFLGRp2oBBXZRX0IfwAkRsQWQ5sLcHCEJiO6gFY+1KUC4CFNWso7AX6IRJzKarjBNIggP5R6Ol3Zu" +
                "lOIv81pCEN2bVg9RxVvVodo7ymJlUQE4TMHOwhH4a/uOiSq3qfWnDg29iypfDXauX4I4F8hDD8CyJxpd" +
                "sD3e7SezH2Oqz9KI6nVQ2sAN6O1Ru9KTuuWcUx5PIQBQ2wasjUiiqB2kY77EywEjhU4XvlgKoWWIlamn" +
                "LD9qcnvO6Gx/Y49HwdGJhLHEP10JJrCIN/h2jzZLoeeAz0AhLzFPABqvvfIuDVWHsHkEPE0fj7B+szTX" +
                "euhL48nd5M7QQSoAmY4etRb4HEK7s9QN4D+hZ/5dkNCLJ1VqSD8d1kW1K/EBjymnZBZuPCsLJMjJYpfH" +
                "8A6K3VaPWUWyrXQXx/tQPlFtiUL0ePqBJHKUbSLZXGWlg0LjqANmBmUf/NwG1oTaY/A//8QV/dLX6xz/" +
                "nxVq2Tip+vO2kYpHFRJsMT1IiJRputVsdSsJqr8CUcYJXtqDIlj4EhK5JvHx1JNnfM/d53XvaOocSYkx" +
                "nqr7Xa/JIouhQJurerCUSGgI+TJAL8R41vvwTHCVszBlbW7kqfVRPwXOA04CBVnQ/M8oOiwE/b3t2HvL" +
                "9HgoufXAU8ATwI/L5kou+6lVCETUnMwjAiI1ININ4FXOgHsymZcM3Abf47BfmQ16g3lgz6woS/AfgYVV" +
                "T0LmPKxgUCpETYmrj53FG53K3AfwJn4OJWS4EsA/xaRT4wZ7CbWw48kQ+/4WPQt7GwGx4Ac3FulqOAuR" +
                "id48dDh/+bLbqXbCdIFBat9YzH1v4Z+C3QRxjA6CgEAplMIT7vdcB7/T2SnQRWTX5R3eDrXhp23OPBdC" +
                "1wJy7ud7AiWCl8SN2kHnuCVl53RoDlwM+shH8SbO8+fZtzZ/zH9WsfPfRFV7H+yQWg1Zr4UZAPYG03xt" +
                "TSviqMOgPWZrH2Sqw9FOGMaj44rC4G1mCsC+DcdUA1A7hC4UIDUxGtmil6PRM4rOj9Qzhm/4/rUCimRL" +
                "PKqhJVB6sDcQHslaS7BAiWAAv8VU6WVypLBGwyCXl1yFZRLRGh2xhsENAeBOXKnvHl3rvKbVYH1shApk" +
                "0fmb8/2XDMN9oJ+k7i+K24sLGpXiTxmuuhuDjcS4F7gI/7xQziZNw94riThz+LCvlW4C/Af+NC6BKAqE" +
                "jdfami7y50qhT/vy1iLQM+C1wjQmyCkD16NrFq+jy6Oue2MTj4Imf+VQSDmICPI6wiZyCbhc5Ot+LUE/" +
                "kdhG6k9Y+CWhD6UK7A6otBmipq/8ICVJ2fSmSXmH8C7QrfS+DMZ8MrJi5Q/LtAi8A3y03ggKlB0IKq02" +
                "0NFshUXhyGatymv6TItVb7gUp20ajqmLpST9l7rMUCLUFQungn3gytAtQyNHeoj3v3PIwP/svl0Lse0t" +
                "EMrLkaa87g2XWTnoBwDcjrCcI7SYybS05GvcWUepbKNhM4DeE04JOI/BdgI+sKOE2Qj+CUwQmdUka7ul" +
                "HgvQJPmyBkWn6YhV1reHrmQl777qtZvvDgU9j09DyCsNpCfguB3Oye5tFwNOuAasH8ykueAH2j0NfjUr" +
                "YYO/4M5W8Yez2OuV7pBrM9GBZ8DZuoJ8VMGbDsyA0SxKMYCQirOE4VLk7gTH12HfiRwOXAbQorS5t0VH" +
                "VKnVsB0GsNsyprz1Ibc3cYtMmr0med+RfVecsQ6LcWAzRX1w63/a1NZGNrJ78/8AQYHQAJMlj7FRJ7xs" +
                "6p1Q6rvbMQvolyAlZ7HIo/24UqWZaEjwKPY+zPIq89vdjCi4rbT4sgqmia/UKQN4tfUdpzwzw+Z29uO+" +
                "hkvvT8N7Fy0eGwcfUhhFGHA5KyYgmC69EgS1J05/wgkclz8oO/JVKLlmm09uF+rnvZW8GmoGsdpNPFf8" +
                "4C12NMNbDqw5hWovBHwGKvIa4Alno/z6N1NWJumD8uOoYXb3qM2dkBRsJ0pQkyLxA5dzfZZ5yh8Bbgw6" +
                "VAstUmqAqpKR6no9bSNMkJXwF36uEHbuPtHrSWRCefNioAhqwbk5lJlL85n+Vviw/hitddBlvWggSnku" +
                "i57EZ4ABxIkpxJYr9PKirNM7e7ANZl5JM/ROr68awCJJkKo0PhfnFO0Cw4msI+PZu5/pBT+PD7roEnV8" +
                "CGJyGKFpJUXafXEOp9YwnAxpahiCTTzpVfuYhpSb5sOaYDx6y8jw/+yxeheRrkhiauBMpjWLMRZX6FZz" +
                "8FHEliXuHfH1H0t6e9/f5b4Eaq7byM9HLzIa/ikK4VfHjp9fS3zao0yA+J4LDdiBRxlFcWTLFtHLJrku" +
                "P1W4sJAppEdtQCqpWpo7XU/h9SS6K63WlGAmDYm5CpOrWPJAiIkjxsXgdJHpCTnJ90N6PJqBxDGHzf+Y" +
                "B3ywxLB6McGVllD3FOtwlaVEkG0GGE9wn0CaASsGCwm0fm7sGvjz0LnlwO2SHnS7I6o2pnKA+QJKvGHG" +
                "KFJ0ybx7m//296O2fRp1pWs3o6jHjN7deS9A5w2blfAo0gN1J8oz5EViJlwSqPtQlB8G9liycswYHxhb" +
                "j8WN/zoFXeZ5YfYr/+9TSbfFlT0AP8vnFdg0XB5VDsBYakvjCogoXeAXTWqb7P8/6ArkIZR9SQqBJO8X" +
                "JfuPugNaTCaLu0K3GXAt8BHmGi472AKSng/ybYwMCoWvKqpAPZoTqMqiWQsC64SYDEWkhiiJNmJFiCbA" +
                "cYTMqFIEULeN2/O9y7RnrYbUWOjBQ9BEdZmDCoSgbTNSh3F77Tkh/m4VlL+PfXfoJlS46G7jUQRsVmfj" +
                "VZgYhOaMsow1tv/QLvuf0qels7SYLyWn5kElbPWcSZj/4G87OI/3z9p12JTVwoeS+wscLEXY3qxVj7+j" +
                "pa5hXAS4GfAJ8E1mxr0/TyzSNex4KRHg7sW89gqqnc2N6/roEmcrW4Z/UDo0H93gz1cZovFWs/gOPH1J" +
                "pvqfGOEkbVYph6sCogiQEGjZm0dlXgRnm6wfW4q87fqQPkHfTHFMowai2pOqAg8b4ugmbAdpLk5tbdzF" +
                "YB/oRINyLGA0muzPwqLFrTgA5UFdV24EBEFtX5vCavrfZMEkQHUUaRSWdvSYBOlDQidY52OSBSOF5Lal" +
                "6CycZP2rG/hEmWZTMXs2yPY2DjKqdpmHhc564+MfMTTTcLzTM445HfkU01EQcpqg3j0FrWdy7gVY/8Fl" +
                "Hl8pf/P/JhCtSA28FYWeGn04E31wGmBckAbwc5Fpcn6+6Jetowf9vreax96DqO27yCnlQzwcRyZ3DkxV" +
                "pyWwgXCWSlaFJPzqTnPuNyil1S4+tRAdWdlmAwqmN59UtkD6+JzfL/p/wgU8b5YeIXiE2Mc5wMjitT+H" +
                "xtaXnzqqS3w3fliGRaP5L7toxVsW5mlPbPQcAiHKdJfNnn+PoO+LILjqu03Ps0R3ybCSBadUYqBoEgA2" +
                "SnkcvNdxtPNUv+CEHwPpClBAz5xX2kjr51K6a1GZS5oG9DeTeOkFlN0mzPzp/wQUTuRrV5kr+MvTVwJK" +
                "rvQMsT0EvaZI9IXYdVhAdFHxBk+QSzUAKa4xwM9ri+LKYbqCY10DiY8P1MK5fedDkzR/sYzrRQz3orqv" +
                "S2dnLi6r+QJ3SDIT9cKPCyCrWZu50L6hGIXANcjMtG6gsRQu8zfO6485ib7eOA3nUMpzLl5lcteSJvNb" +
                "u9fgz1GlIo3GNcmuiA8ozqdopIl4Uv2Yno/ULgtcAxis7ymlqzX3UL27vKOHenIAUmecEhM+g/G1anlf" +
                "7Om9bLC5rOqFpS2+G78mX9T+B0xtnQxcqb4PLyf8ahj6JM8FUdBbxT0aP9YtJW5AMrrVuuaMHuBTYL/F" +
                "Lh83m1uVDEVg0/A6xaJclDkk+TJE1IzSExCPwXmfTvJqnsJhQoIIEMgWwln/8PjHkOyAvqcA/MYjxxQH" +
                "06fTr9F4Lg4R2gAN2OSR7C6PW4SI5q0hxZdEYNM3mZogMyQUNVbLkCioAxXTW4UhO1DYk4fP1jTMsO0d" +
                "PcSb3DNwlCrIT8+KfvZOWsvfnkKz4CSQ6S5C6S5G+IHLPzzGVZDPLdIn+WH40xT87ej82ZaRxkE4xmKr" +
                "lqqsnJBj1e0fu2t8sTlFDllgg5qWieFLl5EL+R0gd0C5Dz2oZ34eyj8FGrnOEH7WRBpBS8Zha93l8cS/" +
                "s9wA+BTyrkjI6j33b4vp7jr0rSTZGqVMQbvMwq7wLm1Vm/TIlTf6HAkeo2Tf7NKj3VOtj4g5KIE4gTS5" +
                "wkNYnIyuOEwS/J5twJOM1Nk7S1AxjJQT4HiEHpqq7/jYG8TopLoYDJttHc7AhPOrmfjw0wq/eSJH/GRX" +
                "ZUExNZrT4u1anCE8hvRgRrrSNzZnOlKLea8Yyh5eQwb5K52K0kYTjdgglCJuvFCNWwV8869u9aTWgsl7" +
                "/8QyTWbMCYj6J8B5E5Xj0uuDtM0QIdTdLimofIlxDZBPx1DJyHt/ChEy/m27//FHv1byQbpYtV3aE67n" +
                "uAwLXi0jAP+t9t8Gp/xq+Yz+CIeoUwhRDY7M2sCEglqhsT9G+UUOOs/0HGE3SdRqXE4+bUYkV/lqDHTa" +
                "G7KsJlyPiQIDME3qtoLr/9u2K1TKLBUpcz8JkE+152PJV3ALxBHAt9pLoZaEnUQj4P+VhJYq1ClC4sjK" +
                "sIUoq1447yyRKYVSExDuysfhXltirrTxoX2vK4c6Xo5NaOOIa2NgdYVicHVqOjYK2i+nQ9v4gmnuJV9m" +
                "Gbyj5JLeTjsTO9Jqh2biJVCoU4GLVHoHonCAR5muJcsb9nujfZch5cYj9hR/zruFBQRRhOtyBRhtOX/w" +
                "HUcuVpl5GNMrcyMvBC4vxJSLCfdxw+g4slK8Rk7Ys7YefISZiI+yLyaUReOWYiqNLbMh2xFqzFjC8wZh" +
                "Jq9R5MJLOWKATE477lMXNryE/KwJ84N2SdWWR9nz2D45DdZ9DHh60ZjERokgmhI6LKfxiXonpXSAB6vs" +
                "KfE9UfB+riB7cDsrSev6s3OQPkFQLvZuee5nSqHwO20qJnFdSq06ySJCQxYQ0tR4HNY6Ev1rqQsukd3g" +
                "CfREuN+yH/5K/a3zf+eZPdiOjth6Fhigd/7ZEg0NLsDyiVdD0/iSwalKrZJU3SWcaPRT6IIMp4x/aEEj" +
                "6K5eEqYDUD9GWo3ulWgJinpi3k2M3LiNSQSDjD+xVcwj2XU31Pdf6CJ3B8qM1e+1gJ9FgJBntbOnnF8t" +
                "8TBxE/P/481ki4Ot8+dzWj/WCS8h3guF4HA+9EeTWqi+roqJeCvhfkyrHBE+dZNW0+84e2ABY73h5Pbu" +
                "dECMtoJuVMr1l13CsRuFnhi3m1dw3bhIigABL7inA6u1ZagAv87qeaKeYcCYSKvkG3NVV3hmSqbgiId5" +
                "c4M3AOSTKrhmaVTFAOVKGn1wHIrJnufTUXi6o/PDV05M4gcA4AraOVrD/Et16wKt6JGxn15ZqMGSkwPA" +
                "pRFJIO96ynK6M6siosUQcc+cIHeYmYMdrHnl0rWdM8fXwncEwvsF9G9YWIVCLvnY2x12D1EeI+rnjexS" +
                "TGcvbq2+hLtz1pJHjSP3Nf5zPj+QqnAC8u2ug0XnN4FLhHkT90tc1++uTV946c/eBNfPM5b+Te/U/hgc" +
                "5FMG0uDG8Fm5Rr0GUol2DtVah+HtVT6+isd5HYW7D6sLtdlg8e/y7+8L/vojnJkQvTBdBfZpUu3A7Tsy" +
                "URcKb3G50XIDdZlNjtaJ2CyoI679MD9Hue05CfWIWsCGnvgppH7Z0ngL0E9ldYMZVRSL4P9gHq9V/mvE" +
                "8vV6TNF9L3NAOBjqf1qSlGwdgxsGolNk11+I+2rcWWHmfWLZwHUboyYIUhDI24hJLGeNOsDj1UPP0nMU" +
                "V7xXW1bi+5PBNM1slr2q8mNs+tZwGPVFlfrjZFn+ynbsdkjIPRl27m+PVLufie7/HB138buldBbtDtkL" +
                "l6/BZrbgZ5QxWz5/2g52EUhnv53KHnEcQxb157O0kQ0ZXuyAu6zIPVtaiGCudalyrkOK99HOyvNwBbRf" +
                "WWOIi+tWrW4nvPevx3XHL/tVx54vmsWHAYf150FLTOhMEtlXrvEVRPw9jvA+fXaLhFKOeAPop1kdRic9" +
                "w253BO3vAAGFMgta7AkUs/xLMvHcC31GVhWOsmkx4LmqnDP/RTgV+KyJO+8Xr94iVeU2q3bvv6uep2TY" +
                "+scc92328rprrSis4RZFEdX70duCaApYj0+3oPFgF+B5C2qgco/AvwppoqrSpGLeTHHOzU9FlVAoaePl" +
                "K5hHQqRbk4UwUknSLu6ibe2FUailZdrIXWFmhrqd9nJQJJ/CKgnWC7bPkm0FNJzAV+PNSS/kjRZVrdKf" +
                "AcHBelp4CngSq9mTZacgO8+s9Xcf+sfdm01/Oh7xnIDTvQslyGSY4E2b9C+78Zq/eRJN8gicEGfOaQ8x" +
                "iJWlgw0sXrNtxLLAHDUTNd6WlEmhjgR6jeYF2uon8VaCsiws/E8ajOjWzy3dEo86UVMxc8/va//g8t2R" +
                "/xuRe8nXUz9uTugz1Zf7Cr3ApigYuIk2nAWVVXGOH1WP06qmudVhtxxTFv5VXP3MNo1Iz1/Zeofk0dJe" +
                "CE3QCw5qvqhcBHHU2opmmUFfiEeBpApUHkfYEAS9UFS1+t1Q/qaKI+DtoOAhWgzBSZGH5TBgp+E7qsGL" +
                "kamiXACoVfWbfjeEktm86gBbCS7QIrVUhHpPKGBcuW0TQ4ignDMlaVEFjL1plt9MzppEbI27Zeh2zOmY" +
                "6pSaX4/nKltCx11WtSZH59KlJ4sNyjLGO51VvUHcH18Pg2kzIcNrNP10pOXf1XfrPHETy0YSk37Hk8A4" +
                "uOgp6nIY6fwJjzUa4F2aPCKPk8IoME4Y+dM3GUrx18Pox2syk9g6FMB0f0Luc1G++nPwzZmu5gIGwaFL" +
                "XvV/iTwjcEFpVkhhBRe6EoJwdq/l9vc8dNW5qFd9/zPfJByPe2rmbN9D2469g3OdNwsNuBq4iz8VVyaP" +
                "w2jD0A5OBqpgzoiS7NBqAJKWv5wiFv5PzHbiCRgLQITVG0Ma/6JmPtx63q6d5Uol5le3JGT13yXCBE1T" +
                "CRYlBOVonwtUikEnG0RKcXgCcT9P3G6m+pzKhv2oWm8SJUqyHEgKJfRCQXBcHYeQI16qhW7eWJcpZUOQ" +
                "fAWMUaH26TmNCZWTUntpkwoVMR0XCWeWu6SSVKrqO1rPKjIgSq2HRq+8AjFLdhtvvKisioPuad1QuK1c" +
                "uS1K+XKNyicJ/x/BxVQz5qYumMFg7qfoKXrV3Kfvs8yLpZ+3LVYWdipy+B7qfuI47fgOoPgIPKzKdmkG" +
                "/jeI0/xCqMbAIJ+cpB50OQYV7vcp5omc/Glnm8Zt3tnLB1JStbZzEaRr8S1S1WuVZgDx2br2N12Be4Dr" +
                "goUP3hpvbZBGp5z90/ZFPrNA7bsor75x3M3w49A3rWuNUv78N2jO3D2stQvb4qI99yMsK1boBZFMtQEj" +
                "OYxCQEpIKAWJVEdV0o8vaUyHOt6tF+06DdoNOAWZ6Ok4hz2C7wjttR6mfBTwasFvp7bqJMdoISeUKVES" +
                "PO5msKIxzBRMs+eVSN4zapbrTKGuCQ3WCQ17KH+gUeyqtiVEkFbpExZSZ8IMKITRw3xDJqHFVkYWUz0G" +
                "KsdTvmcbyVOBlCgrYanThe3iggGskyb3MfKWOJ09U3EwUwgThflZnw8Uy/QCQVfJohIt0YM+zoxLtRWo" +
                "ixOS0PRQrrVPV3wFt0W+dkQaYLfAF4ucBgcZhkZA1D6VYezkzjuese4PQn/8zcnqfoap/HN444B9Kd9z" +
                "Ow6dUol6F6XhlHXBNwFcqewJdB+gNriEa3EFjDpsx0vrbvORCmWdE0h70Xbuata25men6Qvqj5PtC3qA" +
                "s4nlYmrVQauAq3e3irlYCnO+eQMjHvuO8aXtHSzmcF7jz4FFi/jjEintPPfkli7sHRGyrZGftgbSeqW5" +
                "GQllw/lzx0DUPp1rHMmIM+PW4kQhIE96vq/UVmSgDMSQcSWDCBc1TPAtICWXVgtciDViEZ2gIm7g5ahU" +
                "7UHo3wsjpGWrpI46mliK9FPbkRJVZLZypDWsIxwPLgxEASE6uOBxvXjqbZVVlLayXq0wCJUYjVkrOWaa" +
                "kUTUGILUmTNBDnyXnntvdDV62DEYtRK+QTJZ90ESdbaoBVhOPzQSikRmLmbh0ilVjiVEhYIzGlCYRpI3" +
                "nyqYhcJkKMReEIgc+pW6BMeRuQAOUTwC9cEr7dS6US1Ts0Ch+JVDVRuMHnOioy9Lax/5+PyzT5DsqQ8i" +
                "Jr6G+aRm9zwAvW/oXIxCzofYZ1bfP51pFvfoKRgQsweh1qLhR4lW47gf4Ll8/6CuBXQKIS0GxyRLHLCP" +
                "rItL15ZOYRrGydz+ce+QbNJsdokL7DKh9zQLdtyf2u4pdwjuWNokoSRDzROZd9+zZz6LI/cef8k2BwyF" +
                "NHbTEMXI/q86lMHt0X1XmgWwu7v93N02ky+YJiPT8U6QASC+Ssxe/PFLhieYGNeYV04AwMq/qM+lXcr+" +
                "5/qXPXa7q1ernCO+sAiXq9BbZ01m/MjjjukBQ/X0gFIUVZ0GQSIXzP+lxI0FSx37grN4qxus26mgrCid" +
                "XW6rwtYxVjrZJYgsQGUWykDjNwXiYxNMWGKDGkjCWJgjqidMCKkIoNc3qH6WlvIpsKCa0epPCSOjrjcO" +
                "AXuxVQCYglZwL5lGL6C9SFu1T1ToWTKjvaFeAcx9zl/bjE89uqymrpbXLnoT5/w4MID4JN+NZRF8SMDt" +
                "1MfuRuQY4OrX2lwLEKx447euU4kJ8g+hDCDSh3A/c7mzygLRlB4mEeb9+HDxx6MR99/AdMS4bJE37Xom" +
                "egvLQc0AocoOh7repHKMTNqeXp9g5OWv9/LL//x/xhz1MI4/7SHy4XW5n0502AxcBjVlyO6IL/TJ1v6i" +
                "pvCtkKyrVR+Fps7FdHkwRFaQ4jAhH68jnaUmkyQVA+tKnYNDEJidXelIQ3KnouLgK/mt9Z67QdW4ufk0" +
                "0ShpPY7UYVzWQBWqIUmSAqwFUo8HdzeqxFpVDHxCQMxTFGFSmpY1NoaQ5Txb6lfFWwUr/25QwSm6A5MY" +
                "GtBVbC4enEzGjLxj1WIJlkMjwTBgRWmTGYJZeOyEXhbBOQiNYkxM7z43y3OS9MlH4C/n00jG7DKJFxtl" +
                "OfwqcoA1aUOPvVaT+HAz8CvoU7NaP89klTB5E1vOyZezm2exn3zDyUHxx6Xp9m+24XuCtSOlH2R+3egW" +
                "VvXFD1vghHgOyr6FsE1qg7nfkeYKmKrG+Lh3R52x4MBilajMEEMqzwac8lKssdU3iHOhrB2HZ5NkixZK" +
                "ibJYPrwEY0bXsI62ZVtlKZ4R54Ew0jkEoMRi1WrYu2d2BWi/B2mLGWBAsKI+qOLjOqDCcxo/VMNp8Ujo" +
                "A26k8vDj6vVTXntCAYtcRqsdZ6wvG2j8hZQ6KWACGUYGEozNf6yjDVUmu2pwOYY1S7RtWOgVS5BHuxKt" +
                "bELt5JwrZAKiZ5HPNZWbUQG5piI81xIrb2buABolxpguBSRUe3C3wDIbCQSswMRc8SrSuLc8RkDcCp08" +
                "KWC9xgkV/kMuGDhdWieGL/QZWvAJeWpzJM+HQxLj3u2QK/VvgDjlnex/ixUh1AKglCq8jmBUOb15/d5z" +
                "JuXH3wOcT5kTgw8ZZAzRbgHnGDNwPajNIOmhLnK5utjoT4DG6bXBVoNVlEbbGasNS6NC4nVWjEDtzJPS" +
                "vGtUAlG0IiacjnSW8LVj2489bm1tq9MaKkY4N6R611nw/U0TEnK5waIH9D6FdQfO4lVa3rHCRxSkGrUX" +
                "2N1M5pFRb5vIZrfPdoxZ5krL3T1lDFCr4rl5xRTzbK9Cr3jZl4CMRUSi1AnmHgDKP20YIGW18d7clW5b" +
                "jqmpWiqiJqNTRmuClv+o3o4jqs+gvURXH8Bpd6O/Jza32FbjBM9G9aG7BvYPXMprw5us52embS8CPchr" +
                "KW7Y8OKNRjk59n62woKxMJnhLYikwc/qVHcX1W0aOAF9ZR6gDYG+Xd6vxYee/IHvW7THMLq6egK4ZSLZ" +
                "8Mg/Qvz151M4mE/GavlxObPNkgg7isMsb7wkao45y7XJh2avZ4BoheHLHvpEqgr/BCq3pVsdM1DwTGEs" +
                "aWTLKNBjwNqk66In8BpBMXuBqoxUes1hOLtg/wS3Ha4//5+qeoz04rBGfPRjnRYPerQ7MaLQKpWoHW8w" +
                "SuQuSDonqHuubSIp9bYRxEOJZOM+jZRu3FNe6b9YNzilUqQYTuRDUnlUNjUri4wTXiOGIjTPQrFtcxEO" +
                "cDPcuqfkzRqPrzXTjZ/N4tpG1+QFQ3R2IOrdNbcyxu1zgpMnBM5R8okdGwqP8j0MnYkD2ieIO4LhMNE8" +
                "lnbBjcuRP8k7boqqi0RclEf8gGhQ+gXCfo4jpCcQqfZzyitxd/WFSDo0T5ZiJhb09Tx52vXHsH71hxAx" +
                "8++lLuWvQiWkY2MZntUhVh8fAmQpNgVIqDvZf6YkqFZBJHej9M/0Q/m9OKUnFpamKdpyK1qANhAaxSxp" +
                "JYS+AOFxim/sNFW4Dj/TXVshHo8q29SpmYYLqM7I/Iz0AfwOWmGvUrYSERXxMwX9E2hUMt7FdH9H1WfJ" +
                "D3VPtzBfrV9Xc1XtdC4McIS72FMODrWPBJNfs6NgN7JXBMPRkGAmtILKiJSCUjgyjrAtXJ1DmgvvClHX" +
                "NiA4qsHecraT0/I50jn2uK4lqUip0l5VaG+60LJfgfb+7VhVjluqBET5kLvBPkThUhG0YcMLCau8zzyI" +
                "bNtNrRugBaVBlp6uTSB77AnOxWhqMJIQKbFM1SmT80U50P6eFijShIDJlsTLqEFKciC2xQ1bdiCif9WA" +
                "kIkwTr/RSK9AOP4VIk707yGJAXEVCWKzpYwyEPSgZ4nr92hv9ik0jwV2fxTvkoX+2vOTXngnIM1eIIJ1" +
                "nURCGdDLPXwBNsTc3JB9i/iNYM5dq14ur0jI1kmQ0CQmNrEmOLwbRlMMdoW5pcU4rATm1fBhUqcK8qr7" +
                "Kq91hnd49fqKc/ll4TdsLUus0QSq59FJoV2Nw0nTev+hUXPv4jbNjEaNhUVz6rOExxcPcjpEyOXBBisc" +
                "XlMBbiMZ3Sm4hFl6hqW6EuBiFtoT0/RDpvycTJhCs05iVRYsIocdvIpVcqNmsDY58OjKVwlcSB/p6dc/" +
                "z2zpRbnE2hKHofldNAT2kZVG2yi85S7MadWDS1070cWAVpZue7eOWGaxkNW8Blmn2G3U9uDYyuiPJmMk" +
                "DlKh8IzcN50tkYDWTXg5V7pD6k8EoLVxvImyIHiS25tGBQu2u9Ue636gLOrbXF10xr7f7WWsQkrG2ZxT" +
                "mrbuWCZT8iiZrIBZmqgBWoJdu2gLc88XMWDq1lWCIStWOXURtYa1OF5+m2ZfVZp9z7tMnyZNM8lrUcSJ" +
                "MZHTsdzF/7RIk9NRUbKl1RYldGxm4MjFK4SuROxvhfu4V8A7cZQpHP6pNsz0GvO6bpfL26El5LSa/LB1" +
                "Is3wbu28n1ML4tu6tMZasCA6kORsJ2QmvXAB9kN6IHIKxH/BiVSeuQWnDLtHjAslMIWLUccL0o56vqm6" +
                "3qbcVaVgkIoV7Hsdj3WOxDFpt27yeARbuF2WOeNGtY0zqTNz71Gy5Y/mPyqeaKJ9sAZFMtPGft7bTFww" +
                "yHrQToeN5e95U5xSagqo6bZa7cgVV3CnWiMCM/zMrmfbiz8yQ68v2I1eLrg2J0jhioeFkeCmM7kMobUn" +
                "kHYCXdbICPA1/dDYblT/xEKZWbcBskG3dBGVYAF5XRLmrtYHaUrKW1DjcojeLfgMuksTMB62PAR6gest" +
                "QWS4rZuS6O7b2bbNgMcA1wNu4osWdbbgdew7acyWbqO0AiU+xHbhnOk8klUwZY9WZO/DkuNvDVqno28G" +
                "It3V1xntrfAb8A3l5BU+4AFk+Y0GpY3zyds1bfSqLCDw853wUWl1NHW+dw+oNfZkn/Bja2TifY9juHle" +
                "YaG88RZgHiQGSDFaEz6WNDpoO/TjuW+bmNGJngmjoVeEsNF1oM+sex6SMQiGVa0s9oOGH8DgGX+oFxmn" +
                "eiHzTljlMnA7gUzD/H5T+vpEFdjTu95R04Ht28KdCm/tdrOMvL/D2c5DhNbce4XgmcCbzXT9D9trMua3" +
                "F8vU/7vqwaPhNLmvnZbp7T+yfumfES9hxdhZXwWtzO75uAFwCH4pz8u4L1v8E/+zYcV3JgO5SYiUbY2B" +
                "wTmodyLjSjKUVgdq4CKdd3zt4GVtRCQStSR/wZ9zuptuFY58co+jxgCS4/1WyQV6pyM8JjuHxFvkYTQO" +
                "Xbqlw0scaCoLTHQ9wy93jiMDVmOxfHKCZhmkO2Psb87FbibY9sbxPhRlFerCX3dnaBArJCRY5sSYazub" +
                "CZby9+F081LWFOvqsYrPbH8Vv2qtF293hQGyl0VIByRP/9vHDLb6lC/luCc/LP8Ndi3A6l4ngy7ZPwc4" +
                "mf6H24PNqFMxjWA6twu2DLmdx5cEf7chzkJ9BCxpMvBl57LU181OfNoSb/+Ub/zDW4Xb91OL5QJTkOl4" +
                "poqMREKgT2PgncUfT5ybhc/r1lJlnon1VNizoQl4X2cN8fc3y5W/3rwr7YFsbpNKtwIVurGd+gWQC8mJ" +
                "LklEXa3wrg7kgTskErt805naUdz6HFDJdqgXu7+cMMXGzoXoynsV6AixqZjO8zxOXiKvCyuvzrbl+n5Z" +
                "Scq1AiBzN+IEc5ukTKP+MmHF1pvOIOMBhty5DPRJP2gdlQSPzcsSLkMiGjqcjN3u0Aq6LvaiswQx3PZz" +
                "Zwh6KdKPfgsh6Uk6cVTlX0iUKvJtY9QVDassMVl5cA6IsiSDcTbtsGpwC3VtJY1Pm8bggxb8oFmfgHiy" +
                "7imaY9mZ70YscXkhMYD5GpJW8Hvj9B1ZIMGR3lklWXY7cryRoZP+HNJMAqwNEJ8lO0Erf5gVlIs9ru27" +
                "i4B0Y90BQ0nkF2u3DYqm1YWCwyvr6FNXLIt2t+R9pXEWbmh1naeQg/2PNSZuS7J/PztDfLJpP9KfBadO" +
                "5ZaVCjJOmQoY6mSe8QVgOrHU2gP+yvZ8Dtvila64SlJcBZqvrZwgeJFvYToS/TUkdjmLE85+Lo222CjB" +
                "3mWI5GIUDaZm8bjtrinyy8gE2ZhXQmfQWgynjz4NI6TaA7gJ9u+wxLazK4I+35rA2wKjJU4z0loPX3Jl" +
                "pUp2Gm4Ah1QYkDGAnbCHTSptEOAeWzBf9TsTMY7cTyFXp+s1c1t8kQWqSVXYTbvn7Ujq09MqnRVRyLq3" +
                "Cmoq+a+KwSvdXmzU1zX3vXyraDGQqn0WYGWy1BQY2/EDiC+uLVutj1O2gNacg/veyUyHgXM+XoA6pqVX" +
                "WZqqX0GjMzYW+FzyjM2Qm2wgtU+awqwUQ+2Dj7yqJE1vyiOz27c3Nm/qczNvtdS3Az7qy+L3s/Tb2BtV" +
                "8t8Z+MI78m9KZmceOCc8nYbGN0NaQhu5tmpWNG3Nj7Pymc55nPJd8ce306jvfzTmoHm1aSF6nq9xWtGP" +
                "1uEKbHw/pwx5E/39i0+Nz2ZOCCHQDpq4Arq7WEkZDBaBryd+OyaUhD/gk0K5cszpI1CUbtGLtd4deq+r" +
                "Sy7T8mvOK1uC31Ayf56DTwOpyDe+/K0CE0WYMJghsf7jg2P5jqPCOl8fbW+X9webxMLf9EqKYxshrSkN" +
                "0JrBTGclWXhNX0KnxRKRvyMjF8R/U0XBjC+3A7iNVOImnBHXjwWRy5ripQqcCcONv/VOt+X1o27fA3ti" +
                "RDC2XyIR7WA+rFlBxL3pCGNOTvwAwUgdhYskniTz7ZBgS+j+MhvbaO2+0NfA53tNHdwJ9xAbcDHizacA" +
                "7w43GBwZ21EUbojIfYkGl//52zTt0rUnNWpPFks1l0+XJ9vjFUGtKQv1OwKjYFK0gCvAdHMDuoztvt6a" +
                "9zPEgNebNrGnU6wBXBiDA9zjIctn/9R3te+NdVrYffNDu3sTXA1gtWimP4fgoX39eQhjTk79kMrEPWAW" +
                "/F5QbanrIVkt/VDVQKdMZDzDfJL/4644SvLWs/6oud8dZFIUk9QKW4kKGzgZc3gKohDfkH0qzqkPv95P" +
                "8pVM9ZvSPimFqWlMkzkur4441zjvvdvTNedN3M3JbD0jaLrYx3hfCDPwP3eo1qqDE0GtKQfz6wAsdLeg" +
                "2OqnDUVD0kY/OkrfnBDbNP+8Btc886bUZu8zOtycBtiBQnMhRvWvYyHrO2ml2QZrchDWnI7g9W4AJLz8" +
                "Sl1XjHFNy/N0A/CXxjS3pOLq9cNy3u/ZkJouTv5wi7hjSkIZVkV5/tthaXnP+1wJ920j0HcDuPL1d3mG" +
                "kuY3OkNMlaCZJGFzekIQ3NanslD9zgweoFwBuB09k2YVotWYpLV3szLmfTaKM7G9KQBlhNhXQDv8Ttvu" +
                "2HO3nmRBzNYT7juZEKNtxmnG/pIVzysKX+HnGjGxvSkH98+f8DACeR8Z8W+T8oAAAAAElFTkSuQmCC\" " +
                "/>\r\n  <h2>DiskSpace Report</h2>\r\n  <table border=\"1\">\r\n    <tr bgcolor=\"#66ccff\"" +
                ">\r\n\t\t<th>PackageID</th>\r\n        <th>QuotaValue</th>\r\n        <th>Bandwidth</th>" +
                "\r\n        <th>UsagePercentage</th>\r\n        <th>PackageName</th>\r\n        <th>Pa" +
                "ckagesNumber</th>\r\n        <th>StatusID</th>\r\n        <th>UserID</th>\r\n      <th" +
                ">Username</th>\r\n        <th>FirstName</th>\r\n        <th>LastName</th>\r\n        <" +
                "th>FullName</th>\r\n        <th>RoleID</th>\r\n        <th>Email</th>\r\n    </tr>\r\n  " +
                "  <xsl:for-each select=\"//Table1\">\r\n    <tr>\r\n\t<td><xsl:value-of select=\"Package" +
                "ID\"/></td>\r\n        <td><xsl:value-of select=\"QuotaValue\"/></td>\r\n        <td><x" +
                "sl:value-of select=\"Bandwidth\"/></td>\r\n        <td><xsl:value-of select=\"UsagePe" +
                "rcentage\"/>%</td>\r\n        <td><xsl:value-of select=\"PackageName\"/></td>\r\n      " +
                "  <td><xsl:value-of select=\"PackagesNumber\"/></td>\r\n        <td><xsl:value-of se" +
                "lect=\"StatusID\"/></td>\r\n        <td><xsl:value-of select=\"UserID\"/></td>\r\n      " +
                "<td><xsl:value-of select=\"Username\"/></td>\r\n        <td><xsl:value-of select=\"Fi" +
                "rstName\"/></td>\r\n        <td><xsl:value-of select=\"LastName\"/></td>\r\n        <td" +
                "><xsl:value-of select=\"FullName\"/></td>\r\n        <td><xsl:value-of select=\"RoleI" +
                "D\"/></td>\r\n        <td><xsl:value-of select=\"Email\"/></td>\r\n        <td><xsl:val" +
                "ue-of select=\"UserComments\"/></td>\r\n    </tr>\r\n    </xsl:for-each>\r\n  </table>\r\n" +
                "  </body>\r\n  </html>\r\n</xsl:template>\r\n</xsl:stylesheet>" },
            new UserSetting() { UserId = 1, SettingsName = "DiskspaceXLST", PropertyName = "TransformContentType", PropertyValue = "text/html" },
            new UserSetting() { UserId = 1, SettingsName = "DiskspaceXLST", PropertyName = "TransformSuffix", PropertyValue = ".htm" },
            new UserSetting() { UserId = 1, SettingsName = "DisplayPreferences", PropertyName = "GridItems", PropertyValue = "10" },
            new UserSetting() { UserId = 1, SettingsName = "DomainExpirationLetter", PropertyName = "CC", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "DomainExpirationLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "DomainExpirationLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Domain Expiratio" +
                "n Information</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-colo" +
                "r: ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; " +
                "font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: sol" +
                "id 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { " +
                "font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; " +
                "color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { fo" +
                "nt-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px " +
                "##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; fon" +
                "t-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary " +
                "TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em;" +
                " font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight" +
                ": normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"to" +
                "p\"></a>\r\n<div class=\"Header\">\r\n\tDomain Expiration Information\r\n</div>\r\n\r\n<ad:if " +
                "test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nPlease, fin" +
                "d below details of your domain expiration information.\r\n</p>\r\n\r\n<table>\r\n    <th" +
                "ead>\r\n        <tr>\r\n            <th>Domain</th>\r\n\t\t\t<th>Registrar</th>\r\n\t\t\t<th>C" +
                "ustomer</th>\r\n            <th>Expiration Date</th>\r\n        </tr>\r\n    </thead>\r" +
                "\n    <tbody>\r\n            <ad:foreach collection=\"#Domains#\" var=\"Domain\" index=" +
                "\"i\">\r\n        <tr>\r\n            <td>#Domain.DomainName#</td>\r\n\t\t\t<td>#iif(isnull" +
                "(Domain.Registrar), \"\", Domain.Registrar)#</td>\r\n\t\t\t<td>#Domain.Customer#</td>\r\n" +
                "            <td>#iif(isnull(Domain.ExpirationDate), \"\", Domain.ExpirationDate)#<" +
                "/td>\r\n        </tr>\r\n    </ad:foreach>\r\n    </tbody>\r\n</table>\r\n\r\n<ad:if test=\"#" +
                "IncludeNonExistenDomains#\">\r\n\t<p>\r\n\tPlease, find below details of your non-exist" +
                "en domains.\r\n\t</p>\r\n\r\n\t<table>\r\n\t\t<thead>\r\n\t\t\t<tr>\r\n\t\t\t\t<th>Domain</th>\r\n\t\t\t\t<th" +
                ">Customer</th>\r\n\t\t\t</tr>\r\n\t\t</thead>\r\n\t\t<tbody>\r\n\t\t\t\t<ad:foreach collection=\"#No" +
                "nExistenDomains#\" var=\"Domain\" index=\"i\">\r\n\t\t\t<tr>\r\n\t\t\t\t<td>#Domain.DomainName#<" +
                "/td>\r\n\t\t\t\t<td>#Domain.Customer#</td>\r\n\t\t\t</tr>\r\n\t\t</ad:foreach>\r\n\t\t</tbody>\r\n\t</" +
                "table>\r\n</ad:if>\r\n\r\n\r\n<p>\r\nIf you have any questions regarding your hosting acco" +
                "unt, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest" +
                " regards\r\n</p>" },
            new UserSetting() { UserId = 1, SettingsName = "DomainExpirationLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "DomainExpirationLetter", PropertyName = "Subject", PropertyValue = "Domain expiration notification" },
            new UserSetting() { UserId = 1, SettingsName = "DomainExpirationLetter", PropertyName = "TextBody", PropertyValue = "=================================\r\n   Domain Expiration Information\r\n===========" +
                "======================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>" +
                "\r\n\r\nPlease, find below details of your domain expiration information.\r\n\r\n\r\n<ad:f" +
                "oreach collection=\"#Domains#\" var=\"Domain\" index=\"i\">\r\n\tDomain: #Domain.DomainNa" +
                "me#\r\n\tRegistrar: #iif(isnull(Domain.Registrar), \"\", Domain.Registrar)#\r\n\tCustome" +
                "r: #Domain.Customer#\r\n\tExpiration Date: #iif(isnull(Domain.ExpirationDate), \"\", " +
                "Domain.ExpirationDate)#\r\n\r\n</ad:foreach>\r\n\r\n<ad:if test=\"#IncludeNonExistenDomai" +
                "ns#\">\r\nPlease, find below details of your non-existen domains.\r\n\r\n<ad:foreach co" +
                "llection=\"#NonExistenDomains#\" var=\"Domain\" index=\"i\">\r\n\tDomain: #Domain.DomainN" +
                "ame#\r\n\tCustomer: #Domain.Customer#\r\n\r\n</ad:foreach>\r\n</ad:if>\r\n\r\nIf you have any" +
                " questions regarding your hosting account, feel free to contact our support depa" +
                "rtment at any time.\r\n\r\nBest regards" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "CC", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>MX and NS Change" +
                "s Information</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-colo" +
                "r: ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; " +
                "font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: sol" +
                "id 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { " +
                "font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; " +
                "color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { fo" +
                "nt-size: 1.3em; color: ##1F4978; } \r\n\t\t.Summary H3 { font-size: 1em; color: ##1F" +
                "4978; } \r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summa" +
                "ry TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: b" +
                "old; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-siz" +
                "e: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n    " +
                "    .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n" +
                "</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\"></a>\r\n<div class=\"Header" +
                "\">\r\n\tMX and NS Changes Information\r\n</div>\r\n\r\n<ad:if test=\"#user#\">\r\n<p>\r\nHello " +
                "#user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nPlease, find below details of MX and N" +
                "S changes.\r\n</p>\r\n\r\n    <ad:foreach collection=\"#Domains#\" var=\"Domain\" index=\"i" +
                "\">\r\n\t<h2>#Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #Domain" +
                "Users[Domain.PackageId].LastName#</h2>\r\n\t<h3>#iif(isnull(Domain.Registrar), \"\", " +
                "Domain.Registrar)# #iif(isnull(Domain.ExpirationDate), \"\", Domain.ExpirationDate" +
                ")#</h3>\r\n\r\n\t<table>\r\n\t    <thead>\r\n\t        <tr>\r\n\t            <th>DNS</th>\r\n\t\t\t" +
                "\t<th>Type</th>\r\n\t\t\t\t<th>Status</th>\r\n\t            <th>Old Value</th>\r\n          " +
                "      <th>New Value</th>\r\n\t        </tr>\r\n\t    </thead>\r\n\t    <tbody>\r\n\t        " +
                "<ad:foreach collection=\"#Domain.DnsChanges#\" var=\"DnsChange\" index=\"j\">\r\n\t      " +
                "  <tr>\r\n\t            <td>#DnsChange.DnsServer#</td>\r\n\t            <td>#DnsChange" +
                ".Type#</td>\r\n\t\t\t\t<td>#DnsChange.Status#</td>\r\n                <td>#DnsChange.Old" +
                "Record.Value#</td>\r\n\t            <td>#DnsChange.NewRecord.Value#</td>\r\n\t        " +
                "</tr>\r\n\t    \t</ad:foreach>\r\n\t    </tbody>\r\n\t</table>\r\n\t\r\n    </ad:foreach>\r\n\r\n<p" +
                ">\r\nIf you have any questions regarding your hosting account, feel free to contac" +
                "t our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p>" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "NoChangesHtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>MX and NS Change" +
                "s Information</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-colo" +
                "r: ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; " +
                "font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: sol" +
                "id 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { " +
                "font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; " +
                "color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { fo" +
                "nt-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px " +
                "##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; fon" +
                "t-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary " +
                "TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em;" +
                " font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight" +
                ": normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"to" +
                "p\"></a>\r\n<div class=\"Header\">\r\n\tMX and NS Changes Information\r\n</div>\r\n\r\n<ad:if " +
                "test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nNo MX and N" +
                "S changes have been found.\r\n</p>\r\n\r\n<p>\r\nIf you have any questions regarding you" +
                "r hosting account, feel free to contact our support department at any time.\r\n</p" +
                ">\r\n\r\n<p>\r\nBest regards\r\n</p>" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "NoChangesTextBody", PropertyValue = "=================================\r\n   MX and NS Changes Information\r\n===========" +
                "======================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>" +
                "\r\n\r\nNo MX and NS changes have been founded.\r\n\r\nIf you have any questions regardi" +
                "ng your hosting account, feel free to contact our support department at any time" +
                ".\r\n\r\nBest regards\r\n" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "Subject", PropertyValue = "MX and NS changes notification" },
            new UserSetting() { UserId = 1, SettingsName = "DomainLookupLetter", PropertyName = "TextBody", PropertyValue = "=================================\r\n   MX and NS Changes Information\r\n===========" +
                "======================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>" +
                "\r\n\r\nPlease, find below details of MX and NS changes.\r\n\r\n\r\n<ad:foreach collection" +
                "=\"#Domains#\" var=\"Domain\" index=\"i\">\r\n\r\n #Domain.DomainName# - #DomainUsers[Doma" +
                "in.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#\r\n Registrar:  " +
                "    #iif(isnull(Domain.Registrar), \"\", Domain.Registrar)#\r\n ExpirationDate: #iif" +
                "(isnull(Domain.ExpirationDate), \"\", Domain.ExpirationDate)#\r\n\r\n        <ad:forea" +
                "ch collection=\"#Domain.DnsChanges#\" var=\"DnsChange\" index=\"j\">\r\n            DNS:" +
                "       #DnsChange.DnsServer#\r\n            Type:      #DnsChange.Type#\r\n\t    Stat" +
                "us:    #DnsChange.Status#\r\n            Old Value: #DnsChange.OldRecord.Value#\r\n " +
                "           New Value: #DnsChange.NewRecord.Value#\r\n\r\n    \t</ad:foreach>\r\n</ad:fo" +
                "reach>\r\n\r\n\r\n\r\nIf you have any questions regarding your hosting account, feel fre" +
                "e to contact our support department at any time.\r\n\r\nBest regards\r\n" },
            new UserSetting() { UserId = 1, SettingsName = "ExchangeMailboxSetupLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "ExchangeMailboxSetupLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Account Summary " +
                "Information</title>\r\n    <style type=\"text/css\">\r\n        body {font-family: 'Se" +
                "goe UI Light','Open Sans',Arial!important;color:black;}\r\n        p {color:black;" +
                "}\r\n\t\t.Summary { background-color: ##ffffff; padding: 5px; }\r\n\t\t.SummaryHeader { " +
                "padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color:" +
                " ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0" +
                "153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Su" +
                "mmary H1 { font-size: 1.5em; color: ##1F4978; border-bottom: dotted 3px ##efefef" +
                "; font-weight:normal; }\r\n        .Summary H2 { font-size: 1.2em; color: ##1F4978" +
                "; } \r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary T" +
                "H,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold;" +
                " background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9" +
                "pt; color:black;}\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold;" +
                " }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n      " +
                "  .Label { color:##1F4978; }\r\n        .menu-bar a {padding: 15px 0;display: inli" +
                "ne-block;}\r\n    </style>\r\n</head>\r\n<body>\r\n<table border=\"0\" cellspacing=\"0\" cel" +
                "lpadding=\"0\" width=\"100%\"><!-- was 800 -->\r\n<tbody>\r\n<tr>\r\n<td style=\"padding: 1" +
                "0px 20px 10px 20px; background-color: ##e1e1e1;\">\r\n<table border=\"0\" cellspacing" +
                "=\"0\" cellpadding=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"text-align: left; " +
                "padding: 0px 0px 2px 0px;\"><a href=\"\"><img src=\"\" border=\"0\" alt=\"\" /></a></td>\r" +
                "\n</tr>\r\n</tbody>\r\n</table>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" wi" +
                "dth=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"padding-bottom: 10px;\">\r\n<table border=\"0" +
                "\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"backg" +
                "round-color: ##2e8bcc; padding: 3px;\">\r\n<table class=\"menu-bar\" border=\"0\" cells" +
                "pacing=\"0\" cellpadding=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"text-align: " +
                "center;\" width=\"20%\"><a style=\"color: ##ffffff; text-transform: uppercase; font-" +
                "size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-de" +
                "coration: none;\" href=\"\"</a></td>\r\n<td style=\"text-align: center;\" width=\"20%\"><" +
                "a style=\"color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight" +
                ": bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;\" href=" +
                "\"\"></a></td>\r\n<td style=\"text-align: center;\" width=\"20%\"><a style=\"color: ##fff" +
                "fff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: " +
                "Arial, Helvetica, sans-serif; text-decoration: none;\" href=\"\"></a></td>\r\n<td sty" +
                "le=\"text-align: center;\" width=\"20%\"><a style=\"color: ##ffffff; text-transform: " +
                "uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, san" +
                "s-serif; text-decoration: none;\" href=\"\"></a></td>\r\n<td style=\"text-align: cente" +
                "r;\" width=\"20%\"><a style=\"color: ##ffffff; text-transform: uppercase; font-size:" +
                " 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decorat" +
                "ion: none;\" href=\"\"></a></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>" +
                "\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table border=\"0\" cellspacing=\"0\" " +
                "cellpadding=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"background-color: ##fff" +
                "fff;\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\"><!-- was " +
                "759 -->\r\n<tbody>\r\n<tr>\r\n<td style=\"vertical-align: top; padding: 10px 10px 0px 1" +
                "0px;\" width=\"100%\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"10" +
                "0%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family: 'Segoe UI Light','Open Sans',Arial;" +
                " padding: 0px 10px 0px 0px;\">\r\n<!-- Begin Content -->\r\n<div class=\"Summary\">\r\n  " +
                "  <ad:if test=\"#Email#\">\r\n    <p>\r\n    Hello #Account.DisplayName#,\r\n    </p>\r\n " +
                "   <p>\r\n    Thanks for choosing as your Exchange hosting provider.\r\n    </p>\r\n  " +
                "  </ad:if>\r\n    <ad:if test=\"#not(PMM)#\">\r\n    <h1>User Accounts</h1>\r\n    <p>\r\n" +
                "    The following user accounts have been created for you.\r\n    </p>\r\n    <table" +
                ">\r\n        <tr>\r\n            <td class=\"Label\">Username:</td>\r\n            <td>#" +
                "Account.UserPrincipalName#</td>\r\n        </tr>\r\n        <tr>\r\n            <td cl" +
                "ass=\"Label\">E-mail:</td>\r\n            <td>#Account.PrimaryEmailAddress#</td>\r\n  " +
                "      </tr>\r\n\t\t<ad:if test=\"#PswResetUrl#\">\r\n        <tr>\r\n            <td class" +
                "=\"Label\">Password Reset Url:</td>\r\n            <td><a href=\"#PswResetUrl#\" targe" +
                "t=\"_blank\">Click here</a></td>\r\n        </tr>\r\n\t\t</ad:if>\r\n    </table>\r\n    </a" +
                "d:if>\r\n    <h1>DNS</h1>\r\n    <p>\r\n    In order for us to accept mail for your do" +
                "main, you will need to point your MX records to:\r\n    </p>\r\n    <table>\r\n       " +
                " <ad:foreach collection=\"#SmtpServers#\" var=\"SmtpServer\" index=\"i\">\r\n           " +
                " <tr>\r\n                <td class=\"Label\">#SmtpServer#</td>\r\n            </tr>\r\n " +
                "       </ad:foreach>\r\n    </table>\r\n   <h1>\r\n    Webmail (OWA, Outlook Web Acces" +
                "s)</h1>\r\n    <p>\r\n    <a href=\"\" target=\"_blank\"></a>\r\n    </p>\r\n    <h1>\r\n    O" +
                "utlook (Windows Clients)</h1>\r\n    <p>\r\n    To configure MS Outlook to work with" +
                " the servers, please reference:\r\n    </p>\r\n    <p>\r\n    <a href=\"\" target=\"_blan" +
                "k\"></a>\r\n    </p>\r\n    <p>\r\n    If you need to download and install the Outlook " +
                "client:</p>\r\n        \r\n        <table>\r\n            <tr><td colspan=\"2\" class=\"L" +
                "abel\"><font size=\"3\">MS Outlook Client</font></td></tr>\r\n            <tr>\r\n     " +
                "           <td class=\"Label\">\r\n                    Download URL:</td>\r\n         " +
                "       <td><a href=\"\"></a></td>\r\n            </tr>\r\n<tr>\r\n                <td cl" +
                "ass=\"Label\"></td>\r\n                <td><a href=\"\"></a></td>\r\n            </tr>\r\n" +
                "            <tr>\r\n                <td class=\"Label\">\r\n                    KEY:</" +
                "td>\r\n                <td></td>\r\n            </tr>\r\n        </table>\r\n \r\n       <" +
                "h1>\r\n    ActiveSync, iPhone, iPad</h1>\r\n    <table>\r\n        <tr>\r\n            <" +
                "td class=\"Label\">Server:</td>\r\n            <td>#ActiveSyncServer#</td>\r\n        " +
                "</tr>\r\n        <tr>\r\n            <td class=\"Label\">Domain:</td>\r\n            <td" +
                ">#SamDomain#</td>\r\n        </tr>\r\n        <tr>\r\n            <td class=\"Label\">SS" +
                "L:</td>\r\n            <td>must be checked</td>\r\n        </tr>\r\n        <tr>\r\n    " +
                "        <td class=\"Label\">Your username:</td>\r\n            <td>#SamUsername#</td" +
                ">\r\n        </tr>\r\n    </table>\r\n \r\n    <h1>Password Changes</h1>\r\n    <p>\r\n    P" +
                "asswords can be changed at any time using Webmail or the <a href=\"\" target=\"_bla" +
                "nk\">Control Panel</a>.</p>\r\n    <h1>Control Panel</h1>\r\n    <p>\r\n    If you need" +
                " to change the details of your account, you can easily do this using <a href=\"\" " +
                "target=\"_blank\">Control Panel</a>.</p>\r\n    <h1>Support</h1>\r\n    <p>\r\n    You h" +
                "ave 2 options, email <a href=\"mailto:\"></a> or use the web interface at <a href=" +
                "\"\"></a></p>\r\n    \r\n</div>\r\n<!-- End Content -->\r\n<br></td>\r\n</tr>\r\n</tbody>\r\n</t" +
                "able>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td style=\"backgrou" +
                "nd-color: ##ffffff; border-top: 1px solid ##999999;\">\r\n<table border=\"0\" cellspa" +
                "cing=\"0\" cellpadding=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"vertical-align" +
                ": top; padding: 0px 20px 15px 20px;\">\r\n<table border=\"0\" cellspacing=\"0\" cellpad" +
                "ding=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family: Arial, Helvetica," +
                " sans-serif; text-align: left; font-size: 9px; color: ##717073; padding: 20px 0p" +
                "x 0px 0px;\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">\r\n<" +
                "tbody>\r\n<tr>\r\n<td style=\"font-family: Arial, Helvetica, sans-serif; font-size: 9" +
                "px; text-align: left; color: ##1666af; vertical-align: top;\" width=\"33%\"><a styl" +
                "e=\"font-weight: bold; text-transform: uppercase; text-decoration: underline; col" +
                "or: ##1666af;\" href=\"\"></a><br />Learn more about the services can provide to im" +
                "prove your business.</td>\r\n<td style=\"font-family: Arial, Helvetica, sans-serif;" +
                " font-size: 9px; text-align: left; color: ##1666af; padding: 0px 10px 0px 10px; " +
                "vertical-align: top;\" width=\"34%\"><a style=\"font-weight: bold; text-transform: u" +
                "ppercase; text-decoration: underline; color: ##1666af;\" href=\"\">Privacy Policy</" +
                "a><br /> follows strict guidelines in protecting your privacy. Learn about our <" +
                "a style=\"font-weight: bold; text-decoration: underline; color: ##1666af;\" href=\"" +
                "\">Privacy Policy</a>.</td>\r\n<td style=\"font-family: Arial, Helvetica, sans-serif" +
                "; font-size: 9px; text-align: left; color: ##1666af; vertical-align: top;\" width" +
                "=\"33%\"><a style=\"font-weight: bold; text-transform: uppercase; text-decoration: " +
                "underline; color: ##1666af;\" href=\"\">Contact Us</a><br />Questions? For more inf" +
                "ormation, <a style=\"font-weight: bold; text-decoration: underline; color: ##1666" +
                "af;\" href=\"\">contact us</a>.</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tb" +
                "ody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</tabl" +
                "e>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</body>\r\n</html>" },
            new UserSetting() { UserId = 1, SettingsName = "ExchangeMailboxSetupLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "ExchangeMailboxSetupLetter", PropertyName = "Subject", PropertyValue = " Hosted Exchange Mailbox Setup" },
            new UserSetting() { UserId = 1, SettingsName = "ExchangeMailboxSetupLetter", PropertyName = "TextBody", PropertyValue = "<ad:if test=\"#Email#\">\r\nHello #Account.DisplayName#,\r\n\r\nThanks for choosing as y" +
                "our Exchange hosting provider.\r\n</ad:if>\r\n<ad:if test=\"#not(PMM)#\">\r\nUser Accoun" +
                "ts\r\n\r\nThe following user accounts have been created for you.\r\n\r\nUsername: #Accou" +
                "nt.UserPrincipalName#\r\nE-mail: #Account.PrimaryEmailAddress#\r\n<ad:if test=\"#PswR" +
                "esetUrl#\">\r\nPassword Reset Url: #PswResetUrl#\r\n</ad:if>\r\n</ad:if>\r\n\r\n===========" +
                "======================\r\nDNS\r\n=================================\r\n\r\nIn order for u" +
                "s to accept mail for your domain, you will need to point your MX records to:\r\n\r\n" +
                "<ad:foreach collection=\"#SmtpServers#\" var=\"SmtpServer\" index=\"i\">#SmtpServer#</" +
                "ad:foreach>\r\n\r\n=================================\r\nWebmail (OWA, Outlook Web Acce" +
                "ss)\r\n=================================\r\n\r\n\r\n\r\n=================================\r" +
                "\nOutlook (Windows Clients)\r\n=================================\r\n\r\nTo configure MS" +
                " Outlook to work with servers, please reference:\r\n\r\n\r\n\r\nIf you need to download " +
                "and install the MS Outlook client:\r\n\r\nMS Outlook Download URL:\r\n\r\nKEY: \r\n\r\n=====" +
                "============================\r\nActiveSync, iPhone, iPad\r\n========================" +
                "=========\r\n\r\nServer: #ActiveSyncServer#\r\nDomain: #SamDomain#\r\nSSL: must be check" +
                "ed\r\nYour username: #SamUsername#\r\n\r\n=================================\r\nPassword " +
                "Changes\r\n=================================\r\n\r\nPasswords can be changed at any ti" +
                "me using Webmail or the Control Panel\r\n\r\n\r\n=================================\r\nCo" +
                "ntrol Panel\r\n=================================\r\n\r\nIf you need to change the deta" +
                "ils of your account, you can easily do this using the Control Panel \r\n\r\n\r\n======" +
                "===========================\r\nSupport\r\n=================================\r\n\r\nYou h" +
                "ave 2 options, email or use the web interface at " },
            new UserSetting() { UserId = 1, SettingsName = "ExchangePolicy", PropertyName = "MailboxPasswordPolicy", PropertyValue = "True;8;20;0;2;0;True" },
            new UserSetting() { UserId = 1, SettingsName = "FtpPolicy", PropertyName = "UserNamePolicy", PropertyValue = "True;-;1;20;;;" },
            new UserSetting() { UserId = 1, SettingsName = "FtpPolicy", PropertyName = "UserPasswordPolicy", PropertyValue = "True;5;20;0;1;0;True" },
            new UserSetting() { UserId = 1, SettingsName = "MailPolicy", PropertyName = "AccountNamePolicy", PropertyValue = "True;;1;50;;;" },
            new UserSetting() { UserId = 1, SettingsName = "MailPolicy", PropertyName = "AccountPasswordPolicy", PropertyValue = "True;5;20;0;1;0;False;;0;;;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "MailPolicy", PropertyName = "CatchAllName", PropertyValue = "mail" },
            new UserSetting() { UserId = 1, SettingsName = "MariaDBPolicy", PropertyName = "DatabaseNamePolicy", PropertyValue = "True;;1;40;;;" },
            new UserSetting() { UserId = 1, SettingsName = "MariaDBPolicy", PropertyName = "UserNamePolicy", PropertyValue = "True;;1;16;;;" },
            new UserSetting() { UserId = 1, SettingsName = "MariaDBPolicy", PropertyName = "UserPasswordPolicy", PropertyValue = "True;5;20;0;1;0;False;;0;;;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "MsSqlPolicy", PropertyName = "DatabaseNamePolicy", PropertyValue = "True;-;1;120;;;" },
            new UserSetting() { UserId = 1, SettingsName = "MsSqlPolicy", PropertyName = "UserNamePolicy", PropertyValue = "True;-;1;120;;;" },
            new UserSetting() { UserId = 1, SettingsName = "MsSqlPolicy", PropertyName = "UserPasswordPolicy", PropertyValue = "True;5;20;0;1;0;True;;0;0;0;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "MySqlPolicy", PropertyName = "DatabaseNamePolicy", PropertyValue = "True;;1;40;;;" },
            new UserSetting() { UserId = 1, SettingsName = "MySqlPolicy", PropertyName = "UserNamePolicy", PropertyValue = "True;;1;16;;;" },
            new UserSetting() { UserId = 1, SettingsName = "MySqlPolicy", PropertyName = "UserPasswordPolicy", PropertyValue = "True;5;20;0;1;0;False;;0;0;0;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "OrganizationUserPasswordRequestLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "OrganizationUserPasswordRequestLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password request" +
                " notification</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-colo" +
                "r: ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; " +
                "font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: sol" +
                "id 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { " +
                "font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; " +
                "color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { fo" +
                "nt-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px " +
                "##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; fon" +
                "t-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary " +
                "TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em;" +
                " font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight" +
                ": normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class=\"H" +
                "eader\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password request notification</h1>\r\n" +
                "\r\n<ad:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nYo" +
                "ur account have been created. In order to create a password for your account, pl" +
                "ease follow next link:\r\n</p>\r\n\r\n<a href=\"#passwordResetLink#\" target=\"_blank\">#p" +
                "asswordResetLink#</a>\r\n\r\n<p>\r\nIf you have any questions regarding your hosting a" +
                "ccount, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nB" +
                "est regards\r\n</p>\r\n</div>\r\n</body>" },
            new UserSetting() { UserId = 1, SettingsName = "OrganizationUserPasswordRequestLetter", PropertyName = "LogoUrl", PropertyValue = "" },
            new UserSetting() { UserId = 1, SettingsName = "OrganizationUserPasswordRequestLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "OrganizationUserPasswordRequestLetter", PropertyName = "SMSBody", PropertyValue = "\r\nUser have been created. Password request url:\r\n#passwordResetLink#" },
            new UserSetting() { UserId = 1, SettingsName = "OrganizationUserPasswordRequestLetter", PropertyName = "Subject", PropertyValue = "Password request notification" },
            new UserSetting() { UserId = 1, SettingsName = "OrganizationUserPasswordRequestLetter", PropertyName = "TextBody", PropertyValue = "=========================================\r\n   Password request notification\r\n===" +
                "======================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.Fir" +
                "stName#,\r\n</ad:if>\r\n\r\nYour account have been created. In order to create a passw" +
                "ord for your account, please follow next link:\r\n\r\n#passwordResetLink#\r\n\r\nIf you " +
                "have any questions regarding your hosting account, feel free to contact our supp" +
                "ort department at any time.\r\n\r\nBest regards" },
            new UserSetting() { UserId = 1, SettingsName = "OsPolicy", PropertyName = "DsnNamePolicy", PropertyValue = "True;-;2;40;;;" },
            new UserSetting() { UserId = 1, SettingsName = "PackageSummaryLetter", PropertyName = "CC", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "PackageSummaryLetter", PropertyName = "EnableLetter", PropertyValue = "True" },
            new UserSetting() { UserId = 1, SettingsName = "PackageSummaryLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "PackageSummaryLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "PackageSummaryLetter", PropertyName = "Subject", PropertyValue = "\"#space.Package.PackageName#\" <ad:if test=\"#Signup#\">hosting space has been crea" +
                "ted for<ad:else>hosting space summary for</ad:if> #user.FirstName# #user.LastNam" +
                "e#" },
            new UserSetting() { UserId = 1, SettingsName = "PasswordReminderLetter", PropertyName = "CC", PropertyValue = "" },
            new UserSetting() { UserId = 1, SettingsName = "PasswordReminderLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "PasswordReminderLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Account Summary " +
                "Information</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-color:" +
                " ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; fo" +
                "nt-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid" +
                " 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { fo" +
                "nt-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; co" +
                "lor: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font" +
                "-size: 1.3em; color: ##1F4978; }\r\n        .Summary TABLE { border: solid 1px ##e" +
                "5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-s" +
                "ize: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD " +
                "{ padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; fo" +
                "nt-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: n" +
                "ormal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\">" +
                "</a>\r\n<div class=\"Header\">\r\n\tHosting Account Information\r\n</div>\r\n\r\n<p>\r\nHello #" +
                "user.FirstName#,\r\n</p>\r\n\r\n<p>\r\nPlease, find below details of your control panel " +
                "account. The one time password was generated for you. You should change the pass" +
                "word after login. \r\n</p>\r\n\r\n<h1>Control Panel URL</h1>\r\n<table>\r\n    <thead>\r\n  " +
                "      <tr>\r\n            <th>Control Panel URL</th>\r\n            <th>Username</th" +
                ">\r\n            <th>One Time Password</th>\r\n        </tr>\r\n    </thead>\r\n    <tbo" +
                "dy>\r\n        <tr>\r\n            <td><a href=\"http://panel.HostingCompany.com\">htt" +
                "p://panel.HostingCompany.com</a></td>\r\n            <td>#user.Username#</td>\r\n   " +
                "         <td>#user.Password#</td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n\r\n\r\n<p" +
                ">\r\nIf you have any questions regarding your hosting account, feel free to contac" +
                "t our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards,<br />\r\nSolidCP" +
                ".<br />\r\nWeb Site: <a href=\"https://solidcp.com\">https://solidcp.com</a><br />\r\n" +
                "E-Mail: <a href=\"mailto:support@solidcp.com\">support@solidcp.com</a>\r\n</p>\r\n\r\n</" +
                "div>\r\n</body>\r\n</html>" },
            new UserSetting() { UserId = 1, SettingsName = "PasswordReminderLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "PasswordReminderLetter", PropertyName = "Subject", PropertyValue = "Password reminder for #user.FirstName# #user.LastName#" },
            new UserSetting() { UserId = 1, SettingsName = "PasswordReminderLetter", PropertyName = "TextBody", PropertyValue = "=================================\r\n   Hosting Account Information\r\n=============" +
                "====================\r\n\r\nHello #user.FirstName#,\r\n\r\nPlease, find below details of" +
                " your control panel account. The one time password was generated for you. You sh" +
                "ould change the password after login.\r\n\r\nControl Panel URL: https://panel.solidc" +
                "p.com\r\nUsername: #user.Username#\r\nOne Time Password: #user.Password#\r\n\r\nIf you h" +
                "ave any questions regarding your hosting account, feel free to contact our suppo" +
                "rt department at any time.\r\n\r\nBest regards,\r\nSolidCP.\r\nWeb Site: https://solidcp" +
                ".com\"\r\nE-Mail: support@solidcp.com" },
            new UserSetting() { UserId = 1, SettingsName = "RDSSetupLetter", PropertyName = "CC", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "RDSSetupLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "RDSSetupLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>RDS Setup Inform" +
                "ation</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-color: ##fff" +
                "fff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; font-siz" +
                "e: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px #" +
                "#86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-fam" +
                "ily: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: #" +
                "#1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size:" +
                " 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px ##e5e5e5" +
                "; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: " +
                "8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { pad" +
                "ding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-we" +
                "ight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal" +
                "; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\"></a>\r" +
                "\n<div class=\"Header\">\r\n\tRDS Setup Information\r\n</div>\r\n</div>\r\n</body>" },
            new UserSetting() { UserId = 1, SettingsName = "RDSSetupLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "RDSSetupLetter", PropertyName = "Subject", PropertyValue = "RDS setup" },
            new UserSetting() { UserId = 1, SettingsName = "RDSSetupLetter", PropertyName = "TextBody", PropertyValue = "=================================\r\n   RDS Setup Information\r\n===================" +
                "==============\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nPlea" +
                "se, find below RDS setup instructions.\r\n\r\nIf you have any questions, feel free t" +
                "o contact our support department at any time.\r\n\r\nBest regards" },
            new UserSetting() { UserId = 1, SettingsName = "SharePointPolicy", PropertyName = "GroupNamePolicy", PropertyValue = "True;-;1;20;;;" },
            new UserSetting() { UserId = 1, SettingsName = "SharePointPolicy", PropertyName = "UserNamePolicy", PropertyValue = "True;-;1;20;;;" },
            new UserSetting() { UserId = 1, SettingsName = "SharePointPolicy", PropertyName = "UserPasswordPolicy", PropertyValue = "True;5;20;0;1;0;True;;0;;;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "SolidCPPolicy", PropertyName = "DemoMessage", PropertyValue = "When user account is in demo mode the majority of operations are\r\ndisabled, espe" +
                "cially those ones that modify or delete records.\r\nYou are welcome to ask your qu" +
                "estions or place comments about\r\nthis demo on  <a href=\"http://forum.SolidCP.net" +
                "\"\r\ntarget=\"_blank\">SolidCP  Support Forum</a>" },
            new UserSetting() { UserId = 1, SettingsName = "SolidCPPolicy", PropertyName = "ForbiddenIP", PropertyValue = "" },
            new UserSetting() { UserId = 1, SettingsName = "SolidCPPolicy", PropertyName = "PasswordPolicy", PropertyValue = "True;6;20;0;1;0;True;;0;;;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordExpirationLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordExpirationLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password expirat" +
                "ion notification</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-c" +
                "olor: ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10p" +
                "x; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: " +
                "solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary" +
                " { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7e" +
                "m; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 {" +
                " font-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1" +
                "px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; " +
                "font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summa" +
                "ry TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1" +
                "em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-wei" +
                "ght: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class" +
                "=\"Header\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password expiration notification<" +
                "/h1>\r\n\r\n<ad:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<" +
                "p>\r\nYour password expiration date is #user.PasswordExpirationDateTime#. You can " +
                "reset your own password by visiting the following page:\r\n</p>\r\n\r\n<a href=\"#passw" +
                "ordResetLink#\" target=\"_blank\">#passwordResetLink#</a>\r\n\r\n\r\n<p>\r\nIf you have any" +
                " questions regarding your hosting account, feel free to contact our support depa" +
                "rtment at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p>\r\n</div>\r\n</body>" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordExpirationLetter", PropertyName = "LogoUrl", PropertyValue = "" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordExpirationLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordExpirationLetter", PropertyName = "Subject", PropertyValue = "Password expiration notification" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordExpirationLetter", PropertyName = "TextBody", PropertyValue = "=========================================\r\n   Password expiration notification\r\n" +
                "=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user." +
                "FirstName#,\r\n</ad:if>\r\n\r\nYour password expiration date is #user.PasswordExpirati" +
                "onDateTime#. You can reset your own password by visiting the following page:\r\n\r\n" +
                "#passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account," +
                " feel free to contact our support department at any time.\r\n\r\nBest regards" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password reset n" +
                "otification</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-color:" +
                " ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; fo" +
                "nt-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid" +
                " 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { fo" +
                "nt-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; co" +
                "lor: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font" +
                "-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px ##" +
                "e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-" +
                "size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD" +
                " { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; f" +
                "ont-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: " +
                "normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class=\"Hea" +
                "der\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password reset notification</h1>\r\n\r\n<a" +
                "d:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nWe rec" +
                "eived a request to reset the password for your account. If you made this request" +
                ", click the link below. If you did not make this request, you can ignore this em" +
                "ail.\r\n</p>\r\n\r\n<a href=\"#passwordResetLink#\" target=\"_blank\">#passwordResetLink#<" +
                "/a>\r\n\r\n\r\n<p>\r\nIf you have any questions regarding your hosting account, feel fre" +
                "e to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p" +
                ">\r\n</div>\r\n</body>" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetLetter", PropertyName = "LogoUrl", PropertyValue = "" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetLetter", PropertyName = "PasswordResetLinkSmsBody", PropertyValue = "Password reset link:\r\n#passwordResetLink#\r\n" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetLetter", PropertyName = "Subject", PropertyValue = "Password reset notification" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetLetter", PropertyName = "TextBody", PropertyValue = "=========================================\r\n   Password reset notification\r\n=====" +
                "====================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.First" +
                "Name#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account" +
                ". If you made this request, click the link below. If you did not make this reque" +
                "st, you can ignore this email.\r\n\r\n#passwordResetLink#\r\n\r\nIf you have any questio" +
                "ns regarding your hosting account, feel free to contact our support department a" +
                "t any time.\r\n\r\nBest regards" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetPincodeLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetPincodeLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password reset n" +
                "otification</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-color:" +
                " ##ffffff; padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; fo" +
                "nt-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid" +
                " 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { fo" +
                "nt-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; co" +
                "lor: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font" +
                "-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px ##" +
                "e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-" +
                "size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD" +
                " { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; f" +
                "ont-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: " +
                "normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class=\"Hea" +
                "der\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password reset notification</h1>\r\n\r\n<a" +
                "d:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nWe rec" +
                "eived a request to reset the password for your account. Your password reset pinc" +
                "ode:\r\n</p>\r\n\r\n#passwordResetPincode#\r\n\r\n<p>\r\nIf you have any questions regarding" +
                " your hosting account, feel free to contact our support department at any time.\r" +
                "\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p>\r\n</div>\r\n</body>" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetPincodeLetter", PropertyName = "LogoUrl", PropertyValue = "" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetPincodeLetter", PropertyName = "PasswordResetPincodeSmsBody", PropertyValue = "\r\nYour password reset pincode:\r\n#passwordResetPincode#" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetPincodeLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetPincodeLetter", PropertyName = "Subject", PropertyValue = "Password reset notification" },
            new UserSetting() { UserId = 1, SettingsName = "UserPasswordResetPincodeLetter", PropertyName = "TextBody", PropertyValue = "=========================================\r\n   Password reset notification\r\n=====" +
                "====================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.First" +
                "Name#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account" +
                ". Your password reset pincode:\r\n\r\n#passwordResetPincode#\r\n\r\nIf you have any ques" +
                "tions regarding your hosting account, feel free to contact our support departmen" +
                "t at any time.\r\n\r\nBest regards" },
            new UserSetting() { UserId = 1, SettingsName = "VerificationCodeLetter", PropertyName = "CC", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "VerificationCodeLetter", PropertyName = "From", PropertyValue = "support@HostingCompany.com" },
            new UserSetting() { UserId = 1, SettingsName = "VerificationCodeLetter", PropertyName = "HtmlBody", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Verification cod" +
                "e</title>\r\n    <style type=\"text/css\">\r\n\t\t.Summary { background-color: ##ffffff;" +
                " padding: 5px; }\r\n\t\t.Summary .Header { padding: 10px 0px 10px 10px; font-size: 1" +
                "6pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B" +
                "9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family:" +
                " Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4" +
                "978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3" +
                "em; color: ##1F4978; }\r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n" +
                "        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; " +
                "font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding:" +
                " 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight:" +
                " bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n" +
                "    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\"></a>\r\n<div" +
                " class=\"Header\">\r\n\tVerification code\r\n</div>\r\n\r\n<p>\r\nHello #user.FirstName#,\r\n</" +
                "p>\r\n\r\n<p>\r\nto complete the sign in, enter the verification code on the device. \r" +
                "\n</p>\r\n\r\n<table>\r\n    <thead>\r\n        <tr>\r\n            <th>Verification code</" +
                "th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n            <td>#ve" +
                "rificationCode#</td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n\r\n<p>\r\nBest regards" +
                ",<br />\r\n\r\n</p>\r\n\r\n</div>\r\n</body>\r\n</html>" },
            new UserSetting() { UserId = 1, SettingsName = "VerificationCodeLetter", PropertyName = "Priority", PropertyValue = "Normal" },
            new UserSetting() { UserId = 1, SettingsName = "VerificationCodeLetter", PropertyName = "Subject", PropertyValue = "Verification code" },
            new UserSetting() { UserId = 1, SettingsName = "VerificationCodeLetter", PropertyName = "TextBody", PropertyValue = "=================================\r\n   Verification code\r\n=======================" +
                "==========\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nto compl" +
                "ete the sign in, enter the verification code on the device.\r\n\r\nVerification code" +
                "\r\n#verificationCode#\r\n\r\nBest regards,\r\n" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "AddParkingPage", PropertyValue = "True" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "AddRandomDomainString", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "AnonymousAccountPolicy", PropertyValue = "True;;5;20;;_web;" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "AspInstalled", PropertyValue = "True" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "AspNetInstalled", PropertyValue = "2" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "CgiBinInstalled", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "DefaultDocuments", PropertyValue = "Default.htm,Default.asp,index.htm,Default.aspx" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableAnonymousAccess", PropertyValue = "True" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableBasicAuthentication", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableDedicatedPool", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableDirectoryBrowsing", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableParentPaths", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableParkingPageTokens", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableWindowsAuthentication", PropertyValue = "True" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "EnableWritePermissions", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "FrontPageAccountPolicy", PropertyValue = "True;;1;20;;;" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "FrontPagePasswordPolicy", PropertyValue = "True;5;20;0;1;0;False;;0;0;0;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "ParkingPageContent", PropertyValue = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>The web site is " +
                "under construction</title>\r\n<style type=\"text/css\">\r\n\tH1 { font-size: 16pt; marg" +
                "in-bottom: 4px; }\r\n\tH2 { font-size: 14pt; margin-bottom: 4px; font-weight: norma" +
                "l; }\r\n</style>\r\n</head>\r\n<body>\r\n<div id=\"PageOutline\">\r\n\t<h1>This web site has " +
                "just been created from <a href=\"https://www.SolidCP.com\">SolidCP </a> and it is " +
                "still under construction.</h1>\r\n\t<h2>The web site is hosted by <a href=\"https://" +
                "solidcp.com\">SolidCP</a>.</h2>\r\n</div>\r\n</body>\r\n</html>" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "ParkingPageName", PropertyValue = "default.aspx" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "PerlInstalled", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "PhpInstalled", PropertyValue = "" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "PublishingProfile", PropertyValue = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<publishData>\r\n<ad:if test=\"#WebSite.Web" +
                "DeploySitePublishingEnabled#\">\r\n\t<publishProfile\r\n\t\tprofileName=\"#WebSite.Name# " +
                "- Web Deploy\"\r\n\t\tpublishMethod=\"MSDeploy\"\r\n\t\tpublishUrl=\"#WebSite[\"WmSvcServiceU" +
                "rl\"]#:#WebSite[\"WmSvcServicePort\"]#\"\r\n\t\tmsdeploySite=\"#WebSite.Name#\"\r\n\t\tuserNam" +
                "e=\"#WebSite.WebDeployPublishingAccount#\"\r\n\t\tuserPWD=\"#WebSite.WebDeployPublishin" +
                "gPassword#\"\r\n\t\tdestinationAppUrl=\"http://#WebSite.Name#/\"\r\n\t\t<ad:if test=\"#Not(I" +
                "sNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#\">SQLServerDBConnectionString=\"" +
                "server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser" +
                ".Name#;pwd=#MsSqlUser.Password#\"</ad:if>\r\n\t\t<ad:if test=\"#Not(IsNull(MySqlDataba" +
                "se)) and Not(IsNull(MySqlUser))#\">mySQLDBConnectionString=\"server=#MySqlAddress#" +
                ";database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#\"</a" +
                "d:if>\r\n\t\t<ad:if test=\"#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))" +
                "#\">MariaDBDBConnectionString=\"server=#MariaDBAddress#;database=#MariaDBDatabase." +
                "Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#\"</ad:if>\r\n\t\thostingProvi" +
                "derForumLink=\"https://solidcp.com/support\"\r\n\t\tcontrolPanelLink=\"https://panel.so" +
                "lidcp.com/\"\r\n\t/>\r\n</ad:if>\r\n<ad:if test=\"#IsDefined(\"FtpAccount\")#\">\r\n\t<publishP" +
                "rofile\r\n\t\tprofileName=\"#WebSite.Name# - FTP\"\r\n\t\tpublishMethod=\"FTP\"\r\n\t\tpublishUr" +
                "l=\"ftp://#FtpServiceAddress#\"\r\n\t\tftpPassiveMode=\"True\"\r\n\t\tuserName=\"#FtpAccount." +
                "Name#\"\r\n\t\tuserPWD=\"#FtpAccount.Password#\"\r\n\t\tdestinationAppUrl=\"http://#WebSite." +
                "Name#/\"\r\n\t\t<ad:if test=\"#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#\"" +
                ">SQLServerDBConnectionString=\"server=#MsSqlServerExternalAddress#;database=#MsSq" +
                "lDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#\"</ad:if>\r\n\t\t<ad:if" +
                " test=\"#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#\">mySQLDBConnectio" +
                "nString=\"server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name" +
                "#;pwd=#MySqlUser.Password#\"</ad:if>\r\n\t\t<ad:if test=\"#Not(IsNull(MariaDBDatabase)" +
                ") and Not(IsNull(MariaDBUser))#\">MariaDBDBConnectionString=\"server=#MariaDBAddre" +
                "ss#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Pass" +
                "word#\"</ad:if>\r\n\t\thostingProviderForumLink=\"https://solidcp.com/support\"\r\n\t\tcont" +
                "rolPanelLink=\"https://panel.solidcp.com/\"\r\n    />\r\n</ad:if>\r\n</publishData>\r\n\r\n<" +
                "!--\r\nControl Panel:\r\nUsername: #User.Username#\r\nPassword: #User.Password#\r\n\r\nTec" +
                "hnical Contact:\r\nsupport@solidcp.com\r\n-->" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "PythonInstalled", PropertyValue = "False" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "SecuredGroupNamePolicy", PropertyValue = "True;;1;20;;;" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "SecuredUserNamePolicy", PropertyValue = "True;;1;20;;;" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "SecuredUserPasswordPolicy", PropertyValue = "True;5;20;0;1;0;False;;0;0;0;False;False;0;" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "VirtDirNamePolicy", PropertyValue = "True;-;3;50;;;" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "WebDataFolder", PropertyValue = "\\[DOMAIN_NAME]\\data" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "WebLogsFolder", PropertyValue = "\\[DOMAIN_NAME]\\logs" },
            new UserSetting() { UserId = 1, SettingsName = "WebPolicy", PropertyName = "WebRootFolder", PropertyValue = "\\[DOMAIN_NAME]\\wwwroot" }
        );
#endregion
    }
#endif
}
