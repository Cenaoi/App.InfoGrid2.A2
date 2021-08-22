/// <reference path="define.js" />


Mini2.define('Mini2.ui.Editor', {

    extend: 'Mini2.ui.Component',
    

    value: '',

    offsets: [0, 0],

    cancelOnEsc: true,

    focusOnToFront: false,

    hidden: true,

    field: null,



    initComponent: function () {


    },

    afterRender: function (ct, position) {
        
    },

    startEdit: function (el, value) {
        /// <summary>开始编辑</summary>

    },


    onShow: function () {

    },

    cancelEdit: function (remainVisible) {
        /// <summary>取消编辑</summary>
    
    
    },


    hideEdit: function (remainVisible) {
        /// <summary>隐藏编辑</summary>



    },


    onHide: function () {



    },

    setValue : function(value) {
        this.field.setValue(value);
    },

    getValue : function() {
        return this.field.getValue();
    }
});