<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationCreateOrganization.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationCreateOrganization" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="OrganizationAdd48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Welcome"></asp:Localize>
				</h3>
                        </div>

				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locOrgName" runat="server" meta:resourcekey="locOrgName" Text="Organization Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtOrganizationName" runat="server" CssClass="form-control"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireOrgName" runat="server" meta:resourcekey="valRequireOrgName" ControlToValidate="txtOrganizationName"
									ErrorMessage="Enter Organization Name" ValidationGroup="CreateOrganization" Display="Dynamic" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locOrganizationID" runat="server" meta:resourcekey="locOrganizationID" Text="Organization ID: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtOrganizationID" runat="server" CssClass="form-control" 
                                    MaxLength="128"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequiretxtOrganizationID" runat="server" meta:resourcekey="valRequiretxtOrganizationID" ControlToValidate="txtOrganizationID"
									ErrorMessage="Enter Organization ID" ValidationGroup="CreateOrganization" Display="Dynamic" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
								<asp:RegularExpressionValidator ID="valRequireCorrectOrgID" runat="server"
									ErrorMessage="Please enter valid organization ID" ControlToValidate="txtOrganizationID"
										Display="Dynamic" ValidationExpression="[a-zA-Z0-9.-]{1,128}" meta:resourcekey="valRequireCorrectOrgID"
										ValidationGroup="CreateOrganization">*</asp:RegularExpressionValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locDomainName" runat="server" meta:resourcekey="locDomainName" Text="Domain Name:"></asp:Localize></td>
							<td>
                            <asp:DropDownList id="ddlDomains" runat="server" CssClass="NormalTextBox" DataTextField="DomainName" DataValueField="DomainID" style="vertical-align:middle;"></asp:DropDownList>
							</td>
						</tr>
					</table>

				</div>

				<div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" ValidationGroup="CreateOrganization" OnClientClick="ShowProgressDialog('Creating Organization...');"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateOrganization" />
				</div>