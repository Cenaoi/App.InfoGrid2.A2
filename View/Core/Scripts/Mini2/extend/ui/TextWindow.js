/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />


//文本框窗体
Mini2.define('Mini2.ui.extend.TextWindow', {

    extend: 'Mini2.ui.Window',

    log: Mini2.Logger.getLogger('Mini2.ui.extend.TextWindow'),


    mode: true,
    text: '文本框',
    iframe: false,
    startPosition: 'center_screen',

    //fontSize: '30px',
    padding: {
        left: 8,
        top: 8,
        right: 8,
        bottom: 8
    },

    /**
    * 消息内容
    */
    msg: '请输入:',

    contentTpl: [
        '<div data-dock="full" class="mi-form-item-body">',
            '<div class="mi-input-msg" style="padding:8px;line-height: 20px;"></div>',
            '<textarea class="mi-input-box mi-form-field mi-form-text mi-form-textarea" style="width:590px;height:250px;resize: none;padding: 8px;"  spellcheck="false" autocapitalize="off" autocomplete="off" autocorrect="off">',
                '',
            '</textarea>',
        '</div>',
    ],


    getContentEl: function () {
        var me = this,
            contentEl;

        contentEl = Mini2.$join(me.contentTpl);

        if (me.contentFontSize) {
            contentEl.css({
                'font-size': me.contentFontSize
            });
        }

        return contentEl;
    },

    buttons: [{
        text: '确定',
        width: 80,
        click: function () {
            var me = this,
                win = me.ownerWindow;

            if (win.okClick) {
                win.okClick.call(this);
            } else {
                win.close({
                    result: 'ok',
                    value: win.getValue()
                });
            }
        }
    }, {
        text: '取消',
        width: 80,
        click: function () {
            var me = this,
                win = me.ownerWindow;

            if (win.cancelClick) {
                win.cancelClick.call(this);
            }
            else {
                win.close({ result: 'cancel' });
            }
        }
    }],

    okClick: false,

    cancelClick: false,

    setValue: function (value) {
        var me = this;

        $(me.contentEl).children('.mi-input-box').val(value);
        return me;
    },

    getValue: function () {
        var me = this,
            log = me.log,
            contentEl = me.contentEl,
            value;

        value = $(contentEl).children('.mi-input-box').val();

        return value;
    },

    focus: function () {
        var me = this,
            contentEl = me.contentEl;
            
        this.focusBase();

        $(contentEl).children('.mi-input-box').focus();

        return me;
    },


    showBefore: function () {
        var me = this,
            contentEl = me.contentEl;

        var w = contentEl.width();
        var h = contentEl.height();

        var msgEl = contentEl.children('.mi-input-msg');

        msgEl.html(me.msg);

        var msgH = msgEl.outerHeight();

        var textHeight = h - msgH - 30;

        $(contentEl).children('.mi-input-box').height(textHeight);

        return me;
    }

});





//文件下载
Mini2.define('Mini2.ui.extend.DownloadWindow', {

    extend: 'Mini2.ui.Window',


    mode: true,
    text: '下载',
    iframe: false,
    startPosition: 'center_screen',

    okClick: false,

    cancelClick: false,

    width: 300,
    height: 200,

    fileAEl: null,

    fileName: '下载文件',

    fileUrl: false,

    contentTpl: [
        '<div style="width:590px;height:323px;text-align:center;" dock="full">',
            '<div style="height:20px;"></div>',
            '<a href="下载地址" class="mi-file-download" target="_blank" >下载文件</a>',
        '</div>'
    ],

    buttons: [{
        text: '关闭',
        width: 80,
        dock: 'center',
        click: function () {
            var me = this,
                win = me.ownerWindow;

            if (win.cancelClick) {
                win.cancelClick.call(this);
            }
            else {
                win.close({ result: 'cancel' });
            }
        }
    }],


    //显示下拉按钮
    render_DownBtn: function () {
        var me = this,
            fileAEl,
            url = me.fileUrl || me.fielUrl; //历史上写错为 fielUrl

        me.fileAEl = fileAEl = $(me.contentEl).children('.mi-file-download');

        fileAEl.html(me.fileName)
            .attr('href', url);
    },


    render: function () {
        var me = this;

        me.renderMask();
        me.renderEl();

        me.renderContent();

        me.render_DownBtn();

        me.setSize(me.width, me.height);
        me.setLocation(me.left, me.top, true);


    }


});




//富文本框窗体
Mini2.define('Mini2.ui.extend.HtmlWindow', {

    extend: 'Mini2.ui.Window',


    mode: true,
    text: '富文本框',
    iframe: false,
    startPosition: 'center_screen',
    ///这是一开始的值
    ueditValue: '',
    contentTpl: [
        '<textarea dock="full"  >',
        '</textarea>'
    ],

    buttons: [{
        text: '确定',
        width: 80,
        click: function () {
            var me = this,
                win = me.ownerWindow;

            if (win.okClick) {
                win.okClick.call(this);
            } else {
                win.close({ result: 'ok' });
            }
        }
    }, {
        text: '取消',
        width: 80,
        click: function () {
            var me = this,
                win = me.ownerWindow;

            if (win.cancelClick) {
                win.cancelClick.call(this);
            }
            else {
                win.close({ result: 'cancel' });
            }
        }
    }],

    okClick: false,

    cancelClick: false,

    setValue: function (value) {
        var me = this;
        me.uedit.setContent(value);
        return me;
    },

    getValue: function () {
        var me = this;

        return me.uedit.getContent();
    },


    setHeight: function (value) {
        var me = this;

        this.setSize(undefined, value);

        var h = $(me.bodyEl).outerHeight();


        me.uedit.setHeight(h - 150);

    },

    //显示窗体
    show: function () {

        if (!this.el) {
            this.render();
        }

        var me = this,
            el = me.el,
            shadowEl = me.shadowEl;

        if (me.shadowVisible) {
            shadowEl.show();
        }


        el.show();
        me.focus();

        me.contentId = 'UEdit_' + Mini2.getIdentity();

        me.contentEl.attr('value', me.ueditValue);
        me.contentEl.attr('id', me.contentId);



        var h = $(me.bodyEl).outerHeight();

        me.uedit = UE.getEditor(me.contentId, {
            autoHeightEnabled: false,
            zIndex: me.zIndex + 20,
            initialFrameHeight: h - 150,
            scaleEnabled: false

        });



        return me;
    }

});



