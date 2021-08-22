/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

Mini2.define('Mini2.flow.Tooltip', {

    extend: 'Mini2.flow.FlowNode',



    width: 100,

    height: 40,

    x: 0,

    y: 0,

    mainObj: null,

    txtObj: null,

    boxObj: null,


    //状态
    state: 0,



    //事件集
    //eventSet:{
    //    //'mousedown':[],
    //    //'
    //},


    //当前样式
    curStyle: null,


    style: {

        //状态0 ,橘色-未处理
        state_0: {
            //背景颜色
            back_color: '#FFF799',

            back_color_over: '#ffb28c',

            //文字颜色
            font_color: '#000000',

            //边框大小
            border_width: 1,

            //边框类型
            border_style: 'solid', //solid-实线, dashed-虚线
        }


    },



    defaultRect: {
        x: 0,
        y: 0,
        w: 100,
        h: 50
    },



    initComponent: function () {


    },

    

    setText: function (value) {
        "use strict";
        var me = this,
            oldText = me.oldText,
            txtObj = me.txtObj;

        if (oldText == value) {
            return;
        }

        me.oldText = value;

        if (value) {

            txtObj.text = value;

            var sp = value.split('\n');

            var rowCount = sp.length;
            var maxLen = 0;

            for (var i = 0; i < sp.length; i++) {

                var curLen = me.getTextLen(sp[i]);
                maxLen = Math.max(maxLen, curLen);

            }

            var chatWieth = 8.2;

            me.width = maxLen * chatWieth + 10;
            me.height = rowCount * chatWieth * 2.6 + 10;


        }
        else {
            txtObj.text = '';

            me.width = 0;
            me.height = 0;
        }

        me.drawReset();
    },

    getText: function () {
        "use strict";
        var me = this,
            txtObj = me.txtObj;

        return txtObj.text;
    },


    setXY: function (x, y) {
        "use strict";
        var me = this,
            el = me.el;

        me.x = el.x =  x;
        me.y = el.y = y;
    },


    createTextObj: function () {
        "use strict";
        var me = this,
            style = me.curStyle || me.style.state_0,
            textObj,
            g;

        textObj = new createjs.Text(me.text, '12pt 微软雅黑', style.font_color);

        textObj.x = 5;
        textObj.y = 5;
        //textObj.width = me.width;
        //textObj.height = me.height;
        //textObj.textAlign = 'center';
        //textObj.textBaseline = 'middle';

        //textObj.setBounds(0, 0, me.width, me.height);

        return textObj;
    },

    //获取状态
    setState: function (value) {
        "use strict";
        var me = this,
            state = me.state = value,
            style;

        if (0 == state) {
            style = me.style.state_0;
        }


        me.curStyle = style;

        me.drawReset();
    },

    //获取状态
    getState: function () {
        var me = this,
            state = me.state;

        return state;
    },

    createBoxObj: function () {
        "use strict";
        var me = this,
            state = me.state,
            boxObj,
            g;

        boxObj = new createjs.Shape();

        return boxObj;
    },


    drawReset: function () {
        "use strict";
        var me = this,
            g,
            style = me.curStyle,
            boxObj = me.boxObj,
            txtObj = me.txtObj;


        g = boxObj.graphics;

        boxObj.x = 0;
        boxObj.y = 0;

        g.clear();

        g.setStrokeStyle(style.border_width);

        if ('dashed' == style.border_style) {
            g.setStrokeDash([3, 3]);
        }

        g.beginStroke(createjs.Graphics.getRGB(202, 201, 199, 1));
        g.beginFill(style.back_color)
            .drawRoundRect(0, 0, me.width, me.height, 4);


        txtObj.color = style.font_color;
    },

   
    show: function () {
        "use strict";
        var me = this,
            el = me.el;

        if (me.width <= 0 || me.height <= 0) {
            el.visible = false;
        }
        else {
            el.visible = true;
        }
    },


    hide: function () {
        "use strict";
        var me = this,
            el = me.el;

        el.visible = false;
    },


    render: function () {
        "use strict";
        var me = this,
            el,
            mainObj,
            boxObj,
            txtObj,
            srcRect = me._srcRect = me._srcRect || me.defaultRect;


        me.el = me.mainObj = mainObj = new createjs.Container();
        

        me.boxObj = boxObj = me.createBoxObj();


        me.txtObj = txtObj = me.createTextObj();


        mainObj.addChild(boxObj, txtObj);

        me.setState(me.state);



        me.page.addWindow(me);

    },
   
    //获取 Text 的长度
    getTextLen: function (str) {
        "use strict";
        ///<summary>获得字符串实际长度，中文2，英文1</summary>
        ///<param name="str">要获得长度的字符串</param>

        var realLength = 0,
            len = str.length,
            charCode = -1;

        for (var i = 0; i < len; i++) {
            charCode = str.charCodeAt(i);
            if (charCode >= 0 && charCode <= 128) {
                realLength += 1;
            }
            else {
                realLength += 2;
            }
        }

        return realLength;
    }



}, function () {
    "use strict";
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});