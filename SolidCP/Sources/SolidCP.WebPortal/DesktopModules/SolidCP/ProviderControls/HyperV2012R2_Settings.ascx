﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HyperV2012R2_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.HyperV2012R2_Settings" %>
<%@ Register Src="../UserControls/EditIPAddressControl.ascx" TagName="EditIPAddressControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>

<asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="true" ShowSummary="false" />

<fieldset>
    <legend>
        <asp:Localize ID="locHyperVServer" runat="server" meta:resourcekey="locHyperVServer" Text="Hyper-V Server"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px;">
	    <tr>
		    <td colspan="2">
		        <asp:RadioButtonList ID="radioServer" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="radioServer_SelectedIndexChanged">
                    <asp:ListItem Value="local" meta:resourcekey="radioServerLocal" Selected="True">Local</asp:ListItem>
                    <asp:ListItem Value="remote" meta:resourcekey="radioServerRemote">Remote</asp:ListItem>
                </asp:RadioButtonList>
		    </td>
	    </tr>
	    <tr id="ServerNameRow" runat="server">
		    <td class="SubHead" style="padding-left:30px;" colspan="2">
		        <asp:Localize ID="locServerName" runat="server" meta:resourcekey="locServerName" Text="Server name:"></asp:Localize>
                <asp:TextBox Width="200px" CssClass="form-control" Runat="server" ID="txtServerName"></asp:TextBox>
                <p style="margin: 10px;">
	                <asp:Localize ID="locRemoteServerHelp" runat="server" meta:resourcekey="locRemoteServerHelp" Text="Help text goes here..."></asp:Localize>
	            </p>
                <CPCC:StyleButton id="btnConnect" CssClass="btn btn-success" runat="server" OnClick="btnConnect_Click" CausesValidation="false"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnConnectText"/> </CPCC:StyleButton>
                <asp:RequiredFieldValidator ID="ServerNameValidator" runat="server" ControlToValidate="txtServerName"
                    Text="*" meta:resourcekey="ServerNameValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
	    <tr id="ServerErrorRow" runat="server">
	        <td colspan="2">
	            <asp:Label ID="locErrorReadingNetworksList" runat="server"
	                meta:resourcekey="locErrorReadingNetworksList" ForeColor="Red"></asp:Label>
	        </td>
	    </tr>
	</table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locMaintenanceMode" runat="server" meta:resourcekey="locMaintenanceMode" Text="Maintenance Mode:"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
	    <tr>
			<td class="SubHead" style="width:200px; vertical-align: top;">
				<asp:Localize ID="locStatusMaintenanceMode" runat="server" meta:resourcekey="locStatusMaintenanceMode" Text="Maintenance mode status:"></asp:Localize>
			</td>
			<td>
				<asp:RadioButtonList ID="radioMaintenanceMode" runat="server">
					<asp:ListItem Value="disabled" meta:resourcekey="radioMaintenanceModeDisable" Selected="True">Disabled</asp:ListItem>
					<asp:ListItem Value="enabled" meta:resourcekey="radioMaintenanceModeEnable">Enabled</asp:ListItem>
				</asp:RadioButtonList>
			</td>
		</tr>
	</table>
	<p style="margin: 10px;">
	    <asp:Localize ID="locMaintenanceModeText" runat="server" meta:resourcekey="locMaintenanceModeText" Text="Help text goes here..."></asp:Localize>
	</p>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locGuacamole" runat="server" meta:resourcekey="locGuacamole" Text="Guacamole"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px;">

	    <tr>
		    <td class="SubHead" style="width:250px;">
		        <asp:Localize ID="locGuacamoleConnectScript" runat="server" meta:resourcekey="locGuacamoleConnectScript" Text="Guacamole Connect Script URL:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="form-control" Runat="server" ID="txtGuacamoleConnectScript"></asp:TextBox>
            </td>
	    </tr>


	    <tr>
		    <td class="SubHead" style="width:250px;">
		        <asp:Localize ID="locGuacamoleConnectPassword" runat="server" meta:resourcekey="locGuacamoleConnectPassword" Text="Guacamole Encryption Password Base64 Encoded 256Bit Key:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="form-control" Runat="server" ID="txtGuacamoleConnectPassword"></asp:TextBox>
                <asp:Button ID="btnguacamolepassword" runat="server" meta:resourcekey="btnguacamolepassword" CssClass="Button1" Text="Generate Random Password" CausesValidation="false" 
                    onclick="btnguacamolepassword_Click" />
            </td>
	    </tr>

       <tr>
		    <td class="SubHead" style="width:250px;">
		        <asp:Localize ID="locGuacamoleHyperVIP" runat="server" meta:resourcekey="locGuacamoleHyperVIP" Text="Hyper-V IP:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="form-control" Runat="server" ID="txtGuacamoleHyperVIP"></asp:TextBox>
                
            </td>
	    </tr>

        <tr>
		    <td class="SubHead" style="width:250px;">
		        <asp:Localize ID="locGuacamoleHyperVDomain" runat="server" meta:resourcekey="locGuacamoleHyperVDomain" Text="Hyper-V Domain:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="form-control" Runat="server" ID="txtGuacamoleHyperVDomain"></asp:TextBox>
                
            </td>
	    </tr>

        <tr>
		    <td class="SubHead" style="width:250px;">
		        <asp:Localize ID="locGuacamoleHyperVUser" runat="server" meta:resourcekey="locGuacamoleHyperVUser" Text="Hyper-V User:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="form-control" Runat="server" ID="txtGuacamoleHyperVUser" Text="Administrator"></asp:TextBox>
                
            </td>
	    </tr>

        <tr>
		    <td class="SubHead" style="width:250px;">
		        <asp:Localize ID="locGuacamoleHyperVAdministratorPassword" runat="server" meta:resourcekey="locGuacamoleHyperVAdministratorPassword" Text="Hyper-V Password:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="250px" CssClass="form-control" Runat="server" ID="txtGuacamoleHyperVAdministratorPassword"  TextMode="Password"></asp:TextBox>
                
            </td>
	    </tr>

        <tr id="rowPassword" runat="server">
    		<td class="SubHead">
		        <asp:Label ID="lblCurrPassword" runat="server" meta:resourcekey="lblCurrPassword" Text="Current Hyper-V Password:"></asp:Label>
    		</td>
    		<td class="Normal">*******
		    </td>
	    </tr>

	</table>
