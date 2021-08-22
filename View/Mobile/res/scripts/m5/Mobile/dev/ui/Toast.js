

Mini2.define('Mini2.ui.Toast', {

    text: '消息',

    //停留时间（毫秒）
    duration: 2000,

    //扩展样式
    extraclass: '',
        

    onInit: function () {

    },


    baseRender: function () {
        var me = this,
            extraClass = me.extraclass || '';

        var el = $('<div class="modal toast ' + extraClass + '">' + me.text + '</div>');

        $(me.renderTo).append(el);

        me.el = el;
    },


    open: function () {
        var me = this,
            el = me.el;

        el.css({
            'display': 'block',
            'margin-top': '-31px',
            'margin-left': '-60px'
        });

        setTimeout(function () {
            el.addClass('modal-in');
        }, 50);

        return me;
    },


    close: function () {
        var me = this,
            el = me.el;

        el.removeClass('modal-in').addClass('modal-out');

        setTimeout(function(){
            el.remove();
        },500);


        return me;
    },


    render: function () {
        var me = this;

        me.baseRender();

        me.open();

        setTimeout(function () {
            
            me.close();
                        
        }, me.duration);
    }

});

//显示一个消息，会在2秒钟后自动消失
$.toast = function (text, duration, extraclass) {

    var frm = Mini2.create('Mini2.ui.Toast', {
        text: text,
        duration: duration || 2000,
        extraclass: extraclass
    });

    frm.render();
};