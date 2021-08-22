/// <reference path="../Mini.js" />

Mini2.define('Mini2.ui.Pagination', {

    //store: undefined,


    renderTo: Mini2.getBody(),

    urlFormat: '?page={0}',

    prevText: '上一页',
    nextText: '下一页',

    firstText: '首页',
    lastText: '尾页',


    curPage: 0,

    itemTotal: 0,
    rowCount: 20,

    pageCount: 0,

    startPage: 0,
    endPage: 0,


    /**
     * ui 类型, default=带分页码, more=更多, backAndNext=上一页, 下一页
     */
    uiType: 'default',


    /**
    * 隐藏行数选择框
    */
    hideRowCountSelect: false,

    /**
    el:null,
    **/

    visible: true,

    constructor: function (options) {
        "use strict";
        /// <summary>初始化</summary>
        var me = this;

        if (Mini2.isString(me.store)) {

            var store = Mini2.data.StoreManager.lookup(me.store);

            me.store = store;
        }

        me.bindStore(me.store);

    },

    bindStore: function (store) {
        "use strict";
        /// <summary>绑定数据仓库</summary>
        var me = this;

        if (!store) { return; }

        me.itemTotal = store.getTotalCount();
        me.renderButton();

        $(store).muBind('datachanged', me, me.store_dataChanged);
        $(store).muBind('load', me, me.store_load);
    },

    store_load: function (event, store) {
        "use strict";
        var me = event.data;

        me.itemTotal = store.getTotalCount();
        me.rowCount = store.pageSize;

        me.curPage = store.currentPage;

        me.renderButton();

    },

    store_dataChanged: function (event, store) {
        "use strict";
        var me = event.data;

        me.itemTotal = store.getTotalCount();
        me.rowCount = store.pageSize;

        me.curPage = store.currentPage;

        me.renderButton();

    },

    click: function (page, store) {

        var me = this;

        if (store) {
            store.loadPage(page);
        }
    },


    getFirstUrl: function () {
        "use strict";
        /// <summary>获取"首页"的超链接</summary>
        var me = this;
        return $.format(me.urlFormat, 0);
    },

    getPrevUrl: function () {
        "use strict";
        /// <summary>获取"上一页"的链接</summary>
        var me = this,
            p = me.curPage - 1;

        if (p < 0) { p = 0; }

        return $.format(me.urlFormat, p);
    },

    getNextUrl: function () {
        "use strict";
        /// <summary>获取"下一页"的链接</summary>
        var me = this,
            p = me.curPage + 1;

        if (p >= me.pageCount) {
            p = me.pageCount - 1;
        }

        return $.format(me.urlFormat, p);
    },


    /**
     * 获取"尾页"的链接
     */
    getLastUrl: function () {
        "use strict";
        var me = this;
        return $.format(me.urlFormat, me.pageCount - 1);
    },

    /**
     * 重新计算参数
     */
    resetParam: function () {
        "use strict";

        var me = this,
            curPage = me.curPage,
            itemTotal = me.itemTotal || 0,
            rowCount = me.rowCount,
            pageCount,
            startPage,
            endPage,
            startRow,
            endRow;

        var m = itemTotal % rowCount;

        pageCount = (itemTotal - m) / rowCount;

        if (m > 0) {
            pageCount++;
        }

        startPage = curPage - 4;
        endPage = curPage + 5;

        if (startPage < 0) {
            startPage = 0;

            //            m_EndPageIndex += 4 - m_Params.curPage;
        }

        if (endPage >= pageCount) {
            endPage = pageCount - 1;
        }


        startRow = curPage * rowCount;
        endRow = startRow + rowCount;

        if (endRow > itemTotal) {
            endRow = itemTotal;
        }

        me.pageCount = pageCount;
        me.startPage = startPage;
        me.endPage = endPage;

        me.startRow = startRow;
        me.endRow = endRow;
    },

    render: function () {
        "use strict";
        var me = this,
            el,
            uiType = me.uiType;

        me.visible = !!me.visible;

        if ('default' == uiType) {

            me.el = el = $('<div class="flickr" style="border-color: silver;border-width: 1px;"></div>');


            if (me.applyTo) {
                $(me.applyTo).append(el);
            }
            else {
                $(me.renderTo).append(el);
            }
                      

            if (!me.visible) {
                el.hide();
            }

            me.renderButton();
        }
        else if ('more' == uiType) {
            me.el = el = Mini2.$join([
                '<nav>',
                '<ul class="pager" style="margin:4px;">',
                '<li><a href="#">加载更多</a></li>',
                '</ul>',
                '</nav>']);


            if (me.applyTo) {
                $(me.applyTo).append(el);
            }
            else {
                $(me.renderTo).append(el);
            }
            if (!me.visible) {
                el.hide();
            }
        }
        else if ('backAndNext' == uiType) {

        }

    },


    /**
    * 渲染页数的选择框
    */
    renderRowCountSelectBox: function () {
        "use strict";
        var me = this,
            el = $(me.el),
            rowCount = me.rowCount;

        var pageSizeTB = Mini2.$joinStr([
            '<span style="float:left;position: relative;top: -6px;">',
            '<span>每页显示 </span>',
            '<select style="padding: 0px 2px 0px 2px;  border-bottom-style: none;" class="mi-form-field mi-form-text">',
            '<option>20</option>',
            '<option>50</option>',
            '<option>100</option>',
            '</select>',
            '<span> 行</span>',
            '</span>']);

        //每页显示多少的下拉框
        var selectEl = pageSizeTB.children('.mi-form-field');

        //显示多少条记录
        selectEl.val(me.rowCount);

        //改变事件
        selectEl.change(function () {
            me.rowCount = parseInt($(this).val());
            me.store.pageSize = me.rowCount;
            me.click.call(this, 0, me.store);
            return false;
        });


        el.append(pageSizeTB);
    },

    renderButton: function () {
        "use strict";
        /// <summary>重新设置</summary>


        var me = this,
            i;

        me.resetParam();

        var curPage = me.curPage,
            itemTotal = me.itemTotal || 0,
            rowCount = me.rowCount,
            pageCount = me.pageCount,
            startPage = me.startPage,
            endPage = me.endPage,
            el = $(me.el),
            command = 'loadPage';

        //log.debug("itemTotal = " + itemTotal);

        //重新构造分页的所有按钮
        el.html("");

        var btnFormat = "<a href='{0}' command='{3}' commandParam='{2}' page='{2}' class='button' valid='false'>{1}</a>";

        if (!me.hideRowCountSelect) {
            me.renderRowCountSelectBox();   //渲染行数选择框
        }

        if (curPage > 0) {
            el.append($.format(btnFormat, me.getFirstUrl(), me.firstText, 0, command));
        }
        else {
            el.append($.format("<span class='disabled'>{0}</span>", me.firstText));
        }

        if (curPage > 0) {
            el.append($.format(btnFormat, me.getPrevUrl(), me.prevText, curPage - 1, command));
        }
        else {
            el.append($.format("<span class='disabled'>{0}</span>", me.prevText));
        }


        for (i = startPage; i <= endPage; i++) {

            var url = $.format(me.urlFormat, i);
            if (i == curPage) {
                el.append($.format("<span class='current'>{0}</span>", i + 1));
            }
            else {
                el.append($.format(btnFormat, url, i + 1, i, command));
            }
        }


        if (curPage < pageCount - 1) {
            el.append($.format(btnFormat, me.getNextUrl(), me.nextText, curPage + 1, command));
            el.append($.format(btnFormat, me.getLastUrl(), me.lastText, pageCount - 1, command));
        }
        else {
            el.append($.format("<span class='disabled'>{0}</span>", me.nextText));
            el.append($.format("<span class='disabled'>{0}</span>", me.lastText));
        }

        //给各个按钮加上事件
        if (me.click) {
            var items = el.children("a.button");
            $(items).click(function () {
                var page = parseInt($(this).attr('page'));
                me.click.call(this, page, me.store);
                return false;
            });
        }

        el.append($.format("当前记录 {0}--{1} 条,共 {2} 条记录", me.startRow + 1, me.endRow, itemTotal));
    },

    show: function () {
        var me = this,
            el = me.el;

        el.show();
    },

    hide: function () {
        var me = this,
            el = me.el;

        el.hide();
    }
},
    function () {
        var me = this;

        Mini2.apply(this, arguments[0]);

        me.constructor();
    });