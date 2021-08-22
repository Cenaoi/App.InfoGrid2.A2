
Mini.ui.data = Mini.ui.data || {};

Mini.ui.data.EntityStore = function (ps) {

    
    var defaults = {
        id: ''
    };

    function init(options) {
        defaults = $.extend(defaults, options);
        
    }

    this.add = function (sender) {
        
        widget1.submit(sender, {
            subName: defaults.id,
            subEvent: 'Call_Add'
        });

        return false;
    };

    this.load = function (sender) {
        
        widget1.submit(sender, {
            subName: defaults.id,
            subEvent: 'Call_Load'
        });
        
        return false;
    };

    this.update = function (sender) {

        widget1.submit(sender, {
            subName: defaults.id,
            subEvent: 'Call_Update'
        });
        
        return false;
    };


    this.delete = function(sender){


        widget1.submit(sender, {
            subName: defaults.id,
            subEvent: 'Call_Delete'
        });
        
        return false;
    };

    
    init(ps);
}