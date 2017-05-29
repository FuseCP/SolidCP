<h3>Sync automation</h3>
{if $settings.SyncActive eq 0}
    <div id="servicecontent_sync">
        <div class="errorbox">{$LANG.SolidCP_syncnotactive}</div>
    </div>
{else}
    <div id="servicecontent_sync"></div>
    <p>{$LANG.SolidCP_sync_nosetting}</p>
{/if}
