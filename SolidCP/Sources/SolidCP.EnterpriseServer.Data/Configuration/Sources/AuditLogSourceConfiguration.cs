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

public partial class AuditLogSourceConfiguration: EntityTypeConfiguration<AuditLogSource>
{
    public override void Configure() {

        #region Seed Data
        HasData(() => new AuditLogSource[] {
            new AuditLogSource() { SourceName = "APP_INSTALLER" },
            new AuditLogSource() { SourceName = "AUTO_DISCOVERY" },
            new AuditLogSource() { SourceName = "BACKUP" },
            new AuditLogSource() { SourceName = "DNS_ZONE" },
            new AuditLogSource() { SourceName = "DOMAIN" },
            new AuditLogSource() { SourceName = "ENTERPRISE_STORAGE" },
            new AuditLogSource() { SourceName = "EXCHANGE" },
            new AuditLogSource() { SourceName = "FILES" },
            new AuditLogSource() { SourceName = "FTP_ACCOUNT" },
            new AuditLogSource() { SourceName = "GLOBAL_DNS" },
            new AuditLogSource() { SourceName = "HOSTING_SPACE" },
            new AuditLogSource() { SourceName = "HOSTING_SPACE_WR" },
            new AuditLogSource() { SourceName = "IMPORT" },
            new AuditLogSource() { SourceName = "IP_ADDRESS" },
            new AuditLogSource() { SourceName = "MAIL_ACCOUNT" },
            new AuditLogSource() { SourceName = "MAIL_DOMAIN" },
            new AuditLogSource() { SourceName = "MAIL_FORWARDING" },
            new AuditLogSource() { SourceName = "MAIL_GROUP" },
            new AuditLogSource() { SourceName = "MAIL_LIST" },
            new AuditLogSource() { SourceName = "OCS" },
            new AuditLogSource() { SourceName = "ODBC_DSN" },
            new AuditLogSource() { SourceName = "ORGANIZATION" },
            new AuditLogSource() { SourceName = "REMOTE_DESKTOP_SERVICES" },
            new AuditLogSource() { SourceName = "SCHEDULER" },
            new AuditLogSource() { SourceName = "SERVER" },
            new AuditLogSource() { SourceName = "SHAREPOINT" },
            new AuditLogSource() { SourceName = "SPACE" },
            new AuditLogSource() { SourceName = "SQL_DATABASE" },
            new AuditLogSource() { SourceName = "SQL_USER" },
            new AuditLogSource() { SourceName = "STATS_SITE" },
            new AuditLogSource() { SourceName = "STORAGE_SPACES" },
            new AuditLogSource() { SourceName = "USER" },
            new AuditLogSource() { SourceName = "VIRTUAL_SERVER" },
            new AuditLogSource() { SourceName = "VLAN" },
            new AuditLogSource() { SourceName = "VPS" },
            new AuditLogSource() { SourceName = "VPS2012" },
            new AuditLogSource() { SourceName = "WAG_INSTALLER" },
            new AuditLogSource() { SourceName = "WEB_SITE" }
        });
        #endregion

    }
}
