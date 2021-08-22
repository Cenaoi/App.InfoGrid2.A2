/// <reference path="../Mini.js" />
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />


//数据实体
Mini2.define('Mini2.data.Model', {



    //别名：数据记录
    alternateClassName: ['Mini2.data.Record'],

    persistenceProperty: 'data',

    log: Mini2.Logger.getLogger('Mini2.data.Model', false),  //日志管理器

    emptyData: [],

    idField: [],

    //锁的字段名称
    //lockedField:'',

    idProperty: 'id',

    dataSave: false,

    evented: false,

    isModel: true,

    editing: false,

    //已经修改
    //dirty: false,

    //新创建，但未保存
    //phantom: false,

    //锁行规则
    //lockedRule:false,

    //字段集合 []
    fields: false,
    
    //字段模式: fixed=固定模式, free=自由模式
    //固定模式,不允许没有字段的实体保存
    fieldModeId: 'fixed',

    modified: false,


    statics: {

        PREFIX: 'Mini-record',

        AUTO_ID: 1,

        EDIT: 'edit',

        REJECT: 'reject',

        COMMIT: 'commit',

        id: function (rec) {

        }
    },

    //构造函数
    //raw = 原数据,可不填写
    onInit: function (data, id, raw, convertedData) {
        "use strict";
        var me = this,
            log = me.log,
            passedId = (id || id === 0),
            hasId,
            fields,
            len,
            field,
            name,
            value,
            newId,
            persistenceProperty,
            idProperty = me.idProperty,
            idField = me.constructor.idField,
            lockedField = me.constructor.lockedField,
            i;

        me.id = me.muid;

        me.raw = raw || data;
        me.modified = {};

        me.data = data;
        me.dataSave = {};

        me.stores = [];


        fields = me.fields;

        for (i = 0, len = fields.length; i < len; i++) {
            field = new Mini2.data.Field(fields[i]);

            fields[i] = field;
        }

        me.fields = new Mini2.collection.ArrayList(fields);

        me.idField = idField;

        me.lockedField = lockedField;
        //log.end();

        me.initProperty();
    },


    /**
    * 把字段名映射为属性
    */
    initProperty: function () {
        var me = this,
            log = me.log,
            field,
            fields = me.fields,
            i,
            len = fields.length;

        if (Object.defineProperty) {

            for (i = 0; i < len; i++) {
                field = fields.get(i);

                if (!Mini2.isString(field.name) ) {
                    log.debug('无法自定义属性的字段: ', field);
                    continue;
                }

                if (Mini2.String.isBlank(field.name)) {
                    log.debug('无法自定义空字段的属性: ', field);
                    continue;
                }

                if (me[field.name]) {
                    continue;
                }

                try{
                    (function () {
                        var name = field.name;
                        Object.defineProperty(me, name, {
                            get: function () {
                                return me.get(name)
                            },
                            set: function (value) {
                                me.set(name, value);
                            }
                        });
                    })();

                }
                catch (ex) {
                    //console.error('自定义属性错误: ', field.name, ex);
                    //console.debug('自定义属性: ', field);
                }
            }
        }
        else {
            console.error("浏览器不支持 Object.defineProperty 函数.");
        }
    },

    inheritableStatics: {

        setFields: function (fields, idProperty, clientIdProperty) {

        },

        getFields: function () {
            return this.prototype.fields.items;
        }
    },

    _singleProp: {},

    isEqual: function (a, b) {
        "use strict";
        if (a instanceof Date && b instanceof Date) {
            return a.getTime() === b.getTime();
        }
        return a === b;
    },

    setFields: function (fields, idProperty, clientIdProperty) {
        "use strict";
        var me = this,
            log=  me.log,
            newField,
            idField,
            idFieldDefined = false,
            proto = me.prototype,
            prototypeFields = proto.fields,
            superFields = proto.superClass.fields,
            len,
            i;



        if (idProperty) {
            proto.idProperty = idProperty;
            idField = idProperty.isField ? idProperty : new Mini2.data.Field(idProperty);

        }
        if (clientIdProperty) {
            proto.clientIdProperty = clientIdProperty;
        }

        if (prototypeFields) {
            prototypeFields.clear();
        }
        else {
            prototypeFields = me.prototype.fields = new Mini2.collection.ArrayList();
        }


        if (superFields) {
            fields = superFields.items.concat(fields);
        }


        for (i = 0, len = fields.length; i < len; i++) {
            newField = new Mini2.data.Field(fields[i]);

            if (idField && ((newField.mapping && (newField.mapping === idField.mapping)) || (newField.name === idField.name))) {
                idFieldDefined = true;
                newField.defaultValue = undefined;
            }
            prototypeFields.add(newField);
        }


        if (idField && !idFieldDefined) {
            idField.defaultValue = undefined;
            prototypeFields.add(idField);
        }

        me.fields = prototypeFields;

        return prototypeFields;
    },

    //获取主键值
    getId: function () {
        "use strict";
        var me = this,
            idField = me.idField,
            id;

        if (!idField) {
            return null;
        }

        id = me.get(idField.name);

        if (id == undefined) {
            id = null;
        }

        return id;
    },

    get: function (field) {
        return this[this.persistenceProperty][field];
    },


    baseSet: function(fieldName, newValue){
        "use strict";
        var me = this,
            log = me.log,
            data = me[me.persistenceProperty],
            fieldModeId = me.fieldModeId,   // fixed=固定, free=自由
            fields = me.fields,
            modified = me.modified,
            single = Mini2.isString(fieldName),
            currentValue, field, idChanged, key, modifiedFieldNames, name, oldId,
            newId, value, values;

        if (undefined == fieldName) {
            throw new Error("没有指定字段名，无法对实体进行赋值");
        }


        if (single) {
            values = me._singleProp;
            values[fieldName] = newValue;
        }
        else {
            values = fieldName;
        }

        //log.debug("model : " + fieldName + " = " + newValue);

        for (name in values) {


            if (!values.hasOwnProperty(name)) {

                if ('fixed' == fieldModeId) {
                    continue;
                }
            }


            value = values[name];
            field = (fields ? fields.getByProp('name', name) : null);

            if (fields && field && field.convert) {
                value = field.convert(value, me);

                //                    log.debug("转换数据");
            }


            currentValue = data[name];

            //字符串会有以下问题。
            if ((currentValue == '' && value == null) ||
                (currentValue == null && value == '')) {
                continue;
            }

            if (me.isEqual(currentValue, value)) {
                continue; // new value is the same, so no change...
            }

            //me[name] = value;
            data[name] = value;
            (modifiedFieldNames || (modifiedFieldNames = [])).push(name);

            //log.debug("赋值: " + name + " = " + value);

            if (field && field.persist) {
                if (modified.hasOwnProperty(name)) {
                    if (me.isEqual(modified[name], value)) {

                        delete modified[name];

                        me.dirty = false;

                        for (key in modified) {
                            if (modified.hasOwnProperty(key)) {
                                me.dirty = true;
                                break;
                            }
                        }
                    }
                }
                else {
                    me.dirty = true;
                    modified[name] = currentValue;
                }
            }

            //log.debug("me.dirty = " + me.dirty);

        }

        if (single) {
            // cleanup our reused object for next time... important to do this before
            // we fire any events or call anyone else (like afterEdit)!
            delete values[fieldName];
        }

        //if (!me.editing) {
        //    log.debug('实体赋值失败:没有开启编辑状态 Model.beginEdit() 。');
        //}

        //log.debug("editing = " + me.editing + ", " + modifiedFieldNames + " = " + value);

        if (!me.editing && modifiedFieldNames) {
            //log.debug('触发 afterEdit 事件.', modifiedFieldNames);
            me.afterEdit(modifiedFieldNames);
        }

        if (me.editing && modifiedFieldNames) {
            //log.debug("触发 afterCommit 事件.", modifiedFieldNames);
            me.afterCommit(modifiedFieldNames);
        }


        return modifiedFieldNames || null;

    },

    set: function (fieldName, newValue) {
        var me = this;

        return me.baseSet(fieldName, newValue);
    },


    //调用数据仓库的某个函数
    callStore: function (fn) {
        "use strict";
        var args = [],
            stores = this.stores,
            i,
            j,
            len = stores.length,
            aLen = arguments.length,
            store;

        for (j = 0; j < aLen; j++) {
            args[j] = arguments[j];
        }

        args[0] = this;

        for (i = 0; i < len; ++i) {
            store = stores[i];

            if (store && Mini2.isFunction(store[fn])) {

                store[fn].apply(store, args);
            }
        }

    },

    //出现异常，产生的事件
    afterInvalid: function (fieldNames) {
        this.callStore('afterInvalid', fieldNames);
    },


    //修改后触发的事件
    //modifiedFieldNames : 修改的字段名集合
    afterEdit: function (modifiedFieldNames) {
        this.callStore('afterEdit', modifiedFieldNames);
    },

    //丢弃修改触发的事件
    afterReject: function () {
        this.callStore('afterReject');
    },

    //保存修改触发的事件
    //modifiedFieldNames : 修改的字段名集合
    afterCommit: function (modifiedFieldNames) {
        this.callStore('afterCommit', modifiedFieldNames);
    },


    //执行锁行规则。
    // 返回 true = 锁行,false = 不锁行
    execLockedRule: function () {
        "use strict";
        var me = this,
            lockedRule = me.lockedRule,
            result = false;


        if (lockedRule && lockedRule != '') {

            if (Mini2.isString(lockedRule)) {
                
                try {
                    result = eval(lockedRule);
                }
                catch (ex) {
                    log.debug('执行锁行规则错误:' + lockedRule);

                    throw new Error('执行锁行规则错误:' + lockedRule);
                }
            }
            else if (Mini2.isFunction(lockedRule)) {

                try {
                    result = lockedRule.call(me);
                }
                catch (ex) {
                    log.debug('执行锁行规则错误:' + lockedRule);

                    throw new Error('执行锁行规则错误:' + lockedRule);
                }

            }
        }

        return result;
    },


    //是否已经是锁住整个实体
    isLocked: function () {
        "use strict";
        var me = this,
            log = me.log,
            value,
            lockedField = me.lockedField,
            store = me.store;

        if (store && store.readOnly) {

            //log.debug("isLocked() ---- store.readOnly ", store.readOnly);
                       

            return true;
        }


        try {
            
            if (me.execLockedRule()) {

                //log.debug("isLocked() ---- me.execLockedRule() ", me.execLockedRule());

                return true;
            }
        }
        catch (ex) {
            console.error('执行锁行代码错误.' + ex.Message, ex);
        }


        if (lockedField) {
            value = me.get(lockedField);

            //log.debug("isLocked() ---- me.lockedField() ", value);


            return !!value;
        }
        //        if (me.get('COL_14') == '已收款') {
        //            return true;
        //        }

        return false;
    },

    //锁后, 排除的字段集合
    isLockedExclusionField:function(fieldName){
        var me = this,
            store = me.store,
            fs = store.lockedExclusionFields;

        if (fs) {

            var exist = Mini2.Array.contains(fs, fieldName);

            if (exist) {
                return true;
            }
        }

        return false;
    },


    //字段值是否已经锁住
    isLockedForField: function (fieldName) {
        var me = this;
        
        return false;
    },

    //字段是否无效，配合 UI 使用。
    isDisabledForField:function(fieldName){
        return false;
    },



    //开始编辑
    beginEdit: function () {
        "use strict";
        var me = this,
            key,
            data,
            o;


        //log.begin("Model.beginEdit();   " + me.editing);


        if (!me.editing) {
            me.editing = true;
            me.dirtySave = me.dirty;

            o = me[me.persistenceProperty];
            data = me.dataSave = {};
            for (key in o) {
                if (o.hasOwnProperty(key)) {
                    data[key] = o[key];
                }
            }

            o = me.modified;
            data = me.modifiedSave = {};
            for (key in o) {
                if (o.hasOwnProperty(key)) {
                    data[key] = o[key];
                }
            }
        }

        //log.end();
    },


    //取消编辑
    cancelEdit: function () {
        "use strict";
        var me = this,
            log = me.log;


        //log.debug("Mode.cancelEdit();   " + me.editing);

        if (me.editing) {
            me.editing = false;
            // reset the modified state, nothing changed since the edit began
            me.modified = me.modifiedSave;
            me[me.persistenceProperty] = me.dataSave;
            me.dirty = me.dirtySave;
            me.modifiedSave = me.dataSave = me.dirtySave = null;
        }
    },


    //结束编辑
    endEdit: function (silent, modifiedFieldNames) {
        "use strict";
        var me = this,
            dataSave,
            changed;

        //        log.begin("Model.endEdit();   " + me.editing);

        silent = silent === true;
        if (me.editing) {
            me.editing = false;
            dataSave = me.dataSave;
            me.modifiedSave = me.dataSave = me.dirtySave = null;
            if (!silent) {
                if (!modifiedFieldNames) {
                    modifiedFieldNames = me.getModifiedFieldNames(dataSave);
                }
                changed = me.dirty || modifiedFieldNames.length > 0;
                if (changed) {
                    me.afterEdit(modifiedFieldNames);

                }
            }
        }

        //        log.end();
    },

    //获取修改过的字段名集合
    getModifiedFieldNames: function (saved) {
        "use strict";
        var me = this,
            data = me[me.persistenceProperty],
            modified = [],
            key;

        saved = saved || me.dataSave;
        for (key in data) {
            if (data.hasOwnProperty(key)) {
                if (!me.isEqual(data[key], saved[key])) {
                    modified.push(key);
                }
            }
        }
        return modified;
    },



    getChanges: function () {
        "use strict";
        var modified = this.modified,
            changes = {},
            field;

        for (field in modified) {
            if (modified.hasOwnProperty(field)) {
                changes[field] = this.get(field);
            }
        }

        return changes;
    },


    //丢弃修改
    reject: function (silent) {
        "use strict";
        var me = this,
            modified = me.modified,
            field;

        for (field in modified) {
            if (modified.hasOwnProperty(field)) {
                if (typeof modified[field] != "function") {
                    me[me.persistenceProperty][field] = modified[field];
                }
            }
        }

        me.dirty = false;
        me.editing = false;
        me.modified = {};

        if (silent !== true) {
            me.afterReject();
        }
    },


    //设为正常状态
    commit: function (silent, modifiedFieldNames) {
        "use strict";
        var me = this,
            modified = me.modified;

        me.phantom = me.dirty = me.editing = false;

        //me.modified = {};

        if (modified && modifiedFieldNames) {

            for (var i = 0; i < modifiedFieldNames.length; i++) {
                var field = modifiedFieldNames[i];

                if (modified.hasOwnProperty(field)) {
                    delete modified[field];
                }
            }

        }

        if (true !== silent) {

            me.afterCommit(modifiedFieldNames);
        }
    },


    //判断字段已修改
    isModified: function (fieldName) {
        "use strict";
        return this.modified.hasOwnProperty(fieldName);
    },


    //关联数据仓库对象
    joinStore: function (store) {
        "use strict";
        /// <summary>连接数据仓库，作为联动用。</summary>

        var me = this;

        // Code for the 99% use case using fast way!
        if (!me.stores.length) {
            me.stores[0] = store;
        } else {
            Mini2.Array.include(this.stores, store);
        }

        /**
        * @property {Ext.data.Store} store
        * The {@link Ext.data.Store Store} to which this instance belongs. NOTE: If this
        * instance is bound to multiple stores, this property will reference only the
        * first. To examine all the stores, use the {@link #stores} property instead.
        */
        this.store = this.stores[0]; // compat w/all releases ever
    },


    //断开关联数据仓库对象
    unjoinStore: function (store) {
        "use strict";
        Mini2.Array.remove(this.stores, store);
        this.store = this.stores[0] || null; // compat w/all releases ever
    },


    //错误信息
    errors: false,



    //创建异常信息
    //消息格式 {field:'字段名', message:'消息', level:'ALL'}
    markInvalid: function (error) {
        "use strict";
        var me = this,
            errors = me.errors,
            items,
            item,
            field = error.field,
            fields = [field];

        if (!errors) {
            me.errors = errors = {
                length: 0,
                items: {}
            };
        }

        items = errors.items;
        item = items[field];

        if (!item) {
            items[field] = item = [];
        }

        item.push(error);

        errors.length++;

        try {
            me.afterInvalid(fields);
        }
        catch (ex) {
            console.error("调用错误: Model.afterInvalid(error)\n" + ex.Message,ex);
        }
    },


    /**
    * 清理全部异常信息
    **/
    clearInvalidAll: function () {
        var me = this;

        if (me.errors) {
            
            var fields = [];

            for (var field in me.errors) {
                fields.push(field);
            }

            delete me.errors;

            me.afterInvalid(fields);
        }

    },

    //清理异常信息
    clearInvalid: function (field) {
        "use strict";
        var me = this,
            errors = me.errors;

        if (errors) {
            if (field) {
                delete errors.items[field];

                me.afterInvalid(fields);
            }
            else {
                delete me.errors;
                me.errors = false;

                me.afterInvalid(fields);
            }
        }
    },

    //按字段获取错误消息集合
    getErrors: function (fieldName) {
        "use strict";
        var me = this,
            errors = me.errors,
            items,
            error;

        try {
            if (errors) {
                items = errors.items;
                error = items[fieldName];
            }
        } catch (ex) {
            alert(ex.Message);
        }

        return error;
    },


    //验证数据
    isValid: function () {
        return this.validate().isValid();
    },



    //验证
    validate: function () {
        "use strict";
        var me = this,
            errors = new Mini2.data.Errors(),
            validations = me.validations,
            validators = Mini2.data.validations,
            length, validation, field, valid, type, i;


        if (validations) {
            length = validations.length;

            for (i = 0; i < length; i++) {
                validation = validations[i];
                field = validation.field || validation.name;
                type = validation.type;
                valid = validators[type](validation, me.get(field));

                if (!valid) {
                    errors.add({
                        field: field,
                        message: validation.message || validators[type + 'Message']
                    });
                }
            }
        }

        return errors;
    },

    //释放内存
    dispose: function () {
        var me = this;

        if (!me.isDisposed) {
            me.isDisposed = true;

            //delete me.data;
            //delete me.raw;
            //delete me.fields;
            //delete me.modified;
            //delete me.idField;
            //delete me.lockedField;
            //delete me.dataSave;
            //delete me.stores;

        }

    }

}, function () {
    var me = this;

    me.muid = Mini2.newId();

    //Mini2.apply(me, arguments[0]);
    me.onInit(arguments[0]);


});


//自由实体
Mini2.define('Mini2.data.FreeModel', {

    extend: 'Mini2.data.Model',

    fieldModeId: 'free'

        
});

Mini2.ModelManager.registerType('Mini2.data.FreeModel', Mini2.create('Mini2.data.FreeModel'));


Mini2.define('Mini2.data.ModelX', {

    extend: 'Mini2.data.Model',
    

    //lockedRule:'',


    //字段值是否已经锁住
    isLockedForField: function (fieldName) {
        var me = this;

        //if ('COL_4' == fieldName) {

        //    var v = me.get('COL_2');

        //    if (v == '123') {
        //        return true;
        //    }
        //}

        return false;
    },

    //字段是否无效，配合 UI 使用。
    isDisabledForField: function (fieldName) {
        var me = this;

        if ('COL_4' == fieldName) {

            if (me.get('COL_2') == '123') {
                return true;
            }

        }

        return false;
    },


    //设置字段值
    set: function (fieldName, newValue) {
        var me = this,
            result ;

        result = me.baseSet(fieldName, newValue);



        return result;
    }

});