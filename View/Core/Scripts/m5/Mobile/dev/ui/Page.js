


//弹出窗体
Mini2.define('Mini2.ui.Page', {

    extend: 'Mini2.ui.Component',

    //动画开始事件
    //pageAnimationStart: Mini2.emptyFn,

    //动画结束事件
    //pageAnimationEnd: Mini2.emptyFn

    //onLoad: Mini2.emotyFn

    //url: null,
    //exParam: null,
    
    /**
     * 当前页面元素
     */
    el: null,
    
    /**
     * 页面内容
     */
    contentEl:null,

    //触发返回的事件
    pageBacking: Mini2.emptyFn,

    //页面恢复的事件
    pageRevert : Mini2.emptyFn,

    /**
     * 页面刷新的函数
     *
     */
    pageRefresh: Mini2.emptyFn,



    isPreLoaded : false,
    
    onPreLoad : function(){
        var me = this;

        if (!me.isPreLoaded) {
            me.isPreLoaded = true;

            me.onInitInfinterScroll();

            if (me.onLoad) {

                try{
                    me.onLoad();
                }
                catch (ex) {
                    console.error('初始化 onLoad  错误', ex);
                }
            }
        }
    },

    /**
     * 初始化无限滚动事件
     */
    onInitInfinterScroll: function(){
        var me = this,
            el = me.el,
            infiniteScroll;

        // 初始化下拉滚动条
        infiniteScroll = Mini2.create('Mini2.ui.InfiniteScroll', {
            el: el,
            ownerPage : me
        });

        me.infiniteScroll = infiniteScroll;

        try {
            console.debug('执行 infinterScroll.onInit()');
            infiniteScroll.onInit();
        }
        catch (ex) {
            console.error('初始化无限滚动错误.', ex);
        }
    },

    //能显示不
    canShow: function () {
        return true;
    }
    

});