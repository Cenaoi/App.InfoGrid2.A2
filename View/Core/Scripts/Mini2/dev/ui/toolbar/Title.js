/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../Mini.js" />

Mini2.define('Mini2.ui.toolbar.Hr', {

    extend: 'Mini2.ui.Component',

    renderTpl: [
        '<div class="mi-toolbar-separator  mi-toolbar-separator-horizontal mi-box-item mi-toolbar-item" ',
            'style="right: auto; top: 5px; margin: 0px;">',
        '</div>'
    ],

    getInputEl: function () {
        "use strict";
        var me = this,
            el;

        el = Mini2.$joinStr(me.renderTpl);

        if (!me.visible) {
            el.css('display', 'none');
        }

        return el;
    },


    getWidth: function () {
        var me = this,
            el = me.el;

        return $(el).outerWidth();
    },

    render: function () {
        "use strict";
        var me = this,
            el;

        me.el = el = me.getInputEl();

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error(ex);
        }
    }

});


Mini2.define('Mini2.ui.toolbar.Title', {

    extend: 'Mini2.ui.Component',

    btnInner: undefined,

    baseCls: Mini2.baseCSSPrefix + 'btn',

    scale: 'small',
    allowedScales: ['small', 'medium', 'large'],

    //    dock: 'none',

    text: '标题',

    icon: false,


    renderTpl: [
        '<div class="',
            'mi-unselectable ',
            'mi-box-item ',
            'mi-toolbar-item ',
            'mi-toolbar-title " ',
            'role="button" hidefocus="on" unselectable="on" tabindex="0" ',
            'style="">',
            '<span class="mi-btn-wrap" unselectable="on">',
                '<span class="mi-btn-button">',
                    '<span class="mi-btn-inner mi-btn-inner-center" unselectable="on" style="">',

                    '</span>',
                '</span>',
            '</span>',
        '</div>'
    ],



    getInputEl: function () {
        "use strict";
        var me = this,
            command = me.command,
            imgEl,
            inputEl,
            overCls = ['mi-over', 'mi-btn-over', 'mi-btn-default-toolbar-small-over'],
            pressedCls = ['mi-pressed', 'mi-btn-pressed', 'mi-btn-default-toolbar-small-pressed'];

        inputEl = Mini2.$joinStr(me.renderTpl);
        inputEl.css({
            width: me.width,
            height: me.height,
            left: me.left
        });

        if (command && command != '') {
            inputEl.attr('command', command);
        }

        if (me.icon) {
            inputEl.addCls('mi-icon-text-left',
                'mi-btn-icon-text-left',
                'mi-btn-default-toolbar-small-icon-text-left');

            me.imgEl = imgEl = $('<img src="" class="mi-btn-icon-el  mi-btn-glyph"  />');
            imgEl.attr('src', me.icon);

            inputEl.find('.mi-btn-button').append(imgEl);
        }


        me.btnInner = inputEl.find('.mi-btn-inner:first');
        me.btnInner.html(me.text);


//        inputEl.bind('mouseenter', function () {
//            $(this).addCls(overCls);
//        })
//        .bind('mouseleave', function () {
//            $(this).removeCls(overCls);
//        });

        inputEl.muBind('mousedown', function () {
            //$(this).addCls(pressedCls);

            Mini2.ui.FocusMgr.setControl(this);

        })
        .muBind('mouseup', function () {

            //$(this).removeCls(pressedCls);

            if (me.click) {
                me.click.call(this);
            }
            else if (me.command) {
                me.serverForSubmit();
            }

        });

        return inputEl;
    },


    //提交到服务器
    serverForSubmit: function () {
        "use strict";
        var me = this,
            widget = me.widget || window.widget1;

        if (widget) {
            widget.submit(me.el, {
                command: me.command,
                commandParams: me.commandParams
            });
        }
    },

    getWidth: function () {
        "use strict";
        var me = this,
            el = me.el;

        return $(el).outerWidth();
    },

    render: function () {
        "use strict";
        var me = this,
            el;

        me.el = el = me.getInputEl();

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error(ex);
        }
    }

});


