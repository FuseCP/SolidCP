<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplyEnableHardQuotaFeature.ascx.cs" Inherits="SolidCP.Portal.ApplyEnableHardQuotaFeature" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="messageBox" TagPrefix="uc4" %>

<div class="panel-body form-horizontal">
    <table cellspacing="0" cellpadding="4" width="100%">
        <tr>

            <td class="Normal">
                <uc4:messageBox id="messageBox" runat="server" >                   
                </uc4:messageBox>
            </td>
        </tr>
    </table>
    
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/> </CPCC:StyleButton>
</div>
