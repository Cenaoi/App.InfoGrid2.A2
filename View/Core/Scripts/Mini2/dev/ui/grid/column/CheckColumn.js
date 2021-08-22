
/// <reference path="../../define.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.CheckColumn', {

    extend: 'Mini2.ui.grid.column.Colunm',



    /**
    * true 值显示的文本内容
    */
    trueText: null,

    /**
    * false 值显示的文本内容
    */
    falseText : null,

    /*
    * 设置复选框的状态
    *
    * @parma {bool|'reverse'} checkMode 复选框模式
    */
    setCheckCell:function(col,checkMode){
        var me = this,
            //col = me.owner,
            store = col.store,
            dataIndex = col.dataIndex;

        store.each(function (record, i, len) {
            if ('reverse' == checkMode) {
                var srcValue = !!record.get(dataIndex);
                record.set(dataIndex, !srcValue);
            }
            else {
                record.set(dataIndex, checkMode);
            }
        });

        return me;
    },

    /**
    * 菜单
    */
    menu: [{
        text: '全选',

        click: function () {
            var me = this,
                col = me.owner;
            col.setCheckCell(col, true);
        }

    },{
        text: '反选',
        click: function () {
            var me = this,
                col = me.owner;
            col.setCheckCell(col, 'reverse');
        }
    }, {
        text: '全不选',

        click: function () {
            var me = this,
                col = me.owner;
            col.setCheckCell(col, false);
        }

    }],


    //`'left'`, `'center'`, and `'right'`
    align: 'center',


    renderTpl: [
        '<td role="gridcell" class="mi-grid-cell mi-grid-td mi-grid-cell-checkcolumn">' ,
            '<div class="mi-grid-cell-inner mi-grid-cell-inner-checkcolumn">',

            '</div>',
        '</td>'
    ],


    
    falseIconTpl: '<img class="mi-grid-checkcolumn" src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" style=" vertical-align:middle;">',

    trueIconTpl: '<img class="mi-grid-checkcolumn mi-grid-checkcolumn-checked" src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" style=" vertical-align:middle;" >',




    defaultRenderer: function (value) {
        return value;
    },


    renderer: function (value, meta) {
        var me = this,
            resultHtml;

        if (!value || value === 'false' ||
            (typeof value == 'string' && value.toLowerCase() == 'false')) {

            resultHtml = me.falseIconTpl;

            if (me.falseText) {
                resultHtml += '<span style="vertical-align:middle;margin-left: 4px;">'+ me.falseText +'</span>';
            }
        }
        else {
            resultHtml = me.trueIconTpl;

            if (me.trueText) {
                resultHtml += '<span style="vertical-align:middle;margin-left: 4px;">' + me.trueText + '</span>';
            }
        }

        return resultHtml;

    }

});



