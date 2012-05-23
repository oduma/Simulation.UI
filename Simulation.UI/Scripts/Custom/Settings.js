$(function () {
    $("#rulesSelector").change(function () {
        $.getJSON("ChangeRules", "ruleName=" + $("#rulesSelector option:selected").val(), getRulesChanged);
    });
    })
function getRulesChanged(response) {
    alert("Rules Changed Successfuly");
    }