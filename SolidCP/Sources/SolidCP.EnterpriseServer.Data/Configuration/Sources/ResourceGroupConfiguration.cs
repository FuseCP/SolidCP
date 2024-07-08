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

public partial class ResourceGroupConfiguration: EntityTypeConfiguration<ResourceGroup>
{
    public override void Configure() {
        Property(e => e.GroupId).ValueGeneratedNever();
        Property(e => e.GroupOrder).HasDefaultValue(1);

        #region Seed Data
        HasData(() => new ResourceGroup[] {
            new ResourceGroup() { GroupId = 1, GroupController = "SolidCP.EnterpriseServer.OperatingSystemController", GroupName = "OS", GroupOrder = 1, ShowGroup = true },
            new ResourceGroup() { GroupId = 2, GroupController = "SolidCP.EnterpriseServer.WebServerController", GroupName = "Web", GroupOrder = 2, ShowGroup = true },
            new ResourceGroup() { GroupId = 3, GroupController = "SolidCP.EnterpriseServer.FtpServerController", GroupName = "FTP", GroupOrder = 3, ShowGroup = true },
            new ResourceGroup() { GroupId = 4, GroupController = "SolidCP.EnterpriseServer.MailServerController", GroupName = "Mail", GroupOrder = 4, ShowGroup = true },
            new ResourceGroup() { GroupId = 5, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2000", GroupOrder = 7, ShowGroup = true },
            new ResourceGroup() { GroupId = 6, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MySQL4", GroupOrder = 11, ShowGroup = true },
            new ResourceGroup() { GroupId = 7, GroupController = "SolidCP.EnterpriseServer.DnsServerController", GroupName = "DNS", GroupOrder = 17, ShowGroup = true },
            new ResourceGroup() { GroupId = 8, GroupController = "SolidCP.EnterpriseServer.StatisticsServerController", GroupName = "Statistics", GroupOrder = 18, ShowGroup = true },
            new ResourceGroup() { GroupId = 10, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2005", GroupOrder = 8, ShowGroup = true },
            new ResourceGroup() { GroupId = 11, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MySQL5", GroupOrder = 12, ShowGroup = true },
            new ResourceGroup() { GroupId = 12, GroupName = "Exchange", GroupOrder = 5, ShowGroup = true },
            new ResourceGroup() { GroupId = 13, GroupName = "Hosted Organizations", GroupOrder = 6, ShowGroup = true },
            new ResourceGroup() { GroupId = 20, GroupController = "SolidCP.EnterpriseServer.HostedSharePointServerController", GroupName = "Sharepoint Foundation Server", GroupOrder = 14, ShowGroup = true },
            new ResourceGroup() { GroupId = 21, GroupName = "Hosted CRM", GroupOrder = 16, ShowGroup = true },
            new ResourceGroup() { GroupId = 22, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2008", GroupOrder = 9, ShowGroup = true },
            new ResourceGroup() { GroupId = 23, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2012", GroupOrder = 10, ShowGroup = true },
            new ResourceGroup() { GroupId = 24, GroupName = "Hosted CRM2013", GroupOrder = 16, ShowGroup = true },
            new ResourceGroup() { GroupId = 30, GroupName = "VPS", GroupOrder = 19, ShowGroup = true },
            new ResourceGroup() { GroupId = 31, GroupName = "BlackBerry", GroupOrder = 21, ShowGroup = true },
            new ResourceGroup() { GroupId = 32, GroupName = "OCS", GroupOrder = 22, ShowGroup = true },
            new ResourceGroup() { GroupId = 33, GroupName = "VPS2012", GroupOrder = 20, ShowGroup = true },
            new ResourceGroup() { GroupId = 40, GroupName = "VPSForPC", GroupOrder = 20, ShowGroup = true },
            new ResourceGroup() { GroupId = 41, GroupName = "Lync", GroupOrder = 24, ShowGroup = true },
            new ResourceGroup() { GroupId = 42, GroupController = "SolidCP.EnterpriseServer.HeliconZooController", GroupName = "HeliconZoo", GroupOrder = 2, ShowGroup = true },
            new ResourceGroup() { GroupId = 44, GroupController = "SolidCP.EnterpriseServer.EnterpriseStorageController", GroupName = "EnterpriseStorage", GroupOrder = 26, ShowGroup = true },
            new ResourceGroup() { GroupId = 45, GroupName = "RDS", GroupOrder = 27, ShowGroup = true },
            new ResourceGroup() { GroupId = 46, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2014", GroupOrder = 10, ShowGroup = true },
            new ResourceGroup() { GroupId = 47, GroupName = "Service Levels", GroupOrder = 2, ShowGroup = true },
            new ResourceGroup() { GroupId = 49, GroupController = "SolidCP.EnterpriseServer.StorageSpacesController", GroupName = "StorageSpaceServices", GroupOrder = 26, ShowGroup = true },
            new ResourceGroup() { GroupId = 50, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MariaDB", GroupOrder = 11, ShowGroup = true },
            new ResourceGroup() { GroupId = 52, GroupName = "SfB", GroupOrder = 26, ShowGroup = true },
            new ResourceGroup() { GroupId = 61, GroupName = "MailFilters", GroupOrder = 5, ShowGroup = true },
            new ResourceGroup() { GroupId = 71, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2016", GroupOrder = 10, ShowGroup = true },
            new ResourceGroup() { GroupId = 72, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2017", GroupOrder = 10, ShowGroup = true },
            new ResourceGroup() { GroupId = 73, GroupController = "SolidCP.EnterpriseServer.HostedSharePointServerEntController", GroupName = "Sharepoint Enterprise Server", GroupOrder = 15, ShowGroup = true },
            new ResourceGroup() { GroupId = 74, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2019", GroupOrder = 10, ShowGroup = true },
            new ResourceGroup() { GroupId = 75, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MsSQL2022", GroupOrder = 10, ShowGroup = true },
            new ResourceGroup() { GroupId = 90, GroupController = "SolidCP.EnterpriseServer.DatabaseServerController", GroupName = "MySQL8", GroupOrder = 12, ShowGroup = true },
            new ResourceGroup() { GroupId = 167, GroupName = "Proxmox", GroupOrder = 20, ShowGroup = true }
        });
        #endregion

    }
}
