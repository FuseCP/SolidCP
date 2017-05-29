<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsInsertDvd.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsInsertDvd" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_dvd" />
	
                        <scp:SimpleMessageBox id="messageBox" runat="server" />
                        
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
							                CommandName="insert" CommandArgument='<%# Eval("Path") %>' CssClass="btn btn-primary">
							            </asp:Button>
						            </ItemTemplate>
					            </asp:TemplateField>
				            </Columns>
			            </asp:GridView>
			            <br />
			            <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
			    </div>
		    </div>
	    </div>
