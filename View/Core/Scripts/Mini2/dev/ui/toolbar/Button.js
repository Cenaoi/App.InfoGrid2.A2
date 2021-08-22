/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../Mini.js" />


Mini2.define('Mini2.ui.toolbar.Button', {

    extend: 'Mini2.ui.Component',

    btnInner: undefined,

    baseCls: Mini2.baseCSSPrefix + 'btn',

    /*
    * 图标样式
    */
    iconCls: null,

    scale: 'small',
    allowedScales: ['small', 'medium', 'large'],

    //    dock: 'none',

    //text: '按钮',

    //icon: false,

    //href




    renderTpl: [
        '<a class="mi-btn ',
            'mi-unselectable ',
            'mi-btn-toolbar ',
            'mi-box-item ',
            'mi-toolbar-item ',

            'mi-btn-default-toolbar-small ',

            '" ',
            'role="button" hidefocus="on" unselectable="on" tabindex="0" ',
            'style="right: auto; left: 229px; top: 0px; margin: 0px;">',
            '<span class="mi-btn-wrap" unselectable="on">',
                '<span class="mi-btn-button">',
                    '<span class="mi-btn-inner mi-btn-inner-center" unselectable="on">',

                    '</span>',

                '</span>',
            '</span>',
        '</a>'
    ],



    setText: function (value) {
        "use strict";
        var me = this,
            btnInner = me.btnInner;

        me.text = value;

        if (btnInner) {
            btnInner.html(value);
        }

        return me;
    },

    getText: function () {
        "use strict";
        var me = this,
            btnInner = me.btnInner;

        return btnInner.html();
    },

    /**
    * 鼠标进入里面
    * 
    */
    isMouseEnter: false,

    tooltipEl: null,

    preMouseEnter: function () {
        var me = this,
            el = me.el,
            tplContentEl = me.tooltipContentEl,
            tipEl = me.tooltipEl;

        // 提示框
        if (tplContentEl) {
            if (!tipEl) {

                me.tooltipEl = tipEl = Mini2.create('Mini2.ui.menu.MenuPanel', {
                    contentEl: tplContentEl
                });

                tipEl.render();
                tipEl.bind('showing', function () {

                    me.trigger('tooltipShowing');

                });
            }

            var offset = el.offset();
            var height = el.height();

            tipEl.setLocal(offset.left, offset.top + height + 10);

            tipEl.show();
        }
    },

    preMouseLeave: function () {
        var me = this,
            el = me.el;

    },

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

        if (me.id) {
            inputEl.attr('id', me.id);
        }

        if (!me.visible) {
            inputEl.css('display', 'none');
        }

        if (me.href) {
            inputEl.attr('href', me.href);
        }

        if (command && command != '') {
            inputEl.attr('command', command);
        }


        if (Mini2.isBlank(me.text) && (me.icon || me.iconCls)) {

            inputEl.addCls('mi-btn-default-toolbar-small-icon');

        }
        else if (me.text != '' && (me.icon || me.iconCls)) {
            inputEl.addCls('mi-icon-text-left',
                'mi-btn-icon-text-left',
                'mi-btn-default-toolbar-small-icon-text-left');
        }

        if (me.icon) {
            me.imgEl = imgEl = $('<img src="" class="mi-btn-icon-el  mi-btn-glyph"  />');
            imgEl.attr('src', me.icon);

            inputEl.find('.mi-btn-button').append(imgEl);
        }
        else if (me.iconCls) {
            me.imgEl = imgEl = Mini2.$join(['<i class="mi-btn-icon-el  mi-btn-glyph fa  ', me.iconCls, '"></i>']);
            inputEl.find('.mi-btn-button').append(imgEl);
        }

        if (me.tooltip) {
            inputEl.attr('title', me.tooltip);

            if (inputEl.tooltip) {
                inputEl.tooltip({
                    placement: 'auto',
                    delay: 300,
                    container: Mini2.getBody()
                });
            }
        }

        if (me.userCls) {
            inputEl.addClass(me.userCls);
        }

        if (me.disabled) {
            inputEl.addCls(['mi-item-disabled', 'mi-disabled', 'mi-btn-disabled', 'mi-btn-default-small-disabled']);
        }


        me.btnInner = inputEl.find('.mi-btn-inner:first');
        me.btnInner.html(me.text);


        inputEl.on('mouseenter', function () {
            $(this).addCls(overCls);

            me.isMouseEnter = true;

            me.preMouseEnter();

        })
        .on('mouseleave', function () {
            $(this).removeCls(overCls);

            me.isMouseEnter = false;

            me.preMouseLeave();
        });

        inputEl.muBind('mousedown', function () {
            $(this).addCls(pressedCls);

            Mini2.ui.FocusMgr.setControl(this);

        })
        .muBind('mouseup', function (evt, srcEvt) {

            $(this).removeCls(pressedCls);

            if (0 == srcEvt.button) {
                if (me.beforeClick) {

                    var result = me.beforeClick.call(me, evt);

                    if (false === result) {
                        return;
                    }

                }
                else if (!Mini2.isBlank(me.beforeAskText)) {

                    return me.showAskMsg(me);

                }

                me.callback_click.call(me);
            }



        });

        return inputEl;
    },


    //显示询问弹出窗口
    showAskMsg: function (owner) {
        "use strict";
        var me = owner;

        Mini2.Msg.confirm("询问", me.beforeAskText, function () {
            me.click();
        });

        return false;
    },


    //提交到服务器
    serverForSubmit: function () {
        "use strict";
        var me = this,
            widget = me.widget || window.widget1;



        if (widget) {
            widget.submit(me.el, {
                command: me.command,
                commandParam: me.commandParams
            });
        }
    },

    getWidth: function () {
        "use strict";
        var me = this,
            el = me.el,
            width = 0;

        width = $(el).outerWidth();

        if (width <= 4) {
            width = 4;
        }

        return width;
    },

    render: function () {
        "use strict";
        var me = this,
            el;

        var icoPlg = Mini2.ui.icon.Manager.getPlugin('btnConfig');

        var cfg;

        if (icoPlg) {
            cfg = icoPlg.getItemConfig(me.text);

            if (cfg && Mini2.isBlank(me.icon) && Mini2.isBlank(me.iconCls)) {
                me.icon = cfg.icon;
                me.iconCls = cfg.iconCls;
            }
        }

        me.el = el = me.getInputEl();

        if (cfg) {
            el.addClass(cfg.cls);
        }


        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }

        if (me.click) {
            me.userClick = me.click;

            delete me.click;
        }

        me.click = me.callback_click;

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error('注册权限错误',ex);
        }

        return me;
    },

    callback_click: function () {
        "use strict";
        var me = this;

        if (me.userClick) {
            me.userClick.call(me);
        }
        else if (me.command) {
            me.serverForSubmit.call(me);
        }

    }

});


