$(function () {
    $("#getOptions").click(function () {
        $.getJSON("Home/Options", null, getOptions);
    });

    $("#addNew").click(function () {
        var newOption = $("#newOption")[0].value;
        if (newOption == "") {
            alert("You must provide a new option!");
            return;
        }
        var option = {
            Value: newOption,
            Displayname: newOption
        };
        $.post("Home/Options", option, getOptions, "json");
    });
    $("#listWeeks").change(function () { alert("I changed something in the list"); });

    $("a").click(function (event) {
        if (this.id.indexOf("addToTotal") > -1) {
            var url = "Home/" + $("#typeOfSelection").val() + "/AddToTotal";
            var data={
                WeekNo:$(this).attr("href"),
                TopItems [
                    {ItemName: '"' + $("#w"+$(this).attr("href") + "i1").text() +'"',
                    Rank:"1", Position: "0", NumberOfPlays:"0",ItemImage:"",TopItemType:"Artist"},
                    {ItemName:$("#w"+$(this).attr("href") + "i2").text(),
                    Rank:"2", Position: "0", NumberOfPlays:"0",ItemImage:"",TopItemType:"Artist"},
                    {ItemName:$("#w"+$(this).attr("href") + "i3").text(),
                    Rank:"3", Position: "0", NumberOfPlays:"0",ItemImage:"",TopItemType:"Artist"}]
            };
            $.getJSON(url, data, getTotalsAgain);
            event.preventDefault();
        }
    });

    $("#totalsList").jqGrid({
        url: "GetTotals",
        datatype: "json",
        colNames: [$("#typeOfSelection").val(), "Position", "Score", "Entry Week"],
        colModel: [
   		{ name: 'TypeOfSelection', index: 'TypeOfSelection', width: 355 },
   		{ name: 'Position', index: 'Position', width: 90 },
   		{ name: 'Score', index: 'Score', width: 100 },
   		{ name: 'EntryWeek', index: 'EntryWeek', width: 180, align: "right" }
   	],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#totalsPager',
        sortname: 'Position',
        viewrecords: true,
        sortorder: "desc",
        caption: "Totals " + $("#typeOfSelection").val()
    });

});

function getTotalsAgain(newTotals) {
    alert("I have some totals again!");
}

function getOptions(myOptions) {
    $("#optionsList").text = "";
    $.each(myOptions, function (i) {
        if($("#li"+myOptions[i].Value).length<=0)
            $("#optionsList").append("<li id='li" + myOptions[i].Value + "'>" + myOptions[i].Value + "</li>");
    });
}

$(function () {
    $("#accordion").accordion({
        change: function (event, ui) {
            var nextWeek = ui.newHeader.text().substr(ui.newHeader.text().indexOf("Week: ") + 6, ui.newHeader.text().indexOf("(") - ui.newHeader.text().indexOf("Week: ") - 6);
            if ($("#TopWeek" + nextWeek + " li").length <= 0) {
                var data = { WeekNo: nextWeek, TypeOfSelection: $("#typeOfSelection").val() };
                $.getJSON("Top", data, printTop);
            }
        }
    });

});

function printTop(weeklyTop)
{
    $("#TopWeek" + weeklyTop.WeekNo).text = "";
    var topItems = weeklyTop.TopItems;
    $.each(topItems, function (i){
        if ($("#li" + weeklyTop.WeekNo + weeklyTop.TopItems[i].Rank).length <= 0)
            $("#TopWeek" + weeklyTop.WeekNo).append("<li class='topItem' id='li" + weeklyTop.WeekNo + topItems[i].Rank + "'><span>" + topItems[i].Rank + "</span><span>" + topItems[i].Position + "</span><span id='w" + weeklyTop.WeekNo + "i" + topItems[i].Rank + "'>" + topItems[i].ItemName + "</span><span>(" + topItems[i].NumberOfPlays + ")</span></li>");
    });
}

$("#totalsList").jqGrid('navGrid', '#totalsPager', { edit: false, add: false, del: false });