/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />
/// <reference path="../../../../jstree/3.0.2/jstree.js" />

//焦点管理
Mini2.define('Mini2.ui.tree.Panel', {

    extend: 'Mini2.ui.Component',


    log: Mini2.Logger.getLogger('Mini2.ui.tree.Panel', console),

    treeCls: Mini2.baseCSSPrefix + 'tree-panel',


    rootId: '0',

    store: false,

    rowLines: false,

    lines: true,

    rootVisible: true,

    displayField: 'text',

    //显示复选框
    checkbox: false,

    /**
     * 节点渲染加工器
     *
     */
    nodeRenderer: null,

    el: null,

    data: false,

    showMenu: true,
    showMenuDelete: true,
    showMenuNew: true,
    showMenuRename: true,


    valueField: 'value',

    textField: 'text',

    //父节点字段名
    parField: 'par_id',


    typeField: 'type',

    iconField: 'icon',

    autoExpand: true,


    /**
    * 节点类型名称
    */
    nodeTypeText: '分类',

    rootText: '分类目录',

    /**
    * 动态加载
    */
    dymLoad: true,

    /**
    * 显示根节点
    */
    showRoot: true,


    //移动节点的 Json 数据
    //moveNodeJson : {},


    //允许拖拉
    //allowDragDrop :false,

    /*自定义菜单*/
    contextMenu: [],


    //焦点节点
    focusNode: null,


    //触发创建事件
    triggerEvent_create: false,

    //触发重命名事件
    triggerEvent_rename: false,

    //触发删除事件
    triggerEvent_remove: false,


    //触发服务器事件
    triggerEvent_selected: false,


    //节点选择事件
    nodeSelected: function (fun) {
        this.bind('nodeSelected', fun);
    },

    //节点打开
    nodeExpand: function (fun) {
        this.bind('nodeExpand', fun);
    },


    //节点选择事件
    nodeRename: function (fun) {
        this.bind('nodeRename', fun);
    },


    /**
    * @cfg {Mini2.data.Model/Ext.data.NodeInterface/Object} root
    * Allows you to not specify a store on this TreePanel. This is useful for creating a simple tree with preloaded
    * data without having to specify a TreeStore and Model. A store and model will be created and root will be passed
    * to that store. For example:
    *
    *     Mini2.create('Mini2.ui.tree.Panel', {
    *         title: 'Simple Tree',
    *         root: {
    *             text: "Root node",
    *             expanded: true,
    *             children: [
    *                 { text: "Child 1", leaf: true },
    *                 { text: "Child 2", leaf: true }
    *             ]
    *         },
    *         renderTo: Mini2.getBody()
    *     });
    */
    root: null,

    isTree: true,

    initComponent: function () {
        "use strict";
        var me = this;

        console.debug("初始化 TreePanel.initComponent()");

        if (me.id && undefined == me.clientId) {
            me.clientId = me.id;
        }

        if (me.selected) {
            me.nodeSelected(me.selected);

            delete me.selected;
        }

        if (me.expand) {
            me.nodeExpand(me.expand);

            delete me.expand;
        }

        if (me.rename) {
            me.nodeRename(me.rename);
            delete me.rename;
        }


        var nodeRenderer = me.nodeRenderer;
        if (nodeRenderer) {

            if (Mini2.isString(nodeRenderer)) {

                try {
                    var funStr = '(function(e, node){' + nodeRenderer + '})';
                    var fun = eval(funStr);

                    console.debug('fun = ', fun);

                    delete me.nodeRenderer;
                    me.nodeRenderer = fun;
                }
                catch (ex) {
                    console.error('编译动态脚本 nodeRenderer 函数错误:' + funStr);
                }
            }

        }

    },


    setSize: function (w, h) {
        var me = this,
            el = me.el;

        if (undefined != w) {
            el.css('width', w);
        }

        if (undefined != h) {
            el.css('height', h);
        }

    },


    css: function (styleName, styleValue) {
        var me = this,
            el = me.el;

        return el.css(styleName, styleValue);
    },


    isVisible: function () {
        var me = this,
            el = me.el;

        return el.is(':visible');
    },


    getStore: function () {

        var me = this;

        if (!me.store) {
            var newStore = Mini2.create('Mini2.data.Store', {
                storeId: "MiniStore_" + Mini2.getIdentity()
            });

            me.bindStore(newStore);

        }

        return me.store;
    },


    initStore: function () {
        "use strict";
        var me = this,
            store;


        if (me.store) {

            console.info('开始初始化数据仓库 TreePanel.initStore().');

            store = Mini2.data.StoreManager.lookup(me.store);

            //console.debug("store", store);

            delete me.store;

            me.store = store;

            me.bindStore(store);

            me.proLineData(store.data);

        }


    },




    /** region Store **/

    bindStore: function (store) {

        var me = this;

        $(store)
            .muBind('update', me, me.store_update)
            .muBind('add', me, me.store_add)
            .muBind('remove', me, me.store_remove)
        //            .muBind('datachanged', me, me.store_dataChanged)
        //            .muBind('refresh', me, me.store_refresh)
            .muBind('load', me, me.store_load)
        //            .muBind('bulkremove', me, me.store_bulkremove)
            .muBind('clear', me, me.store_clear);
        //            .muBind('currentchanged', me, me.store_currentchanged);



    },

    store_remove: function (event, record) {

        var me = event.data,
            log = me.log,
            delFun = me['delete'];

        for (var i = 0; i < record.length; i++) {

            var id = record[i];

            var nodeId = me.id + '_NODE_' + id;
                        
            delFun.call(me, nodeId);

        }



    },

    store_update: function (event, record, operation, modifiedFieldNames) {

        var me = event.data,
            log = me.log;

        log.debug('TreeStore 触发 Update 事件');

    },


    store_clear: function (event) {
        var me = event.data,
            log = me.log;


        log.debug('TreeStore 触发 Clear 事件');

        me.clear();
    },

    /**
     * 处理行数据
     * @param records {Array} 记录集合
     */
    proLineData: function (records) {
        "use strict";
        var me = this,
            el = me.el,
            i,
            record,
            len = records.length,
            pField = me.parField,
            pValue,
            node,
            rootNode,
            newNodes = [],
            checkNodes = me.checkNodes;

        var list = records;

        var groups = me.toGroup(records);


        var pId = me.rootId + '';
        var list = groups[pId] || [];

        var srcEventSelected = me.event_selected;

        me.event_selected = false;


        if (me.showRoot && me.store) {
            var store = me.store;
            var rootRect = store.getById(pId);


            if (rootRect) {
                rootRect.expand = true;

                node = me.record_2_node.call(me, rootRect);
            }
            else {
                rootRect = {};
                rootRect[me.idField || me.valueField] = me.rootId;
                rootRect[me.textField] = me.rootText;
                rootRect.expand = true;


                node = me.record_2_node.call(me, rootRect);

            }

            node.li_attr.child_loaded = true;

            me.add(node);
            me.proGroupsToTree.call(me, groups, rootRect, node, newNodes);

            if ($(el).jstree('is_closed', node)) {
                $(el).jstree("toggle_node", node);
            }

            rootNode = node;
        }
        else {

            if (list && list.length) {
                for (i = 0; i < list.length; i++) {
                    record = list[i];

                    node = me.record_2_node.call(me, record);

                    //console.debug("node = ", node);

                    me.add(node);

                    me.proGroupsToTree.call(me, groups, record, node, newNodes);
                }
            }
        }


        if (me.checkbox && checkNodes) {

            i = checkNodes.length;

            while (i--) {
                checkNodes[i] = me.id + '_NODE_' + checkNodes[i];
            }

            me.checkNode(checkNodes);
            delete me.checkNodes;

            // 关闭第二层节点
            setTimeout(function () {

                var o = $(el).jstree('get_node', rootNode, true).children('.jstree-children');

                var openEls = $(o).children('.jstree-open');

                $(openEls).each(function () {
                    $(el).jstree('close_node', this, false);
                });

            }, 10);

        }

        me.event_selected = srcEventSelected;

        //特殊处理, 把叶节点设置为
        if (newNodes) {

            for (var i = 0; i < newNodes.length; i++) {

                var node = newNodes[i];

                //console.debug("node = ", node);

                if (node.has_child) {
                    var nodeEl = $('#' + node.id);
                    nodeEl.removeClass('jstree-leaf').addClass('jstree-closed');
                }
            }

        }

    },

    store_add: function (event, index, out) {
        "use strict";
        var me = event.data,
            i,
            record,
            records = out,
            len = records.length,
            pField = me.parField,
            pValue,
            node;

        var list = records;

        if (list && list.length) {
            for (i = 0; i < list.length; i++) {
                record = list[i];

                if (record.get) {
                    pValue = record.get(pField);
                } else {
                    pValue = record[pField];
                }

                node = me.record_2_node.call(me, record);


                if (pValue + '' == me.rootId + '') {
                    me.add(node.parent, node);
                }
                else {
                    me.add(node.parent, node);
                }


            }


        }

    },

    store_load: function (event, store) {
        "use strict";
        var me = event.data,
            log = me.log,
            el = me.el,
            tree = $(el).jstree('true'),
            i,
            record,
            newNodes = [],
            records = store.data;

        log.debug('TreeStore 触发 load 事件. ' + '共有 ' + records.length + '记录.');

        me.add_ForRecord(records, newNodes);


        if (me.focusNode) {

            $(el).jstree("toggle_node", me.focusNode.node);


            //特殊处理, 把叶节点设置为
            if (newNodes) {

                console.debug('特殊处理...', newNodes);

                for (var i = 0; i < newNodes.length; i++) {

                    var node = newNodes[i];


                    if (node.has_child) {

                        var nodeEl = $('#' + node.id);

                        nodeEl.removeClass('jstree-leaf').addClass('jstree-closed');
                    }
                }
            }

        }


    },

    /**
    *
    *
    * param {newNodeIds} 新节点 ID
    */
    proGroupsToTree: function (groups, parRecord, parNode, newNodes) {
        "use strict";
        var me = this,
            i,
            node,
            record,
            vField = me.idField || me.valueField,
            pValue,
            newId,
            list;


        var checkNodes = me.checkNodes;

        pValue = parRecord[vField] + '';

        list = groups[pValue];

        if (list && list.length) {

            for (i = 0; i < list.length; i++) {
                record = list[i];

                var id = record[vField].toString();


                node = me.record_2_node.call(me, record);

                newId = me.add(node.parent, node);

                if (newNodes) {
                    newNodes.push(node);

                    if (record.has_child) {
                        node.has_child = true;
                    }
                }

                me.proGroupsToTree(groups, record, node, newNodes);
            }
        }
    },


    //按父键值分组
    toGroup: function (records) {
        "use strict";
        var me = this,
            i,
            treeModel,
            record,
            len = records.length,
            pField = me.parField,
            pValue,
            group = {},
            list;

        console.debug("records", records);

        for (i = 0; i < len; i++) {

            if (records.isArray || records.get) {
                record = records.get(i);
            }
            else {
                record = records[i];
            }

            //console.debug("xxxxxxxxxxxxxxxxx", record);

            //console.debug("pField = ", pField);
            //console.debug("record = ", record);


            if ('tree-model' == record.role) {
                treeModel = record;
                record = record.data;
            }
            else {
                treeModel = record;
            }

            pValue = record[pField];

            //console.debug("pValue = ", pValue);

            if (undefined === pValue) {
                console.error('tree-model 的主键(' + pField + ')　,值不能为空.', record);
                continue;
            }

            pValue += '';

            list = group[pValue];

            if (undefined == list) {
                list = [];
                group[pValue] = list;
            }

            list.push(treeModel);

        }

        return group;
    },

    /**
     * 'record' 记录转换为 'node' 节点
     * @param record {Object} 记录数据
     * @param checked {bool} 选中状态.. 
     */
    record_2_node: function (record, checked) {
        "use strict";
        var me = this,
            vField = me.idField || me.valueField,
            tField = me.textField,
            pField = me.parField,
            typeField = me.typeField,
            v, t, p, type, icon, expand, tag,
            parId, id,
            node,
            treeModel,
            hasChild = true;

        if ('tree-model' == record.role) {
            hasChild = record.has_child;

            treeModel = record;
            record = treeModel;// record.data;
        }
        else {
            hasChild = record.has_child;

            if (!hasChild && undefined == record['child_loaded']) {
                hasChild = true;    //让其去执行脚本
            }


            treeModel = record;
        }

        if (record.isModel) {
            v = record.get(vField);
            t = record.get(tField);
            p = record.get(pField) || 'ROOT';

            expand = record.get('expand');

            type = record.get(typeField);
            icon = record.get(me.iconField);

            checked = checked || record.get('checked');

            tag = record.get('tag') || '';
        }
        else {
            v = record[vField];
            t = record[tField];
            p = record[pField] || 'ROOT';

            expand = record['expand'];

            type = record[typeField];
            icon = record[me.iconField];

            checked = checked || record['checked'];

            tag = record['tag'] || '';
        }

        checked = !!checked;


        parId = me.id + '_NODE_' + p;

        id = me.id + '_NODE_' + v;

        node = {
            id: id,
            text: t,

            //leaf: !hasChild,

            a_attr: {
                nodeId: v,
                parNodeId: p,
                tag: tag
            },
            parent: parId,
            li_attr: { 'child_loaded': !hasChild },
            state: {
                opened: false
            },


            data: record,

            raw: record //原数据
        };


        if (icon) {
            node.icon = icon;
        }

        if (expand) {
            node.state.opened = true;
        }

        if (me.checkbox && checked) {
            //node.state.selected = true;
        }


        return node;
    },


    //选中节点
    checkNode: function (node) {
        var me = this,
            el = me.el,
            tree = $(el).jstree('true');

        if (Mini2.isArray(node)) {
            tree.jstree('select_node', node, true, false);
        }
        else if (Mini2.isString(node)) {
            tree.jstree('select_node', node, true, false);
        }
        else {
            tree.check_node(node);
        }

    },


    //设置节点加载的状态
    setNodeLoaded: function (node, value) {
        var me = this,
            el = me.el,
            tree = $(el).jstree(true);


        var treeNode = tree.get_node(node);


        treeNode.li_attr.child_loaded = value;

        //        Mini2.alertProps(treeNode);

        var nodeEl = $('#' + node);

        nodeEl.attr('child_loaded', value);

        //Mini2.alertProps(node);
    },


    setTag: function (node, value) {
        var me = this,
            el = me.el,
            tree = $(el).jstree(true);


        var treeNode = tree.get_node(node);

        treeNode.a_attr.tag = value;

        console.log("属性: ", treeNode.a_attr);

    },


    /** End region **/

    getContainer: function () {
        var me = this,
            el = me.el;

        return el.children('.jstree-container-ul:first');
    },


    open_node: function (node) {
        var me = this,
            el = me.el;

        el.jstree('open_node', node);

        //$(el).jstree(true).open_node(node);

    },

    //清理所有节点
    clear: function () {
        var me = this,
            el = me.el;

        var cont = me.getContainer();

        var childs = $(cont).children('li');

        $(childs).each(function () {
            $(el).jstree(true).delete_node(this);
        });
    },

    cancelRename: false,

    setNodeText: function (node, text, cancel) {
        var me = this,
            el = me.el,
            tree = $(tree).jstree(true);

        //tree.rename_node(node, text);

        me.cancelRename = !!cancel;

        el.jstree('rename_node', node, text);

        me.cancelRename = !me.cancelRename;
    },


    add_ForRecord: function (record, newNodes) {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            tree = $(el).jstree(true),
            sel, pValue, node, i, records, rect;

        if (record) {

            log.debug('批量添加 Tree.Panel.add_ForRecord(...), 上级=' + me.parField);


            records = Mini2.isArray(record) ? record : [record];

            if (records.isArray) {
                records = records.items;
            }

            //log.debug('批量添加 records', records);

            for (i = 0; i < records.length; i++) {


                rect = records[i];



                pValue = rect[me.parField];
                node = me.record_2_node(rect);

                //log.debug('pValue=%s, text=%s', pValue, node.text);
                //log.debug('rect=', rect);
                //log.debug('node=', node);



                if (pValue + '' == me.rootId + '') {
                    me.add(null, node);
                }
                else {
                    me.add(node.parent, node);
                }

                if (newNodes) {

                    //log.debug('pValue=%s, text=%s', pValue, node.text);
                    //log.debug('rect=', rect);
                    //log.debug('node=', node);

                    node.has_child = rect.has_child;
                    newNodes.push(node);
                }
            }
        }
        else {
            log.debug('Tree.Panel.add_ForRecord(...) 没有记录需要添加');
        }

        //sel = tree.create_node(node.parent, node);
    },

    ///添加节点
    add: function (parId, node, callback) {
        var me = this,
            el = me.el,
            nodeRenderer = me.nodeRenderer,
            tree = $(el).jstree(true),
            sel;

        if (arguments.length == 1) {
            node = arguments[0];

            if (nodeRenderer) {
                nodeRenderer.call(me, {
                    node: node,
                    raw: node.raw || node.data
                }, node);
            }

            sel = tree.create_node(me.focusNode, node, undefined, callback);
        }
        else {

            if (nodeRenderer) {
                nodeRenderer.call(me, {
                    node: node,
                    raw: node.raw || node.data
                }, node);
            }

            sel = tree.create_node(parId, node, undefined, callback);
        }

        return sel;
    },


    removeChilds: function (node) {
        var me = this,
            el = me.el,
            tree = $(el).jstree(true);


        var curNode = tree.get_node(node);

        var ids = Mini2.clone(curNode.children);

        for (var i = 0; i < ids.length; i++) {

            var cId = ids[i];
            console.debug("删除: ", cId);

            tree.delete_node(cId);
        }

    },


    'delete': function (node) {
        var me = this,
            el = me.el,
            tree = $(el).jstree(true);;

        tree.delete_node(node);
    },


    edit: function (node) {
        var me = this,
            el = me.el,
            tree = $(el).jstree(true),
            sel;


        tree.edit(node);
    },



    //设置复选框选中的节点
    setCheckedNode: function (ids) {
        var me = this,
            el = me.el,
            t = [],
            str;

        if (ids) {

            if (Mini2.isArray(ids)) {
                str = ids;
            }
            else if (Mini2.isString(ids)) {
                str = ids.split(',');
            }


            $(str).each(function (i, item) {
                t.push(me.id + '_NODE_' + item);
            })


            el.jstree('check_node', t.join(','));


        }



    },

    //获取树状态的信息
    getJson: function () {
        var me = this,
            el = me.el,
            obj = {},
            node;

        if (me.focusNode && me.focusNode.node) {
            node = me.focusNode.node;

            var attr = node.a_attr;
            var liAttr = node.li_attr;

            obj.select = {
                value: attr.nodeId,
                par_id: attr.parNodeId,
                text: node.text,
                child_loaded: liAttr.child_loaded,
                children: node.children,
                tag: attr.tag
            };


        }





        if (me.renameParam) {

            var rp = me.renameParam;

            //Mini2.alertProps(rp.node.a_attr);

            var childLoaded = false;

            if (rp.li_attr) {
                childLoaded = !!(rp.li_attr.child_loaded);
            }

            obj.rename = {
                text: rp.text,
                old: rp.old,
                value: rp.node.a_attr.nodeId,
                par_id: rp.node.a_attr.parNodeId,
                child_loaded: childLoaded,
                children: node.children,
                tag: rp.node.a_attr.tag
            }

        }

        if (me.moveNodeJson) {
            obj.move = me.moveNodeJson;
        }

        if (me.checkbox) {

            var checkedIds = me.getCheckedIds();

            var undeIds = me.getUndeterminedIds();

            obj.checked_ids = checkedIds;
            obj.unde_ids = undeIds;

        }


        var json = Mini2.Json.toJson(obj);

        return json;
    },


    /**
    * 取得所有选中的节点，返回节点对象的集合  
    */
    getCheckedIds: function () {

        var me = this,
            el = me.el,
            tree = $(el).jstree('true'),
            nodeList,
            i,
            ids = '',
            nodeId, id,
            qzLen = (me.id + '_NODE_').length;

        nodeList = tree.jstree('get_selected');

        for (i = 0; i < nodeList.length; i++) {
            nodeId = nodeList[i];
            id = nodeId.substring(qzLen);

            if (i > 0) { ids += ","; }
            ids += id;
        }

        return ids;
    },

    /**
    * 取得所有选中的节点，返回节点对象的集合  
    */
    getUndeterminedIds: function () {

        var me = this,
            el = me.el,
            tree = $(el).jstree('true'),
            nodeList,
            i,
            ids = '',
            nodeId, id,
            qzLen = (me.id + '_NODE_').length;

        //nodeList = tree.jstree('get_selected',true);

        //alert(nodeList.length);

        //for (i = 0; i < nodeList.length; i++) {
        //    nodeId = nodeList[i];
        //    id = nodeId.substring(qzLen);

        //    if (i > 0) { ids += ","; }
        //    ids += id;
        //}


        return '';

        ////取得所有选中的节点，返回节点对象的集合  
        //var checkeds = $('#' + me.clientId).find(".jstree-undetermined");

        ////得到节点的id，拼接成字符串   
        //var ids = "";

        //for (var i = 0; i < checkeds.length; i++) {
        //    ids += $(checkeds[i]).parent().attr("nodeid") + ',';
        //}

        //return ids;
    },


    /**
    * 获取菜单配置
    */
    getMenu: function () {
        var me = this,
            nodeTypeText = me.nodeTypeText,
            items,
            menuList = [];

        if (!me.showMenu) {
            return null;
        }

        items = {
            ccp: false,

        }

        if (me.showMenuNew) {
            menuList.push({
                id: Mini2.newId(),
                label: "创建" + nodeTypeText,
                action: function (obj) {

                    //this.create(obj);                
                    var widget = window.widget1;

                    if (me.triggerEvent_create && widget) {
                        widget.subMethod($('form:first'), { subName: me.id, subMethod: 'OnCreating' });
                    }

                    var store = me.store;

                    if (store) {

                        //var newNodeData = me.getFocusNodeConfig();  //获取焦点节点的配置信息

                        var sd = {
                            subName: store.id,
                            subMethod: 'Insert',
                        };

                        try {
                            widget1.subMethod('form:first', sd);
                        }
                        catch (ex) {
                            console.error('调用服务器 PreCommand 错误', ex);
                        }
                    }

                },
                "_disabled": function (obj) {
                    //alert("obj=" + obj); return "default" != obj.attr('rel'); 
                }
            });
        }

        if (me.showMenuRename) {
            menuList.push({
                id: Mini2.newId(),
                label: "重命名",
                action: function (obj) {

                    //this.create(obj); 

                    //var widget = window.widget1;

                    //if (me.triggerEvent_rename && widget) {
                    //    widget.subMethod($('form:first'), { subName: me.id, subMethod: 'OnEditing' });
                    //}


                    //Mini2.alertProps(me.focusNode.node);



                    me.edit(me.focusNode.node);

                },
                "_disabled": function (obj) {
                    //alert("obj=" + obj); return "default" != obj.attr('rel'); 
                }
            });
        }


        if (me.showMenuDelete) {
            menuList.push({
                id: Mini2.newId(),
                label: "删除",
                action: function (obj) {

                    //this.create(obj); 

                    var widget = window.widget1;

                    if (widget) {

                        var focusNode = me.focusNode.node;

                        Mini2.Msg.confirm('询问', '确定删除 "' + focusNode.text + '" ?', function () {

                            if (me.triggerEvent_remove) {
                                widget.subMethod($('form:first'), { subName: me.id, subMethod: 'OnRemoveing' });
                            }

                            var store = me.store;

                            if (store) {

                                try {
                                    widget1.subMethod('form:first', {
                                        subName: store.id,
                                        subMethod: 'Delete',
                                    });
                                }
                                catch (ex) {
                                    console.error('调用服务器 PreCommand 错误', ex);
                                }
                            }


                        });
                    }



                },
                "_disabled": function (obj) {
                    //alert("obj=" + obj); return "default" != obj.attr('rel'); 
                }
            });

        }

        var contextMenu = me.contextMenu;

        for (var i = 0; i < contextMenu.length; i++) {

            var menuItem = contextMenu[i];


            var menu = {
                id: Mini2.newId(),
                label: menuItem.text,
                command: menuItem.command,
                command_params: menuItem.command_params,

                click: menuItem.click,

                action: function (obj) {
                    me.menu_action.call(me, obj, obj.item);

                }
            };

            menuList.push(menu);
        }

        for (var i = 0; i < menuList.length; i++) {

            var mItem = menuList[i];

            items[mItem.id] = mItem;

        }


        return items;
    },

    /**
    * 获取焦点节点的配置信息
    */
    getFocusNodeConfig: function () {
        var me = this,
            focusNode = me.focusNode,
            cfg = null;


        if (focusNode && focusNode.node) {
            node = focusNode.node;

            var attr = node.a_attr;
            var liAttr = node.li_attr;

            cfg = {
                value: attr.nodeId,
                par_id: attr.parNodeId,
                text: node.text,
                child_loaded: liAttr.child_loaded,
                children: node.children,
                tag: attr.tag
            };
        }

        return cfg;
    },

    //菜单项动作
    menu_action: function (obj, item) {

        var me = this;
        //this.create(obj); 

        var widget = window.widget1;

        if (item.click) {

            var isContinue = item.click.call(me, obj, item);

            if (false === isContinue) {
                return;
            }

        }


        if (widget && !Mini2.isBlank(item.command)) {

            var newNodeData = me.getFocusNodeConfig();  //获取焦点节点的配置信息

            var newNodeId = 'node-' + Mini2.newId();


            var sd = {
                subName: me.id,
                subMethod: 'PreCommand',
                actionPs: {
                    cmdName: item.command,
                    cmdParam: item.commandParam || '',
                    node: 'ref ' + newNodeId,
                    record: 'ref xxxx'
                }
            };

            sd.data = {};
            sd.data[newNodeId] = window.JSON.stringify(newNodeData);

            console.debug('sd = ', sd);

            try {
                widget1.subMethod('form:first', sd);
            }
            catch (ex) {
                console.error('调用服务器 PreCommand 错误', ex);
            }

        }

    },

    /**
    * 创建配置信息
    */
    createConfig: function () {
        var me = this,
            plugins = ['html_data', 'themes', 'ui', 'crrm', 'contextmenu'],
            menus,
            cfg;

        menus = me.getMenu();


        cfg = {

            core: {
                check_callback: true,
                strings: true,
                //'data': function (obj, callback) {

                //    //console.debug("+++++++++++++", obj);

                //    if (me.store) {

                //        console.info('开始初始化数据仓库 TreePanel.initStore().');

                //        me.proLineData(me.store.data);

                //    }


                //}
            },
            plugins: plugins,

            lang: {
                loading: "加载中..."
            },
            contextmenu: {
                items: menus
            }
        };

        if (me.allowDragDrop) {
            plugins.push('dnd');
        }

        if (me.checkbox) {
            cfg.checkbox = {
                keep_selected_style: false
            };

            plugins.push('checkbox');
        }

        //plugins.push('wholerow');   //焦点行...整行

        if (me.types) {
            cfg.types = me.types;
            plugins.push('types');

            delete me.types;
        }

        return cfg;
    },

    /**
     * 焦点节点
     */
    focusNode: null,

    baseRender: function () {
        var me = this,
            el,
            cfg = me.createConfig();


        me.bind('nodeRename', function (e) {

            if (e.text == e.old) {
                return;
            }

            me.renameParam = e;

            var widget = window.widget1;

            if (me.triggerEvent_rename && widget) {
                widget.subMethod($('form:first'), { subName: me.id, subMethod: 'OnRenaming' });
            }


            var store = me.store;

            if (store) {


                try {
                    widget1.subMethod('form:first', {
                        subName: store.id,
                        subMethod: 'Rename',
                        actionPs: {
                            oldText: e.old,
                            text: e.text
                        }
                    });
                }
                catch (ex) {
                    console.error('调用服务器 PreCommand 错误', ex);
                }
            }

        });


        if (me.data) {

            if (Mini2.isArray(me.data)) {

            }
            else {
                cfg.core.data = me.data;
                delete me.data;
            }

        }



        if (me.applyTo) {
            el = $(me.applyTo).jstree(cfg);
        }
        else {
            el = $(me.renderTo).jstree(cfg);
        }

        me.el = el;

        el.css('background-color', '#FFFFFF');

        me.setSize(me.width, me.height);


        //复选框做了特殊处理.
        if (me.checkbox) {
            el.bind('changed.jstree', function (event, data) {

                if ('select-hwq' == data.action) {

                    if (me.autoExpand && $(el).jstree('is_closed', data.node)) {
                        $(el).jstree("toggle_node", data.node);
                    }

                    me.trigger_selected(event, data);
                }
            });
        }
        else {
            el.bind('select_node.jstree', function (event, data) {

                if (me.autoExpand && $(el).jstree('is_closed', data.node)) {
                    $(el).jstree("toggle_node", data.node);
                }


                me.trigger_selected(event, data);

            });
        }


        el.bind('open_node.jstree', function (event, data) {

            me.trigger('nodeOpened', data);
        });

        el.bind("loaded.jstree", function (e, data) { data.inst.open_all(-1); });

        el.bind('rename_node.jstree', function (event, data) {

            if (!me.cancelRename) {
                me.trigger('nodeRename', data);
            }
        });



        if (me.allowDragDrop) {
            me.bind_DragDrop();
        }

        if (me.data && Mini2.isArray(me.data)) {
            me.proLineData(me.data);
        }

        me.initStore();

        return el;
    },


    //绑定拖放
    bind_DragDrop: function () {
        var me = this,
            el = me.el;

        el.bind('move_node.jstree', function (event, data) {

            //data.node.id = 当前节点 ID
            //alert(data.node.id);
            //alert(data.parent);
            //alert(data.position);


            try {

                var pId = data.parent.substring(me.id.length + "_NODE_".length);

                //console.log(me.clientId);


                me.moveNodeJson = {
                    node_id: data.node.a_attr.nodeId,
                    parent_id: pId,
                    pos: data.position
                };

                me.onNodeMoved();

            }
            catch (ex) {
                alert("ui.tree.Panel.bind_DragDrop() 提交数据错误");
            }

        });

    },


    onNodeMoved: function () {
        var me = this;

        if (me.event_move) {
            var widget = window.widget1;

            if (widget) {
                widget.subMethod($('form:first'), { subName: me.id, subMethod: 'OnNodeMoved' });
            }
        }
    },

    trigger_selected: function (event, data) {
        var me = this,
            log = me.log,
            store = me.store;

        if (me.focusNode) {
            $(me.focusNode.node).removeClass('jstree-focus-item');
        }

        $(data.node).addClass('jstree-focus-item');


        me.focusNode = data;    // ref.get_selected();


        if (store && store.dymHasChild) {

            var node = data.node;
            var attr = node.a_attr;

            console.debug('节点 data', node.data);

            store.setCurrent(node.data);

            log.debug('问:需要加载远程节点?', node);

            if (!node.li_attr.child_loaded) {

                log.debug('开始加载远程节点. ');

                if (me.remoteEnabled) {

                    var store = me.store,
                        data = node.data,
                        parentId;

                    if (data.get) {
                        parentId = data.get(me.idField);
                    }
                    else {
                        parentId = data[me.idField];
                    }

                    console.debug('开始加载远程数据...', me.remoteUrl);
                    console.debug('上级节点...', node);


                    var remotePs = {
                        parent_id: parentId
                    };

                    Mini2.post(me.remoteUrl, remotePs, function (data) {

                        console.debug('数据回来了...', data);

                        store.add(data);

                        node.li_attr.child_loaded = true;
                    });


                }
                else {
                    var widget = window.widget1;

                    if (widget) {

                        widget.subMethod($('form:first'), {
                            subName: store.id,
                            subMethod: 'LoadChilds'
                        });

                        node.li_attr.child_loaded = true;
                    }
                }

            }
            else {
                log.debug('无需加载远程节点. node.li_attr.child_loaded=', node.li_attr.child_loaded)
            }
        }

        //Mini2.alertProps(data.node);



        if (me.event_selected) {
            var widget = window.widget1;

            if (widget) {

                widget.subMethod($('form:first'), {
                    subName: me.id,
                    subMethod: 'OnSelected'
                });
            }
        }

        me.trigger('nodeSelected', data);
    },

    render: function () {
        var me = this,
            el;

        el = me.baseRender();

        $(el).data('me', me);
    }

});