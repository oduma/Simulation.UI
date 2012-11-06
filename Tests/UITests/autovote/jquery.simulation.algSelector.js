ScoreAlgorythmType = function () { };
ScoreAlgorythmType.prototype = {
    name: "",
    algorythm: function () {  }
};

ScoreData = function () { };
ScoreData.prototype = {
    rank:0,
    score:0
};

(function ($) {
    var methods = {
        init: function (options) {
            return this.each(function () {
                var currentSelection = $(this);
                if (!currentSelection.data(this.id)) {
                    currentSelection.data(this.id, options.algorythms);
                }
            });
        },
        run: function (scoreData) {
            return this.each(function () {
                var currentSelection = $(this);
                if (!currentSelection.data(this.id))
                    $.error('algSelector not initialized');
                var selectedName=$("#"+this.id + " option:selected").val();
                $.each(currentSelection.data(this.id),function(i, alg){
                    if(alg.name===selectedName)
                        alg.algorythm(scoreData);
                });
            });
        }
    };

    $.fn.algSelector = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        }
        else {
            $.error("Method " + method + " does not exist in jQuery.selector");
        }
    };

})(jQuery);
