/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="define.js" />



Mini2.EventManager = new function () {
    "use strict";
    /// <summary>事件管理器</summary>

    var EventManager = this,
        doc = document,
        win = window;

    Mini2.apply(EventManager, {

        log: Mini2.Logger.getLogger('Mini2.EventManager'),  //日志管理器

        //控件
        con: null,

        //范围
        scope: null,

        data: null,

        //拖动
        drag: false,

        //鼠标最后动作时间
        lastTimeMouseAction: 0,

        //键盘最后动作时间
        lastTimeKeyAction: 0,

        //全部最后动作时间
        lastTimeAction: 0,




        /**
        * 设置鼠标最后执行时间
        */
        setMouseAction: function () {
            var me = this;
            me.lastTimeMouseAction = me.lastTimeAction = Mini2.Date.now();

        },

        //设置键盘最后执行时间
        setKeyAction:function(){
            var me = this;
            me.lastTimeKeyAction = me.lastTimeAction = Mini2.Date.now();

        },

        //设置活动
        setAction:function(){
            var me = this;
            me.lastTimeAction = Mini2.Date.now();

        },


        //是否活动状态
        isAction: function (timeSpan) {
            "use strict";
            var me = this,
                isInput = Mini2.ui.FocusMgr.isInputting(),
                lastTime = me.lastTimeAction,
                span;

            if (isInput) { return true; }
            
            timeSpan = timeSpan || 0;

            span = Mini2.Date.now() - lastTime - timeSpan;

            return (span <= 0);
        },


        stopEvent: function (e) {

            //log.begin("stopEvent(e)");
            e = e || window.event;

            if (e) {
                this.stopPropagation(e);
                this.preventDefault(e);
            }

            //log.end();
        },

        stopPropagation: function (e) {

            //log.debug("stopPropagation(e)");

            e = e || window.event;

            if (e.stopPropagation) {
                e.stopPropagation();
            }
            else {
                e.cancelBubble = false;
            }
        },

        preventDefault: function (e) {

            //log.debug("preventDefault(e)");

            e = e || window.event;

            if (e.preventDefault) {
                e.preventDefault();

            } else {
                e.returnValue = false;
            }
        },

        withinOne: function (el, related, allowEl) {
            "use strict";
            var me = this,
                objs ;

            if (allowEl && allowEl.el && typeof allowEl == 'object') {
                allowEl = allowEl.el;
            }


            if (allowEl) {

                if (el == allowEl || el === allowEl) {
                    return true;
                }

                //console.log("objs.length = ?");

                objs = $(allowEl).find(el);

                //console.log("objs.length = " + objs.length);

                return (objs.length > 0);
            }
        },

        within: function (el, related, allowEl) {
            "use strict";
            var me = this;

            if (el && el.el && typeof el == 'object') {
                el = el.el;
            }

            if (el) {

                if (allowEl instanceof Array) {

                    var allowEls = allowEl,
                        i = 0,
                        subEl,
                        isWithin;

                    for (; i < allowEls.length; i++) {

                        subEl = allowEls[i];

                        isWithin = me.withinOne(el, related, subEl);

                        if (isWithin) {
                            return true;
                        }
                    }

                }
                else {
                    return me.withinOne(el, related, allowEl);
                }
            }


            return false;

        },

        //查找父级范围
        offsetParent: function (elem) {
            "use strict";
            var el = $(elem),
                cls = 'mi-border-box',
                i = 0;

            if (el.hasClass(cls)) {
                return elem;
            }

            for (; i < 99; i++) {

                el = el.parent();

                if (el.hasClass(cls)) {

                    break;
                }
            }

            return el;
        },

        getBorderBox: function (con) {
            "use strict";
            var me = this,
                log = me.log;

            if (con == undefined || con == null) {
                return me;
            }

            if (con.toString() == '[object Window]') {
                con = window.document.body;
            }

            if (con && con.el) {

                if (con.el.jquery) {
                    con = con.el.get(0);
                }
                else if (Mini2.isDom(con.el)) {
                    con = con.el;
                }
            }

            if (!Mini2.isDom(con)) {
                return me;
            }

            var borderBox;


            if ($(con).hasClass("mi-border-box")) {
                //log.debug("自身是 mi-border-box");
                borderBox = $(con);
            }
            else {
                borderBox = me.offsetParent(con);
            }

            var eventMgr = null;

            if (borderBox.length) {
                eventMgr = borderBox.data("EVENT_MANAGER");

                if (!eventMgr) {

                    eventMgr = {
                        control: null,
                        data: null,
                        scope: null,
                        el: borderBox.get(0),
                        typeString: borderBox.get(0).toString(),
                        muid: "F_" + Mini2.getIdentity()
                    };

                    //log.debug("创建 eventMgr = " + eventMgr);
                    //$.data(borderBox, 'EVENT_MANAGER', eventMgr); 
                    borderBox.data("EVENT_MANAGER", eventMgr);
                }

                //log.end();
                return eventMgr;
            }

            //log.end();

            return me;
        },

        //当前控件
        curControl: function () {

            var me = this,
                mm = me.getBorderBox(con);

            return mm.con;
        },

        //清理
        clear: function (con) {

            //console.log("EventManager.clear()");

            var me = this,
                mm = me.getBorderBox(con);

            mm.con = null;
            mm.scope = null;

        },

        //设置有效范围
        setScope: function (con, scope) {
            var me = this,
                mm = me.getBorderBox(con);

            //console.log("setScope(); " + "scope = " + scope);

            mm.con = con;
            mm.scope = scope;
        },


        setKeyDown: function (con, e, scope) {

            var me = this,
                mm = this.getBorderBox(con);

            me.setKeyAction();

            if (mm.con != null) {
                return;
            }


            //console.log("EventManager.setKeyDown();  me.scope = " + me.scope);

            mm.con = con;
            mm.scope = scope;

            $(con).trigger('keydown', e);

            me.stopEvent(e);
        },

        setKeyUp: function (con, e) {
            "use strict";
            var me = this,
                allow,
                mm = this.getBorderBox(con);

            me.setKeyAction();

            allow = me.within(con, null, me.scope);




            if (allow) {

                $(con).trigger('keyup', e);
            }
            else {
                Mini2.ui.FocusMgr.setControl(con);
            }

            mm.con = null;

            me.stopEvent(e);

            //log.end();
        },

        setMouseDown: function (con, data, scope, event, continueEvent) {

            var me = this,
                log = me.log,
                mm = this.getBorderBox(scope || con);


            me.mousedownControl = con;

            me.setMouseAction();

            me.borderBox = mm;
            mm.mouseUp_repetition = 'no';

            if (mm.con != null) {
                return;
            }

            if (scope == undefined) {
                scope = con;
            }

            //log.begin("EventMgr.setMouseDown()");

            mm.con = con;
            mm.scope = scope;
            mm.data = data;


            if (con && !continueEvent) {
                me.stopEvent(event);
            }

            //log.end();
        },

        borderBox: null,

        setMouseMove: function (con, evt) {

            var me = this,
                log = me.log,
                mm = me.borderBox || this.getBorderBox(con),
                lastCon = mm.con,
                lastScope = mm.scope;

            me.setMouseAction();

            //log.debug('-------mouseUp :' + event.pageX);



            lastCon = lastCon || con;

            $(lastCon).triggerHandler('mu_mousemove', [evt, con]);
            
            //me.stopEvent(event);

        },

        setMouseUp: function (con, evt) {


            var me = this,
                log = me.log,
                con = con || me.mousedownControl,
                mm = me.borderBox || this.getBorderBox(con),
                lastCon = mm.con,
                lastScope = mm.scope;


            me.mousedownControl = null;

            me.setMouseAction();

            if (mm.con && mm.mouseUp_repetition == 'no') {
                mm.mouseUp_repetition = 'yes';

                //log.begin("EventMgr.setMouseUp()");

                $(mm.con).triggerHandler('mu_mouseup', [evt, con]);

                mm.con = null;
                mm.data = null;

                if (evt == null) {
                    log.error("没有事件啊", evt);
                }

                me.stopPropagation(evt);

                me.borderBox = null;

                //log.end();
            }


        },

        addListener: function (element, eventName, fn, scope, options) {


        },

        removeListener: function (element, eventName, fn, scope) {

        },

        //tab 按键触发
        tab: function (curTab) {
            var parent = curTab.ownerParent,
                nextItem = null;


            if (!parent) {
                return nextItem;
            }

            var pItems = parent.getTabItems();  //获取允许带 Tab 对象的


            var pLen = pItems.length;

            var curTabIndex = -1;

            for (var i = 0; i < pLen; i++) {
                var item = pItems[i];

                if (item === curTab) {
                    curTabIndex = i;

                    break;
                }
            }

            if (curTabIndex >= 0) {


                var nextTabIndex = curTabIndex + 1;

                if (nextTabIndex >= pLen) {
                    parent.focus();
                    return;
                }

                nextItem = pItems[nextTabIndex];

                try {

                    nextItem.focus();

                    //如果文本框，自动选择全部
                    if (nextItem.select) {
                        nextItem.select();
                    }
                }
                catch (ex) {
                    console.error('焦点属性赋值错误.', item);
                    console.log('pLen', pLen);
                }

            }

            return nextItem;
        }

    });



};

