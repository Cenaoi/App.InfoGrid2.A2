/* 
 * 文件版本号：2013-1-30 22:56
 *
 */

/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />


Mini.ui.Tooltip = function (ps) {


    function getIeVersion() {
        try {
            var browser = navigator.appName
            var b_version = navigator.appVersion
            var version = b_version.split(";");
            var trim_Version = version[1].replace(/[ ]/g, "");

            if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0") {
                return ("IE6");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE7.0") {
                return ("IE7");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE8.0") {
                return ("IE8");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE9.0") {
                return ("IE9");
            }
        }
        catch (ex) {

            return "";
        }


        return "";
    }

    function GetParentWin() {

//        var ieVersion = getIeVersion();

//        if (ieVersion == "IE9") {
//            return window;
//        }

        var lastParent = window;

        var i = 0;

        for (i = 0; i < 9; i++) {
            if (lastParent.parent == lastParent || lastParent.parent == undefined) {
                break;
            }

            lastParent = lastParent.parent;
        }

        return lastParent;
    }

    function sticky(win, msg) {
        try {
            win.$.sticky(msg);
        }
        catch (ex) {
            alert(msg);
        }
    }

    this.show = function (messsage) {

        var win = GetParentWin();

        if (win.Mini_Tooltop == undefined) {

            win.In.ready("mi.Tooltip", function () {
                sticky(win, messsage);
            });
        }
        else {
            sticky(win, messsage);
        }
    }

};

window.Mini_Tooltop = new Mini.ui.Tooltip();