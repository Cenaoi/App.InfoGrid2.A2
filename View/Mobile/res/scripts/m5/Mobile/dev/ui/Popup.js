


//弹出窗体
Mini2.define('Mini2.ui.Popup', {

    extend: 'Mini2.ui.Component',

    

    popupOverlayClass: 'modal-overlay',

    initComponent:function(){
        var me = this,
            el = me.el,
            popupOverlay = me.popupOverlayClass;

        var closeBtnEls = $(el).find('.close-popup');
        
        closeBtnEls.on('click', function (e) {
            me.close();
        });


        //创建蒙层
        if ($('.' + popupOverlay).length === 0) {
            $(document.body).append('<div class="' + popupOverlay + '"></div>');
        }
    },

    onInit: function () {
        var me = this;


    },


    baseRender: function () {
        "use strict";
        var me = this,
            el = me.el;



    },

    open: function () {
        var me = this,
            el = me.el,
            popupOverlay = me.popupOverlayClass;


        $('.' + popupOverlay).addClass(popupOverlay+'-visible');


        $(el).css({
            'display': 'block'
        });

        setTimeout(function () {
            $(el).removeClass('modal-out').addClass('modal-in');
        }, 100);

        return me;
    },


    close: function () {
        var me = this,
            el = me.el,
            popupOverlay = me.popupOverlayClass;

        $(el).removeClass('modal-in').addClass('modal-out');



        setTimeout(function () {
            $(el).css({
                'display': 'none'
            });

            $(el).remove();
        }, 500);
        
        return me;
    },

    render: function () {
        "use strict";
        var me = this,
            el = me.el;

        me.baseRender();

        me.open();
    }

});