<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowBuilder.aspx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowBuilder" %>
<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<meta charset="utf-8" />

    <script src="/Core/Scripts/jquery/jquery-3.1.0.js"></script>
    <script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js"></script>
    <script src="/Core/Scripts/EaselJs/easeljs-NEXT.combined.js"></script>
    <script src="/Core/Scripts/EaselJs/tweenjs-NEXT.combined.js"></script>

    <!--<script src="..Scripts/Mini2/dev/Mini.js"></script>
    <script src="..Scripts/Mini2/dev/lang/Array.js"></script>-->

    <script src="/Core/Scripts/Mini2/Mini2.js"></script>

    <script src="/Core/Scripts/Mini2/dev/EventManager.js"></script>
    <script src="/Core/Scripts/Mini2/dev/ui/Window.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/FlowComponent.js"></script>


    <script src="/Core/Scripts/Mini2.Flow/dev/FlowNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/Hand.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/Hotspot.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/LineAnchor.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/Line.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/GhostItem.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/GhostShapePanel.js"></script>


    <script src="/Core/Scripts/Mini2.Flow/dev/SelectPanel.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/LineSelectPanel.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/RectSelectPanel.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/Action.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/ActionContainer.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/ActionGroup.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/Tooltip.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/WindowManager.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/FlowPageGridBack.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/FlowPage.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/FlowPageBuilder.js"></script>
    
    <script src="/Core/Scripts/Mini2.Flow/extend/Node.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/StartNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/EndNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/AutoNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/Note.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/Line.js"></script>


    <style type="text/css" media="screen">
        #my_canvas {
            background-color: #FFFFFF;
        }
    </style>
    <link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" />
    <link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" />
    <link href="/Core/Scripts/Mini2/Themes/Win8/theme-win8.css" rel="stylesheet" />


