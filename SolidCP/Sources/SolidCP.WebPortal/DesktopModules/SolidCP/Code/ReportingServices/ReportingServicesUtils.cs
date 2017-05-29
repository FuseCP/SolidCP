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
using System.Configuration;
using System.Data;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.Reporting.WebForms;

namespace SolidCP.Portal.ReportingServices
{
	/// <summary>
	/// Utility class that provides helper methods
	/// </summary>
	public static class ReportingServicesUtils
	{
		/// <summary>
		/// Loads embedded report file.
		/// </summary>
		/// <param name="assembly"><see cref="Assembly"/> containing the report file.</param>
		/// <param name="embeddedReportName">Name of the report in format {default namespace}.{folder structure inside assembly}.{file name with extension}</param>
		/// <param name="embeddedReportResourceId">Resource identifier of a report being loaded.</param>
		/// <param name="module">Instance of a module containing the ReportViewer component.</param>
		/// <returns><see cref="TextReader"/> containing the localized report file.</returns>
		/// <exception cref="ArgumentNullException">When <paramref name="assembly"/>, <paramref name="embeddedReportName"/> or <paramref name="module"/> is null.</exception>
		public static TextReader LoadReportFileFromAssembly(Assembly assembly, string embeddedReportName, string embeddedReportResourceId, SolidCPModuleBase module)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (String.IsNullOrEmpty(embeddedReportName))
			{
				throw new ArgumentNullException("embeddedReportName");
			}
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}

			return new EmbeddedReportLocalizer(
					assembly
					, embeddedReportName
					, embeddedReportResourceId
					, new SolidCPModuleResourceStorage(module)
				).GetLocalizedReportStream();
		}

		/// <summary>
		/// Load report from file on disk.
		/// </summary>
		/// <param name="reportPath">Path to the report file.</param>
		/// <param name="reportResourceId">Resource id of a report inside resource file.</param>
		/// <param name="module">Instance of a module containing the ReportViewer component.</param>
		/// <returns><see cref="TextReader"/> containing the localized report file.</returns>
		/// <exception cref="ArgumentNullException">When <paramref name="reportPath"/> or <paramref name="module"/> is null.</exception>
		public static TextReader LoadReportFromFile(string reportPath, string reportResourceId, SolidCPModuleBase module)
		{
			if (String.IsNullOrEmpty(reportPath))
			{
				throw new ArgumentNullException("reportPath");
			}
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}

			return new FileSystemReportLocalizer(
						  reportPath
						, reportResourceId
						, new SolidCPModuleResourceStorage(module)
					).GetLocalizedReportStream();
		}

		/// <summary>
		/// Returns a URL that one can use to force Reporting Services 
		/// </summary>
		/// <param name="report"><see cref="Report"/> instance currently loaded.</param>
		/// <param name="reportViewer"><see cref="ReportViewer"/> component instance containing <paramref name="report"/></param>
		/// <param name="contentDisposition"><paramref name="reportViewer"/> export content disposition.</param>
		/// <returns>String representing URL to export report using Reporting Serivces engine.</returns>
		/// <exception cref="ArgumentNullException">When <paramref name="report"/> or <paramref name="reportViewer"/> is null.</exception>
		public static string GetReportExportUrl(Report report, ReportViewer reportViewer, ContentDisposition contentDisposition)
		{
			if (report == null)
			{
				throw new ArgumentNullException("report");
			}
			if (reportViewer == null)
			{
				throw new ArgumentNullException("reportViewer");
			}

			//Get the viewer instance id
			FieldInfo instanceIdInfo = typeof(ReportViewer).GetField("m_instanceIdentifier", BindingFlags.Instance | BindingFlags.NonPublic);
			String viewerInstanceId = ((Guid)instanceIdInfo.GetValue(reportViewer)).ToString("N");

			//Get drill through field 
			FieldInfo drillthroughField = typeof(Report).GetField("m_drillthroughDepth", BindingFlags.Instance | BindingFlags.NonPublic);
			int drillthroughDepth = (int)drillthroughField.GetValue(report);

			//Create query
			StringBuilder exportQuery = new StringBuilder();
			exportQuery.AppendFormat("Mode={0}&", "true");
			exportQuery.AppendFormat("ReportID={0}&", Guid.NewGuid().ToString("N"));
			exportQuery.AppendFormat("ControlID={0}&", viewerInstanceId);
			exportQuery.AppendFormat("Culture={0}&", CultureInfo.InvariantCulture.LCID.ToString(CultureInfo.InvariantCulture));
			exportQuery.AppendFormat("UICulture={0}&", CultureInfo.InvariantCulture.LCID.ToString(CultureInfo.InvariantCulture));
			exportQuery.AppendFormat("ReportStack={0}&", drillthroughDepth);
			exportQuery.AppendFormat("OpType={0}&", "Export");
			exportQuery.AppendFormat("FileName={0}&", report.DisplayName); //it should be empty as we do not use either Embedded or File reports
			exportQuery.AppendFormat("ContentDisposition={0}&", contentDisposition);
			exportQuery.Append("Format=");

			//Build exact URL
			UriBuilder handlerUri = GetReportHandlerUri();
			handlerUri.Query = exportQuery.ToString();

			return handlerUri.Uri.PathAndQuery;
		}

		#region Private functions
		static UriBuilder GetReportHandlerUri()
		{
			UriBuilder builder = new UriBuilder(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));

			string applicationPath = GetServerPath();

			StringBuilder builder2 = new StringBuilder(applicationPath);
			if (!applicationPath.EndsWith("/", true, CultureInfo.InvariantCulture))
			{
				builder2.Append("/");
			}

			builder2.Append("Reserved.ReportViewerWebControl.axd");
			builder.Path = builder2.ToString();
			return builder;
		}

		static string GetServerPath()
		{
			return HttpContext.Current.Response.ApplyAppPathModifier(
				HttpContext.Current.Request.ApplicationPath
				);
		}
		#endregion
	}
}
