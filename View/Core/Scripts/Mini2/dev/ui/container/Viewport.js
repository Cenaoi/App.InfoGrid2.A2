

/// <reference path="../../Mini.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />



Mini2.define("Mini2.ui.container.Viewport", {

    log: Mini2.Logger.getLogger('Mini2.ui.container.Viewport',false),

    alias: 'Mini2.ui.Viewport',

    layout: 'border',

    owner: window,

    /**
    * 容器模式
    */
    isContainer:false,

    /**
    * 各个方位之间的间距
    */
    space : 0,

    el: null,

    //    northItems: [], //北
    //    southItems: [], //南
    //    westItems: [],   //西
    //    eastItems: [], //东
    //    centerItems: [], //中间

    //    region: {
    //        north: [],
    //        south: [],
    //        west: [],
    //        east: [],
    //        center: []
    //    },
    
    //disable: true,

    //宽度补充
    widthOffset: 0,

    //高度补充
    heightOffset: 0,

    /**
     * 版面锚固定的项目
     */
    anchorfixedItems : null,

    margin: {
        top: 0
    },

    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this;

        //me.disabled = false;

        me.length = 0;
        me.region = {
            north: [],
            south: [],
            west: [],
            east: [],
            center: []
        };

        me.isHide_north = false;
        me.isHide_south = false;
        me.isHide_west = false;
        me.isHide_east = false;
        
    },

    //设置为激活状态
    enable:function(){
        this.disabled = false;
    },

    //设置为无效状态
    disable:function(){
        this.disabled = true;
    },


    toggleRegion: function (region) {
        "use strict";
        var me = this,
            isVisibled = me.isVisibled(region);


        if (isVisibled) {
            me.hideRegion(region);
        }
        else {
            me.showRegion(region);
        }

        return me;
    },

    isVisibled: function (region) {
        "use strict";
        var me = this,
            tag,
            hide = false;

        tag = 'isHide_' + region;

        hide = me[tag];

        return !hide;
    },


    //显示版面
    showRegion: function (region) {
        "use strict";
        var me = this,
            el = me.el,
            ms = me.region,
            panel,
            items = ms[region];

        if (items && items.length) {

            if ('west' == region) {
                if (!me.isHide_west) { return; }

                me.isHide_west = false;

                panel = items[0];

                var panelWidth = panel.getWidth();

                var x0 = 0;
                var x1 = -panelWidth;

                panel.show().animate({ 'left': 0 }, 'fast', 'swing');

            }
            else if ('east' == region) {    //东
                if (!me.isHide_east) { return; }
                me.isHide_east = false;

                panel = items[0];

                var viewWidth = el.width();
                var panelWidth = panel.getWidth();

                var x0 = viewWidth - panelWidth;
                var x1 = viewWidth;

                panel.setLeft(x1).show().animate({ 'left': x0 }, 'fast', 'swing');

            }
            else if ('north' == region) {    //北

                if (!me.isHide_north) { return; }

                me.isHide_north = false;

                panel = items[0];

                var panelHeight = panel.getHeight();

                var y0 = 0;
                var y1 = -panelHeight;

                panel.setTop(y1).show().animate({ 'top': y0 }, 'fast', 'swing');


            }
            else if ('south' == region) {    //南
                if (!me.isHide_south) { return; }
                me.isHide_south = false;

                panel = items[0];

                var viewHeight = el.height();
                var panelHeight = panel.getHeight();

                var y0 = viewHeight - panelHeight;
                var x1 = viewHeight;

                panel.setTop(x1).show().animate({ 'top': x0 }, 'fast', 'swing');
            }

            me.resize(me.owner);
        }

    },

    //隐藏版面
    hideRegion: function (region) {
        "use strict";
        var me = this,
            el = me.el,
            ms = me.region,
            margin = me.margin,
            panel,
            items = ms[region];

        if (items && items.length) {

            if ('west' == region) { //西

                if (me.isHide_west) { return; }

                me.isHide_west = true;

                panel = items[0];

                var panelWidth = panel.getWidth();

                var x0 = 0;
                var x1 = -panelWidth;

                panel.animate({ 'left': x1 }, 'fast', 'swing', function () {
                    panel.hide();
                });

            }
            else if ('east' == region) {    //东

                if (me.isHide_east) { return; }

                me.isHide_east = true;

                panel = items[0];

                var viewWidth = el.width();
                var panelWidth = panel.getWidth();

                var x0 = viewWidth - panelWidth;
                var x1 = viewWidth;

                panel.animate({ 'left': x1 }, 'fast', 'swing', function () {
                    panel.hide();
                });

            }
            else if ('north' == region) {    //北

                if (me.isHide_north) { return; }

                me.isHide_north = true;

                panel = items[0];

                var panelHeight = panel.getHeight();

                var y0 = margin.top;
                var y1 = -panelHeight + margin.top;

                panel.animate({ 'top': y1, 'z-index': 800 }, 'fast', 'swing', function () {
                    panel.hide();
                });

            }
            else if ('south' == region) {    //南

            }

            me.resize(me.owner);
        }
    },


    winResize: function (e) {
        "use strict";
        var me = e.data['owner'],
            log = me.log;

        me.resize(me.owner);
        
    },





    resize: function (owner) {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            ms = me.region,
            h,
            heightAll ,
            w,
            space = me.space,
            margin = me.margin,

            mTop = margin.top || 0,
            mBtm = margin.buttom || 0,

            h0 = 0, //north 北方-的高度
            h1 = 0,
            h2 = 0, //south 南方-的高度

            w0 = 0,
            w1 = 0,
            w2 = 0;

        owner = owner || me.owner;

        //log.debug("viewport.resize(...)");
        
        h0 = me.isHide_north ? 0 : me.getItemsHeight(ms.north);
        h2 = me.isHide_south ? 0 : me.getItemsHeight(ms.south);


        w0 = me.isHide_west ? 0 : me.getItemsWidth(ms.west);
        w2 = me.isHide_east ? 0 : me.getItemsWidth(ms.east);


        if (owner && owner.muid) {
            w = owner.getWidth();
            h = owner.getHeight();
        }
        else {

            var ownerWidth = $(owner).width();

            if (me.isContainer) {

                if (ownerWidth >= 1200) {
                    w = 1170;
                }
                else if (ownerWidth >= 992) {
                    w = 970;
                }
                else if (ownerWidth >= 768) {
                    w = 750;
                }
                else {
                    w = ownerWidth;
                }
            }
            else {
                w = ownerWidth;
            }

            h = $(owner).height();
        }

        //if ('widget1_I_TopViewport' != me.clientId) {
            
        //    console.log("viewport.id = '%s'", me.clientId);

        //    console.log("scroll = " + me.scroll);

        //    console.log("win.Height = ", $(window).height());

        //    console.log("mTop = %d, mBtm = %d", mTop, mBtm);
        //}

        
        h -= (mTop + mBtm);

        h += me.heightOffset;   
        w += me.widthOffset;


        el.css({ width: w , height: $(window).height() - (mTop + mBtm) });


        //if ('widget1_I_TopViewport' != me.clientId) {

        //    console.log("h = %d", h);

        //    console.log("h0 = %d ,h2 = %d", h0, h2);
        //}

        h1 = h - h0 - h2;

        var minCentreH = me.getItemsHeight(ms.center, h1);

        if ( h1 < minCentreH) {

            heightAll = h0 + h1 + h2;

            if ('none' != me.scroll) {
                

            }

            //console.log("减去滚动条");

            w -= 18;    //减去滚动条
            el.css({
                'overflow-y': 'scroll'
            });
        }
        else {
            heightAll = h;

            el.css({
                'overflow-y': 'hidden'
            });
        }
        
        //if ('widget1_I_TopViewport' != me.clientId) {
        //    console.log("minCentreH = ", minCentreH);

        //    console.log("h1 = %d", h1);
        //}

        w1 = w - w0 - w2;

        //设置 [北, 南] 的高度和宽度
        if (ms.north.length || ms.south.length) {
            me.setItemsTop(0, ms.north);

            if (h1 < minCentreH) {
                me.setItemsTop(h0 + minCentreH, ms.south);
            }
            else {
                me.setItemsTop(h0 + h1, ms.south);
            }



            me.setItemsWidth(w, ms.north);
            me.setItemsWidth(w, ms.south);
        }


        //设置[东,西] 的 宽度,坐标
        if (ms.west.length || ms.east.length) {

            var hC0 = h0 + (ms.north.length ? space : 0);

            me.setItemsTop(hC0, ms.west);
            me.setItemsTop(hC0, ms.east);

            me.setItemsLeft(0, ms.west);
            me.setItemsLeft(w0 + w1, ms.east);

            var hC1 = h1 - (ms.north.length ? space - 1 : 0) - (ms.south.length ? space - 1 : 0);

            me.setItemsHeight(hC1, ms.west);
            me.setItemsHeight(hC1, ms.east);
        }

        var hPaddingH1 = 0;
        var hPaddingH2 = 0;

        //设置中心部分
        if (ms.center.length) {

            hPaddingH1 = (ms.north.length ? space : 0);
            hPaddingH2 = (ms.south.length ? space : 0);

            var wC0 = w0 + (ms.west.length ? space : 0);
            var wC1 = w1 - (ms.west.length ? space : 0) - (ms.east.length ? space : 0);

            var hC0 = h0 + hPaddingH1;
            var hC1 = h1 - (hPaddingH1 > 0 ? hPaddingH1 - 1 : 0) - (hPaddingH2 > 0 ? hPaddingH2 - 1 : 0);

            //if ('widget1_I_TopViewport' != me.clientId) {
            //    console.debug("hC0 = ", hC0);
            //    console.debug("hC1 = ", hC1);
            //}


            me.setItemsLeft(wC0, ms.center);
            me.setItemsTop(hC0, ms.center);


            me.setItemsWidth(wC1, ms.center);
            me.setItemsHeight(hC1, ms.center);
        }

        var allH = heightAll - (hPaddingH1 > 0 ? hPaddingH1 - 1 : 0) - (hPaddingH2 > 0 ? hPaddingH2 - 1 : 0);
        var allW = w;

        if (heightAll > h) {
            allW -= 20;
        }



        if ('none' == me.scroll) {

            me.innerEl.css({
                width: allW,
                height: h
            });
        }
        else {

            me.innerEl.css({
                width: allW,
                height: allH
            });
        }




        var items = me.anchorfixedItems;

        if (items) {

            $(items).each(function () {
                var item = this;
                item._tmpAnchor = null;
            });
            
            me.preAnchorFixedItem();
            
        }

        return me;
    },

    setItemsWidth: function (w, items) {
        "use strict";
        var me = this,
            item;

        $(items).each(function () {
            item = this;

            if ('relative' == item.position) {
                return;
            }

            if (item.isVisible && item.isVisible()) {
                item.setWidth(w);
            }
        });
    },

    setItemsHeight: function (h, items) {
        "use strict";
        var me = this,
            item;

        $(items).each(function () {
            item = this;

            if ('relative' == item.position) {
                return;
            }

            if (item.isVisible && item.isVisible()) {
                item.setHeight(h);
            }
        });
    },


    setItemsTop: function (y, items) {
        "use strict";
        var me = this,
            item,
            v = y;

        $(items).each(function () {
            item = this;

            if ('relative' == item.position ) {
                return;
            }
            if (item.isVisible && item.isVisible()) {
                item.setTop(v);


                var minHeight = item.minHeight || 0;
                var height = item.getHeight();

                if (height < minHeight) {
                    v += minHeight;
                }
                else {
                    v += height;
                }

            }
        });

        return me;
    },

    setItemsLeft: function (x, items) {
        "use strict";
        var me = this,
            item,
            v = x;

        $(items).each(function () {
            item = this;

            if ('relative' == item.position) {
                return;
            }


            if (item.isVisible && item.isVisible()) {
                item.setLeft(v);
                v += item.getWidth();
            }
        });

        return me;
    },

    getItemsWidth: function (items) {
        "use strict";
        var me = this,
            item,
            v = 0;

        $(items).each(function () {
            item = this;
            if ('relative' == item.position) {
                return;
            }

            if (item.isVisible && item.isVisible()) {
                v += item.getWidth();
            }
        });

        return v;
    },

    //curHeight : 
    getItemsHeight: function (items, curHeight) {
        "use strict";
        var me = this,
            item,
            v = 0;

        $(items).each(function () {
            item = this;
            if ('relative' == item.position) {
                return;
            }
            if (item.isVisible && item.isVisible()) {


                var minHeight = item.minHeight || 0;
                var height = item.getHeight();

                if (height < minHeight) {
                    v += minHeight;
                }
                else {
                    v += height;
                }
            }
        });



        return v;
    },


    getItem: function (ps) {
        "use strict";
        var me = this,
            panel;

        if (ps.muid) {
            return ps;
        }


        panel = Mini2.create('Mini2.ui.panel.Panel', ps);

        panel.render();

        return panel;
    },


    /**
     * 初始化固定锚的项目
     */
    initAnchorFixedItems: function () {
        var me = this,
            el = me.el,
            items = [],
            westItems = me.region['west'],
            eastItems = me.region['east'];   //西

        $(westItems, eastItems).each(function () {

            if ('topbottom' == this.anchorFixedType) {
                items.push(this);
            }
        });

        if (items.length) {

            me.anchorfixedItems = items;

            $(el).scroll(function () {

                me.preAnchorFixedItem();

            });
        }
    },


    scrollTop: function (value) {
        var me = this,
            el = me.el;

        el.scrollTop(value);

        return me;
    },

    preAnchorFixedItem: function () {
        var me = this,
            el = me.el,
            items = me.anchorfixedItems,
            sTop = $(el).scrollTop(),
            vpHeight = $(el).height();


        $(items).each(function () {

            var item = this,
                newH,
                newTop = -1,
                tmpAnchor = item._tmpAnchor;

            if (!tmpAnchor) {

                tmpAnchor = {
                    top: parseInt(item.getTop()),
                    height: parseInt(item.getHeight()),

                    tmpTop: 0,     //临时y
                    tmpHeight: 0    //临时高度
                };

                item._tmpAnchor = tmpAnchor;
            }


            if (sTop > tmpAnchor.top) {
                newTop = sTop - 1;
            }
            else {
                newTop = tmpAnchor.top;
            }

            if (newTop != tmpAnchor.tmpTop) {
                tmpAnchor.tmpTop = newTop;
                item.setTop(newTop);
            }

            if (sTop > 0) {

                newH = sTop + tmpAnchor.height;

                if (newH > vpHeight) {
                    newH = vpHeight ;
                }

                if (tmpAnchor.tmpHeight != newH) {
                    tmpAnchor.tmpHeight = newH;
                    item.setHeight(newH);
                }

            }



        });


    },


    baseRender: function () {
        "use strict";
        var me = this,
            el,
            innerEl,
            i = 0,
            margin = me.margin,
            item,
            len,
            items = me.items;

        me.el = el = $(me.contentEl);

        el.addClass('mi-viewport');
        me.innerEl = innerEl = $(el).children('.mi-viewport-inner');

        if (me.isContainer) {

            el.addClass('mi-viewport-container');

            el.css({
                'margin-left': 'auto',
                'margin-right': 'auto'
            });

        }
        else {
            

            if (margin) {
                el.css({
                    'position': 'absolute',
                    'top': margin.top
                });
            }
        }

        if ('none' == me.scroll ) {
            el.css('overflow', 'hidden');
            innerEl.css('overflow', 'hidden');
        }
        else {
            el.css('overflow-x', 'hidden');
            innerEl.css('overflow-x', 'hidden');

            setTimeout(function () {
                me.initAnchorFixedItems();  //初始化固定锚的版面

            }, 500);
            

        }

        if (items) {

            len = items.length;

            //1.分析结构
            for (i; i < len; i++) {

                item = items[i];

                if (!item || !item.contentEl) { continue; }

                if (!Mini2.isString(item.contentEl)) { continue; }


                var c = item.contentEl.substr(0, 1);

                if (c == '#') {
                    var panel = Mini2.create('Mini2.ui.panel.Panel', item);

                    panel.render();



                    items[i] = panel;
                }
            }
        }

    },

    afterRender: function () {
        "use strict";
        var me = this,
            i = 0,
            item,
            region,
            obj,
            ms = me.region,
            items = me.items,
            rItem;


        if (items) {

            //1.分析结构
            for (i; i < items.length; i++) {

                item = items[i];

                if (item.isAfterRender) {
                    item = window[item.id];
                }

                region = item.region || 'north';
                obj = me.getItem(item);


                obj.el.addClass('mi-box-item');
                //if ( 'relative' != obj.position ) {
                //    obj.el.addClass('mi-box-item');
                //}

                rItem = ms[region];

                if (rItem) {
                    rItem.push(obj);
                }
                else {
                    ms['center'].push(obj);
                }
            };

            delete me.items;
        }

        return me;
    },

    setZIndex: function (items, zIndex) {
        "use strict";
        var me = this,
            i,
            n;

        if (items) {

            n = items.length;

            while (n--) {
                items[n].css('z-index', zIndex);
            }
        }

        return me;
    },

    //        north: [],
    //        south: [],
    //        west: [],
    //        east: [],
    //        center: []

    //重新设置各个版面的z坐标
    resetZIndex: function () {
        "use strict";
        var me = this,
            region = me.region,
            items,
            setZIndex = me.setZIndex;

        setZIndex(region['center'], 50);

        setZIndex(region['north'], 40);
        setZIndex(region['south'], 30);
        setZIndex(region['west'], 20);
        setZIndex(region['east'], 10);

    },

    render: function () {
        "use strict";
        var me = this;

        me.baseRender();

        me.resetZIndex();

        Mini2.LoaderManager.afterRender(me);

        Mini2.LoaderManager.resize(me);



        //        setTimeout(function () {
        //            me.resize(me.owner);
        //        }, 100);

        //        $(window).bind('resize', { 'owner': me }, me.winResize);
    }

}, function () {
    var me = this;

    Mini2.apply(this, arguments[0]);

    me.initComponent();

});