</fieldset>
<br />


<fieldset>
    <legend>
        <asp:Localize ID="locGeneralSettings" runat="server" meta:resourcekey="locGeneralSettings" Text="General Settings"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locVpsRootFolder" runat="server" meta:resourcekey="locVpsRootFolder" Text="VPS root folder:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="400px" CssClass="form-control" Runat="server" ID="txtVpsRootFolder"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RootFolderValidator" runat="server" ControlToValidate="txtVpsRootFolder"
                    Text="*" meta:resourcekey="RootFolderValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
	    <tr>
	        <td></td>
	        <td>
	            <asp:Localize ID="locFolderVariables" runat="server" meta:resourcekey="locFolderVariables" Text="The following variables..."></asp:Localize>
	            <br />
	            <br />
	        </td>
	    </tr>
	  
	    <tr>
		    <td class="SubHead">
		        <asp:Localize ID="locExportedVpsPath" runat="server" meta:resourcekey="locExportedVpsPath" Text="Exported VPS path:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="300px" CssClass="form-control" Runat="server" ID="txtExportedVpsPath"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ExportedVpsPathValidator" runat="server" ControlToValidate="txtExportedVpsPath"
                    Text="*" meta:resourcekey="ExportedVpsPathValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
	 </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locProcessorSettings" runat="server" meta:resourcekey="locProcessorSettings" Text="Processor Resource Settings"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px;">
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locCpuReserve" runat="server" meta:resourcekey="locCpuReserve" Text="Virtual machine reserve:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="50px" CssClass="form-control" Runat="server" ID="txtCpuReserve"></asp:TextBox>
                <asp:RequiredFieldValidator ID="CpuReserveValidator" runat="server" ControlToValidate="txtCpuReserve"
                    Text="*" meta:resourcekey="CpuReserveValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
            <td>%</td>
	    </tr>
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locCpuLimit" runat="server" meta:resourcekey="locCpuLimit" Text="Virtual machine limit:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="50px" CssClass="form-control" Runat="server" ID="txtCpuLimit"></asp:TextBox>
                <asp:RequiredFieldValidator ID="CpuLimitValidator" runat="server" ControlToValidate="txtCpuLimit"
                    Text="*" meta:resourcekey="CpuLimitValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
            <td>%</td>
	    </tr>
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locCpuWeight" runat="server" meta:resourcekey="locCpuWeight" Text="Relative weight:"></asp:Localize>
		    </td>
		    <td colspan="2">
                <asp:TextBox Width="50px" CssClass="form-control" Runat="server" ID="txtCpuWeight"></asp:TextBox>
                <asp:RequiredFieldValidator ID="CpuWeightValidator" runat="server" ControlToValidate="txtCpuWeight"
                    Text="*" meta:resourcekey="CpuWeightValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
	 </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locRamSettings" runat="server" meta:resourcekey="locRamSettings" Text="Memory Resource Settings"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px;">
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locRamReserve" runat="server" meta:resourcekey="locRamReserve" Text="Node RAM reserve:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="200px" CssClass="form-control" Runat="server" ID="txtRamReserve"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RamReserveValidator" runat="server" ControlToValidate="txtRamReserve"
                    Text="*" meta:resourcekey="RamReserveValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
            <td>MB (0 = disabled)</td>
	    </tr>
	 </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locConfigVersion" runat="server" meta:resourcekey="locConfigVersion" Text="Virtual Machines Configuration Version"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
	    <tr><td class="SubHead" style="width:200px;">
                        <asp:Localize ID="locHyperVConfig" runat="server" meta:resourcekey="locHyperVConfig" Text="HyperV Config Version:"></asp:Localize>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlHyperVConfig" runat="server" CssClass="form-control" Width="450"
                            DataValueField="Version" DataTextField="Name">
                        </asp:DropDownList>                        
                    </td>
                </tr>
	</table>
	<p style="margin: 10px;">
	    <asp:Localize ID="locConfigVersionText" runat="server" meta:resourcekey="locConfigVersionText" Text="Help text goes here..."></asp:Localize>
	</p>
