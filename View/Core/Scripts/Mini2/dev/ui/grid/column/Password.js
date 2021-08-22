
/// <reference path="../../Mini2.js" />
/// <reference path="../../lang/Number.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.Password', {

    extend: 'Mini2.ui.grid.column.Colunm',


    passwordChar: false,


    defaultRenderer: function (value) {

        "use strict";
        var me = this,
            pwChar = me.passwordChar,
            result = value,
            notDisplayValue = me.notDisplayValue;

        if (Mini2.isBlank(value) || (notDisplayValue && value == notDisplayValue)) {
            return '';
        }

        try {
            result = Mini2.String.rightPad('', value.length, pwChar || '*');
        }
        catch (ex) {
            console.error('格式化错误:  ' + value,ex );
        }

        return result;

    },



    // private
    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store) {
        return this.defaultRenderer(value);
    }


});
