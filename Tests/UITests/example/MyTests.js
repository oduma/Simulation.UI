DomElement = function () { };

DomElement.prototype = {
    val: function () {
        return "default string"
    }
};


module('Testing the tester');
test('Testing isEven()', function () {
    ok(isEven(0), 'Zero is an even number');
    ok(isEven(2), 'So is two');
    ok(isEven(-4), 'So is negative four');
    ok(!isEven(1), 'One is not an even number');
    ok(!isEven(-7), 'Neither is negative seven');

    equal(0, 0, '0 is equal with 0');
    equal(1, true, '1 is equal with true');

    deepEqual({}, {}, 'null object is the same as null object');
})

module('Testing the tester asynch');
test('asynchronous test', function () {
    // Pause the test first  
    stop();

    setTimeout(function () {
        ok(true);

        // After the assertion has been called,  
        // continue the test  
        start();
    }, 100)
})

asyncTest('asynchronous test with asyncTest', function () {
    // The test is automatically paused  
    expect(1);
    setTimeout(function () {
        ok(true);

        // After the assertion has been called,  
        // continue the test  
        start();
    }, 100)
})

module("Mock Ajax", {
    setup: function () {
        $.mockjax({
            url: "/mockedservice",
            contentType: "text/json",
            responseText: [{ bla: "Test"}]
        });
    },
    teardown: function () {
        $.mockjaxClear();
    }
});
asyncTest("Test I get a mocked response from a service", function () {
    expect(2);
    $.getJSON("/mockedservice", function (response) {
        ok(response, "There's a response!");
        equal(response[0].bla, "Test", "response was Test");
        start();
    });
});

module("Mock No Mock");
test('No Mock', function () {

    equal(realOne(), "this is the real one", "The real one returns its own value");
})

test('Mock the realone', function(){
    stub(this,"realOne",function(){ return "mocked!";});
    equal(anotherOneCallingTheRealOne(),"I called an alien!","Using the mocked version.");
})

test('Mock a dom element', function () {
    var mockedDomElement = new DomElement();
    stub(mockedDomElement, "val", function () { return "my own value"; });
    equal(methodCallingADOMElement(mockedDomElement), true, "Dom Element val function mocked");
    equal("I'm happy", $("#inputResult").val(), "Dom Element obtained when reading");
})