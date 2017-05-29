<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlUsers.ascx.cs" Inherits="SolidCP.Portal.SqlUsers" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddUser"
    CreateControlID="edit_item"
    GroupName="MsSQL2000"
    TypeName="SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base"
    QuotaName="MsSQL2000.Users" />