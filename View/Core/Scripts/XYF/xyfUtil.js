
var xyf_util = function () {

    var obj = {};

    //url上的参数对象 
    var urlArgs = null;

    //获取url上的参数对象
    function getQueryArgs(url) {

        var beg_index = url.indexOf("?");

        var qs = url.slice(beg_index + 1);

        console.log(qs);

        var args = {};

        var items = qs.split("&");

        var item = null,
            name = null,
            value = null,
            i = 0,
            len = items.length;

        for (i; i < len; i++) {
            item = items[i].split("=");
            name = decodeURIComponent(item[0]);
            value = decodeURIComponent(item[1]);

            if (name.length) {
                args[name] = value;
            }



        }

        return args;
    }

    /**
    * 获取url上的参数值
    * @param {String} url
    * @param {String} name
    * @param {Boolean} flag
    */
    // 参数1--地址 参数2--要获取的参数名称 参数3--强制性重新拿数据 
    obj.getQuery = function (url, name, flag) {

        if (flag || urlArgs === null) {
            urlArgs = getQueryArgs(url);
        }

        console.log(urlArgs);

        if (!urlArgs) {
            return null;
        }

        return urlArgs[name];

    };

    /**
    * 判断字符串是否为空
    * @param {String} string 字符串
    */
    obj.isNullOrWhiteSpace = function (string) {

        if (undefined === string || null === string) {
            return true;
        }

        if (typeof string == 'string') {

            if (string.length === 0) {
                return true;
            }

            var trimStr = string.trim();

            if (trimStr.length === 0) {
                return true;
            }
        }


        try {
            if (typeof string == 'number') {

                if (string + '' == '') {
                    return true;
                }

            }
        }
        catch (ex) {

        }


        return false;

    };

    /**
    * 查找所有符合的节点对象
    * @param {String} selectors querySelectorAll 查找标签
    * @param {Boolean} is_arr 是否要转成数组
    * @returns {NodeList|Array} 节点集合
    */
    obj.querySelectorAll = function (selectors, is_arr) {


        if (is_arr) {

            var nodes = document.querySelectorAll(selectors);

            return Array.prototype.slice.call(nodes);


        } else {

            return document.querySelectorAll(selectors);

        }

    }

    /**
    * 压缩图片
    * @param {String} path 地址或者base64值也行
    * @param {Object} obj {width:500,hwight:500,quality:0.7}
    * @param {Function} callback 回调函数  参数1 压缩后的base64
    */
    obj.drawImage = function (path, obj, callback) {

        var img = new Image();
        img.src = path;

        img.onload = function () {

            console.log("图片加载完成了");

            var that = this;
            // 默认按比例压缩
            var w = that.width,
                h = that.height,
                scale = w / h;
            w = obj.width || w;
            h = obj.height || (w / scale);
            var quality = 0.7;        // 默认图片质量为0.7

            //生成canvas
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext('2d');

            // 创建属性节点
            var anw = document.createAttribute("width");
            anw.nodeValue = w;
            var anh = document.createAttribute("height");
            anh.nodeValue = h;
            canvas.setAttributeNode(anw);
            canvas.setAttributeNode(anh);

            ctx.drawImage(that, 0, 0, w, h);
            // 图像质量
            if (obj.quality && obj.quality <= 1 && obj.quality > 0) {
                quality = obj.quality;
            }
            // quality值越小，所绘制出的图像越模糊
            var base64 = canvas.toDataURL('image/jpeg', quality);

            // 回调函数返回base64的值
            callback(base64);
        }




    }

    /**
    * 数组删除对象函数
    * @param {Array} arr 数组
    * @param {Object} a 要删除的对象
    * @returns {Number} 删除的对象索引
    */
    obj.arrayRemove = function (arr, a) {



        if (!arr) {
            throw new Error("数组不能为空！");
        }

        if (!a) {
            throw new Error("删除的对象不能为空！");
        }

        var index = arr.indexOf(a);

        if (index == -1) {
            return index;
        }


        arr.splice(index, 1);

        return index;

    }

    /**
    * 提交事件 依赖jquery 或 zepto 
    * @param {String} url 提交地址
    * @param {String} action 执行动作
    * @param {Object} data 提交的from参数
    * @param {Function} fn 返回成功执行的回调函数 参数1 data  参数2 result
    * @param {Function} fnError 返回失败执行的回调函数 没有就弹出错误消息
    */
    obj.post = function (url, action, data, fn, fnError) {



        data["action"] = action;

        $.post(url, data, function (result) {


            if (!result.success) {

                if (fnError) {

                    fnError(result);

                    return;

                }



                $.alert(result.error_msg);


                return;


            }


            fn(result.data, result);





        }, "json");



    }



    //升級版自動保存字段函數 參數1--主鍵ID 參數2--表名 參數3--字段 參數4-字段值
    obj.autoSaveDetaV1 = function (pk, table_name, field_text, field_value) {


        $.post("/App/DIY/View/Handlers/CurrencyHandler.ashx",
            {
                action: 'SAVE_FIELD_DATA',
                pk: pk,
                table_name: table_name,
                field: field_text,
                value: field_value

            }, function (result) {

                if (!result.success) {

                    console.error(result);

                    return;
                }

            }, "json");

    }




    obj.getUrl = function (url) {


        var beg_index = url.indexOf("?");

        var qs = url.substr(0, beg_index);

        return qs;


    }

    /**
    * 注册这个界面能调用微信脚本函数
    * @param {Boolean} debug 是否要开启调试模式  false -- 不开启 true -- 开启
    * @param {String} url 后台对接地址
    */
    obj.GetWxJsConfig = function (debug, url) {

        var me = this;

        me.post(url, "GET_WX_JS_CONFIG", { debug: debug }, function (data) {

            var config = data;

            var obj = {
                debug: data.debug,
                appId: data.appId,
                timestamp: data.timestamp,
                nonceStr: data.nonceStr,
                signature: data.signature,
                jsApiList: data.jsApiList


            }

            wx.config(obj);

        });

    }

    /**
    * 拼接 post 路径
    * @param {String} path post 地址
    * @param {String} action 函数名称
    * @returns {String} 新的地址
    */
    obj.appUrl = function (path, action) {


        var url = "/App/InfoGrid2/Core/Mini/API.ashx?__path=" + path + "&__action=" + action;

        return url;

    };

    //更新剩余时间 参数1--节点对象  参数2--结束时间点 参数3--倒计时结束后调用方法 参数4--倒计时结束后显示文字
    obj.updateSurplusDate = function (node, time, fn, endText) {

        var me = this;

        var now = moment(new Date());

        var then = moment(time);

        var spanDate = moment.duration(then.diff(now));

        if (spanDate.days() === 0 && spanDate.hours() === 0 && spanDate.minutes() === 0 && spanDate.seconds() <= 0) {

            var end_text = endText || '倒计时结束了';

            node.text(end_text)

            fn();

            return;
        }

        var text = spanDate.days() + " 天 " + spanDate.hours() + " 时 " + spanDate.minutes() + " 分 " + spanDate.seconds() + " 秒 ";

        node.text(text);

        setTimeout(function () {

            me.updateSurplusDate(node, time, fn, endText);

        }, 1 * 1000);


    }

    /**
    *  延时执行函数 只执行一下
    * @param {Function} fn 函数
    * @param {Number} time  时间 （秒单位）
    */
    obj.delay = function (fn, time) {

        setTimeout(fn, time * 1000);

    };

    /**
    * 循环助手
    * @param {Function} fn 函数 返回true才能继续循环
    * @param {Number} time  时间 （秒单位）
    */
    obj.loop = function (fn, time, params) {



        var flag = fn();

        if (flag) {

            setTimeout(fn, time * 1000);

        }

    }

    return obj;

}()
