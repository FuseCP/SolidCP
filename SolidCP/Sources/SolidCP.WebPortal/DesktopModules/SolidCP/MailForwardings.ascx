<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailForwardings.ascx.cs" Inherits="SolidCP.Portal.MailForwardings" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="SolidCP.Providers.Mail.MailAlias, SolidCP.Providers.Base"
    QuotaName="Mail.Forwardings" />