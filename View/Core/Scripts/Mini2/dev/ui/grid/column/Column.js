/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../../Mini.js" />


Mini2.define('Mini2.ui.grid.column.StyleAction', {

    /**
     * 原始值
     */
    srcValue: null,

    /**
     * 值
     */
    value: null,

    /**
     * 样式对象
     */
    style: null,

    /**
     * 样式集合
     */
    opClassList: null,    //操作样式的列表



    onInit: function () {

    },

    addClass: function (className) {
        "use strict";
        var me = this;

        var op = {
            action: 'add',
            className: className
        };

        if (me.opClassList) { me.me.opClassList = []; }

        me.opClassList.push(op);
    },

    removeClass: function (className) {
        "use strict";
        var me = this;

        var op = {
            action: 'remove',
            className: className
        };

        if (me.opClassList) { me.me.opClassList = []; }

        me.opClassList.push(op);
    },

    reset: function (value) {
        "use strict";
        var me = this;

        me.srcValue = value;
        me.value = value;
        me.style = {};

        if (me.opClassList) {
            me.opClassList = [];
        }
    },

    /**
     * 获取渲染结果后的值
     */
    getHtml: function (actT) {
        "use strict";
        var me = this,
            value = me.value;

        if (value != me.srcValue) {
            actT.isAct = true;
            return (actT.text || value);
        }



        if (actT.formatFun) {

            try {
                value = actT.formatFun(value, actT.formatStr);
            }
            catch (ex) {
                console.error('格式化错误:Mini2.util.Format.number = ' + value + ', format=' + actT.formatStr);
            }

            actT.isAct = true;
        }

        var hasStyle = false;

        for (var i in me.style) {
            hasStyle = true;
            break;
        }

        if (hasStyle && value != null) {

            actT.isAct = true;

            var spanEl = $('<span>' + (actT.text || value) + '</span>');

            if (hasStyle) {

                spanEl.css(me.style);
            }


            var tmpEl = $('<div></div>');
            tmpEl.append(spanEl);

            var html = tmpEl.html();

            return html;
        }
        else {
            return (actT.text || value);
        }
    }

}, function () {

    var me = this;
    me.style = {};
});

