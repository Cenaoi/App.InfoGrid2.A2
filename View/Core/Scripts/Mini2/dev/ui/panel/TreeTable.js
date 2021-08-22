
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini.js" />

/// <reference path="../Mini-more.js" />
/// <reference path="../lang/Array.js" />

/// <reference path="template.min.js" />
/// <reference path="EventManager.js" />
/// <reference path="FocusManager.js" />

/// <reference path="grid/column/Column.js" />



Mini2.define('Mini2.ui.panel.TreeTable', {

    extend: 'Mini2.ui.panel.Table',

    rootId : '0',

    valueField: 'ID',
    textField: 'TEXT',
    parField: 'P_ID',

    pagerVisible:false,

    store_refresh: function (event, store) {

        var me = event.data,
            i,
            record,
            records;

        me.itemRemoteAll();

        records = me.store.data;


        for (i = 0; i < records.length; i++) {
            record = records.get(i);
            me.itemAdd(record);
        }

        me.itemsReset();

        if (me.rowCheckColumn) {
            me.rowCheckColumn.updateHeaderState();
        }
    },

    rootNode:{
     
        childs :[]
    },

    store_load: function (event, store) {

        var me = event.data,
            i,
            record,
            rootId = me.rootId,
            parField = me.parField,
            treeModels = store.getByGroup(rootId),
            treeModel,
            len,
            rowCheckColumn = me.rowCheckColumn;

        
        me.itemRemoteAll();

        len = treeModels.length;

        var outData = {};//返回的参数

        for (var i = 0; i < len; i++) {
            treeModel = treeModels[i];

            record = treeModel.data;

            me.itemInsert(i, record, outData);
            
            me.proRowEls(null, outData.rowEls, me.rootNode, 0, store, treeModel);
        }
        

        if (rowCheckColumn) {
            rowCheckColumn.updateHeaderState();
        }

    },


    proRowEls: function (parentRowEl,rowEls, parentNode, dept, store, parentTreeModel) {
        var me = this,
            pNode = parentNode,
            paddingLeft = dept * 24;

        if (!pNode.childs) {
            pNode.childs = [];
        }


        $(rowEls).each(function () {
            var rowEl = $(this);

            pNode.childs.push(rowEl);


            //深度
            var cellInnerEl = me.findCellInnerEl(rowEl);

            cellInnerEl.css('padding-left', paddingLeft + 'px');

            //叶设置
            var record = rowEl.data('record');

            var pk = record.getId();

            if (!store.hasChilds(pk)) {
                cellInnerEl.addClass('tree-leaf');
            }

            

            rowEl.attr('tree-dept', dept)
                .attr('tree-node-id', pk)
                .data('tree-node', {
                    parentRowEl: parentRowEl
                });

            //显示的节点缓冲
            me.displayRowsBuffer[pk] = rowEl;

        });
    },

    //显示的节点缓冲
    displayRowsBuffer : {},

    //查找树控件的内部容器
    findCellInnerEl: function (rowEl) {
        var me = this,
            cellEl = rowEl.children('.mi-grid-cell-treecolumn:first'),
            cellInnerEl = cellEl.children('.mi-grid-cell-inner:first');

        return cellInnerEl;
    },



    //删除记录
    store_bulkRemove: function (event, allRecords, indexes, isMove) {

        var me = event.data,
            exist,
            record,
            rowEl,
            pk,
            treeModel,
            rowEls = me.tbodyEl.children('tr');


        $(rowEls).each(function () {
            rowEl = $(this);
            record = rowEl.data("record");

            exist = Mini2.Array.contains(allRecords, record);

            if (exist) {
                pk = record.getId();

                treeModel = record._ownerTreeNode;

                var treeNode = rowEl.data('tree-node');

                rowEl.remove();
                Mini2.Array.remove(me.checkedRecords, record);

                delete me.displayRowsBuffer[pk];

                me._deleteTreeNode(treeNode.parentRowEl, record)

                return false;
            }

        });

        me.itemsReset();

    },

    _deleteTreeNode:function(parentRowEl, record){
        var me = this,
            pNode,
            childRowEl,
            i,
            pk,
            nodeId;

        if (parentRowEl) {

            pk = record.getId();

            pNode = $(parentRowEl).data('tree-node');

            for (i = 0; i < pNode.childs.length; i++) {

                childRowEl = pNode.childs[i];

                nodeId = $(childRowEl).attr('tree-node-id');

                if (nodeId == pk) {

                    pNode.childs.splice(i, 1);

                    break;
                }
            }
        }
    },


    store_add: function (event, index, out) {

        var me = event.data,
            store = this,
            i,
            record,
            treeModel,
            parField = me.parField,
            pValue,
            rowCheckColumn = me.rowCheckColumn;

        for (i = 0; i < out.length; i++) {

            record = out[i];

            treeModel = record._ownerTreeNode;

            pValue = record.get(parField);
            
            //获取上级已经显示的行。
            var pRowEl = me.displayRowsBuffer[pValue];

            if (pRowEl) {

                var pNode = $(pRowEl).data('tree-node');

                var n = $(pRowEl).index() + 1;

                if (pNode.childs) {
                    n += pNode.childs.length;
                }


                var depth = $(pRowEl).attr('tree-dept');

                depth = parseInt(depth) + 1;

                var outData = {}; //插入数据后，返回的对象

                me.itemInsert(n, record, outData);

                me.proRowEls(pRowEl,outData.rowEls, pNode, depth, store, treeModel);
            }
            
        }

        if (rowCheckColumn) {
            rowCheckColumn.updateHeaderState();
        }
    },



    deleteRows: function (pRows) {
        var me = this;

        $(pRows).each(function () {

            var treeNode = $(this).data('tree-node');


            me.deleteRows(treeNode.childs);
            

            $(this).remove();
        });
    },


    //展开节点
    expandNode: function (treeNode, rowEl,cellEl) {
        var me = this,
            store = me.store,
            dept = $(rowEl).attr('tree-dept'),
            cellInnerEl = $(cellEl).children('.mi-grid-cell-inner');;

        dept = parseInt(dept);

        var record = $(rowEl).data('record');

        var id = record.getId();

        var rootId = store.rootId;

        var treeModels = store.getByGroup(id);

        if (treeModels) {
            //var rowElTreeNode = $(rowEl).data('tree-node');

            cellInnerEl.addClass('tree-open');

            treeNode.expanded = true;

            var rowIndex = $(rowEl).index() + 1;

            var len = treeModels.length;

            var outData = {};

            dept += 1;

            for (var i = 0; i < len; i++) {
                var treeModel = treeModels[i];

                var record = treeModel.data;

                me.itemInsert(rowIndex + i, record, outData);

                me.proRowEls(rowEl, outData.rowEls, treeNode, dept, store);
            }



        }

    },

    //折叠节点
    collapseNode: function (treeNode,rowEl, cellEl) {
        var me = this,
            cellInnerEl = $(cellEl).children('.mi-grid-cell-inner');

        cellInnerEl.removeClass('tree-open');

        if (treeNode) {

            //var node = $(rowEl).data('tree-node');

            treeNode.expanded = false;

            me.deleteRows(treeNode.childs);

            delete treeNode.childs;
        }

    },

    //展开折叠，进行切换
    toggleNode: function (treeNode,rowEl,cellEl) {
        var me = this;

        if (treeNode.expanded) {
            me.collapseNode(treeNode, rowEl, cellEl);
        }
        else {
            me.expandNode(treeNode, rowEl, cellEl);
        }

    }

});