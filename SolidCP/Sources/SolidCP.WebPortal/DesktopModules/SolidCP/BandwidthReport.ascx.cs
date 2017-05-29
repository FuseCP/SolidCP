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

using SolidCP.EnterpriseServer;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace SolidCP.Portal
{
    public partial class BandwidthReport : SolidCPModuleBase
    {
        public string StartDate
        {
            get { return litStartDate.Text; }
        }

        public string EndDate
        {
            get { return litEndDate.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            gvReport.PageSize = UsersHelper.GetDisplayItemsPerPage();

            if (!IsPostBack)
            {                
                // set start date
                if (Request["StartDate"] != null)
                {
                    calStartDate.SelectedDate = new DateTime(Int64.Parse(Request["StartDate"]));
                    calEndDate.SelectedDate = new DateTime(Int64.Parse(Request["EndDate"]));
                }
                else
                {
                    DateTime dt = DateTime.Now;
					calStartDate.SelectedDate = new DateTime(dt.Year, dt.Month, 1);
					calEndDate.SelectedDate = new DateTime(dt.Year, dt.Month,
						DateTime.DaysInMonth(dt.Year, dt.Month));

                }

                // apply to the report
                DisplayReport();
            }
        }

        private void DisplayReport()
        {
            // set period
            DateTime startDate = calStartDate.SelectedDate;
            DateTime endDate = calEndDate.SelectedDate;
            
            if (startDate > endDate)
            {
                ShowWarningMessage("START_END_DATE_VALIDATION");
                return;
            }

            litPeriod.Text = startDate.ToString("MMM dd, yyyy") +
                " - " + endDate.ToString("MMM dd, yyyy");

            litStartDate.Text = startDate.ToString();
            litEndDate.Text = endDate.ToString();
        }

        private void ShiftMonth(int number)
        {
            // change dates
			DateTime dt = calStartDate.SelectedDate.AddMonths(number);
			calStartDate.SelectedDate = new DateTime(dt.Year, dt.Month, 1);
			calEndDate.SelectedDate = new DateTime(dt.Year, dt.Month,
				DateTime.DaysInMonth(dt.Year, dt.Month));

            // rebind report
            DisplayReport();
        }

        protected void odsReport_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected string GetNavigatePackageLink(int packageId)
        {
			return NavigateURL(PortalUtils.SPACE_ID_PARAM, packageId.ToString(),
				"StartDate=" + DateTime.Parse(litStartDate.Text).Ticks.ToString(),
				"EndDate=" + DateTime.Parse(litEndDate.Text).Ticks.ToString());

            /*return NavigateURL(TabId, "PackageID", packageId.ToString(),
                "StartDate", DateTime.Parse(litStartDate.Text).Ticks.ToString(),
                "EndDate", DateTime.Parse(litEndDate.Text).Ticks.ToString());*/
        }

        protected void cmdPrevMonth_Click(object sender, EventArgs e)
        {
            ShiftMonth(-1);
        }

        public string GetAllocatedValue(int val)
        {
            return (val == -1) ? GetLocalizedString("Unlimited.Text") : val.ToString();
        }

        protected void cmdNextMonth_Click(object sender, EventArgs e)
        {
            ShiftMonth(1);
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DisplayReport();
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (dr != null)
            {
                if (dr["UsagePercentage"] != DBNull.Value && (int)dr["UsagePercentage"] > 100)
                    e.Row.CssClass = "NormalBold";
            }
        }


        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            ExportReport();
        }

        private void ExportReport()
        {
            // first try to load user specific transform (ie, reseller) if not found, then load the default admin user set          
            UserSettings BWTransform;
            try {
                BWTransform = ES.Services.Users.GetUserSettings(PanelSecurity.SelectedUserId, SystemSettings.BANDWIDTH_TRANSFORM);
            } catch (AmbiguousMatchException) {
                BWTransform = ES.Services.Users.GetUserSettings(1, SystemSettings.BANDWIDTH_TRANSFORM);   //admin case
            }
            RenderReport(BWTransform);

        }

        private string ReportTransform(string xslInput, string xmlInput)
        {
 

            string output = String.Empty;
            using (StringReader xmltransform = new StringReader(xslInput)) // xslInput is a string that contains xsl
            using (StringReader xmlinput = new StringReader(xmlInput)) // xmlInput is a string that contains xml
            {
                using (XmlReader xmlReaderTransform = XmlReader.Create(xmltransform))
                using (XmlReader xmlReaderInput = XmlReader.Create(xmlinput))
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xmlReaderTransform);
                    using (StringWriter outstream = new StringWriter())
                    using (XmlWriter xmlOutput = XmlWriter.Create(outstream, xslt.OutputSettings)) // use OutputSettings of xsl, so it can be output as HTML
                    {
                        xslt.Transform(xmlReaderInput, xmlOutput);
                        output = outstream.ToString();
                    }
                }
            }
            return output;
        }

        private void RenderReport(UserSettings Transform)
        {
            // build Report data into XML
            DataTable dtRecords = new ReportsHelper().GetPackagesBandwidthPaged(-1, Int32.MaxValue, 0, "", litStartDate.Text, litEndDate.Text);

            XmlSerializer xmlser = new XmlSerializer(dtRecords.GetType());
            string xmlString;
            using (StringWriter strwriter = new StringWriter())
            {
                xmlser.Serialize(strwriter, dtRecords);
                xmlString = strwriter.ToString();
            }

            // where the magic happens
            string TransformedXML = ReportTransform(Transform["Transform"], xmlString);

            string fileName = "BandwidthReport-" + litPeriod.Text.Replace(" ", "").Replace("/", "-").Replace(",","") + Transform["TransformSuffix"];

            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            Response.ContentType = Transform["TransformContentType"];

            Response.Write(TransformedXML);

            Response.End(); ;
        }



    }
}
