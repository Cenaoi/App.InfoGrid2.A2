


Mini2.Ajax = new function () {

    "use strict";
    Mini2.apply(this, {

        /**
        * 把原始 url 的参数, 转换为服务器使用的参数 
        */
        request: function (url) {

            var newStr = '',
                i, j,
                n = url.indexOf("?"),
                paraString;

            if (n > -1) {
                paraString = url.substring(n + 1, url.length).split("&");

                for (i = 0; j = paraString[i]; i++) {
                    if (i > 0) { newStr += '$$$'; }
                    newStr += j;
                }
            }

            return newStr;
        },

        /**
        * 转换为服务器端可以使用的地址
        */
        getPath: function (url) {

            var appIndex = url.indexOf('/App/'),
                appQueryIndex = url.indexOf('?', appIndex);

            if (appQueryIndex == -1) {
                appQueryIndex = url.length;
            }

            var n = appQueryIndex - appIndex - 5;

            var cc = url.substr(appIndex + 5, n);

            var exN = cc.lastIndexOf('.');

            if (exN > 0) {
                cc = cc.substr(0, exN);
            }


            return cc;
        },

        /**
        * 执行 Ajax Post 操作
        * @param {page} 当前页面对象
        * @param {command} 跟服务器端对应的名称
        * @param {data} 提交的数据
        * @param {successFn} 反馈 json 数据包
        * @param {errorFn} 反馈 json 错误的数据包
        */
        post: function (page, command, data, successFn, errorFn) {
                        
            var me = this,
                ajax = Mini2.Ajax,
                mi_param,
                query = ajax.request(page.url),
                axPath = ajax.getPath(page.url);

            if (typeof data != 'object') {

                errorFn = successFn;
                successFn = data;
                data = {};
            }

            mi_param = {
                __Path: axPath,
                __IsPost: 1,
                __Query: query,
                __Action: command,
                __ReturnFormat: 'text',
                __ActionPs: '',
                __Rum__: Mini2.Random.newNum()
            };

            var url = Mini2.urlAppend('/Core/Mini/EcWidgetAction.aspx', $.param(mi_param));


            $.post(url,data, function (result) {

                if (result.success) {

                    if (successFn) {

                        try{
                            successFn.call(page, result.data, result);
                        }
                        catch (ex) {
                            console.error('执行成功的反馈函数错误.', ex);
                        }
                    }
                    else {

                    }

                }
                else {

                    if (errorFn) {
                        errorFn.call(page, result);
                    }
                    else {
                        $.alert(result.error_msg);
                    }

                }


            }, 'json');


        },





    });
}

Mini2.post = Mini2.Ajax.post;