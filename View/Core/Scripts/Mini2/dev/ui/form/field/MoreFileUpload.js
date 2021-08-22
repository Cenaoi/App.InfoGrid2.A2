/**
* 多文件上传
*
*
*/

Mini2.define('Mini2.ui.form.field.MoreFileUpload', {

    extend: 'Mini2.ui.form.field.Text',

    alias: 'widget.morefileupload',

    dataType: 'text',

    imageUrl: '',

    fileUrl: '',

    thumbWidth: 100,
    thumbHeight: 100,

    hideThumbTitle: false,


    height: 270,

    /**
    * 展示模式 .editor=编辑模式, preview=预览模式
    */
    displayMode: 'editor',

    /**
    * 分割线, 分区普通文件和正常文件
    */
    slicer: true,

    /**
    * 图片容器
    */
    imagePanelEl: null,

    /**
    * 其它文件容器
    */
    otherPanelEl: null,

    getFieldEl: function () {
        "use strict";
        var me = this,
            slicer = me.slicer, //分割线
            innerEl,
            inputEl;


        var height = parseInt(me.height);

        if ('editor' == me.displayMode) {

            me.inputEl = inputEl = Mini2.$join([
                '<div class="mi-file-panel" >',
                    '<div class="file-inner"  style="border: 1px solid #ddd; padding:10px; height:', height - 60, 'px; overflow: auto;">',

                    '</div>',
                    '<div class="mi-file-toobar"  style="border: 1px solid #ddd; padding:6px;height:50px;" >',
                        '<div class="upload-button" style="flow:left;"></div>',
                        '<button type="button" class="remove-btn" style="float:left;">删除</button>',
                    '</div>',
                '</div>'
            ]);
        }
        else if ('preview' == me.displayMode) {

            me.inputEl = inputEl = Mini2.$join([
                '<div class="mi-file-panel" >',
                    '<div class="file-inner"  style="border: 1px solid #ddd; padding:10px; height:', height - 60, 'px; overflow: auto;">',

                    '</div>',
                    '<div class="mi-file-toobar"  style="border: 1px solid #ddd; padding:6px;height:40px;" >',
                        '<button type="button" class="editor-btn" style="float:left; width:80px;">编辑</button>',
                    '</div>',
                '</div>'
            ]);


            $(inputEl).find('.editor-btn').click(function () {

                me.onButtonClick();
            });

        }

        me.fileInnerEl = innerEl = $(inputEl).children('.file-inner');

        if (me.maxHeight) {
            var mh = parseInt(me.maxHeight) - 50;
            innerEl.css('max-height', mh + 'px');
        }

        if (me.minHeight) {
            var mh = parseInt(me.minHeight) - 50;
            innerEl.css('min-height', mh + 'px');
        }


        if (slicer && 'preview' == me.displayMode) {

            var otherPanelEl = Mini2.$join([
                '<div role="other-file-panel" style="height:auto; margin-bottom:10px; padding-bottom:10px; border-bottom: 1px solid #dddddd;">',

                '</div>'
            ]);

            var imagePanelEl = Mini2.$join([
                '<div role="image-file-panel" style="height:auto;">',

                '</div>'
            ]);

            innerEl.append(otherPanelEl);
            innerEl.append(imagePanelEl);

            me.imagePanelEl = imagePanelEl;
            me.otherPanelEl = otherPanelEl;
        }
        else {
            me.imagePanelEl = innerEl;
            me.otherPanelEl = null;
        }


        inputEl.attr("id", me.clientId);

        $(inputEl).find('.upload-button').attr('id', me.clientId + '_UploadBtn');

        inputEl.css({
            'width': me.width,
            'height': me.height
        });


        if (me.readOnly) {
            inputEl.attr('readonly', 'readonly');
        }

        return inputEl;

    },


    _isValueChanged: false,


    isValueChanged: function () {
        "use strict";
        var me = this;

        return me._isValueChanged;
    },


    items: false,


    clear: function () {
        "use strict";
        var me = this,
            imagePanelEl = me.imagePanelEl,
            otherPanelEl = me.otherPanelEl,
            hideEl2 = me.hideEl2;

        me.items = [];

        imagePanelEl.children().remove();

        if (otherPanelEl) {
            otherPanelEl.children().remove();
        }

        if (hideEl2) {
            hideEl2.val('');
        }

        return me;
    },

    removeSeleted: function () {
        "use strict";
        var me = this,
            imagePanelEl = me.imagePanelEl,
            otherPanelEl = me.otherPanelEl,
            inputEl = me.inputEl,
            items = me.items,
            hideEl2 = me.hideEl2;

        console.debug('imagePanelEl = ', imagePanelEl);

        var checkEls = imagePanelEl.children('.mi-file-check');

        console.log("选中的图片数量: ", checkEls.length);

        for (var i = 0; i < checkEls.length; i++) {

            var checkEl = checkEls[i];

            var file_id = $(checkEl).attr('data-file-id');

            var n = Mini2.Array.indexOf(items, function () {
                return (this.file_id == file_id);
            });



            if (n >= 0) {
                items.splice(n, 1);
            }
        }

        checkEls.remove();

        me.autoSave();

        if (hideEl2) {
            var newValueStr = me.getValue();
            hideEl2.val(newValueStr);
        }
    },


    getValue: function () {
        "use strict";
        var me = this,
            items = me.items || [],
            txt, i, item,
            hideEl2 = me.hideEl2;

        if ('text' == me.dataType) {

            txt = '';

            for (i = 0; i < items.length; i++) {

                if (i > 0) { txt += '\n'; }

                item = items[i];


                txt += Mini2.join([item.url, '||', item.thumb_url, '||', item.title, '||', item.code]);

            }

            return txt;

        }


    },

    setValue: function (value) {
        "use strict";
        var me = this,
            inputEl = me.inputEl,
            hideEl2 = me.hideEl2;

        me.clear();


        if (Mini2.isBlank(value)) {
            return;
        }


        if ('text' == me.dataType) {

            var lines = value.split('\n');

            for (var i = 0; i < lines.length; i++) {

                var text = lines[i];

                if (Mini2.String.isBlank(text)) {
                    continue;
                }

                try {
                    me.addForText(text);
                }
                catch (ex) {
                    console.error("text", text);
                    console.error('添加对象失败', ex);
                }
            }

        }
        else if ('josn' == me.dataType) {

            if (Mini2.isString(value)) {
                try {
                    value = JSON.parse(value);
                }
                catch (ex) {
                    console.error("json 转化为 对象失败. ", value);
                }
            }

            for (var i = 0; i < value.length; i++) {

                var cfg = value[i];

                try {
                    me.add(cfg);
                }
                catch (ex) {
                    console.error("cfg", cfg);
                    console.error('添加对象失败', ex);
                }
            }
        }


        //if (hideEl2) {
        //    hideEl2.val(value);
        //}
    },

    //是否已经初始化预览窗体
    isPreviewed: false,

    /**
     * 预览的元素
     */
    previewEl: null,

    add: function (cfg) {
        "use strict";
        var me = this,
            innerEl = me.fileInnerEl,
            items = me.items,
            inputEl = me.inputEl,
            hideEl2 = me.hideEl2;


        if (!items) {
            me.items = items = [];
        }


        var fileId = "file" + Mini2.newId();

        cfg.file_id = fileId;   //临时id, 使用

        var fileEl = me.createFileEl(cfg);

        items.push(cfg);

        if (fileEl.hasClass('image')) {
            me.imagePanelEl.append(fileEl);
        }
        else {
            if (me.otherPanelEl) {
                me.otherPanelEl.append(fileEl);
            }
            else {
                me.imagePanelEl.append(fileEl);
            }
        }

        var xdd = me.imagePanelEl.html();

        //如果有 $.fn.viewer 多功能图片预览框
        if (window.Viewer) {

            me.initPreviewEl();

        }
        else {
            fileEl.attr('rel', 'boxer' + me.muid)
            fileEl.boxer({

            });
        }

        if (hideEl2) {

            var fileListStr = hideEl2.val();
            fileListStr += "\n" + (cfg.url + "||" + cfg.thumb_url + "||" + cfg.title + "||" + (cfg.code || ''));

            hideEl2.val(fileListStr);
        }

        return me;
    },


    initPreviewTime: null,

    /**
     * 初始化预览元素
     */
    initPreviewEl: function () {
        var me = this,
            time = me.initPreviewTime;


        if (time) {
            time.resetStart();
            return;
        }

        me.initPreviewTime = Mini2.setTimer(function () {
            
            if (me.previewEl) {
                me.previewEl.update();
            }
            else {
                me.previewEl = new Viewer(me.imagePanelEl[0], {
                    url: 'data-original'
                });
            }

        }, [1000, 1])

    },


    addForText: function (cfg) {
        var me = this,
            items = me.items,
            innerEl = me.fileInnerEl,
            inputEl = me.inputEl,
            hideEl2 = me.hideEl2 //这个对象在初始化的时候, 还未出现;



        if (Mini2.isString(cfg)) {

            var ps = cfg.split('||');

            cfg = {
                thumb_url: ps[1] || ps[0],
                url: ps[0],
                title: ps[2],
                code: ps[3]
            };
        }

        me.add(cfg);

        return me;
    },

    getUrlFilename: function (url) {

        var qIndex = url.indexOf('?');

        if (qIndex > 0) {
            url = url.substring(0, qIndex);
        }

        var arrUrl = url.split("/");
        var strPage = arrUrl[arrUrl.length - 1];

        return strPage;
    },

    createFileEl: function (cfg) {
        "use strict";
        var me = this,
            slicer = me.slicer,
            strPage,
            fileEl;
        
        strPage = me.getUrlFilename(cfg.url);

        var ldot = strPage.lastIndexOf(".");
        var type = strPage.substring(ldot).toLowerCase();

        var isImage = false;


        

        if (type == '.jpg' || type == '.jpeg' || type == '.gif' || type == '.png' || type == '.bmp') {
            isImage = true;
        }

        if ('editor' == me.displayMode) {
            fileEl = Mini2.$join([
                '<div class="mi-file thumb " style="" title="', cfg.title, '">',
                '<div class="mi-file-inner thumb-inner" style="width:', me.thumbWidth, 'px;height:', me.thumbHeight,'px;">',
                        '<img class="mi-file-image thumb-img" src="', cfg.thumb_url || cfg.url, '" data-original="', cfg.url || cfg.thumb_url, '" style="height:100%;">',
                    '</div>',
                    '<div class="mi-file-mask"></div>',
                    '<div class="mi-checkbox"></div>',                        
                '</div>',
            ]);

            if (!me.hideThumbTitle) {

                fileEl.append(Mini2.$join([
                    '<div class="mi-file-text" title="', strPage, '">',
                    strPage,
                    '</div>'
                ]));
            }

            $(fileEl).children('.mi-checkbox').muBind('click', function () {


                if ($(fileEl).hasClass('mi-file-check')) {
                    $(fileEl).removeClass('mi-file-check');
                }
                else {
                    $(fileEl).addClass('mi-file-check');
                }
            });



        }
        else if ('preview' == me.displayMode) {

            var thumbUrl = cfg.thumb_url || cfg.url;
            var srcUrl = cfg.url || cfg.thumb_url;

            if (slicer && !isImage) {
                fileEl = Mini2.$join([
                    '<a class="" style="display:block;margin-bottom:6px;" target="_blank" href="', cfg.url, '" title="', cfg.title, '">',
                    '<img class="mi-file-image" src="', thumbUrl, '" style="height:16px;height:16px; margin-right:4px;">',
                        '<span>', cfg.title, '</span>',
                    '</a>'
                ]);
            }
            else {
                fileEl = Mini2.$join([
                    '<div class="mi-file thumb " style=""  title="', cfg.title, '">',
                    '<div class="mi-file-inner thumb-inner" style="width:', me.thumbWidth, 'px;height:', me.thumbHeight, 'px;">',
                    '<img class="mi-file-image thumb-img" src="', thumbUrl, '" data-original="', srcUrl, '" style="height:100%;">',
                        '</div>',

                    '</div>',
                ]);
                

                if (!me.hideThumbTitle) {

                    fileEl.append(Mini2.$join([
                        '<div class="mi-file-text" title="', strPage, '">',
                        strPage,
                        '</div>'
                    ]));
                }
            }

        }


        if (isImage) {
            fileEl.addClass('image');
        }
        else {
            var typeName = type.substring(1);

            $(fileEl).find('.mi-file-image').attr('src', '/res/file_icon_256/' + typeName + '.png');
        }

        fileEl.attr('data-file-id', cfg.file_id);


        return fileEl;
    },

    autoSave: function () {
        "use strict";
        var me = this;

        me._isValueChanged = true;

        var owner = me.ownerParent;

        if (owner && owner.setStoreRecrod) {
            owner.setStoreRecrod.call(owner, me);
        }
    },

    /**
    * 附加一个隐藏元素
    */
    appendHideEl: function () {
        "use strict";
        var me = this,
            hideEl2 = me.hideEl2,
            bodyCls = '.mi-form-item-body:first';

        //return;

        console.debug("隐藏项目....+++++++++++++++++++++++++");

        me.hideEl2 = hideEl2 = Mini2.$join([
            '<input type="hidden"  />'
        ]);

        if (me.name) {
            hideEl2.attr('name', me.name);
        }

        console.debug("附加的值 :", me.oldValue);

        hideEl2.val(me.oldValue);

        me.appendCell(me.el, hideEl2, bodyCls);
    },

    appendUploadBtn: function () {
        "use strict";
        var me = this;


        var uploadBtn = Mini2.create('Mini2.ui.form.field.FileUpload', {

            id: me.id + '_UploadBtn',
            clientId: me.clientId + '_UploadBtn',

            hideLabel: true,
            pluginType: 'other',

            fileUrl: me.fileUrl,
            dock: 'left',
            applyTo: '#' + me.clientId + '_UploadBtn'

        });

        uploadBtn.render();

        uploadBtn.bind('uploadSuccess', function (response) {

            var r = response;
            //var ttt = r.url + '||' + (r.thumb_url || r.url) + '||' + r.original + '||' + (r.code || '')

            var cfg = {
                url: r.url,
                thumb_url: r.thumb_url || r.url,
                title: r.original,
                code: r.code || ''

            };



            me.add(cfg);

            me.autoSave();

            me.callParent();
        });


        if (uploadBtn.el) {
            uploadBtn.el.css({
                'float': 'left',
                width: 0,
                marginRight: '6px'
            });
        }


    },


    //引发事件
    onButtonClick: function () {




        var me = this,
            ownerParent = me.ownerParent,
            store = ownerParent.store,
            record = store.getCurrent();

        var urlStr = Mini2.urlAppend('/App/InfoGrid2/View/OneForm/Dialogs/MoreFileDialog.aspx', {
            table: store.modelName,
            field: me.dataField,
            row_pk: record.getId(),
            tag_code: me.tagCode || 'sub_file'
        }, true);


        var win = Mini2.createTop('Mini2.ui.Window', {
            width: 800,
            height: 600,
            url: urlStr,
            mode: true,
            text: '文件管理'
        });

        win.formClosed(function (e) {

            if (e.success) {

                try {
                    console.debug('界面关闭: ', e);

                    record.set(me.dataField, e.data);
                    //record.afterCommit([me.dataIndex]);
                }
                catch (ex) {
                    console.error("刷新数据仓库错误.", ex);
                }
            }

        });

        win.show();

    },

    render: function () {
        "use strict";
        var me = this,
            el,
            hideEl2;


        me.baseRender();

        el = me.el;



        //$(me.inputEl).find('.remove-btn').click(function () {

        //    me.removeSeleted();
        //});

        if ('editor' == me.displayMode) {

            me.appendHideEl();

            var delBtn = Mini2.create('Mini2.ui.button.Button', {
                text: '删除',
                width: 80,
                height: 28,
                applyTo: el.find('.remove-btn'),
                click: function () {
                    me.removeSeleted();
                }
            });

            delBtn.render();

            delBtn.el.css({ 'float': 'right' });

            me.appendUploadBtn();
        }

        me.el.data('me', me);


    }


});





