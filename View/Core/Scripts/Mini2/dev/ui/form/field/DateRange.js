/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../../Mini.js" />


Mini2.define('Mini2.ui.form.field.DateRange', {

    extend: 'Mini2.ui.form.field.Base',

    alias: 'widget.daterange',
    isRangeControl:true,

    startItem: null,

    endItem: null,

    startCellEl: null,

    endCellEl: null,

    //改写:初始化组件
    initComponent: function () {
        "use strict";
        var me = this;

        me.initLabelable();
    },



    getFieldEl: function () {
        "use strict";
        var me = this,
            tableEl,
            rowEl,
            startCellEl,
            endCellEl;


        tableEl = Mini2.$joinStr([
            '<table class="mi-table-plain"  cellpadding="0" cellspacing="0" border="0" role="presentation" style="table-layout: auto;">',
                '<tbody>',
                    '<tr>',
                        '<td class="mi-item-cell-start"></td>',
                        '<td>&nbsp;-&nbsp;</td>',
                        '<td class="mi-item-cell-end"></td>',
                    '</tr>',
                '</tbody>',
            '</table>']);

        rowEl = tableEl.find('tr:first');

        me.startCellEl = startCellEl = rowEl.children('.mi-item-cell-start');
        me.endCellEl = endCellEl = rowEl.children('.mi-item-cell-end');

        me.startItem = Mini2.create('Mini2.ui.form.field.Date', {
            renderTo: startCellEl,
            hideLabel: true,
            value: me.startValue,
            clientId: me.clientId + '_Start',
            name: me.name + '_Start'
        });


        me.endItem = Mini2.create('Mini2.ui.form.field.Date', {
            renderTo: endCellEl,
            hideLabel: true,
            value:me.endValue,
            clientId: me.clientId + '_End',
            name: me.name + '_End'
        });


        return tableEl;
    },


    setStartValue: function (value) {
        var me = this,
            startItem = me.startItem;

        startItem.setValue(value);

        return me;
    },

    getStartValue: function () {
        var me = this,
            startItem = me.startItem;

        return startItem.getValue();
    },


    setEndValue: function (value) {
        var me = this,
            endItem = me.endItem;

        endItem.setValue(value);

        return me;
    },

    getEndValue: function () {
        var me = this,
            endItem = me.endItem;

        return endItem.getValue();
    },


    setOldValue:function(rangeType, value){
        var me = this;

        if ('start' == rangeType) {
            me.startItem.setOldValue(value);
        }
        else if ('end' == rangeType) {
            me.endItem.setOldValue(value);
        }
        else {
            throw new Error('无效类型, 参数只有 ["start" | "end"] 两个选择. 用户输入: ' + rangeType);
        }

        return me;
    },

    setValue: function (rangeType, value) {
        var me = this;

        if ('start' == rangeType) {
            me.startItem.setValue(value);
        }
        else if ('end' == rangeType) {
            me.endItem.setValue(value);
        }
        else {
            throw new Error('无效类型, 参数只有 ["start" | "end"] 两个选择. 用户输入: ' + rangeType);
        }

        return me;
    },

    getValue: function (rangeType) {
        var me = this;

        if ('start' == rangeType) {
            return me.startItem.getValue();
        }
        else if ('end' == rangeType) {
            return me.endItem.getValue();
        }
        else {
            //throw new Error('无效类型, 参数只有 ["start" | "end"] 两个选择. 用户输入: ' + rangeType);
        }
    },

    /**
    * 设置
    *
    */
    setDirty:function(rangeType, dirty){
        var me = this,
            startItem = me.startItem,
            endItem = me.endItem;

        if ('start' == rangeType) {
            startItem.setDirty(dirty);
        }
        else if ('end' == rangeType) {
            endItem.setDirty(dirty);
        }
        else {
            throw new Error('无效类型, 参数只有 ["start" | "end"] 两个选择. 用户输入: ' + rangeType);
        }

        return me;
    },


    /**
    * 值是否发生变化
    * @param {string} rangeType [ start | end ] 
    */
    isValueChanged:function(rangeType){

        var me = this,
            startItem = me.startItem,
            endItem = me.endItem;

        if (rangeType) {

            if ('start' == rangeType) {
                return startItem.isValueChanged();
            }
            else if ('end' == rangeType) {
                return endItem.isValueChanged();
            }
            else {
                throw new Error('无效类型, 参数只有 ["start" | "end"] 两个选择. 用户输入: ' + rangeType);
            }
        }
        else {
            return (startItem.isValueChanged() || endItem.isValueChanged());
        }


    },


    render: function () {
        "use strict";
        var me = this,
            bodyCls = 'td.mi-form-item-body:first',
            tableEl,
            fieldEl;


        tableEl = me.getTableContainer();

        fieldEl = me.getFieldEl();

        if (me.applyTo) {
            $(me.applyTo).replaceWith(tableEl);
        }
        else {
            $(me.renderTo).append(tableEl);
        }


        me.appendCell(tableEl, fieldEl, bodyCls);

        me.renderForLableable(tableEl);


        me.startItem.render();

        me.endItem.render();


        $(me.startItem).muBind('focusout', function (e) {

            var lastData = me.startItem.getValue();

            $(me).muTriggerHandler('focusout', [{
                sender: this,    //原焦点控件
                target: null,        //新获取焦点控件
                data: lastData      //原附带的数据
            }]);
        });


        $(me.endItem).muBind('focusout', function (e) {

            var lastData = me.endItem.getValue();

            $(me).muTriggerHandler('focusout', [{
                sender: this,    //原焦点控件
                target: null,        //新获取焦点控件
                data: lastData      //原附带的数据
            }]);
        });


        me.el = tableEl;

        tableEl.data('me', me);
    }



});