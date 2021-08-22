/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />



Mini2.define('Mini2.ui.form.field.Time', {

    extend: 'Mini2.ui.form.field.Text',

    inputTpl:[
        '<table class="mi-form-trigger-wrap" cellpadding="0" cellspacing="0" style="table-layout: fixed;">',
            '<tr>',
                '<td style="">',
                    '<input type="text" class="mi-form-time-hour" style="width:100%; font-size:15px; border:none; " value="0:0" maxLength="5" />',
                '</td>',
                //'<td style="width:6px;">:</td>',
                //'<td style="width:20px;">',
                //    '<input type="text" class="mi-form-time-minute" style="width:100%;font-size:14px; border:none; " value="00" maxLength="2" />',
                //'</td>',
                '<td style="display:none;">',
                    '<input type="hidden" class="mi-form-timepicker-input" />',
                    
                '</td>',

                //'<td>',
                //    '<div class="mi-form-trigger mi-form-arrow-trigger " role="button"></div>',
                //'</td>',
            '</tr>',
        '</table>'
    ],



    initComponent: function () {
        "use strict";
        var me = this,
            log = me.log;

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
            
        });

        me.initLabelable();
    },

    /**
    * 数值的值
    */
    numVaue: 0,


    getTimeObj :function(value){
        "use strict";
        var me = this;

        if (Mini2.isNumber(value)) {

            var m1 = value % 3600;

            var sec = m1 % 60;

            var hour = (value - m1) / 3600;
            var min = (m1 - sec) / 60;

            return {
                hour: hour,
                minute: min,
                sec: sec
            };
        }
        else {

        }

    },


    /*
    * 设置时间值
    */
    setValue:function(value){
        "use strict";
        var me = this,
            hideEl = me.hideEl;
        
        if (Mini2.isBlank(value)) {

            me.numVaue = null;

            me.hideValueEl.val('');

            me.hourEl.val('');

            return me;
        }

        if (Mini2.isNumber(value)) {

            var timeObj = me.getTimeObj(value);

            
            me.numVaue = value;

            me.hideValueEl.val(value);

            me.hourEl.val(timeObj.hour + ":" + timeObj.minute);

            if (hideEl) {
                hideEl.val(value);
            }
        }
        else {

        }

        return me;
    },


    textToNum:function(valueText){

        var sp = valueText.split(':');

        var numValue = 0;

        numValue += parseInt(sp[0]) * 3600;
        numValue += parseInt(sp[1]) * 60;
       
        if (sp[2]) {
            numValue += parseInt(sp[2]);
        }

        return numValue;
    },

    //获取时间值
    getValue: function (value) {
        "use strict";
        var me = this;

        var srcText = me.hourEl.val();
        
        if (Mini2.isBlank(srcText)) {
            return null;
        }

        me.numVaue = me.textToNum(srcText);

        return me.numVaue;
    },

    
    //textValid: function (textEl) {
    //    "use strict";
    //    var obj = textEl,
    //        reg = /^[\d]+$/g;

    //    if (!reg.test(obj.value)) {
    //        var txt = obj.value;

    //        txt.replace(/[^0-9]+/, function (char, index, val) {//匹配第一次非数字字符
    //            obj.value = val.replace(/\D/g, "");//将非数字字符替换成""
    //            var rtextRange = null;

    //            if (obj.setSelectionRange) {
    //                obj.setSelectionRange(index, index);
    //            }
    //            else {//支持ie
    //                rtextRange = obj.createTextRange();
    //                rtextRange.moveStart('character', index);
    //                rtextRange.collapse(true);
    //                rtextRange.select();
    //            }
    //        });
    //    }
    //},

    getFieldEl: function () {
        "use strict";

        var me = this,
            hideValueEl,
            inputEl, srcInputEl,
            checkedCls = me.checkedCls;


        me.inputEl = inputEl = Mini2.$join(me.inputTpl);

        me.hideValueEl = hideValueEl = inputEl.find('.mi-form-timepicker-input');

        hideValueEl.attr("id", me.clientId);
        hideValueEl.attr('name', me.name);
        hideValueEl.attr('value', me.value);

       
        
        var hourEl = inputEl.find('.mi-form-time-hour');
        //var minEl = inputEl.find('.mi-form-time-minute');

        //hourEl.muBind('keyup', function (e) {
        //    me.textValid(this);
        //});

        me.hourEl = hourEl;

        me.bindInputEl_Mouse(hourEl);



        //minEl.muBind('keyup', function (e) {
        //    me.textValid(this);
        //});


        //me.bindInputEl_Mouse(minEl);

        return inputEl;
    }


});