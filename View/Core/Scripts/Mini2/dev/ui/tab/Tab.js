
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />
/// <reference path="../../../../jstree/3.0.2/jstree.js" />


Mini2.define('Mini2.ui.tab.Tab', {

    extend: 'Mini2.ui.Component',

    tabPanel: null,

    padding: '10px 10px 10px 10px',

    /**
    * 显示关闭按钮
    */
    closable: false,

    //x-tab x-unselectable x-box-item x-tab-default x-noicon x-tab-noicon x-tab-default-noicon x-top x-tab-top x-tab-default-top
    //            'mi-tab-after-title" ',
    reandeTpl: [
        '<a class="mi-tab ',
            'mi-unselectable ',
            'mi-box-item ',
            'mi-tab-default ',
            //'mi-noicon ',
            //'mi-tab-noicon ',
            //'mi-tab-default-noicon ',
            'mi-top ',
            'mi-tab-top ',
            'mi-tab-default-top" ',
            'style="" ',
            'role="button" hidefocus="on" unselectable="on" tabindex="0" >',

            '<span class="mi-tab-wrap" unselectable="on">',
                '<span class="mi-tab-button">',
                    '<span class="mi-tab-inner mi-tab-inner-center" unselectable="on">',
                    'Tab',
                    '</span>',
                    //'<span role="img" class="mi-tab-icon-el" unselectable="on" ></span>',
                '</span>',
            '</span>',
        '</a>'
    ],

    /**
    * 图标样式
    */
    iconCls: null,

    

    //关闭按钮的图标
    closeBtnTpl: ['<span class="mi-tab-close-btn" title="" tabindex="0"></span>'],

    closeOverCls: 'mi-tab-close-btn-over',

    getText: function () {
        var me = this,
            btnInnerEl = me.btnInnerEl;

        var html = $(btnInnerEl).html();

        return html;
    },

    setText: function (value) {
        var me = this,
            btnInnerEl = me.btnInnerEl;

        $(btnInnerEl).html(value);
    },


    /**
    * 隐藏组件
    */
    hide: function () {
        var me = this,
            el = me.el;

        if (me.visible != false) {
            me.visible = false;

            el.hide();

            me.callParent();
        }

        return me;
    },

    /**
    * 显示组件
    */
    show: function () {
        var me = this,
            el = me.el;

        if (me.visible != true) {
            me.visible = true;

            el.show();

            me.callParent();
        }

        return me;
    },


    /**
     * 设置版面的 url
     */
    setUrl: function (url) {
        var me = this,
            contentEl = me.contentEl;

        if (!me.iframe) {
            console.error('当前 tab 不是 iframe 对象.');

            return me;
        }

        me.url = url;

        $(contentEl).attr('url', url);

        return me;
    },

    /**
     * 获取 iframe 框架的 win 对象
     */
    getIFrameWindow: function () {
        var me = this,
            iframeWin = null,
            contentEl = me.contentEl,
            con0;

        if (!me.iframe) {
            console.warn('当前 tab 不是 iframe 对象.', me);

            return me;
        }

        if (!contentEl || 0 == contentEl.length) {

            console.warn('当前 tab 不是 contentEl 对象.', contentEl);

            return me;
        }

        con0 = contentEl[0];

        if (con0) {
            iframeWin = con0.contentWindow;// || con0.contentDocument.parentWindow;
        }

        return iframeWin;
    },

    setActive: function () {
        "use strict";
        var me = this,
            ui = me.ui,
            baseCls = 'mi-tab-' + ui,
            el = me.el;


        var activeCls = [
            'mi-active',
            'mi-tab-active',
            baseCls + '-active',
            'mi-top-active',
            'mi-tab-top-active',
            baseCls + '-top-active'];

        var tabPanel = me.tabPanel;
        var lastTab = tabPanel.curActiveTab;

        if (tabPanel.curActiveTab === me) {
            return;
        }

        tabPanel.lastActveTab = tabPanel.curActiveTab;
        tabPanel.curActiveTab = me;
        Mini2.ui.FocusMgr.setControl(me.scope || me);


        $(el).addCls(activeCls);

        me.contentEl && $(me.contentEl).removeClass('mi-hide-display');


        me.panel.show();
        me.panel.updateLayout();

        /*** 临时解决,以后作废 **************/
        setTimeout(function () {
            me.panel.updateLayout();
        }, 50);
        /***********************************/

        me.resetPaint();

        if (lastTab) {
            $(lastTab.el).removeCls(activeCls);

            if (lastTab.panel) {
                lastTab.panel.hide();
            }
        }

        me.trigger('action', { tab: me });

        return me;

    },


    /**
    * 重置层次
    */
    resetPaint: function () {
        var me = this;

        me.panel.resetPaint();

        return me;
    },


    //关闭
    close: function () {
        "use strict";
        var me = this;

        me.tabPanel.remove(me)

        return me;
    },

    //创建关闭按钮
    createCloseBtn: function () {
        "use strict";
        var me = this,
            el = me.el,
            ui = me.ui,
            closeEl,
            closable = me.closable;

        if (closable) {
            closeEl = Mini2.$joinStr(me.closeBtnTpl);

            el.append(closeEl);

            el.addCls('mi-closable',
                'mi-tab-closable',
                'mi-tab-' + ui + '-closable',
                'mi-closable-top',
                'mi-tab-closable-top',
                'mi-tab-' + ui + '-closable-top');

            me.closeEl = closeEl;


            closeEl.mouseenter(function () {
                $(this).addClass(me.closeOverCls);
            }).mouseleave(function () {
                $(this).removeClass(me.closeOverCls);
            });


            closeEl.muBind('mousedown', function () {

            }).muBind('mouseup', function () {
                
                me.close();

            });

        }
    },

    //isDelayRender:false,

    //延迟显示
    delayRender: function () {
        "use strict";
        var me = this,
            el = $(me.applyTo);

        me.isDelayRender = true;

        el.attr('mi-delay-render', 'true');
        el.data('me', me);

        return me;
    },

    render: function () {
        "use strict";
        var me = this,
            ui = me.ui,
            baseCls = 'mi-tab-' + ui,
            el,btnEl;

        if (me.isDelayRender) {
            //me.isDelayRender = false;
        }

        me.el = el = Mini2.$join(me.reandeTpl);

        el.addCls([baseCls, baseCls + '-top']);

        me.btnEl = btnEl = el.find('.mi-tab-button');

        me.btnInnerEl = btnEl.children('.mi-tab-inner');

        if (me.iconCls || me.icon) {
            el.addClass('mi-icon-text-left mi-btn-icon-text-left mi-tab-default-icon-text-left mi-btn-default-small-icon-text-left');
        }

        if (me.iconCls) {
            var iconEl = Mini2.$join(['<i class="mi-tab-icon-el fa ', me.iconCls, '"></i>']);
            btnEl.append(iconEl);
        }
        else if (me.icon) {
            var iconEl = $('<span role="img" class="mi-tab-icon-el" unselectable="on" ></span>');
            btnEl.append(iconEl);
        }



        me.setText(me.text || '按钮');

        me.createCloseBtn();

        el.css('left', me.left || 0);

        el.mouseenter(function () {

            $(this).addClass('mi-tab-' + me.ui + '-over');


        }).mouseleave(function () {

            $(this).removeClass('mi-tab-' + me.ui + '-over');
        });


        el.muBind('mousedown', function () {
            //Mini2.ui.FocusMgr.setControl(me.scope || me);

        }).muBind('mouseup', function () {
            //me.setActive();
            me.tabPanel.setActiveTab(me);
        });

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else if (me.renderTo) {
            $(me.renderTo).append(el);
        }

        $(el).data('me', me);


    }

});
