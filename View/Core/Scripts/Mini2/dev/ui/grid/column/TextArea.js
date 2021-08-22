

/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../../Mini.js" />


Mini2.define('Mini2.ui.grid.column.TextArea', {

    extend: 'Mini2.ui.grid.column.Colunm',


    renderTpl: [
        '<td role="gridcell" class="mi-grid-cell mi-grid-td ">',
        '<div class="mi-grid-cell-inner" style="white-space:pre-line;">',
        '</div>',
        '</td>'
    ],


    renderer: function (value, fieldValue, cellValues, record, recordIndex, columnIndex, store, table) {
        "use strict";
        var me = this;


        if (!Mini2.isBlank(value)) {
            
            value = value
                .replace(/&/g, '&amp;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;')
                .replace(/ /g, '&nbsp;')
                .replace(/\n/g, '<br />');

        }
        else {
            value = '&nbsp;';
        }

        return value;
    }

});