

Mini2.define('Mini2.ui.Window', {

    extend: 'Mini2.ui.Component',


    onInit: function () {

    },

    title: false,

    text: false,

    contentHtml: false,

    buttons: false,

    afterTextHTML :'',

    extraClass: '',

    modalOverlayClass: 'modal-overlay',



    setTitle:function(value){
        var me = this,
            el = me.el;

        me.title = value;

        $(el).find('.modal-title').html(value);
    },


    getTitle:function(){
        var me = this;

        return me.title;
    },


    baseRender: function () {
        "use strict";
        var me = this,
            extraClass = me.extraClass,//扩展的 class
            modalHTML, //窗体
            titleHTML = '', //标题
            textHTML = '',   //内容
            btnsHTML = '', //按钮集合窗体
            afterTextHTML = me.afterTextHTML,
            verticalButtons = me.verticalButtons ? 'modal-buttons-vertical' : '',
            contentHtml = me.contentHtml,
            contentEl = me.contentEl,

            btns = me.buttons || [],
            noButtons = (btns.length ? '' : ' modal-no-buttons');


        if (me.title) {
            titleHTML = me.join(['<div class="modal-title">', me.title, '</div>']);
        }

        // 如果有 contentEl 元素存在, 就另外处理
        if (contentEl) {
            textHTML = '';
        }
        else if (contentHtml) {
            textHTML = contentHtml;
        }
        else if(me.text){
            textHTML = me.join(['<div class="modal-text">', me.text, '</div>']);
        }

        var _modalTemplateTempDiv = document.createElement('div');


        modalHTML = me.join([
            '<div class="modal ' , extraClass , noButtons , '">',
                '<div class="modal-inner">' , titleHTML , textHTML , afterTextHTML, '</div>',
                '<div class="modal-buttons ' , verticalButtons , '">' , btnsHTML , '</div>',
            '</div>'
        ]);

        _modalTemplateTempDiv.innerHTML = modalHTML;

        var modal = $(_modalTemplateTempDiv).children();

        if (contentEl) {
            $(modal).children('.modal-inner').append(contentEl);
        }

        me.initButtonGroup(modal);

        me.showOverlay();       //显示蒙层
        

        me.renderTo = Mini2.getBody();

        $(me.renderTo).append(modal);
        
        me.el = modal;

        return modal;
    },


    /**
    * 初始化按钮组
    *
    * @param {Object} modal 
    */
    initButtonGroup: function (modal) {
        "use strict";
        var me = this,
            i,
            btnEl,
            btnGroup,
            btn,
            btns = me.buttons || [],
            len = btns.length;

        if (len) {

            btnGroup = $(modal).children('.modal-buttons');

            for (i = 0; i < len; i++) {
                btn = btns[i];

                btnEl = me.createButton(btn);

                btnGroup.append(btnEl);
            }
        }
        
    },

    /**
    * 创建按钮 Html 节点元素
    *
    * @param {Object} btn 按钮对象
    * @return {Dom Element} 按钮的html节点元素
    */
    createButton: function (btn) {
        "use strict";
        var me = this,
            btnEl,
            boldCls = (btn.bold ? ' modal-button-bold' : '');

        btnEl = me.$join(['<span class="modal-button' , boldCls , '">' , btn.text , '</span>']);

        btnEl.on('click', function () {
            var tmpBtn = btn;
            return function () {
                tmpBtn.click.call(me, {
                    sender: btnEl
                });
            }
        }());

        return btnEl;
    },

    //显示蒙层
    showOverlay:function(){
        var me = this,
            overlayCls = me.modalOverlayClass,
            overlayVisibleCls = overlayCls +'-visible';

        //创建蒙层
        if ($('.' + overlayCls).length === 0) {
            $(document.body).append('<div class="' + overlayCls + '"></div>');
        }

        $('.' + overlayCls).addClass(overlayVisibleCls);

        return me;
    },

    hideOverlay:function(){
        var me = this,
            overlayCls = me.modalOverlayClass,
            overlayVisibleCls = overlayCls + '-visible';

        $("." + overlayCls).removeClass(overlayVisibleCls);

        return me;
    },


    setVisible: function (value) {
        var me = this,
            el = me.el;

        el.switchClass('modal-in', 'modal-out', value);

        return me;
    },

    open :function(){
        var me = this,
            el = me.el;


        el.show().css({
            marginTop: -Math.round(el.outerHeight() / 2) + 'px'
        });


        me.setVisible(true);
        
        return me;
    },


    close:function(){
        var me = this,
            el = me.el;

        me.hideOverlay();

        me.setVisible(false);

        el.transitionEnd(function (e) {
            el.remove();
            console.debug('关闭窗体.');
        });
        

        return me;
    },

    render: function () {
        "use strict";
        var me = this,
            el = me.el;
        
        me.baseRender();

        me.open();
    }


});


