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

using SolidCP.EnterpriseServer;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	public class HostingPlan : ProvisioningControllerBase, IProvisioningController
	{
		public const string HOSTING_PLAN = "HostingPlan";
		public const string INITIAL_STATUS = "InitialStatus";
		public const string PACKAGE_ID = "PackageID";
		public const string USER_ROLE = "UserRole";
		public const string USER_STATUS = "UserStatus";

		private PackageStatus previousStatus;

		public HostingPlan(Service serviceInfo)
			: base(serviceInfo)
		{
		}

		#region IProvisioningController Members

		public void Activate()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				RememberCurrentStatus(packageId);

				int status = PackageController.ChangePackageStatus(packageId, PackageStatus.Active, false);

				if (status < 0)
					throw new Exception("Unable to change package status. Status code: " + status);
			}
		}

		public void Suspend()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				RememberCurrentStatus(packageId);

				int status = PackageController.ChangePackageStatus(packageId, PackageStatus.Suspended, false);

				if (status < 0)
					throw new Exception("Unable to change package status. Status code: " + status);
			}
		}

		public void Cancel()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				RememberCurrentStatus(packageId);

				int status = PackageController.ChangePackageStatus(packageId, PackageStatus.Cancelled, false);

				if (status < 0)
					throw new Exception("Unable to change package status. Status code: " + status);
			}
		}

		public void Order()
		{
			RememberCurrentStatus(PackageStatus.New);

			PackageResult result = PackageController.AddPackage(
				UserInfo.UserId,
				Convert.ToInt32(ServiceSettings[HOSTING_PLAN]),
				String.Format("My Space ({0})", UserInfo.Username),
				String.Empty,
				Convert.ToInt32(ServiceSettings[INITIAL_STATUS]),
				DateTime.Now,
				true
			);

			// throws an exception
			if (result.Result <= 0)
				throw new Exception("Couldn't add package to the user: " + UserInfo.Username);

			ServiceSettings[PACKAGE_ID] = result.Result.ToString();

			// update user details
			if (!String.IsNullOrEmpty(ServiceSettings[USER_ROLE]))
				UserInfo.Role = (UserRole)Enum.Parse(typeof(UserRole), ServiceSettings[USER_ROLE]);

			if (!String.IsNullOrEmpty(ServiceSettings[USER_STATUS]))
				UserInfo.Status = (UserStatus)Enum.Parse(typeof(UserStatus), ServiceSettings[USER_STATUS]);

			UserController.UpdateUser(UserInfo);
		}

		public void Rollback()
		{
			int packageId = Utils.ParseInt(ServiceSettings[PACKAGE_ID], -1);

			if (packageId > -1)
			{
				switch (previousStatus)
				{
					case PackageStatus.New: // delete package
						int removeResult = PackageController.DeletePackage(packageId);
						if (removeResult != 0)
							throw new Exception("Unable to rollback(remove) package. Status code: " + removeResult);
						break;
					case PackageStatus.Active: // return to active state
					case PackageStatus.Cancelled:
					case PackageStatus.Suspended:

						PackageInfo package = PackageController.GetPackage(packageId);
						package.StatusId = (int)previousStatus;

						PackageResult result = PackageController.UpdatePackage(package);

						if (result.Result != 0)
							throw new Exception("Unable to rollback(update) package. Status code: " + result.Result);

						break;
				}
			}
		}

		#endregion

		#region Helper routines

		private void RememberCurrentStatus(PackageStatus current)
		{
			previousStatus = current;
		}

		private void RememberCurrentStatus(int packageId)
		{
			PackageInfo package = PackageController.GetPackage(packageId);

			previousStatus = (PackageStatus)package.StatusId;
		}

		#endregion
	}
}
