<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeContactGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeContactGeneralSettings" %>
<%@ Register Src="UserControls/CountrySelector.ascx" TagName="CountrySelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/ContactTabs.ascx" TagName="ContactTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeContact48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Contact"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                        </h3>
                </div>
				<div class="panel-body form-horizontal">
				    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:ContactTabs id="tabs" runat="server" SelectedTab="contact_settings" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<table class="col-sm-12">
						<tr>
							<td class="col-sm-2"><asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" ValidationGroup="CreateMailbox"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
									ErrorMessage="Enter Display Name" ValidationGroup="EditContact" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="col-sm-2"><asp:Localize ID="locEmail" runat="server" meta:resourcekey="locEmail" Text="E-mail Address: *"></asp:Localize></td>
							<td>
							    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valRequireAccount" runat="server" meta:resourcekey="valRequireAccount" ControlToValidate="txtEmail"
									ErrorMessage="Enter E-mail address" ValidationGroup="EditContact" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
								<asp:RegularExpressionValidator id="valCorrectEmail" runat="server" meta:resourcekey="valCorrectEmail" Display="Dynamic" ValidationGroup="EditContact" ErrorMessage="Enter correct e-mail address" ControlToValidate="txtEmail" ValidationExpression="^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$">*</asp:RegularExpressionValidator>
                            </td>
						</tr>
						<tr>
					        <td class="col-sm-2"><asp:Localize ID="locMAPIRichTextFormat" runat="server" meta:resourcekey="locMAPIRichTextFormat" Text="Use MAPI rich text format:"></asp:Localize></td>
					        <td><asp:DropDownList runat="server" ID="ddlMAPIRichTextFormat" Cssclass="form-control"/></td>
					    </tr>
						<tr>
						    <td></td>
						    <td>
						        <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
						        <br />
						        <br />
						    </td>
						</tr>
						
						<tr>
							<td class="col-sm-2"><asp:Localize ID="locFirstName" runat="server" meta:resourcekey="locFirstName" Text="First Name:"></asp:Localize></td>
							<td>
                                <div class="form-inline">
								<asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
								&nbsp;
								<asp:Localize ID="locInitials" runat="server" meta:resourcekey="locInitials" Text="Middle Initial:" />&nbsp;
								<asp:TextBox ID="txtInitials" runat="server" CssClass="form-control"></asp:TextBox>&nbsp;
                                <asp:Localize ID="locLastName" runat="server" meta:resourcekey="locLastName" Text="Last Name:"></asp:Localize>&nbsp;
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                 </div>
							</td>
						</tr>
					</table>

					<table class="col-sm-12">
					    <tr>
						    <td class="col-sm-2"><asp:Localize ID="locNotes" runat="server" meta:resourcekey="locNotes" Text="Notes:"></asp:Localize></td>
						    <td>
							    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
						    </td>
					    </tr>					    
					</table>
					
					<scp:CollapsiblePanel id="secCompanyInfo" runat="server" IsCollapsed="true"
                        TargetControlID="CompanyInfo" meta:resourcekey="secCompanyInfo" Text="Company Information">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="CompanyInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table class="col-sm-12">
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locJobTitle" runat="server" meta:resourcekey="locJobTitle" Text="Job Title:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtJobTitle" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locCompany" runat="server" meta:resourcekey="locCompany" Text="Company:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locDepartment" runat="server" meta:resourcekey="locDepartment" Text="Department:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locOffice" runat="server" meta:resourcekey="locOffice" Text="Office:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtOffice" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locManager" runat="server" meta:resourcekey="locManager" Text="Manager:"></asp:Localize></td>
							    <td>
                                    <scp:MailboxSelector id="manager" runat="server"
                                            ShowOnlyMailboxes="true" 
											MailboxesEnabled="true"
											ContactsEnabled="true"/>
                                </td>
						    </tr>
					    </table>
					</asp:Panel>
					
					
					<scp:CollapsiblePanel id="secContactInfo" runat="server" IsCollapsed="true"
                        TargetControlID="ContactInfo" meta:resourcekey="secContactInfo" Text="Contact Information">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="ContactInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table class="col-sm-12">
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locBusinessPhone" runat="server" meta:resourcekey="locBusinessPhone" Text="Business Phone:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtBusinessPhone" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locFax" runat="server" meta:resourcekey="locFax" Text="Fax:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtFax" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locHomePhone" runat="server" meta:resourcekey="locHomePhone" Text="Home Phone:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtHomePhone" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locMobilePhone" runat="server" meta:resourcekey="locMobilePhone" Text="Mobile Phone:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locPager" runat="server" meta:resourcekey="locPager" Text="Pager:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtPager" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locWebPage" runat="server" meta:resourcekey="locWebPage" Text="Web Page:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtWebPage" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
					    </table>
					</asp:Panel>
					
					<scp:CollapsiblePanel id="secAddressInfo" runat="server" IsCollapsed="true"
                        TargetControlID="AddressInfo" meta:resourcekey="secAddressInfo" Text="Address">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="AddressInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table class="col-sm-12">
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locAddress" runat="server" meta:resourcekey="locAddress" Text="Street Address:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" Rows="2" TextMode="MultiLine"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locCity" runat="server" meta:resourcekey="locCity" Text="City:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locState" runat="server" meta:resourcekey="locState" Text="State/Province:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtState" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locZip" runat="server" meta:resourcekey="locZip" Text="Zip/Postal Code:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="col-sm-2"><asp:Localize ID="locCountry" runat="server" meta:resourcekey="locCountry" Text="Country/Region:"></asp:Localize></td>
							    <td>
									<scp:CountrySelector id="country" runat="server">
									</scp:CountrySelector>
								</td>
						    </tr>
					    </table>
					</asp:Panel>
					
					

				</div>
                    </div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditContact"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>&nbsp;
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditContact" />
				    </div>