
Mini2.SLog = new function () {

    "use strict";
    Mini2.apply(this, {

        /**
         * 接收日志的服务器地址
         */ 
        server: null, 

        /**
         * 日志等级
         */
        level: 'all',


        getPageUrl: function (page) {
            return Mini2.isString(page) ? page : (page ? page.url : null);
        },

        debug: function (page, msg) {

            var me = this,
                pageUrl = me.getPageUrl(page);

            try {
                this.sendToServer('DEBUG', pageUrl, msg);
            }
            catch (ex) {
                console.warn('发送给服务器日志异常, 不影响系统操作.', ex);
            }
        },


        info: function (page, msg) {

            var me = this,
                pageUrl = me.getPageUrl(page);

            try {
                this.sendToServer('INFO', pageUrl, msg);
            }
            catch (ex) {
                console.warn('发送给服务器日志异常, 不影响系统操作.', ex);
            }
        },


        error: function (page, msg) {
            var me = this,
                pageUrl = me.getPageUrl(page);

            try {
                this.sendToServer('ERROR', pageUrl, msg);
            }
            catch (ex) {
                console.warn('发送给服务器日志异常, 不影响系统操作.',ex);
            }
        },


        sendToServer: function (msgType, reference_url, msg) {
            var me = this,
                url,
                data;

            if (!me.server ) {
                return;
            }

            if ('all' != me.level) {
                return;
            }

            if (!reference_url) {
                return;
            }

            if (!msg) {
                return;
            }


            url = me.server + '?action=' + msgType;


            data = {
                url: reference_url,
                msg : msg
            }

            $.post(url, data, function (result) {

                if (result.success) {

                }
                else {

                    console.warn('没有指定日志服务器地址错误!');

                }


            },'json');

        }


    });
}