<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Generation.ascx.cs" Inherits="SolidCP.Portal.VPS2012.UserControls.Generation" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../../UserControls/CollapsiblePanel.ascx" %>

<% if (Mode != VirtualMachineSettingsMode.Summary){ %>
    <scp:CollapsiblePanel ID="secGeneration" runat="server" TargetControlID="GenerationPanel" meta:ResourceKey="secGeneration" Text="Generation"></scp:CollapsiblePanel>
    <asp:Panel ID="GenerationPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
<div class="form-group">
            <% if (Mode == VirtualMachineSettingsMode.Edit) { %>
                    <asp:Label ID="locGeneration" meta:resourcekey="locGeneration" runat="server" Text="Generation:" CssClass="col-sm-2" AssociatedControlID="ddlGeneration"></asp:Label>
                <div class="col-sm-10 form-inline">
                    <asp:DropDownList ID="ddlGeneration" runat="server" CssClass="form-control" resourcekey="ddlGeneration">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                    </asp:DropDownList>
                </div>
            <% } else { %>
            <asp:Label ID="locGenerationDisplay" meta:resourcekey="locGeneration" runat="server" Text="Generation:" CssClass="col-sm-2"></asp:Label>
                     <div class="col-sm-10 form-inline">
                    <asp:Label runat="server" ID="lblGeneration"/>
                    </div>
            <% } %>
        </div>
    </asp:Panel>
<% } else { %>
<div class="form-group">
    <asp:Label ID="locGeneration2" meta:resourcekey="locGeneration2" runat="server" Text="Generation:" CssClass="col-sm-2"></asp:Label>
    <div class="col-sm-10 form-inline">
    <asp:Literal ID="litGeneration" runat="server"></asp:Literal>
    </div>
    </div>
<% } %>