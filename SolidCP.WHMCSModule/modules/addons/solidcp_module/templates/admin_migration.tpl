<script type="text/javascript">
    function runMigrationCommand(cmd,value1,value2,option) {
        // Hide the modal that was activated.
        $("[id^=modal]").modal("hide");
        $("#migrationworking").modal('show');

        var reqstr = "module=solidcp_module&action=migration&command="+cmd+"&value1="+value1+"&value2="+value2+"&option="+option+"&ajax=1";

        $.post("addonmodules.php", reqstr,
        function(data){
            if (data.substr(0,9)=="redirect|") {
                window.location = data.substr(9);
            } else if (data.substr(0,7)=="window|") {
                window.open(data.substr(7), '_blank');
                $("#migrationworking").modal('hide');
            } else {
                $("#servicecontent_migration").html(data);
                $("#migrationworking").modal('hide');
            }

        });

    }
</script>
<div id="migrationworking" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">{$LANG.SolidCP_migrationrunning}</h4>
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
<h3>{$LANG.SolidCP_migration_needed}</h3>
<div id="servicecontent_migration"></div>
{foreach from=$migrationsteps item=migrationstep}
    {if $migrationstep.command eq 'migrateDbValues'}
        <div class="modal fade" id="modal{$migrationstep.command}_{$migrationstep.value1}_migrate" tabindex="-1" role="dialog" aria-labelledby="{$migrationstep.command}_{$migrationstep.value1}_migrateLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content panel panel-primary">
                    <div id="modal{$migrationstep.command}_{$migrationstep.value1}_migrateHeading" class="modal-header panel-heading">
                        <button type="button" class="close" data-dismiss="modal">
                            <span aria-hidden="true">&times;</span>
                            <span class="sr-only">{$LANG.SolidCP_close}</span>
                        </button>
                        <h4 class="modal-title" id="{$migrationstep.command}_{$migrationstep.value1}_migrateLabel">{$LANG.SolidCP_confirmmigrate}</h4>
                    </div>
                    <div id="modal{$migrationstep.command}_{$migrationstep.value1}_migrateBody" class="modal-body panel-body">
                        {$LANG.SolidCP_confirmmigrate_long}
                    </div>
                    <div id="modal{$migrationstep.command}_{$migrationstep.value1}_migrateFooter" class="modal-footer panel-footer">
                        <button type='button' id='{$migrationstep.command}_{$migrationstep.value1}_migrate-{$LANG.SolidCP_yes}' class='btn btn-primary' onclick='runMigrationCommand("{$migrationstep.command}","{$migrationstep.value1}","{$migrationstep.value2}", "migrate");'>
                            {$LANG.SolidCP_yes}
                        </button>
                        <button type='button' id='{$migrationstep.command}_{$migrationstep.value1}_migrate-{$LANG.SolidCP_no}' class='btn btn-default' data-dismiss="modal">
                            {$LANG.SolidCP_no}
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div class="alert alert-warning" id="box_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}">
            <div class='container-fluid'>
                <div class="row">
                    <div class="col-xs-12 col-sm-9">
                        <span class="fa fa-warning" id="icon_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}"></span>
                        {$LANG.SolidCP_migrateDbValues_text|sprintf:$migrationstep.value2:$migrationstep.value1}
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <div class="btn-group pull-right">
                            <button type="button" class="btn btn-warning dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="button_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}">
                                {$LANG.SolidCP_action} <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a href="#" onclick="jQuery('#modal{$migrationstep.command}_{$migrationstep.value1}_migrate').modal('show');return false;">{$LANG.SolidCP_migratedbvalues|sprintf:$migrationstep.value1}</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    {elseif $migrationstep.command eq 'migrateTable'}
        <div class="modal fade" id="modal{$migrationstep.command}_{$migrationstep.value1}_delete" tabindex="-1" role="dialog" aria-labelledby="{$migrationstep.command}_{$migrationstep.value1}_deleteLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content panel panel-primary">
                    <div id="modal{$migrationstep.command}_{$migrationstep.value1}_deleteHeading" class="modal-header panel-heading">
                        <button type="button" class="close" data-dismiss="modal">
                            <span aria-hidden="true">&times;</span>
                            <span class="sr-only">{$LANG.SolidCP_close}</span>
                        </button>
                        <h4 class="modal-title" id="{$migrationstep.command}_{$migrationstep.value1}_deleteLabel">{$LANG.SolidCP_confirmdelete}</h4>
                    </div>
                    <div id="modal{$migrationstep.command}_{$migrationstep.value1}_deleteBody" class="modal-body panel-body">
                        {$LANG.SolidCP_confirmdelete_long}
                    </div>
                    <div id="modal{$migrationstep.command}_{$migrationstep.value1}_deleteFooter" class="modal-footer panel-footer">
                        <button type='button' id='{$migrationstep.command}_{$migrationstep.value1}_delete-{$LANG.SolidCP_yes}' class='btn btn-primary' onclick='runMigrationCommand("{$migrationstep.command}","{$migrationstep.value1}","{$migrationstep.value2}", "delete");'>
                            {$LANG.SolidCP_yes}
                        </button>
                        <button type='button' id='{$migrationstep.command}_{$migrationstep.value1}_delete-{$LANG.SolidCP_no}' class='btn btn-default' data-dismiss="modal">
                            {$LANG.SolidCP_no}
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div class="alert alert-warning" id="box_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}">
            <div class='container-fluid'>
                <div class="row">
                    <div class="col-xs-12 col-sm-9">
                        <span class="fa fa-warning" id="icon_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}"></span>
                        {$LANG.SolidCP_migrateTable_text|sprintf:$migrationstep.value1:$migrationstep.value2}
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <div class="btn-group pull-right">
                            <button type="button" class="btn btn-warning dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="button_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}">
                                {$LANG.SolidCP_action} <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a href="#" onclick="runMigrationCommand('{$migrationstep.command}','{$migrationstep.value1}','{$migrationstep.value2}', 'copy');return false;">{$LANG.SolidCP_copytable|sprintf:$migrationstep.value1:$migrationstep.value2}</a></li>
                                <li role="separator" class="divider"></li>
                                <li><a href="#" onclick="jQuery('#modal{$migrationstep.command}_{$migrationstep.value1}_delete').modal('show');return false;">{$LANG.SolidCP_deletetable|sprintf:$migrationstep.value1}</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    {elseif $migrationstep.command eq 'deactivateModules'}
        <div class="alert alert-warning" id="box_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}">
            <div class='container-fluid'>
                <div class="row">
                    <div class="col-xs-12 col-sm-9">
                        <span class="fa fa-warning" id="icon_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}"></span>
                        {$LANG.SolidCP_deactivateModules_text|sprintf:$migrationstep.value1}
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <div class="btn-group pull-right">
                            <button type="button" class="btn btn-warning dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="button_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}">
                                {$LANG.SolidCP_action} <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a href="configaddonmods.php#{$migrationstep.value1}" target="_blank">{$LANG.SolidCP_deactivatemodule|sprintf:$migrationstep.value1}</a></li>
                                <li role="separator" class="divider"></li>
                                <li><a href="#" onclick="runMigrationCommand('{$migrationstep.command}','{$migrationstep.value1}','{$migrationstep.value2}', 'deactivate');return false;">{$LANG.SolidCP_checkagain}</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    {elseif $migrationstep.command eq 'deleteFiles'}
        <div class="alert alert-danger" id="box_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}">
            <div class='container-fluid'>
                <div class="row">
                    <div class="col-xs-12 col-sm-12">
                        <span class="fa fa-minus-circle" id="icon_{$migrationstep.command}_{$migrationstep.value1}_{$migrationstep.value2}"></span>
                        {$LANG.SolidCP_deleteFiles_text|sprintf:$migrationstep.value1}
                    </div>
                </div>
            </div>
        </div>
    {/if}
{/foreach}
<div class="btn-container">
    <button class='btn btn-primary' onclick="location.href='{$SCRIPT_NAME}?module=solidcp_module';">{$LANG.SolidCP_checkagain}</button>
</div>