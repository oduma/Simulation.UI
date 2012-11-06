function dummyForTest() {
    return "I was here";
}

function informOfProgress(progress) {
    $("#messages").children().remove();
    $("#messages").append("<p>" + progress + "</p>");
}

function addTotalsFor(itemType, newTotals) {
    var i = 0;
    $("#total" + itemType + "sList").clearGridData();
    $.each(newTotals, function (i, newTotal) {
        $("#total" + itemType + "sList").jqGrid('addRowData', i + 1, newTotal);
    });
}

var totals;

function addNewTotals(newTotals) {
    totals = newTotals;
    if (newTotals !== null) {
        addTotalsFor("Artist", newTotals[0]);
        addTotalsFor("Track", newTotals[1]);
    }
}

function scoreItem(itemRank, itemType) {
    var scoreDate = new ScoreData();
    scoreData = { rank: itemRank, score: 0 };
    $("#rulesSelector" + itemType).algSelector("run", scoreData);
    return scoreData.score;
}

function getExistingItem(existingTopItems, topItem) {
    if (!existingTopItems) {
        return null;
    } else {
        for (var i = 0; i < existingTopItems; i++) {
            if (existingTopItems[i].Name === topItem.Name) {
                return existingTopItems[i];
            }
        }
        return null;
    }
}

function recalculateRanks(existingTopItems) {
    existingTopItems.sort(function (a, b) {
        return (b.Score - a.Score);
    });
    $.each(existingTopItems, function (i, existingTopItem) {
        existingTopItem.Rank = i + 1;
    });
}

function voteClientSide(weeklyTop, existingTopItems) {
    for (i = 0; i < existingTopItems.length; i++) {
        var topItem = existingTopItems[i];
        var itemScore = scoreItem(topItem.Rank, (topItem.ItemType === 1) ? "Artist" : "Track");
        if (itemScore > 0) {
            var existingTopItem = getExistingItem(existingTopItems, topItem.Name);
            if (existingTopItem === null) {
                topItem.Score = itemScore;
                topItem.EntryWeek = weeklyTop.WeekNo;
                existingTopItems.push(topItem);
            } else {
                existingTopItem.Score += topItem.Score;
                existingTopItem.NumberOfPlays += topItem.NumberOfPlays;
                existingTopItems.pop(existingTopItem);
                existingTopItems.push(existingTopItem);
            }
        }
    }
//    $.each(weeklyTop.TopItems, function (i, topItem) {
//        var itemScore = scoreItem(topItem.Rank, (topItem.ItemType === 1) ? "Artist" : "Track");
//        if (itemScore > 0) {
//            var existingTopItem = getExistingItem(existingTopItems, topItem.Name);
//            if (existingTopItem === null) {
//                topItem.Score = itemScore;
//                topItem.EntryWeek = weeklyTop.WeekNo;
//                existingTopItems.push(topItem);
//            } else {
//                existingTopItem.Score += topItem.Score;
//                existingTopItem.NumberOfPlays += topItem.NumberOfPlays;
//                existingTopItems.pop(existingTopItem);
//                existingTopItems.push(existingTopItem);
//            }
//        }
//    });
    recalculateRanks(existingTopItems);
}

function voteForWeek(weeklyTop) {
    if (weeklyTop === null) {
        informOfProgress("Not voted!");
    } else {
        if (weeklyTop.ItemType === 1) {
            voteClientSide(weeklyTop, totals[0]);
            informOfProgress("Voted for Artists!");
//            addTotalsFor("Artist", totals[0]);
        }
        if (weeklyTop.ItemType===2) {
            voteClientSide(weeklyTop, totals[1]);
            informOfProgress("Voted for Tracks.");
//            addTotalsFor("Track", totals[1]);
        }
    }
}


function voteForItems(weeksToVote, itemType) {
    for (i = 0; i < weeksToVote.length; i++) {
        informOfProgress("Get" + itemType + " Week: " + weeksToVote[i].WeekNo);
        var data = {
            "WeekNo": weeksToVote[i].WeekNo,
            "StartingFrom": weeksToVote[i].StartingFrom,
            "EndingIn": weeksToVote[i].EndingIn,
            "ScoreAlgorythmName": $("#rulesSelector" + itemType).children("option:selected").val(),
            "ItemType": itemType
        };
        $.post("Auto/GetTopItems", data, voteForWeek, "json");
    }
    addTotalsFor("Artist", totals[0]);
    addTotalsFor("Track", totals[1]);

//    $.each(weeksToVote, function (i, weekToVote) {
//        informOfProgress("Get" + itemType + " Week: " + weekToVote.WeekNo);
//        var data = {
//            "WeekNo": weekToVote.WeekNo,
//            "StartingFrom": weekToVote.StartingFrom,
//            "EndingIn": weekToVote.EndingIn,
//            "ScoreAlgorythmName": $("#rulesSelector" + itemType).children("option:selected").val(),
//            "ItemType": itemType
//        };
//        $.post("Auto/GetTopItems", data, voteForWeek, "json");
//    });
}

function voteForAllWeeks(weeksToVote) {
    informOfProgress("Weeks considered for artists: " + weeksToVote[0].Weeks.length);
    informOfProgress("Weeks considered for tracks: " + weeksToVote[1].Weeks.length);
    voteForItems(weeksToVote[0].Weeks, "Artist");
    voteForItems(weeksToVote[1].Weeks, "Track");
}

$(document).ready(function () {
    aggregateAlgorythmPool(clientSideAlgorythms);
    $.getJSON("Auto/GetTotals", null, addNewTotals);
});


$(function () {
    $("#vote").click(function (event) {
        informOfProgress("Starting Vote ...");
        $("#vote").hide();
        $.getJSON("Auto/GetWeeksToVote", null, voteForAllWeeks);
    });
    $("#rulesSelectorArtist").algSelector(clientSideAlgorythms);
    $("#rulesSelectorTrack").algSelector(clientSideAlgorythms);


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
