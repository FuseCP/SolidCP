<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Proxmox_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Proxmox_Settings" %>


<asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="true" ShowSummary="false" />

<fieldset>
    <legend>
        <asp:Localize ID="locProxmoxServer" runat="server" meta:resourcekey="locProxmoxCluster" Text="Proxmox Cluster"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" style="margin: 10px;">



	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locProxmoxClusterServerHost" runat="server" meta:resourcekey="locProxmoxClusterServerHost" Text="Proxmox Cluster Server Host:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtProxmoxClusterServerHost" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="ProxmoxClusterServerHostValidator" runat="server" ControlToValidate="txtProxmoxClusterServerHost"
                    Text="*" meta:resourcekey="ProxmoxClusterServerHostValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>


	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locProxmoxClusterServerPort" runat="server" meta:resourcekey="locProxmoxClusterServerPort" Text="Proxmox Cluster Server Port:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtProxmoxClusterServerPort" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="ProxmoxClusterServerPortValidator" runat="server" ControlToValidate="txtProxmoxClusterServerPort"
                    Text="*" meta:resourcekey="ProxmoxClusterServerPortValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>


        <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locProxmoxClusterAdminUser" runat="server" meta:resourcekey="locProxmoxClusterAdminUser" Text="Proxmox Admin User:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtProxmoxClusterAdminUser" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="ProxmoxClusterAdminUserValidator" runat="server" ControlToValidate="txtProxmoxClusterAdminUser"
                    Text="*" meta:resourcekey="ProxmoxClusterAdminUserValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>

         <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locProxmoxClusterRealm" runat="server" meta:resourcekey="locProxmoxClusterRealm" Text="Proxmox Admin User Realm:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtProxmoxClusterRealm" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="ProxmoxClusterRealmValidator" runat="server" ControlToValidate="txtProxmoxClusterRealm"
                    Text="*" meta:resourcekey="ProxmoxClusterRealmValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>


     <tr id="rowPassword" runat="server">
		<td class="SubHead">
		    <asp:Label ID="lblCurrPassword" runat="server" meta:resourcekey="lblCurrPassword" Text="Current Proxmox Admin Password:"></asp:Label>
		</td>
		<td class="Normal">*******
		</td>
	</tr>
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locProxmoxClusterAdminPass" runat="server" meta:resourcekey="locProxmoxClusterAdminPass" Text="Proxmox Admin Pass:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtProxmoxClusterAdminPass" TextMode="Password"></asp:TextBox>
                
            </td>
	    </tr>
<%--
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locProxmoxClusterNode" runat="server" meta:resourcekey="locProxmoxClusterNode" Text="Proxmox Cluster Node:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtProxmoxClusterNode" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="ProxmoxClusterNodeValidator" runat="server" ControlToValidate="txtProxmoxClusterNode"
                    Text="*" meta:resourcekey="ProxmoxClusterNodeValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
--%>

      </table>



</fieldset>
<br />



