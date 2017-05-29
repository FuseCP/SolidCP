<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSSessionLimit.ascx.cs" Inherits="SolidCP.Portal.RDS.UserControls.RDSSessionLimit" %>
<asp:DropDownList ID="SessionLimit" runat="server" CssClass="form-control">
    <asp:ListItem Value="0" Text="Never" />
    <asp:ListItem Value="1" Text="1 minute" />
    <asp:ListItem Value="5" Text="5 minutes" />
    <asp:ListItem Value="10" Text="10 minutes" />
    <asp:ListItem Value="15" Text="15 minutes" />
    <asp:ListItem Value="30" Text="30 minutes" />
    <asp:ListItem Value="60" Text="1 hour" />
    <asp:ListItem Value="120" Text="2 hours" />
    <asp:ListItem Value="180" Text="3 hours" />
    <asp:ListItem Value="360" Text="6 hours" />
    <asp:ListItem Value="480" Text="8 hours" />
    <asp:ListItem Value="720" Text="12 hours" />
    <asp:ListItem Value="960" Text="16 hours" />
    <asp:ListItem Value="1080" Text="18 hours" />
    <asp:ListItem Value="1440" Text="1 day" />
    <asp:ListItem Value="2880" Text="2 days" />
    <asp:ListItem Value="4320" Text="3 days" />
    <asp:ListItem Value="5760" Text="4 days" />
    <asp:ListItem Value="7200" Text="5 days" />
</asp:DropDownList>