

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




    baseRender: function () {
        "use strict";
        var me = this,
            modalOverlay = me.modalOverlayClass,
            extraClass = me.extraClass,//扩展的 class
            modalHTML, //窗体
            titleHTML, //标题
            textHTML,   //内容
            btnsHTML = '', //按钮集合窗体
            afterTextHTML = me.afterTextHTML,
            verticalButtons = me.verticalButtons ? 'modal-buttons-vertical' : '',
            contentHtml = me.contentHtml,
            btn,
            btns = me.buttons,
            noButtons = (btns && btns.length > 0) ? '' : 'modal-no-buttons';

        titleHTML = me.title ? '<div class="modal-title">' + me.title + '</div>' : '';

        if (contentHtml) {
            textHTML = contentHtml;
        }
        else {
            textHTML = me.text ? '<div class="modal-text">' + me.text + '</div>' : '';
        }

        var _modalTemplateTempDiv = document.createElement('div');


        modalHTML = '<div class="modal ' + extraClass + ' ' + noButtons + '"><div class="modal-inner">' + (titleHTML + textHTML + afterTextHTML) + '</div><div class="modal-buttons ' + verticalButtons + '">' + btnsHTML + '</div></div>';

        _modalTemplateTempDiv.innerHTML = modalHTML;

        var modal = $(_modalTemplateTempDiv).children();

        if (btns && btns.length > 0) {

            var btnGroup = $(modal).children('.modal-buttons');

            for (var i = 0; i < btns.length; i++) {
                btn = btns[i];

                var btnEl = $('<span class="modal-button' + (btn.bold ? ' modal-button-bold' : '') + '">' + btn.text + '</span>');

                btnGroup.append(btnEl);

                btnEl.on('click', function () {
                    var tmpBtn = btn;
                    return function () {
                        tmpBtn.click.call(me, {
                            sender: btnEl
                        });
                    }
                }());
            }
        }



        //创建蒙层
        if ($('.' + modalOverlay).length === 0) {
            $(document.body).append('<div class="' + modalOverlay + '"></div>');
        }

        $(document.body).append(modal);
        
        me.el = modal;

        return modal;
    },

    open :function(){
        var me = this,
            el = me.el,
            modalOverlay = me.modalOverlayClass;

        $('.' + modalOverlay).addClass(modalOverlay + '-visible');

        el.show().css({
            'margin-top': '-76px'
        });

        setTimeout(function () {
            el.addClass('modal-in');
        }, 50);

        return me;
    },


    close:function(){
        var me = this,
            el = me.el,
            modalOverlay = me.modalOverlayClass;


        $("." + modalOverlay).removeClass(modalOverlay + '-visible');

        el.removeClass('modal-in').addClass('modal-out');

        el.remove();

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

    buttons:[{
        text: '确定',
        click: function () {
            
            this.close();
        }
    }],

    onInit: function () {

    }

});


$.alert = function (text, title, callbackOk) {

    var frm = Mini2.create('Mini2.ui.MessageBox', {
        text: text,
        title: title,

        buttons:[{
            text: '确定',
            click: function () {
                this.close();

                if (callbackOk) {
                    callbackOk.call(this);
                }
            }
        }]
    });

    frm.render();
};

$.confirm = function (text, title, callbackOk, callbackCancel) {

    var frm = Mini2.create('Mini2.ui.MessageBox', {
        text: text,
        title: title,

        buttons: [{
            text: '确定',
            click: function () {
                this.close();
                if (callbackOk) {
                    callbackOk.call(this);
                }
            }
        }, {
            text: '取消',
            click: function () {
                this.close();

                if (callbackCancel) {
                    callbackCancel.call(this);
                }
            }
        }]
    });

    frm.render();
};

$.prompt = function (text, title, callbackOk, callbackCancel) {


    var frm = Mini2.create('Mini2.ui.MessageBox', {
        
        contentHtml: '<div class="modal-text">' + text + '</div><input type="text" class="modal-text-input">',

        title: title,

        getValue:function(){
            var me = this,
                el = me.el;

            return $(el).find('.modal-text-input').val();
        },

        setValue:function(value){
            var me = this,
                el = me.el;

            $(el).find('.modal-text-input').val(value);

            return me;
        },

        buttons: [{
            text: '确定',
            click: function (e) {

                if (callbackOk) {
                    var result = callbackOk.call(this);

                    if (result == true || result == undefined) {
                        this.close();
                    }
                }
            }
        }, {
            text: '取消',
            click: function (e) {

                this.close();

                if (callbackCancel) {
                    callbackCancel.call(this);
                }
            }
        }]
    });

    frm.render();

};