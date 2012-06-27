module('Dummy tests');
test('Testing dummyForTests', function () {
    ok(dummyForTest(), 'I can call it at least');
    equal(dummyForTest(), 'I was here', 'returns good');
})
