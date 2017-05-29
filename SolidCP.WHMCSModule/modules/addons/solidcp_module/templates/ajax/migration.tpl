{if $result.status eq "error"}
    <div class="errorbox">
        {$result.description}
    </div>
{elseif $result.status eq "success"}
    <script type='text/javascript'>
    $('#{$html_ids.box}').removeClass('alert-warning');
    $('#{$html_ids.box}').addClass('alert-success');
    $('#{$html_ids.button}').removeClass('btn-warning');
    $('#{$html_ids.button}').addClass('btn-success');
    $('#{$html_ids.button}').attr('disabled',true);
    $('#{$html_ids.button}').addClass('disabled');
    $('#{$html_ids.button}').html('SUCCESS');
    $('#{$html_ids.icon}').removeClass('fa-warning');
    $('#{$html_ids.icon}').addClass('fa-check-circle');

    </script>
{/if}
