$(function () {
    $("#rulesSelector").change(function () {
        alert("I changed the rule" + $("#rulesSelector option:selected").val());

        $.getJSON("ChangeRules", "ruleName=" + $("#rulesSelector option:selected").val(), getRulesChanged);
    });
    })
function getRulesChanged(response) {
    alert("Rules Changed Successfuly");
    }