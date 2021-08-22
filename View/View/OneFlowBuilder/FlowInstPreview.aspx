<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowInstPreview.aspx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowInstPreview" %>
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

    <div style="height:10px; font-size:12px; display:none;">
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

    <div id="canvasMainBox" style="width:1280px;height:1024px; background-color:#ffffff; overflow:auto;">
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

                var flow = Mini2.create('Mini2.flow.FlowPage', {
                    renderTo: 'my_canvas',
                    width: 1280,
                    height: 1024

                });

                flow.render();

                me.flow = flow;



                flow.bind('itemDblclick', function (e) {
                    
                });



                me.flowRead();
            },


            //根据 '流程定义id' , , 对齐流程图
            flowRead: function(){
                var me = this,
                    flow = me.flow;

                var flow_def_id = $.query.get('flow_def_id');

                var flow_inst_code = $.query.get('flow_inst_code'); //流程实例代码


                var url = $.format('/View/OneFlowBuilder/FlowInstHandler.ashx?action={0}', 'GET_FLOW_INST');

                Mini2.post(url, {

                    def_id: flow_def_id,
                    
                    flow_inst_code: flow_inst_code

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



            }


        },function(){
            this.onInit();

        });


       

    </script>


</body>
</html>
