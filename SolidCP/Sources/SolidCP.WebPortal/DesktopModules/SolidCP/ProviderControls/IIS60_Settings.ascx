<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IIS60_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.IIS60_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<%@ Register Src="Common_ActiveDirectoryIntegration.ascx" TagName="ActiveDirectoryIntegration" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/EditDomainsList.ascx" TagName="EditDomainsList" TagPrefix="uc5" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagPrefix="scp" TagName="CollapsiblePanel" %>
<%@ Register Src="../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Register src="../UserControls/EditFeedsList.ascx" tagname="EditFeedsList" tagprefix="uc6" %>

<fieldset>
    <legend>
        <asp:Label ID="secServiceSettings" runat="server" meta:resourcekey="secServiceSettings" Text="Service Settings" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">
		<tr>
			<td class="Normal" width="200" nowrap>
			    <asp:Label ID="lblSharedIP" runat="server" meta:resourcekey="lblSharedIP" Text="Web Sites Shared IP Address:"></asp:Label>
			</td>
			<td width="100%">
                <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" />
			</td>
		</tr>
		<tr>
			<td class="Normal" width="200" nowrap>
			    <asp:Label ID="lblPublicSharedIP" runat="server" meta:resourcekey="lblPublicSharedIP" Text="Web Sites Public Shared IP Address:"></asp:Label>
			</td>
			<td width="100%">
                <asp:TextBox ID="txtPublicSharedIP" runat="server" Width="200" CssClass="NormalTextBox"></asp:TextBox>
			</td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="Web Users Group Name:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtWebGroupName" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
            </td>
		</tr>
	    <tr>
	        <td colspan="2">
	            <asp:CheckBox ID="chkAssignIPAutomatically" runat="server" meta:resourcekey="chkAssignIPAutomatically" Text="Assign IP addresses to the space on creation" />
	        </td>
	    </tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="secAspNet" runat="server" meta:resourcekey="secAspNet" Text="ASP.NET" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">

		<tr>
		    <td width="192" class="Normal">
		        <asp:Label ID="lblAspPath" runat="server" meta:resourcekey="lblAspPath" Text="ASP Library Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspPath" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblAspNet11Path" runat="server" meta:resourcekey="lblAspNet11Path" Text="ASP.NET 1.1 Library Path: "></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet11Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblAspNet20Path" runat="server" meta:resourcekey="lblAspNet20Path" Text="ASP.NET 2.0 Library Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet20Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblAspNet40Path" runat="server" meta:resourcekey="lblAspNet40Path" Text="ASP.NET 4.0 Library Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet40Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="secPools" runat="server" meta:resourcekey="secPools" Text="Pools" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">

		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblAsp11Pool" runat="server" meta:resourcekey="lblAsp11Pool" Text="ASP.NET 1.1 Application Pool:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet11Pool" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblAsp20Pool" runat="server" meta:resourcekey="lblAsp20Pool" Text="ASP.NET 2.0 Application Pool:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet20Pool" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblAspNet40Pool" runat="server" meta:resourcekey="lblAspNet40Pool" Text="ASP.NET 4.0 Application Pool:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet40Pool" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="lblWebAppGallery" runat="server" meta:resourcekey="lblWebAppGallery" Text="Pools" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
      <table width="100%" cellpadding="4">
		<tr>
			<td class="Normal" valign="top" width="192">
		        <asp:Label ID="Label1" runat="server" meta:resourcekey="GalleryFeedFilter" Text="Gallery feed filter:"></asp:Label>
                
		    </td>
		    <td class="Normal" valign="top">
                <asp:RadioButtonList  ID="radioFilterAppsList" runat="server">
                    <asp:ListItem Value="Exclude">Exclude selected applications</asp:ListItem>
                    <asp:ListItem Value="Include">Include only selected applications</asp:ListItem>
                </asp:RadioButtonList>
                <br/>
                <asp:Button runat="server" CssClass="Button1" ID="FilterDialogButton" Text="Change a filter" />
                <br/><br/>
                <asp:CheckBox ID="chkGalleryAppsAlwaysIgnoreDependencies" runat="server" meta:resourcekey="chkGalleryAppsAlwaysIgnoreDependencies" Text="Always ignore dependencies" />
			</td>
		</tr>


        <tr>
			<td class="SubHead" colspan="2">Custom feeds:</td>
		</tr>

        <tr>
			<td colspan="2"><uc6:EditFeedsList ID="wpiEditFeedsList" runat="server" DisplayNames="false" /><br/></td>
		</tr>

    </table>
</fieldset>
<br />

