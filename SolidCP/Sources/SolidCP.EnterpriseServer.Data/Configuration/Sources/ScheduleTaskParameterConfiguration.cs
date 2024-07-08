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

public partial class ScheduleTaskParameterConfiguration: EntityTypeConfiguration<ScheduleTaskParameter>
{
    public override void Configure() {
        HasOne(d => d.Task).WithMany(p => p.ScheduleTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskParameters_ScheduleTasks");

        #region Seed Data
        HasData(() => new ScheduleTaskParameter[] {
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", ParameterId = "AUDIT_LOG_DATE", DataTypeId = "List", DefaultValue = "today=Today;yesterday=Yesterday;schedule=Schedule", ParameterOrder = 5 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", ParameterId = "AUDIT_LOG_SEVERITY", DataTypeId = "List", DefaultValue = "-1=All;0=Information;1=Warning;2=Error", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", ParameterId = "AUDIT_LOG_SOURCE", DataTypeId = "List", DefaultValue = "", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", ParameterId = "AUDIT_LOG_TASK", DataTypeId = "List", DefaultValue = "", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", ParameterId = "MAIL_TO", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_AUDIT_LOG_REPORT", ParameterId = "SHOW_EXECUTION_LOG", DataTypeId = "List", DefaultValue = "0=No;1=Yes", ParameterOrder = 6 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP", ParameterId = "BACKUP_FILE_NAME", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP", ParameterId = "DELETE_TEMP_BACKUP", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP", ParameterId = "STORE_PACKAGE_FOLDER", DataTypeId = "String", DefaultValue = "\\", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP", ParameterId = "STORE_PACKAGE_ID", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP", ParameterId = "STORE_SERVER_FOLDER", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", ParameterId = "BACKUP_FOLDER", DataTypeId = "String", DefaultValue = "\\backups", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", ParameterId = "BACKUP_NAME", DataTypeId = "String", DefaultValue = "database_backup.bak", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", ParameterId = "DATABASE_GROUP", DataTypeId = "List", DefaultValue = "MsSQL2014=SQL Server 2014;MsSQL2016=SQL Server 2016;MsSQL2017=SQL Server 2017;Ms" +
                "SQL2019=SQL Server 2019;MsSQL2022=SQL Server 2022;MySQL5=MySQL 5.0;MariaDB=Maria" +
                "DB", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", ParameterId = "DATABASE_NAME", DataTypeId = "String", DefaultValue = "", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_BACKUP_DATABASE", ParameterId = "ZIP_BACKUP", DataTypeId = "List", DefaultValue = "true=Yes;false=No", ParameterOrder = 5 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "MAIL_BODY", DataTypeId = "MultiString", DefaultValue = "", ParameterOrder = 10 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "MAIL_FROM", DataTypeId = "String", DefaultValue = "admin@mysite.com", ParameterOrder = 7 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "MAIL_SUBJECT", DataTypeId = "String", DefaultValue = "Web Site is unavailable", ParameterOrder = 9 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "MAIL_TO", DataTypeId = "String", DefaultValue = "admin@mysite.com", ParameterOrder = 8 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "PASSWORD", DataTypeId = "String", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "RESPONSE_CONTAIN", DataTypeId = "String", ParameterOrder = 5 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "RESPONSE_DOESNT_CONTAIN", DataTypeId = "String", ParameterOrder = 6 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "RESPONSE_STATUS", DataTypeId = "String", DefaultValue = "500", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "URL", DataTypeId = "String", DefaultValue = "http://", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "USE_RESPONSE_CONTAIN", DataTypeId = "Boolean", DefaultValue = "false", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "USE_RESPONSE_DOESNT_CONTAIN", DataTypeId = "Boolean", DefaultValue = "false", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "USE_RESPONSE_STATUS", DataTypeId = "Boolean", DefaultValue = "false", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_CHECK_WEBSITE", ParameterId = "USERNAME", DataTypeId = "String", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", ParameterId = "DAYS_BEFORE", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", ParameterId = "ENABLE_NOTIFICATION", DataTypeId = "Boolean", DefaultValue = "false", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", ParameterId = "INCLUDE_NONEXISTEN_DOMAINS", DataTypeId = "Boolean", DefaultValue = "false", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION", ParameterId = "MAIL_TO", DataTypeId = "String", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", ParameterId = "DNS_SERVERS", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", ParameterId = "MAIL_TO", DataTypeId = "String", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", ParameterId = "PAUSE_BETWEEN_QUERIES", DataTypeId = "String", DefaultValue = "100", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP", ParameterId = "SERVER_NAME", DataTypeId = "String", DefaultValue = "", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_FTP_FILES", ParameterId = "FILE_PATH", DataTypeId = "String", DefaultValue = "\\", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_FTP_FILES", ParameterId = "FTP_FOLDER", DataTypeId = "String", ParameterOrder = 5 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_FTP_FILES", ParameterId = "FTP_PASSWORD", DataTypeId = "String", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_FTP_FILES", ParameterId = "FTP_SERVER", DataTypeId = "String", DefaultValue = "ftp.myserver.com", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_FTP_FILES", ParameterId = "FTP_USERNAME", DataTypeId = "String", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", ParameterId = "CRM_REPORT", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 6 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", ParameterId = "EMAIL", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", ParameterId = "EXCHANGE_REPORT", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", ParameterId = "LYNC_REPORT", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", ParameterId = "ORGANIZATION_REPORT", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 7 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", ParameterId = "SFB_REPORT", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 5 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_HOSTED_SOLUTION_REPORT", ParameterId = "SHAREPOINT_REPORT", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "MARIADB_OVERUSED", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "MSSQL_OVERUSED", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "MYSQL_OVERUSED", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "OVERUSED_MAIL_BCC", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "OVERUSED_MAIL_BODY", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "OVERUSED_MAIL_FROM", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "OVERUSED_MAIL_SUBJECT", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "OVERUSED_USAGE_THRESHOLD", DataTypeId = "String", DefaultValue = "100", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "SEND_OVERUSED_EMAIL", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "SEND_WARNING_EMAIL", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "WARNING_MAIL_BCC", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "WARNING_MAIL_BODY", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "WARNING_MAIL_FROM", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "WARNING_MAIL_SUBJECT", DataTypeId = "String", DefaultValue = "", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES", ParameterId = "WARNING_USAGE_THRESHOLD", DataTypeId = "String", DefaultValue = "80", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_RUN_SYSTEM_COMMAND", ParameterId = "EXECUTABLE_PARAMS", DataTypeId = "String", DefaultValue = "", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_RUN_SYSTEM_COMMAND", ParameterId = "EXECUTABLE_PATH", DataTypeId = "String", DefaultValue = "Executable.exe", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_RUN_SYSTEM_COMMAND", ParameterId = "SERVER_NAME", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SEND_MAIL", ParameterId = "MAIL_BODY", DataTypeId = "MultiString", ParameterOrder = 4 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SEND_MAIL", ParameterId = "MAIL_FROM", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SEND_MAIL", ParameterId = "MAIL_SUBJECT", DataTypeId = "String", ParameterOrder = 3 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SEND_MAIL", ParameterId = "MAIL_TO", DataTypeId = "String", ParameterOrder = 2 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "BANDWIDTH_OVERUSED", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "DISKSPACE_OVERUSED", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SEND_SUSPENSION_EMAIL", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SEND_WARNING_EMAIL", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SUSPEND_OVERUSED", DataTypeId = "Boolean", DefaultValue = "true", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SUSPENSION_MAIL_BCC", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SUSPENSION_MAIL_BODY", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SUSPENSION_MAIL_FROM", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SUSPENSION_MAIL_SUBJECT", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "SUSPENSION_USAGE_THRESHOLD", DataTypeId = "String", DefaultValue = "100", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "WARNING_MAIL_BCC", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "WARNING_MAIL_BODY", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "WARNING_MAIL_FROM", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "WARNING_MAIL_SUBJECT", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_SUSPEND_PACKAGES", ParameterId = "WARNING_USAGE_THRESHOLD", DataTypeId = "String", DefaultValue = "80", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION", ParameterId = "DAYS_BEFORE_EXPIRATION", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_ZIP_FILES", ParameterId = "FOLDER", DataTypeId = "String", ParameterOrder = 1 },
            new ScheduleTaskParameter() { TaskId = "SCHEDULE_TASK_ZIP_FILES", ParameterId = "ZIP_FILE", DataTypeId = "String", DefaultValue = "\\archive.zip", ParameterOrder = 2 }
        });
        #endregion

    }
}
