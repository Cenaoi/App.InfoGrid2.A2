

Mini2.define('Mini2.data.ResultSet', {

    loaded: true,

    count: 0,

    total: 0,

    success: false,

    constructor: function (config) {
        Mini2.apply(this, config);

        /**
        * @property {Number} totalRecords
        * Copy of this.total.
        * @deprecated Will be removed in Ext JS 5.0. Use {@link #total} instead.
        */
        this.totalRecords = this.total;

        if (config.count === undefined) {
            this.count = this.records.length;
        }
    }

});