/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />

if (window.EC5 == undefined) { window.EC5 = {}; };


EC5.Tongji = function (options) {

    function Base64() {

        // private property
        _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        // public method for decoding
        this.decode = function (input) {
            var output = "";
            var chr1, chr2, chr3;
            var enc1, enc2, enc3, enc4;
            var i = 0;
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
            while (i < input.length) {
                enc1 = _keyStr.indexOf(input.charAt(i++));
                enc2 = _keyStr.indexOf(input.charAt(i++));
                enc3 = _keyStr.indexOf(input.charAt(i++));
                enc4 = _keyStr.indexOf(input.charAt(i++));
                chr1 = (enc1 << 2) | (enc2 >> 4);
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                chr3 = ((enc3 & 3) << 6) | enc4;
                output = output + String.fromCharCode(chr1);
                if (enc3 != 64) {
                    output = output + String.fromCharCode(chr2);
                }
                if (enc4 != 64) {
                    output = output + String.fromCharCode(chr3);
                }
            }
            output = _utf8_decode(output);
            return output;
        }

        // private method for UTF-8 decoding
        _utf8_decode = function (utftext) {
            var string = "";
            var i = 0;
            var c = c1 = c2 = 0;
            while (i < utftext.length) {
                c = utftext.charCodeAt(i);
                if (c < 128) {
                    string += String.fromCharCode(c);
                    i++;
                } else if ((c > 191) && (c < 224)) {
                    c2 = utftext.charCodeAt(i + 1);
                    string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                    i += 2;
                } else {
                    c2 = utftext.charCodeAt(i + 1);
                    c3 = utftext.charCodeAt(i + 2);
                    string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                    i += 3;
                }
            }
            return string;
        }
    }

    var defaults = {
        tag: "_htj",
        key: ""
    };

    var _time = null;

    this.setAccount = function (key) {
        defaults.key = key;
    }

    function timeRun() {
        var list = window[defaults.tag];

        //var list = new Array();

        var len = list.length;

        if (len == 0) { return; }

        for (var i = 0; i < len; i++) {
            var obj = list[i];
            try {
                sendAction(obj);
            }
            catch (ex) {
                //alert(ex.Message);
            }
        }

        list.splice(0, len);
    }

    function sendAction(obj) {

        var ps = {
            key: defaults.key,
            t: obj[0],
            u: window.location.href,
            r: document.referrer,
            rem: parseInt(1000000 * Math.random())
        };

        var uu = (new Base64()).decode("aHR0cDovL2F4NDMyNTcudmljcC5jYzo4MDgvQ04vaC5hc3B4Pw==");

        var psStr = $.param(ps) + "&callback=?";
        $.getJSON(uu + psStr, callback);
    }

    function callback(data) {
        //$("body").append("...");
    }

    function init(options) {
        for (var i in options) {
            defaults[i] = options[i];
        }

        window[defaults.tag] = window[defaults.tag] || [];

        _time = setInterval(timeRun, 500);

        $(window).load(function () {
            window[defaults.tag].push(['p']);
        });
    }

    init(options);
};

_Tongji = new EC5.Tongji({ key: "13eb67b078bc42b798a5d078cde30b33" });
