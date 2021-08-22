

/**
 * 列表控件
 *
 */
Mini2.define('Mini2.ui.form.field.ListBox', {

    extend: 'Mini2.ui.form.CheckboxGroup',

    alias: 'widget.listbox',


    height: 150,

    extraFieldBodyCls: false,
    
    getGroup_Table: function () {
        "use strict";
        var me = this,
            items = me.items,
            len = items.length,
            columns = me.repeatColumns,
            col, yuNum, row,
            tableEl,
            result,
            height = me.height;

        tableEl = Mini2.$join([
            '<div class="mi-checkbox-group-inner mi-form-trigger-wrap" style="overflow: auto; padding: 4px; border: 1px solid #CCCCCC;" >',
                '<table id="radiogroup-', me.muid, '-innerCt" class="mi-table-plain" cellpadding="0" role="presentation" style="table-layout: auto; width:100%;">',
                    '<tbody>',
                    '</tbody>',
                '</table>',
            '</div>'
        ]);


        if (height && height != 'auto') {
            tableEl.css('height', height);
        }



        if (columns == 'auto') {
            me.createGroupCell(tableEl, 1, len);

            result = { 'el': tableEl, 'row': 1, 'col': len };
        }
        else if (columns <= 1) {
            me.createGroupCell(tableEl, len, 1);

            result = { 'el': tableEl, 'row': len, 'col': 1 };
        }
        else {
            col = parseInt(columns);
            yuNum = len % col;
            row = len / (len - yuNum);

            if (yuNum > 0) { row++; }

            me.createGroupCell(tableEl, row, col);

            result = { 'el': tableEl, 'row': row, 'col': col };
        }


        $(tableEl).on('click', function () {

            me.focus();
        });


        return result;
    }

});