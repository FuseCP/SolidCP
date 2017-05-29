<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsReplications.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsReplications" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<script type="text/javascript">
    function ValidateCheckBoxList(sender, args) {
        var checkBoxList = document.getElementsByClassName("vhdContainer");
        var checkboxes = checkBoxList.getElementsByTagName("input");
        var isValid = false;
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].checked) {
                isValid = true;
                break;
            }
        }
        args.IsValid = isValid;
    }
</script>
<style>
    .ReplicaTable td {
        padding-bottom: 10px;
    }
    .ReplicaTable .AdditionalPoints td {
        padding-bottom: 0px;
    }
</style>

	    <div class="Content">
		    <div class="Center">
                <div class="FormBody">
                    <wsp:ServerTabs ID="tabs" runat="server" SelectedTab="vps_replication" />
                    <wsp:SimpleMessageBox ID="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server"
                        ValidationGroup="Vps" ShowMessageBox="True" ShowSummary="False" />

                    <%--<wsp:CollapsiblePanel ID="secReplicationDetails" runat="server" Visible="False"
                        TargetControlID="ReplicationDetailsPanel" meta:ResourceKey="secReplicationDetails" Text="Health"></wsp:CollapsiblePanel> Height="0" Style="overflow: hidden; padding: 10px; width: 750px;"--%>
                    <asp:Panel ID="ReplicationDetailsPanel" runat="server" Visible="False" Style="padding: 10px;">
                        <asp:Localize ID="locHealth" runat="server" meta:resourcekey="locHealth" Text="Health:"></asp:Localize>
                        <asp:Label ID="labHealth" runat="server"></asp:Label>
                        <asp:Button ID="btnDetailInfo" runat="server" CausesValidation="false" CssClass="ActionButtonRename"
                            meta:resourcekey="btnDetailInfo" Text="Details" Style="margin-left: 20px"></asp:Button>
                        <asp:Button ID="btnPause" runat="server" CausesValidation="false" CssClass="ActionButtonRename"
                            meta:resourcekey="btnPause" Text="Pause" OnClick="btnPause_Click" Style="margin-left: 20px"></asp:Button>
                        <asp:Button ID="btnResume" runat="server" CausesValidation="false" CssClass="ActionButtonRename"
                            meta:resourcekey="btnResume" Text="Resume" OnClick="btnResume_Click" Style="margin-left: 20px"></asp:Button>
                    </asp:Panel>

                    <wsp:CollapsiblePanel ID="secReplication" runat="server" Visible="True"
                        TargetControlID="ReplicationPanel" meta:ResourceKey="secReplication" Text="Replication Configuration">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="ReplicationPanel" runat="server" Height="0" Style="overflow: hidden; padding: 10px; width: 750px;">
                        <div class="FormButtonsBarClean" onclick="ShowProgressDialog('Prepare UI...');">
                            <asp:CheckBox ID="chbEnable" runat="server" meta:resourcekey="chbEnable" Text="Enable replication" AutoPostBack="True"  />
                        </div>
                        <table runat="server" id="ReplicaTable" class="ReplicaTable" style="margin: 10px; width: 100%;">
				            <tr>
				                <td style="width: 200px;">
				                   <asp:Localize ID="locPrimaryServer" runat="server" meta:resourcekey="locPrimaryServer" Text="Primary Server:"></asp:Localize>
				                </td>
				                <td>
				                   <asp:Label ID="labPrimaryServer" runat="server"></asp:Label>
				                </td>
                            </tr>
                            <tr>
				                <td>
				                   <asp:Localize ID="locReplicaServer" runat="server" meta:resourcekey="locReplicaServer" Text="Replica Server:"></asp:Localize>
				                </td>
				                <td>
				                   <asp:Label ID="labReplicaServer" runat="server"></asp:Label>
				                </td>
                            </tr>
                            <tr>
				                <td>
				                   <asp:Localize ID="locLastSynchronized" runat="server" meta:resourcekey="locLastSynchronized" Text="Last synchronized at:"></asp:Localize>
				                </td>
				                <td>
				                   <asp:Label ID="labLastSynchronized" runat="server"></asp:Label>
				                </td>
                            </tr>
                            <tr runat="server" ID="trVHDEditable">
				                <td>
				                   <asp:Localize ID="locVHDs" runat="server" meta:resourcekey="locVHDs" Text="Choose Replication VHDs:"></asp:Localize>
                                </td>
                                <td>
                                    <asp:CheckBoxList runat="server" ID="chlVHDs"  class="vhdContainer"/>
                                    <asp:CustomValidator ID="valVHDs" ErrorMessage="Please select at least one VHD" ValidationGroup="Vps" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" ClientValidationFunction="ValidateCheckBoxList" runat="server" />
                                </td>
                            </tr>
                            <tr runat="server" ID="trVHDReadOnly" Visible="False">
				                <td>
				                   <asp:Localize ID="locVHDsReadOnly" runat="server" meta:resourcekey="locVHDsReadOnly" Text="Replication VHDs:"></asp:Localize>
                                </td>
                                <td>
                                    <asp:Label ID="labVHDs" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
				                <td>
				                   <asp:Localize ID="locCeritficate" runat="server" meta:resourcekey="locCeritficate" Text="SSL ceritficate:"></asp:Localize>
                                </td>
                                <td>
                                    <div runat="server" ID="ddlCeritficateDiv">
                                        <asp:DropDownList runat="server" ID="ddlCeritficate" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCeritficate" ValidationGroup="Vps" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="SSL certificate is required" ForeColor="Red" meta:resourcekey="valCeritficateRequire" Text="*" />
                                    </div>
                                    <div runat="server" ID="txtCeritficateDiv">
                                        <asp:TextBox runat="server" ID="txtCeritficate" Width="350px"/>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCeritficate" ValidationGroup="Vps" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="SSL certificate is required" ForeColor="Red" meta:resourcekey="valCeritficateRequire" Text="*" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
				                <td>
				                   <asp:Localize ID="locFrequency" runat="server" meta:resourcekey="locFrequency" Text="Replication Frequency:"></asp:Localize>
				                </td>
				                <td>
				                   <asp:DropDownList  runat="server" ID="ddlFrequency" >
				                       <asp:ListItem Value="30" Selected="True" meta:resourcekey="ddlFrequency30seconds">30seconds</asp:ListItem>
				                       <asp:ListItem Value="300" meta:resourcekey="ddlFrequency5minutes">5minutes</asp:ListItem>
				                       <asp:ListItem Value="900" meta:resourcekey="ddlFrequency15minutes">15minutes</asp:ListItem>
				                   </asp:DropDownList>
				                </td>
                            </tr>
                            <tr class="AdditionalPoints">
                                <td colspan="2">
                                    <asp:Label ID="locRecoveryPoints" runat="server" meta:resourcekey="locRecoveryPoints" 
                                        Text="Additional Recovery Points:" style="margin-bottom: 10px;"></asp:Label>

                                    <asp:RadioButtonList ID="radRecoveryPoints" runat="server" AutoPostBack="true">
                                        <asp:ListItem Value="OnlyLast" meta:resourcekey="radRecoveryPointsLast" Selected="True">Maintain only the latest recovery point</asp:ListItem>
                                        <asp:ListItem Value="Additional" meta:resourcekey="radRecoveryPointsAdditional">Create additional hourly recovery points</asp:ListItem>
                                    </asp:RadioButtonList>

                                    <table runat="server" ID="tabAdditionalRecoveryPoints" class="AdditionalPoints" style="margin: 10px; width: 100%;">
                                        <tr>
                                            <td style="width: 480px;">
                                                <asp:Localize ID="locRecoveryPointsAdditional" runat="server" meta:resourcekey="locRecoveryPointsAdditional"
                                                    Text="Coverage provided by additional recovery points (in hours):"></asp:Localize>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRecoveryPointsAdditional" Width="30px"></asp:TextBox>

                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecoveryPointsAdditional" Display="Dynamic" SetFocusOnError="true" 
                                                    ErrorMessage="Additional recovery points cannot be empty." ForeColor="Red" ValidationGroup="Vps"
                                                    meta:resourcekey="valRecoveryPointsAdditionalRequire" Text="*" />
                                                <asp:RangeValidator runat="server" ControlToValidate="txtRecoveryPointsAdditional" MinimumValue="1" MaximumValue="24" 
                                                    ErrorMessage="Additional recovery points range is 1-24." ValidationGroup="Vps" Display="Dynamic" SetFocusOnError="true"
                                                    meta:resourcekey="valRecoveryPointsAdditionalRange" ForeColor="Red" Type="Integer" Text="*" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chbVSS" AutoPostBack="True" meta:resourcekey="chbVSS" Text="Volume Shadow Copy Service (VSS) snapshot frequency (in hours):" />
                                            </td>
                                            <td>
                                                <div runat="server" ID="VSSdiv">
                                                    <asp:TextBox runat="server" ID="txtRecoveryPointsVSS" Width="30px"></asp:TextBox>

                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecoveryPointsVSS"
                                                        ErrorMessage="VSS snapshot frequency cannot be empty." ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                        meta:resourcekey="valRecoveryPointsVSSRequire" Text="*"  ValidationGroup="Vps"/>
                                                    <asp:RangeValidator runat="server" ControlToValidate="txtRecoveryPointsVSS" MinimumValue="1" MaximumValue="24"
                                                        ErrorMessage="VSS snapshot frequency range is 1-12." ValidationGroup="Vps" Display="Dynamic" SetFocusOnError="true"
                                                        meta:resourcekey="valRecoveryPointsVSSRange" Text="*" ForeColor="Red" Type="Integer" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div class="FormButtonsBarClean">
                            <asp:Button ID="btnUpdate" runat="server" CssClass="ActionButtonRename"
                                meta:resourcekey="btnUpdate" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="Vps"></asp:Button>
                        </div>
                    </asp:Panel>
                </div>
            </div>

        </div>

