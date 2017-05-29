<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsInsertDvd.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsInsertDvd" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_dvd" />
	
                        <wsp:SimpleMessageBox id="messageBox" runat="server" />
                        
			            <p class="SubTitle">
			                <asp:Localize ID="locSubTitle" runat="server" meta:resourcekey="locSubTitle" Text="Browse Media Library" />
			            </p>
			            
			            <asp:GridView ID="gvDisks" runat="server" AutoGenerateColumns="False" EnableViewState="true"
				            Width="100%" EmptyDataText="gvDisks" CssSelectorClass="NormalGridView"
                            onrowcommand="gvDisks_RowCommand">
				            <Columns>
					            <asp:TemplateField HeaderText="gvTitle" meta:resourcekey="gvTitle">
						            <ItemTemplate>
						                <asp:Image ID="Image2" SkinID="Dvd48" runat="server" style="float: left;" />
						                <div style="font-weight: bold;padding: 3px; margin-left: 50px;">
						                    <%# Eval("Name") %>
						                </div>
						                <div style="padding: 3px; margin-left: 50px;">
							                <%# Eval("Description") %>
							            </div>
						            </ItemTemplate>
					            </asp:TemplateField>
					            <asp:TemplateField>
						            <ItemTemplate>
							            <asp:Button ID="btnInsert" runat="server" Text="Insert" meta:resourcekey="btnInsert"
							                CommandName="insert" CommandArgument='<%# Eval("Path") %>' CssClass="SmallButton">
							            </asp:Button>
						            </ItemTemplate>
					            </asp:TemplateField>
				            </Columns>
			            </asp:GridView>
			            <br />
			            <asp:Button ID="btnCancel" runat="server" CausesValidation="false"
			                      Text="Cancel" meta:resourcekey="btnCancel" CssClass="Button1" 
                        onclick="btnCancel_Click" Width="60px" />
     
			    </div>
		    </div>
	    </div>