<fieldset>
    <legend>
        <asp:Localize ID="locProxmoxSSH" runat="server" meta:resourcekey="locDeploySSH" Text="Proxmox VM deploy SSH"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" style="margin: 10px;">



	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHServerHost" runat="server" meta:resourcekey="locDeploySSHServerHost" Text="SSH Server Host:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtDeploySSHServerHost" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="DeploySSHServerHostValidator" runat="server" ControlToValidate="txtDeploySSHServerHost"
                    Text="*" meta:resourcekey="DeploySSHServerHostValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>


	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHServerPort" runat="server" meta:resourcekey="locDeploySSHServerPort" Text="SSH Server Port:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtDeploySSHServerPort" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="DeploySSHServerPortValidator" runat="server" ControlToValidate="txtDeploySSHServerPort"
                    Text="*" meta:resourcekey="DeploySSHServerPortValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>


        <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHUser" runat="server" meta:resourcekey="locDeploySSHUser" Text="SSH User:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtDeploySSHUser" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="DeploySSHUserValidator" runat="server" ControlToValidate="txtDeploySSHUser"
                    Text="*" meta:resourcekey="DeploySSHUserValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>


             <tr id="rowSSHPassword" runat="server">
		<td class="SubHead">
		    <asp:Label ID="lblCurrSSHPassword" runat="server" meta:resourcekey="lblCurrSSHPassword" Text="Current SSH Password:"></asp:Label>
		</td>
		<td class="Normal">******* <asp:CheckBox ID="chkdelsshpass" runat="server" Checked="false" meta:resourcekey="chkdelsshpass" Text="Delete SSH Password" />
		</td>
	</tr>

	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHPass" runat="server" meta:resourcekey="locDeploySSHPass" Text="SSH Password:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtDeploySSHPass" TextMode="Password"></asp:TextBox>
            </td>
	    </tr>

	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHKey" runat="server" meta:resourcekey="locDeploySSHKey" Text="SSH Private Key:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox ID="txtDeploySSHKey" runat="server" Rows="10" TextMode="MultiLine" Width="300px"></asp:TextBox>
            </td>
	    </tr>   
                            
        <tr id="rowSSHKEYPassword" runat="server">
		    <td class="SubHead">
    		    <asp:Label ID="lblCurrSSHPassphrase" runat="server" meta:resourcekey="lblCurrSSHPassphrase" Text="Current SSH Key Passphrase:"></asp:Label>
		    </td>
		    <td class="Normal">******* <asp:CheckBox ID="chkdelsshkeypass" runat="server" Checked="false" meta:resourcekey="chkdelsshkeypass" Text="Delete SSH Key Passphrase" />
		    </td>
	    </tr>
        <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHKeyPass" runat="server" meta:resourcekey="locDeploySSHKeyPass" Text="SSH Key Passphrase:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtDeploySSHKeyPass" TextMode="Password"></asp:TextBox>
            </td>
	    </tr>
 
        			
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHScript" runat="server" meta:resourcekey="locDeploySSHScript" Text="VM Deploy Script:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtDeploySSHScript" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="DeploySSHScriptValidator" runat="server" ControlToValidate="txtDeploySSHScript"
                    Text="*" meta:resourcekey="DeploySSHScriptValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
        			
			
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locDeploySSHScriptParams" runat="server" meta:resourcekey="locDeploySSHScriptParams" Text="VM Deploy Script Parameters:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="470px" CssClass="NormalTextBox" Runat="server" ID="txtDeploySSHScriptParams" Required="true"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="DeploySSHScriptParamsValidator" runat="server" ControlToValidate="txtDeploySSHScriptParams"
                    Text="*" meta:resourcekey="DeploySSHScriptParamsValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
    </table>

    <p style="margin: 10px;">
	    <asp:Localize ID="locvmdeploytext" runat="server" meta:resourcekey="locvmdeploytext" Text="Help text goes here..."></asp:Localize>
	</p>
</fieldset>
<br />


