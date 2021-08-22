
/// <reference path="../../define.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.Date', {

    extend: 'Mini2.ui.grid.column.Colunm',

    format: 'Y-m-d',



    defaultRenderer: function (value) {
        "use strict";
        //return Mini2.util.Format.number(value, this.format);

        var me = this,
            valueStr;


        if (value && !Mini2.isDate(value)) {
            value = Mini2.Date.parse(value, me.format);

        }


        valueStr = Mini2.Date.format(value, me.format);

        return valueStr; ;
    },

    // private
    renderer: function (value, metaData,cellValues, record, rowIdx, colIdx, store) {
        "use strict";
        var me = this,
            valueStr,
            srcValue = value;

        

        if (value == null) { return value; }
        
        var isDate = Mini2.isDate(value);

        if (value && !isDate) {

            
            if (value.length <= 10) {
                value += " 0:0:0";
            }


            if (($.support && $.support.mozilla) || Mini2.support.msie || Mini2.support.mozilla) {

                //火狐浏览器特殊处理

                var sssIndex = value.lastIndexOf("."),
                    dtStr, sssStr, dt;

                if (sssIndex > 0) {
                    dtStr = value.substring(0, sssIndex);   //获取年月日时分秒
                    sssStr= value.substr(sssIndex + 1); //获取毫秒

                    dtStr = dtStr.replace(/-/g, "/");

                    dt = new Date(dtStr);

                    try {
                        dt.setMilliseconds(sssStr);
                    }
                    catch (ex) {
                        console.log("错误啦...." + ex.message);
                    }

                    value = dt;
                }
                else {
                    value = new Date(value);
                }

            }
            else {
                value = new Date(value);
            }
        }
                
        valueStr = Mini2.Date.format(value, me.format);
        
        return valueStr; ;
    }

});
