/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />



Mini2.define('Mini2.ui.form.field.Text', {

    extend: 'Mini2.ui.form.field.Base',


    alias: 'widget.text',

    size: 20,
    minLength: 0,
    maxLength: Number.MAX_VALUE,
    minLengthText: 'The minimum length for this field is {0}',
    maxLengthText: 'The maximum length for this field is {0}',


    //    initComponent: function () {

    //        var me = this;


    //        me.initLabelable();
    //        //this.constructor.base.initComponent.call(this);

    //        //alert("Mini2.ui.form.field.Text");
    //    },


    initEvents: function () {


    },

    bindInputEl_Mouse: function (inputEl) {

        var me = this;

        $(inputEl)
        .on('mousedown', function (e) {

            me.focus();
            Mini2.EventManager.setMouseDown(me, null, me.scope || me, e, true);
        })
        .on('mouseup', function (e) {
            me.on('mouseup', e);
            Mini2.EventManager.setMouseUp(me, e);
        });

    },

    bindInputEl_Key:function(inputEl){
        var me = this;

        $(inputEl).on('keydown', function (e) {
            //console.log('text = ', e);
            me.on('keydown', e);
        }).on('keyup', function (e) {
            me.on('keyup', e);
        });
    },


    //获取文本框光标位置
    getCursorPosition: function () {
        var me = this,
            inputEl = me.inputEl[0],
            curPos = 0,
            selectItem, range;

        if (inputEl) {

            if (inputEl.selectionStart) {//非IE浏览器
                curPos = inputEl.selectionStart;
            }
            else {//IE
                var selectItem = document.selection;

                if (selectItem) {
                    range = selectItem.createRange();
                    range.moveStart("character", -inputEl.value.length);
                    curPos = range.text.length;
                }
            }
        }

        return curPos;
    },



    //光标是否在开始位置
    isFirstCursorPos: function () {
        var me = this,
            curPos = me.getCursorPosition();

        return curPos == 0;
    },

    //光标是否在尾部
    isLastCursorPos: function () {
        var me = this,
            inputEl = me.inputEl,
            len = $(inputEl).val().length,
            curPos = me.getCursorPosition();

        return curPos == len;
    },


    onChange: function (newVal, oldVal) {


        this.autoSize();



    },


    autoSize: function () {


    }

});