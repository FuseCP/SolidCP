<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailGroups.ascx.cs" Inherits="SolidCP.Portal.MailGroups" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="SolidCP.Providers.Mail.MailGroup, SolidCP.Providers.Base"
    QuotaName="Mail.Groups" />