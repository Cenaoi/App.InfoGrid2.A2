
/**
 * 滚动条
 */
Mini2.define('Mini2.ui.Scroller', {

    /**
     * 页面内容的容器
     */
    pageContentEl: null,


    onInit: function (content) {

        var me = this,
            el = content || me.pageContentEl;

        el.addClass('native-scroll');

    },

    //获取scroller对象
    getScroller: function (content) {
        var me = this,
            el = content || me.pageContentEl;

        //以前默认只能有一个无限滚动，因此infinitescroll都是加在content上，现在允许里面有多个，因此要判断父元素是否有content
        el = el.hasClass('content') ? el : el.parents('.content');

        if (el) {
            return $(el).data('scroller');
        }
        else {
            return $('.content.javascript-scroll').data('scroller');
        }
    },

    //检测滚动类型,
    //‘js’: javascript 滚动条
    //‘native’: 原生滚动条
    detectScrollerType: function (content) {

        var me = this,
            el = content || me.pageContentEl ;

        if (el) {
            if ($(el).data('scroller') && $(el).data('scroller').scroller) {
                return 'js';
            }
            else {
                return 'native';
            }
        }
    },


});