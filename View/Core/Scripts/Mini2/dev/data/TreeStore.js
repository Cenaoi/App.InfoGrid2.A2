/// <reference path="../Mini.js" />

/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="StoreManager.js" />
/// <reference path="../lang/Array.js" />



Mini2.define('Mini2.data.TreeStore', {

    extend: 'Mini2.data.Store' ,

    isTreeStore : true ,

    //上级字段名
    parentField: null,

    textField: null,

    //记录分组
    tree : false,

    //根节点的值
    rootValue: '0',

    //动态处理子节点
    dymHasChild:true,


    //是否有子节点
    hasChilds: function (parentId) {
        var me = this,
            tree = me.tree || me.createTree();

        return !!tree.getChilds(parentId)
    },

    //根据上级Id，获取子节点集合
    getByGroup: function (parentId) {
        var me = this,
            tree = me.tree || me.createTree();
        
        return tree.getChilds(parentId)
    },


    //初始化树分组
    createTree: function () {
        var me = this,
            pField = me.parentField,
            tree = me.tree,
            srcData = me.data ;

        if (!tree) {
            me.tree = tree = Mini2.create('Mini2.data.Tree', {
                model:me.model,
                parentField: pField,
                data : srcData
            });
        }

        return tree;
    },


    //创建初始化数据集
    createInlineData: function () {
        "use strict";
        var me = this,
            log = me.log,
            model = me.model,
            i,
            config,
            record,
            data = me.data,
            len;

        if (data) {

            len = data.length;

            for (i = 0; i < len; i++) {
                config = data[i];

                if ('tree-model' == config.role) {

                    var has_child = config.has_child;
                    var role = config.role;

                    record = Mini2.ModelManager.create(config.data, model);
                    
                    record.has_child = has_child;
                    record.role = role;

                }
                else {
                    record = Mini2.ModelManager.create(config, model);
                }

                record.joinStore(me);


                data[i] = record;
            }
        }

        me.inlineData = data;
        delete me.data;

        //log.debug('封装 data', data);
        me.data = new Mini2.collection.ArrayList(me.inlineData);

        //log.debug('封装后 data', me.data);

        me.updateRecordsIndex();
    },


    //按 id 删除记录
    removeById: function (id, silent) {
        "use strict";
        //log.debug("按 id 删除记录.");

        var me = this,
            log = me.log,
            i,
            list,
            data = me.data;


        if (!Mini2.isArray(id)) {
            id = [id];
        }

        //list = data.filterBy(function (record) {
        //    return Mini2.Array.contains(id, record.getId(), true);
        //});

        //if (list.length) {
        //    me.remove(list, false, silent);
        //}
        //else {

            var resultRecords = id;

            if (!silent) {

                me.on('remove', [resultRecords, false]);
                me.onDataChanged();
            }

        //}


        me.disposeModels(list); //释放实体的内存

        return list;
    },



    remove: function (records, /* private */isMove, silent) {
        var me = this,
            base = me.constructor.base,
            resultRecords,
            i,
            tree = me.tree || me.createTree();;
        
        resultRecords = base.remove.call(me, records, isMove, silent);

        for (i = 0; i < resultRecords.length; i++) {
            tree.removeNode(resultRecords[i]);
        }
    },

    //创建记录实体
    createModel: function (record) {
        "use strict";
        //log.begin("Store.createModel()");
        var me = this;

        //console.debug('加载实体...', record);

        if ('tree-model' == record.role) {

            if (!me.model) {
                throw new Error('Store.Model 不能等于 null');
            }

            var hasChild = record.has_child;

            record = Mini2.ModelManager.create(record, me.model);


            record.role = 'tree-model';
            record.has_child = hasChild;


        }
        else if (!record.isModel) {
            if (!me.model) {
                throw new Error('Store.Model 不能等于 null');
            }


            record = Mini2.ModelManager.create(record, me.model);

        }

        //log.end();


        return record;
    },


    //插入记录
    insert: function (index, records) {
        var me = this,
            log = me.log,
            base = me.constructor.base,
            treeModel,
            record,
            out ,
            records2 = [],
            treeModels = [],
            i,
            tree = me.tree || me.createTree();

        if (!Mini2.isArray(records)) {
            out = [records];
        }
        else{
            out = records;
        }

        records = out;
        

        for (i = 0; i < records.length; i++) {

            record = records[i];
            
            if ('tree-model' == record.role) {

                treeModel = record;

                record = record.data;

                record._ownerTreeNode = treeModel;
                
                record.role = 'tree-model';
                record.has_child = treeModel.has_child;
                
            }
            else if (record.isNode) {
                treeModel = record;
                record = treeModel.data;

                record._ownerTreeNode = treeModel;

                //if (treeModel.has_child) {
                //    record.has_child = treeModel.has_child;
                //}
            }
            else {

                //try {
                //    record = me.createModel(record);
                //}
                //catch (ex) {
                //    throw new Error('创建 record 实体错误.' + ex.message);
                //}

            }
            
            records2.push(record);

        }

        //基础函数
        base.insert.call(me, index, records2, false);

        
        for (i = 0; i < records2.length; i++) {
            tree.insertNode(records2[i]);
        }

        me.updateRecordsIndex();

        me.on('add', [index, records2]);

        me.onDataChanged();

    },



    //设置当前焦点行
    setCurrent: function (index, isTriggerSer) {
        "use strict";
        var me = this,
            log = me.log,
            cur,
            curIndex = 0,
            list,
            data = me.data;

        console.debug('数据仓库 setCurrent ', index);
        
        if (undefined === data ) {

            me.current = null;
            me.currentIndex = -1;

            me.focusNode = null;

            
            me.on('currentchanged', [me.currentIndex, me.current]);

            if (me.currentChanged) {
                me.currentChanged.call(me);
            }

            return;
        }

        if (Mini2.isNumber(index)) {

        }
        else {
            cur = index;

            me.focusNode = cur;
            me.current = cur;
            me.currentIndex = curIndex;
        }

        me.on('currentchanged', [curIndex, cur]);

        if (me.currentChanged) {
            me.currentChanged.call(me);
        }
    }

});