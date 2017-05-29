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

namespace SolidCP.EnterpriseServer.Base.Scheduling
{
	/// <summary>
	/// Represents view configuration for a certain hosting environment.
	/// </summary>
	[Serializable]
	public class ScheduleTaskViewConfiguration
	{
		private string environment;
		private string description;
		private string taskId;
		private string configurationId;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduleTaskViewConfiguration"/> class.
		/// </summary>
		public ScheduleTaskViewConfiguration()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduleTaskViewConfiguration"/> class.
		/// </summary>
		/// <param name="environment">Hosting environment for the view.</param>
		/// <param name="description">Configuration details for the view.</param>
		public ScheduleTaskViewConfiguration(string environment, string description)
		{
			this.environment = environment;
			this.description = description;
		}

		/// <summary>
		/// Gets or sets owner task id.
		/// </summary>
		public string TaskId
		{
			get
			{
				return this.taskId;
			}
			set
			{
				this.taskId = value;
			}
		}

		/// <summary>
		/// Gets or sets configuration's id.
		/// </summary>
		public string ConfigurationId
		{
			get
			{
				return this.configurationId;
			}
			set
			{
				this.configurationId = value;
			}
		}

		/// <summary>
		/// Gets or sets hosting environment for the view.
		/// </summary>
		public string Environment
		{
			get
			{
				return this.environment;
			}
			set
			{
				this.environment = value;
			}
		}

		/// <summary>
		/// Gets or sets configuration details for the view.
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}
	}
}
