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
using System.Globalization;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SolidCP.LocalizationToolkit
{
	public partial class SelectLocaleForm : Form
	{
		public SelectLocaleForm()
		{
			InitializeComponent();
			LoadLocales();
		}

		private string BaseDirectory
		{
			get
			{
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
			}
		}

		private void LoadLocales()
		{
			Hashtable existingLocales = new Hashtable();
			string baseDir = this.BaseDirectory;
			string[] dirs = Directory.GetDirectories(baseDir);

			DataSet dsLocales = new DataSet();
			DataTable dt = new DataTable("Locales");
			dsLocales.Tables.Add(dt);
			DataColumn col1 = new DataColumn("Name", typeof(string));
			DataColumn col2 = new DataColumn("EnglishName", typeof(string));
			dt.Columns.AddRange(new DataColumn[] { col1, col2 });
			foreach (string dir in dirs)
			{
				try
				{
					string cultureName = Path.GetFileName(dir);
					CultureInfo ci = new CultureInfo(Path.GetFileName(dir));
					if (!existingLocales.ContainsKey(ci.Name))
						existingLocales.Add(ci.Name, ci.Name);
				}
				catch (ArgumentException) { }
			}
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.FrameworkCultures);
			foreach (CultureInfo ci in cultures)
			{
				if (!existingLocales.ContainsKey(ci.Name) && !ci.IsNeutralCulture)
				{
					dsLocales.Tables[0].Rows.Add(new object[] { ci.Name, ci.EnglishName });
				}
			}
			DataView dv = new DataView(dsLocales.Tables[0]);
			dv.Sort = "EnglishName";
			lstLocales.DataSource = dv;
			lstLocales.DisplayMember = "EnglishName";
			lstLocales.ValueMember = "Name";
			lstLocales.SelectedIndex = 0;
		}

		private string selectedLocale;

		public string SelectedLocale
		{
			get { return selectedLocale; }
			set { selectedLocale = value; }
		}

		private void OnOKClick(object sender, EventArgs e)
		{
			CloseForm();
		}

		private void lstLocales_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			CloseForm();
		}

		private void CloseForm()
		{

			this.SelectedLocale = (string)lstLocales.SelectedValue;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
