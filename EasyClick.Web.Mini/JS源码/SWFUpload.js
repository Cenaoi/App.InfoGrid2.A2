/// <reference path="../jquery/jquery-1.7.1.js" />
/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../SWFUpload/swfupload.js" />


Mini.ui.SWFUpload = function (options) {
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
            upload_url: (m_Options.Upload_Url == "" ? ("/Core/Mini/EcWidgetAction.aspx?" + $.param(widgetUriObj)) : m_Options.Upload_Url),

            // Flash Settings
            flash_url: "/Core/Scripts/SWFUpload/swfupload.swf", // Relative to this file
            flash9_url: "/Core/Scripts/SWFUpload/swfupload_FP9.swf", // Relative to this file

            file_post_name: m_Options.File_Post_Name,

            post_params: { "ASPSESSID": m_Options.ASPSESSID },
            file_size_limit: "1900000",
            file_types: "*",
            file_types_description: "All Files",
            file_upload_limit: 100,
            file_queue_limit: 0,

            custom_settings: {
                upload_target: m_Options.FileProgressContainer
            },


            // Button settings
            button_image_url: "/Core/Scripts/SWFUpload/images/XPButtonUploadText_61x22.png",
            button_width: "61",
            button_height: "22",
            button_placeholder_id: m_Options.ButtonID,
            button_text: '<span class="theFont">选择</span>',
            button_text_style: ".theFont { font-size: 12px; }",
            button_text_left_padding: '16',


            // The event handler functions are defined in handlers.js
            swfupload_preload_handler: preLoad,
            swfupload_load_failed_handler: loadFailed,
            file_queued_handler: function (file) {
                $("#" + m_Options.FileProgressContainer).show();
            },
            file_queue_error_handler: fileQueueError,
            file_dialog_complete_handler: fileDialogComplete,
            upload_progress_handler: uploadProgress,
            upload_error_handler: uploadError,
            upload_success_handler: UploadSuccessHeandler,
            upload_complete_handler: uploadComplete,

            upload_start_handler: OnUploadStart,

            debug: false

        };


        m_Swfu = new SWFUpload(settings);


    }

    this.addPostParam = function (name,value) {

        m_Swfu.addPostParam(name,value);

    }

    var m_Event_UploadStart = null;

    function OnUploadStart(fileObj) {

        if (m_Event_UploadStart == null) {
            return;
        }

        m_Event_UploadStart(fileObj);
    }


    this.uploadStart = function (fn) {

        m_Event_UploadStart = fn;
    }

    function UploadSuccessHeandler(file, serverData) {
        try {
            //            alert("serverData = " + serverData);
            eval(serverData);

        } catch (ex) {
            this.debug(ex);
        }
    }


    function uploadComplete(file) {
        try {
            /*  I want the next upload to continue automatically so I'll call startUpload here */
            if (this.getStats().files_queued > 0) {
                this.startUpload();
            } else {
                var progress = new FileProgress(file, this.customSettings.upload_target);
                progress.setComplete();
                progress.setStatus("文件上传成功..");
                progress.toggleCancel(false);

                setInterval(function () {
                    $("#" + m_Options.FileProgressContainer).hide(500);
                }, 8000);
            }
        } catch (ex) {
            this.debug(ex);
        }
    }


    init(options);
}