<fieldset>
    <legend>
        <asp:Localize ID="locTemplates" runat="server" meta:resourcekey="locTemplates" Text="OS Templates"></asp:Localize>
    </legend>

    <div style="margin-top: 15px;margin-bottom: 25px;margin-left: 10px;">
        <asp:Button ID="btnAddOsTemplate" runat="server" meta:resourcekey="btnAddOsTemplate"
            CssClass="Button1" Text="Add OS Template" CausesValidation="false"
            OnClick="btnAddOsTemplate_Click" />
    </div>
    <asp:Repeater ID="repOsTemplates" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
                <tr>
                    <td class="SubHead" style="width: 200px;">
                        <asp:Localize ID="locTemplateName" runat="server" meta:resourcekey="locTemplateName" Text="Name:"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="NormalTextBox" runat="server" ID="txtTemplateName" Text='<%# Eval("Name") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TemplateNameValidator" runat="server" ControlToValidate="txtTemplateName"
                            Text="*" meta:resourcekey="TemplateNameValidator" Display="Dynamic" SetFocusOnError="true" />
                    </td>
                    <td rowspan="3">
                        <asp:CheckBox ID="chkLegacyNetworkAdapter" runat="server" Checked='<%# Eval("LegacyNetworkAdapter") %>' meta:resourcekey="chkLegacyNetworkAdapter" Text="Use legacy adapter" /><br />
                        <%--<asp:CheckBox ID="chkRemoteDesktop" runat="server" Checked='<%# Eval("RemoteDesktop") %>' meta:resourcekey="chkRemoteDesktop" Text="Remote desktop" /><br/>--%>
                        <asp:CheckBox ID="chkCanSetComputerName" runat="server" Checked='<%# Eval("ProvisionComputerName") %>' meta:resourcekey="chkCanSetComputerName" Text="Can set a computer name" /><br />
                        <asp:CheckBox ID="chkCanSetAdminPass" runat="server" Checked='<%# Eval("ProvisionAdministratorPassword") %>' meta:resourcekey="chkCanSetAdminPass" Text="Can set an Administrator password" /><br />
                        <asp:CheckBox ID="chkCanSetNetwork" runat="server" Checked='<%# Eval("ProvisionNetworkAdapters") %>' meta:resourcekey="chkCanSetNetwork" Text="Can set Ip addresses" /><br />
                        <asp:CheckBox ID="chkDeployScript" runat="server" Checked='true' Enabled="false" meta:resourcekey="chkDeployScript" Text="Use Deploy Script" /><br />
                    </td>
                    <td rowspan="3">
                        <asp:Button ID="btnRemoveOsTemplate" runat="server" meta:resourcekey="btnRemoveOsTemplate"
                            CssClass="Button1" Text="Remove" CausesValidation="false"
                            CommandName="Remove" CommandArgument="<%# Container.ItemIndex %>" OnCommand="btnRemoveOsTemplate_OnCommand" />
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locTemplateFileName" runat="server" meta:resourcekey="locTemplateFileName" Text="File name (with extension):"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="NormalTextBox" runat="server" ID="txtTemplateFileName" Text='<%# Eval("Path") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TemplateFileNameValidator" runat="server" ControlToValidate="txtTemplateFileName"
                            Text="*" meta:resourcekey="TemplateFileNameValidator" Display="Dynamic" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locProcessVolume" runat="server" meta:resourcekey="locProcessVolume" Text="Index of the volume to expand:"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="NormalTextBox" runat="server" ID="txtProcessVolume" Text='<%# Eval("ProcessVolume") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valProcessVolume" runat="server" ControlToValidate="txtProcessVolume"
                            Text="*" meta:resourcekey="valProcessVolume" Display="Dynamic" SetFocusOnError="true" />
                        <asp:CompareValidator runat="server" ID="vcmProcessVolume" ControlToValidate="txtProcessVolume"
                            Type="Integer" Operator="GreaterThanEqual" Display="Dynamic" ValueToCompare="0" meta:resourcekey="vcmProcessVolume" />
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locSysprep" runat="server" meta:resourcekey="locSysprep" Text="Sysprep files:"></asp:Localize>
                    </td>
                    <td colspan="2">
                        <asp:TextBox Width="470px" CssClass="NormalTextBox" runat="server" ID="txtSysprep" Enabled="false" Text='<%# Eval("SysprepFiles") != null ? string.Join(";", (string[])Eval("SysprepFiles")) : "" %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locDeployScriptParams" runat="server" meta:resourcekey="locDeployScriptParams" Text="Additional Deploy Script Parameters:"></asp:Localize>
                    </td>
                    <td colspan="2">
                        <asp:TextBox Width="470px" CssClass="NormalTextBox" runat="server" ID="txtDeployScriptParams" Text='<%# Eval("DeployScriptParams") %>'></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <SeparatorTemplate>
            <br/>
            <%--<hr style="margin-bottom: 20px; margin-top: 10px; margin-left: 10px; margin-right: 10px;"/>--%>
        </SeparatorTemplate>
    </asp:Repeater>
</fieldset>
<br />


<fieldset>
    <legend>
        <asp:Localize ID="Localize1" runat="server" meta:resourcekey="locProxmoxISOs" Text="DVD Library"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" style="margin: 10px;">
       <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locProxmoxIsosonStorage" runat="server" meta:resourcekey="locProxmoxIsosonStorage" Text="DVD ISO Images on Storage:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="NormalTextBox" Runat="server" ID="txtProxmoxIsosonStorage"></asp:TextBox>
                                

            </td>
	    </tr>

	</table>


</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locHostname" runat="server" meta:resourcekey="locHostname" Text="Host name"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locHostnamePattern" runat="server" meta:resourcekey="locHostnamePattern" Text="VPS host name pattern:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="200px" CssClass="NormalTextBox" Runat="server" ID="txtHostnamePattern"></asp:TextBox>
                <asp:RequiredFieldValidator ID="HostnamePatternValidator" runat="server" ControlToValidate="txtHostnamePattern"
                    Text="*" meta:resourcekey="HostnamePatternValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
	</table>
	<p style="margin: 10px;">
	    <asp:Localize ID="locPatternText" runat="server" meta:resourcekey="locPatternText" Text="Help text goes here..."></asp:Localize>
	</p>
</fieldset>
<br />


<br />
