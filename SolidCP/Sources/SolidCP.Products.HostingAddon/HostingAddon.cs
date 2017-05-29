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
	public class HostingAddon : ProvisioningControllerBase, IProvisioningController
	{
		public const string ROOT_SERVICE_ID = "RootServiceID";
		public const string PACKAGE_ID = "PackageID";
		public const string HOSTING_ADDON = "HostingAddon";
		public const string PACKAGE_ADDON_ID = "PackageAddonID";

		public string PackageAddonID
		{
			get { return ServiceSettings[PACKAGE_ADDON_ID]; }
			set { ServiceSettings[PACKAGE_ADDON_ID] = value; }
		}

		public HostingAddon(Service serviceInfo)
			: base(serviceInfo)
		{
		}

		#region IProvisioningController Members

		public void Activate()
		{
			int packageAddonId = 0;

			if (!Int32.TryParse(PackageAddonID, out packageAddonId))
				throw new Exception("Unable to parse Package Add-On ID: " + PackageAddonID);
			// try to get package addon
			PackageAddonInfo addon = PackageController.GetPackageAddon(packageAddonId);
			// check returned package
			if (addon != null)
			{
				// workaround for bug in GetPackageAddon routine
				addon.PackageAddonId = packageAddonId;
				// change package add-on status
				addon.StatusId = (int)PackageStatus.Active;
				// save package add-on changes
				PackageResult result = PackageController.UpdatePackageAddon(addon);
				// check returned result
				if (result.Result < 0)
					throw new Exception("Unable to activate Package Add-On: " + addon.PackageAddonId);
			}
		}

		public void Suspend()
		{
			int packageAddonId = 0;

			if (!Int32.TryParse(PackageAddonID, out packageAddonId))
				throw new Exception("Unable to parse Package Add-On ID: " + PackageAddonID);
			// try to get package addon
			PackageAddonInfo addon = PackageController.GetPackageAddon(packageAddonId);
			// check returned package
			if (addon != null)
			{
				// workaround for bug in GetPackageAddon routine
				addon.PackageAddonId = packageAddonId;
				// change package add-on status
				addon.StatusId = (int)PackageStatus.Suspended;
				// save package add-on changes
				PackageResult result = PackageController.UpdatePackageAddon(addon);
				// check returned result
				if (result.Result < 0)
					throw new Exception("Unable to suspend Package Add-On: " + addon.PackageAddonId);
			}
		}

		public void Cancel()
		{
			int packageAddonId = 0;

			if (!Int32.TryParse(PackageAddonID, out packageAddonId))
				throw new Exception("Unable to parse Package Add-On ID: " + PackageAddonID);
			// try to get package addon
			PackageAddonInfo addon = PackageController.GetPackageAddon(packageAddonId);
			// check returned package
			if (addon != null)
			{
				// workaround for bug in GetPackageAddon routine
				addon.PackageAddonId = packageAddonId;
				// change package add-on status
				addon.StatusId = (int)PackageStatus.Cancelled;
				// save package add-on changes
				PackageResult result = PackageController.UpdatePackageAddon(addon);
				// check returned result
				if (result.Result < 0)
					throw new Exception("Unable to cancel Package Add-On: " + addon.PackageAddonId);
			}
		}

		public void Order()
		{
			int rootServiceId = Utils.ParseInt(ServiceSettings[ROOT_SERVICE_ID], 0);

			// each add-on should have root service id assigned
			if (rootServiceId < 0)
			{
				throw new Exception(
					"Incorrect add-on settings. Root Service ID couldn't be found please review logs and correct this issue."
				);
			}

			// get root service settings
			KeyValueBunch rootSettings = ServiceController.GetServiceSettings(
				ServiceInfo.SpaceId,
				rootServiceId
			);

			// failed to load root service settings
			if (rootSettings == null)
				throw new Exception("Unable to load root service settings.");

			// add package add-on
			PackageAddonInfo addon = new PackageAddonInfo();

			// load Package ID
			int packageId = 0;
			if (!Int32.TryParse(rootSettings[PACKAGE_ID], out packageId))
				throw new Exception("Couldn't parse parent service settings: PackageID property. Parent Service ID: " + rootServiceId);

			// load Plan ID
			int hostingAddon = 0;
			if (!Int32.TryParse(ServiceSettings[HOSTING_ADDON], out hostingAddon))
				throw new Exception("Couldn't parse service settings: HostingAddon property. Service ID: " + ServiceInfo.ServiceId);

			addon.PackageId = packageId;
			addon.PlanId = hostingAddon;
			addon.Quantity = 1;
			addon.StatusId = (int)PackageStatus.Active;
			addon.PurchaseDate = DateTime.UtcNow;

			PackageResult result = PackageController.AddPackageAddon(addon);

			// failed to create package add-on
			if (result.Result < 0)
				throw new Exception("Unable to add package add-on. Status code: " + result.Result);
			
			// save service settings
			PackageAddonID = result.Result.ToString();
		}

		public void Rollback() {}

		#endregion
	}
}
