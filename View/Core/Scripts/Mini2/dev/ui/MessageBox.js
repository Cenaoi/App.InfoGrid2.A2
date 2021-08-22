/// <reference path="../Mini.js" />


Mini2.define('Mini2.ui.MessageBox', {

    extend: 'Mini2.ui.Window',

    mode: true,

    buttonAlign: 'center',

    startPosition: 'center_screen',

    width:350,

    height:130,

    icon:'info',

    buttons: [{
        text: '确定',
        width: 80,
        click: function (e) {
            this.ownerWindow.close();
        }
    }],



    /**
    * @property
    * The CSS class that provides the INFO icon image
    */
    INFO: Mini2.baseCSSPrefix + 'message-box-info',
    /**
    * @property
    * The CSS class that provides the WARNING icon image
    */
    WARNING: Mini2.baseCSSPrefix + 'message-box-warning',
    /**
    * @property
    * The CSS class that provides the QUESTION icon image
    */
    QUESTION: Mini2.baseCSSPrefix + 'message-box-question',
    /**
    * @property
    * The CSS class that provides the ERROR icon image
    */
    ERROR: Mini2.baseCSSPrefix + 'message-box-error',

    contentTpl: [
         '<div class="mi-box-inner " role="presentation" style="height: 55px; width: 339px;" dock="full">',
            '<div class="mi-box-target" style="width: 339px;">',
                '<div class="mi-container mi-box-item mi-window-item mi-container-default mi-box-layout-ct" ',
                    'style="overflow: hidden; padding: 10px; margin: 0px; right: auto; left: 0px; top: 0px;">',



                '</div>',
            '</div>',
        '</div>'],

    contentTpl2:[

        '<div class="mi-box-inner " role="presentation" style="width: 319px; height: 35px;">',
            '<div class="mi-box-target" style="width: 319px;">',
                '<div class="mi-component mi-box-item mi-component-default mi-dlg-icon " ',
                    'style="width: 50px; height: 35px; margin: 0px; right: auto; left: 0px; top: 0px;">',
                '</div>',
                '<div class="mi-container mi-box-item mi-container-default" style="margin: 0px;right: auto; left: 50px; top: 0px;">',
                    '<span style="display: table;">',
                        '<div class="mi-message-box-textbox" style="height: 100%; vertical-align: top;  white-space:nowrap;">',

                        '</div>',
                    '</span>',
                '</div>',
            '</div>',
        '</div>'
    ],

    title: '标题',

    msg: '提示信息',
        
    renderHeader: function (el) {
        "use strict";
        var me = this,
            header;
        if (me.headerVisible) {
            header = me.header = Mini2.create('Mini2.ui.WindowHeader', {
                width: me.width,
                owner: me,
                renderTo: el,
                text: me.title
            });
            header.render();
            $(header).bind('action', me, me.header_Action);
        }
    },

    renderContent: function () {
        "use strict";
        var me = this,
            sysInfo = Mini2.SystemInfo,
            cfg = sysInfo.window,
            content2El,
            contentEl,
            iconEl,
            inputEl,
            inputBodyEl,
            bodyEl = me.bodyEl;

        me.contentEl = contentEl = Mini2.$join(me.contentTpl);
        
        bodyEl.append(contentEl);

        me.content2El = content2El = Mini2.$join(me.contentTpl2);

        $(contentEl).find('.mi-container').append(content2El);



        me.iconEl = iconEl = content2El.find('.mi-dlg-icon');
        me.inputBodyEl = inputBodyEl = content2El.find('.mi-message-box-textbox');

        //默认 'info': 
        switch (me.icon) {
            case 'error': iconEl.addClass(me.ERROR);break;
            case 'question':iconEl.addClass(me.QUESTION);break;
            case 'warning': iconEl.addClass(me.WARNING);break; 
            default: iconEl.addClass(me.INFO);break;  
        }

        inputBodyEl.html(me.msg);

        //me.inputEl = inputEl = Mini2.create('Mini2.ui.form.Label', {
        //    renderTo: inputBodyEl,
        //    hideLabel: true,
        //    value: me.msg
        //});

        //inputEl.render();
    },



    render: function () {
        "use strict";
        var me = this,
            sysInfo = Mini2.SystemInfo,
            cfg = sysInfo.window;

        me.renderMask();
        me.renderEl();

        me.renderContent();


        var msgHeight = me.inputBodyEl.height();
        var msgWidth = me.inputBodyEl.width() ;

        if (msgWidth < 200) { msgWidth = 200; }
        if (msgHeight < 40) { msgHeight = 40; }

        var footerHeight = me.buttonPanel.getHeight();

        me.content2El.height(msgHeight);
        me.content2El.width(msgWidth + 50);

        var innerEl = me.content2El.children('.mi-box-inner');

        me.height = msgHeight + (cfg.headerHeight + 16) + footerHeight;   //  || cfg.msgHeight || me.height;

        me.height += 30;

        me.width = msgWidth + 40 + 50;

        me.setSize(me.width, me.height);
        me.setLocation(me.left, me.top, true);

        setTimeout(function () {

            me.buttons[0].focus();

        }, 200);
    }



});



