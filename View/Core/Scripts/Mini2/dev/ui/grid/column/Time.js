
/// <reference path="../../define.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.Time', {

    extend: 'Mini2.ui.grid.column.Colunm',

    format: 'H:m',



    defaultRenderer: function (value) {
        "use strict";
        //return Mini2.util.Format.number(value, this.format);

        var me = this,
            valueStr;


        if (value && !Mini2.isDate(value)) {
            value = Mini2.Date.parse(value, me.format);

        }


        valueStr = Mini2.Date.format(value, me.format);

        return valueStr;;
    },

    getTimeObj: function (value) {
        "use strict";
        var me = this;

        if (Mini2.isNumber(value)) {

            var m1 = value % 3600;

            var sec = m1 % 60;

            var hour = (value - m1) / 3600;
            var min = (m1 - sec) / 60;

            return {
                hour: hour,
                minute: min,
                sec: sec
            };
        }
        else {

        }

    },


    // private
    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store) {
        "use strict";
        var me = this,
            valueStr,
            srcValue = value;

        var timeObj = me.getTimeObj(value);

        var valueStr = timeObj.hour + ":" + timeObj.minute


        return valueStr;;
    }

});
