<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllocatePackageIPAddresses.ascx.cs" Inherits="SolidCP.Portal.UserControls.AllocatePackageIPAddresses" %>
<%@ Register Src="SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>


<scp:SimpleMessageBox id="messageBox" runat="server" />

<asp:ValidationSummary ID="validatorsSummary" runat="server" 
    ValidationGroup="AddAddress" ShowMessageBox="True" ShowSummary="False" />
 
<ul id="ErrorMessagesList" runat="server" visible="false">
    <li id="EmptyAddressesMessage" runat="server">
        <asp:Localize ID="locNotEnoughAddresses" runat="server" Text="Not enough..." meta:resourcekey="locNotEnoughAddresses"></asp:Localize>
    </li>
    <li id="QuotaReachedMessage" runat="server">
        <asp:Localize ID="locQuotaReached" runat="server" Text="Quota reached..." meta:resourcekey="locQuotaReached"></asp:Localize>
    </li>
</ul>

 <asp:UpdatePanel runat="server" ID="AddressesTable" UpdateMode="Conditional">
     <ContentTemplate>
        <table cellspacing="5" style="width: 100%;">
            <tr>
                <td>
                    <asp:RadioButton ID="radioExternalRandom" runat="server" AutoPostBack="true"
                        meta:resourcekey="radioExternalRandom" Text="Randomly select IP addresses from the pool" 
                        Checked="True" GroupName="ExternalAddress" 
                        oncheckedchanged="radioExternalRandom_CheckedChanged" />
                </td>
            </tr>
            <tr id="AddressesNumberRow" runat="server">
                <td style="padding-left: 30px;">
                    <asp:Localize ID="locExternalAddresses" runat="server"
                            meta:resourcekey="locExternalAddresses" Text="Number of IP addresses:"></asp:Localize>

                    <asp:TextBox ID="txtExternalAddressesNumber" runat="server" CssClass="form-control" Width="50"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator ID="ExternalAddressesValidator" runat="server" Text="*" Display="Dynamic"
                            ControlToValidate="txtExternalAddressesNumber" meta:resourcekey="ExternalAddressesValidator" SetFocusOnError="true"
                            ValidationGroup="AddAddress">*</asp:RequiredFieldValidator>
                            
                    <asp:Literal ID="litMaxAddresses" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButton ID="radioExternalSelected" runat="server" AutoPostBack="true"
                        meta:resourcekey="radioExternalSelected" Text="Select IP addresses from the list" 
                        GroupName="ExternalAddress" 
                        oncheckedchanged="radioExternalSelected_CheckedChanged" />
                </td>
            </tr>
            <tr id="AddressesListRow" runat="server">
                <td style="padding-left: 30px;">
                    <asp:ListBox ID="listExternalAddresses" SelectionMode="Multiple" runat="server" Rows="8"
                        CssClass="form-control" Width="220" style="height:100px;" ></asp:ListBox>
                    <br />
                    <asp:Localize ID="locHoldCtrl" runat="server" 
                            meta:resourcekey="locHoldCtrl" Text="* Hold CTRL key to select multiple addresses" ></asp:Localize>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<p style="text-align:right;">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" ValidationGroup="AddAddress"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAdd"/> </CPCC:StyleButton>
</p>