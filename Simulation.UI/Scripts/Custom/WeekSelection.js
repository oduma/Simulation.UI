$(document).ready(function () {
    $.getJSON("GetTotals", null, getTotalsAgain);
})
function getTotalsAgain(newTotals) {
    $("#totalsList").clearGridData();
    for (i = 0; i < newTotals.length; i++)
        $("#totalsList").jqGrid('addRowData', i + 1, newTotals[i]);
}

function addTotalsAgain(totalsAfterWeek)
{
    $("#addToTotal" + totalsAfterWeek.WeekNo).remove();
    $("#action"+totalsAfterWeek.WeekNo).append("<p>Processed</p>");
    getTotalsAgain(totalsAfterWeek.TopItems);
}

$(function () {
    $('#settings-dialog').dialog({
        autoOpen: false,
        width: 400,
        resizable: false,
        modal: true
    });
});
$(function () {
    $("#addToTotal"+$("#nextWeekToProcess").val()).click(function (event) {
        var url, data;
        url = "AddToTotals";
        data = {
            "WeekNo": $("#nextWeekToProcess").val(),
            "TopItems": [
                    { "ItemName": $("#w" + $("#nextWeekToProcess").val() + "i1").text(),
                        "Rank": "1"
                    },
                    { "ItemName": $("#w" + $("#nextWeekToProcess").val() + "i2").text(),
                        "Rank": "2"
                    },
                    { "ItemName": $("#w" + $("#nextWeekToProcess").val() + "i3").text(),
                        "Rank": "3"
                    }]
        };
        $.post(url, data, addTotalsAgain, "json");
    });

    $("#totalsList").jqGrid({
        datatype: "local",
        colNames: [$("#typeOfSelection").val(), "Position", "Score", "Entry Week"],
        colModel: [
            { name: 'ItemName', index: 'ItemName', width: 355 },
            { name: 'Position', index: 'Position', width: 90 },
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
    $("#TopWeek" + weeklyTop.WeekNo).text = "";
    if (weeklyTop.TopProcessed === false) {
        $("#action" + weeklyTop.WeekNo).append("<input type='button' class='addToTotal' id='addToTotal" + weeklyTop.WeekNo + "' value='Add to Total'/>");
        $("#addToTotal" + weeklyTop.WeekNo).click(function (event) {
            var url, data;
            url = "AddToTotals";
            data = {
                "WeekNo": weeklyTop.WeekNo,
                "TopItems": [
                    { "ItemName": $("#w" + weeklyTop.WeekNo + "i1").text(),
                        "Rank": "1"
                    },
                    { "ItemName": $("#w" + weeklyTop.WeekNo + "i2").text(),
                        "Rank": "2"
                    },
                    { "ItemName": $("#w" + weeklyTop.WeekNo + "i3").text(),
                        "Rank": "3"
                    }]
            };
            $.post(url, data, addTotalsAgain, "json");

        });
    }
    else {
        $("#action" + weeklyTop.WeekNo).append("<p>Processed</p>");
    }
    var topItems = weeklyTop.TopItems;
    $.each(topItems, function (i) {
        if ($("#li" + weeklyTop.WeekNo + weeklyTop.TopItems[i].Rank).length <= 0) {
            $("#TopWeek" + weeklyTop.WeekNo).append("<li class='topItem' id='li" + weeklyTop.WeekNo + topItems[i].Rank + "'><span>" + topItems[i].Rank + "</span><span>" + topItems[i].Position + "</span><span id='w" +weeklyTop.WeekNo + "i" + topItems[i].Rank + "'>" + topItems[i].ItemName + "</span><span>(" + topItems[i].NumberOfPlays + ")</span></li>");
        }
    });
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