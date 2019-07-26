<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsEditConfiguration.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsEditConfiguration" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="DynamicMemoryControl" Src="UserControls/DynamicMemoryControl.ascx" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_config" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
		            <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server" meta:resourcekey="locSubTitle" Text="Edit Configuration" />
		            </p>

                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Vps" ShowMessageBox="True" ShowSummary="False" />
                    
                    <scp:CollapsiblePanel id="secResources" runat="server"
                        TargetControlID="ResourcesPanel" meta:resourcekey="secResources" Text="Resources">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="ResourcesPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <div class="form-group">

                            <asp:Label ID="lblCpu" runat="server" AssociatedControlID="ddlCpu"
                                        meta:resourcekey="lblCpu" Text="CPU:" CssClass="col-sm-1" />
                            <div class="col-sm-11 form-inline">
                                <asp:DropDownList ID="ddlCpu" runat="server" CssClass="form-control" Width="70">
                                    </asp:DropDownList>
                                <asp:Localize ID="locCores" runat="server" meta:resourcekey="locCores" Text="cores"/>
                            </div>

                            <asp:Label ID="lblRam" runat="server" AssociatedControlID="txtRam"
                                        meta:resourcekey="lblRam" Text="RAM:" CssClass="col-sm-1" />
                            <div class="col-sm-11 form-inline">
                                <asp:TextBox ID="txtRam" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireRamValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtRam" meta:resourcekey="RequireRamValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:Localize ID="locMB" runat="server" meta:resourcekey="locMB" Text="MB"/>
                            </div>

                            <asp:Label ID="lblHdd" runat="server" AssociatedControlID="txtHdd"
                                        meta:resourcekey="lblHdd" Text="HDD:" CssClass="col-sm-1" />
                            <div class="col-sm-11 form-inline">
                                <asp:TextBox ID="txtHdd" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireHddValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtHdd" meta:resourcekey="RequireHddValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>                                    
                                <asp:Localize ID="locGB" runat="server" meta:resourcekey="locGB" Text="GB"/>
                                <asp:HiddenField id="hiddenTxtValHdd" runat="server" />
                            </div>
                            <div class="col-sm-11">
                            <asp:CheckBox ID="chkIgnoreHddWarning" runat="server" 
                                Text="Ignore reduce size warning (You'll reduce only quota number!)" meta:resourcekey="chkIgnoreHddWarning" />                                
                            </div> 
                        </div>                           
                    </asp:Panel>

                    <scp:CollapsiblePanel id="secHddQOS" runat="server" IsCollapsed="true"
                        TargetControlID="QOSManag" meta:resourcekey="secHddQOS" Text="Virtual Hard Disk Drive Quality of Service management">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="QOSManag" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <p>
		                <asp:Localize ID="locHddIOPSTitle" runat="server" meta:resourcekey="locHddIOPSTitle" 
                            Text="Specify Quality of Service management for this virtual hard disk. Minimum and maximum IOPS are measured in 8KB increments. Default value is 0." />
		                </p>
                        <div class="form-group">

                            <asp:Label ID="lblHddMinIOPS" runat="server" AssociatedControlID="txtHddMinIOPS"
                                        meta:resourcekey="lblHddMinIOPS" Text="Minimum:" CssClass="col-sm-1" />
                            <div class="col-sm-11 form-inline">
                                <asp:TextBox ID="txtHddMinIOPS" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireHddMinIOPSValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtHddMinIOPS" meta:resourcekey="RequireHddMinIOPSValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:Localize ID="locHddMinIOPS" runat="server" meta:resourcekey="locHddMinIOPS" Text="IOPS"/>
                            </div>

                            <asp:Label ID="lblHddMaxIOPS" runat="server" AssociatedControlID="txtHddMaxIOPS"
                                        meta:resourcekey="lblHddMaxIOPS" Text="Maximum:" CssClass="col-sm-1" />
                            <div class="col-sm-11 form-inline">
                                <asp:TextBox ID="txtHddMaxIOPS" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequireHddMaxIOPSValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtHddMaxIOPS" meta:resourcekey="RequireHddMaxIOPSValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:Localize ID="locHddMaxIOPS" runat="server" meta:resourcekey="locHddMaxIOPS" Text="IOPS"/>
                            </div>
                        </div>
                    </asp:Panel>

                    
                    <scp:DynamicMemoryControl runat="server" ID="DynamicMemorySetting" Mode="Edit"/>
                    
                    <scp:CollapsiblePanel id="secSnapshots" runat="server"
                        TargetControlID="SnapshotsPanel" meta:resourcekey="secSnapshots" Text="Snapshots">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="SnapshotsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <div class="form-group">

                            <asp:Label ID="locSnapshots" runat="server"
                                    meta:resourcekey="locSnapshots" Text="Number of snapshots:" CssClass="col-sm-1"></asp:Label>
                            <div class="col-sm-11 form-inline">
                                <asp:TextBox ID="txtSnapshots" runat="server" CssClass="form-control" Width="50"></asp:TextBox>                                    
                                    <asp:RequiredFieldValidator ID="SnapshotsValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtSnapshots" meta:resourcekey="SnapshotsValidator" SetFocusOnError="true"
                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secDvd" runat="server"
                        TargetControlID="DvdPanel" meta:resourcekey="secDvd" Text="DVD">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="DvdPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <div class="form-group">
                            <div class="col-sm-12 form-inline">
                                <asp:CheckBox ID="chkDvdInstalled" runat="server" Checked="true"
                                        Text="DVD drive installed" meta:resourcekey="chkDvdInstalled" />
                            </div>
                        </div>
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secBios" runat="server"
                        TargetControlID="BiosPanel" meta:resourcekey="secBios" Text="BIOS">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="BiosPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;width:700px;">
                        <div class="form-group">
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkBootFromCd" runat="server" Text="Boot from CD" meta:resourcekey="chkBootFromCd" />
                            </div>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkNumLock" runat="server" Text="Num Lock enabled" meta:resourcekey="chkNumLock" />
                            </div>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkSecureBoot" runat="server" Text="Secure Boot enabled" meta:resourcekey="chkSecureBoot" />
                            </div>
                        </div>                        
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secActions" runat="server"
                        TargetControlID="ActionsPanel" meta:resourcekey="secActions" Text="Allowed actions">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="ActionsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;width:700px;">
                        <div class="form-group">
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkStartShutdown" runat="server" Text="Start, Turn off and Shutdown" meta:resourcekey="chkStartShutdown" />
                            </div>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkReset" runat="server" Text="Reset" meta:resourcekey="chkReset" />
                            </div>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkPauseResume" runat="server" Text="Pause, Resume" meta:resourcekey="chkPauseResume" />
                            </div>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkReinstall" runat="server" Text="Re-install" meta:resourcekey="chkReinstall" />
                            </div>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkReboot" runat="server" Text="Reboot" meta:resourcekey="chkReboot" />
                            </div>
                        </div>
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secNetwork" runat="server"
                        TargetControlID="NetworkPanel" meta:resourcekey="secNetwork" Text="Network">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="NetworkPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                        <div class="form-group">
                            <div class="col-sm-12 form-inline">
                                <asp:CheckBox ID="chkExternalNetworkEnabled" runat="server"
                                             meta:resourcekey="chkExternalNetworkEnabled" Text="External network enabled" />
                            </div>
                            <div class="col-sm-12 form-inline">
                                <asp:CheckBox ID="chkPrivateNetworkEnabled" runat="server"
                                                    meta:resourcekey="chkPrivateNetworkEnabled" Text="Private network enabled" />
                            </div>
                        </div>
                    </asp:Panel>
                    
                    <p>
                        <asp:CheckBox ID="chkForceReboot" runat="server" CssClass="NormalBold"
				                    meta:resourcekey="chkForceReboot" Text="Force reboot the server (if you have a problem)" />
                    </p>

                    <div class="text-right">
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" ValidationGroup="Vps" OnClientClick="if(!confirm('Before applying new configuration VPS could be stopped.\n\nAfter the configuration is changed it will be started again automatically.\n\nDo you want to proceed?')) return false; ShowProgressDialog('Updating configuration...');"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/> </CPCC:StyleButton>
                    </div>
			    </div>
		    </div>
	    </div>