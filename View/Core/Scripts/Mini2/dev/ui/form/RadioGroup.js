/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />
/// <reference path="../../Mini-more.js" />


Mini2.define('Mini2.ui.form.RadioGroup', {

    extend: 'Mini2.ui.form.CheckboxGroup',
    alias: 'widget.radiogroup',

    log: Mini2.Logger.getLogger('Mini2.ui.form.RadioGroup',false),

    defaultType: 'radiofield',

    defaultItemType: 'Mini2.ui.form.field.Radio',

    groupCls: Mini2.baseCSSPrefix + 'form-radio-group',

    // private
    extraFieldBodyCls: Mini2.baseCSSPrefix + 'form-radiogroup-body',

    // private
    layout: 'radiogroup',

    componentCls: Mini2.baseCSSPrefix + 'form-radiogroup',

    //aaa: bindItem
    bindItem: function (item) {
        "use strict";
        var me = this,
            log = me.log;

        $(item).on('change', this, function (e, newValue, oldValue) {

            if (newValue) {
                me.setCheck(this);
            }


            var lastData = item.getValue();

            //上锁,防止触发事件导致'值倒灌'.
            me.locked = true;

            try {
                $(me).muTriggerHandler('changed', [{
                    sender: this,    //原焦点控件
                    target: null,        //新获取焦点控件
                    data: lastData      //原附带的数据
                }]);
            }
            catch (ex) {
                console.error('触发 changed 事件错误', ex);
            }

            me.locked = false;
        });
    },

    setCheck: function (checkItem) {
        "use strict";

        var me = this,
            item,
            items = me.items,
            len = items.length,
            i = len;

        while (i--) {
            item = items[i];
            if (item !== checkItem) {
                item.setValue(false);
            }
        }

        return me;
    },

    setValue: function (value) {
        "use strict";
        var me = this,
            log = me.log,
            item,
            items = me.items,
            len = items.length,
            isEqual,
            i;

        console.trace();

        console.group("setValue=" + value);

        for (i = 0; i < len; i++) {

            item = items[i];

            isEqual = (item.inputValue == value);


            console.debug(this.id + "初始化......." + item.inputValue + "....", isEqual);
            

            if (isEqual) {
                console.debug("item = ", item);
                item.setValue(isEqual);
                break;
            }
        }

        console.groupEnd();
        
        me.trigger('changed', {
            value: value
        });


        return me;
    }


});