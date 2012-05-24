$(function () {
    $("#dialog-confirm").dialog({
        resizable: true,
        height: 240,
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
        $("#dialogmessage").append("Your previous tops were calculated using: " + previous + " rule. If you change to : " + $("#rulesSelector option:selected").val() +" your old tops will be deleted.<br/> Do you want to continue?");
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
