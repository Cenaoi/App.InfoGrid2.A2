/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />


Mini2.define('Mini2.ui.form.field.Number', {
    extend: 'Mini2.ui.form.field.Text',
    alias: 'widget.number',

    log : Mini2.Logger.getLogger('Mini2.ui.form.field.Number'),

    allowDecimals: true,

    decimalSeparator: '.',

    submitLocaleSeparator: true,

    decimalPrecision: 2,

    minValue: -1000000000,

    maxValue: 1000000000,

    step: 1,

    /**
    * 方向(水平,垂直)
    * h v
    */
    direction: 'h', //'horizontal',

    value: 0,
    defaultValue: 0,

    align: 'right',


    initComponent: function () {
        "use strict";
        var me = this,
            log = me.log;

        $(me).muBind('focusout', function () {

            var oldValue = me.getValue();

            var value = Mini2.Number.from(oldValue, me.defaultValue);

            if (value < me.minValue)
            {
                me.setValue(me.minValue);
            }
            else if (value > me.maxValue) {
                me.setValue(me.maxValue);
            }
            else {
                me.setValue(value);
            }

            
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
            return 0;
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




    bindInputEl_Key: function (inputEl) {
        "use strict";
        var me = this;

        $(inputEl)
        .on('keydown', function (e) {

            var keyCode = e.keyCode ;


            //107= +加按钮, 109= -减按钮

            if (107 == keyCode) {   //+ 按钮
                me.onSpinUp();
                Mini2.EventManager.stopEvent(e);
            }
            else if (109 == keyCode) { //-按钮
                me.onSpinDown();
                Mini2.EventManager.stopEvent(e);
            }
            //else if(37 == keyCode){ //左

            //    if (!me.isFirstCursor()) {

            //        Mini2.EventManager.stopEvent(e);
            //    }
            //}
            //else if (39 == keyCode) { //右

            //    if (!me.isLastCursor()) {

            //        Mini2.EventManager.stopEvent(e);
            //    }
            //}
            else {
                //log.debug(" - " + e.keyCode);


                me.on('keydown', e);
            }

            


        })
        .on('keyup', function (e) {


            me.on('keyup', e);


        })
        .on('keypress', function (e) {

            //            var keyCode = e.keyCode;

            //            log.debug(" - " + e.keyCode);

            me.on('keypress', e);
        });
    },


    getFieldEl_ForComputer:function(){
        "use strict";
        var me = this,
            inputEl;

        var tableEl = Mini2.$join([
            '<table class="mi-form-trigger-wrap" cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%; ">',
                '<tbody>',
                    '<tr>',
                        '<td class="mi-form-trigger-input-cell" style="width: 100%; "></td>',
                        '<td valign="top" class="mi-trigger-cell"></td>',
                    '</tr>',
                '</tbody>',
            '</table>']);


        //if (Mini2.support.webkit) {
        //    inputEl = Mini2.$join([
        //        '<input type="number" class="mi-form-field mi-form-required-field mi-form-text" ',
        //            'autocomplete="off" aria-invalid="false" data-errorqtip="" style="width: 100%; " min="', me.mixValue ,'" max="',me.maxValue,'" >']);
        //}
        //else {
        inputEl = Mini2.$join([
            '<input type="text" class="mi-form-field mi-form-text" ',
                'autocomplete="off" aria-invalid="false"  style="width: 100%; ">']);
        //}

        //mi-form-required-field

        var btnUpEl = Mini2.$join(['<div class="mi-trigger-index-0 mi-form-trigger mi-form-spinner-up" role="button"></div>']);
        var btnDownEl = Mini2.$join(['<div class="mi-trigger-index-1 mi-form-trigger mi-form-spinner-down" role="button"></div>']);

        inputEl.attr("id", me.clientId);


        inputEl.val(me.value);

        if (me.name) {
            inputEl.attr('name', me.name);
        }

        me.appendCell(tableEl, inputEl, 'td.mi-form-trigger-input-cell');
        me.appendCell(tableEl, btnUpEl, 'td.mi-trigger-cell');
        me.appendCell(tableEl, btnDownEl, 'td.mi-trigger-cell');


        btnUpEl.muBind('mousedown', me, function (e) {
            me.focus();
            me.spinUpEnabled(true);
        });

        btnUpEl.muBind('mouseup', me, function (e) {
            me.spinUpEnabled(false);
            Mini2.EventManager.stopEvent(e);
        });

        btnDownEl.muBind('mousedown', me, function () {
            me.focus();
            me.spinDownEnabled(true);
        });
        btnDownEl.muBind('mouseup', me, function (e) {
            me.spinDownEnabled(false);
            Mini2.EventManager.stopEvent(e);
        });

        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);

        //me.bindInputEl_Mouse2(inputEl);

        me.inputEl = inputEl;


        return tableEl;

    },


    /**
     * 触摸控件
     */
    getFieldEl_ForTouch: function () {
        "use strict";
        var me = this,
            inputEl;

        var tableEl = Mini2.$join([
            '<table class="mi-form-trigger-wrap mi-form-trigger-wrap-touch" cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%;padding:0px; ">',
                '<tbody>',
                    '<tr>',
                        '<td valign="" class="mi-trigger-cell mi-trigger-cell-left mi-trigger-down" ></td>',
                        '<td class="mi-form-trigger-input-cell" style="width: 100%; "></td>',
                        '<td valign="" class="mi-trigger-cell mi-trigger-cell-right mi-trigger-up" ></td>',
                    '</tr>',
                '</tbody>',
            '</table>']);

        inputEl = Mini2.$join([
            '<input type="text" class="mi-form-field mi-form-text" ',
                'autocomplete="off" aria-invalid="false"  style="width: 100%; ">']);


        var btnUpEl = Mini2.$join(['<div class="mi-trigger-index-0 mi-form-trigger fa fa-plus" role="button"></div>']);
        var btnDownEl = Mini2.$join(['<div class="mi-trigger-index-1 mi-form-trigger  fa fa-minus"  role="button"></div>']);

        inputEl.attr("id", me.clientId);


        inputEl.val(me.value);

        if (me.name) {
            inputEl.attr('name', me.name);
        }

        me.appendCell(tableEl, inputEl, 'td.mi-form-trigger-input-cell');
        me.appendCell(tableEl, btnUpEl, 'td.mi-trigger-up');
        me.appendCell(tableEl, btnDownEl, 'td.mi-trigger-down');


        btnUpEl.muBind('mousedown', me, function (e) {
            me.focus();
            me.spinUpEnabled(true);
        });

        btnUpEl.muBind('mouseup', me, function (e) {
            me.spinUpEnabled(false);
            Mini2.EventManager.stopEvent(e);
        });

        btnDownEl.muBind('mousedown', me, function () {
            me.focus();
            me.spinDownEnabled(true);
        });
        btnDownEl.muBind('mouseup', me, function (e) {
            me.spinDownEnabled(false);
            Mini2.EventManager.stopEvent(e);
        });

        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);

        //me.bindInputEl_Mouse2(inputEl);

        me.inputEl = inputEl;


        return tableEl;

    },



    /**
     * 触摸控件 (垂直的)
     */
    getFieldEl_ForTouch_V: function () {
        "use strict";
        var me = this,
            inputEl;

        var tableEl = Mini2.$join([
            '<table class="mi-form-trigger-wrap mi-form-trigger-wrap-vertical" cellpadding="0" cellspacing="0" style="table-layout: fixed;padding:0px; ">',
                '<tbody>',
                    '<tr>',
                        '<td valign="" class="mi-trigger-cell mi-trigger-cell-top mi-trigger-up" style="background-color:#EEE;" ></td>',
                    '</tr>',
                    '<tr>',
                        '<td class="mi-form-trigger-input-cell" style="width: 100%; "></td>',
                    '</tr>',
                    '<tr>',
                        '<td valign="" class="mi-trigger-cell mi-trigger-cell-bottom mi-trigger-down" style="background-color:#EEE;" ></td>',
                    '</tr>',
                '</tbody>',
            '</table>']);

        inputEl = Mini2.$join([
            '<input type="text" class="mi-form-field mi-form-text" ',
                'autocomplete="off" aria-invalid="false"  style="width: 100%; ">']);


        var btnUpEl = Mini2.$join(['<div class="mi-trigger-index-0 mi-form-trigger mi-form-spinner-up" role="button" style="margin: auto;background-color:#EEE;"></div>']);
        var btnDownEl = Mini2.$join(['<div class="mi-trigger-index-1 mi-form-trigger  mi-form-spinner-down"  role="button" style="margin: auto;background-color:#EEE;"></div>']);

        inputEl.attr("id", me.clientId);


        inputEl.val(me.value);

        if (me.name) {
            inputEl.attr('name', me.name);
        }

        me.appendCell(tableEl, inputEl, 'td.mi-form-trigger-input-cell');
        me.appendCell(tableEl, btnUpEl, 'td.mi-trigger-up');
        me.appendCell(tableEl, btnDownEl, 'td.mi-trigger-down');


        btnUpEl.muBind('mousedown', me, function (e) {
            me.focus();
            me.spinUpEnabled(true);
        });

        btnUpEl.muBind('mouseup', me, function (e) {
            me.spinUpEnabled(false);
            Mini2.EventManager.stopEvent(e);
        });

        btnDownEl.muBind('mousedown', me, function () {
            me.focus();
            me.spinDownEnabled(true);
        });
        btnDownEl.muBind('mouseup', me, function (e) {
            me.spinDownEnabled(false);
            Mini2.EventManager.stopEvent(e);
        });

        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);

        //me.bindInputEl_Mouse2(inputEl);

        me.inputEl = inputEl;


        return tableEl;

    },


    getFieldEl: function () {
        var me = this,
            tableEl ,
            deviceType = me.deviceType;

        
        if ('auto' === deviceType) {

            if ('touch' == Mini2.SystemInfo.mode) {

                deviceType = 'touch';

            }

        }
        
        if ('v' === me.direction || 'vertical' === me.direction) {

            tableEl = me.getFieldEl_ForTouch_V();
        }
        else {
            if ('auto' === deviceType || 'computer' === deviceType) {
                //电脑模式
                tableEl = me.getFieldEl_ForComputer();
            }
            else if ('touch' === deviceType) {
                //触摸模式

                tableEl = me.getFieldEl_ForTouch();
            }
        }


        return tableEl;
    },

    render: function () {
        var me = this;

        me.baseRender();

        //me.inputEl.css('padding-right', '0px');
        
        me.el.attr('data-muid', me.muid);

        me.el.data('me', me);

    }


});
