



Mini2.define('Mini2.ui.form.Label', {

    extend: 'Mini2.ui.form.field.Base',
    alias: 'widget.label' ,
    mode: 'ENCODE',

    getFieldEl: function () {
        "use strict";
        var me = this,
            inputEl;

        if ('ENCODE' == me.mode) {
            me.inputEl = inputEl = Mini2.$joinStr(['<label role="Label" class="mi-form-field mi-form-label" ',
                'aria-invalid="false" style="width: 100%; "></label>']);


            if (me.name) {
                inputEl.attr('name', me.name);
            }

            inputEl.text(me.value)
            .attr("id", me.clientId)
            .css('text-align', me.align)
            .attr('autocomplete', me.autoComplete);
        }
        else {
            me.inputEl = inputEl = Mini2.$joinStr(['<div role="Label" class="mi-form-field mi-form-label" ',
                'aria-invalid="false" style="width: 100%; "></div>']);
        }


        return inputEl;
    },


    setValue: function (value) {
        "use strict";
        var me = this,
            hideEl = me.hideEl,
            inputEl = me.inputEl;


        //if ('ENCODE' != me.mode) {
        //    alert(value);
        //    inputEl.val(value);
        //}
        //else {
        //    inputEl.html(value);
        //}


        inputEl.val(value);
        inputEl.html(value);

        if (hideEl) {
            hideEl.val(value);
        }

        return me;
    },

    getValue: function () {
        "use strict";
        var me = this,
            hideEl = me.hideEl,
            inputEl = me.inputEl,
            value;

        if (me.readOnly && hideEl) {
            value = hideEl.val();
        }
        else {
            if ('ENCODE' == me.mode) {
                value = inputEl.val();
            }
            else {
                value = inputEl.html();
            }
        }

        return value;
    },

    render: function () {
        "use strict";
        var me = this,
            hideEl,
            value = me.value;


        me.baseRender();

        me.inputEl.removeAttr('name');

        me.hideEl = hideEl = me.createHideEl();

        hideEl.val(value);

        var bodyCls = 'td.mi-form-item-body:first';
        var tableEl = me.getTableContainer();

        var bodyEl = tableEl.find(bodyCls);

        if (me.bodyAlign) {
            bodyEl.css('text-align', me.bodyAlign);
        }

        me.appendCell(tableEl, hideEl, bodyCls);

        me.el.data('me', me);
    }


});