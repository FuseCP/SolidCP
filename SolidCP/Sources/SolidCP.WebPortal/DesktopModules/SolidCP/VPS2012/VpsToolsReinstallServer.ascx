<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsToolsReinstallServer.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsToolsReinstallServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    	
	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_tools" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Reinstall" ShowMessageBox="True" ShowSummary="False" />

                    <div id="reinstallForms" runat="server">
                        <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server"
		                    meta:resourcekey="locSubTitle" Text="Re-install VPS Server" />
		                </p>
                        <p>
                            <asp:Localize ID="locDescription" runat="server"
		                        meta:resourcekey="locDescription" Text="This wizard will re-create VPS with the same configuration settings from scratch and then apply current OS template." />
                        </p>

                        <scp:CollapsiblePanel id="secVirtualMachineSetttings" runat="server"
                                TargetControlID="VirtualMachineSettingsPanel" meta:resourcekey="secVirtualMachineSetttings" Text="Virtual Machine Settings">
                            </scp:CollapsiblePanel>
                        <asp:Panel ID="VirtualMachineSettingsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <div class="form-group" id="hostnameSetting" runat="server">
                            <asp:Label ID="locHostname" meta:resourcekey="locHostname" runat="server" Text="Host name:" CssClass="col-sm-2"  AssociatedControlID="txtHostname"></asp:Label>
                                <div class="col-sm-10 form-inline">
                                <asp:TextBox ID="txtHostname" runat="server" CssClass="form-control form-control" Width="40%"></asp:TextBox>
                                                
                                <asp:RequiredFieldValidator ID="HostnameValidator" runat="server" Text="*" Display="Dynamic"
                                    ControlToValidate="txtHostname" meta:resourcekey="HostnameValidator" SetFocusOnError="true"
                                    ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator id="valCorrectHostname" runat="server" Text="*" meta:resourcekey="valCorrectHostname"
                                    ValidationExpression="^[a-zA-Z]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?$"
			                        ControlToValidate="txtHostname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Vps">
			                    </asp:RegularExpressionValidator>
                                                
                                . 
                                <asp:TextBox ID="txtDomain" runat="server" CssClass="form-control form-control" Width="40%"></asp:TextBox>
                                                    
                                <asp:RequiredFieldValidator ID="DomainValidator" runat="server" Text="*" Display="Dynamic"
                                    ControlToValidate="txtDomain" meta:resourcekey="DomainValidator" SetFocusOnError="true"
                                    ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator id="valNewDomainFormat" runat="server" Text="*" meta:resourcekey="valNewDomainFormat"
                                    ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,15}$"
			                        ControlToValidate="txtDomain" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Vps">
			                    </asp:RegularExpressionValidator>
                                </div>
                        </div>
				    
                        <div class="form-group">
                            <asp:Label ID="locOperatingSystem" meta:resourcekey="locOperatingSystem" runat="server" Text="Operating system:" CssClass="col-sm-2"  AssociatedControlID="listOperatingSystems"></asp:Label>
                                <div class="col-sm-10 form-inline">
                                    <asp:DropDownList ID="listOperatingSystems" runat="server"
                                        DataValueField="Path" DataTextField="Name">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="OperatingSystemValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="listOperatingSystems" meta:resourcekey="OperatingSystemValidator" SetFocusOnError="true"
                                            ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                </div>
                            <div class="col-sm-10">
                                    <asp:Localize ID="locAdminPassword" runat="server"
                                        meta:resourcekey="locAdminPassword" Text="Administrator password:"></asp:Localize>
                            </div>
                        </div>
                            <scp:PasswordControl id="password" runat="server" ValidationGroup="Vps" AllowGeneratePassword="true">
                            </scp:PasswordControl>
                        </asp:Panel> 
				        <%--<table cellspacing="5"> Too demanding resources!!!				        
				            <tr> 
				                <td colspan="2">
				                    <asp:CheckBox ID="chkPreserveExistingFiles" runat="server" CssClass="NormalBold"
				                        meta:resourcekey="chkPreserveExistingFiles" Text="Save existing VPS hard drive files" />
				                    <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				                    <asp:Localize ID="locPreserveHelp" runat="server"
		                                meta:resourcekey="locPreserveHelp" Text="All files from existing hard drive will be copied to &quot;old&quot; disk folder on hard drive of new VPS." />
				                </td>
				            </tr>
				        </table>--%>

                        <scp:CollapsiblePanel id="secVirtualMachineSummary" runat="server"
                                TargetControlID="VirtualMachineSummaryPanel" meta:resourcekey="secVirtualMachineSummary" Text="Virtual Machine Summary">
                            </scp:CollapsiblePanel>
                            <asp:Panel ID="VirtualMachineSummaryPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                                <table style="border-collapse: separate; border-spacing: 6px 1px;">                                
                                    <tr>
                                        <td><asp:Localize ID="locCpu" runat="server" meta:resourcekey="locCpu" Text="CPU cores:" /></td>
                                        <td><asp:Literal ID="litCpu" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Localize ID="locRam" runat="server" meta:resourcekey="locRam" Text="RAM, MB:" /></td>
                                        <td><asp:Literal ID="litRam" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Localize ID="locHdd" runat="server" meta:resourcekey="locHdd" Text="Hard disk size, GB:" /></td>
                                        <td><asp:Literal ID="litHdd" runat="server"></asp:Literal></td>
                                    </tr>
                                    <asp:Repeater ID="repHdd" runat="server">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Localize ID="locAdditionalHdd" runat="server" meta:resourcekey="locHdd" Text="Hard disk size, GB:" />
                                                </td>
                                                <td>
                                                    <asp:Literal ID="litAdditionalHdd" runat="server" Text='<%# Eval("DiskSize") %>'></asp:Literal>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                        </SeparatorTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td><asp:Localize ID="locHddIOPSmin" runat="server" meta:resourcekey="locHddIOPSmin" Text="HDD minimum IOPS:" /></td>
                                        <td><asp:Literal ID="litHddIOPSmin" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Localize ID="locHddIOPSmax" runat="server" meta:resourcekey="locHddIOPSmax" Text="HDD maximum IOPS:" /></td>
                                        <td><asp:Literal ID="litHddIOPSmax" runat="server"></asp:Literal></td>
                                    </tr>
                                </table>
                            </asp:Panel> 

                        <scp:CollapsiblePanel id="secVirtualMachineNetwork" runat="server"
                                TargetControlID="VirtualMachineNetworkPanel" meta:resourcekey="secVirtualMachineNetwork" Text="Virtual Machine Networks">
                            </scp:CollapsiblePanel>
                            <asp:Panel ID="VirtualMachineNetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                                <table style="border-collapse: separate; border-spacing: 6px;">                                
                                    <tr id="ExternalAddressesRow" runat="server">
                                        <td><asp:Localize ID="locExternalAddressesList" runat="server"
                                                    meta:resourcekey="locExternalAddressesList" Text="External IP addresses list:" /></td>
                                        <td><asp:Literal ID="litExternalAddresses" runat="server"></asp:Literal>
                                            <asp:HiddenField id="hiddenTxtExternalAddressesNumber" runat="server" Value=""/>
                                        </td>
                                    </tr>
                                    <tr id="PrivateAddressesRow" runat="server">
                                        <td><asp:Localize ID="locPrivateAddressesList" runat="server"
                                                    meta:resourcekey="locExternalAddressesList" Text="Private IP addresses list:" /></td>
                                        <td><asp:Literal ID="litPrivateAddresses" runat="server"></asp:Literal>
                                            <asp:HiddenField id="hiddenTxtPrivateAddressesNumber" runat="server" Value=""/>
                                        </td>                                    
                                    </tr>
                                </table>
                            </asp:Panel>                        

				        <br />
                        <%--<table cellspacing="5" id="AdminOptionsPanel" runat="server">
				            <tr>
				                <td>
				                    <asp:CheckBox ID="chkSaveVhd" runat="server"
				                        meta:resourcekey="chkSaveVhd" Text="Do not delete VPS virtual hard drive file" />
				                </td>
				            </tr>
				            <tr>
				                <td>
				                    <asp:CheckBox ID="chkExport" runat="server"
				                        meta:resourcekey="chkExport" Text="Export VPS before re-installation to the following folder:" />
				                </td>
				            </tr>
				            <tr>
				                <td style="padding-left:20px;">
				                    <asp:TextBox ID="txtExportPath" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
				                
				                    <asp:RequiredFieldValidator ID="ExportPathValidator" runat="server" Text="*" Display="Dynamic"
                                            ControlToValidate="txtExportPath" meta:resourcekey="ExportPathValidator" SetFocusOnError="true"
                                            ValidationGroup="Reinstall">*</asp:RequiredFieldValidator>
				                </td>
				            </tr>
				        </table>--%>
				        <p>
                            <asp:CheckBox ID="chkConfirmReinstall" runat="server" CssClass="NormalBold"
				                        meta:resourcekey="chkConfirmReinstall" Text="Yes, I confirm re-installation of this VPS" />
                        </p>
                    </div>		            
                    <div class="text-right">
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnReinstall" CssClass="btn btn-success" runat="server" onclick="btnUpdate_Click" ValidationGroup="Reinstall"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnReinstall"/> </CPCC:StyleButton>
                    </div>
			    </div>
		    </div>
	    </div>
