<script type="text/javascript">
    function runSaveSettingsCommand() {
        // Hide the modal that was activated.
        $("[id^=modal]").modal("hide");
        $("#saveworking").modal('show');
        var form = $(settings_form);
        var reqstr   = form.serialize();
        reqstr = reqstr+'&ajax=1';
        
        $.post("addonmodules.php", reqstr,
        function(data){
            if (data.substr(0,9)=="redirect|") {
                window.location = data.substr(9);
            } else if (data.substr(0,7)=="window|") {
                window.open(data.substr(7), '_blank');
                $("#saveworking").modal('hide');
            } else {
                $("#servicecontent_settings").html(data);
                $("#saveworking").modal('hide');
            }

        });
    }
</script>
<div id="saveworking" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">{$LANG.SolidCP_saverunning}</h4>
            </div>
            <div class="modal-body">
            <div class="progress">
                <div class="progress-bar progress-bar-striped active" role="progressbar" style="width:98%">
                </div>
            </div>
            </div>
        </div>
    </div>
</div>
<h3>{$LANG.SolidCP_general_settings}</h3>
<div id="servicecontent_settings">
    {if $settings.NeedFirstConfiguration eq 1}<div class="errorbox">{$LANG.SolidCP_needfirstconfiguration}</div>{/if}
    {if $result.status eq "error"}<div class="errorbox">{$result.description}</div>{/if}
</div>
<form class="form-horizontal" role="form" id="settings_form" action="{$params['modulelink']}" method="POST">
    <input type="hidden" name="action" value="save_settings">
    <input type="hidden" name="module" value="solidcp_module">
    <table class="form" width="100%" border="0" cellspacing="2" cellpadding="3">
        <tr>
            <td class="fieldlabel">{$LANG.SolidCP_setting_AddonsActive}:</td>
            <td class="fieldarea"><input type="checkbox" name="AddonsActive" {if $settings.AddonsActive eq 1} checked {/if}data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_setting_AddonsActive_tooltip}" /></td>
            <td class="fieldlabel">{$LANG.SolidCP_setting_ConfigurableOptionsActive}:</td>
            <td class="fieldarea"><input type="checkbox" name="ConfigurableOptionsActive" {if $settings.ConfigurableOptionsActive eq 1} checked {/if}data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_setting_ConfigurableOptionsActive_tooltip}" /></td>
        </tr>
        <tr>
            <td class="fieldlabel">{$LANG.SolidCP_setting_SyncActive}:</td>
            <td class="fieldarea"><input type="checkbox" name="SyncActive" {if $settings.SyncActive eq 1} checked {/if}data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_setting_SyncActive_tooltip}" /></td>
            <td class="fieldlabel">{$LANG.SolidCP_setting_DeleteTablesOnDeactivate}:</td>
            <td class="fieldarea"><input type="checkbox" name="DeleteTablesOnDeactivate" {if $settings.DeleteTablesOnDeactivate eq 1} checked {/if}data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_setting_DeleteTablesOnDeactivate_tooltip}" /></td>
        </tr>
        <tr>
            <td class="fieldlabel">{$LANG.SolidCP_setting_WhmcsAdmin}:</td>
            <td class="fieldarea"><select class="form-control" name="WhmcsAdmin" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_setting_WhmcsAdmin_tooltip}">
            {foreach from=$admins item=admin}
                <option id="{$admin->username}"{if $settings.WhmcsAdmin eq $admin->username} selected {/if}>{$admin->username}</option>
            {/foreach}    
            </select></td>
            <td class="fieldlabel">&nbsp;</td>
            <td class="fieldarea">&nbsp;</td>
        </tr>
    </table>
    <div class="btn-container">
        <input type="submit" onclick="runSaveSettingsCommand();return false;" value="{$LANG.SolidCP_save_changes}" class="btn btn-primary">
        <input type="reset" value="{$LANG.SolidCP_cancel_changes}" class="btn btn-default" />
    </div>
    <input type="hidden" name="tab" id="tab" value="0" />
</form>