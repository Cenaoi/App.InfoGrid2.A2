/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />


Mini2.define('Mini2.ui.container.DockingContainer', {


    isDockingContainer: true,

    //    items: {
    //        top: [],
    //        left: [],
    //        right: [],
    //        bottom: [],
    //        full:[],
    //        center: []
    //    },

    //    length: 0,

    owner: window,


    //初始化组件
    initComponent: function () {
        var me = this;

        me.clearItems();
    },

    configureItem: function (item) {


    },

    clearItems: function () {
        var me = this;

        me.length = 0;
        me.items = {
            top: [],
            left: [],
            right: [],
            bottom: [],
            center: [],
            full: []
        };
    },

    addDocked: function (items, pos) {
        var me = this,
            i = 0,
            ms = me.items,
            item,
            len = items.length;



        for (; i < len; i++) {
            item = items[i];

            if (Mini2.isDom(item)) {
                item.dock = $(item).attr('data-dock') || $(item).attr('dock');
            }
            else {
                item.dock = item.dock || 'top';
            }


            switch (item.dock) {
                case 'none': break;
                case 'top': ms.top.push(item); break;
                case 'bottom': ms.bottom.push(item); break;
                case 'left': ms.left.push(item); break;
                case 'right': ms.right.push(item); break;
                case 'full': ms.full.push(item); break;
                case 'center': ms.center.push(item); break; //注:这个居中属性放在整个引擎里面。有待商议或取消
                    //default: ms.contre.push(item); break;                                   
            }
        }


        me.length = len;
    },

    updateLayout: function (owner) {

        var me = this,
            ms = me.items,
            el = me.el,
            padding = me.padding,
            itemMargin = me.itemMargin,
            vScroll = false, //垂直滚动条

            h,
            w,
            h0 = 0,
            h1 = 0,
            h2 = 0,

            w0 = 0,
            w1 = 0,
            w2 = 0,
            wCenter = 0;


        if (!me.length) {
            return;
        }

        owner = owner || me.owner;


        itemMargin = Mini2.clone(itemMargin);

        if ('auto' == itemMargin.left) {
            itemMargin.left = 0;
        }
        else if (Mini2.isString(itemMargin.left)) {
            itemMargin.left = parseInt(itemMargin.left);
        }

        if ('auto' == itemMargin.right) {
            itemMargin.right = 0;
        }
        else if (Mini2.isString(itemMargin.right)) {
            itemMargin.right = parseInt(itemMargin.right);
        }

        if ('auto' == itemMargin.top) {
            itemMargin.top = 0;
        }
        else if (Mini2.isString(itemMargin.top)) {
            itemMargin.top = parseInt(itemMargin.top);
        }

        if ('auto' == itemMargin.buttom) {
            itemMargin.buttom = 0;
        }
        else if (Mini2.isString(itemMargin.buttom)) {
            itemMargin.buttom = parseInt(itemMargin.buttom);
        }


        me.itemMargin = itemMargin;


        h0 = me.getItemsHeight(ms.top);
        hC = me.getItemsHeight(ms.full);
        h2 = me.getItemsHeight(ms.bottom);



        w0 = me.getItemsWidth(ms.left);


        w2 = me.getItemsWidth(ms.right);


        if (owner && owner.muid) {
            w = owner.getWidth();
            h = owner.getHeight();
        }
        else {
            w = $(owner).width();
            h = $(owner).height();
        }


        h -= (padding.top + padding.bottom);
        w -= (padding.left + padding.right);

        h1 = h - h0 - h2;
        w1 = w - w0 - w2;



        if ('none' != owner.scroll && h0 + hC + h2 > h ) {
            w1 -= 20;
        }

        //设置[上、下] 的高度和宽度
        if (ms.top.length || ms.bottom.length) {
            me.setItemsTop2(padding.top, ms.top);
            me.setItemsTop2(padding.top + h0 + h1, ms.bottom);

            me.setItemsWidth(w1, ms.top);
            me.setItemsWidth(w1, ms.bottom);
        }



        //设置[左、右] 的 宽度,坐标
        if (ms.left.length || ms.right.length) {

            //me.setItemsTop(padding.top + h0, ms.left);
            //me.setItemsTop(padding.top + h0, ms.right);

            me.setItemsTop(h0, ms.left);
            me.setItemsTop(h0, ms.right);

            me.setItemsLeft2(0, ms.left);
            me.setItemsLeft2(w0 + w1 + itemMargin.right, ms.right);

            me.setItemsHeight(h1, ms.left);
            me.setItemsHeight(h1, ms.right);


        }


        if (ms.center.length) {
            wCenter = me.getItemsWidth(ms.center);

            me.setItemsLeft2(padding.left + (w - wCenter) / 2, ms.center);
        }


        //设置中心填充部分
        if (ms.full.length) {

            //me.setItemsLeft(padding.left + w0, ms.full);
            //me.setItemsTop(padding.top + h0, ms.full);

            me.setItemsLeft(w0, ms.full);
            me.setItemsTop(h0, ms.full);

            me.setItemsWidth(w1, ms.full);
            me.setItemsHeight(h1, ms.full);
        }


    },


    setItemCss: function (item, value, propName, cssName) {
        var me = this;

        if (item) {
            if (item[propName]) {
                item[propName](value);
            }
            else {
                $(item).css(cssName, value);
            }
        }
    },



    setItemsWidth: function (w, items) {
        var me = this,
            i,
            item,
            len = items.length;

        if (len) {
            for (i = 0; i < len; i++) {
                item = items[i];
                me.setItemCss(item, w, 'setWidth', 'width');
            }
        }
    },

    setItemsHeight: function (h, items) {
        var me = this,
            item,
            len = items.length;

        if (len) {

            $(items).each(function () {
                me.setItemCss(this, h, 'setHeight', 'height');
            });
        }
    },


    setItemsTop: function (y, items) {
        var me = this,
            item,
            v = y,
            len = items.length;

        if (len) {

            $(items).each(function () {
                me.setItemCss(this, v, 'setTop', 'top');
            });
        }
    },

    setItemsTop2: function (y, items) {
        var me = this,
            item,
            itemMargin = me.itemMargin,
            v = y,
            len = items.length;

        if (len) {

            $(items).each(function () {
                item = this;
                v += itemMargin.top;

                if (item.marginTop) {
                    try {
                        var top = parseInt(item.marginTop);

                        v += top;
                    }
                    catch (ex) {

                    }
                }

                me.setItemCss(item, v, 'setTop', 'top');

                v += itemMargin.bottom;


                if (item.getHeight) {
                    v += item.getHeight();
                }
                else {
                    v += $(item).outerHeight();
                }
            });
        }
    },

    setItemsLeft: function (x, items) {
        var me = this,
            item,
            itemMargin = me.itemMargin,
            v = x,
            len = items.length;

        if (len) {

            $(items).each(function () {
                me.setItemCss(this, v, 'setLeft', 'left');
            });
        }
    },

    setItemsLeft2: function (x, items) {

        var me = this,
            item,
            itemMargin = me.itemMargin,
            v = x,
            itemW,
            len = items.length;

        if (len) {

            $(items).each(function () {
                item = this;

                if ('auto' != itemMargin.left) {
                    v += itemMargin.left;
                }

                if (item.setLeft) {
                    item.setLeft(v);
                    itemW = item.getWidth();
                }
                else {
                    $(item).css("left", v);
                    itemW = $(item).outerWidth();
                }

                if (!Mini2.isNumber(itemW)) {
                    itemW = parseInt(itemW);
                }

                v += itemW;

                if ('auto' != itemMargin.right) {
                    v += itemMargin.right;
                }

            });
        }
    },

    getItemsWidth: function (items) {
        var me = this,
            item,
            itemMargin = me.itemMargin,
            v = 0,
            itemW;

        if (items.length) {

            $(items).each(function () {
                item = this;
                if (item.getWidth) {
                    itemW = item.getWidth();
                }
                else {
                    itemW = $(item).outerWidth();
                }

                if (!Mini2.isNumber(itemW)) {
                    itemW = parseInt(itemW);
                }

                v += (itemW + itemMargin.left + itemMargin.right);

            });
        }

        return v;
    },

    getItemsHeight: function (items) {
        var me = this,
            item,
            itemMargin = me.itemMargin,
            v = 0,
            len = items.length;

        if (len) {
            $(items).each(function () {
                item = this;
                if (item.getHeight) {
                    v += item.getHeight();
                }
                else {
                    v += $(item).outerHeight();
                }

                v += itemMargin.top + itemMargin.bottom;
            });
        }

        return v;
    }

}, function () {
    var me = this;

    Mini2.apply(this, arguments[0]);

    me.initComponent();

});