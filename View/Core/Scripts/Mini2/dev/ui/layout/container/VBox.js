/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />


//垂直排列
Mini2.define("Mini2.ui.layout.container.VBox", {

    itemCls: [Mini2.baseCSSPrefix + 'form-item', Mini2.baseCSSPrefix + 'box-item'],

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

    getItemObj: function (obj) {
        "use strict";
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
            w = 0;

        $(items).each(function () {

            var itemObj = me.getItemObj(this);

            if (itemObj) {

                w += itemObj.getWidth();

            }
            else if (Mini2.isDom(this) && $(this).is(':visible')) {
                w += $(this).outerWidth();
            }
        });

        return w;
    },


    updateLayout: function (owner) {
        "use strict";
        var me = this,
            y = 0,
            h = 0,
            i,
            w = owner.width,
            boxTarget = owner.boxTarget,
            items;

        items = boxTarget.children();

        var itemsW = me.getItemWidth2(items);



        $(items).each(function () {

            var itemObj = me.getItemObj(this);

            if (itemObj) {

                itemObj.setTop(y);
                itemObj.setWidth(w);

                h = itemObj.getHeight();
                y += h;
            }
            else if (Mini2.isDom(this) && $(this).is(':visible')) {


                $(this).css({
                    'top': y,
                    'width': w
                });

                h = $(this).outerHeight();
                y += h;
            }
        });


    },


    beginLayout: function (ownerContext) {


    },


    getLayoutItems: function () {


    }

});