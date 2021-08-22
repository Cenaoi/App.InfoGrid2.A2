/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.Offline', {

    tpl: [
        //offline-ui offline-ui-down offline-ui-down-5s
        //offline-ui offline-ui-down offline-ui-connecting offline-ui-waiting
        //offline-ui offline-ui-up offline-ui-up-5s

        '<div class="mi-offline" >',
            '<div class="mi-offline-content" >',
                '<div class="mi-offline-icon"></div>',
                '<div class="mi-offline-text">(尝试重新连接...)</div>',
            '</div>',
            '<a class="mi-offline-retry" >重新连接</a>',
        '</div>'
    ],

    info : {

        recont: '尝试重新连接...',

        lost: '你与服务器发生断网.',

        success: '你已重新连接服务器'
    },


    visible: false,



    initComponent:function(){
        var me = this;

    },

    baseRender:function(){
        var me = this,
            el ;

        me.el = el = Mini2.$join(me.tpl);

        me.textEl = el.find('.mi-offline-text')

        if (me.visible) {

            el.show(); 
        }
        else {
            el.hide();
        }

        $('body').append(el);

        return me;
    },

    render: function () {
        var me = this;

        me.baseRender();

        return me;
    },


    hide: function () {
        var me = this,
            txt,
            el = me.el;

        if (!me.visible) {
            return;
        }

        me.visible = false;

        txt = me.info['success'];
        me.textEl.html(txt);

        el.removeClass('mi-offline-down').removeClass('mi-offline-connecing');
        el.addClass('mi-offline-online');

        setTimeout(function () {
            el.animate({ top: -100 }, function () {
                el.hide();
            });
        }, 2000);

        return me;
    },

    show: function (infoCode) {
        var me = this,
            txt,
            el = me.el;

        if (me.visible) {
            return;
        }


        me.visible = true;

        infoCode = infoCode || 'lost';

        if (!el) {
            me.render();
            el = me.el;
        }

        txt = me.info[infoCode];
        
        me.textEl.html(txt);

        el.removeClass('mi-offline-connecing').removeClass('mi-offline-online');
        el.addClass('mi-offline-down');

        
        if (el.is(':visible')) {
        }
        else {
            el.css({ top: -100 });
            el.show().animate({ top: '0px' });
        }

        setTimeout(function () {

            txt = me.info['recont'];
            me.textEl.html(txt);

            el.removeClass('mi-offline-down').removeClass('mi-offline-online');
            el.addClass('mi-offline-connecing');

        }, 3000);

        //setTimeout(function () {

        //    me.hide();

        //}, 5000);
    }

}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});