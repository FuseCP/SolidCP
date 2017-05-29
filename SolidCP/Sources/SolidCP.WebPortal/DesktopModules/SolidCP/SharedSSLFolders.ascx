<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedSSLFolders.ascx.cs" Inherits="SolidCP.Portal.SharedSSLFolders" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="add"
    GroupName="Web"
    TypeName="SolidCP.Providers.Web.SharedSSLFolder, SolidCP.Providers.Base"
    QuotaName="Web.SharedSSL" />