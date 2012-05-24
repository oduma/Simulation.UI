$(function () {
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        autoOpen:false,
        buttons: {
            "Ok": function () {
                $.getJSON("ChangeRules", "ruleName=" + $("#rulesSelector option:selected").val(), getRulesChanged);
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
});

$(function () {
    var previous;

    $("#rulesSelector").focus(function () {
        // Store the current value on focus, before it changes
        previous = this.value;
    }).change(function () {
        $("#dialogmessage").append("You had: " + previous + " and yu want: " + $("#rulesSelector option:selected").val());
        if ($("#shouldAsk").val() == "true") {
            $("#dialog-confirm").dialog("open");
        }
        else
        $.getJSON("ChangeRules", "ruleName=" + $("#rulesSelector option:selected").val(), getRulesChanged);
    });
})
function getRulesChanged(response) {
    alert("Rules Changed Successfuly");
}
$(function () {
    $("#getAuthorization").click(function () {
        $.getJSON("TryAuthorize", null, getRulesChanged);
    });
})
