
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />
/// <reference path="../../../../jstree/3.0.2/jstree.js" />



//Tab 版面
Mini2.define('Mini2.ui.tab.Panel', {

    extend: 'Mini2.ui.Component',

    log: Mini2.Logger.getLogger('Mini2.ui.tab.Panel',true),  //日志管理器

    tabCls: Mini2.baseCSSPrefix + 'tab-panel',

    
    //当前激活的 Tab 
    curActiveTab: null,

    //
    plain: false,

    tabHeight: 32,


    //left | right | buttom | top
    dock: 'top',

    region: 'north',

    //显示按钮
    buttonVisible: true,

    //状态: normal-普通,min-最小化, max-最大化
    state: 'normal',

    //tab 标签默认起始位置
    tabLeft:0,

    renderTpl_body: [
        '<div class="mi-panel-body ',
            'mi-panel-body-default ',
            'mi-layout-fit ',
            'mi-noborder-trbl" ',
            'style="width: 450px; left: 0px; top: 36px; height: 160px;" >',

        '</div>'
    ],


    renderTpl_bar: [
        '<div role="Tab-bar" class="mi-tab-bar ',
            'mi-header ',
            'mi-header-horizontal ',
            'mi-docked ',
            'mi-unselectable ',
            //'mi-tab-bar-default ',
            'mi-horizontal ',
            'mi-tab-bar-horizontal ',
            //'mi-tab-bar-default-horizontal ',
            'mi-top ',
            'mi-tab-bar-top ',
            //'mi-tab-bar-default-top ',
            'mi-horizontal-noborder ',
            'mi-tab-bar-horizontal-noborder ',
            //'mi-tab-bar-default-horizontal-noborder ',
            'mi-docked-top ',
            'mi-tab-bar-docked-top ',
            //'mi-tab-bar-default-docked-top ',
            'mi-noborder-trl" >',

            '<div class="mi-tab-bar-body ',
                //'mi-tab-bar-body-default ',
                'mi-tab-bar-body-horizontal ',
                //'mi-tab-bar-body-default-horizontal ',
                'mi-tab-bar-body-top ',
                //'mi-tab-bar-body-default-top ',
                'mi-tab-bar-body-horizontal-noborder ',
                //'mi-tab-bar-body-default-horizontal-noborder ',
                //'mi-tab-bar-body-docked-top ',
                //'mi-tab-bar-body-default-docked-top ',
                'mi-box-layout-ct ',
                //'mi-tab-bar-body-default ',
                //'mi-tab-bar-body-default-horizontal ',
                //'mi-tab-bar-body-default-top ',
                //'mi-tab-bar-body-default-horizontal-noborder ',
                //'mi-tab-bar-body-default-docked-top" ',
                '" style="width: 450px;position: static;" >',

    //左移按钮，默认隐藏
                '<div class="mi-box-inner mi-box-scroller-left ">',
                    '<div class="mi-box-scroller mi-tabbar-scroll-left" style="display:none;"></div>',
                '</div>',

                '<div class="mi-box-inner mi-horizontal-box-overflow-body" role="presentation" style="width:450px;height:31px;" >',
                    '<div class="mi-box-target" style="width: 450px;">',

                    '</div>',
                '</div>',

    //右移按钮，默认隐藏
                '<div class="mi-tab-bar-buttons mi-box-scroller-right " style="text-align:right;top:0;right:10px;position: absolute;">',
                    '<a class="mi-tab-bar-close mi-box-inner mi-box-scroller-right" style="top:9px;margin-right:10px;" href="#">隐藏</a>',
                    '<a class="mi-tab-bar-max mi-box-inner mi-box-scroller-right" style="top:9px;margin-right:10px;" href="#">放大</a>',
                '</div>',
                '<div class="mi-box-inner mi-box-scroller-right ">',
                    '<div class="mi-box-scroller mi-tabbar-scroll-right" style="display:none;"></div>',
                '</div>',
            '</div>',

        '</div>'
    ],

    renderTpl_bar_strip:[
        '<div class="mi-tab-bar-strip ">',
        '</div>',
    ],


    initComponent: function () {
        var me = this;



    },


    //激活标签
    //tab 可以是数值,也可以是对象
    setActiveTab: function (tab) {
        "use strict";
        var me = this,
            log = me.log,
            items = me.items;

        if (Mini2.isNumber(tab)) {

            if (tab < items.length) {
                tab = items[tab];
            }
        }

        if (tab) {

            if (tab.isDelayRender) {

                var panel = tab.panel;

                panel.render();



                var html = tab.contentEl;   //.html();
               
                var htmlItems = $(html);
                
                $(panel.boxTarget).append(html);


                $(htmlItems).each(function () {

                    var itemId = $(this).attr('id');

                    if (itemId && Mini2.delayRS) {

                        itemId = '#' + itemId;

                        var dd = Mini2.delayRS[itemId];

                        if (dd) {

                            dd.render();

                            delete Mini2.delayRS[itemId];

                            log.debug("加载:" + itemId);

                        }
                    }

                });

                

                panel.boxTarget.css({
                    display: 'table-cell',
                    height: '100%',
                    width: me.width || '100%',
                    'vertical-align': 'top'
                });

                //  'padding': '10px 10px 10px 10px'

                delete tab.isDelayRender;
                delete tab.contentEl;


                tab.bind('action', me, me.tab_action);
            }

            tab.setActive();
        }
    },

    /**
     * tab 标签事件
     */
    tab_action: function (e) {
        var me = this;


        me.trigger('tabAction', {

            tab: e.tab,

            lastTab: me.lastActveTab

        });

    },


    setSize: function (width, height) {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            barEl = me.barEl,
            boxTargetEl = me.boxTargetEl,
            bodyEl = me.bodyEl,
            i,
            items = me.items,
            item,
            itemPanel;

        //log.debugFormat('width={0}, height={1}', [width, height]);

        if (undefined != width) {

            me.width = width;

            $(el).children('.mi-panel:first').css('width', width);


            barEl.css('width', width);

            var barBody = $(barEl).children('.mi-tab-bar-body');
            var boxInner = barBody.children('.mi-box-inner');

            var boxTarget = boxInner.children('.mi-box-target');

            barBody.css('width', width);
            boxInner.css({
                width: width - 60
            });
            boxTarget.css({
                width: width
            });

            bodyEl.css('width', width);
        }


        if (undefined != height) {

            me.height = height;


            $(el).css('height', height);

            $(el).children('.mi-panel:first').css('height', height);

            //bodyEl.css('height', height - 32 );

            if ('pills' == me.ui) {
                bodyEl.css('height', height - me.tabHeight);
            }
            else {
                bodyEl.css('height', height - me.tabHeight - 6);
            }

        }


        me.refresh_tabPage();




    },

    //添加标签
    // tabsReload = 强制刷新版面 
    add: function (tab, tabsReload) {
        
        "use strict";
        var me = this,
            log = me.log,        
            boxTargetEl = me.boxTargetEl,
            bodyEl = me.bodyEl,
            contentEl,
            x = 0,
            cfg,
            btn;

        var index = me.items.length;

        console.debug("tab.parentPage=", tab.parentPage);

        cfg = Mini2.applyIf(tab, {
            ui: me.ui,
            left: x,
            tabPanel: me,
            ownerParent : me,
            tabIndex: index ,
            renderTo: boxTargetEl
        });

        btn = Mini2.create('Mini2.ui.tab.Tab', cfg);

        btn.render();
        

        if (btn.contentEl) {
            var cEl = $(btn.contentEl);
            btn.contentEl = cEl.html();
        }
        else if (btn.iframe) {


            btn.contentEl = contentEl = $('<iframe src="" frameborder="0" dock="full">框架窗体</iframe>');

            btn.contentEl.attr('src', btn.url);

            if (cfg.parentPage) {

                contentEl.bind('load', {
                    'owner': me,
                    'tab': btn,
                    'parentPager': contentEl[0].contentWindow
                }, me.iframe_load);

                me.iFrameReady.call(me, contentEl[0], function () {

                    //console.log("xxxxxxxxxx" + new Date().getTime());

                    var win = contentEl[0].contentWindow;

                    win.ownerWindow = btn;

                    if (win.Mini2) {
                        win.Mini2.parentPage = cfg.parentPage;

                    }
                    else {
                        win.parentPage = cfg.parentPage;
                    }
                });

            }
        }



        var w = me.width;   // $(bodyEl).outerWidth();
        var h = $(bodyEl).outerHeight();

        var panel = Mini2.create('Mini2.ui.panel.Panel', {
            dock: 'full',
            width: w,
            height: h,
            scroll: btn.scroll,
            layout: {
                itemCls: 'mi-box-item'
            },
            renderTo: bodyEl
        });


        btn.panel = panel;


        //newItems[i] = btn;

        Mini2.Array.add(me.items, btn);


        if (cfg.isDelayRender) {
            panel.delayRender();
        }
        else {
            panel.render();

            panel.hide();


            $(panel.boxTarget).append(btn.contentEl);

            panel.boxTarget.css({
                'display': 'table-cell',
                'height': '100%',
                'width': w,
                'vertical-align': 'top'
            });

            //  'padding': '10px 10px 10px 10px'

            // 注意: 不要删除啊, 其它地方需要使用...
            //delete btn.contentEl;

            btn.bind('action', me, me.tab_action);
        }

        if (undefined == tabsReload || false === tabsReload) {
            me.refresh_btn();
        }

        return btn;
    },


    iframe_load: function (e) {
        "use strict";
        var me = this,
            iframeId = $(me).attr('muid'),
            win = me.contentWindow,
            curMi,
            doc = win.document,
            body = doc.body;

        console.log("iframe 加载完成" + new Date().getTime(), win);

        if (!win.ownerWindow) {

            console.log("iframe 加载完成 2");

            win.ownerWindow = e.data['tab'];

            var parentPage = e.data['parentPage'];

            curMi = win.Mini2;

            if (curMi && curMi.onwerPage) {
                curMi.onwerPage.parentPage = e.data['parentPage'];
            }
            else {
                win.parentPage = parentPage;
            }
        }
    },


    // This function ONLY works for iFrames of the same origin as their parent
    iFrameReady: function (iFrame, fn) {
        var timer;
        var fired = false;

        function ready() {
            if (!fired) {
                fired = true;
                clearTimeout(timer);
                fn.call(this);
            }
        }

        function readyState() {
            if (this.readyState === "complete") {
                ready.call(this);
            }
        }

        // cross platform event handler for compatibility with older IE versions
        function addEvent(elem, event, fn) {
            if (elem.addEventListener) {
                return elem.addEventListener(event, fn);
            } else {
                return elem.attachEvent("on" + event, function () {
                    return fn.call(elem, window.event);
                });
            }
        }

        // use iFrame load as a backup - though the other events should occur first
        addEvent(iFrame, "load", function () {
            ready.call(iFrame.contentDocument || iFrame.contentWindow.document);
        });

        function checkLoaded() {

            var doc;

            if (iFrame.contentDocument) {
                doc = iFrame.contentDocument;
            }

            if (!doc && iFrame.contentWindow) {
                doc = iFrame.contentWindow.document;
            }



            // We can tell if there is a dummy document installed because the dummy document
            // will have an URL that starts with "about:".  The real document will not have that URL
            if (doc && doc.URL.indexOf("about:") !== 0) {
                if (doc.readyState === "complete") {
                    ready.call(doc);
                } else {
                    // set event listener for DOMContentLoaded on the new document
                    addEvent(doc, "DOMContentLoaded", ready);
                    addEvent(doc, "readystatechange", readyState);
                }
            } else {
                // still same old original document, so keep looking for content or new document
                timer = setTimeout(checkLoaded, 1);
            }
        }

        checkLoaded();
    },

    /**
     * 更新布局...改写
     *
     */
    updateLayout:function(){

        var me = this;

        /**
         * 只运行一次
         */
        Mini2.awaitRun(me.muid, 100, me, function () {
            
            me.refresh_btn();

            var nextTab = me.getNextActiveTab();
            
            if (nextTab) {
                nextTab.setActive();
            }

        });

    },


    /**
     * 获取下一个焦点的 tab 
     */
    getNextActiveTab:function(){
        "use strict";
        var me = this,
            i,
            item,
            items = me.items,
            newTab = null,
            lastTab = me.lastActveTab;

        if (lastTab && lastTab.visible) {
            
            return lastTab;
        }

        if (!lastTab) {

            for (i = 0; i < items.length; i++) {
                item = items[i];

                if (item.visible) {
                    newTab = item;
                    break;
                }
            }

        }
        else {

            var lastIndex = Mini2.Array.indexOf(items, lastTab);

            var n1 = lastIndex;

            //先往前查找
            while (--n1 >= 0) {
                item = items[n1];

                if (item.visible) {
                    newTab = item;
                    break;
                }
            }

            //找不到就再, 往后查找
            if (!newTab) {

                for (i = lastIndex + 1; i < items.length; i++) {
                    item = items[i];

                    if (item.visible) {
                        newTab = item;
                        break;
                    }
                }

            }


        }

        return newTab;
    },

    


    /**
     * 实例化按钮
     * 
     */
    render_btn: function (boxTargetEl, bodyEl) {
        "use strict";
        var me = this,
            i,
            items = me.items,
            cfg ,
            newItems;

        if (items) {

            delete me.items;
            me.items = [];

            newItems = [items.length];

            for (i = 0; i < items.length; i++) {
                cfg = items[i];
                me.add(cfg,false);
            }


            //me.items = newItems;
        }
    },

    /**
     * 查找标签
     */
    find: function(tabId){
        "use strict";
        var me = this,
            i,
            findTab = null,
            item,
            items = me.items || [];

        for (i = 0; i < items.length; i++) {

            item = items[i];

            if (item.id == tabId) {
                findTab = item;
                break;
            }

        }

        return findTab;

    },


    isVisible: function () {
        var me = this,
            el = me.el;

        return el.is(':visible');
    },

    
    //删除 Tab 标签
    remove: function (tab) {
        "use strict";
        var me = this,
            curTab = me.curActiveTab,
            actTab,
            index,
            log = me.log;


        $(tab.el).fadeOut(300, function () {                      

            tab.el.remove();

            tab.panel.el.remove();

            me.refresh_btn();

            index = Mini2.Array.remove(me.items, tab);

            if (tab === curTab) {
                if (index > 0) {
                    actTab = me.items[index - 1];
                    me.setActiveTab(actTab);
                }
                else if (index == 0) {
                    actTab = me.items[index];
                    me.setActiveTab(actTab);
                }
            }

        });

        return me;
    },

    //刷新按钮坐标
    refresh_btn: function () {
        "use strict";
        var me = this,
            log = me.log,
            x = me.tabLeft;


        $(me.items).each(function () {

            var tab = this;
                        
            if (tab.visible && tab.setLeft) {
                tab.setLeft(x);

                x += tab.getWidth() + 1;
            }
            else {
            }
            
        });

        //下面代码作为补救用
        setTimeout(function () {

            x = me.tabLeft;

            $(me.items).each(function () {

                var tab = this;

                if (tab.visible && tab.setLeft) {
                    tab.setLeft(x);

                    x += tab.getWidth() + 1;
                }
                else {
                }

            });
        },1000);

    },

    //刷新 Tab 板块
    refresh_tabPage: function () {
        "use strict";
        var me = this,
            bodyEl = me.bodyEl,
            w, h,
            i,
            item,
            items = me.items,
            len = items.length,
            itemPanel;


        w = bodyEl.outerWidth();
        h = bodyEl.outerHeight();

        for (i = 0; i < len; i++) {
            item = items[i];
            itemPanel = item.panel;

            if (itemPanel && itemPanel.visible) {
                itemPanel.setSize(w, h);
            }
        }

    },


    baseRender: function (tabHeight) {
        "use strict";
        var me = this,
            log = me.log,
            ui = me.ui,
            barStripCls = 'mi-tab-bar-strip-' + ui,
            el,
            sysInfo = Mini2.SystemInfo,
            cfg = sysInfo.tabPanel;

        if (tabHeight) {
            me.tabHeight = tabHeight;
        }
        else {
            if (me.tabHeight != cfg.tabHeight) {
                me.tabHeight = cfg.tabHeight;
            }
        }

        


        me.el = el = $('<div role="TabPanel"></div>');

        var panelEl = Mini2.$join([
            '<div role="Tab-Body" class="mi-panel mi-border-box mi-panel-', me.ui ,' mi-tab-bar-' , me.ui ,'" style="width: 450px; height: 196px;" >',

            '</div>'
        ]);

        panelEl.css('width', me.width);

        var barEl = Mini2.$join(me.renderTpl_bar);
        var barStripEl = Mini2.$join(me.renderTpl_bar_strip);

        var boxTargetEl = barEl.find('.mi-box-target:first');


        var barBodyEl = barEl.children('.mi-tab-bar-body');
        var boxInner = barBodyEl.children('.mi-horizontal-box-overflow-body');



        boxInner.css({
            height: me.tabHeight
        });


        barStripEl.addCls([
            'mi-tab-bar-strip-top', 
            barStripCls,
            barStripCls + '-horizontal',
            barStripCls + '-top',
            barStripCls + '-horizontal-noborder',
            barStripCls + '-docked-top'
        ]);

        barStripEl.css({
            top: me.tabHeight - 1
        });

        barEl.append(barStripEl);

        barEl.css('height', me.tabHeight);
        boxTargetEl.css('height', me.tabHeight);

        var bodyEl = Mini2.$join(me.renderTpl_body);

        bodyEl.addClass("mi-renderTpl_body")

        if ('pills' == me.ui) {

            bodyEl.css('top', me.tabHeight );

        }
        else {
            if ('touch' == sysInfo.mode) {
                bodyEl.css('top', me.tabHeight + 8);
            }
            else {

                bodyEl.css('top', me.tabHeight - 6);
            }
        }

        var barButtons = barEl.find('.mi-tab-bar-buttons');

        ///拿到要显示和隐藏子表格的按钮
        var closeEl = barButtons.children('.mi-tab-bar-close');

        var maxEl = barButtons.children('.mi-tab-bar-max');

        if (me.buttonVisible) {
            barButtons.show();
        }
        else {
            barButtons.hide();
        }

        maxEl.click(function () {


            var height = $(window).height() - 40;

            var parentEl = $(this).parent();

            if (me.state == 'max') {
                $(this).text('放大');
                

                Mini2.ui.Page.resetMaxControl(me);

                parentEl.children('.mi-tab-bar-close').show();
                me.state = 'normal';
            }
            else {
                $(this).text('还原');

                Mini2.ui.Page.maxControl(me);

                parentEl.children('.mi-tab-bar-close').hide();
                me.state = 'max';
            }
            
            bodyEl.show();

            Mini2.LoaderManager.preResize();

            return false;

        })

        closeEl.click(function () {

            var height = el.css("height");

            var parentEl = $(this).parent();


            if (me.state == 'min') {
                $(this).text('隐藏');

                el.removeCls('mi-state-max', 'mi-state-min');

                el.css('height', me.height);
                panelEl.css('height', me.height);


                parentEl.children('.mi-tab-bar-max').show();
                me.state = 'normal';
            }
            else {
                $(this).text('还原');

                el.addClass('mi-state-min');

                el.css("height", 36);
                panelEl.css('height', 36);

                me.state = 'min';

                parentEl.children('.mi-tab-bar-max').hide();
            }


            bodyEl.toggle(me.state != 'min');

            Mini2.LoaderManager.preResize();

            return false;
        });



        

        barEl.css('width', me.width);
        boxTargetEl.css('width', me.width);
        bodyEl.css('width', me.width);

        me.barEl = barEl;
        me.boxTargetEl = boxTargetEl;
        me.bodyEl = bodyEl;


        if (me.plain) {
            barEl.addClass('mi-tab-bar-plain');

            var barBodyEl = barEl.children('.mi-tab-bar-body');

            barBodyEl.children('.mi-box-scroller-left').addClass('mi-box-scroller-plain');
            barBodyEl.children('.mi-box-scroller-right').addClass('mi-box-scroller-plain');
        }


        me.render_btn(boxTargetEl, bodyEl);


        panelEl.append(barEl);
        panelEl.append(bodyEl);


        el.append(panelEl);



        if (me.contentEl) {

            var contentEl = $(me.contentEl);
            var items = contentEl.children();

            contentEl.after(el);


            el.attr('id', contentEl.attr('id'));

            contentEl.remove();
        }
        else if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else if (me.renderTo) {
            $(me.renderTo).append(el);
        }

        if (me.visible == false) {
            el.hide();
            delete me.visible;
        }

        el.css('height', me.height);


        bodyEl.css('height', me.height - 31 - 2);

        $(el).children('.mi-panel').css('height', me.height);

        el.data('me', me);

        me.refresh_tabPage();
        me.refresh_btn();

        setTimeout(function () {
            me.setActiveTab.call(me, 0);

        }, 200);

    },

    render: function () {
        var me = this;

        //特殊配置
        if ('pills' == me.ui) {

            me.baseRender(40);            
        }
        else {
            me.baseRender();           
        }



    }


});