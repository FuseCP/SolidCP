// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.EnterpriseServer
{
    public class AuditLog
    {
        public static DataSet GetAuditLogRecordsPaged(int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate,
            int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return DataProvider.GetAuditLogRecordsPaged(SecurityContext.User.UserId,
                userId, packageId, itemId, itemName, GetStartDate(startDate), GetEndDate(endDate),
                severityId, sourceName, taskName, sortColumn, startRow, maximumRows);
        }

        public static DataSet GetAuditLogSources()
        {
            return DataProvider.GetAuditLogSources();
        }

        public static DataSet GetAuditLogTasks(string sourceName)
        {
            return DataProvider.GetAuditLogTasks(sourceName);
        }

        public static LogRecord GetAuditLogRecord(string recordId)
        {
            return ObjectUtils.FillObjectFromDataReader<LogRecord>(
                DataProvider.GetAuditLogRecord(recordId));
        }

        public static int DeleteAuditLogRecords(int userId, int itemId, string itemName,
            DateTime startDate, DateTime endDate, int severityId, string sourceName, string taskName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            DataProvider.DeleteAuditLogRecords(SecurityContext.User.UserId,
                userId, itemId, itemName, GetStartDate(startDate), GetEndDate(endDate), severityId, sourceName, taskName);

            return 0;
        }

        public static int DeleteAuditLogRecordsComplete()
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            DataProvider.DeleteAuditLogRecordsComplete();

            return 0;
        }

        public static void AddAuditLogRecord(string recordId, int severityId, int userId, string username,
            int packageId, int itemId, string itemName, DateTime startDate, DateTime finishDate,
            string sourceName, string taskName, string executionLog)
        {
            try
            {
                DataProvider.AddAuditLogRecord(recordId, severityId, userId, username, packageId, itemId, itemName,
                    startDate, finishDate, sourceName, taskName, executionLog);
            }
            catch { }
        }

        private static DateTime GetStartDate(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
        }

        private static DateTime GetEndDate(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
        }
    }
}
