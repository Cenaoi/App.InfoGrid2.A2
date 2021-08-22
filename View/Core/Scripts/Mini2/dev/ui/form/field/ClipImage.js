/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />


// 图像剪辑

Mini2.define('Mini2.ui.form.field.ClipImage', {

    extend: 'Mini2.ui.form.field.Trigger',

    alias: 'widget.clipimage',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.ClipImage', false),

    //normal | stretchimage | autosize | centerimage | zoom
    sizeMode: 'normal',

    initEvents: function () {


    },

    //绑定显示的字段名
    displayField: 'text',
    
    //绑定值的字段名
    valueField: 'value',

    //图片地址
    value: null,

    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this,
            log = me.log;



        me.initStore();

        me.initLabelable();
    },





    getFieldEl: function () {
        "use strict";
        var me = this,
            sizeMode = me.sizeMode,
            inputEl;

        me.inputEl = inputEl = Mini2.$joinStr([
            '<div class="mi-form-field mi-form-image" style="width:100%;height:auto; " >',
                '<img class="mi-form-image-item" src="" />',
            '</div>']);

        inputEl.attr("id", me.clientId);

        var imgEl = $(inputEl).children('.mi-form-image-item');

        imgEl.attr("src", me.value);

        //console.log("sizeMode = ", sizeMode);

        //normal | stretchimage | autosize | centerimage | zoom
        if ('normal' == sizeMode) {
            inputEl.css('overflow', 'hidden');
        }
        else if ('zoom' == sizeMode) {
            inputEl.css('width', '100%');

            imgEl.css({ width: '100%' });
        }

        if (me.maxHeight) {
            inputEl.css('max-height', me.maxHeight);
        }

        if (me.height) {
            inputEl.css('height', me.height);
                       
        }



        $(inputEl).muBind('mousedown', function (e) {

            me.focus();

            Mini2.EventManager.stopEvent(e);

        }).muBind('mouseup', function (e) {

            Mini2.EventManager.stopEvent(e);
        });


        return inputEl;
    },


    setValue: function (value) {
        "use strict";
        var me = this,
            log = me.log,
            inputEl = me.inputEl,
            imgEl = $(inputEl).children('.mi-form-image-item'),
            record,
            store = me.store,
            imgSrc,
            displayField = me.displayField,
            valueField = me.valueField;

        me.value = value;

        //log.debug("setValue = ", value);

        record = store.filterFirstBy(valueField, value);

        //log.debug("record ", record);

        if (record) {
            me.curRecord = record;
            
            imgSrc = record.get(displayField);

        }
        else {
            imgSrc = null;
        }

        //log.debug("imgSrc = " ,imgSrc);

        imgEl.attr('src', imgSrc);

        return me;
    }

});