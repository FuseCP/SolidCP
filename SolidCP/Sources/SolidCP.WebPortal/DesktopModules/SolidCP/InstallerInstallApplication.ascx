<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstallerInstallApplication.ascx.cs" Inherits="SolidCP.Portal.InstallerInstallApplication" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="installerapplicationheader.ascx" TagName="ApplicationHeader" TagPrefix="dnc" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">

	<dnc:applicationheader id="installerapplicationheader" runat="server"></dnc:applicationheader>
    <br />
    <br />
    
	<!-- location -->
    <scp:CollapsiblePanel id="secLocation" runat="server"
        TargetControlID="LocationPanel" meta:resourcekey="secLocation" Text="Installation location">
    </scp:CollapsiblePanel>
    <asp:Panel ID="LocationPanel" runat="server" Height="0" style="overflow:hidden;">
	    <table cellpadding="2" width="100%" runat="server">
		    <tr>
			    <td class="SubHead" noWrap width="200"><asp:Label ID="lblInstallOnWebSite" runat="server" meta:resourcekey="lblInstallOnWebSite" Text="Install on web site:"></asp:Label>
			    </td>
			    <td width="100%"><asp:dropdownlist id="ddlWebSite" runat="server" DataValueField="Id" DataTextField="Name" CssClass="NormalTextBox"></asp:dropdownlist>
                    <asp:requiredfieldvalidator id="valRequireWebSite" runat="server" CssClass="NormalBold" ControlToValidate="ddlWebSite"
					    Display="Dynamic" ErrorMessage="Select web site to install on">*</asp:requiredfieldvalidator></td>
		    </tr>
		    <tr>
			    <td class="SubHead" noWrap width="200"><asp:Label ID="lblInstallOnDirectory" runat="server" meta:resourcekey="lblInstallOnDirectory" Text="Install on directory:"></asp:Label>
			    </td>
			    <td width="100%">
                    <uc2:UsernameControl id="directoryName" runat="server" RequiredField="false">
                    </uc2:UsernameControl>
			    </td>
		    </tr>
		    <tr>
			    <td></td>
			    <td class="Normal"><asp:Label ID="lblLeaveThisFieldBlank" runat="server" meta:resourcekey="lblLeaveThisFieldBlank" Text="Leave this field blank..."></asp:Label>
				    <br/>
				    <br/>
			    </td>
		    </tr>
	    </table>
	</asp:Panel>
	
	<!-- database -->
	<div id="divDatabase" runat="server">
        <scp:CollapsiblePanel id="secDatabase" runat="server"
            TargetControlID="DatabasePanel" meta:resourcekey="secDatabase" Text="Configure Database">
        </scp:CollapsiblePanel>
        <asp:Panel ID="DatabasePanel" runat="server" Height="0" style="overflow:hidden;">
	        <table cellspacing="0" cellpadding="0" width="100%">
	            <tr>
	                <td class="NormalBold" width="200" height="50" valign="middle" nowrap>
	                    <asp:DropDownList ID="ddlDatabaseGroup" runat="server"
	                        AutoPostBack="true" OnSelectedIndexChanged="ddlDatabaseGroup_SelectedIndexChanged" CssClass="NormalTextBox">
	                    </asp:DropDownList>
	                </td>
	            </tr>
	            <tr>
	                <td class="Normal">
	                    <fieldset>
	                        <legend class="NormalBold">
	                            <asp:RadioButtonList ID="rblDatabase" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
									resourcekey="rblDatabase" AutoPostBack="True" OnSelectedIndexChanged="rblDatabase_SelectedIndexChanged">
	                                <asp:ListItem Value="New" Selected="True">NewDatabase</asp:ListItem>
	                                <asp:ListItem Value="Existing">ExistingDatabase</asp:ListItem>
	                            </asp:RadioButtonList>&nbsp;
	                        </legend>
	                        <table id="tblNewDatabase" runat="server" cellpadding="3">
	                            <tr>
	                                <td class="NormalBold" width="190" nowrap>
	                                    <asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName" Text="Database Name:"></asp:Label>
	                                </td>
	                                <td width="100%" class="Normal">
                                        <uc2:UsernameControl id="databaseName" runat="server">
                                        </uc2:UsernameControl>
                                     </td>
	                            </tr>
	                        </table>
	                        <table id="tblExistingDatabase" runat="server" cellpadding="3">
	                            <tr>
	                                <td class="NormalBold" width="190" nowrap>
	                                    <asp:Label ID="lblExistingDatabaseName" runat="server" meta:resourcekey="lblDatabaseName" Text="Database Name:"></asp:Label>
	                                </td>
	                                <td width="100%" class="Normal">
                                        <asp:dropdownlist id="ddlDatabase" runat="server" DataValueField="Id" DataTextField="Name"
							                    CssClass="NormalTextBox"></asp:dropdownlist>
                                        <asp:RequiredFieldValidator ID="valRequireDatabase" runat="server" ControlToValidate="ddlDatabase"
                                            CssClass="NormalBold" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	                            </tr>
	                        </table>
	                    </fieldset>
	                </td>
	            </tr>
	            <tr>
	                <td class="Normal">
	                    <fieldset>
	                        <legend class="NormalBold">
	                            <asp:RadioButtonList ID="rblUser" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
									resourcekey="rblUser" AutoPostBack="True" OnSelectedIndexChanged="rblDatabase_SelectedIndexChanged">
	                                <asp:ListItem Value="New" Selected="True">NewUser</asp:ListItem>
	                                <asp:ListItem Value="Existing">ExistingUser</asp:ListItem>
	                            </asp:RadioButtonList>&nbsp;
	                        </legend>
	                        <table cellpadding="3">
	                            <tr id="rowNewUser" runat="server">
	                                <td class="NormalBold" width="190" nowrap>
	                                    <asp:Label ID="lblUsername" runat="server" meta:resourcekey="lblUsername" Text="Username:"></asp:Label>
	                                </td>
	                                <td width="100%" class="Normal">
                                        <uc2:UsernameControl id="databaseUser" runat="server">
                                        </uc2:UsernameControl>
                                     </td>
	                            </tr>
	                            <tr id="rowExistingUser" runat="server">
	                                <td class="NormalBold" width="190" nowrap>
	                                    <asp:Label ID="lblUsername2" runat="server" meta:resourcekey="lblUsername" Text="Username:"></asp:Label>
	                                </td>
	                                <td width="100%" class="Normal">
                                        <asp:dropdownlist id="ddlUser" runat="server" DataValueField="Id" DataTextField="Name"
								            CssClass="NormalTextBox"></asp:dropdownlist>
                                        <asp:RequiredFieldValidator ID="valRequireUser" runat="server" ControlToValidate="ddlUser"
                                            CssClass="NormalBold" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
	                            </tr>
	                            <tr>
	                                <td class="NormalBold" valign="top">
	                                    <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label>
	                                </td>
	                                <td class="Normal">
                                        <uc3:PasswordControl id="databasePassword" runat="server">
                                        </uc3:PasswordControl></td>
	                            </tr>
	                        </table>
	                    </fieldset>
	                    <br />
	                </td>
	            </tr>
	        </table>
	     </asp:Panel>
	</div>
	<div id="divSettings" runat="server">
	    <!-- app settings -->
        <scp:CollapsiblePanel id="secAppSettings" runat="server"
            TargetControlID="SettingsPanel" meta:resourcekey="secAppSettings" Text="Application settings">
        </scp:CollapsiblePanel>
        <asp:Panel ID="SettingsPanel" runat="server" Height="0" style="overflow:hidden;">
	        <table width="100%" runat="server">
		        <tr>
			        <td class="Normal"><asp:placeholder id="appSettings" Runat="server"></asp:placeholder></td>
		        </tr>
	        </table>
	    </asp:Panel>
	</div>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnInstall" CssClass="btn btn-success" runat="server" OnClick="btnInstall_Click" OnClientClick="ShowProgressDialog('Installing application...');"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInstallText"/> </CPCC:StyleButton>
</div>