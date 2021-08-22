/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />



//菜单分隔符
Mini2.define('Mini2.ui.menu.Item', {

    extend: 'Mini2.ui.Component',

    //菜单主容器
    menu : null,

    //初始化组件
    initComponent: function () {

        //        log.debug("Mini2.ui.Component.initComponent()");

    },

    isMouseOver: false,

    /**
    * 图标样式
    */
    iconCls: null,

    /**
    * 图标路径
    */
    icon : null,

    text : '菜单项目',

    tpl :[
        '<div class="mi-component mi-hmenu-sort-asc mi-box-item mi-component-default mi-menu-item" style="right: auto; left: 0px; top: 0px; margin: 0px; ">',
            '<a class="mi-menu-item-link" href="#" hidefocus="true" unselectable="on">',
                '<div role="img" class="mi-menu-item-icon  " style=""></div>',
                '<span class="mi-menu-item-text" unselectable="on"></span>',
                '<img src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==">',
            '</a>',
        '</div>'
    ],

    setText: function(value){
        var me = this,
            itemTextEl = me.itemTextEl;

        me.text = value;
        itemTextEl.html(value);


        return me;
    },

    getText:function(){
        var me = this;

        return me.text;
    },

    getHeight: function () {
        var me = this,
            el = me.el;
        return $(el).outerHeight();
    },


    getWidth: function () {
        var me = this,
            el = me.el;

        return $(el).width();
    },

    setWidth :function(value){
        var me = this,
            el = me.el;

        el.css('min-width', value);

        return me;
    },

    baseRender: function () {
        var me = this,
            el,
            itemLinkEl,
            iconEl,
            itemTextEl;

        me.el = el = Mini2.$joinStr(me.tpl);
                
        me.itemLinkEl = itemLinkEl = el.children('.mi-menu-item-link');

        me.itemTextEl = itemTextEl = itemLinkEl.children('.mi-menu-item-text');

        itemTextEl.html(me.text);

        el.css('top', me.top);

        $(me.renderTo).append(el);
        

        if (me.icon || me.iconCls) {
                      

            if (me.iconCls) {

                me.iconEl = iconEl = $('<i role="img" class="fa " style="margin-right:4px;"></i>'); 

                iconEl.addClass(me.iconCls);

                itemTextEl.prepend(iconEl);
            }
            else if (me.icon) {


            }
        }





        $(el).muBind('mousemove',function () {
            if (!me.isMouseOver) {
                me.isMouseOver = true;
                $(el).addClass('mi-menu-item-active');
            }
        }).muBind('mouseout',function () {
            if (me.isMouseOver) {
                me.isMouseOver = false;
                $(el).removeClass('mi-menu-item-active');
            }
        });
    

        $(el).muBind('mousedown',function (e) {

            //log.debug("form.field.Trigger.mousedown() ");

            //Mini2.EventManager.setScope(this, [me.el, me.dropDownPanel]);


        })
        .muBind('mousemove',function (e) {

        })
        .muBind('mouseup',function (e) {
            //console.log("form.field.Trigger.mouseup() ");

            if (me.click) {

                console.log("xxxxxxxxxxxxxxxxx");

                me.click.call(me);
            }

            me.trigger('click', me);

            Mini2.EventManager.stopEvent(e);
            Mini2.EventManager.clear(me.scope || me.el);

        });


    },


    render: function () {

        var me = this;

        me.baseRender();

    }


});