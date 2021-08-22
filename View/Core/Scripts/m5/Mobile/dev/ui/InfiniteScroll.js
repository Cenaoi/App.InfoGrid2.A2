
/**
 * 无限加载
 *
 */
Mini2.define('Mini2.ui.InfiniteScroll', {

    el: null,

    pageContentEl: null,

    distance: 50,

    //已经出发加载
    isLoading: false,

    /**
     * 无限滚动激活
     */
    enabled : true,

    /**
     * 初始化当前页面元素的内容
     * @parem {Dom} pageContainer 页面节点元素
     */
    onInit: function (pageContainer) {
        var me = this,
            pageContentEl,
            distance,
            el = pageContainer || me.el;

        el = $(el);
        
        me.pageContentEl = pageContentEl = el.hasClass('infinite-scroll') ? el : el.find('.infinite-scroll');


        distance = pageContentEl.attr('data-distance');
        
        me.distance = distance || me.distance;

        if (Mini2.isString(me.distance)) { me.distance = parseInt(me.distance); }

        if (0 === pageContentEl.length ) return;

        $(pageContentEl).on('scroll', function () {

            if (!me.enabled) { return; }

            if (me.isLoading) { return; }

            Mini2.awaitRun(null, 100, me, function () {
                me.handle();
            });

        });
    },

    

    handle: function () {
        var me = this,
            el = me.el,
            h,sTop, sHeight,
            pageContentEl = me.pageContentEl;
           

        h = pageContentEl.height();
        sTop = pageContentEl.scrollTop();

        sHeight = pageContentEl[0].scrollHeight;

        if (sTop + h >= sHeight - me.distance) {
            
            me.isLoading = true;
            
            console.debug('呵呵! 到底了, 触发事件 page.bind("infinite", data, callback ) .');

            me.ownerPage.on('infinite', null, function (enable) {
                me.endLoad();

                if (false === enable) {
                    me.setEnabled(false);
                }
            });            
        }

    },

    /**
     * 设置无限模式激活
     */
    setEnabled: function(value){
        var me = this;

        console.debug('激活触发无限滚动的事件.' + value);

        me.enabled = !!value;

        return me;
    },


    /**
     * 结束加载
     */
    endLoad : function(){
        var me = this;

        me.isLoading = false;

        return me;
    }


});