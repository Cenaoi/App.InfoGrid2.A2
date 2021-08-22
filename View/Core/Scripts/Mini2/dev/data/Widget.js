// 未完成


/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../jquery.form.js" />
/// <reference path="_Default.js" />
/// <reference path="../validate/jquery.validate-vsdoc.js" />


Mini2.define('Mini2.data.Widget',{

    ActionPath : '/Core/Mini/WidgetAction.aspx',

    m_EventSuccess : new Array(),

    m_EventErrors : new Array(),



    m_Params : {
        __Path: '',
        __CID: '',
        __PID: '',
        __IsUser: 1,
        __Lang: '',
        __WidgetPs: '',
        __Action: '',
        __ActionPs: '',
        __ReturnFormat: '',
        __IsPost: '1',
        __SubName: '',
        __SubEvent: ''
    },

    item : function (itemName) {
        var me = this;

        var fullName = me.m_Params.__CID + "_" + itemName;

        var obj = window[fullName];

        return obj;
    },

    success : function (fn) {
        var me = this;

        me.m_EventSuccess.push(fn);
    },

    error :function (fn) {
        var me = this;
        me.m_EventErrors.push(fn);
    },

    onSuccess:function() {
        var me = this,
            evts = me.m_EventSuccess;

        for (var i = 0; i < evts.length; i++) {
            var fn = evts[i];

            fn.call(me,me);
        }
    },

    onError:function() {
        var me = this,
            evts = me.m_EventErrors;

        for (var i = 0; i < evts.length; i++) {
            var fn = evts[i];
            fn.call(me,me);
        }

    },


    getValues : function (filter) {
        /// <summary>
        /// 获取各个控件的值
        /// </summary>
        /// <param name="values" type="values">
        /// 配置一组键/值对。
        /// </param>
        /// <returns type="" />
        var me = this;

        var cid = me.m_Params.__CID;
        var cLen = cid.length;

        var objs = null;

        if (filter) {
            objs = $(filter);
        }
        else {
            objs = $(":input");
        }


        var items = [];

        var values = {};

        for (var i = 0; i < objs.length; i++) {

            var obj = objs[i];
            var id = $(obj).attr("id");


            if (id.length <= (cLen + 1) || id.substring(0, cLen) != cid) {
                continue;
            }

            items.push(obj);

            var wId = id.substring(cLen + 1);

            if ($(obj).attr("type") == "checkbox") {

                var v = $(obj).attr("checked");

                values[wId] = (v == undefined) ? false : v;
            }
            else {
                values[wId] = $(obj).val();
            }

        }

        return values;

    },

    setValues : function (values) {
        /// <summary>
        /// 对各个控件赋值
        /// </summary>
        /// <param name="values" type="values">
        /// 配置一组键/值对。
        /// </param>
        /// <returns type="" />
        var me =this;
        var cid = me.m_Params.__CID;

        for (var key in values) {

            var value = values[key];

            var wId = cid + "_" + key;     //构造 input 对象的 id

            var wObj = $("#" + wId);


            if (wObj.length == 0) {
                continue;
            }


            if (wObj.attr("type") == "checkbox") {

                wObj.attr("checked", value);
            }
            else {
                wObj.val(value);
            }

        }
    },

    getWidgetParams : function (owner) {
        var me = this,
            ps = me.m_Params;

        if (owner == undefined || owner == null || owner == '') {
            ps.__Action = '';
            ps.__Rum__ = Math.ceil(Math.random() * 1000);
            return ps;
        }

        if (owner.length && owner[0].nodeName == "BUTTON") {
            ps.__Action = (owner.attr("command") ? owner.attr("command") : owner[0].name);
        }
        else {
            ps.__Action = owner.attr("command");
        }

        ps.__ActionPs = (owner.attr("commandParam") ? owner.attr("commandParam") : '');
        ps.__ReturnFormat = (owner.attr("returnFormat") ? owner.attr("returnFormat") : ps.__ReturnFormat);
        ps.__Rum__ = Math.ceil(Math.random() * 1000);

        var actPs = ps.__ActionPs;
        if (actPs.startsWith("(") && actPs.endWith(")")) {
            ps.__ActionPs = "{" + actPs.substr(1, actPs.length - 2) + "}";
        }

        return ps;

    },


    this.Invoke = function (owner) {
        owner = $(owner);


        var exPath = this.getWidgetParams(owner);

        var options = {
            url: this.ActionPath + "?" + $.param(exPath),
            success: function (responseText, statusText, xhr, $form) {

                if (exPath.__ReturnFormat == 'html') {

                    $(exPath.__PID).html(responseText);
                }
                else {
                    try {

                        eval(responseText);
                    }
                    catch (ex) {

                        alert("错误\n\n" + ex.Message);
                    }
                }
            }
        };

        $.get(options.url, null, options.success);
    }

    this.getUrl = function (owner, command) {

        var exPath = this.getWidgetParams(owner);

        if (exPath.__Action == '' || exPath.__Action == undefined) {
            exPath.__Action = command;
        }

        var url = this.ActionPath + "?" + $.param(exPath);

        return url;
    }

    this.event = function (owner, formMethod, exData) {

        owner = $(owner);

        var formObjs = owner.closest('form');

        var loadingBoxId = owner.attr("loadingBoxId");

        if (loadingBoxId == undefined && Mini.globel.loadingBoxId) {
            loadingBoxId = Mini.globel.loadingBoxId;
        }

        if (formMethod == "post" && formObjs.length == 0) {
            alert("没有提供 Form 表单，无法提交!");
            return;
        }

        var formObj = formObjs[0];

        ProSubmitBufter(formObj);

        var exPath = this.getWidgetParams(owner);

        var eventSuccess = undefined;

        var srcReturnFormat = exPath.__ReturnFormat;



        if (exData) {

            if (exData.commandParam != undefined) {
                exPath.__ActionPs = exData.commandParam;
            }
            else if (exData.actionPs != undefined) {
                exPath.__ActionPs = exData.actionPs;
            }

            if (exData.command != undefined) {
                exPath.__Action = exData.command;
            }
            else if (exData.action != undefined) {
                exPath.__Action = exData.action;
            }

            if (exData.__IsPost != undefined) {
                exPath.__IsPost = exData.__IsPost;
            }

            if (exData.returnFormat != undefined) {
                exPath.__ReturnFormat = exData.returnFormat;
            }

            if (exData.success != undefined) {
                eventSuccess = exData.success;
            }
        }

        if (exPath.__Action == '' || exPath.__Action == undefined) {

            alert("没有提供 Command 命令参数，无法提交!");
            return;
        }

        if (typeof exPath.__ActionPs == "object") {
            exPath.__ActionPs = Mini.JsonHelper.toJson(exPath.__ActionPs);
        }

        $(owner).enable(false);


        var options = {
            url: this.ActionPath + "?" + $.param(exPath),
            success: function (responseText, statusText, xhr, $form) {
                $(owner).enable(true);

                if (exPath.__ReturnFormat == 'html') {

                    try {
                        $(exPath.__PID).html(responseText);
                        onSuccess();
                    }
                    catch (ex) {
                        onError();
                        alert("浏览器脚本解析错误(event): \n\n" + ex.Message + "\n\n" + responseText);
                    }
                }
                else {

                    //var jsLines = responseText.split("\n"); //字符分割 


                    //for (var i = 0; i < jsLines.length; i++) {

                    //    eval(jsLines[i]);
                    //}




                    try {

                        eval(responseText);

                        onSuccess();
                    }
                    catch (ex) {

                        onError();
                        alert("浏览器脚本解析错误(event): \n\n" + ex.Message + "\n\n" + responseText);

                    }
                }

                if (loadingBoxId != undefined) {
                    $("#" + loadingBoxId).hide();
                }
            }

        };

        if (eventSuccess != undefined && eventSuccess) {
            options.success = eventSuccess;
        }

        var form_method = formMethod;

        if (formMethod == undefined && $(owner).attr("form_method")) {
            form_method = $(owner).attr("form_method");
        }


        if (form_method == "get") {
            $.get(options.url, null, options.success);
        }
        else if (form_method == "post") {
            $.post(options.url, null, options.success);
        }
        else {
            if (formObj.method == "post") {
                $(formObj).ajaxSubmit(options);
            }
            else {
                $.get(options.url, null, options.success);
            }
        }

        m_Params.__ReturnFormat = srcReturnFormat;

        $(owner).enable(true);

        return false;
    };

    function ProSubmitBufter(formObj) {
        var textareList = $(formObj).find(":input");

        for (var i = 0; i < textareList.length; i++) {
            var conObj = $(textareList).get(i);
            var code = $(conObj).attr("SubmitBufore");

            if (!code) { continue; }


            eval(code);

        }
    }

    var m_WaitVisibleNum = 0;


    function ProExData(exData, exPath) {
        if (exData == undefined) {
            return;
        }

        exPath.__Action = exData.action || exData.command || '';

        exPath.__ReturnFormat = exData.returnFormat || '';

        if (undefined != exData.actionPs) {
            exPath.__ActionPs = exData.actionPs;
        }
        else if (undefined != exData.commandParam) {
            exPath.__ActionPs = exData.commandParam;
        }
        else {
            exPath.__ActionPs = '';
        }

        exPath.__ReturnFormat = exData.returnFormat || '';

        exPath.__RMode = exData.RMode || '';
        exPath.__SubName = exData.subName || '';
        exPath.__SubMethod = exData.subMethod || '';
        exPath.__SubEvent = exData.subEvent || '';

    }


    this.getFormObjs = function (owner) {

        var formObjs = null;

        if (owner != null && owner.length > 0 && "FORM" == owner.get(0).tagName) {
            formObjs = owner;
        }
        else if (owner == null) {
            formObjs = $("form");
        }
        else {
            formObjs = owner.closest('form');
        }

        return formObjs;
    }


    this.subMethod = function (owner, exData) {
        /// <summary>提交表单</summary>
        /// <param name="owner" type="object">触发源对象</param>
        /// <returns type="jQuery" />

        if (owner != null) {
            owner = $(owner);
        }

        var formObjs = this.getFormObjs(owner);
        var loadingBoxId;

        if (owner != null) {
            loadingBoxId = owner.attr("loadingBoxId");

            if (loadingBoxId == undefined && Mini.globel.loadingBoxId) {
                loadingBoxId = Mini.globel.loadingBoxId;
            }
        }

        if (formObjs.length == 0) {

            alert("没有提供 Form 表单，无法提交!");
            return;
        }


        var formObj = $(formObjs[0]);

        ProSubmitBufter(formObj);

        try {

            //以下验证代码 $(formObj).valid()  在 IE6 会出错。
            if (owner != null && owner.attr('valid') != "false") {

                var isValid = formObj.valid();

                if (isValid == false) {
                    alert("您的资料未填写完整!\n\n无法提交!");

                    return false;
                }
            }
        }
        catch (ex) {

        }

        var exPath = this.getWidgetParams(owner);

        var eventSuccess;

        ProExData(exData, exPath);

        if (exData && exData.success != undefined) {
            eventSuccess = exData.success;
        }


        if ((exPath.__Action == '' || exPath.__Action == undefined) &&
            (exPath.__SubMethod == '' || exPath.__SubMethod == undefined)) {

            alert("没有提供 函数 命令参数，无法提交!");
            return;
        }


        if (typeof exPath.__ActionPs == "object") {
            exPath.__ActionPs = Mini.JsonHelper.toJson(exPath.__ActionPs);
        }

        if (owner != null && owner.length > 0 && "FORM" != owner.get(0).tagName) {
            $(owner).enable(false);
        }

        var options = {
            url: this.ActionPath + "?" + $.param(exPath),
            success: function (responseText, statusText, xhr, $form) {

                if (owner != null) {
                    $(owner).enable(true);
                }

                if (loadingBoxId != "") {
                    m_WaitVisibleNum--;
                    $("#" + loadingBoxId).hide();
                }

                if (exPath.__ReturnFormat == 'html') {

                    try {
                        $(exPath.__PID).html(responseText);
                        onSuccess();
                    }
                    catch (ex) {
                        console.error('浏览器脚本解析错误(subMethod):', ex);
                        onError();
                        alert("浏览器脚本解析错误(subMethod): \n\n" + ex.Message + "\n\n" + responseText);
                    }
                }
                else {

                    //var jsLines = responseText.split("\n"); //字符分割 


                    //for (var i = 0; i < jsLines.length; i++) {


                    //    try{
                    //        eval(jsLines[i]);
                    //    }
                    //    catch (ex) {
                    //        console.error('浏览器脚本解析错误(subMethod):', ex);
                    //        console.debug(jsLines[i]);

                    //        onError();
                    //        alert("浏览器脚本解析错误(subMethod): \n\n" + ex.Message + "\n\n" + jsLines[i]);

                    //        break;
                    //    }
                    //}


                    try {

                        eval(responseText);

                        onSuccess();
                    }
                    catch (ex) {

                        console.error('浏览器脚本解析错误(subMethod):', ex);
                        onError();
                        alert("浏览器脚本解析错误(subMethod): \n\n" + ex.Message + "\n\n" + responseText);

                    }
                }

            },
            error: function (responseText, statusText, xhr, $form) {
                if (owner != null) {
                    $(owner).enable(true);
                }

                if (loadingBoxId != "") {
                    m_WaitVisibleNum--;
                    $("#" + loadingBoxId).hide();
                }

                alert("链接远程服务器超时或服务器关闭.\n\n请重新登录或检查网络畅通.");
            }

        };


        if (eventSuccess != undefined && eventSuccess) {
            options.success = eventSuccess;
        }

        if (loadingBoxId != undefined) {

            m_WaitVisibleNum++;

            setTimeout(function () {
                if (m_WaitVisibleNum > 0) {
                    $("#" + loadingBoxId).show();
                }
            }, 500);
        }

        try {
            $(formObj).ajaxSubmit(options);
        }
        catch (ex) {
            alert(ex.Message);
        }

        return false;
    };



    this.submit = function (owner, exData) {
        /// <summary>提交表单</summary>
        /// <param name="owner" type="object">触发源对象</param>
        /// <returns type="jQuery" />

        if (owner != null) {
            owner = $(owner);
        }

        var formObjs = this.getFormObjs(owner);
        var loadingBoxId;


        if (owner != null) {
            loadingBoxId = owner.attr("loadingBoxId");

            if (loadingBoxId == undefined && Mini.globel.loadingBoxId) {
                loadingBoxId = Mini.globel.loadingBoxId;
            }
        }

        if (formObjs.length == 0) {

            alert("没有提供 Form 表单，无法提交!");
            return;
        }


        var formObj = $(formObjs[0]);

        ProSubmitBufter(formObj);

        try {

            //以下验证代码 $(formObj).valid()  在 IE6 会出错。
            if (owner != null && owner.attr('valid') != "false") {

                var isValid = formObj.valid();

                if (isValid == false) {
                    alert("您的资料未填写完整!\n\n无法提交!");

                    return false;
                }
            }
        }
        catch (ex) {

        }

        var exPath = this.getWidgetParams(owner);

        var eventSuccess;

        ProExData(exData, exPath);

        if (exData && exData.success != undefined) {
            eventSuccess = exData.success;
        }


        if ((exPath.__Action == '' || exPath.__Action == undefined) && (exPath.__SubName == '' || exPath.__SubName == undefined)) {

            alert("没有提供 Command 命令参数，无法提交!");
            return;
        }


        if (typeof exPath.__ActionPs == "object") {
            exPath.__ActionPs = Mini.JsonHelper.toJson(exPath.__ActionPs);
        }

        if (owner != null && owner.length > 0 && "FORM" != owner.get(0).tagName) {
            $(owner).enable(false);
        }

        var options = {
            url: this.ActionPath + "?" + $.param(exPath),
            success: function (responseText, statusText, xhr, $form) {

                if (owner != null) {
                    $(owner).enable(true);
                }

                if (loadingBoxId != "") {
                    m_WaitVisibleNum--;
                    $("#" + loadingBoxId).hide();
                }

                if (exPath.__ReturnFormat == 'html') {

                    try {
                        $(exPath.__PID).html(responseText);
                        onSuccess();
                    }
                    catch (ex) {
                        console.error('浏览器脚本解析错误(submit):', ex);
                        onError();
                        alert("浏览器脚本解析错误(submit): " + ex.Message + "  ,行数:" + ex.link + "\n\n" + responseText);
                    }
                }
                else {
                    try {

                        eval(responseText);

                        onSuccess();
                    }
                    catch (ex) {

                        console.error('浏览器脚本解析错误(submit):', ex);
                        onError();
                        alert("浏览器脚本解析错误(submit): " + ex.Message + "  ,行数:" + ex.link + "\n\n" + responseText);

                    }
                }

            },
            error: function (responseText, statusText, xhr, $form) {
                if (owner != null) {
                    $(owner).enable(true);
                }

                if (loadingBoxId != "") {
                    m_WaitVisibleNum--;
                    $("#" + loadingBoxId).hide();
                }

                alert("链接远程服务器超时或服务器关闭.\n\n请重新登录或检查网络畅通.");
            }

        };


        if (eventSuccess != undefined && eventSuccess) {
            options.success = eventSuccess;
        }

        if (loadingBoxId != undefined) {

            m_WaitVisibleNum++;

            setTimeout(function () {
                if (m_WaitVisibleNum > 0) {
                    $("#" + loadingBoxId).show();
                }
            }, 500);
        }

        try {
            $(formObj).ajaxSubmit(options);
        }
        catch (ex) {
            alert(ex.Message);
        }

        return false;
    };

    this.reset = function () {
        var formObjs = $(owner).closest('form');

        if (formObjs.length > 0) {
            formObjs[0].clearForm();
        }
    };

    this.init = function (ps) {

        for (var p in ps) {

            m_Params[p] = ps[p];
        }
    };

    this.init(ps);

    return this;

};