$(document).on('keydown',function (e) {
    "use strict";
    var keyCode = e.keyCode,
        log = Mini2.Logger,
        foucsCon = Mini2.ui.FocusMgr.lastActionControl,
        tagName = e.target.tagName,
        eventMgr = Mini2.EventManager,
        tabKeyCode = Mini2.SystemInfo.form.tabKeyCode || 9;


    eventMgr.setKeyAction();

    //console.log('foucsCon=', foucsCon);

    //console.log($.format('tagName={0}, keyCode={1}', tagName , keyCode));

    if (tagName == 'TEXTAREA') {
        return;
    }

    
    if (keyCode == tabKeyCode || keyCode === 9) {
        //eventMgr.stopEvent(e);

        return false;
    }

    //if (keyCode == tabKeyCode) {
    //    var data = Mini2.ui.FocusMgr.data;
               
        
    //    if (foucsCon != null) {

    //        var ea = {
    //            data: data,
    //            cancel: false
    //        };

    //        $(foucsCon).triggerHandler('keydown', ea);

    //        if (ea.cancel) {
    //            eventMgr.stopEvent(e);
    //            return;
    //        }
    //    }
        
    //    eventMgr.stopEvent(e);

    //    var nextCon = eventMgr.tab(foucsCon);
               
    //    if (nextCon) {
    //        return;
    //    }
    //}
    //else if (keyCode == 13) {

    //    eventMgr.stopEvent(e);
    //}


    /***************************************************************/

    //console.log("xxxxxxxxxxxxxxxxxxxxxxxxxx");


});


