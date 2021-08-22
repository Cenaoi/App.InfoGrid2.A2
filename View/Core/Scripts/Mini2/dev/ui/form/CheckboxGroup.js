/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />
/// <reference path="../../Mini-more.js" />


Mini2.define('Mini2.ui.form.CheckboxGroup', {

    extend: 'Mini2.ui.form.field.Base',

    alias: 'widget.checkboxgroup',

    log: Mini2.Logger.getLogger('Mini2.ui.form.CheckboxGroup'),

    mixins: {
        labelable: 'Mini2.ui.form.Labelable'
    },

    /**
    * 关键标记
    */
    isCheckGroup: true,

    labeldable: null,


    columns: 'auto',

    repeatColumns: 0,
    repeatDirection: 'vertical',
    repeatLayout: 'table',

    vertical: false,

    blankText: "You must select at least one item in this group",

    defaultType: 'checkboxfield',

    defaultItemType: 'Mini2.ui.form.field.CheckBox',


    groupCls: Mini2.baseCSSPrefix + 'form-check-group',

    // private
    extraFieldBodyCls: Mini2.baseCSSPrefix + 'form-checkboxgroup-body',

    // private
    layout: 'checkboxgroup',

    componentCls: Mini2.baseCSSPrefix + 'form-checkboxgroup',


    /**
    * @type {Array}
    */
    items: null,


    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this,
            i,
            items = me.items,
            len = items.length,
            itemCfg,
            elem;


        $(me).muBind('focusout', function () {

            me.fieldEl.removeClass("mi-form-focus");
            me.el.removeClass("mi-form-trigger-wrap-focus");

            me.on('hided');

            //me.isValid();
        });

        me.initLabelable();



        for (i = 0; i < len; i++) {
            itemCfg = Mini2.apply({
                hideLabel: true,
                name: me.name,
                width: ''
            }, items[i]);



            elem = Mini2.create(me.defaultItemType, itemCfg);

            items[i] = elem;
        }

    },


    /**
     * 清空所有 items 
     */
    clearItems: function () {

        "use strict";

        var me = this,
            layouyt = me.repeatLayout,
            items = me.items;

        for (var i = 0; i < items.length; i++) {
            var itemEl = items[i].el;

            $(itemEl).remove();
        }

        me.items = [];

        return me;
    },


    insert :function(items){

        throw new Error('未完成此函数...');

        var me = this;

        if (!Mini2.isArray(items)) {
            items = [items];
        }


        for (var i = 0; i < items.length; i++) {

            var item = items[i];


            var itemCfg = Mini2.apply({
                hideLabel: true,
                name: me.name,
                width: ''
            }, item);



            var elem = Mini2.create(me.defaultItemType, itemCfg);

            me.items.push(elem);

        }

    },

    /**
     * 重置新的 items 
     */
    resetItems: function (items) {
        "use strict";
        var me = this,
            el = me.el, i, j,
            fieldEl = me.fieldEl,
            item,
            itemEl,
            cellEl,
            cols = me.columns,
            layout = me.repeatLayout;
            
        me.clearItems();
        


        for (i = 0; i < items.length; i++) {
            var itemCfg = Mini2.apply({
                hideLabel: true,
                name: me.name,
                width: ''
            }, items[i]);



            var elem = Mini2.create(me.defaultItemType, itemCfg);

            items[i] = elem;
        }

        me.items = items;


        console.debug("layout = ", me.repeatLayout);

        if ('table' == me.repeatLayout) {
            

            fieldEl.find('tbody:first').children().remove();

            var tableEl = fieldEl.children('table');

            var groupTableEl = me.getGroup_TableCell(tableEl, cols, items);


            var row = groupTableEl.row;
            var col = groupTableEl.col;

            var tableEl = groupTableEl.el;//.find('table:first');
            var itemIndex = 0;


            for (i = 0; i < row; i++) {

                for (j = 0; j < col; j++) {
                    cellEl = me.getCell(tableEl, i, j);

                    item = items[itemIndex++];

                    item.renderTo = cellEl;
                    item.render();

                    item.el.css('margin', '0px 15px 0px 0px');

                    if (itemIndex >= items.length) {
                        break;
                    }
                }
            }

        }
        else if ('flow' == me.repeatLayout) {

            var groupTableEl = me.getGroup_Flow();
            groupTableEl = groupTableEl.el;

            var itemWidth = 0;

            if (me.repeatColumns > 0) {
                itemWidth = parseInt(100 / me.repeatColumns);
            }

            for (i = 0; i < items.length; i++) {

                item = items[i];
                item.renderTo = groupTableEl;
                item.render();

                itemEl = item.el;

                //itemEl.css('margin', '0px 15px 0px 0px');

                var itemBodyEl = $(itemEl).find('td.mi-form-item-body:first');

                itemBodyEl.css('padding-right', '15px');


                if ('vertical' == me.repeatDirection) {
                    itemEl.css('float', 'left');
                }

                if (itemWidth > 0) {
                    itemEl.css('width', itemWidth + '%');
                }
            }

        }

        console.debug("----------------------");

        me.bindItemAll(items);
    },

    initValue: function () {
        "use strict";
        var me = this,
            valueCfg = me.value;

        me.originalValue = me.lastValue = valueCfg || me.getValue();

        me.setValue(valueCfg, false);

    },



    getValue: function () {
        "use strict";
        var me = this,
            log = me.log,
            item,
            items = me.items,
            len = items.length,
            checked,
            resultValueStr = '',
            n = 0,
            i;

        for (i = 0; i < len; i++) {

            item = items[i];

            checked = item.checked;

            if (checked) {

                if (n++ > 0) { resultValueStr += ', '; }

                resultValueStr += item.inputValue;
            }
        }

        return resultValueStr;

    },

    setValue: function (value, triggerChangedEvent) {
        "use strict";
        var me = this,
            log = me.log,
            item,
            items = me.items,
            len = items.length,
            isEqual,
            i, j,
            valueList,
            valueDict;

        if (me.locked) {
            console.debug('上锁..');
            return me;
        }

        if (Mini2.isBlank(value)) {
            value = '';
        }

        if (Mini2.isArray(value)) {
            valueList = value;
        }
        else if(Mini2.isString(value)) {
            valueList = value.split(',');
        }

        valueDict = {};

        for (j = 0; j < valueList.length; j++) {
            var v = valueList[j];

            if (Mini2.isString(v)) {
                v = Mini2.String.trim(v);
            }
            else {
                v = v + '';
            }
            valueList[j] = v;
            valueDict[v] = true;
        }

        for (i = 0; i < len; i++) {

            item = items[i];

            isEqual = !!valueDict[item.inputValue];


            item.setValue(isEqual, triggerChangedEvent);

        }

        return me;

    },

    setHeight: function (value) {

    },

    createGroupCell: function (tableEl, row, col) {
        "use strict";
        var i,
            j,
            tbodyEl,
            trEl;

        tbodyEl = tableEl.find('tbody:first');

        for (i = 0; i < row; i++) {

            trEl = $('<tr></tr>');

            tbodyEl.append(trEl);

            for (j = 0; j < col; j++) {
                //tdEl = $();
                trEl.append('<td class="mi-form-radio-group" valign="top"></td>');
            }
        }
    },



    getGroup_TableCell: function (tableEl, columns, items) {
        "use strict";
        var me = this,
            result,
            col, yuNum, row,
            len = items.length;
            
        if ('auto' == columns) {
            me.createGroupCell(tableEl, 1, len);

            result = { 'el': tableEl, 'row': 1, 'col': len };
        }
        else if (columns <= 1) {
            me.createGroupCell(tableEl, len, 1);

            result = { 'el': tableEl, 'row': len, 'col': 1 };
        }
        else {
            col = parseInt(columns);
            yuNum = len % col;
            row = (len - yuNum) / col;

            if (yuNum > 0) { row++; }

            me.createGroupCell(tableEl, row, col);


            result = { 'el': tableEl, 'row': row, 'col': col };
        }

        return result;
    },

    getGroup_Table: function () {
        "use strict";
        var me = this,
            items = me.items,
            columns = me.repeatColumns,
            
            tableEl,
            result;

        tableEl = Mini2.$join([
            '<div class="mi-checkbox-group-inner" >',
                '<table id="radiogroup-', me.muid, '-innerCt" class="mi-table-plain" cellpadding="0" role="presentation" style="table-layout: auto;">',
                    '<tbody>',
                    '</tbody>',
                '</table>',
            '</div>'
        ]);

        result = me.getGroup_TableCell(tableEl, columns, items);

        return result;
    },

    //获取组的流
    getGroup_Flow: function () {
        "use strict";

        var me = this,
            result,
            columns = me.repeatColumns,
            tableEl;

        tableEl = Mini2.$join([
            '<div>',
            '</div>']);

        result = { 'el': tableEl };

        return result;
    },

    

    getFieldEl: function () {
        "use strict";
        var me = this,
            i, j,
            item,
            itemEl,
            cellEl,
            items = me.items;

        if ('table' == me.repeatLayout) {
            var groupTableEl = me.getGroup_Table();

            var row = groupTableEl.row;
            var col = groupTableEl.col;

            groupTableEl = groupTableEl.el;
            var tableEl = groupTableEl.children('table');
            var itemIndex = 0;


            for (i = 0; i < row; i++) {

                for (j = 0; j < col; j++) {
                    cellEl = me.getCell(tableEl, i, j);

                    item = items[itemIndex++];

                    item.renderTo = cellEl;
                    item.render();

                    item.el.css('margin', '0px 15px 0px 0px');

                    if (itemIndex >= items.length) {
                        break;
                    }
                }
            }

            return groupTableEl;
        }
        else if ('flow' == me.repeatLayout) {

            var groupTableEl = me.getGroup_Flow();
            groupTableEl = groupTableEl.el;

            var itemWidth = 0;

            if (me.repeatColumns > 0) {
                itemWidth = parseInt(100 / me.repeatColumns);
            }

            for (i = 0; i < items.length; i++) {

                item = items[i];
                item.renderTo = groupTableEl;
                item.render();

                itemEl = item.el;

                //itemEl.css('margin', '0px 15px 0px 0px');

                var itemBodyEl = $(itemEl).find('td.mi-form-item-body:first');

                itemBodyEl.css('padding-right', '15px');


                if ('vertical' == me.repeatDirection) {
                    itemEl.css('float', 'left');
                }

                if (itemWidth > 0) {
                    itemEl.css('width', itemWidth + '%');
                }
            }

            return groupTableEl;
        }
    },

    /**
     * 绑定所有控件的事件
     *
     */
    bindItemAll: function (items) {
        var me = this;

        items = items || me.items;

        $(items).each(function () {
            
            me.bindItem(this);
        });

        return me;
    },

    /**
     * 绑定 item 的 changed 事件
     */
    bindItem: function (item) {

        var me = this;

        $(item).muBind('change', function (e) {
            
            console.debug("触发事件了.......");
            console.debug("item2 === ", item.muid);

            var lastData = item.getValue();

            //上锁,防止触发事件导致'值倒灌'.
            me.locked = true;

            try {
                $(me).muTriggerHandler('changed', [{
                    sender: this,    //原焦点控件
                    target: null,        //新获取焦点控件
                    data: lastData      //原附带的数据
                }]);

            }
            catch (ex) {
                console.error('执行 changed 事件错误.', ex);
            }

            me.locked = false;
        });

    },



    baseRender: function () {
        "use strict";
        var me = this,
            log = me.log,
            bodyCls = 'td.mi-form-item-body:first',
            tableEl,
            fieldEl;

        tableEl = me.getTableContainer();


        if (me.minWidth) {
            tableEl.css({ 'min-width': me.minWidth });
        }

        if (me.maxWidth) {
            tableEl.css({ 'max-width': me.maxWidth });
        }



        if (me.applyTo) {
            $(me.applyTo).replaceWith(tableEl);
        }
        else {
            $(me.renderTo).append(tableEl);
        }

        me.fieldBodyEl = tableEl.find(bodyCls);

        fieldEl = me.getFieldEl();

        me.renderForLableable(tableEl);

        me.appendCell(tableEl, fieldEl, bodyCls);

        me.renderForBoxLabel(tableEl);


        me.fieldEl = fieldEl;
        me.el = tableEl;
        //////////////////////


        me.el.addClass(me.componentCls);
        me.fieldBodyEl.addClass(me.extraFieldBodyCls);

        me.initValue();

        me.bindItemAll();

        if (me.dirty) {
            me.setDirty(me.dirty);
        }

        //me.el.css({ 'height': '24px' });
    },


    render: function () {
        var me = this;

        me.baseRender();

        me.el.data('me', me);
    }
});
