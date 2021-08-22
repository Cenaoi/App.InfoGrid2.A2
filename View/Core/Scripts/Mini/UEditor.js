/// <reference path="../jquery/jquery-1.7.1.js" />
/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../SWFUpload/swfupload.js" />


Mini.ui.UEditor = function (options) {
    /// <summary>模板</summary>

    var m_Swfu;

    var defaults = {
        ID: ''
        , FileProgressContainer: "FileProgressContainer"
        , ButtonID: 'spanButtonPlaceHolder'
        , TargetNameID: ''    //目标文件的存放路径
        , SrcNameID: ''      //原图片文本框的id
        , ASPSESSID: ''
        , Upload_Url: ''      //文件上传的接受路径
        , File_Post_Name: 'Filedata'
        , AppRelativeVirtualPath: ''
    };



    var m_Options = {
        ID: ''
        , FileProgressContainer: "FileProgressContainer"
        , ButtonID: 'spanButtonPlaceHolder'
        , TargetNameID: ''    //目标文件的存放路径
        , SrcNameID: ''      //原图片文本框的id
        , ASPSESSID: ''
        , Upload_Url: ''      //文件上传的接受路径
        , File_Post_Name: 'Filedata'
        , AppRelativeVirtualPath: ''
    };

    function request() {
        var url = location.href;
        var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");

        var newStr = "";

        for (i = 0; j = paraString[i]; i++) {
            newStr += (j + "$$$");
        }

        return newStr;
    }

    var m_UEditOptions;

    function LocalQuery() {

        var n = window.location.href.indexOf('?');

        if (n == -1) {
            return "";
        }

        var len = location.href.length;

        var txt = location.href.substring(n + 1, len);

        return "&" + txt;
    }

    function init(options) {

        m_Options = $.extend(defaults, options);

        m_Options.File_Post_Name = options.ID;

        if (options.ID != undefined && options.FileProgressContainer == undefined) {
            m_Options.FileProgressContainer = options.ID + "_FileProgressContainer";
        }

        if (options.ID != undefined && options.ButtonID == undefined) {
            m_Options.ButtonID = options.ID + "_Button";
        }

        if (options.ID != undefined && options.TargetNameID == undefined) {
            m_Options.TargetNameID = options.ID; // +"_TargetName";
        }

        if (options.ID != undefined && options.SrcNameID == undefined) {
            m_Options.SrcNameID = options.ID + "_SrcName";
        }

        var widgetUriObj = {
            __SubEvent: "Uploader",
            __ReturnFormat: "script",
            __SubName: options.SubName,
            __Path: encodeURI(m_Options.AppRelativeVirtualPath),
            __CID: options.CID,
            __Query: request()
        };

        if (options.params) {
            widgetUriObj.__Query += options.params;
        }

        //        alert(widgetUriObj.__Query);


        var settings = {
            imageUrl: (m_Options.Upload_Url == "" ? ("/Core/Mini/EcWidgetAction.aspx?" + $.param(widgetUriObj)) : m_Options.Upload_Url),
            imagePath: "",

            initialFrameWidth: 860,
            initialFrameHeight: 320

        };

        settings.imageUrl += LocalQuery();

        m_UEditOptions = settings;

        m_Swfu = new UE.ui.Editor(settings);
        m_Swfu.render(options.ID);

    }

    this.getOptions = function () {

        return m_UEditOptions;

    };

    //    this.addPostParam = function (name, value) {

    //        m_Swfu.addPostParam(name, value);

    //    }

    //    var m_Event_UploadStart = null;

    //    function OnUploadStart(fileObj) {

    //        if (m_Event_UploadStart == null) {
    //            return;
    //        }

    //        m_Event_UploadStart(fileObj);
    //    }


    //    this.uploadStart = function (fn) {

    //        m_Event_UploadStart = fn;
    //    }

    //    function UploadSuccessHeandler(file, serverData) {
    //        try {
    //            //            alert("serverData = " + serverData);
    //            eval(serverData);

    //        } catch (ex) {
    //            this.debug(ex);
    //        }
    //    }


    //    function uploadComplete(file) {
    //        try {
    //            /*  I want the next upload to continue automatically so I'll call startUpload here */
    //            if (this.getStats().files_queued > 0) {
    //                this.startUpload();
    //            } else {
    //                var progress = new FileProgress(file, this.customSettings.upload_target);
    //                progress.setComplete();
    //                progress.setStatus("文件上传成功..");
    //                progress.toggleCancel(false);

    //                setInterval(function () {
    //                    $("#" + m_Options.FileProgressContainer).hide(500);
    //                }, 8000);
    //            }
    //        } catch (ex) {
    //            this.debug(ex);
    //        }
    //    }

    init(options);
}
