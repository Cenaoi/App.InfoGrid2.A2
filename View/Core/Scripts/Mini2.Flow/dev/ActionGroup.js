

/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

//容器
Mini2.define('Mini2.flow.ActionGroup', {

    extend: 'Mini2.flow.Action',

    isContainer: true, // 容器的标记



    initComponent: function () {
        var me = this;

        me.initComponentBase();

        me.initSub();


    },


    groupBox: null,


    setSize: function (width, height) {
        "use strict";
        var me = this,
            groupBox = me.groupBox;

        if (undefined != width) {
            me.width = width;
        }

        if (undefined != height) {
            me.height = height;
        }

        groupBox.setY(30);
        groupBox.setSize(width - 8, height - 30);


        me.drawReset();

        return me;
    },


    setRect: function (x, y, width, height) {
        "use strict";
        var me = this,
            base = me.constructor.base,
            el = me.el,
            groupBox = me.groupBox;

        base.setRect.call(me, x, y, width, height)

        groupBox.setY(30);
        groupBox.setSize(width - 8, height - 30);


        return me;
    },


    initSub: function () {
        "use strict";
        var me = this,
            el = me.el,
            groupBox;

        me.groupBox = groupBox = Mini2.create('Mini2.flow.ActionContainer', {
            x: 4,
            y: 30,
            width: me.width - 8,
            height: me.height - 30,
            visible: true,
            maskAlpha : 0.1
        });

        groupBox.render();

        groupBox.drawReset();
        el.addChild(groupBox.el);


    },

    
    //测试坐标在那个组内
    testRollover: function (x, y, oldPanel, dragTarget) {
        "use strict";
        var me = this,
            el = me.el,
            groupBox = me.groupBox,
            setX = x - me.x - groupBox.x,
            setY = y - me.y - groupBox.y;

        if (dragTarget) {
            dragTarget.setX = setX;
            dragTarget.setY = setY;
        }

        if (oldPanel) {
            if (oldPanel.hitTest(setX, setY)) {
                return oldPanel;
            }
        }

        if (groupBox.hitTest(setX, setY)) {
            return groupBox;
        }

        return null;
    },


    //隐藏组的蒙层
    //容器
    hideGroupMask: function (cont) {
        "use strict";
        var me = this,
            el = me.el,
            groupBox = me.groupBox;

        console.log('半透明容器...');

        cont.setMaskAlpha(0.2);
        
    },

    //显示组的蒙层
    //容器
    showGroupMask: function (cont) {
        "use strict";
        var me = this,
            el = me.el,
            groupBox = me.groupBox;

        console.log('显示容器...');
            
        cont.setMaskAlpha(1);
       
    }




}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});