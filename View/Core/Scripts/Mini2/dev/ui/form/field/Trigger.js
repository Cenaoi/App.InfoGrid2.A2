/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />



Mini2.define('Mini2.ui.form.field.Trigger', {

    extend: 'Mini2.ui.form.field.Text',

    alias: 'widget.trigger',
    log : Mini2.Logger.getLogger( 'Mini2.ui.form.field.Trigger',  false),

    //按钮元素
    buttonEl: null,

    //弹出的版面
    dropDownPanel: null,


    emptyText: '',

    focusCls: 'mi-form-focus',

    //数据仓库
    store: false,

    itemBody: null,

    content: null,

    changed: Mini2.emptyFn,


    /**
     * 模式，可编辑-UserInput， 不可键盘输入-None .  mode=[none|user_input]
     */
    mode: 'user_input',

    //buttonCls:'mi-form-trigger-more',


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


    initStore: function () {
        /// <summary>初始化数据仓库</summary>

        var me = this,
            srcStore = me.store,
            store;


        if (Mini2.isString(srcStore)) {
            store = Mini2.data.StoreManager.lookup(me.store);

            delete me.store;
        }
        else if (Mini2.isArray(srcStore)) {

            store = Mini2.create('Mini2.data.Store', {
                storeId: 'MiniStore_' + Mini2.newId(),
                fields: me.fields || [],
                data: srcStore,
                autoSave: false
            });

            delete me.store;
        }
        else {
            store = me.store;
        }

        me.store = store;

        me.bindStore(store);
    },


    getStore: function () {

        var me = this;

        return me.store;

    },

    bindStore: function (store) {
        //        var me = this;
        //        me.store = store;

        //        $(store).muBind('add', me, me.store_add);
        //        $(store).muBind('datachanged', me, me.store_dataChanged);
        //        $(store).muBind('refresh', me, me.store_refresh);
        //        $(store).muBind('load', me, me.store_load);
    },


    getTriggerMarkup: function () {

        var me = this,
            tpl = Mini2.$join([
            '<div class="mi-boundlist ',
                'mi-layer ',
                'mi-border-box" ',
                'style="border-width: 1px; right: auto; z-index: 39001; display:none; ">',
                '',
            '</div>'
        ]);

        //    '<div id="datepicker-1041-innerEl" role="grid">']);

        return tpl;
    },


    /**
     * 改写隐藏标签
     */
    hide: function () {

        var me = this;
        
        me.visible = false;

        me.el.hide();

        me.hideDropDownPanel();

        me.callParent();

        return me;
    },

    layoutReset: function () {
        var me = this;

        me.hideDropDownPanel();

    },





    init_DropDwonPanel: function () {
        /// <summary>初始化下拉版面</summary>

        var me = this;
        if (me.dropDownPanel != null) {
            return;
        }


        var tpl = me.getTriggerMarkup();

        me.dropDownPanel = $(tpl);
        $(document.body).append(me.dropDownPanel);

        $(me.dropDownPanel).mousedown(function (e) {

            //log.debug("form.field.Trigger.mousedown() ");

            //Mini2.EventManager.setScope(this, [me.el, me.dropDownPanel]);
        })
        .mousemove(function (e) {

        })
        .mouseup(function (e) {
            //console.log("form.field.Trigger.mouseup() ");

            Mini2.EventManager.stopEvent(e);
            Mini2.EventManager.clear(me.scope || me.el);

        });

    },


    getBoundPanel: Mini2.emptyFn,

    initContentPanel: function () {

        var me = this;

        if (!me.isCustom) {
            var content = null;

            if (Mini2.isString(me.content)) {

                if (window[me.content]) {
                    content = window[me.content];
                }
                else {
                    content = $(me.content);
                }

                delete me.content;
            }
            else {
                content = me.content;
            }

            me.isCustom = true;


            $(content).addCls('mi-boundlist', 'mi-layer', 'mi-border-box')
                .css({ right: 'auto', 'z-index': 39001 })
                .show();

            me.dropDownPanel = content;

        }
    },


    beforeShowDropDownPanel: function () {
        /// <summary>开始显示之前的</summary>
        var me = this;

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


    showDropDownPanel: function () {
        /// <summary>显示下拉框</summary>

        var me = this;

        me.init_DropDwonPanel();

        if (me.dropDown) {
            try {
                me.dropDown.call(me, me);
            }
            catch (ex) {
                throw new Error("执行 Trigger.dropDown(...) 事件发生错误");
            }
        }


        me.trigger('dropDown'); //触发下拉框


        var el = me.el,
            panel = me.dropDownPanel,
            w = me.dropDownWidth || (me.dropDownWidth > 0) || el.width(),
            h = me.dropDownHeight, //|| 200;
            office = me.fieldEl.offset();


        //console.log("下拉偏移量",office);

        if (!me.isCustom) {
            panel.css({
                'width': w - 4,
                'height': h
            });
        }

        var buttomY = office.top + el.height() - 2 + h;


        panel.css({
            'left': office.left,
            'top': office.top + el.height() - 2
        }).show();


    },


    hideDropDownPanel: function () {
        var me = this,
            dropDownPanel = me.dropDownPanel;

        if (dropDownPanel == null) {
            return;
        }

        dropDownPanel.hide();


        me.trigger('dropDownClosed'); //下拉框关闭的事件
    },


    triggerDropDownPanel: function () {
        var me = this,
            dropDownPanel = me.dropDownPanel;

        if (dropDownPanel  && dropDownPanel.css('display') != 'none') {
            //console.log('隐藏');
            me.hideDropDownPanel();
        }
        else {
            //console.log('显示');
            me.beforeShowDropDownPanel();
            me.showDropDownPanel();
        }
    },

    onPositionChanged: function () {
        var me = this,
            el = me.el,
            office,
            panel = me.dropDownPanel;

        if (panel) {
            office = el.offset();

            panel.css({
                left: office.left,
                top: office.top + el.height() - 2
            });
        }
    },

    //引发事件
    onButtonClick: function () {
        var me = this,
            btnClick = me.buttonClick,
            btnClick_Cb = me.buttonClick_Callback;
        

        if (btnClick || btnClick_Cb) {

            if (btnClick && Mini2.isFunction(btnClick)) {
                btnClick.call(me,me);
            }

            if (btnClick_Cb && Mini2.isFunction(btnClick_Cb)) {
                btnClick_Cb.call(me,me);
            }

        }
        else {
            me.triggerDropDownPanel();
        }


    },

    //按钮绑定事件
    bindEvent_Button: function (btnEl) {
        var me = this,
            baseCls = "mi-form",
            overCls = baseCls + "-trigger-over",
            clickCls = baseCls + "-trigger-click";

        $(btnEl).mouseenter(function () {
            if (!me.readOnly) {
                $(this).addClass(overCls);
            }
        })
        .mouseleave(function () {
            if (!me.readOnly) {
                $(this).removeClass(overCls);
            }
        });

        $(btnEl).muBind('mousedown',function (e) {
            if (!me.readOnly) {
                me.focus();
            }
            Mini2.EventManager.stopEvent(e);
        })
        .muBind('mouseup',function (e) {
            if (!me.readOnly) {
                $(this).removeClass(clickCls);

                me.onButtonClick();
            }
            Mini2.EventManager.stopEvent(e);
        });


    },



    focus: function () {

        var me = this,
            el = me.el,
            fieldEl = me.fieldEl;

        if (!el.hasClass('mi-form-trigger-wrap-focus')) {
            fieldEl.addClass('mi-form-focus');

            el.addClass('mi-form-trigger-wrap-focus');


            //特殊处理，延时获取焦点。不然无法及时得到焦点。
            if (!me._focusTimeout) {

                if (me.inputEl) {
                    me._focusTimeout = setTimeout(function () {

                        var inputEl = me.inputEl;

                        //如果焦点已经移除， 那么就作废
                        if (fieldEl.hasClass('mi-form-focus')) {

                            inputEl.focus();

                        }

                        me._focusTimeout = null;

                    }, 200);
                }
            }

            //console.log('me.scope', me.scope);

            //最后一个参数,强制设置
            Mini2.ui.FocusManager.setControl(me.scope || me, null, null, true);
        }
    },




    bindInputEl_Mouse2: function (inputEl) {

        var me = this;

        if ('none' == me.mode) {

            inputEl.css('current', 'pointer');

            $(inputEl).mousedown(function (e) {
                if (!me.readOnly) {
                    me.focus();
                }
                Mini2.EventManager.stopEvent(e);
            })
            .mouseup(function (e) {
                if (!me.readOnly) {
                    me.onButtonClick();
                }
                Mini2.EventManager.stopPropagation(e);
            });

            //$(inputEl).keyup(function (e) {

            //    if (!me.readOnly) {

            //        console.debug('----keys = ', e.keyCode);

            //        if (46 == e.keyCode) {
            //            me.setValue('');
            //            Mini2.EventManager.stopPropagation(e);
            //        }
            //    }

            //});
        }


    },


    getFieldEl: function () {
        var me = this,
            log = me.log,
            tableEl,
            inputEl,
            btnEl,
            sysInfo = Mini2.SystemInfo.formTrigger;

        tableEl = Mini2.$join([
            '<table class="mi-form-trigger-wrap" cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%; ">',
                '<tbody>',
                    '<tr>',
                        '<td class="mi-form-trigger-input-cell" style="width: 100%; "></td>',
                        '<td valign="top" class="mi-trigger-cell mi-unselectable" style="width:22px;" role="button" ></td>',
                    '</tr>',
                '</tbody>',
            '</table>'
        ]);

        me.inputEl = inputEl = Mini2.$join([
            '<input type="text" class="mi-form-field mi-form-text" ',
                'autocomplete="off" aria-invalid="false" spellcheck="false" style="width: 100%; ">'
        ]);

        if (!Mini2.isBlank(me.placeholder)) {
            inputEl.attr('placeholder', me.placeholder);
        }

        if (me.required) {
            inputEl.addClass('mi-form-required-field');
        }

        btnEl = Mini2.$join(['<div class="mi-form-trigger mi-form-arrow-trigger " role="button"></div>']);

        me.buttonEl = btnEl;

        if (me.buttonCls) {
            btnEl.addCls(me.buttonCls);
        }

        inputEl.attr("id", me.clientId);

        if (me.name) {
            inputEl.attr('name', me.name);
        }

        me.appendCell(tableEl, inputEl, 'td.mi-form-trigger-input-cell');
        me.appendCell(tableEl, btnEl, 'td.mi-trigger-cell');


        tableEl.find('td.mi-trigger-cell').width(sysInfo.width);

        if (undefined != me.value) {
            me.setValue(me.value);

            me.srcValue = me.value;

            delete me.value;
        }

        me.bindEvent_Button(btnEl);

        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);

        me.bindInputEl_Mouse2(inputEl);

        me.initTypeahead(inputEl);  //绑定输入提示


        $(me).on('keyup', function (data, ea) {

                      
            
            if (13 == ea.keyCode && me.mapping) { //13=回车
                me.onMaping();
                ea.cancel = true;
            }

        });


        if ('none' == me.mode) {
            me.setMode(true);
        }

        return tableEl;
    },


    /**
    * 触发映射
    *
    */
    onMaping:function(){

        var me = this,
            mapTimer = me.mapTimer;

        if (mapTimer) {
            mapTimer.resetStart();
        }
        else {
            me.mapTimer = Mini2.setTimer(function () {
                       
                me.mapping.call(me, me);

            }, [500,1]);
        }

    },



    //设置只读状态。bool 值
    setMode: function (value) {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            inputEl = me.inputEl,
            hideEl = me.hideEl,
            readonly = 'readonly';



        if (value) {
            //log.debug('设置只读属性');
            inputEl.attr(readonly, readonly);
        }
        else {
            //log.debug('删除只读属性');
            inputEl.removeAttr(readonly);
        }

        if (value) {
            if (!hideEl) {
                //log.debug('创建隐藏标签');

                inputEl.attr('name', '');
                me.hideEl = hideEl = me.createHideEl();
                hideEl.attr('name', me.name);
                inputEl.after(hideEl);
            }
        }
        else {
            if (hideEl) {

                //log.debug("删除隐藏标签");

                inputEl.attr('name', me.name);

                $(hideEl).remove();

                delete me.hideEl;
            }
        }


        return me;
    },


    //设置只读状态。bool 值
    setReadOnly: function (value) {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            inputEl = me.inputEl,
            hideEl = me.hideEl,
            readonly = 'readonly',
            readonlyCls = 'mi-form-item-readonly';

        //log.debugFormat('调用 setReadOnly({0}) 函数', value);

        value = !!value;
        me.readOnly = value;
        el && el.toggleClass(readonlyCls, value);


        if (!inputEl) {
            return;
        }

        if (value || me.mode == 'none') {
            log.debug('设置只读属性');
            inputEl.attr(readonly, readonly);
        }
        else {
            log.debug('删除只读属性');
            inputEl.removeAttr(readonly);
        }


        if (value || me.mode == 'none') {
            if (!hideEl) {
                log.debug("创建隐藏标签");
                inputEl.attr('name', '');
                me.hideEl = hideEl = me.createHideEl();
                hideEl.attr('name', me.name);
                inputEl.after(hideEl);
            }
        }
        else {
            if (hideEl) {
                log.debug("删除隐藏标签");

                inputEl.attr('name', me.name);

                $(hideEl).remove();

                delete me.hideEl;
            }
        }

        $(inputEl).change(function () {
            me.clearInvalid();
        });

        $(inputEl).keyup(function () {
            me.clearInvalid();
        });

        return me;
    }


});