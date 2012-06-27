ScoreAlgorythm = function () { };
ScoreAlgorythm.prototype = {
    name: "",
    algorythm: function () {  },
};
AppContext = function() {};

AppContext.prototype={
    algorythms: []
};


(function ($) {
    
    var methods

    $.fn.algorythmSelector = function (inputs) {

        var settings=$.extend({"options":inputs.options});

        return this.each(function () {
            var currentSelection=$(this);
            var dataOptions;
            $.each(inputs.options ,function(dataOptions,i) {
                currentSelection.append("<option value='" + inputs.options[i].id + "'>"+ inputs.options[i].label + "</option>");
                dataOptions.push({"id":options[i].id,"algorythm":options[i].algorythm});
            });
            if(!currentSelection.data('algorythmSelector')){
                currentSelection.data('algorythmSelector',dataOptions);
            }
        });
    };
})(jQuery);