$(document).bind('keyup', function (e) {
    "use strict";
    var keyCode = e.keyCode,
        log = Mini2.Logger,
        foucsCon = Mini2.ui.FocusMgr.lastActionControl,
        tagName = e.target.tagName,
        eventMgr = Mini2.EventManager,
        tabKeyCode = Mini2.SystemInfo.form.tabKeyCode || 9;


    eventMgr.setKeyAction();

    if (tagName == 'TEXTAREA') {
        return;
    }


    if (keyCode == tabKeyCode || keyCode === 9) {
        var data = Mini2.ui.FocusMgr.data;


        if (foucsCon != null) {

            var ea = {
                data: data,
                cancel: false
            };

            $(foucsCon).triggerHandler('keyup', ea);

            if (ea.cancel) {
                eventMgr.stopEvent(e);
                return;
            }
        }

        eventMgr.stopEvent(e);

        var nextCon = eventMgr.tab(foucsCon);

        if (nextCon) {
            return;
        }
    }
   

    //////////////////////////////////

    var conME = foucsCon;

    var conEl = conEl;

    if (conME && conME.muid) {

    }
    else {

        var conEl = foucsCon;

        for (var i = 0; i < 99; i++) {

            conME = $(conEl).data('me');

            if (conME && conME.muid) {

                break;
            }

            conEl = $(conEl).parent();

        }
    }

    //log.debug("事件 conME.proKeyEvent = " + conME.proKeyEvent);
    //console.log(conME);

    if (conME && conME.proKeyEvent) {

        conME.proKeyEvent.call(conME, e);

    }

});


$(window)
.mousedown(function (e) {
    var eventMgr = Mini2.EventManager;

    eventMgr.setMouseAction();
})
.mouseup(function (e) {
    
    var keyCode = e.keyCode,
        foucsCon = Mini2.ui.FocusMgr.control,
        target = e.target,
        tagName = target.tagName,
        eventMgr = Mini2.EventManager;
    
    if (target && $(target).hasClass('mi-mask')) {
        eventMgr.stopEvent(e);
        return;
    }

    if (eventMgr.mousedownControl) {
        Mini2.ui.FocusMgr.setControl(eventMgr.mousedownControl);
    }
    else {
        Mini2.ui.FocusMgr.setControl(this);
    }

    eventMgr.setMouseUp(null, e);

    if (Mini2.ui && Mini2.ui.menu && Mini2.ui.menu.Manager) {
        Mini2.ui.menu.Manager.hideAll();
    }
})
.mousemove(function (e) {
    var me = this,
        eventMgr = Mini2.EventManager;

    eventMgr.setMouseMove(this, e);

});