Mini2.define('Mini2.ui.grid.column.Colunm', {

    extend: 'Mini2.ui.Component',

    //列默认宽度。单位是 px
    width: 120,

    /**
    * 录入的时候,自动去除左右两边的空格
    */
    autoTrim: true,

    headerWidth: 120,

    handleWidth: 4,

    headerAlign: 'left',

    noWrap: true,

    emptyCellText: '&#160;',

    //数据类型
    miType: 'col',

    //数据。字段名或属性名称
    dataIndex: '',

    //标题
    headerText: '&#160',

    //显示的格式
    renderer: false,

    //编辑器
    editor: false,

    //自动锁定
    autoLock: false,

    //可改变大小
    resizable: true,

    //是否为自定义
    hasCustomRenderer: false,

    lockedHeader: false,

    rowspan: undefined,

    menuDisabled: false,

    //`'left'`, `'center'`, and `'right'`.
    align: 'left',

    isHeader: true,

    sortAsc: false,
    sortDesc: false,

    //排序开关
    sortable: true,

    /**
     * 样式规则集合
     * Array 对象.
     */
    styleRules: null,

    //排序的字段
    //sortExpression


    //direction: 'ASC',

    //水印
    //placeholder:false,

    tdCls: '',
    innerCls: '',

    /**
    * 设置字段只读模式
    */
    setReadOnly:function(value){

        var me = this;

        me.readOnly = !!value;

        console.debug('设置只读: ', me);
        console.debug('设置只读    : ', me.readOnly);

        return me;
    },


    renderTpl: [
        '<td role="gridcell" class="mi-grid-cell mi-grid-td ">',
            '<div class="mi-grid-cell-inner">',
            '</div>',
        '</td>'
    ],


    initComponent: function () {
        "use strict";
        var me = this,
            renderer,
            listeners;

        renderer = me.renderer;

        if (renderer) {

            if (typeof renderer == 'string') {
                //me.renderer = Ext.util.Format[renderer];
            }
            me.hasCustomRenderer = true;
        }
        else if (me.defaultRenderer) {
            me.scope = me;
            me.renderer = me.defaultRenderer;
        }

        if (me.clientId) {
            window[me.clientId] = me;
        }

        me.initStyleRule();

        try {
            var sec = Mini2.ui.SecManager;
            sec.reg(me.secFunCode, me,'visible');
            sec.reg(me.secReadonly, me,'readonly');
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }
    },

    /**
    * 设置组件可视状态
    * @param {bool} value 
    */
    setVisible: function (value) {
        "use strict";
        var me = this;
        value = !!value;

        if (value !== me.visible) {

            me.visible = value;

            if (value) {
                me.show();
            }
            else {
                me.hide();
            }
        }

        return me;
    },


    /**
     * 缓冲 Style action 对象
     */
    bufferStyleAction: null,


    /**
     * 初始化样式规则
     */
    initStyleRule: function () {
        "use strict";
        var me = this,
            i,
            rule,
            rules = me.styleRules;

        if (!rules || rules.length == 0) {
            return;
        }

        console.debug("动态编译样式规则...");

        for (i = 0; i < rules.length; i++) {

            rule = rules[i];

            if (rule.exec) {
                continue;
            }

            if ('script' == rule.role) {
                rule.exec = eval('(function(T){ ' + rule.script + '})');

                console.debug('动态编译为脚本函数: ', rule.exec);
            }



        }

    },

    /**
     * 执行样式规则
     *
     */
    execStyleRule: function (actT) {
        "use strict";
        var me = this,
            i,
            rule,
            rules = me.styleRules;

        if (!rules || rules.length == 0) {

            return actT.text || actT.value;
        }

        //动作对象

        var styleAction = me.bufferStyleAction;

        if (!styleAction) {
            me.bufferStyleAction = styleAction = Mini2.create('Mini2.ui.grid.column.StyleAction');
        }

        styleAction.reset(actT.value);




        for (i = 0; i < rules.length; i++) {

            rule = rules[i];

            if ('script' == rule.role) {

                rule.exec(styleAction);

            }
        }



        var htmlValue = styleAction.getHtml(actT);


        return htmlValue;
    },




    hasLockedHeader: function () {
        return this.lockedHeader;
    },

    defaultRenderer: function (value) {

        return value;
    },


    /*
    * 缓冲数据
    */
    //buffer:{
    //    cellEl: null
    //},


    getBufferCellEl: function () {
        "use strict";
        var me = this,
            cellEl,
            buffer = me.buffer;

        if (!buffer) {
            me.buffer = buffer = {};
        }

        cellEl = buffer.cellEl;

        return cellEl;
    },

    setBufferCellEl: function (cellEl) {
        "use strict";
        var me = this,
            cellEl,
            buffer = me.buffer;

        if (!buffer) {
            me.buffer = buffer = {};
        }

        buffer.cellEl = cellEl;

        return me;
    },


    /**
    * 触发点击事件
    *
    * 
    */
    triggerClick: function () {
        var me = this,
            el = me.el,
            innerEl = el.children('.mi-column-header-inner'),
            triggerEl = innerEl.children('.mi-column-header-trigger'),

            menu = me.menu,
            menuInst = me.menuInst;


        //console.log("triggerEl = ", triggerEl[0]);

        var offset = $(triggerEl).offset();

        var x = offset.left;
        var y = offset.top + triggerEl.height();

        //如果菜单不存在, 创建默认菜单
        if (!menu) {

            //menu = {};

            //menu.items = [{

            //    text: '全选',

            //    click: function () {
            //        var me = this,
            //            owner = me.owner;

            //        //console.log("cccc ", owner);
            //    }

            //},{
            //    text :'反选'  
            //},{
            //    text:'-'
            //}, {
            //    text: '设置...'
            //}];

        }

        if (menu) {

            if (!menuInst) {

                //delete me.menu;

                me.menuInst = menuInst = Mini2.create('Mini2.ui.menu.Menu', {
                    left: x,
                    top: y,

                    owner: me,
                    items: menu
                });

                menuInst.bind('showed', function () {
                    innerEl.addClass('mi-column-header-open');
                });

                menuInst.bind('hidded', function () {
                    innerEl.removeClass('mi-column-header-open');
                });

                menuInst.render();

            }
            else {
                menuInst.setLocal(x, y).show();
            }
        }

    },


    renderCell: function () {
        "use strict";
        var me = this,
            cellEl;

        cellEl = me.getBufferCellEl();


        if (!cellEl) {

            cellEl = Mini2.$joinStr(me.renderTpl);

            cellEl.children('.mi-grid-cell-inner').css('text-align', me.align);

            me.setBufferCellEl(cellEl);

        }

        cellEl = cellEl.clone();

        return cellEl;
    },

    getStore: function () {
        var me = this,
            displayStore = me.displayStore;

        return displayStore;
    },

    renderer: function (value, fieldValue, cellValues, record, recordIndex, columnIndex, store, table) {
        "use strict";
        var me = this,
            colRecord,
            text,
            editor = me.editor,
            editorMode = me.editorMode,
            colStore = me.displayStore,
            displayField = me.displayField,
            itemValueField = me.itemValueField || 'value',
            itemDisplayField = me.itemDisplayField || 'text';

        //log.debug(columnIndex + ' = ' + value);


        if (displayField) {

            try {
                text = record.get(displayField);
            }
            catch (ex) {
                throw new Error('获取字段“' + displayField + '”的值错误。');
            }


            // 执行样式规则
            var actT = {
                value: value,
                text: text,
                record: record,
                dataIndex: me.dataIndex,
                formatFun: null,
                format: null,
                isAct: false
            };


            text = me.execStyleRule(actT);

            if (!Mini2.isBlank(text)) {
                return text;
            }
        }


        //log.debug('     最后  【' + itemDisplayField + '】' + ' me.editorMode = ' + me.editorMode);

        // 注: colStore 一般情况下,是下拉框的'项目集合'
        if (colStore && ('none' == editorMode || (editor && 'none' == editor.mode))) {

            //record = store.getById(value);

            //alert('me.valueField = ' + me.valueField);

            colRecord = colStore.filterFirstBy(itemValueField, value);

            //            alert(record);

            if (colRecord) {
                text = colRecord.get(displayField || itemDisplayField);

                // 执行样式规则
                var actT = {
                    value: value,
                    text: text,
                    record: record,
                    dataIndex: me.dataIndex,
                    formatFun: null,
                    format: null,
                    isAct: false
                };


                text = me.execStyleRule(actT);

                return text;
            }
        }

        // 执行样式规则
        var actT = {
            value: value,
            record: record,
            dataIndex: me.dataIndex,
            formatFun: null,
            format: null,
            isAct: false
        };


        value = me.execStyleRule(actT);


        //增加水印效果
        if (me.placeholder && Mini2.isBlank(value)) {
            value = '<span style="color:#c0c0c0;">' + me.placeholder + '</span>';
        }

        return value;
    },



    //清理异常提示
    //record = 记录
    //cellEl = 单元格
    //dataIndex = 字段名
    clearInvalid: function (record, cellEl, dataIndex) {
        "use strict";
        var me = this;

        if ($(cellEl).hasClass('mi-grid-cell-invalid')) {
            //构造错误提示信息
            var errorEl = $(cellEl).children('.mi-grid-cell-invalid-icon');

            if (errorEl.length) {
                $(errorEl).remove();
            }

            $(cellEl).removeClass('mi-grid-cell-invalid');
        }

    },


    //创建异常提示
    //record = 记录
    //cellEl = 单元格
    //dataIndex = 字段名
    markInvalid: function (record, cellEl, dataIndex, invalidMessage) {
        "use strict";
        var me = this,
            errors = record.errors;

        if (invalidMessage) {

            errors = invalidMessage;

            if (errors) {
                error = errors[0];   //暂时只取第一个
            }

            //构造错误提示信息
            var errorEl = $(cellEl).children('.mi-grid-cell-invalid-icon');
            if (errorEl.length) {
                $(errorEl).remove();
            }

            var errorType = error.type.toLowerCase();

            errorEl = Mini2.$join([
                    '<div role="presentation" class="mi-form-',
                        errorType,
                        '-msg mi-form-invalid-icon mi-grid-cell-invalid-icon" >',
                    '</div>']);


            errorEl.attr('title', error.message);
            errorEl.attr('data-errorqtip', '');

            $(cellEl).addClass('mi-grid-cell-invalid');
            $(cellEl).append(errorEl);

        }
        else if (errors && errors.length) {

            var error = record.getErrors(dataIndex);

            if (error) {
                error = error[0];   //暂时只取第一个
            }

            if (error) {

                //构造错误提示信息
                var errorEl = $(cellEl).children('.mi-grid-cell-invalid-icon');
                if (errorEl.length) {
                    $(errorEl).remove();
                }

                var errorType = error.type.toLowerCase();

                errorEl = Mini2.$join([
                    '<div role="presentation" class="mi-form-',
                        errorType,
                        '-msg mi-form-invalid-icon mi-grid-cell-invalid-icon" >',
                    '</div>']);


                errorEl.attr('title', error.message);
                errorEl.attr('data-errorqtip', '');

                $(cellEl).addClass('mi-grid-cell-invalid');
                $(cellEl).append(errorEl);
            }
        }

        //return errorEl;
    },

    getSortParam: function () {
        return this.dataIndex;
    },



    doSort: function (state) {
        "use strict";
        var me = this,
            ORDER_ASC = 'ASC',
            ORDER_DESC = 'DESC',
            sortText = '',
            direction = me.direction,
            sortAsc = me.sortAsc,
            sortDesc = me.sortDesc,
            sortExpression = me.sortExpression,
            store = me.grid.getStore();


        if (!state) {
            state = me.direction = (ORDER_ASC == direction ? ORDER_DESC : ORDER_ASC);
        }
        else {
            state = me.direction = state.toUpperCase();
        }


        if (ORDER_ASC == state && sortAsc) {
            sortText = sortAsc;
        }
        else if (ORDER_DESC == state && sortDesc) {
            sortText = sortDesc;
        }
        else if (sortExpression) {
            sortText = sortExpression + " " + state;
        }
        else {
            sortText = me.dataIndex + " " + state;
        }

        store.sort(sortText);
    }

});
