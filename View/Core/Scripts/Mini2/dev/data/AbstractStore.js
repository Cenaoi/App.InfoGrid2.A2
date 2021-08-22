
/// <reference path="../Mini2.js" />
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.define('Mini2.data.Store', {


    log: Mini2.Logger,  //日志管理器

    getNewRecords: function () {
        return [];
    },

    getUpdatedRecords: function() {
        return [];
    },

    getModifiedRecords : function(){
        //return [].concat(this.getNewRecords(), this.getUpdatedRecords());
    },

    getRemovedRecords: function() {
        return this.removed;
    },

    filter: function(filters, value) {

    },

    getCount: Mini2.emptyFn,

    removeAll: Mini2.emptyFn
});