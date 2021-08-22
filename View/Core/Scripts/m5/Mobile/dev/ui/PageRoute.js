
//路由
Mini2.define('Mini2.ui.PageRoute', {

    cache: null,

    pageCur: null, // 当前页

    pageGroupEl: null,

    currentUrl: null,


    firstUrl: null,

    isCur: true,


    routerConfig: {
        sectionGroupClass: 'page-group',
        // 表示是当前 page 的 class
        curPageClass: 'page-current',
        // 用来辅助切换时表示 page 是 visible 的,
        // 之所以不用 curPageClass，是因为 page-current 已被赋予了「当前 page」这一含义而不仅仅是 display: block
        // 并且，别的地方已经使用了，所以不方便做变更，故新增一个
        visiblePageClass: 'page-visible',
        // 表示是 page 的 class，注意，仅是标志 class，而不是所有的 class
        pageClass: 'page'
    },


    DIRECTION: {
        leftToRight: 'from-left-to-right',
        rightToLeft: 'from-right-to-left'
    },


    identityPageId: 0,

    newPageId: function () {
        var me = this;
        me.identityPageId++;

        return me.identityPageId;
    },

    Util: Mini2.Uri,

    constructor: function () {
        "use strict";
        var me = this;


        me.cache = {};

        me.onInit();

        me.onHandleClicks();
    },


    //执行每个 Page 的脚本
    execMainScript: function (pageEl, pageId, scriptCode, curUrl, exParam) {
        "use strict";
        var me = this;

        if (scriptCode) {

            var uid = 'page_uid_' + pageId;
            var fullname = 'Mini2._tmp_page_define.' + uid;
            var funMain;

            try {
                funMain = eval("(" + scriptCode + ")");
            }
            catch (ex) {
                console.error('执行 Page 脚本错误', ex);
                console.debug('Page 脚本', scriptCode);

                Mini2.SLog.error(curUrl, '执行 Page 脚本错误! \n error.description = ' + ex.description);
                Mini2.SLog.debug(curUrl, 'Page 脚本! ' + scriptCode);

                throw ex;
            }

            var cfg = funMain();

            if (!cfg.extend) {
                if (pageEl.hasClass('popup')) {
                    cfg.extend = 'Mini2.ui.Popup'
                }
                else {
                    cfg.extend = 'Mini2.ui.Page';
                }
            }

            Mini2.define(fullname, cfg);

            var page = Mini2.create(fullname, {
                url: curUrl,
                exParam: exParam,
                el: pageEl
            });


            console.debug("当前界面实例.", page);




            try {
                page.onPreInit();
            }
            catch (ex) {
                console.error('执行用户初始化函数错误. onPreInit() ', page.onPreInit);
                Mini2.SLog.error(page, '执行用户初始化函数错误. onPreInit() \n error.description = ' + ex.description );
                throw ex;
            }

            return page;
        }
        else {

            var uid = 'page_uid_' + pageId,
                fullname = 'Mini2._tmp_page_define.' + uid

            var scriptMain = $(pageEl).children('script[data-main]')
            var cfg = main();

            if (!cfg.extend) {
                if (pageEl.hasClass('popup')) {
                    cfg.extend = 'Mini2.ui.Popup'
                }
                else {
                    cfg.extend = 'Mini2.ui.Page';
                }
            }

            Mini2.define(fullname, cfg);

            var page = Mini2.create(fullname, {
                url: curUrl,
                exParam: exParam,
                el: pageEl
            });


            console.debug("当前界面实例.", page);

            try {
                page.onPreInit();
            }
            catch (ex) {
                console.error('执行用户初始化函数错误. onPreInit() ');
                Mini2.SLog.error(page, '执行用户初始化函数错误. onPreInit() \n error.description = ' + ex.description);
                throw ex;
            }

            scriptMain.remove();

            return page;
        }
    },


    onInit: function () {
        "use strict";
        var me = this,
            log = Mini2.Logger,
            cfg = me.routerConfig,
            Util = me.Util,
            pageCur,
            currentUrl = me.currentUrl = me.firstUrl = location.href,
            urlAsKey = Util.toUrlObject(currentUrl).base,
            curPageId, pageJs, hist, curState;

        me.pageGroupEl = $('.page-group');

        if (me.pageGroupEl.length == 0) {
            throw new Error('当前页面没有指定 .page-group 样式节点.');
        }

        //当前页
        pageCur = me.pageGroupEl.children('.page-group .page');


        if (pageCur.length == 0) {
            throw new Error('当前页面组, 没有找到 .page 样式的节点.');
        }


        me.pageCur = pageCur;
        //if (!pageCur.hasClass('page-current')) {
        //    pageCur.addClass('page-current');
        //}


        curPageId = Mini2.newId();

        me.scroller(pageCur);

        pageJs = me.execMainScript(pageCur, curPageId, null, currentUrl);

        me.pageJs = pageJs;

        hist = window.history;

        if (null === hist.state) {
            curState = {
                id: curPageId,
                url: Util.toUrlObject(currentUrl),
                pageId: curPageId
            };

            hist.replaceState(curState, '', currentUrl);
        }

        //log.debug("历史数量: " + hist.length);

        me.cache[urlAsKey] = {
            url: currentUrl,
            lastTime: new Date(), //最后使用时间
            docEl: $(window),
            contentEl: $('body'),
            pageEl: pageCur,
            pageJs: pageJs
        };

        me.allowPageShow_First(pageCur, pageJs);



        //后退事件
        window.addEventListener("popstate", function (event) {

            var state = event.state;

            if (null === state) {
                return;
            }

            if (state.url.base == me.currentUrl) {
                return;
            }

            console.debug("window 后退事件。 url=", state.url);
            Mini2.SLog.debug(pageCur, 'window 后退事件。 url=' + state.url);

            me.doSwitchDocument(state.url.base, false, me.DIRECTION.leftToRight);
        });


        me.initClickEvent();
    },


    call_onPreLoad: function (toPage, toPageJs) {
        var me = this;


        if (!toPage.hasClass('page-current')) {
            toPage.addClass('page-current');
        }


        if (toPageJs.onPreLoad) {
            try {
                console.info('执行 onPreLoad()');
                Mini2.SLog.info(toPage, '执行 onPreLoad()' );

                toPageJs.onPreLoad();
            }
            catch (ex) {
                console.error('执行 onLoad() 错误', ex);
                Mini2.SLog.error(toPage, '执行 onLoad() 错误 \n error.description=' + ex.description);
                throw ex;
            }
        }


        //执行页面刷新函数
        if (toPageJs.pageRefresh) {

            console.debug('执行页面刷新 pageRefresh()');
            Mini2.SLog.debug(toPageJs, '执行页面刷新 pageRefresh()');

            try {
                toPageJs.pageRefresh();
            }
            catch (ex) {
                console.error('执行页面刷新 pageRefresh() 错误', toPageJs.pageRefresh);
                console.error(ex);

                Mini2.SLog.error(toPageJs, '执行页面刷新 pageRefresh() 错误. \n error.description=' + ex.description);
            }
        }

    },


    /**
    * 整个页面第一加载的时候, 调用的.
    *
    */
    allowPageShow_First: function (toPage, toPageJs) {
        var me = this;

        if (toPageJs.canShow()) {

            me.call_onPreLoad(toPage, toPageJs);

            return;
        }

        setTimeout(function () {

            if (toPageJs.canShow()) {
                me.call_onPreLoad(toPage, toPageJs);
            }
            else {
                me.allowPageShow_First(toPage, toPageJs);
            }

        }, 100);

    },



    classBlackList: [
        'external',
        'tab-link',
        'open-popup',
        'close-popup',
        'open-panel',
        'close-panel'
    ],

    protoWhiteList: [
        'http',
        'https'
    ],

    /**
     * 判断一个链接是否使用 router 来处理
     *
     * @param $link
     * @returns {boolean}
     */
    isInRouterBlackList: function (linkEl) {

        "use strict";
        var me = this,
            i,
            linkEle,
            linkHref,
            protoWhiteList = me.protoWhiteList,
            classBlackList = me.classBlackList;

        for (i = classBlackList.length - 1 ; i >= 0; i--) {
            if (linkEl.hasClass(classBlackList[i])) {
                return true;
            }
        }

        linkEle = linkEl.get(0);
        linkHref = linkEle.getAttribute('href');


        //如果非noscheme形式的链接，且协议不是http(s)，那么路由不会处理这类链接
        if (/^(\w+):/.test(linkHref) && protoWhiteList.indexOf(RegExp.$1) < 0) {
            return true;
        }

        //noinspection RedundantIfStatementJS
        if (linkEle.hasAttribute('external')) {
            return true;
        }

        return false;

    },

    /**
    * 自定义是否执行路由功能的过滤器
    *
    * 可以在外部定义 $.config.routerFilter 函数，实参是点击链接的 Zepto 对象。
    *
    * @param $link 当前点击的链接的 Zepto 对象
    * @returns {boolean} 返回 true 表示执行路由功能，否则不做路由处理
    */
    customClickFilter: function (targetEl) {



        return true;
    },


    //a 连接点击事件
    initClickEvent: function (e) {
        "use strict";
        var me = this;

        $(document).on('click', 'a', function (e) {
            var targetEl = $(e.currentTarget);

            var filterResult = me.customClickFilter(targetEl);
            if (!filterResult) {
                return;
            }

            if (me.isInRouterBlackList(targetEl)) {
                return;
            }

            e.preventDefault();

            if (targetEl.hasClass('back')) {
                me.back();
            }
            else {
                var url = targetEl.attr('href');
                if (!url || url === '#') {
                    return;
                }

                var ignoreCache = targetEl.attr('data-no-cache') === 'true';

                var direction = targetEl.attr('data-direction');

                if (me.DIRECTION.leftToRight == direction) {

                }
                else if (me.DIRECTION.rightToLeft == direction) {

                }
                else {
                    direction = null;
                }

                me.load(url, ignoreCache, null, direction);
            }
        });

    },

    /**
     * 退回事件
     */
    back: function () {
        var me = this,
            pageCur = me.pageCur,
            pageJs = me.pageJs,
            allowNext,
            hist = window.history;

        try {
            console.debug('准备执行 pageBacking().');
            console.debug('函数 ', pageJs.pageBacking);

            Mini2.SLog.debug(pageCur, '准备执行 pageBacking().');

            allowNext = pageJs.pageBacking();
        }
        catch (ex) {
            console.error('执行页面 pageBacking 事件错误.' + ex.message, pageJs.pageBacking);
            Mini2.SLog.error(pageCur, '执行页面 pageBacking 事件错误. \n error.description=' + ex.description);

            return;
        }

        if (false === allowNext) {
            return;
        }

        hist.back();

    },

    //加载页面
    // ignoreCache =  忽略缓冲
    // exParam = 扩展属性
    // direction = 方向, DIRECTION
    // openedCallback 打开的回调函数
    // closedCallback 关闭的回调函数
    load: function (pageUrl, ignoreCache, exParam, direction, openedCallback, closedCallback) {
        "use strict";
        var me = this,
            Util = me.Util,
            cacheDocument,
            context,
            baseUrl;

        if (!pageUrl) {
            throw new Error("pageUrl 跳转地址的参数能为空.");
        }


        if (!Mini2.isString(pageUrl)) {
            var p = pageUrl;

            pageUrl = p.url || p.pageUrl;
            //ignoreCache = p.ignoreCache || false;
            //exParam = p.exParam || null;
            //direction = p.direction || null;
            openedCallback = p.opened;
            closedCallback = p.closed;

            console.debug('加载参数: ', p);
        }


        console.debug("加载页面 pageUrl=", pageUrl);
        Mini2.SLog.debug(pageUrl, "加载页面 pageUrl=", pageUrl);


        ignoreCache = !!ignoreCache;

        baseUrl = Util.toUrlObject(pageUrl).base;


        if (ignoreCache) {
            delete me.cache[baseUrl];
        }


        cacheDocument = me.cache[baseUrl];

        direction = direction || me.DIRECTION.rightToLeft;  //方向

        //是否为弹出框，还是普通框
        if (cacheDocument) {
            if (cacheDocument.isPopup) {

                me.openPopup(baseUrl, exParam, openedCallback, closedCallback);

            }
            else {
                console.debug("缓冲弹出 cacheDocument, url=", baseUrl);

                console.debug("direction 方向:", direction);
                
                Mini2.SLog.debug(pageUrl, "缓冲弹出 cacheDocument, url=", baseUrl);

                me.doSwitchDocument(baseUrl, true, direction, openedCallback, closedCallback);
            }
        }
        else {
            me.loadDocument(baseUrl, exParam, direction, ignoreCache, openedCallback, closedCallback);
        }


        me.isCur = false;
    },


    scroller: function (pageEl) {
        "use strict";
        var me = this,
            contentEl = $(pageEl).find('.content'),
            ht;


        //允许下来的事件 
        //样式 pull-to-refresh-content
        if (contentEl.hasClass('pull-to-refresh-content')) {


            ht = new Hammer(contentEl[0]);

            ht.get('swipe').set({ direction: Hammer.DIRECTION_VERTICAL });

            //添加事件
            ht.on("pan", function (e) {

                var targetY = e.deltaY;
                $(contentEl).css('-webkit-transform', 'translate3d(0, ' + targetY + 'px, 0)');

            });

            ht.on('panstart', function (e) {

            });

            ht.on('panend', function (e) {

                $(contentEl).animate({ translate3d: '0, 0px,0' }, 'fast', 'ease-out');

            });
        }

    },

    // exParam = 扩展属性
    saveDocumentIntoCache: function (doc, url, exParam) {
        "use strict";
        var me = this,
            Util = me.Util,
            urlAsKey = Util.toUrlObject(url).base,
            docEl = $(doc),
            contentEl = docEl.find('.page-group'),
            pageEl = contentEl.children('.page'),
            popupEl = contentEl.children('.popup'),
            pageJsHtml,
            scriptEl;

        if (popupEl.length > 0) {
            pageEl = popupEl;
        }

        console.debug("exParam: saveDocumentIntoCache()", exParam);

        scriptEl = $(pageEl).children('script[data-main]');
        pageJsHtml = scriptEl.html();

        scriptEl.remove();

        me.cache[urlAsKey] = {
            url: url,
            exParam: exParam,

            lastTime: new Date(),
            docEl: docEl,
            contentEl: contentEl,
            pageEl: pageEl,
            isPopup: (popupEl.length > 0),
            pageJs: null,
            pageJsCode: pageJsHtml
        };



    },


    //切换到文档
    switchToDocument: function () {

    },

    //动画元素
    animateElement: function (fromEl, toEl, direction) {
        var me = this,
            fromEl = $(fromEl),
            toEl = $(toEl),
            classForFrom, classForTo;


        var animPageClasses = [
            'page-from-center-to-left',
            'page-from-center-to-right',
            'page-from-right-to-center',
            'page-from-left-to-center'].join(' ');


        switch (direction) {
            case me.DIRECTION.rightToLeft:
                classForFrom = 'page-from-center-to-left';
                classForTo = 'page-from-right-to-center';
                break;
            case me.DIRECTION.leftToRight:
                classForFrom = 'page-from-center-to-right';
                classForTo = 'page-from-left-to-center';
                break;
            default:
                classForFrom = 'page-from-center-to-left';
                classForTo = 'page-from-right-to-center';
                break;
        }

        fromEl.removeClass(animPageClasses).addClass(classForFrom);
        toEl.removeClass(animPageClasses).addClass(classForTo);



        fromEl.animationEnd(function () {
            //$(this).removeClass('page-from-center-to-left');

            fromEl.off('webkitAnimationEnd');
            fromEl.removeClass(animPageClasses)
            fromEl.removeClass("page-current");

            fromEl.remove();

        });

        toEl.animationEnd(function () {

            toEl.off('webkitAnimationEnd');
            toEl.removeClass(animPageClasses);
            toEl.addClass("page-current");

        });

    },

    //exParam = 扩展属性
    doSwitchDocument: function (url, isPushState, direction) {
        "use strict";
        var me = this,
            Util = me.Util,
            urlAsKey = Util.toUrlObject(url).base,
            toPageJs,
            pageGroupEl = me.pageGroupEl,
            cache, fromPage, toPage;


        console.debug("doSwitchDocument(...)  url=", url);

        cache = me.cache[urlAsKey];

        if (!cache) {
            console.debug("doSwitchDocument(...) cache 不存在");
            return;
        }



        fromPage = me.pageCur;
        toPage = null;

        console.debug("cache.pageEl = ", cache.pageEl);

        if (cache.pageEl) {
            toPage = cache.pageEl;
        }
        else {
            var htmlEl, newDoc, pageEls;

            htmlEl = $('<div></div>').append(cache.contentEl);
            newDoc = $($(htmlEl).html());
            pageEls = newDoc.find('.page');

            cache.pageEl = pageEls;

            toPage = pageEls;
        }



        var uid, curPageId;

        //==============处理脚本===================
        if (!cache.pageJs) {

            curPageId = Mini2.newId();
            uid = 'uid-' + curPageId;
            toPage.addClass(uid);

            $(toPage).attr("data-uid", uid);
        }
        //==============================================

        pageGroupEl.append(toPage);

        console.debug("exParam: doSwitchDocument()  cache.exParam=", cache.exParam);

        cache.pageJs = cache.pageJs || me.execMainScript(toPage, curPageId, cache.pageJsCode, cache.url, cache.exParam);

        if (cache.pageJsCode) {
            delete cache.pageJsCode;
        }

        toPageJs = cache.pageJs;


        me.pageCur = toPage;
        me.pageJs = cache.pageJs;

        if (isPushState) {

            var curState = {
                exParam: cache.exParam,
                id: uid,
                url: Util.toUrlObject(urlAsKey),
                pageId: curPageId
            };

            history.pushState(curState, '', urlAsKey);
        }

        me.currentUrl = urlAsKey;

        me.allowPageShow(fromPage, toPage, toPageJs, direction);

        if (!isPushState) {

            //恢复函数
            if (toPageJs.pageRevert) {

                console.debug("执行恢复函数 pageRevert().")

                Mini2.SLog.debug(toPageJs, "执行恢复函数 pageRevert()");

                try {
                    toPageJs.pageRevert();
                }
                catch (ex) {
                    console.error("执行恢复函数 pageRevert(). 错误", toPageJs.pageRevert);
                    console.error(ex);


                    Mini2.SLog.error(toPageJs, "执行恢复函数 pageRevert(). 错误 \n + " + ex.description);
                }
            }
        }



        //执行页面刷新函数
        if (toPageJs.pageRefresh) {

            console.debug('执行页面刷新 pageRefresh()');

            Mini2.SLog.debug(toPageJs, "执行页面刷新 pageRefresh()");

            try {
                toPageJs.pageRefresh();
            }
            catch (ex) {
                console.error('执行页面刷新 pageRefresh() 错误', toPageJs.pageRefresh);
                console.error(ex);

                Mini2.SLog.error(toPageJs, "执行页面刷新 pageRefresh(). 错误 \n + " + ex.description);
            }
        }

        return me;
    },


    call_pageLoad: function (fromPage, toPage, toPageJs, direction) {

        var me = this;

        me.pageAnimationStart(fromPage, toPage, toPageJs, direction);

        if (toPageJs.onPreLoad) {
            try {
                console.info('执行 onPreLoad()');
                toPageJs.onPreLoad();
            }
            catch (ex) {
                console.error('执行 onPreLoad() 错误', ex);
                Mini2.SLog.error(toPageJs, "执行 onPreLoad(). 错误 \n + " + ex.description);
                throw ex;
            }
        }

    },


    /**
     * 允许页面显示出来？
     * @param {Object} fromPage 起始的页面
     * @param {object} toPage 结束页面
     * @param {object} toPageJs 结束页面的 js 脚本
     * @param {string} direction 进入方向 
     */
    allowPageShow: function (fromPage, toPage, toPageJs, direction) {
        var me = this;

        console.debug("allowPageShow() ", {
            fromPage: fromPage,
            toPage: toPage
        });


        if (toPageJs.canShow()) {

            me.call_pageLoad(fromPage, toPage, toPageJs, direction);

            return;
        }

        setTimeout(function () {

            if (toPageJs.canShow()) {
                me.call_pageLoad(fromPage, toPage, toPageJs, direction);
            }
            else {
                console.debug('等待中...');
                me.allowPageShow(fromPage, toPage, toPageJs, direction);
            }

        }, 100);

    },


    pageAnimationStart: function (fromPage, toPage, toPageJs, direction) {
        var me = this;

        toPageJs.on('pageAnimationStart');  //触发动画开始事件


        me.animateElement(fromPage, toPage, direction);

        $(fromPage).animationEnd(function () {
            $(this).removeClass("page-current");
        });

        $(toPage).animationEnd(function () {

            $(this).addClass("page-current");

            toPageJs.on('pageAnimationEnd');    //触发动画结束事件
        });
    },



    //解析页面代码
    // exParam = 扩展属性
    parsePage: function (data, url, exParam, direction, openedCallback, closedCallback) {

        var me = this;


        console.debug("exParam: parsePage()", exParam);

        var docEl = $('<html></html>');

        docEl.append(data);

        var pageGroupEl = docEl.find('.page-group');

        var pageEls = pageGroupEl.children('.page');
        var popupEls = pageGroupEl.children('.popup');

        if (popupEls.size() > 0) {

            try {
                me.saveDocumentIntoCache(docEl, url, exParam);
            }
            catch (ex) {
                throw new Error("saveDocumentIntoCache(...) 执行异常.503");
            }

            me.openPopup(url, exParam, openedCallback, closedCallback);
        }
        else {

            direction = direction || me.DIRECTION.rightToLeft;

            try {
                me.saveDocumentIntoCache(docEl, url, exParam);
            }
            catch (ex) {
                throw new Error("saveDocumentIntoCache(...) 执行异常.504");
            }

            console.debug("parsePage(...) 解析页面 html 后弹出. url=", url);

            me.doSwitchDocument(url, true, me.DIRECTION.rightToLeft);
        }

    },


    //加载的时间
    loadTime: false,

    loadFunNum: 0, //加载的数量

    preloader: false,

    //提示加载框
    showToast: function () {

        var me = this;

        if (me.loadFunNum == 0) {
            me.loadTime = new Date();
        }

        me.showToastX2();

    },

    showToastX2: function () {
        var me = this;

        console.log("显示加载器.");

        setTimeout(function () {

            var now = new Date(),
                preloader = me.preloader;

            var span = now - me.loadTime;

            if (me.loadFunNum > 0) {

                if (span > 500 && !preloader) {
                    me.preloader = $.preloader();
                }

                me.showToastX2();
            }
            else {
                if (preloader) {
                    preloader.close();
                    me.preloader = false;
                }
            }

            console.log("加载器提示...loadFunNum ", me.loadFunNum);

        }, 100);

    },


    //  * @param {Boolean=} isPushState 是否需要 pushState
    //  * @param {String=} direction 新文档切入的方向
    //  * @param {Object=} exParam 用户扩展数据用 
    loadDocument: function (url, exParam, direction, ignoreCache, openedCallback, closedCallback) {
        "use strict";
        var me = this,
            rumUrl = url;

        me.showToast();

        me.loadFunNum++;
        console.debug("ajax: loadDocument(...) 加载前...loadFunNum=", me.loadFunNum);

        if (ignoreCache) {

            if (rumUrl.lastIndexOf('?') == -1) {
                rumUrl += '?';
            }

            rumUrl += '&' + Mini2.Guid.newGuid();
        }


        var xhr = $.ajax({
            url: rumUrl,
            success: function (data, status, xhr) {

                console.debug("exParam: loadDocument()", exParam);

                try {
                    me.parsePage.call(me, data, url, exParam, direction, openedCallback, closedCallback);
                }
                catch (ex) {
                    console.error("加载失败!", ex);
                    $.toast('加载失败...501\n' + url + "\n" + ex.message);
                }
            },
            error: function (xhr, status, err) {
                console.error("加载失败...", url);
                $.toast('加载失败...502');

            },
            complete: function () {
                me.loadFunNum--;
                console.debug("ajax: loadDocument(...) 请求完成, loadFunNum = ", me.loadFunNum);

                if (me.loadFunNum < 0) {
                    console.error('me.loadFunNum < 0 ', me.loadFunNum);
                    me.loadFunNum = 0;
                }
            }
        });

        return me;
    },


    //打开弹出窗口
    openPopup: function (url, exParam, openedCallback, closedCallback) {
        "use strict";
        var me = this,
            Util = me.Util,
            urlAsKey = Util.toUrlObject(url).base,
            pageGroupEl = me.pageGroupEl,
            cache, toPage;


        cache = me.cache[urlAsKey];

        if (!cache) {
            return;
        }

        if (cache.pageEl) {
            toPage = cache.pageEl;
        }
        else {
            var htmlEl = $('<div></div>').append(cache.contentEl);

            var newDoc = $($(htmlEl).html());

            var pageEls = newDoc.find('.popup');

            cache.pageEl = pageEls;

            toPage = cache.pageEl;
        }

        var uid, curPageId;

        //==============处理脚本===================
        if (!cache.pageJs) {

            curPageId = Mini2.newId();
            uid = 'uid-' + curPageId;
            toPage.addClass(uid);

            $(toPage).attr("data-uid", uid);
        }
        //==============================================

        pageGroupEl.append(toPage);

        cache.pageJs = cache.pageJs || me.execMainScript(toPage, curPageId, cache.pageJsCode, cache.url, cache.exParam);

        //调用打开的回调函数
        if (openedCallback) {
            cache.pageJs.bind('opened', openedCallback);
        }

        if (closedCallback) {
            cache.pageJs.bind('closed', closedCallback);
        }

        cache.pageJs.open();

        if (cache.pageJs.onPreLoad) {
            try {
                console.info('执行 onPreLoad()');
                cache.pageJs.onPreLoad();
            }
            catch (ex) {
                console.error('执行 onPreLoad() 错误', ex);
                Mini2.SLog.error(cache.pageJs, "执行 onPreLoad(). 错误 \n + " + ex.description);
                throw ex;
            }
        }


        //执行页面刷新函数
        if (cache.pageJs.pageRefresh) {

            console.debug('执行页面刷新 pageRefresh()');

            try {
                cache.pageJs.pageRefresh();
            }
            catch (ex) {
                console.error('执行页面刷新 pageRefresh() 错误', cache.pageJs.pageRefresh);
                Mini2.SLog.error(cache.pageJs, "执行 pageRefresh(). 错误 \n + " + ex.description);
                console.error(ex);
            }
        }



        return me;
    },


    closePanel: function () {
        "use strict";
        var me = this,
            body = $(document.body);

        var hasLER = body.hasClass('with-panel-left-reveal');

        if (hasLER) {

            var pageEl = $('.page-group').children('.active');

            setTimeout(function () {
                body.removeClass("panel-closing");

                pageEl.removeClass('active');
                pageEl.css('display', 'none');

            }, 500);

            body.removeClass('with-panel-left-reveal').addClass('panel-closing');


        }

    },


    /**
    * 创建 page 的实例化类
    *
    *
    */
    createPanelCode: function (panelEl) {
        "use strict";
        var me = this;

        var mainFnName = panelEl.attr('data-main-fn');
        var mainFn = window[mainFnName];

        if (mainFn) {

            var pageId = Mini2.newId(),
                uid = 'page_uid_' + pageId,
                fullname = 'Mini2._tmp_page_define.' + uid;

            var cfg = mainFn();
            var pageEl = panelEl;

            if (!cfg.extend) {
                if (pageEl.hasClass('popup')) {
                    cfg.extend = 'Mini2.ui.Popup'
                }
                else {
                    cfg.extend = 'Mini2.ui.Page';
                }
            }


            Mini2.define(fullname, cfg);

            var page = Mini2.create(fullname, {
                //url: curUrl,
                //exParam: exParam,
                el: pageEl
            });

            console.debug("当前界面实例.", page);

            try {
                page.onPreInit();
            }
            catch (ex) {
                console.error('执行用户初始化函数错误. onPreInit() ', page.onPreInit);
                Mini2.SLog.error(page, "执行用户初始化函数错误. onPreInit(). 错误 \n + " + ex.description);
                throw ex;
            }

            panelEl.attr('data-instance', 'true');

        }
    },

    /**
    * 打开左右侧的页面
    * @param {String} panelId 页面元素 id
    */
    openPanel: function (panelId) {
        "use strict";
        var me = this,
            body = $(document.body),
            panelEl;

        panelEl = $(panelId);


        if (panelEl.attr('data-instance') != 'true') {
            me.createPanelCode(panelEl);
        }

        panelEl.addClass('active');
        panelEl.css({ 'display': 'block' });

        body.addClass('with-panel-left-reveal');

    },

    onHandleClicks: function () {
        "use strict";
        var me = this;

        //注: 下面这个 click 函数针对 iphone 做的特殊处理
        $('.panel-overlay').click(function () {
            if (!$(this).hasClass('event-ignore')) {
                me.closePanel();
            }
        });



        $(document).on("click", ".close-panel", function (e) {
            if (!$(this).hasClass('event-ignore')) {
                me.closePanel();
            }
        });

        $(document).on('click', '.open-panel', function (e) {

            if (!$(this).hasClass('event-ignore')) {
                var panelId = $(this).attr('data-panel');

                me.openPanel(panelId);
            }

        });

        //$(document).on('click', '.modal-overlay, .popup-overlay, .close-popup, .open-popup, .close-picker', function (e) {

        //    console.log("xxxxxxxxxxxxxxxx");

        //});
    }


}, function (args) {
    "use strict";
    var me = this,
        newArgs = [],
        srcArgs = arguments,
        i = 0;

    for (; i < srcArgs.length; i++) {
        newArgs[i] = srcArgs[i];
    }

    me.muid = Mini2.newId();

    me.constructor(newArgs[0], newArgs[1]);

});
