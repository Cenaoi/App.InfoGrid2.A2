/// <reference path="../Mini.js" />




Mini2.define('Mini2.ui.Window', {

    extend: 'Mini2.ui.Component',

    alias: 'widget.window',

    text: 'Window',

    /**
     * 特殊属性, 用于非 IFrame 框架的...作为内部处理用
     */
    bite: true,

    /**
     * 启动晃动
     */
    isTween:true,


    formClosed: function (fun) {
        this.bind('formClosed', fun);
    },

    formClosing: function (fun) {
        this.bind('formClosing', fun);
    },

    closeEnabled:true,

    windwoBoxTpl: [
        '<div class="mi-window ',
            'mi-layer ',
            'mi-window-default ',
            'mi-closable ',
            'mi-window-closable ',
            'mi-window-default-closable ',
            'mi-border-box ',
            'mi-resizable ',
            'mi-window-resizable ',
            'mi-window-default-resizable" ',
            'style="right: auto; position: fixed !important;">',
        '</div>'
    ],

    ghostTpl: ['<div class="mi-window-proxy" style="right: auto;display:none;"></div>'],


    bodyTpl: [
        '<div class="mi-window-body mi-window-body-default mi-closable mi-window-body-closable mi-window-body-default-closable ',
            'mi-window-body-default mi-window-body-default-closable mi-noborder-trbl " ',
            'style="padding: 0px; overflow: hide; left: 0px;">',
        '</div>'
    ],

    shadowTpl: [
        '<div class="mi-css-shadow mi-css-shadow-default" role="presentation" style="right: auto;position: fixed ; ',
            'left: 572px; top: 186px; width: 400px; height: 261px; ',
            'display: block;">',
        '</div>'
    ],

    maskTpl: ['<div class="mi-mask" style="right: auto; left: 0px; top: 0px; position: fixed;"></div>'],

    //蒙层的透明度
    //maskOpacity:1,


    contentTpl: ['<iframe src="" frameborder="0" dock="full">框架窗体</iframe>'],


    url: null,


    iframe: true,

    // iframe 内容
    iframeContent: null, 

    //default,center_screen,center_parent
    startPosition: 'default',

    //normal = 根据尺寸, min, max = 最大化
    //    state: 'normal',

    autoScroll: false,

    width: 600,
    height: 400,

    left: 100,
    top: 20,


    headerVisible: true,    //显示标题
    shadowVisible: true,    //显示阴影

    header: null,

    onInit: Mini2.emptyFn,

    onLoad: Mini2.emptyFn,

    //@private
    zIndex: 19000,

    //模式窗体
    mode: false,

    buttons: false,
    buttonPanel: false,
    buttonAlign: 'right',




    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this;

        Mini2.ui.WindowManager.regWin(this);

        me.onInit();
        me.onLoad();

    },

    render_ButtonPanel: function (renderTo) {
        "use strict";
        var me = this,
            panel,
            btns = me.buttons,
            sysInfo = Mini2.SystemInfo,
            cfg = sysInfo.window;

        if (btns) {

            var footerHeight = (cfg.footerHeight + 12) || 24;

            //console.debug("footerHeight = ", cfg.footerHeight);

            panel = Mini2.create('Mini2.ui.panel.Panel', {
                baseCls: Mini2.baseCSSPrefix + 'toolbar',
                dock: 'bottom',
                ui: 'footer',
                height: footerHeight,
                scroll: 'none',
                padding: {
                    left: 8,
                    top: 8,
                    right: 8,
                    bottom: 4
                },
                renderTo: renderTo
            });

            panel.render();


            me.render_Button(panel.boxTarget);

            me.buttonPanel = panel;
        }

    },


    /**
    * 初始化按钮组
    **/
    render_Button: function (renderTo) {
        "use strict";
        var me = this,
            cfg,
            btn,
            i,
            btns = me.buttons,
            newBtns = [];

        if (btns) {

            for (i = 0; i < btns.length; i++) {
                cfg = Mini2.applyIf(Mini2.clone(btns[i]), {
                    renderTo: renderTo,
                    ownerWindow: me
                });

                newBtns[i] = btn = Mini2.create('Mini2.ui.button.Button', cfg);
                btn.render();

                btn.el.addClass('mi-box-item');
            }

            delete me.buttons;
            me.buttons = newBtns;

        }
    },

    focusBase:function(){
        "use strict";
        var me = this;

        Mini2.ui.WindowManager.moveFirst(me);

        Mini2.ui.FocusMgr.setControl(me);

        return me;
    },

    focus: function () {
        return this.focusBase();
    },


    setZIndex: function (zIndex) {
        "use strict";

        var me = this,
            el = me.el;

        me.zIndex = zIndex;

        if (!me.el) {
            return;
        }

        el.css('z-index', zIndex);

        if (me.shadowVisible) {
            me.shadowEl.css({ 'z-index': zIndex - 1 });
        }

    },


    header_Action: function (e, name) {
        "use strict";
        var me = e.data;

        switch (name) {
            case 'close': me.close(); break;
            default:

        }

    },


    getOffset: function () {
        return $(this.el).offset();
    },


    getBtnWidth: function (btns) {
        "use strict";
        var w = 0, i = btns.length;

        while (i--) {
            w += btns[i].getWidth();
        }

        return w;
    },

    setButtonLayout: function (x, btns) {
        "use strict";
        var me = this,
            i,
            btn,
            len = btns.length;

        for (i = 0; i < len; i++) {
            btn = btns[i];
            btn.setLeft(x);

            x += btn.getWidth() + 6;
        }

    },

    updateLayout: function () {
        "use strict";
        var me = this,
            btn,
            w, i,
            l,
            btns = me.buttons;

        if (btns) {
            i = btns.length;
            w = me.getBtnWidth(btns);
            l = me.width - w - i * 6 - 20;

            switch (me.buttonAlign) {
                case 'left': me.setButtonLayout(0, btns); break;
                case 'center': me.setButtonLayout(l / 2, btns); break;
                case 'right': me.setButtonLayout(l, btns); break;
            }
        }

    },


    setSizeBase:function(w,h){
        "use strict";
        var me = this,
            el = me.el,
            bodyH = h - 10,
            bodyT = 0,
            btn,
            btns = me.buttons,
            bodyW = w - 10,
            contentEl = me.contentEl,
            size,
            sysInfo = Mini2.SystemInfo,
            cfg = sysInfo.window;


        me.width = w || me.width;
        me.height = h || me.height;

        el.css({ width: w, height: h });

        if (me.buttonPanel) {

            var footerHeight = (cfg.footerHeight + 8) || 36; //12 是 padding-top + padding-bottom

            me.buttonPanel.setSize(w - 10, footerHeight);


            bodyH -= footerHeight;

            me.updateLayout();
        }

        if (me.headerVisible) {
            me.header.setWidth(w);
            bodyH -= cfg.headerHeight + 12 || 31;
            bodyT += cfg.headerHeight + 12 || 31;
        }

        if (me.buttonPanel) {
            me.buttonPanel.setTop(bodyT + bodyH);
        }

        size = { width: bodyW, height: bodyH };

        me.bodyEl.css(size).css({
            top: bodyT
        });


        if (contentEl && ('full' == contentEl.attr('dock') || 'full' == contentEl.attr('data-dock'))) {

            if (contentEl.get(0).tagName == 'IFRAME') {
                contentEl.attr(size);
            }
            else {
                contentEl.css(size);

                if (me.bite) {
                    //因为事件机制的不完善，加了两个事件，防止点击到底下 Window，触发了其他事件。
                    contentEl.bind('mousedown', function (e) {

                        Mini2.ui.FocusMgr.setControl(this);

                    });
                    contentEl.muBind('mouseup', Mini2.emptyFn);
                }

            }
        }


        if (me.shadowVisible) {
            me.shadowEl.css({
                width: w,
                height: h - 8
            });
        }
            
        return me;
    },


    //@private
    setSize: function (w, h) {
        var me = this;

        me.setSizeBase(w, h);

        return me;
    },

    /**
    * 设置尺寸(动画效果)
    */
    setSizeTween: function (w, h) {
        "use strict";
        var me = this,
            srcX = me.left,
            srcY = me.top,
            srcW = me.width,
            srcH = me.height;

        if (!TWEEN) {
            me.setSize(w, h);
            return;
        }

        var span = 300;

        if (!me.isTween) {
            span = 100;
        }

        var coords = {
            x: srcX, y: srcY,
            w: srcW, h: srcH
        };

        var tween = new TWEEN.Tween(coords)
            .to({
                x: srcX - (w - srcW) / 2,
                y: srcY - (h - srcH) / 2,
                w: w,
                h: h
            }, span)
            .onUpdate(function () {
                me.setLocation(this.x,this.y);
                me.setSize(this.w, this.h);

            })
            .onComplete(function () {
                Mini2.TweenManager.remove(tween);
            })
            .easing(TWEEN.Easing.Quadratic.In);

        tween.muid = Mini2.newId();
            
        Mini2.TweenManager.add(tween);

        tween.start();


        return me;
    },

    setLocationTween:function(x,y){
        "use strict";
        var me = this,
            srcX = me.left,
            srcY = me.top;

        var coords = {
            x: srcX, y: srcY,
            opacity:0
        };

        var tween = new TWEEN.Tween(coords)
            .to({
                x: x,
                y: y,
                opacity: 1
            }, 500)
            .onUpdate(function () {

                me.setLocation(this.x, this.y);

                me.el.css('opacity', this.opacity)
            })
            .onComplete(function () {

                Mini2.TweenManager.remove(tween);

            })
            .easing(TWEEN.Easing.Quadratic.In);

        tween.muid = Mini2.newId();

        Mini2.TweenManager.add(tween);

        tween.start();


        return me;
    },

    setLocation: function (x, y, isInit) {
        "use strict";
        var me = this;


        if (!Mini2.isNumber(x)) {
            x = parseInt(x);
        }

        if (!Mini2.isNumber(y)) {
            y = parseInt(y);
        }

        if (isInit && ('center_screen' == me.startPosition || 'default' == me.startPosition)) {
            x = ($(window).width() - me.width) / 2;
            y = ($(window).height() - me.height) / 2;
        }

        if (y < 0) { y = 0; }

        me.left = x;
        me.top = y;


        me.el.css({ left: x, top: y });

        if (me.shadowVisible) {
            me.shadowEl.css({ left: x, top: y + 8 });
        }
        return me;
    },


    initGhost: function () {
        "use strict";
        var me = this;

        if (!me.ghostEl) {
            me.ghostEl = Mini2.$joinStr(me.ghostTpl);

            $(me.renderTo).append(me.ghostEl);


        }
    },


    setGhostLocation: function (x, y) {
        "use strict";
        var me = this,
            ghostEl = me.ghostEl;

        ghostEl.css({
            left: x,
            top: y
        });
        return me;
    },


    beginDrag: function () {
        "use strict";
        var me = this,
            ghostEl,
            el = me.el,
            zIndex = el.css('z-index');

        zIndex = parseInt(zIndex);

        me.initGhost();

        ghostEl = me.ghostEl;



        ghostEl.css({
            left: el.css('left'),
            top: el.css('top'),
            width: el.css('width'),
            height: el.css('height'),
            'z-index': zIndex + 100
        });

        ghostEl.muBind('mouseup', Mini2.emptyFn);

        ghostEl.show();

        if (me.shadowVisible) {
            me.shadowEl.hide();
        }

        el.hide();
        return me;
    },


    endDrag: function () {
        "use strict";
        var me = this,
            ghostEl = me.ghostEl,
            el = me.el,
            x, y;

        ghostEl.hide();

        x = ghostEl.css('left');
        y = ghostEl.css('top');

        me.setLocation(x, y);

        if (me.shadowVisible) {
            me.shadowEl.show();
        }

        el.show();

        return me;

    },

    

    /**
    * 显示蒙层
    **/
    renderMask: function () {
        "use strict";
        var me = this,
            el,
            maskEl,
            win = $(window);

        if (me.mode) {

            me.maskEl = maskEl = Mini2.$join(me.maskTpl);
            //maskEl.removeClass('mi-mask-hide');

            maskEl.css({
                width: win.width(),
                height: win.height(),
                'z-index': me.zIndex - 2
            });


            if (me.maskOpacity) {
                maskEl.css('opacity', me.maskOpacity);
                maskEl.css('filter', 'alpha(opacity=' + me.maskOpacity * 100 + ')');
            }

            me.renderTo.append(maskEl);

            //因为事件机制的不完善，加了两个事件，防止点击到底下 Window，触发了其他事件。
            maskEl.muBind('mousedown', Mini2.emptyFn);
            maskEl.muBind('mouseup', Mini2.emptyFn);
        }
        return me;

    },


    getText: function () {
        return this.text;
    },

    setText: function (value) {
        "use strict";
        var me = this;
        me.text = value;
        me.header.setText(value);
        return me;
    },

    //显示标题
    renderHeader: function (el) {
        "use strict";
        var me = this;
        if (me.headerVisible) {

            var header = me.header = Mini2.create('Mini2.ui.WindowHeader', {
                width: me.width,
                owner: me,
                renderTo: el,
                text: me.text,
                closeEnabled : me.closeEnabled
            });

            header.render();
            $(header).bind('action', me, me.header_Action);
        }
        return me;
    },

    //显示阴影
    renderShadow: function () {
        "use strict";
        var me = this;
        if (me.shadowVisible) {
            me.shadowEl = Mini2.$joinStr(me.shadowTpl);
            me.shadowEl.css('z-index', me.zIndex - 1);

            me.renderTo.append(me.shadowEl);

        }

        return me;
    },

    renderEl: function () {
        "use strict";
        var me = this,
            el,
            bodyEl,
            header;

        me.el = el = Mini2.$join(me.windwoBoxTpl);
        me.renderTo.append(el);

        el.mouseup(function (e) {

            Mini2.EventManager.stopEvent(e);

        });


        el.data('me', me);
        el.css('z-index', me.zIndex);


        me.bodyEl = bodyEl = Mini2.$join(me.bodyTpl);
        el.append(bodyEl);

        if (me.autoScroll && !me.iframe) {
            bodyEl.css('overflow', 'auto');
        }

        bodyEl.css('border-style', 'none');

        me.renderHeader(el);
        me.renderShadow();

        me.render_ButtonPanel(el);

        return me.el;
    },

    setUrl: function (url) {
        "use strict";
        var me = this,
            contentEl = me.contentEl;

        if (me.iframe) {
            contentEl.attr('src', me.url);
        }
    },

    getContentEl:function(){
        var me = this,
            contentEl;

        contentEl = Mini2.$joinStr(me.contentTpl);

        return contentEl;
    },

    renderContent: function () {
        "use strict";
        var me = this,
            contentEl,
            curMini2;

        me.contentEl = contentEl = me.getContentEl();
        me.bodyEl.append(contentEl);

        if (me.iframe) {


            contentEl.attr('muid', me.muid);

            if (!me.autoScroll) {
                contentEl.attr('scrolling', 'no');
            }

            //var win = contentEl[0].contentWindow;

            //win.ownerWindow = me;

            //curMini2 = win.Mini2;

            //if (curMini2 && curMini2.onwerPage) {
            //    curMini2.onwerPage.ownerWindow = me;
            //}

            contentEl.bind('load', { 'owner': me }, me.iframe_load);

            me.iFrameReady.call(me, contentEl[0], function () {

                //console.log("xxxxxxxxxx" + new Date().getTime());

                var win = contentEl[0].contentWindow;
                win.ownerWindow = me;

                curMini2 = win.Mini2;

                if (curMini2 && curMini2.onwerPage) {
                    curMini2.onwerPage.ownerWindow = me;
                }
            });

            if (me.url) {
                contentEl.attr('src', me.url);
            }
            else if (me.iframeContent) {

                var cw = contentEl[0].contentWindow;
                var doc = cw.document;

                doc.open();
                doc.write(me.iframeContent);
                doc.close();
            }

        }

        return contentEl;
    },

    iframe_load: function (e) {
        "use strict";
        var me = this,
            iframeId = $(me).attr('muid'),
            win = me.contentWindow,
            curMi,
            doc = win.document,
            body = doc.body;

        //console.log("iframe 加载完成" + new Date().getTime(), win);

        if (!win.ownerWindow) {

            //console.log("iframe 加载完成 2");

            win.ownerWindow = e.data['owner'];

            curMi = win.Mini2;

            if (curMi && curMi.onwerPage) {
                curMi.onwerPage.ownerWindow = e.data['owner'];
            }
        }
        //        body.attr('mi-owner-window', 'true');    //当做标记。
    },


    renderBase: function () {
        "use strict";
        var me = this,
            el,
            win = $(window);

        me.renderMask();
        el = me.renderEl();

        me.renderContent();



        if ('max' == me.state) {

            me.win_resize.call(me);

            win.resize(function () {
                me.win_resize.call(me);
            });

        }
        else {


            me.setSize(me.width, me.height);
            me.setLocation(me.left, me.top, true);
        }


        return el;
    },

    render: function () {
        "use strict";
        var me = this,
            el;

        el = me.renderBase();
        return el;
    },

    win_resize: function () {
        "use strict";
        var me = this,
            win = $(window),
            w = win.width() - 20,
            h = win.height() - 20;

        me.setSize(w, h);
        me.setLocation(10, 10, true);
    },

    setOpacity:function(value){
        var me = this,
            el = me.el;

        el.css({
            opacity: value
        });

        if (me.shadowVisible) {
            me.shadowEl.css({ opacity: value });
        }

        return me;
    },

    setOpacityTween: function (value) {
        "use strict";
        var me = this,
            el = me.el;

        if (!window.TWEEN) {

            el.css({ opacity: value });

            if (me.shadowVisible) {
                me.shadowEl.css({ opacity: value });
            }

            return;
        }

        var coords = {
            opacity: el.css('opacity')
        };

        var tween = new TWEEN.Tween(coords)
            .to({
                opacity: 1
            }, 300)
            .onUpdate(function () {
                el.css('opacity', this.opacity)

                if (me.shadowVisible) {
                    me.shadowEl.css({ opacity: this.opacity });
                }

            })
            .onComplete(function () {

                Mini2.TweenManager.remove(tween);

            })
            .easing(TWEEN.Easing.Quadratic.In);

        tween.muid = Mini2.newId();

        Mini2.TweenManager.add(tween);

        tween.start();

        return me;
    },

    firstShow: 0,


    /**
    * show 之前执行的函数
    */
    showBefore: Mini2.emptyFn,

    /**
    * show 之后执行的函数
    */
    showAfter: Mini2.emptyFn,

    //显示窗体
    show: function () {
        "use strict";
        var me = this,
            isTween = me.isTween,
            el = me.el,
            shadowEl;

        if (!this.el) {
            me.firstShow = 0;
            me.render();
        }
        else {
            me.firstShow++;
        }

        el = me.el;

        if (isTween) {
            me.setOpacity(0);
        }


        shadowEl = me.shadowEl;

        if (me.shadowVisible) {
            shadowEl.show();
        }

        me.showBefore();

        el.show();

        me.showAfter();

        if (isTween) {
            me.setOpacityTween(1);
        }

        me.focus();

        return me;
    },


    //隐藏窗体，不关闭
    hide: function () {
        "use strict";
        var me = this,
            el = me.el,
            shadowEl = me.shadowEl,
            maskEl = me.maskEl;

        if (el) {
            el.hide();
        }

        if (me.shadowVisible) {
            shadowEl.hide();
        }

        if (me.mode) {


            if (maskEl[0].addEventListener) {

                maskEl[0].addEventListener("webkitAnimationStart", function () {
                    me.isCartoonEnd = true;
                });

                maskEl[0].addEventListener("webkitAnimationEnd", function () {
                    if (me.isDispose) {
                        maskEl.remove();
                    }
                });



                //如果 100 毫秒内没有开始动画, 那么直接销毁掉
                Mini2.setTimer(function () {
                    if (me.isCartoonEnd) {
                        return false;
                    }

                    if (me.isDispose) {
                        maskEl.remove();
                        return false;
                    }
                }, {
                    limit: 1,
                    overtime: 1000,
                    interval: 100,
                });

            }
            else {

                Mini2.setTimer(function () {
                    if (me.isDispose) {
                        maskEl.remove();
                        return false;
                    }
                }, {
                    limit: 1,       //只执行一次
                    interval: 300
                });
            }


            maskEl.addClass('mi-mask-hide');


        }

        return me;
    },

    // This function ONLY works for iFrames of the same origin as their parent
    iFrameReady: function(iFrame, fn) {
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



    //关闭窗体
    close: function (e) {
        "use strict";
        var me = this,
            cancel;

        e = e || {};

        cancel = me.trigger('formClosing', e);

        if (!cancel) {
            me.trigger('formClosed', e);

            me.hide();

            me.dispose();
        }

        return me;
    },


    isDispose : false,

    //释放内存
    dispose: function () {
        "use strict";
        var me = this,
            el = me.el,
            shadowEl = me.shadowEl,
            mask = me.maskEl;

        me.isDispose = true;

        //if (mask) {
        //    if (mask.hasClass('mi-mask-hide')) {
        //        setTimeout(function () {
        //            mask.remove();
        //        }, 300);
        //    }
        //    else {
        //        mask.remove();
        //    }
        //}

        if (el) { el.remove(); }
        if (shadowEl) { shadowEl.remove(); }
        if (me.ghostEl) { me.ghostEl.remove(); }

        Mini2.ui.WindowManager.removeWin(me);
    }

});




/**
* 拖拽
*
*
*/
Mini2.define('Mini2.ui.Draggable', {

    el: null,

    srcX: 0,
    srcY: 0,

    srcLocation:null,

    bind: function () {

        "use strict";
        var me = this,
            el = me.el;

        el.muBind('mousedown', me, function (e) {

            me.srcLocation = $(el).offset();

            me.srcX = event.pageX;
            me.srcY = event.pageY;

            el.focus();

            console.log("ddddddddddd");

        })
        .muBind('mouseup', me, function (e) {

        })
        .muBind('mousemove', me, function (e) {

            var offsetX = event.pageX - me.srcX;
            var offsetY = event.pageY - me.srcY;

            var x = offsetX + me.srcLocation.left;
            var y = offsetY + me.srcLocation.top;

            if (!me.isDrag) {
                if (Math.abs(offsetX) >= 4 || Math.abs(offsetY) >= 4) {
                    me.isDrag = true;

                }
            }
            else if (me.isDrag) {
                el.css({
                    'left': x,
                    'top': y
                });
                
                //me.owner.setGhostLocation(x, y);
            }
            
        });

    }

});

