<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGallery.ascx.cs" Inherits="SolidCP.Portal.WebApplicationGallery" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="uc1" %>
<%@ Import Namespace="SolidCP.Portal" %>

<style>
ul.WPIKeywordList {
    padding: 10px 0 0 0;
}
ul.WPIKeywordList>li {
    padding: .4em .2em;
    margin-right: .4em;
    display: inline;
    list-style-type: none;
    line-height: 200%;
}
ul.WPIKeywordList li span,
ul.WPIKeywordList li label {
    padding: .2em;
    white-space: nowrap;
}
ul.WPIKeywordList li .selected 
{
    /*padding: .4em .4em;*/
    background-color: #E5F2FF;
    border: solid 1px #86B9F7;
    color: #000;
}
ul.WPIKeywordList input {
    display: none;
}

ul.WPIKeywordList label {
    padding: 0 .1em;
    cursor: pointer;
    text-decoration: underline;
    text-decoration-color: #888;
}
ul.WPIKeywordList li .selected label {
    text-decoration: none;
}
.AspNet-GridView-Pagination {
    font-size: 10pt !important;
}
</style>
<!--[if lt IE 9]>
<style>
ul.WPIKeywordList input {
    display: inline;
}
ul.WPIKeywordList>li {
    padding: .1em .2em;
}
</style>
<![endif]-->
<script type="text/javascript">
    Sys.Application.add_init(function () {
        $('.ProductsSearchInput').focus();
        $('.ProductsSearchInput').live("keypress", function (e) {
            /* ENTER PRESSED*/
            if (e.keyCode == 13) {
                e.stopPropagation();
                e.preventDefault();

                $('.ProductsSearchButton')[0].click();

                return false;
            }
        });
    });
</script>


<asp:Panel ID="WebApplicationGalleryPanel" runat="server" DefaultButton="ApplicationSearchGo">
    <div class="panel-body form-horizontal">
        <div class="row">
	        <div class="col-md-9">
                <div class="input-group">
                    <asp:RadioButtonList ID="rbsCategory" runat="server" 
                        RepeatLayout="UnorderedList"
                        CssClass="WPIKeywordList panel-body form-horizontal"
                        DataValueField="Id" 
                        DataTextField="Name" 
                        AutoPostBack="True" 
                        OnSelectedIndexChanged="CategorySelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
            </div>
	        <div class="col-md-3">
                <div class="form-group">
                    <div class="input-group">
                        <asp:TextBox ID="searchBox" runat="server" CssClass="form-control" ></asp:TextBox>
                        <div class="input-group-btn">
                            <CPCC:StyleButton ID="ApplicationSearchGo" runat="server" CssClass="btn btn-primary" OnClick="SearchButton_Click">
                                <i class="fa fa-search" aria-hidden="true"></i>
                            </CPCC:StyleButton>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <asp:DropDownList ID="dropDownLanguages" runat="server" CssClass="form-control" onselectedindexchanged="dropDownLanguages_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<div class="NormalGridView">
    <uc1:SimpleMessageBox ID="messageBox" runat="server" />

    <%--            <asp:RadioButtonList ID="keywordsList" runat="server" 
                        RepeatLayout="UnorderedList" CssClass="WPIKeywordList panel-body form-horizontal" 
                        onselectedindexchanged="keywordsList_SelectedIndexChanged" 
                        AutoPostBack="True">
                </asp:RadioButtonList>--%>

    <asp:GridView id="gvApplications" runat="server" AutoGenerateColumns="False" AllowPaging="true" 
	    ShowHeader="false" CssSelectorClass="LightGridView" EmptyDataText="gvApplications" OnRowCommand="gvApplications_RowCommand" 
	    OnPageIndexChanging="gvApplications_PageIndexChanging">
	    <Columns>
		    <asp:TemplateField HeaderText="gvApplicationsApplication">
		        <ItemStyle HorizontalAlign="Center" />
			    <ItemTemplate>
				    <div style="text-align:center;">
					    <asp:hyperlink NavigateUrl='<%# EditUrl("ApplicationID", Eval("Id").ToString(), "edit", "SpaceID=" + PanelSecurity.PackageId.ToString()) %>'
							    runat="server" ID="Hyperlink3" ToolTip='<%# Eval("Title") %>'>
						    <asp:Image runat="server" ID="Image1" Width="120" Height="120"
                                ImageUrl='<%# GetIconUrlOrDefault((string)Eval("IconUrl"))  %>'
							    AlternateText='<%# Eval("Title") %>'>
						    </asp:Image>
					    </asp:hyperlink>
				    </div>
			    </ItemTemplate>
		    </asp:TemplateField>
		    <asp:TemplateField>
			    <ItemTemplate>
				    <div class="MediumBold" style="padding:4px;">
					    <asp:hyperlink CssClass="MediumBold" NavigateUrl='<%# EditUrl("ApplicationID", Eval("Id").ToString(), "edit", "SpaceID=" + PanelSecurity.PackageId.ToString()) %>'
					        runat="server" ID="lnkAppDetails" ToolTip='<%# Eval("Title") %>'>
						    <%# Eval("Title")%>
					    </asp:hyperlink>
				    </div>
				    <div class="Normal" style="padding:4px;">
					    <%# Eval("Summary") %>
				    </div>				
			    </ItemTemplate>
		    </asp:TemplateField>
		    <asp:TemplateField HeaderText="gvApplicationsApplication">
		        <ItemStyle HorizontalAlign="Center" />
			    <ItemTemplate>
			        <CPCC:StyleButton id="btnInstall" CssClass="btn btn-success" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Install">
                        <i class="fa fa-check">&nbsp;</i>&nbsp;
                        <asp:Localize runat="server" meta:resourcekey="btnInstallText"/>
			        </CPCC:StyleButton>
			    </ItemTemplate>
		    </asp:TemplateField>
	    </Columns>
    </asp:GridView>
</div>