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
using System.Reflection;
using System.IO;

namespace SolidCP.Portal.ReportingServices
{
	/// <summary>
	/// Class that is used to localize reports embedded to the assembly.
	/// </summary>
	public class EmbeddedReportLocalizer : AbstractReportLocalizer
	{
		#region Data
		/// <summary>
		/// The assembly containing the required report file.
		/// </summary>
		protected Assembly assembly;

		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a localizer.
		/// </summary>
		/// <param name="assembly">The assembly containing the report file.</param>
		/// <param name="embeddedReportName">Embedded report full name: {default namespace}.{folder path inside assembly}.{report name with extension}</param>
		/// <param name="embeddedReportId">Report identifier that will be used to load resource strings related to this report.</param>
		/// <param name="resourceStorage"><see cref="IResourceStorage"/> instance.</param>
		public EmbeddedReportLocalizer(Assembly assembly, string embeddedReportName, string embeddedReportId, IResourceStorage resourceStorage)
			: base(embeddedReportName, embeddedReportId, resourceStorage)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			this.assembly = assembly;
		}

		/// <summary>
		/// Shorter constructor that assume that report file is located in the assembly where this type is declared.
		/// </summary>
		/// <param name="embeddedReportName">Embedded report full name: {default namespace}.{folder path inside assembly}.{report name with extension}</param>
		/// <param name="embeddedReportId">Report identifier that will be used to load resource strings related to this report.</param>
		/// <param name="resourceStorage"><see cref="IResourceStorage"/> instance.</param>
		public EmbeddedReportLocalizer(string embeddedReportName, string embeddedReportId, IResourceStorage resourceStorage)
			: base(embeddedReportName, embeddedReportId, resourceStorage)
		{
			this.assembly = this.GetType().Assembly;
		}
		#endregion

		#region Algorithm Methods overloads
		/// <summary>
		/// Verify whether current assembly contains report file requested in constructor.
		/// </summary>
		/// <returns>True, if file exists. False, if not.</returns>
		public override bool IsReportExists()
		{
			bool result = false;

			foreach (string resourceName in this.assembly.GetManifestResourceNames())
			{
				if (String.Compare(resourceName, this.reportName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = true;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Returns report file stream.
		/// </summary>
		/// <returns>report file <see cref="Stream"/>.</returns>
		public override Stream GetReportStream()
		{
			return this.assembly.GetManifestResourceStream(this.reportName);
		}
		#endregion
	}

}
