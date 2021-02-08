<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcImportServer.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcImportServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="Generation" Src="UserControls/Generation.ascx" %>
<%@ Register TagPrefix="scp" TagName="DynamicMemoryControl" Src="UserControls/DynamicMemoryControl.ascx" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
    			    	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="ImportWizard" ShowMessageBox="True" ShowSummary="False" />
                    <div class="form-group">

                        <asp:Label ID="locHyperVService" runat="server" CssClass="col-sm-1" AssociatedControlID="HyperVServices" meta:resourcekey="locHyperVService" Text="Hyper-V Service:"></asp:Label>
                        <div class="col-sm-11">
                            <asp:DropDownList ID="HyperVServices" runat="server" CssClass="form-control"
                                    DataValueField="ServiceID" DataTextField="FullServiceName" AutoPostBack="true"
                                    onselectedindexchanged="HyperVServices_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequireHyperVService" runat="server"
                                    ControlToValidate="HyperVServices" ValidationGroup="ImportWizard" meta:resourcekey="RequireHyperVService"
                                    Display="Dynamic" SetFocusOnError="true" Text="*">
                                </asp:RequiredFieldValidator>
                        </div>

                        <div id="VMsRow" runat="server">

                            <asp:Label ID="locVirtualMachine" runat="server" CssClass="col-sm-1" meta:resourcekey="locVirtualMachine" Text="Virtual machine:"></asp:Label>
                            <div class="col-sm-11">
                                <asp:DropDownList ID="VirtualMachines" runat="server" CssClass="form-control"
                                    DataValueField="VirtualMachineId" DataTextField="Name" AutoPostBack="true"
                                    onselectedindexchanged="VirtualMachines_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredVirtualMachine" runat="server"
                                    ControlToValidate="VirtualMachines" ValidationGroup="ImportWizard" meta:resourcekey="RequiredVirtualMachine"
                                    Display="Dynamic" SetFocusOnError="true" Text="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>                        
                    </div>
                    
                    <scp:CollapsiblePanel id="secOsTemplate" runat="server"
                        TargetControlID="OsTemplatePanel" meta:resourcekey="secOsTemplate" Text="OS Template">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="OsTemplatePanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <div class="form-group">
                            <asp:Label ID="locOsTemplate" runat="server" CssClass="col-sm-1" meta:resourcekey="locOsTemplate" AssociatedControlID="OsTemplates" Text="OS Template:"></asp:Label>
                            <div class="col-sm-11 form-inline">
                                <asp:DropDownList ID="OsTemplates" runat="server" CssClass="form-control"
                                        DataValueField="Path" DataTextField="Name"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredOsTemplate" runat="server"
                                        ControlToValidate="OsTemplates" ValidationGroup="ImportWizard" meta:resourcekey="RequiredOsTemplate"
                                        Display="Dynamic" SetFocusOnError="true" Text="*">
                                    </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-12">
                                <asp:CheckBox ID="EnableRemoteDesktop" runat="server" AutoPostBack="true"
                                        meta:resourcekey="EnableRemoteDesktop" Text="Enable Remote Desktop Web Connection" />
                            </div>
                            <div id="AdminPasswordPanel" runat="server" visible="false">
                                <asp:Label ID="locAdminPassword" runat="server" CssClass="col-sm-2" meta:resourcekey="locAdminPassword" AssociatedControlID="adminPassword" Text="Administrator password:"></asp:Label>
                                <div class="col-sm-10 form-inline">
                                    <asp:TextBox ID="adminPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredAdminPassword" runat="server"
                                        ControlToValidate="adminPassword" ValidationGroup="ImportWizard" meta:resourcekey="RequiredAdminPassword"
                                        Display="Dynamic" SetFocusOnError="true" Text="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    
                    <asp:Panel ID="VirtualMachinePanel" runat="server">
                        <scp:CollapsiblePanel id="secConfiguration" runat="server"
                            TargetControlID="ConfigurationPanel" meta:resourcekey="secConfiguration" Text="Configuration">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="ConfigurationPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <scp:Generation runat="server" ID="GenerationSetting" Mode="Summary"/>

                            <div class="form-group">
                                <asp:Label ID="locCPU" runat="server" CssClass="col-sm-1" meta:resourcekey="locCPU" Text="CPU:"></asp:Label>
                                <div class="col-sm-11">
                                    <asp:Literal ID="CpuCores" runat="server" Text="0"></asp:Literal>
                                </div>

                                <asp:Label ID="locRAM" runat="server" CssClass="col-sm-1" meta:resourcekey="locRAM" Text="RAM:"></asp:Label>
                                <div class="col-sm-11">
                                    <asp:Literal ID="RamSize" runat="server" Text="0"></asp:Literal>
                                </div>

                                <asp:Label ID="locHDD" runat="server" CssClass="col-sm-1" meta:resourcekey="locHDD" Text="HDD:"></asp:Label>
                                <div class="col-sm-11">
                                    <asp:Literal ID="HddSize" runat="server" Text="0"></asp:Literal>
                                </div>

                                <asp:Label ID="locVhdPath" runat="server" CssClass="col-sm-1" meta:resourcekey="locVhdPath" Text="VHD location:"></asp:Label>
                                <div class="col-sm-11">
                                    <asp:Literal ID="VhdPath" runat="server" Text="0"></asp:Literal>
                                </div>

                                <asp:Repeater ID="repHdd" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="locAdditionalHDD" runat="server" CssClass="col-sm-1" meta:resourcekey="locHDD" Text="HDD:"></asp:Label>
                                        <div class="col-sm-11">
                                            <asp:Literal ID="AdditionalHddSize" runat="server" Text='<%# Eval("DiskSize") %>'></asp:Literal>
                                        </div>

                                        <asp:Label ID="locAdditionalVhdPath" runat="server" CssClass="col-sm-1" meta:resourcekey="locVhdPath" Text="VHD location:"></asp:Label>
                                        <div class="col-sm-11">
                                            <asp:Literal ID="AdditionalVhdPath" runat="server" Text='<%# Eval("DiskPath") %>'></asp:Literal>
                                        </div>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                    </SeparatorTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:Panel>
                        
                        <scp:DynamicMemoryControl runat="server" ID="DynamicMemorySetting" Mode="Display"/>

                        <scp:CollapsiblePanel id="secBios" runat="server"
                            TargetControlID="BiosPanel" meta:resourcekey="secBios" Text="BIOS">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="BiosPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;width:450px;">                            
                            <div class="form-group">
                                <div id="divBootFromCdChkOption" runat="server" class="col-sm-6">
                                    <scp:CheckBoxOption id="BootFromCd" runat="server" Value="False" />
                                    <asp:Localize ID="locBootFromCd" runat="server" meta:resourcekey="locBootFromCd"></asp:Localize>
                                </div>
                                <div id="divNumLockChkOption" runat="server" class="col-sm-6">
                                    <scp:CheckBoxOption id="NumLockEnabled" runat="server" Value="False" />
                                    <asp:Localize ID="locNumLockEnabled" runat="server" meta:resourcekey="locNumLockEnabled"></asp:Localize>
                                </div>

                                <div id="divBootFromCdChkBox" runat="server" class="col-sm-6">
                                    <asp:CheckBox ID="chkBootFromCd" runat="server" Text="Boot from CD" meta:resourcekey="chkBootFromCd" />
                                </div>
                                <div id="divNumLockChkBox" runat="server" class="col-sm-6">
                                    <asp:CheckBox ID="chkNumLock" runat="server" Text="Num Lock enabled" meta:resourcekey="chkNumLock" />
                                </div>
                            </div>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secDvd" runat="server"
                            TargetControlID="DvdPanel" meta:resourcekey="secDvd" Text="DVD">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="DvdPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <div class="form-group">
                                <div id="divDvdInstalledChkOption" runat="server" class="col-sm-12 form-inline">
                                    <scp:CheckBoxOption id="DvdInstalled" runat="server" Value="False" />
                                        <asp:Localize ID="locDvdInstalled" runat="server" meta:resourcekey="locDvdInstalled"></asp:Localize>
                                </div>
                                <div id="divDvdInstalledChkBox" runat="server" class="col-sm-12 form-inline">
                                    <asp:CheckBox ID="chkDvdInstalled" runat="server" Checked="False"
                                        Text="DVD drive installed" meta:resourcekey="chkDvdInstalled" />
                                </div>
                            </div>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secAllowedActions" runat="server"
                            TargetControlID="AllowedActionsPanel" meta:resourcekey="secAllowedActions" Text="Allowed Actions">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="AllowedActionsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;width:450px;">
                            <div class="form-group">
                                <div class="col-sm-6">
                                    <asp:CheckBox ID="AllowStartShutdown" runat="server" meta:resourcekey="AllowStartShutdown" />
                                </div>
                                <div class="col-sm-6">
                                    <asp:CheckBox ID="AllowReboot" runat="server" meta:resourcekey="AllowReboot" />
                                </div>
                                <div class="col-sm-6">
                                    <asp:CheckBox ID="AllowPause" runat="server" meta:resourcekey="AllowPause" />
                                </div>
                                <div class="col-sm-6">
                                    <asp:CheckBox ID="AllowReset" runat="server" meta:resourcekey="AllowReset" />
                                </div>
                                <div class="col-sm-12">
                                    <asp:CheckBox ID="AllowReinstall" runat="server" Text="Re-install" meta:resourcekey="AllowReinstall" />
                                </div>
                            </div>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secExternalNetwork" runat="server"
                            TargetControlID="ExternalNetworkPanel" meta:resourcekey="secExternalNetwork" Text="External Network">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="ExternalNetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">

                            <div class="form-group">

                                <asp:Label ID="locExternalAdapter" runat="server" CssClass="col-sm-1" 
                                    meta:resourcekey="locExternalAdapter" AssociatedControlID="ExternalAdapters" Text="Connected NIC:"></asp:Label>
                                <div class="col-sm-11">
                                    <asp:DropDownList ID="ExternalAdapters" runat="server" CssClass="form-control"
                                            DataValueField="MacAddress" DataTextField="Name" AutoPostBack="true"
                                            onselectedindexchanged="ExternalAdapters_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div id="ExternalAddressesRow" runat="server">

                                    <asp:Label ID="locExternalAddresses" runat="server" CssClass="col-sm-1" meta:resourcekey="locExternalAddresses" Text="Assign IP addresses:"></asp:Label>
                                    <div class="col-sm-11">
                                        <asp:ListBox ID="ExternalAddresses" runat="server" Rows="5"
                                            Width="220" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="RequiredExternalAddresses" runat="server"
                                            ControlToValidate="ExternalAddresses" ValidationGroup="ImportWizard" meta:resourcekey="RequiredExternalAddresses"
                                            Display="Dynamic" SetFocusOnError="true" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        
                        <scp:CollapsiblePanel id="secManagementNetwork" runat="server"
                            TargetControlID="ManagementNetworkPanel" meta:resourcekey="secManagementNetwork" Text="Management Network">
                        </scp:CollapsiblePanel>
                        <asp:Panel ID="ManagementNetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                            <div class="form-group">

                                <asp:Label ID="locManagementAdapter" runat="server" CssClass="col-sm-1" 
                                    meta:resourcekey="locManagementAdapter" AssociatedControlID="ManagementAdapters" Text="Connected NIC:"></asp:Label>
                                <div class="col-sm-11">
                                    <asp:DropDownList ID="ManagementAdapters" runat="server" CssClass="form-control"
                                            DataValueField="MacAddress" DataTextField="Name" AutoPostBack="true"
                                            onselectedindexchanged="ManagementAdapters_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div id="ManagementAddressesRow" runat="server">

                                    <asp:Label ID="locManagementAddresses" runat="server" CssClass="col-sm-1" meta:resourcekey="locManagementAddresses" Text="Assign IP addresses:"></asp:Label>
                                    <div class="col-sm-11">
                                        <asp:ListBox ID="ManagementAddresses" runat="server" Rows="5"
                                            Width="220" SelectionMode="Single"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="RequiredManagementAddresses" runat="server"
                                            ControlToValidate="ManagementAddresses" ValidationGroup="ImportWizard" meta:resourcekey="RequiredManagementAddresses"
                                            Display="Dynamic" SetFocusOnError="true" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>

                    <p>
                        <asp:CheckBox ID="chkIgnoreCheckes" runat="server" CssClass="NormalBold"
				                    meta:resourcekey="chkIgnoreCheckes" Text="Ignore Quotas checkes" />
                    </p>
                    
                    <div class="text-right">
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnImport" CssClass="btn btn-success" runat="server" OnClick="btnImport_Click" ValidationGroup="ImportWizard"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnImportText"/> </CPCC:StyleButton>
                    </div>
                        
			    </div>
		    </div>
	    </div>
    	
