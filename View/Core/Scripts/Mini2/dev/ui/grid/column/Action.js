/// <reference path="../../Mini2.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.Action', {

    extend: 'Mini2.ui.grid.column.Colunm',

    sortable: false,

    menuDisabled: true,

    innerCls: function () {
        "use strict";
        var me = this,
            cls = 'mi-grid-cell-inner-action-col';

        if (me.autoHide) {
            cls += ' mi-grid-cell-autohide';
        }

        return cls;
    },

    tdCls: 'mi-action-col-cell',

    //自动隐藏按钮
    //    autoHide: false,

    renderItem_ForIcon: function (i, item) {
        "use strict";
        var itemObj = $('<img role="button" alt="" src="" class="mi-action-col-icon" data-qtip="" style="margin-left:3px;margin-right:3px; " click="alert(\'dddddddddd\')" />');

        itemObj.addClass('mi-action-col-' + i)
                    .attr('src', item.icon)
                    .attr('data-qtip', item.tooltip)
                    .attr('itemId', i)
                    .attr('title', item.text);

        return itemObj;
    },


    renderItem_ForText: function (i, item) {
        var itemObj = $('<a role="button" alt="" href="#" class="mi-action-col-icon" data-qtip="" style="margin-left:3px;margin-right:3px;"></a>');
        itemObj.attr('itemId', i)
                    .html(item.text);

        return itemObj;
    },

    renderItem_ForIconText: function (i, item) {
        "use strict";
        var itemObj = Mini2.$joinStr([
                    '<a role="button" alt="" href="#" class="mi-action-col" data-qtip="" ',
                        'style="margin-left:3px;margin-right:3px; ">',
                        '<img src="" class="mi-action-col-icon" data-qtip="" />',
                        '<span class="mi-action-col-text" style="margin-left: 4px;">',
                            item.text,
                        '</span>',
                    '</a>']);

        itemObj.cFirst('.mi-action-col-icon')
                    .attr('src', item.icon);

        itemObj.attr('itemId', i);

        return itemObj;
    },

    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store) {
        "use strict";
        var me = this,
            i,
            displayMode,
            item,
            itemObj,
            items = me.items,
            len = items.length,
            ret;

        ret = '<table border="0" cellpadding="0" cellspacing="0" align="center" style="height:16px;"><tr>';

        for (i = 0; i < len; i++) {

            item = items[i];

            displayMode = item.displayMode = (item.displayMode || 'auto');

            if ('auto' == displayMode) {

                var hasIcon = !Mini2.isBlank(item.icon);
                var hasText = !Mini2.isBlank(item.text);

                if (hasIcon && hasText) {
                    displayMode = 'icontext';
                }
                else if (hasIcon && !hasText) {
                    displayMode = 'icon';
                }
                else if (!hasIcon && hasText) {
                    displayMode = 'text';
                }

            }


            switch (displayMode) {
                case 'icontext': itemObj = me.renderItem_ForIconText(i, item); break;
                case 'icon': itemObj = me.renderItem_ForIcon(i, item); break;
                case 'text': itemObj = me.renderItem_ForText(i, item); break;
            }

            var tmpDiv = $('<div></div>').append(itemObj);

            ret += '<td>' + tmpDiv.html() + '</td>';
        }

        ret += '</tr></table>';

        return ret;
    },


    bindEvent: function (view, cell, recordIndex, cellIndex, e, record, row) {
        "use strict";
        var me = this,
            btns = cell.find("[role='button']");

        $(btns).muBind('mousedown', Mini2.emptyFn)
        .muBind('mouseup', function (e2) {

            me.processEvent("mouseup", view, cell, recordIndex, cellIndex, e2, record, row);

            Mini2.EventManager.stopEvent(e2);

        }).on('click', function (e2) { return false; });
    },

    processEvent: function (type, view, cell, recordIndex, cellIndex, e, record, row) {
        "use strict";
        var me = e.currentTarget,
            item,
            handler,
            clickHandler,
            result = null,
            items = this.items,
            itemId = parseInt($(me).attr("itemId"));

        item = items[itemId];

        clickHandler = item.clickHandler;
        handler = item.handler;

        if (clickHandler && Mini2.isFunction(clickHandler)) {

            result = clickHandler.call(me, {
                record: record,
                view: view,
                cell: cell,
                recordIndex: recordIndex,
                cellIndex: cellIndex,
                event: e,
                row: row
            });

            if (result === false) {
                return;
            }
        }
        else if (handler && Mini2.isFunction(handler)) {

            result = handler.call(me, view, cell, recordIndex, cellIndex, e, record, row);

            if (result === false) {
                return;
            }
        }


        if (!Mini2.isBlank(item.href)) {

            console.debug("item ---- ", item);

            var rendererHref = this.rendererHref_art;

            var href;

            try {
                href = rendererHref.call(this, item, record);
            }
            catch (ex) {
                console.error('地址模板解析处理失败,', item);
                return;
            }

            var target = item.target.toLowerCase();

            if (Mini2.isBlank(target)) {

                window.location.href = href;

            }
            else if ('ecview' == target) {

                EcView.show(href, item.targetText);
            }
            else if ('dialog' == target) {
                var frm = Mini2.createTop('widget.window', {
                    url: href,
                    text: item.targetText,
                    width: 400,
                    height: 300,
                    state:'max',
                    mode: true
                });

                frm.show();
            }
            else {

                console.debug('未知标识', target);

            }

            return;
        }
    },

    /**
    * jTemplate 模板引擎
    */
    rendererHref_j: function (item, record) {


    },

    /**
    * artTemplate 腾信模板引擎
    */
    rendererHref_art: function (item, record) {
        "use strict";
        var me = this,
            valueStr,
            artTemp = item.artTemplate;

        if (!artTemp) {
            item.artTemplate = artTemp = template.compile(item.href);
        }

        valueStr = artTemp({
            T: record
        });

        return valueStr;
    }

});