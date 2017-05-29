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
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Portal.Code.Framework;

namespace SolidCP.Portal.UserControls.ScheduleTaskView
{
	/// <summary>
	/// Represents empty view which states that no configuration is availalbe for this schedule task.
	/// </summary>
	public partial class EmptyView : UserControl, ISchedulerTaskView
	{
		private List<ScheduleTaskParameterInfo> input;

		/// <summary>
		/// Sets scheduler task parameters on view.
		/// </summary>
		/// <param name="parameters">Parameters list to be set on view.</param>
		public virtual void SetParameters(ScheduleTaskParameterInfo[] parameters)
		{
			input = new List<ScheduleTaskParameterInfo>(parameters);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public virtual ScheduleTaskParameterInfo[] GetParameters()
		{
			return new ScheduleTaskParameterInfo[0];
		}

		/// <summary>
		/// Searches for parameter by its id.
		/// </summary>
		/// <param name="id">Parameter id.</param>
		/// <returns>Found parameter.</returns>
		protected ScheduleTaskParameterInfo FindParameterById(string id)
		{
			return input.Find(delegate(ScheduleTaskParameterInfo param)
						{
							return param.ParameterId == id;
						}
				);
		}

		/// <summary>
		/// Sets parameter's value to textbox control's text property.
		/// </summary>
		/// <param name="control">Control to set value to.</param>
		/// <param name="parameterName">Parameter name.</param>
		protected void SetParameter(TextBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = this.FindParameterById(parameterName);
			control.Text = String.IsNullOrEmpty(parameter.ParameterValue)
											? parameter.DefaultValue
											: parameter.ParameterValue;

		}

		/// <summary>
		/// Sets parameter's value to drop down list control's list and selected value.
		/// </summary>
		/// <param name="control">Control to set value to.</param>
		/// <param name="parameterName">Parameter name.</param>
		protected void SetParameter(DropDownList control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = this.FindParameterById(parameterName);
			control.Items.Clear();
			Utils.ParseGroup(parameter.DefaultValue).ForEach(delegate(KeyValuePair<string, string> i) { control.Items.Add(new ListItem(i.Key, i.Value)); });
			Utils.SelectListItem(control, parameter.ParameterValue);
		}

		/// <summary>
		/// Sets parameter's value to checkbox control's checked value.
		/// </summary>
		/// <param name="control">Control to set value to.</param>
		/// <param name="parameterName">Parameter name.</param>
		protected void SetParameter(CheckBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = this.FindParameterById(parameterName);
			control.Checked = String.IsNullOrEmpty(parameter.ParameterValue)
											? Convert.ToBoolean(parameter.DefaultValue)
											: Convert.ToBoolean(parameter.ParameterValue);
		}

		/// <summary>
		/// Gets text parameter value from textbox control.
		/// </summary>
		/// <param name="control">Control to get value from.</param>
		/// <param name="parameterName">Paramter name.</param>
		/// <returns>Parameter.</returns>
		protected ScheduleTaskParameterInfo GetParameter(TextBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = new ScheduleTaskParameterInfo();
			parameter.ParameterId = parameterName;
			parameter.ParameterValue = (control.Text.Length > 1000) ? control.Text.Substring(0, 1000) : control.Text;
			return parameter;
		}

		/// <summary>
		/// Gets text parameter value from drop down list control.
		/// </summary>
		/// <param name="control">Control to get value from.</param>
		/// <param name="parameterName">Paramter name.</param>
		/// <returns>Parameter.</returns>
		protected ScheduleTaskParameterInfo GetParameter(DropDownList control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = new ScheduleTaskParameterInfo();
			parameter.ParameterId = parameterName;
			parameter.ParameterValue = control.SelectedValue;
			return parameter;
		}

		/// <summary>
		/// Gets checked parameter value from textbox control.
		/// </summary>
		/// <param name="control">Control to get value from.</param>
		/// <param name="parameterName">Paramter name.</param>
		/// <returns>Parameter.</returns>
		protected ScheduleTaskParameterInfo GetParameter(CheckBox control, string parameterName)
		{
			ScheduleTaskParameterInfo parameter = new ScheduleTaskParameterInfo();
			parameter.ParameterId = parameterName;
			parameter.ParameterValue = control.Checked.ToString();
			return parameter;
		}

	}
}
