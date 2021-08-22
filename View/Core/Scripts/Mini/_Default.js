/// <reference path="jquery-1.4.1-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery-jtemplates_uncompressed.js" />
/// <reference path="jquery.form.js" />
/// <reference path="../SWFUpload/swfupload.js" />
/// <reference path="../SWFUpload/handlers.js" />
/// <reference path="jquery-ui-1.8.9.custom.min.js" />

$.format = function (source, params) {
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
    });
    return source;
};

String.prototype.startsWith = function (str) {
    if (str == null) return false;

    return (this.substr(0, str.length) == str);
}



String.prototype.endWith = function (str) {
    if (str == null) return false;
    return (this.substr(this.length - str.length) == str);
}



Date.prototype.format = function (formatter) {
    if (!formatter || formatter == "") {
        formatter = "yyyy-MM-dd";
    }
    var year = this.getYear().toString();
    var month = (this.getMonth() + 1).toString();
    var day = this.getDate().toString();
    var yearMarker = formatter.replace(/[^y|Y]/g, '');
    if (yearMarker.length == 2) {
        year = year.substring(2, 4);
    }
    var monthMarker = formatter.replace(/[^m|M]/g, '');
    if (monthMarker.length > 1) {
        if (month.length == 1) {
            month = "0" + month;
        }
    }
    var dayMarker = formatter.replace(/[^d]/g, '');
    if (dayMarker.length > 1) {
        if (day.length == 1) {
            day = "0" + day;
        }
    }
    return formatter.replace(yearMarker, year).replace(monthMarker, month).replace(dayMarker, day);
}

Date.prototype.toLongTimeString = function () {

    var str = this.getYear() + "-" +
                (this.getMonth() + 1) + "-" +
                this.getDate() + " " +
                this.getHours() + ":" +
                this.getMinutes() + ":" +
                this.getSeconds() + "." + 
                this.getMilliseconds();

    return str;
}



Mini = { version: '3.1' };

Mini.namespace = function () {
    var a = arguments, o = null, i, j, d, rt;
    for (i = 0; i < a.length; ++i) {
        d = a[i].split(".");
        rt = d[0];
        eval('if (typeof ' + rt + ' == "undefined"){' + rt + ' = {};} o = ' + rt + ';');
        for (j = 1; j < d.length; ++j) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
}

Mini.ns = Mini.namespace;

Mini.ns("Mini", "Mini.ui", "Mini.util", "Mini.ui.desings");

Mini.JsonHelper = {};

Mini.JsonHelper.toJson = function (o) {
    if (o == undefined) {
        return "";
    }

    var r = [];
    if (typeof o == "string") return "\"" + o.replace(/([\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
    if (typeof o == "object") {
        if (!o.sort) {
            for (var i in o)
                r.push("\"" + i + "\":" + Mini.JsonHelper.toJson(o[i]));
            if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                r.push("toString:" + o.toString.toString());
            }
            r = "{" + r.join() + "}"
        } else {
            for (var i = 0; i < o.length; i++)
                r.push(Mini.JsonHelper.toJson(o[i]))
            r = "[" + r.join() + "]";
        }
        return r;
    }
    return o.toString().replace(/\"\:/g, '":""');
};



Mini.globel = {};

