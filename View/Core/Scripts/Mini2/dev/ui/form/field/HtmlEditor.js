Mini2.define('Mini2.ui.form.field.HtmlEditor', {

    extend: 'Mini2.ui.form.field.Text',


    alias: 'widget.htmleditor',

    /**
     * 图片上传路径
     */
    imageUrl: '',

    /**
     * 文件上传路径
     */
    fileUrl: '',

    /**
    * 有焦点
    */
    hasFocus : false,


    getFieldEl: function () {
        "use strict";
        var me = this,
            inputEl ;




        me.inputEl = inputEl = Mini2.$join([
            '<textarea >',
            '</textarea>'
        ]);


        if (me.name) {
            inputEl.attr('name', me.name);
        }

        inputEl.attr("id", me.clientId);

        inputEl.css({
            'width': me.width,
            'height': me.height
        });


        if (me.readOnly) {
            inputEl.attr('readonly', 'readonly');
        }

        //附加属性
        //me.inputEl.attr("data-errorqtip", "");
        //

        //inputEl.addClass("mi-form-required-field");

        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);




        return inputEl;

    },


    _isValueChanged: false,

    /**
    * UEditor 是否已经准备好
    */
    isUEditReady: false,

    isValueChanged: function () {
        "use strict";
        var me = this;

        return me._isValueChanged;
    },


    getValue: function () {
        "use strict";
        var me = this,
            uedit = me.uedit,
            inputEl = me.inputEl;
        
        return inputEl.val();
    },

    setValue: function (value) {
        "use strict";
        var me = this,
            uedit = me.uedit,
            inputEl = me.inputEl;

        if (!me.hasFocus) {
            inputEl.val(value);

            if (me.isUEditReady) {
                uedit.setContent(value, false);
            }
        }
    },


    /**
    * 自动保存
    */
    autoSave:function(){
        var me = this,
            uedit = me.uedit;

        if (me.saveTimer) {

            me.saveTimer.resetStart();
        }
        else {
            me.saveTimer = Mini2.setTimer(function () {

                var html = uedit.getContent();

                me.inputEl.val(html);

                me._isValueChanged = true;

                var owner = me.ownerParent;

                if (owner && owner.setStoreRecrod) {
                    owner.setStoreRecrod.call(owner, me);
                }

            },{
                interval: 1000,
                limit: 1
            });
        }
    },


    render: function () {
        "use strict";
        var me = this,
            uedit,
            hideEl,
            bodyCls = '.mi-form-item-body:first';


        me.baseRender();

        me.el.data('me', me);


        //var imageUrl = Mini2.urlAppend('/App/InfoGrid2/View/CMS/CmsUploadHandler.aspx', {
        //    method: 'image_up',
        //    module: 'tile',
        //    cata_code: '',
        //    item_id: 123

        //}, true);

        me.uedit = uedit = UE.getEditor(me.clientId, {
            autoHeightEnabled: false,
            initialFrameHeight: 400,
            scaleEnabled: false,
            imageUrl: me.imageUrl,
            imagePath: '',
            maxImageSideLength:9000 //防止被压缩
        });


        //判断ueditor 编辑器是否创建成功
        uedit.addListener("ready", function () {

            me.isUEditReady = true;

        });

        uedit.addListener('focus', function (editor) {
            me.hasFocus = true;

            me.focus();
        });

        uedit.addListener('blur', function (editor) {
            me.hasFocus = false;

            me.autoSave();
        });

        uedit.addListener('selectionchange', function (editor) {
            me.autoSave();
        });



    }

});