<asp:Panel ID="DetailsPanel" runat="server" CssClass="Popup" style="display:none;">
	<table class="Popup-Header" cellpadding="0" cellspacing="0">
		<tr>
			<td class="Popup-HeaderLeft"></td>
			<td class="Popup-HeaderTitle">
				<asp:Localize ID="locDetails" runat="server" Text="Replication details" meta:resourcekey="locDetails"></asp:Localize>
			</td>
			<td class="Popup-HeaderRight"></td>
		</tr>
	</table>
	<div class="Popup-Content">
		<div class="Popup-Body">
			<br />
			
			<table cellspacing="10">
			    <tr>
                    <td style="width: 180px">
				        <asp:Localize ID="locDetailsState" runat="server" Text="Replication State:" meta:resourcekey="locDetailsState"></asp:Localize>
                    </td>
                    <td>
                        <asp:Label ID="labDetailsState" runat="server"></asp:Label>
                    </td>
                </tr>
			    <tr>
                    <td>
				        <asp:Localize ID="locDetailsMode" runat="server" Text="Replication Mode:" meta:resourcekey="locDetailsMode"></asp:Localize>
                    </td>
                    <td>
                        <asp:Label ID="labDetailsMode" runat="server"></asp:Label>
                    </td>
                </tr>
			    <tr>
                    <td>
				        <asp:Localize ID="locDetailsPrimary" runat="server" Text="Current Primary Server:" meta:resourcekey="locDetailsPrimary"></asp:Localize>
                    </td>
                    <td>
                        <asp:Label ID="labDetailsPrimary" runat="server"></asp:Label>
                    </td>
                </tr>
			    <tr>
                    <td>
				        <asp:Localize ID="locDetailsReplica" runat="server" Text="Current Replica Server:" meta:resourcekey="locDetailsReplica"></asp:Localize>
                    </td>
                    <td>
                        <asp:Label ID="labDetailsReplica" runat="server"></asp:Label>
                    </td>
                </tr>
			    <tr>
                    <td>
				        <asp:Localize ID="locDetailsHealth" runat="server" Text="Replication Health:" meta:resourcekey="locDetailsHealth"></asp:Localize>
                    </td>
                    <td>
                        <asp:Label ID="labDetailsHealth" runat="server"></asp:Label>
                        <br/>
                        <asp:Label ID="labDetailsHealthDetails" runat="server"></asp:Label>
                    </td>
                </tr>
			</table>

            <wsp:CollapsiblePanel ID="StatisticCollapsiblePanel" runat="server" Visible="True"
                TargetControlID="StatisticPanel" meta:ResourceKey="secStatisticPanel" Text="Statistic for past "></wsp:CollapsiblePanel>
            <asp:Panel ID="StatisticPanel" runat="server" Height="0" Style="overflow: hidden; padding: 10px; width: 400px;">
                <table cellspacing="10">
                    <tr>
                        <td style="width: 180px">
                            <asp:Localize ID="locFromTime" runat="server" Text="From time:" meta:resourcekey="locFromTime"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labFromTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locToTime" runat="server" Text="To time:" meta:resourcekey="locToTime"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labToTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locAverageSize" runat="server" Text="Average size:" meta:resourcekey="locAverageSize"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labAverageSize" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locMaximumSize" runat="server" Text="Maximum size:" meta:resourcekey="locMaximumSize"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labMaximumSize" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locAverageLatency" runat="server" Text="Average latency:" meta:resourcekey="locAverageLatency"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labAverageLatency" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locErrorsEncountered" runat="server" Text="Errors encountered:" meta:resourcekey="locErrorsEncountered"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labErrorsEncountered" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locSuccessfulReplicationCycles" runat="server" Text="Successful replication cycles:" meta:resourcekey="locSuccessfulReplicationCycles"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labSuccessfulReplicationCycles" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

                      
            <wsp:CollapsiblePanel ID="PendingReplicationCollapsiblePanel" runat="server" Visible="True"
                TargetControlID="PendingReplicationPanel" meta:ResourceKey="secPendingReplication" Text="Pending replication"></wsp:CollapsiblePanel>
            <asp:Panel ID="PendingReplicationPanel" runat="server" Height="0" Style="overflow: hidden; padding: 10px; width: 400px;">
                <table cellspacing="10">
                    <tr>
                        <td style="width: 180px">
                            <asp:Localize ID="locSizeData" runat="server" Text="Size of data yet to be replicated:" meta:resourcekey="locSizeData"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labSizeData" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locLastSyncro" runat="server" Text="Last synchronized at:" meta:resourcekey="locLastSyncro"></asp:Localize>
                        </td>
                        <td>
                            <asp:Label ID="labLastSyncro" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

			<br />
		</div>
		
		<div class="FormFooter">
			&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="Button1" meta:resourcekey="btnCancel" Text="Cancel" CausesValidation="false" />
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="DetailModal" runat="server" BehaviorID="DetailModal"
	TargetControlID="btnDetailInfo" PopupControlID="DetailsPanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancel" />