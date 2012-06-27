function isEven(val) {
    return val % 2 === 0;
}

function realOne() {
    return "this is the real one";
}

function anotherOneCallingTheRealOne() {
    if (realOne() === "this is the real one")
        return "I called the real one";
    else
        return "I called an alien!";
}

function methodCallingADOMElement(myElement) {
    if (myElement.val() == "my own value") {
        $("#inputResult").val("I'm happy");
        return true;
    }
    else {
        $("#inputResult").val("I'm depressed");
        return false;
    }
}
function methodCallingADOMElement(myElement) {
    if (myElement.val() == "my own value") {
        $("#inputResult").val("I'm happy");
        return true;
    }
    else {
        $("#inputResult").val("I'm depressed");
        return false;
    }
}