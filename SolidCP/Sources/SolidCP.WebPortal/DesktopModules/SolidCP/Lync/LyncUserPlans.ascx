<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LyncUserPlans.ascx.cs" Inherits="SolidCP.Portal.Lync.LyncUserPlans" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/LyncUserPlanSelector.ascx" TagName="LyncUserPlanSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

			<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="LyncUserPlan48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Domain Names"></asp:Localize>
				</h3>
                        </div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
                    <div class="FormButtonsBarClean">
                        <CPCC:StyleButton id="btnAddPlan" CssClass="btn btn-primary" runat="server" OnClick="btnAddPlan_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddPlanText"/> </CPCC:StyleButton>
                    </div>

				    <asp:GridView ID="gvPlans" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvPlans" CssSelectorClass="NormalGridView" OnRowCommand="gvPlan_RowCommand">
					    <Columns>
						    <asp:TemplateField>
							    <ItemTemplate>							        
								    <asp:Image ID="img2" runat="server" Width="16px" Height="16px" ImageUrl='<%# GetPlanType((int)Eval("LyncUserPlanType")) %>' ImageAlign="AbsMiddle" />
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvPlan">
							    <ItemStyle Width="70%"></ItemStyle>
							    <ItemTemplate>
								    <asp:hyperlink id="lnkDisplayPlan" runat="server" EnableViewState="false"
									    NavigateUrl='<%# GetPlanDisplayUrl(Eval("LyncUserPlanId").ToString()) %>'>
									    <%# Eval("LyncUserPlanName")%> 
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvPlanDefault">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <input type="radio" name="DefaultPlan" value='<%# Eval("LyncUserPlanId") %>' <%# IsChecked((bool)Eval("IsDefault")) %> />
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField>
							    <ItemTemplate>
									<CPCC:StyleButton id="imgDelMailboxPlan" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("LyncUserPlanId") %>' OnClientClick="return confirm('Are you sure you want to delete selected plan?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
				    <br />
				    <div style="text-align: center">
				        <CPCC:StyleButton id="btnSetDefaultPlan" CssClass="btn btn-success" runat="server" OnClick="btnSetDefaultPlan_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetDefaultPlanText"/> </CPCC:StyleButton>
                    </div>
				    
                    <scp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true" TargetControlID="ToolsPanel" meta:resourcekey="secMainTools" Text="Lync user plan maintenance">
					</scp:CollapsiblePanel>
					<asp:Panel ID="ToolsPanel" runat="server" Height="0" Style="overflow: hidden;">
						<table id="tblMaintenance" runat="server" cellpadding="10">
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="lblSourcePlan" runat="server" meta:resourcekey="locSourcePlan" Text="Replace"></asp:Localize></td>
					            <td>                                
                                    <scp:LyncUserPlanSelector ID="lyncUserPlanSelectorSource" runat="server" AddNone="true"/>
                                </td>
					        </tr>
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="lblTargetPlan" runat="server" meta:resourcekey="locTargetPlan" Text="With"></asp:Localize></td>
					            <td>                                
                                    <scp:LyncUserPlanSelector ID="lyncUserPlanSelectorTarget" runat="server" AddNone="false"/>
                                </td>
					        </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStatus" runat="server"  CssClass="form-control" MaxLength="128" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
						</table>
				        <div class="panel-footer text-right">
					        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick = "ShowProgressDialog('Stamping mailboxes, this might take a while ...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
				        </div>
					</asp:Panel>
				</div>