/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />

//水平排列
Mini2.define("Mini2.ui.layout.container.HBox", {

    itemCls: [
        Mini2.baseCSSPrefix + 'form-item',
        Mini2.baseCSSPrefix + 'box-item'
    ],

    //初始化组件
    initComponent: function () {
        var me = this;

        me.length = 0;


    },


    configureItem: function (item) {
        "use strict";
        var me = this,
            itemCls = me.itemCls,
            ownerItemCls = me.owner.itemCls,
            addClasses;

        item.ownerLayout = me;

        if (addClasses) {
            item.addCls(addClasses);
        }
    },

    afterHeight: 0,


    getItemObj: function (obj) {
        var itemObj;
        if (obj.muid) {
            itemObj = obj;
        }
        else {
            itemObj = $(obj).data('me');
        }
        return itemObj;
    },


    //获取各个项目的总宽度
    getItemWidth2: function (items) {
        "use strict";
        var me = this,
            item,
            w = 0;

        $(items).each(function () {
            item = this;
            item = me.getItemObj(item);

            if (item) {
                w += item.getWidth(true);
            }
            else if (Mini2.isDom(item) && $(item).is(':visible')) {
                w += $(item).outerWidth(true);
            }
        });

        return w;
    },


    updateLayout: function (owner) {
        "use strict";

        var me = this,
            boxTarget = owner.boxTarget,
            el = owner.el,
            oWidth,
            items,
            itemsW, //全部宽度
            len,
            x0 = 0, y0 = 0,
            index = 0,
            y2 = 0,
            align = me.align,
            padding = owner.padding,
            sysInfo = Mini2.SystemInfo.form,
            itemMarginBottom = sysInfo.itemMarginBottom || 0;

        //var oWidth = owner.getWidth();

        oWidth = owner.targetWidth; //目标宽度

        items = $(boxTarget).children(':visible');


        itemsW = 0;


        len = items.length;


        if (align == 'center') {
            itemsW = me.getItemWidth2(items);
            x0 = (oWidth - itemsW) / 2;
        }
        else if (align == 'right') {
            itemsW = me.getItemWidth2(items);
            x0 = oWidth - itemsW;
        }


        var lineHegith = 0, //当前行的高度
            i,
            item,
            itemEl,
            w, h,
            obj;

        for (i = 0; i < len; i++) {

            item = items[i];

            itemEl = $(item);

            w = itemEl.outerWidth(true);
            h = itemEl.outerHeight(true);

            lineHegith = Math.max(lineHegith, h);

            obj = me.getItemObj(item);



            if (undefined === obj) {

                if (itemEl && itemEl.hasClass('mi-newline')) {

                    //换行
                    y0 += lineHegith + itemMarginBottom;
                    x0 = 0;

                    lineHegith = 0;

                    index = 0;

                }

                continue;
            }

            if (index > 0 && x0 + w > oWidth) {

                //换行
                y0 += lineHegith + itemMarginBottom;
                x0 = 0;


                index = 0;


                console.debug("换行2");
            }


            obj.setLeft(x0);
            obj.setTop(y0);


            y2 = y0 + lineHegith;
            index++;

            if (x0 + w < oWidth) {
                x0 += w;
            }
            else {

                //换行
                y0 += lineHegith + itemMarginBottom;

                x0 = 0;

                lineHegith = 0;

                index = 0;

            }

        }


        y2 += padding.bottom + padding.top;

        me.afterHeight = y2;
        owner.targetHeight = y2;

    },

    beginLayout: function (ownerContext) {


    },


    getLayoutItems: function () {


    }

});