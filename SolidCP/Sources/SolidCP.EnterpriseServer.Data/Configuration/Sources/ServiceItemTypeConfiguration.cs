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

public partial class ServiceItemTypeConfiguration: EntityTypeConfiguration<ServiceItemType>
{
    public override void Configure() {
        Property(e => e.ItemTypeId).ValueGeneratedNever();
        Property(e => e.Backupable).HasDefaultValue(true);
        Property(e => e.Importable).HasDefaultValue(true);
        Property(e => e.TypeOrder).HasDefaultValue(1);

        HasOne(d => d.Group).WithMany(p => p.ServiceItemTypes).HasConstraintName("FK_ServiceItemTypes_ResourceGroups");

        #region Seed Data
        HasData(() => new ServiceItemType[] {
            new ServiceItemType() { ItemTypeId = 2, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "HomeFolder", Disposable = true,
                GroupId = 1, Searchable = false, Suspendable = false, TypeName = "SolidCP.Providers.OS.HomeFolder, SolidCP.Providers.Base", TypeOrder = 15 },
            new ServiceItemType() { ItemTypeId = 5, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2000Database", Disposable = true,
                GroupId = 5, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 9 },
            new ServiceItemType() { ItemTypeId = 6, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2000User", Disposable = true,
                GroupId = 5, Importable = true, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 10 },
            new ServiceItemType() { ItemTypeId = 7, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MySQL4Database", Disposable = true,
                GroupId = 6, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 13 },
            new ServiceItemType() { ItemTypeId = 8, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MySQL4User", Disposable = true,
                GroupId = 6, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 14 },
            new ServiceItemType() { ItemTypeId = 9, Backupable = true, CalculateBandwidth = true, CalculateDiskspace = false, DisplayName = "FTPAccount", Disposable = true,
                GroupId = 3, Importable = true, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.FTP.FtpAccount, SolidCP.Providers.Base", TypeOrder = 3 },
            new ServiceItemType() { ItemTypeId = 10, Backupable = true, CalculateBandwidth = true, CalculateDiskspace = true, DisplayName = "WebSite", Disposable = true,
                GroupId = 2, Importable = true, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Web.WebSite, SolidCP.Providers.Base", TypeOrder = 2 },
            new ServiceItemType() { ItemTypeId = 11, Backupable = true, CalculateBandwidth = true, CalculateDiskspace = false, DisplayName = "MailDomain", Disposable = true,
                GroupId = 4, Importable = true, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Mail.MailDomain, SolidCP.Providers.Base", TypeOrder = 8 },
            new ServiceItemType() { ItemTypeId = 12, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "DNSZone", Disposable = true,
                GroupId = 7, Importable = true, Searchable = false, Suspendable = true, TypeName = "SolidCP.Providers.DNS.DnsZone, SolidCP.Providers.Base",  },
            new ServiceItemType() { ItemTypeId = 13, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "Domain", Disposable = false, GroupId = 1,
                Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.OS.Domain, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 14, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "StatisticsSite", Disposable = true,
                GroupId = 8, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Statistics.StatsSite, SolidCP.Providers.Base", TypeOrder = 17 },
            new ServiceItemType() { ItemTypeId = 15, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MailAccount", Disposable = false, GroupId = 4,
                Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Mail.MailAccount, SolidCP.Providers.Base", TypeOrder = 4 },
            new ServiceItemType() { ItemTypeId = 16, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MailAlias", Disposable = false, GroupId = 4,
                Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Mail.MailAlias, SolidCP.Providers.Base", TypeOrder = 5 },
            new ServiceItemType() { ItemTypeId = 17, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MailList", Disposable = false, GroupId = 4,
                Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Mail.MailList, SolidCP.Providers.Base", TypeOrder = 7 },
            new ServiceItemType() { ItemTypeId = 18, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MailGroup", Disposable = false, GroupId = 4,
                Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Mail.MailGroup, SolidCP.Providers.Base", TypeOrder = 6 },
            new ServiceItemType() { ItemTypeId = 20, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "ODBCDSN", Disposable = true,
                GroupId = 1, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.OS.SystemDSN, SolidCP.Providers.Base", TypeOrder = 22 },
            new ServiceItemType() { ItemTypeId = 21, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2005Database", Disposable = true,
                GroupId = 10, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 11 },
            new ServiceItemType() { ItemTypeId = 22, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2005User", Disposable = true,
                GroupId = 10, Importable = true, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 12 },
            new ServiceItemType() { ItemTypeId = 23, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MySQL5Database", Disposable = true,
                GroupId = 11, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 15 },
            new ServiceItemType() { ItemTypeId = 24, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MySQL5User", Disposable = true,
                GroupId = 11, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 16 },
            new ServiceItemType() { ItemTypeId = 25, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "SharedSSLFolder", Disposable = true,
                GroupId = 2, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Web.SharedSSLFolder, SolidCP.Providers.Base", TypeOrder = 21 },
            new ServiceItemType() { ItemTypeId = 28, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "SecondaryDNSZone", Disposable = true,
                GroupId = 7, Searchable = false, Suspendable = true, TypeName = "SolidCP.Providers.DNS.SecondaryDnsZone, SolidCP.Providers.Base",  },
            new ServiceItemType() { ItemTypeId = 29, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "Organization", Disposable = true,
                GroupId = 13, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.HostedSolution.Organization, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 30, Backupable = true, DisplayName = "OrganizationDomain", GroupId = 13, TypeName = "SolidCP.Providers.HostedSolution.OrganizationDomain, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 31, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2008Database", Disposable = true,
                GroupId = 22, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 32, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2008User", Disposable = true,
                GroupId = 22, Importable = true, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 33, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VirtualMachine", Disposable = true, GroupId = 30,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 34, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VirtualSwitch", Disposable = true, GroupId = 30,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base", TypeOrder = 2 },
            new ServiceItemType() { ItemTypeId = 35, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VMInfo", Disposable = true, GroupId = 40,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VMInfo, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 36, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VirtualSwitch", Disposable = true, GroupId = 40,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base", TypeOrder = 2 },
            new ServiceItemType() { ItemTypeId = 37, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2012Database", Disposable = true,
                GroupId = 23, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 38, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2012User", Disposable = true,
                GroupId = 23, Importable = true, Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 39, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2014Database", Disposable = true,
                GroupId = 46, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 40, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2014User", Disposable = true,
                GroupId = 46, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 41, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VirtualMachine", Disposable = true, GroupId = 33,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 42, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VirtualSwitch", Disposable = true, GroupId = 33,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base", TypeOrder = 2 },
            new ServiceItemType() { ItemTypeId = 71, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2016Database", Disposable = true,
                GroupId = 71, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 72, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2016User", Disposable = true,
                GroupId = 71, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 73, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2017Database", Disposable = true,
                GroupId = 72, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 74, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2017User", Disposable = true,
                GroupId = 72, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 75, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MySQL8Database", Disposable = true,
                GroupId = 90, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 18 },
            new ServiceItemType() { ItemTypeId = 76, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MySQL8User", Disposable = true,
                GroupId = 90, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 19 },
            new ServiceItemType() { ItemTypeId = 77, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2019Database", Disposable = true,
                GroupId = 74, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 78, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2019User", Disposable = true,
                GroupId = 74, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 79, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MsSQL2022Database", Disposable = true,
                GroupId = 75, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 80, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MsSQL2022User", Disposable = true,
                GroupId = 75, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 143, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VirtualMachine", Disposable = true, GroupId = 167,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 144, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "VirtualSwitch", Disposable = true, GroupId = 167,
                Searchable = true, Suspendable = true, TypeName = "SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base", TypeOrder = 2 },
            new ServiceItemType() { ItemTypeId = 200, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "SharePointFoundationSiteCollection", Disposable = true,
                GroupId = 20, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.SharePoint.SharePointSiteCollection, SolidCP.Providers.Base", TypeOrder = 25 },
            new ServiceItemType() { ItemTypeId = 202, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "MariaDBDatabase", Disposable = true,
                GroupId = 50, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 203, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = false, DisplayName = "MariaDBUser", Disposable = true,
                GroupId = 50, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", TypeOrder = 1 },
            new ServiceItemType() { ItemTypeId = 204, Backupable = true, CalculateBandwidth = false, CalculateDiskspace = true, DisplayName = "SharePointEnterpriseSiteCollection", Disposable = true,
                GroupId = 73, Importable = true, Searchable = true, Suspendable = false, TypeName = "SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection, SolidCP.Provide" +
                "rs.Base", TypeOrder = 100 }
        });
        #endregion

    }
}
