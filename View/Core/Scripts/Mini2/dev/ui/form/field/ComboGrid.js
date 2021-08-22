/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.define('Mini2.ui.form.field.ComboGrid', {

    extend: 'Mini2.ui.form.field.Trigger',
    
    
    alias: 'widget.combogrid',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.ComboGrid', true),

    dropDownPanel :null,

    //表格对象
    grid: null,

    //表格的数据仓库
    store : null,

    dropDownWidth: 500,
    dropDownHeight: 400,

    //绑定值的字段名
    valueField: 'value',

    /**
     * 配合表格, 动态增加行的.
     */
    delayLoad: true,

    //初始化组件
    initComponent: function () {

        var me = this;


        me.fieldCls = 'mi-form-type-text';

        $(me).muBind('focusout', function () {

            me.fieldEl.removeClass('mi-form-focus');
            //me.fieldEl.removeClass('mi-form-invalid-field');

            me.el.removeClass("mi-form-trigger-wrap-focus");

            me.hideDropDownPanel();

            me.clearInvalid();
        });

        me.initStore();

        me.initLabelable();
    },


    beforeShowDropDownPanel: function () {
        /// <summary>开始显示之前的</summary>
        var me = this;

        if (!me.isCustom && !me.content ) {
            me.content = me.initGrid();
        }

        if (me.content) {
            me.initContentPanel();
        }

        if (!me.isCustom) {
            me.init_DropDwonPanel();
            var panel = me.dropDownPanel;

            var boundList = me.getBoundPanel();
            $(panel).children().remove();
            $(panel).append(boundList);

        }
    },


    store_currentChanged: function (event, index, record) {
        "use strict";
        var me = event.data,
            focusedCls = 'mi-grid-row-focused',
            row,
            rowRecord,
            rows = $(me.tbodyEl).children(),
            lastRow = me.lastRowFocused,
            curRow = null;

        if (index > -1) {

            var value = record.get(me.valueField);

            me.setValue(value);

            if (me.ownerParent && me.ownerParent.store) {

                var ownerStore = me.ownerParent.store;
                var ownerRect = ownerStore.current;

                $(me.mapItems).each(function () {

                    var src = this.srcField;
                    var target = this.targetField;

                    var srcValue = record.get(src);


                    ownerRect.set(target, srcValue);

                });
            }


        }

    },

    initGrid :function(){
        
        var me = this,
            bodyEl = Mini2.getBody(),
            panelEl,
            delayLoad = me.delayLoad;

        panelEl = $('<div class="mi-boundlist mi-box-target" style="display:none;"></div>');
        panelEl.css({
            width: me.dropDownWidth,
            height: me.dropDownHeight
        });

        $(bodyEl).append(panelEl);

        if (!me.store) {
            var store = Mini2.create('Mini2.data.Store', {

                pageSize: 0,
                totalCount: 0,
                jsonMode: 'sample',
                //idField: 'IG2_TABLE_ID',
                model: 'Mini2.data.FreeModel',

                fields: me.fields,
                data: me.data

            });

            me.store = store;
        }

        $(me.store).muBind('currentchanged', me, me.store_currentChanged);  //焦点行发生改变的事件


        if (undefined == delayLoad || null == delayLoad) {
            delayLoad = true;
        }

        var grid = Mini2.create('Mini2.ui.panel.Table', {

            scope: me,
            delayLoad: delayLoad,
            renderTo: panelEl,
            store: me.store,
            checkedMode: 'SINGLE',
            focusBorders:false,
            scroll: 'auto',
            columnLines: false,
            dock: 'full',
            selType: 'checkBoxModel',
            readOnly: true,
            autoRowCheck:true,
            columns: me.columns
        });

        grid.render();
                
        grid.setSize(panelEl.width(), panelEl.height());

        
        panelEl.muBind('mousedown', function (e) {

            Mini2.EventManager.stopEvent(e);

        });

        return panelEl;

    }



});