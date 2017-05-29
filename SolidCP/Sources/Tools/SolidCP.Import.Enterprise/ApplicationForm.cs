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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using System.DirectoryServices;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Import.Enterprise
{
	public partial class ApplicationForm : BaseForm
	{
		private string username;
		private string password;
		private OrganizationImporter importer;
		internal bool ImportStarted = false;
		

		public ApplicationForm()
		{
			InitializeComponent();
			CheckForIllegalCrossThreadCalls = false;
		}

		public void InitializeForm(string username, string password)
		{
			this.username = username;
			this.password = password;
			UserInfo info = UserController.GetUser(username);
			SecurityContext.SetThreadPrincipal(info);
			
			importer = new OrganizationImporter();

			Assembly assembly = Assembly.GetExecutingAssembly();
			this.Text += " v" + assembly.GetName().Version.ToString(3);
			UpdateForm();
		}



		private void OnBrowseSpace(object sender, EventArgs e)
		{
			SelectSpace();
		}

		private void SelectSpace()
		{
			SpaceForm form = new SpaceForm();
			form.InitializeForm(username, password);
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				Global.Space = form.SelectedSpace;
				txtSpace.Text = Global.Space.PackageName;
				LoadSpaceData(Global.Space);
			}
		}

		private void LoadSpaceData(PackageInfo packageInfo)
		{
			int serviceId = PackageController.GetPackageServiceId(packageInfo.PackageId, ResourceGroups.HostedOrganizations);
			ServiceInfo serviceInfo = ServerController.GetServiceInfo(serviceId);
			StringDictionary serviceSettings = ServerController.GetServiceSettingsAdmin(serviceId);
			Global.RootOU = serviceSettings["RootOU"];
			Global.PrimaryDomainController = serviceSettings["PrimaryDomainController"];
			Global.TempDomain = serviceSettings["TempDomain"];
			ServerInfo serverInfo = ServerController.GetServerById(serviceInfo.ServerId);
			Global.ADRootDomain = serverInfo.ADRootDomain;
            Global.NetBiosDomain = ActiveDirectoryUtils.GetNETBIOSDomainName(Global.ADRootDomain);
		}

		private void OnBrowseOU(object sender, EventArgs e)
		{
			SelectOU();
		}

		private void SelectOU()
		{
			if (string.IsNullOrEmpty(txtSpace.Text))
			{
				ShowWarning("Please select hosting space first.");
				return;
			}
			OUForm form = new OUForm();
			form.InitializeForm();
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				Global.OrgDirectoryEntry = form.DirectoryEntry;
				string orgId = (string)Global.OrgDirectoryEntry.Properties["name"].Value;
				txtOU.Text = form.DirectoryEntry.Path;
				Global.OrganizationId = orgId;


				LoadOrganizationData(Global.OrgDirectoryEntry);
			}
		}

        private void BindMailboxPlans(string orgId)
        {
            cbMailboxPlan.Items.Clear();
            cbMailboxPlan.Items.Add("<not set>");
            cbMailboxPlan.SelectedIndex = 0;

            Organization org = OrganizationController.GetOrganizationById(orgId);

            if (org == null)
            {
                List<Organization> orgs = ExchangeServerController.GetExchangeOrganizations(1, false);
                if (orgs.Count > 0)
                    org = orgs[0];
            }

            if (org != null)
            {
                int itemId = org.Id;
                List<ExchangeMailboxPlan> plans = ExchangeServerController.GetExchangeMailboxPlans(itemId, false);
                cbMailboxPlan.Items.AddRange(plans.ToArray());
            }

        }

		private void LoadOrganizationData(DirectoryEntry parent)
		{
			string orgId = (string)parent.Properties["name"].Value;
			txtOrgId.Text = orgId;
		
			Organization org = OrganizationController.GetOrganizationById(orgId);
			if (org != null)
			{
				rbCreateAndImport.Checked = false;
				rbImport.Checked = true;
				txtOrgName.Text = org.Name;
			}
			else
			{
				rbCreateAndImport.Checked = true;
				rbImport.Checked = false;
				txtOrgName.Text = orgId;
			}

            BindMailboxPlans(orgId);

			LoadOrganizationAccounts(parent);
		}

		private void LoadOrganizationAccounts(DirectoryEntry ou)
		{
			lvUsers.Items.Clear();
			ListViewItem item = null;
			string email;
			string type;
			string name;
			PropertyValueCollection typeProp;
			string ouName = (string)ou.Properties["name"].Value;
			foreach (DirectoryEntry child in ou.Children)
			{
				type = null;
				email = null;
				name = (string)child.Properties["name"].Value;

				//account type
				typeProp = child.Properties["msExchRecipientDisplayType"];

                int typeDetails = 0;
                PropertyValueCollection typeDetailsProp = child.Properties["msExchRecipientTypeDetails"];
                if (typeDetailsProp != null)
                {
                    if (typeDetailsProp.Value != null)
                    {
                        try
                        {
                            object adsLargeInteger = typeDetailsProp.Value;
                            typeDetails = (Int32)adsLargeInteger.GetType().InvokeMember("LowPart", System.Reflection.BindingFlags.GetProperty, null, adsLargeInteger, null);
                        }
                        catch { } // just skip
                    }
                }

				
				switch (child.SchemaClassName)
				{
					case "user":
						email = (string)child.Properties["userPrincipalName"].Value;

                        if (typeDetails == 4)
                        {
                            type = "Shared Mailbox";
                        }
                        else
                        {

                            if (typeProp == null || typeProp.Value == null)
                            {
                                type = "User";
                            }
                            else
                            {
                                int mailboxType = (int)typeProp.Value;

                                switch (mailboxType)
                                {
                                    case 1073741824:
                                        type = "User Mailbox";
                                        break;
                                    case 7:
                                        type = "Room Mailbox";
                                        break;
                                    case 8:
                                        type = "Equipment Mailbox";
                                        break;
                                }
                            }
                        }
						if (!string.IsNullOrEmpty(type))
						{

							item = new ListViewItem(name);
							item.ImageIndex = 0;
							item.Checked = true;
							item.Tag = child;
							item.SubItems.Add(email);
							item.SubItems.Add(type);
							lvUsers.Items.Add(item);
						}
						break;
					case "contact":
						if (typeProp != null && typeProp.Value != null && 6 == (int)typeProp.Value)
						{
							type = "Mail Contact";
							if (child.Properties["targetAddress"] != null)
							{
								email = (string)child.Properties["targetAddress"].Value;
								if (email != null && email.ToLower().StartsWith("smtp:"))
									email = email.Substring(5);
							}

						}

						if (!string.IsNullOrEmpty(type))
						{

							item = new ListViewItem(name);
							item.ImageIndex = 1;
							item.Checked = true;
							item.Tag = child;
							item.SubItems.Add(email);
							item.SubItems.Add(type);
							lvUsers.Items.Add(item);
						}
						break;
					case "group":
						if (child.Properties["mail"] != null)
							email = (string)child.Properties["mail"].Value;

                        bool isDistributionList = false;

                        if ((typeProp != null) && (typeProp.Value != null) && (1073741833 == (int)typeProp.Value))
                            isDistributionList = true;

                        if (typeDetails == 262144)
                            isDistributionList = true;

                        if (typeDetails == 0)
                            isDistributionList = true;

                        if (isDistributionList)
						{
							//Universal Security Group
                            type = "Distribution List";
							//email
							PropertyValueCollection proxyAddresses = child.Properties["proxyAddresses"];
							if (proxyAddresses != null)
							{
								foreach (string address in proxyAddresses)
								{
									if (address != null && address.StartsWith("SMTP:"))
									{
										email = address.Substring(5);
										break;
									}
								}
							}
						}

						if (!string.IsNullOrEmpty(type) && name != ouName)
						{

							item = new ListViewItem(name);
							item.ImageIndex = 2;
							item.Checked = true;
							item.Tag = child;
							item.SubItems.Add(email);
							item.SubItems.Add(type);
							lvUsers.Items.Add(item);
						}
						break;
				}
			}
		}
		

		private void OnDataChanged(object sender, EventArgs e)
		{
			UpdateForm();
		}

		private void UpdateForm()
		{
			if (string.IsNullOrEmpty(txtSpace.Text) ||
				string.IsNullOrEmpty(txtOU.Text))
			{
				btnStart.Enabled = false;
			}
			else
			{
				btnStart.Enabled = true;
			}
		}

		private void OnImportClick(object sender, EventArgs e)
		{
			List<DirectoryEntry> accounts = new List<DirectoryEntry>();
			foreach (ListViewItem item in lvUsers.Items)
			{
				if (item.Checked)
				{
					accounts.Add((DirectoryEntry)item.Tag);
				}
			}
			Global.SelectedAccounts = accounts;
			Global.OrganizationName = txtOrgName.Text;
			Global.ImportAccountsOnly = rbImport.Checked;
			Global.HasErrors = false;

            Global.defaultMailboxPlanId = 0;
            if (cbMailboxPlan.SelectedItem!=null)
            {
                ExchangeMailboxPlan plan = cbMailboxPlan.SelectedItem as ExchangeMailboxPlan;
                if (plan != null)
                    Global.defaultMailboxPlanId = plan.MailboxPlanId;

            }

			importer.Initialize(this.username, this);
			importer.Start();
			
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = ImportStarted;
		}

		private void OnCheckedChanged(object sender, EventArgs e)
		{
			txtOrgName.ReadOnly = !rbCreateAndImport.Checked;
		}

		private void OnSelectAllClick(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvUsers.Items)
			{
				item.Checked = true;
			}
		}

		private void OnDeselectAllClick(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvUsers.Items)
			{
				item.Checked = false;
			}
		}
	}
}
