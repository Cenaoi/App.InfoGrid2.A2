/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../ui-lightness/jquery-ui-1.8.16.custom.min.js" />


Mini.ui.EcView = function (options) {

    var m_ShowEvent;
    var m_ShowDialogEvent;

    this.setShowEvent = function (event) {
        m_ShowEvent = event;
    };


    this.setShowDialogEvent = function (event) {
        m_ShowDialogEvent = event;
    }


    function GetParentWin() {

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


    ///转换 Url 地址，配合 EcView 使用
    function ChangeAppUri(srcUri) {

        if (srcUri.substr(0, 1) == '/') {
            return srcUri;
        }

        var path = window.location.pathname;
        var search = window.location.search;

        var n = path.lastIndexOf('/');

        var dir = path.substr(0, n + 1);


        while ((srcUri.length > 3 && srcUri.substr(0, 3) == "../")) {
            n = dir.lastIndexOf('/', dir.length - 2);

            dir = dir.substr(0, n + 1);

            srcUri = srcUri.substr(3);
        }

        var newPath = dir + srcUri;

        return newPath;
    }


    this.show = function (uri, title, width, height) {

        uri = ChangeAppUri(uri);

        var win = GetParentWin();


        if (win.AddTab) {

            win.AddTab(uri, title);

            return;
        }

        if (width == undefined) { width = 800; }
        if (height == undefined) { height = 600; }

        window.open(uri, title, "height=" + width + ",width=" + height + ",toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no");
    }



    this.showDialog = function (uri, title, width, height, modal) {

        if (width == undefined) { width = 800; }
        if (height == undefined) { height = 600; }

        var win = GetParentWin();

        if (win == undefined) {
            alert("Error code:593\nEcView.shoDialgo(...)");
            return;
        }

        var nowUri = ChangeAppUri(uri);

        var txt = "<iframe frameborder='0'  src='" + nowUri + "' ></iframe>";

        try {

            var frm = new win.Mini.ui.Window({
                title: title,
                contentHtml: txt,
                width: width,
                height: height,
                modal: modal
            });

            frm.ownerWindow = window;
            frm.show();

            win.DialogEcView = frm;

            return frm;
        }
        catch (ex) {
            alert("EcView.showDialog(...)\n\n" + ex.Message);
        }

    }


    this.close = function (e) {

        if (window.curDialog && window.curDialog.close) {
            window.curDialog.close();
            return;
        }

        var win = GetParentWin();

        //关闭模式窗体
        if (win.DialogEcView) {
            var frm = win.DialogEcView;

            if (e == undefined) {
                e = {};
            }

            try {
                frm.close(e);

                var ownerWindow = frm.ownerWindow;

                win.DialogEcView = {};
            }
            catch (ex) {

                alert("关闭窗体错误:" + ex.Message);
            }

            return;
        }


        if (win.RemoveTab) {
            var url = window.location.href;

            win.RemoveTab(url);
        }

    }

    this.create = function () {

        var frm = new Mini.ui.EcForm();

        return frm;

    };
};

var EcView = new Mini.ui.EcView();