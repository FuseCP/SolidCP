<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlansEditPlan.ascx.cs" Inherits="SolidCP.Portal.HostingPlansEditPlan" %>
<%@ Register Src="HostingPlansQuotas.ascx" TagName="HostingPlansQuotas" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<asp:Panel ID="HostingPlansEditPanel" runat="server" DefaultButton="btnSave" >
    <asp:UpdatePanel runat="server" ID="updatePanelUsers" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate> 
            <div class="panel-body form-horizontal">
                <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold" ForeColor="red"></asp:Label>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtPlanName">
                        <asp:Localize ID="lblPlanName" runat="server" meta:resourcekey="lblPlanName" Text="Plan Name:"></asp:Localize>
                    </asp:Label>
                    <div class="col-sm-10">
                        <div class="input-group col-sm-12">
                            <asp:TextBox ID="txtPlanName" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valPlanName" runat="server" ErrorMessage="*" ControlToValidate="txtPlanName" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtPlanDescription">
                        <asp:Localize ID="lblPlanDescription" runat="server" meta:resourcekey="lblPlanDescription" Text="Plan Description:"></asp:Localize>
                    </asp:Label>
                    <div class="col-sm-10">
                        <div class="input-group col-sm-12">
                            <asp:TextBox ID="txtPlanDescription" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <scp:CollapsiblePanel id="secTarget" runat="server" TargetControlID="TargetPanel" meta:resourcekey="secTarget" Text="Plan Target"></scp:CollapsiblePanel>
                <asp:Panel ID="TargetPanel" runat="server" Height="0" style="overflow:hidden;">
                    <div class="form-group" id="rowTargetServer" runat="server">
                        <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="ddlServer">
                            <asp:Localize ID="lblTargetServer" runat="server" meta:resourcekey="lblTargetServer" Text="Server:"></asp:Localize>
                        </asp:Label>
                        <div class="col-sm-10">
                            <div class="input-group col-sm-12">
                                <asp:DropDownList ID="ddlServer" runat="server" CssClass="form-control" DataValueField="ServerID" DataTextField="ServerName" AutoPostBack="True" OnSelectedIndexChanged="planTarget_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="valRequireServer" runat="server" ControlToValidate="ddlServer" ErrorMessage="Select target server"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="rowTargetSpace" runat="server">
                        <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="ddlSpace">
                            <asp:Localize ID="lblTargetSpace" runat="server" meta:resourcekey="lblTargetSpace" Text="Hosting Space:"></asp:Localize>
                        </asp:Label>
                        <div class="col-sm-10">
                            <div class="input-group col-sm-12">
                                <asp:DropDownList ID="ddlSpace" runat="server" CssClass="form-control" DataValueField="PackageId" DataTextField="PackageName" AutoPostBack="True" OnSelectedIndexChanged="planTarget_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="valRequireSpace" runat="server" ControlToValidate="ddlSpace" ErrorMessage="Select target space"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <scp:CollapsiblePanel id="secQuotas" runat="server" TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas"></scp:CollapsiblePanel>
                <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
                    <uc1:HostingPlansQuotas id="hostingPlansQuotas" runat="server">
                    </uc1:HostingPlansQuotas>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete hosting plan?');">
            <i class="fa fa-trash-o">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnDeleteText"/>
        </CPCC:StyleButton>
        &nbsp;
        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
            <i class="fa fa-times">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
        </CPCC:StyleButton>
        &nbsp;
        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnSave"/>
        </CPCC:StyleButton>
    </div>
</asp:Panel>