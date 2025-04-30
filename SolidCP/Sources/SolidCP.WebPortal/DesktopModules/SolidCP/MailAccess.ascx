<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailAccess.ascx.cs" Inherits="SolidCP.Portal.MailAccess" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    ShowCreateButton="False"
    ShowQuota="False"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="SolidCP.Providers.Mail.MailDomain, SolidCP.Providers.Base"
    QuotaName="Mail.AllowAccessControls" />