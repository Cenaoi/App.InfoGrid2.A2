/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../Mini.js" />


Mini2.define('Mini2.ui.toolbar.Toolbar', {
    extend: 'Mini2.ui.panel.Panel',

    baseCls: Mini2.baseCSSPrefix + 'toolbar',


    defaultType: 'button',

    items: [],

    height: 24,

    region: 'north',

    scroll: 'none',

    userCls: null,

    /**
     * ui 类型, 有 large_flat=大扁平, button_group=小按钮组
     */ 
    uiType : 'default',

    item_span: 4,  //按钮之间的间隔距离

    padding:{
        all:'4px'
    },

    initComponent: function () {
        "use strict";
        var me = this;

    },


    
    //删除
    remove: function(item){
        'use strict';

        var me = this,
            items = me.items,
            i, index,
            del_items;

        if (Mini2.isArray(item)) {
            del_items = item;
        }
        else {
            del_items = [item];
        }

        for (i = 0; i < del_items.length; i++) {
            item = del_items[i];
            item.el.remove();

            index = Mini2.Array.remove(items,item);
        }

        return me;
    },

    createItem: function (cfg) {
        "use strict";
        var me = this,
            item,
            cType ,
            ns = 'Mini2.ui.toolbar',
            fullname;

        cfg = Mini2.apply(cfg, {

            ownerParent: me,
            toolbar: me,
            renderTo: me.boxTarget
        });


        cType = cfg.type;

        if ('title' == cType) {
            fullname = ns + '.Title';
        }
        else if ('hr' == cType) {
            fullname = ns + '.Hr';
        }
        else if ('template' == cType) {
            fullname = ns + '.Template';
        }
        else if ('group' == cType) {
            fullname = ns + '.Group';
        }
        else {
            fullname = ns + '.Button';
        }

        item = Mini2.create(fullname, cfg);
        item.render();

        if (item.el && item.el.width) {
            item.width = item.el.width() + 6;
        }
        else {
            item.width = item.getWidth() + 6;
        }


        return item;
    },

    //添加
    add: function (item) {
        var me = this,
            items = me.items;

        if (!item) {
            throw new Error('item 对象不能为 ' + item);
        }

        if ('-' == item) {
            var hr = Mini2.create('Mini2.ui.toolbar.Hr', {});

            item = hr;
        }

        item.toolbar = me,
        item.renderTo = me.boxTarget

        items.push(item);

        item.render();

        return item;
    },

    renderItems: function () {
        "use strict";
        var me = this,
            i,
            item,
            items = me.items,
            len = items.length,
            newItems = [];

        delete me.items;

        for (i = 0; i < len; i++) {
            item = me.createItem(items[i]);
            newItems[i] = item;

            if (item.id) {
                Mini2.onwerPage.controls[item.id] = item;
                window[item.id] = item;
            }
        }

        me.items = newItems;
    },


    getBtnWidth: function (w, btns) {
        "use strict";
        var me = this,
            i = btns.length,
            item_span = me.item_span ;

        while (i--) {
            w += btns[i].getWidth() + item_span;
        }

        return w;
    },

    setItemsLayout: function (x, items) {
        "use strict";
        var me = this,
            i,
            item,
            item_span = me.item_span ,
            len = items.length;

        for (i = 0; i < len; i++) {
            item = items[i];

            if (item.visible) {
                item.setLeft(x);

                if (item.updateLayout) {
                    item.updateLayout();
                }

                x += item.getWidth() + item_span;
            }
            else {
                //console.warn('未知的工具栏项', item);
            }
        }

    },



    /**
    * 更新布局
    */
    updateLayout: function () {
        "use strict";
        var me = this;

        Mini2.awaitRun(me.muid, 50, me, function () {

            me.baseUpdateLayout();

        });

        return me;
    },


    /**
    * (基础函数)更新布局
    */
    baseUpdateLayout: function () {
        "use strict";
        var me = this,
            item,
            w, i, itemAllW,
            item_span = me.item_span ,
            items = me.items;


        if (items) {
            i = items.length;

            var lItems = [],
                rItems = [],
                cItems = [];

            while (i--) {
                item = items[i];

                switch (item.dock) {
                    case 'right': rItems.push(item); break;
                    case 'center': cItems.push(item); break;
                    default: lItems.push(item); break;
                }

                if (item.updateLayout) {
                    item.updateLayout();
                }
            }



            lItems.reverse();
            me.setItemsLayout(0, lItems);

            if (cItems.length) {
                w = me.getBtnWidth(0, cItems);
                itemAllW = me.width - w - i * item_span - 20;

                cItems.reverse();
                me.setItemsLayout(w / 2, cItems);
            }

            if (rItems.length) {

                w = me.getBtnWidth(0, rItems);
                itemAllW = me.width - w - i * item_span - 20;

                rItems.reverse();
                me.setItemsLayout(itemAllW, rItems);
            }
        }

    },


    //延迟显示
    //isDelayRender:false,

    //显示延迟
    delayRender: function () {
        "use strict";
        var me = this,
            el = $(me.applyTo);


        Mini2.delayRS = Mini2.delayRS || {};

        Mini2.delayRS[me.applyTo] = me;

        me.isDelayRender = true;

        el.attr('mi-delay-render', 'true');
        el.data('me', me);
    },

    render: function () {
        "use strict";
        var me = this;

        me.baseRender();

        me.renderItems();

        me.setSize();

        try{
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error(ex);
        }
    }

}, function () {

    var me = this,
        cfg = Mini2.SystemInfo.toolbar,
        args = arguments[0];

    
    Mini2.apply(me, {
        item_span: cfg.item_span,
        height: cfg.height
    });

    Mini2.apply(me, args);

    if ('large_flat' == me.uiType) {
        //大扁平按钮
        Mini2.apply(me, {
            item_span: cfg.uiType_largeFlat.item_span,
            height: cfg.uiType_largeFlat.height
        });

        if (!me.userCls) {
            me.userCls = '';
        }

        me.userCls += ' mi-toolbar-largeflat';
        
        
    }
    else if ('button_group' == me.uiType) {
        //按钮组
        Mini2.apply(me, {
            item_span: cfg.uiType_buttonGroup.item_span,
            height: cfg.uiType_buttonGroup.height
        });


        if (!me.userCls) {
            me.userCls = '';
        }

        me.userCls += ' mi-toolbar-btns';
    }


});