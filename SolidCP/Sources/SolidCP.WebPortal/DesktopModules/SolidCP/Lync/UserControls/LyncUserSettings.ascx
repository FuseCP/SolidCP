<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LyncUserSettings.ascx.cs" Inherits="SolidCP.Portal.Lync.UserControls.LyncUserSettings" %>
<%@ Register Src="../../ExchangeServer/UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<asp:DropDownList ID="ddlSipAddresses" runat="server" CssClass="form-control"></asp:DropDownList>
<scp:EmailAddress id="email" runat="server" ValidationGroup="CreateMailbox"></scp:EmailAddress>

