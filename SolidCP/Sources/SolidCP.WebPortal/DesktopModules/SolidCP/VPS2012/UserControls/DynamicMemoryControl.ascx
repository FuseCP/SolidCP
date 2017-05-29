<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicMemoryControl.ascx.cs" Inherits="SolidCP.Portal.VPS2012.UserControls.DynamicMemoryControl" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../../UserControls/CollapsiblePanel.ascx" %>
<%@ Register TagPrefix="scp" TagName="CheckBoxOption" Src="../../UserControls/CheckBoxOption.ascx" %>

<% if (Mode != VirtualMachineSettingsMode.Summary){ %>
    <scp:CollapsiblePanel id="secDymanicMemory" runat="server" TargetControlID="DymanicMemoryPanel" meta:resourcekey="secDymanicMemory" Text="Dymanic memory">
    </scp:CollapsiblePanel>
    <asp:Panel ID="DymanicMemoryPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
        <table>
            <% if (Mode == VirtualMachineSettingsMode.Edit) { %>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="chkDynamicMemoryEnabled" runat="server" AutoPostBack="true" meta:resourcekey="chkDynamicMemoryEnabled" Text="Dynamic memory enabled" />
                
                    <table id="tableDynamicMemory" runat="server" cellspacing="5" style="width: 100%; margin-top: 15px">
                        <tr>
                            <td class="FormLabel150" >
                                <asp:Localize ID="locMinimum" runat="server" meta:resourcekey="locMinimum" Text="Minimum RAM:"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMinimum" runat="server" CssClass="form-control" Width="50" Text=""></asp:TextBox>

                                <asp:RequiredFieldValidator ControlToValidate="txtMinimum" Display="Dynamic"
                                    meta:resourcekey="MinimumRequireValidator" runat="server" SetFocusOnError="true"
                                    Text="*" ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ControlToValidate="txtMinimum" ErrorMessage="Value must be a whole number" 
                                    meta:resourcekey="MinimumCompareValidator" Operator="DataTypeCheck" runat="server" SetFocusOnError="True" 
                                    Text="*" Type="Integer" ValidationGroup="Vps">*</asp:CompareValidator>
                            </td>
                        </tr>   
                        <tr>
                            <td class="FormLabel150" >
                                <asp:Localize ID="locMaximum" runat="server" meta:resourcekey="locMaximum" Text="Maximum RAM:"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaximum" runat="server" CssClass="form-control" Width="50" Text=""></asp:TextBox>

                                <asp:RequiredFieldValidator ControlToValidate="txtMaximum" Display="Dynamic"
                                    meta:resourcekey="MaximumRequireValidator" runat="server" SetFocusOnError="true"
                                    Text="*" ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ControlToValidate="txtMaximum" ErrorMessage="Value must be a whole number" 
                                    meta:resourcekey="MaximumCompareValidator" Operator="DataTypeCheck" runat="server" SetFocusOnError="True" 
                                    Text="*" Type="Integer" ValidationGroup="Vps">*</asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150" >
                                <asp:Localize ID="locBuffer" runat="server" meta:resourcekey="locBuffer" Text="Buffer (%):"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuffer" runat="server" CssClass="form-control" Width="50" Text=""></asp:TextBox>

                                <asp:RequiredFieldValidator ControlToValidate="txtBuffer" Display="Dynamic"
                                    meta:resourcekey="BufferRequireValidator" runat="server" SetFocusOnError="true"
                                    Text="*" ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator runat="server" Type="Integer" MinimumValue="0" MaximumValue="100" ControlToValidate="txtBuffer" 
                                    meta:resourcekey="BufferRangeValidator" ErrorMessage="Value must be a whole number between 0 and 100" 
                                    Text="*" ValidationGroup="Vps">*</asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150" >
                                <asp:Localize ID="locPriority" runat="server" meta:resourcekey="locPriority" Text="Weight (Priority):"></asp:Localize>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriority" runat="server" CssClass="form-control" Width="50" Text=""></asp:TextBox>

                                <asp:RequiredFieldValidator ControlToValidate="txtPriority" Display="Dynamic"
                                    meta:resourcekey="PriorityRequireValidator" runat="server" SetFocusOnError="true"
                                    Text="*" ValidationGroup="Vps">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator runat="server" Type="Integer" MinimumValue="0" MaximumValue="100" ControlToValidate="txtPriority" 
                                    meta:resourcekey="PriorityRangeValidator" ErrorMessage="Value must be a whole number between 0 and 100" 
                                    Text="*" ValidationGroup="Vps">*</asp:RangeValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <% } else { %>
            <tr>
                <td colspan="2">
                    <scp:CheckBoxOption ID="optionDymanicMemoryDisplay" runat="server" Text="Dymanic memory enabled" meta:resourcekey="optionDymanicMemory" />
                </td>
            </tr>
            <tr runat="server" id="trMinimumDisplay">
                <td><asp:Localize ID="locMinimumDisplay" runat="server" meta:resourcekey="locMinimum" Text="Minimum:" /></td>
                <td><asp:Literal ID="litMinimumDisplay" runat="server"></asp:Literal></td>
            </tr>
            <tr runat="server" id="trMaximumDisplay">
                <td><asp:Localize ID="locMaximumDisplay" runat="server" meta:resourcekey="locMaximum" Text="Maximum:" /></td>
                <td><asp:Literal ID="litMaximumDisplay" runat="server"></asp:Literal></td>
            </tr>
            <tr runat="server" id="trBufferDisplay">
                <td><asp:Localize ID="locBufferDisplay" runat="server" meta:resourcekey="locBuffer" Text="Buffer (%):" /></td>
                <td><asp:Literal ID="litBufferDisplay" runat="server"></asp:Literal></td>
            </tr>
            <tr runat="server" id="trPriorityDisplay">
                <td><asp:Localize ID="locPriorityDisplay" runat="server" meta:resourcekey="locPriority" Text="Weight (Priority):" /></td>
                <td><asp:Literal ID="litPriorityDisplay" runat="server"></asp:Literal></td>
            </tr>
            <% } %>
        </table>
    </asp:Panel>
<% } else { %>
    <tr>
        <td><asp:Localize ID="locDymanicMemorySummary" runat="server" meta:resourcekey="locDymanicMemorySummary" Text="Dymanic memory enabled:" /></td>
        <td><scp:CheckBoxOption id="optionDymanicMemorySummary" runat="server" /></td>
    </tr>
    <tr runat="server" id="trMinimumSummary">
        <td><asp:Localize ID="locMinimumSummary" runat="server" meta:resourcekey="locMinimum" Text="Minimum:" /></td>
        <td><asp:Literal ID="litMinimumSummary" runat="server"></asp:Literal></td>
    </tr>
    <tr runat="server" id="trMaximumSummary">
        <td><asp:Localize ID="locMaximumSummary" runat="server" meta:resourcekey="locMaximum" Text="Maximum:" /></td>
        <td><asp:Literal ID="litMaximumSummary" runat="server"></asp:Literal></td>
    </tr>
    <tr runat="server" id="trBufferSummary">
        <td><asp:Localize ID="locBufferSummary" runat="server" meta:resourcekey="locBuffer" Text="Buffer (%):" /></td>
        <td><asp:Literal ID="litBufferSummary" runat="server"></asp:Literal></td>
    </tr>
    <tr runat="server" id="trPrioritySummary">
        <td><asp:Localize ID="locPrioritySummary" runat="server" meta:resourcekey="locPriority" Text="Weight (Priority):" /></td>
        <td><asp:Literal ID="litPrioritySummary" runat="server"></asp:Literal></td>
    </tr>
<% } %>
