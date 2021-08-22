/// <reference path="../Mini2.js" />
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.define('Mini2.ui.grid.CellEditor', {


    extend: 'Mini2.ui.Editor',

    log: Mini2.Logger.getLogger('Mini2.ui.grid.CellEditor'),  //日志管理器

    grid: null,

    el: null,
    inputObj: null,

    renderTo: Mini2.getBody(),

    rowEl: null,
    cellEl: null,

    record: null,
    columnHeader: null,

    xtype: 'TextColumn',

    readOnly: false,

    isInput: true,



    //初始化组件
    initComponent: function () {

        var me = this;





    },




    focus: function () {
        "use strict";
        var me = this,
            log = me.log;

        //log.begin("CellEditor.focus()");

        //log.debug("cellEl = " + me.cellEl);

        me.inputObj.focus();

        //Mini2.ui.FocusMgr.setControl(me, me.cellEl, me.el);

        //me.on('foncusin');

        //log.end();

        return me;
    },


    getBox: function () {
        "use strict";
        var boxHtml = Mini2.$joinStr(['<div class="',
            'mi-editor ',
            'mi-small-editor ',
            'mi-grid-editor ',
            'mi-grid-cell-editor ',
            'mi-layer ',
            'mi-editor-default ',
            '" ',
            'style="right: auto;left: 0px;top: -91px;z-index: 19000;position: relative;" ',
        '>',

        '</div>']);

        //'mi-border-box ',


        return boxHtml;
    },

    css: function (name, value) {
        "use strict";
        var me = this,
            el = me.el;

        return el.css(name, value);
    },

    setSize: function (w, h) {
        "use strict";
        var me = this,
            args = arguments,
            sz = args[0],
            inputObj = me.inputObj;

        if (args.length && typeof sz == 'object') {
            inputObj.setWidth(sz.width);
            inputObj.setHeight(sz.height);
        }
        else {
            inputObj.setWidth(w);
            inputObj.setHeight(h);
        }

        return me;
    },

    setWidth: function (value) {

        var me = this;

        me.inputObj.setWidth(value);

        return me.el.css("width", value);
    },

    setHeight: function (value) {

        var me = this;

        me.inputObj.setHeight(value);

        return me.el.css("height", value);
    },


    show: function () {

        var me = this;

        //me.el.addClass("mi-form-trigger-wrap-focus");
        
        me.visible = true;
        me.el.show();

        return me;
    },

    setRowCell: function (rowEl, cellEl) {
        var me = this;

        me.rowEl = rowEl;
        me.cellEl = cellEl;

        return me;
    },

    setRow: function (rowEl) {
        var me = this;

        me.rowEl = rowEl;
        return me;
    },

    setCell: function (cellEl) {

        var me = this;

        me.cellEl = cellEl;
        return me;
    },

    setCellValue: function (value) {

        var me = this,
            log = me.log;

        //var div = $(me.cellEl).children("div.mi-grid-cell-inner:first");
        //var text = $(div).text();

        //log.debug('setCellValue(...) ', value);
        

        me.inputObj.setValue(value);

        return me;
    },

    setValue: function (value) {
        var me = this;

        me.setCellValue(value);

        return me;
    },
    
    isVisible:function(){
        var me = this,
            el = me.el;

        return $(el).is(":visible");
    },

    isHidden: function () {
        var me = this,
            el = me.el;

        return $(el).is(":hidden");
    },

    hide: function () {
        "use strict";
        var me = this,
            cellEl = me.cellEl;

        if (!me.visible) {
            return;
        }

        me.visible = false;
        me.el.hide();


        if (me.editing && me.lastKeyCode != '27') {
            //log.debug("保存单元格数据。hide()");

            me.inputSave(cellEl);

            me.endEdit();
        }

        $(cellEl).cFirst("div.mi-grid-cell-inner").show();

        me.cellEl = null;

        return me;
    },

    layoutReset: function () {
        var me = this;

        me.inputObj.layoutReset();
    },

    inputSave: function (cell, evt) {
        "use strict";
        /// <summary>保存数据</summary>

        var me = this,
            log = me.log,
            inputObj = me.inputObj;

                //log.begin("inputSave() " + me.grid.clientId);

        //        log.debug("me.editing = " + me.editing);

        //        log.debug("inputObj.isValueChanged() = " + inputObj.isValueChanged());

        if (me.editing ) {

            //log.debug("inputObj.isValueChanged = " + "准备");


            if (me.autoTrim) {
                var srcValue = inputObj.getValue();

                if (srcValue && Mini2.isString(srcValue)) {
                    var curValue = Mini2.String.trim(srcValue);

                    if (curValue != srcValue) {
                        inputObj.setValue(curValue);
                    }
                }
            }

            var isValueChanged = (inputObj.isValueChanged ? inputObj.isValueChanged() : true);

            //log.debug("inputObj.isValueChanged = " + isValueChanged);

            if (isValueChanged) {

                var value = me.inputObj.getValue();
                
                var dataIndex = me.columnHeader.dataIndex;

//                if (me.inputObj.cellDisplayField) {
//                    var text = me.inputObj.text;
//                    me.record.set(me.inputObj.cellDisplayField, text);
//                }

                //注释掉，采用其他地方【2015-5-7 11：37】
                var newValues = me.getNewValuesForMap(me.record, inputObj.curRecord);

                if (newValues) {
                    newValues[dataIndex] = newValues[dataIndex] || value;
                }
                else {

                    if (evt && me.mapItems && evt.keydown && 13 === evt.keyCode) {
                        //回车 + 映射 = 不处理
                    }
                    else {
                        newValues = {};
                        newValues[dataIndex] = value;
                    }
                }
                

                if (newValues) {
                    me.record.set(newValues);
                }


                //log.debug("保存数据: " + dataIndex + " = " + value);
            }
        }

        //        log.debug("---------------");
        //        log.end();
    },


    //设置映射值
    getNewValuesForMap: function (record, srcRecrod) {
        "use strict";

        var me = this,
            log = me.log,
            i,
            item,
            mapItems = me.mapItems,
            newValues = null,
            srcValue,
            srcField;

        if (srcRecrod && mapItems) {
            
            for (i = 0; i < mapItems.length; i++) {

                item = mapItems[i];

                srcField = item.srcField;

                if ('' == srcField || '' == item.targetField) {
                    continue;
                }

                if (srcRecrod) {
                    if (srcRecrod.get) {
                        srcValue = srcRecrod.get(srcField);
                    }
                    else {
                        srcValue = srcRecrod[srcField];
                    }
                }
                else {
                    srcValue = null;
                }
                
                newValues = newValues || {};
                
                newValues[item.targetField] = srcValue;
            }

        }

        return newValues
    },



    isFirstCursorPos: function () {
        "use strict";
        var me = this,
            inputObj = me.inputObj;

        if(inputObj.isFirstCursorPos){
            
            return inputObj.isFirstCursorPos();
        }

        return false;
    },


    isLastCursorPos: function () {
        "use strict";
        var me = this,
            inputObj = me.inputObj;

        if (inputObj.isLastCursorPos) {
            return inputObj.isLastCursorPos();
        }

        return false;

    },

    proKeyEvent: function (e) {
        "use strict";
        var me = this,
            inputObj = me.inputObj;

        //log.debug("---------------------");
        //console.log(inputObj);

        
         me.onKeydown(e);
        

        return false;
    },

    proKeyEvent2: function (e) {
        "use strict";
        var me = this,
            inputObj = me.inputObj;

        //log.debug("---------------------");
        //console.log(inputObj);

        if (inputObj && inputObj.proKeyEvent) {

            return inputObj.proKeyEvent(e);
        }

        return false;
    },


    lastKeyCode: 0,


    onKeydown: function (e) {
        "use strict";
        var me = this,
            log = me.log,
            inputObj = me.inputObj,
            cellEl = me.cellEl;

        var keyCode =  e.keyCode;

        // 13 = 回车
        // 9  = Tab 键
        // 27 = Esc

        me.lastKeyCode = keyCode;
        
        //log.debug('keyCode = ' + keyCode);

        //console.log(e);

        //log.debug("========================== " + keyCode);

        if (13 == keyCode) {    //13 = 回车
            

            me.proKeyEvent2({
                keyCode:keyCode
            });


            if (e.stoped) { return; }


            me.inputSave(me.cellEl, {

                keydown: true,

                keyCode: keyCode
                
            });

            me.endEdit();



            Mini2.ui.FocusMgr.setControl(me.cellEl);


            me.visible = false;
            me.el.hide();
            $(cellEl).cFirst("div.mi-grid-cell-inner").show();
            me.cellEl = null;

            //Mini2.EventManager.stopEvent(e);

        }
        else if (9 == keyCode) {    // 9  = Tab 键

            me.inputSave(me.cellEl);
            me.endEdit();


            var nextCellEl = $(me.cellEl).next();

            if (nextCellEl.length ) {
                me.grid.autoFocusScroll(nextCellEl);

                Mini2.EventManager.stopEvent(e);
            }

            me.visible = false;
            me.el.hide();
            $(cellEl).cFirst("div.mi-grid-cell-inner").show();
            me.cellEl = null;
        }
        else if (27 == keyCode) {   // 27 = Esc
            me.record.cancelEdit();


            Mini2.ui.FocusMgr.setControl(me.cellEl);

            me.visible = false;
            me.el.hide();
            $(cellEl).cFirst("div.mi-grid-cell-inner").show();
            me.cellEl = null;

            Mini2.EventManager.stopEvent(e);
        }
        else {

        }

    },


    gotoNextCell: function (nCellEl,e) {
        "use strict";
        var me = this,
            log = me.log,
            grid = me.grid;

        var n = me.columnHeader.index + 1;
        var colHead = grid.m_Cols[n];

        if (colHead.miType == 'checkcolumn') {

            me.hide();
            Mini2.ui.FocusMgr.setControl(nCellEl, grid, null);

            return;
        }
        else if (colHead.miType == 'rowcheckcolumn') {
            return;
        }
        else if (colHead.miType == 'actioncolumn') {
            return;
        }

        //如果没有编辑器模式，就退出
        if (!colHead.editor || colHead.editorMode == 'none') {
            //log.debug("没有编辑模式.退出");

            Mini2.ui.FocusMgr.setControl(nCellEl, grid, null);

        }
        else {
            var editor = grid.getEditor(colHead, me.record);
            grid.showEditor(nCellEl, nCellEl, me.rowEl, colHead, me.record, editor);

            Mini2.EventManager.stopEvent(e);
        }
    },

    onKeyup: function (e) {
        "use strict";
        var me = this,
            keyCode = e.keyCode;


        if (9 == keyCode) {
            var nCellEl = $(me.cellEl).next();

            if (nCellEl.length) {
                me.gotoNextCell(nCellEl,e);
            }
        }
        else if (13 == keyCode) {

            console.debug("休息休息");
            
            Mini2.EventManager.stopEvent(e);
        }
        else {
            me.on('keyup', e);
        }
    },


    bind_key: function (inputObj) {
        "use strict";
        var me = this;

        inputObj.bind('keydown', function (e) {
            me.onKeydown(e);
        })
        .bind('keyup', function (e) {
            me.onKeyup(e);
        });

    },

    bind_mouse: function (inputObj) {
        "use strict";
        var me = this;

        $(me.inputObj)
        .muBind('mousedown', function (e) {


            me.on('mousedown', e);

        })
        .muBind('mouseup', function (e) {


            me.on('mouseup', e);

        })
        .muBind('mousemove', function (e) {

            //me.on('mousemove', e);

        });

    },



    render: function (boxType) {
        "use strict";
        /// <summary>创建容器</summary>

        var me = this,
            log = me.log,
            conBasePath = 'Mini2.ui.form.field',
            conFullname,
            el,
            subControlConfig,
            inputObj;


        me.el = el = $(me.getBox());
        me.visible = false;

        el.data('me', me);

        switch (me.xtype.toLowerCase()) {
            case 'combobox': conFullname = conBasePath + '.ComboBox'; break;
            case 'comboboxbase': conFullname = conBasePath + '.ComboBoxBase'; break;
            case 'datefield': conFullname = conBasePath + '.Date'; break;
            case 'numberfield': conFullname = conBasePath + '.Number'; break;
            case 'trigger': conFullname = conBasePath + '.Trigger'; break;
            case 'textarea': conFullname = conBasePath + '.TextArea'; break;
            case 'timefield': conFullname = conBasePath + '.Time'; break;
            case 'more_file': conFullname = conBasePath + '.MoreFileEditor'; break;
            case 'password': conFullname = conBasePath + '.Password'; break;

            default: conFullname = conBasePath + '.Text'; break;
        }


        subControlConfig = Mini2.applyIf({
            renderTo: el,
            scope: me,
            grid: me.grid,
            autoTrim : me.autoTrim,
            labelable: {
                hideLabel: true
            },
            placeholder: me.placeholder
        }, me.config);

        

        //if (colHead && colHead.placeholder) {

        //    subControlConfig.placeholder = colHead.placeholder;
        //}
        

        me.inputObj = inputObj = Mini2.create(conFullname, subControlConfig);

        inputObj.render();

        $(me.renderTo).append(el);

        me.bind_key(inputObj);
        me.bind_mouse(inputObj);


        $(me).muBind('focusin', function (e) {

            var me = this;

            me.inputObj.focus();


        })
        .muBind('focusout', function (e, muObj) {
            var me = this;

            if (muObj.data != me.cellEl) {

                if (27 != me.lastKeyCode) {

                    me.inputSave(muObj.data);

                    me.endEdit();
                }
            }

            //console.log('muObj.target = ', muObj.target);



            if (muObj.target != me) {

                me.hide();

                me.inputObj.on('focusout', muObj);

            }

        });

    },



    hideDropDownPanel: function () {
        "use strict";
        var me = this,
            inputObj = me.inputObj;

        if (inputObj && inputObj.hideDropDownPanel) {

            me.inputSave(me.cellEl);
            inputObj.hideDropDownPanel();

        }

        return me;
    },


    init: function (grid) {
        this.grid = grid;


    },

    onInit: function () {

        this.init();
    },

    isCellEditable: function (record, columnHeader) {


    },


    input_mapping: function (data) {
        "use strict";
        var me = this,
            values = {},
            inputObj = me.inputObj,
            mapItems = me.mapItems,
            record = me.record;


        if (mapItems) {
            $(mapItems).each(function () {

                var src = this.srcField;
                var target = this.targetField;

                values[target] = data[src];
            });
        }

        record.set(values);


    },

    startEdit: function (record, columnHeader, /* private */context) {
        "use strict";
        var me = this,
            inputObj = me.inputObj,
            colHead = columnHeader;

        //log.begin("CellEditor.startEdit(...)");

        record.beginEdit(); //.editing = true;

        me.record = record;
        me.columnHeader = colHead;

        context.originalValue = context.value = record.get(colHead.dataIndex);

        if (inputObj.setOldValue) {
            inputObj.setOldValue(context.value);
        }
        else {
            inputObj.oldValue = context.value;
        }
        inputObj.record = record;
        inputObj.dataIndex = colHead.dataIndex;
        inputObj.mapItems = me.mapItems;

        inputObj.cellDisplayField = colHead.displayField;   //警告：不能用 'displayField' 当做属性赋值。跟下拉控件的 'displayField' 属性冲突

        inputObj.map = me.input_mapping;

        me.setCellValue(context.value);

        me.show();

        //inputObj.focus();

        me.editing = true;

        //log.end();

        return me;
    },


    showEditor: function (ed, context, value) {


    },

    getEditor: function (record, column) {



    },

    getCell: function (record, column) {
        var me = this;

        return me.cell;
    },



    cancelEdit: function () {

        var me = this,
            record = me.record;

        //log.begin("CellEditor.cancelEdit(...)");

        record.cancelEdit();

        me.editing = false;

        //log.end();
    },

    endEdit: function () {
        var me = this,
            record = me.record;

        //log.begin("CellEditor.endEdit(...)");

        record.endEdit();

        me.editing = false;
        //log.end();
    },


    onShow: function () {

    },

    onHide: function () {



    },

    afterRender: function () {

    },

    onEditorTab: function () {


    },


    startEditByPosition: function (position) {
        "use strict";
        var me = this,
            inputObj = me.inputObj;

        me.css({
            left: position.left,
            top: position.top + 2
        });

        if (inputObj && inputObj.onPositionChanged) {
            inputObj.onPositionChanged();
        }
        return me;
    },

    /**
    * 删除自身编辑器
    */
    remove: function () {
        var me = this;

        me.el.remove();

        return me;
    }


});