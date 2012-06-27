function dummyForTest() {
    return "I was here";
}

function informOfProgress(progress) {
    $("#messages").children().remove();
    $("#messages").append("<p>" + progress + "</p>");
}

$(document).ready(function () {
    $.getJSON("Auto/GetTotals", null, function (newTotals) {
        if (newTotals != null) {
            AddTotalsFor("Artist", newTotals[0]);
            AddTotalsFor("Track", newTotals[1]);
        }
    }
);
})

function AddTotalsFor(itemType, newTotals) {
    $("#total" + itemType + "sList").clearGridData();
    if (newTotals != null)
        for (i = 0; i < newTotals.length; i++)
            $("#total" + itemType + "sList").jqGrid('addRowData', i + 1, newTotals[i]);
}

$(function () {
    $("#vote").click(function (event) {
        informOfProgress("Starting Vote ...");
        $("#vote").hide();
        $.getJSON("Auto/GetWeeksToVote", null, function (weeksToVote) {
            informOfProgress("Weeks considered for artists: " + weeksToVote[0].length);
            informOfProgress("Weeks considered for tracks: " + weeksToVote[1].length);
            voteForItems(weeksToVote[0].Weeks, "Artist");
            voteForItems(weeksToVote[1].Weeks, "Track");
        });
    });

    function voteForItems(weeksToVote, itemType) {
        $.each(weeksToVote, function (i) {
            informOfProgress("Get" + itemType + " Week: " + weeksToVote[i].WeekNo);
            var data = {
                "WeekNo": weeksToVote[i].WeekNo,
                "StartingFrom": weeksToVote[i].StartingFrom,
                "EndingIn": weeksToVote[i].EndingIn,
                "ScoreAlgorythmName": $("#rulesSelector" + itemType).children("option:selected").val(),
                "ItemType": itemType
            };
            $.post("Auto/GetTopItems", data, function (weeklyTop, itemType) {
                if (weeklyTop == null)
                    informOfProgress("Not voted!");
                else {
                    existingTopItems = $("#total" + itemType + "sList").getRows();
                    voteClientSide(weeklyTop, existingTopItems);
                    informOfProgress("Voted!");
                    AddTotalsFor(itemType, existingTopItems);
                }
            }
, "json");
        });
    }


    function voteClientSide(weeklyTop, existingTopItems) {
        $.each(topItem, function (i) {
            itemScore = scoreItem(topItem.Rank);
            if (itemScore > 0) {
                existingTopItem = getExistingItem(existingTopItems, topItem.Name);
                if (existingTopItem == null) {
                    existingTopItems.push(topItem);
                }
                else {
                    existingTopItem.Score += topItem.Score;
                    existingTopItem.NumberOfPlays += topItem.NumberOfPlays;
                    existingTopItems.pop(existingTopItem);
                    existingTopItems.push(existingTopItem);
                }
                recalculateRanks(existingTopItems);
            }
        });
    }

    $("#totalArtistsList").jqGrid({
        datatype: "local",
        colNames: ["Artist", "Position", "Score", "Entry Week"],
        colModel: [
            { name: 'ItemName', index: 'ItemName', width: 355 },
            { name: 'Rank', index: 'Rank', width: 90 },
            { name: 'Score', index: 'Score', width: 100 },
            { name: 'EntryWeek', index: 'EntryWeek', width: 180, align: "right"}],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#totalArtistsPager',
        sortname: 'Position',
        viewrecords: true,
        sortorder: "desc",
        caption: "Totals Artists"
    });
    $("#totalTracksList").jqGrid({
        datatype: "local",
        colNames: ["Track", "Position", "Score", "Entry Week"],
        colModel: [
            { name: 'ItemName', index: 'ItemName', width: 355 },
            { name: 'Rank', index: 'Rank', width: 90 },
            { name: 'Score', index: 'Score', width: 100 },
            { name: 'EntryWeek', index: 'EntryWeek', width: 180, align: "right"}],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#totalTracksPager',
        sortname: 'Position',
        viewrecords: true,
        sortorder: "desc",
        caption: "Totals Tracks"
    });
});
