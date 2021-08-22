
/// <reference path="define.js" />
/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />

//系统信息
Mini2.define("Mini2.SystemInfo", {

    singleton: true,

    mode : 'default',

    //表单
    form: {

        //下一个键
        tabKeyCode: 13,

        //元素的间距
        itemMarginBottom: 8,
        
        //元素的间距
        itemMarginRight: 8

    },

    //表单触发器的按钮
    formTrigger:{

        width: 22
        
    },

    //工具栏
    toolbar: {
        //高度
        height: 36,

        item_span: 4,

        //普通按钮组
        uiType_buttonGroup: {
            height: 36,
            item_span : 0
        },

        //大扁平模式
        uiType_largeFlat: {
            height: 36,
            item_span: 0
        }
    },

    //表格
    table: {

        //表格列头宽度的比率
        headerWidthRaed: 1,

        headerHeight: 31,
        rowHeight: 31,

        //底部高度
        footerHeight: 36,

        headerAlign: 'center',

        columnLines: true,

        focusBorders: true,

        cols: {

            rowNumberer: {
                width: 40
            },

        }
    },


    window: {
        headerHeight: 24,
        footerHeight: 36,

        contentHeight: 70,

        //消息框高度
        msgHeight : 180
    },

    list:{

        itemHeight: 24

    },

    //字体
    font: {

        //字体尺寸
        size: 12,

        //半角字符宽度
        chatWidth: 6
    },

    //工具栏
    tabPanel: {
        //标签页高度
        tabHeight: 32
    },

    //分页控件
    pagination:{

        height: 24
    },


    //菜单
    menu:{

        paddingTop: 3,
        paddingBottom:3

    },

    modeList:false,

    'defaultMode':false,

    //设置模式
    setMode: function (mode) {
        "use strict";
        var me = this,
            modeFn;

        if (!me.modeList) {

            me.defaultMode = me.computeMode;

            me.modeList = {
                "default": me.computeMode,
                "compute": me.computeMode,
                "touch": me.touchMode,
                "mobile": me.mobileMode,
                "compact": me.compact,   //紧凑版
                "bootstrap": me.bootstrap,
                "triton2": me.triton2,
            }

        }
        
        modeFn = me.modeList[mode];

        if (modeFn) {
            modeFn.call(me);
        }
        else {
            me.defaultMode.call(me);
        }
    },


    //紧凑版
    compactMode:function(){
        "use strict";
        var me = this;

        me.toolbar.height = 24;

        Mini2.apply(me.table, {
            headerHeight: 24,
            rowHeight: 24,
            headerWidthRaed: 0.8,
            footerHeight: 24
        });
    },

    //Bootstrap
    bootstrap : function(){
        "use strict";
        var me = this;

        me.mode = 'bootstrap';

        console.debug('切换 SystemInfo.mode 为 ', Mini2.SystemInfo.mode);


        me.toolbar.height = 44;
    },

    triton2: function () {
        "use strict";
        var me = this;

        me.mode = 'triton2';

        Mini2.apply(me.table, {
            columnLines: false,
            focusBorders: false,
            headerWidthRaed: 1.1,
            headerHeight: 52,
            rowHeight:52
        });

        Mini2.apply(me.toolbar, {
            height:44,
            item_span: 0,
        });

        //普通按钮组
        Mini2.apply(me.toolbar.uiType_buttonGroup, {
            height: 34,
            item_span: 0
        });

        //大扁平模式
        Mini2.apply(me.toolbar.uiType_largeFlat, {
            height: 44,
            item_span: 0
        });

        Mini2.apply(me.pagination, {
            height: 45
        });
    },
    
    //电脑模式
    computeMode:function(){
        var me = this;

        
    },
    
    //平板模式(默认模式);
    touchMode: function () {
        "use strict";
        var me = this;

        me.mode = 'touch';

        console.debug('切换 SystemInfo.mode 为 ', Mini2.SystemInfo.mode);

        Mini2.apply(me.toolbar, {
            height: 60
        });

        Mini2.apply(me.table, {
            headerWidthRaed: 1,
            footerHeight: 37,
            headerHeight: 37,
            rowHeight : 37
        });


        Mini2.apply(me.table.cols.rowNumberer, {

            width : 60

        });

        
        Mini2.apply(me.window, {
            footerHeight: 52,
            contentHeight: 70,
            msgHeight: 180,
            itemHeight:46
        });

        Mini2.apply(me.list, {
            itemHeight: 46
        });

        Mini2.apply(me.tabPanel, {
            tabHeight: 42
        });

        Mini2.apply(me.pagination, {
            height: 50
        });

        Mini2.apply(me.form, {

            //元素的间距
            itemMarginBottom: 15,

            //元素的间距
            itemMarginRight: 15

        });

        Mini2.apply(me.formTrigger, {
            width: 38
        });
    },

    //手机模式
    mobileMode: function () {

    }

}, function () {

    var me = this;




});