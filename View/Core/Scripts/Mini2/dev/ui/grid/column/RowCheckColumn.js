
/// <reference path="../../define.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.RowCheckColumn', {

    extend: 'Mini2.ui.grid.column.Colunm',


    //`'left'`, `'center'`, and `'right'`
    align: 'center',

    width: 40,

    renderTpl: [
        '<td role="gridcell" class="mi-grid-cell mi-grid-td mi-grid-cell-checkcolumn">',
            '<div class="mi-grid-cell-inner mi-unselectable mi-grid-cell-inner-checkcolumn">',
            '</div>',
        '</td>'
    ],

    initComponent: function () {
        "use strict";
        var me = this,
            renderer,
            listeners;

        renderer = me.renderer;

        if (renderer) {

            if (typeof renderer == 'string') {
                //me.renderer = Ext.util.Format[renderer];
            }
            me.hasCustomRenderer = true;
        }
        else if (me.defaultRenderer) {
            me.scope = me;
            me.renderer = me.defaultRenderer;
        }

        me.lockedHeader = true;
        me.headerWidth = 40;
    },


    getHeaderConfig: function () {
        "use strict";
        var me = this,
            showCheck = me.showHeaderCheckbox !== false;

        return {
            isCheckerHd: showCheck,
            text: '&#160;',
            clickTargetName: 'el',
            width: me.headerWidth,
            sortable: false,
            draggable: false,
            resizable: false,
            hideable: false,
            menuDisabled: true,
            dataIndex: '',
            cls: showCheck ? 'mi-column-header-checkbox ' : '',
            renderer: Mini2.Function.bind(me.renderer, me),
            editRenderer: me.editRenderer || me.renderEmpty,
            locked: me.hasLockedHeader()
        };
    },

    checkerOnCls: 'mi-grid-checkcolumn-checked',

    toggleUiHeader: function (isChecked) {

        this.grid.setHeaderCheck(isChecked);

    },

    defaultRenderer: function (value) {
        return value;
    },


    onHeaderClick: function (headerCt, header, isChecked) {
        "use strict";
        var me = this;

        if (header.isCheckerHd) {
            me.grid.setRowCheckAll(isChecked);

        }
    },

    renderEmpty: function () {
        return '&#160;';
    },

    renderer: function (value, meta) {
        "use strict";

        //console.debug('value = ', value);

        if (!value || value === false || value === 'false') {
            return '<img class="mi-grid-checkcolumn" src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" >';
        }

        return '<img class="mi-grid-checkcolumn mi-grid-checkcolumn-checked" src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw=="  >';

    },

    onSelectChange: function () {
        "use strict";
        //this.callParent(arguments);
        if (!this.suspendChange) {
            this.updateHeaderState();
        }
    },

    //更新标头的状态
    updateHeaderState: function () {
        "use strict";
        var me = this,
            isChecked = undefined,
            cellEl,
            imgEl,
            isCheck,
            grid = me.grid,
            col = grid.rowCheckColumn,
            rows,
            checkedCount = 0;

        rows = $(grid.tbodyEl).children();
                

        if (rows.length) {
            $(rows).each(function () {
                cellEl = $(this).children("td:eq(" + col.index + ")");
                imgEl = cellEl.find('img.mi-grid-checkcolumn');

                if (imgEl.length) {

                    isCheck = $(imgEl).hasClass("mi-grid-checkcolumn-checked");

                    if (isCheck) {
                        checkedCount++;
                    }

                }

                //if (!isCheck) {
                //    isChecked = false;
                //    return false;
                //}

            });


            if (checkedCount === rows.length) {
                isChecked = true;
            }
            else if (0 === checkedCount ) {
                isChecked = false;
            }
        }
        else {
            isChecked = false;
        }

        me.toggleUiHeader(isChecked);

        return me;
    }



});



