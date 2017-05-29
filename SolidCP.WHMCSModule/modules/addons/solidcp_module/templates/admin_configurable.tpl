<h3>Configurable Options automation</h3>
{if $settings.ConfigurableOptionsActive eq 0}
    <div id="servicecontent_configurable">
        <div class="errorbox">{$LANG.SolidCP_configurablenotactive}</div>
    </div>
{else}
    <script type="text/javascript">
    function runConfigurableSaveCommand() {
            // Hide the modal that was activated.
            $("[id^=modal]").modal("hide");

            var form = $(configurable_form);
            var reqstr   = form.serialize();
            reqstr = reqstr+'&ajax=1';

            $.post("addonmodules.php?module=solidcp_module", reqstr,
            function(data){
                if (data.substr(0,9)=="redirect|") {
                    window.location = data.substr(9);
                } else if (data.substr(0,7)=="window|") {
                    window.open(data.substr(7), '_blank');
                } else {
                    $("#configurable").html(data);
                }

            });

        }
    function runConfigurableDeleteCommand(whmcs_id) {
            // Hide the modal that was activated.
            $("[id^=modal]").modal("hide");
            if(confirm('Are you sure you want to delete this configurable option?')){
                var reqstr = "module=solidcp_module&action=delete_configurable&id="+whmcs_id+"&ajax=1";

                $.post("addonmodules.php", reqstr,
                function(data){
                    if (data.substr(0,9)=="redirect|") {
                        window.location = data.substr(9);
                    } else if (data.substr(0,7)=="window|") {
                        window.open(data.substr(7), '_blank');
                    } else {
                        $("#configurable").html(data);
                    }

                });
            }
        }
    </script>
    <div class="modal fade" id="modal_configurable_add_form" tabindex="-1" role="dialog" aria-labelledby="configurable_add_form_Label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content panel panel-primary">
                <div id="modal_configurable_add_form_Heading" class="modal-header panel-heading">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span>
                        <span class="sr-only">{$LANG.SolidCP_close}</span>
                    </button>
                    <h4 class="modal-title" id="modal_configurable_add_form_Label">{$LANG.SolidCP_add_configurable_option}</h4>
                </div>
                <form class="form" role="form" id="configurable_form" action="{$params['modulelink']}" method="POST">
                <div id="modal_configurable_add_form_Body" class="modal-body panel-body">
                        <input type="hidden" name="action" value="add_configurable">
                        <input type="hidden" name="module" value="solidcp_module">
                        <div class="form-group" style="float: left; margin-right: 20px"><label>{$LANG.SolidCP_whmcs_id}:</label><input type="text" name="whmcs_id" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_whmcs_id_tooltip}" /></div>
                        <div class="form-group"><label>{$LANG.SolidCP_solidcp_id}:</label><input type="text" name="scp_id" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_solidcp_id_tooltip}" /></div>
                        <div class="form-group"><input type="checkbox" name="is_ipaddress" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="{$LANG.SolidCP_is_ip_address_tooltip}" /> {$LANG.SolidCP_is_ip_address}</div>
                </div>
                <div id="modal_configurable_add_form_Footer" class="modal-footer panel-footer">
                    <input type="submit" onclick="runConfigurableSaveCommand();return false;" value="{$LANG.SolidCP_add_new}" class="btn btn-primary">
                    <button type='button' id='configurable_add_form-{$LANG.SolidCP_no}' class='btn btn-default' data-dismiss="modal">
                        {$LANG.SolidCP_cancel}
                    </button>
                </div>
                </form>
            </div>
        </div>
    </div>

    <div id="servicecontent_configurable">
        {if $result.status == "error"}
            <div class="errorbox">{$result.description}</div>
        {/if}
    </div>
    <div class="btn-container">
        <button class='btn btn-primary' onclick="jQuery('#modal_configurable_add_form').modal('show');return false;">{$LANG.SolidCP_addassignment}</button>
    </div>
    <div class="tablebg">
        <table class="table table-striped" width="100%" border="0" cellspacing="1" cellpadding="3" id="configurable_table">
            <tr><th>{$LANG.SolidCP_configurableoptionname}</th><th>{$LANG.SolidCP_whmcs_id}</th><th>{$LANG.SolidCP_solidcp_id}</th><th>{$LANG.SolidCP_hidden}</th><th width="20"></th></tr>
            {foreach key=num from=$configurableoptions item=option}
                <tr><td>{$option->name}</td><td>{$option->whmcs_id}</td><td>{$option->scp_id}</td><td>{$option->hidden}</td><td><a href="{$params['modulelink']}&action=configurable_delete&id={$option->whmcs_id}" onclick="runConfigurableDeleteCommand('{$option->whmcs_id}'); return false;" title="{$LANG.SolidCP_delete}"><i class="fa fa-times textred"></i></a></td></tr>
            {foreachelse}
                <tr><td colspan="5">{$LANG.SolidCP_norecordsfound}</td></tr>
            {/foreach}
        </table>

    </div>
{/if}
