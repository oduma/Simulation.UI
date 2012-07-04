test("Selector plugin tests", function () {

    var options;
    var abc = new ScoreAlgorythmType();
    abc.name = "abc";
    abc.inUse = false;
    abc.algorythm = function () { return 1 + 1; };

    var def = new ScoreAlgorythmType();
    def.name = "def";
    def.inUse = true;
    def.algorythm = function () { return 1 + 2; };

    var feg = new ScoreAlgorythmType();
    feg.name = "feg";
    feg.inUse = false;
    feg.algorythm = function () { return 1 + 3; };

    options = { "itemType": "itemType1", "algorythms": [abc, def, feg] };


    ok($("#algSelector").algSelector(options) != null, 'I can call it at least');
    equal(3, $("#algSelector").data('algSelector.itemType1').length, "The Data is Ok");
    equal("abc", $("#algSelector").data('algSelector.itemType1')[0].name, "First One in data is Ok");
    equal("def", $("#algSelector").data('algSelector.itemType1')[1].name, "Second One in data is Ok");
    equal("feg", $("#algSelector").data('algSelector.itemType1')[2].name, "Third One in data is Ok");
    equal(2, $("#algSelector").data('algSelector.itemType1')[0].algorythm(), "First Alg in data is Ok");
    equal(3, $("#algSelector").data('algSelector.itemType1')[1].algorythm(), "Second Alg in data is Ok");
    equal(4, $("#algSelector").data('algSelector.itemType1')[2].algorythm(), "Third Alg in data is Ok");
    ok(!$("#algSelector").data('algSelector.itemType1')[0].inUse, "First One not in use");
    ok($("#algSelector").data('algSelector.itemType1')[1].inUse, "Second One in use");
    ok(!$("#algSelector").data('algSelector.itemType1')[2].inUse, "Third One not in use");
    function results() { this.returnValue = 0; };
    $("#algSelector").algSelector("run", "itemType1", results);
    equal(3, results.returnValue, "Execution of run is Ok");
    $("#algSelector option")[0].selected = true;
    $("#algSelector option")[1].selected = false;
    $("#algSelector").change();
});
