ScoreAlgorythmType = function () { };
ScoreAlgorythmType.prototype = {
    name: "",
    inUse:"",
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
                    //Add the event if selection is changed to set the inUse flag
                    $(this).bind('change.' + this.id, methods.selectAlgorythm);
                }
            });
        },
        destroy: function () {

            return this.each(function () {
                $(this).unbind('.' + this.id);
            })
        },
        selectAlgorythm: function (e) {
            var selectedOption = $("#" + this.id + " option:selected").val();
            $.each($(this).data(this.id), function (i, alg) {
                alg.inUse = false;
                if (alg.name === selectedOption)
                    alg.inUse = true;
            });
        },
        run: function (scoreData) {
            return this.each(function () {
                var currentSelection = $(this);
                if (!currentSelection.data(this.id))
                    $.error('algSelector not initialized');
                $.each(currentSelection.data(this.id),function(i, alg){
                    if(alg.inUse)
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
