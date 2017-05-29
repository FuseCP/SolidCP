<script>
$(document).ready(function(){
    $('[data-toggle="popover"]').popover();   
});

    function reloadTabContent(arg) {
        // Hide the modal that was activated.
        $("[id^=modal]").modal("hide");
        var reqstr   = 'module=solidcp_module&action=load&area='+arg+'&ajax=1';
        $.post('addonmodules.php', reqstr ,function(data) {
            $(arg).html(data);
        });    
    }
</script>
<ul class="nav nav-tabs admin-tabs" role="tablist" id="solidcp_tabs">
    {if $settings.NeedMigration eq 1}
        <li class="active"><a data-toggle="tab" href="#migration" onclick="reloadTabContent('#migration')">{$LANG.SolidCP_migration}</a></li>
    {else}
    <li class="active"><a data-toggle="tab" href="#settings" onclick="reloadTabContent('#settings')">{$LANG.SolidCP_settings}</a></li>
    <li><a data-toggle="tab" href="#addon" onclick="reloadTabContent('#addon')">{$LANG.SolidCP_addon_automation}</a></li>
    <li><a data-toggle="tab" href="#configurable" onclick="reloadTabContent('#configurable')">{$LANG.SolidCP_configurable_options}</a></li>
    <li><a data-toggle="tab" href="#sync" onclick="reloadTabContent('#sync')">{$LANG.SolidCP_sync_automation}</a></li>
    {/if}
</ul>
<div class="tab-content admin-tabs">
    {if $settings.NeedMigration eq 1}
        <div id="migration" class="tab-pane fade in active">{include file='./admin_migration.tpl'}</div>
    {else}
        <div id="settings" class="tab-pane fade in active">{include file='./admin_settings.tpl'}</div>
        <div id="addon" class="tab-pane fade">{include file='./admin_addon.tpl'}</div>
        <div id="configurable" class="tab-pane fade">{include file='./admin_configurable.tpl'}</div>
        <div id="sync" class="tab-pane fade">{include file='./admin_sync.tpl'}</div>
    {/if}
</div>
<script type="text/javascript">
$('#solidcp_tabs a').click(function(e) {
  e.preventDefault();
  $(this).tab('show');
});

// store the currently selected tab in the hash value
$("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
  var id = $(e.target).attr("href").substr(1);
  window.location.hash = 'tab_'+id;
});

// on load of the page: switch to the currently selected tab
var hash = window.location.hash;
$('#solidcp_tabs a[href="' + hash.replace('tab_','') + '"]').tab('show');
</script>
