/// <reference path="../Mini2.js" />
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.define('Mini2.data.Field', {

    isField: true,


    constructor: function (config) {

        var me = this;

        if (Mini2.isString(config)) {
            config = { name: config };
        }

        Mini2.apply(me, config);

    },

    dateFormat: null,

    dateReadFormat: null,

    dateWriteFormat: null,

    useNull: false,

    defaultValue: "",

    mapping: null,

    sortType: null,

    sortDir: "ASC",

    allowBlank: true,

    persist: true

}, function () {
    this.constructor(arguments[0]);
});