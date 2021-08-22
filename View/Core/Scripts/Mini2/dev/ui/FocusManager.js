/// <reference path="define.js" />
/// <reference path="../Mini.js" />
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />

//焦点管理
Mini2.define("Mini2.ui.FocusManager", {


    //单件模式
    singleton: true,

    log: Mini2.Logger.getLogger('Mini2.ui.FocusManager', false),

    //别名
    alternateClassName: ['Mini2.ui.FocusMgr'],

    //控件
    control: null,


    //附带数据
    data: null,

    scope: null,

    el: null,

    //最后光标所在位置
    lastActionControl: null,
    lastActionTime:Mini2.Date.now(),


    //设置焦点判断
    setActionControl: function (con) {
        "use strict";
        var me = this;
        me.lastActionControl = con;
        me.lastActionTime = Mini2.Date.now();
    },

    //是否用户输入中.
    isInputting: function () {
        "use strict";
        var me = this,
            isInput,
            span,
            con = me.lastActionControl;

        isInput = (con && con.isInput) || false;

        //如果超过20秒还没有进行输入,就证明这个界面没有用户在进行操作..
        if (isInput) {

            span = Mini2.Date.now() - me.lastActionTime;

            //log.debug("超时:" + span);

            if (span > 20000) {
                isInput = false;
            }
        }

        return isInput;
    },

    offsetParent: function (elem) {
        "use strict";
        var el = $(elem),
            cls = 'mi-border-box',
            i = 0;

        if (el.hasClass(cls)) {
            return elem;
        }

        for (i; i < 999; i++) {

            el = el.parent();

            if (el.hasClass(cls)) {

                break;
            }

        }

        return el;
    },

    getBorderBox: function (con) {
        "use strict";
        var me = this;

        if (con == undefined) {
            return me;
        }

        if (con.toString() == '[object Window]') {
            con = window.document.body;

        }

        if (con.el) {
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

        //log.debug("con Type = " + con.toString());

        //if (con.toString() == "[object HTMLDivElement]") {
        //log.debug($(con).html());
        //}

        if ($(con).hasClass("mi-border-box")) {
            borderBox = $(con);
        }
        else {
            borderBox = me.offsetParent(con);
        }


        var eventMgr = null;

        if (borderBox.length) {
            eventMgr = borderBox.data("FOCUS_MANAGER");

            if (!eventMgr) {

                eventMgr = {
                    control: null,
                    data: null,
                    scope: null,
                    el: borderBox.get(0),
                    typeString: borderBox.get(0).toString(),
                    muid: "F_" + Mini2.getIdentity()
                };

                borderBox.data("FOCUS_MANAGER", eventMgr);

//                log.debug("创建焦点管理器");
            }

            return eventMgr;
        }

        return me;
    },

    clear: function (con) {
        var me = this,
            mm = me.getBorderBox(con);

        mm.data = null;
        mm.scope = null;
        mm.control = null;
    },


    setControl: function (con, data, scope, isReset) {
        "use strict";
        /// <summary>设置控件焦点</summary>
        /// <param name="con" type="Control">UI 控件</param>
        /// <param name="data" type="object">附带对象</param>
        /// <param name="isReset" type="bool" >强制重置焦点控件</param>

        var me = this,
            log = me.log,
            mm = me.getBorderBox(con),
            lastCon = mm.control,
            lastData = mm.data;

        if (con) {
            
            if (con.getType) {
                log.debug(con.getType().fullName);
            }
        }


        //if (undefined == scope) { scope = con; }

        //if (undefined == data) {
        //    data = null;
        //}



        //scope = scope || con;

        con = scope || con;

        data = data || null;


        if (isReset != true) {
            if (lastCon == con && lastData == data) {

                log.debug('对比控件相同, 退出');
                return;
            }
        }

        log.debug('lastCon', lastCon);

        log.debug("cur con : " , con);
        

        if (lastCon) {

            //var lastMM = me.getBorderBox(lastCon);
            log.debug("触发控件移出的事件");

            $(lastCon).muTriggerHandler('focusout', [{
                sender: lastCon,    //原焦点控件
                target: con,        //新获取焦点控件
                data: lastData      //原附带的数据
            }]);

        }


        mm.control = con;
        mm.data = data;


        me.setActionControl(con);   // 最后选中的控件

        if (con) {
            //            log.debug("调用事件 con.focusin");

            $(con).triggerHandler('focusin');
        }


        //        log.end();
    }



});

