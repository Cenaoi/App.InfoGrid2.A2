

/// <reference path="../Mini.js" />

/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="StoreManager.js" />
/// <reference path="../lang/Array.js" />


Mini2.define('Mini2.data.Store', {

    log: Mini2.Logger.getLogger('Mini2.data.Store',false),
        
    storeId: false,

    /**
    * 全部提交.
    * smaple | full
    */
    jsonMode: 'sample',
    
    isStore:true,

    /**
     * 增量加载模式
     */
    isIncr : false,

    //自动加载数据
    autoLoad: true,

    /**
    * 自动触发回调函数. 默认 true
    */
    autoCallback:true,

    //自动同步
    autoSync: true,


    autoSyncSuspended: false,

    //远程排序参数
    remoteSort: false,

    //远程过滤参数
    remoteFilter: false,

    //远程分组参数
    remoteGroup: false,

    //
    groupField: undefined,

    //"ASC" and "DESC".
    groupDir: "ASC",

    sorters: false,


    pageSize: 65535,

    //当前页码
    currentPage: 0,

    //已缓冲, 默认 false
    buffered: false,

    data: false,

    //fields: [],

    //idField : '',  //组件字段

    //lockedField:'' ,//锁字段，不允许修改

    //锁后, 排序的字段集合
    //lockedExclusionFields:false,


    //代理器
    proxy: {},

    modelDefaults: null,

    //数据实体模型
    model: null,

    //数据仓库内部的实体对象
    implicitModel: false,

    //是否已经加载了数据
    isLoadData: false,


    totalCount: 0,


    //自动提交保存到服务器
    autoSave: true,

    //虚拟的数量，也就是分页中的总数量
    vTotalCount: -1,

    //特殊汇总字段
    summary: false,

    //自动焦点触发
    autoFocus: true,

    //当前焦点行
    //current 

    //当前焦点行的索引
    //currentIndex

    /* requestOperation 请求的参数 */

    
    on: function (eventName, data, scope) {
        "use strict";
        /// <summary>绑定事件</summary>
        var me = this;

        if (me.isLoadData) { return me; }

        $(me).triggerHandler(eventName, data);

        return me;
    },



    intiSorters: function () {
        "use strict";
        var me = this,
            sorters = me.sorters;

        me.sorters = new Mini2.collection.ArrayList(false, function (item) {
            return item.id || item.property;
        });

        if (sorters) {
            me.sorters.addAll(me.decodeSorters(sorters));
        }


    },

    //创建隐形实体
    createImplicitModel: function () {
        "use strict";
        var me = this,
            name = me.modelName,
            fields = me.fields,
            newModelId;

        if (!me.model && fields) {

            newModelId = 'Mini2.data.Store.ImplicitModel_' + (me.storeId || Mini2.newId());


            me.model = Mini2.define(newModelId, {
                name: name,
                extend: 'Mini2.data.ModelX',
                fields: fields
            });

            delete me.fields;
            me.implicitModel = true;
        }
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
            len ;

        if (data ) {

            len = data.length;

            for (i = 0; i < len; i++) {
                config = data[i];
                
                record = Mini2.ModelManager.create(config, model);

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

    preInit: function (config) {
        "use strict";

        var me = this,
            modelName,
            model = me.model,
            log = me.log;


        if (me.storeId) {
            //log.debug("Mini2.data = " + Mini2.data);
            Mini2.data.StoreManager.regStore(me);
        }

        me.removed = [];


        if (Mini2.isBlank(model)) {

            model = Mini2.ModelManager.getModel('Mini2.data.FreeModel');

        }
        else {

            if (Mini2.isString(model)) {
                modelName = model;
                me.modelName = modelName;
            }

            model = Mini2.ModelManager.getModel(model);

            if (model) {
                model.prototype.name = modelName;
            }
        }

        me.model = model;

        me.intiSorters();

        me.createImplicitModel();   //创建隐形实体


        if (!me.disableMetaChangeEvent) {
            //原：me.proxy.on('metachange', me.onMetaChange, me);



            var i,
                len,
                fields ,
                idField, newField,
                prototypeFields = new Mini2.collection.ArrayList(false, function (field) {
                    return field.name;
                });

            fields = me.model.prototype.fields;
            

            for (i = 0, len = fields.length; i < len; i++) {
                newField = new Mini2.data.Field(fields[i]);

                if (idField && ((newField.mapping && (newField.mapping === idField.mapping)) || (newField.name === idField.name))) {
                    idFieldDefined = true;
                    newField.defaultValue = undefined;
                }
                prototypeFields.add(newField);
            }

            me.model.prototype.constructor['fields'] = prototypeFields;

            if (me.idField) {
                var idField = new Mini2.data.Field(me.idField);
                me.model.prototype.constructor['idField'] = idField;

                delete me.idField;
            }

            if (me.lockedField) {

                me.model.prototype.constructor['lockedField'] = me.lockedField;

                delete me.lockedField;
            }

            if (me.lockedRule) {

                me.model.prototype.lockedRule = me.lockedRule;
                delete me.lockedRule;
            }

        }


        me.createInlineData();  //创建初始化数据集

        me.createBindEvent();

        if (me.totalCount <= 0) {
            me.totalCount = me.data.getCount();
        }
    },



    //设置汇总字段值
    setSummary: function (field, value) {
        "use strict";
        var me = this,
            i,
            summary = me.summary || {};

        if (!me.summary) {
            me.summary = summary;
        }

        if (Mini2.isString(field)) {
            summary[field] = value;

            i = {};
            i[field] = value;

            me.on('summarychnaged', [i]);
        }
        else {
            for (i in field) {
                summary[i] = field[i];
            }

            me.on('summarychnaged', [field]);
        }


        return me;
    },

    //获取汇总字段值
    getSummary: function (field) {
        "use strict";
        var me = this,
            summary = me.summary || {};

        if (arguments.length) {
            return summary[field];
        }

        return summary;
    },



    //设置当前页码
    setCurrentPage: function (page) {
        this.currentPage = page;
    },

    //获取当前页码
    getCurrentPage: function () {
        return this.currentPage;
    },

    //更新数据的标记,用于触发自动保存的时候.
    //updated_tag: false,


    //循环调用
    loopSaveAll: function () {
        "use strict";
        var me = this,
            log = me.log;

        if (!me.autoCallback) {
            return;
        }

        Mini2.awaitRun( me.muid, 500, me, function () {

            var fields = me.getUpdatedRecords();

            if (0 == fields.length) {
                return;
            }


            try {

                var widget = window.widget1;

                if (widget && widget.subMethod) {
                    try {
                        widget.subMethod($('form:first'), {
                            subName: me.id,
                            subMethod: 'SaveAll'
                        });
                    }
                    catch (ex) {
                        console.error("提交失败", ex);
                        alert('提交失败:' + ex.message);
                    }
                }
            }
            catch (ex) {
                log.debug("error:" + ex.message);
            }

        });
        



        //setTimeout(function () {

        //    if (!me.updated_tag) {
        //        return;
        //    }

        //    me.updated_tag = false;

        //    try {
        //        var fields = me.getUpdatedRecords();

        //        if (0 == fields.length) {
        //            return;
        //        }

        //        var widget = window.widget1;
        //        if (widget && widget.subMethod) {
        //            try {
        //                widget.subMethod($('form:first'), {
        //                    subName: me.id,
        //                    subMethod: 'SaveAll'
        //                });
        //            }
        //            catch (ex) {
        //                console.error("提交失败", ex);
        //                alert('提交失败:' + ex.message);
        //            }
        //        }
        //    }
        //    catch (ex) {
        //        log.debug("error:" + ex.message);
        //    }


        //    me.loopSaveAll();

        //},1000);
    },



    //创建绑定事件,配合服务器控件使用
    createBindEvent: function () {
        "use strict";
        var me = this;

        if (me.autoSave) {

            $(me).muBind('update', me, function () {

                //var fields = me.getUpdatedRecords();
                //if (0 == fields.length) {
                //    return;
                //}

                //me.updated_tag = true;

                me.loopSaveAll();

            });
        }
    },




    updateRecordsIndex: function () {
        "use strict";
        var me = this,
            i = 0,
            record,
            data = me.data,
            count = data.getCount();

        for (; i < count ; i++) {

            record = data.get(i);
            record.index = i;
        }
    },


    //记录恢复为正常状态触发的事件
    afterCommit: function (record, modifiedFieldNames) {
        "use strict";

        var me = this;

        if (!modifiedFieldNames) {
            modifiedFieldNames = null;
        }

        //alert("update");

        me.on('update', [record, 'COMMIT', modifiedFieldNames]);

        //me.on('update', this, record, Ext.data.Model.COMMIT, modifiedFieldNames);
    },


    //根据id，设置记录为焦点行
    setCurrntForId: function (id) {
        "use strict";
        var me = this,
            rect;

        rect = me.getById(id);
                
        me.setCurrent(rect);

        return !!rect;
    },

    //给外部作为手动触发事件
    onCurrentChanged:function(){
        "use strict";
        var me = this,
            log = me.log,
            cur = me.current,
            curIndex = me.currentIndex;
        
        me.on('currentchanged', [curIndex, cur]);

    },


    __lastCurIndex :-1,

    //设置当前焦点行
    setCurrent: function (index,isTriggerSer) {
        "use strict";
        var me = this,
            log = me.log,
            cur,
            curIndex,
            list,
            data = me.data;



        log.debug("设置当前焦点行 setCurrent({0})", index);
        log.debug("记录数量", data.length);

        if (undefined == data || data.length == 0) {

            me.current = null;
            me.currentIndex = -1;

            log.debug("setCurrent(...) 触发 currentchange 事件");

            me.on('currentchanged', [me.currentIndex, me.current]);


            log.debug("isTriggerSer = ", isTriggerSer);


            if (me.currentChanged) {
                me.currentChanged.call(me);
            }

            return;
        }

        if (Mini2.isNumber(index)) {

            cur = data.get(index);
            curIndex = index;
        }
        else {

            list = data.filterBy(function (rec) {
                if (rec == index) {
                    return true;
                }

                return false;
            });

            if (list.length > 0) {
                cur = list.get(0);

                curIndex = cur.index;
            }
            else {
                cur = null;
                curIndex = -1;
            }
        }

        me.current = cur;
        me.currentIndex = curIndex;


        me.on('currentchanged', [curIndex, cur]);

        if (me.__lastCurIndex !== curIndex) {

            me.__lastCurIndex = curIndex;

            if (me.currentChanged) {
                me.currentChanged.call(me);
            }
        }
    },


    //获取当前焦点对象
    getCurrent: function () {
        var me = this;

        return me.current;
    },

    //获取当前焦点对象的索引值
    getCurrentIndex: function () {
        "use strict";
        var me = this;

        return me.currentIndex;
    },


    //更改全部记录为正常状态
    commitChanges: function () {
        "use strict";
        var me = this,
            recs = me.getModifiedRecords(),
            len = recs.length,
            i = 0;

        for (; i < len; i++) {
            recs[i].commit();
        }

        // Since removals are cached in a simple array we can simply reset it here.
        // Adds and updates are managed in the data MixedCollection and should already be current.
        me.removed.length = 0;
    },


    //根据 muid ，更改记录某些字段为正常状态
    commitChangesForMuid: function (muid, modifiedNames) {
        "use strict";
        var me = this,
            data = me.data,
            record,
            index;

        index = data.findIndexBy(function (record) {
            if (record.muid == muid) {
                return true;
            }
        });

        if (index == -1) {
            console.debug('没有找打需要修改的记录, muid=', muid);
            return;
        }

        record = data.get(index);

        record.commit(null, modifiedNames);

    },


    //添加记录。可以是对象，或集合
    add: function (arg) {
        "use strict";
        var me = this,
            records,
            length, isSorted;

        if (Mini2.isArray(arg)) {
            records = arg;
        }
        else {
            records = arguments[0];
        }


        length = records.length;

        //records = me.insert(me.data.length, records);
        records = me.insert(1000000, records);


        return records;
    },


    load: function (options) {
        "use strict";
        /// <summary>加载数据</summary>

        var me = this;

        me.lastOptions = Mini2.apply({
            action: 'read',
            sorters: me.getSorters()
        }, options);

        me.on('load', [me]);

    },

    //过滤器：
    filterNewOnly: function (item) {
        return item.phantom === true;
    },


    //过滤器：
    filterNew: function (item) {
        return item.phantom === true && item.isValid();
    },


    //过滤器：
    filterUpdated: function (item) {
        //return item.dirty === true && item.phantom !== true && item.isValid();

        return item.dirty === true && item.phantom !== true;
    },


    //获取新建的记录集合
    getNewRecords: function () {
        return this.data.filterBy(this.filterNew);
    },


    //获取更新的记录集合
    getUpdatedRecords: function () {
        return this.data.filterBy(this.filterUpdated);
    },

    //获取删除的记录集合
    getRemovedRecords: function () {
        return this.removed;
    },


    //同步过程
    sync: function (options) {
        "use strict";
        var me = this,
            operations = {},
            toCreate = me.getNewRecords(),
            toUpdate = me.getUpdatedRecords(),
            toDestroy = me.getRemovedRecords(),
            needsSync = false;

        if (toCreate.length > 0) {
            operations.create = toCreate;
            needsSync = true;
        }

        if (toUpdate.length > 0) {
            operations.update = toUpdate;
            needsSync = true;
        }

        if (toDestroy.length > 0) {
            operations.destroy = toDestroy;
            needsSync = true;
        }

        //alert("needsSync = " + needsSync);

        if (needsSync && me.on('beforesync', operations) !== false) {
            options = options || {};

            //            me.proxy.batch(Ext.apply(options, {
            //                operations: operations,
            //                listeners: me.getBatchListeners()
            //            }));
        }

        return me;
    },


    //按 id 删除记录
    removeById: function (id, silent) {
        "use strict";
        //log.debug("按 id 删除记录.");

        var me = this,
            log = me.log,
            list,
            data = me.data;


        if (!Mini2.isArray(id)) {
            id = [id];
        }

        list = data.filterBy(function (record) {
            return Mini2.Array.contains(id, record.getId(), true);
        });

        //log.debug("删除 " + list.length + " 记录");

        me.remove(list, false, silent);
        
        me.disposeModels(list); //释放实体的内存

        return list;
    },


    removeByMuid: function (muid, silent) {
        "use strict";
        /// <summary>按 muid 删除记录</summary>

        //log.debug("按 muid 删除记录.");

        var me = this,
            list,
            data = me.data;

        if (!Mini2.isArray(muid)) {
            muid = [muid];
        }

        list = data.filterBy(function (record) {
            return Mini2.Array.contains(muid, record.muid);
        });


        me.remove(list, false, silent);
        
        me.disposeModels(list); //释放实体的内存

        return list;
    },

    //释放记录对象
    disposeModels: function (records) {
        "use strict";
        var me = this,
            record,
            len,
            i = 0;

        if (records && Mini2.isArray(records)) {
            len = records.length;
            for (i; i < len; i++) {
                record = records[i];

                if (record && record.isModel) {
                    record.dispose();
                }
            }
        }
    },


    removeAll: function (silent) {
        "use strict";
        var me = this,
            snapshot = me.snapshot,
            data = me.data,
            list;

        me.__lastCurIndex = -1;

        if (snapshot) {
            snapshot.removeAll(data.getRange());
        }

        list = me.remove({
            start: 0,
            end: me.getCount() - 1
        }, false, silent);



        if (silent !== true) {
            me.on('clear', me);

            me.onDataChanged();
        }

        me.disposeModels(list); //释放实体的内存

        return me;
    },

    /**
     * 当前加载的数据
     */
    curLoadData : null,

    beginLoadData: function () {
        "use strict";
        var me = this;

        me.isLoadData = true;

        me.curLoadData = [];

        me.onBeginLoaded();

        return me;
    },

    endLoadData: function () {
        var me = this,
            log = me.log;

        if (me.isLoadData) {

            me.isLoadData = false;
            me.onLoad();

            //me.on('add', [10000000, me.curLoadData]);

            me.onDataChanged();

            try {


                if (me.autoFocus) {
                    me.setCurrent(me.currentIndex || 0);
                }
            }
            catch (ex) {
                console.error('自动设置焦点行错误', ex);
            }

            me.onEndLoaded();
        }

        return me;
    },

    onBeginLoaded:function(){
        var me = this;

        try{
            me.on('beginloaded',me);
        }
        catch (ex) {
            console.error('触发 onBeginLoaded 错误', ex);
        }

    },

    onEndLoaded: function () {
        var me = this;


        try {
            me.on('endloaded', me);
        }
        catch (ex) {
            console.error('触发 onEndLoaded 错误', ex);
        }

    },


    onDataChanged: function () {
        "use strict";
        var me = this,
            log = me.log;



        if (me.getCount() > me.totalCount) {
            me.totalCount = me.getCount();
        }

        log.debug("触发 onDataChanged 事件。");

        try {
            me.on('datachanged', me);
        }
        catch (ex) {
            console.error('触发 onDataChanged 错误', ex);
        }
    },

    onLoad: function () {
        var me = this;

        try{
            me.on('load', me);
        }
        catch (ex) {
            console.error('触发 onLoad 错误', ex);
        }
    },


    //删除记录，或范围
    //返回：被删除的对象。
    remove: function (records, /* private */isMove, silent) {
        "use strict";
        isMove = isMove === true;

        var me = this,
            sync = false,
            snapshot = me.snapshot,
            data = me.data,
            i = 0,
            length,
            info = [],
            allRecords = [],
            indexes = [],
            item,
            isNotPhantom,
            index,
            record,
            removeRange,
            removeCount,
            resultItems = [];

        //fireRemoveEvent = !silent && me.hasListeners.remove;

        // Remove a single record
        if (records.isModel) {
            records = [records];
            length = 1;
        }

        if (records.isModel) {
            records = [records];
            length = 1;

        }
        else if (Mini2.isIterable(records)) {

            if (records.toArray) {
                records = records.toArray();
            }

            length = records.length;

        }
        else if (typeof records === 'object') {
            removeRange = true;
            i = records.start;
            length = records.end + 1;
            removeCount = length - i;
        }




        if (!removeRange) {


            for (i = 0; i < length; i++) {
                record = records[i];

                index = data.remove(record);

                record.unjoinStore(me);

                indexes.push(index);
                allRecords.push(record);


                me.removed.push(record);

                me.on('remove', record, index, !!isMove);
            }

            resultItems = records;
        }

        if (removeRange) {
            resultItems = data.removeRange(records.start, removeCount);
        }


        me.updateRecordsIndex();

        if (!silent) {

            me.on('bulkremove', [allRecords, indexes, !!isMove]);
            me.onDataChanged();
        }


        //删除记录后,重新设置焦点行

        var existDel = Mini2.Array.contains(indexes, me.currentIndex);


        if (existDel) {

            var newCurIndex = -1;

            if (me.currentIndex >= me.data.length) {
                newCurIndex = me.data.length - 1;
            }
            else {
                newCurIndex = me.currentIndex;
            }

            me.setCurrent(newCurIndex);
        }

        return resultItems;
    },


    //编辑以后触发的事件
    afterEdit: function (record, modifiedFieldNames) {
        "use strict";
        var me = this,
            i, shouldSync;



        //        if (me.autoSync && !me.autoSyncSuspended) {
        //            for (i = modifiedFieldNames.length; i--; ) {
        //                // only sync if persistent fields were modified
        //                if (record.fields.get(modifiedFieldNames[i]).persist) {
        //                    shouldSync = true;
        //                    break;
        //                }
        //            }
        //            if (shouldSync) {
        //                me.sync();
        //            }
        //        }

        //me.onUpdate(record, Ext.data.Model.EDIT, modifiedFieldNames);

        //console.log('store.record', record);
        //console.log('store.modifiedFieldNames', modifiedFieldNames);

        me.on('update', [record, 'EDIT', modifiedFieldNames]);
    },




    //根据 record 主键，清理
    clearInvalidAll_ByRecordId: function (recordId, fields) {
        var me = this,
            record = me.getById(recordId);

        if (record) {
            record.clearInvalid(fields);
        }
    },



    //根据 record 主键，清理
    clearInvalid_ByRecordId: function (recordId, fields) {
        var me = this,
            record = me.getById(recordId);

        if (record) {
            record.clearInvalid(fields);
        }
    },


    //根据 record 主键，设置错误信息
    markInvalid_ByRecordId: function (recordId, error) {
        var me = this,
            record = me.getById(recordId);

        if (record) {
            try {
                record.clearInvalidAll();
                record.markInvalid(error);
            }
            catch (ex) {
                console.error("执行函数错误: markInvalid_ByRecordId(...,..)." + ex.Message);
            }
        }

    },


    //创建异常信息触发的事件
    afterInvalid: function (record, fieldNames) {
        var me = this;

        me.on('invalid', [record, fieldNames]);
    },


    filterFirstBy: function (field, value) {
        "use strict";
        var me = this,
            list,
            data = me.data;

        list = data.filterFirstBy(function (record) {
            return value == record.get(field);
        });

        return list;
    },


    getById: function (id) {
        "use strict";
        var me = this,
            list,
            data = me.data;

        list = data.filterFirstBy(function (record) {
            return id == record.getId();
        });

        return list;
    },



    //更新记录值
    // dirty: true=弄脏， false=正常状态
    setRecordValue: function (recordId, field, value, dirty) {
        "use strict";
        var me = this,
            log = me.log,
            record;


        record = me.getById(recordId);

        
        if (record) {

            record.set(field, value);

            //log.debug("setRecordValue(...) field=" + field + ", dirty=", dirty);
            
            if (dirty !== true) {
                record.commit(null, [field]);
            }
        }
        else {
            //console.error('没找到主键对应的实体。主键值=', recordId);
        }
    },


    getModifiedRecords: function () {

        return [].concat(this.getNewRecords().toArray(), this.getUpdatedRecords().toArray());
    },


    //排序
    sort: function (sortText) {
        "use strict";
        var me = this;

        me.sortText = sortText;

        if (!me.requestOptions) {

            var page = me.currentPage,
                pageSize = me.pageSize,
                op;

            op = Mini2.apply({
                action: 'read',
                //filters: me.filters.items,    //过滤
                sorters: me.getSorters()
            }, {});

            op.page = page;
            op.start = page * pageSize;
            op.limit = pageSize;
            op.end = op.start + op.limit;

            me.requestOptions = op;
        }

        me.requestOptions.sorters = me.getSorters();


        var actId = me.clientId + '_Action';
        var actEl = $('#' + actId);

        var json = me.getRequestJson();
        actEl.val(json);

        me.loadPage(0);
    },

    //触发排序事件
    doSort: function (sorterFn) {
        "use strict";
        var me = this;

        if (me.remoteSort) {
            //the load function will pick up the new sorters and request the sorted data from the proxy
            me.load();
        }
        else {
            me.data.sortBy(sorterFn);
            me.onDataChanged();
            me.on('refresh', me);
        }

        me.on('sort', me, me.sorters.getRange());
    },

    //设置只读
    setReadOnly: function (value) {
        var me = this;

        me.readOnly = value;

        return me;
    },

    getReadOnly: function () {
        var me = this;

        return !!me.readOnly;
    },

    /**
    * 刷新实体
    *
    **/
    refresh: function () {
        var me = this;

        me.on('refresh', me);

        return me;
    },


    /**
    * 获取分页时使用的总数量 
    * @return {int}
    **/
    getTotalCount: function () {
        return this.totalCount;
    },

    setTotalCount: function (value) {
        this.totalCount = value;
    },

    /**
    * 获取当前显示的记录数量
    *
    **/
    getCount: function () {
        var me = this;
        return me.data.getCount();
    },


    each: function (fn, scope) {
        "use strict";
        var data = this.data.items,
            len = data.length,
            record, i;

        for (i = 0; i < len; i++) {
            record = data[i];
            if (fn.call(scope || record, record, i, len) === false) {
                break;
            }
        }

    },


    //创建记录实体
    createModel: function (record) {
        "use strict";
        //log.begin("Store.createModel()");
        var me = this;

        if (!record.isModel) {
            if (!me.model ) {
                throw new Error('Store.Model 不能等于 null');
            }

            record = Mini2.ModelManager.create(record, me.model);
        }

        //log.end();


        return record;
    },


    //插入记录
    insert: function (index, records, triggerEvent) {
        "use strict";
        var me = this,
            sync = false,
            i, len, record,
            defaults = me.modelDefaults,
            out;

        if (Mini2.isArray(records)) {
            out = records;
        }
        else {
            out = [records];
        }

        records = out;

        len = records.length;

        for (i = 0; i < len; i++) {
            record = records[i];

            if (!record.isModel) {

                try{
                    record = me.createModel(record);
                }
                catch (ex) {
                    throw new Error('创建 record 实体错误.'+ ex.message);
                }
            }

            record.joinStore(me);


            out[i] = record;
            if (defaults) {
                record.set(defaults);
            }


            //record.join(me);

            sync = sync || record.phantom === true;


            me.data.insert(index + i, record);

            if (me.curLoadData) {
                me.curLoadData.push(record);
            }
        }



        if (undefined === triggerEvent || true === triggerEvent) {

            me.updateRecordsIndex();

            me.on('add', [index, out]);


            me.onDataChanged();
        }

        return me;
    },


    /**
     * 递增模式
     */
    isIncr: false,

    incrPage: 0,

    /**
     * 最后一页
     */
    isLastPage:function(){

        var me = this,
            data = me.data,
            totalCount = me.totalCount;

        if (data.length >= totalCount) {
            return true;
        }

        return false;

    },

    nextPage: function(result){

        "use strict";
        var me = this,
            log = me.log,
            op;
        

        me.incrPage++;
        
        me.loadPage(me.incrPage);


        return me;

    },


    //按页码加载数据集
    loadPage: function (page) {
        "use strict";
        var me = this,
            log = me.log,
            op;

        if (undefined == page) {
            page = me.currentPage;
        }

        op = Mini2.apply({
            action: 'read',
            //filters: me.filters.items,
            sorters: me.getSorters()
        }, {});

        op.page = page;
        op.start = page * me.pageSize;
        op.limit = me.pageSize;
        op.end = op.start + op.limit;

        me.requestOptions = op;

        console.debug('---------- op ', op);

        var widget = window.widget1;

        if (widget && widget.subMethod) {

            widget.subMethod('form:first', {
                subName: me.id,
                subMethod: 'LoadPage',
                actionPs: page
            });

        }

        return me;
    },

    //获取排序的字段
    getSorters: function () {
        /// <summary>获取排序参数</summary>
        var me = this;

        return me.sortText;
    },


    getJsonForRecord: function (record) {
        "use strict";
        var me = this,
            srcData,
            i;

        var recObj = {
            action: "none",
            id: record.getId(),
            clientId: record.muid,
            index: me.currentIndex,
            values: {}
        };


        if (record.dirty) {
            recObj.action = 'modified';
        }

        srcData = record.data;

        for (i in srcData) {
            if ('_ownerTreeNode' == i) {
                continue;
            }
            
            recObj.values[i] = srcData[i];
        }

        return recObj;
    },


    //按修改过的记录 json
    getJsonForRecordModified: function (record, i) {
        "use strict";
        var me = this,
            j,
            recObj;

        recObj = {
            action: 'none',
            id: record.getId(),
            clientId: record.muid,
            index: i,
            values: {}
        };

        if (record.dirty) {
            recObj.action = 'modified';
        }

        for (j in record.modified) {
            recObj.values[j] = record.get(j);
        }

        return recObj;
    },




    getRequestJson: function () {
        "use strict";
        /// <summary>获取请求的json参数</summary>

        var me = this;



        var jsonStr = Mini2.Json.toJson(me.requestOptions);


        return jsonStr;
    },



    getUploadJson: function () {
        "use strict";
        /// <summary>获取提交 JSON 记录数据</summary>

        var me = this,
            log = me.log,
            record,
            i,
            recObj,
            jsonStr,
            jsonObj,
            records = me.data,
            len = records.getCount();

        //var reqJson = me.getRequestJson();

        jsonObj = {
            cur_index: me.currentIndex,
            current: null,    //当前焦点记录
            records: []     //记录状态
        };


        try {
            if (me.current && me.currentIndex > -1) {
                record = me.current;
                jsonObj.current = me.getJsonForRecord(record);
            }


            for (i = 0; i < len; i++) {
                record = records.get(i);

                if ('full' == me.jsonMode) {
                    recObj = me.getJsonForRecord(record);
                }
                else {
                    if (!record.dirty) {
                        continue;
                    }
                    recObj = me.getJsonForRecordModified(record, i);
                }

                jsonObj.records.push(recObj);
            }
        }
        catch (ex) {

            log.error('Store.getUploadJson(...) 执行错误。' + ex.message);

            throw new Error('Store.getUploadJson(...) 执行错误。' + ex.message);
        }

        try {
            jsonStr = Mini2.Json.toJson(jsonObj);
        }
        catch (ex) {
            console.error("getUpladJson ", jsonObj);

            log.error('执行 Mini2.Json.toJson(...) 错误', ex.message);
            throw new Error('执行 Mini2.Json.toJson(...) 错误', ex.message);
        }


        return jsonStr;
    },


    //页面加载结束后，触发的事件
    onLoader: function () {
        var me = this,
            log = me.log;

        log.debug(me.id + '  onLoader()');

        if (me.autoFocus) {
            log.debug('自动设置焦点记录。');
            me.setCurrent(0);
        }
    }



}, function () {
    "use strict";
    var me = this,
        log = me.log,
        args = arguments;
    
    if (args && args[0]) {
        Mini2.apply(this, args[0]);
    }

    me.preInit();
});