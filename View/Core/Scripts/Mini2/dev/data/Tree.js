/// <reference path="../Mini.js" />

/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="StoreManager.js" />
/// <reference path="../lang/Array.js" />


Mini2.define('Mini2.data.Tree', {

    root: null,

    groupBuffer: false,

    PID_EX:'PID_',

    constructor: function (root) {
        var me = this,
            data = me.data;

        if (root) {
            me.root = root;
        }

        me.groupBuffer = {};

        if (data) {
            me.initGroupBuffer(data);

            delete me.data;
        }

    },

    //把并行数据转换为树结构
    initGroupBuffer:function(data){
        var me = this,
            i,
            record,
            len = data.length;


        for (i = 0; i < len; i++) {
            record = data.get(i);

            me.insertNode(record);


        }
    },


    removeNode:function(record){
        var me = this,
            pValue,
            pField = me.parentField,
            group = me.groupBuffer,
            records,
            tmpRec,
            i,id;


        pValue = 'PID_' + record.get(pField);

        records = group[pValue];

        if (records) {
            
            id = record.getId();

            for (i = 0; i < records.length; i++) {

                tmpRec = records[i];

                if (tmpRec.isNode) {
                    tmpRec = tmpRec.data;
                }

                if (tmpRec.getId() == id) {
                    records.splice(i, 1);

                    break;
                }

            }
        }

    },

    insertNode:function(record){
        var me = this,
            pValue,
            pField = me.parentField,
            tmpRecords,
            group = me.groupBuffer
            ;

        pValue = 'PID_' + record.get(pField);

        tmpRecords = group[pValue];

        if (!tmpRecords) {
            tmpRecords = [];
            group[pValue] = tmpRecords;
        }


        if (!record.isNode) {
            record = Mini2.create('Mini2.data.TreeModel', {
                model: me.model,
                data: record
            });
        }

        tmpRecords.push(record);
    },

    //根据上级Id，获取子节点集合
    getChilds:function(parentId){
        var me = this,
            pValue = 'PID_' + parentId,
            group = me.groupBuffer,
            records;

        records = group[pValue];

        return records;
    },



    getRootNode: function () {
        return this.root;
    },

}, function () {
    var me = this;

    me.muid = Mini2.getIdentity();

    Mini2.apply(me, arguments[0]);
    me.constructor(arguments[0]);


});