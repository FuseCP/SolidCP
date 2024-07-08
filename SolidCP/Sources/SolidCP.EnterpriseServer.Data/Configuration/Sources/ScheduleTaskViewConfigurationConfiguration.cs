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

public partial class ScheduleTaskViewConfigurationConfiguration: EntityTypeConfiguration<ScheduleTaskViewConfiguration>
{
    public override void Configure() {
        HasOne(d => d.Task).WithMany(p => p.ScheduleTaskViewConfigurations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration");

        #region Seed Data
        HasData(() => new ScheduleTaskViewConfiguration[] {
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_ACTIVATE_PAID_INVOICES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/AuditLogReportView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_BACKUP", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/Backup.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/BackupDatabase.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/CheckWebsite.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/DomainExpirationView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/DomainLookupView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_FTP_FILES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/SendFilesViaFtp.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_GENERATE_INVOICES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/HostedSolutionReport.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/NotifyOverusedDatabases.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_RUN_PAYMENT_QUEUE", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_RUN_SYSTEM_COMMAND", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/ExecuteSystemCommand.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_SEND_MAIL", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/SendEmailNotification.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/SuspendOverusedSpaces.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/UserPasswordExpirationNotification" +
                "View.ascx", Environment = "ASP.NET" },
            new ScheduleTaskViewConfiguration() { ConfigurationId = "ASP_NET", TaskId = "SCHEDULE_TASK_ZIP_FILES", Description = "~/DesktopModules/SolidCP/ScheduleTaskControls/ZipFiles.ascx", Environment = "ASP.NET" }
        });
        #endregion

    }
}
