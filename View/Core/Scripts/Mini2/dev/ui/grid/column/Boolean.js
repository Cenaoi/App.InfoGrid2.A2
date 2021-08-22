
/// <reference path="../../define.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.Boolean', {

    extend: 'Mini2.ui.grid.column.Colunm',

    trueText: 'true',

    falseText: 'false',

    undefinedText: '&#160;',

    // private
    renderer: function (value, metaData,cellValues, record, rowIdx, colIdx, store) {
        var me = this;

        if (value === undefined) {
            return me.undefinedText;
        }

        if (!value || value === 'false') {
            return me.falseText;
        }

        return me.trueText;
    }

});
