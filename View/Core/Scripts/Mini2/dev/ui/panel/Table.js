/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini.js" />

/// <reference path="../Mini-more.js" />
/// <reference path="../lang/Array.js" />

/// <reference path="template.min.js" />
/// <reference path="EventManager.js" />
/// <reference path="FocusManager.js" />

/// <reference path="grid/column/Column.js" />

/***
 
 var grid = new Mini2.ui.GridBase({
    renderTo : "grid1",
    store : "store1",
    columns: [
    {
        miType: "col",
        width:"120px",
        dbField :"NAME"
    },{
        miType: "col",
        width:"120px",
        dbField :"AGE"
    },{
        miType: "col",
        width:"120px",
        dbField :"LOGIN_NAME"
    }]
 });

 grid.render(); 

 */



Mini2.define('Mini2.ui.panel.Table', {

    extend: 'Mini2.ui.panel.AbstractTable',

    log: Mini2.Logger.getLogger('Mini2.ui.panel.Table',console),  //日志管理器
        

    //范围
    scope:null,


    focusMgr: Mini2.ui.FocusMgr,    //焦点管理器


    scroll:'auto',

    visible: true,

    defaultEditorMode: 'auto',

    //重新设置 div 的宽度。。以后可以取消
    isResetCellWidth: true,

    readOnly: false,

    //行的线
    rowLines: true,

    //列的线
    columnLines: true,

    //显示焦点边框
    focusBorders: true,

    isInit: false,

    columns: [],

    m_Cols: [],    //列集合

    m_ColGroups: null,

    //none, top, full,bottom,left,right
    dock: 'top',


    m_Table: null,
    tbodyEl: null,


    store: false,


    bodyEl: null,

    el: null,

    cellValues: {},

    //选择单元格，自动行打钩
    autoRowCheck: false,


    //新行的索引
    m_NewRowIndex: 0,

    //焦点行
    lastRowFocused: null,
    curRowFocused: null,


    //当前获取焦点的单元格
    m_FocusCell: null,

    //最后获取焦点的单元格
    m_LastCell: null,

    m_LastEditor: null,

    width: '100%',

    height: '600px',

    //标题容器
    headerContainer: null,

    //被选中的记录
    checkedRecords: [],

    //被选中的行元素
    checkedRowEls: [],

    /**
    * 复选框模式。默认多选。
    * @cfg {"SINGLE"/"SIMPLE"/"MULTI"} mode
    */
    checkedMode: 'MULTI',


    /**
    * 行选中的列。全选，反选。。。的列
    */
    rowCheckColumn: null,

    //页面参数配置
    pager: {},


    editor: null,


    //排序开关
    sortable: true,

    //分页控件显示
    pagerVisible: true,

    //分页控件的行数选择框
    hidePagerRowCountSelect: false,


    cellTpl: ['<td role="gridcell" class="mi-grid-cell mi-grid-td ">',
                '<div class="mi-grid-cell-inner">',
    //'<%= value %>',
                '</div>',
             '</td>'],

    //样式模式 'json' | 'css'
    rowStyleType: 'json',

    //样式字段
    rowStyleField: 'ROW_STYLE_JSON',

    //样式激活
    rowStyleEnabled: true,

    //隔行样式.激活
    rowAltEnabled: true,



    //最小高度
    //minHeight: 0,

    //最小宽度
    //minWidth:0,

    //最大高度
    //maxHeight:0,

    //最大宽度
    //maxWidth:0

    //提交的 json 数据模式。 simpe = 简单数据, full=全部数据
    jsonMode: 'simple',

    //单元格双击事件
    cellDbclickEvents: false,


    


    //单元格双击事件
    _cellDbclick: function (fn) {
        "use strict";
        var me = this,
            evts = me.cellDbclickEvents;
                    
        if (Mini2.isFunction(fn)) {

            if (evts == false) {
                me.cellDbclickEvents = evts = [];
            }

            evts.push(fn);
        }        
    },
    

    // 触发单元格双击事件
    onCellDbclick: function (rowEl, cellEl, record) {
        "use strict";
        var me = this,
            i,
            fn,
            evts = me.cellDbclickEvents;

        if (evts) {

            for (i = 0; i < evts.length; i++) {
                fn = evts[i];

                fn.call(me, {
                    rowEl: rowEl,
                    cellEl: cellEl,
                    record: record
                });

            }

        }
    },

    updateLayout: function (owner) {
        var me = this,
            headerContainer = me.headerContainer,
            w,
            h;

        //if (owner.muid) {
        //    w = owner.getWidth();
        //    h = owner.getHeight();
        //}


        headerContainer.resetWidth();

        me.resetBodyWidth();

    },

    createColGroup: function () {
        "use strict";
        /// <summary>创建列组</summary>

        var me = this,
            i = 0,
            col,
            cols = me.m_Cols,
            colHtml = '';

        for (; i < cols.length; i++) {

            col = cols[i];

            colHtml += Mini2.join([
                '<colgroup>',
                    '<col style="width:', col["width"], 'px;"  data-muid="', col.muid, '" />',
                '</colgroup>'
            ]);

        }

        return colHtml;
    },

    renderCell: function (renderEl, column, record, recordIndex, columnIndex, out, rowEl) {
        "use strict";
        var me = this,
            selModel = me.selModel,
            cellValues = me.cellValues,
            value, cellEl, innerEl,
            fieldValue, cellInnerEl,
            dataIndex = column.dataIndex,
            colRenderer = column.renderer,
            colInnerCls = column.innerCls;
        
        fieldValue = record.get(dataIndex);

        cellEl = column.renderCell();

        $(renderEl).append(cellEl);

        innerEl = cellEl.children(":first");

        if (me.isResetCellWidth) {
            innerEl.css('width', (column.width - 2) + 'px');
        }

        if (colRenderer && colRenderer.call) {
            
            try{
                value = colRenderer.call(column,
                    fieldValue,
                    cellValues,
                    cellValues,
                    record,
                    recordIndex,
                    columnIndex,
                    me.store,
                    me,
                    rowEl,
                    cellEl);
            }
            catch (ex) {
                console.error("渲染对象 colRenderer ", colRenderer);
                throw new Error("渲染单元格的值错误, 字段名=" + dataIndex);
            }
        }
        else {
            value = fieldValue;
        }


        cellValues.value = Mini2.isBlank(value)? '&#160;' : value;


        //innerEl.html(cellValues.value + '===========');

        cellEl.addClass(column.tdCls);

        if (Mini2.isFunction(colInnerCls)) {
            $(innerEl).addClass(colInnerCls.call(column));
        }
        else {
            $(innerEl).addClass(colInnerCls);
        }


        //如果数据发生修改，就标记小红点
        if (record.isModified(dataIndex)) {
            cellEl.addClass('mi-grid-dirty-cell');
        }


        if (column.groupRoot) {
            var gIndex = column.groupRoot.groupIndex;
            if (undefined != gIndex && gIndex % 2 == 0) {
                cellEl.addClass('mi-grid-td-group-back');
            }
        }


        if (column.isFirst && columnIndex > 0) {

            var prvCol = me.m_Cols[columnIndex - 1];

            if (prvCol && !prvCol.isLast) {
                cellEl.prev().addClass('mi-grid-td-group-last');
            }
            //cellEl.addClass('mi-grid-td-group-first');
        }

        if (column.isLast) {
            cellEl.addClass('mi-grid-td-group-last');
        }


        cellInnerEl = cellEl.children(".mi-grid-cell-inner");

        if (record.isDisabledForField(dataIndex)) {
            cellInnerEl.html('<hr style="border:none;border-top:1px dashed #b0b0b0;width:90%" />');
        }
        else {
            cellInnerEl.html(value);
        }

        if (column.bindEvent) {
            column.bindEvent.call(column, me, cellEl, recordIndex, columnIndex, null, record, rowEl);
        }

        //创建错误标记
        me.preColumnInvalid(column,cellEl, record);

        return cellEl;
    },

    //创建错误标记
    preColumnInvalid: function (column, cellEl, record) {
        "use strict";
        var me = this,
            log = me.log,
            dataIndex = column.dataIndex;


        try {
            //创建错误标记
            if (record && dataIndex && dataIndex != '') {

                if (record.errors) {
                    column.markInvalid.call(column, record, cellEl, dataIndex);
                }
                else {
                    column.clearInvalid.call(column, record, cellEl, dataIndex);
                }
            }
        }
        catch (ex) {
            log.error("创建错误标记失败. column.markInvalid（....) ");
        }

    },


    renderRow: function (renderEl, record, rowIdx, out) {
        "use strict";
        var me = this,
            log = me.log,
            i = 0,
            rowEl,
            col,
            cellEl,
            cols = me.m_Cols;

        rowEl = $('<tr role="row" class="mi-grid-row  mi-grid-data-row" tabindex="-1" ></tr>');

        $(renderEl).append(rowEl);

        rowEl.data('record', record);
        
        for (; i < cols.length; i++) {
            col = cols[i];

            try {
                cellEl = me.renderCell(rowEl, col, record, rowIdx,i, out, rowEl);
            }
            catch (ex) {
                throw new Error('初始化单元格错误.' + 'miType=' + col.miType + '\r\n' + ex.message);
            }

        }


        if (me.rowStyleEnabled) {
            try {
                me.store_update_setInvalid(rowEl, record);
            }
            catch (ex) {
                log.debug("设置异常提示信息失败.");
            }
        }

        return rowEl;
    },


    renderRows: function (rows, viewStartIndex, out) {

    },




    render_GridBody: function () {
        "use strict";
        var me = this,

            colGroups,
            gridViewEl,
            tableEl,
            tableBodyEl;

        colGroups = me.createColGroup();

        tableBodyEl = Mini2.$join([
            '<div class="mi-panel-body ',
                'mi-grid-body ',
                'mi-panel-body-default ',
                'mi-layout-fit ',
                'mi-panel-body-default ',
                'mi-noborder-rbl" ',
                'style="width:', me.width, '; height: 284px; left: 0px; top: 0px; ">',

                '<div class="mi-grid-view mi-fit-item mi-grid-view-default" ',
                    'style="overflow: auto; margin: 0px; width: ', me.width, '; height: 283px; " tabindex="-1" >',

                '</div>',
            '</div>']);

        tableEl = Mini2.$join([
            '<table class="mi-grid-table" border="0" cellspacing="0" cellpadding="0" >',
                colGroups,
                '<tbody>',
                '</tbldy>',
            '</table>']);

        me.bodyEl = tableBodyEl;
        me.gridViewEl = gridViewEl = tableBodyEl.children(".mi-grid-view:first");


        if (me.rowLines) {
            me.el.addClass("mi-grid-with-row-lines")
        }

        if (me.columnLines) {
            me.el.addClass("mi-grid-with-col-lines");
        }

        if ('auto' != me.scroll) {


            if ('none' == me.scroll) {
                gridViewEl.css('overflow', 'none');
            }

            
        }

        $(me.el).append(me.bodyEl);
        $(gridViewEl).append(tableEl);


        me.m_ColGroups = tableEl.children("colgroup");

        me.m_Table = tableEl;
        me.tbodyEl = tableEl.children("tbody:first");


        $(me.tbodyEl).muBind('contextmenu', function (evt) {
            
            me.showCheckMenu(evt);

            return false;
        });

    },

    



    setSize: function (w, h) {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            bodyEl = me.bodyEl,
            gridViewEl = me.gridViewEl,
            pagerHeight = 0,
            pagerInnerHeight,
            summaryHeight = 0,
            summary = me.summary,
            headerHeight = me.headerContainer.height;

        if (undefined != w) {

            //log.debug('width', w);

            me.width = w;


            var borderLeftWidth = $(el).css('border-left-width');
            var borderRightWidth = $(el).css('border-right-width');
            
            borderLeftWidth = parseInt(borderLeftWidth);
            borderRightWidth = parseInt(borderRightWidth);

            w -= (borderLeftWidth + borderRightWidth);


            el.width(w);
            bodyEl.width(w);
            gridViewEl.width(w);

            //me.headerContainer.setWidth(w);

            if (summary) {
                summary.setWidth(w);
            }

            //log.debug('width=' +  el.width=' + $(el).width() );
        }


        if (undefined != h) {

            h = parseInt(h);

            if (me.minHeight && h < me.minHeight) {
                h = me.minHeight;
            }

            me.height = h;

            var borderTopWidth = $(el).css('border-top-width');
            var borderBottomWidth = $(el).css('border-bottom-width');

            borderTopWidth = parseInt(borderTopWidth);
            borderBottomWidth = parseInt(borderBottomWidth);

            h -= (borderTopWidth + borderBottomWidth);


            pagerHeight = me.getPagetHeight();

            summaryHeight = me.getSummaryHeight();

            pagerInnerHeight = h - headerHeight - 2 - pagerHeight - summaryHeight;

            el.css('height', h);


            bodyEl.css('height', pagerInnerHeight);
            gridViewEl.css('height', pagerInnerHeight);

        }


        return me;
    },

    render_Panel: function () {
        "use strict";
        var me = this,
            el;

        me.el = el = Mini2.$join([
            '<div class="mi-panel mi-panel-default-framed mi-grid" ',
                'style="width: 100%; height: 350px; right: auto; left: 0px; top: 0px;      overflow: hidden;" >',
            '</div>'
        ]);

        el.css({
            width: me.width,
            height: me.height
        });

        if (!me.visible) {  el.hide();  }

        if (me.minHeight) {
            el.css({'min-height': me.minHeight + 'px'});
        }
        

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }

        return el;
    },


    //页点击事件
    pager_click: function (page, store) {
        var me = this;

        store.loadPage(page);

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

    //创建概要版面
    render_Summary: function () {
        "use strict";
        var me = this,
            summary;

        if (me.summaryVisible) {
            summary = Mini2.create('Mini2.ui.grid.feature.Summary', {
                renderTo: me.el,
                grid: me,
                store: me.getStore(),
                columns: me.m_Cols
            });

            summary.render();

            me.summary = summary;
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


    //延迟显示
    //private isDelayRender:false,

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

        return me;
    },

    //设置表格焦点
    setFocus: function () {
        "use strict";
        var me = this,
             tbodyEl = me.tbodyEl;

        var rowEl = $(tbodyEl).children('.mi-grid-row:first');

        //console.log("row size = " + rowEl.length);
       
        var cellEl = $(rowEl).children(':eq(2)');

        //console.log("cellEl size = " + cellEl.length);


        me.cell_focus(cellEl);
    },


    




    render: function (obj) {
        "use strict";
        var me = this,
            el = null,
            headerContainer,
            width = me.maxWidth || me.width,
            height = me.maxHeight || me.height,
            store = me.store,
            tableCfg = Mini2.SystemInfo.table;

        me.onInit();

        el = me.render_Panel();      //创建主框 div

        me.headerContainer = headerContainer = Mini2.create('Mini2.ui.grid.header.Container', {
            renderTo: el,
            columns: me.columns,
            width: width,
            height: tableCfg.headerHeight,
            rowHeight: tableCfg.rowHeight,
            grid: me
        });



        headerContainer.render();

        headerContainer.columnWidthResize(function (curCol) {
            me.resetBodyWidth(curCol);

            var fr = me.focusedRegion;

            if (fr) {
                me.setFocusRegion(fr.itemA, fr.itemB, true);
            }
            else {
                me.setFocusRegion();
            }
        });

        headerContainer.columnMoved = function (e) {
                        
            //console.log("moved from=%d, to=%d", e.fromIndex, e.toIndex);

            var cols = me.srcColumns;

            Mini2.Array.move(cols, e.fromIndex, e.toIndex);

            me.clearColumnAll();

            me.addColumn(cols);
        };

        me.render_GridBody();   //创建表内容

        me.render_Summary();    //创建底部概要

        me.render_Pager(); //创建分页栏


        me.setSize(width, height);

        me.syncScrollLeft();    //表格头和数据区区域同步滚动

        me.resetBodyWidth();

        if (store && store.isStore) {
            me.bindStore(store);
        }
        else {
            store = me.getStore();
        }

        store.load();


        var parentEl = $(el).parent();
        var parentIsTarget = $(parentEl).hasClass('mi-box-target');

        if (!parentIsTarget) {
            
            $(window).resize(function () {
                var ww = $(parentEl).width();

                //var hh = $(parentEl).height();

                me.setSize(ww,null);
            });
        }

        el.data('me', me);

        try {
            var sec = Mini2.ui.SecManager;
            sec.reg(me.secFunCode, me, 'visible');
            sec.reg(me.secReadonly, me, 'readonly');
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }

        return me;
    },


    keydownEvent: [],

    setKeydownEvent:function(fun){
        var me = this,
            keydownEvent = me.keydownEvent;

        keydownEvent.push(fun);
    },


    onKeydownEvent: function (e) {
        "use strict";
        var me = this,
            fun,
            keydownEvent = me.keydownEvent || [];

        for (var i = 0; i < keydownEvent.length; i++) {
            fun = keydownEvent[i];
            fun.call(this,e);

        }
    },


    //
    // 处理键盘事件
    // 
    proKeyEvent: function (e) {
        "use strict";
        var me = this,
            log = Mini2.Logger,
            keyCode = e.keyCode,
            rowEl = me.curRowFocused,
            cellEl = me.m_FocusCell,
            editor = me.editor;

        //log.debug("------表格事件 keyCode = " + keyCode + ", me.editor.visible = " + (editor ? editor.visible : 'null') + ", shift=" + e.shiftKey);


        me.onKeydownEvent(e);

        if (e.stoped) {
            return;
        }

        // keyCode: 37-左, 38-上, 39-右, 40-下 , 13-回车

        if (editor && editor.isVisible()) {

            editor.proKeyEvent2(e);
            
            if (e.stoped) {
                return;
            }

            if (Mini2.onwerPage.typeahead.length) {
                return;
            }


            if (9 == keyCode) {    //Tab 键
                editor.hide();
            }
            else if (38 == keyCode || 40 == keyCode) {
                var nextRowEl,
                    nextCellEl;


                if (40 == keyCode) {
                    nextRowEl = $(rowEl).next();
                }
                else if (38 == keyCode) {

                    nextRowEl = $(rowEl).prev();
                }


                if ($(nextRowEl).length > 0) {
                    editor.hide();
                }
                
            }
            else if (37 == keyCode && editor.isFirstCursorPos()) {  //左
                editor.hide();
            }
            else if (39 == keyCode && editor.isLastCursorPos()) {   //右
                editor.hide();
            }
            else {
                editor.onKeydown(e);

                return;
            }
        }


        if (13 == keyCode) {


            var tdIndex = $(cellEl).index();

            var record = $(rowEl).data('record');

            var columnHeader = me.m_Cols[tdIndex];
            var miType = columnHeader.miType;



            if ('rowcheckcolumn' == miType) {
                me.cell_click_rowCheckColumn(cellEl, cellEl, rowEl, record);
                return;
            }
            else if ('actioncolumn' == miType) {
                return;
            }

            //if (me.autoRowCheck && 'SINGLE' == me.checkedMode) {
            //    me.setRowCheck(rowEl, true);
            //}
                        
            if (me.autoRowCheck) {
                me.setRowCheck(rowEl, true);
            }



            //只读状态，不执行后面代码
            if (me.readOnly || (me.store && me.store.readOnly)) {
                return;
            }

            if (record.isLocked && record.isLocked()) {
                return;
            }

            if ('checkcolumn' == miType) {
                me.cell_click_checkColumn(cellEl, cellEl, columnHeader, record);
                return;
            }


            //如果有点击事件，就执行
            if (Mini2.isFunction(columnHeader.click)) {

                var fieldValue = record.get(columnHeader.dataIndex);
                var rowIndex = $(rowEl).index();

                columnHeader.click.call(columnHeader, fieldValue, record, columnHeader,
                    cellEl, rowEl, rowIndex, tdIndex, me.store, me);
            }

            console.debug('column header = ', columnHeader);

            if (columnHeader.readOnly) {

                return;
            }

            //如果没有编辑器模式，就退出
            if (true !== columnHeader.readOnly && columnHeader.editor && 'none' != columnHeader.editorMode) {

                var xtype = columnHeader.editor.xtype;


                if (columnHeader.isPropertyColumn) {
                    //属性的，特殊处理

                    me.cell_click_propertyColumn(cellEl, cellEl, columnHeader, record);


                }

                if (Mini2.ui.grid.CellEditor) {

                    //log.debug('键盘触发，显示编辑框。');

                    var editor = me.getEditor(columnHeader, record);
                    me.showEditor(cellEl, cellEl, rowEl, columnHeader, record, editor);

                    Mini2.EventManager.stopEvent(e);
                }


            }



        }


        if (40 == keyCode || 38 == keyCode) {


            //log.debug("rowEl = " + rowEl);

            var nextRowEl,
                nextCellEl;

            if (40 == keyCode) {
                nextRowEl = $(rowEl).next();
            }
            else if (38 == keyCode) {

                nextRowEl = $(rowEl).prev();
            }


            if ($(nextRowEl).length) {

                $(cellEl).removeClass("mi-grid-cell-selected");

                var index = $(cellEl).index();

                nextCellEl = $(nextRowEl).children(':eq(' + index + ')');

                //log.debug("nextRowEl.len = " + $(nextRowEl).length);
                //log.debug("nextCellEl.len = " + $(nextCellEl).length);

                record = $(nextRowEl).data('record');

                me.store.setCurrent(record);

                me.cell_focus(nextCellEl);


                Mini2.EventManager.stopEvent(e);
            }


            if ($(nextRowEl).length && nextCellEl) {

                var rowEl = nextRowEl,
                    cellEl = nextCellEl;

                var tdIndex = $(cellEl).index();

                var record = $(rowEl).data('record');

                var columnHeader = me.m_Cols[tdIndex];
                var miType = columnHeader.miType;


                if ('rowcheckcolumn' == miType) {
                    me.cell_focus(nextCellEl);
                    Mini2.EventManager.stopEvent(e);
                    return;
                }
                else if ('actioncolumn' == miType) {
                    me.cell_focus(nextCellEl);
                    Mini2.EventManager.stopEvent(e);
                    return;
                }

                //if (me.autoRowCheck && 'SINGLE' == me.checkedMode) {
                //    me.cell_focus(nextCellEl);
                //    Mini2.EventManager.stopEvent(e);
                //}

                // 改为支持多选
                if (me.autoRowCheck ) {
                    me.cell_focus(nextCellEl);
                    Mini2.EventManager.stopEvent(e);
                }

                //只读状态，不执行后面代码
                if (me.readOnly || (me.store && me.store.readOnly)) {
                    me.cell_focus(nextCellEl);
                    Mini2.EventManager.stopEvent(e);
                    return;
                }

                if (record.isLocked && record.isLocked()) {
                    me.cell_focus(nextCellEl);
                    Mini2.EventManager.stopEvent(e);
                    return;
                }

                if ('checkcolumn' == miType) {
                    me.cell_focus(nextCellEl);
                    Mini2.EventManager.stopEvent(e);
                    return;
                }


                    //如果没有编辑器模式，就退出
                if (true !== columnHeader.readOnly && columnHeader.editor && 'none' != columnHeader.editorMode) {

                    var xtype = columnHeader.editor.xtype;

                   

                    if ('text' != xtype && 'numberfield' != xtype) {

                    }
                    else {
                        if (columnHeader.isPropertyColumn) {
                            //属性的，特殊处理

                            me.cell_click_propertyColumn(cellEl, cellEl, columnHeader, record);

                            return;
                        }

                        if (Mini2.ui.grid.CellEditor) {

                            var editor = me.getEditor(columnHeader, record);
                            me.showEditor(cellEl, cellEl, rowEl, columnHeader, record, editor);

                            Mini2.EventManager.stopEvent(e);

                            return;
                        }
                    }

                }
                
                me.cell_focus(nextCellEl);
                Mini2.EventManager.stopEvent(e);
                
            }
        }
        else if (37 == keyCode || 39 == keyCode || 9 == keyCode) {

            var nextCellEl;

            if (37 == keyCode || (9==keyCode && e.shiftKey) ) {
                nextCellEl = $(cellEl).prev();
            }
            else if (39 == keyCode || 9 == keyCode) {
                nextCellEl = $(cellEl).next();
            }


            if ($(nextCellEl).length) {

                $(cellEl).removeClass("mi-grid-cell-selected");

                me.cell_focus(nextCellEl);

                Mini2.EventManager.stopEvent(e);
            }
            else if (39 == keyCode || 9 == keyCode ) { //已经到最后的单元格...自动跳到下一行

                nextRowEl = $(rowEl).next();

                if ($(nextRowEl).length) {

                    $(cellEl).removeClass("mi-grid-cell-selected");

                    var nextCellEl = $(nextRowEl).children(':eq(0)');


                    record = $(nextRowEl).data('record');

                    me.store.setCurrent(record);

                    me.cell_focus(nextCellEl);


                    Mini2.EventManager.stopEvent(e);
                }
            }

        }
        else if ((keyCode >= 65 && keyCode <= 90) || ( keyCode >= 96 && keyCode <= 105 ) ) {

            //var tdIndex = $(cellEl).index();

            //var record = $(rowEl).data('record');

            //var columnHeader = me.m_Cols[tdIndex];
            //var miType = columnHeader.miType;



            ////如果没有编辑器模式，就退出
            //if (columnHeader.editor && 'none' != columnHeader.editorMode) {

            //    var xtype = columnHeader.editor.xtype;

            //    if (columnHeader.isPropertyColumn) {
            //        //属性的，特殊处理

            //        me.cell_click_propertyColumn(cellEl, cellEl, columnHeader, record);

            //    }

            //    if (Mini2.ui.grid.CellEditor) {

            //        var editor = me.getEditor(columnHeader, record);
            //        me.showEditor(cellEl, cellEl, rowEl, columnHeader, record, editor);

            //        Mini2.EventManager.stopEvent(e);

            //        var keycode = e.keyCode;
            //        var realkey = String.fromCharCode(e.keyCode);
                                        
            //        editor.setValue(realkey);
                                       
            //    }


            //}
        }


    },




    setTop: function (t) {
        var me = this,
            el = me.el;

        el.css('top', t);
        return me;
    },

    setLeft: function (l) {
        var me = this,
            el = me.el;

        el.css('left', l);
        return me;
    },

    getTop: function () {
        var me = this,
            el = me.el;

        return el.css('top');
    },

    getLeft: function () {
        var me = this,
            el = me.el;

        return el.css('left');
    },

    setWidth: function (w) {
        var me = this;
        me.setSize(w, undefined);
        return me;
    },

    setHeight: function (h) {
        var me = this;

        me.setSize(undefined, h);
        return me;
    },

    getWidth: function () {
        var me = this,
            el = me.el;

        return el.width();
    },

    getHeight: function () {
        var me = this,
            el = me.el;

        return el.height();
    },

    getSize: function () {
        var me = this,
            el = me.el,
            w = el.width(),
            h = el.height();

        return { width: w, height: h };
    },


    isVisible: function () {
        var me = this,
            el = me.el;

        return el && el.is(':visible');
    },


    syncScrollLeft: function () {
        "use strict";
        /// <summary>同步滚动</summary>

        var me = this,
            gridViewEl = me.gridViewEl;

        $(gridViewEl).mousedown(function (e) {
            //            log.begin('Table.gridViewEl.mouosedown(...)');

            me.focusMgr.setControl(me);

            Mini2.EventManager.clear();

            //log.end();

        });

        gridViewEl.scroll(function (e) {

            var sLeft = $(this).scrollLeft();

            me.headerContainer.scrollLeft(sLeft);

            //me.headerContainer.css('left', -sLeft);

            if (me.summary) {
                me.summary.scrollLeft(sLeft);
            }



        });




        return me;
    },


    //设置每个 td -> div 里面的宽度
    resizeCellWidth: function (curCol) {
        "use strict";
        var me = this,
            i, j,
            cols = me.m_Cols,
            col,
            tdEls,
            tbodyEl = me.tbodyEl,
            rowEls = tbodyEl.children('tr.mi-grid-row'),
            rowEl,
            tdEl, divEl,
            curColIndex = -1;
        
        if (curCol) {
            curColIndex = curCol.index;
        }

        for (i = 0; i < rowEls.length; i++) {

            rowEl = rowEls[i];


            if (curColIndex > -1) {
                col = cols[curColIndex];

                tdEl = $(rowEl).children('td.mi-grid-cell:eq(' + curColIndex + ')');

                divEl = $(tdEl).children('.mi-grid-cell-inner:first');
                $(divEl).css('width', (col.width - 2) + 'px');

                //if (col.visible) {
                //    $(tdEl).show();
                //}
                //else {
                //    $(tdEl).hide();
                //}

            }
            else {
                tdEls = $(rowEl).children('td.mi-grid-cell');

                for (j = 0; j < cols.length; j++) {

                    col = cols[j];
                    tdEl = tdEls[j];

                    divEl = $(tdEl).children('.mi-grid-cell-inner:first');
                    $(divEl).css('width', (col.width - 2) + 'px');


                    //if (col.visible) {
                    //    $(tdEl).show();
                    //}
                    //else {
                    //    $(tdEl).hide();
                    //}
                    
                }
            }
        }

        if (me.summary) {
            me.summary.resizeCellWidth.call(me.summary);
        }

        return me;
    },


    resetBodyWidth: function (curCol) {
        "use strict";
        /// <summary>重新设置单元格宽度</summary>
        var me = this,
            i,
            cols = me.m_Cols,
            col,
            colEl,
            widthAll = 0,
            table, colGroups;

        table = $(me.bodyEl).find(".mi-grid-table:first");

        colGroups = $(table).children("colgroup");

        for (i = 0; i < cols.length; i++) {

            colEl = $(colGroups[i]).children("col");
            col = cols[i];

            if (col.visible) {
                widthAll += col.width;

                colEl.css('width', col.width);

                colEl.show();
            }
            else {
                colEl.hide();
            }


        }


        me.m_Table.css('width', widthAll);

        me.headerContainer.setWidth(widthAll);

        if (me.editor) {

            me.editor.hide();

        }

        //以后可以取消
        if (me.isResetCellWidth) {
            me.resizeCellWidth(curCol);
        }

        return me;
    },


    itemRemoteAll: function () {
        "use strict";
        var me = this,
            rows = $(me.tbodyEl).children();

        $(rows).remove();

        me.m_NewRowIndex = 0;

        me.checkedRecords = [];
        me.checkedRowEls = [];
    },


    itemInsert: function (index, data, outData) {
        "use strict";
        /// <summary>插入记录</summary>
        /// <param name="index" type="int">索引</param>

        var me = this,
            tbodyEl = me.tbodyEl,
            i,
            count,
            record,
            records,
            rowEl,
            rowEls = [],
            rows,
            rowPrv;


        if (outData) {
            outData.rowEls = rowEls;
        }

        rows = $(tbodyEl).children();


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


        for (i = 0; i < records.length; i++) {
            record = records[i];
            rowEl = me.renderRow(me.tbodyEl, record, me.m_NewRowIndex);
            rowEls.push(rowEl);
        }



        //$(me.tbodyEl).append(rowEls);

        if (count) {
            if (index == 0) {
                rowPrv = $(rows).eq(0);
                $(rowPrv).before(rowEls);
            }
            else {
                rowPrv = $(rows).eq(index - 1);
                $(rowPrv).after(rowEls);
            }
        }

        me.itemsReset();

        for (i = 0; i < rowEls.length; i++) {
            rowEl = rowEls[i];
            record = records[i];

            me.bindEvent_ForRow(record, rowEl);

            me.bindEvent_ForCell(record, rowEl);
        }


        return me;
    },



    getCellPosition: function (cell) {
        "use strict";
        /// <summary>获取单元格的坐标</summary>
        var me = this,
            officeX = 0,
            officeY = 0,
            left, top,
            row,
            tdIndex,
            pCells,
            pRows;

        tdIndex = cell.index();

        row = cell.parent();

        pCells = $(cell).prevUntil();

        pCells.each(function () {
            if ($(this).is(':visible')) {
                officeX += $(this).outerWidth();
            }
        });

        pRows = row.prevUntil();
        pRows.each(function () {
            if ($(this).is(':visible')) {
                officeY += $(this).outerHeight();
            }
        });


        officeY += me.headerContainer.height;


        left = officeX - me.gridViewEl.scrollLeft();
        top = officeY - me.gridViewEl.scrollTop();


        return { left: left, top: top };
    },

    getCellSize: function (cell) {
        "use strict";
        var w, h;

        w = cell.outerWidth() + 1;
        h = cell.outerHeight();

        return { width: w, height: h };
    },

    setHeaderCheck: function (isChecked) {
        "use strict";
        var me = this;

        me.headerContainer.setHeaderCheck(isChecked);

        return me;
    },


    //设置选中的记录
    setRecordCheck: function (recordId, isCheck) {
        "use strict";
        var me = this,
            rowEl,
            rows,
            record;
        
        rows = $(me.tbodyEl).children();
        
        $(rows).each(function () {

            rowEl = $(this);

            record = rowEl.data('record');

            if (record.getId() == recordId) {
                me.setRowCheck(rowEl, isCheck);

                return false;
            }
        });

        return me;
    },

    //单选
    setRowCheck_ForSingle: function (rowEl, record, col, selectedCls, checkColumnCls, isForceSingle) {
        "use strict";
        var me = this,
            bodyEl,
            rowEls,
            checkedRecord,
            cellEl,
            imgEl;

        if (isForceSingle || 'SINGLE' == me.checkedMode) {

            bodyEl = $(rowEl).parent();

            rowEls = $(bodyEl).children('.' + selectedCls);


            $(rowEls).each(function () {

                rowEl = $(this);

                checkedRecord = rowEl.data('record');

                if (checkedRecord === record) {
                    return;
                }

                cellEl = rowEl.children(":eq(" + col.index + ")");
                imgEl = cellEl.find('img:first');

                rowEl.removeClass(selectedCls);

                $(imgEl).removeClass(checkColumnCls);
            });

            me.checkedRecords.length = 0;
            me.checkedRowEls.length = 0;
        }

        return me;
    },

    /**
     * 设置行选中状态
     * @param isForceSingle = 强制单选..默认=false
     *
     */
    setRowCheck: function (rowEl, isCheck, isForceSingle) {
        "use strict";
        var me = this,
            Array = Mini2.Array,
            col = me.rowCheckColumn,
            cell, imgEl,
            $rowEl = $(rowEl),
            record = $rowEl.data('record'),
            rowCls = 'mi-grid-row',
            selectedCls = rowCls + "-selected",
            checkColumnCls = "mi-grid-checkcolumn-checked";

        if (col != null) {
            
            cell = $rowEl.children(":eq(" + col.index + ")");

            imgEl = cell.find('img:first');

            //单选
            me.setRowCheck_ForSingle(rowEl, record, col, selectedCls, checkColumnCls, isForceSingle);


            $rowEl.toggleClass(selectedCls, isCheck);
            $(imgEl).toggleClass(checkColumnCls, isCheck);

            
            if (isCheck) {
                Array.include(me.checkedRowEls, $rowEl[0]);
                Array.include(me.checkedRecords, record);
            }
            else {
                Array.remove(me.checkedRowEls, $rowEl[0]);
                Array.remove(me.checkedRecords, record);
            }

            col.onSelectChange();

            me.on('rowsChecked', {
                rowEls: me.checkedRowEls,
                records: me.checkedRecords
            });
        }

        return me;
    },



    //全选,全不选
    setRowCheckAll: function (isCheck) {
        "use strict";
        var me = this,
            rowEl,
            rowEls,
            imgEl,
            cell,
            record,
            col = me.rowCheckColumn,

            rowCls = 'mi-grid',
            selectedCls = rowCls + '-row-selected',
            checkedCls = rowCls + '-checkcolumn-checked';

        //console.log('isCheck = ', isCheck);

        // mi-grid-checkcolumn-undefined

        me.setHeaderCheck(isCheck);

        rowEls = $(me.tbodyEl).children();

        me.checkedRecords.length = 0;

        me.checkedRowEls.length = 0;

        $(rowEls).each(function () {
            rowEl = $(this);
            cell = rowEl.children('td:eq(' + col.index + ')');
            record = rowEl.data('record');

            imgEl = cell.find('img.mi-grid-checkcolumn');

            //imgEl.add('mi-grid-checkcolumn-undefined');
            //imgEl.addClass('mi-xxxxxxxxxxxx');

            rowEl.toggleClass(selectedCls, isCheck);
            imgEl.toggleClass(checkedCls, isCheck);

            if (isCheck) {
                me.checkedRecords.push(record);
                me.checkedRowEls.push(this);
            }

        });

        //console.log('长度 = ' ,me.checkedRowEls.length)

        me.on('rowsChecked', {
            rowEls : me.checkedRowEls,
            records: me.checkedRecords
        });

        return me;
    },



    cell_click_rowCheckColumn: function (cell, cellEl, rowEl, record) {
        "use strict";
        var me = this,
            imgEl,
            isChecked;


        me.focusMgr.setControl(cellEl, me, null);

        imgEl = cell.find('img:first');
        isChecked = imgEl.hasClass('mi-grid-checkcolumn-checked');

        me.setRowCheck(rowEl, !isChecked);

        return me;
    },


    cell_click_checkColumn: function (cell, cellEl, columnHeader, record) {
        "use strict";
        var me = this,
            imgEl,
            isChecked,
            checkedCls = 'mi-grid-checkcolumn-checked';

        if (true === columnHeader.readOnly || 'none' == columnHeader.editorMode) {
            return;
        }

        //只读状态下，复选框无效。
        if (!me.readOnly ) {


            me.focusMgr.setControl(cellEl, me, null);

            imgEl = cell.find('img:first');
            imgEl.toggleClass(checkedCls);

            isChecked = $(imgEl).hasClass(checkedCls);
            record.set(columnHeader.dataIndex, isChecked);

        }

        return me;
    },


    //配合"属性控件"的函数
    cell_click_propertyColumn: function (cell, cellEl, columnHeader, record) {
        "use strict";
        var me = this,
            log = me.log,
            type = record.get('Type'), 
            pName = record.get('Name'),
            editXType,
            editorId,
            propEditor;


        switch (type) {
            case 'int': editXType = 'numberfield'; break;
            case 'date': editXType = 'datefield'; break;
            case 'bool': editXType = 'combobox'; break;
            case 'enum': editXType = 'combobox'; break;
            
            default: editXType = 'textfield'; break;
        }


        columnHeader.xtype = editXType;
        
        editorId = ('enum' == type) ? (pName + '_' + editXType) : editXType;

        propEditor = columnHeader.propEditor[editorId];

        if (!propEditor) {
            propEditor = {
                xtype: editXType
            };
            columnHeader.propEditor[editorId] = propEditor;
        }

        if (!propEditor.isInit) {

            switch (type) {
                case 'bool':
                    Mini2.apply(propEditor, {
                        fields: ['value', 'text'],
                        store: [{
                            value: 'true', text: 'true'
                        }, {
                            value: 'false', text: 'false'
                        }]
                    });

                    break;

                case 'enum':

                    var enumStore = [],
                        enumObj = record.get('Extend');
                    

                    if (Mini2.isArray(enumObj)) {
                        for (var i = 0; i < enumObj.length; i++) {
                            var enumValue = enumObj[i];

                            if (Mini2.isString(enumValue)) {
                                enumStore.push({
                                    value: enumValue,
                                    text: enumValue
                                });
                            }
                        }

                    }
                    else {
                        for (var enumName in enumObj) {
                            var enumValue = enumObj[enumName];
                            enumStore.push({
                                value: enumName,
                                text: enumValue
                            });
                        }
                    }

                    Mini2.apply(propEditor, {
                        fields: ['value', 'text'],
                        store: enumStore
                    });

                    break;
            }



        }


        return me;
    },

    //单元格获取焦点
    cell_focus: function( cellEl){
        "use strict";

        var me = this,
            cell,
            rowEl,
            tdIndex,
            record,
            columnHeader,
            miType;

        if (!cellEl) {
            return;
        }

        cell = $(cellEl);

        if (0 == cell.length) {
            return;
        }





        me.curRowFocused = rowEl = cell.parent();
        tdIndex = cell.index();

        record = $(rowEl).data('record');
        columnHeader = me.m_Cols[tdIndex];
        miType = columnHeader.miType;

        me.m_LastCell = me.m_FocusCell;

        if (me.m_LastCell != null) {
            var lastInnerEl = $(me.m_LastCell).children(".mi-grid-cell-inner");
            lastInnerEl.show();
        }

        me.m_FocusCell = cell;


        if ('rowcheckcolumn' == miType) {
            //me.cell_click_rowCheckColumn(cell, cellEl, rowEl, record);

            me.focusMgr.setControl(cellEl, me, null);

            me.autoFocusScroll_2(cellEl);

            return;
        }
        else if ('actioncolumn' == miType) {
            me.focusMgr.setControl(cellEl, me, null);

            me.autoFocusScroll_2(cellEl);

            return;
        }

        //if (me.autoRowCheck && 'SINGLE' == me.checkedMode) {
        //    //me.setRowCheck(rowEl, true);
        //}

        //只读状态，不执行后面代码
        //if (me.readOnly) {
        //    return;
        //}

        //if (record.isLocked && record.isLocked()) {
        //    return;
        //}

        if ('checkcolumn' == miType) {
            //me.cell_click_checkColumn(cell, cellEl, columnHeader, record);

            me.focusMgr.setControl(cellEl, me, null);

            me.autoFocusScroll_2(cellEl);
            return;
        }


        ////如果有点击事件，就执行
        //if (Mini2.isFunction(columnHeader.click)) {

        //    var fieldValue = record.get(columnHeader.dataIndex);
        //    var rowIndex = $(rowEl).index();

        //    //columnHeader.click.call(columnHeader, fieldValue, record, columnHeader,
        //    //    cellEl, rowEl, rowIndex, tdIndex, me.store, me);

        //    me.focusMgr.setControl(cellEl, me, null);
        //}


        //如果没有编辑器模式，就退出
        if (true  === columnHeader.readOnly || !columnHeader.editor || 'none' == columnHeader.editorMode) {
            //log.debug("没有编辑模式.退出");

            me.focusMgr.setControl(cellEl, me, null);

            me.autoFocusScroll_2(cellEl);
            return;
        }


        me.focusMgr.setControl(cellEl, me, null);

        var xtype = columnHeader.editor.xtype;

        if (columnHeader.isPropertyColumn) {
            //属性的，特殊处理

            //me.cell_click_propertyColumn(cell, cellEl, columnHeader, record);

            me.focusMgr.setControl(cellEl, me, null);
            //log.debug("columnHeader.propEditor[" + editXType + "] = " + columnHeader.propEditor[editXType]);

            me.autoFocusScroll_2(cellEl);
        }

        if (Mini2.ui.grid.CellEditor) {

            //var editor = me.getEditor(columnHeader, record);
            //me.showEditor(cell, cellEl, rowEl, columnHeader, record, editor);

            me.focusMgr.setControl(cellEl, me, null);

            me.autoFocusScroll_2(cellEl);
        }
        else {
            //log.debug("异步加载: Mini2.ui.grid.CellEditor ");

            //In('Mini2.ui.grid.CellEditor', function () {
            //    var editor = me.getEditor(columnHeader, record);
            //    me.showEditor(cell, cellEl, rowEl, columnHeader, record, editor);
            //});
        }


        //log.end();

    },

    //单元格双击事件
    cell_dblclick:function(cellEl){

        "use strict";

        var me = this,
            cell,
            rowEl,
            tdIndex,
            record,
            columnHeader,
            miType;

        if (!cellEl) {
            return;
        }

        cell = $(cellEl);

        if (0 == cell.length) {
            return;
        }

        me.curRowFocused = rowEl = cell.parent();
        tdIndex = cell.index();

        record = $(rowEl).data('record');
        columnHeader = me.m_Cols[tdIndex];
        miType = columnHeader.miType;

        me.m_LastCell = me.m_FocusCell;

        if (me.m_LastCell != null) {
            var lastInnerEl = $(me.m_LastCell).children(".mi-grid-cell-inner");
            lastInnerEl.show();
        }

        me.m_FocusCell = cell;


        if ('rowcheckcolumn' == miType) {
            me.cell_click_rowCheckColumn(cell, cellEl, rowEl, record);
            return;
        }
        else if ('actioncolumn' == miType) {
            return;
        }

        //log.debug("zzzzzzzzzzzzzzzz双击事件.");

        
        me.setRowCheck(rowEl, true, true);  //强制单选

        
        me.onCellDbclick();   //触发单元格双击事件

    },

    cell_Click: function (cellEl) {
        "use strict";
        /// <summary>显示单元格</summary>
        /// <param name="cellEl" type="HtmlElement">单元格对象 td</param>

        var me = this,
            cell,
            rowEl,
            tdIndex,
            record,
            columnHeader,
            miType;

        if (!cellEl) {
            return;
        }

        cell = $(cellEl);

        if (0 == cell.length) {
            return;
        }

        me.curRowFocused = rowEl = cell.parent();
        tdIndex = cell.index();

        record = $(rowEl).data('record');
        columnHeader = me.m_Cols[tdIndex];
        miType = columnHeader.miType;

        me.m_LastCell = me.m_FocusCell;

        if (me.m_LastCell != null) {
            var lastInnerEl = $(me.m_LastCell).children(".mi-grid-cell-inner");
            lastInnerEl.show();
        }

        me.m_FocusCell = cell;


        if ('rowcheckcolumn' == miType) {
            me.cell_click_rowCheckColumn(cell, cellEl, rowEl, record);
            return;
        }
        else if ('actioncolumn' == miType) {
            return;
        }

        if (me.autoRowCheck) {
            
            me.setRowCheck(rowEl, true, true);

            //if ('SINGLE' == me.checkedMode) {
            //    me.setRowCheck(rowEl, true);
            //}
            //else {  //鼠标双击事件
            //    //log.debug("准备触发鼠标双击事件.");
            //    //me.setRowCheck(rowEl, true);
            //}
        }



        //只读状态，不执行后面代码
        if (me.readOnly) {
            return;
        }

        var isLockedEx = false;

        //判断整行锁住
        if (record.isLocked && record.isLocked()) {

            if (record.isLockedExclusionField(columnHeader.dataIndex)) {
                isLockedEx = true;
            }
            else {
                return;
            }
        }

        if (!isLockedEx) {

            //数据字段锁住
            if (record.isLockedForField(columnHeader.dataIndex)) {
                return;
            }

            //数据字段锁住
            if (record.isDisabledForField(columnHeader.dataIndex)) {
                return;
            }
        }


        if ('checkcolumn' == miType) {
            me.cell_click_checkColumn(cell, cellEl, columnHeader, record);
            return;
        }


        //如果有点击事件，就执行
        if (Mini2.isFunction(columnHeader.click)) {

            var fieldValue = record.get(columnHeader.dataIndex);
            var rowIndex = $(rowEl).index();

            columnHeader.click.call(columnHeader, fieldValue, record, columnHeader,
                cellEl, rowEl, rowIndex, tdIndex, me.store, me);
        }


        //如果没有编辑器模式，就退出
        if (true === columnHeader.readOnly || !columnHeader.editor || 'none' == columnHeader.editorMode) {
            //log.debug("没有编辑模式.退出");

            me.focusMgr.setControl(cellEl, me, null);

            return;
        }


        var xtype = columnHeader.editor.xtype;

        if (columnHeader.isPropertyColumn) {
            //属性的，特殊处理

            me.cell_click_propertyColumn(cell, cellEl, columnHeader, record);

            //log.debug("columnHeader.propEditor[" + editXType + "] = " + columnHeader.propEditor[editXType]);

        }

        if (Mini2.ui.grid.CellEditor) {

            var editor = me.getEditor(columnHeader, record);
            me.showEditor(cell, cellEl, rowEl, columnHeader, record, editor);

        }
        else {
            //log.debug("异步加载: Mini2.ui.grid.CellEditor ");

            In('Mini2.ui.grid.CellEditor', function () {
                var editor = me.getEditor(columnHeader, record);
                me.showEditor(cell, cellEl, rowEl, columnHeader, record, editor);
            });
        }


        //log.end();
    },



    showEditor: function (cell, cellEl, rowEl, columnHeader, record, editor) {
        "use strict";
        var me = this,
            log = me.log,
            divEl = cell.children(".mi-grid-cell-inner"),
            sz,
            office //偏移量;

        if (!editor) { throw new Error("编辑控件不存在"); }

        //log.debug('显示编辑框');

        if (!editor.isSaveed) {
            editor.isSaveed = true;
        }

        if (columnHeader.innerAutoHide == false) {

        }
        else {
            divEl.hide();
        }

        sz = me.getCellSize(cell);
        office = me.getCellPosition(cell);

        office.left -= 1;
        office.top -= 1;

        sz.height += 2;
        
        
        editor.startEditByPosition(office)
            .setSize(sz)
            .focus()
            .setRowCell(rowEl, cellEl)
            .startEdit(record, columnHeader, {});

        me.editor = editor;

        //log.end();
    },

    /**
     * 隐藏编辑框
     */
    hideEditor:function(){
        var me = this,
            editor = me.editor;

        if (editor) {
            editor.hide();
        }

        return me;
    },

    getEditor: function (col, record) {
        "use strict";
        var me = this,
            xtype,
            config,
            editorObj,
            editor;


        //属性列
        if (col.isPropertyColumn) {

            var propEditor = col.propEditor,

                pName = record.get('Name'),
                pType = record.get('Type'),
                editorId = col.xtype;

            if ('enum' == pType) {
                editorId = pName + '_' + col.xtype;
            }

            editor = propEditor[editorId];

            //log.debug("editorId = " + editorId);

            //如果未初始化，就初始化一下
            if (!editor.isInit) {

                xtype = col.xtype || 'textcolumn';

                config = Mini2.applyIf({
                    grid: me,
                    renderTo: me.el,
                    isInit: true,
                    visible: false,
                    xtype: xtype,
                    placeholder: col.placeholder
                }, editor);


                editorObj = Mini2.create('Mini2.ui.grid.CellEditor', config);
                editorObj.config = editor;

                editorObj.render();

                propEditor[editorId] = editorObj;


            }

            return propEditor[editorId];
        }
        else {

            editor = col.editor;

            //如果未初始化，就初始化一下
            if (editor.isInit == false) {

                xtype = editor.xtype || 'textcolumn';

                //console.debug("col.autoTrim", col.autoTrim);

                config = Mini2.applyIf({
                    grid: me,
                    renderTo: me.el,
                    isInit: true,
                    visible: false,
                    placeholder: col.placeholder,
                    autoTrim: col.autoTrim,
                    column : col
                }, editor);

                //console.log("初始化配置: ", config);

                editorObj = Mini2.create('Mini2.ui.grid.CellEditor', config);
                editorObj.config = editor;

                editorObj.render();

                col.editor = editorObj;


            }

            return col.editor;
        }

    },


    clearSelectAndFocus: function () {
        "use strict";
        var me = this,
            rowCls = "mi-grid-row",
            selectedCls = rowCls + "-selected",
            rows = me.tbodyEl.children("." + selectedCls);

        $(rows).removeClass(selectedCls);
        return me;
    },

    autoFocusScroll_2:function(cellEl){
        "use strict";
        var me = this,
            bodyView = me.gridViewEl,
            cell = $(cellEl);

        if (cell.length) {

            var scrollX = 0,
                scrollY = 0,
                rowEl = cell.parent(),
                tdIndex = cell.index(),
                sz, office, bodyViewWidth, bodyViewHeight, cellR, cellB, sLeft, sTop;

            sz = me.getCellSize(cell);
            office = me.getCellPosition(cell);

            bodyViewWidth = bodyView.width();
            bodyViewHeight = bodyView.height();

            cellR = office.left + sz.width;
            cellB = office.top + sz.height;

            sLeft = bodyView.scrollLeft();
            sTop = bodyView.scrollTop();

            if (cellR > bodyViewWidth - 15) {
                scrollX = cellR - bodyViewWidth + 15;
            }
            else if (office.left < 0) {
                scrollX = office.left - 2;
            }

            if (cellB > (bodyViewHeight + 16 - 2)) {
                scrollY = (cellB - bodyViewHeight - 16 + 2);
            }
            else if (office.top < me.headerContainer.height) {
                scrollY = -(me.headerContainer.height - office.top);
            }


            bodyView.scrollLeft(sLeft + scrollX);
            bodyView.scrollTop(sTop + scrollY);

        }
    },

    //单元格焦点自动滚动
    autoFocusScroll: function (cellEl) {
        "use strict";
        var me = this,
            bodyView = me.gridViewEl,
            cell = $(cellEl);

        if (cell.length) {

            var scrollX = 0,
                scrollY = 0,
                rowEl = cell.parent(),
                tdIndex = cell.index(),
                sz, office, bodyViewWidth, bodyViewHeight, cellR, cellB, sLeft, sTop;

            sz = me.getCellSize(cell);
            office = me.getCellPosition(cell);

            bodyViewWidth = bodyView.width();
            bodyViewHeight = bodyView.height();

            cellR = office.left + sz.width;
            cellB = office.top + sz.height;

            sLeft = bodyView.scrollLeft();
            sTop = bodyView.scrollTop();

            if (cellR > bodyViewWidth - 15) {
                scrollX = cellR - bodyViewWidth + 15;
            }
            else if (office.left < 0) {
                scrollX = office.left - 2;
            }

            if (cellB > (bodyViewHeight + 16 - 2)) {
                scrollY = (cellB - bodyViewHeight - 16 + 2);
            }
            else if (office.top < me.headerContainer.height) {
                scrollY = -(me.headerContainer.height - office.top);
            }


            //        me.bodyEl.scrollLeft(sLeft + scrollX);
            //        me.bodyEl.scrollTop(sTop + scrollY);

            bodyView.scrollLeft(sLeft + scrollX);
            bodyView.scrollTop(sTop + scrollY);

            //$(this).trigger('focusin');


            Mini2.EventManager.setMouseDown(cellEl);


            //        log.begin("autoFocusScroll()");

            Mini2.ui.FocusManager.setControl(cellEl);

            //        if (me.editor && me.editor.visible) {
            //            me.editor.hide();
            //        }

            //        log.end();
        }
    },


    getSelectRange:function(){
        var me = this,
            fr = me.focusedRegion;


        var itemA = fr.itemA || fr.itemB;
        var itemB = fr.itemB || fr.itemA;

        var rowAEl = $(itemA).parent();
        var rowBEl = $(itemB).parent();

        var colAIndex = $(itemA).index();
        var colBIndex = $(itemB).index();

        var rowAIndex = $(rowAEl).index();
        var rowBIndex = $(rowBEl).index();


        var colA = Math.min(colAIndex, colBIndex);
        var colB = Math.max(colAIndex, colBIndex);

        var rowA = Math.min(rowAIndex, rowBIndex);
        var rowB = Math.max(rowAIndex, rowBIndex);

        return {
            colA: colA,
            colB: colB,
            rowA: rowA,
            rowB: rowB
        };
    },


    /**
    * 显示复选框的菜单项目
    *
    */
    showCheckMenu:function(evt){
        var me = this,
            menu = me.menu;
        

        if (!menu) {

            function setCheckCol(checkMode) {
                var fr = me.focusedRegion,
                    store = me.store;

                if (fr && (fr.itemA || fr.itemB)) {

                    var sr = me.getSelectRange();

                    for (var row = sr.rowA; row <= sr.rowB; row++) {

                        var rowEl = me.tbodyEl.children(':eq(' + row + ')');
                        var record = $(rowEl).data('record');

                        for (var col = sr.colA; col <= sr.colB; col++) {

                            var cc = me.m_Cols[col];

                            if ('checkcolumn' != cc.miType) {
                                continue;
                            }

                            if ('reverse' == checkMode) {
                                var srcValue = !!record.get(cc.dataIndex);
                                record.set(cc.dataIndex, !srcValue);
                            }
                            else {
                                record.set(cc.dataIndex, checkMode);
                            }

                        }
                    }

                }


            }

            me.menu = menu = Mini2.create('Mini2.ui.menu.Menu', {

                left: evt.pageX,
                top: evt.pageY,

                items: [{
                    text: '全选',

                    click: function () {
                        setCheckCol(true);
                    }

                }, {
                    text: '反选',
                    click: function () {
                        setCheckCol('reverse');
                    }
                }, {
                    text: '全不选',

                    click: function () {
                        setCheckCol(false);
                    }
                }]

            });

            menu.render();

        }
        else {
            menu.setLocal(evt.pageX, evt.pageY).show();
        }

    },
    
                
    getSeelctCellText:function() {

        var me = this,
            fr = me.focusedRegion,
            store = me.store,
            txt = '';

        if (fr && (fr.itemA || fr.itemB)) {

            var sr = me.getSelectRange();

            var rowIndex = 0;

            for (var row = sr.rowA; row <= sr.rowB; row++) {

                if (rowIndex++ > 0) {
                    txt += '\n';
                }

                var rowEl = me.tbodyEl.children(':eq(' + row + ')');
                var record = $(rowEl).data('record');

                var colIndex = 0;

                for (var col = sr.colA; col <= sr.colB; col++) {

                    var cc = me.m_Cols[col];

                    var srcValue = record.get(cc.dataIndex);

                    if (colIndex++ > 0) {
                        txt += '\t';
                    }

                    txt += srcValue;

                }

            }

        }

        return txt;
    },





    //显示焦点边界
    showFocusBorder:function(cellEl){
        var me = this,
            el = me.el,
            $focusEl = me.$focusEl;

        if (me.focusBorders) {

            if (!$focusEl) {

                $focusEl = Mini2.$joinStr([
                    '<div class="mi-grid-cell-focused" >',
                        '<div class="mi-focused-backdrop"></div>',

                        '<div class="mi-focused-border mi-focused-border-left"></div>',
                        '<div class="mi-focused-border mi-focused-border-top"></div>',
                        '<div class="mi-focused-border mi-focused-border-right"></div>',
                        '<div class="mi-focused-border mi-focused-border-bottom"></div>',

                        '<div class="mi-focused-handle"></div>',
                    '</div>']);  //焦点面板

               
                me.gridViewEl.append($focusEl);

                me.$focusEl = $focusEl;

                var $focusHandleEl = $focusEl.children('.mi-focused-handle');
                var $focusBorderEls = $focusEl.children('.mi-focused-border');

                $focusHandleEl.muBind('mousedown', function () {

                }).muBind('mouseup', function () {

                });

                $focusBorderEls.muBind('mousedown', function () {

                }).muBind('mouseup', function () {

                }).muBind('contextmenu', function (evt) {

                    me.showCheckMenu(evt);

                    return false;
                });
            }

            me.setFocusRegion(cellEl);

        }

        return me;
    },

    //隐藏焦点边界框
    hideFocusBorder:function(cellEl){
        var me = this,
            el = me.el,
            $focusEl = me.$focusEl;
        
        if (me.focusBorders) {
            var cellEl = $focusEl.data('cellEl');

            if (cellEl === this) {
                $focusEl.hide();
            }
        }

        return me;
    },


    cell_mouse_button : null,


    /**
    * 焦点区域
    * @private focusedRegion
    */
    //focusedRegion:{
    //    itemA: null,
    //    itemB:null
    //},

    /**
    * 设置焦点区域框
    * @param {Object} startCellEl 起始单元格 Dom
    * @param {Object} endCellEl 结束单元格 Dom
    */
    setFocusRegion: function (startCellEl, endCellEl, force) {
        "use strict";

        var me = this,
            log = me.log,
            $focusEl = me.$focusEl,
            viewEl = me.gridViewEl,
            itemA = startCellEl,
            itemB = endCellEl,
            lastRegion = me.focusedRegion,
            aLocal,bLocal,
            x0, y0, x1, y1;

        if (true != force) {
            //如果全没有参数, 就隐藏
            if (!startCellEl && !endCellEl) {

                if ($focusEl) {
                    $focusEl.hide();
                }

                return;
            }

            if (lastRegion && lastRegion.itemA == itemA && lastRegion.itemB == itemB) {
                return;
            }
        }
        
        if ($focusEl) {
            me.focusedRegion = {
                itemA: itemA,
                itemB: itemB
            };



            var pOffset = viewEl.offset();

            var scrollTop = viewEl.scrollTop();
            var scrollLeft = viewEl.scrollLeft();

            //console.log("scrollTop = %d, scrollLeft = %d", scrollTop, scrollLeft);

            aLocal = $(itemA).offset();


            pOffset.left -= scrollLeft;
            pOffset.top -= scrollTop;

            if (itemA && itemB && itemA != itemB) {

                bLocal = $(itemB).offset();

                x0 = Math.min(aLocal.left, bLocal.left);
                y0 = Math.min(aLocal.top, bLocal.top);

                x1 = Math.max(aLocal.left + $(itemA).width(), bLocal.left + $(itemB).width());
                y1 = Math.max(aLocal.top + $(itemA).height(), bLocal.top + $(itemB).height());

            }
            else {
                x0 = aLocal.left;
                y0 = aLocal.top;

                x1 = aLocal.left + $(itemA).width();
                y1 = aLocal.top + $(itemA).height();
            }

            x0 -= pOffset.left;
            y0 -= pOffset.top;

            x1 -= pOffset.left;
            y1 -= pOffset.top;



            $focusEl.css({
                left: x0 - 2,
                top: y0 - 2,
                width: x1 - x0 + 4,
                height: y1 - y0 + 4
            }).show();
        }

        // 如果自动选中启动
        if (me.autoRowCheck && itemB) {

            var rowEl_A = $(itemA).parent();
            var rowEl_B = $(itemB).parent();

            var aIndex = $(rowEl_A).index();
            var bIndex = $(rowEl_B).index();


            var tmp1 = Math.min(aIndex, bIndex);
            var tmp2 = Math.max(aIndex, bIndex);

            var tbodyEl = me.tbodyEl;

            
            var i = 0;

            var checkCol = me.rowCheckColumn;

            tbodyEl.children().each(function () {

                var cell = $(this).children(":eq(" + checkCol.index + ")");

                var imgEl = cell.find('img:first');
                
                if (i >= tmp1 && i <= tmp2) {
                    if (imgEl.hasClass('mi-grid-checkcolumn-checked')) {

                    }
                    else {
                        me.setRowCheck(this, true);
                    }
                }
                else {
                    if (imgEl.hasClass('mi-grid-checkcolumn-checked')) {
                        me.setRowCheck(this, false);
                    }
                    else {
                        //me.setRowCheck(rowEl, true);
                    }
                }

                i++;

            });

            
        }

        return me;
    },

    /**
    * 扩展事件
    **/
    extendCellEvent: function (cellEl) {
        "use strict";
        /// <summary>扩展事件</summary>

        var me = this,
            log = me.log;



        $(cellEl).on('focusin',function () {
            $(this).addClass("mi-grid-cell-selected");

            me.showFocusBorder(this);
        })
        .on('focusout',function () {
            $(this).removeClass("mi-grid-cell-selected");

            me.hideFocusBorder(this);
        });

        $(cellEl).muBind('mousedown', function (evt,srcEvt) {
            //log.begin("cell.MouseDown()");

            me.cell_mouse_button = evt.button;

            //log.debug("鼠标单击下啊....", evt);
            
            if (me.cell_mouse_button == 0) {
                me.autoFocusScroll(this);
            }

            //log.end();

        }).muBind('mousemove',function(evt,srcEvt){
            
            if (me.cell_mouse_button == 0) {

                var hoverEl = srcEvt.target;    //鼠标所在的元素上方
                
                if (!$(hoverEl).hasClass('mi-grid-cell')) {
                    var cellEl = $(hoverEl).parents('.mi-grid-cell');
                    hoverEl = cellEl[0];
                }

                if (hoverEl != null && this !== hoverEl) {

                    if ($(hoverEl).hasClass('mi-grid-cell-row-numberer') ) {
                        return;
                    }

                    me.setFocusRegion(this, hoverEl);
                }

               // console.debug("hoverObject = ", hoverObject);

            }


        }).muBind('mouseup', function (evt,srcEvt) {

            //log.begin("cell.MouseUp()");

            //console.log("mouserup = ", this);

            var cellEl;
            
            if ($(srcEvt.target).hasClass('mi-grid-cell')) {
                cellEl = srcEvt.target;
            }
            else {
                cellEl = $(srcEvt.target).parents('.mi-grid-cell');

                cellEl = cellEl[0];
            }



            if (me.cell_mouse_button == 0 && this === cellEl) {



                var rowEl, record;

                rowEl = $(this).parent();
                record = $(rowEl).data('record');

                me.store.setCurrent(record);

                me.cell_Click(this);

            }

            me.cell_mouse_button = null;

            //log.end();
        }).on('dblclick',function(e) {

            //log.debug("双击啊.....");

            //me.store.setCurrent(record);
            
            me.cell_dblclick(this);
        });


        //$(cellEl).muBind('keydown', function (e, exData) {

        //    var keyCode = event.keyCode;


        //    if (keyCode == 9) {

        //        var grid = exData.data,
        //            nextCellEl = $(this).next();


        //        if (nextCellEl.length >= 1) {

        //            grid.autoFocusScroll(nextCellEl);

        //            grid.cell_Click(nextCellEl);

        //            grid.m_FocusCell = nextCellEl;

        //            Mini2.EventManager.stopEvent(e);
        //        }
        //    }

        //});


        return me;
    },

    bindEvent_ForCell: function (record, rowEl) {
        "use strict";
        /// <summary>绑定事件到单元格</summary>

        var me = this,
            tdList = $(rowEl).children('td');

        $(tdList).each(function () {
            me.extendCellEvent(this);
        });
    },


    bindEvent_ForRow: function (record, rowEl) {
        "use strict";
        /// <summary>记录绑定事件</summary>
        var me = this,
            rowCls = 'mi-grid-row',
            overCls = rowCls + '-over';

        $(rowEl).mouseenter(function (e) {
            $(this).addClass(overCls);
        })
        .mouseleave(function (e) {
            $(this).removeClass(overCls);
        });

    },

    //按类型获取列集合
    getColumnByType: function (miType) {
        "use strict";
        var me = this,
            col,
            i = 0,
            colArray = [],
            cols = me.m_Cols,
            len = cols.length;

        for (; i < len; i++) {
            col = cols[i];

            if (col.miType.toLowerCase() == miType) {
                colArray.push(col);
            }
        }

        return colArray;
    },


    //刷新纪录后，触发的事件
    //itemReseted: false,



    //刷新纪录
    itemsReset: function () {
        "use strict";
        /// <summary>刷新记录</summary>
        var me = this,
            log = me.log,
            rowNumbererColumn = null,
            rows,
            rowNumbererCols,
            rowIndex = 0,
            itemReseted = me.itemReseted,   //刷新纪录后，触发的事件
            rowAltEnabled = me.rowAltEnabled,
            altCls = 'mi-grid-row-alt',
            locledCls = 'mi-grid-locked-row';

        rows = $(me.tbodyEl).children('.mi-grid-row');

        rowNumbererCols = me.getColumnByType('rownumberer');

        $(rows).each(function () {
            var rowEl = $(this),
                record,
                isLocked,
                i = rowNumbererCols.length;

            if (rowAltEnabled) {
                rowEl.toggleClass(altCls, !(rowIndex % 2));
            }

            if (i) {
                record = rowEl.data('record');
                isLocked = record.isLocked();

                rowEl.toggleClass(locledCls, isLocked);

                while (i--) {
                    me.afterCellUpdate(rowNumbererCols[i], this, record, rowIndex);
                }
            }

            if (itemReseted) {
                try {
                    itemReseted.call(me, {
                        rowEl: rowEl,
                        rowIndex: rowIndex,
                        record: record
                    });
                }
                catch (ex) {
                    log.debug('调用刷新记录错误.');
                }
            }

            rowIndex++;
        });

        me.m_NewRowIndex = rowIndex;
    },


    //填充明细列的信息,也就是最底层的列
    fullDetailCols: function (detailCols, parentCol, cols, dept) {
        "use strict";
        var me = this,
            i,j,
            col,
            colLen = (cols?cols.length:0) ,
            config;

        dept = dept || 1;

        if (colLen) {
            
            //计算标题行数
            for (i = 0; i < colLen; i++) {
                config = cols[i];
                col = me.getColumnForConfig(config);

                col.groupRoot = col.groupRoot || parentCol; //根组
                col.parent = parentCol; //上级


                cols[i] = col;

                if (config.columns ) {
                    me.fullDetailCols(detailCols,col, col.columns, dept + 1);
                }
                else {
                    detailCols.push(col);
                }


                if (dept > 1) {
                    col.isSub = true;
                }
            }

            if (dept > 1) {
                cols[0].isFirst = true;

                cols[colLen - 1].isLast = true;
            }

            if (dept == 1) {

                j = 0;

                for (i = 0; i < colLen; i++) {
                    col = cols[i];
                    if (col.columns) {
                        col.groupIndex = j++;
                    }
                    else {
                        j = 0;
                    }
                }
            }
        }

        return detailCols.length;
    },

    onInit: function () {
        "use strict";
        var me = this,
            index = 0,
            i,
            store = me.store,
            col,
            cols;


        if (me.isInit) {
            return;
        }

        me.isInit = true;

        me.checkedRecords = [];

        //被选中的行元素
        me.checkedRowEls = [];

        if (me.cellDbclick) {
            
            me._cellDbclick(me.cellDbclick);
            
            delete me.cellDbclick;
        }

        me.cellDbclick = me._cellDbclick;
        
        delete me._cellDbclick;


        if (Mini2.isString(store)) {

            store = Mini2.data.StoreManager.lookup(store);

            me.bindStore(store);
        }

        me.srcColumns = Mini2.clone(me.columns);   //用户添加进来的列对象

        delete me.m_Cols;
        me.m_Cols = cols = [];

        me.fullDetailCols(cols, null, me.columns);

        for (i = 0; i < cols.length; i++) {
            col = cols[i];
            col.index = i;
        }

        me.cellTpl = me.StrJoin(me.cellTpl);


        //重新绑定到事件集上面
        if (me.rowsChecked) {

            var fun = me.rowsChecked;
            
            me.bind('rowsChecked', fun);

            delete me.rowsChecked;
        }
    },

    //根据列标签，获取列对象的命名空间
    getColumnNameForMiType: function (miType) {
        "use strict";
        var colName = 'Mini2.ui.grid.column.';

        switch (miType) {
            case 'booleancolumn':
            case 'boolcolumn': colName += 'Boolean'; break;
            case 'checkcolumn': colName += 'CheckColumn'; break;
            case 'rownumberer': colName += 'RowNumberer'; break;
            case 'rowcheckcolumn': colName += 'RowCheckColumn'; break;
            case 'numbercolumn': colName += 'Number'; break;
            case 'datecolumn': colName += 'Date'; break;
            case 'actioncolumn': colName += 'Action'; break;
            case 'propertycolumn': colName += 'PropertyColumn'; break;
            case 'treecolumn': colName += 'TreeColumn'; break;
            case 'template': colName += 'Template'; break;
            case 'textareacolumn': colName += 'TextArea'; break;
            case 'morefilecolumn': colName += 'MoreFileColumn'; break;
            case 'timecolumn': colName += 'Time'; break;
            case 'passwordcolumn': colName += 'Password'; break;
            default: colName += 'Colunm'; break;
        }

        return colName;
    },

    getColumnForConfig: function (colConfig) {
        "use strict";
        var me = this,
            sysInfoTab = Mini2.SystemInfo.table,
            miType,
            col,
            cols = me.m_Cols,
            colEditor,
            colName;

        //var col = this;
        
        Mini2.applyIf(colConfig, {
            miType:'col',
            width: 120,
            headerAlign: sysInfoTab.headerAlign
        });
;
        colConfig.width = colConfig.width * sysInfoTab.headerWidthRaed;


        miType = colConfig.miType = colConfig.miType.toLowerCase();
        colName = me.getColumnNameForMiType(miType);


        col = Mini2.create(colName, colConfig);

        switch (miType) {
            case 'rowcheckcolumn': me.rowCheckColumn = col; break;
        }

        col.grid = me;
        col.store = me.getStore();

        colEditor = col.editor;

        if (colEditor) {
            colEditor.isInit = false;
        }

        //如果存在数据仓库
        if (colEditor.store) {
            var newData, newStore;

            newData = Mini2.clone(col.editor.store);

            newStore = Mini2.create('Mini2.data.Store', {
                storeId: 'MiniStore_' + Mini2.newId(),
                idField: 'value',
                fields: ['value', 'text'],
                data: newData
            });

            col.displayStore = newStore;

            colEditor.store = newStore;
        }

        if (!col.renderer) {

        }

        //col.index = index;

        //cols.push(col);

        return col;
    },

    //设置只读状态
    setReadOnly: function (value) {
        var me = this;

        me.readOnly = value;

        return me;
    },


    clearColumnEditor: function (col) {
        var me = this,
            editor = col.editor;

        if (editor) {
            if (editor.remove) {
                editor.remove();

                delete col.editor;
            }
        }
    },


    //清理掉全部列
    clearColumnAll: function () {
        "use strict";
        var me = this,
            col,
            cols = me.m_Cols,
            len = cols.length,
            i,
            delayData = me.delayData,
            header = me.headerContainer;

        delayData.i = Number.MAX_VALUE;

        me.setFocusRegion();

        me.itemRemoteAll();

        me.m_LastEditor = null;
        for (i = 0; i < len; i++) {
            col = cols[i];           
            me.clearColumnEditor(col);
        }

        header.clear();

        return me;
    },

    //添加列
    addColumn: function (columns) {
        "use strict";
        var me = this,
            i,
            colGroupEl,
            cg,
            delayData = me.delayData,
            header = me.headerContainer,
            col,
            cols ;



        delete me.m_Cols;
        me.m_Cols = cols = [];

        me.srcColumns = Mini2.clone(columns);   //用户添加进来的列对象

        //console.debug("me.srcColumns", me.srcColumns);

        me.columns = columns;

        me.fullDetailCols(cols, null, me.columns);

        for (i = 0; i < cols.length; i++) {
            col = cols[i];
            col.index = i;
        }


        header.add(columns);


        $(me.m_ColGroups).remove();

        cg = me.createColGroup();

        me.m_ColGroups = $(cg);

        me.tbodyEl.before(me.m_ColGroups);

        me.setHeight(me.height);


        //处理加载数据
        delayData.i = 0;

        me.loopLoadItem();


        if (me.summaryVisible) {

            me.summary.columns = cols;

            try {
                me.summary.reset();
            }
            catch (ex) {
                console.error("重置合计 html 错误", ex);
            }
        }


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
        
        if ( 'full' == me.jsonMode) {
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

        var refKey = 'record-' + Mini2.Guid.newGuid();
        var refData = {};

        refData[refKey] = recordJson;


        widget1.subMethod('form:first', {
            subName: me.id,
            subMethod: 'PreCommand',
            actionPs: {
                cmdName: cmdName,
                cmdParam: cmdParam,
                record: 'ref ' + refKey
            },
            data: refData
        });


    },

    /**
     * 获取选中的记录.加工过的
     */
    getCheckeds: function (fields) {
        "use strict";

        var me = this,
            col = me.rowCheckColumn,
            records = [];


        if (!col) {
            return records;
        }


        var rowEl, record, cell, imgEl, isCheck,
            rows = $(me.tbodyEl).children();


        $(rows).each(function () {
            rowEl = $(this);

            record = rowEl.data('record');
            cell = rowEl.children("td:eq(" + col.index + ")");
            imgEl = cell.find('img:first');

            isCheck = $(imgEl).hasClass("mi-grid-checkcolumn-checked");

            if (isCheck) {

                if (fields) {

                    var cloneRect = {};

                    $(fields).each(function () {
                        cloneRect[this] = record.get(this);
                    });

                    records.push(cloneRect);

                }
                else {
                    records.push(Mini2.clone(record.data));
                }


            }
        });

        return records;

    },


    /**
    * 获取选中都记录
    */
    getCheckRecords:function(jsonMode){
        "use strict";

        var me = this,
            col = me.rowCheckColumn,
            checkObj = {
                records: []
            };


        if (!col) {
            return checkObj;
        }

        jsonMode = jsonMode || me.jsonMode;

        var rowEl, record, cell, imgEl, isCheck,
            rows = $(me.tbodyEl).children();


        $(rows).each(function () {
            rowEl = $(this);

            record = rowEl.data('record');
            cell = rowEl.children("td:eq(" + col.index + ")");
            imgEl = cell.find('img:first');

            isCheck = $(imgEl).hasClass("mi-grid-checkcolumn-checked");

            if (isCheck) {

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
            }
        });

        return checkObj;
    },


    //
    // jsonMode = sipmle-简单类型 | full-全部类型;
    getJson: function (jsonMode) {
        "use strict";
        var me = this,
            json,
            checkObj;

        checkObj = me.getCheckRecords(jsonMode);

        json = Mini2.Json.toJson(checkObj);

        return json;
    }
}, function () {

    var me = this,
        cfg = Mini2.SystemInfo.table,
        ps = arguments[0];

    Mini2.apply(this, {
        columnLines: cfg.columnLines,
        focusBorders: cfg.focusBorders
    });

    Mini2.apply(this, ps);

});