/**
* 配合多文件列的, 多文件上传
*
*
*/
Mini2.define('Mini2.ui.form.field.MoreFileEditor', {

    extend: 'Mini2.ui.Component',

    buttonCls: 'mi-icon-more',


    //初始化组件
    initComponent: function () {

        var me = this;

        me.fieldCls = 'mi-form-type-text';

        $(me).muBind('focusout', function () {


            console.debug("失去焦点..");

            me.el.removeClass("mi-form-trigger-wrap-focus");

            //me.clearInvalid();

        });


    },


    focus: function () {
        var me = this;

        console.debug("获取焦点..");

        me.el.addClass('mi-form-trigger-wrap-focus');

        Mini2.ui.FocusMgr.setControl(me, null, me.scope);

        return me;
    },


    getFieldEl: function () {
        var me = this,
            log = me.log,
            tableEl,
            btnEl;

        tableEl = Mini2.$join([
            '<div class="" style="width: 120px;  position: relative; ">',

            '</div>'
        ]);


        btnEl = Mini2.$join(['<div role="button" class="mi-form-trigger mi-form-arrow-trigger " style="position: absolute;right:0px;background-color:#fff;border-color: #157fcc;    border-width: 1px; border-style: solid;" ></div>']);

        tableEl.append(btnEl);

        me.buttonEl = btnEl;

        if (me.buttonCls) {
            btnEl.addCls(me.buttonCls);
        }

        btnEl.click(function () {

            me.onButtonClick();


        });

        return tableEl;
    },


    setWidth: function (value) {
        var me = this;

        me.el.css('width', value);

    },


    //引发事件
    onButtonClick: function () {
        var me = this,
            record = me.record,
            store = record.store;

        console.debug("文件编辑", me);

        var urlStr = Mini2.urlAppend('/App/InfoGrid2/View/OneForm/Dialogs/MoreFileDialog.aspx', {
            table: store.modelName,
            field: me.dataIndex,
            row_pk: record.getId(),
            tag_code: me.tagCode || 'sub_file'
        }, true);


        var win = Mini2.createTop('Mini2.ui.Window', {
            width: 800,
            height: 600,
            url: urlStr,
            mode: true,
            text: '文件管理'
        });

        win.formClosed(function (e) {

            if (e.success) {

                try {
                    console.debug('界面关闭: ', e);

                    record.set(me.dataIndex, e.data);
                    //record.afterCommit([me.dataIndex]);
                }
                catch (ex) {
                    console.error("刷新数据仓库错误.", ex);
                }
            }

        });

        win.show();

    },


    value: null,

    setValue: function (value) {
        var me = this;

        console.debug("设置值...", value);

        me.value = value;

        return me;
    },

    getValue: function () {

        var me = this;

        return me.value;
    },


    render: function () {

        var me = this;

        var el = me.getFieldEl();

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }

        el.data('me', me);

        me.el = el;


        return me;

    }

});
