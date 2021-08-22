
/// <reference path="../../Mini2.js" />
/// <reference path="../../lang/Number.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.TreeColumn', {

    extend: 'Mini2.ui.grid.column.Colunm',

    format: '0',        //0,000.00

    align: 'right',

    defaultValue: 0,


    renderTpl: [
    '<td role="gridcell" class="mi-grid-cell mi-grid-td mi-grid-cell-treecolumn">',
        '<div class="mi-grid-cell-inner mi-grid-cell-inner-treecolumn tree-node" style="padding-top:0px;padding-bottom:0px;" >',
        '</div>',
    '</td>'
    ],

    defaultRenderer: function (value) {
        return value;
    },


    renderCell: function () {
        var me = this,
            cellEl,
            innerEl;

        cellEl = me.getBufferCellEl();

        if (!cellEl) {
            cellEl = Mini2.$joinStr(me.renderTpl);

            innerEl = cellEl.children('.mi-grid-cell-inner');
            innerEl.css('text-align', me.align);

            me.setBufferCellEl(cellEl);
        }

        cellEl = cellEl.clone();

        return cellEl;
    },

    // private
    //metaData :原字段值
    //cellValues : 字段值集合
    //record : 实体
    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store,view,rowEl,cellEl) {
        var me = this,
            result,
            rootId = store.rootId,
            valueField = store.valueField,
            textField = store.textField,
            parField = store.parField;


        try{


            result = '<i class="tree-icon tree-ocl" ></i>' +
                     '<a class="tree-anchor"><i class="tree-icon tree-themeicon"></i> ' + value + '</a>';


        }
        catch (ex) {
            alert(ex.message);
        }

        return result;
    },


    bindEvent: function (view, cell, recordIndex, cellIndex, e, record, row) {

        var me = this,
            btns = cell.find("i.tree-icon");

        $(btns).muBind('mousedown', Mini2.emptyFn)
        .muBind('mouseup', function (e2) {

            me.processEvent("mouseup", view, cell, recordIndex, cellIndex, e2, record, row);

            Mini2.EventManager.stopEvent(e2);
        });
    },


    processEvent: function (type, view, cell, recordIndex, cellIndex, e, record, row) {

        var column = this,
            me = e.currentTarget,
            item,
            items = this.items,
            node = $(row).data('tree-node');


        view.toggleNode(node,row,cell);

    }


});
