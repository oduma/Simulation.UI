$(function () {
    $("#getAuthorization").click(function () {
        $.getJSON("TryAuthorize", null, useToken);
    });
})

function useToken(token) {
    window.location = token;
}