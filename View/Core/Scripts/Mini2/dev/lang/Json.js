﻿

Mini2.Json = new function () {
    "use strict";
    Mini2.apply(this, {

        toJson: function (o) {

            if (o == undefined) {
                return "\"\"";
            }

            var r = [];
            if (typeof o == "string") return "\"" + o.replace(/([\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t").replace(/\+/g, '%2B') + "\"";
            if (Mini2.isDate(o)) { return "\"" + Mini2.Date.format(o, 'Y-m-d H:i:s.u') + "\""; }
            if (typeof o == "object") {
                if (!o.sort) {
                    for (var i in o)
                        r.push("\"" + i + "\":" + Mini2.Json.toJson(o[i]));
                    if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                        r.push("toString:" + o.toString.toString());
                    }
                    r = "{" + r.join() + "}"
                } else {
                    for (var i = 0; i < o.length; i++)
                        r.push(Mini2.Json.toJson(o[i]))
                    r = "[" + r.join() + "]";
                }
                return r;
            }
            return o.toString().replace(/\"\:/g, '":""');
        }
    });
};


