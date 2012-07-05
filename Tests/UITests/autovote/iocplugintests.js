test("Selector plugin tests", function () {

    var scoreData = new ScoreData();
    var options;
    var abc = new ScoreAlgorythmType();
    abc.name = "abc";
    abc.inUse = false;
    abc.algorythm = function (scoreData) { scoreData.score = scoreData.rank + 1; };

    var def = new ScoreAlgorythmType();
    def.name = "def";
    def.inUse = true;
    def.algorythm = function (scoreData) { scoreData.score = scoreData.rank + 2; };

    var feg = new ScoreAlgorythmType();
    feg.name = "feg";
    feg.inUse = false;
    feg.algorythm = function (scoreData) { scoreData.score = scoreData.rank + 3; };

    options = { "algorythms": [abc, def, feg] };


    ok($("#algSelector").algSelector(options) != null, 'I can call it at least');
    equal(3, $("#algSelector").data('algSelector').length, "The Data is Ok");
    equal("abc", $("#algSelector").data('algSelector')[0].name, "First One in data is Ok");
    equal("def", $("#algSelector").data('algSelector')[1].name, "Second One in data is Ok");
    equal("feg", $("#algSelector").data('algSelector')[2].name, "Third One in data is Ok");
    ok(!$("#algSelector").data('algSelector')[0].inUse, "First One not in use");
    ok($("#algSelector").data('algSelector')[1].inUse, "Second One in use");
    ok(!$("#algSelector").data('algSelector')[2].inUse, "Third One not in use");
    scoreData.rank = 1;
    $("#algSelector").algSelector("run", scoreData);
    equal(3, scoreData.score, "Execution of run is Ok");
    $("#algSelector option")[0].selected = true;
    $("#algSelector option")[1].selected = false;
    $("#algSelector").change();
    $("#algSelector").algSelector("run", scoreData);
    equal(2, scoreData.score, "Execution of run is Ok");

});
