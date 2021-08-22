
Mini.ui.Pagination = function (options) {
    /// <summary>分页组件</summary>

    var defaults = {
        id: '',
        itemTotal: 500,
        urlFormat: "pageIndex={0}",
        rowCount: 10,
        buttonCount: 10,
        curPage: 1,
        pageCount: 0,
        command: '',
        buttonText: { first: "First", prev: "Prev", next: "Next", last: "Last" }
    };

    var m_Params = {
        id: '',
        itemTotal: 500,
        urlFormat: "pageIndex_{0}",
        rowCount: 10,
        buttonCount: 10,
        curPage: 0,
        pageCount: 0,
        command: '',
        buttonText: { first: "First", prev: "Prev", next: "Next", last: "Last" }
    };

    var m_Box;

    var m_StartPageIndex = 0;
    var m_EndPageIndex = 0;

    var m_ClickEvent;

    this.click = function (c) {
        var items = m_Box.children("a");

        $(items).click(c);

        m_ClickEvent = c;

    };

    function init(options) {
        /// <summary>初始化</summary>

        m_Params = $.extend(defaults, options);

        m_Box = $("#" + m_Params.id);

        if (m_Box.length == 0) {
            alert("Pagination.js \nfunction init(options) 初始化错误,不存在 html 对象'" + m_Params.id + "'");
        }
    }

    function getFirstUrl() {
        /// <summary>获取"首页"的超链接</summary>

        return $.format(m_Params.urlFormat, "0");
    }

    function getPrevUrl() {
        /// <summary>获取"上一页"的链接</summary>
        var p = m_Params.curPage - 1;

        if (p < 0) { p = 0; }

        return $.format(m_Params.urlFormat, p);
    }

    function getNextUrl() {
        /// <summary>获取"下一页"的链接</summary>

        var p = m_Params.curPage + 1;

        if (p >= m_Params.pageCount) {
            p = m_Params.pageCount - 1;
        }

        return $.format(m_Params.urlFormat, p);
    }

    function getLastUrl() {
        /// <summary>获取"尾页"的链接</summary>

        return $.format(m_Params.urlFormat, m_Params.pageCount - 1);
    }

    this.reset = function (options) {
        /// <summary>重新设置</summary>

        for (var i in options) {
            if (m_Params[i] != undefined) {
                m_Params[i] = options[i];
            }
        }


        var inputBox = $("#" + m_Params.id + "_CurPIndex");
        $(inputBox).val(m_Params.curPage);

        var m = m_Params.itemTotal % m_Params.rowCount;

        m_Params.pageCount = (m_Params.itemTotal - m) / m_Params.rowCount;

        if (m > 0) {
            m_Params.pageCount++;
        }

        m_StartPageIndex = m_Params.curPage - 4;
        m_EndPageIndex = m_Params.curPage + 5;

        if (m_StartPageIndex < 0) {
            m_StartPageIndex = 0;

            //            m_EndPageIndex += 4 - m_Params.curPage;
        }

        if (m_EndPageIndex >= m_Params.pageCount) {
            m_EndPageIndex = m_Params.pageCount - 1;
        }

        //重新构造分页的所有按钮
        m_Box.html("");

        var btnFormat = "<a href='{0}' command='{3}' commandParam='{2}' valid='false'>{1}</a>";

        var btnTxt = m_Params.buttonText;

        if (m_Params.curPage > 0) {
            m_Box.append($.format(btnFormat, getFirstUrl(), btnTxt.first, 0, m_Params.command));
        }
        else {
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.first));
        }

        if (m_Params.curPage > 0) {
            m_Box.append($.format(btnFormat, getPrevUrl(), btnTxt.prev, m_Params.curPage - 1, m_Params.command));
        }
        else {
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.prev));
        }

        for (var i = m_StartPageIndex; i <= m_EndPageIndex; i++) {

            var url = $.format(m_Params.urlFormat, i);
            if (i == m_Params.curPage) {
                m_Box.append($.format("<span class='current'>{0}</span>", i + 1));
            }
            else {
                m_Box.append($.format(btnFormat, url, i + 1, i, m_Params.command));
            }

        }

        if (m_Params.curPage < m_Params.pageCount - 1) {
            m_Box.append($.format(btnFormat, getNextUrl(), btnTxt.next, m_Params.curPage + 1, m_Params.command));
            m_Box.append($.format(btnFormat, getLastUrl(), btnTxt.last, m_Params.pageCount - 1, m_Params.command));
        }
        else {
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.next));
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.last));
        }

        //给各个按钮加上事件
        if (m_ClickEvent) {
            var items = m_Box.children("a");
            $(items).click(m_ClickEvent);
        }

        var startRowIndex = m_Params.curPage * m_Params.rowCount;
        var endRowIndex = startRowIndex + m_Params.rowCount;

        if (endRowIndex > m_Params.itemTotal) {
            endRowIndex = m_Params.itemTotal;
        }

        m_Box.append($.format("当前记录 {0}--{1} 条,共 {2} 条记录", startRowIndex + 1, endRowIndex, m_Params.itemTotal));
    }

    init(options);
};