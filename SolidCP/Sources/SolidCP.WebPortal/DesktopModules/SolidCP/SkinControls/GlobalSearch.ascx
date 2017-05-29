<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalSearch.ascx.cs" Inherits="SolidCP.Portal.SkinControls.GlobalSearch" %>

<style>
    .ui-menu-item a {white-space: nowrap; }
</style>

<script type="text/javascript">
    //<![CDATA[
    $("#<%= tbSearch.ClientID %>").keypress(function (e) {
        if (e.keyCode != 13) { // VK_RETURN
            $("#<%= tbSearchText.ClientID %>").val('');
            $("#<%= tbObjectId.ClientID %>").val('');
            $("#<%= tbPackageId.ClientID %>").val('');
            $("#<%= tbAccountId.ClientID %>").val('');
        }
    });

    $(document).ready(function () {
        $("#<%= tbSearch.ClientID %>").autocomplete({
            zIndex: 100,
            source: function(request, response) {
                $.ajax({
                    type: "post",
                    dataType: "json",
                    data: {
                        term: request.term
                    },
                    url: "AjaxHandler.ashx",
                    success: function(data)
                    {
                        response($.map(data, function (item) {
                            return {
                                label: item.TextSearch + " [" + item.FullTypeLocalized + "]",
                                code: item
                            };
                        }));
                    }
                })
            },
            select: function (event, ui) {
                var item = ui.item;
                $("#<%= tbSearchColumnType.ClientID %>").val(item.code.ColumnType);
                $("#<%= tbSearchFullType.ClientID %>").val(item.code.FullType);
                $("#<%= tbSearchText.ClientID %>").val(item.code.TextSearch);
                $("#<%= tbObjectId.ClientID %>").val(item.code.ItemID);
                $("#<%= tbPackageId.ClientID %>").val(item.code.PackageID);
                $("#<%= tbAccountId.ClientID %>").val(item.code.AccountID);
                var $ImgBtn = $("#<%= ImageButton1.ClientID %>");
                $ImgBtn.trigger("click");
                $ImgBtn.attr('disabled', 'disabled');
            }
        });
        if (document.referrer.search("pid=Login") > 0 || window.location.href.search("pid=SearchObject") > 0) {
            $("#<%= tbSearch.ClientID %>").focus();
        }

    });//]]>
</script>

<asp:Panel runat="server" ID="updatePanelUsers" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="SearchQuery navbar-form navbar-right" DefaultButton="ImageButton1">


                    <div class="input-group">
                        <asp:TextBox
                            ID="tbSearch"
                            runat="server"
                            CssClass="form-control"
                            style="vertical-align: middle; z-index: 100;"
                        >
                        </asp:TextBox>
                        <asp:TextBox
                            ID="tbSearchColumnType"
                            runat="server"
                            type="hidden"
                        >
                        </asp:TextBox>
                        <asp:TextBox
                            ID="tbSearchFullType"
                            runat="server"
                            type="hidden"
                        >
                        </asp:TextBox>
                        <asp:TextBox
                            ID="tbSearchText"
                            runat="server"
                            type="hidden"
                        >
                        </asp:TextBox>
                        <asp:TextBox
                            ID="tbObjectId"
                            runat="server"
                            type="hidden"
                        >
                        </asp:TextBox>
                        <asp:TextBox
                            ID="tbPackageId"
                            runat="server"
                            type="hidden"
                        >
                        </asp:TextBox>
                        <asp:TextBox
                            ID="tbAccountId"
                            runat="server"
                            type="hidden"
                        >
                        </asp:TextBox>
                        <div class="input-group-btn">
                            <CPCC:StyleButton
                            ID="ImageButton1"
                            runat="server"
                            SkinID="SearchButton"
                            OnClick="btnSearchObject_Click"
                            CausesValidation="false"
                            CssClass="btn btn-primary"
                        >
                                <i class="fa fa-search" aria-hidden="true"></i>
                            </CPCC:StyleButton>
                        </div>             
                    </div>
</asp:Panel>
