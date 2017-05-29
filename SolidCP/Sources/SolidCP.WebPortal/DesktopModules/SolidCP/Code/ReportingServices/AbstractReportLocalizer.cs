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
using System.Xml;

namespace SolidCP.Portal.ReportingServices
{
	/// <summary>
	/// This class encapsulates the algorithm of localization of Reports.
	/// </summary>
	public abstract class AbstractReportLocalizer
	{
		#region Data
		/// <summary>
		/// The string identifier that will be used to load current report related localization strings.
		/// In resource file they are contained in the following format: {reportIdentifierId}.{localizationStringName}
		/// </summary>
		protected string reportIdentifier;

		/// <summary>
		/// Name of the report
		/// </summary>
		protected string reportName;

		/// <summary>
		/// Reference to the class that helps get localization strings.
		/// </summary>
		protected IResourceStorage resourceStorage;

		/// <summary>
		/// Nodes inside the report that can be localized.
		/// </summary>
		protected string[] localizableNodes = new string[] { "Value", "ToolTip", "Label" };
		#endregion

		#region Constructors
		/// <summary>
		/// Base constructor for every report localizer.
		/// </summary>
		/// <param name="reportName">Name of the report to localize.</param>
		/// <param name="reportIdentifier">Identifier of the report inside the resource file.</param>
		/// <param name="resourceStorage">Instance of the class used to get the strings from localization resource file.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="reportName"/> or <paramref name="resourceStorage"/> is null.</exception>
		public AbstractReportLocalizer(string reportName, string reportIdentifier, IResourceStorage resourceStorage)
		{
			if (String.IsNullOrEmpty(reportName))
			{
				throw new ArgumentNullException("reportName", "Please, specify the name of the report to view.");
			}
			if (resourceStorage == null)
			{
				throw new ArgumentNullException("resourceStorage");
			}

			this.reportName = reportName;
			this.reportIdentifier = reportIdentifier;
			this.resourceStorage = resourceStorage;
		}
		#endregion

		/// <summary>
		/// Localizes report and provides a reference to <see cref="TextReader"/> class that can be then given to the Report Viewer component.
		/// </summary>
		/// <returns><see cref="TextReader"/> class containing the localized report contents via stream.</returns>
		/// <exception cref="InvalidOperationException">When required report does not exists or cannot be found.</exception>
		public TextReader GetLocalizedReportStream()
		{
			//1. Check if report exists
			if (!IsReportExists())
			{
				ThrowReportDoesNotExist();
			}

			// 2. Load report (report is an XLM document.)
			XmlDocument reportXml = new XmlDocument();
			reportXml.Load(
				GetReportStream()
				);

			XmlNamespaceManager nsmgr = new XmlNamespaceManager(reportXml.NameTable);
			nsmgr.AddNamespace("nm", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
			nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

			// 3. localize it
			foreach (string localizableNode in this.localizableNodes)
			{
				//for each of the node we can localize - do localize it
				// - find all nodes with LocID parameter
				foreach (XmlNode node in reportXml.DocumentElement.SelectNodes(
											String.Format("//nm:{0}[@rd:LocID]", localizableNode)
											, nsmgr
										 )
						)
				{
					//test if LocID is not null and it's value is not empty
					if (node.Attributes["rd:LocID"] != null)
					{
						if (!String.IsNullOrEmpty(node.Attributes["rd:LocID"].Value))
						{
							//replace nodes' text value with localization string from resources
							node.InnerText = GetLocalizedResourceString(node.Attributes["rd:LocID"].Value);
						}
					}
				}
			}

			// 4. return TextReader
			return new StringReader(reportXml.DocumentElement.OuterXml) as TextReader;
		}

		#region Vistual Members - optional to override

		/// <summary>
		/// Thrown <see cref="InvalidOperationException"/> to indicate that report does not exists.
		/// </summary>
		/// <exception cref="InvalidOperationException">Always, when called.</exception>
		public virtual void ThrowReportDoesNotExist()
		{
			throw new InvalidOperationException(
				String.Format(
					"Report '{0}' does not exist."
					, this.reportName
				)
			);
		}

		/// <summary>
		/// Provides a strign requested by a <paramref name="localizationId"/> (resource key).
		/// It delegates the call to the <see cref="IResourceStorage"/> class instance.
		/// </summary>
		/// <param name="localizationId">The identifier of the string contained in the localization resource file.</param>
		/// <returns>String.</returns>
		/// <remarks>
		/// In case <code>reportIdentifier</code> passed to the constructor is null or empty 
		/// the resource string will be requested using the <paramref name="localizationId"/> only.
		/// Otherwise, the following resource key will be used as a search pattern: {<code>reportIdentifier</code>}.{<paramref name="localizationId"/>}
		/// </remarks>
		public virtual string GetLocalizedResourceString(string localizationId)
		{
			String localizedString = String.Empty;

			if (String.IsNullOrEmpty(this.reportIdentifier))
			{
				localizedString = this.resourceStorage.GetString(localizationId);
			}
			else
			{
				localizedString = this.resourceStorage.GetString(
					String.Format("{0}.{1}", this.reportIdentifier, localizationId)
				);
			}

			return localizedString;
		}
		#endregion

		#region Astract Members - Required to implement
		/// <summary>
		/// Verify whether report exists.
		/// </summary>
		/// <returns>True, if report exists. Otherwise false.</returns>
		public abstract bool IsReportExists();

		/// <summary>
		/// Returns the report <see cref="Stream"/>.
		/// </summary>
		/// <returns>Report <see cref="Stream"/>.</returns>
		public abstract Stream GetReportStream();
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns Report Name being passed to the constuctor
		/// </summary>
		public string ReportName
		{
			get { return this.reportName; }
		}
		#endregion
	}
}
