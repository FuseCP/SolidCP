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

public partial class ScheduleTaskConfiguration: EntityTypeConfiguration<ScheduleTask>
{
    public override void Configure() {

        #region Seed Data
        HasData(() => new ScheduleTask[] {
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_ACTIVATE_PAID_INVOICES", TaskType = "SolidCP.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, SolidCP.EnterpriseS" +
                "erver.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.AuditLogReportTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_BACKUP", RoleId = 1, TaskType = "SolidCP.EnterpriseServer.BackupTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.BackupDatabaseTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE", RoleId = 2, TaskType = "SolidCP.EnterpriseServer.CalculateExchangeDiskspaceTask, SolidCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH", RoleId = 1, TaskType = "SolidCP.EnterpriseServer.CalculatePackagesBandwidthTask, SolidCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE", RoleId = 1, TaskType = "SolidCP.EnterpriseServer.CalculatePackagesDiskspaceTask, SolidCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES", TaskType = "SolidCP.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, SolidCP.Enterprise" +
                "Server.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.CheckWebSiteTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_DELETE_EXCHANGE_ACCOUNTS", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.DeleteExchangeAccountsTask, SolidCP.EnterpriseServer.Co" +
                "de" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.DomainExpirationTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", RoleId = 1, TaskType = "SolidCP.EnterpriseServer.DomainLookupViewTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_FTP_FILES", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.FTPFilesTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_GENERATE_INVOICES", TaskType = "SolidCP.Ecommerce.EnterpriseServer.GenerateInvoicesTask, SolidCP.EnterpriseServe" +
                "r.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", RoleId = 2, TaskType = "SolidCP.EnterpriseServer.HostedSolutionReportTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", RoleId = 2, TaskType = "SolidCP.EnterpriseServer.NotifyOverusedDatabasesTask, SolidCP.EnterpriseServer.C" +
                "ode" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_RUN_PAYMENT_QUEUE", TaskType = "SolidCP.Ecommerce.EnterpriseServer.RunPaymentQueueTask, SolidCP.EnterpriseServer" +
                ".Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_RUN_SYSTEM_COMMAND", RoleId = 1, TaskType = "SolidCP.EnterpriseServer.RunSystemCommandTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_SEND_MAIL", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.SendMailNotificationTask, SolidCP.EnterpriseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES", TaskType = "SolidCP.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, SolidCP.Enterpris" +
                "eServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", RoleId = 2, TaskType = "SolidCP.EnterpriseServer.SuspendOverusedPackagesTask, SolidCP.EnterpriseServer.C" +
                "ode" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION", RoleId = 1, TaskType = "SolidCP.EnterpriseServer.UserPasswordExpirationNotificationTask, SolidCP.Enterpr" +
                "iseServer.Code" },
            new ScheduleTask() { TaskId = "SCHEDULE_TASK_ZIP_FILES", RoleId = 3, TaskType = "SolidCP.EnterpriseServer.ZipFilesTask, SolidCP.EnterpriseServer.Code" }
        });
        #endregion
    }
}
