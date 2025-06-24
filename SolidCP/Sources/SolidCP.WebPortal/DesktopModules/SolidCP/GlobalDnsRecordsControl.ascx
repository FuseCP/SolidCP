<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalDnsRecordsControl.ascx.cs" Inherits="SolidCP.Portal.GlobalDnsRecordsControl" %>
<%@ Register Src="UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>

<asp:Panel ID="pnlRecords" runat="server">
    <div class="text-right" style="margin-bottom:10px;">
        <CPCC:StyleButton ID="btnAdd" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click" CausesValidation="False">
            <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAdd" />
        </CPCC:StyleButton>
        <br />
    </div>
</asp:Panel>
<asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False"
    DataKeyNames="RecordID" EmptyDataText="gvRecords" CssSelectorClass="NormalGridView"
    OnRowEditing="gvRecords_RowEditing" OnRowDeleting="gvRecords_RowDeleting">
    <Columns>
        <asp:TemplateField HeaderText="gvRecordsName" ItemStyle-CssClass="NormalBold" ItemStyle-Wrap="false">
            <ItemTemplate>
                <ItemStyle Width="25px" HorizontalAlign="Center" />
                <asp:ImageButton ID="cmdEdit" runat="server" SkinID="EditSmall" CommandName="edit" AlternateText="Edit record">
                </asp:ImageButton>
                <%# Eval("RecordName") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="RecordType" HeaderText="gvRecordsType" />
        <asp:BoundField DataField="FullRecordData" HeaderText="gvRecordsData" ItemStyle-Width="100%" />
        <asp:TemplateField>
            <ItemStyle Width="65px" HorizontalAlign="Center" />
            <ItemTemplate>
                <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="delete" OnClientClick="return confirm('Delete?');"> 
                    &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                </CPCC:StyleButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnSave">
    <table>
        <tr>
            <td class="SubHead" width="150" nowrap><asp:Label ID="lblRecordType" runat="server" meta:resourcekey="lblRecordType" Text="Record Type:"></asp:Label></td>
            <td class="Normal" width="260px">
                <asp:DropDownList ID="ddlRecordType" runat="server" SelectedValue='<%# Bind("RecordType") %>' CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlRecordType_SelectedIndexChanged">
                    <asp:ListItem>A</asp:ListItem>
                    <asp:ListItem>AAAA</asp:ListItem>
                    <asp:ListItem>MX</asp:ListItem>
                    <asp:ListItem>NS</asp:ListItem>
                    <asp:ListItem>TXT</asp:ListItem>
                    <asp:ListItem>CNAME</asp:ListItem>
                    <asp:ListItem>SRV</asp:ListItem>
                    <asp:ListItem>PTR</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="SubHead"><asp:Label ID="lblRecordName" runat="server" meta:resourcekey="lblRecordName" Text="Record Name:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtRecordName" runat="server" Width="260px" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr id="rowData" runat="server">
            <td class="SubHead"><asp:Label ID="lblRecordData" runat="server" meta:resourcekey="lblRecordData" Text="Record Data:"></asp:Label></td>
            <td class="Normal" nowrap>
                <div class="form-inline">
                <asp:TextBox ID="txtRecordData" runat="server" Width="260px" CssClass="form-control"></asp:TextBox> <uc1:SelectIPAddress ID="ipAddress" CssClass="form-control" runat="server" />
                    </div>
<!--
                <asp:RequiredFieldValidator ID="valRequireData" runat="server" ControlToValidate="txtRecordData"
                    ErrorMessage="*" ValidationGroup="DnsRecord" Display="Dynamic"></asp:RequiredFieldValidator>
-->
                <asp:CustomValidator ID="IPValidator" runat="server" ControlToValidate="txtRecordData" ValidationGroup="DnsRecord" Display="Dynamic" CssClass="NormalBold" 
                    OnServerValidate="Validate" Text="Please enter a valid IP" meta:resourcekey="IPValidator" ValidateEmptyText="True" />

            </td>
        </tr>
        <tr id="rowMXPriority" runat="server">
            <td class="SubHead"><asp:Label ID="lblMXPriority" runat="server" meta:resourcekey="lblMXPriority" Text="MX Priority:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtMXPriority" runat="server" style="width:60px;" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireMxPriority" runat="server" ControlToValidate="txtMXPriority"
                    ErrorMessage="*" ValidationGroup="DnsRecord" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="valRequireCorrectPriority" runat="server" ControlToValidate="txtMXPriority"
                    ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="DnsRecord" />
            </td>
        </tr>

        <tr id="rowSRVPriority" runat="server">
            <td class="SubHead"><asp:Label ID="lblSRVPriority" runat="server" meta:resourcekey="lblSRVPriority" Text="SRV Priority:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtSRVPriority" runat="server" style="width:60px;" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>

        <tr id="rowSRVWeight" runat="server">
            <td class="SubHead"><asp:Label ID="lblSRVWeight" runat="server" meta:resourcekey="lblSRVWeight" Text="Weight:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtSRVWeight" runat="server" style="width:60px;" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>

        <tr id="rowSRVPort" runat="server">
            <td class="SubHead"><asp:Label ID="lblSRVPort" runat="server" meta:resourcekey="lblSRVPort" Text="Port Number:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtSRVPort" runat="server" style="width:60px;" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td colspan="2" align="right">
                <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="DnsRecord"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSave"/> </CPCC:StyleButton>
            </td>
        </tr>
    </table>
</asp:Panel>