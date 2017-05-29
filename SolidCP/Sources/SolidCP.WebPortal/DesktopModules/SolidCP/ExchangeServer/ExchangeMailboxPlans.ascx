<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxPlans.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxPlans" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxPlanSelector.ascx" TagName="MailboxPlanSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeMailboxPlans48" runat="server" />
					<asp:Localize ID="locTitle" runat="server"></asp:Localize>
                        </h3>
				</div>
       <div class="FormButtonsBar right">
                        <CPCC:StyleButton id="btnAddMailboxPlan" CssClass="btn btn-primary" runat="server" OnClick="btnAddMailboxPlan_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddMailboxPlan"/> </CPCC:StyleButton>
                    </div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
             </div>

				    <asp:GridView ID="gvMailboxPlans" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvMailboxPlans" CssSelectorClass="NormalGridView" OnRowCommand="gvMailboxPlan_RowCommand">
					    <Columns>
						    <asp:TemplateField>
							    <ItemTemplate>							        
								    <asp:Image ID="img2" runat="server" Width="16px" Height="16px" ImageUrl='<%# GetPlanType((int)Eval("MailboxPlanType")) %>' ImageAlign="AbsMiddle" />
							    </ItemTemplate>
						    </asp:TemplateField>

						    <asp:TemplateField HeaderText="gvMailboxPlan">
							    <ItemStyle Width="70%"></ItemStyle>
							    <ItemTemplate>
								    <asp:hyperlink id="lnkDisplayMailboxPlan" runat="server" EnableViewState="false"
									    NavigateUrl='<%# GetMailboxPlanDisplayUrl(Eval("MailboxPlanId").ToString()) %>'>
									    <%# Eval("MailboxPlan")%> 
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvMailboxPlanDefault">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <input type="radio" name="DefaultMailboxPlan" value='<%# Eval("MailboxPlanId") %>' <%# IsChecked((bool)Eval("IsDefault")) %> />
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField >
							    <ItemTemplate>
									<CPCC:StyleButton id="imgDelMailboxPlan" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("MailboxPlanId") %>' OnClientClick="return confirm('Are you sure you want to delete selected?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
				    <br />
				    <div class="panel-body text-right">
				        <CPCC:StyleButton id="btnSetDefaultMailboxPlan" CssClass="btn btn-success" runat="server"  CausesValidation="false" OnClick="btnSetDefaultMailboxPlan_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetDefaultMailboxPlan"/> </CPCC:StyleButton>
                    </div>
<div class="panel-body">
					<scp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true" TargetControlID="ToolsPanel" meta:resourcekey="secMainTools" Text="Mailbox plan maintenance">
					</scp:CollapsiblePanel>
					<asp:Panel ID="ToolsPanel" runat="server" Height="0" Style="overflow: hidden;" CssClass="panel panel-default">
                        <div class="panel-body">
						<table id="tblMaintenance" runat="server" cellpadding="10">
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="lblSourcePlan" runat="server" meta:resourcekey="locSourcePlan" Text="Replace"></asp:Localize></td>
					            <td>                                
                                    <scp:MailboxPlanSelector ID="mailboxPlanSelectorSource" runat="server" AddNone="true"/>
                                </td>
					        </tr>
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="lblTargetPlan" runat="server" meta:resourcekey="locTargetPlan" Text="With"></asp:Localize></td>
					            <td>                                
                                    <scp:MailboxPlanSelector ID="mailboxPlanSelectorTarget" runat="server" AddNone="false"/>
                                </td>
					        </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStatus" runat="server" CssClass="TextBox400" MaxLength="128" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
						</table>
                            </div>
                        				        <div class="panel-footer text-right">
					        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick = "ShowProgressDialog('Stamping mailboxes, this might take a while ...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
				        </div>
					</asp:Panel>
				</div>