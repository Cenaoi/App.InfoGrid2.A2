/// <reference path="../Mini2.js" />
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.define('Mini2.data.Errors', {

    extend: 'Mini2.collection.ArrayList',

    isValid: function() {
        return this.length === 0;
    },

    getByField: function (fieldName) {
        var errors = [],
            error, i;

        for (i = 0; i < this.length; i++) {
            error = this.items[i];

            if (error.field == fieldName) {
                errors.push(error);
            }
        }

        return errors;
    }

});


//定义异常信息集合
Mini2.define('Mini2.data.Invalids', {
    
    length: 0,

    items: []


});