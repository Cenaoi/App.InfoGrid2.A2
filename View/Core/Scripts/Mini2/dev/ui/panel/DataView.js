
/**
* 数据列, 作为卡片的方式显示
*/
Mini2.define('Mini2.ui.panel.DataView', {

    extend: 'Mini2.ui.panel.AbstractDataView',

    baseCls: 'mi-dataview',

    readonly: false,

    /**
     * 项目集合容器
     */
    itemlistEl: null,

    /**
     * 空记录显示的元素
     */
    emptyEl: null,

    /**
     * 加载更多按钮
     */
    laoderBtnEl: null,

    /**
     * 加载更多的容器
     */
    laoderEl: null,

    visible: true,

    /**
    * 模板引擎名称
    */
    engineName: 'j',

    //none, top, full,bottom,left,right
    dock: 'top',

    userCls:null,

    region: 'north',

    selectCls: 'mi-dataview-item-selected',
    focusCls: 'mi-dataview-item-focused',

    itemDefaultCls: 'mi-dataview-item-default',

    itemNotBorderCls: 'mi-dataview-item-notborder',

    /**
     * 隐藏 item 的边框
     */
    itemBorderHide: false,

    jsonMode: 'simple',

    /**
    * 新项目的索引
    */
    newItemIndex: 0,

    /**
    * 显示的模板代码
    */
    renderTpl: [
        '<div role="Panel" class="mi-dataview " style="">',
            '<div class="mi-box-inner " role="presentation" style="">',
                '<div class="mi-box-target" style="width: 100%;padding-right: 0px; position:static; ">',

                    '<div class="mi-dataview-itemlist" style="; width:100%; position:static;display: inline-block;" >',

                    '</div>',

                    '<div class="mi-dataview-empty" >',
                        '没有相关记录!',
                    '</div>',


                    '<div class="mi-box-loader" style="width:100%;height:100px;padding:6px;text-align: center; position:static;">',
                        '<a class="mi-btn mi-btn-loaders" style="width:90%;display:none;">加载更多...</a>',
                    '</div>',
                    
                '</div>',

            '</div>',



        '</div>'
    ],

    padding: {
        left: 0,
        top: 0,
        right: 0,
        bottom: 0
    },

    ui: 'default',

    /**
    * 子项目框模板
    */
    itemBoxTpl: ['<div role="dataview-item" class="mi-dataview-item " style=" "></div>'],



    //范围
    scope: null,

    scroll: 'auto',


    visible: true,


    isInit: false,



    store: false,


    /**
    * 复选框模式。默认多选。
    * @cfg { "single" | "multi"} mode
    */
    checkedMode: 'multi',


    /**
    * 项目模板
    */
    itemTemplate: false,

    /**
    * 编辑的模板
    */
    editItemTemplate: false,


    //页面参数配置
    pager: {},

    //分页控件显示
    pagerVisible: true,

    //分页控件的行数选择框
    hidePagerRowCountSelect: false,

    /**
    * 概要版面
    */
    summaryVisible: false,

    /**
    * 被选中的项目集合
    */
    selectedItems: null,

    /**
    * 焦点项目 
    */
    focusItem: null,


    onInit: function () {
        var me = this,
            store = me.store,
            isFun = Mini2.isFunction;


        if (isFun(me.itemAdded)) {
            me.bind('itemAdded', me.itemAdded);
        }

        if (isFun(me.itemRender)) {
            me.bind('itemRender', me.itemRender);
        }

        if (isFun(me.itemRendering)) {
            me.bind('itemRendering', me.itemRendering);
        }

        if (isFun(me.itemRendered)) {
            me.bind('itemRendered', me.itemRendered);
        }

        if (isFun(me.itemRenderScript)) {
            me.bind('itemRenderScript', me.itemRenderScript);
        }


    },

    isVisible: function () {
        return this.visible;
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

    setHeight: function (value) {
        var me = this;

        me.setSize(undefined, value);

        return me;
    },

    setWidth: function (value) {
        var me = this;

        me.setSize(value);

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


        me.width = me.targetWidth || me.width;
        me.height = me.targetHeight || me.height;


        if (el) {

            if (me.autoSize) {

            }
            else {
                var boxW = me.width - (padding.right + padding.left);
                var boxH = me.height - (padding.bottom + padding.top) - me.getPagetHeight();

                el.css({ 'width': me.width, 'height': me.height });
                boxInner.css({ 'width': '100%', 'height': boxH });
                boxTarget.css({ 'width': boxW, 'height': boxH });
            }
        }

        me.resize();


        return me;
    },


    resize: function () {

    },


    dymResult: null,



    /**
     * 监控加载
     */
    targetEl_scroll: function (targetEl) {
        var me = this,
            laoderBtnEl = me.laoderBtnEl,
            itemlistEl = me.itemlistEl;



        var tarTop = $(targetEl).scrollTop();
        var tarHeight = $(targetEl).height();

        var itemlistBoxHeight = $(itemlistEl).outerHeight();


        console.debug('tarTop=%d, tarHeight=%d, itemlistBoxHeight=%d ', tarTop, tarHeight, itemlistBoxHeight);


        if (tarHeight + tarTop > itemlistBoxHeight) {

            if (me.store.isLastPage()) {

            }
            else {
                if (!laoderBtnEl.is(':visible')) {

                    laoderBtnEl.show();

                    setTimeout(function () {

                        me.loadNextData();

                    }, 500);

                }
            }
        }


    },

    /**
     * 加载状态, 加载中...
     */ 
    isLoading: false,

    //加载下一页数据
    loadNextData: function () {
        var me = this,
            laoderBtnEl = me.laoderBtnEl;


        laoderBtnEl.hide();

        if (!me.dymResult) {
            me.dymResult = {};
        }


        var result = me.dymResult;

        me.store.nextPage(result);


    },



    scrollTime: null,

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

        me.itemlistEl = boxTarget.children('.mi-dataview-itemlist');

        me.emptyEl = boxTarget.children('.mi-dataview-empty');

        me.laoderEl = boxTarget.children('.mi-box-loader');


        me.laoderBtnEl = me.laoderEl.children('.mi-btn-loaders');


        if ('none' == me.scroll) {
            me.laoderEl.hide();
        }
        else {
            me.laoderBtnEl.on('click', function () {

                me.loadNextData();

            });



            $(boxTarget).scroll(function (e) {

                var scrollTime = me.scrollTime;

                if (scrollTime) {
                    scrollTime.resetStart();
                }
                else {
                    me.scrollTime = Mini2.setTimer(function () {

                        me.log.debug('DataView isLoading = ', me.isLoading);

                        if (me.isLoading) {

                            Mini2.EventManager.stopEvent(e);
                            return false;
                        }

                        me.targetEl_scroll(boxTarget);

                    }, [100, 1])
                }
            });
        }

        me.render_Summary();

        me.render_Pager();

        el.mouseup(function () {
            Mini2.ui.FocusMgr.setControl(me);
        });


        if (me.itemBorderHide) {
            boxTarget.addClass(me.itemNotBorderCls);
        }

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
            'width', 'height', 'position'
        ];

        var cssPs = {};



        for (var i = 0; i < attrList.length; i++) {
            var attrName = attrList[i];

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

            //el.css({ width: w });

            //boxInner.css({ width: w, height: h });

            //boxTarget.css({ width: w, height: h });

            var boxW = me.width - (padding.right + padding.left);
            var boxH = me.height - (padding.bottom + padding.top) - me.getPagetHeight();

            el.css({ 'width': me.width, 'height': me.height });
            boxInner.css({ 'width': '100%', 'height': boxH });
            boxTarget.css({ 'width': boxW, 'height': boxH });
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

    },




    //创建显示分页控件
    render_Pager: function () {
        "use strict";
        var me = this,
            cfg,
            pager = me.pager,
            pagerVisible = !!me.pagerVisible;

        if ((pager && pager.visible == false) ||
            me.pagerVisible == false) {
            return;
        }


        console.debug("table.hidePagerRowCountSelect = ", me.hidePagerRowCountSelect);

        me.pagerVisible = pagerVisible;

        cfg = Mini2.apply(me.pager, {
            renderTo: me.el,
            store: me.getStore(),
            visible: pagerVisible,
            hideRowCountSelect: me.hidePagerRowCountSelect || false,
            click: me.pager_click
        });

        pager = Mini2.create('Mini2.ui.Pagination', cfg);
        pager.render();

        me.pager = pager;
    },

    //页点击事件
    pager_click: function (page, store) {
        var me = this;

        store.loadPage(page);

    },


    //创建概要版面
    render_Summary: function () {
        "use strict";
        var me = this,
            summary;

        if (me.summaryVisible) {

        }
    },

    //获取概要面板高度
    getSummaryHeight: function () {
        "use strict";
        var me = this,
            summary = me.summary;

        if (summary && summary.muid) {
            return summary.height;
        }

        return 0;
    },


    //获取页面高度
    getPagetHeight: function () {
        "use strict";
        var me = this,
            pager = me.pager;

        if (!me.pagerVisible) {
            return 0;
        }

        if (pager && pager.muid) {
            return Mini2.SystemInfo.pagination.height || 24;
        }

        return 0;
    },





    /**
    * 渲染 Item
    */
    renderItemInner: function (index, dataItem) {
        "use strict";
        var me = this,
            valueStr,
            rendererFn;


        rendererFn = me.renderer_j;


        try {
            valueStr = rendererFn.call(me, index, dataItem);

        }
        catch (ex) {
            console.error("输出错误 ", ex);

            return '';
        }

        return valueStr;
    },

    renderItemList: function () {
        "use strict";
        var me = this,
            len,
            store = me.getStore(),
            records;


        if (store) {
            records = store.data;

            if (!records) {
                return;
            }

            me.itemRemoteAll();

            len = records.length;

            me.delayData = {
                i: 0,
                len: len,
                items: records
            };

            me.loopLoadItem();

        }
        else {
            records = me.data;

            me.itemInsert(0, records);
        }
        
    },

    /**
    * 单选项
    */
    itemSelect_single: function (dataviewItem, check) {
        var me = this,
            selectCls = me.selectCls,
            itemEl = $(dataviewItem);
        
        $(me.selectedItems).removeClass(selectCls);
        
        if (check) {
            $(dataviewItem).toggleClass(selectCls, true);

            if (Mini2.isArray(dataviewItem)) {
                me.selectedItems = dataviewItem;
            }
            else {
                me.selectedItems = [dataviewItem];
            }
        }
        else {
            me.selectedItems = [];
        }
   },

    /**
    * 多选
    */
    itemSelect_multi: function (dataviewItem, check) {
        var me = this,
            itemEl = $(dataviewItem);


        if (check) {
            Mini2.Array.include(me.selectedItems, dataviewItem);
        }
        else {
            Mini2.Array.remove(me.selectedItems, dataviewItem);
        }
    },


    /**
    * 设置焦点项目
    */
    setFocusItem: function (dataviewItem) {
        "use strict";
        var me = this,
            lastItemEl,
            cancel = false,
            focusCls = me.focusCls,
            isFocus,
            focusItem = me.focusItem,
            itemEl = $(dataviewItem);

        lastItemEl = me.focusItem;
        

        isFocus = itemEl.hasClass(focusCls);

        if (focusItem && dataviewItem !== focusItem) {
            $(focusItem).removeClass(focusCls);
        }

        me.focusItem = dataviewItem;
        itemEl.addClass(focusCls);

        me.trigger('itemFocused', {
            'old': lastItemEl ,
            'item' : dataviewItem
        });

    },

    itemSelect: function (dataviewItem) {
        var me = this,
            itemEl = $(dataviewItem),
            check = false,
            selectCls = me.selectCls,
            focusCls = me.focusCls,
            checkedMode = me.checkedMode;

        check = itemEl.hasClass(selectCls);

        check = !check;


        $(itemEl).toggleClass(selectCls, check);

        console.debug("itemSelect = ", itemEl);

        console.debug("checkedMode = ", checkedMode);

        if ('single' == checkedMode) {
            me.itemSelect_single(dataviewItem, check);
        }
        else if ('multi' == checkedMode) {
            me.itemSelect_multi(dataviewItem, check);
        }
    },


    renderer_j: function (index, record) {
        "use strict";
        var me = this,
            jTemp = me.jTemplate;

        if (!jTemp) {

            jTemp = me.jTemplate = $.createTemplate(me.itemTemplate);
        }

        if ('free' == record.fieldModeId) {
            record = record.raw;
        }

        return jTemp.get(record, {
            index: index,
        });

    },


    /**
    * 删除全部项目
    */
    itemRemoteAll: function () {
        "use strict";
        var me = this,
            itemlistEl = me.itemlistEl,
            //targetEl = me.boxTarget,
            rows = $(itemlistEl).children('.mi-dataview-item');

        $(rows).remove();

        me.selectedItems = [];
        me.focusItem = null;

    },

    /**
     * 重置所有项目 的 索引号
     */
    resetItmesIndex: function () {

    },


    /**
    * 渲染 item box
    */
    renderItemBox: function (newId, record) {
        var me = this,
            boxEl = $(me.itemBoxTpl);

        boxEl.attr('data-muid', newId);

        boxEl.css({
            width: me.itemWidth,
            height: me.itemHeight
        });

        boxEl.data('record', record);

        boxEl.addClass(me.itemDefaultCls);


        if (me.itemCls) {
            boxEl.addClass(me.itemCls);
        }

        return boxEl;
    },



    /**
    * artTemplate 腾信模板引擎
    */
    rendererHref_art: function (item, record) {
        "use strict";
        var me = this,
            valueStr,
            artTemp = item.data('artTemplate');

        if (!artTemp) {
            artTemp = template.compile(item.attr('href'));
            item.data('artTemplate', artTemp);
        }

        valueStr = artTemp({
            T: record
        });

        return valueStr;
    },

    item_command: function (sender, boxEl) {
        "use strict";
        var me = this,
            itemEl = $(sender),

            record,
            cmd,
            cmdParam;

        record = boxEl.data('record');



        cmd = itemEl.attr('command');

        if (!Mini2.isBlank(cmd)) {
            cmdParam = itemEl.attr('commandParam');


            me.onCommand(cmd, cmdParam, record);
        }
        else {

            var target = itemEl.attr('target');
            var href = itemEl.attr('href')

            var rendererHref = this.rendererHref_art;


            try {
                href = rendererHref.call(this, itemEl, record);
            }
            catch (ex) {
                console.error('地址模板解析处理失败,', itemEl);
                return;
            }

            target = target.toLowerCase();

            if (Mini2.isBlank(target)) {

                window.location.href = href;

            }
            else if ('ecview' == target) {

                EcView.show(href, itemEl.attr('targetText'));

                return false;
            }
            else if ('dialog' == target) {

                var frm = Mini2.createTop('widget.window', {
                    url: href,
                    text: itemEl.attr('targetText'),
                    width: 400,
                    height: 300,
                    mode: true,
                    state: 'max'
                });

                frm.show();

                return false;
            }
            else {

                console.debug('未知标识', target);

            }

        }
    },


    /**
     * 重置空消息
     */
    resetEmptyMsg: function () {
        var me = this,
            store = me.store,
            emptyEl = me.emptyEl;

        if (store.data.length) {
            emptyEl.hide();
        }
        else {
            emptyEl.show();
        }

    },


    /**
    * 插入记录
    */
    itemInsert: function (index, data, outData) {
        "use strict";
        var me = this,
            itemlistEl = me.itemlistEl,
            //targetEl = me.boxTarget,
            i,
            newId,
            itemInnerStr,
            count,
            record,
            records,
            cmdEls,
            rowEls = [],
            rows;


        if (outData) {
            outData.rowEls = rowEls;
        }

        rows = $(itemlistEl).children();

        count = rows.length;


        if (index > count) {
            index = count;
        }

        if (Mini2.isArray(data)) {
            records = data;
        }
        else {
            records = [data];
        }

        if (0 === records.length) {
            return;
        }

        for (i = 0; i < records.length; i++) {
            record = records[i];
            //rowEl = me.renderRow(me.tbodyEl, record, me.newItemIndex);


            newId = Mini2.newId();

            itemInnerStr = me.renderItemInner(i, record);

            var boxEl = me.renderItemBox(newId, record);

            boxEl.append(itemInnerStr);


            //
            rowEls.push(boxEl);


            cmdEls = boxEl.find('*[command],a[target]');

            if (cmdEls.length) {
                cmdEls.muBind('click', function () {
                    return me.item_command(this, boxEl);
                });
            }

        }

        itemlistEl.append(rowEls);

        $(rowEls).each(function () {

            var itemEl = this,
                cancel,
                record = $(itemEl).data('record');

            cancel = me.trigger('itemRendering', {
                cancel : false,
                itemEl: this,
                record : record
            });

            if (true === cancel) { return; }

            //触发 itemRender 事件
            cancel = me.trigger('itemRender', {
                itemEl: itemEl,
                record : record
            });

            if (true === cancel) { return; }

            cancel = me.trigger('itemRenderScript', {

                itemEl: itemEl,
                record: record

            });

            cancel = me.trigger('itemRendered', {
                itemEl: itemEl,
                record : record
            });

            //触发 itemAdded 添加成功事件
            cancel = me.trigger('itemAdded', {
                itemEl: itemEl,
                record: record
            });

            me.itemAfterRender(itemEl, record);

        });

        me.resetItmesIndex();



        if (!me.readonly) {
            $(rowEls).each(function () {

                $(this).muBind('click', function () {
                    var dvItem = this,
                        store = me.store;


                    if ('none' != me.checkedMode) {
                        me.itemSelect(dvItem);
                    }

                    if (store) {

                        var record = $(dvItem).data('record');

                        if (record) {
                            store.setCurrent(record);
                        }
                    }
                    else {
                        me.setFocusItem(dvItem);
                    }

                });

            });
        }



        //for (i = 0; i < rowEls.length; i++) {
        //    rowEl = rowEls[i];
        //    record = records[i];

        //    me.bindEvent_ForRow(record, rowEl);

        //    me.bindEvent_ForCell(record, rowEl);
        //}


        return me;
    },


    getJsonForRecord: function (record) {
        "use strict";
        var me = this,
            i;

        var recObj = {
            action: "none",
            id: record.getId(),
            clientId: record.muid,
            index: 0,
            values: {}
        };


        if (record.dirty) {
            recObj.action = 'modified';
        }

        recObj.values = Mini2.clone(record.data);

        return recObj;
    },

    //触发命令提交函数
    onCommand: function (cmdName, cmdParam, record) {
        "use strict";
        var me = this,
            recordObj,
            recordJson;

        if ('full' == me.jsonMode) {
            recordObj = me.getJsonForRecord(record);  //整个实体提交命令太大,无法一次性提交
        }
        else {
            recordObj = {
                action: "none",
                id: record.getId(),
                clientId: record.muid,
                index: 0,
                values: {}
            };
        }

        recordJson = Mini2.Json.toJson(recordObj);


        var dataName = this.id + "-" + Mini2.Guid.newGuid('N');

        var actionPs = {
            cmdName: cmdName,
            cmdParam: cmdParam,
            record: 'ref ' + dataName
        };

        var data = {};

        data[dataName] = recordJson;

        widget1.subMethod('form:first', {
            subName: me.id,
            subMethod: 'PreCommand',
            actionPs: actionPs,
            data: data
        });

    },



    //
    // jsonMode = sapmle-简单类型 | full-全部类型;
    getJson: function (jsonMode) {
        "use strict";
        var me = this,
            itemlistEl = me.itemlistEl,
            json,
            checkObj = {
                records: []
            };


        jsonMode = jsonMode || me.jsonMode;

        var rowEl, record, cell, imgEl, isCheck,
            rows = $(itemlistEl).children('.' + me.selectCls);


        $(rows).each(function () {
            rowEl = $(this);

            record = rowEl.data('record');

            var recObj = {
                action: 'none',
                id: record.getId(),
                clientId: record.muid,
                index: rowEl.index()
            };

            if (record.dirty) {
                recObj.action = 'modified';
            }

            if (!recObj.id) {
                recObj.action = 'added';
                recObj.values = Mini2.clone(record.data);
            }
            else if ('full' == jsonMode) {
                recObj.values = Mini2.clone(record.data);
            }


            checkObj.records.push(recObj);

        });

        json = JSON.stringify(checkObj);


        return json;
    },

    render: function () {
        var me = this,
            store = me.store;

        if (Mini2.isArray(me.itemTemplate)) {
            me.itemTemplate = Mini2.join(me.itemTemplate);
        }


        me.onInit();


        me.selectedItems = [];

        me.baseRender();



        me.itemBoxTpl = Mini2.join(me.itemBoxTpl);


        if (Mini2.isString(store)) {

            store = Mini2.data.StoreManager.lookup(store);

            me.store = store;
        }


        if (store && store.isStore) {
            me.bindStore(store);
        }
        else {
            store = me.getStore();
        }

        me.renderItemList();


        $(me.el).data('me', me);

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }

        return me;

    },



    itemAfterRender: function (parentEl,record) {
        var me = this,
            item,
            items,
            len,
            i;

        items = me.getBindItems(parentEl);
        len = items.length;



        for (i = 0; i < len; i++) {
            item = items[i];

            //item.store = me.store;
            //item.record = record;


            if (item.setRecord) {
                item.setRecord(record);
            }
            else {
                item.record = record;
            }

            //item.oldValue = record[item.dataField];

            $(item).muBind('focusout', function () {

                me.itemWidget_focusout.call(me, this);
            });


            //如果是复选框, 就特殊处理,一旦值发生变化,直接触发事件
            if (item.isCheckbox || item.isCheckGroup) {

                $(item).muBind('changed', function () {

                    me.itemWidget_changed.call(me, this);
                });

            }

            //先记录一下控件的原始状态
            if (item.getReadOnly()) {
                item.allowChangeReadonly = false;
            }

        }

    },

    itemWidget_focusout:function(sender){
        var me = this,
            item = sender,
            value,
            field = item.dataField,
            record = item.record;

        if (item.readOnly || !item.isValueChanged()) {

            return me;
        }


        if (item.isRangeControl) {

            if (item.startDataField && item.isValueChanged('start')) {

                var value = item.getStartValue();

                item.setDirty(true);
                record.set(item.startDataField, value);
            }

            if (item.endDataField && item.isValueChanged('end')) {

                var value = item.getEndValue();

                item.setDirty(true);
                record.set(item.endDataField, value);
            }

        }
        else {

            if (item.isValueChanged()) {
                var value = item.getValue();

                item.setDirty(true);
                record.set(field, value);
            }
        }


    },

    itemWidget_changed:function(sender){
        var me = this,
            item = sender,
            value,
            field = item.dataField,
            record = item.record;

        value = item.getValue();
            
        item.setDirty(true);
        record.set(field, value);

    },


    /**
     * 查找所有 Mini2 控件
     * @param {domEl} parentEl 上级html节点
     * @param {Array} list 搜索到的集合 
     */
    findItems: function (parentEl, list) {
        var me = this;

        if (!list) {
            list = [];
        }

        var childItems = $(parentEl).children();

        $(childItems).each(function () {
            var childEl = this;

            var data = $(childEl).data('me');

            if (data) {
                list.push(childEl);
                //return false;
            }
            else {
                me.findItems(childEl, list);
            }

        });

        return list;
    },

    /**
     * 获取需要绑定的项目
     */
    getBindItems: function (parentEl) {
        var me = this,
            store = me.store,
            items,
            len,
            i = 0,
            dbItems = [];


        //items = me.prepareItems(items);

        items = me.findItems(parentEl);


        len = items.length;


        for (i = 0; i < len; i++) {
            var item = items[i];

            item = me.getItemObj(item);

            if (!item) {
                continue;
            }


            if (item.isRangeControl) {

                if (item.startDataField || item.endDataField) {
                    dbItems.push(item);
                }

            }
            else if (item.dataField) {
                dbItems.push(item);;
            }


        }

        return dbItems;
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

                if (itemMi.labelable) {

                    if (me.itemLabelWidth) {
                        itemMi.labelable.setWidth(me.itemLabelWidth);
                    }

                    if (me.itemLabelAlign) {
                        itemMi.labelable.setLabelAlign(me.itemLabelAlign);
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
    }


});