/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />



Mini2.define('Mini2.ui.form.field.Image', {

    extend: 'Mini2.ui.form.field.Base',

    alias: 'widget.image',

    
    //normal | stretchimage | autosize | centerimage | zoom
    sizeMode: 'normal',

    initEvents: function () {


    },

    //图片地址
    value: null,
    

    getFieldEl: function () {

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

        console.log("sizeMode = ", sizeMode);

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
        var me = this,
            inputEl = me.inputEl,
            imgEl = $(inputEl).children('.mi-form-image-item');

        me.value = value;

        imgEl.attr('src', value);

        return value;
    }


});