//带文本框的收入窗体
Mini2.define('Mini2.ui.PromptWinddow', {

    extend: 'Mini2.ui.Window',

    width:500,
    height:180,

    mode: true,
    text: '文本框',
    iframe: false,
    startPosition: 'center_screen',

    contentTpl: [
        '<div style="padding:10px;" class="mi-form-item-body">',
            '<div class="input-message" style="height:40px;">请输入:</div>',
            '<input type="text" style="width:100%;resize: none;" class="input-box mi-form-field mi-form-text">',
                '',
            '</input>',
        '</div>'
    ],

    buttons: [{
        text: '确定',
        width: 80,
        click: function () {
            var me = this,
                win = me.ownerWindow,
                okClick = win.okClick,
                cancel ;

            if (okClick) {
                cancel = okClick.call(win);

                if (true === cancel) {
                    return;
                }
            }

            win.close({ result: 'ok' });
        }
    }, {
        text: '取消',
        width: 80,
        click: function () {
            var me = this,
                win = me.ownerWindow;

            if (win.cancelClick) {
                win.cancelClick.call(win);
            }

            win.close({ result: 'cancel' });
            
        }
    }],

    okClick: false,

    cancelClick: false,


    setValue: function (value) {
        "use strict";
        var me = this;

        $(me.contentEl).children('.input-box').val(value);
        return me;
    },

    getValue: function () {
        "use strict";
        var me = this;

        return $(me.contentEl).children('.input-box').val();
    },


    render: function () {
        "use strict";
        var me = this,
            contentEl;

        me.renderBase();

        contentEl = $(me.contentEl);

        contentEl.children('.input-message').html(me.message);

        setTimeout(function () {

            contentEl.children('.input-box').focus();

        }, 100);
    }


});




Mini2.Msg = {

    /**
    * 提示框
    **/
    alert: function (title, msg, fn) {
        "use strict";

        var msgBox, cfg;

        if (typeof msg === 'function') {
            fn = msg;
            msg = title;
            title = '';
        }
        else if (1 == arguments.length) {
            msg = title;
            title = '提示';
        }

        cfg = {
            title: title,
            msg: msg,
            icon: 'info',
            buttons: [{
                text: '确定',
                width: 80,
                click: fn ? fn : function (e) {
                    this.ownerWindow.close();
                }
            }]
        };

        msgBox = Mini2.createTop('Mini2.ui.MessageBox', cfg);
        msgBox.show();

        return msgBox;
    },




    /**
    * 询问
    **/
    confirm: function (title, msg, fn, cancelFn) {
        "use strict";
        var msgBox, cfg;

        console.debug("title", title);
        console.debug("msg", msg);

        if (typeof msg === 'function') {
            cancelFn = fn;
            fn = msg;
            msg = title;
            title = '询问';
        }


        cfg = {
            title: title,
            msg: msg,
            icon: 'question',
            buttons: [{
                text: '确定',
                width: 80,
                click: function (e) {
                    var result;

                    if (fn) {
                        result = fn.call(this);
                    }

                    if (false !== result) {
                        this.ownerWindow.close();
                    }
                }
            }, {
                text: '取消',
                width: 80,
                click: function (e) {
                    var result;

                    if (cancelFn) {
                        result = cancelFn.call(this);
                    }

                    if (false !== result) {
                        this.ownerWindow.close();
                    }
                }
            }]
        };


        msgBox = Mini2.createTop('Mini2.ui.MessageBox', cfg);
        msgBox.show();

        return msgBox;
    },


    /**
    * 文字收入框
    **/
    prompt: function (title, msg, fn, cancelFn) {
        "use strict";
        var msgBox;
        
        if (typeof msg === 'function') {
            cancelFn = fn;
            fn = msg;
            msg = title;
                        
            title = '';
        }

        msgBox = Mini2.createTop('Mini2.ui.PromptWinddow', {

            text: title,
            message: msg,
            
            okClick: fn,

            cancelClick: cancelFn

        }); 

        msgBox.show();

        return msgBox;
    }

}