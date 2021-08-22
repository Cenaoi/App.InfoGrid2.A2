


/**
* 弹出窗体
*
*/
Mini2.define('Mini2.ui.Popup', {

    extend: 'Mini2.ui.Component',
       
    /**
    * 蒙层样式
    */
    popupOverlayClass: 'popup-overlay',

    

    initComponent:function(){
        var me = this,
            el = me.el,
            popupOverlay = me.popupOverlayClass;

        var closeBtnEls = $(el).find('.close-popup');
        
        closeBtnEls.on('click', function (e) {
            me.close();
        });


    },

    /**
    * 显示蒙层
    */
    showOverlay:function(){
        var me = this,
            popupOverlay = me.popupOverlayClass;

        //创建蒙层
        if (0 === $('.' + popupOverlay).length ) {
            $(me.renderTo).append('<div class="' + popupOverlay + '"></div>');
        }

        $('.' + popupOverlay).addClass(popupOverlay + '-visible');

        return me;
    },


    /**
    * 隐藏蒙层
    */
    hideOverlay:function(){
        var me = this,
            popupOverlay = me.popupOverlayClass;

        $('.' + popupOverlay).removeClass(popupOverlay + '-visible');

        return me;
    },


    onInit: function () {
        var me = this;


    },


    baseRender: function () {
        "use strict";
        var me = this,
            el = me.el;



    },



    setVisible: function (value) {
        var me = this,
            el = me.el;

        el.switchClass('modal-in', 'modal-out', value);

        return me;
    },


    isPreLoaded: false,

    onPreLoad: function () {
        var me = this;
        if (!me.isPreLoaded) {
            me.isPreLoaded = true;

            if (me.onLoad) {
                me.onLoad();
            }
        }
    },


    open: function () {
        "use strict";
        var me = this,
            el = me.el;
        
        me.showOverlay();   //显示蒙层
        
        $(el).show();   

        setTimeout(function () {
            me.setVisible(true);
            
            me.on('opened');    //触发关闭事件

        }, 50);

        return me;
    },

    /**
    * 关闭
    */
    close: function () {
        "use strict";
        var me = this,
            el = me.el;

        me.hideOverlay();   //隐藏蒙层

        me.setVisible(false);

        setTimeout(function () {
            $(el).remove();
        }, 500);
        
        me.on('closed');    //触发关闭事件

        return me;
    },

    render: function () {
        "use strict";
        var me = this,
            el = me.el;

        me.baseRender();

        me.open();

        return me;
    }

});