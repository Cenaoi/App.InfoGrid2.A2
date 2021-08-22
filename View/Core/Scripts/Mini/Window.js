/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../ui-lightness/jquery-ui-1.8.16.custom.min.js" />

Mini.ui.Window = function (ps) {

    var defaults = {
        resizable: true,
        stack: true,
        width: 200,
        height: 150,
        modal: true,
        title: "Window",
        autoOpen: false
    };

    var m_Options = {
        resizable: true,
        stack: true,
        width: 200,
        height: 150,
        modal: true,
        title: "Window",
        autoOpen: false
    };



    var m_contentHtml;
    var m_owner;
    var m_contentObj;

    var m_Box;

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

    function resizeEvent(event, ui) {
        var sz = ui.size;


        $(m_contentObj).width(m_Box.width());
        $(m_contentObj).height(m_Box.height());

    }

    function openEvent(event, ui) {
        var sz = ui.size;

        $(m_contentObj).width(m_Box.width());
        $(m_contentObj).height(m_Box.height());

    }

    function GetParentWin() {

        //        var ieVersion = getIeVersion();

        //        if (ieVersion == "IE6" || ieVersion == "IE7") {
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

    var m_closedEvent = new Array();

    this.closed = function (fn) {
        m_closedEvent.push(fn);
    };

    function onClosed(e) {
        for (var i = 0; i < m_closedEvent.length; i++) {
            var fn = m_closedEvent[i];

            try {
                fn(this, e);
            }
            catch (ex) {
                alert("MiniHtml Error: Window.onClosed(...) \n\n" + ex.Message);
            }
        }
    }

    this.close = function (e) {

        if (e == undefined) {
            e = {};
        }

        var win = window; //GetParentWin();

        onClosed(e);

        win.curDialog.dialog("close");

        //释放内存,有错误.暂时注释掉


        //        win.curDialog.dialog("destroy");
        //        delete win.curDialog;
        //$(win.curDialog).remove();
    }

    this.show = function () {

        var win = window; // GetParentWin();


        m_contentObj = $(m_Options.contentHtml);

        m_Box = $("<div style='border:0px solid #000000;'></div>");
        m_Box.attr("title", m_Options.title);


        m_Options.resizeStop = resizeEvent;
        m_Options.create = openEvent;

        
        //m_Options.buttons = [{ text: "Ok", click: function () { $(this).dialog("close"); } }];

        m_owner = $(m_Box).dialog(m_Options);

        curDialog = m_owner;


        m_owner.dialog("open");

        m_Box.append(m_contentObj);
        m_contentObj.curDialog = m_owner;

        var obj = $(m_contentObj).get(0);

        //如果是 iframe 模式窗体，就加入 curDialog 字段
        if (obj.contentWindow) {
            obj.contentWindow.curDialog = m_owner;
        }

        $(m_contentObj).width(m_Box.width());
        $(m_contentObj).height(m_Box.height());


    }

    this.getContentObj = function () {
        return m_contentObj;
    }

    function init(options) {
        m_Options = $.extend(defaults, options);

        m_contentHtml = options.contentHtml;
    }

    init(ps);
}