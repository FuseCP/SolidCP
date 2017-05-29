<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxMobile.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxMobile" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>



				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeMailbox48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                        </h3>
                </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_mobile" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:GridView ID="gvMobile" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvUsers" CssSelectorClass="NormalGridView"
					    AllowSorting="False" onrowcommand="gvMobile_RowCommand" 
                        onrowdatabound="gvMobile_RowDataBound" meta:resourcekey="gvMobile" 
                        onrowdeleting="gvMobile_RowDeleting" onrowediting="gvMobile_RowEditing">
					    <Columns>						     						   						    
						    <asp:TemplateField HeaderText=""  SortExpression="DeviceUserAgent" meta:resourcekey="deviceUserAgentColumn" >
							    <ItemStyle Width="50%"></ItemStyle>
							    <ItemTemplate>							       
								    <asp:hyperlink id="lnkDeviceUserAgent" runat="server" NavigateUrl='<%# GetEditUrl(Eval("Id").ToString()) %>' >
									    <%# Eval("DeviceUserAgent") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:BoundField HeaderText="" DataField="DeviceType" ItemStyle-Width="50%" meta:resourcekey="deviceTypeColumn" />
						    	   
						    <asp:TemplateField HeaderText="" SortExpression="LastSuccessSync" meta:resourcekey="lastSyncTimeColumn" >
							    <ItemStyle Width="50%" Wrap="false"></ItemStyle>
							    <ItemTemplate>							       
								    <asp:Label runat="server" ID="lblLastSyncTime" Text='<%# Eval("LastSuccessSync") %>' />								    
							    </ItemTemplate>
						    </asp:TemplateField>
						    
						   <asp:TemplateField HeaderText="" SortExpression="Status"  meta:resourcekey="deviceStatus" >
							    <ItemStyle Width="50%"  Wrap="false"></ItemStyle>
							    <ItemTemplate>							       
								     <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status") %>' />								    
							    </ItemTemplate>
						    </asp:TemplateField>
						    
						    <asp:TemplateField>
							    <ItemTemplate>
								    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Remove this item?');"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
					

                        </div>
				</div>
				    <div class="panel-footer text-right">
					   
				    </div>