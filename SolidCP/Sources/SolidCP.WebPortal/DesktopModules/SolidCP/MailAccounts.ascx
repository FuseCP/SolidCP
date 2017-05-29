<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailAccounts.ascx.cs" Inherits="SolidCP.Portal.MailAccounts" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="SolidCP.Providers.Mail.MailAccount, SolidCP.Providers.Base"
    QuotaName="Mail.Accounts" />