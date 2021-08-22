

Mini2.define('Mini2.ui.grid.CellContext', {


    isCellContext: true,

    constructor: function (view) {
        this.view = view;
    },

    setPosition: function (row, col) {
        "use strict";
        var me = this;

        // We were passed {row: 1, column: 2, view: myView}
        if (arguments.length === 1) {
            if (row.view) {
                me.view = row.view;
            }
            col = row.column;
            row = row.row;
        }

        me.setRow(row);
        me.setColumn(col);

        return me;
    },


    setRow: function (row) {
        "use strict";
        var me = this;

        if (row !== undefined) {
            // Row index passed
            if (typeof row === 'number') {
                me.row = Math.max(Math.min(row, me.view.dataSource.getCount() - 1), 0);
                me.record = me.view.dataSource.getAt(row);
            }
            // row is a Record
            else if (row.isModel) {
                me.record = row;
                me.row = me.view.indexOf(row);
            }
            // row is a grid row
            else if (row.tagName) {
                me.record = me.view.getRecord(row);
                me.row = me.view.indexOf(me.record);
            }
        }
    },


    setColumn: function (col) {
        "use strict";
        var me = this,
            columnManager = me.view.ownerCt.columnManager;

        if (col !== undefined) {
            // column index passed
            if (typeof col === 'number') {
                me.column = col;
                me.columnHeader = columnManager.getHeaderAtIndex(col);
            }
            // column Header passed
            else if (col.isHeader) {
                me.columnHeader = col;
                me.column = columnManager.getHeaderIndex(col);
            }
        }

    }


});