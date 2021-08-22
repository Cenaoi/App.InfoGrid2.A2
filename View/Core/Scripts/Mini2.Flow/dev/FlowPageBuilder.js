/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />



Mini2.define('Mini2.flow.FlowPageBuilder', {


    extend: 'Mini2.flow.FlowPage',

    designMode: true,    //设计模式 

    
    // 焦点类型. none-没有选择, line-线段, rect-矩形
    focusType: 'none',

    //当前焦点对象
    curFocusObj: null,



    //线选中的辅助图层
    lineSelectPanel: null,

    //区域选中的辅助图层
    rectSelectPanel: null,

    //热点面板
    hotspotPanel: null,

    //当前鼠标所在的热点上面
    curHotOwnerOver:null,


    //当前准备插入的对象名称
    curInsertElem: null,

    //选择版面
    selectPanel: null,

    //拖拉前容器
    beginDragPanel : null,

    //拖拉结束的容器
    endDragPanel: null,


    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this,
            el, stage;

        //当前拖动到上方的容器组
        me.curOverdragCont = {

            shape: null,

            //容器对象
            cont: null,

            mouseX: 0,
            mouseY:0
        };

        me.el = el = document.getElementById(me.renderTo);

        me.stage = stage = new createjs.Stage(el);


        //初始化基础版面
        me.initPanels();

        //线和矩形编辑层
        me.initLineEditPanel();
        me.initRectEditPanel();

        //初始化热点层
        me.initHotspotPanel();

        //初始化选择版面
        me.initSelectPanel();
    },

    //初始化选择版面
    initSelectPanel: function () {
        "use strict";
        var me = this,
            mainPanel = me.mainPanel,
            panel;

        me.selectPanel = panel = Mini2.create('Mini2.flow.SelectPanel', {

        });


        mainPanel.addChild(panel.el);
    },

    //设置插入对象的名称
    // 对象的结构的是 { type:'line|rect', name:'line' , data: {...} }
    // type = 类型, name=是对象的名称,包含明名控件, data=扩展数据
    // id 
    setInsertShapeType: function (item_type, item_fullname, itemConfig) {
        "use strict";
        var me = this,
            cfg = itemConfig;
        
        me.curInsertElem = {
            id: cfg.id,
            item_id: cfg.id,
            item_code: cfg.code,

            ex_node_type: cfg.ex_node_type,

            item_type: item_type,
            item_fullname: item_fullname
        };

        console.log("setInsertShapeType(...) itemConfig = " ,me.curInsertElem);

        return me;
    },


    // 添加子节点
    addChild: function (item) {
        var me = this;

        me.baseAddChild(item);

        if (item.isFlowNode) {

            item.bind('hotRollover', me.item_hotRollover, null, me)
                .bind('hotRollout', me.item_hotRollout, null, me);
        }

    },


    stage_mousedown: function (evt) {
        "use strict";
        var me = this;


        if (evt.relatedTarget == null) {

            me.lineSelectPanel.setVisible(false);
            me.rectSelectPanel.setVisible(false);

            me.curFocusObj = null;
            me.selecteds = [];
        }
    },


    getLinesForArea: function (spRect, lines) {
        "use strict";
        var me = this,
            line,
            rect,
            len = lines.length,
            i,
            items = [];


        for (i = 0; i < len; i++) {
            line = lines[i];

            rect = line.getRect();

            if (rect.x0 >= spRect.x0 && rect.x1 <= spRect.x1 &&
               rect.y0 >= spRect.y0 && rect.y1 <= spRect.y1) {

                items.push(line);
            }
        }


        return items;
    },

    getShapesForArea: function (spRect, shapes, lines) {
        "use strict";
        var me = this,
            line,
            shape,
            rect,
            len = shapes.length,
            i,
            items  = [];

        for (i = 0; i < len; i++) {

            shape = shapes[i];

            rect = shape.getRect();

            if (rect.x0 >= spRect.x0 && rect.x1 <= spRect.x1 &&
               rect.y0 >= spRect.y0 && rect.y1 <= spRect.y1) {

                items.push(shape);
            }

        }


        return items;
    },

    //处理选择范围区域的
    proRectSelected: function () {
        "use strict";
        var me = this,
            i,
            rectSP = me.rectSelectPanel,
            selectPanel = me.selectPanel,
            selecteds,
            spRect,
            selectLines,
            selectRects;


        selectPanel.hide();


        //console.log("P0(" + selectPanel.x0 + ", " + selectPanel.y0 + ") - P1(" + selectPanel.x1 + ", " + selectPanel.y1 + ")");

        spRect = selectPanel.getRect();

        selectRects = me.getShapesForArea(spRect, me.shapes)

        selectLines = me.getLinesForArea(spRect, me.lines);

        //console.log("P0(" + aRect.x0 + ", " + aRect.y0 + ") - P1(" + aRect.x1 + ", " + aRect.y1 + ")");

        //console.log('选了 ' + selecteds.length + ' 个对象');

        //console.log('lineSelecteds.len = ' + selectLines.length);

        if (selectRects.length + selectLines.length > 1) {

            var rectAll = rectSP.getShapesArea(selectRects, selectLines);

            rectSP.setShapes(selectRects, selectLines);

            rectSP.show();


            rectSP.setRectForCartoon(spRect, rectAll, selectRects, selectLines);

            //me.rectSelectPanel.setGhost(selectShapes);

            me.beginDragPanel = me;
        }
        else if (selectRects.length) {
            me.item_MouseDown(selectRects[0], {});
        }
        else if (selectLines.length) {
            me.item_MouseDown(selectLines[0], {});
        }
        else {

            rectSP.setShapes(null,null).hide();
        }



        me.curOverdragCont.shape = null;
        me.curOverdragCont.cont = null;


        me.selecteds = selectRects || [];

        Array.prototype.push.apply(me.selecteds, selectLines);


    },



    stage_mouseup: function (evt) {
        "use strict";
        var me = this,
            overdragTraget = me.curOverdragCont,   //拖动的对象,
            selecteds = me.selecteds,
            rectSP = me.rectSelectPanel,
            selectPanel = me.selectPanel,
            mdTarget = me.mousedownTarget;


        //console.log('FlowPageBuilder.stage_mouseup(...)');


        var spVisible = selectPanel.getVisible();
        var spSz = selectPanel.getSize();

        //console.log('spVisible=' + spVisible);

        //console.log('FlowPageBuilder.stage_mouseup(...)  3');

        //curInsertShapeType 添加模式
        if (me.curInsertElem && me.curInsertElem.item_type) {

            //console.log('FlowPageBuilder.stage_mouseup(...)  4');

            var itemCfg = me.curInsertElem;

            me.curInsertElem = null;

            //console.log("curInsertElem ", curInsertElem);

            if (spVisible && (Math.abs(spSz.width) > 4 || Math.abs(spSz.height) > 4)) {

                var elemCfg = {
                    id: itemCfg.id,
                    item_id: itemCfg.item_id,
                    item_code: itemCfg.item_code,

                    ex_node_type: itemCfg.ex_node_type,

                    x: selectPanel.x0,
                    y: selectPanel.y0,
                    width: Math.abs(spSz.width),
                    height: Math.abs(spSz.height)
                };
                

                me.baseAddChildByConfig(itemCfg, elemCfg);

                selectPanel.hide();

                var eventArgs = {
                    item_type: itemCfg.item_type,

                    id: itemCfg.id,
                    item_id: itemCfg.item_id,
                    item_code: itemCfg.item_code,

                    x: elemCfg.x,
                    y: elemCfg.y,
                    width: elemCfg.width,
                    height: elemCfg.height
                };

                me.on('itemCreated', eventArgs );
            }
            else {

                var elemCfg = {
                    id: itemCfg.id,
                    item_id: itemCfg.item_id,
                    item_code: itemCfg.item_code,
                    x: evt.stageX,
                    y: evt.stageY,
                    width: 80,
                    height: 40
                };

                me.baseAddChildByConfig(itemCfg, elemCfg);



                var eventArgs = {
                    item_type: itemCfg.item_type,

                    id: itemCfg.id,
                    item_id: itemCfg.item_id,
                    item_code: itemCfg.item_code,

                    x: elemCfg.x,
                    y: elemCfg.y,
                    width: elemCfg.width,
                    height: elemCfg.height
                };

                me.on('itemCreated', eventArgs);
            }

            return;
        }


        if (spVisible && (Math.abs(spSz.width) > 4 || Math.abs(spSz.height) > 4)) {
            me.proRectSelected();
            return;
        }

        selectPanel.hide();


        if (mdTarget && mdTarget.onPreEvent) {

            //console.log('FlowPageBuilder.stage_mouseup(...)  5 触发  ---- Text=' + mdTarget.text);

            //console.log(mdTarget);

            //var m2parent = mdTarget.parent;

            //var isMoved = rectSP.isMoved();

            //console.log('mdTarget.text = ' + mdTarget.text);
            //console.log('rectSP.isMoved()  = ' + isMoved);
            //console.log('m2parent = ' + m2parent);

            //if (isMoved && m2parent && m2parent != me) {

            //    console.log('FlowPageBuilder.stage_mouseup(...)  5 改变容器范围');

            //    m2parent.removeChild(mdTarget);

            //    me.addChild(mdTarget);
            //}

            mdTarget.onPreEvent.call(mdTarget, 'mouseup');

            me.mousedownTarget = null;


        }
        else if (evt.relatedTarget) {

            //console.log('FlowPageBuilder.stage_mouseup(...) 6 不触发  ---- Text=' + evt.relatedTarget.text + ', mdTarget = ' + mdTarget);

            var target = evt.relatedTarget,
                m2Obj = target.Mini2_Object;

            if (m2Obj && m2Obj.onPreEvent) {
                m2Obj.onPreEvent.call(m2Obj, 'mouseup');
            }
        }
        else {

            //console.log('FlowPageBuilder.stage_mouseup(...) 7');
            me.selecteds = [];
            
        }
    },



    //根据 x,y 获取坐标范围内的带容器的对象
    getContainerShapeForXY: function (x, y, lastShape, excludeShapes) {
        "use strict";
        var me = this,
            i,
            shape,resultShape = null,
            shapes = me.shapes,
            len = shapes.length;
       
        if (lastShape && lastShape.hitTest(x - lastShape.x, y - lastShape.y)) {
            return lastShape;
        }

        for (i = 0; i < len; i++) {
            shape = shapes[i];

            if (!shape.isContainer) {
                continue;
            }

            //如果是排除的就不选
            if (excludeShapes && Mini2.Array.contains(excludeShapes, shape)) {
                continue;
            }

            if (shape.hitTest(x - shape.x, y - shape.y)) {

                resultShape = shape;
                break;
            }

        }

        return resultShape;

    },

    //获取容器的子容器
    getContainerPanel: function (x, y) {
        "use strict";
        var me = this,
            selecteds = me.selecteds,
            old = me.curOverdragCont,
            oldShape = old.shape,
            oldCont = old.cont,
            curShape = null,
            curCont = null;


        curShape = me.getContainerShapeForXY(x, y, oldShape, selecteds);

        if (curShape) {
            curCont = curShape.testRollover(x, y, oldCont, old);

            curCont && curShape.showGroupMask(curCont);
        }

        if (oldCont && oldCont !== curCont) {
            oldShape.hideGroupMask(oldCont);
        }


        old.shape = curShape;
        old.cont = curCont;
    },




    stage_mousemove: function (evt) {
        "use strict";
        var me = this,
            selectPanel = me.selectPanel,
            mdTarget = me.mousedownTarget,
            selecteds = me.selecteds,
            stageX = evt.stageX,
            stageY = evt.stageY;

        if (selecteds && selecteds.length) {

            me.getContainerPanel(stageX, stageY);
            return;
        }

        //console.log('stageX=' + stageX + ', staageY=' + stageY);

        if (mdTarget) {
            return;
        }
        
        if (!selectPanel.getVisible()) {

            selectPanel.setPoint0(stageX, stageY).setVisible(true);

        }
        else {

            selectPanel.setPoint1(stageX, stageY);

        }     

    },

    item_hotRollout: function(sender, e){
        var me = this;

        me.curHotspotOver = null;
        me.curHotOwnerOver = null;

    },

    item_hotRollover: function (sender, e) {
        var me = this;

        me.curHotspotOver = e.hotspot;

        me.curHotOwnerOver = e.node;



    },

    //初始化热点层
    initHotspotPanel: function () {
        "use strict";
        var me = this,
            hotPanel,
            mainPanel = me.mainPanel;


        hotPanel = new createjs.Container();

        me.hotspotPanel = hotPanel;

        mainPanel.addChild(hotPanel);
    },

    initLineEditPanel: function () {
        "use strict";
        var me = this,
            mainPanel = me.mainPanel,
            lineSelectPanel;


        me.lineSelectPanel = lineSelectPanel = Mini2.create('Mini2.flow.LineSelectPanel', {
            page: me,
            isBuilder: true, //设计工具
            x: 100,
            y: 100,
            visible: false
        });
        lineSelectPanel.render();

        mainPanel.addChild(lineSelectPanel.el);

        lineSelectPanel.bind('changed', me.lineSelectPanel_Changed, null, me)
            .bind('mousedown', me.lineSelectPanel_MouseDown, null, me)
            .bind('mouseup', me.lineSelectPanel_MouseUp, null, me)
            .bind('remove', me.lineSelectPanel_Remove, null, me)
            .bind('insert', me.lineSelectPanel_Insert, null, me)
            .bind('dblclick', function(sender,evt){

                me.on('itemDblclick', evt);
            });

    },

    lineSelectPanel_Insert: function (sender, e) {
        "use strict";
        var me = this,
            line = me.curFocusObj,
            aLocal = line.getALocal();

        line.insert(e.index, e.x - aLocal.x, e.y - aLocal.y);
    },

    lineSelectPanel_Remove: function (sender, e) {
        "use strict";
        var me = this,
            line = me.curFocusObj;

        line.removePoint(e.index);

    },

    //断开这个对象的所有连线
    unlinkForHotAll: function (shape) {
        "use strict";
        var me = this,
            hot,
            i=0,
            hots = shape.hotspots,
            len = hots.length;


        for (; i < len; i++) {

            hot = hots[i];
            
            hot.unlinkAll();
        }

    },
    

    //断开与热点的连接
    // @endPointTag = end | start
    unlinkForHot: function (line, endPointTag, point) {
        "use strict";
        var me = this,
            oldNode,
            oldHot;

        if (!point) return;        

        oldNode = me.getNodeById(point.node_id);

        if (!oldNode) return;

        oldHot = oldNode.getHotById(point.hot_id);

        if (!oldHot) return;

        oldHot.unlink(endPointTag, line);
        
        return me;
    },







    lineSelectPanel_MouseUp: function (sender, e) {
        "use strict";
        var me = this,
            i,
            index = e.index,
            shape,
            shapes = me.shapes,
            curFocusObj,
            line = curFocusObj = me.curFocusObj;

        //line = curFocusObj;

        //断开末端,前端与热点的连接
        if (e.isLast) {

            var point = line.end_point;

            if (point != null) {
                var eventArgs = {
                    anchor_type: 'end',

                    line_id: line.id,
                    line_item_id: line.item_id,

                    src_item_id: point.item_id,
                    src_node_id: point.node_id,
                    src_hot_id: point.hot_id
                };

                me.unlinkForHot(line, 'end', point);
                line.setEndPoint(null);

                //触发线段断开事件
                me.on('lineUnlinked', eventArgs);
            }
        }
        else if (e.isFirst) {

            var point = line.start_point;

            if (point != null) {
                var eventArgs = {
                    anchor_type: 'start',


                    line_id: line.id,
                    line_item_id: line.item_id,

                    src_item_id: point.item_id,
                    src_node_id: point.node_id,
                    src_hot_id: point.hot_id
                };


                me.unlinkForHot(line, 'start', point);
                line.setStartPoint( null);

                //触发线段断开事件
                me.on('lineUnlinked', eventArgs);
            }
        }

        //连接新的热点
        if (me.curHotspotOver != null && (e.isLast || e.isFirst)) {
            var hot = me.curHotspotOver;

            var node = me.curHotOwnerOver;

            var pX = node.x + hot.x - line.x;
            var pY = node.y + hot.y - line.y;

            line.setPointByIndex(index, pX, pY);

            console.log('接入: node=' + node.id + ', hot=' + hot.id);

            if (e.isLast) {

                line.setEndPoint({
                    item_id: node.item_id,
                    node_id: node.id,
                    hot_id: hot.id,

                    item_code:node.item_code
                });

                hot.link('end', line);

                var eventArgs = {
                    anchor_type: 'end',

                    line_id: line.id,
                    line_item_id: line.item_id,

                    cur_item_id: node.item_id,
                    cur_node_id: node.id,
                    cur_hot_id: hot.id,
                    
                    cur_item_code:node.item_code
                };

                //触发线段连接事件
                me.on('lineLinked', eventArgs);
            }

            
            if (e.isFirst) {

                line.setStartPoint({
                    item_id: node.item_id,
                    node_id: node.id,
                    hot_id: hot.id,

                    item_code: node.item_code
                    
                });

                hot.link('start', line);


                var eventArgs = {
                    anchor_type: 'start',

                    line_id: line.id,
                    line_item_id: line.item_id,

                    cur_item_id: node.item_id,
                    cur_node_id: node.id,
                    cur_hot_id: hot.id,
                    cur_item_code: node.item_code
                };

                //触发线段连接事件
                me.on('lineLinked', eventArgs);
            }

        }
        else {

            //console.log('接入 ----------- : line=' + line);

            var aLocal = line.getALocal();

            line.setPointByIndex(index, e.toX - aLocal.x, e.toY - aLocal.y);
        }


        if (e.isFirst || e.isLast) {
            me.hideHotspotAll();
        }
    },

    lineSelectPanel_MouseDown: function (sender, e) {
        "use strict";
        var me = this,
            i,
            index = e.index,
            shape = me.curFocusObj,
            shapes = me.shapes;

        
        if (e.isFirst || e.isLast) {
            me.showHotspotAll(shape, shape.parent);
        }

    },

    //隐藏所有热点
    hideHotspotAll: function () {
        "use strict";
        var me = this,
            i,
            shape,
            shapes = me.shapes;

        for (i = 0; i < shapes.length; i++) {
            shape = shapes[i];
            if (shape.hideHotspot) {
                shape.hideHotspot();
            }
        }

        return me;
    },

    //显示所有热点
    showHotspotAll: function (item, owner) {
        "use strict";
        var me = this,
            i,
            shape,
            shapes,
            itemParent = false;

        if (owner) {
            shapes = me.shapes;
        }
        else {
            shapes = owner.shapes;
        }

        if (item) {
            itemParent = item.parent;
        }

        //console.log('热点数量: ' + shapes.length);

        for (i = 0; i < shapes.length; i++) {
            shape = shapes[i];

            if (item && itemParent != shape.parent) {
                //console.log("作废...");
                continue;
            }

            if (shape.showHotspot) {
                shape.showHotspot();
            }
        }

        return me;
    },

    lineSelectPanel_Changed:function(sender,e){
        var me = this,
            index = e.index,
            curFocusObj = me.curFocusObj;

        //console.log("me.curHotspotOver = " + me.curHotspotOver);
        
        if (me.curHotspotOver != null && (e.isLast || e.isFirst)) {
            var hot = me.curHotspotOver;
            
            var node = me.curHotOwnerOver;

            sender.setFixedPoint({
                x: node.x + hot.x ,
                y: node.y + hot.y
            });

            //curFocusObj.setPointByIndex(index, node.x + hot.x - curFocusObj.x, node.y + hot.y - curFocusObj.y);

        }
        else {

            sender.setFixedPoint(null);
            //curFocusObj.setPointByIndex(index, e.toX - curFocusObj.x, e.toY - curFocusObj.y);
        }

        //console.log("index = " + index + ", x=" + e.toX + ", y=" + e.toY );



    },

    initRectEditPanel:function(){
        var me = this,
            mainPanel = me.mainPanel,
            rectSP,
            childs = me.childs;



        me.rectSelectPanel = rectSP = Mini2.create('Mini2.flow.RectSelectPanel', {
            page:me,
            isBuilder: true, //设计工具
            x: 100,
            y: 100,
            visible: false
        });
        rectSP.render();

        mainPanel.addChild(rectSP.el);

        rectSP.bind('changed', me.rectSelectPanel_Changed, null, me);

        //绑定移动结束的事件
        rectSP.bind('moved', me.rectSelectPanel_Moved, null, me);

        rectSP.bind('dblclick', function (sender, e) {

            //console.log("双击........", e);

            me.on('itemDblclick', e);

        });
    },


    //移动结束的事件
    rectSelectPanel_Moved: function (sender, e) {
        "use strict";
        var me = this,
            rectSP = sender,
            i,
            ghost,
            ghosts = e.ghosts,
            shape,
            shapes = e.shapes,
            selecteds = me.selecteds,
            overDragTraget = me.curOverdragCont,    //移动上方的容器
            beginDragPanel = me.beginDragPanel || me,     //开始拖动的容器
            endDragPanel ;          //拖动结束的容器
        

        if (!overDragTraget.cont) {
            endDragPanel = me;
        }
        else {
            endDragPanel = overDragTraget.cont;
        }

        me.beginDragPanel = null;
        me.overDragTraget = null;

        //console.log('FlowPageBuilder.rectSelectPanel_Moved(...)');
        //console.log('move:' + e.moveX + ', ' + e.moveY);
        

        //console.log(endDragPanel);

        //console.log('isMove=' + rectSP.isMoved())
        //console.log('rectSP.getVisible() = ' + rectSP.getVisible());

        //console.log('开始容器是: ' + beginDragPanel.toString());
        //console.log('结束容器是: ' + endDragPanel.toString());


        if (beginDragPanel !== endDragPanel) {

            //console.log('FlowPageBuilder.rectSelectPanel_Moved(...)   3');

            var rect = rectSP.getGhostBounds();


            var xx = 0, yy = 0;
            var m2Obj ;    //容器

            if (overDragTraget.shape && overDragTraget.cont) {
                xx = overDragTraget.shape.x + overDragTraget.cont.x;
                yy = overDragTraget.shape.y + overDragTraget.cont.y

                m2Obj = overDragTraget.cont;
            }
            else {
                m2Obj = me;
            }

            

            var aX = rect.x - xx;
            var aY = rect.y - yy;


            //移动的距离
            var moveX = rectSP.ghost.el.x;
            var moveY = rectSP.ghost.el.y;


            //转移到新容器
            for (i = 0; i < selecteds.length; i++) {

                var shape = selecteds[i];
                var aLocal = shape.getALocal();

                var dx = aLocal.x + moveX - xx;
                var dy = aLocal.y + moveY - yy;


                var m2Parent = shape.parent;

                me.unlinkForHotAll(shape);  //断开所有连线

                //console.log('删除容器是: ' + m2Parent.toString());
                //console.log('添加容器是: ' + m2Obj.toString());

                m2Parent.removeChild(shape);

                m2Obj.addChild(shape);
                
                shape.setXY(dx, dy);
            }

            me.mousedownTarget = null;

            //rectSP.setVisible(false);

            rectSP.offset(moveX, moveY);

            //me.selecteds = [];
            me.mdTarget = null;



        }
        else {
            for (i = 0; i < shapes.length; i++) {
                shape = shapes[i];

                if (shape.offset) {
                    shape.offset(e.moveX, e.moveY).resetLinePoint();
                }
            }

            sender.offset(e.moveX, e.moveY);
        }


        me.curOverdragCont.shape = null;
        me.curOverdragCont.cont = null;


    },

    rectSelectPanel_Changed: function (sender, e) {
        "use strict";
        var me = this,
            ghost,
            ghosts = e.ghosts,
            len = ghosts.length,
            i,
            shape,
            shapes = e.shapes,
            rectSP = me.rectSelectPanel,
            curFocusObj = me.curFocusObj;


        console.log('FlowPageBuilder.rectSelectPanel_Changed(...)');



        for (i = 0; i < len; i++) {

            ghost = ghosts[i];

            shape = ghost.src;

            var parent = shape.parent;
            var pALocal;

            if (parent && parent.getALocal) {
                pALocal = parent.getALocal();
            }
            else {
                pALocal = { x: 0, y: 0 };
            }

            //console.log(pALocal);

            pALocal.x = ghost.x - pALocal.x;
            pALocal.y = ghost.y - pALocal.y;

            //console.log(pALocal);

            if (shape.setRect) {
                shape.setRect(pALocal.x, pALocal.y, ghost.width, ghost.height);
                shape.resetLinePoint();
            }

        }

        if (shapes.length > 1) {
            rectSP.setShapes(shapes, null);
        }
        else {
            rectSP.setShapes(shapes[0]);
        }
    },



    item_MouseDown: function (sender, e) {
        "use strict";
        var me = this,
            item = sender,
            lineSP = me.lineSelectPanel,
            rectSP = me.rectSelectPanel;

        //console.log('item_MouseDown ++++++++++++++++++++++++++++++');

        if (me.curFocusObj === item) {
            return;
        }


        me.curFocusObj = item;

        me.selecteds = [item];

        if (item.isLine) {

            var aLocal = item.getALocal();

            lineSP.curShape = item;

            lineSP.setPoints(item.getPoints(), aLocal.x, aLocal.y)
                .setVisible(true);


            rectSP.setVisible(false);
        }
        else {

            rectSP.setShapes(item).setVisible(true);

            lineSP.setVisible(false);
            

            me.stage._handleMouseDown(window.event);    //切换到Ghost焦点
        }



        //开始拖动的容器
        me.beginDragPanel = item.parent;

        me.curOverdragCont.shape = null;
        me.curOverdragCont.cont = null;

    },

    item_PressMove: function (sender, e) {
        "use strict";
        var me = this,
            item = sender,
            rectSP = me.rectSelectPanel,
            lineSelectPanel = me.lineSelectPanel;

        if (sender.isLine) {
         

        }
        else {
            rectSP.setXY(item.x, item.y);
        }
    },

    //当前鼠标所在的热点上方
    curHotspotOver: null,

    //设置线段的编辑动作
    setLineAction: function (actionName) {
        "use strict";
        var me = this,
            lineSelectPanel = me.lineSelectPanel;

        console.log('线编辑 action=' + actionName);

        lineSelectPanel.action = actionName;

        return me;
    },


    //删除全部
    removeAll: function () {

        var me = this,
            i,
            lines = me.lines,
            shapes = me.shapes,
            line,
            shape;

        for (i = 0; i < lines.length; i++) {
            line = lines[i];
            line.remove();
        }

        for (i = 0; i < shapes.length; i++) {
            shape = shapes[i];

            shape.remove();
        }

        return me;
    },


    removeChild: function (item) {
        var me = this;

        me.baseRemoveChild(item);
    },

    //删除选中的对象
    remoteSelecteds:function(){

        "use strict";
        var me = this,
            curFocusObj = me.curFocusObj,
            selects = me.selecteds,
            linePanel = me.lineSelectPanel,
            rectPanel = me.rectSelectPanel;

        if (selects.length) {

            for (var i = 0; i < selects.length; i++) {

                var item = selects[i];

                console.log(item);

                var eventArgs = {
                    item_type: item.isLine ? 'line' : 'node',
                    item_id: item.item_id,
                    item_code: item.item_code
                }

                item.remove()

                me.on('itemRemove', eventArgs);
            }

            linePanel.setVisible(false);
            rectPanel.setVisible(false);

            me.curFocusObj = null;

            selects.length = 0;
        }
        else if (curFocusObj && curFocusObj.remove) {

            if (curFocusObj.isLine) {
                linePanel.setVisible(false);

                var eventArgs = {
                    item_type: 'line',
                    item_id: curFocusObj.item_id,
                    item_code: curFocusObj.item_code
                }

                me.on('itemRemove', eventArgs);
            }
            else {
                rectPanel.setVisible(false);

                var eventArgs = {
                    item_type: 'node',
                    item_id: curFocusObj.item_id,
                    item_code: curFocusObj.id
                }

                me.on('itemRemove', eventArgs);
            }

            me.curFocusObj = null;

        }
    },

    _keydown_Shift :false,

    _keydown_alt : false,
    
    onKeydown: function (evt) {
        var me = this,
            keyCode = evt.keyCode;

        if (46 == keyCode) {
            console.log("点击删除...");
            me.keydown_delete(evt);
        }
        else if (16 == keyCode) {

            if (me._keydown_Shift == false && me.curFocusObj && me.curFocusObj.isLine) {
                me._keydown_Shift = true;

                console.log('添加点.');
                me.setLineAction('add');
            }
        }
        else if (18 == keyCode) {
            if (me._keydown_alt == false && me.curFocusObj && me.curFocusObj.isLine) {
                me._keydown_alt = true;

                console.log('删除点.');
                me.setLineAction('remove');
            }
        }

        //console.log('evt.keyCode = ', keyCode);

    },


    onKeyup: function (evt) {
        var me = this,
            keyCode = evt.keyCode;

        if (16 == keyCode) {
            me._keydown_Shift = false;
        }
        else if (18 == keyCode) {
            me._keydown_alt = false;            
        }

        if (me._keydown_Shift == false && me._keydown_alt == false) {
            console.log('移动点.');
            me.setLineAction('move');
        }

    },



    keydown_delete: function (evt) {
        var me = this;

        me.remoteSelecteds();
            
    },



    render: function () {
        var me = this;

        me.baseRender();


        me.proResize();
    },

    toString: function () {
        var me = this;

        return 'FlowPageBuilder';
    }


});