
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />


Mini2.define('Mini2.ui.form.field.CheckBox', {

    extend: 'Mini2.ui.form.field.Base',


    alias: 'widget.checkbox',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.CheckBox',false),

    alternateClassName: 'Mini2.ui.form.Checkbox',

    fieldCls: Mini2.baseCSSPrefix + '-form-type-checkbox',

    focusCls: Mini2.baseCSSPrefix + 'form-checkbox-focus',

    checkedCls: Mini2.baseCSSPrefix + 'form-cb-checked',

    // private
    extraFieldBodyCls: Mini2.baseCSSPrefix + 'form-checked-body',

    componentLayout: 'field',

    isCheckbox: true,
    
    checked: null,


    textEl: null, //标签文本对象
    //true 文本标签
    trueText: null,

    //false 标签
    falseText: null,


    inputValue: 'on',

    inputType: 'checkbox',

    inputTypeAttr: 'button',

    originalValue: false,

    lastValue: false,

    initComponent: function () {
        "use strict";
        var me = this;

        $(me).muBind('focusout', function () {


            me.fieldEl.removeClass("mi-form-focus")
                .removeClass(me.focusCls);

            me.el.removeClass("mi-form-trigger-wrap-focus");

        });

        me.initLabelable();

        //        if (me.checked) {
        //            me.baseSetValue(true);
        //        }
    },


    initValue: function () {
        "use strict";
        var me = this,
            checked = !!me.checked;

        /**
        * @property {Object} originalValue
        * The original value of the field as configured in the {@link #checked} configuration, or as loaded by the last
        * form load operation if the form's {@link Ext.form.Basic#trackResetOnLoad trackResetOnLoad} setting is `true`.
        */
        me.originalValue = me.lastValue = checked;

        // Set the initial checked state
        me.setValue(checked, false);

        if (me.textEl) {
            var txt = checked ? me.trueText : me.falseText;
            me.textEl.html(txt);
        }

    },


    //private
    baseSetValue: function (value, triggerChangedEvent) {
        "use strict";
        var me = this,
            el = me.el,
            srcInputEl = me.srcInputEl,
            checkedCls = me.checkedCls,
            checked,
            originalValue = me.originalValue;

        //if (value) {
        //    el.addClass(checkedCls);
        //}
        //else {
        //    el.removeClass(checkedCls);
        //}

        me.lastValue = me.checked;

        me.checked = checked = !!value;


        $(el).toggleClass(checkedCls, checked);

        if (srcInputEl) {
            srcInputEl.prop('checked', checked);
        }



        if (false !== triggerChangedEvent) {
            me.trigger('changed', {
                value: checked
            });
        }
    },

    setValue: function (value, triggerChangedEvent) {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            srcInputEl = me.srcInputEl,
            checkedCls = me.checkedCls,
            checked,
            originalValue = me.originalValue;

        value = !!value;

        if (me.checked === value && $(el).hasClass(checkedCls) ) {


            return;
        }

        me.lastValue = me.checked;

        me.checked = checked = value;
        
        $(el).toggleClass(checkedCls, checked);
        
        if (srcInputEl) {
            srcInputEl.prop('checked', checked);
        }

        //if (value) {
        //    $(me.inputEl).children().removeClass('switch-off').addClass('switch-on');
        //}
        //else {
        //    $(me.inputEl).children().removeClass('switch-on').addClass('switch-off');

        //}

        if (me.textEl) {

            var txt = value ? me.trueText : me.falseText;

            me.textEl.html(txt);
        }

        if (checked != originalValue) {
            
            if (false !== triggerChangedEvent) {
                me.onChange(checked, originalValue);
            }

            me.originalValue = checked;
        }



        if (false !== triggerChangedEvent) {
            me.trigger('changed', {
                value: checked
            });
        }


        return me;
    },

    getValue: function () {
        return this.checked;
    },


    onChange: function (newVal, oldVal) {
        "use strict";

        var me = this;

        if (Mini2.isFunction(me.change)) {
            me.change.call(me, newVal, oldVal);
        }

        $(me).triggerHandler('change', [newVal, oldVal]);

    },


    renderForLableable: function (tableEl) {
        "use strict";
        var me = this,
            labelable = me.labelable,
            labelEl;

        if (labelable && !labelable.hideLabel) {
            labelEl = labelable.getEl();

            if (me.clientId) {
                labelEl.attr("for", me.clientId);
            }

            labelEl.muBind('mousedown', function (e) { me.focus(); });
            //labelEl.muBind('mouseup', function (e) {

            //    me.setValue(!me.checked);
            //    Mini2.EventManager.stopEvent(e);
            //});


            me.appendCell(tableEl, labelEl, 'td.mi-field-label-cell:first');

            if (me.checked) {
                tableEl.addClass('mi-form-cb-checked');
            }
        }
    },


    renderForBoxLabel: function (tableEl) {
        "use strict";
        var me = this,
            boxLabel = me.boxLabel;

        if (boxLabel) {
            var boxLabelEl = Mini2.$joinStr([
                '<label class="mi-form-cb-label mi-form-cb-label-after" " >',
                    boxLabel,
                '</label>'
            ]);

            boxLabelEl.attr('for', me.clientId);

            boxLabelEl.muBind('mousedown', function (e) { me.focus(); });
            boxLabelEl.muBind('mouseup', function (e) {

                me.setValue(!me.checked);
                Mini2.EventManager.stopEvent(e);
            });

            me.boxLableEl = boxLabelEl;

            me.appendCell(tableEl, boxLabelEl, 'td.mi-form-item-body:first');
        }

    },

    /**
    * 初始化原控件
    **/
    initSrcInput: function () {
        "use strict";
        var me = this,
            srcInputEl;

        me.srcInputEl = srcInputEl = Mini2.$join(['<input type="checkbox" style="display:none;" />']);
        srcInputEl.attr('name', me.name)
            .attr('value', me.inputValue);

        if (me.checked) {
            srcInputEl.prop('checked', true);
        }
    },


    //扩展
    getBodyExpand:function(){
        var me = this,
            textEl,
            trueText = me.trueText,
            falseText = me.falseText;

        if (trueText || falseText) {

            textEl = $('<label style="white-space: nowrap;margin-top: 4px;margin-left: 4px;position: absolute; "></label>');

            textEl.attr('for', me.clientId);

            textEl.muBind('mousedown', function (e) { me.focus(); });
            textEl.muBind('mouseup', function (e) {

                me.setValue(!me.checked);
                Mini2.EventManager.stopEvent(e);
            });

            me.textEl = textEl;

            return textEl;
        }


    },

    getFieldEl: function () {
        "use strict";

        var me = this,
            inputEl, srcInputEl,
            checkedCls = me.checkedCls;

        me.initSrcInput();

        me.inputEl = inputEl = Mini2.$joinStr(['<input type="button" class="mi-form-field mi-form-checkbox mi-form-cb " ',
            'autocomplete="off" ',
            'hidefocus="true" ',
            'aria-invalid="false" ',
            'data-errorqtip="" style="" >']);


        inputEl.attr("id", me.clientId);



        $(inputEl).muBind('mousedown', function (e) {

            me.focus();

            Mini2.EventManager.stopEvent(e);

        }).muBind('mouseup', function (e) {

            var isChecked = me.checked; // me.el.hasClass(me.checkedCls);

            me.setValue(!isChecked);

            Mini2.EventManager.stopEvent(e);
        });




        //var tip = [
        //    '<div class="switch has-switch">',
        //        '<div class="switch-on switch-animate">',
        //            '<input type="checkbox" checked="">',
        //            '<span class="switch-left switch-small">是</span>',
        //            '<label class="switch-small">&nbsp;</label>',
        //            '<span class="switch-right switch-small">否</span>',
        //        '</div>',
        //    '</div>'];


        //me.inputEl = inputEl = Mini2.$joinStr(tip);


        //$(inputEl).muBind('mousedown', function (e) {

        //    me.focus();

        //    Mini2.EventManager.stopEvent(e);

        //}).muBind('mouseup', function (e) {

        //    var isChecked = me.checked; // me.el.hasClass(me.checkedCls);

        //    isChecked = !isChecked;

        //    me.setValue(isChecked);
            
        //    Mini2.EventManager.stopEvent(e);
        //});

        return inputEl;
    }


});
