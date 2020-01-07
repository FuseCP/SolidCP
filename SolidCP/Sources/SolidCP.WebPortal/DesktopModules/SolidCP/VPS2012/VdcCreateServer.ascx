<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcCreateServer.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcCreate" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%--<%@ Register TagPrefix="scp" TagName="Generation" Src="UserControls/Generation.ascx" %>--%>
<%@ Register TagPrefix="scp" TagName="DynamicMemoryControl" Src="UserControls/DynamicMemoryControl.ascx" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
    			    	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Vps" ShowMessageBox="True" ShowSummary="False" />
                    
                    <asp:Wizard ID="wizard" runat="server"
                        meta:resourcekey="wizard" CellSpacing="5" CssClass="wizardvps"
                        onfinishbuttonclick="wizard_FinishButtonClick" 
                        onsidebarbuttonclick="wizard_SideBarButtonClick" 
                        onactivestepchanged="wizard_ActiveStepChanged" 
                        onnextbuttonclick="wizard_NextButtonClick">
                        
                        <SideBarStyle CssClass="SideBar" VerticalAlign="Top" />
                        <StepStyle VerticalAlign="Top" />
                        
                        <StartNavigationTemplate>
                            <CPCC:StyleButton id="btnNext" CssClass="btn btn-primary" runat="server" CommandName="MoveNext" ValidationGroup="Vps"> <i class="fa fa-arrow-right">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnNextText"/> </CPCC:StyleButton>
                        </StartNavigationTemplate>
                        
                        <StepNavigationTemplate>
                            <CPCC:StyleButton id="btnPrevious" CssClass="btn btn-primary" runat="server" CommandName="MovePrevious" ValidationGroup="Vps"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnPreviousText"/> </CPCC:StyleButton>&nbsp;
                            <CPCC:StyleButton id="btnNext" CssClass="btn btn-primary" runat="server" CommandName="MoveNext" ValidationGroup="Vps"> <i class="fa fa-arrow-right">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnNextText"/> </CPCC:StyleButton>

                        </StepNavigationTemplate>
                        
                        <FinishNavigationTemplate>
                            <CPCC:StyleButton id="btnPrevious" CssClass="btn btn-primary" runat="server" CommandName="MovePrevious" ValidationGroup="Vps"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnPreviousText"/> </CPCC:StyleButton>&nbsp;        
                            <CPCC:StyleButton id="btnFinish" CssClass="btn btn-success" runat="server"  CommandName="MoveComplete" ValidationGroup="Vps"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnFinishText"/> </CPCC:StyleButton>
                        </FinishNavigationTemplate>
                       
                        
                        <WizardSteps>
                            <asp:WizardStep ID="stepName" runat="server" meta:resourcekey="stepName" Title="Name and OS">
                                    <div class="form-group">
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

                                        <div class="col-sm-10">
                                            <asp:CheckBox ID="chkAutoHostName" runat="server" AutoPostBack="true" Checked="false"
                                                Text="Generate hostname automatically" meta:resourcekey="chkAutoHostName" />
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
                                            <asp:Localize ID="locAdminPassword" runat="server"
                                                meta:resourcekey="locAdminPassword" Text="Administrator password:"></asp:Localize><br />
                                    </div>
                                                <scp:PasswordControl id="password" runat="server" ValidationGroup="Vps" AllowGeneratePassword="true">
                                                </scp:PasswordControl>
                                        <div class="form-group">
                                            <div class="col-sm-12">
                                                <asp:CheckBox ID="chkSendSummary" runat="server" AutoPostBack="true" Checked="true"
                                                    meta:resourcekey="chkSendSummary" Text="Send summary letter to:" /><br />
                                                <asp:TextBox ID="txtSummaryEmail" runat="server" CssClass="form-control form-control" AutoPostBack="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="SummaryEmailValidator" runat="server" Text="*" Display="Dynamic"
                                                    ControlToValidate="txtSummaryEmail" meta:resourcekey="SummaryEmailValidator" SetFocusOnError="true"
                                                    ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                            </div>
                                       </div>    
                            </asp:WizardStep>
                            <asp:WizardStep ID="stepConfig" runat="server" meta:resourcekey="stepConfig" Title="Configuration">
                                    <%--<scp:Generation runat="server" ID="GenerationSetting" Mode="Edit"/>--%>

                                    <scp:CollapsiblePanel id="secResources" runat="server"
                                        TargetControlID="ResourcesPanel" meta:resourcekey="secResources" Text="Resources">
                                    </scp:CollapsiblePanel>
                                    <asp:Panel ID="ResourcesPanel" runat="server" Height="0" style="overflow: hidden; padding: 5px;">
                                        <div class="form-group">
                                            <asp:Label ID="lblCpu" meta:resourcekey="lblCpu" runat="server" Text="CPU:" CssClass="col-sm-2"  AssociatedControlID="ddlCpu"></asp:Label>
                                            <div class="col-sm-10 form-inline">
                                                    <asp:DropDownList ID="ddlCpu" runat="server" CssClass="form-control" Width="70">
                                                    </asp:DropDownList>
                                             <asp:Localize ID="locCores" runat="server" meta:resourcekey="locCores" Text="cores"/>
                                            </div>
                                         <asp:Label ID="lblRam" meta:resourcekey="lblRam" runat="server" Text="RAM:" CssClass="col-sm-2"  AssociatedControlID="txtRam"></asp:Label>
                                            <div class="col-sm-10 form-inline">
                                                    <asp:TextBox ID="txtRam" runat="server" CssClass="form-control form-control" Width="150" Text="0"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequireRamValidator" runat="server" Text="*" Display="Dynamic"
                                                        ControlToValidate="txtRam" meta:resourcekey="RequireRamValidator" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator id="valRamSize" runat="server" Text="*" meta:resourcekey="valRamSize"
														ValidationExpression="^\d*[02468]$"
														ControlToValidate="txtRam" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Vps">
													</asp:RegularExpressionValidator>
                                                    <asp:Localize ID="locMB" runat="server" meta:resourcekey="locMB" Text="MB"/>
                                            </div>
                                         <asp:Label ID="lblHdd" meta:resourcekey="lblHdd" runat="server" Text="HDD:" CssClass="col-sm-2"  AssociatedControlID="txtHdd"></asp:Label>
                                                    <div class="col-sm-10 form-inline">
                                                    <asp:TextBox ID="txtHdd" runat="server" CssClass="form-control form-control" Width="150" Text="0"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequireHddValidator" runat="server" Text="*" Display="Dynamic"
                                                        ControlToValidate="txtHdd" meta:resourcekey="RequireHddValidator" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator id="valHddSize" runat="server" Text="*" meta:resourcekey="valHddSize"
														ValidationExpression="^[0-9]+$"
														ControlToValidate="txtHdd" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Vps">
													</asp:RegularExpressionValidator>
                                                    <asp:Localize ID="locGB" runat="server" meta:resourcekey="locGB" Text="GB"/>
                                                    </div>
                                            </div>
                                    </asp:Panel>

                                    <scp:CollapsiblePanel id="secHddQOS" runat="server" IsCollapsed="true"
                                        TargetControlID="QOSManag" meta:resourcekey="secHddQOS" Text="Virtual Hard Disk Drive Quality of Service management">
                                    </scp:CollapsiblePanel>
                                    <asp:Panel ID="QOSManag" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                                       <div class="form-group">
                                            <div class="col-sm-10">
                                        <asp:Localize ID="locHddIOPSTitle" runat="server" meta:resourcekey="locHddIOPSTitle" 
                            Text="Specify Quality of Service management for this virtual hard disk. Minimum and maximum IOPS are measured in 8KB increments. Default value is 0." />
		                                    </div>
                                            <div class="col-sm-10 form-inline"> 
                                            <asp:Label ID="lblHddMinIOPS" meta:resourcekey="lblHddMinIOPS" runat="server" Text="Minimum:" CssClass="col-sm-2" AssociatedControlID="txtHddMinIOPS"/>
                                                                                           
                                                <asp:TextBox ID="txtHddMinIOPS" runat="server" CssClass="form-control form-control" Width="150" Text="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequireHddMinIOPSValidator" runat="server" Text="*" Display="Dynamic"
                                                     ControlToValidate="txtHddMinIOPS" meta:resourcekey="RequireHddMinIOPSValidator" SetFocusOnError="true"
                                                     ValidationGroup="Vps">*</asp:RequiredFieldValidator>    
                                                <asp:Localize ID="locHddMinIOPS" runat="server" meta:resourcekey="locHddMinIOPS" Text="IOPS"/>                         
                                            </div>

                                            <div class="col-sm-10 form-inline">  
                                            <asp:Label ID="lblHddMaxIOPS" meta:resourcekey="lblHddMaxIOPS" runat="server" Text="Maximum:" CssClass="col-sm-2" AssociatedControlID="txtHddMaxIOPS"/>
                                                                            
                                                <asp:TextBox ID="txtHddMaxIOPS" runat="server" CssClass="form-control form-control" Width="150" Text="0"></asp:TextBox>
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
                                        <asp:Label ID="locSnapshots" meta:resourcekey="locSnapshots" runat="server" Text="Number of snapshots:" CssClass="col-sm-2"  AssociatedControlID="txtSnapshots"></asp:Label>
                                                <div class="col-sm-10 form-inline">
                                                    <asp:TextBox ID="txtSnapshots" runat="server" CssClass="form-control form-control" Width="150" Text="0"></asp:TextBox>
                                                    
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
                                             <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkDvdInstalled" runat="server"
                                                        Text="DVD drive installed" meta:resourcekey="chkDvdInstalled" />
                                                </div>
                                            </div>
                                    </asp:Panel>
                                    
                                    <scp:CollapsiblePanel id="secBios" runat="server"
                                        TargetControlID="BiosPanel" meta:resourcekey="secBios" Text="BIOS">
                                    </scp:CollapsiblePanel>
                                    <asp:Panel ID="BiosPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
                                        <div class="form-group">
                                            <div class="col-sm-6">
                                                    <asp:CheckBox ID="chkBootFromCd" runat="server" Text="Boot from CD" meta:resourcekey="chkBootFromCd" />
                                            </div>
                                            <div class="col-sm-6">
                                                    <asp:CheckBox ID="chkNumLock" runat="server" Text="Num Lock enabled" meta:resourcekey="chkNumLock" />
                                               </div>
                                            </div>
                                    </asp:Panel>
                                    
                                    <scp:CollapsiblePanel id="secActions" runat="server"
                                        TargetControlID="ActionsPanel" meta:resourcekey="secActions" Text="Allowed actions">
                                    </scp:CollapsiblePanel>
                                    <asp:Panel ID="ActionsPanel" runat="server" Height="0" style="overflow:hidden;padding:5px;">
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
                                    <br />
                            </asp:WizardStep>

                            <asp:WizardStep ID="stepExternalNetwork" runat="server" meta:resourcekey="stepExternalNetwork" Title="External network">
                                    <p>
                                        <asp:CheckBox ID="chkExternalNetworkEnabled" runat="server" AutoPostBack="true" Checked="true"
                                                    meta:resourcekey="chkExternalNetworkEnabled" Text="External network enabled" />
                                        <asp:DropDownList ID="listVlanLists" runat="server"
                                                    DataValueField="Path" DataTextField="Name" AutoPostBack="true" onselectedindexchanged="VlanLists_SelectedIndexChanged">
                                                </asp:DropDownList>
                                    </p>
                                    
                                     <div runat="server" ID="EmptyExternalAddressesMessage" style="padding: 5px;" visible="false">
                                        <asp:Localize ID="locNotEnoughExternalAddresses" runat="server" Text="Not enough..."
                                                meta:resourcekey="locNotEnoughExternalAddresses"></asp:Localize>
                                     </div>
                                    
                                    <table id="tableExternalNetwork" runat="server" cellspacing="5" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radioExternalRandom" runat="server" AutoPostBack="true"
                                                    meta:resourcekey="radioExternalRandom" Text="Randomly select IP addresses from the pool" 
                                                    Checked="True" GroupName="ExternalAddress" />
                                            </td>
                                        </tr>
                                        <tr id="ExternalAddressesNumberRow" runat="server">
                                            <td style="padding-left: 30px;">
                                                <asp:Localize ID="locExternalAddresses" runat="server"
                                                        meta:resourcekey="locExternalAddresses" Text="Number of IP addresses:"></asp:Localize>

                                                <asp:TextBox ID="txtExternalAddressesNumber" runat="server" CssClass="form-control form-control" Width="150" Text=""></asp:TextBox>
                                                
                                                <asp:RequiredFieldValidator ID="ExternalAddressesValidator" runat="server" Text="*" Display="Dynamic"
                                                        ControlToValidate="txtExternalAddressesNumber" meta:resourcekey="ExternalAddressesValidator" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                                        
                                                <asp:Literal ID="litMaxExternalAddresses" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radioExternalSelected" runat="server" AutoPostBack="true"
                                                    meta:resourcekey="radioExternalSelected" Text="Select IP addresses from the list" 
                                                    GroupName="ExternalAddress" />
                                            </td>
                                        </tr>
                                        <tr id="ExternalAddressesListRow" runat="server">
                                            <td style="padding-left: 30px;">
                                                <asp:ListBox ID="listExternalAddresses" runat="server" Rows="8"
                                                    CssClass="form-control form-control" Width="300" SelectionMode="Multiple" Height="80"></asp:ListBox>
                                                <br />
                                                <asp:Localize ID="locHoldCtrl" runat="server"
                                                        meta:resourcekey="locHoldCtrl" Text="* Hold CTRL key to select multiple addresses"></asp:Localize>
                                            </td>
                                        </tr>
                                        <tr id="ExternalMACAddressRow" runat="server">
	                                        <td style="padding-left: 30px;">
                                                <div class="col-sm-10 form-inline">
		                                            <asp:Localize ID="locExternalMACAddress" runat="server"
				                                            meta:resourcekey="locExternalMACAddress" Text="MAC:"></asp:Localize>

		                                            <asp:TextBox ID="txtExternalMACAddress" runat="server" CssClass="form-control form-control" Width="145" Text=""></asp:TextBox>
		
		                                            <asp:RequiredFieldValidator id="MACAddressValidator" ControlToValidate="txtExternalMACAddress" 
		                                            ErrorMessage="MAC Address is incorrect." Display="Dynamic" Width="100%" runat="server" 
                                                        meta:resourcekey="MACAddressValidator" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
		                                            <asp:RegularExpressionValidator ID="valMACformat" runat="server" ControlToValidate="txtExternalMACAddress" 
		                                            ErrorMessage="MAC Address format is incorrect." Display="Dynamic" Width="100%" ValidationExpression="((\d|([a-f]|[A-F])){2}[:\-]?){5}(\d|([a-f]|[A-F])){2}" 
                                                        meta:resourcekey="valMACformat" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RegularExpressionValidator>
                                                </div>
	                                        </td>
                                        </tr>
                                    </table>
                                    <br />
                            </asp:WizardStep>
                            
                            
                            
                            <asp:WizardStep ID="stepPrivateNetwork" runat="server" meta:resourcekey="stepPrivateNetwork" Title="Private network">
                                    <p>
                                        <asp:CheckBox ID="chkPrivateNetworkEnabled" runat="server" AutoPostBack="true" Checked="true"
                                                    meta:resourcekey="chkPrivateNetworkEnabled" Text="Private network enabled" />
                                        <asp:DropDownList ID="listPrivateNetworkVLAN" runat="server">
                                        </asp:DropDownList>
                                    </p>
                                    
                                   
                                    <table id="tablePrivateNetwork" runat="server" cellspacing="5" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radioPrivateRandom" runat="server" AutoPostBack="true"
                                                    meta:resourcekey="radioPrivateRandom" Text="Randomly select next available IP addresses to the addresses format" 
                                                    Checked="True" GroupName="PrivateAddress" />
                                            </td>
                                        </tr>
                                        <tr id="PrivateAddressesNumberRow" runat="server">
                                            <td style="padding-left: 30px;">
                                                <asp:Localize ID="locPrivateAddresses" runat="server"
                                                        meta:resourcekey="locPrivateAddresses" Text="Number of IP addresses:"></asp:Localize>

                                                <asp:TextBox ID="txtPrivateAddressesNumber" runat="server" CssClass="form-control form-control" Width="150" Text=""></asp:TextBox>
                                                
                                                <asp:RequiredFieldValidator ID="PrivateAddressesValidator" runat="server" Text="*" Display="Dynamic"
                                                        ControlToValidate="txtPrivateAddressesNumber" meta:resourcekey="PrivateAddressesValidator" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                                        
                                                <asp:Literal ID="litMaxPrivateAddresses" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radioPrivateSelected" runat="server" AutoPostBack="true"
                                                    meta:resourcekey="radioPrivateSelected" Text="Assign specified IP addresses" 
                                                    GroupName="PrivateAddress" />
                                            </td>
                                        </tr>
                                        <tr id="PrivateAddressesListRow" runat="server">
                                            <td style="padding-left: 30px;">
                                                <asp:TextBox ID="txtPrivateAddressesList" runat="server" TextMode="MultiLine"
                                                    CssClass="form-control form-control" Width="300" Rows="5"></asp:TextBox>
                                                <br />
                                                <asp:Localize ID="locOnePerLine" runat="server"
                                                        meta:resourcekey="locOnePerLine" Text="* Type one IP address per line"></asp:Localize>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <asp:CheckBox ID="chkCustomGateway" runat="server" AutoPostBack="true" Checked="false"
                                                    meta:resourcekey="chkCustomGateway" Text="Custom Gateway and DNS" />
                                            </td>
                                        </tr>
                                        <tr id="trCustomGateway" runat="server">
                                            <td style="padding-left: 30px;">
                                                <asp:RequiredFieldValidator ID="GatewayValidator" runat="server" Text="*" Display="Dynamic"
                                                        ControlToValidate="txtGateway" meta:resourcekey="GatewayValidator" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                                <asp:Localize ID="locGateway" runat="server"
                                                        meta:resourcekey="locGateway" Text="Gateway:"></asp:Localize>
                                                <asp:TextBox ID="txtGateway" runat="server" CssClass="form-control form-control" Width="150" Text=""></asp:TextBox>

                                                <asp:RequiredFieldValidator ID="DNSValidator" runat="server" Text="*" Display="Dynamic"
                                                        ControlToValidate="txtDNS1" meta:resourcekey="DNSValidator" SetFocusOnError="true"
                                                        ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                                <asp:Localize ID="locDNS1" runat="server"
                                                        meta:resourcekey="locDNS1" Text="Preferred DNS server:"></asp:Localize>
                                                <asp:TextBox ID="txtDNS1" runat="server" CssClass="form-control form-control" Width="150" Text=""></asp:TextBox>

                                                <asp:Localize ID="locDNS2" runat="server"
                                                        meta:resourcekey="locDNS2" Text="Alternate DNS server:"></asp:Localize>
                                                <asp:TextBox ID="txtDNS2" runat="server" CssClass="form-control form-control" Width="150" Text=""></asp:TextBox>
                                            </td>
                                            <td>
                                                
                                            </td>
                                            <td>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    
                                    <table style="border-collapse: separate; border-spacing: 3px;">
                                        <tr>
                                            <td><asp:Localize ID="locPrivateNetworkFormat" runat="server"
                                            meta:resourcekey="locPrivateNetworkFormat" Text="Network addresses format:"></asp:Localize></td>
                                            <td><b><asp:Literal ID="litPrivateNetworkFormat" runat="server" Text="[network format]"></asp:Literal></b></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locPrivateSubnetMask" runat="server"
                                            meta:resourcekey="locPrivateSubnetMask" Text="Subnet mask:"></asp:Localize></td>
                                            <td><b><asp:Literal ID="litPrivateSubnetMask" runat="server" Text="[subnet mask]"></asp:Literal></b></td>
                                        </tr>
                                    </table>
                                    
                                    <br />
                                    
                            </asp:WizardStep>
                            
                            
                            
                            <asp:WizardStep ID="stepSummary" runat="server" meta:resourcekey="stepSummary" Title="Summary">
                                    <table style="border-collapse: separate; border-spacing: 6px 1px;">
                                        <tr>
                                            <td colspan="2" class="NormalBold">
                                                <asp:Localize ID="locNameStepTitle2" runat="server"
                                                    meta:resourcekey="locNameStepTitle" Text="Name and Operating System" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="Localize1" runat="server"
                                                meta:resourcekey="locHostname" Text="Host name"></asp:Localize></td>
                                            <td><asp:Literal ID="litHostname" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="Localize2" runat="server"
                                                meta:resourcekey="locOperatingSystem" Text="Operating system"></asp:Localize></td>
                                            <td><asp:Literal ID="litOperatingSystem" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr id="SummSummaryEmailRow" runat="server">
                                            <td><asp:Localize ID="locSendSummary" runat="server"
                                                meta:resourcekey="chkSendSummary" Text="Send summary letter to"></asp:Localize></td>
                                            <td><asp:Literal ID="litSummaryEmail" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="NormalBold">
                                                <asp:Localize ID="locConfig2" runat="server"
                                                    meta:resourcekey="locConfigStepTitle" Text="Configuration" />
                                            </td>
                                        </tr>

                                        <%--<scp:Generation runat="server" ID="GenerationSettingsSummary" Mode="Summary"/>--%>

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
                                        <tr>
                                            <td><asp:Localize ID="locHddIOPSmin" runat="server" meta:resourcekey="locHddIOPSmin" Text="HDD minimum IOPS:" /></td>
                                            <td><asp:Literal ID="litHddIOPSmin" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locHddIOPSmax" runat="server" meta:resourcekey="locHddIOPSmax" Text="HDD maximum IOPS:" /></td>
                                            <td><asp:Literal ID="litHddIOPSmax" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locSnapshots2" runat="server" meta:resourcekey="locSnapshots" Text="Number of snapshots:" /></td>
                                            <td><asp:Literal ID="litSnapshots" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locDvdInstalled" runat="server" meta:resourcekey="locDvdInstalled" Text="DVD Drive installed:" /></td>
                                            <td><scp:CheckBoxOption id="optionDvdInstalled" runat="server" Value="True" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locBootFromCd" runat="server" meta:resourcekey="locBootFromCd" Text="Boot from CD:" /></td>
                                            <td><scp:CheckBoxOption id="optionBootFromCd" runat="server" Value="False" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locNumLock" runat="server" meta:resourcekey="locNumLock" Text="Num Lock enabled:" /></td>
                                            <td><scp:CheckBoxOption id="optionNumLock" runat="server" Value="False" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locStartShutdownAllowed" runat="server"
                                                meta:resourcekey="locStartShutdownAllowed" Text="Start, turn off and shutdown allowed:" /></td>
                                            <td><scp:CheckBoxOption id="optionStartShutdown" runat="server" Value="True" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locPauseResumeAllowed" runat="server"
                                                meta:resourcekey="locPauseResumeAllowed" Text="Pause, resume allowed:" /></td>
                                            <td><scp:CheckBoxOption id="optionPauseResume" runat="server" Value="True" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locRebootAllowed" runat="server"
                                                meta:resourcekey="locRebootAllowed" Text="Reboot allowed:" /></td>
                                            <td><scp:CheckBoxOption id="optionReboot" runat="server" Value="True" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locResetAllowed" runat="server"
                                                meta:resourcekey="locResetAllowed" Text="Reset allowed:" /></td>
                                            <td><scp:CheckBoxOption id="optionReset" runat="server" Value="True" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locReinstallAllowed" runat="server"
                                                meta:resourcekey="locReinstallAllowed" Text="Re-install allowed:" /></td>
                                            <td><scp:CheckBoxOption id="optionReinstall" runat="server" Value="True" /></td>
                                        </tr>
                                        
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="NormalBold">
                                                <asp:Localize ID="locDynamicMemory" runat="server"
                                                    meta:resourcekey="locDynamicMemory" Text="Dynamic Memory" />
                                            </td>
                                        </tr>
                                        <scp:DynamicMemoryControl runat="server" ID="DynamicMemoryControlSummary" Mode="Summary"/>
                                        
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="NormalBold">
                                                <asp:Localize ID="locExternalNetwork2" runat="server"
                                                    meta:resourcekey="locExternalNetwork" Text="External Network" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locExternalNetworkEnabled" runat="server"
                                                meta:resourcekey="locExternalNetworkEnabled" Text="External network enabled:" /></td>
                                            <td><scp:CheckBoxOption id="optionExternalNetwork" runat="server" Value="True" /></td>
                                        </tr>
                                        <tr id="SummExternalAddressesNumberRow" runat="server">
                                            <td><asp:Localize ID="locExternalAddressesNumber" runat="server"
                                                meta:resourcekey="locExternalAddressesNumber" Text="Number of IP addresses:" /></td>
                                            <td><asp:Literal ID="litExternalAddressesNumber" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr id="SummExternalAddressesListRow" runat="server">
                                            <td><asp:Localize ID="locExternalAddressesList" runat="server"
                                                meta:resourcekey="locExternalAddressesList" Text="IP addresses list:" /></td>
                                            <td><asp:Literal ID="litExternalAddresses" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr id="SummExternalAddressMAC" runat="server">
                                            <td><asp:Localize ID="locSummExternalAddressMAC" runat="server"
                                                meta:resourcekey="locSummExternalAddressMAC" Text="MAC:" /></td>
                                            <td><asp:Literal ID="litSummExternalAddressMAC" runat="server"></asp:Literal></td>
                                        </tr>
                                        
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="NormalBold">
                                                <asp:Localize ID="locPrivateNetwork2" runat="server"
                                                    meta:resourcekey="locPrivateNetwork" Text="Private Network" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Localize ID="locPrivateNetworkEnabled" runat="server"
                                                meta:resourcekey="locPrivateNetworkEnabled" Text="Private network enabled:" /></td>
                                            <td><scp:CheckBoxOption id="optionPrivateNetwork" runat="server" Value="True" /></td>
                                        </tr>
                                        <tr id="SummPrivateAddressesNumberRow" runat="server">
                                            <td><asp:Localize ID="locPrivateAddressesNumber" runat="server"
                                                meta:resourcekey="locPrivateAddressesNumber" Text="Number of IP addresses:" /></td>
                                            <td><asp:Literal ID="litPrivateAddressesNumber" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr id="SummPrivateAddressesListRow" runat="server">
                                            <td><asp:Localize ID="locPrivateAddressesList" runat="server"
                                                meta:resourcekey="locPrivateAddressesList" Text="IP addresses list:" /></td>
                                            <td><asp:Literal ID="litPrivateAddressesList" runat="server"></asp:Literal></td>
                                        </tr>
                                    </table>
                                    <br />
                            </asp:WizardStep>
                        </WizardSteps>
                        <StepPreviousButtonStyle CssClass="btn btn-primary" />
                        <CancelButtonStyle CssClass="btn btn-warning" />
                    </asp:Wizard>
				    
			    </div>
		    </div>
	    </div>