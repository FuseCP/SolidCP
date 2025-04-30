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
using SolidCP.Providers.Database;
using SolidCP.Providers.Mail;
using System.Xml;
using System.Linq;

namespace SolidCP.Portal.ProviderControls
{
    public partial class SmarterMail100x_EditAccess : SolidCPControlBase, IMailEditDomainControl
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            PackageInfo info = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                LoadSelectedCountries();
                BindAvailableCountriesDropdown();
            }
        }

        public void BindItem(MailDomain item)
        {
            ddlAuthType.SelectedValue = item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_TYPE];

            string SelectedCountryCodes = item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_COUNTRIES];
            if (!string.IsNullOrEmpty(SelectedCountryCodes))
            {
                ViewState["SelectedCountryCodes"] = SelectedCountryCodes;
                LoadSelectedCountries();
            }
        }

        public void SaveItem(MailDomain item)
        {
            item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_TYPE] = ddlAuthType.SelectedValue;
                
            List<string> codesToSave = selectedCountries.Select(c => c.Code).ToList();
            item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_COUNTRIES] = string.Join(",", codesToSave);

            item[MailDomain.SMARTERMAIL100_SET_BLOCKED_COUNTRIES] = "1";
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

        [Serializable]
        public class Country
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }


    }
}