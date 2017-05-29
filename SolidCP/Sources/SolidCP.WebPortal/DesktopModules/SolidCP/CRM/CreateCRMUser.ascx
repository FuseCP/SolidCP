<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateCRMUser.ascx.cs" Inherits="SolidCP.Portal.CRM.CreateCRMUser" %>
<%@ Register Src="../UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="scp" %>

<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../ExchangeServer/UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<%@ Register src="../ExchangeServer/UserControls/UserSelector.ascx" tagname="UserSelector" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>



				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeMailboxAdd48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create Mailbox"></asp:Localize>
				    </h3>
                </div>
				
				<div class="panel-body form-horizontal" width="100%">
				    
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
										  					   					    							
					<table id="ExistingUserTable"   runat="server"> 					    
					    <tr>
					        <td class="FormLabel150"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
					        <td><scp:UserSelector id="userSelector" runat="server" IncludeMailboxes="true"></scp:UserSelector></td>
					    </tr>

                        <tr>
                            <td class="FormLabel150"><asp:Localize runat="server" meta:resourcekey="locLicenseType" Text="License Type: *" /></td>
                            <td>
                                <asp:DropDownList ID="ddlLicenseType" runat="server" CssClass="NormalTextBox" AutoPostBack="false">
                                </asp:DropDownList>
                            </td>
                        </tr>
					    
					    <tr>
					        <td class="FormLabel150">
					            <asp:Localize ID="Localize2" runat="server" meta:resourcekey="locBusinessUnit" Text="Business Unit:"></asp:Localize>
					        </td>
					        <td>
					            <asp:DropDownList runat="server" ID="ddlBusinessUnits" />
					        </td>
					    </tr>
					    
					</table>																			  					
					

				</div>
					<div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" ValidationGroup="CreateCRMUser" OnClientClick="ShowProgressDialog('Creating CRM user...');"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateMailbox" />
				    </div>