Mini2.define('Mini2.ui.MessageBox', {

    extend: 'Mini2.ui.Window',

    buttons: [{
        title:'消息框',
        text: '确定',
        click: function () {
            this.close();
        }
    }],

    onInit: function () {

    }

});



//强制提示框
$.alert = function (text, title, callback_ok) {

    if (typeof title === 'function') {
        callback_ok = arguments[1];
        title = undefined;
    }

    var frm = Mini2.create('Mini2.ui.MessageBox', {
        text: text,
        title: title,

        callback_ok: callback_ok,

        buttons: [{
            text: '确定',
            click: function () {
                var me = this,
                    callback = me.callback_ok;

                me.close();

                if (callback) {
                    callback.call(me);
                }
            }
        }]
    });

    frm.render();

    return frm;
};

//询问框
$.confirm = function (text, title, callback_ok, callback_cancel) {

    if (typeof title === 'function') {
        callback_cancel = arguments[2];
        callback_ok = arguments[1];
        title = undefined;
    }

    var frm = Mini2.create('Mini2.ui.MessageBox', {
        text: text,
        title: title,

        callback_ok: callback_ok,
        callback_cancel: callback_cancel,

        buttons: [{
            text: '确定',
            click: function () {
                var me = this,
                    callback = me.callback_ok;

                me.close();
                if (callback) {
                    callback.call(me);
                }
            }
        }, {
            text: '取消',
            click: function () {
                var me = this,
                    callback = me.callback_cancel;

                me.close();

                if (callback) {
                    callback.call(me);
                }
            }
        }]
    });

    frm.render();

    return frm;
};


Mini2.define('Mini2.ui.PromptWindow', {

    extend: 'Mini2.ui.MessageBox',


    contentHtml: '<div class="modal-text">请输入：</div><input type="text" class="modal-text-input">',

    title: '标题',

    callback_ok: false,
    callback_cancel: false,

    getValue: function () {
        var me = this,
            el = me.el;

        return $(el).find('.modal-text-input').val();
    },

    setValue: function (value) {
        var me = this,
            el = me.el;

        $(el).find('.modal-text-input').val(value);

        return me;
    },

    render: function () {
        "use strict";
        var me = this,
            el,
            inputEl;

        me.baseRender();
        me.open();

        el = me.el;
        inputEl = el.find('.modal-text-input');

        if (me.value) {
            inputEl.val(me.value);
        }


        setTimeout(function () {
            inputEl.focus();
        }, 300);
    },


    buttons: [{
        text: '确定',
        bold: true,
        click: function (e) {
            var me = this,
                callback = me.callback_ok,
                result;

            if (callback) {
                result = callback.call(me);

                if (false !== result) {
                    me.close();
                }
            }
        }
    }, {
        text: '取消',
        click: function (e) {
            var me = this,
                callback = me.callback_cancel;

            me.close();

            if (callback) {
                callback.call(me);
            }
        }
    }]

});


/**
* 输入框
*
* @param {String} text 内容
* @param {String} title 标题
* @param {Function} callback_ok 点击确定按钮的回调函数
* @param {Function} callback_cancel 点击取消按钮回调函数
* @return {MessageBox}
*/
$.prompt = function (text, title, callback_ok, callback_cancel) {

    if (typeof title === 'function') {
        callback_cancel = arguments[2];
        callback_ok = arguments[1];
        title = undefined;
    }

    var frm = Mini2.create('Mini2.ui.PromptWindow', {

        contentHtml: '<div class="modal-text">' + text + '</div><input type="text" class="modal-text-input">',

        title: title,

        callback_ok: callback_ok,
        callback_cancel: callback_cancel,

    });


    frm.render();
    return frm;
};