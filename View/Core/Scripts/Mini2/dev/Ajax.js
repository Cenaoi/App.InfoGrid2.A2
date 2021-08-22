/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.post = function (url, formParsms, ok_Callback, error_Callback) {
    
    var me = this;

    if ($.isFunction(formParsms)) {
        error_Callback = ok_Callback;
        ok_Callback = formParsms;
        formParsms = null;
    }

    $.post(url, formParsms, function (result) {
                
        Mini2.Ajax.proResult(result,ok_Callback, error_Callback);
    });

};



Mini2.get = function (url, formParsms, ok_Callback, error_Callback) {

    var me = this;

    if ($.isFunction(formParsms)) {
        error_Callback = ok_Callback;
        ok_Callback = formParsms;
        formParsms = null;
    }

    $.get(url, formParsms, function (result) {

        Mini2.Ajax.proResult(result,ok_Callback, error_Callback);

    });

};


/**
* 数组操作类
*/
Mini2.Ajax = new function () {
    "use strict";

    Mini2.apply(this, {

        /**
        * 克隆函数
        */
        proResult: function (result, ok_Callback, error_Callback) {

            var pack;

            try {
                pack = JSON.parse(result);
            }
            catch (ex) {
                console.error('解析服务器反馈消息包错误,数据内容: ', result);
                return;
            }

            if (pack.success === true || pack.success === false) {

                try{
                    this.proResutV1(pack,result, ok_Callback, error_Callback);
                }
                catch (ex) {
                    console.error('反馈后执行错误', ex);
                    console.trace();
                }
            }
            else if (pack.result) {
                try{
                    this.proResultV2(pack,result, ok_Callback, error_Callback);
                }
                catch (ex) {
                    console.error('反馈后执行错误', ex);
                    console.trace();
                }
            }



        },

        proResutV1: function (pack,result, ok_Callback, error_Callback) {

            if (pack.success) {

                var data = pack.data;

                if (ok_Callback) {
                    ok_Callback( data, pack, result);
                }
            }
            else {

                if (error_Callback) {
                    error_Callback( pack, result);
                }
                else {
                    console.error('(v1)服务器反馈消息', pack.error_msg || pack.msg);

                    if (Mini2.Msg && Mini2.Msg.alert) {
                        //Mini2.Msg.alert('服务器反馈错误', pack.error_msg || pack.msg);
                    }
                }
            }
        },

        proResultV2: function (pack,result, ok_Callback, error_Callback) {


            if ('ok' == pack.result) {

                var data = pack.data;

                if (ok_Callback) {
                    ok_Callback(data, pack, result);
                }
            }
            else {

                if (error_Callback) {
                    error_Callback( pack, result);
                }
                else {
                    console.error('(v2)服务器反馈消息 ', pack.error_msg || pack.msg);

                    if (Mini2.Msg && Mini2.Msg.alert) {
                        //Mini2.Msg.alert('服务器反馈错误', pack.error_msg || pack.msg);
                    }
                }
            }
        }

    });


};
