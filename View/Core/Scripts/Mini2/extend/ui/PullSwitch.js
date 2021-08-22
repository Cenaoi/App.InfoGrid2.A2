/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />

//文本框窗体
Mini2.define('Mini2.ui.extend.PullSwitch', {

    extend: 'Mini2.ui.Component',

    el: null,

    panelId: null,

    right: 10,

    top: 0,

    height: 0,

    render: function () {
        var me = this,
            panelEl,
            el;

        me.el = el = $('<div style="position: fixed ; z-index:10010;width:20px;height:20px;font-family:Webdings;text-align: center;" unselectable="on">6</div>');

        el.css({
            'background-color': '#FFFFFF',
            'color': '#AAAAAA',
            'border-style': 'solid',
            'border-color': '#C0C0C0',
            'border-width': '0px 1px 1px 1px',
            'cursor': 'pointer'
        });

        el.css({ right: me.right, top: me.top });

        panelEl = $('#' + me.panelId);

        me.height = $(panelEl).outerHeight();

        panelEl.css({
            position: "fixed",
            right: me.right,
            top: me.top + 10,
            "z-index": 100,
            "margin": 0,
            "padding": 0,

            '-moz-box-shadow': '0px 2px 6px rgba(20%,20%,40%,0.5)',
            '-webkit-box-shadow': '0px 2px 6px rgba(20%,20%,40%,0.5)',
            'box-shadow':'0px 2px 6px rgba(20%,20%,40%,0.5)'
        });

        panelEl.hide();

        $(el).click(function (e) {


            if (panelEl.is(':hidden')) {
                panelEl.css({
                    right: 10,
                    top: -me.height,
                    "z-index": 10000
                });

                panelEl.show();

                panelEl.animate({
                    top: 0
                }, 'fast', function () {

                    el.text('5');
                });

            }
            else {
                panelEl.animate({
                    top: -me.height
                }, 'fast', function () {
                    panelEl.hide();

                    el.text('6');
                });

            }


        });


        me.renderTo.append(el);

    }
});