</fieldset>
<br />
<fieldset>
    <legend>
        <asp:Localize ID="locTemplates" runat="server" meta:resourcekey="locTemplates" Text="OS Templates"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
          <tr>
		    <td class="SubHead">
		        <asp:Localize ID="locOSTemplatesPath" runat="server" meta:resourcekey="locOSTemplatesPath" Text="OS Templates path:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="300px" CssClass="form-control" Runat="server" ID="txtOSTemplatesPath"></asp:TextBox>
                <asp:RequiredFieldValidator ID="TemplatesPathValidator" runat="server" ControlToValidate="txtOSTemplatesPath"
                    Text="*" meta:resourcekey="TemplatesPathValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>        
    </table>
    <div style="margin-top: 15px;margin-bottom: 25px;margin-left: 10px;">
        <CPCC:StyleButton id="btnAddOsTemplate" CssClass="btn btn-success" runat="server" OnClick="btnAddOsTemplate_Click" CausesValidation="false"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddOsTemplateText"/> </CPCC:StyleButton>
    </div>
    <asp:Repeater ID="repOsTemplates" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
                <tr>
                    <td class="SubHead" style="width: 200px;">
                        <asp:Localize ID="locTemplateName" runat="server" meta:resourcekey="locTemplateName" Text="Name:"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtTemplateName" Text='<%# Eval("Name") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TemplateNameValidator" runat="server" ControlToValidate="txtTemplateName"
                            Text="*" meta:resourcekey="TemplateNameValidator" Display="Dynamic" SetFocusOnError="true" />
                    </td>
                    <td rowspan="3">
                        <asp:CheckBox ID="chkLegacyNetworkAdapter" runat="server" Checked='<%# Eval("LegacyNetworkAdapter") %>' meta:resourcekey="chkLegacyNetworkAdapter" Text="Use legacy adapter (First Generation only)" Enabled='<%# IsLegacyAdapterSupport(Eval("Generation")) %>' /><br />
                        <%--<asp:CheckBox ID="chkRemoteDesktop" runat="server" Checked='<%# Eval("RemoteDesktop") %>' meta:resourcekey="chkRemoteDesktop" Text="Remote desktop" /><br/>--%>
                        <asp:CheckBox ID="chkCanSetComputerName" runat="server" Checked='<%# Eval("ProvisionComputerName") %>' meta:resourcekey="chkCanSetComputerName" Text="Can set a computer name" /><br />
                        <asp:CheckBox ID="chkCanSetAdminPass" runat="server" Checked='<%# Eval("ProvisionAdministratorPassword") %>' meta:resourcekey="chkCanSetAdminPass" Text="Can set an Administrator password" /><br />
                        <asp:CheckBox ID="chkCanSetNetwork" runat="server" Checked='<%# Eval("ProvisionNetworkAdapters") %>' meta:resourcekey="chkCanSetNetwork" Text="Can set Ip addresses" /><br />
                        <asp:CheckBox ID="chkEnableSecureBoot" runat="server" Checked='<%# Eval("EnableSecureBoot") %>' meta:resourcekey="chkEnableSecureBoot" Text="Enable Secure Boot (Second Generation only)" AutoPostBack="True" onCheckedChanged="cbEnableSecureBoot_OnChecked"/><br />
                    </td>
                    <td rowspan="3">
                        <CPCC:StyleButton id="btnRemoveOsTemplate" CssClass="btn btn-danger" runat="server" CausesValidation="false" CommandName="Remove" CommandArgument="<%# Container.ItemIndex %>" OnCommand="btnRemoveOsTemplate_OnCommand"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRemoveOsTemplateText"/> </CPCC:StyleButton>
                    </td>
                </tr>
                <tr><td class="SubHead">
                        <asp:Localize ID="locTemplateGeneration" runat="server" meta:resourcekey="locTemplateGeneration" Text="Generation of VM:"></asp:Localize>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTemplateGeneration" runat="server" CssClass="form-control" Width="350"
                            DataValueField="TemplateGenerationId" DataTextField="Name" SelectedIndex='<%# Eval("Generation") %>'>
                                <asp:ListItem Text="Select a VM Generation" Value="0"></asp:ListItem>
                                <asp:ListItem Text="First" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Second" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RegularExpressionValidator id="valTemplateGeneration" runat="server" Text="*" meta:resourcekey="valTemplateGeneration"
							ValidationExpression="^[1-9]+$"
							ControlToValidate="ddlTemplateGeneration" Display="Dynamic" SetFocusOnError="true">
						</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locTemplateFileName" runat="server" meta:resourcekey="locTemplateFileName" Text="File name (with extension):"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtTemplateFileName" Text='<%# Eval("Path") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TemplateFileNameValidator" runat="server" ControlToValidate="txtTemplateFileName"
                            Text="*" meta:resourcekey="TemplateFileNameValidator" Display="Dynamic" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locProcessVolume" runat="server" meta:resourcekey="locProcessVolume" Text="Index of the volume to expand:"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtProcessVolume" Text='<%# Eval("ProcessVolume") %>'></asp:TextBox>
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
                        <asp:TextBox Width="470px" CssClass="form-control" runat="server" ID="txtSysprep" Text='<%# Eval("SysprepFiles") != null ? string.Join(";", (string[])Eval("SysprepFiles")) : "" %>'></asp:TextBox>
                    </td>
                </tr>               

                <tr>
                    <div runat="server" ID="dSecureBootTemplate" visible='<%# GetSecureBootTemplatesList().Count > 0 %>'>
                        <td class="SubHead">
                            <asp:Localize ID="locSecureBootTemplate" runat="server" meta:resourcekey="locSecureBootTemplate" Text="Secure boot template:"></asp:Localize>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSecureBootTemplate" runat="server" CssClass="form-control" Width="470" Enabled='<%# IsSecureBootEnabled(Eval("EnableSecureBoot")) %>' DataSource ='<%# GetSecureBootTemplatesList() %>'
                                DataValueField="Name" DataTextField="Description" SelectedIndex='<%# GetSecureBootTemplateIndex(Eval("SecureBootTemplate")) %>'>
                            </asp:DropDownList>
                        </td>
                    </div>
                </tr>
                
                <tr>
                    <td colspan="3">
                        <scp:CollapsiblePanel id="clpAdvancedTemplateSettings" runat="server" IsCollapsed="true"
	                        TargetControlID="pAdvancedTemplateSettings" meta:resourcekey="clpAdvancedTemplateSettings" Text="Advanced template settings">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="pAdvancedTemplateSettings" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
                                <tr>
	                                <td class="SubHead">
		                                <asp:Localize ID="locVhdBlockSizeBytes" runat="server" meta:resourcekey="locVhdBlockSizeBytes" Text="VHD Block Size (Bytes):"></asp:Localize>
	                                </td>
	                                <td>
		                                <asp:TextBox Width="400px" CssClass="form-control" runat="server" ID="txtVhdBlockSizeBytes" Text='<%# Eval("VhdBlockSizeBytes") %>'></asp:TextBox>
	                                </td>
                                    <td>
		                                <asp:Localize ID="LocBlockSizeDesc" runat="server" meta:resourcekey="LocBlockSizeDesc" Text="Default value is 0. Examples: 1024KB, 1MB, 32MB, etc."></asp:Localize>
	                                </td>
                                </tr>
                                <tr>
	                                <td class="SubHead">
		                                <asp:Localize ID="locDiskSize" runat="server" meta:resourcekey="locDiskSize" Text="VHD Disk Size (GB):"></asp:Localize>
	                                </td>
	                                <td>
		                                <asp:TextBox Width="400px" CssClass="form-control" runat="server" ID="txtDiskSize" Text='<%# Eval("DiskSize") %>'></asp:TextBox>
	                                </td>
                                </tr>
                                <tr>
	                                <td class="SubHead">
		                                <asp:Localize ID="locTemplateTimeZone" runat="server" meta:resourcekey="locTemplateTimeZone" Text="OS Time Zone (#os_template.TimeZoneId#):"></asp:Localize>
	                                </td>
	                                <td>
                                        <asp:TextBox Width="300px" CssClass="form-control" Runat="server" ID="txtManualTempplateTimeZone" Text='<%# Eval("timeZoneId") %>'></asp:TextBox>
	                                </td>
	                                <td>
                                        <asp:DropDownList ID="ddlTemplateTimeZone" runat="server" CssClass="form-control" Width="450"
	                                                DataValueField="Key" DataTextField="Value" DataSource='<%# SolidCP.EnterpriseServer.Base.Virtualization.VirtualMachineTimeZoneList.GetList() %>'
                                            SelectedValue='<%#  SetSelectedValueIfTimeZoneExis(Eval("timeZoneId")) %>'>
                                        </asp:DropDownList>
	                                </td>
                                </tr>
                                <tr>
                                    <td class="SubHead">
	                                    <asp:Localize ID="locCDKey" runat="server" meta:resourcekey="locCDKey" Text="OS CD-key (#os_template.CDKey#):"></asp:Localize>
                                    </td>
                                    <td>
	                                    <asp:TextBox Width="300px" CssClass="form-control" Runat="server" ID="txtTemplateCDKey" Text='<%# Eval("cdKey") %>'></asp:TextBox>
                                    </td>
                                </tr>                        
                            </table>
                        </asp:Panel>
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
        <asp:Localize ID="locDvdLibrary" runat="server" meta:resourcekey="locDvdLibrary" Text="DVD Library"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locDvdIsoPath" runat="server" meta:resourcekey="locDvdIsoPath" Text="Path to DVD ISO files:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="300px" CssClass="form-control" runat="server" ID="txtDvdLibraryPath"></asp:TextBox>
                <asp:RequiredFieldValidator ID="DvdLibraryPathValidator" runat="server" ControlToValidate="txtDvdLibraryPath"
                    Text="*" meta:resourcekey="DvdLibraryPathValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>
    <div style="margin-top: 15px;margin-bottom: 25px;margin-left: 10px;">
        <CPCC:StyleButton id="btnAddDvd" CssClass="btn btn-success" runat="server" OnClick="btnAddDvd_Click" CausesValidation="false"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddDvdText"/> </CPCC:StyleButton>
    </div>
    <asp:Repeater ID="repDvdLibrary" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
                <tr>
                    <td class="SubHead" style="width: 200px;">
                        <asp:Localize ID="locDvdName" runat="server" meta:resourcekey="locDvdName" Text="Name:"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtDvdName" Text='<%# Eval("Name") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="DvdNameValidator" runat="server" ControlToValidate="txtDvdName"
                            Text="*" meta:resourcekey="DvdNameValidator" Display="Dynamic" SetFocusOnError="true" />
                    </td>
                    <td rowspan="3">
                        <CPCC:StyleButton id="btnRemoveDvd" CssClass="btn btn-danger" runat="server" CausesValidation="false" CommandName="Remove" CommandArgument="<%# Container.ItemIndex %>" OnCommand="btnRemoveDvd_OnCommand"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRemoveDvdText"/> </CPCC:StyleButton>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locDvdDescription" runat="server" meta:resourcekey="locDvdDescription" Text="Description:"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtDvdDescription" Text='<%# Eval("Description") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="DvdDescriptionValidator" runat="server" ControlToValidate="txtDvdDescription"
                            Text="*" meta:resourcekey="DvdDescriptionValidator" Display="Dynamic" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="SubHead">
                        <asp:Localize ID="locDvdFileName" runat="server" meta:resourcekey="locDvdFileName" Text="File name (with extension):"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtDvdFileName" Text='<%# Eval("Path") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="DvdFileNameValidator" runat="server" ControlToValidate="txtDvdFileName"
                            Text="*" meta:resourcekey="DvdFileNameValidator" Display="Dynamic" SetFocusOnError="true" />
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
        <asp:Localize ID="locPsScript" runat="server" meta:resourcekey="locPsScript" Text="Custom PowerShell Scripts"></asp:Localize>
    </legend>
    <div style="margin-top: 15px;margin-bottom: 25px;margin-left: 10px;">
        <table>
            <tr>
                <td style="width: 250px;">
                    <asp:Localize ID="locVar" runat="server" meta:resourcekey="locVar" Text="The following variables are supported:"></asp:Localize>
                </td>
                <td>
                    <asp:Label ID="labPsVars" Font-Names="Consolas" runat="server" Text="$vmName, $vmId, $vmObject, $vmTemplateName, $vmTemplatePath, $extIpAddresses, $extMasks, $extGateway,
                        $extAdapterName, $extAdapterMac, $privIpAddresses, $privMasks, $privGateway, $privAdapterName, $privAdapterMac,
                        $mngIpAddresses, $mngMasks, $mngGateway, $mngAdapterName, $mngAdapterMac"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 15px;margin-bottom: 25px;margin-left: 10px;">
        <CPCC:StyleButton id="btnAddPsScript" CssClass="btn btn-success" runat="server" OnClick="btnAddPsScript_Click" CausesValidation="false"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddPsScript"/> </CPCC:StyleButton>
    </div>
    <asp:Repeater ID="repPsScript" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
                <tr>
                    <td class="SubHead" style="width: 200px;">
                        <asp:Localize ID="locRunAt" runat="server" meta:resourcekey="locRunAt" Text="Execute at:"></asp:Localize>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRunAt" runat="server" CssClass="form-control" Width="450"
                            SelectedIndex='<%# GetPsScriptIndex(Container, Eval("Name")) %>'>
                                <asp:ListItem meta:resourcekey="liRunAtDisabled" Text="Disabled" Value="disabled"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="liRunAtAfterCreation" Text="After VM creation" Value="after_creation"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="liRunAtBeforeDeletion" Text="Before VM deletion" Value="before_deletion"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="liRunAtBeforeRenaming" Text="Before VM renaming" Value="before_renaming"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="liRunAtAfterRenaming" Text="After VM renaming" Value="after_renaming"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="liRunAtExtNetwork" Text="External network configuration" Value="external_network_configuration"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="liRunAtPrivNetwork" Text="Private network configuration" Value="private_network_configuration"></asp:ListItem>
                                <asp:ListItem meta:resourcekey="liRunAtMngNetwork" Text="Management network configuration" Value="management_network_configuration"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td rowspan="3">
                        <CPCC:StyleButton id="btnRemovePsScript" CssClass="btn btn-danger" runat="server" CausesValidation="false" CommandName="Remove" CommandArgument="<%# Container.ItemIndex %>" OnCommand="btnRemovePsScript_OnCommand"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRemovePsScript"/> </CPCC:StyleButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Localize ID="locScript" runat="server" meta:resourcekey="locScript" Text="PS Script:"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox runat="server" Font-Names="Consolas" Font-Size="11" ID="txtPsScript" TextMode="MultiLine" Rows="10" Width="100%" Spellcheck="false" Text='<%# Eval("Description") %>'></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <SeparatorTemplate>
            <br/>
        </SeparatorTemplate>
    </asp:Repeater>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locReplication" runat="server" meta:resourcekey="locReplication" Text="Replication"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
        <tr>
            <td>
                <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px;">
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="ReplicationModeList" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="radioServer_SelectedIndexChanged">
                            <asp:ListItem Value="None" meta:resourcekey="ReplicationModeDisabled" Selected="True">No Hyper-v Replication</asp:ListItem>
                            <asp:ListItem Value="ReplicationEnabled" meta:resourcekey="ReplicationModeEnabled">Enable Hyper-V Replication</asp:ListItem>
                            <asp:ListItem Value="IsReplicaServer" meta:resourcekey="ReplicationModeIsReplicaServer">This is a Replica Server</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="EnableReplicaRow" runat="server">
                    <td class="SubHead" style="padding-left: 20px;" colspan="2">
                        <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px;">
                            <tr>
                                <td style="width: 180px">
                                    <asp:Localize ID="locReplicaServer" runat="server" meta:resourcekey="locReplicaServer" Text="Replication Server:"></asp:Localize>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReplicaServer" runat="server" Width="300px"></asp:DropDownList>

                                    <asp:RequiredFieldValidator ID="ReplicaServerValidator" runat="server" ControlToValidate="ddlReplicaServer"
                                        Text="*" meta:resourcekey="ReplicaServerValidator" Display="Dynamic" SetFocusOnError="true" />
                                </td>
                            </tr>
                            <tr id="EnableReplicaErrorTr" runat="server" visible="False">
                                <td colspan="2">
                                    <asp:Label ID="locEnableReplicaError" runat="server" meta:resourcekey="locEnableReplicaError" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="IsReplicaServerRow" runat="server">
                <td class="SubHead" style="padding-left: 20px;" colspan="2">
                    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px;">
                        <tr>
                            <td style="width: 200px;">
                                <asp:Localize ID="locReplicaPath" runat="server" meta:resourcekey="locReplicaPath" Text="Path to Replications:"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox Width="300px" CssClass="form-control" runat="server" ID="txtReplicaPath"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReplicaPathValidator" runat="server" ControlToValidate="txtReplicaPath"
                                    Text="*" meta:resourcekey="ReplicaPathValidator" Display="Dynamic" SetFocusOnError="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px">
                                <asp:Localize ID="locCertThumbnail" runat="server" meta:resourcekey="locCertThumbnail" Text="SSL Certificate Thumbnail:"></asp:Localize>
                            </td>
                            <td> 
                                <asp:DropDownList ID="ddlCertThumbnail" runat="server" Width="500px"></asp:DropDownList>
                                <asp:TextBox Width="400px" CssClass="form-control" runat="server" ID="txtCertThumbnail"></asp:TextBox>
                                <CPCC:StyleButton id="btnSetReplicaServer" CssClass="btn btn-success" runat="server" OnClick="btnSetReplicaServer_Click" CausesValidation="false"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetReplicaServerText"/> </CPCC:StyleButton>
                                <asp:RequiredFieldValidator ID="CertificateThumbnailValidator" runat="server" ControlToValidate="txtCertThumbnail"
                                    Text="*" meta:resourcekey="CertificateThumbnailValidator" Display="Dynamic" SetFocusOnError="true" />
                                <asp:RequiredFieldValidator ID="CertificateDdlThumbnailValidator" runat="server" ControlToValidate="ddlCertThumbnail"
                                    Text="*" meta:resourcekey="CertificateThumbnailValidator" Display="Dynamic" SetFocusOnError="true" />
                            </td>
                        </tr>
                        <tr id="ReplicaPathErrorTr" runat="server" Visible="False">
                            <td colspan="2">
                                <asp:Label ID="locErrorPathReplica" runat="server"
                                    meta:resourcekey="locErrorPathReplica" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="ReplicaErrorTr" runat="server" Visible="False">
                            <td colspan="2">
                                <asp:Label ID="locErrorSetReplica" runat="server"
                                    meta:resourcekey="locErrorSetReplica" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </td>
                </tr>
	        </table>
            </td>
        </tr>        
	</table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locVhd" runat="server" meta:resourcekey="locVhd" Text="Virtual Hard Drive"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
	    <tr>
		    <td class="SubHead" style="width:200px; vertical-align: top;">
		        <asp:Localize ID="locDiskType" runat="server" meta:resourcekey="locDiskType" Text="Disk Type:"></asp:Localize>
		    </td>
		    <td>
                <asp:RadioButtonList ID="radioVirtualDiskType" runat="server">
                    <asp:ListItem Value="dynamic" meta:resourcekey="radioVirtualDiskTypeDynamic" Selected="True">Dynamic</asp:ListItem>
                    <asp:ListItem Value="fixed" meta:resourcekey="radioVirtualDiskTypeFixed">Fixed</asp:ListItem>
                </asp:RadioButtonList>
            </td>
	    </tr>
	</table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locExternalNetwork" runat="server" meta:resourcekey="locExternalNetwork" Text="External Network"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
        <tr>
            <td class="SubHead" style="width:200px; vertical-align: top;">
		        <asp:Localize ID="locSwitchType" runat="server" meta:resourcekey="locSwitchType" Text="Switch Type:"></asp:Localize>
		    </td>
	        <td colspan="2">
		        <asp:RadioButtonList ID="radioSwitchType" runat="server" AutoPostBack="true" 
			        onselectedindexchanged="radioSwitchType_SelectedIndexChanged">
			        <asp:ListItem Value="external" meta:resourcekey="radioSwitchTypeExternal" Selected="True">External</asp:ListItem>
			        <asp:ListItem Value="internal" meta:resourcekey="radioSwitchTypeInternal">Internal (Not recommended)</asp:ListItem>
		        </asp:RadioButtonList>
	        </td>
        </tr>
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locExternalNetworkName" runat="server" meta:resourcekey="locExternalNetworkName" Text="Connect to Network:"></asp:Localize>
		    </td>
		    <td>
                <asp:DropDownList ID="ddlExternalNetworks" runat="server" CssClass="form-control" Width="450"
                    DataValueField="SwitchId" DataTextField="Name"></asp:DropDownList>
            </td>
            <td>
                <asp:CheckBox ID="chkGetSwitchesByPS" runat="server" AutoPostBack="true" meta:resourcekey="chkGetSwitchesByPS" Text="Use an alternative method to get external switches. (slow)" OnCheckedChanged="chkGetSwitchesByPS_CheckedChanged" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Localize ID="locPreferredNameServer" runat="server" meta:resourcekey="locPreferredNameServer" Text="Preferred Name Server:"></asp:Localize>
		    </td>
		    <td colspan="2">
		        <scp:EditIPAddressControl id="externalPreferredNameServer" runat="server" Required="true" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Localize ID="locAlternateNameServer" runat="server" meta:resourcekey="locAlternateNameServer" Text="Alternate Name Server:"></asp:Localize>
		    </td>
		    <td colspan="2">
		        <scp:EditIPAddressControl id="externalAlternateNameServer" runat="server" />
            </td>
	    </tr>
	    <tr>
	        <td colspan="3">
	            <asp:CheckBox ID="chkAssignIPAutomatically" runat="server" meta:resourcekey="chkAssignIPAutomatically" Text="Assign IP addresses to the space on creation" />
	        </td>
	    </tr>
	</table>
