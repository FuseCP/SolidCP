<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeAddDomainName.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeAddDomainName" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="Image1" SkinID="ExchangeDomainNameAdd48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Add Domain"></asp:Localize>
    </h3>
</div>
<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <table>
        <tr>
            <td class="FormLabel150">
                <asp:Localize ID="locDomainName" runat="server" meta:resourcekey="locDomainName" Text="Domain Name:"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList id="ddlDomains" runat="server" CssClass="form-control" DataTextField="DomainName" DataValueField="DomainID" style="vertical-align:middle;"></asp:DropDownList>
            </td>
        </tr>
    </table>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Creating Domain...');" ValidationGroup="CreateDomain">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddText"/>
    </CPCC:StyleButton>
    &nbsp;
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateDomain" />
</div>