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
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using System.Collections.Generic;
using System.Xml;
using SolidCP.Providers.Mail;
using System.Linq;

namespace SolidCP.Portal
{
    public partial class SettingsMailPolicy : SolidCPControlBase, IUserSettingsEditorControl
    {
        public void BindSettings(UserSettings settings)
        {
            // accounts
            accountNamePolicy.Value = settings["AccountNamePolicy"];
            accountPasswordPolicy.Value = settings["AccountPasswordPolicy"];

            // general
            txtCatchAll.Text = settings["CatchAllName"];

            // Access
            ddlAuthType.SelectedValue = settings["AcessAuthTypePolicy"];

            string SelectedCountryCodes = settings["AccessSelectedCountry"];
            if (!string.IsNullOrEmpty(SelectedCountryCodes))
            {
                ViewState["SelectedCountryCodes"] = SelectedCountryCodes;
                LoadSelectedCountries();
            }

            if (!IsPostBack)
            {
                LoadSelectedCountries();
                BindAvailableCountriesDropdown();
            }

        }

        public void SaveSettings(UserSettings settings)
        {
            // accounts
            settings["AccountNamePolicy"] = accountNamePolicy.Value;
            settings["AccountPasswordPolicy"] = accountPasswordPolicy.Value;

            // databases
            settings["CatchAllName"] = txtCatchAll.Text.Trim();

            // Access
            settings["AcessAuthTypePolicy"] = ddlAuthType.SelectedValue;

            List<string> codesToSave = selectedCountries.Select(c => c.Code).ToList();
            settings["AccessSelectedCountry"] = string.Join(",", codesToSave);
        }

        private void BindAvailableCountriesDropdown()
        {
            List<Country> availableCountries = GetCountryList();

            availableCountries = availableCountries.Where(c => !selectedCountries.Any(sc => sc.Code == c.Code)).ToList();

            ddlAddCountry.DataSource = availableCountries;
            ddlAddCountry.DataTextField = "Name";
            ddlAddCountry.DataValueField = "Code";
            ddlAddCountry.DataBind();
        }

        private List<Country> GetCountryList()
        {
            string countriesPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Countries.config");
            XmlDocument xmlCountriesDoc = new XmlDocument();
            xmlCountriesDoc.Load(countriesPath);

            List<Country> countries = new List<Country>();

            XmlNodeList xmlCountries = xmlCountriesDoc.SelectNodes("/Countries/Country");

            foreach (XmlElement xmlCountry in xmlCountries)
            {
                countries.Add(new Country { Code = xmlCountry.GetAttribute("key"), Name = xmlCountry.GetAttribute("name") });
            }
            return countries;
        }

        private void LoadSelectedCountries()
        {
            selectedCountries.Clear();

            string selectedCodes = ViewState["SelectedCountryCodes"] as string;
            if (!string.IsNullOrEmpty(selectedCodes))
            {
                string[] codes = selectedCodes.Split(',');
                List<Country> allCountries = GetCountryList();

                foreach (string code in codes)
                {
                    Country country = allCountries.FirstOrDefault(c => c.Code == code);

                    if (country != null)
                    {
                        selectedCountries.Add(country);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Warning: Country code '{code}' not found in Countries.config.");
                    }
                }
            }
            BindSelectedCountriesGrid();
        }

        private void BindSelectedCountriesGrid()
        {
            gvSelectedCountries.DataSource = selectedCountries;
            gvSelectedCountries.DataBind();
        }

        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            string code = ddlAddCountry.SelectedValue;
            string name = ddlAddCountry.SelectedItem.Text;

            if (!selectedCountries.Any(c => c.Code == code))
            {
                selectedCountries.Add(new Country { Code = code, Name = name });
                BindSelectedCountriesGrid();
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            string codeToRemove = ((CPCC.StyleButton)sender).CommandArgument;
            selectedCountries.RemoveAll(c => c.Code == codeToRemove);
            BindSelectedCountriesGrid();
        }

        private List<Country> selectedCountries
        {
            get
            {
                List<Country> countries = ViewState["SelectedCountries"] as List<Country>;
                if (countries == null)
                {
                    countries = new List<Country>();
                    ViewState["SelectedCountries"] = countries;
                }
                return countries;
            }
            set { ViewState["SelectedCountries"] = value; }
        }

        [Serializable]
        public class Country
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}
