<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsWebPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsWebPolicy" %>
<%@ Register Src="UserControls/UsernamePolicyEditor.ascx" TagName="UsernamePolicyEditor" TagPrefix="uc2" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secParkingPage" runat="server"
    TargetControlID="ParkingPagePanel" meta:resourcekey="secParkingPage" Text="Parking Page"/>
<asp:Panel ID="ParkingPagePanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
        <tr>
            <td class="Normal" colspan="2">
                <asp:CheckBox ID="chkAddParkingPage" runat="server" meta:resourcekey="chkAddParkingPage" Text="Add Parking Page" /></td>
        </tr>
        <tr>
            <td class="SubHead" style="width:150px;"><asp:Label ID="lblParkingPageName" runat="server" meta:resourcekey="lblParkingPageName" Text="Page Name:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtPageName" runat="server" Width="200" CssClass="form-control"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="SubHead" valign=top><asp:Label ID="lblParkingPageContent" runat="server" meta:resourcekey="lblParkingPageContent" Text="Page Content:"></asp:Label></td>
            <td class="Normal" valign=top>
                <asp:TextBox ID="txtPageContent" runat="server" Rows="10" TextMode="MultiLine" Width="100%" CssClass="form-control" Wrap="False"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="SubHead" valign=top></td>
            <td><asp:checkbox id="chkEnableParkingPageTokens" meta:resourcekey="chkEnableParkingPageTokens" Text="Allow tokens" Runat="server"></asp:checkbox></td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secHostNamePanel" runat="server"
    TargetControlID="HostNamePanel" meta:resourcekey="secHostNamePanel" Text="Parking Page"/>
<asp:Panel ID="HostNamePanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
        <tr>
            <td class="SubHead" style="width:150px;"><asp:Label ID="lblHostName" runat="server" meta:resourcekey="lblHostName" Text="Page Name:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtHostName" runat="server" Width="200" CssClass="form-control"></asp:TextBox></td>
        </tr>
    </table>
</asp:Panel>


<scp:CollapsiblePanel runat="server" ID="WebPublishingProfile" 
	meta:resourcekey="WebPublishingProfile" Text="Web Publishing Profile" TargetControlID="WebPublishingProfilePanel" />
<asp:Panel runat="server" ID="WebPublishingProfilePanel" Height="0" style="overflow:hidden;">
	<table width="100%">
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
        <tr>
            <td class="SubHead" valign=top style="width:150px;"><asp:Label ID="PublishingProfileLabel" runat="server" meta:resourcekey="PublishingProfileLabel" Text="Publishing Profile:"></asp:Label></td>
            <td class="Normal" valign=top>
                <asp:TextBox ID="PublishingProfileTextBox" runat="server" Rows="10" TextMode="MultiLine" Width="100%" CssClass="form-control" Wrap="False"></asp:TextBox></td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secDefaultDocs" runat="server"
    TargetControlID="DefaultDocsPanel" meta:resourcekey="secDefaultDocs" Text="Default Documents"/>
<asp:Panel ID="DefaultDocsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" style="width:150px;" valign="top">
                <asp:Label ID="lblDefaultDocs" runat="server" meta:resourcekey="lblDefaultDocs" Text="Default Documents:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtDefaultDocs" runat="server" TextMode="MultiLine"
	                                        Rows="7" CssClass="form-control" Width="200px"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>


<scp:CollapsiblePanel id="secGeneralSettings" runat="server"
    TargetControlID="GeneralSettingsPanel" meta:resourcekey="secGeneralSettings" Text="General Settings"/>
