<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesHeliconApeControl.ascx.cs"
	Inherits="SolidCP.Portal.WebSitesHeliconApeControl" %>
<%@ Import Namespace="SolidCP.Portal" %>

<style type="text/css">
    .HtaccessPanel {
        margin: 1em 0 3em 0;
    }
    .DelButton {
        margin-right: 5px;
    }
</style>

<asp:Panel ID="HeliconApeFoldersPanel" runat="server" CssClass="HtaccessPanel">
	
    <table class="FormButtonsBar" width="100%">
	    <tr>
	        <td>
	            <asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="labelSelectHtacesEdit"></asp:Label>
            </td>
            <td align="right">
		        <asp:Button ID="btnAddHeliconApeFolder" runat="server" meta:resourcekey="btnAddHeliconApeFolder"
			        Text="Add .htaccess" CssClass="Button2" CausesValidation="false" OnClick="btnAddHeliconApeFolder_Click" />
            </td>
        </tr>
	</table>
	<asp:GridView ID="gvHeliconApeFolders" runat="server" EnableViewState="True" AutoGenerateColumns="false"
		ShowHeader="False" CssSelectorClass="LightGridView" EmptyDataText="gvHeliconApeFolders"
		DataKeyNames="Path,ContentPath" OnRowDeleting="gvHeliconApeFolders_RowDeleting" Width="100%">
		<Columns>
			<asp:TemplateField HeaderText="gvHeliconApeFoldersName" ItemStyle-Width="782px">
				<ItemStyle CssClass="NormalText"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditHeliconApeFolder" runat="server" 
                    NavigateUrl='<%# GetEditControlUrl("edit_htaccessfolder", Eval("Path").ToString()) %>'
                     CssClass="NormalBold">
			            <%# GetHtaccessPathOnSite((string)Eval("Path")) %>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-Width="110px">
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditHeliconApeFolderAuth" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_htaccessfolderauth", Eval("Path").ToString()) %>'
						title="Folder Security Properties (.htpasswd)">
			            <image src="/App_Themes/Default/Images/shield.png" style="border: 0; vertical-align: top; margin-right: 3px;" />Security options
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<CPCC:StyleButton id="cmdDeleteHeliconApeFolder" CssClass="btn btn-danger" runat="server" CommandName='delete' CausesValidation="false" OnClientClick="return confirm('Delete .htaccess?');"> 
                        &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                    </CPCC:StyleButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
    
    
    
    
    
    
    
    
    

    
    <input class="Button2" id="ShowSecurityPanelButton" value="Show Security Options (.htpasswd)" type="button" />
    <div id="SecurityPanel" class="Hidden">
	<table class="FormButtonsBar" width="100%" style="margin-top: 2em;">
	    <tr>
	        <td>
	            <asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="HeliconApeUsersHeader"></asp:Label>
	        </td>
            <td align="right">
                <asp:Button ID="btnAddHeliconApeUser" runat="server" meta:resourcekey="btnAddHeliconApeUser"
			        Text="Add User" CssClass="Button2" CausesValidation="false" OnClick="btnAddHeliconApeUser_Click" />
            </td>
	    </tr>
	</table>
	<asp:GridView ID="gvHeliconApeUsers" runat="server" EnableViewState="True" AutoGenerateColumns="false"
		ShowHeader="False" CssSelectorClass="LightGridView" EmptyDataText="gvHeliconApeUsers"
		DataKeyNames="Name" OnRowDeleting="gvHeliconApeUsers_RowDeleting" Width="100%">
		<Columns>
			<asp:TemplateField ItemStyle-Width="100%">
				<ItemStyle CssClass="NormalBold"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditUser" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_htaccessuser", Eval("Name").ToString()) %>'>
			            <%# Eval("Name") %>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-Width="30px">
				<ItemTemplate>
					<CPCC:StyleButton id="cmdDeleteUser" CssClass="btn btn-danger" runat="server" CommandName='delete' CausesValidation="false" OnClientClick="return confirm('Delete user?');"> 
                        &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                    </CPCC:StyleButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	
    
    
    

    
    <table class="FormButtonsBar" width="100%" style="margin-top: 3em;">
	    <tr>
	        <td>
	            <asp:Label ID="Label1" runat="server" CssClass="NormalBold" meta:resourcekey="HeliconApeGroupsHeader"></asp:Label>
	        </td>
            <td align="right">
                <asp:Button ID="btnAddHeliconApeGroup" runat="server" meta:resourcekey="btnAddHeliconApeGroup"
        			Text="Add Group" CssClass="Button2" CausesValidation="false" OnClick="btnAddHeliconApeGroup_Click" />
            </td>
	    </tr>
	</table>

	<asp:GridView ID="gvHeliconApeGroups" runat="server" EnableViewState="True" AutoGenerateColumns="false"
		ShowHeader="False" EmptyDataText="gvHeliconApeGroups" CssSelectorClass="LightGridView"
		DataKeyNames="Name" OnRowDeleting="gvHeliconApeGroups_RowDeleting" Width="100%">
		<Columns>
			<asp:TemplateField ItemStyle-Width="100%">
				<ItemStyle CssClass="NormalBold"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditGroup" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_htaccessgroup", Eval("Name").ToString()) %>'>
			            <%# Eval("Name") %>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-Width="30px">
				<ItemTemplate>
					<CPCC:StyleButton id="cmdDeleteGroup" CssClass="btn btn-danger" runat="server" CommandName='delete' CausesValidation="false" OnClientClick="return confirm('Delete group?');"> 
                        &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                    </CPCC:StyleButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
    </div>
</asp:Panel>





<div class="FormButtonsBar">
	<asp:Panel runat="server" ID="panelHeliconApeIsNotInstalledMessage" Visible="false">
		<p>
			<asp:Localize ID="Localize1" runat="server" meta:resourcekey="ApeModuleNotes" /></p>
	</asp:Panel>
	<asp:Panel runat="server" ID="panelHeliconApeIsNotEnabledMessage" Visible="false">
		<p>
			<asp:Localize ID="Localize2" runat="server" meta:resourcekey="ApeProductNotes" /></p>
	</asp:Panel>
	<asp:Button ID="btnToggleHeliconApe" runat="server" meta:resourcekey="btnToggleHeliconApe"
		Text="Enable Helicon Ape" CssClass="Button2" CausesValidation="false" OnClick="btnToggleHeliconApe_Click" />
	<div style="float: right;">
		<asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl="http://www.helicontech.com/ape/doc/wsp.htm"
			meta:resourcekey="ModuleHelpLink" />
	</div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#ShowSecurityPanelButton').click(function () {
            $('#ShowSecurityPanelButton').slideUp();
            $('#SecurityPanel').slideDown();
        });
    });
</script>