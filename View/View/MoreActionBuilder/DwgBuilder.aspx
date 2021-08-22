<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DwgBuilder.aspx.cs" Inherits="App.InfoGrid2.View.MoreActionBuilder.DwgBuilder" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta charset="utf-8" />

    <script src="/Core/Scripts/jquery/jquery-3.1.0.js"></script>
    <script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js"></script>
    <script src="/Core/Scripts/EaselJs/easeljs-NEXT.combined.js"></script>
    <script src="/Core/Scripts/EaselJs/tweenjs-NEXT.combined.js"></script>

    <!--<script src="..Scripts/Mini2/dev/Mini.js"></script>
    <script src="..Scripts/Mini2/dev/lang/Array.js"></script>-->

    <%
        
        string JsHome = string.Empty;

        string mapPath = MapPath("~/Core/Scripts/Mini2/Mini2.join.ini");
        string[] jsLines = System.IO.File.ReadAllLines(mapPath);
           
        foreach (string item in jsLines)
        {
            if(string.IsNullOrEmpty(item) || item.Trim().StartsWith("--")){
                continue;   
            }

            Response.Write(string.Format("<script src='{0}/Core/Scripts/Mini2/dev/{1}?v=2.1' ></script>", JsHome, item));
        }
    %>


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

<%--    <script src="/Core/Scripts/Mini2.Flow/extend/Node.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/StartNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/EndNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/AutoNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/Note.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/extend/Line.js"></script>--%>

    <script src="/Core/Scripts/Mini2.Flow/ac3/css/ItemCssSet.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/ac3/Line.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/ac3/Node.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/ac3/Note.js"></script>

    <style type="text/css" media="screen">
        #my_canvas {
            background-color: #FFFFFF;
        }


    </style>
    <link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" />
    <link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" />
    <link href="/Core/Scripts/Mini2/Themes/Win8/theme-win8.css" rel="stylesheet" />