<asp:Panel ID="GeneralSettingsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" style="width:150px;" valign="top">
                <asp:Label ID="lblGeneralSettings" runat="server" meta:resourcekey="lblGeneralSettings" Text="Web Site Settings:"></asp:Label></td>
            <td class="Normal">
                <table class="Normal" cellSpacing="0" cellPadding="3">
                    <tr>
                        <td class="NormalBold">
                            <asp:Label ID="lblSecuritySettings" runat="server" meta:resourcekey="lblSecuritySettings" Text="Security Settings:"></asp:Label>
                        </td>
                    </tr>
	                <tr>
		                <td><asp:checkbox id="chkWrite" meta:resourcekey="chkAllowWrite" Text="Write" Runat="server"></asp:checkbox></td>
	                </tr>
	                <tr>
		                <td><asp:checkbox id="chkDirectoryBrowsing" meta:resourcekey="chkAllowDirectoryBrowsing" Text="Directory browsing" Runat="server"></asp:checkbox></td>
	                </tr>
	                <tr>
		                <td><asp:checkbox id="chkParentPaths" meta:resourcekey="chkParentPaths" Text="Enabled Parent Paths" Runat="server"></asp:checkbox></td>
	                </tr>
	                <tr>
		                <td><asp:checkbox id="chkDedicatedPool" meta:resourcekey="chkDedicatedPool" Text="Dedicated Application Pool" Runat="server"></asp:checkbox></td>
	                </tr>
                </table>
                <br />
                <table class="Normal" cellSpacing="0" cellPadding="3">
                    <tr>
                        <td class="NormalBold">
                            <asp:Label ID="lblAuthentication" runat="server" meta:resourcekey="lblAuthentication" Text="Authentication:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap><asp:checkbox id="chkAuthAnonymous" meta:resourcekey="chkAuthAnonymous" Text="Enable anonymous access" Runat="server"></asp:checkbox></td>
                    </tr>
                    <tr>
                        <td nowrap><asp:checkbox id="chkAuthWindows" meta:resourcekey="chkAuthWindows" Text="Integrated Windows authentication" Runat="server"></asp:checkbox></td>
                    </tr>
                    <tr>
                        <td nowrap><asp:checkbox id="chkAuthBasic" meta:resourcekey="chkAuthBasic" Text="Basic authentication" Runat="server"></asp:checkbox></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>


<scp:CollapsiblePanel id="secExtensions" runat="server"
    TargetControlID="ExtensionsPanel" meta:resourcekey="secExtensions" Text="Extensions"/>
<asp:Panel ID="ExtensionsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table cellpadding="4">
        <tr>
            <td class="SubHead" style="width:150px;">
                <asp:Label ID="lblAsp" runat="server" meta:resourcekey="lblAsp" Text="ASP:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkAsp" runat="server" Text="Enabled" /></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblAspNet" runat="server" meta:resourcekey="lblAspNet" Text="ASP.NET:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:DropDownList ID="ddlAspNet" runat="server" CssClass="form-control" meta:resourcekey="ddlAspNet">
                    <asp:ListItem Value="">None</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ddlAspNetItem1" Value="1">1</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ddlAspNetItem2" Value="2">2</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ddlAspNetItem3" Value="2I">2I</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ddlAspNetItem4" Value="4">4</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ddlAspNetItem5" Value="4I">4I</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblPhp" runat="server" meta:resourcekey="lblPhp" Text="PHP:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:DropDownList ID="ddlPhp" runat="server" CssClass="form-control" meta:resourcekey="ddlPhp">
                    <asp:ListItem Value="">None</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
       <tr>
		    <td class="NormalBold">
			    <asp:Label ID="lblCompression" runat="server" meta:resourcekey="lblCompression" Text="Compression:"></asp:Label>
		    </td>
           <td></td>
	    </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblDynamicCompression" runat="server" meta:resourcekey="lblDynamicCompression" Text="Dynamic compression:"></asp:Label>
            </td>
	        <td><asp:checkbox id="chkDynamicCompression" meta:resourcekey="chkDynamicCompression" Text="Enabled" Runat="server"></asp:checkbox></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblStaticCompression" runat="server" meta:resourcekey="lblStaticCompression" Text="Static compression:"></asp:Label>
            </td>
	        <td><asp:checkbox id="chkStaticCompression" meta:resourcekey="chkStaticCompression" Text="Enabled" Runat="server"></asp:checkbox></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblPerl" runat="server" meta:resourcekey="lblPerl" Text="Perl:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkPerl" runat="server" Text="Enabled" /></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblPython" runat="server" meta:resourcekey="lblPython" Text="Python:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkPython" runat="server" Text="Enabled" /></td>
        </tr>
        <tr id="rowCgiBin" runat="server">
            <td class="SubHead">
                <asp:Label ID="lblCgiBin" runat="server" meta:resourcekey="lblCgiBin" Text="CGI-BIN:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkCgiBin" runat="server" Text="Installed" /></td>
        </tr>
		
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblCfExt" runat="server" meta:resourcekey="lblCfExt" Text="ColdFusion:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkCfExt" runat="server" Text="Enabled" /></td>
        </tr>

        <tr>
            <td class="SubHead">
                <asp:Label ID="lblVirtDir" runat="server" meta:resourcekey="lblVirtDir" Text="CFAppVirtualDirectories:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkVirtDir" runat="server" Text="Enabled" /></td>
        </tr>
		
    </table>