<asp:Panel ID="FilterDialogPanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="OkButton">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-list"></i> <scp:PopupHeader runat="server" meta:resourcekey="popWebAppGalleryFilter" Text="Web Application Gallery Filter" /></h3>
        </div>
        <div class="widget-content">
			<div class="BorderFillBox" style="padding: 5px 5px 5px 5px; overflow-y: scroll; width: auto; height: 300px;">
				<asp:CheckBoxList runat="server" ID="WebAppGalleryList" DataSourceID="WebAppGalleryListDS" 
					DataValueField="Id" DataTextField="Title" OnDataBound="WebAppGalleryList_DataBound">
				</asp:CheckBoxList>
			</div>
            <br /><br />
            <asp:Button ID="OkButton" runat="server" CssClass="Button1" Text="Apply" CausesValidation="false" />
            <asp:Button ID="ResetButton" runat="server" CssClass="Button1" Text="Reset" OnClick="ResetButton_Click" CausesValidation="false" />
            <asp:Button ID="CancelButton" runat="server" CssClass="Button1" Text="Cancel" CausesValidation="false" />
	
	<asp:ObjectDataSource runat="server" ID="WebAppGalleryListDS" TypeName="SolidCP.Portal.WebAppGalleryHelpers" 
		SelectMethod="GetGalleryApplicationsByServiceId">
		<SelectParameters>
			<asp:QueryStringParameter Name="serviceId" QueryStringField="ServiceID" Type="Int32" />
		</SelectParameters>
	</asp:ObjectDataSource>
            </div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="FilterDialogModal" runat="server"
    TargetControlID="FilterDialogButton" PopupControlID="FilterDialogPanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="CancelButton" 
    OkControlID="OkButton" />

<fieldset>
    <legend>
        <asp:Label ID="secWebExtensions" runat="server" meta:resourcekey="secWebExtensions" Text="Pools" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">

		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblPhp4Path" runat="server" meta:resourcekey="lblPhp4Path" Text="PHP 4.x Executable Path:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtPhp4Path" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblPhp5Path" runat="server" meta:resourcekey="lblPhp5Path" Text="PHP 5.x Executable Path:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtPhp5Path" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblPerlPath" runat="server" meta:resourcekey="lblPerlPath" Text="Perl Executable Path:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtPerlPath" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top"  width="192">
		        <asp:Label ID="lblPythonPath" runat="server" meta:resourcekey="lblPythonPath" Text="Python Executable Path:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtPythonPath" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox></td>
		</tr>		
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="secColdFusion" runat="server" meta:resourcekey="secColdFusion" Text="ColdFusion" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">

		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblColdFusionPath" runat="server" meta:resourcekey="lblColdFusionPath" Text="ColdFusion Path:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtColdFusionPath" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblScriptsDirectory" runat="server" meta:resourcekey="lblScriptsDirectory" Text="Scripts Directory:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtScriptsDirectory" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblFlashRemotingDir" runat="server" meta:resourcekey="lblFlashRemotingDir" Text="Flash Remoting Directory:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtFlashRemotingDir" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="secIISpassword" runat="server" meta:resourcekey="secIISpassword" Text="IISPassword" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">

		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblPasswordFilterPath" runat="server" meta:resourcekey="lblPasswordFilterPath" Text="IISPassword Filter Path:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtPasswordFilterPath" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblProtectedAccessFile" runat="server" meta:resourcekey="lblProtectedAccessFile" Text="IISPassword Access File:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtProtectedAccessFile" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>    
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblProtectedUsersFile" runat="server" meta:resourcekey="lblProtectedUsersFile" Text="IISPassword Users File:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtProtectedUsersFile" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblProtectedGroupsFile" runat="server" meta:resourcekey="lblProtectedGroupsFile" Text="IISPassword Groups File:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtProtectedGroupsFile" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblProtectedFoldersFile" runat="server" meta:resourcekey="lblProtectedFoldersFile" Text="IISPassword Folders File:"></asp:Label>
		    </td>
		    <td valign="top">
                <asp:TextBox ID="txtProtectedFoldersFile" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>		
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="secOther" runat="server" meta:resourcekey="secOther" Text="Other Settings" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
<table cellpadding="4" cellspacing="0" width="100%">
		
		<tr>
		    <td class="SubHead" valign="top">
		        <asp:Label ID="lblSharedSslSites" runat="server" meta:resourcekey="lblSharedSslSites" Text="Shared SSL Sites:"></asp:Label>
		    </td>
		    <td valign="top">
                <uc5:EditDomainsList id="sharedSslSites" runat="server" DisplayNames="false">
                </uc5:EditDomainsList>
            </td>
		</tr>
		<tr>
		    <td class="SubHead" valign="top">
		        <asp:Label ID="lblADIntegration" runat="server" meta:resourcekey="lblADIntegration" Text="Active Directory Integration:"></asp:Label>
		    </td>
		    <td valign="top">
                <uc1:ActiveDirectoryIntegration ID="ActiveDirectoryIntegration" runat="server" />
            </td>
		</tr>
</table>
</fieldset>
<br />

