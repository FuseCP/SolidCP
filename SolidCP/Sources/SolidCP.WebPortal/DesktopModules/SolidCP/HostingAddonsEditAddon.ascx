<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingAddonsEditAddon.ascx.cs" Inherits="SolidCP.Portal.HostingAddonsEditAddon" %>
<%@ Register Src="HostingPlansQuotas.ascx" TagName="HostingPlansQuotas" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<asp:Panel ID="HostingAddonsEditPanel" runat="server" DefaultButton="btnSave" >
    <div class="panel-body form-horizontal">
        <asp:UpdatePanel runat="server" ID="updatePanelUsers">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold" ForeColor="red"></asp:Label>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtPlanName">
                        <asp:Localize ID="lblAddOnName" runat="server" meta:resourcekey="lblAddOnName" Text="Add-On Name:"></asp:Localize>
                    </asp:Label>
                    <div class="col-sm-10">
                        <div class="input-group col-sm-12">
                            <asp:TextBox ID="txtPlanName" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valPlanName" runat="server" ControlToValidate="txtPlanName" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtPlanDescription">
                        <asp:Localize ID="lblAddOnDescription" runat="server" meta:resourcekey="lblAddOnDescription" Text="Add-On Description:"></asp:Localize>
                    </asp:Label>
                    <div class="col-sm-10">
                        <div class="input-group col-sm-12">
                                <asp:TextBox ID="txtPlanDescription" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <scp:CollapsiblePanel id="secQuotas" runat="server" TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas"></scp:CollapsiblePanel>
                <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
                    <uc1:HostingPlansQuotas id="hostingPlansQuotas" runat="server" IsPlan="false"></uc1:HostingPlansQuotas>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click">
            <i class="fa fa-trash-o">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnDeleteText"/>
        </CPCC:StyleButton>
        &nbsp;
        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
            <i class="fa fa-times">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnCancelText"/>
        </CPCC:StyleButton>
        &nbsp;
        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click">
            <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnSaveText"/>
        </CPCC:StyleButton>
    </div>
</asp:Panel>