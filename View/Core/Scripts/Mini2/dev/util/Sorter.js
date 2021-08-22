

Mini2.define('Mini2.util.Sorter', {

    direction: "ASC",

    constructor: function (config) {
        var me = this;

        Mini2.apply(me, config);

        alert("dddddddddddddddddddddddddddd");

        //me.updateSortFunction();
    },

    serialize: function () {
        return {
            root: this.root,
            property: this.property,
            direction: this.direction
        };
    }

});