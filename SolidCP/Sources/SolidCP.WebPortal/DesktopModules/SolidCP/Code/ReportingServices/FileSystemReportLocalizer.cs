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
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

namespace SolidCP.Portal.ReportingServices
{
	/// <summary>
	/// Class that is used to load Reporting Services report files from local file system.
	/// </summary>
	public class FileSystemReportLocalizer : AbstractReportLocalizer
	{
		/// <summary>
		/// Constructs the class to function.
		/// </summary>
		/// <param name="reportName">Actually, the full path to the report file.</param>
		/// <param name="reportIdentifier">
		/// String used to identify report file related strings in resources.
		/// The following string format is used to assosiate report and its strings: {report name}.{localiation id}
		/// </param>
		/// <param name="resourceStorage"><see cref="IResourceStorage"/> instance.</param>
		public FileSystemReportLocalizer(string reportName, string reportIdentifier, IResourceStorage resourceStorage)
			: base(reportName, reportIdentifier, resourceStorage)
		{
		}


		/// <summary>
		/// Verify if report exists for the <code>reportName</code> passed to the constructor
		/// </summary>
		/// <returns>
		/// True, if report file exists. Otherwise, false.
		/// </returns>
		public override bool IsReportExists()
		{
			FileInfo info = new FileInfo(this.reportName);
			if (info.Exists)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns <see cref="Stream"/> one can use to read report file.
		/// </summary>
		/// <returns><see cref="Stream"/> instance for the report file.</returns>
		public override Stream GetReportStream()
		{
			return File.OpenRead(this.reportName);
		}
	}
}
