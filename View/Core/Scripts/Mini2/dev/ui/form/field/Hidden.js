

Mini2.define('Mini2.ui.form.field.Hidden', {

    extend: 'Mini2.ui.Component',


    alias: 'widget.hidden',


    initComponent: function () {

        var me = this;

    },





    setValue: function (value) {
        var me = this,
            el = me.el;

        el.val(value);


        log.debug('赋值 hidden.value = ' + me.el.val());

        return me;
    },

    getValue: function () {
        var me = this,
            el = me.el,
            value;

        value =  el.val();
        
        return value;
    },



    baseRender: function () {
        "use strict";
        var me = this,
            el;

        


        if (me.applyTo) {
            
            var srcEl = $(me.applyTo);

            if (srcEl.length) {
                me.el = el = srcEl;
            }
            else {
                me.el = el = $('<input type="hidden" />');

                $(me.applyTo).replaceWith(el);
            }
        }
        else {
            me.el = el = $('<input type="hidden" />');
            $(me.renderTo).append(el);
        }

        el.attr('name', me.name);
        
        if (me.value != undefined) {
            me.setValue(me.value);
            delete me.value;
        }

        return me;
    },

    initValue: Mini2.emptyFn,


    render: function () {
        var me = this;

        me.baseRender();


        me.el.data('me', me);
    }


});
