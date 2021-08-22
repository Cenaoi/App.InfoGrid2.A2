/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.DataGrid = function (options) {


    var defaults = {
        id: ''
    };

    //模板对象
    var m_TemplateObj;

    //分页对象
    var m_PaginatorObj;

    var m_ResetEvent = new Array();

    function init(options) {
        defaults = $.extend(defaults, options);



    }

    this.showCellError = function (cellGuid, msg) {

        try {
            var items = $("input[name='" + cellGuid + "']");

            $(items).addClass("error");
        }
        catch (ex) {
            alert("Mini.ui.DataGrid.showCellError(,,,);\n\n" + ex.Message);
        }

    }

    this.onReset = function (fn) {
        m_ResetEvent.push(fn);
    }

    this.reset = function () {

        for (var i = 0; i < m_ResetEvent.length; i++) {
            var fn = m_ResetEvent[i];

            fn();
        }

    }

    init(options);
};