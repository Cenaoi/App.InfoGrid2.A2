

/**
 * 下拉树控件
 *
 */
Mini2.define('Mini2.ui.form.field.ComboTree', {

    extend: 'Mini2.ui.form.field.Trigger',


    alias: 'widget.combotree',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.ComboTree', true),

    dropDownPanel: null,

    //表格对象
    grid: null,

    //表格的数据仓库
    store: null,

    dropDownWidth: 200,
    dropDownHeight: 300,

    //绑定值的字段名
    valueField: 'value',


    //初始化组件
    initComponent: function () {
        "use strict";
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


    /**
     * 调用远程数据
     *
     */
    callRemoteData:function(){
        "use strict";
        var me = this,
            store = me.store;

        console.debug('开始加载远程数据...', me.remoteUrl);

        var remotePs = {
            parent_id: me.rootValue
        };

        Mini2.post(me.remoteUrl, remotePs, function (data) {

            console.debug('数据回来了...', data);

            store.add(data);

        });



    },

    /**
     * 开始显示之前的
     *
     */
    beforeShowDropDownPanel: function () {
        "use strict";
        var me = this;

        if (!me.isCustom && !me.content) {
            me.content = me.initTree();


            //开始加载远程数据

            if (me.remoteEnabled) {

                me.callRemoteData();

                
            }

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


    /**
     * 数据仓库焦点行发生变化
     *
     */
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

            var field = me.valueField,
                value;

            if (record.get) {
                value = record.get(field);
            }
            else {
                value = record[field]
            }

            me.setValue(value);


        }

    },

    /**
     * 初始化树控件
     *
     */
    initTree: function () {
        "use strict";
        var me = this,
            bodyEl = Mini2.getBody(),
            store = me.store,
            panelEl;

        panelEl = $('<div class="mi-boundlist mi-box-target" style="display:none;"></div>');
        panelEl.css({
            width: me.dropDownWidth,
            height: me.dropDownHeight
        });

        $(bodyEl).append(panelEl);

        if (!store) {
            store = Mini2.create('Mini2.data.TreeStore', {

                id : 'store' + Mini2.newId(),

                remoteEnabled: me.remoteEnabled,
                remoteUrl: me.remoteUrl,

                pageSize: 0,
                totalCount: 0,
                jsonMode: 'sample',
                //idField: 'IG2_TABLE_ID',
                model: 'Mini2.data.FreeModel',

                idField: me.idField,
                parentField: me.parField,
                textField: me.displayField,
                rootValue: me.rootValue,
                rootText: me.rootText,

                sortText: me.sortText,
                

                fields: me.fields,
                data: me.data

            });

            me.store = store;
        }

        $(store).muBind('currentchanged', me, me.store_currentChanged);  //焦点行发生改变的事件

        var tree = Mini2.create('Mini2.ui.tree.Panel', {
            id: 'tree' + Mini2.newId() ,

            renderTo: panelEl,

            store: store,

            remoteEnabled: me.remoteEnabled,
            remoteUrl : me.remoteUrl,

            scroll: 'auto',
            dock: 'full',
            region: 'north',

            rootId: me.rootValue,

            idField: me.idField,
            parField: me.parField,
            valueField: me.valueField,
            textField: me.displayField,

            triggerEvent_selected: false,
            triggerEvent_create: false,
            triggerEvent_remove: false,
            triggerEvent_rename: false,

            types: {
                'default': { icon: '/res/icon/application_view_columns.png' },
                table: { icon: '/res/icon/table.png' },
                view: { icon: '/res/icon/view.png' }
            },
            contextMenu: [],
            event_selected: false,
            event_move: false,

            isTree: true
        });
        tree.render();


        tree.setSize(panelEl.width(), panelEl.height());


        panelEl.muBind('mousedown', function (e) {

            Mini2.EventManager.stopEvent(e);

        });

        return panelEl;

    },


});