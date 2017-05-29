<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationDeletedUserGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.HostedSolution.DeletedUserGeneralSettings" %>
<%@ Register Src="UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/CountrySelector.ascx" TagName="CountrySelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>



<%@ Register src="UserControls/DeletedUserTabs.ascx" tagname="UserTabs" tagprefix="uc1" %>
<%@ Register src="UserControls/MailboxTabs.ascx" tagname="MailboxTabs" tagprefix="uc1" %>

<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Deleted User"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                    <asp:Image ID="imgVipUser" SkinID="VipUser16" runat="server" tooltip="VIP user" Visible="false"/>
                    <asp:Label ID="litServiceLevel" runat="server" style="float:right;padding-right:8px;" Visible="false"></asp:Label>
                    </h3>
                </div>

				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <uc1:UserTabs ID="UserTabsId" runat="server" SelectedTab="view_deleted_user" />
                    <uc1:MailboxTabs ID="MailboxTabsId" runat="server" SelectedTab="view_deleted_user" />
                     </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<table>
						<tr>
						    <td class="FormLabel150">
                                <asp:Localize ID="locUserPrincipalName" runat="server" meta:resourcekey="locUserPrincipalName" Text="Login Name:" />
						    </td>
						    <td>
                                <asp:Label runat="server" ID="lblUserPrincipalName" />
						    </td>
                            <td>
                                <asp:CheckBox ID="chkInherit" runat="server" meta:resourcekey="chkInherit" Text="Services inherit Login Name" checked="true" Enabled="false" />
                            </td>
						</tr>					   

						<tr>
							<td class="FormLabel150">
                                <asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *" />
							</td>
							<td>
								<asp:Label ID="lblDisplayName" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
						    <td>
						    </td>
						    <td>
						        <br />
						        <asp:CheckBox ID="chkDisable" runat="server" meta:resourcekey="chkDisable" Text="Disable User" Enabled="false"/>
						        <br />
						        <asp:CheckBox ID="chkLocked" runat="server" meta:resourcekey="chkLocked" Text="Lock User" Enabled="false"/>
						        <br />
						    </td>
						</tr>
						<tr>
							<td class="FormLabel150">
                                <asp:Localize ID="locFirstName" runat="server" meta:resourcekey="locFirstName" Text="First Name:" />
							</td>
							<td>
								<asp:label ID="lblFirstName" runat="server"></asp:label>
								&nbsp;
								<asp:Localize ID="locInitials" runat="server" meta:resourcekey="locInitials" Text="Middle Initial:" />
								<asp:Label ID="lblInitials" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150">
                                <asp:Localize ID="locLastName" runat="server" meta:resourcekey="locLastName" Text="Last Name:" />
							</td>
							<td>
								<asp:Label ID="lblLastName" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
						    <td class="FormLabel150" valign="top">
                                <asp:Localize ID="locSubscriberNumber" runat="server" meta:resourcekey="locSubscriberNumber" />
						    </td>
						    <td>
                                <asp:Label runat="server" ID="lblSubscriberNumber" />
						    </td>
						</tr>
						<tr>
						    <td class="FormLabel150" valign="top">
                                <asp:Localize ID="locExternalEmailAddress" runat="server" meta:resourcekey="locExternalEmailAddress" />
						    </td>
						    <td><asp:Label runat="server" ID="lblExternalEmailAddress" />
						    </td>
						</tr>
					</table>
                    <table>
					    <tr>
						    <td class="FormLabel150">
                                <asp:Localize ID="locNotes" runat="server" meta:resourcekey="locNotes" Text="Notes:" />
						    </td>
						    <td>
							    <asp:Label ID="lblNotes" runat="server" />
						    </td>
					    </tr>
					</table>
                    
                    <scp:CollapsiblePanel id="secServiceLevels" runat="server" IsCollapsed="true"
                        TargetControlID="ServiceLevels" meta:resourcekey="secServiceLevels" Text="Service Level Information">
                    </scp:CollapsiblePanel>

                    <asp:Panel ID="ServiceLevels" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locServiceLevel" runat="server" meta:resourcekey="locServiceLevel"  Text="Service Level:" />
							    </td>
							    <td>
								    <asp:Label ID="lblServiceLevel" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locVIPUser" runat="server" meta:resourcekey="locVIPUser" Text="VIP:" />
							    </td>
							    <td>
								    <asp:CheckBox ID="chkVIP" runat="server" Enabled="false" />
							    </td>
						    </tr>
					    </table>
					</asp:Panel>
									
					<scp:CollapsiblePanel id="secCompanyInfo" runat="server" IsCollapsed="true"
                        TargetControlID="CompanyInfo" meta:resourcekey="secCompanyInfo" Text="Company Information">
                    </scp:CollapsiblePanel>

                    <asp:Panel ID="CompanyInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locJobTitle" runat="server" meta:resourcekey="locJobTitle" Text="Job Title:" />
							    </td>
							    <td>
								    <asp:Label ID="lblJobTitle" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locCompany" runat="server" meta:resourcekey="locCompany" Text="Company:" />
							    </td>
							    <td>
								    <asp:Label ID="lblCompany" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locDepartment" runat="server" meta:resourcekey="locDepartment" Text="Department:" />
							    </td>
							    <td>
								    <asp:Label ID="lblDepartment" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locOffice" runat="server" meta:resourcekey="locOffice" Text="Office:" />
							    </td>
							    <td>
								    <asp:Label ID="lblOffice" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locManager" runat="server" meta:resourcekey="locManager" Text="Manager:" />
							    </td>
							    <td>
                                    <asp:Label ID="lblManager" runat="server" />
                                </td>
						    </tr>
					    </table>
					</asp:Panel>
					
					<scp:CollapsiblePanel id="secContactInfo" runat="server" IsCollapsed="true"
                        TargetControlID="ContactInfo" meta:resourcekey="secContactInfo" Text="Contact Information">
                    </scp:CollapsiblePanel>
                                      
                    <asp:Panel ID="ContactInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locBusinessPhone" runat="server" meta:resourcekey="locBusinessPhone" Text="Business Phone:" />
							    </td>
							    <td>
								    <asp:Label ID="lblBusinessPhone" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locFax" runat="server" meta:resourcekey="locFax" Text="Fax:" />
							    </td>
							    <td>
								    <asp:Label ID="lblFax" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locHomePhone" runat="server" meta:resourcekey="locHomePhone" Text="Home Phone:" />
							    </td>
							    <td>
								    <asp:Label ID="lblHomePhone" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locMobilePhone" runat="server" meta:resourcekey="locMobilePhone" Text="Mobile Phone:" />
							    <td>
								    <asp:Label ID="lblMobilePhone" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locPager" runat="server" meta:resourcekey="locPager" Text="Pager:" />
							    <td>
								    <asp:Label ID="lblPager" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locWebPage" runat="server" meta:resourcekey="locWebPage" Text="Web Page:" />
							    </td>
							    <td>
								    <asp:Label ID="lblWebPage" runat="server" />
							    </td>
						    </tr>
					    </table>
					</asp:Panel>
					
					<scp:CollapsiblePanel id="secAddressInfo" runat="server" IsCollapsed="true"
                        TargetControlID="AddressInfo" meta:resourcekey="secAddressInfo" Text="Address">
                    </scp:CollapsiblePanel>

                    <asp:Panel ID="AddressInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locAddress" runat="server" meta:resourcekey="locAddress" Text="Street Address:" />
							    </td>
							    <td>
								    <asp:Label ID="lblAddress" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locCity" runat="server" meta:resourcekey="locCity" Text="City:" />
							    </td>
							    <td>
								    <asp:Label ID="lblCity" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locState" runat="server" meta:resourcekey="locState" Text="State/Province:" />
							    </td>
							    <td>
								    <asp:Label ID="lblState" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locZip" runat="server" meta:resourcekey="locZip" Text="Zip/Postal Code:" />
							    </td>
							    <td>
								    <asp:Label ID="lblZip" runat="server" />
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150">
                                    <asp:Localize ID="locCountry" runat="server" meta:resourcekey="locCountry" Text="Country/Region:" />
							    </td>
							    <td>
									<asp:Label id="lblCountry" runat="server" />
								</td>
						    </tr>
					    </table>
					</asp:Panel>
					
					<scp:CollapsiblePanel id="secAdvanced" runat="server" IsCollapsed="true"
                        TargetControlID="AdvancedInfo" meta:resourcekey="secAdvanced" Text="Advanced">
                    </scp:CollapsiblePanel>	
                    
                    <asp:Panel ID="AdvancedInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
						    <td class="FormLabel150">
                                <asp:Localize ID="locUserDomainName" runat="server" meta:resourcekey="locUserDomainName" Text="User Domain Name:" />
						    </td>
						    <td>
                                <asp:Label runat="server" ID="lblUserDomainName" />
						    </td>
						</tr>					   
					    </table>
					</asp:Panel>
				</div>
			</div>