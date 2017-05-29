<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FtpAccounts.ascx.cs" Inherits="SolidCP.Portal.FtpAccounts" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="FTP"
    TypeName="SolidCP.Providers.FTP.FtpAccount, SolidCP.Providers.Base"
    QuotaName="FTP.Accounts" />