var _taskId = null;
var _showProgressDialog = false;
var _showCurrentStep = false;
var _showProgressBar = false;
var _showDialogButtons = false;
var _dialogTitle = "";
var _popupBehavior = null;
var updateTimerHandler = 0;
var checkCompletedTasksHandler = 0;
var tStart = null;
var timerStopped = true;
function UpdateProgressTimer() {
    if (!timerStopped) {
        var tDate = new Date();
        var tDiff = tDate.getTime() - tStart.getTime();
        tDate.setTime(tDiff);
        $get("progressDuration").innerHTML = PadNumber(Math.floor(tDiff / (1000 * 60 * 60))) + ":" + PadNumber(tDate.getMinutes()) + ":"
                                        + PadNumber(tDate.getSeconds());
        updateTimerHandler = window.setTimeout(UpdateProgressTimer, 1000);
    }
}
function StartProgressTimer() {
    timerStopped = false;
    tStart = new Date();
    $get("progressDuration").innerHTML = "00:00:00";
    updateTimerHandler = window.setTimeout(UpdateProgressTimer, 1000);
}
function StopTimer() {
    timerStopped = true;
}
function PadNumber(num) {
    return (num.toString().length == 1) ? "0" + num : num;
}
function EnableProgressDialog() {
    _showProgressDialog = true;
    //window.setInterval(DisableProgressDialog, 10); // disable dialog with some delay
}
function DisableProgressDialog() {
    _showProgressDialog = false;
}
function ShowProgressDialog(title, popupBehavior) {
    _dialogTitle = title;
    _popupBehavior = popupBehavior;
    EnableProgressDialog();
}
function CloseProgressDialog() {
    DisableProgressDialog();
    $find('ModalPopupProperties').hide();
}
function ShowProgressDialogWithCallback(title) {
    _dialogTitle = title;
    _showCurrentStep = true;
    _showProgressBar = true;
    EnableProgressDialog();
}
function ShowProgressDialogAsync(taskId, title) {
    _dialogTitle = title;
    _taskId = taskId;
    _showCurrentStep = true;
    _showProgressBar = true;
    _showDialogButtons = true;
    EnableProgressDialog();
}
function ShowProgressDialogInternal() {
    if (_showProgressDialog) {
        // close popup behavior
        if (_popupBehavior != null) {
            var popupCtrl = $find(_popupBehavior);
            if (popupCtrl != null)
                popupCtrl.hide();
        }

        _showProgressDialog = false; // reset field

        // set task id
        if (_taskId == null) {
            // get from control
            _taskId = $get(_ctrlTaskID).value;
        }
        // set dialog title
        $get("objProgressDialogTitle").innerHTML = _dialogTitle;

        $get("ProgressPanelArea").style.display = _showCurrentStep ? "block" : "none";

        // buttons
        $get("objProgressDialogCommandButtons").style.display = _showDialogButtons ? "block" : "none";
        $get("PopupFormFooter").style.display = _showDialogButtons ? "block" : "none";

        // update timer handlers
        if (updateTimerHandler) {
            window.clearTimeout(updateTimerHandler);
            updateTimerHandler = 0;
        }

        // reload progress image
        $find('ModalPopupProperties').show();

        $get("progressStartTime").innerHTML = new Date().toLocaleTimeString();
        StartProgressTimer();

        var initialTimeout = _showDialogButtons ? 100 /* async */ : 1000 /* sync */;

        window.setTimeout(ReloadProgressImage, 100);

        if (_showCurrentStep)
            window.setTimeout(GetTaskProgress, initialTimeout);
    }
    return true;
}
function ReloadProgressImage() {
    $get("imgAjaxIndicator").src = $get("imgAjaxIndicator").src;
}
function GetTaskProgress() {
    requestSimpleService = SolidCP.Portal.TaskManager.GetTaskWithLogRecords(
    _taskId,       //params
    new Date(1, 2, 3, 4),
    OnGetTaskProgressComplete,     //Complete event
    OnGetTaskProgressTimeout       //Timeout event
    );
}
function OnGetTaskProgressComplete(task) {
    if (task == null || task.Completed) {
        // switch buttons
        $get("objProgressDialogCommandButtons").style.display = "none";
        $get("objProgressDialogCloseButton").style.display = "block";
        // stop timer
        StopTimer();
        // hide image indicator
        $get("imgAjaxIndicator").style.display = "none";
        // show success message
        $get('objProgressDialogStep').innerHTML = _completeMessage;

        $get("objProgressDialogProgressBar").style.width = 100 + "%";
        return;
    }
    if (task.Logs != null) {
        $get('objProgressDialogStep').innerHTML = task.Logs.length > 0 ? task.Logs[task.Logs.length - 1].Text : "";
    }
    // set progress indicator
    if (task.IndicatorMaximum > 0)
        $get("objProgressDialogProgressBar").style.width = task.IndicatorCurrent / task.IndicatorMaximum * 100 + "%";
    $find('ModalPopupProperties')._layout();
    //alert(result);
    window.setTimeout(GetTaskProgress, 1000);
}
function OnGetTaskProgressTimeout(result) {
    alert("Timed out");
}
function OnCancelProgressDialog() {
}