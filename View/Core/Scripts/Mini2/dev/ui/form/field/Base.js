/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />
/// <reference path="../../EventManager.js" />
/// <reference path="../../FocusManager.js" />


Mini2.define('Mini2.ui.form.field.Base', {

    extend: 'Mini2.ui.Component',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.Base'),

    mixins: {
        labelable: 'Mini2.ui.form.Labelable'
    },

    secFunCode: null,

    secReadonly:null,
    

    //清除按钮
    clearText: false,

    /**
    * 用户自定义样式. 
    * 例如:
    */
    style: null,

    //输入容器对象
    isInput:true,

    //有错误的红色边框样式
    invalidCls: 'mi-form-invalid',

    //扩展字段
    extraFieldBodyCls : false,
    
    //获取或设置一个值，该值指示用户能否使用 Tab 键将焦点放到该控件上。
    tabStop : true,


    labelable: null,

    //    forceValidation: true,    //强制验证

    preventMark: false,  //阻止显示错误信息

    disabled: false,

    //    boxLable:false,

    renderTo: Mini2.getBody(),

    el: null,
    //    inputEl: null,

    name: '',

    oldValue: '',
    newValue: '',

    /**
    * 自动去掉左右空格
    */
    autoTrim : true,

    //自动提示。
    autoComplete: 'off',

    //允许空白
    allowBlank: true,

    readOnly: false,

    //left,right
    align: 'left',

    width: "100%",
    height: '',

    inputDom: null, //输入框的html元素

    //placeholder: '',

    autoWidth: true,

    //弄脏，做标记用
    dirty: false,

    //private  焦点的时间,（特殊处理)
    //_focusTimeout: null,

    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this;

        $(me).muBind('focusout', function () {

            
            if (me._focusTimeout) {
                clearTimeout(me._focusTimeout);
                delete me._focusTimeout;
            }

            if (me.fieldEl) {
                me.fieldEl.removeClass("mi-form-focus").trigger('blur'); //让文本框失去焦点
            }


            me.el.removeClass("mi-form-trigger-wrap-focus");

            me.on('hided');

            me.clearInvalid();
            //me.isValid();

            if (me.clearBtnEl) {
                me.clearBtnEl.hide();
            }

            if (me.autoTrim) {
                var srcValue = me.getValue();

                if (srcValue) {
                    var curValue = Mini2.String.trim(srcValue);

                    if (curValue != srcValue) {
                        me.setValue(curValue);
                    }
                }
            }


        });

        me.initLabelable();



    },


    //弄脏
    setDirty:function(value){
        var me = this,
            el = me.el,
            fieldEl = me.fieldEl;

        value = !!value;
        

        me.dirty = value;
        el.toggleClass('mi-form-dirty-item', value);

        return me;
    },

    getDirty :function(){
        var me = this;

        return me.dirty;
    },


    getRawValue: function () {
        "use strict";
        var me = this,
            value;

        if (me.readOnly && me.hideEl) {
            value = me.hideEl.val();
        }
        else {
            value = me.inputEl.val();
        }
        return value;

    },

    processRawValue: function (rawValue) {

        return rawValue;
    },

    isValid: function () {
        "use strict";
        var me = this,
            disabled = me.disabled,
            validate = me.forceValidation || !disabled;


        return validate ? me.validateValue(me.processRawValue(me.getRawValue())) : disabled;
    },


    //获取错误信息
    getErrors: function (value) {
        "use strict";
        var me = this;

        if (value == '' || value == null) {

            return '必填';
        }

        return '';
    },

    //验证值
    validateValue: function (value) {
        "use strict";
        var me = this,
            log = me.log,
            errors = me.getErrors(value),
            isValid = Mini2.isEmpty(errors);

        //log.debug('Errors = ' + errors);

        if (!me.preventMark) {
            if (isValid) {
                me.clearInvalid();
            }
            else {
                me.markInvalid(errors);
            }
        }

        return isValid;
    },


    getActiveError: function () {
        "use strict";
        var me = this,
            labelable = me.labelable;

        return labelable.getActiveError();
    },

    //判断有没有显示错误标记
    hasActiveError: function () {
        "use strict";
        var me = this,
            labelable = me.labelable;

        return labelable.hasActiveError();
    },

    //卸载删除错误标记
    unsetActiveError: function () {
        var me = this,
            labelable = me.labelable;

        labelable.unsetActiveError();
    },

    //清理异常提示
    clearInvalid: function () {
        "use strict";
        var me = this,
            el = me.el,
            hadError = me.hasActiveError(),
            inputEl = me.inputEl,
            fieldEl = me.fieldEl,
            invalidCls = me.invalidCls;


        el.removeClass(invalidCls);

        if (fieldEl) {
            fieldEl.removeClass(invalidCls + '-field');
        }
        else if (inputEl && inputEl.removeClass) {
            inputEl.removeClass(invalidCls + '-field');
        }

        me.unsetActiveError();
        if (hadError) {
            me.setError('');
        }
    },


    //创建异常提示
    markInvalid: function (errors) {
        "use strict";
        var me = this,
            el = me.el,
            oldMsg = me.getActiveError(),
            inputEl = me.inputEl,
            fieldEl = me.fieldEl,
            invalidCls = me.invalidCls;

        el.addClass(invalidCls);

        if (fieldEl) {
            fieldEl.addClass(invalidCls + '-field');
        }
        else {
            inputEl.addClass(invalidCls + '-field');
        }

        if (oldMsg !== errors) {
            me.setError(errors);
        }

        return me;
    },

    //设置错误提示消息
    setError: function (active) {
        "use strict";
        var me = this,
            msgTarget = me.msgTarget,
            prop,
            labelable = me.labelable;

        if (active == undefined || active == '' || active == null) {
            me.unsetActiveError();
        }
        else {
            if (labelable) {
                labelable.setActiveError(active);
            }
        }
    },

    insertText: function (obj, str) {
        "use strict";
        if (document.selection) {
            var sel = document.selection.createRange();
            sel.text = str;
        }
        else if (typeof obj.selectionStart === 'number' && typeof obj.selectionEnd === 'number') {
            var startPos = obj.selectionStart,
                endPos = obj.selectionEnd,
                cursorPos = startPos,
                tmpStr = obj.value;

            obj.value = tmpStr.substring(0, startPos) + str + tmpStr.substring(endPos, tmpStr.length);
            cursorPos += str.length;
            obj.selectionStart = obj.selectionEnd = cursorPos;
        }
        else {
            obj.value += str;
        }
    },

    moveEnd: function (obj) {
        "use strict";
        obj.focus();
        var len = obj.value.length;
        if (document.selection) {
            var sel = obj.createTextRange();
            sel.moveStart('character', len);
            sel.collapse();
            sel.select();
        }
        else if (typeof obj.selectionStart == 'number' && typeof obj.selectionEnd == 'number') {
            obj.selectionStart = obj.selectionEnd = len;
        }
    },


    layoutReset: function () {

    },


    onPositionChanged: Mini2.emptyFn,


    isValueChanged: function () {
        var me = this,
            log = me.log;

        log.debug('---- oldValue = ' + me.oldValue);
        log.debug('---- me.getValue = ' + me.getValue());

        return me.oldValue != me.getValue();
    },

    getLeft: function () {
        var me = this;

        return me.el.css('left');
    },

    setLeft: function (value) {
        var me = this,
            el = me.el;
        el.css('left', value);

        return me;
    },

    getTop: function () {
        var me = this,
            el = me.el;

        return el.css('top');
    },

    setTop: function (value) {
        var me = this,
            el = me.el;

        el.css('top', value);

        return me;
    },

    setWidth: function (value) {
        var me = this,
            el = me.el;

        me.width = value;
        el.css('width', value);

        return me;
    },

    getWidth: function () {
        return this.width;
    },

    setHeight: function (value) {
        var me = this;
        me.height = value;
        me.el.css('height', value);
        return me;
    },

    getHeight: function () {
        return this.height;
    },


    setValue: function (value) {
        var me = this,
            log = me.log,
            hideEl = me.hideEl;

        me.inputEl.val(value);

        if (hideEl) {
            hideEl.val(value);
        }
        
        return me;
    },

    getValue: function () {
        var me = this,
            hideEl = me.hideEl,
            value;

        if (me.readOnly || 'none' == me.mode) {
            value = hideEl.val();
        }
        else {
            value = me.inputEl.val();
        }



        return value;
    },


    hide: function () {
        var me = this;

        me.visible = false;

        me.el.hide();

        me.callParent();

        return me;
    },





    focus: function () {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            fieldEl = me.fieldEl,
            inputEl = me.inputEl;
       
        Mini2.ui.FocusMgr.setControl(me, null, me.scope);

        if (!el.hasClass('mi-form-trigger-wrap-focus')) {

            if (fieldEl) {
                fieldEl.addCls("mi-form-focus", me.focusCls);
            }

            el.addClass("mi-form-trigger-wrap-focus");            
        }

        //特殊处理，延时获取焦点。不然无法及时得到焦点。
        if (!me.readOnly && inputEl && inputEl.focus && !me._focusTimeout) {
            
            me._focusTimeout = setTimeout(function () {

                if (el.hasClass('mi-form-trigger-wrap-focus')) {
                    inputEl.focus();
                }

                me._focusTimeout = null;

            }, 200);
        }


        if (me.clearBtnEl) {
            me.clearBtnEl.css('display', 'inline');
        }


        return me;
    },

    select: function () {

        var me = this;

        me.inputEl.select();

        return me;
    },

    onLostFocus: function (e) {
        var me = this;

        me.inputEl.removeClass('mi-form-focus');

        me.on('hided');
    },

    show: function () {

        var me = this;

        me.visible = true;

        me.el.show();

        me.callParent();

        return me;
    },

    fieldCls: 'mi-form-type-text',


    //获取带标签的表格元素
    getTableContainer: function () {
        "use strict";
        /// <summary>获取容器</summary>

        var me = this,
            mixins = me.mixins,
            labelable = mixins.labelable,
            tableEl;

        if (me.labelable) {
            tableEl = me.labelable.labelableRenderTpl;
        }
        else {
            tableEl = Mini2.$join([
            '<table class="mi-field mi-table-plain mi-form-item  mi-field-default " ',
                'cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%; ">',
                '<tbody>',
                    '<tr role="presentation" class="mi-form-item-input-row">',
                        '<td class="mi-form-item-body"></td>',
                    '</tr>',
                '</tbody>',
            '</table>']);
        }


        //如果是设计模式, 就在外面套一层 div
        //if (Mini2.designMode()) {
        //    tableEl = tableEl.wrap('<div></div>');
        //}


        tableEl.addClass(me.fieldCls);
        tableEl.css('width', me.width);

        if (me.left) {
            tableEl.css('left', me.left);
        }

        if (me.top) {
            tableEl.css('top', me.top);
        }

        if ('static' != me.position) {
            tableEl.css('position', me.position);
        }

        return tableEl;
    },



    bindInputEl_Key: function (inputEl) {
        "use strict";
        var me = this;

        $(inputEl)
        .on('keydown', function (e) {


            me.on('keydown', e);
        })
        .on('keyup', function (e) { me.on('keyup', e); })
        .on('keypress', function (e) {

            me.on('keypress', e);
        });

        //        $(inputEl).change(function (e) {
        //            me.isValid();

        //            log.debug(" me========");
        //        });
    },

    bindInputEl_Mouse: function (inputEl) {
        "use strict";
        var me = this;


        $(inputEl).muBind('mousedown', function (e) {
            me.focus();
            me.on('mousedown', e);
            //Mini2.EventManager.preventDefault(e); 
        }).muBind('mouseup', function (e) {

            me.on('mouseup', e);
            Mini2.EventManager.stopEvent(e);
        });
    },


    getCell: function (tableEl, rowIndex, colIndex) {
        "use strict";
        var tbodyEl, rowEl, tdEl;

        tbodyEl = $(tableEl).children("tbody:first");

        if (tbodyEl.length == 0) {
            tbodyEl = tableEl;
        }

        rowEl = $(tbodyEl).children("tr:eq(" + rowIndex + ")");

        tdEl = $(rowEl).children(":eq(" + colIndex + ")");

        return tdEl;

    },


    /**
    * 附加到单元格中
    */
    appendCell: function (tableEl, targetEl, rowIndex, colIndex) {
        "use strict";
        var me = this,
            cellEl,
            args = arguments;

        if (targetEl) {

            if (args.length == 4) {
                cellEl = me.getCell(tableEl, rowIndex, colIndex);
            }
            else if (Mini2.isString(args[2])) {
                cellEl = tableEl.find(args[2]);


                if (cellEl.length == 0) {
                    console.error('没有找到对应的节点, 无法附加元素');
                }
            }
            else {
                console.error("错误....");
            }

            $(cellEl).append(targetEl);

        }
        else {
            console.error("targetEl 不能为空, 无法附加到框架上.", targetEl);
        }

        return me;
    },

    //初始化标签容器
    initLabelable: function () {
        "use strict";
        var me = this,
            mixins = me.mixins,
            labelable = mixins.labelable,
            labCfg = me.labelable || {},
            lab;


        if (labCfg && labCfg.hideLable) {
            return;
        }

        labCfg = Mini2.apply(labCfg, {
            owner: me,
            required: me.required
        });

        lab = Mini2.create(labelable, labCfg);

        delete me.labelable;

        me.labelable = lab;

        try {

            var sec = Mini2.ui.SecManager;

            sec.reg(me.secFunCode, me, 'visible');
            sec.reg(me.secReadonly, me, 'readonly');
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }

    },


    //隐藏标记
    hideEl: null,


    //创建隐藏标记
    createHideEl: function () {
        "use strict";
        var me = this,
            name = me.name,
            hideEl = $('<input type="hidden" value="" />');

        if (name) {
            hideEl.attr('name', name);
        }


        hideEl.val(me.value || me.srcValue);

        return hideEl;
    },



    //设置只读状态。bool 值
    setReadOnly: function (value) {
        "use strict";
        var me = this,
            el = me.el,
            inputEl = me.inputEl,
            hideEl = me.hideEl,
            readonly = 'readonly',
            readonlyCls = 'mi-form-item-readonly';

        value = !!value;
        me.readOnly = value;

        el && el.toggleClass(readonlyCls, value);
        

        if (!inputEl) {
            return;
        }

        if (value) {

            inputEl.attr(readonly, readonly);
        }
        else {
            inputEl.removeAttr(readonly);
        }


        if (value) {
            if (!hideEl) {
                inputEl.attr('name', '');
                me.hideEl = hideEl = me.createHideEl();

                var bodyCls = 'td.mi-form-item-body:first';
                var tableEl = me.getTableContainer();

                me.appendCell(tableEl, hideEl, bodyCls);
            }
            
        }
        else {
            if (hideEl) {
                inputEl.attr('name', me.name);

                $(hideEl).remove();

                delete me.hideEl;
            }

        }

        $(inputEl).change(function () {
            me.clearInvalid();
        });

        $(inputEl).keydown(function () {
            me.clearInvalid();
        });

        return me;
    },

    getReadOnly: function () {
        var me = this;
        return me.readOnly;
    },


    /**
     * 映射值
     */
    setMap: function (record, userMapItems) {
        "use strict";
        var me = this,
            mapItems = userMapItems || me.mapItems,
            ownerParent = me.ownerParent,
            store,
            curRect;

        //没有映射就取消
        if (!mapItems) {
            return;
        }

        if (me.record) {
            curRect = me.record;
        }
        else {

            if (!ownerParent) {
                return;
            }

            store = ownerParent.store;

            if (!store) {
                return;
            }

            curRect = store.getCurrent();
        }


        for (var i = 0; i < mapItems.length; i++) {
            var mapItem = mapItems[i];

            if (mapItem && mapItem.srcField && mapItem.targetField) {

                var value;

                if (record.get) {
                    value = record.get(mapItem.srcField);
                }
                else {
                    value = record[mapItem.srcField];
                }

                curRect.set(mapItem.targetField, value);
            }
            else {
                console.warn("映射规则定义错误:", mapItem);
            }
        }


    },


    /**
    * 获取要发送的数据
    */
    getSentDataForTypeahead: function () {
        "use strict";
        var me = this,
            sentData = {},
            ps = me.typeaheadParams;

        $(ps).each(function () {

            var p = this;

            if ('query_string' == p.role) {
                var qValue = $.query.get(p.queryStringField);

                sentData[p.name] = qValue;
            }
            else if ('control' == p.role) {

                var con = Mini2.find(p.control);

                var value = con.getValue();

                sentData[p.name] = value;

            }
            else {

                var paramDef = p['default'];

                if (paramDef.indexOf('{{') >= 0 && paramDef.indexOf('}}')) {

                    var artTemp = template.compile(paramDef);
                    var valueStr = artTemp({
                        T: { '未定义': '' }
                    });

                    sentData[p.name] = valueStr;
                }
                else {
                    sentData[p.name] = paramDef;
                }

            }

        });

        return sentData;
    },


    /**
    * 初始化输入提示
    */
    initTypeahead: function (inputEl) {
        "use strict";
        var me = this,
            type = me.type || 'text',
            th = me.typeahead;



        if (th && 'text' == type) {

            $(inputEl).addClass('mi-typeahead');

            $(inputEl).typeahead({
                //fitToElement: true,
                autoSelect: th.autoSelect || false,
                appendTo: Mini2.getBody(),
                delay: 200,

                items: th.items,

                source: function (query, process) {
                    //'/api/InfoGrid2/View/Sample/Examples/WebAPI/Doc001Form?action=query_search'
                    var ps = me.getSentDataForTypeahead();

                    ps[th.searchKey || 'name'] = query;

                    
                    return Mini2.post(th.remoteUrl, ps,
                        function (data) {
                            return process(data);
                        }
                    );
                },
                //matcher: function (item) {
                //    return true;
                //},
                //updater: function (item) {

                //    console.debug('选中: ' + item);

                //    return item ;//这里一定要return，否则选中不显示，外加调用display的时候null reference错误。
                //},
                displayText: function (item) {

                    return item[th.field] || '未指定字段';
                },
                afterSelect: function (item) {
                    //选择项之后的时间，item是当前选中的项
                    //console.debug("---- 选择项之后的 ", item);

                    me.curRecord = item;

                    try {

                        me.setMap(item, me.typeaheadMapItems);
                    }
                    catch (ex) {
                        console.error('设置映射值错误', ex);
                    }


                },
                highlighter: function (text, item) {

                    if (th.displayFormat) {
                        var artTemp = me.artTemplate;

                        if (!artTemp) {
                            me.artTemplate = artTemp = template.compile(th.displayFormat);
                        }

                        var valueStr = artTemp({
                            T: item
                        });

                        return valueStr;
                    }
                    else {
                        return text;
                    }
                }
            });
        }
    },


    getFieldEl: function () {
        "use strict";
        var me = this,
            type = me.type || 'text',
            inputEl = Mini2.$joinStr([
            '<input type="',type,'" class="mi-form-field mi-form-text" ',
                'aria-invalid="false"  style="width: 100%; " autocomplete="off" spellcheck="false"  >']);


        if (me.name) {
            inputEl.attr('name', me.name);
        }

        inputEl.attr("id", me.clientId)
            .attr('placeholder', me.placeholder)
            .css('text-align', me.align);


        me.initTypeahead(inputEl);


        //        if (me.readOnly) {
        //            inputEl.attr('readonly', 'readonly');
        //        }

        //附加属性
        //me.inputEl.attr("data-errorqtip", "");
        //

        //inputEl.addClass("mi-form-required-field");



        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);


        me.inputEl = inputEl;

        return inputEl;

    },

    renderForLableable: function (tableEl) {
        "use strict";
        var me = this,
            labelEl,
            labelable = me.labelable;


        if (labelable ) {

            //处理显示的标签
            if (!labelable.hideLabel) {
                labelEl = labelable.getEl();

                if (me.clientId) {
                    labelEl.attr("for", me.clientId);
                }

                me.appendCell(tableEl, labelEl, 'td.mi-field-label-cell:first');

                if (!Mini2.designMode()) {
                    labelEl.muBind('mousedown', function (e) {
                        me.focus();
                    });
                    labelEl.muBind('mouseup', function (e) {
                        Mini2.EventManager.stopEvent(e);
                    });
                }
            }

            //处理显示的帮助信息
            if (!labelable.hideHelper) {



            }

        }

        return me;
    },

    renderForBoxLabel: function (tableEl) {
        "use strict";
        var me = this,
            boxLabelEl;

        if (me.boxLabel) {
            boxLabelEl = Mini2.$joinStr([
                '<label class="mi-form-cb-label mi-form-cb-label-after" for="', me.clientId, '">',
                    me.boxLabel,
                '</label>'
            ]);

            boxLabelEl.muBind('mousedown', function (e) {
                me.focus();
            });
            boxLabelEl.muBind('mouseup', function (e) {
                Mini2.EventManager.stopEvent(e);
            });

            me.boxLableEl = boxLabelEl;

            me.appendCell(tableEl, boxLabelEl, 'td.mi-form-item-body:first');
        }

        return me;
    },


    baseRenderSize:function(tableEl){
        'use strict';
        var me = this,
            i, cssName,
            cssAttrs = null,
            attrs = {
                'width': 'width',
                'height': 'height',
                'minHeight': 'min-height',
                'minWidth': 'min-width',
                'maxHeight': 'max-height',
                'maxWidth' : 'max-width'
            };


        for (i in attrs) {
            if (me[i]) {
                cssName = attrs[i];

                cssAttrs = cssAttrs || {};
                cssAttrs[cssName] = me[i];
            }
        }

        if (cssAttrs) {
            tableEl.css(cssAttrs);
        }
    },


    ////获取 body 扩展对象
    //getBodyExpand:function(){
    //    return null;
    //},

    baseRender: function () {
        "use strict";
        var me = this,
            log = me.log,
            tableEl, fieldEl,
            bodyExpandEl,
            bodyCls = 'td.mi-form-item-body:first';

        //容器
        tableEl = me.getTableContainer();

        //存放于 body 里面的对象
        fieldEl = me.getFieldEl();

        //容器尺寸
        me.baseRenderSize(tableEl);

        if (me.applyTo) {

            if (me.parentEl && Mini2.isString(me.applyTo)) {
                var srcEl = $(me.parentEl).find(me.applyTo);
                srcEl.replaceWith(tableEl);
            }
            else {
                $(me.applyTo).replaceWith(tableEl);
            }

        }
        else {
            $(me.renderTo).append(tableEl);
        }

        //容器中 body DOM 节点
        me.fieldBodyEl = tableEl.find(bodyCls);


        //显示表单的标签
        me.renderForLableable(tableEl);

        if (me.srcInputEl) {
            me.appendCell(tableEl, me.srcInputEl, bodyCls);
        }

        me.appendCell(tableEl, fieldEl, bodyCls);

        //扩展 dom属性
        if (me.getBodyExpand) {
            bodyExpandEl = me.getBodyExpand();

            if (bodyExpandEl) {
                me.appendCell(tableEl, bodyExpandEl, bodyCls);
            }
        }

        me.renderForBoxLabel(tableEl);


        me.fieldEl = fieldEl;



        if (me.clearText) {
            
            var clearBtnEl;

            me.clearBtnEl = clearBtnEl = $('<a class="mi-form-text-clear" style="display:none;"></a>');

            $(fieldEl).after(clearBtnEl)

            $(clearBtnEl).click(function () {
                fieldEl.val('');                
            });            
        }

        me.el = tableEl;

        if (me.readOnly) {
            me.setReadOnly(me.readOnly);
        }


        me.fieldBodyEl.addClass(me.extraFieldBodyCls);

        if (me.dirty) {
            me.setDirty(me.dirty);
        }


        if (me.dataSource ) {

            //log.debug("查找数据源:" + me.dataSource);
            var dataSource = Mini2.data.StoreManager.lookup(me.dataSource);

            me.dataSource = dataSource;

            //console.log("数据对象", dataSource);

            me.bindDataSource(dataSource);

        }

        me.initValue();

        if (undefined != me.value) {
            me.oldValue = me.value;
            me.setValue(me.value);
            delete me.value;
        }

        //用户自定义样式
        if (me.style) {

            var srcStyle = tableEl.attr('style');

            srcStyle += me.style;

            tableEl.attr('style', srcStyle);

        }

        return me;
    },


    /**
     * 设置集合
     */
    setRecord:function(record){
        var me = this,
            value ,
            field = me.dataField;

        if (!Mini2.isBlank(field)) {
            
            if (record.isModel) {
                value = record.get(field);
            }
            else {
                value = record[field];
            }
            
            me.oldValue = value;
            me.setValue(value);
        }

        me.record = record;

        return me;
    },
    

    bindDataSource: function (dataSource) {
        "use strict";
        var me = this,
            log = me.log;
        
        log.debug("bindDataSource 绑定数据源");

        $(dataSource)
            .muBind('update', me, me.dataSource_update)              //监听记录更新事件
            //.muBind('add', me, me.store_add)                    //监听记录添加事件
            //.muBind('datachanged', me, me.store_dataChanged)    //监听数据更改
            //.muBind('refresh', me, me.store_refresh)            //监听刷新事件
            .muBind('load', me, me.dataSource_load)                  //加载事件
            //.muBind('bulkremove', me, me.store_bulkRemove)      //左上角标记移除事件
            //.muBind('invalid', me, me.store_invalid)            //出现异常的单元格
            //.muBind('clear', me, me.store_clear)                //删除全部记录事件
            .muBind('currentchanged', me, me.dataSource_currentChanged);  //焦点行发生改变的事件

    },


    dataSource_update: function (event, record, operation, modifiedFieldNames) {
        "use strict";
        var me = event.data,
            i, j,
            modifiedField,
            items,
            len,
            isUpdateStyleField = false;


        if ('COMMIT' == operation) {

            var value = record.get(me.dataField);

            me.setValue(value);
        }
        else if ('EDIT' == operation) {

            var value = record.get(me.dataField);

            me.setValue(value);
        }
        else {
            console.error('operation=', operation);
        }

        return isUpdateStyleField;
    },

    dataSource_load: function (event, store) {
        "use strict";
        var me = event.data,
            log = me.log,
            i,
            record,
            records = store.data,
            len;


        log.debug('store_load(...)');

    },


    dataSource_currentChanged: function (event, index, record) {
        "use strict";
        var me = event.data,
            log = me.log;

        log.debug("数据仓库数据发生变化 store_currentChanged(...)");

        if (index > -1) {

            if (!record) {
                return;
            }

            //log.debug('item.dataField', item.dataField);

            var rectValue = record.get(me.dataField);

            //log.debug('value=' + rectValue);

            if (me.setOldValue) {
                me.setOldValue(rectValue);
            }
            else {
                me.oldValue = rectValue;
            }

            me.setValue(rectValue);

            

        }


    },




    initValue: Mini2.emptyFn,

    //显示延迟
    delayRender: function () {
        "use strict";
        var me = this,
            el = $(me.applyTo);

        me.isDelayRender = true;

        Mini2.delayRS = Mini2.delayRS || {};

        Mini2.delayRS[me.clientId] = me;

        el.attr('mi-delay-render', 'true');
        el.data('me', me);

        return me;
    },



    render: function () {
        "use strict";
        var me = this,
            el;

        me.baseRender();

        el = me.el;

        el.attr('data-muid', me.muid);
        el.data('me', me);

        if (!me.visible) {
            el.hide();
        }

        return me;
    }

});