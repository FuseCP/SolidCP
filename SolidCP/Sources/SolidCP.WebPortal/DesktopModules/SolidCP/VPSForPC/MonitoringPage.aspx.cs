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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.VPSForPC
{
    public partial class MonitoringPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));
            cs.RegisterClientScriptInclude("jqueryui", ResolveUrl("~/JavaScript/jquery-ui-1.8.9.min.js"));

			if (Page.IsPostBack == false)
			{
				SetDefaultMonitoringTimeFrame();

				BindPerformanceValues();
			}
        }

		private void SetDefaultMonitoringTimeFrame()
		{
			if (String.IsNullOrEmpty(txtStartPeriod.Text))
			{
				txtStartPeriod.Text = DateTime.Now.AddDays(-1).ToShortDateString();
			}
			//
			if (String.IsNullOrEmpty(txtEndPeriod.Text))
			{
				txtEndPeriod.Text = DateTime.Now.ToShortDateString();
			}
		}

		private void BindPerformanceValues()
		{
			DateTime StartP = Convert.ToDateTime(txtStartPeriod.Text);
			DateTime EndP = Convert.ToDateTime(txtEndPeriod.Text);

			EndP = (EndP.CompareTo(DateTime.Now.Date) == 0 ? DateTime.Now : EndP);

			PerformanceType pt = PerformanceType.Processor;

			string charType = Page.Request.QueryString["chartType"];

			InitControls(charType, StartP, EndP);

			switch (charType)
			{
				case "Processor":
					pt = PerformanceType.Processor;
					break;
				case "Network":
					pt = PerformanceType.Network;
					break;
				case "Memory":
					pt = PerformanceType.Memory;
					break;
			}

			LoadChartData(ChartCounter, pt, StartP, EndP);
		}

		protected void operationTimer_Tick(object sender, EventArgs e)
		{
			BindPerformanceValues();
		}

        private void LoadChartData(Chart control, PerformanceType perfType, DateTime startPeriod, DateTime endPeriod)
        {
            PerformanceDataValue[] perfValues = ES.Services.VPSPC.GetPerfomanceValue(PanelRequest.ItemID, perfType, startPeriod, endPeriod);

            if (perfValues != null)
            {
                foreach (PerformanceDataValue item in perfValues)
                {
					//
					control.Series["series"].Points.AddXY(item.TimeSampled.ToString(), item.SampleValue);
                }
            }
        }

        private void InitControls(string charType, DateTime startPeriod, DateTime endPeriod)
        {
            ChartCounter.Titles.Add(charType);
            ChartCounter.Series["series"].ChartType = (charType.Equals("Processor") ? SeriesChartType.Line : SeriesChartType.SplineArea);
            //ChartCounter.Series["series"].IsValueShownAsLabel = true;
            ChartCounter.Series["series"].Color = (!charType.Equals("Memory") ? System.Drawing.Color.FromArgb(220, 65, 140, 240) : ChartCounter.Series["series"].Color);
            ChartCounter.Series["series"]["ShowMarkerLines"] = "True";

            ChartCounter.ChartAreas["chartArea"].AxisX.IsMarginVisible = true;
        }
    }
}
