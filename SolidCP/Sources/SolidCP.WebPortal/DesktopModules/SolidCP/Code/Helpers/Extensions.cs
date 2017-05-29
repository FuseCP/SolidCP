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
using System.Web.UI.WebControls;

namespace SolidCP.Portal.Code.Helpers
{
	public static class Extensions
	{
		/// <summary>
		/// Sets active view by the specifed id
		/// </summary>
		/// <param name="mv"></param>
		/// <param name="viewId"></param>
		public static void SetActiveViewById(this MultiView mv, string viewId)
		{
			foreach (View tab in mv.Views)
			{
				if (tab.ID.Equals(viewId))
				{
					mv.SetActiveView(tab);
					//
					break;
				}
			}
		}

		/// <summary>
		/// Filters tabs list by hosting plan quotas assigned to the package.
		/// </summary>
		/// <param name="tl"></param>
		/// <param name="packageId"></param>
		/// <returns></returns>
		public static IEnumerable<Tab> FilterTabsByHostingPlanQuotas(this List<Tab> tl, int packageId)
		{
			return from t in tl where String.IsNullOrEmpty(t.Quota)
							|| PackagesHelper.CheckGroupQuotaEnabled(packageId, t.ResourceGroup, t.Quota)
						select t;
		}


        /// <summary>
        /// Adds the specified parameter to the Query String.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName">Name of the parameter to add.</param>
        /// <param name="paramValue">Value for the parameter to add.</param>
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return new Uri(uriBuilder.ToString());
        }
	}

	public class Tab
		{
			/// <summary>
			/// Gets or sets index of the tab
			/// </summary>
			public int Index { get; set; }

			/// <summary>
			/// Gets or sets id assosicated with the tab
			/// </summary>
			public string Id { get; set; }

			/// <summary>
			/// Gets or sets localized name of the tab
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets name of a hosting plan quota associated with the tab
			/// </summary>
			public string Quota { get; set; }

			/// <summary>
			/// Gets or sets resource group of a hosting plan quota associated with the tab
			/// </summary>
			public string ResourceGroup { get; set; }

			/// <summary>
			/// Gets or sets resource key associated with the tab for localization purposes
			/// </summary>
			public string ResourceKey { get; set; }

			/// <summary>
			/// Gets or sets identificator of the view control associated with the tab
			/// </summary>
			public string ViewId { get; set; }
		}
}
