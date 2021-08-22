
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />


Mini2.define('Mini2.ui.form.field.Radio', {

    extend: 'Mini2.ui.form.field.CheckBox',
    alias: 'widget.radio',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.Radio'),

    alternateClassName: 'Mini2.ui.form.Radio',

    fieldCls: Mini2.baseCSSPrefix + '-form-type-radio',

    focusCls: Mini2.baseCSSPrefix + 'form-radio-focus',

    checkedCls: Mini2.baseCSSPrefix + 'form-cb-checked',


    inputType: 'radio',


    /**
    * 初始化原控件
    **/
    initSrcInput: function () {
        var me = this;

        var srcInputEl = Mini2.$joinStr(['<input type="radio" style="display:none;" />']);
        srcInputEl.attr('name', me.name)
            .attr('value', me.inputValue);

        me.srcInputEl = srcInputEl;
    },

    getFieldEl: function () {

        var me = this,
            inputEl;

        me.initSrcInput();

        me.inputEl = inputEl = Mini2.$joinStr(['<input type="button" class="mi-form-field mi-form-radio mi-form-cb " ',
            'autocomplete="off" ',
            'hidefocus="true" ',
            'aria-invalid="false" ',
            'data-errorqtip="" style="" >']);

        inputEl.attr("id", me.clientId);

        $(inputEl).muBind('mousedown', function (e) {

            me.focus();

            Mini2.EventManager.stopEvent(e);

        }).muBind('mouseup', function (e) {

            var isChecked = me.el.hasClass(me.checkedCls);

            //log.debug('me.checkedCls = ' + me.checkedCls);
            //log.debug('isChecked = '+ isChecked);

            if (!isChecked) {
                me.setValue(true);
            }

            //log.debug('me.checked=' + me.checked);

            Mini2.EventManager.stopEvent(e);
        });


        return inputEl;
    },


    renderForBoxLabel: function (tableEl) {
        "use strict";
        var me = this,
            boxLabel = me.boxLabel;

        if (boxLabel) {
            var boxLabelEl = Mini2.$joinStr([
                '<label class="mi-form-cb-label mi-form-cb-label-after" for="', me.clientId, '" >',
                    boxLabel,
                '</label>'
            ]);

            boxLabelEl.muBind('mousedown', function (e) { me.focus(); });
            boxLabelEl.muBind('mouseup', function (e) {

                if (!me.checked) {
                    me.setValue(!me.checked);
                }

                Mini2.EventManager.stopEvent(e);
            });

            me.boxLableEl = boxLabelEl;

            me.appendCell(tableEl, boxLabelEl, 'td.mi-form-item-body:first');
        }

    }


});