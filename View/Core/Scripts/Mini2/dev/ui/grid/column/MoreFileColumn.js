
/// <reference path="../../Mini2.js" />
/// <reference path="../../lang/Number.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

/**
* 多文件展示
*/
Mini2.define('Mini2.ui.grid.column.MoreFileColumn', {

    extend: 'Mini2.ui.grid.column.Colunm',

    thumbWidth: 72,

    thumbHeight: 72,

    //扩展字段
    innerCls: 'mi-file-panel',

    //内部不隐藏
    innerAutoHide: false,

    dataType: 'text',


    /**
    * 视图类型
    * list=列表模式, large=大图展示
    */
    view: 'large',

    IMAGE_TYPES: ['.jpg', '.jpeg', '.gif', '.png', '.bmp'],

    /**
     * 判断是否为图片类型
     */
    isImgType: function (fileEx) {
        var me = this;

        return Mini2.Array.contains(me.IMAGE_TYPES, fileEx, true);
    },

    createFileEl: function (cfg) {
        "use strict";
        var me = this,
            tW = me.thumbWidth || 16,
            tH = me.thumbHeight || 16;

        var arrUrl = cfg.url.split("/");
        var strPage = arrUrl[arrUrl.length - 1];



        var ldot = cfg.url.lastIndexOf(".");
        var type = cfg.url.substring(ldot).toLowerCase();

        var fileEl;

        console.debug("cfg ==== ", cfg);
        console.debug("type ==== ", type);


        if ('list' == me.view) {

            fileEl = Mini2.$join([
                '<a class="" style="display:block;margin-bottom:6px;" target="_blank" href="', cfg.url, '" title="', cfg.title, '">',
                    '<img class="mi-file-image" src="', cfg.thumb_url || cfg.url, '" data-original=', cfg.url, ' style="height:16px;height:16px; margin-right:4px;">',
                    '<span>', cfg.title, '</span>',
                '</a>'
            ]);

        }
        else if ('large' == me.view) {
            if (!me.isImgType(type)) {
                fileEl = Mini2.$join([
                    '<a class="mi-file thumb " style="width:', me.thumbWidth + 10, 'px;height:', me.thumbHeight + 10, 'px;" target="_blank"  href="', cfg.url, '">',
                        '<div class="mi-file-inner thumb-inner" style="width:', me.thumbWidth, 'px;height:', me.thumbHeight, 'px;">',
                            '<img class="mi-file-image thumb-img" src="', cfg.thumb_url || cfg.url, '"  style="height:100%;">',
                        '</div>',
                    '</a>'
                ]);
            }
            else {
                fileEl = Mini2.$join([
                    '<div class="mi-file thumb " style="width:', me.thumbWidth + 10, 'px;height:', me.thumbHeight + 10, 'px;" target="_blank"  href="', cfg.url, '">',
                        '<div class="mi-file-inner thumb-inner" style="width:', me.thumbWidth, 'px;height:', me.thumbHeight, 'px;">',
                            '<img class="mi-file-image thumb-img" src="', cfg.thumb_url || cfg.url, '" data-original=', cfg.url, ' style="height:100%;">',
                        '</div>',
                    '</div>'
                ]);
            }
        }


        fileEl.attr('title', cfg.title);

        fileEl.attr('data-file-id', cfg.file_id);



        if (me.isImgType(type)) {
            fileEl.addClass('image');
        }
        else {
            var typeName = type.substring(1);

            $(fileEl).find('.mi-file-image').attr('src', '/res/file_icon_256/' + typeName + '.png');
        }

        //$(fileEl).muBind('click', function () {

        //    if ($(this).hasClass('mi-file-check')) {
        //        $(this).removeClass('mi-file-check');
        //    }
        //    else {
        //        $(this).addClass('mi-file-check');
        //    }
        //});

        return fileEl;
    },


    getFieldEl: function () {
        "use strict";
        var me = this,
            slicer = me.slicer,
            inputEl;

        //list=列表模式, large=大图展示

        if ('large' == me.view) {
            inputEl = Mini2.$join([
                '<div>',
                    '<div class="file-inner"  style=" padding:0px; height: ', me.thumbHeight + 10, 'px; overflow: hidden;">',

                    '</div>',
                '</div>'
            ]);
        }
        else if ('list' == me.view) {
            inputEl = Mini2.$join([
                '<div>',
                    '<div class="file-inner"  style=" padding:0px; height: auto; ">',

                    '</div>',
                '</div>'
            ]);
        }


        var fileInnerEl = $(inputEl).children('.file-inner');
        //if (me.name) {
        //    inputEl.attr('name', me.name);
        //}

        inputEl.css({
            'width': me.width,
            'height': me.height
        });


        if (me.readOnly) {
            inputEl.attr('readonly', 'readonly');
        }

        return inputEl;

    },


    addForText: function (fileInnerEl, cfg) {
        "use strict";
        var me = this;

        if (Mini2.isString(cfg)) {

            var ps = cfg.split('||');

            cfg = {
                thumb_url: ps[1] || ps[0],
                url: ps[0],
                title: ps[2],
                code: ps[3] || ''
            };
        }

        try {
            var fileEl = me.createFileEl(cfg);

            fileInnerEl.append(fileEl);

        }
        catch (ex) {
            console.error("text", cfg);
            console.error('添加对象失败', ex);
        }
    },


    renderer: function (value, fieldValue, cellValues, record, recordIndex, columnIndex, store, table) {
        "use strict";
        var me = this,
            fieldEl;

        fieldEl = me.getFieldEl();


        try {
            var fileInnerEl = $(fieldEl).find('.file-inner');

            var count = 0;

            if ('text' == me.dataType) {

                if (!Mini2.isBlank(value)) {
                    var lines = value.split('\n');

                    for (var i = 0; i < lines.length; i++) {

                        var text = lines[i];

                        if (Mini2.isBlank(text)) {
                            continue;
                        }

                        try {
                            me.addForText(fileInnerEl, text);

                            count++;
                        }
                        catch (ex) {
                            console.error("text", text);
                            console.error('添加对象失败', ex);
                        }
                    }

                    if (count == 0) {
                        return '';
                    }
                }
                else {
                    return '';
                }

            }

            var newGroupId = 'boxer' + Mini2.newId();

            var aImgs = fileInnerEl.children('.image');

            aImgs.attr('rel', newGroupId);

            if ('large' == me.view) {
                fileInnerEl.css('width', count * (me.thumbWidth + 10 + 12));
            }
            else if ('view' == me.view) {
                fileInnerEl.css('height', count * (16));
            }
        }
        catch (ex2) {

            console.error("eeeeeeeeeeee", ex2);

        }


        return fileInnerEl;
    },


    bindEvent: function (view, cell, recordIndex, cellIndex, e, record, row) {
        "use strict";
        var me = this,
            innerEl = cell.find('.file-inner:first');

        cell.children('.mi-grid-cell-inner').css('max-height', me.thumbHeight + 36);

        var imgBoxEl = innerEl.children();


        if (window.Viewer) {

            if (0 !== imgBoxEl.length) {
                me.initPreviewEl(cell, innerEl);
            }
        }
        else {
            imgBoxEl.boxer({

            });
        }


        //$(btns).muBind('mousedown', Mini2.emptyFn)
        //.muBind('mouseup', function (e2) {

        //    me.processEvent("mouseup", view, cell, recordIndex, cellIndex, e2, record, row);

        //    Mini2.EventManager.stopEvent(e2);

        //}).on('click', function (e2) { return false; });
    },



    /**
     * 初始化预览元素
     */
    initPreviewEl: function (cellEl, imgBoxEl) {
        var me = this,
            time = cellEl.data('preview_time'),
            el = cellEl.data('preview_el');


        if (time) {
            time.resetStart();
            return;
        }


        time = Mini2.setTimer(function () {

            if (el) {
                el.update();
            }
            else {
                el = new Viewer(cellEl[0], {
                    url: 'data-original'
                });

                cellEl.data('preview_el', el);
            }

        }, [1000, 1])

        cellEl.data('preview_time', time);
    }

});


