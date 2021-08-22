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


Mini.ui.SplitContainer = function (options) {

    var defaults = {
        id: '',
        owner: null,
        FixedPanel: 'Panel1',
        Orientation: 'Vertical',
        Panel1Size: 200,
        Panel2Size: 200,
        SplitSize: 4,
        OffsetHeight: 0,
        OffsetWidth: 0,

        Dock: "Full"
    };

    var m_Owner;

    var m_Container;

    var m_Panel1;
    var m_Panel2;

    var m_Split;

    function init(options) {
        defaults = $.extend(defaults, options);

        m_Container = $("#" + defaults.id);

        if (defaults.owner) {
            m_Owner = defaults.owner;
        }
        else {
            m_Owner = $(m_Container).parent();
        }

        m_Panel1 = $(m_Container).children(".Panel1:first");
        m_Panel2 = $(m_Container).children(".Panel2:first");
        m_Split = $(m_Container).children(".Split:first");

        m_Panel1.css("overflow", "auto");
        m_Panel2.css("overflow", "auto");



        $(m_Owner).resize(ParentResize);

        $(m_Owner).resize();
    }


    function ParentResize() {
        var w = $(m_Owner).width();
        var h = $(m_Owner).height();

        var conLocal = $(m_Container).offset();

        var top = conLocal.top;

        $(m_Container).width(w);

        if (defaults.Dock == "Full") {
            $(m_Container).height(h - defaults.OffsetHeight - top);
        }

        if (defaults.FixedPanel == "Panel1") {

            var panel2W = w - defaults.Panel1Size;

            $(m_Panel1).width(defaults.Panel1Size);

            $(m_Panel2).width(panel2W - 4);

            var cH = $(m_Container).height();

            $(m_Panel1).height(cH);
            $(m_Panel2).height(cH);
            $(m_Split).height(cH);
            $(m_Split).width(defaults.SplitSize);
        }
    }

    init(options);
};

Mini.globel = {};

