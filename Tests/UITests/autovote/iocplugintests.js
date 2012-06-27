test('Calling the plugin', function () {
    ok($("#algs").algorythmSelector({ "options": [{ "id": "abc", "label": "Abc", "algotyhm": function abc() { return 1 + 1; } },
    { "id": "def", "label": "Def", "algotyhm": function def() { return 1 + 2; } },
    { "id": "feg", "label": "Feg", "algotyhm": function feg() { return 1 + 3; } }]
    }) != null, 'I can call it at least');
    equal(3, $("#algs option").length, "I have the number correctly");
    equal("abc", $("#algs option")[0].value, "First one is Ok");
    equal("def", $("#algs option")[1].value, "Second one is Ok");
    equal("feg", $("#algs option")[2].value, "Third one is Ok");
    equal(3, $("#algs").data('algorythmSelector').length, "The Data is Ok");
    equal("abc", $("#algs").data('algorythmSelector')[0], "First One in data is Ok");
    equal("def", $("#algs").data('algorythmSelector')[1], "Second One in data is Ok");
    equal("feg", $("#algs").data('algorythmSelector')[2], "Third One in data is Ok");

    //equal(jQuery.fn.iocplugin(), 'something', 'returns good');
})
test("Algorythms", function () {
    var appContext = new AppContext();
    ok(appContext != null, 'It lives!');
    appContext.algorythms.push({name:"add", algorythm: function add() { return 1 + 1; }});
    equal(2, appContext.algorythms.length, 'I have one function!');
    equal(2, appContext.algorythms[1].algorythm(), 'It does what it says on the tin');
})