</fieldset>
<br />

<asp:UpdatePanel ID="ManageUpdatePanel" runat="server" ChildrenAsTriggers="true">
    <ContentTemplate>
   
<fieldset>
    <legend>
        <asp:Localize ID="locManagementNetwork" runat="server" meta:resourcekey="locManagementNetwork" Text="Management Network"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
	    <tr>
		    <td style="width:200px;">
		        <asp:Localize ID="locManagementNetworkName" runat="server" meta:resourcekey="locManagementNetworkName" Text="Connect to Network:"></asp:Localize>
		    </td>
		    <td>
                <asp:DropDownList ID="ddlManagementNetworks" runat="server" 
                    CssClass="form-control" Width="450"
                    DataValueField="SwitchId" DataTextField="Name" AutoPostBack="true" 
                    onselectedindexchanged="ddlManagementNetworks_SelectedIndexChanged"></asp:DropDownList>
            </td>
	    </tr>
	    <tr id="ManageNicConfigRow" runat="server">
		    <td>
		        <asp:Localize ID="locManageNicConfig" runat="server" meta:resourcekey="locManageNicConfig" Text="Network Adapter Configuration:"></asp:Localize>
		    </td>
		    <td>
                <asp:DropDownList ID="ddlManageNicConfig" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlManageNicConfig_SelectedIndexChanged">
                    <asp:ListItem Value="Pool" meta:resourcekey="ddlManageNicConfigPool" Selected="True">POOL</asp:ListItem>
                    <asp:ListItem Value="DHCP" meta:resourcekey="ddlManageNicConfigDhcp">DHCP</asp:ListItem>
                </asp:DropDownList>
            </td>
	    </tr>
	    <tr id="ManagePreferredNameServerRow" runat="server">
		    <td>
		        <asp:Localize ID="locManagePreferredNameServer" runat="server" meta:resourcekey="locPreferredNameServer" Text="Preferred Name Server:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="managePreferredNameServer" runat="server" Required="true" />
            </td>
	    </tr>
	    <tr id="ManageAlternateNameServerRow" runat="server">
		    <td>
		        <asp:Localize ID="locManageAlternateNameServer" runat="server" meta:resourcekey="locAlternateNameServer" Text="Alternate Name Server:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="manageAlternateNameServer" runat="server" />
            </td>
	    </tr>
	</table>
