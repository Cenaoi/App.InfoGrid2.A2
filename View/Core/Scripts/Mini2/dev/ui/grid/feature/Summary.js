
/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../../Mini.js" />

//摘要
Mini2.define('Mini2.ui.grid.feature.Summary', {


    renderTo: Mini2.getBody(),

    grid: null,

    el: null,

    columns: null,

    height: 30,

    width: 1024,

    //索引
    //items: false,

    panelTpl: [
        '<div class="mi-component mi-docked-summary mi-docked-summary-bottom  mi-component-default mi-docked-bottom mi-component-docked-bottom mi-component-default-docked-bottom mi-noborder-rbl" ',
            'style="overflow: hidden; width: 489px; right: auto; left: 0px; top: 352px; height:30px;">',
            '<div data-ref="innerCt" class="innerCt" role="presentation" style="width: 506px; position: absolute; ">',

            '</div>',
        '</div>'
    ],

    tableTpl: [
        '<table data-ref="item" cellpadding="0" cellspacing="0" class="mi-grid-item mi-grid-with-col-lines" ',
            '>',
            '<tbody>',
                '<tr class="mi-grid-row-summary " tabindex="-1">',

                '</tr>',
            '</tbody>',
        '</table>'
    ],

    cellTpl: [
        '<td class="mi-grid-cell mi-grid-td mi-grid-cell-first mi-unselectable" ',
            'style="width: 125px;" >',
            '<div unselectable="on" class="mi-grid-cell-inner " style="text-align: left;">',
                '&nbsp;',
            '</div>',
        '</td>'
    ],



    //初始化组件
    initComponent: function () {

        var me = this,
            store = me.store;

        me.items = {};
    },


    bindStore: function () {
        var me = this,
            store = me.store;


        $(store).muBind('summarychnaged', me, me.store_summaryChnaged);
    },


    store_summaryChnaged: function (event, summary) {
        "use strict";
        var me = event.data;

        if (summary) {
            me.setSummary.call(me, summary);
        }
    },

    setSummary: function (field, value) {
        "use strict";
        var me = this,
            cols = me.columns,
            items = me.items,
            cellInnerEl,

            fields;

        if (Mini2.isString(field)) {
            fields = {};
            fields[field] = value;
        }
        else {
            fields = field;
        }

        for (field in fields) {

            cellInnerEl = items[field];

            
            if (cellInnerEl) {
                value = fields[field];

                var col = me.findColumnByField(field);

                if (!Mini2.isBlank(col.summaryFormat)) {


                    if (Mini2.isNumber(value) && !Mini2.String.isBlank(col.format)) {

                        try {
                            value = Mini2.util.Format.number(value, col.format);
                        }
                        catch (ex) {
                            console.error('格式化数字错误.', ex);
                        }
                    }


                    var valueStr = $.format(col.summaryFormat, value);
                    cellInnerEl.html(valueStr);

                }
                else {
                    cellInnerEl.html(value);
                }

            }
        }

    },

    findColumnByField: function (field) {
        "use strict";
        var me = this,
            cols = me.columns,
            col;

        for (var i = 0; i < cols.length; i++) {

            col = cols[i];

            if (col.dataIndex == field) {
                
                return col;
            }
        }

        return null;
    },

    createCell: function (col) {
        "use strict";
        var me = this,
            cellEl,
            cellInnerEl;

        cellEl = Mini2.$joinStr(me.cellTpl);

        cellInnerEl = cellEl.children('.mi-grid-cell-inner');

        if (col.summaryType) {
            cellInnerEl.data('me', col);
            me.items[col.dataIndex] = cellInnerEl;

            cellInnerEl.css({
                'font-size': '14px',
                'font-weight': 'bold'
            });

            cellInnerEl.css({
                'text-align': col.align
            });
        }

        return cellEl;
    },


    createCellGroups: function () {
        "use strict";
        var me = this,
            i=0,
            col,
            cols = me.columns,
            tableEl,
            rowSummaryEl,
            cellEl,
            tabWidth = 0;

        tableEl = Mini2.$joinStr(me.tableTpl);
        rowSummaryEl = tableEl.find('.mi-grid-row-summary');



        for (; i < cols.length; i++) {
            col = cols[i];

            if (!col) {
                continue;
            }

            cellEl = me.createCell(col);

            cellEl.css({
                width: (col.width - 1) + "px"
            });

            cellEl.children('.mi-grid-cell-inner').width(col.width - 1);

            tabWidth += col.width;

            rowSummaryEl.append(cellEl);
        }

        //tableEl.css('width', tabWidth);


        return tableEl;
    },

    setWidth: function (value) {
        "use strict";
        var me = this,
            el = me.el,
            innerCtEl = me.innerCtEl,
            tableEl = me.tableEl;

        me.width = value;

        el.css("width", value - 20);

        innerCtEl.css("width", value);
        //tableEl.css("width", value);

        return me;
    },


    scrollLeft: function (x) {
        var me = this,
            el = me.el,
            innerCtEl = me.innerCtEl;

        innerCtEl.css('left', -x);

        return me;
    },


    resizeCellWidth: function () {
        "use strict";
        var me = this,
            i,
            col,
            cols = me.columns,
            tableEl = me.tableEl,
            rowSummaryEl,
            cellEl,
            tabWidth = 0;


        rowSummaryEl = tableEl.find('.mi-grid-row-summary');

        var cellEls = $(rowSummaryEl).children('.mi-grid-cell');

        for (i = 0; i < cols.length; i++) {
            col = cols[i];

            if (col.visible) {
                cellEl = $(cellEls[i]);

                cellEl.css({
                    width: (col.width - 1) + "px"
                });

                cellEl.children('.mi-grid-cell-inner').width(col.width - 9);

                tabWidth += col.width;

                cellEl.show();
            }
            else {

                cellEl.hide();
            }

        }

        //tableEl.css('width', tabWidth);
    },

    reset: function () {
        "use strict";
        var me = this,
            el = me.el,
            tableEl,
            innerCtEl;

        $(me.tableEl).remove();

        me.innerCtEl = innerCtEl = el.children('.innerCt');

        me.tableEl = tableEl = me.createCellGroups();

        innerCtEl.append(tableEl);

        var w = me.grid.getWidth();

        me.setWidth(w);
    },

    render: function () {
        "use strict";
        var me = this,
            store = me.store,
            grid = me.grid,
            el,
            innerCtEl,
            tableEl;

        me.el = el = Mini2.$joinStr(me.panelTpl);

        if (me.applyTo) {
            $(me.applyTo).append(el);
        }
        else {
            $(me.renderTo).append(el);
        }


        me.innerCtEl = innerCtEl = el.children('.innerCt');

        me.tableEl = tableEl = me.createCellGroups();

        innerCtEl.append(tableEl);


        if (store && store.summary) {
            me.setSummary(store.summary);
        }

        me.bindStore();


        var w = me.grid.getWidth();

        me.setWidth(w);
    }

}, function (args) {
    var me = this;

    Mini2.apply(me, args);
    me.initComponent();

});