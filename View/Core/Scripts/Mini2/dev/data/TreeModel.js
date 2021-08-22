/// <reference path="../Mini.js" />

/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="StoreManager.js" />
/// <reference path="../lang/Array.js" />



Mini2.define('Mini2.data.TreeModel', {

    //用户数据
    data: null,

    isNode: true,

    //下级节点
    childs: null,

    //上级节点
    parentNode: null,

    //上级ID
    parentId: null,
    index:0,

    //深度
    depth: 0,

    //展开
    expanded: false,

    //叶
    leaf: false,

    //是否最后节点
    isLast: false,
    isFirst:false,

    //加载完成
    loaded: false,
    loading: false,

    hasChild: false,

    //复选框
    checked: false,

    cls: '',
    icon: '',
    iconCls: '',


    model:false,

    onInit: function () {
        var me = this,
            data = me.data,
            userData,   //用户数据
            nodeAttrs    //节点属性;

        me.attrs = {};
        me.childs = [];

        if (data.isNode) {
            userData = data.data;

            delete data.data;

            nodeAttrs = data;
        }
        else {
            userData = data;

            nodeAttrs = data;
        }

        if (!userData.isModel) {
            userData = Mini2.ModelManager.create(userData, this.model);
        }

        me.data = userData;

        Mini2.apply(me.attrs, nodeAttrs);

    },


    addNode:function(parentId, node){
        var me = this;

        

    },

    getAttr: function (attr) {
        var me = this,
            attrs = me.attrs;

        return attrs[attr];
    },

    setArrt: function (attr, value) {
        var me = this,
            attrs = me.attrs;

        attrs[attr] = value;

        return me;
    }

}, function () {
    var me = this;

    me.muid = Mini2.newId();

    Mini2.apply(me, arguments[0]);
    me.onInit(arguments[0]);


});