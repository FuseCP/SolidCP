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

public partial class ProviderConfiguration: EntityTypeConfiguration<Provider>
{
    public override void Configure() {
        HasKey(e => e.ProviderId).HasName("PK_ServiceTypes");

#if NetCore
        Property(e => e.ProviderId).ValueGeneratedNever();

        HasOne(d => d.Group).WithMany(p => p.Providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Providers_ResourceGroups");
#else
        HasRequired(d => d.Group).WithMany(p => p.Providers);
#endif

		#region Seed Data
		HasData(() => new Provider[] {
			new Provider() { ProviderId = 1, DisplayName = "Windows Server 2003", EditorControl = "Windows2003", GroupId = 1, ProviderName = "Windows2003", ProviderType = "SolidCP.Providers.OS.Windows2003, SolidCP.Providers.OS.Windows2003" },
			new Provider() { ProviderId = 2, DisplayName = "Internet Information Services 6.0", EditorControl = "IIS60", GroupId = 2, ProviderName = "IIS60", ProviderType = "SolidCP.Providers.Web.IIs60, SolidCP.Providers.Web.IIs60" },
			new Provider() { ProviderId = 3, DisplayName = "Microsoft FTP Server 6.0", EditorControl = "MSFTP60", GroupId = 3, ProviderName = "MSFTP60", ProviderType = "SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs60" },
			new Provider() { ProviderId = 4, DisplayName = "MailEnable Server 1.x - 7.x", EditorControl = "MailEnable", GroupId = 4, ProviderName = "MailEnable", ProviderType = "SolidCP.Providers.Mail.MailEnable, SolidCP.Providers.Mail.MailEnable" },
			new Provider() { ProviderId = 5, DisplayName = "Microsoft SQL Server 2000", EditorControl = "MSSQL", GroupId = 5, ProviderName = "MSSQL", ProviderType = "SolidCP.Providers.Database.MsSqlServer, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 6, DisplayName = "MySQL Server 4.x", EditorControl = "MySQL", GroupId = 6, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 7, DisplayName = "Microsoft DNS Server", EditorControl = "MSDNS", GroupId = 7, ProviderName = "MSDNS", ProviderType = "SolidCP.Providers.DNS.MsDNS, SolidCP.Providers.DNS.MsDNS" },
			new Provider() { ProviderId = 8, DisplayName = "AWStats Statistics Service", EditorControl = "AWStats", GroupId = 8, ProviderName = "AWStats", ProviderType = "SolidCP.Providers.Statistics.AWStats, SolidCP.Providers.Statistics.AWStats" },
			new Provider() { ProviderId = 9, DisplayName = "SimpleDNS Plus 4.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "SolidCP.Providers.DNS.SimpleDNS, SolidCP.Providers.DNS.SimpleDNS" },
			new Provider() { ProviderId = 10, DisplayName = "SmarterStats 3.x", EditorControl = "SmarterStats", GroupId = 8, ProviderName = "SmarterStats", ProviderType = "SolidCP.Providers.Statistics.SmarterStats, SolidCP.Providers.Statistics.SmarterS" +
				"tats" },
			new Provider() { ProviderId = 11, DisplayName = "SmarterMail 2.x", EditorControl = "SmarterMail", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail2, SolidCP.Providers.Mail.SmarterMail2" },
			new Provider() { ProviderId = 12, DisplayName = "Gene6 FTP Server 3.x", EditorControl = "Gene6FTP", GroupId = 3, ProviderName = "Gene6FTP", ProviderType = "SolidCP.Providers.FTP.Gene6, SolidCP.Providers.FTP.Gene6" },
			new Provider() { ProviderId = 13, DisplayName = "Merak Mail Server 8.0.3 - 9.2.x", EditorControl = "Merak", GroupId = 4, ProviderName = "Merak", ProviderType = "SolidCP.Providers.Mail.Merak, SolidCP.Providers.Mail.Merak" },
			new Provider() { ProviderId = 14, DisplayName = "SmarterMail 3.x - 4.x", EditorControl = "SmarterMail", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail3, SolidCP.Providers.Mail.SmarterMail3" },
			new Provider() { ProviderId = 16, DisplayName = "Microsoft SQL Server 2005", EditorControl = "MSSQL", GroupId = 10, ProviderName = "MSSQL", ProviderType = "SolidCP.Providers.Database.MsSqlServer2005, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 17, DisplayName = "MySQL Server 5.0", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer50, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 18, DisplayName = "MDaemon 9.x - 11.x", EditorControl = "MDaemon", GroupId = 4, ProviderName = "MDaemon", ProviderType = "SolidCP.Providers.Mail.MDaemon, SolidCP.Providers.Mail.MDaemon" },
			new Provider() { ProviderId = 19, DisableAutoDiscovery = true, DisplayName = "ArGoSoft Mail Server 1.x", EditorControl = "ArgoMail", GroupId = 4, ProviderName = "ArgoMail",
				ProviderType = "SolidCP.Providers.Mail.ArgoMail, SolidCP.Providers.Mail.ArgoMail" },
			new Provider() { ProviderId = 20, DisplayName = "hMailServer 4.2", EditorControl = "hMailServer", GroupId = 4, ProviderName = "hMailServer", ProviderType = "SolidCP.Providers.Mail.hMailServer, SolidCP.Providers.Mail.hMailServer" },
			new Provider() { ProviderId = 21, DisplayName = "Ability Mail Server 2.x", EditorControl = "AbilityMailServer", GroupId = 4, ProviderName = "AbilityMailServer", ProviderType = "SolidCP.Providers.Mail.AbilityMailServer, SolidCP.Providers.Mail.AbilityMailServ" +
				"er" },
			new Provider() { ProviderId = 22, DisplayName = "hMailServer 4.3", EditorControl = "hMailServer43", GroupId = 4, ProviderName = "hMailServer43", ProviderType = "SolidCP.Providers.Mail.hMailServer43, SolidCP.Providers.Mail.hMailServer43" },
			new Provider() { ProviderId = 24, DisplayName = "ISC BIND 8.x - 9.x", EditorControl = "Bind", GroupId = 7, ProviderName = "Bind", ProviderType = "SolidCP.Providers.DNS.IscBind, SolidCP.Providers.DNS.Bind" },
			new Provider() { ProviderId = 25, DisplayName = "Serv-U FTP 6.x", EditorControl = "ServU", GroupId = 3, ProviderName = "ServU", ProviderType = "SolidCP.Providers.FTP.ServU, SolidCP.Providers.FTP.ServU" },
			new Provider() { ProviderId = 26, DisplayName = "FileZilla FTP Server 0.9", EditorControl = "FileZilla", GroupId = 3, ProviderName = "FileZilla", ProviderType = "SolidCP.Providers.FTP.FileZilla, SolidCP.Providers.FTP.FileZilla" },
			new Provider() { ProviderId = 27, DisplayName = "Hosted Microsoft Exchange Server 2007", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2007", ProviderType = "SolidCP.Providers.HostedSolution.Exchange2007, SolidCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 28, DisplayName = "SimpleDNS Plus 5.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "SolidCP.Providers.DNS.SimpleDNS5, SolidCP.Providers.DNS.SimpleDNS50" },
			new Provider() { ProviderId = 29, DisplayName = "SmarterMail 5.x", EditorControl = "SmarterMail50", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail5, SolidCP.Providers.Mail.SmarterMail5" },
			new Provider() { ProviderId = 30, DisplayName = "MySQL Server 5.1", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer51, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 31, DisplayName = "SmarterStats 4.x", EditorControl = "SmarterStats", GroupId = 8, ProviderName = "SmarterStats", ProviderType = "SolidCP.Providers.Statistics.SmarterStats4, SolidCP.Providers.Statistics.Smarter" +
				"Stats" },
			new Provider() { ProviderId = 32, DisplayName = "Hosted Microsoft Exchange Server 2010", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2010", ProviderType = "SolidCP.Providers.HostedSolution.Exchange2010, SolidCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 55, DisableAutoDiscovery = true, DisplayName = "Nettica DNS", EditorControl = "NetticaDNS", GroupId = 7, ProviderName = "NetticaDNS",
				ProviderType = "SolidCP.Providers.DNS.Nettica, SolidCP.Providers.DNS.Nettica" },
			new Provider() { ProviderId = 56, DisableAutoDiscovery = true, DisplayName = "PowerDNS", EditorControl = "PowerDNS", GroupId = 7, ProviderName = "PowerDNS",
				ProviderType = "SolidCP.Providers.DNS.PowerDNS, SolidCP.Providers.DNS.PowerDNS" },
			new Provider() { ProviderId = 60, DisplayName = "SmarterMail 6.x", EditorControl = "SmarterMail60", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail6, SolidCP.Providers.Mail.SmarterMail6" },
			new Provider() { ProviderId = 61, DisplayName = "Merak Mail Server 10.x", EditorControl = "Merak", GroupId = 4, ProviderName = "Merak", ProviderType = "SolidCP.Providers.Mail.Merak10, SolidCP.Providers.Mail.Merak10" },
			new Provider() { ProviderId = 62, DisplayName = "SmarterStats 5.x +", EditorControl = "SmarterStats", GroupId = 8, ProviderName = "SmarterStats", ProviderType = "SolidCP.Providers.Statistics.SmarterStats5, SolidCP.Providers.Statistics.Smarter" +
				"Stats" },
			new Provider() { ProviderId = 63, DisplayName = "hMailServer 5.x", EditorControl = "hMailServer5", GroupId = 4, ProviderName = "hMailServer5", ProviderType = "SolidCP.Providers.Mail.hMailServer5, SolidCP.Providers.Mail.hMailServer5" },
			new Provider() { ProviderId = 64, DisplayName = "SmarterMail 7.x - 8.x", EditorControl = "SmarterMail60", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail7, SolidCP.Providers.Mail.SmarterMail7" },
			new Provider() { ProviderId = 65, DisplayName = "SmarterMail 9.x", EditorControl = "SmarterMail60", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail9, SolidCP.Providers.Mail.SmarterMail9" },
			new Provider() { ProviderId = 66, DisplayName = "SmarterMail 10.x +", EditorControl = "SmarterMail100", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail10, SolidCP.Providers.Mail.SmarterMail10" },
			new Provider() { ProviderId = 67, DisplayName = "SmarterMail 100.x +", EditorControl = "SmarterMail100x", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "SolidCP.Providers.Mail.SmarterMail100, SolidCP.Providers.Mail.SmarterMail100" },
			new Provider() { ProviderId = 90, DisplayName = "Hosted Microsoft Exchange Server 2010 SP2", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2010SP2", ProviderType = "SolidCP.Providers.HostedSolution.Exchange2010SP2, SolidCP.Providers.HostedSoluti" +
				"on" },
			new Provider() { ProviderId = 91, DisableAutoDiscovery = true, DisplayName = "Hosted Microsoft Exchange Server 2013", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2013",
				ProviderType = "SolidCP.Providers.HostedSolution.Exchange2013, SolidCP.Providers.HostedSolution." +
				"Exchange2013" },
			new Provider() { ProviderId = 92, DisplayName = "Hosted Microsoft Exchange Server 2016", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2016", ProviderType = "SolidCP.Providers.HostedSolution.Exchange2016, SolidCP.Providers.HostedSolution." +
				"Exchange2016" },
			new Provider() { ProviderId = 93, DisplayName = "Hosted Microsoft Exchange Server 2019", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2016", ProviderType = "SolidCP.Providers.HostedSolution.Exchange2019, SolidCP.Providers.HostedSolution." +
				"Exchange2019" },
			new Provider() { ProviderId = 100, DisplayName = "Windows Server 2008", EditorControl = "Windows2008", GroupId = 1, ProviderName = "Windows2008", ProviderType = "SolidCP.Providers.OS.Windows2008, SolidCP.Providers.OS.Windows2008" },
			new Provider() { ProviderId = 101, DisplayName = "Internet Information Services 7.0", EditorControl = "IIS70", GroupId = 2, ProviderName = "IIS70", ProviderType = "SolidCP.Providers.Web.IIs70, SolidCP.Providers.Web.IIs70" },
			new Provider() { ProviderId = 102, DisplayName = "Microsoft FTP Server 7.0", EditorControl = "MSFTP70", GroupId = 3, ProviderName = "MSFTP70", ProviderType = "SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs70" },
			new Provider() { ProviderId = 103, DisplayName = "Hosted Organizations", EditorControl = "Organizations", GroupId = 13, ProviderName = "Organizations", ProviderType = "SolidCP.Providers.HostedSolution.OrganizationProvider, SolidCP.Providers.HostedS" +
				"olution" },
			new Provider() { ProviderId = 104, DisplayName = "Windows Server 2012", EditorControl = "Windows2012", GroupId = 1, ProviderName = "Windows2012", ProviderType = "SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012" },
			new Provider() { ProviderId = 105, DisplayName = "Internet Information Services 8.0", EditorControl = "IIS70", GroupId = 2, ProviderName = "IIS80", ProviderType = "SolidCP.Providers.Web.IIs80, SolidCP.Providers.Web.IIs80" },
			new Provider() { ProviderId = 106, DisplayName = "Microsoft FTP Server 8.0", EditorControl = "MSFTP70", GroupId = 3, ProviderName = "MSFTP80", ProviderType = "SolidCP.Providers.FTP.MsFTP80, SolidCP.Providers.FTP.IIs80" },
			new Provider() { ProviderId = 110, DisplayName = "Cerberus FTP Server 6.x", EditorControl = "CerberusFTP6", GroupId = 3, ProviderName = "CerberusFTP6", ProviderType = "SolidCP.Providers.FTP.CerberusFTP6, SolidCP.Providers.FTP.CerberusFTP6" },
			new Provider() { ProviderId = 111, DisplayName = "Windows Server 2016", EditorControl = "Windows2008", GroupId = 1, ProviderName = "Windows2016", ProviderType = "SolidCP.Providers.OS.Windows2016, SolidCP.Providers.OS.Windows2016" },
			new Provider() { ProviderId = 112, DisplayName = "Internet Information Services 10.0", EditorControl = "IIS70", GroupId = 2, ProviderName = "IIS100", ProviderType = "SolidCP.Providers.Web.IIs100, SolidCP.Providers.Web.IIs100" },
			new Provider() { ProviderId = 113, DisplayName = "Microsoft FTP Server 10.0", EditorControl = "MSFTP70", GroupId = 3, ProviderName = "MSFTP100", ProviderType = "SolidCP.Providers.FTP.MsFTP100, SolidCP.Providers.FTP.IIs100" },
			new Provider() { ProviderId = 135, DisableAutoDiscovery = true, DisplayName = "Web Application Engines", EditorControl = "HeliconZoo", GroupId = 42, ProviderName = "HeliconZoo",
				ProviderType = "SolidCP.Providers.Web.HeliconZoo.HeliconZoo, SolidCP.Providers.Web.HeliconZoo" },
			new Provider() { ProviderId = 160, DisplayName = "IceWarp Mail Server", EditorControl = "IceWarp", GroupId = 4, ProviderName = "IceWarp", ProviderType = "SolidCP.Providers.Mail.IceWarp, SolidCP.Providers.Mail.IceWarp" },
			new Provider() { ProviderId = 200, DisplayName = "Hosted Windows SharePoint Services 3.0", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint30", ProviderType = "SolidCP.Providers.HostedSolution.HostedSharePointServer, SolidCP.Providers.Hoste" +
				"dSolution" },
			new Provider() { ProviderId = 201, DisplayName = "Hosted MS CRM 4.0", EditorControl = "CRM", GroupId = 21, ProviderName = "CRM", ProviderType = "SolidCP.Providers.HostedSolution.CRMProvider, SolidCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 202, DisplayName = "Microsoft SQL Server 2008", EditorControl = "MSSQL", GroupId = 22, ProviderName = "MsSQL", ProviderType = "SolidCP.Providers.Database.MsSqlServer2008, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 203, DisableAutoDiscovery = true, DisplayName = "BlackBerry 4.1", EditorControl = "BlackBerry", GroupId = 31, ProviderName = "BlackBerry 4.1",
				ProviderType = "SolidCP.Providers.HostedSolution.BlackBerryProvider, SolidCP.Providers.HostedSol" +
				"ution" },
			new Provider() { ProviderId = 204, DisableAutoDiscovery = true, DisplayName = "BlackBerry 5.0", EditorControl = "BlackBerry5", GroupId = 31, ProviderName = "BlackBerry 5.0",
				ProviderType = "SolidCP.Providers.HostedSolution.BlackBerry5Provider, SolidCP.Providers.HostedSo" +
				"lution" },
			new Provider() { ProviderId = 205, DisableAutoDiscovery = true, DisplayName = "Office Communications Server 2007 R2", EditorControl = "OCS", GroupId = 32, ProviderName = "OCS",
				ProviderType = "SolidCP.Providers.HostedSolution.OCS2007R2, SolidCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 206, DisableAutoDiscovery = true, DisplayName = "OCS Edge server", EditorControl = "OCS_Edge", GroupId = 32, ProviderName = "OCSEdge",
				ProviderType = "SolidCP.Providers.HostedSolution.OCSEdge2007R2, SolidCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 208, DisplayName = "Hosted SharePoint Foundation 2010", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint2010", ProviderType = "SolidCP.Providers.HostedSolution.HostedSharePointServer2010, SolidCP.Providers.H" +
				"ostedSolution" },
			new Provider() { ProviderId = 209, DisplayName = "Microsoft SQL Server 2012", EditorControl = "MSSQL", GroupId = 23, ProviderName = "MsSQL", ProviderType = "SolidCP.Providers.Database.MsSqlServer2012, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 250, DisplayName = "Microsoft Lync Server 2010 Multitenant Hosting Pack", EditorControl = "Lync", GroupId = 41, ProviderName = "Lync2010", ProviderType = "SolidCP.Providers.HostedSolution.Lync2010, SolidCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 300, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V", EditorControl = "HyperV", GroupId = 30, ProviderName = "HyperV",
				ProviderType = "SolidCP.Providers.Virtualization.HyperV, SolidCP.Providers.Virtualization.HyperV" },
			new Provider() { ProviderId = 301, DisplayName = "MySQL Server 5.5", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer55, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 302, DisplayName = "MySQL Server 5.6", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer56, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 303, DisplayName = "MySQL Server 5.7", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer57, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 304, DisplayName = "MySQL Server 8.0", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer80, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 305, DisplayName = "MySQL Server 8.1", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer81, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 306, DisplayName = "MySQL Server 8.2", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer82, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 307, DisplayName = "MySQL Server 8.3", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer83, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 308, DisplayName = "MySQL Server 8.4", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer84, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 320, DisplayName = "MySQL Server 9.0", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "SolidCP.Providers.Database.MySqlServer90, SolidCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 350, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2012 R2", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2012R2",
				ProviderType = "SolidCP.Providers.Virtualization.HyperV2012R2, SolidCP.Providers.Virtualization." +
				"HyperV2012R2" },
			new Provider() { ProviderId = 351, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V Virtual Machine Management", EditorControl = "HyperVvmm", GroupId = 33, ProviderName = "HyperVvmm",
				ProviderType = "SolidCP.Providers.Virtualization.HyperVvmm, SolidCP.Providers.Virtualization.Hyp" +
				"erVvmm" },
			new Provider() { ProviderId = 352, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2016", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2016",
				ProviderType = "SolidCP.Providers.Virtualization.HyperV2016, SolidCP.Providers.Virtualization.Hy" +
				"perV2016" },
			new Provider() { ProviderId = 370, DisableAutoDiscovery = true, DisplayName = "Proxmox Virtualization (remote)", EditorControl = "Proxmox", GroupId = 167, ProviderName = "Proxmox (remote)",
				ProviderType = "SolidCP.Providers.Virtualization.Proxmoxvps, SolidCP.Providers.Virtualization.Pr" +
				"oxmoxvps" },
			new Provider() { ProviderId = 371, DisableAutoDiscovery = false, DisplayName = "Proxmox Virtualization", EditorControl = "Proxmox", GroupId = 167, ProviderName = "Proxmox",
				ProviderType = "SolidCP.Providers.Virtualization.ProxmoxvpsLocal, SolidCP.Providers.Virtualizati" +
				"on.Proxmoxvps" },
			new Provider() { ProviderId = 400, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V For Private Cloud", EditorControl = "HyperVForPrivateCloud", GroupId = 40, ProviderName = "HyperVForPC",
				ProviderType = "SolidCP.Providers.VirtualizationForPC.HyperVForPC, SolidCP.Providers.Virtualizat" +
				"ionForPC.HyperVForPC" },
			new Provider() { ProviderId = 410, DisplayName = "Microsoft DNS Server 2012+", EditorControl = "MSDNS", GroupId = 7, ProviderName = "MSDNS.2012", ProviderType = "SolidCP.Providers.DNS.MsDNS2012, SolidCP.Providers.DNS.MsDNS2012" },
			new Provider() { ProviderId = 500, DisplayName = "Unix System", EditorControl = "Unix", GroupId = 1, ProviderName = "UnixSystem", ProviderType = "SolidCP.Providers.OS.Unix, SolidCP.Providers.OS.Unix" },
			new Provider() { ProviderId = 600, DisableAutoDiscovery = true, DisplayName = "Enterprise Storage Windows 2012", EditorControl = "EnterpriseStorage", GroupId = 44, ProviderName = "EnterpriseStorage2012",
				ProviderType = "SolidCP.Providers.EnterpriseStorage.Windows2012, SolidCP.Providers.EnterpriseSto" +
				"rage.Windows2012" },
			new Provider() { ProviderId = 700, DisableAutoDiscovery = true, DisplayName = "Storage Spaces Windows 2012", EditorControl = "StorageSpaceServices", GroupId = 49, ProviderName = "StorageSpace2012",
				ProviderType = "SolidCP.Providers.StorageSpaces.Windows2012, SolidCP.Providers.StorageSpaces.Win" +
				"dows2012" },
			new Provider() { ProviderId = 1201, DisplayName = "Hosted MS CRM 2011", EditorControl = "CRM2011", GroupId = 21, ProviderName = "CRM", ProviderType = "SolidCP.Providers.HostedSolution.CRMProvider2011, SolidCP.Providers.HostedSoluti" +
				"on.CRM2011" },
			new Provider() { ProviderId = 1202, DisplayName = "Hosted MS CRM 2013", EditorControl = "CRM2011", GroupId = 24, ProviderName = "CRM", ProviderType = "SolidCP.Providers.HostedSolution.CRMProvider2013, SolidCP.Providers.HostedSoluti" +
				"on.Crm2013" },
			new Provider() { ProviderId = 1203, DisplayName = "Microsoft SQL Server 2014", EditorControl = "MSSQL", GroupId = 46, ProviderName = "MsSQL", ProviderType = "SolidCP.Providers.Database.MsSqlServer2014, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1205, DisplayName = "Hosted MS CRM 2015", EditorControl = "CRM2011", GroupId = 24, ProviderName = "CRM", ProviderType = "SolidCP.Providers.HostedSolution.CRMProvider2015, SolidCP.Providers.HostedSoluti" +
				"on.Crm2015" },
			new Provider() { ProviderId = 1206, DisplayName = "Hosted MS CRM 2016", EditorControl = "CRM2011", GroupId = 24, ProviderName = "CRM", ProviderType = "SolidCP.Providers.HostedSolution.CRMProvider2016, SolidCP.Providers.HostedSoluti" +
				"on.Crm2016" },
			new Provider() { ProviderId = 1301, DisplayName = "Hosted SharePoint Foundation 2013", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint2013", ProviderType = "SolidCP.Providers.HostedSolution.HostedSharePointServer2013, SolidCP.Providers.H" +
				"ostedSolution.SharePoint2013" },
			new Provider() { ProviderId = 1306, DisplayName = "Hosted SharePoint Foundation 2016", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint2016", ProviderType = "SolidCP.Providers.HostedSolution.HostedSharePointServer2016, SolidCP.Providers.H" +
				"ostedSolution.SharePoint2016" },
			new Provider() { ProviderId = 1401, DisplayName = "Microsoft Lync Server 2013 Enterprise Edition", EditorControl = "Lync", GroupId = 41, ProviderName = "Lync2013", ProviderType = "SolidCP.Providers.HostedSolution.Lync2013, SolidCP.Providers.HostedSolution.Lync" +
				"2013" },
			new Provider() { ProviderId = 1402, DisplayName = "Microsoft Lync Server 2013 Multitenant Hosting Pack", EditorControl = "Lync", GroupId = 41, ProviderName = "Lync2013HP", ProviderType = "SolidCP.Providers.HostedSolution.Lync2013HP, SolidCP.Providers.HostedSolution.Ly" +
				"nc2013HP" },
			new Provider() { ProviderId = 1403, DisplayName = "Microsoft Skype for Business Server 2015", EditorControl = "SfB", GroupId = 52, ProviderName = "SfB2015", ProviderType = "SolidCP.Providers.HostedSolution.SfB2015, SolidCP.Providers.HostedSolution.SfB20" +
				"15" },
			new Provider() { ProviderId = 1404, DisplayName = "Microsoft Skype for Business Server 2019", EditorControl = "SfB", GroupId = 52, ProviderName = "SfB2019", ProviderType = "SolidCP.Providers.HostedSolution.SfB2019, SolidCP.Providers.HostedSolution.SfB20" +
				"19" },
			new Provider() { ProviderId = 1501, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2012", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2012",
				ProviderType = "SolidCP.Providers.RemoteDesktopServices.Windows2012,SolidCP.Providers.RemoteDesk" +
				"topServices.Windows2012" },
			new Provider() { ProviderId = 1502, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2016", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2012",
				ProviderType = "SolidCP.Providers.RemoteDesktopServices.Windows2016,SolidCP.Providers.RemoteDesk" +
				"topServices.Windows2016" },
			new Provider() { ProviderId = 1503, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2019", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2019",
				ProviderType = "SolidCP.Providers.RemoteDesktopServices.Windows2019,SolidCP.Providers.RemoteDesk" +
				"topServices.Windows2019" },
			new Provider() { ProviderId = 1550, DisplayName = "MariaDB 10.1", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB101, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1552, DisplayName = "Hosted SharePoint Enterprise 2013", EditorControl = "HostedSharePoint30", GroupId = 73, ProviderName = "HostedSharePoint2013Ent", ProviderType = "SolidCP.Providers.HostedSolution.HostedSharePointServer2013Ent, SolidCP.Provider" +
				"s.HostedSolution.SharePoint2013Ent" },
			new Provider() { ProviderId = 1560, DisplayName = "MariaDB 10.2", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB102, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1570, DisableAutoDiscovery = true, DisplayName = "MariaDB 10.3", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB",
				ProviderType = "SolidCP.Providers.Database.MariaDB103, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1571, DisableAutoDiscovery = true, DisplayName = "MariaDB 10.4", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB",
				ProviderType = "SolidCP.Providers.Database.MariaDB104, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1572, DisplayName = "MariaDB 10.5", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB105, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1573, DisplayName = "MariaDB 10.6", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB106, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1574, DisplayName = "MariaDB 10.7", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB107, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1575, DisplayName = "MariaDB 10.8", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB108, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1576, DisplayName = "MariaDB 10.9", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB109, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1577, DisplayName = "MariaDB 10.10", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB1010, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1578, DisplayName = "MariaDB 10.11", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB1011, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1579, DisplayName = "MariaDB 11.0", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB110, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1580, DisplayName = "MariaDB 11.1", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB111, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1581, DisplayName = "MariaDB 11.2", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB112, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1582, DisplayName = "MariaDB 11.3", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB113, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1583, DisplayName = "MariaDB 11.4", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB114, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1584, DisplayName = "MariaDB 11.5", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB115, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1585, DisplayName = "MariaDB 11.6", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB116, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1586, DisplayName = "MariaDB 11.7", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "SolidCP.Providers.Database.MariaDB117, SolidCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1601, DisableAutoDiscovery = true, DisplayName = "Mail Cleaner", EditorControl = "MailCleaner", GroupId = 61, ProviderName = "MailCleaner",
				ProviderType = "SolidCP.Providers.Filters.MailCleaner, SolidCP.Providers.Filters.MailCleaner" },
			new Provider() { ProviderId = 1602, DisableAutoDiscovery = true, DisplayName = "SpamExperts Mail Filter", EditorControl = "SpamExperts", GroupId = 61, ProviderName = "SpamExperts",
				ProviderType = "SolidCP.Providers.Filters.SpamExperts, SolidCP.Providers.Filters.SpamExperts" },
			new Provider() { ProviderId = 1701, DisplayName = "Microsoft SQL Server 2016", EditorControl = "MSSQL", GroupId = 71, ProviderName = "MsSQL", ProviderType = "SolidCP.Providers.Database.MsSqlServer2016, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1702, DisplayName = "Hosted SharePoint Enterprise 2016", EditorControl = "HostedSharePoint30", GroupId = 73, ProviderName = "HostedSharePoint2016Ent", ProviderType = "SolidCP.Providers.HostedSolution.HostedSharePointServer2016Ent, SolidCP.Provider" +
				"s.HostedSolution.SharePoint2016Ent" },
			new Provider() { ProviderId = 1703, DisplayName = "SimpleDNS Plus 6.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "SolidCP.Providers.DNS.SimpleDNS6, SolidCP.Providers.DNS.SimpleDNS60" },
			new Provider() { ProviderId = 1704, DisableAutoDiscovery = true, DisplayName = "Microsoft SQL Server 2017", EditorControl = "MSSQL", GroupId = 72, ProviderName = "MsSQL",
				ProviderType = "SolidCP.Providers.Database.MsSqlServer2017, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1705, DisableAutoDiscovery = true, DisplayName = "Microsoft SQL Server 2019", EditorControl = "MSSQL", GroupId = 74, ProviderName = "MsSQL",
				ProviderType = "SolidCP.Providers.Database.MsSqlServer2019, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1706, DisplayName = "Microsoft SQL Server 2022", EditorControl = "MSSQL", GroupId = 75, ProviderName = "MsSQL", ProviderType = "SolidCP.Providers.Database.MsSqlServer2022, SolidCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1711, DisplayName = "Hosted SharePoint 2019", EditorControl = "HostedSharePoint30", GroupId = 73, ProviderName = "HostedSharePoint2019", ProviderType = "SolidCP.Providers.HostedSolution.HostedSharePointServer2019, SolidCP.Providers.H" +
				"ostedSolution.SharePoint2019" },
			new Provider() { ProviderId = 1800, DisplayName = "Windows Server 2019", EditorControl = "Windows2012", GroupId = 1, ProviderName = "Windows2019", ProviderType = "SolidCP.Providers.OS.Windows2019, SolidCP.Providers.OS.Windows2019" },
			new Provider() { ProviderId = 1801, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2019", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2019",
				ProviderType = "SolidCP.Providers.Virtualization.HyperV2019, SolidCP.Providers.Virtualization.Hy" +
				"perV2019" },
			new Provider() { ProviderId = 1802, DisplayName = "Windows Server 2022", EditorControl = "Windows2012", GroupId = 1, ProviderName = "Windows2022", ProviderType = "SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022" },
			new Provider() { ProviderId = 1803, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2022", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2022",
				ProviderType = "SolidCP.Providers.Virtualization.HyperV2022, SolidCP.Providers.Virtualization.Hy" +
				"perV2022" },
			new Provider() { ProviderId = 1901, DisplayName = "SimpleDNS Plus 8.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "SolidCP.Providers.DNS.SimpleDNS8, SolidCP.Providers.DNS.SimpleDNS80" },
			new Provider() { ProviderId = 1902, DisplayName = "Microsoft DNS Server 2016", EditorControl = "MSDNS", GroupId = 7, ProviderName = "MSDNS.2016", ProviderType = "SolidCP.Providers.DNS.MsDNS2016, SolidCP.Providers.DNS.MsDNS2016" },
			new Provider() { ProviderId = 1903, DisplayName = "SimpleDNS Plus 9.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "SolidCP.Providers.DNS.SimpleDNS9, SolidCP.Providers.DNS.SimpleDNS90" },
			new Provider() { ProviderId = 1910, DisplayName = "vsftpd FTP Server 3 (Experimental)", EditorControl = "vsftpd", GroupId = 3, ProviderName = "vsftpd", ProviderType = "SolidCP.Providers.FTP.VsFtp3, SolidCP.Providers.FTP.VsFtp" },
			new Provider() { ProviderId = 1911, DisplayName = "Apache Web Server 2.4 (Experimental)", EditorControl = "Apache", GroupId = 2, ProviderName = "Apache", ProviderType = "SolidCP.Providers.Web.Apache24, SolidCP.Providers.Web.Apache" }
		});
		#endregion
    }
}
