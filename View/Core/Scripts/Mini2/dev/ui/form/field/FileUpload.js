/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />



//文件上传
Mini2.define('Mini2.ui.form.field.FileUpload', {


    extend: 'Mini2.ui.form.field.Base',

    alias: 'widget.fileupload',

    /**
    * 启动上传分块. 默认:false
    */
    chunked:false,


    //renderTpl: [
    //    '<div>',
    //        '<div style="border: 1px solid #CCCCCC; height:250px; width:250px; background-color: #F0F0F0; margin:2px;">',
    //            '<img src="" style="width:100%;">',
    //        '</div>',

    //    '</div>'
    //],


    buttonTpl:[
        '<a class="mi-btn ',
            'mi-unselectable ',
            'mi-btn-default-small ',
            '" ',
            'style="margin-left: 6px;" ',
            'role="button" hidefocus="on" unselectable="on" tabindex="0" >',

            '<input type="hidden" class="temp_value" value="" />',    //临时名称
            '<input type="hidden" class="value" />',                    //正式路径
            '<div class="progress" style="display:none; height:100%;width:20%; background-color:#ffffff;opacity:0.50; position:absolute;overflow:hidden; top:0px;left:0px;">',

            '</div>',
            '<span class="mi-btn-wrap" unselectable="on">',
            '</span>',
        '</a>'
    ],


    innerHtmlTpl: [
   
        '<span class="mi-btn-button">',
            '<span class="mi-btn-inner mi-btn-inner-center" unselectable="on">选择文件</span>',

        '</span>'
   
    ],

    //图像缩略图插件
    imageThumbPlugin:{

        htmlTpl: [
            '<div class="thumb" style="border: 1px solid #ddd; height:110px; width:110px; background-color: #FFFFFF; margin-bottom:4px; padding:4px; ',
                'border-radius: 4px;',
                '-webkit-box-shadow: 0 1px 2px rgba(0,0,0,0.075);box-shadow: 0 1px 2px rgba(0,0,0,0.075);',
                'display: block;">',
                '<div class="thumb-inner" style=" overflow:hidden;width:100px;height:100px;">',
                    '<img class="thumb-img" src="" style="height:100%;" />',
                '</div>',
            '</div>'
        ]


    },

    formType:'full',

    //fileUrl: '', //上传文件路径
    isUserUrl: false,   //自定义 url

    //插件类型。 none=没有， image=图片类型，other_file=其它文件
    pluginType: 'image',

    thumbWidth: 100,
    thumbHeight: 100,

    thumbEl : null, //预览对象


    valueEl: null,      //正式值的隐藏 dom
    tempValueEl:null,   //临时值的隐藏 dom 

    //server: '/View/Sample/WebForm1.aspx',

    widgetActionUrl: '/Core/Mini/EcWidgetAction.aspx',

    //appRelativeVirtualPath :  'InfoGrid2/View/Sample/XSS',


    text: '选择文件',
    
    //是否已经初始化预览
    isPreviewed :false,

    setValue:function(value){
        var me = this,
            pluginType = me.pluginType,
            valueEl = me.valueEl,
            thumbEl = me.thumbEl;

        valueEl.val(value);


        if (pluginType == 'image' ) {

            value = value || '';

            var imgEl = thumbEl.find('.thumb-img');

            var lines = value.split('||');

            imgEl.attr('data-original', lines[0]);

            if (imgEl && $.fn.viewer) {
                if (!me.isPreviewed) {

                    me.isPreviewed = true;

                    imgEl.viewer({
                        url: 'data-original'
                    });
                }
                else {
                    imgEl.viewer('update');
                }
            }


        }

        return me;
    },

    getValue: function () {
        var me = this,
            valueEl = me.valueEl,
            value;

        value = valueEl.val();

        return value;
    },

    setTempValue:function(value){
        var me = this,
            pluginType = me.pluginType,
            tempValueEl = me.tempValueEl;

        tempValueEl.val(value);

        if (pluginType == 'image') {

            me.thumbEl.find('.thumb-img').attr('src', value);
        }

        return me;
    },


    request:function() {
        var url = location.href,
            i, newStr,
            paraString;

        paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");

        newStr = "";

        for (i = 0; j = paraString[i]; i++) {
            if (i > 0) {
                newStr += "$$$";
            }

            newStr += j ;
        }

        if (this.tag) {
            newStr += "$$$" + "tag=" + encodeURIComponent(this.tag);
        }

        

        return newStr;
    },

    DefaultParams : {
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

    getFieldEl:function(){
        var me = this,
            el,
            uploader,
            tempValueEl,
            valueEl,
            pluginType = me.pluginType,
            thumb = me.imageThumbPlugin;

        var widgetUriObj = {
            __Path: encodeURI(me.appRelativeVirtualPath),
            __Action: '',
            __ActionPs: '',
            __CID: 'widget1_I',
            __ReturnFormat: "script",
            __SubName: me.id,
            __SubMethod: 'OnUploader',
            __Query: me.request()
        };

        el = Mini2.$join(me.buttonTpl);

        if ('image' == pluginType ) {

            var thumbEl = Mini2.$join(thumb.htmlTpl);

            thumbEl.css({
                width: (me.thumbWidth + 10) + 'px',
                height: (me.thumbHeight + 10) + 'px'
            });

            $(thumbEl).children('.thumb-inner').css({
                width: me.thumbWidth  + 'px',
                height: me.thumbHeight + 'px' 
            });

            thumbEl.find('.thumb-img').attr('src', me.value).attr('data-original', me.value);

            var boxEl = $('<div></div>');

            boxEl.append(thumbEl);

            boxEl.append(el);

            el = boxEl;

            me.thumbEl = thumbEl;
        }


        el.attr('id', me.clientId);


        //if (me.applyTo) {
        //    $(me.applyTo).replaceWith(el);
        //}
        //else {
        //    $(me.renderTo).append(el);
        //}


        me.tempValueEl = tempValueEl = el.find('.temp_value:first');
        me.valueEl = valueEl = el.find('.value:first');

        tempValueEl.attr('name', me.clientId + '_temp_value');
        valueEl.attr('name', me.clientId + '_value')

        if (me.temp_value) {
            tempValueEl.val(me.temp_value);
        }

        if (me.value) {
            valueEl.val(me.value);
        }

        var url;

        if (!Mini2.String.isBlank(me.fileUrl)) {
            url = me.fileUrl;
            me.isUserUrl = true;
        }
        else {
            url = me.widgetActionUrl + "?" + $.param(widgetUriObj);
        }


        var innerHtmlTpl = [
            '<span class="mi-btn-button">',
                '<span class="mi-btn-inner mi-btn-inner-center" unselectable="on">' , me.text, '</span>',

            '</span>'
        ];

        me.uploader = uploader = new WebUploader.Uploader({
            swf: '\Core\Scripts\webuploader-0.1.5\Uploader.swf',
            pick: {
                id: el.find('.mi-btn-wrap:first'),
                multiple: true,
                innerHTML: Mini2.$join(innerHtmlTpl)
            },

            fileVal: me.clientId,

            // 文件接收服务端。
            server: url,

            auto: true,

            chunked: me.chunked, //分块

            resize: false



            // 其他配置项
        });



        if ('image' == pluginType) {
            uploader.on('fileQueued', function (file) {

                uploader.makeThumb(file, function (error, ret) {
                    if (error) {
                        console.error('预览错误');
                    }
                    else {
                        me.thumbEl.find('.thumb-img').attr('src', ret);
                    }
                }, me.thumbWidth, me.thumbHeight);

            });
        }

        uploader.on('startUpload', function () {

            var data = {};

            if ('full' == me.formType) {
                var formData = $('form:first').formToArray();

                for (var i = 0; i < formData.length; i++) {
                    var fd = formData[i];

                    data[fd.name] = fd.value;
                }
            }
            else if ('sample' == me.formType ) {
                //简单模式

                data[me.clientId + '_temp_value'] = me.tempValueEl.val();
                data[me.clientId + '_value'] = me.valueEl.val();
            }

            uploader.options.formData = data;

            $(el).find('.progress').show();
        });

        // 文件上传过程中创建进度条实时显示。
        uploader.on('uploadProgress', function (file, percentage) {
            //var $li = $('#' + file.id),
            //    $percent = $li.find('.progress span');

            //// 避免重复创建
            //if (!$percent.length) {
            //    $percent = $('<p class="progress"><span></span></p>')
            //            .appendTo($li)
            //            .find('span');
            //}

            //$percent.css('width', percentage * 100 + '%');

            var perText = percentage * 100 + '%';

            $(el).find('.progress').css('width', perText);
            $(el).find('.mi-btn-inner-center').text('已上传 ' + perText);
        });

        // 文件上传成功，给item添加成功class, 用样式标记上传成功。
        uploader.on('uploadSuccess', function (file, response) {
            console.log("上传完成", file);

            if (me.isUserUrl) {

                var url = response.url;
                console.debug("反馈回的路径: " + url);

                var valueEl = me.valueEl;
                valueEl.val(url);

               

                if (me.thumbEl && $.fn.viewer) {

                    me.thumbEl.find('.thumb-img').attr('data-original', url);

                    if (!me.isPreviewed) {

                        me.isPreviewed = true;

                        me.thumbEl.viewer({
                            url: 'data-original'
                        });
                    }
                    else {
                        me.thumbEl.viewer('update');
                    }
                }

                //me.setValue(response.url);

                /**
                * 注意: 触发这个事件,是给上级控件作为监控, 并自动保存
                */
                me.trigger('focusout'); 

                var owner = me.ownerParent;

                if (owner && owner.setStoreRecrod) {
                    owner.setStoreRecrod.call(owner, me);
                }

                me.trigger('uploadSuccess', response);

            }
            else {
                try {
                    var raw = response._raw + '';

                    eval( raw );

                }
                catch (ex) {
                    console.error('解析对象出错。' + ex.message + '\r\n' + response._raw);
                }
            }
        });

        // 文件上传失败，显示上传出错。
        uploader.on('uploadError', function (file, reason) {

            console.error("上传出错");
        });

        // 完成上传完了，成功或者失败，先删除进度条。
        uploader.on('uploadComplete', function (file) {
            //console.log("完成上传完了....", file);

            $(el).find('.mi-btn-inner-center').text(me.text);

            $(el).find('.progress').hide();
        });

        return el;
    },



    //设置新的地址
    setTag: function (tagString) {
        var me = this,
            uploader = me.uploader;

        me.tag = tagString;

        var widgetUriObj = {
            __Path: encodeURI(me.appRelativeVirtualPath),
            __Action: '',
            __ActionPs: '',
            __CID: 'widget1_I',
            __ReturnFormat: "script",
            __SubName: me.id,
            __SubMethod: 'OnUploader',
            __Query: me.request()
        };


        var url;

        if (!Mini2.String.isBlank(me.fileUrl)) {
            url = me.fileUrl;
            me.isUserUrl = true;
        }
        else {
            url = me.widgetActionUrl + "?" + $.param(widgetUriObj);
        }

        try {

            console.debug(uploader);

            uploader.options.server = url;
        }
        catch (ex) {
            console.error("设置 tag 属性错误", ex);
        }
    }



});