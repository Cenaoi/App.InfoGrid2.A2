/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../jquery.form.js" />
/// <reference path="_Default.js" />
/// <reference path="../validate/jquery.validate-vsdoc.js" />


Mini.ui.Widget = function (ps) {

    this.ActionPath = '/Core/Mini/WidgetAction.aspx';

    var m_EventSuccess = new Array();

    var m_EventErrors = new Array();



    var m_Params = {
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
    };

    this.item = function (itemName) {

        var fullName = m_Params.__CID + "_" + itemName;

        var obj = window[fullName];

        return obj;
    }

    this.success = function (fn) {
        m_EventSuccess.push(fn);
    }

    this.error = function (fn) {
        m_EventErrors.push(fn);
    }

    function onSuccess() {
        for (var i = 0; i < m_EventSuccess.length; i++) {
            var fn = m_EventSuccess[i];

            fn(this);
        }
    }

    function onError() {

        for (var i = 0; i < m_EventErrors.length; i++) {
            var fn = m_EventErrors[i];
            fn(this);
        }

    }


    this.getValues = function (filter) {
        /// <summary>
        /// 获取各个控件的值
        /// </summary>
        /// <param name="values" type="values">
        /// 配置一组键/值对。
        /// </param>
        /// <returns type="" />

        var cid = m_Params.__CID;
        var cLen = cid.length;

        var objs = null;

        if (filter) {
            objs = $(filter);
        }
        else {
            objs = $(":input");
        }


        var items = new Array();

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

    };

    this.setValues = function (values) {
        /// <summary>
        /// 对各个控件赋值
        /// </summary>
        /// <param name="values" type="values">
        /// 配置一组键/值对。
        /// </param>
        /// <returns type="" />

        var cid = m_Params.__CID;

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
    };

    this.getWidgetParams = function (owner) {

        if (owner == undefined || owner == null || owner == '') {
            m_Params.__Action = '';
            m_Params.__Rum__ = Math.ceil(Math.random() * 1000);
            return m_Params;
        }

        if (owner.length && owner[0].nodeName == "BUTTON") {
            m_Params.__Action = (owner.attr("command") ? owner.attr("command") : owner[0].name);
        }
        else {
            m_Params.__Action = owner.attr("command");
        }

        m_Params.__ActionPs = (owner.attr("commandParam") ? owner.attr("commandParam") : '');
        m_Params.__ReturnFormat = (owner.attr("returnFormat") ? owner.attr("returnFormat") : m_Params.__ReturnFormat);
        m_Params.__Rum__ = Math.ceil(Math.random() * 1000);

        var actPs = m_Params.__ActionPs;
        if (actPs.startsWith("(") && actPs.endWith(")")) {
            m_Params.__ActionPs = "{" + actPs.substr(1, actPs.length - 2) + "}";
        }

        return m_Params;

    };


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
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);
                    }
                }
                else {
                    try {
                        eval(responseText);

                        onSuccess();
                    }
                    catch (ex) {

                        onError();
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);

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

        if (exData.action) {
            exPath.__Action = exData.action;
        }
        else if (exData.command) {
            exPath.__Action = exData.command;
        }

        if (exData.returnFormat) {
            exPath.__ReturnFormat = exData.returnFormat;
        }

        if (exData.actionPs) {
            exPath.__ActionPs = exData.actionPs;
        }
        else if (exData.commandParam) {
            exPath.__ActionPs = exData.commandParam;
        }

        if (exData.returnFormat != undefined) {
            exPath.__ReturnFormat = exData.returnFormat;
        }

        if (exData.subName) {
            exPath.__SubName = exData.subName;
        }

        if (exData.subEvent) {
            exPath.__SubEvent = exData.subEvent;
        }

    }


    this.submit = function (owner, exData) {
        /// <summary>提交表单</summary>
        /// <param name="owner" type="object">触发源对象</param>
        /// <returns type="jQuery" />

        if (owner != null) {
            owner = $(owner);
        }

        var formObjs;
        var loadingBoxId;


        if (owner != null && owner.length > 0 && "FORM" == owner.get(0).tagName) {
            formObjs = owner;
        }
        else if (owner == null) {
            formObjs = $("form");
        }
        else {
            formObjs = owner.closest('form');
        }

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
                        onError();
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);
                    }
                }
                else {
                    try {

                        eval(responseText);

                        onSuccess();
                    }
                    catch (ex) {

                        onError();
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);

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