</asp:Panel>


<scp:CollapsiblePanel id="secAnonymousAccount" runat="server"
    TargetControlID="AnonymousAccountPanel" meta:resourcekey="secAnonymousAccount" Text="Anonymous Account Policy"/>
<asp:Panel ID="AnonymousAccountPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" style="width:150px;" valign="top">
                <asp:Label ID="lblAnonymuousUsername" runat="server" meta:resourcekey="lblAnonymuousUsername" Text="Username Policy:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="anonymousUsername" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
    </table>
</asp:Panel>


<scp:CollapsiblePanel id="secAppVirtualDirectories" runat="server"
    TargetControlID="AppVirtualDirectoriesPanel" meta:resourcekey="secAppVirtualDirectories" Text="Virtual Directories"/>
<asp:Panel ID="AppVirtualDirectoriesPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" style="width:150px;" valign="top">
                <asp:Label ID="lblVirtDirName" runat="server" meta:resourcekey="lblVirtDirName" Text="Virtual Directory Name:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="virtDirName" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secFrontPage" runat="server"
    TargetControlID="FrontPagePanel" meta:resourcekey="secFrontPage" Text="FrontPage Account Policy"/>
<asp:Panel ID="FrontPagePanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" style="width:150px;" valign="top">
                <asp:Label ID="lblFrontPageUsername" runat="server" meta:resourcekey="lblFrontPageUsername" Text="Username Policy:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="frontPageUsername" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblFrontPagePassword" runat="server" meta:resourcekey="lblFrontPagePassword" Text="Password Policy:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:PasswordPolicyEditor id="frontPagePassword" runat="server">
                </uc1:PasswordPolicyEditor></td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secSecuredFolders" runat="server"
    TargetControlID="SecuredFoldersPanel" meta:resourcekey="secSecuredFolders" Text="Secured Web Folders"/>
<asp:Panel ID="SecuredFoldersPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" style="width:150px;" valign="top">
                <asp:Label ID="lblUsername" runat="server" meta:resourcekey="lblUsername" Text="User Name:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="securedUserNamePolicy" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="User Password:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:PasswordPolicyEditor id="securedUserPasswordPolicy" runat="server">
                </uc1:PasswordPolicyEditor></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="Group Name:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="securedGroupNamePolicy" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
    </table>
</asp:Panel>


<scp:CollapsiblePanel id="secFolders" runat="server"
    TargetControlID="FoldersPanel" meta:resourcekey="secFolders" Text="Web Site Folders"/>
<asp:Panel ID="FoldersPanel" runat="server" Height="0" style="overflow:hidden;">
    <table cellpadding="4">
        <tr>
            <td class="Normal"></td>
            <td class="Normal">
                <asp:Label ID="lblFoldersDescription" runat="server" meta:resourcekey="lblFoldersDescription" Text="* All folders are relative to Space home<br/>* [DOMAIN_NAME] variable is allowed"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="SubHead" style="width:150px;">
                <asp:Label ID="lblWebSiteRootFolder" runat="server" meta:resourcekey="lblWebSiteRootFolder" Text="Root Folder:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtSiteRootFolder" runat="server" CssClass="form-control" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireRootFolder" runat="server" ControlToValidate="txtSiteRootFolder"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="SettingsEditor"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblWebSiteLogsFolder" runat="server" meta:resourcekey="lblWebSiteLogsFolder" Text="Logs Folder:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtSiteLogsFolder" runat="server" CssClass="form-control" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireLogsFolder" runat="server" ControlToValidate="txtSiteLogsFolder"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="SettingsEditor"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblWebSiteDataFolder" runat="server" meta:resourcekey="lblWebSiteDataFolder" Text="Data Folder:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtSiteDataFolder" runat="server" CssClass="form-control" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireDataFolder" runat="server" ControlToValidate="txtSiteDataFolder"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="SettingsEditor"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="Normal">
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkAddRandomDomainString" runat="server" meta:resourcekey="chkAddRandomDomainString" Text="Add random string to the end of [DOMAIN_NAME] variable" />
            </td>
        </tr>
    </table>
</asp:Panel>
