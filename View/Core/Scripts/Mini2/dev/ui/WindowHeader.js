/// <reference path="../Mini.js" />


Mini2.define('Mini2.ui.WindowHeader', {

    renderTo: Mini2.getBody(),

    text: 'Window',

    headerTpl: [
        '<div class="mi-window-header ',
            'mi-header ',
            'mi-header-horizontal ',
            'mi-header-draggable ',
            'mi-docked ',
            'mi-unselectable ',
            'mi-window-header-default ',
            'mi-horizontal ',
            'mi-window-header-horizontal ',
            'mi-window-header-default-horizontal ',
            'mi-top ',
            'mi-window-header-top ',
            'mi-window-header-default-top ',
            'mi-docked-top ',
            'mi-window-header-docked-top ',
            'mi-window-header-default-docked-top" ',
            'style="right: auto; left: -5px; top: -5px; width: 400px; ">',
            '<div class="mi-header-body ',
                'mi-window-header-body ',
                'mi-window-header-body-default ',
                'mi-window-header-body-horizontal ',
                'mi-window-header-body-default-horizontal ',
                'mi-window-header-body-top ',
                'mi-window-header-body-default-top ',
                'mi-window-header-body-default-docked-top ',
                'mi-box-layout-ct ',
                'mi-window-header-body-default-horizontal ',
                'mi-window-header-body-default-top " ',
                'style="width: 380px; ">',
                '<div class="mi-box-inner " role="presentation" style="width: 380px;  ">',
                    '<div class="mi-box-target" style="width: 380px; ">',
                        '<div class="mi-component ',
                            'mi-header-text-container ',
                            'mi-window-header-text-container ',
                            'mi-window-header-text-container-default ',
                            'mi-box-item mi-component-default" ',
                            'unselectable="on" ',
                            'style="right: auto; left: 0px; top: 0px; margin: 0px; width: 358px; ">',
                            '<span class="mi-header-text ',
                                'mi-window-header-text ',
                                'mi-window-header-text-default" ',
                                'unselectable="on">',
                                'Window',
                            '</span>',
                        '</div>',

                        '<div class="mi-tool ',
                            'mi-box-item ',
                            'mi-tool-default ',
                            'mi-tool-after-title" ',
                            'title="关闭"',
                            'style="width: 16px; height: 16px;  left: 364px; top: 0px; right: auto;margin: 0px; " >',
                            '<img role="presentation" ',
                                'src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" ',
                                'class="mi-tool-img mi-tool-close">',
                        '</div>',

                    '</div>',
                '</div>',
            '</div>',
        '</div>'
    ],


    /**
     * 关闭按钮激活
     */
    closeEnabled: true,


    setText: function (value) {
        this.headerTextEl.html(value);
    },

    getText: function () {
        return this.headerTextEl.html();
    },


    setWidth: function (value) {
        "use strict";
        var me = this,
            sysInfo = Mini2.SystemInfo;

        me.el.css('width', value);

        me.boxInnerEl.css('width', value - 20);
        me.boxTargetEl.css('width', value - 20);

        me.textContainerEl.css('width', value - 32);
        
        if ('touch' == sysInfo.mode) {
            me.toolEl.css({
                left: value - 46
            });
        }
        else {
            me.toolEl.css({
                left: value - 36
            });
        }
    },

    srcLocation: {},
    srcX: 0,
    srcY: 0,

    button1: false,

    isDrag: false,

    mouseBind: function (el) {
        "use strict";
        var me = this;

        el.muBind('mousedown', me, function (e) {
            me.button1 = true;

            me.srcLocation = me.owner.getOffset();

            me.srcX = event.pageX;
            me.srcY = event.pageY;

            me.owner.focus();

        })
        .muBind('mouseup', me, function (e) {
            me.button1 = false;

            if (me.isDrag) {

                me.isDrag = false;
                me.owner.endDrag();

                Mini2.EventManager.stopEvent(e);
            }
        })
        .muBind('mousemove', me, function (e) {

            if (me.button1) {

                var offsetX = event.pageX - me.srcX;
                var offsetY = event.pageY - me.srcY;

                var x = offsetX + me.srcLocation.left;
                var y = offsetY + me.srcLocation.top;

                if (!me.isDrag) {
                    if (Math.abs(offsetX) >= 4 || Math.abs(offsetY) >= 4) {

                        me.isDrag = true;
                        me.owner.beginDrag();
                    }
                }
                else if (me.isDrag) {
                    me.owner.setGhostLocation(x, y);
                }
            }
        });


    },

    render: function () {
        "use strict";
        var me = this,
            sysInfo = Mini2.SystemInfo,
            cfg = sysInfo.window;

        var el = Mini2.$joinStr(me.headerTpl);

        var headerBodyEl = el.cFirst('.mi-header-body');
        var boxInnerEl = headerBodyEl.cFirst('.mi-box-inner');
        var boxTargetEl = boxInnerEl.cFirst('.mi-box-target');

        var textContainerEl = boxTargetEl.cFirst('.mi-header-text-container');
        var headerTextEl = textContainerEl.cFirst('.mi-header-text');
        var toolEl = boxTargetEl.cFirst('.mi-tool');

        console.debug('me.closeEnabled = ', me.closeEnabled);

        if (!me.closeEnabled) {
            toolEl.hide();
        }

        headerTextEl.html(me.text);


        me.el = el;

        me.headerBodyEl = headerBodyEl;
        me.boxInnerEl = boxInnerEl;
        me.boxTargetEl = boxTargetEl;

        me.textContainerEl = textContainerEl;
        me.headerTextEl = headerTextEl;
        me.toolEl = toolEl;


        if ('touch' == sysInfo.mode) {

            toolEl.css({
                width: 32,
                height: 32,
                top: -10,
            });
        }


        $(toolEl).mouseenter(function () {
            $(this).addClass('mi-tool-over');
        })
        .mouseout(function () {
            $(this).removeClass('mi-tool-over');
        })
        .muBind('mousedown',Mini2.emptyFn)
        .muBind('mouseup', function (e) {
            me.onAction('close',e);
        });

        $(me.renderTo).append(el);
        
        boxInnerEl.css('height', cfg.headerHeight || 16);

        //width: 16px; height: 16px;  left: 364px; top: 0px; 


        me.mouseBind(el);
    },

    onAction: function (name) {
        "use strict";
        //log.debug('onAction = ' + name);

        $(this).triggerHandler('action', [name]);

        Mini2.EventManager.stopEvent();
    }

});
