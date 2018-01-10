<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebsitesSSL.ascx.cs" Inherits="SolidCP.Portal.WebsitesSSL" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register Src="SkinControls/BootstrapDropDownList.ascx" TagName="BootstrapDropDownList" TagPrefix="bsddl" %>

<asp:UpdatePanel ID="MessageBoxUpdatePanel" runat="server" UpdateMode="Always">
	<contenttemplate>
		<scp:SimpleMessageBox id="messageBox" runat="server"></scp:SimpleMessageBox>
	</contenttemplate>
</asp:UpdatePanel>
<ajaxToolkit:TabContainer ID="TabContainer1" runat="server">
	<ajaxToolkit:TabPanel ID="tabInstalled" runat="server" Visible="false" Enabled="false" CssClass="nav nav-tabs">
		<ContentTemplate>
			<div class="Normal">
				<h2><asp:Localize runat="server" meta:resourcekey="headerInstalledCertificate"/></h2>
				<table>
					<tr>
						<td class="SubHead" style="width: 200px;">
							<asp:Localize runat="server" meta:resourcekey="sslDomain" /></td>
						<td class="Normal">
							<asp:Literal ID="lblInstalledDomain" runat="server" /></td>
					</tr>
					<tr>
						<td class="SubHead">
							<asp:Localize runat="server" meta:resourcekey="sslExpiry" /></td>
						<td class="Normal">
							<asp:Label ID="lblInstalledExpiration" CssClass="Normal" runat="server" /></td>
					</tr>
					<tr>
						<td class="SubHead">
							<asp:Localize runat="server" meta:resourcekey="sslBitLength" /></td>
						<td class="Normal">
							<asp:Literal ID="lblInstalledBits" runat="server" /></td>
					</tr>
					<tr id="trOrganization" runat="server">
						<td class="SubHead">
							<asp:Localize runat="server" meta:resourcekey="sslOrganization" /></td>
						<td class="Normal">
							<asp:Literal ID="lblInstalledOrganization" runat="server" /></td>
					</tr>
					<tr id="trOrganizationUnit" runat="server">
						<td class="SubHead">
							<asp:Localize runat="server" meta:resourcekey="sslOrganizationUnit" /></td>
						<td class="Normal">
							<asp:Literal ID="lblInstalledOU" runat="server" /></td>
					</tr>
					<tr id="trCountry" runat="server">
						<td class="SubHead">
							<asp:Localize ID="Localize1" runat="server" meta:resourcekey="sslCountry" /></td>
						<td class="Normal">
							<asp:Literal ID="lblInstalledCountry" runat="server" /></td>
					</tr>
					<tr id="trState" runat="server">
						<td class="SubHead">
							<asp:Localize ID="Localize2" runat="server" meta:resourcekey="sslState" /></td>
						<td class="Normal">
							<asp:Literal ID="lblinstalledState" runat="server" /></td>
					</tr>
					<tr id="trCity" runat="server">
						<td class="SubHead">
							<asp:Localize runat="server" meta:resourcekey="sslCity" /></td>
						<td class="Normal">
							<asp:Literal ID="lblInstalledCity" runat="server" /></td>
					</tr>
				</table>
			</div>
			<br />
			<asp:Button ID="btnRenew" runat="server" UseSubmitBehavior="true" meta:resourcekey="btnRenew"
				CssClass="btn btn-primary" Text="Renew" OnClick="btnRenew_Click" />&nbsp;&nbsp;
			<asp:Button ID="btnExportModal" runat="server" meta:resourcekey="btnExportModal"
				CssClass="btn btn-warning" Text="Export" />&nbsp;&nbsp;
			<asp:Button ID="btnDelete" runat="server" Text="Delete" meta:resourcekey="btnDelete"
				CssClass="btn btn-danger" OnClick="btnDelete_Click" />&nbsp;&nbsp;
			<asp:Panel ID="pnlPFXPassword" Style="display: none" runat="server">
                <div class="widget" style="max-width:100%;">
                <div class="widget-header clearfix">
								<h3><i class="fa fa-server"></i> <span><asp:Localize runat="server" meta:resourcekey="headerPFXPassword" /></span></h3>
                </div>
                                <div class="widget-content">
						<div class="FormFieldDescription">
							<asp:Localize runat="server" meta:resourcekey="PfxPassword" /></div>
						<div class="FormField">
							<asp:TextBox ID="txtPFXPass" ValidationGroup="pfxExport" runat="server" CssClass="form-control" TextMode="Password" />
							<asp:RequiredFieldValidator ID="valtxtPFXPass" runat="server" Display="Dynamic" ValidationGroup="pfxExport"
								ControlToValidate="txtPFXPass" meta:resourcekey="valtxtPFXPass" /></div>
						<div class="FormFieldDescription">
							<asp:Localize runat="server" meta:resourcekey="PfxPasswordConfirmation" /></div>
						<div class="FormField">
							<asp:TextBox ID="txtPFXPassConfirm" ValidationGroup="pfxExport" runat="server" CssClass="form-control" TextMode="Password" />
							<asp:CompareValidator ID="valtxtPFXPassConfirm" runat="server" ValidationGroup="pfxExport" 
								ControlToCompare="txtPFXPass" ControlToValidate="txtPFXPassConfirm" meta:resourcekey="valtxtPFXPassConfirm" /></div>
					</div>
					<div class="popup-buttons text-right">
						<asp:Button ID="btnExport" meta:resourcekey="btnExport" ValidationGroup="pfxExport"
							runat="server" OnClick="btnExport_Click" CssClass="btn btn-warning" UseSubmitBehavior="false"
							Text="Export" />&nbsp;&nbsp;
						<asp:Button ID="btnPFXExportCancel" meta:resourcekey="btnPFXExportCancel" runat="server"
							Text="Cancel" CssClass="btn btn-danger" />
					</div>
				</div>
			</asp:Panel>
			<ajaxToolkit:ModalPopupExtender ID="modalPfxPass" runat="server" TargetControlID="btnExportModal"
				PopupControlID="pnlPFXPassword" OkControlID="btnExport" BackgroundCssClass="modalBackground"
				DropShadow="false" CancelControlID="btnPFXExportCancel" />
		</ContentTemplate>
	</ajaxToolkit:TabPanel>
	<ajaxToolkit:TabPanel ID="tabCSR" runat="server" meta:resourcekey="tabNewCertificate" CssClass="nav nav-tabs">
		<ContentTemplate>
			<asp:Panel ID="SSLNotInstalled" runat="server" Visible="true">
                <div id="NoLE" class="col-sm-6">
                    <h3>
						<asp:Literal ID="LENotInstalledHeading" runat="server" meta:resourcekey="LENotInstalledHeading" /></h3>
                    <p class="Normal">
						<asp:Literal ID="LENotInstalledDescription" runat="server" meta:resourcekey="LENotInstalledDescription" /></p>
                    <div class="form-group text-center">
                        <CPCC:StyleButton ID="LEInstall" CssClass="btn btn-primary" runat="server" OnClick="LEInstallCertificate_Click"> <i class="fa fa-lock">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="LEInstallText" /></CPCC:StyleButton>
                    </div>
                </div>
				<div id="NoSSL" class="col-sm-6">
					<h3>
						<asp:Literal ID="SSLNotInstalledHeading" runat="server" meta:resourcekey="SSLNotInstalledHeading" /></h3>
					<p class="Normal">
						<asp:Literal ID="SSLNotInstalledDescription" runat="server" meta:resourcekey="SSLNotInstalledDescription" /></p>
				</div>
				<div class="form-group text-center">
					<asp:Button ID="btnShowpnlCSR" runat="server" meta:resourcekey="btnShowpnlCSR" CssClass="btn btn-primary"
						Text="Generate CSR" OnClick="btnShowpnlCSR_click" />
					<asp:Button ID="btnShowUpload" meta:resourcekey="btnShowUpload" CssClass="btn btn-primary"
						runat="server" OnClick="btnShowUpload_click" Text="Upload Certificate" />
				</div>
			</asp:Panel>
			<asp:Panel ID="SSLImport" runat="server" Visible="false">
				<div>
					<h2>
						<asp:Localize ID="SSLImportHeading" runat="server" meta:resourcekey="SSLImportHeading" /></h2>
					<p class="Normal">
						<asp:Localize ID="SSLImportDescription" runat="server" meta:resourcekey="SSLImportDescription" /></p>
					<asp:Button ID="btnImport" meta:resourcekey="btnImport" CssClass="Button1" runat="server" OnClick="btnImport_click" />
			        <asp:Button ID="btnDeleteAll" runat="server" Text="Delete" meta:resourcekey="btnDelete"
				        CssClass="Button1" OnClick="btnDeleteAll_Click" />
				</div>
			</asp:Panel>
			<asp:Panel ID="pnlCSR" runat="server" Visible="false">
				<h2>
					<asp:Localize runat="server" meta:resourcekey="GenerateCSR" /></h2>
				<table style="width: 100%;">
	                <tr>
						<td class="SubHead">
							<asp:Localize ID="SelectCertType" runat="server" meta:resourcekey="SelectCertType" /></td>
		                <td class="NormalBold" ><asp:DropDownList id="ddlbSiteCertificate" GroupName="Content" Runat="server" Checked="True"></asp:DropDownList></td>
					<tr>
						<td class="SubHead">
							<asp:Localize ID="sslBitLength" runat="server" meta:resourcekey="sslBitLength" /></td>
						<td class="Normal">
							<asp:DropDownList ID="lstBits" runat="server">
								<asp:ListItem>1024</asp:ListItem>
								<asp:ListItem Selected="True">2048</asp:ListItem>
								<asp:ListItem>4096</asp:ListItem>
							</asp:DropDownList></td>
					</tr>
					<tr>
						<td class="SubHead">
							<asp:Localize ID="sslOrganization" runat="server" meta:resourcekey="sslOrganization" /></td>
						<td class="Normal">
							<asp:TextBox ID="txtCompany" CssClass="form-control" runat="server" /><asp:RequiredFieldValidator ID="SSLCompanyReq" Display="Dynamic" ValidationGroup="SSL" runat="server"
								ControlToValidate="txtCompany" ErrorMessage="*" /></td>
					</tr>
					<tr>
						<td class="SubHead">
							<asp:Localize ID="sslOrganizationUnit" runat="server" meta:resourcekey="sslOrganizationUnit" /></td>
						<td class="Normal">
							<asp:TextBox ID="txtOU" CssClass="form-control" runat="server" /></td>
					</tr>
					<tr>
						<td class="SubHead">
							<asp:Localize ID="sslCountry" runat="server" meta:resourcekey="sslCountry" /></td>
						<td class="Normal">
							<asp:dropdownlist runat="server" id="lstCountries" cssclass="form-control" AutoPostBack="true" 
								OnSelectedIndexChanged="lstCountries_SelectedIndexChanged" width="200px" /></td>
					</tr>
					<tr>
						<td class="SubHead">
							<asp:Localize ID="sslState" runat="server" meta:resourcekey="sslState" /></td>
						<td class="Normal">
							<asp:TextBox id="txtState" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
							<asp:DropDownList ID="ddlStates" Runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control"
								Width="200px" Visible="false" />
							<asp:RequiredFieldValidator ID="SSLSSLStateReq" ValidationGroup="SSL" runat="server"
								ControlToValidate="txtState" Display="Dynamic" ErrorMessage="*" /></td>
					</tr>
					<tr>
						<td class="SubHead">
							<asp:Localize ID="sslCity" runat="server" meta:resourcekey="sslCity" /></td>
						<td class="Normal">
							<asp:TextBox ID="txtCity" CssClass="form-control" runat="server" />
							<asp:RequiredFieldValidator ID="SSLCityReq" ValidationGroup="SSL" runat="server"
								ControlToValidate="txtCity" ErrorMessage="*" /></td>
					</tr>
				</table>
				<br />
				<asp:Button ID="btnCSR" meta:resourcekey="btnCSR" runat="server" CssClass="btn btn-primary"
					Text="Generate CSR" ValidationGroup="SSL" OnClick="btnCSR_Click" />&nbsp;&nbsp;
				<asp:Button ID="btnRenCSR" meta:resourcekey="btnRenCSR" runat="server" CssClass="btn btn-warning"
					Text="Generate CSR" ValidationGroup="SSL" OnClick="btnRenCSR_Click" Visible="false" />
			</asp:Panel>
			<asp:Panel ID="pnlShowUpload" runat="server" Visible="false">
				<div class="FormBody">
					<div class="FormField">
						<asp:FileUpload ID="upPFX" runat="server"></asp:FileUpload></div>
					<div class="FormFieldDescription">
						<asp:Localize runat="server" meta:resourcekey="lblPFXInstallPassword" /></div>
					<div class="FormField">
						<asp:TextBox ID="txtPFXInstallPassword" runat="server" TextMode="Password" CssClass="form-control" />
						<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPFXInstallPassword" 
							Display="Dynamic" ValidationGroup="InstallPfxGrp" ErrorMessage="*" /></div>
					<br />
					<asp:Button CssClass="btn btn-primary" ID="btnInstallPFX" runat="server" Text="Install" OnClick="btnInstallPFX_Click"
						meta:resourcekey="btnInstallPFX" ValidationGroup="InstallPfxGrp" />
				</div>
			</asp:Panel>
			<asp:Panel ID="pnlInstallCertificate" runat="server" Visible="false">
				<div class="Normal">
					<h2>
						<asp:Localize ID="InstallCSRHeading" runat="server" meta:resourcekey="InstallCSRHeading" /></h2>
					<p>
						<asp:Localize ID="InstallCSRDescription" runat="server" meta:resourcekey="InstallCSRDescription" /></p>
					<asp:Localize ID="sslCSR" runat="server" meta:resourcekey="sslCSR" />:<br />
					<asp:TextBox ID="txtCSR" runat="server" CssClass="form-control" Style="text-align: left; font-family: Courier New"
						Rows="25" TextMode="MultiLine" ReadOnly="True" Columns="65" Wrap="false" onfocus="this.select();"></asp:TextBox>
					<br />
					<br />
					<asp:Button ID="btnRegenCSR" CssClass="btn btn-primary" runat="server" meta:resourcekey="btnRegenCSR"
						Text="Generate New CSR" OnClick="btnRegenCSR_Click" OnClientClick="return confirm('Are you Sure? This will delete the current request.');" />
					<br />
					<p>
						<asp:Localize ID="InstallCSRCertificate" runat="server" meta:resourcekey="InstallCSRCertificate" />
					</p>
					<asp:Localize ID="sslCertificate" runat="server" meta:resourcekey="sslCertificate" />:<br />
					<asp:TextBox ID="txtCertificate" CssClass="form-control" runat="server" Rows="30" Columns="65" Wrap="false"
						Style="text-align: left; font-family: Courier New" TextMode="MultiLine" ReadOnly="False"></asp:TextBox>
					<br />
					<br />
					<asp:Button ID="btnInstallCertificate" meta:resourcekey="btnInstallCertificate" runat="server"
						CssClass="btn btn-primary" Text="Install" OnClick="btnInstallCertificate_Click" />&nbsp;&nbsp;
					<asp:Button ID="btnCancelRequest" runat="server" OnClientClick="return confirm('Are you Sure? This will delete the current request.');"
						CssClass="btn btn-warning" Text="Cancel request" OnClick="btnCancelRequest_Click" />
				</div>
			</asp:Panel>
		</ContentTemplate>
	</ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>