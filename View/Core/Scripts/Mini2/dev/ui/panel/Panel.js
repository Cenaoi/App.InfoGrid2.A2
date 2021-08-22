/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />

Mini2.define("Mini2.ui.panel.Panel", {

    extend: 'Mini2.ui.Component',

    log: Mini2.Logger.getLogger('Mini2.ui.panel.Panel', false),  //日志管理器

    baseCls: Mini2.baseCSSPrefix + "panel",

    //显示的模板代码
    renderTpl: [
        '<div role="Panel" style="right: auto; left: 0px; top: 0px;">',
            '<div class="mi-box-inner " role="presentation" style="">',
                '<div class="mi-box-target" style="width: 100%; "></div>',
            '</div>',
        '</div>'
    ],


    // auto=自动绑定模式， filter=过滤模式，配合服务器组件使用
    // auto | filter 
    formMode : 'auto', 

    //显示边框
    border: true,

    isPanel: true,

    width: '100%',
    //height: 100,


    //    minWidth: 0,
    //    minHeight:0,


    // top: 0,
    // left: 0,
    // bottom
    // right

    //auot, none = 没有滚动条, vertical = 垂直滚动条,Horizontal = 水平滚动条
    //scroll: 'auto',

    /**
    * 边缘空白
    */
    //margin: 0,
    //marginLeft: 0,
    //marginRight: 0,
    //marginTop: 0,
    //marginBottom: 0,


    //left | right | buttom | top
    dock: 'top',

    region: 'north',

    //head | body | footer
    ui: 'default',

    uiCls: [],

    userCls: null,

    lockable: false,

    flowDirection: 'lefttoright',

    wrapContents: true,

    itemWidth: 0,

    fixed: false,

    //自动调整尺寸
    autoSize: false,

    ownerParent: null,



    callParent: function () {
        var me = this,
            ownerParent = me.ownerParent;

        ownerParent && ownerParent.updateLayout();

    },


    //updateLayout: Mini2.emptyFn,

    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this,
            layout = me.layout;

        if (Mini2.isString(layout)) {
            me.layout = {
                type: layout
            };
        }
        else if (!layout) {
            me.layout = {
                type: 'auto'
            };
        }

        //第一次显示
        me.oneShow = !!me.visible;

        me.itemMargin = Mini2.apply({ left: 0, top: 0, right: 0, bottom: 0 }, me.itemMargin);
        me.padding = Mini2.apply({ left: 0, top: 0, right: 0, bottom: 0 }, me.padding);

        if (me.layout.type == "toolbar") {
            me.baseCls = Mini2.baseCSSPrefix + "toolbar";
        }
    },


    scrollTop: function (value) {
        var me = this,
            boxTarget = me.boxTarget;

        boxTarget.scrollTop(value);

        return me;
    },


    //调整下方子项的尺寸
    resizeChilds: function () {
        var me = this;


    },


    toggle: function () {
        var me = this,
            el = me.el;

        if (me.isVisible()) {
            me.hide();
        }
        else {
            me.show();
        }

        return me;
    },



    resize: function (fn) {
        var me = this,
            i,
            fns = me.resizeFns || [],
            len ,
            fn;

        if (arguments.length == 0) {

            me.updateLayout(me);

            for (i = 0,len = fns.length; i < len; i++) {
                fn = fns[i];
                fn.call(me);
            }
        }
        else {
            if (!me.resizeFns) {
                me.resizeFns = fns;
            }

            fns.push(fn);
        }



    },

    prepareItems_ForFlow: function (items) {
        var me = this,
            preItems = [];


        return items;
    },

    prepareItems_ForTable: function (items) {
        var me = this,
            preItems = [];



        return items;
    },

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

    prepareItems: function (items) {
        "use strict";

        var me = this,
            i,
            item,
            itemMi,
            itemEl,
            flowDirection = me.flowDirection,
            layout = me.layout,
            itemLabel,
            preItems = [];


         
        $(items).each(function () {
            item = this;
            itemMi = me.getItemObj(item);

            if (itemMi) {

                itemMi.ownerParent = me;

                itemEl = itemMi.el;

                if (itemMi.isVisible && !itemMi.isVisible()) {
                    return;
                }

                preItems.push(itemMi);


                if ('lefttoright' == flowDirection) {

                    itemEl.addClass('mi-form-item-float-left');
                }
                
                itemEl.addClass('mi-anchor-form-item'); //增加表单底部锚点

                //对子项注入新的样式
                if (layout && layout.itemCls) {
                    itemEl.addClass(layout.itemCls);
                }

                if (me.itemWidth && itemMi.setWidth) {

                    if (itemMi.width == '100%') {

                        if (itemMi.colspan) {
                            itemMi.setWidth(me.itemWidth * itemMi.colspan);
                        }
                        else {
                            itemMi.setWidth(me.itemWidth);
                        }
                    }
                }

                itemLabel = itemMi.labelable;

                if (itemLabel) {

                    if (me.itemLabelWidth) {
                        itemLabel.setWidth(me.itemLabelWidth);
                    }

                    if (me.itemLabelAlign) {
                        itemLabel.setLabelAlign(me.itemLabelAlign);
                    }

                    if (me.itemLabelPad) {
                        itemLabel.setLabelPad(me.itemLabelPad);
                    }

                }


            }
            else if (Mini2.isDom(item) && $(item).is(':visible')) {

                preItems.push(item);

                var itemEl = $(item);

                //换行的样式
                if (itemEl.hasClass('mi-newline')) {

                }
                else {

                    if ('lefttoright' == flowDirection) {
                        
                        itemEl.addClass('mi-form-item-float-left');
                    }

                    //对子项注入新的样式
                    if (layout && layout.itemCls) {
                        itemEl.addClass(layout.itemCls);
                    }

                    if (me.itemWidth) {
                        itemEl.css('width', me.itemWidth);
                    }
                }
            }

        });

        return preItems;
    },

    //排版引擎
    layoutEngine: null,



    offsetParent: function (elem) {
        "use strict";
        var el = $(elem),
            borderBoxCls = 'mi-border-box',
            i = 0;

        if (el.hasClass(borderBoxCls)) {
            return elem;
        }

        for (i; i < 99; i++) {

            el = el.parent();

            if (el.hasClass(borderBoxCls)) {

                break;
            }
        }

        return el;
    },


    //获取拥有 TabStop=true 的项目
    getTabItems: function () {
        "use strict";
        var me = this,
            log = me.log,
            item, itemObj,
            items,
            layout = me.layout,
            layoutType ,
            boxTarget = me.boxTarget,
            preItems =[];


        items = boxTarget.children();

        if (layout) {
            layoutType = layout.type;
        }

        $(items).each(function () {
            item = this;
            itemObj = me.getItemObj(item);

            if (itemObj && itemObj.tabStop ) {             
                preItems.push(itemObj);
            }
        });

        return preItems;
    },


    /**
    * items 集合缓冲
    */
    itemsBuffer : null,


    updateLayout: function (owner) {
        "use strict";

        var me = this,
            log = me.log,
            items,
            pBox,
            engine = me.layoutEngine,
            layout = me.layout,
            layoutType = layout.type,
            boxTarget = me.boxTarget;

        if (!me.isVisible()) {
            return;
        }

        pBox = me.offsetParent(me.el);


        //items = me.itemsBuffer;

        if (!items) {

            items = boxTarget.children();

            if ('table' == layoutType) {
                items = me.prepareItems_ForTable(items);
            }
            else {
                items = me.prepareItems(items);
            }

            me.itemsBuffer = items;
        }


        if (items && items.length == 0) {
            return;
        }
        

        if ('form' == layoutType) {

            engine = engine || Mini2.create('Mini2.ui.layout.container.Form', {
                owner: me
            });
        }
        else if ('hbox' == layoutType) {    //水平排列.宽度固定，高度自动。

            engine = engine || Mini2.create('Mini2.ui.layout.container.HBox', {
                owner: me,
                align: layout.align
            });
        }
        else if ('vbox' == layoutType) {    //垂直排列.高度固定，宽度自动
            engine = engine || Mini2.create('Mini2.ui.layout.container.VBox', {
                owner: me,
                align: layout.align
            });
        }
        else {

            engine = engine || Mini2.create('Mini2.ui.container.DockingContainer', {
                owner: me,
                itemMargin: me.itemMargin,
                padding: me.padding
            });


            engine.clearItems();

            if (items.length) {
                engine.addDocked(items);
            }
        }

        engine.updateLayout(me);

        me.layoutEngine = engine;
    },


    getWidth: function () {
        return this.el.outerWidth();
    },

    getHeight: function () {
        return this.el.outerHeight();
    },

    getSize: function () {
        var me = this;
        return { 'width': me.getWidth(), 'height': me.getHeight() };
    },

    setLeft: function (value) {
        var me = this,
            el = me.el;

        el.css('left', value);

        return me;
    },
    getLeft: function () {
        var me = this,
            el = me.el;

        return el.css('left');
    },

    getTop: function () {
        var me = this,
            el = me.el;

        return el.css('top');
    },
    setTop: function (value) {
        var me = this,
            el = me.el;

        el.css('top', value);

        return me;
    },

    
    setSize: function (w, h, callEvent) {
        "use strict";

        var me = this,
            log = me.log,
            el = me.el,
            minHeight = me.minHeight,
            boxInner = me.boxInner,
            boxTarget = me.boxTarget,
            padding = me.padding,
            allValue = 0;



        if (minHeight && h && h < minHeight) {
            h = minHeight;
        }

        me.targetWidth = w || me.width;     //预计达到宽度
        me.targetHeight = h || me.height;   //预计达到高度


        me.updateLayout(me);

        me.width = me.targetWidth || me.width;
        me.height = me.targetHeight || me.height;


        if (el) {

            if (me.autoSize) {

                if ('vertical' == me.scroll) {
                    //只有垂直滚动条

                    var boxW = me.width - (padding.right + padding.left);

                    el.css({ 'width': me.width});
                    boxInner.css({ 'width': '100%' });
                    boxTarget.css({ 'width': boxW });
                }


            }
            else {
                var boxW = me.width - (padding.right + padding.left);
                var boxH = me.height - (padding.bottom + padding.top);
                                    
                el.css({ 'width': me.width, 'height': me.height });
                boxInner.css({ 'width': '100%', 'height': boxH });
                boxTarget.css({ 'width': boxW, 'height': boxH });
            }
        }

        me.resize();


        return me;
    },




    animate: function (styles, speed, easing, callback) {
        var me = this,
            el = me.el;

        el.animate(styles, speed, easing, callback)

        return me;
    },

    hide: function () {
        var me = this,
            el = me.el;

        if (el) {
            el.hide();

            me.callParent();
        }

        return me;
    },

    show: function () {
        var me = this,
            el = me.el;
        
        if (el) {
            el.show();

            me.callParent();
            me.callParent();    //注: 调用了两次，临时解决方法，以后改进。(2014-11-02 20:32)
        }

        return me;
    },

    css: function (styleName, styleValue) {
        var me = this,
            el = me.el;

        return el.css(styleName, styleValue);
    },


    isVisible: function () {
        var me = this,
            el = me.el;

        return el && el.is(':visible');
    },

    /**
    * 重置层次
    */
    resetPaint: function(){
        var me = this,
            items = me.itemsBuffer || [],
            item,
            i,
            len = items.length;

        for (i = 0; i < len; i++) {
            item = items[i];

            if (item && item.resetPaint) {
                item.resetPaint.call(item);
            }
        }
    },

    baseRender: function () {
        "use strict";
        var me = this,
            log = me.log,
            el,
            padding = me.padding,
            boxInner,
            boxTarget,
            w = me.width,
            h = me.height;


        me.el = el = Mini2.$join(me.renderTpl);
        me.boxInner = boxInner = el.cFirst('.mi-box-inner');
        me.boxTarget = boxTarget = boxInner.cFirst('.mi-box-target');




        //因为事件机制的不完善，加了两个事件，防止点击到底下 Window，触发了其他事件。

        //el.muBind('mousedown', function () {
        //    //Mini2.ui.FocusMgr.setControl(this);
        //});
        //el.muBind('mouseup', function () {
        //    Mini2.ui.FocusMgr.setControl(me);
        //});

        

        //el.css({
        //    'margin': me.margin,
        //    'margin-left': me.marginLeft,
        //    'margin-right': me.marginRight,
        //    'margin-top': me.marginTop,
        //    'margin-bottom': me.marginBottom
        //});



        el.mouseup(function () {
            Mini2.ui.FocusMgr.setControl(me);
        });
        

        if (me.visible == false) {
            el.hide();
            delete me.visible;
        }

        if (me.onResize) {
            me.resize(me.onResize);
        }

        el.data('me', me);

        el.addCls(me.baseCls, 'mi-box-layout-ct');

        if (me.ui) {
            el.addCls(me.baseCls + '-' + me.ui);
        }

        if (me.anchorFixedType) {

            if (me.anchorFixedType == 'topbottom') {

                el.addClass('mi-panel-anchorfixed-topbotom');

            }

        }


        if ('default' != me.dock) {

            if ('none' != me.dock) {
                el.addCls('mi-docked',
                    'mi-docked-' + me.dock,
                    me.baseCls + '-docked',
                    me.baseCls + '-docked-' + me.dock);

                if (me.ui) {
                    el.addCls(me.baseCls + '-' + me.ui + '-docked-' + me.dock);
                }
            }
        }


        if (me.fixed) {
            el.addClass('mi-fixed-layer');
            el.css('z-index', 1009);
        }

        //auot, none = 没有滚动条, vertical = 垂直滚动条,Horizontal = 水平滚动条
        switch (me.scroll) {
            case 'none': boxTarget.css({ 'overflow': 'hide' }); break;
            case 'vertical': boxTarget.css({ 'overflow-x': 'hidden', 'overflow-y': 'auto' }); break;
            case 'horizontal': boxTarget.css({ 'overflow-x': 'auto', 'overflow-y': 'hidden' }); break;
            default: boxTarget.css({ 'overflow': 'auto' }); break;
        }



        var attrList = [
            'left', 'top', 'right', 'bottom',
            'width', 'height', 'position'];

        var cssPs = {};

       

        for (var i = 0; i < attrList.length; i++) {
            var attrName = attrList[i];

            //log.debug('attr=' + attrName + ", value=" + me[attrName]);

            if (me[attrName]) {
                cssPs[attrName] = me[attrName];
            }
        }


        for (var i in padding) {
            var attrName = 'padding-' + i;

            cssPs[attrName] = padding[i];
        }

        if (me.maxWidth) {
            el.css({ 'max-width': me.maxWidth });
            boxTarget.css({ 'max-width': me.maxWidth });
        }

        if (me.minWidth) {
            el.css({ 'min-width': me.minWidth });
            boxTarget.css({ 'min-width': me.minWidth });
        }

        if (me.maxHeight) {
            el.css({ 'max-height': me.maxHeight });
            boxTarget.css({ 'max-height': me.maxHeight });
        }

        if (me.minHeight) {
            el.css({ 'max-height': me.minHeight });
            boxTarget.css({ 'max-height': me.minHeight });
        }


        if (me.margin) {
            el.css({ 'margin': me.margin });
        }

        if (me.marginLeft) {
            el.css({ 'margin-left': me.marginLeft });
        }

        if (me.marginRight) {
            el.css({ 'margin-right': me.marginRight });
        }
                
        if (me.marginTop) {
            el.css({ 'margin-top': me.marginTop });
        }

        if (me.marginBottom) {
            el.css({ 'margin-bottom': me.marginBottom });
        }

        el.css(cssPs);


        if (me.right) {
            
        }

        if (me.autoSize) {
            boxTarget.css({
                position: 'static',
                height: 'auto'
            });
        }
        else {

            el.css({ width: w });

            boxInner.css({ width: w, height: h });

            boxTarget.css({ width: w, height: h });
        }


        if (me.userCls) {
            el.addClass(me.userCls);
        }



        if (me.contentEl) {

            var contentEl = $(me.contentEl);
            var items = contentEl.children();
            
            contentEl.after(el);

            boxTarget.append(items);

            el.attr('id', contentEl.attr('id'));

            contentEl.remove();
        }
        else if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else if (me.renderTo) {
            $(me.renderTo).append(el);
        }



        //setTimeout(function () {
        //    me.updateLayout();
        //}, 100);


    },

    focus:function(){
        var me = this;

        Mini2.ui.FocusMgr.setControl(me);

        return me;
    },


    afterRender: function () {
        var me = this;

        me.updateLayout();
    },


    //显示延迟
    delayRender: function () {
        var me = this,
            el = $(me.applyTo);

        me.isDelayRender = true;

        Mini2.delayRS = Mini2.delayRS || {};

        Mini2.delayRS[me.clientId] = me;

        el.attr('mi-delay-render', 'true');
        el.data('me', me);
    },


    //延迟加载定时器
    delayRenderTimer:function(){
        var me = this,
            log = me.log;

        setTimeout(function () {
            var htmlItems = me.boxTarget.children();


            $(htmlItems).each(function () {

                var itemId = $(this).attr('id');

                //log.debug('itemId = ' + itemId);

                if (itemId && Mini2.delayRS) {

                    var dd = Mini2.delayRS[itemId];

                    if (!dd) {
                        itemId = '#' + itemId;
                        dd = Mini2.delayRS[itemId]
                    }

                    if (dd) {

                        dd.render();

                        delete Mini2.delayRS[itemId];

                        //log.debug("加载:" + itemId);

                    }
                }

            });

            me.updateLayout();

        }, 100);

        delete me.isDelayRender;
    },

    render: function () {
        var me = this,
            log = me.log,
            padding = me.padding,
            itemMargin = me.itemMargin,
            allValue = 0;


        if (padding.all && 'auto' != padding.all) {
            allValue = parseInt(padding.all);
        }

        Mini2.applyIf(padding, {
            left: allValue,
            right: allValue,
            top: allValue,
            bottom: allValue
        });



        if (itemMargin.all && 'auto' != itemMargin.all) {
            allValue = parseInt(padding.all);
        }

        Mini2.applyIf(itemMargin, {
            left: allValue,
            right: allValue,
            top: allValue,
            bottom: allValue
        });


        me.baseRender();


        if (me.isDelayRender) {
            me.delayRenderTimer();
        }

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }
    }


}, function () {
    var me = this;

    Mini2.apply(this, arguments[0]);

    me.initComponent();


    Mini2.LoaderManager.afterRender(me);
});