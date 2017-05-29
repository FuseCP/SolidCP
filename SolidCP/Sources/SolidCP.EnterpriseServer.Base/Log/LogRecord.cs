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
using System.Collections.Generic;
using System.Text;

namespace SolidCP.EnterpriseServer
{
    public class LogRecord
    {
	    string recordID;
	    int severityID;
	    int userID;
	    string username;
	    int itemID;
	    string sourceName;
	    DateTime startDate;
	    DateTime finishDate;
	    string taskName;
	    string itemName;
	    string executionLog;

        public string RecordID
        {
            get { return this.recordID; }
            set { this.recordID = value; }
        }

        public int SeverityID
        {
            get { return this.severityID; }
            set { this.severityID = value; }
        }

        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public int ItemID
        {
            get { return this.itemID; }
            set { this.itemID = value; }
        }

        public string SourceName
        {
            get { return this.sourceName; }
            set { this.sourceName = value; }
        }

        public System.DateTime StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        public System.DateTime FinishDate
        {
            get { return this.finishDate; }
            set { this.finishDate = value; }
        }

        public string TaskName
        {
            get { return this.taskName; }
            set { this.taskName = value; }
        }

        public string ItemName
        {
            get { return this.itemName; }
            set { this.itemName = value; }
        }

        public string ExecutionLog
        {
            get { return this.executionLog; }
            set { this.executionLog = value; }
        }
    }
}
