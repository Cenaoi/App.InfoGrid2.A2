
/// <reference path="../../../Mini.js" />
/// <reference path="../../../lang/Number.js" />
/// <reference path="../../../lang/Date.js" />

/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.PropertyColumn', {

    extend: 'Mini2.ui.grid.column.Colunm',

    isPropertyColumn: true,

    dateFormat: 'Y-m-d',

    trueText: 'true',

    falseText:'false',

    propEditor: {},

    defaultRenderer: function (value) {
        return value;
    },

    // private
    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store) {
        "use strict";
        var me = this,
            vType = record.data['Type'] || 'string';


        if ('bool' == vType && Mini2.isBoolean(value)) {
            return me.renderBool(value);
        }
        else if ('date' == vType && Mini2.isDate(value)) {
            return Mini2.Date.format(value, me.dateFormat);
        }

        return value;
    },

    renderBool : function(bVal) {
        return this[bVal ? 'trueText' : 'falseText'];
    },

});
