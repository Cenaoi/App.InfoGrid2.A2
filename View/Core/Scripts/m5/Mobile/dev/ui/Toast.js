
Mini2.define('Mini2.ui.Toast', {

    extend: 'Mini2.ui.Component',

    text: '消息',

    //停留时间（毫秒）
    duration: 2000,

    //扩展样式
    extraclass: '',
        

    onInit: function () {

    },


    baseRender: function () {
        var me = this,
            tpl,
            el,
            extraClass = me.extraclass || '';

        tpl = [
            '<div class="modal toast ' , extraClass , '">',
                me.text,
            '</div>'
        ];

        me.el = el = me.$join(tpl);

        $(me.renderTo).append(el);
        
    },


    setVisible: function (value) {
        var me = this,
            el = me.el;
        
        el.switchClass('modal-in', 'modal-out', value);
        
        return me;
    },

    open: function () {
        var me = this,
            el = me.el;

        el.show();


        var ow = 0;

        if ($(el).outerWidth) {
            ow = $(el).outerWidth();
        }
        else if ($(el).width) {
            ow = $(el).width();
        }
        
        el.css({
            marginLeft: -Math.round(ow / 2 / 1.185) + 'px'
        });

        me.setVisible(true);
        
        return me;
    },


    close: function () {
        var me = this,
            el = me.el;

        me.setVisible(false);
        
        el.transitionEnd(function (e) {
            el.remove();
        });

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

/**
* 显示一个消息，会在2秒钟后自动消失
* 
* @param {String} text 提示的内容
* @param {Number} duration 停留时间, 单位(毫秒) 
* @param {String} extraclass 扩展的样式名称
* @return {Mini2.ui.Toast} 提示框对象
*/
$.toast = function (text, duration, extraclass) {


    var frm = Mini2.create('Mini2.ui.Toast', {
        text: text,
        duration: duration || 2000,
        extraclass: extraclass
    });

    frm.render();

    return frm;
};