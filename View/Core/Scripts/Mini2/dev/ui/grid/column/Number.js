
/// <reference path="../../Mini2.js" />
/// <reference path="../../lang/Number.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.Number', {

    extend: 'Mini2.ui.grid.column.Colunm',

    format: '0',        //0,000.00

    align: 'right',

    defaultValue: 0,

    defaultRenderer: function (value) {
        return Mini2.util.Format.number(value, this.format);
    },

    // private
    //value, fieldValue, cellValues, record, recordIndex, columnIndex, store, table
    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store) {
        "use strict";
        var me = this,
            result = value,
            format = me.format,
            notDisplayValue = me.notDisplayValue,
            numFormat = Mini2.util.Format.number;
        
        if (notDisplayValue && value == notDisplayValue) {
            return '';
        }

        // 执行样式规则
        var actT = {
            value: value,
            record: record,
            dataIndex: me.dataIndex,
            formatFun: numFormat,
            format: format,
            isAct : false
        };

        try{
            result = me.execStyleRule(actT);
        }
        catch (ex) {
            console.error('渲染"样式规则"错误',ex);
        }

        if (!actT.isAct) {

            try {
                result = numFormat(value, format);
            }
            catch (ex) {
                alert('格式化错误:Mini2.util.Format.number = ' + value + ', format=' + format);
            }
        }

        return result;
    }


});
