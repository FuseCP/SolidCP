<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OdbcSources.ascx.cs" Inherits="SolidCP.Portal.OdbcSources" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="scp" %>

<scp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="OS"
    TypeName="SolidCP.Providers.OS.SystemDSN, SolidCP.Providers.Base"
    QuotaName="OS.ODBC" />