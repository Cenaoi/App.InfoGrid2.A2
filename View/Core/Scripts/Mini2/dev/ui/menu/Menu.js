/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />



//菜单
Mini2.define('Mini2.ui.menu.Menu', {

    
    extend: 'Mini2.ui.Component',



    //初始化组件
    initComponent: function () {

        //        log.debug("Mini2.ui.Component.initComponent()");

    },


    

    tpl:[
        '<div class="mi-panel mi-layer mi-panel-default mi-menu mi-border-box" tabindex="-1" style="width: 146px; height: 79px; right: auto; left: 10px; top:0px; z-index: 19001;">',
            '<div class="mi-panel-body mi-menu-body mi-unselectable mi-panel-body-default mi-box-layout-ct mi-panel-body-default mi-noborder-trbl" style="width: 144px; height: 77px; left: 0px; top: 0px;">',

                '<div class="mi-box-inner mi-box-scroller-top">',
                    '<div id="menu-1075-before-scroller" class="mi-box-scroller mi-menu-scroll-top" style="display:none"></div>',
                '</div>',

                '<div class="mi-box-inner mi-vertical-box-overflow-body" role="presentation" style="height: 77px; width: 144px;">',
                    '<div class="mi-box-target" style="width: 144px;">',
                    '</div>',
                '</div>',

                '<div class="mi-box-inner mi-box-scroller-bottom">',
                    '<div id="menu-1075-after-scroller" class="mi-box-scroller mi-menu-scroll-bottom" style="display:none"></div>',
                '</div>',

            '</div>',
        '</div>'
    ],

    
    //显示子控件
    renderItems: function (boxTargetEl) {
        var me = this,
            items = me.items,
            item,
            cfg,
            newItems = [],
            i,
            y = 0;

        for (i = 0; i < items.length; i++) {

            cfg = Mini2.clone( items[i]);
            
            if ('-' == cfg.text ) {
                item = Mini2.create('Mini2.ui.menu.Separator', {
                    owner: me.owner,
                    renderTo: boxTargetEl,
                    top: y + 2
                });

                item.render();

                y += item.getHeight();
            }
            else {

                cfg = Mini2.apply(cfg, {
                    owner: me.owner,
                    renderTo: boxTargetEl,
                    top: y,
                    text: cfg.text,
                    click: cfg.click
                });


                item = Mini2.create('Mini2.ui.menu.Item', cfg);

                item.bind('click', function () {

                    me.item_click.call(me, this);
                })

                item.render();

                y += item.getHeight();
            }

            newItems.push(item);
        }

        delete me.items;

        me.items = newItems;

    },

    item_click:function(menuItem){
        var me = this;

        me.hide();
    },

    //获取总高度
    getItemsHeight:function(){
        var me = this,
            items = me.items,
            len = items.length,
            i,
            height = 0;

        for (i = 0; i < len; i++) {
            height += items[i].getHeight();
        }

        return height;
    },



    //获取总高度
    getItemsWidth: function () {
        var me = this,
            items = me.items,
            len = items.length,
            i,
            width = 0;

        for (i = 0; i < len; i++) {
            if (!items[i].isSeparator) {
                width = Math.max(width, items[i].getWidth());
            }
        }


        return width;
    },

    /**
    * 设置项目的宽度
    * @parma {Number} width 宽度
    */
    setItemsWidth:function(width){
        var me = this,
            items = me.items,
            len = items.length,
            i;

        for (i = 0; i < len; i++) {
            if (!items[i].isSeparator) {
                items[i].setWidth(width);
            }
        }


        return me;
    },


    callShow:function(){
        var me = this,
            el = me.el;

        me.trigger('showing');
        el.show();
        me.trigger('showed');

        return me;
    },

    
    callHide: function () {
        var me = this,
            el = me.el;

        if (me.visible) {
            el.hide();

            me.trigger('hidded');
        }

        return me;
    },


    /**
    * 延迟显示
    *
    */
    delayShow:function(){
        var me = this;
        Mini2.ui.menu.Manager.show(me);

        return me;
    },

    show: function () {
        var me = this;
                
        me.callShow();
        
        return me;
    },


    hide:function(){
        var me = this,
            el = me.el;
        
        if (me.visible) {
            el.hide();

            me.trigger('hidded');
        }

        return me;
    },
    
    


    baseRender: function () {
        var me = this,
            el ,
            sysInfo = Mini2.SystemInfo.menu;

        me.el = el = Mini2.$joinStr(me.tpl);
        
        $(document.body).append(el);

        me.panelBodyEl = el.find('.mi-panel-body:first');
        me.boxInnerEl = me.panelBodyEl.children('.mi-vertical-box-overflow-body');
        me.boxTargetEl = me.boxInnerEl.children('.mi-box-target');


        // 显示子项目
        me.renderItems(me.boxTargetEl);

        var itemsHeigth = me.getItemsHeight() + 2;
        var itemsWidth = me.getItemsWidth();

        el.css({
            'top': me.top,
            'left': me.left
        });



        var sz = {
            width: itemsWidth,
            height: itemsHeigth
        };

        var sz2 = {
            width: itemsWidth,
            height: itemsHeigth + sysInfo.paddingTop + sysInfo.paddingBottom
        };

        me.boxInnerEl.css({ 'top': sysInfo.paddingTop });
        

        el.css(sz2);

        me.panelBodyEl.css(sz2);
        me.boxInnerEl.css(sz);
        me.boxTargetEl.css(sz);

        me.setItemsWidth(itemsWidth);


        $(el).mousedown(function (e) {

            //log.debug("form.field.Trigger.mousedown() ");

            //Mini2.EventManager.setScope(this, [me.el, me.dropDownPanel]);
        })
        .mousemove(function (e) {

        })
        .mouseup(function (e) {
            //console.log("form.field.Trigger.mouseup() ");

            Mini2.EventManager.stopEvent(e);
            Mini2.EventManager.clear(me.scope || me.el);

        });


        Mini2.ui.menu.Manager.register(me);

        return me;
    },


    render: function () {
        var me = this;

        me.baseRender();

        me.show();

        return me;
    }


});