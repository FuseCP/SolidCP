<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeCreateDistributionList.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeCreateDistributionList" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<%@ Register src="UserControls/MailboxSelector.ascx" tagname="MailboxSelector" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeListAdd48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create Distribution List"></asp:Localize>
				</h3>
                        </div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
					
					<div class="form-group">
                        <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtDisplayName">
                            <asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize>
                        </asp:Label>
                        <div class="col-sm-10">
                                        <div class="input-group">
								            <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
								            <asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
									         ErrorMessage="Enter Display Name" ValidationGroup="CreateList" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							            </div>
                        </div>
                        </div>
                        <div class="form-group">
                        <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="email">
							<asp:Localize ID="locAccount" runat="server" meta:resourcekey="locAccount" Text="E-mail Address: *"></asp:Localize>
                        </asp:Label>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <scp:EmailAddress id="email" runat="server" ValidationGroup="CreateList"></scp:EmailAddress>
                            </div>
                        </div>
                        </div>
                    <div class="form-group">
                        <asp:label runat="server" AssociatedControlID="txtDisplayName" CssClass="control-label col-sm-2">
                        <asp:Localize ID="Localize1" runat="server" meta:resourcekey="locManagedBy" ></asp:Localize>
                        </asp:label>
                        <div class="col-sm-6">
                                 <scp:mailboxselector id="manager" runat="server"
                                            ShowOnlyMailboxes="true" 
											MailboxesEnabled="true"
											DistributionListsEnabled="true" />											
											<asp:CustomValidator runat="server" 
                                     ValidationGroup="CreateList"  meta:resourcekey="valManager" ID="valManager" 
                                     onservervalidate="valManager_ServerValidate" />
                            </div>
                        </div>
				</div>

				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" ValidationGroup="CreateList"> <i class="fa fa-users">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateList" />
				    </div>