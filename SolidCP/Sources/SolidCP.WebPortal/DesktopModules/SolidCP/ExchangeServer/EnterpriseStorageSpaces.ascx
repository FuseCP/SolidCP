<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStorageSpaces.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.EnterpriseStorageSpaces" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<scp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<scp:Menu id="menu" runat="server" SelectedItem="domains" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="EnterpriseStorageSpace48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Space Names"></asp:Localize>
				</div>
				<div class="FormBody">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
                    <div class="FormButtonsBarClean">
                        <asp:Button ID="btnAddSpace" runat="server" meta:resourcekey="btnAddSpace"
                            Text="Add New Space" CssClass="Button1" OnClick="btnAddSpace_Click" />
                    </div>

				    <asp:GridView ID="gvSpaces" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvSpaces" CssSelectorClass="NormalGridView" OnRowCommand="gvSpaces_RowCommand">
					    <Columns>
						    <asp:TemplateField HeaderText="gvSpaceName">
							    <ItemStyle Width="50%"></ItemStyle>
							    <ItemTemplate>
								    <asp:hyperlink id="lnkEditZone" runat="server" EnableViewState="false"
									    NavigateUrl='<%# GetSpaceRecordsEditUrl(Eval("Name")) %>'
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
                            <asp:TemplateField HeaderText="gvQuota">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <asp:Label ID="Label1" Text='<%# Eval("Quota").ToString() %>' runat="server"/>
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
                             <asp:TemplateField HeaderText="gvPublish">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <asp:Button ID="btnPublish" text=<%# IsPublished((bool)Eval("IsPublished")) %> meta:resourcekey="btnPublish" runat="server" CommandName="Publish" CommandArgument='<%# Eval("Name") + "|" + Eval("IsPublished").ToString() %>'/>
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="gvManage">
							    <ItemTemplate>
							        <div style="text-align:center">
								        <asp:Image ID="Image5" runat="server" Width="16px" Height="16px" ToolTip="CRM" ImageUrl='<%# GetManageImage((Guid)Eval("SpaceName")) %>'  />
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>                            
						    <asp:TemplateField>
							    <ItemTemplate>
									&nbsp;<asp:ImageButton ID="imgDelSpace" runat="server" Text="Delete" SkinID="ExchangeDelete"
									    CommandName="DeleteItem" CommandArgument='<%# Eval("SpaceName") %>' 
                                        meta:resourcekey="cmdDelete" OnClientClick="return confirm('Are you sure you want to delete selected space?')"></asp:ImageButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
				    <br />
				
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Spaces Used:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <scp:QuotaViewer ID="spacesQuota" runat="server" QuotaTypeId="2" />
				    
				    
				</div>
			</div>
		</div>
	</div>
</div>