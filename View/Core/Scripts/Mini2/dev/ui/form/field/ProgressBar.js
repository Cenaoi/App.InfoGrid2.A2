/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Core/Scripts/Mini2/dev/Mini.js" />
/// <reference path="../../EventManager.js" />
/// <reference path="../../FocusManager.js" />


/**
* 进度条
* 需要 vue.js 
*/
Mini2.define('Mini2.ui.form.field.ProgressBar', {


    extend: 'Mini2.ui.form.field.Text',
    alias: 'widget.progressbar',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.ProgressBar'),

    allowDecimals: true,

    decimalSeparator: '.',

    submitLocaleSeparator: true,

    decimalPrecision: 2,

    minValue: Number.NEGATIVE_INFINITY,

    maxValue: Number.MAX_VALUE,

    step: 1,


    value: 0,
    defaultValue: 0,

    align: 'right',



    initComponent: function () {
        "use strict";
        var me = this,
            log = me.log;

        $(me).muBind('focusout', function () {


            if (me._focusTimeout) {
                clearTimeout(me._focusTimeout);
                delete me._focusTimeout;
            }


            me.fieldEl.removeClass("mi-form-focus");
            me.el.removeClass("mi-form-trigger-wrap-focus");

            me.clearInvalid();

        });

        me.initLabelable();
    },


    getValue: function () {
        "use strict";
        var me = this,
            defaultValue = me.defaultValue;

        var value = this.constructor.base.getValue.call(this);

        if (value == '') {
            return null;
        }

        value = Mini2.Number.from(value, defaultValue);


        return value;
    },

    setMinValue: function (value) {
        "use strict";
        var me = this,
            allowed;

        me.minValue = Mini2.Number.from(value, Number.NEGATIVE_INFINITY);
        me.toggleSpinners();

        // Build regexes for masking and stripping based on the configured options
        if (me.disableKeyFilter !== true) {
            allowed = me.baseChars + '';

            if (me.allowExponential) {
                allowed += me.decimalSeparator + 'e+-';
            }
            else {
                if (me.allowDecimals) {
                    allowed += me.decimalSeparator;
                }
                if (me.minValue < 0) {
                    allowed += '-';
                }
            }

            allowed = Mini2.String.escapeRegex(allowed);
            me.maskRe = new RegExp('[' + allowed + ']');
            if (me.autoStripChars) {
                me.stripCharsRe = new RegExp('[^' + allowed + ']', 'gi');
            }
        }
    },

    setMaxValue: function (value) {
        this.maxValue = Mini2.Number.from(value, Number.MAX_VALUE);
        this.toggleSpinners();
    },


    toggleSpinners: function () {
        "use strict";
        var me = this,
            value = me.getValue(),
            valueIsNull = value === null,
            enabled;

        // If it's disabled, only allow it to be re-enabled if we are
        // the ones who are disabling it.
        if (me.spinUpEnabled || me.spinUpDisabledByToggle) {
            enabled = valueIsNull || value < me.maxValue;
            me.setSpinUpEnabled(enabled, true);
        }


        if (me.spinDownEnabled || me.spinDownDisabledByToggle) {
            enabled = valueIsNull || value > me.minValue;
            me.setSpinDownEnabled(enabled, true);
        }
    },


    rawToValue: function (rawValue) {
        var value = this.fixPrecision(this.parseValue(rawValue));
        if (value === null) {
            value = rawValue || null;
        }
        return value;
    },


    valueToRaw: function (value) {
        var me = this,
            decimalSeparator = me.decimalSeparator;
        value = me.parseValue(value);
        value = me.fixPrecision(value);
        value = Mini2.isNumber(value) ? value : parseFloat(String(value).replace(decimalSeparator, '.'));
        value = isNaN(value) ? '' : String(value).replace('.', decimalSeparator);
        return value;
    },


    parseValue: function (value) {
        value = parseFloat(String(value).replace(this.decimalSeparator, '.'));
        return isNaN(value) ? null : value;
    },


    fixPrecision: function (value) {
        var me = this,
            nan = isNaN(value),
            precision = me.decimalPrecision;

        if (nan || !value) {
            return nan ? '' : value;
        } else if (!me.allowDecimals || precision <= 0) {
            precision = 0;
        }

        return parseFloat(Mini2.Number.toFixed(parseFloat(value), precision));
    },

    setSpinValue: function (value) {
        var me = this,
            len;

        //        if (me.enforceMaxLength) {
        //            // We need to round the value here, otherwise we could end up with a
        //            // very long number (think 0.1 + 0.2)
        //            if (me.fixPrecision(value).toString().length > me.maxLength) {
        //                return;
        //            }
        //        }
        me.setValue(value);
    },


    onSpinUp: function () {
        var me = this;

        if (!me.readOnly) {
            me.setSpinValue(Mini2.Number.constrain(me.getValue() + me.step, me.minValue, me.maxValue));
        }
    },


    onSpinDown: function () {
        var me = this;

        if (!me.readOnly) {
            me.setSpinValue(Mini2.Number.constrain(me.getValue() - me.step, me.minValue, me.maxValue));
        }
    },

    spinUpEnabled: function (enabled) {
        "use strict";
        var me = this;


        if (enabled) {
            me.onSpinUp();

            if (!me.spinUpDisabledByToggle) {
                me.spinUpDisabledByToggle = setInterval(function () {
                    me.onSpinUp();
                }, 200);
            }
        }
        else {

            if (me.spinUpDisabledByToggle) {
                clearInterval(me.spinUpDisabledByToggle);

                delete me.spinUpDisabledByToggle;
            }
        }
    },

    spinDownEnabled: function (enabled) {
        "use strict";
        var me = this;


        if (enabled) {
            me.onSpinDown();

            if (!me.spinDownDisabledByToggle) {

                me.spinDownDisabledByToggle = setInterval(function () {
                    me.onSpinDown();
                    //log.debug(me.getValue());
                }, 200);
            }
        }
        else {

            if (me.spinDownDisabledByToggle) {
                clearInterval(me.spinDownDisabledByToggle);

                delete me.spinDownDisabledByToggle;
            }
        }

    },

    btnUp_Click: function (e) {
        var me = e.data;

        me.onSpinUp();
        me.focus();
        Mini2.EventManager.stopEvent(e);
    },

    btnDown_Click: function (e) {
        var me = e.data;

        me.onSpinDown();
        me.focus();
        Mini2.EventManager.stopEvent(e);
    },




    getFieldEl: function () {
        "use strict";
        var me = this;

        //var tableEl = Mini2.$joinStr([
        //    '<table class="mi-form-trigger-wrap" cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%; ">',
        //        '<tbody>',
        //            '<tr>',
        //                '<td class="mi-form-trigger-input-cell" style="width: 100%; "></td>',
        //            '</tr>',
        //        '</tbody>',
        //    '</table>']);

        //var inputEl = Mini2.$joinStr([
        //    '<div class="mi-progress">',
        //        '<span class="blue" style="width: 60%;"><span>60%</span></span>',
        //    '</div>'
        //]);

        var inputEl = '<mi-progressbar :ex_cls="ex_cls"  :value="value"></mi-progressbar>';


        //inputEl.attr("id", me.clientId);


        //inputEl.val(me.value);

        if (me.name) {
            //inputEl.attr('name', me.name);
        }

        //me.appendCell(tableEl, inputEl, 'td.mi-form-trigger-input-cell');

        me.inputEl = inputEl;



        return inputEl;
    },


    setValueCartoon:function(value){

        var me = this,
            vue = me.vue,
            oldValue = me.value || 0,
            newValue = value;

        new TWEEN.Tween({ num: oldValue })
          .easing(TWEEN.Easing.Quadratic.Out)
          .to({ num: newValue }, 500)
          .onUpdate(function () {
              vue.value = this.num.toFixed(0);
          })
          .start();
    },

    setValue:function(value){
        var me = this,
            vue = me.vue;

        if (vue && TWEEN) {
            me.setValueCartoon(value);
        }

        me.value = value;

        return me;
    },

    getValue:function(){
        var me = this;
        return me.value;
    },

    elemTpl: ['<div class="mi-progress">',
                '<span :class="[ex_cls]" style="min-width:0px;" :style="{width: value + \'%\'}">',
                    '<span>{{value}}%</span>',
                '</span>',
            '</div>'],


    /**
    * 注册 VUE 组件
    *
    */
    regVue:function(option){
        var me = this,
            cptName = 'mi-progressbar';


        Mini2.vue = Mini2.vue || {};
        Mini2.vue.component = Mini2.vue.component || {};

        if (!Mini2.vue.component[cptName]) {

            // 定义
            var miProgress = Vue.extend({
                template: Mini2.join( me.elemTpl),
                props: ['ex_cls', 'value'],
                methods: {

                }
            });

            // 注册
            Vue.component(cptName, miProgress);

            Mini2.vue.component[cptName] = true;
        }


        // 创建根实例
        me.vue = new Vue({
            el: '#' + me.id,
            data: option
        });


    },

    render: function () {
        var me = this;

        var value = me.value;

        me.baseRender();

        me.el.attr('id', me.id);

        //注册 vue 组件
        me.regVue({
            ex_cls: 'blue',
            value: 0
        });

        me.setValue(value);

        me.el.data('me', me);

    }


});
