<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBox.ascx.cs" Inherits="SolidCP.Portal.SearchBox" %>

<script type="text/javascript">
    //<![CDATA[
    $(document).ready(function () {
        $("#tbSearch").keypress(function (e) {
            if (e.keyCode != 13) { // VK_RETURN
                $("#tbSearchText").val('');
                $("#tbObjectId").val('');
                $("#tbPackageId").val('');
                $("#tbAccountId").val('');
            }
        });

        $("#tbSearch").autocomplete({
            zIndex: 0,
            source: function (request, response) {
                $.ajax({
                    type: "post",
                    dataType: "json",
                    data: {
                        fullType: "TableSearch",
                        FilterValue: request.term,
                        FilterColumns: "<%= GetCriterias() %>",
                        <%= AjaxData %>
                    },
                    url: "AjaxHandler.ashx",
                    success: function (data) {
                        response($.map(data, function (item) {
                            var type = $('#<%= ddlFilterColumn.ClientID %> option[value="' + item.ColumnType + '"]').text();
                            if (type == null) {
                                type = item.ColumnType;
                            }
                            $('#<%= ddlFilterColumn.ClientID %> :selected').removeAttr('selected')
                            return {
                                label: item.TextSearch + " [" + type + "]",
                                code: item
                            };
                        }));
                    }
                })
            },
            select: function (event, ui) {
                var item = ui.item;
                if (item.code.url != null)
                    window.location.href = item.code.url;
                else {
                    $("#ddlFilterColumn").val(item.code.ColumnType);
                    $("#tbSearchText").val(item.code.TextSearch);
                    $("#<%= cmdSearch.ClientID %>").trigger("click");
                }
            }
        });
    });//]]>
</script>

<asp:Panel ID="tblSearch" runat="server" DefaultButton="cmdSearch" CssClass="NormalBold">
<asp:Label ID="lblSearch" runat="server" meta:resourcekey="lblSearch" Visible="false"></asp:Label>

<div class="input-group">
                <asp:DropDownList ClientIDMode="Static" ID="ddlFilterColumn" runat="server" CssClass="form-control" resourcekey="ddlFilterColumn" style="display:none">
                </asp:DropDownList>

                                <asp:TextBox
                                    ID="tbSearch"
                                    ClientIDMode="Static"
                                    runat="server"
                                    CssClass="form-control"
                                >
                                </asp:TextBox>
                                <asp:TextBox
                                    ID="tbSearchFullType"
                                    ClientIDMode="Static"
                                    runat="server"
                                    type="hidden"
                                >
                                </asp:TextBox>
                                <asp:TextBox
                                    ID="tbSearchText"
                                    ClientIDMode="Static"
                                    runat="server"
                                    type="hidden"
                                >
                                </asp:TextBox>
                                <asp:TextBox
                                    ID="tbObjectId"
                                    ClientIDMode="Static"
                                    runat="server"
                                    type="hidden"
                                >
                                </asp:TextBox>
                                <asp:TextBox
                                    ID="tbPackageId"
                                    ClientIDMode="Static"
                                    runat="server"
                                    type="hidden"
                                >
                                </asp:TextBox>
                                <asp:TextBox
                                    ID="tbAccountId"
                                    ClientIDMode="Static"
                                    runat="server"
                                    type="hidden"
                                >
                                </asp:TextBox>
                                <div class="input-group-btn">
                                <CPCC:StyleButton
                                    ID="cmdSearch"
                                    runat="server"
                                    SkinID="SearchButton"
                                    CausesValidation="false"
                                    style="vertical-align: middle;"
                                    CssClass="btn btn-primary"
                                >
                                    <i class="fa fa-search" aria-hidden="true"></i>
                                </CPCC:StyleButton>      
                                    </div>                
                            </div>

</asp:Panel>