</fieldset>

    </ContentTemplate>
</asp:UpdatePanel>
<br />

<asp:UpdatePanel ID="PrivUpdatePanel" runat="server" ChildrenAsTriggers="true">
    <ContentTemplate>
    
<fieldset>
    <legend>
        <asp:Localize ID="locPrivateNetwork" runat="server" meta:resourcekey="locPrivateNetwork" Text="Private Network"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locIPFormat" runat="server" meta:resourcekey="locIPFormat" Text="IP addresses format:"></asp:Localize>
		    </td>
		    <td>
                <asp:DropDownList ID="ddlPrivateNetworkFormat" runat="server" 
                    AutoPostBack="true" onselectedindexchanged="ddlPrivateNetworkFormat_SelectedIndexChanged">
                    <asp:ListItem Value="" meta:resourcekey="ddlPrivateNetworkFormatCustom">Custom</asp:ListItem>
                    <asp:ListItem Value="192.168.0.1/16" meta:resourcekey="ddlPrivateNetworkFormat192" Selected="True">192.168.0.1</asp:ListItem>
                    <asp:ListItem Value="172.16.0.1/12" meta:resourcekey="ddlPrivateNetworkFormat172">172.16.0.1</asp:ListItem>
                    <asp:ListItem Value="10.0.0.1/8" meta:resourcekey="ddlPrivateNetworkFormat10">10.0.0.1</asp:ListItem>
                </asp:DropDownList>
            </td>
	    </tr>
	    <tr id="PrivCustomFormatRow" runat="server">
		    <td class="SubHead">
		        <asp:Localize ID="locPrivCustomFormat" runat="server" meta:resourcekey="locPrivCustomFormat" Text="Start IP Address:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="privateIPAddress" runat="server" Required="true" />
		        /
		        <asp:TextBox ID="privateSubnetMask" runat="server" MaxLength="3" Width="40px" CssClass="form-control"></asp:TextBox>
		        <asp:RequiredFieldValidator ID="privateSubnetMaskValidator" runat="server" ControlToValidate="privateSubnetMask"
                    Text="*" meta:resourcekey="privateSubnetMaskValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Localize ID="locPrivDefaultGateway" runat="server" meta:resourcekey="locDefaultGateway" Text="Default Gateway:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="privateDefaultGateway" runat="server" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Localize ID="locPrivPreferredNameServer" runat="server" meta:resourcekey="locPreferredNameServer" Text="Preferred Name Server:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="privatePreferredNameServer" runat="server" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Localize ID="locPrivAlternateNameServer" runat="server" meta:resourcekey="locAlternateNameServer" Text="Alternate Name Server:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="privateAlternateNameServer" runat="server" />
            </td>
	    </tr>
        <tr>
            <td class="SubHead" style="width:200px; vertical-align: top;">
		        <asp:Localize ID="locSwitchTypePrivateNetwork" runat="server" meta:resourcekey="locSwitchType" Text="Switch Type:"></asp:Localize>
		    </td>
	        <td colspan="2">
		        <asp:RadioButtonList ID="radioSwitchTypePrivateNetwork" runat="server" AutoPostBack="true" 
			        onselectedindexchanged="radioSwitchTypePrivateNetwork_SelectedIndexChanged">
                    <asp:ListItem Value="private" meta:resourcekey="radioSwitchTypePrivatePrivate" Selected="True">New private switch for each Hosting Space</asp:ListItem>
			        <asp:ListItem Value="external" meta:resourcekey="radioSwitchTypePrivateExternal">Use external switch (VLAN separation)</asp:ListItem>
		        </asp:RadioButtonList>
	        </td>
        </tr>
        <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locPrivateNetworkName" runat="server" meta:resourcekey="locExternalNetworkName" Text="Connect to Network:"></asp:Localize>
		    </td>
		    <td>
                <asp:DropDownList ID="ddlExternalNetworksPrivate" runat="server" CssClass="form-control" Width="450" Enabled="false"
                    DataValueField="SwitchId" DataTextField="Name"></asp:DropDownList>
            </td>
	    </tr>
        <tr>
	        <td colspan="3">
	            <asp:CheckBox ID="chkAssignVLANAutomatically" runat="server" meta:resourcekey="chkAssignVLANAutomatically" Text="Assign VLAN to the space on creation" Enabled="false" />
	        </td>
	    </tr>
	</table>
</fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locHostname" runat="server" meta:resourcekey="locHostname" Text="Host name"></asp:Localize>
    </legend>
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locHostnamePattern" runat="server" meta:resourcekey="locHostnamePattern" Text="VPS host name pattern:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox CssClass="form-control" Runat="server" ID="txtHostnamePattern"></asp:TextBox>
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

<fieldset>
    <legend>
        <asp:Localize ID="locStartAction" runat="server" meta:resourcekey="locStartAction" Text="Automatic Start Action"></asp:Localize>
    </legend>
    
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
        <tr>
            <td>
                <asp:Localize ID="locStartOptionsText" runat="server" meta:resourcekey="locStartOptionsText" Text="What do you want VPS to do when the physical computer starts?"></asp:Localize>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButtonList ID="radioStartAction" runat="server">
                    <asp:ListItem Value="0" meta:resourcekey="radioStartActionNothing">Nothing</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="radioStartActionStart" Selected="True">Start</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="radioStartActionAlwaysStart">AlwaysStart</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Localize ID="locStartupDelayText" runat="server" meta:resourcekey="locStartupDelayText" Text="Specify a startup delay to reduce resource contention between virtual machines."></asp:Localize>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Localize ID="locStartupDelay" runat="server" meta:resourcekey="locStartupDelay" Text="Startup delay:"></asp:Localize>
                <asp:TextBox ID="txtStartupDelay" runat="server" Width="30px"></asp:TextBox>
                <asp:Localize ID="locSeconds" runat="server" meta:resourcekey="locSeconds" Text="seconds"></asp:Localize>
                <asp:RequiredFieldValidator ID="StartupDelayValidator" runat="server" ControlToValidate="txtStartupDelay"
                    Text="*" meta:resourcekey="StartupDelayValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locStopAction" runat="server" meta:resourcekey="locStopAction" Text="Automatic Stop Action"></asp:Localize>
    </legend>
    
    <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
        <tr>
            <td>
                <asp:Localize ID="locStopActionText" runat="server" meta:resourcekey="locStopActionText" Text="What do you want VPS to do when the physical shuts down?"></asp:Localize>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButtonList ID="radioStopAction" runat="server">
                    <asp:ListItem Value="1" meta:resourcekey="radioStopActionSave">Save</asp:ListItem>
                    <asp:ListItem Value="0" meta:resourcekey="radioStopActionTurnOff">TurnOff</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="radioStopActionShutDown" Selected="True">ShutDown</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Localize ID="locFailoverCluster" runat="server" meta:resourcekey="locFailoverCluster" Text="Failover Cluster"></asp:Localize>
    </legend>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table style="border-collapse: separate; border-spacing: 5px 3px; margin: 10px; width: 100%;">
                <tr>
                    <td colspan="3">
                        <asp:CheckBox ID="chkUseFailoverCluster" runat="server" meta:resourcekey="chkUseFailoverCluster" OnCheckedChanged="chkUseFailoverCluster_CheckedChanged" Text="Use Failover Cluster" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
		                <asp:Localize ID="locClusterName" runat="server" meta:resourcekey="locClusterName" Text="Cluster FQDN Name:"></asp:Localize>
		            </td>
                    <td>
                        <asp:TextBox ID="tbClusterName" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
		                <asp:RequiredFieldValidator ID="ClusterNameValidator" runat="server" ControlToValidate="tbClusterName"
                            Text="*" meta:resourcekey="ClusterNameValidator" Display="Dynamic" SetFocusOnError="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>