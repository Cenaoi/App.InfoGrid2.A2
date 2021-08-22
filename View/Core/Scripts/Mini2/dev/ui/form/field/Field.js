

Mini2.define('Mini2.ui.form.field.Field', {

    value:"",

    disabled:false,

    submitValue: true,

    validateOnChange: true,

    initField: function () {


    },


    initValue: function () {


    },


    
    getName: function() {
        return this.name;
    },


    
    getValue: function() {
        return this.value;
    },


    setValue: function(value) {
        var me = this;
        me.value = value;
        me.checkChange();
        return me;
    },


    isEqual: function(value1, value2) {
        return String(value1) === String(value2);
    },


    isEqualAsString: function(value1, value2){
        return String(Mini2.value(value1, '')) === String(Mini2.value(value2, ''));
    },


    getSubmitData: function() {
        var me = this,
            data = null;
        if (!me.disabled && me.submitValue && !me.isFileUpload()) {
            data = {};
            data[me.getName()] = '' + me.getValue();
        }
        return data;
    },



    getModelData: function() {
        var me = this,
            data = null;

        if (!me.disabled && !me.isFileUpload()) {
            data = {};
            data[me.getName()] = me.getValue();
        }
        return data;
    },


    reset : function(){
        var me = this;

        me.beforeReset();
        me.setValue(me.originalValue);
        me.clearInvalid();
        // delete here so we reset back to the original state
        delete me.wasValid;
    },
    

    
    beforeReset: Mini2.emptyFn,

    
    resetOriginalValue: function() {
        this.originalValue = this.getValue();
        this.checkDirty();
    },



    checkChange: function () {
        "use strict";
        if (!this.suspendCheckChange) {
            var me = this,
                newVal = me.getValue(),
                oldVal = me.lastValue;
            if (!me.isEqual(newVal, oldVal) && !me.isDestroyed) {
                me.lastValue = newVal;
                me.fireEvent('change', me, newVal, oldVal);
                me.onChange(newVal, oldVal);
            }
        }
    },


    onChange: function(newVal, oldVal) {
        if (this.validateOnChange) {
            this.validate();
        }
        this.checkDirty();
    },


    getErrors: function(value) {
        return [];
    },


    isValid : function() {
        var me = this;
        return me.disabled || Mini2.isEmpty(me.getErrors());
    },

    isFileUpload: function() {
        return false;
    },

    markInvalid: Mini2.emptyFn,

    clearInvalid: Mini2.emptyFn
});