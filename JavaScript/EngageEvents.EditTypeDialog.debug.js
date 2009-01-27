function EngageEvents() { }
EngageEvents.occurrenceUrl = null;
EngageEvents.seriesUrl = null;
EngageEvents.occurrence = 1;
EngageEvents.series = 2;

EngageEvents.GetRadWindow = function() {
    var oWindow = null;
    if (window.radWindow) {
        oWindow = window.radWindow;
    }
    else if (window.frameElement.radWindow) {
        oWindow = window.frameElement.radWindow;
    }
    return oWindow;
};

EngageEvents.OK_Clicked = function(occurrenceRadioButtonId) {
    var occurrenceRadioButton = document.getElementById(occurrenceRadioButtonId),
        oWindow = EngageEvents.GetRadWindow();
    oWindow.close(occurrenceRadioButton.checked ? EngageEvents.occurrence : EngageEvents.series);
};

EngageEvents.Cancel_Clicked = function() {
    var oWindow = EngageEvents.GetRadWindow();
    oWindow.close();
};

EngageEvents.showEditTypeDialog = function(occurrenceUrl, seriesUrl) {
    window.radopen(null, "EditTypeDialogWindow");
    EngageEvents.occurrenceUrl = occurrenceUrl;
    EngageEvents.seriesUrl = seriesUrl;
};

EngageEvents.EditTypeDialogWindow_Callback = function(radWindow, returnValue) {
    if (returnValue) {
        var navigateUrl = returnValue === EngageEvents.occurrence ? EngageEvents.occurrenceUrl : EngageEvents.seriesUrl;
        if (navigateUrl.substring(0, 4) === 'http') {
            window.location.href = navigateUrl;
        }
        else {
            eval(navigateUrl);
        }
    }
};