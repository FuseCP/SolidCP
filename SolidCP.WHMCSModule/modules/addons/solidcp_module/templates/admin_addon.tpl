<h3>Addon automation</h3>
{if $settings.AddonsActive eq 0}
    <div id="servicecontent_addon">
        <div class="errorbox">{$LANG.SolidCP_addonsnotactive}</div>
    </div>
{else}
    <script type="text/javascript">
    function runAddonSaveCommand() {
			var scp_id_input = document.getElementById("addon_scp_id");
			var scp_id = parseInt(scp_id_input.value);
			if (isNaN(scp_id) || scp_id <= 0 || scp_id > 2147483647){
				alert("Invalid SolidCP-ID.");
				scp_id_input.focus();
				return false;
			}
            // Hide the modal that was activated.
            $("[id^=modal]").modal("hide");

            var form = $(addon_form);
            var reqstr = form.serialize();
            reqstr = reqstr+'&ajax=1';

            $.post("addonmodules.php?module=solidcp_module", reqstr,
            function(data){
                if (data.substr(0,9)=="redirect|") {
                    window.location = data.substr(9);
                } else if (data.substr(0,7)=="window|") {
                    window.open(data.substr(7), '_blank');
                } else {
                    $("#addon").html(data);
					addonSearch(0, 2);
                }

            });
        }
    function runAddonDeleteCommand(whmcs_id) {
            // Hide the modal that was activated.
            $("[id^=modal]").modal("hide");
            if(confirm('Are you sure you want to delete this Addon?')){
				var searchAddon = document.getElementById("searchAddon").value;
				var onlyAssigned = document.getElementById("showOnlyAssignedAddon").checked;
                var reqstr = "module=solidcp_module&action=delete_addon&id="+whmcs_id+"&ajax=1&searchAddon="+searchAddon;
				if (onlyAssigned) reqstr += "&showOnlyAssignedAddon=1";

                $.post("addonmodules.php", reqstr,
                function(data){
                    if (data.substr(0,9)=="redirect|") {
                        window.location = data.substr(9);
                    } else if (data.substr(0,7)=="window|") {
                        window.open(data.substr(7), '_blank');
                    } else {
                        $("#addon").html(data);
						addonSearch(0, 2);
                    }

                });
            }
        }
		
		function openAddonEditDialog(whmcs_id, scp_id, is_ipaddress){
			document.getElementById("addon_whmcs_id").value = whmcs_id;
			if (scp_id){
				document.getElementById("addon_scp_id").value = scp_id;
			}else{
				document.getElementById("addon_scp_id").value = "";
			}
			if (is_ipaddress == 1){
				document.getElementById("addon_is_ipaddress").checked = true;
			}else{
				document.getElementById("addon_is_ipaddress").checked = false;
			}
			$('#modal_addon_edit_form').modal('show');
		}
		
		function addonSearch(colName, colId) {
			var input, filter, table, tr, td, i, l, txt;
			var onlyAssigned = document.getElementById("showOnlyAssignedAddon").checked;
			input = document.getElementById("searchAddon");
			filter = input.value.toUpperCase();
			table = document.getElementById("addon_table");
			tr = table.getElementsByTagName("tr");
			l = tr.length;
			for (i = 1; i < l; i++) {
				td = tr[i].getElementsByTagName("td");
				txt = td[colName].textContent + td[colId].textContent;
				if (txt.toUpperCase().includes(filter) && (!onlyAssigned || td[colId].textContent)) {
					tr[i].style.display = "";
				}else{
					tr[i].style.display = "none";
				}
			}
		}
    </script>
<form class="form" role="form" id="addon_form" action="{$params['modulelink']}" method="POST">
    <div class="modal fade" id="modal_addon_edit_form" tabindex="-1" role="dialog" aria-labelledby="modal_addon_edit_form_Label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content panel panel-primary">
                <div class="modal-header panel-heading">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span>
                        <span class="sr-only">{$LANG.SolidCP_close}</span>
                    </button>
                    <h4 class="modal-title" id="modal_addon_edit_form_Label">{$LANG.SolidCP_edit_addon_option}</h4>
                </div>
                <div class="modal-body panel-body">
                        <input type="hidden" name="action" value="edit_addon">
                        <input type="hidden" name="module" value="solidcp_module">
						<input type="hidden" id="addon_whmcs_id" name="whmcs_id" value="" />
                        <div class="form-group"><label>{$LANG.SolidCP_solidcp_id}:</label><input type="text" id="addon_scp_id" name="scp_id" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_solidcp_id_tooltip}" /></div>
                        <div class="form-group"><label><input type="checkbox" id="addon_is_ipaddress" name="is_ipaddress" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_is_ip_address_tooltip}" /> {$LANG.SolidCP_is_ip_address}</label></div>
                </div>
                <div class="modal-footer panel-footer">
                    <input type="submit" onclick="runAddonSaveCommand();return false;" value="{$LANG.SolidCP_save_changes}" class="btn btn-primary">
                    <button type='button' class='btn btn-default' data-dismiss="modal">
                        {$LANG.SolidCP_cancel}
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="servicecontent_addon">
        {if $result.status == "error"}
            <div class="errorbox">{$result.description}</div>
        {/if}
		{if $result.status == "success"}
            <div class="successbox">{$result.description}</div>
        {/if}
    </div>
	
	<div class="panel-heading clearfix" style="background-color: lightgray;">
		<div class="form-group">
			<input type="text" id="searchAddon" name="searchAddon" value="{$searchAddon}" onkeyup="addonSearch(0, 2)" onclick="addonSearch(0, 2)" placeholder="{$LANG.SolidCP_search_addon}" style="width: 500px;"/>
		</div>
		<div class="form-group">
			<label><input type="checkbox" id="showOnlyAssignedAddon" name="showOnlyAssignedAddon" value="1" {if $showOnlyAssignedAddon}checked{/if} onclick="addonSearch(0, 2)" /> {$LANG.SolidCP_show_only_assigned_addons}</label>
		</div>
	</div>
	
    <div class="tablebg">
        <table class="table table-striped" width="100%" border="0" cellspacing="1" cellpadding="3" id="addon_table">
            <tr>
				<th>{$LANG.SolidCP_addonname}</th>
				<th>{$LANG.SolidCP_whmcs_id}</th>
				<th>{$LANG.SolidCP_solidcp_id}</th>
				<th>{$LANG.SolidCP_hidden}</th>
				<th width="20"></th>
				<th width="20"></th>
			</tr>
            {foreach key=num from=$addonautomation item=addon}
                <tr>
					<td>{$addon->name}</td>
					<td>{$addon->whmcs_id}</td>
					<td>{$addon->scp_id}</td>
					<td>{$addon->hidden}</td>
					<td><a href="#" onclick="openAddonEditDialog('{$addon->whmcs_id}', '{$addon->scp_id}', '{$addon->is_ipaddress}'); return false;" title="{$LANG.SolidCP_edit}"><i class="fas fa-pencil-alt"></i></a></td>
					<td>
						{if $addon->scp_id}
						<a href="{$params['modulelink']}&action=addon_delete&id={$addon->whmcs_id}" onclick="runAddonDeleteCommand('{$addon->whmcs_id}'); return false;" title="{$LANG.SolidCP_delete}"><i class="fa fa-times textred"></i></a>
						{/if}
					</td>
            {foreachelse}
                <tr><td colspan="5">{$LANG.SolidCP_norecordsfound}</td></tr>
            {/foreach}
        </table>

    </div>
</form>
{/if}
