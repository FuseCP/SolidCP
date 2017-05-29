<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailEditItems.ascx.cs" Inherits="SolidCP.Portal.MailEditItems" %>
<asp:TextBox id="txtItems" runat="server" Width="300px" TextMode="MultiLine" Rows="7"></asp:TextBox>
<br/>
<span class=Normal>
<asp:Label ID="lblOnePerLine" runat="server" meta:resourcekey="lblOnePerLine" Text="* Enter one e-mail address per line"></asp:Label>
</span>