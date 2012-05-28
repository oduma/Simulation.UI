$(function () {
    $("#getAuthorization").click(function () {
        $.getJSON("TryAuthorize", null, getRulesChanged);
    });
})
