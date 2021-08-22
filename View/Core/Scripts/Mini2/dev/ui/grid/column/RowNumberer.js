
/// <reference path="../../define.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.RowNumberer', {

    extend: 'Mini2.ui.grid.column.Colunm',

    headerText: "&#160",


    sortable: false,


    align: 'center', //'right',

    width: 23,

    dataIndex: '',

    rowspan: undefined,

    menuDisabled: true,

    autoLock: true,

    resizable: false,

    renderTpl: [
        '<td role="gridcell" class="mi-grid-cell mi-grid-td mi-grid-cell-row-numberer ">',
            '<div class="mi-grid-cell-inner mi-unselectable mi-grid-cell-inner-row-numberer">',
            '</div>',
        '</td>'
    ],

    initComponent:function(){

        this.constructor.base.initComponent();  //调用上级函数
        
        var me = this,
            sysInfo = Mini2.SystemInfo,
            cfg = sysInfo.table.cols.rowNumberer;


        me.width = cfg.width || 23;

        
    }, 

    defaultRenderer: function (value) {
        return value;
    },

    renderCell: function () {
        "use strict";
        var me = this,
            cellEl;

        cellEl = me.getBufferCellEl();

        if (!cellEl) {

            cellEl = Mini2.$join(me.renderTpl);

            if ('left' != me.align) {
                cellEl.css('text-align', me.align);
            }

            me.setBufferCellEl(cellEl);
        }

        cellEl = cellEl.clone();
        return cellEl;
    },

    // private
    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store) {
        "use strict";
        var rowspan = this.rowspan,
            page = store.currentPage,
            result = record.index;

        return (page * store.pageSize + rowIdx + 1);
    }

});
