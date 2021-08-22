/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../../Mini.js" />


Mini2.define('Mini2.ui.form.field.NumRange', {

    extend: 'Mini2.ui.form.field.Base',

    alias: 'widget.numrange',

    isRangeControl: true,

    startItem: null,

    endItem: null,

    startCellEl: null,

    endCellEl: null,

    startValue: '',

    endValue: '',

    allowBlank: true,

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

        me.startItem = Mini2.create('Mini2.ui.form.field.Number', {
            renderTo: startCellEl,
            hideLabel: true,
            clientId: me.clientId + '_Start',
            name: me.name + '_Start',
            allowBlank: me.allowBlank,
            value: me.startValue
        });


        me.endItem = Mini2.create('Mini2.ui.form.field.Number', {
            renderTo: endCellEl,
            hideLabel: true,
            clientId: me.clientId + '_End',
            name: me.name + '_End',
            allowBlank: me.allowBlank,
            value: me.endValue
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


    setValue: function (value) {
        var me = this;

        return me;
    },

    getValue: function () {

    },

    focus: function () {
        "use strict";
        var me = this,
            fieldEl = me.fieldEl,
            inputEl = me.startItem;

        Mini2.ui.FocusMgr.setControl(me.scope || me);

        //if (fieldEl) {
        //    fieldEl.addCls("mi-form-focus", me.focusCls);
        //}

        //me.el.addClass("mi-form-trigger-wrap-focus");


        //特殊处理，延时获取焦点。不然无法及时得到焦点。
        if (!me.readOnly && inputEl && inputEl.focus) {
            setTimeout(function () {
                inputEl.focus();
            }, 200);
        }
        return me;
    },

    render: function () {
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


        me.el = tableEl;

        tableEl.data('me', me);
    }



});