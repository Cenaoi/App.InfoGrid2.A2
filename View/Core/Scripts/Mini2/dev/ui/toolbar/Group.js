/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../Mini.js" />

Mini2.define('Mini2.ui.toolbar.Group', {

    extend: 'Mini2.ui.Component',


    baseCls: Mini2.baseCSSPrefix + 'toolbar-group',

    renderTpl: [
        '<div class="mi-toolbar-group  mi-box-item" data-x="33">',
            
        '</div>'
    ],



    /**
     * 清除项目集合
     */
    clear: function () {
        "use strict";
        var me = this,
            el = me.el,
            items = me.items,
            itemEls = el.children();

        console.debug('清除子项目:', itemEls.length);

        $(items).each(function () {

            var item = this;

            if (item.clear) {
                item.clear();
            }
        });

        me.items = [];

        itemEls.remove();

        return me;
    },


    /**
     * 添加项目
     */
    add: function (cfg) {

        var me = this,
            item,
            items = me.items || [];


        item = me.createItem(cfg);

        if (item.id) {
            Mini2.onwerPage.controls[item.id] = item;
            window[item.id] = item;
        }

        items.push(item);

    },

    createItem: function (cfg) {
        "use strict";
        var me = this,
            el = me.el,
            item,
            cType,
            namespace = 'Mini2.ui.toolbar',
            fullname;

        cfg = Mini2.apply(cfg, {

            ownerParent: me,
            toolbar: me,
            renderTo: el
        });

        cType = cfg.type;

        if ('title' == cType) {
            fullname = namespace + '.Title';
        }
        else if ('hr' == cType) {
            fullname = namespace + '.Hr';
        }
        else if ('template' == cType) {
            fullname = namespace + '.Template';
        }
        else if ('group' == cType) {
            fullname = namespace + '.Group';
        }
        else {
            fullname = namespace + '.Button';
        }

        item = Mini2.create(fullname, cfg);
        item.render();

        if (item.getWidth) {
            item.width = item.getWidth();
        }
        else if (item.el && item.el.width) {
            item.width = $(item.el).outerWidth();
        }


        return item;
    },

    getInputEl: function () {
        "use strict";
        var me = this,            
            el;
     
        el = Mini2.$join(me.renderTpl);

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }


        if (me.id) {
            el.attr('id', me.id);
        }

        if (!me.visible) {
            el.css('display', 'none');
        }

        
        if (me.disabled) {
            el.addCls(['mi-item-disabled', 'mi-disabled', 'mi-btn-disabled', 'mi-btn-default-small-disabled']);
        }

        return el;
    },

    getWidth:function(){

        var me = this,
            i,
            item,
            items = me.items,
            len = items.length,
            w = 0;
        
        for (i = 0; i < len; i++) {
            item = items[i];
            w += item.getWidth();
        }

        w = w - len + 1

        return w;
    },


    /**
     * 刷新布局
     */
    updateLayout:function(){
        "use strict";
        var me = this,
            items = me.items;

        me.setItemsLayout(0, items);

        return me;
    },

    setItemsLayout: function (x, items) {
        "use strict";
        var me = this,
            i,
            item,
            items = items || me.items,
            len = items.length;

        x = x || 0;


        for (i = 0; i < len; i++) {
            item = items[i];

            if (item.visible) {
                item.setLeft(x);


                if (item.updateLayout) {
                    item.updateLayout();
                }

                x += item.getWidth() - 1;
            }
        }

        return me;
    },


    renderItems:function(){
        var me = this,
            items = me.items,
            len = items.length,
            i;

        for (i = 0; i < len; i++) {

            var item = me.createItem(items[i]);


            if (item.id) {
                Mini2.onwerPage.controls[item.id] = item;
                window[item.id] = item;
            }

            me.items[i] = item;
        }

    },

    render: function () {
        "use strict";
        var me = this,
            w,
            el;

        me.el = el = me.getInputEl();

        me.renderItems();

        me.setItemsLayout(0, me.items);

        w = me.getWidth();

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }

        return me;
    }

});