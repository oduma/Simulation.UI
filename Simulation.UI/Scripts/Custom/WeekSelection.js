$(function () {
    $("#dialog-confirm").dialog({
        resizable: true,
        height: 240,
        width:480,
        modal: true,
        autoOpen: false,
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
        $("#dialogmessage").append("Your previous tops were calculated using: " + previous + " rule. If you change to : " + $("#rulesSelector option:selected").val() + " your old tops will be deleted.<br/> Do you want to continue?");
        if ($("#shouldAsk").val() == "True") {
            $("#dialog-confirm").dialog("open");
        }
        else
            $.getJSON("ChangeRules", "ruleName=" + $("#rulesSelector option:selected").val(), getRulesChanged);
    });
})
function getRulesChanged(response) {
    $("nextWeekToProcess").val("1");
    getTotalsAgain(null);
    $("#accordion").accordion("option", "active", "0");
    $("#noOfItems").val(response);

}

$(document).ready(function () {
    $.getJSON("GetTotals", null, getTotalsAgain);
})
function getTotalsAgain(newTotals) {
    $("#totalsList").clearGridData();
    if(newTotals!=null)
        for (i = 0; i < newTotals.length; i++)
            $("#totalsList").jqGrid('addRowData', i + 1, newTotals[i]);
}

function addTotalsAgain(totalsAfterWeek)
{
    $("#addToTotal" + totalsAfterWeek.WeekNo).remove();
    $("#action"+totalsAfterWeek.WeekNo).append("<p>Processed</p>");
    getTotalsAgain(totalsAfterWeek.TopItems);
    $("#shouldAsk").val("True");
}

$(function () {
    $("#addToTotal" + $("#nextWeekToProcess").val()).click(function (event) {
        var url, data;
        url = "AddToTotals";

        data = {
            "WeekNo": $("#nextWeekToProcess").val(),
            "TopItems": []
        };
        for (var i = 0; i < $("#noOfItems").val(); i++) {
            data.TopItems.push({ "ItemName": $("#w" + $("#nextWeekToProcess").val() + "i" + (i + 1)).text(),
                        "Rank": i+1});
        }
        $.post(url, data, addTotalsAgain, "json");
    });

    $("#totalsList").jqGrid({
        datatype: "local",
        colNames: [$("#typeOfSelection").val(), "Position", "Score", "Entry Week"],
        colModel: [
            { name: 'ItemName', index: 'ItemName', width: 355 },
            { name: 'Rank', index: 'Rank', width: 90 },
            { name: 'Score', index: 'Score', width: 100 },
            { name: 'EntryWeek', index: 'EntryWeek', width: 180, align: "right"}],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#totalsPager',
        sortname: 'Position',
        viewrecords: true,
        sortorder: "desc",
        caption: "Totals " + $("#typeOfSelection").val()
    });

});

function printTop(weeklyTop) {
    if (weeklyTop.TopItems == null) {
        $("#action" + weeklyTop.WeekNo).clear();
        $("#action" + weeklyTop.WeekNo).append("<p>No Data Retrieved for this week.Check the connection with your top provider.</p>");
    }
    else {
        $("#TopWeek" + weeklyTop.WeekNo).text = "";
        if (weeklyTop.TopProcessed === false) {
            $("#action" + weeklyTop.WeekNo).append("<input type='button' class='addToTotal' id='addToTotal" + weeklyTop.WeekNo + "' value='Add to Total'/>");
            $("#addToTotal" + weeklyTop.WeekNo).click(function (event) {
                var url, data;
                url = "AddToTotals";
                data = {
                    "WeekNo": weeklyTop.WeekNo,
                    "TopItems": []
                };
                for (var i = 0; i < $("#noOfItems").val(); i++) {
                    data.TopItems.push({ "ItemName": $("#w" + weeklyTop.WeekNo + "i" + (i + 1)).text(),
                        "Rank": i + 1
                    });
                }
                $.post(url, data, addTotalsAgain, "json");

            });
        }
        else {
            $("#action" + weeklyTop.WeekNo).append("<p>Processed</p>");
        }
        var topItems = weeklyTop.TopItems;
        $.each(topItems, function (i) {
            if ($("#li" + weeklyTop.WeekNo + weeklyTop.TopItems[i].Rank).length <= 0) {
                $("#TopWeek" + weeklyTop.WeekNo).append("<li class='topItem' id='li" + weeklyTop.WeekNo + topItems[i].Rank + "'><span>" + topItems[i].Rank + "</span><span id='w" + weeklyTop.WeekNo + "i" + topItems[i].Rank + "'>" + topItems[i].ItemName + "</span><span>(" + topItems[i].NumberOfPlays + ")</span></li>");
            }
        });
    }
}

$(function () {
    $("#accordion").accordion({ collapsible: true,
        active: $("#nextWeekToProcess").val()-1,
        change: function (event, ui) {
            var nextWeek, data;
            nextWeek = ui.newHeader.text().substr(ui.newHeader.text().indexOf("Week: ") + 6, ui.newHeader.text().indexOf("(") - ui.newHeader.text().indexOf("Week: ") - 6);
            if ($("#TopWeek" + nextWeek + " li").length <= 0) {
                data = { WeekNo: nextWeek, ItemType: $("#typeOfSelection").val() };
                $.getJSON("Top", data, printTop);
            }
        }
    });
});


$("#totalsList").jqGrid('navGrid', '#totalsPager', { edit: false, add: false, del: false });