</head>
<body style="margin:0px; padding:0px;">

    <div style="height:10px; font-size:12px;">
        <!--<button type="button" onclick="window.flow.start()">开始</button>
        <button type="button" onclick="window.flow.stop()">停止</button>-->

        <%--<button type="button" onclick="Mini2.curPage.flowNew()">新建流程</button>--%>
        <button type="button" onclick="Mini2.curPage.flowSave()">保存</button>

        <span style="border-bottom-style:none">| 功能:</span>

        <button type="button" onclick="Mini2.curPage.flowNewLine()">添加线</button>
        <span style="border-bottom-style:none">|</span>


        <button type="button" onclick="Mini2.curPage.flowNewNode('start');">开始环节</button>
        <button type="button" onclick="Mini2.curPage.flowNewNode('end');">结束环节</button>
        
        <button type="button" onclick="Mini2.curPage.flowNewNode('node');">节点</button>
        <button type="button" onclick="Mini2.curPage.flowNewNode('auto_node');">自动活动</button>

        
        <button type="button" onclick="Mini2.curPage.flowNewNode('note');">便笺</button>

        <span style="border-bottom-style:none">| 线段:</span>

        <button type="button" onclick="Mini2.curPage.flow.setLineAction('move')">线-移动点</button>
        <button type="button" onclick="Mini2.curPage.flow.setLineAction('add')">线-添加点</button>
        <button type="button" onclick="Mini2.curPage.flow.setLineAction('remove')">线-删除点</button>
        <span style="border-bottom-style:none">|</span>
        <button type="button" onclick="Mini2.curPage.removeItem()">删除</button>
         <span style="border-bottom-style:none">|</span>

        <button type="button" onclick="Mini2.curPage.showFlowSetup()">流程属性</button>
        <button type="button" onclick="Mini2.curPage.showFlowItemSetup()">图形属性</button>

    </div>

    <br />
    <br />

    <div id="canvasMainBox" style="width:1024px;height:568px; background-color:#ffffff; overflow:auto;">
        <canvas width="1280" height="1024" id="my_canvas"></canvas>
    </div>


    <script>
        



        var curPage = Mini2.define('Mini2.curPage', {

            singleton: true,


            db_flow: {
                id :0
            },

            onInit : function () {

                var me = this;

                me.flowInit();


                $(document).ready(function () {

                    me.resizeWin();

                    $(window).resize(me.resizeWin);

                });

            },

            resizeWin : function () {

                var w = $(window).width();
                var h = $(window).height();

                $('#canvasMainBox').width(w).height(h - 100);


            },

                        
            //流程初始化
            flowInit : function () {
                var me = this,
                    flowData = me.flowData;

                var flow = Mini2.create('Mini2.flow.FlowPageBuilder', {
                    renderTo: 'my_canvas',
                    width: 1280,
                    height: 1024

                });

                flow.render();

                me.flow = flow;


                //条目删除
                flow.bind('itemRemove', function (e) {

                    console.log("删除事件 e = ", e);

                    var dataStr = JSON.stringify(e);

                    var url = $.format('/View/OneFlowBuilder/FlowDefineHandler.ashx?action={0}', 'remove_item');

                    Mini2.post(url, {
                        def_id: me.db_flow.id,
                        data: dataStr

                    }, function (data, result) {

                        Mini2.toast('删除成功!');

                    });

                });

                flow.bind('itemCreated', function (e) {

                    console.log("节点激活 e = ", e);

                    var dataStr = JSON.stringify(e);

                    var url = $.format('/View/OneFlowBuilder/FlowDefineHandler.ashx?action={0}', 'enabled_item');

                    Mini2.post(url, {
                        def_id: me.db_flow.id,
                        data: dataStr

                    }, function (data, result) {

                        //Mini2.toast('删除成功!');

                    });

                });


                flow.bind('itemDblclick', function (e) {
                    
                    me.showFlowItemSetup();
                });



                me.flowRead();
            },


            //根据 '流程定义id' , , 对齐流程图
            flowRead: function(){
                var me = this,
                    flow = me.flow;

                var flow_def_id = $.query.get('flow_def_id');


                var url = $.format('/View/OneFlowBuilder/FlowDefineHandler.ashx?action={0}', 'get_def');

                

                Mini2.post(url, {

                    def_id : flow_def_id

                }, function (data, result) {
                    var items = data.items;

                    var new_id = data.def_id;


                    me.flow.id = new_id;

                    me.db_flow = {
                        id: new_id
                    };

                    
                    
                    flow.removeAll();

                    for (var i = 0; i < items.length; i++) {

                        var item = items[i];

                        var cfg = Mini2.clone(item);
                        cfg.is_preview = false;

                        if (item.item_type == 'line') {

                            if (item.points_str) {
                                var ps = JSON.parse(item.points_str);

                                cfg.points = ps;
                            }
                            else {
                                cfg.points = [[0, 0], [100, 0]];
                            }

                            flow.addChildByConfig(cfg);
                        }
                        else if (item.item_type == 'node') {
                            flow.addChildByConfig(cfg);
                        }

                    }


                    flow.resetLineLink();

                });



            },


            //新建流程
            flowNew : function () {
                var me = this;

                var url = $.format("/View/OneFlowBuilder/FlowDefineHandler.ashx?action={0}", 'new_def');

                Mini2.post(url, {
                    

                }, function (data,result) {

                    var new_id = data.new_id;

                    me.flow.removeAll();

                    me.flow.id = new_id;

                    me.db_flow = {
                        id : new_id
                    };
                });


            },


            //保存流程
            flowSave: function () {
                var me = this;

                var flow = me.flow;

                var cfg = flow.getConfig();

                console.debug("cfg", cfg);

                var cfgStr = JSON.stringify(cfg);

                var url = $.format("/View/OneFlowBuilder/FlowDefineHandler.ashx?action={0}", 'save_def');

                Mini2.post(url, {
                    
                    flow_define: cfgStr

                }, function (data,result) {
                    
                    Mini2.toast('保存成功');

                });
                
            },
            
            flowNewLine: function () {
                var me = this,
                    flow = me.flow,
                    db_flow = me.db_flow;


                var url = $.format("/View/OneFlowBuilder/FlowDefineHandler.ashx?action={0}", 'new_line');

                Mini2.post(url, {

                    def_id: db_flow.id

                }, function (data, result) {


                    var line_id = data.line_id;
                    var line_code = data.node_code;
                    
                    flow.setInsertShapeType('line', null, {
                        id: line_id,
                        item_id: line_id,
                        item_code: line_code
                    });

                    Mini2.toast('新建线段');

                });


            },
            
            //创建流程节点
            flowNewNode: function (node_type) {
                var me = this,
                    flow = me.flow,
                    db_flow = me.db_flow;


                var url = $.format("/View/OneFlowBuilder/FlowDefineHandler.ashx?action={0}", 'new_node');

                Mini2.post(url, {

                    def_id: db_flow.id,

                    node_type: node_type


                }, function (data, result) {
                    
                    console.debug("数据回来..", data);

                    var item_id = data.item_id;
                    var node_code = data.node_code;

                    var item_fullname = data.item_fullname;

                    flow.setInsertShapeType('action', item_fullname, {
                        id: item_id,
                        item_id: item_id,
                        item_code: node_code
                    });

                    Mini2.toast('新建节点');

                });

            },
            

            

            
            // 显示流设置
            showFlowSetup: function () {

                var me = this,
                    db_flow = me.db_flow;

                console.log("flow", db_flow)

                var urlPath = '/App/InfoGrid2/View/OneFlowBuilder';
                var url = urlPath + $.format("/FlowProSetup.aspx?def_id={0}", db_flow.id);


                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: '流程定义',
                    width: 800,
                    height: 600,
                    mode: true,
                    startPosition: 'center_screen',
                    url: url
                });

                frm.show();

            },


            //设置选中的对象
            showFlowItemSetup:function(){
                var me = this,
                    flow = me.flow,
                    db_flow = me.db_flow,
                    cur = flow.curFocusObj;

                if (cur.isLine) {
                    me.showFlowLineSetup();
                }
                else {
                    me.showFlowNodeSetup();
                }

            },
            
            //设置节点属性
            showFlowNodeSetup: function (nodeId) {
                
                var me = this,
                    cur,
                    flow = me.flow,
                    db_flow = me.db_flow;

                if (undefined == nodeId) {
                    
                    cur = flow.curFocusObj;

                    if (!cur.isFlowNode) {
                        return;
                    }

                    nodeId = cur.item_id;
                    
                }

                console.log("cur", cur);


                if (cur.isNote) {
                    var urlPath, url;

                    urlPath = '/App/InfoGrid2/View/OneFlowBuilder';
                    url = urlPath + $.format("/FlowNoteSetup.aspx?def_id={0}&def_node_id={1}", db_flow.id, nodeId);
                    


                    var frm = Mini2.createTop('Mini2.ui.Window', {
                        text: '流程标签',
                        width: 500,
                        height: 300,
                        mode: true,
                        startPosition: 'center_screen',
                        url: url
                    });

                    frm.formClosed(function (e) {
                        var me = this,
                            ud = me.userData;

                        cur.setText(ud.text);
                        cur.setStyleName(ud.style_name);

                        cur.drawReset();

                    });

                    frm.show();

                   
                }
                else {
                    var urlPath, url;

                    urlPath = '/App/InfoGrid2/View/OneFlowBuilder';
                    url = urlPath + $.format("/FlowNoteSetup.aspx?def_id={0}&def_node_id={1}", db_flow.id, nodeId);

                    if (cur.isAutoNode) {
                        url = urlPath + $.format("/FlowAutoNodeSetup.aspx?def_id={0}&def_node_id={1}", db_flow.id, nodeId);
                    }
                    else {
                        url = urlPath + $.format("/FlowNodeSetup.aspx?def_id={0}&def_node_id={1}", db_flow.id, nodeId);
                    }

                    var frm = Mini2.createTop('Mini2.ui.Window', {
                        text: '流程节点定义',
                        width: 800,
                        height: 600,
                        mode: true,
                        startPosition: 'center_screen',
                        url: url
                    });

                    frm.show();

                    frm.formClosed(function (e) {
                        var me = this,
                            ud = me.userData;

                        cur.setText(ud.text);

                    });
                }
            },
            

            //设置节点属性
            showFlowLineSetup: function (lineId) {

                var me = this,
                    cur,
                    flow = me.flow,
                    db_flow = me.db_flow;

                if (undefined == lineId) {

                    cur = flow.curFocusObj;

                    if (!cur.isLine) {
                        return;
                    }

                    lineId = cur.item_id;

                }


                var urlPath = Mini2.urlAppend('/App/InfoGrid2/View/OneFlowBuilder/FlowLineSetup.aspx', {
                    def_id: db_flow.id,
                    def_line_id: lineId
                });
                

                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: '流程路由定义',
                    width: 500,
                    height: 500,
                    mode: true,
                    startPosition: 'center_screen',
                    url: urlPath
                });

                frm.show();
            },

            removeItem : function(){
                var me = this,
                    flow = me.flow,
                    db_flow = me.db_flow;
                    
                flow.remoteSelecteds();
            }

        },function(){
            this.onInit();

        });


       

    </script>


</body>
</html>
