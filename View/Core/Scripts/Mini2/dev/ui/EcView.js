
Mini2.define('Mini2.ui.EcView', {

    /**
     * 获得父窗口的句柄
     */
    parentPage: null,
    
    getParentWin :function (){

        var lastParent = window;

        var i = 0;

        for (i = 0; i < 9; i++) {
            if (lastParent.parent == lastParent || lastParent.parent == undefined) {
                break;
            }

            lastParent = lastParent.parent;
        }

        return lastParent;
    },


    ///转换 Url 地址，配合 EcView 使用
    changeAppUri : function(srcUri) {
        var me = this;

        if (undefined == srcUri ) {
            throw new Error('srcUrl 不能为空.');
        }

        if (srcUri.length && srcUri.substr(0, 1) == '/') {
            return srcUri;
        }

        if (srcUri.indexOf('http:') == 0 || srcUri.indexOf('https:')) {
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
    },


    show : function (uri, title, iconCls, width, height) {
        var me = this,
            win ;

        uri = me.changeAppUri(uri);

        win = me.getParentWin();
        
        if (win.AddTab) {

            if (iconCls) {

                win.AddTab(uri, { label: title, iconCls: iconCls, parentPage: window });
            }
            else {

                win.AddTab(uri, title, true, true, window);
            }
            return;
        }

        if (width == undefined) { width = 800; }
        if (height == undefined) { height = 600; }

        window.open(uri, title, "height=" + width + ",width=" + height + ",toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no");
    }



});


Mini2.EcView = {


    show: function (uri, title, iconCls, parentPage ) {

        var ev = Mini2.create('Mini2.ui.EcView', {
            
        });

        ev.show(uri, title, iconCls);

        return ev;
    }

};