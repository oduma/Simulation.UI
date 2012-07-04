ScoreAlgorythmType = function () { };
ScoreAlgorythmType.prototype = {
    name: "",
    inUse:"",
    algorythm: function () {  }
};


(function($) {
    var methods={
        init:function(options){
            return this.each(function () {
                var currentSelection=$(this);
                if(!currentSelection.data('algSelector.'+options.itemType)){
                    currentSelection.data('algSelector.'+options.itemType,options.algorythms);
                //Add the event if selection is changed to set the inUse flag
                 $(this).bind('change.algSelector', methods.selectAlgorythm);
                }
            });
        },
        destroy:function(){
        
       return this.each(function(){
         $(this).unbind('.algSelector');
       })
        },
        selectAlgorythm: function(e)
        {
            alert('I got here');
        },
        run:function(itemType, result){
            return this.each(function() {
                var currentSelection=$(this);
                if(!currentSelection.data('algSelector.' + itemType))
                    $.error('algSelector not initialized');
                var algs=currentSelection.data('algSelector.' + itemType);
                for(i=0;i<algs.length;i++)
                {
                    if(algs[i].inUse)
                        result.returnValue=algs[i].algorythm();
                }
            });
        }
    };

    $.fn.algSelector=function(method){
        if(methods[method]){
            return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
        }
        else if(typeof method==='object' || !method){
            return methods.init.apply( this, arguments);
        }
        else{
            $.error("Method "+ method +" does not exist in jQuery.selector");
        }
    };

})(jQuery);