</head>
<body style="margin: 0px; padding: 0px;">

    <div style="height: 10px; font-size: 12px;">
        <!--<button type="button" onclick="window.flow.start()">开始</button>
        <button type="button" onclick="window.flow.stop()">停止</button>-->

        <%--<button type="button" onclick="Mini2.curPage.flowNew()">新建流程</button>--%>
        <button type="button" onclick="Mini2.curPage.flowSave()">保存</button>

        <span style="border-bottom-style: none">| 功能:</span>

        <button type="button" onclick="Mini2.curPage.flowNewLine()">添加线</button>
        <span style="border-bottom-style: none">|</span>


        <%--        <button type="button" onclick="Mini2.curPage.flowNewNode('start');">开始环节</button>
        <button type="button" onclick="Mini2.curPage.flowNewNode('end');">结束环节</button>--%>

        <button type="button" onclick="Mini2.curPage.flowNewNode('node', 'listen_table');">监听节点</button>
        <button type="button" onclick="Mini2.curPage.flowNewNode('node', 'operate_table');">动作节点</button>

        <button type="button" onclick="Mini2.curPage.flowNewNode('node');">节点</button>
        <button type="button" onclick="Mini2.curPage.flowNewNode('node');">节点</button>

        <%--        <button type="button" onclick="Mini2.curPage.flowNewNode('auto_node');">自动活动</button>--%>


        <button type="button" onclick="Mini2.curPage.flowNewNode('note');">便笺</button>

        <span style="border-bottom-style: none">| 线段:</span>

        <button type="button" onclick="Mini2.curPage.flow.setLineAction('move')">线-移动点</button>
        <button type="button" onclick="Mini2.curPage.flow.setLineAction('add')">线-添加点</button>
        <button type="button" onclick="Mini2.curPage.flow.setLineAction('remove')">线-删除点</button>
        <span style="border-bottom-style: none">|</span>
        <button type="button" onclick="Mini2.curPage.removeItem()">删除</button>
        <span style="border-bottom-style: none">|</span>

        <button type="button" onclick="Mini2.curPage.showFlowSetup()">联动属性</button>
        <button type="button" onclick="Mini2.curPage.showFlowSoundCode()">源码</button>
        <%--        <button type="button" onclick="Mini2.curPage.showFlowItemSetup()">图形属性</button>--%>
    </div>

    <br />
    <br />

    <div id="canvasMainBox" style="width: 1024px; height: 568px; background-color: #ffffff; overflow: auto;">
        <canvas width="1280" height="1024" id="my_canvas"></canvas>
    </div>


    <script>




        var curPage = Mini2.define('Mini2.curPage', {

            singleton: true,


            db_flow: {
                dwg_code: $.query.get('dwg_code')
            },

            onInit: function () {

                var me = this;

                me.flowInit();


                $(document).ready(function () {

                    me.resizeWin();

                    $(window).resize(me.resizeWin);

                });

            },

            resizeWin: function () {

                var w = $(window).width();
                var h = $(window).height();

                $('#canvasMainBox').width(w).height(h - 100);


            },


            //流程初始化
            flowInit: function () {
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

                    var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx',{
                        action: 'remove_item'
                    },true);

                    Mini2.post(url, {
                        dwg_code: me.db_flow.dwg_code,
                        data: dataStr
                    },
                    function (data, result) {
                        Mini2.toast('删除成功!');
                    });

                });

                flow.bind('itemCreated', function (e) {

                    var dataStr = JSON.stringify(e);

                    var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
                        action: 'enabled_item'
                    },true);

                    Mini2.post(url, {
                        dwg_code: me.db_flow.dwg_code,
                        data: dataStr
                    },
                    function (data, result) {
                        //Mini2.toast('删除成功!');
                    });
                    
                });


                flow.bind('itemDblclick', function (e) {

                    me.showFlowItemSetup();
                });



                me.flowRead();
            },


            //根据 '流程定义id' , , 对齐流程图
            flowRead: function () {
                var me = this,
                    flow = me.flow;

                var dwg_code = $.query.get('dwg_code');


                var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
                    action: 'get_dwg'
                }, true);

                Mini2.post(url, {
                    dwg_code: dwg_code
                },
                function (data) {
                    var items = data.items;

                    var dwg_code = data.dwg_code;

                    
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
            flowNew: function () {
                var me = this;

                var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
                    action: 'new_dwg'
                },true);

                Mini2.post(url, {},
                function (data) {

                    var new_id = data.new_id;

                    me.flow.removeAll();

                    me.flow.id = new_id;

                    me.db_flow = {
                        id: new_id
                    };
                });


            },


            //保存流程
            flowSave: function () {
                var me = this;

                var flow = me.flow;

                var cfg = flow.getConfig();
                                
                var cfgStr = JSON.stringify(cfg);


                var url = Mini2.urlAppend("/App/InfoGrid2/View/MoreActionBuilder/DwgHandler.ashx", {
                    action: 'save_dwg'
                }, true);


                Mini2.post(url, {
                    dwg_code: me.db_flow.dwg_code,
                    flow_define: cfgStr
                },
                function (data, result) {

                    Mini2.toast('保存成功');

                });

            },

            flowNewLine: function () {
                var me = this,
                    flow = me.flow,
                    db_flow = me.db_flow;


                var url = Mini2.urlAppend("/View/MoreActionBuilder/DwgHandler.ashx", { action: 'new_line' }, true);

                Mini2.post(url, {
                    dwg_code: db_flow.dwg_code
                },
                function (data, result) {

                    var line_code = data.line_code;

                    var item_fullname = data.item_fullname;

                    flow.setInsertShapeType('line', item_fullname, {
                        id: data.item_id,
                        code: data.item_code
                    });

                    Mini2.toast('新建线段');

                });


            },

            /**
             * 创建流程节点
             * @param {string} node_type 节点类型
             * @param {string} ex_node_type 扩展节点类型
             * @return 
             */
            flowNewNode: function (node_type, ex_node_type) {
                var me = this,
                    flow = me.flow,
                    db_flow = me.db_flow;

                var url = Mini2.urlAppend("/View/MoreActionBuilder/DwgHandler.ashx", { action: 'new_node' }, true);


                Mini2.post(url, {
                    dwg_code: db_flow.dwg_code,
                    node_type: node_type,
                    ex_node_type: ex_node_type
                },
                function (data, result) {

                    var item_fullname = data.item_fullname;

                    flow.setInsertShapeType('action', item_fullname, {
                        id: data.node_id,
                        code: data.node_code,
                        ex_node_type: ex_node_type
                    });

                    Mini2.toast('新建节点');

                });

            },






            showFlowSetup: function () {

                var me = this,
                    db_flow = me.db_flow;

                console.log("flow", db_flow)

                var urlPath = '/App/InfoGrid2';
                
                var url = urlPath + Mini2.urlAppend("/View/MoreActionBuilder/DwgSetup.aspx", {
                    dwg_code: db_flow.dwg_code
                }, true);

                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: '属性 - 联动图纸',
                    width: 800,
                    height: 600,
                    mode: true,
                    url: url
                });

                frm.show();

            },

            showFlowSoundCode: function () {

                var me = this,
                    db_flow = me.db_flow;

                console.log("flow", db_flow)

                var urlPath = '/App/InfoGrid2';

                var url = urlPath + Mini2.urlAppend("/View/MoreActionBuilder/DwgSoundCode.aspx", {
                    dwg_code: db_flow.dwg_code
                }, true);

                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: 'XML 源码',
                    width: 800,
                    height: 600,
                    mode: true,
                    url: url
                });

                frm.show();

            },


            //设置选中的对象
            showFlowItemSetup: function () {
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
                    db_flow = me.db_flow,
                    node_code;

                if (undefined == nodeId) {
                    cur = flow.curFocusObj;

                    if (!cur.isFlowNode) {
                        return;
                    }

                    nodeId = cur.item_id;
                }

                node_code = cur.item_code;

                console.log("cur", cur);


                var urlPath, url;

                urlPath = '/App/InfoGrid2/View/MoreActionBuilder';


                if (cur.ex_node_type == 'listen_table') {

                    url = urlPath + Mini2.urlAppend("/DwgSetupListen.aspx", {
                        'dwg_code': db_flow.dwg_code,
                        'node_code': node_code
                    }, true);



                    var frm = Mini2.createTop('Mini2.ui.Window', {
                        text: '监听节点',
                        width: 800,
                        height: 600,
                        mode: true,
                        state: 'max',
                        url: url
                    });

                    frm.formClosed(function (e) {
                        var me = this,
                            ud = me.userData;

                        //cur.setText(ud.text);
                        //cur.setStyleName(ud.style_name);
                        //cur.drawReset();

                    });

                    frm.show({
                        from: {
                            left: 0,
                            top: 0,
                            width: 100,
                            height: 20
                        }
                    });
                }
                else if (cur.ex_node_type == 'operate_table') {
                    url = urlPath + Mini2.urlAppend("/DwgSetupOperate.aspx", {
                        dwg_code: db_flow.dwg_code,
                        node_code: node_code
                    }, true);


                    var frm = Mini2.createTop('Mini2.ui.Window', {
                        text: '操作节点',
                        width: 800,
                        height: 600,
                        mode: true,
                        state: 'max',
                        url: url
                    });

                    frm.formClosed(function (e) {
                        var me = this,
                            ud = me.userData;

                        //cur.setText(ud.text);
                        //cur.setStyleName(ud.style_name);
                        //cur.drawReset();

                    });

                    frm.show();
                }


            },


            /**
            * 显示线段设置的窗体
            */
            showFlowLineSetup: function (lineCode) {

                var me = this,
                    cur,
                    flow = me.flow,
                    db_flow = me.db_flow;

                cur = flow.curFocusObj;

                if (undefined == lineCode) {
                    if (!cur.isLine) {
                        return;
                    }

                    lineCode = cur.item_code;
                }


                var urlPath = '/App/InfoGrid2/View/MoreActionBuilder';

                var url = urlPath + Mini2.urlAppend("/DwgSetupLine.aspx", {
                    dwg_code: db_flow.dwg_code,
                    line_code: lineCode
                },true);

                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: '连接线',
                    width: 500,
                    height: 500,
                    mode: true,
                    url: url
                });


                frm.formClosed(function (e) {
                    var me = this,
                        ud = me.userData;

                    cur.setText(ud.text);

                });

                frm.show();


                //$.get(url, function (result, status) {

                //    var frm = Mini2.createTop('Mini2.ui.Window', {
                //        text: '连接线',
                //        width: 500,
                //        height: 500,
                //        mode: true,
                //        iframeContent : result
                //        //url: url
                //    });


                //    frm.formClosed(function (e) {
                //        var me = this,
                //            ud = me.userData;

                //        cur.setText(ud.text);

                //    });

                //    frm.show();

                //});

            },

            //删除条目
            removeItem: function () {
                var me = this,
                    flow = me.flow,
                    db_flow = me.db_flow;

                flow.remoteSelecteds();
            }

        },
        function () {
            this.onInit();

        });




    </script>


</body>
</html>
