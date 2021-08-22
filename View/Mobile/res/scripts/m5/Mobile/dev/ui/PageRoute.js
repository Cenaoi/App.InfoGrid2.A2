
//路由
Mini2.define('Mini2.ui.PageRoute', {
    
    cache : null,
    
    pageCur : null, // 当前页

    pageGroupEl : null,

    currentUrl: null,


    firstUrl: null,

    isCur: true,
    
    routerConfig : {
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


    DIRECTION : {
        leftToRight: 'from-left-to-right',
        rightToLeft: 'from-right-to-left'
    },


    identityPageId: 0,

    newPageId:function(){
        var me = this;
        me.identityPageId++;

        return me.identityPageId;
    },
    
    Util : Mini2.Uri,

    constructor:function(){
        "use strict";
        var me = this;


        me.cache = {};

        me.onInit();
                
    },


    //执行每个 Page 的脚本
    execMainScript: function (pageEl, pageId, scriptCode) {
        "use strict";
        var me = this;
        
        if (scriptCode) {

            var uid = 'page_uid_' + pageId;

            var fullname = 'Mini2._tmp_page_define.' + uid;

            var funMain = eval("(" + scriptCode + ")");
                        
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

                el: pageEl

            });

            page.onInit();


            return page;
        }
        else {
            var scriptMain = $(pageEl).children('script[data-main]')

            var uid = 'page_uid_' + pageId;

            var fullname = 'Mini2._tmp_page_define.' + uid

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

                el: pageEl

            });

            page.onInit();

            scriptMain.remove();

            return page;
        }
    },


    onInit: function () {
        "use strict";
        var me = this,
            log = Mini2.Logger,
            Util = me.Util,
            pageCur,
            currentUrl = me.currentUrl = me.firstUrl = location.href,
            urlAsKey = Util.toUrlObject(currentUrl).base;

        me.pageGroupEl = $('.page-group');

        //当前页
        pageCur = me.pageGroupEl.children('.page-group .page-current');

        me.pageCur = pageCur;
        

        var curPageId = Mini2.newId();

        me.scroller(pageCur);

        var pageJs = me.execMainScript(pageCur, curPageId);

        
        var hist = window.history;

        if (null === hist.state) {
            var curState = {
                id: curPageId,
                url: Util.toUrlObject(currentUrl),
                pageId: curPageId
            };

            hist.replaceState(curState, '', currentUrl);
        }

        log.debug("历史数量: " + hist.length);

        me.cache[urlAsKey] = {
            lastTime: new Date(), //最后使用时间
            docEl: $(window),
            contentEl: $('body'),
            pageEl: pageCur,
            pageJs: pageJs
        };
        


        //后退事件
        window.addEventListener("popstate", function (event) {
            
            var state = event.state;

            if (null === state ) {
                return;
            }

            if (state.url.base == me.currentUrl) {
                return;
            }


            me.doSwitchDocument(state.url.base, false, me.DIRECTION.leftToRight);
        });


        me.initBackEvent();
    },

    /**
     * 判断一个链接是否使用 router 来处理
     *
     * @param $link
     * @returns {boolean}
     */
    isInRouterBlackList: function (linkEl) {

        "use strict";
        var classBlackList = [
            'external',
            'tab-link',
            'open-popup',
            'close-popup',
            'open-panel',
            'close-panel'
        ];

        for (var i = classBlackList.length - 1 ; i >= 0; i--) {
            if (linkEl.hasClass(classBlackList[i])) {
                return true;
            }
        }

        var linkEle = linkEl.get(0);
        var linkHref = linkEle.getAttribute('href');

        var protoWhiteList = [
            'http',
            'https'
        ];

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


    //a 连接 back 事件
    initBackEvent: function (e) {
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

                me.load(url, ignoreCache);
            }
        });

    },

    back:function(){
        var me = this,
            hist = window.history;

        hist.back();


    },

    //加载页面
    // ignoreCache =  忽略缓冲
    load: function (pageUrl, ignoreCache) {
        "use strict";
        var me = this,
            Util = me.Util,
            cacheDocument,
            context;

        ignoreCache = !!ignoreCache;

        var baseUrl = Util.toUrlObject(pageUrl).base;


        if (ignoreCache) {
            delete me.cache[baseUrl];
        }


        cacheDocument = me.cache[baseUrl];

        //是否为弹出框，还是普通框
        if (cacheDocument) {
            if (cacheDocument.isPopup) {

                me.openPopup(baseUrl);

            }
            else {
                me.doSwitchDocument(baseUrl, true, me.DIRECTION.rightToLeft);
            }
        }
        else {
            
            me.loadDocument(baseUrl);
        }
        

        me.isCur = false;
    },


    scroller: function (pageEl) {
        "use strict";
        var me = this,
            contentEl = $(pageEl).find('.content');

        if (contentEl.hasClass('pull-to-refresh-content')) {
                       

            var ht = new Hammer(contentEl[0]);

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


    saveDocumentIntoCache: function (doc, url) {
        "use strict";
        var me = this,
            Util = me.Util,
            urlAsKey = Util.toUrlObject(url).base,
            docEl = $(doc),
            contentEl = docEl.find('.page-group'),
            pageEl = contentEl.children('.page'),
            popupEl = contentEl.children('.popup'),
            pageJsHtml;

        if (popupEl.length > 0) {
            pageEl = popupEl;
        }

        var scriptEl = $(pageEl).children('script[data-main]');
        pageJsHtml = scriptEl.html();

        scriptEl.remove();

        me.cache[urlAsKey] = {
            lastTime : new Date(),
            docEl: docEl,
            contentEl: contentEl,
            pageEl: pageEl,
            isPopup: (popupEl.length > 0),
            pageJs: null,
            pageJsCode: pageJsHtml
        };



    },


    //切换到文档
    switchToDocument:function(){

    },

    doSwitchDocument: function (url, isPushState, direction) {
        "use strict";
        var me = this,
            Util = me.Util,
            urlAsKey = Util.toUrlObject(url).base,
            pageGroupEl = me.pageGroupEl;
        

        var cache = me.cache[urlAsKey];

        if (!cache) {
            return;
        }

        var fromPage = me.pageCur;
        var toPage = null;

        
        if (cache.pageEl) {
            toPage = cache.pageEl;
        }
        else {
            var htmlEl = $('<div></div>').append(cache.contentEl);

            var newDoc = $($(htmlEl).html());

            var pageEls = newDoc.find('.page');
            
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

        //console.log("fromPage 数量;" + me.currentUrl );
        //console.log("  toPage 数量;" + url);


        var animPageClasses = [
            'page-from-center-to-left',
            'page-from-center-to-right',
            'page-from-right-to-center',
            'page-from-left-to-center'].join(' ');

        var classForFrom, classForTo;
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


        $(fromPage).removeClass("page-current");
        $(toPage).addClass("page-current");

        $(fromPage).removeClass(animPageClasses).addClass(classForFrom);
        $(toPage).removeClass(animPageClasses).addClass(classForTo);



        $(fromPage).on('webkitAnimationEnd', function () {
            //$(this).removeClass('page-from-center-to-left');

            $(this).removeClass(animPageClasses);


            $(this).off('webkitAnimationEnd');
            $(fromPage).remove();

        });

        $(toPage).on('webkitAnimationEnd', function () {
            //$(this).removeClass('page-from-right-to-center');
            $(this).removeClass(animPageClasses);

            $(this).off('webkitAnimationEnd');
        });


        me.pageCur = toPage;


        cache.pageJs = cache.pageJs || me.execMainScript(toPage, curPageId,cache.pageJsCode);
        

        if (isPushState) {
            var curState = {
                id: uid,
                url: Util.toUrlObject(urlAsKey),
                pageId: curPageId
            };


            history.pushState(curState, '', urlAsKey);
        }

        me.currentUrl = urlAsKey;

        return me;
    },



    //  * @param {Boolean=} isPushState 是否需要 pushState
    //  * @param {String=} direction 新文档切入的方向
    loadDocument: function (url) {
        "use strict";
        var me = this;

        $.get(url, function (data, status) {


            var docEl = $('<html></html>');

            docEl.append(data);

            var pageGroupEl = docEl.find('.page-group');

            var pageEls = $(pageGroupEl).children('.page');
            var popupEls = $(pageGroupEl).children('.popup');

            if (popupEls.size() > 0) {

                me.saveDocumentIntoCache(docEl, url);

                me.openPopup(url);
            }
            else {

                me.saveDocumentIntoCache(docEl, url);

                me.doSwitchDocument(url, true, me.DIRECTION.rightToLeft);
            }

        });

        return me;
    },


    //打开弹出窗口
    openPopup: function (url) {
        "use strict";
        var me = this,
            Util = me.Util,
            urlAsKey = Util.toUrlObject(url).base,
            pageGroupEl = me.pageGroupEl;


        var cache = me.cache[urlAsKey];

        if (!cache) {
            return;
        }
        
        var toPage;

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

        cache.pageJs = cache.pageJs || me.execMainScript(toPage, curPageId, cache.pageJsCode);

        cache.pageJs.open();

        return me;
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