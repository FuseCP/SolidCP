<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceEditDnsRecords.ascx.cs" Inherits="SolidCP.Portal.SpaceEditDnsRecords" %>
<%@ Register Src="GlobalDnsRecordsControl.ascx" TagName="GlobalDnsRecordsControl" TagPrefix="uc1" %>

<div class="panel-body form-horizontal">
    <uc1:GlobalDnsRecordsControl ID="GlobalDnsRecordsControl1" runat="server" PackageIdParam="SpaceID" />
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnBack" CssClass="btn btn-warning" runat="server" OnClick="btnBack_Click" CausesValidation="false"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBackText"/> </CPCC:StyleButton>
</div>