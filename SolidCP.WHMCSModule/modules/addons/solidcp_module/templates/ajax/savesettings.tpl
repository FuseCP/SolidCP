{if $result.status eq "error"}
    <div class="errorbox">
        {$result.description}
    </div>
{elseif $result.status eq "success"}
    <div class="successbox">
        {$result.description}
    </div>
{/if}
