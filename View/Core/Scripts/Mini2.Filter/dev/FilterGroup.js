

/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Core/Scripts/Mini2/Mini2.js" />


Mini2.define('Mini2.ui.filter.FilterGroup', {

    extend: 'Mini2.ui.filter.Item',


    renderTpl: Mini2.join([
        '<li class="ui-state-highlight mi-andor mi-filter-item mi-filter-group" >',
            '<div class="mi-text mi-filter-text">AND</div>',
            '<a class="mi-filter-remove" style="" href="#" >',
                '<div class="mi-filter-remove-inner">X</div>',
            '</a>',
            '<ul class="connectedSortable mi-cont mi-filter-inner">',
            '</ul>',
        '</li>'
    ]),


    role: 'and',

    items: false,

    textEl: null,
    innerEl:null,


    baseRender: function () {
        var me = this,
            el,
            textEl,
            innerEl,
            txt;

        me.el = el = $(me.renderTpl);

        if ('and' == me.role) {
            txt = 'AND';
        }
        else if ('or' == me.role) {
            txt = 'OR';
        }


        textEl = el.children('.mi-filter-text');
        textEl.text(txt);

        innerEl = el.children('.mi-filter-inner');

        me.bindEvent(el);

        $(me.renderTo).append(el);

        me.textEl = textEl;
        me.innerEl = innerEl;



        el.children('.mi-filter-inner').sortable({
            connectWith: ".mi-filter-inner"
        }).disableSelection();


        return me;
    },

    getJson: function () {
        var me = this,
            cfg,
            i,
            innerEl = me.innerEl,
            itemEl,
            itemEls,
            itemCfg,
            item,
            items = [];

        itemEls = innerEl.children('.mi-filter-item');

        for (i = 0; i < itemEls.length; i++) {

            itemEl = itemEls[i];

            item = $(itemEl).data('me');

            itemCfg = item.getJson();

            items.push(itemCfg);
        }

        cfg = {
            role: me.role,
            items: items
        };

        return cfg;
    }

});