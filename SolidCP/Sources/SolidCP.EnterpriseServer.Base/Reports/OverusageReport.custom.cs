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
using System.Data;

namespace SolidCP.EnterpriseServer.Base.Reports
{
	public partial class OverusageReport
	{
		#region BandwidthOverusageRow Extension Functions
		public partial class BandwidthOverusageRow
		{

			/// <summary>
			/// Creates a <see cref="BandwidthOverusageRow"/> using the <see cref="OverusageReport"/>
			/// DataSet from a row that contains information about bandwidth usage.
			/// </summary>
			/// <param name="report">Instance of <see cref="OverusageReport"/> class.</param>
			/// <param name="hostingSpaceDataRow">DataRow with bandwidth information.</param>
			/// <returns><see cref="BandwidthOverusageRow"/>.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/> or either <paramref name="hostingSpaceDataRow"/> is <code>null</code>.</exception>
			public static BandwidthOverusageRow CreateFromHostingSpaceDataRow(OverusageReport report, DataRow hostingSpaceDataRow)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}
				if (hostingSpaceDataRow == null)
				{
					throw new ArgumentNullException("hostingSpaceDataRow");
				}

				BandwidthOverusageRow row = report.BandwidthOverusage.NewBandwidthOverusageRow();

				row.HostingSpaceId = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceDataRow["PackageID"].ToString(), 0);
				row.Allocated = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceDataRow["QuotaValue"].ToString(), 0);
				row.Used = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceDataRow["Bandwidth"].ToString(), 0);
				row.Usage = (row.Used - row.Allocated);

				return row;
			}

			/// <summary>
			/// Creates <see cref="BandwidthOverusageRow"/> using <see cref="PackageInfo"/> as a data source.
			/// </summary>
			/// <param name="report">Current <see cref="OverusageReport"/> dataset.</param>
			/// <param name="packageInfo"><see cref="PackageInfo"/> instance.</param>
			/// <returns><see cref="BandwidthOverusageRow"/> instance.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/> or either <paramref name="packageInfo"/> is <code>null</code>.</exception>
			public static BandwidthOverusageRow CreateFromPackageInfo(OverusageReport report, PackageInfo packageInfo)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}

				if (packageInfo == null)
				{
					throw new ArgumentNullException("packageInfo");
				}

				BandwidthOverusageRow row = report.BandwidthOverusage.NewBandwidthOverusageRow();

				row.HostingSpaceId = packageInfo.PackageId;
				row.Allocated = packageInfo.BandWidthQuota;
				row.Used = packageInfo.BandWidth;
				row.Usage = (row.Used - row.Allocated);

				return row;
			}

		}
		#endregion

		#region OverusageDetailsRow Extension methods
		public partial class OverusageDetailsRow
		{
			/// <summary>
			/// Creates <see cref="OverusageDetailsRow"/> using the information about Hosting Space disk space.
			/// </summary>
			/// <param name="report"><see cref="OverusageReport"/> data set. Current report.</param>
			/// <param name="packageDiskspaceRow"><see cref="DataRow"/> containing the Hosting Space disk space information.</param>
			/// <param name="hostingSpaceId">Hosting Space id.</param>
			/// <param name="overusageType">Type of overusage, can be Diskspace, Bandwidth, etc.</param>
			/// <returns>An instance of <see cref="OverusageDetailsRow"/>.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/>, <paramref name="packageDiskspaceRow"/> or <paramref name="overusageType"/> is null.</exception>
			/// <exception cref="ArgumentOutOfRangeException">When <paramref name="hostingSpaceId"/> less then 1.</exception>
			public static OverusageDetailsRow CreateFromPackageDiskspaceRow(OverusageReport report, DataRow packageDiskspaceRow, long hostingSpaceId, string overusageType)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}
				if (packageDiskspaceRow == null)
				{
					throw new ArgumentNullException("packageDiskspaceRow");
				}
				if (String.IsNullOrEmpty(overusageType))
				{
					throw new ArgumentNullException("overusageType");
				}

				if (hostingSpaceId < 1)
				{
					throw new ArgumentOutOfRangeException(
						  "hostingSpaceId"
						, String.Format(
							  "Hosting Space Id cannot be less then 1, however it is {0}."
							, hostingSpaceId
						  )
					);
				}

				OverusageDetailsRow row = report.OverusageDetails.NewOverusageDetailsRow();

				row.HostingSpaceId = hostingSpaceId;
				row.OverusageType = overusageType;
				row.ResourceGroupName = OverusageReportUtils.GetStringOrDefault(packageDiskspaceRow, "GroupName", "Files");
				row.Used = OverusageReportUtils.GetLongValueOrDefault(packageDiskspaceRow["Diskspace"].ToString(), 0);
				row.Allowed = 0;

				return row;
			}

			/// <summary>
			/// Creates <see cref="OverusageDetailsRow"/> using information about Hosting Space bandwidth.
			/// </summary>
			/// <param name="report">Current <see cref="OverusageReport"/> instance.</param>
			/// <param name="bandwidthRow"><see cref="DataRow"/> containing information about Hosting Space bandwidth.</param>
			/// <param name="hostingSpaceId">Hosting Space Id.</param>
			/// <param name="overusageType">Type of overusage. Diskspace, Bandwidth, etc.</param>
			/// <returns><see cref="OverusageDetailsRow"/> filled from <paramref name="bandwidthRow"/>.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/>, <paramref name="bandwidthRow"/> or <paramref name="overusageType"/> is null.</exception>
			/// <exception cref="ArgumentOutOfRangeException">When <paramref name="hostingSpaceId"/> less then 1.</exception>
			public static OverusageDetailsRow CreateFromBandwidthRow(OverusageReport report, DataRow bandwidthRow, long hostingSpaceId, string overusageType)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}
				if (bandwidthRow == null)
				{
					throw new ArgumentNullException("bandwidthRow");
				}
				if (String.IsNullOrEmpty(overusageType))
				{
					throw new ArgumentNullException("overusageType");
				}

				if (hostingSpaceId < 1)
				{
					throw new ArgumentOutOfRangeException(
						  "hostingSpaceId"
						, String.Format(
							  "Hosting Space Id cannot be less then 1, however it is {0}."
							, hostingSpaceId
						  )
					);
				}

				OverusageDetailsRow row = report.OverusageDetails.NewOverusageDetailsRow();

				row.HostingSpaceId = hostingSpaceId;
				row.OverusageType = overusageType;
				row.ResourceGroupName = OverusageReportUtils.GetStringOrDefault(bandwidthRow, "GroupName", "Files");
				row.Used = OverusageReportUtils.GetLongValueOrDefault(bandwidthRow["MegaBytesSent"].ToString(), 0);
				row.AdditionalField = OverusageReportUtils.GetLongValueOrDefault(bandwidthRow["MegaBytesReceived"].ToString(), 0);
				row.Allowed = 0;

				return row;
			}
		}
		#endregion

		#region HostingSpaceRow Extension functions	
		public partial class HostingSpaceRow
		{
			/// <summary>
			/// Returns a package id
			/// </summary>
			/// <param name="hostingSpaceRow"><see cref="DataRow"/> containing information about Hosting Space</param>
			/// <returns>Hosting space ID.</returns>
			/// <exception cref="ArgumentOutOfRangeException">When DataRow does not contain PackageID field or this field contains value less then 1.</exception>
			public static long GetPackageId(DataRow hostingSpaceRow)
			{
				long hostingSpaceId = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceRow["PackageID"].ToString(), 0);
				if (hostingSpaceId < 1)
				{
					throw new ArgumentOutOfRangeException(
						  "hostingSpaceId"
						, String.Format(
							  "PackageID field is either empty or contains incorrect value. Received package ID value: {0}."
							, hostingSpaceId
							)
						);
				}

				return hostingSpaceId;
			}

			/// <summary>
			/// Creates <see cref="HostingSpaceRow"/> using <see cref="PackageInfo"/>.
			/// </summary>
			/// <param name="report">Current <see cref="OverusageReport"/>.</param>
			/// <param name="packageInfo">Information about Hosting Space</param>
			/// <param name="userInfo">Information about user <paramref name="packageInfo"/> belongs to.</param>
			/// <param name="serverInfo">Information about server. Physical storage of a hosting space described in <paramref name="packageInfo"/></param>
			/// <param name="isDiskspaceOverused">Indicates whether disk space is overused.</param>
			/// <param name="isBandwidthOverused">Indicates whether bandwidht is overused.</param>
			/// <param name="packageFullTree">File system -like path of the location of a hosting space described in <paramref name="packageInfo"/></param>
			/// <returns><see cref="HostingSpaceRow"/> instance.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/>, <paramref name="packageInfo"/>, <paramref name="userInfo"/> or <paramref name="serverInfo"/> is null.</exception>
			public static HostingSpaceRow CreateFromPackageInfo(OverusageReport report, PackageInfo packageInfo, UserInfo userInfo, ServerInfo serverInfo,bool isDiskspaceOverused, bool isBandwidthOverused, string packageFullTree)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}
				if (packageInfo == null)
				{
					throw new ArgumentNullException("packageInfo");
				}
				if (userInfo == null)
				{
					throw new ArgumentNullException("userInfo");
				}
				if (serverInfo == null)
				{
					throw new ArgumentNullException("serverInfo");
				}


				HostingSpaceRow row = report.HostingSpace.NewHostingSpaceRow();

				row.IsBandwidthOverused = isBandwidthOverused;
				row.IsDiskspaceOverused = isDiskspaceOverused;

				row.HostingSpaceName = packageInfo.PackageName;
				row.UserEmail = userInfo.Username;
				row.UserName = userInfo.Email;
				row.ChildSpaceQuantity = 1;
				row.UserId = packageInfo.UserId;
				row.HostingSpaceId = packageInfo.PackageId;
				row.Status = packageInfo.StatusId.ToString();
				row.HostingSpaceFullTreeName = packageFullTree;
				row.HostingSpaceCreationDate = packageInfo.PurchaseDate;
				row.Location = serverInfo.ServerName;

				return row;
			}

			/// <summary>
			/// Creating <see cref="HostingSpaceRow"/> from DataRow containing data about Hosting Space
			/// </summary>
			/// <param name="report">Current <see cref="OverusageReport"/></param>
			/// <param name="hostingSpaceRow"><see cref="DataRow"/> containing information about Hosting Space.</param>
			/// <param name="isDiskspaceOverused">Indicates whether disk space is overused.</param>
			/// <param name="isBandwidthOverused">Indicates whether bandwidth is overusaed.</param>
			/// <param name="packageFullTree">File system -like path to hosting space described by <paramref name="hostingSpaceRow"/></param>
			/// <returns><see cref="HostingSpaceRow"/> instance.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/> or <paramref name="hostingSpaceRow"/> is null.</exception>
			public static HostingSpaceRow CreateFromHostingSpacesRow(OverusageReport report, DataRow hostingSpaceRow, bool isDiskspaceOverused, bool isBandwidthOverused, string packageFullTree)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}
				if (hostingSpaceRow == null)
				{
					throw new ArgumentNullException("hostingSpaceRow");
				}

				HostingSpaceRow row = report.HostingSpace.NewHostingSpaceRow();

				row.IsBandwidthOverused = isBandwidthOverused;
				row.IsDiskspaceOverused = isDiskspaceOverused;

				row.HostingSpaceName = OverusageReportUtils.GetStringOrDefault(hostingSpaceRow, "PackageName", String.Empty);
				row.UserEmail = OverusageReportUtils.GetStringOrDefault(hostingSpaceRow, "Email", String.Empty);
				row.UserName = OverusageReportUtils.GetStringOrDefault(hostingSpaceRow, "Username", String.Empty);
				row.UserId = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceRow["UserId"].ToString(), 0);
				row.HostingSpaceId = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceRow["PackageID"].ToString(), 0);
				row.ChildSpaceQuantity = (int)OverusageReportUtils.GetLongValueOrDefault(hostingSpaceRow["PackagesNumber"].ToString(), 0);
				row.Status = hostingSpaceRow["StatusID"].ToString();
				row.HostingSpaceFullTreeName = packageFullTree;

				return row;
			}

			/// <summary>
			/// Verify whether <paramref name="hostingSpacesRow"/> contains child spaces.
			/// </summary>
			/// <param name="hostingSpacesRow"><see cref="DataRow"/> containing information about Hosting Space.</param>
			/// <returns>True it Hosting space contains child spaces. Otherwise, False.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="hostingSpacesRow"/> is null.</exception>
			public static bool IsContainChildSpaces(DataRow hostingSpacesRow)
			{
				if (hostingSpacesRow == null)
				{
					throw new ArgumentNullException("hostingSpacesRow");
				}

				return OverusageReportUtils.GetLongValueOrDefault(hostingSpacesRow["PackagesNumber"].ToString(), 0) > 0;
			}

			/// <summary>
			/// Verify is disk space is overused by a Hosting Space.
			/// </summary>
			/// <param name="hostingSpacesRow"><see cref="DataRow"/> containing information about Hosting Space.</param>
			/// <returns>True, if Hosting Space overuses disk space quota. Otherwise, False.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="hostingSpacesRow"/> is null.</exception>
			public static bool VerifyIfDiskspaceOverused(DataRow hostingSpacesRow)
			{
				if (hostingSpacesRow == null)
				{
					throw new ArgumentNullException("hostingSpacesRow");
				}

				long
					allocated = OverusageReportUtils.GetLongValueOrDefault(hostingSpacesRow["QuotaValue"].ToString(), 0),
					used = OverusageReportUtils.GetLongValueOrDefault(hostingSpacesRow["Diskspace"].ToString(), 0);

				return (used > allocated);
			}

			/// <summary>
			/// Vefiry if bandwidth is overused by Hosting Space
			/// </summary>
			/// <param name="hostingSpacesRow"><see cref="DataRow"/> containing bandwidth information about Hosting Space.</param>
			/// <returns>True, if bandwidth is overused by a Hosting Space. Otherwise, False.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="hostingSpacesRow"/> is null.</exception>
			public static bool VerifyIfBandwidthOverused(DataRow hostingSpacesRow)
			{
				if (hostingSpacesRow == null)
				{
					throw new ArgumentNullException("hostingSpacesRow");
				}

				long
					allocated = OverusageReportUtils.GetLongValueOrDefault(hostingSpacesRow["QuotaValue"].ToString(), 0),
					used = OverusageReportUtils.GetLongValueOrDefault(hostingSpacesRow["Bandwidth"].ToString(), 0);

				return (used > allocated);
			}
		}
		#endregion

		#region DiskspaceOverusageRow extension functions
		public partial class DiskspaceOverusageRow
		{
			/// <summary>
			/// Creates <see cref="DiskspaceOverusageRow"/> from Hosting Space row.
			/// </summary>
			/// <param name="report">Current <paramref name="OverusageReport"/></param>
			/// <param name="hostingSpaceRow"><see cref="DataRow"/> containing Hosting Space information.</param>
			/// <returns><see cref="DiskspaceOverusageRow"/> instance.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/> or <paramref name="hostingSpaceRow"/> is null.</exception>
			public static DiskspaceOverusageRow CreateFromHostingSpacesRow(OverusageReport report, DataRow hostingSpaceRow)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}
				if (hostingSpaceRow == null)
				{
					throw new ArgumentNullException("hostingSpaceRow");
				}

				DiskspaceOverusageRow row = report.DiskspaceOverusage.NewDiskspaceOverusageRow();

				row.HostingSpaceId = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceRow["PackageID"].ToString(), 0);
				row.Allocated = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceRow["QuotaValue"].ToString(), 0);
				row.Used = OverusageReportUtils.GetLongValueOrDefault(hostingSpaceRow["Diskspace"].ToString(), 0);
				row.Usage = (row.Used - row.Allocated);

				return row;
			}

			/// <summary>
			/// Creates <see cref="DiskspaceOverusageRow"/> using <see cref="PackageInfo"/> information.
			/// </summary>
			/// <param name="report">Current <see cref="OverusageReport"/></param>
			/// <param name="packageInfo">Hosting Space information.</param>
			/// <returns><see cref="DiskspaceOverusageRow"/> instance.</returns>
			/// <exception cref="ArgumentNullException">When <paramref name="report"/> or <paramref name="packageInfo"/> is null.</exception>
			public static DiskspaceOverusageRow CreateFromPackageInfo(OverusageReport report, PackageInfo packageInfo)
			{
				if (report == null)
				{
					throw new ArgumentNullException("report");
				}
				if (packageInfo == null)
				{
					throw new ArgumentNullException("packageInfo");
				}

				DiskspaceOverusageRow row = report.DiskspaceOverusage.NewDiskspaceOverusageRow();

				row.HostingSpaceId = packageInfo.PackageId;
				row.Allocated = packageInfo.DiskSpaceQuota;
				row.Used = packageInfo.DiskSpace;
				row.Usage = (row.Used - row.Allocated);

				return row;
			}
		}
		#endregion
	}
}
