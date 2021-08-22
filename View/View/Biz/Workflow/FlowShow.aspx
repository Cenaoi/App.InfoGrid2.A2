<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowShow.aspx.cs" Inherits="App.InfoGrid2.View.Biz.Workflow.FlowShow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>流程图</title>

    <script src="/Core/Scripts/jquery/jquery-2.0.3.min.js"></script>
    <script src="/Core/Scripts/EaselJs/easeljs-NEXT.combined.js"></script>
    <script src="/Core/Scripts/Mini2/dev/Mini.js"></script>


    <script src="/Core/Scripts/Mini2.Flow/dev/FlowComponent.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/FlowPage.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/FlowPageBuilder.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/FlowNode.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/Hand.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/Hotspot.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/LineAnchor.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/Line.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/LineSelectPanel.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/RectSelectPanel.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/Action.js"></script>

    <script src="/Core/Scripts/Mini2.Flow/dev/Tooltip.js"></script>
    <script src="/Core/Scripts/Mini2.Flow/dev/WindowManager.js"></script>

    <style type="text/css" media="screen">
        #my_canvas {
            background-color: #f2f2f2;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
     
         <canvas width="2048" height="500" id="my_canvas"></canvas>

    </form>
</body>
</html>

 <script type="text/javascript">

     var actStyle= {        

             //灰透-未生效
             '0': {

                 back_color: '#f2f2f2',

                 back_color_over: '#f2f2f2',

                 font_color: '#BFBFBF',

                 //边框大小
                 border_width: 1,

                 //边框类型  solid-实线, dashed-虚线
                 border_style: 'dashed' 

             },

             //状态1 ,橘色-未处理
             '1': {
                 //背景颜色
                     back_color: '#ed7d31',

                     back_color_over: '#ed7d31',

                 //文字颜色
                     font_color: '#FFFFFF',

                 //边框大小
                     border_width: 1,

                 //边框类型
                     border_style: 'solid' //solid-实线, dashed-虚线
             },
        
             //绿色-已经处理过.
             '2': {

                 //背景颜色
                     back_color: '#70ad47',

                 //鼠标移动到上面
                     back_color_over: '#70ad47',

                 //文字颜色
                     font_color: '#FFFFFF',

                 //边框大小
                     border_width: 1,

                 //边框类型
                     border_style: 'solid' //solid-实线, dashed-虚线



             } ,

             //绿色-已经处理过.
             '4': {

                 //背景颜色
                     back_color: '#FFFFFF',

                 //鼠标移动到上面
                     back_color_over: '#FFFFFF',

                 //文字颜色
                     font_color: '#000000',

                 //边框大小
                     border_width: 1,

                 //边框类型
                     border_style: 'solid' //solid-实线, dashed-虚线
             },
         
            //绿色-已经处理过.
             '99': {

                 //背景颜色
                 back_color: '#349CFF',

                 //鼠标移动到上面
                 back_color_over: '#FFFFFF',

                 //文字颜色
                 font_color: '#000000',

                 //边框大小
                 border_width: 1,

                 //边框类型
                 border_style: 'solid' //solid-实线, dashed-虚线
             }
        
     };



        var flowData = {

            v: 1,

            page_items: [{
                item_type: 'action',
                text: '下订单',

                //提示框的偏移量
                tipOffset: {
                    x: 40,
                    y: 60
                },

                x: 10.5,
                y: 20.5,
                width : 100,    //宽度
                height: 50      //高度


            }, {
                item_type: 'action',
                text: '下生产单',
                x: 10,
                y: 200,
                tipOffset: {
                    x: 40,
                    y: -70
                },

            },  
//{
//                item_type: 'action',
//                text: '采购需求',
//                x: 200,
//                y: 120
//            },
{
                item_type: 'action',
                text: '下采购单',
                x: 400,
                y: 120
            },{
                item_type: 'action',
                text: '采购到货',
                x: 600,
                y: 120
            }, {
                item_type: 'action',
                text: '车间生产',
                x: 200,
                y: 200
            },
//{
//                item_type: 'action',
//                text: '外放需求',
//                x: 200,
//               y: 280
//            }, 
{
                item_type: 'action',
                text: '下外发单',
                x: 400,
                y: 280
            }, {
                item_type: 'action',
                text: '外发收货',
                x: 600,
                y: 280
            }, {
                item_type: 'action',
                text: '生产入库',
                x: 400,
                y: 200
            }, {
                item_type: 'action',
                text: '销售出货',
                x: 600,
                y: 200
            },
//{
//                item_type: 'line',
//                desc:'to 采购需求',
//                x: 296,
//                y: 140,
//                points: [
//                    [0, 0],
//                    [100, 0]
//                ]
//            },
{
                item_type: 'line',
                desc:'to 采购到货',
                x: 496,
                y: 140,
                points: [
                    [0, 0],
                    [100, 0]
                ]
            }, {
                item_type: 'line',
                desc: 'to 生产入库',
                x: 296,
                y: 220,
                points: [
                    [0, 0],
                    [100, 0]
                ]
            }, {
                item_type: 'line',
                desc: 'to 销售出货',
                x: 498,
                y: 220,
                points: [
                    [0, 0],
                    [100, 0]
                ]
            }, 
//{
//                item_type: 'line',
//                desc: 'to 外放需求',
//               x: 296,
//                y: 300,
//                points: [
//                    [0, 0],
//                    [100, 0]
//                ]
//            },
{
                item_type: 'line',
                desc: 'to 外发收货',
                x: 496,
                y: 300,
                points: [
                    [0, 0],
                    [100, 0]
                ]
            }, {
                item_type: 'line',
                desc: 'to 下生产单',
                x: 60,
                y: 48,
                points: [
                    [0, 0],
                    [0, 150]
                ]
            }, {
                item_type: 'line',
                desc:'to 下采购单',
                x: 110,
                y: 220,
                points: [
                    [0, 0],
                    [40, 0],
                    [40, -80],
                    [288,-80]
                    
                ]
            }, {
                item_type: 'line',
                desc: 'to 车间生产',
                x: 110,
                y: 220,
                points: [
                    [0, 0],
                    [40, 0],
                    [40, 0],
                    [88, 0]

                ]
            }, {
                item_type: 'line',
                desc: 'to 下外发单',
                x: 110,
                y: 220,
                points: [
                    [0, 0],
                    [40, 0],
                    [40, 80],
                    [288, 80]

                ]
            }]


        };
     

        var userData =  <%= m_userData %>;;

        $(document).ready(function () {
            
            var flow = Mini2.create('Mini2.flow.FlowPage', {
                renderTo:'my_canvas'

            });

            flow.render();


            var pageItems = flowData.page_items;

            for (var i = 0; i < pageItems.length; i++) {

                var item = pageItems[i];

                var obj = null;

                if (item.item_type == 'line') {
                    obj = Mini2.create('Mini2.flow.Line', item);
                }
                else if (item.item_type == 'action') {
                    item.style = actStyle;

                    obj = Mini2.create('Mini2.flow.Action', item);
                }

                flow.addChild(obj);
            }

            
            //填充用户数据
            for (var i = 0; i < userData.length; i++) {

                var userItem = userData[i];

                var act = flow.getActionByText(userItem.text);

                if (act) {
                    act.setTooltip(userItem.tooltipText);
                    act.setState(userItem.state);
                }
                else {
                    console.log('没有这个名称:' + userItem.text);
                }

            }


            //var dd = Mini2.create('Mini2.flow.Hotspot', {
            //    x: 10,
            //    y: 10
            //});

            //dd.render();

            //flow.addChild(dd);


            window.flow = flow;
        });

    </script>



