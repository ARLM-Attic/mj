// Anpassungen, die durchgef�hrt werden sollen, wenn JQuery Mobile anschlie�end eingebunden wird

$(document).bind('mobileinit', function () {
    $.mobile.textinput.prototype.options.clearSearchButtonText = "Zur\u00fccksetzen";
});
