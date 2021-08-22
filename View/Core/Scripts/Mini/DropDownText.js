/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />


Mini.ui.DropDownText = function (options) {
    /// <summary>可编辑的选择下拉框</summary>

    var defaults = {
        id: '',
        width: 130,
        officeY :0
    };

    var m_Params = {
        id: '',
        width: 130
    };

    var m_Box;
    var m_SelectDiv;

    var m_Select;
    var m_Input;

    var m_ItemPanel;

    var m_OfficeY = 0;



    function init(options) {
        /// <summary>初始化</summary>

        m_Params = $.extend(defaults, options);

        m_Box = $("#" + m_Params.id);

        m_Select = m_Box.find(".select:first");
        m_Input = m_Box.find(".input:first");

        if (m_Params.width) {
            var ww = parseInt(m_Params.width);

            $(m_Box).css("width", m_Params.width);
            $(m_Input).css("width", ww - 20);
        }

        m_ItemPanel = $(m_Select).next(".Mini_DropDownPanel");



        $(m_ItemPanel).children("div").click(function () {
            $(m_Input).val($(this).text());

            $.powerFloat.hide();
        });

        var toY = ((window.curDialog != undefined) ? -30 : 0);

        $(m_Select).powerFloat({
            width: m_Params.width,
            height: 20 * 8,
            eventType: "click",
            position: "3-2",
            offsets: {
                x: 0,
                y: m_Params.officeY
            },
            target: $(m_ItemPanel)
        });

    }

    this.width = function (w) {

        if (w == undefined) {
            return m_Params.width;
        }
        else {
            var ww = parseInt(w);

            m_Params.width = w;

            if (m_Params.width) {
                $(m_Box).css("width", m_Params.width);
                $(m_Input).css("width", ww - 20);
            }
        }
    }

    this.officeY = function (value) {
        if (value == undefined) {
            return m_OfficeY;
        }
        else {
            m_OfficeY = value;
        }
    }

    init(options);
};