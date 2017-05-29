<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactDetails.ascx.cs" Inherits="SolidCP.Portal.ContactDetails" %>
<div class="form-horizontal">
    <div class="form-group">
		<asp:Label ID="lblCompanyName" runat="server" meta:resourcekey="lblCompanyName" Text="Company Name:"  CssClass="col-sm-2"  AssociatedControlID="txtCompanyName"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtCompanyName" runat="server" Columns="40" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
		<asp:Label ID="lblAddress" runat="server" meta:resourcekey="lblAddress" Text="Address:" CssClass="col-sm-2"  AssociatedControlID="txtAddress"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtAddress" runat="server" Columns="40" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblCity" runat="server" meta:resourcekey="lblCity" Text="City:" CssClass="col-sm-2"  AssociatedControlID="txtCity"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblCountry" runat="server" meta:resourcekey="lblCountry" Text="Country:" CssClass="col-sm-2"  AssociatedControlID="ddlCountry"></asp:Label>
		<div class="col-sm-10">	
			<asp:dropdownlist runat="server" id="ddlCountry" CssClass="form-control" autopostback="True" width="200px" onselectedindexchanged="ddlCountry_SelectedIndexChanged" />
        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblState" runat="server" meta:resourcekey="lblState" Text="State:" CssClass="col-sm-2"  AssociatedControlID="txtState"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtState" runat="server" CssClass="form-control"></asp:TextBox>
			<asp:DropDownList ID="ddlStates" Runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control"
				Width="200px"></asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblZip" runat="server" meta:resourcekey="lblZip" Text="Zip:" CssClass="col-sm-2"  AssociatedControlID="txtZip"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtZip" runat="server" Columns="10" CssClass="form-control"></asp:TextBox>

        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblPrimaryPhone" runat="server" meta:resourcekey="lblPrimaryPhone" Text="Primary phone:" CssClass="col-sm-2"  AssociatedControlID="txtPrimaryPhone"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtPrimaryPhone" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblSecPhone" runat="server" meta:resourcekey="lblSecPhone" Text="Secondary phone:" CssClass="col-sm-2"  AssociatedControlID="txtSecondaryPhone"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtSecondaryPhone" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblFax" runat="server" meta:resourcekey="lblFax" Text="Fax:" CssClass="col-sm-2"  AssociatedControlID="txtFax"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtFax" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
			<asp:Label ID="lblIM" runat="server" meta:resourcekey="lblIM" Text="Instant messenger ID:" CssClass="col-sm-2"  AssociatedControlID="txtMessengerId"></asp:Label>
		<div class="col-sm-10">	
			<asp:TextBox id="txtMessengerId" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
</div>
