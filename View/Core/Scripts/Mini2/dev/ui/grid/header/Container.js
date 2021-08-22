/// <reference path="../../Mini.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />




Mini2.define('Mini2.ui.grid.header.Container', {

    //    extend: '',


    renderTo: Mini2.getBody(),


    log: Mini2.Logger.getLogger('Mini2.ui.grid.header.Container'),  //日志管理器

    height: 31,

    //每行的高度
    rowHeight: 31,

    width: 600,


    //拖拉线1
    m_divMarker1: null,
    //拖拉线2
    m_divMarker2: null,

    //当前移动的 head 对象索引
    m_CurMoveHead: null,

    //是否有出现拖拉想
    m_IsLineMove: false,

    //准备拖来列格的宽度
    headerElMoveWidth: 0,

    headerEl: null,

    innerEl: null,

    targetEl: null,


    //标题集合
    columns: [],

    curIndex: -1,

    baseCls: 'mi-column-header',

    m_ColumnWidthResize: Mini2.emptyFn,

    //明细列, 数组
    detailCols: false,

    //全部列, 数组
    allCols: false,

    /**
    * 列移动事件
    */
    columnMoved: Mini2.emptyFn,

    columnWidthResize: function (fn) {

        this.m_ColumnWidthResize = fn;
    },

    strJoin: function (array) {
        "use strict";
        var i,
            str = "",
            len = array.length;
        for (i = 0; i < len; i++) {
            str += array[i];
        }

        return str;
    },

    headTpl: [
        '<div class="mi-column-header mi-box-item mi-column-header-default mi-unselectable " ',
            'style="right: auto; left: 0px; top: 0px; margin: 0px; ">',
            '<div class="mi-column-header-inner ">',
                '<span class="mi-column-header-text">',

                '</span>',
            '</div>',
        '</div>'
    ],


    //复选框列
    checkColumns: [],

    //最后排序的列
    lastSortColumn: null,

    //创建菜单
    createMenu: function (col, headEl) {
        "use strict";
        var me = this,
        baseCls = me.baseCls,
        headerInnerEl;

        headerInnerEl = headEl.children(".mi-column-header-inner." + baseCls + "-inner");

        if (!col.menuDisabled && col.menu) {

            console.debug("headerInnerEl.len = ", headerInnerEl.length);

            var headerTriggerEl = $('<div class="' + baseCls + '-trigger" ></div>');

            headerInnerEl.append(headerTriggerEl);

            return;

            headerInnerEl.on('mousedown', function (e) {

                var x, w, left, col = $(this).data('me');

                if (col && col.resizable) {
                    left = $(this).offset().left;

                    x = event.clientX - left;

                    w = $(this).width();

                    if (x < w - 4) {
                        console.log("拖住...");
                        Mini2.EventManager.setMouseDown(this, e);
                    }
                    else {

                        console.log("点击...");
                    }
                }

            }).on('mouseup', function (sender, e) {

                //alert("显示下拉菜单 mouseup");

                //var menu = Mini2.create('Mini2.ui.menu.Menu', {

                //    items: [{
                //        text : '固定栏目'
                //    }, {
                //        text: '哈哈哈'
                //    }]
                //});

                //menu.render();


            });
        }

    },

    createColumn: function (col) {
        "use strict";
        var me = this,
            grid = me.grid,
            baseCls = me.baseCls,
            headEl,
            headerTextEl,
            headerInnerEl;


        headEl = $(me.headTpl)
            .addClass(baseCls + "-align-" + col.headerAlign)
            .data('me', col);

        me.renderHeaderText(col, grid, headEl);


        me.createMenu(col, headEl);

        me.head_bindEvent(headEl);

        if (col.required) {
            headEl.find('.mi-column-header-text').after('<span style="color:red;font-weight:bold" data-qtip="Required">*</span>');
        }

        return headEl;
    },


    //创建表头组的容器
    // curDept = 当前深度
    // maxDept = 最大深度
    createGroupColumn: function (col, curDept, maxDept) {
        "use strict";
        var me = this,
            log = Mini2.Logger,  //日志管理器
            grid = me.grid,
            baseCls = me.baseCls,
            headEl,
            headerInnerEl,
            curColEl,
            boxInnerEl;

        //style="cursor: col-resize;"


        headEl = $(me.headTpl)
            .addClass(baseCls + '-align-' + col.headerAlign)
            .addClass('mi-group-header')
            .data('me', col);

        me.renderHeaderText(col, grid, headEl);

        if (col.required) {
            headEl.find('.mi-column-header-text').after('<span style="color:red;font-weight:bold" data-qtip="Required">*</span>');
        }

        headerInnerEl = headEl.children('.mi-column-header-inner');

        boxInnerEl = $('<div class="mi-box-inner" role="presentation" ></div>');

        headEl.append(boxInnerEl);


        var i,
            left = 0,
            width = 0,
            height = 0,
            subCols = col.columns,
            subCol;

        for (i = 0; i < subCols.length; i++) {

            subCol = subCols[i];

            me.allCols.push(subCol);

            subCol.deptPath = col.deptPath + '.' + i;

            if (subCol.columns) {
                curColEl = me.createGroupColumn(subCol, curDept + 1, maxDept);
            }
            else {
                curColEl = me.createColumn(subCol);

                if (maxDept - curDept > 1) {
                    $(curColEl).css({
                        'padding-top': (me.rowHeight * (maxDept - curDept) - me.rowHeight) / 2
                    })
                }
            }

            subCol.el = curColEl;

            curColEl.css({
                left: left + 'px',
                width: (subCol.width) + 'px',
                height: me.rowHeight * (maxDept - curDept)

            }).addClass('mi-group-sub-header').appendTo(boxInnerEl);



            width += subCol.width;
            left += subCol.width;
        }

        col.width = width;
        col.height = me.rowHeight * (maxDept - curDept);


        boxInnerEl.css({
            height: col.height
        });


        me.createMenu(col, headEl);

        me.head_bindEvent(headEl);


        //log.debug('col.width = ' + width);


        return headEl;
    },


    renderHeaderText: function (col, grid, headEl) {
        "use strict";
        var me = this,
            baseCls = me.baseCls;

        var headerTextEl = headEl.children('.mi-column-header-inner').children('.' + baseCls + '-text:first');

        if (col.getHeaderConfig) {

            var headerConfig = col.getHeaderConfig();

            col = Mini2.apply(col, headerConfig);

            me.checkColumns.push(col);

            if (grid && 'SINGLE' == grid.checkedMode) {
                var txt = col.headerText;
                headerTextEl.html(txt);
            }
            else {
                var headerTl = col.renderer.call(col, false, grid);
                headerTextEl.append(headerTl);
            }
        }
        else {

            headerTextEl.html(col.headerText);
        }
    },


    mouse_button: null,


    isDragCol: false,

    /**
    * 拖拽到上方的元素
    */
    dragHoverEl: null,

    /**
    * 拖拽的元素
    */
    dragColEl: null,

    mouseDownLocal: null,


    startColDrag: function (evt, srcEvt) {
        var me = this;

        var hoverEl = srcEvt.target;    //鼠标所在的元素上方

        if (!$(hoverEl).hasClass('mi-column-header')) {
            var cellEl = $(hoverEl).parents('.mi-column-header');
            hoverEl = cellEl[0];
        }


        if (!hoverEl) {
            return;
        }

        me.isDragCol = true;
        me.dragHoverEl = hoverEl;

        var pLocal = me.innerEl.offset();

        var aLocal = $(hoverEl).offset();
        var aWidth = $(hoverEl).width();
        var aHeight = $(hoverEl).height();


        //console.debug("col left=", aLocal.left + aWidth);

        var colMoveTopEl = me.colMoveTopEl;

        var colMoveBottomEl = me.colMoveBottomEl;

        //创建指示
        if (!colMoveTopEl) {

            colMoveTopEl = Mini2.$joinStr([
                '<div class="mi-col-move mi-col-move-top" style="display:none;"></div>'
            ]);

            $(document.body).append(colMoveTopEl);
            me.colMoveTopEl = colMoveTopEl;
        }

        if (!colMoveBottomEl) {

            colMoveBottomEl = Mini2.$joinStr([
                '<div class="mi-col-move mi-col-move-bottom" style="display:none;"></div>'
            ]);

            $(document.body).append(colMoveBottomEl);
            me.colMoveBottomEl = colMoveBottomEl;
        }

        me.colMoveTopEl.show();
        me.colMoveBottomEl.show();

        var left = aLocal.left + aWidth - 5;

        colMoveTopEl.css({
            'top': aLocal.top - 11,
            'left': left
        });


        colMoveBottomEl.css({
            'top': aLocal.top + me.headerEl.height(),
            'left': left
        });



    },

    endColDrag: function (target) {
        var me = this;

        var itemA = target;
        var itemB = me.dragHoverEl;

        var aIndex = $(itemA).index();
        var bIndex = $(itemB).index();

        if (aIndex === bIndex || aIndex - 1 === bIndex) {
            return;
        }

        me.columnMoved.call(me, {

            fromIndex: aIndex,
            toIndex: bIndex,

        });

    },


    //绑定 Head 事件
    head_bindEvent: function (headEl) {
        "use strict";
        var me = this;

        headEl.muBind('mousedown', function (evt, srcEvt) {

            me.mouse_button = evt.button;

            if (0 === me.mouse_button) {


                var col = $(this).data('me');


                if (col.isCheckerHd) {
                    me.dragColEl = null;
                    return;
                }

                var pX = 0; //子元素触发后, 没有预定的偏移量

                if (!$(evt.target).hasClass('mi-column-header')) {
                    
                    var pList = $(evt.target).parentsUntil('.mi-column-header');
                    
                    var pp = $(evt.target).position();
                    pX += pp.left;
                    
                    $(pList).each(function () {
                        pp = $(this).position();
                        pX += pp.left;
                    });
                    
                }

                var w = $(this).width();



                //console.log("header 事件 ", evt);
                //console.log("header 事件 ", col);
                //console.log("列头 down", evt.offsetX);
                //console.log("xxx", w);

                var evtX = evt.offsetX + pX;

                if (evtX > 4 && evtX < w - 4) {

                    //console.debug("点击 offset= (%d, %d)", evt.offsetX, evt.offsetY);

                    me.mouseDownLocal = {
                        x: evt.offsetX,
                        y: evt.offsetY
                    };

                    me.dragColEl = this;
                }
                else {
                    me.dragColEl = null;
                }


                Mini2.ui.FocusMgr.setControl(col, null, this,true);

            }

        }).muBind('mousemove', function (evt, srcEvt) {

            if (0 === me.mouse_button) {

                //console.debug("移动 offset= (%d, %d)", srcEvt.offsetX, srcEvt.offsetY);

                if (me.dragColEl) {
                    var isDrag;

                    var cX = Math.abs(me.mouseDownLocal.x - srcEvt.offsetX);
                    var cY = Math.abs(me.mouseDownLocal.y - srcEvt.offsetY);

                    isDrag = (cX > 8) || (cY > 8);

                    if (isDrag) {
                        me.startColDrag(evt, srcEvt);
                    }
                }

            }


        }).muBind('mouseup', function (sender, evt) {

            if (0 === me.mouse_button) {

                if (me.colMoveTopEl) {
                    me.colMoveTopEl.hide();
                }

                if (me.colMoveBottomEl) {
                    me.colMoveBottomEl.hide();
                }

                var targetEl;

                //console.debug("me.isDragCol ", me.isDragCol);
                //console.debug("dragColEl is ", me.dragColEl?true:false);


                if (me.isDragCol) {

                    me.endColDrag(this);
                }
                else {

                    //console.debug("col....", evt);
                    
                    if (me.dragColEl && $(evt.target).hasClass('mi-column-header-trigger')) {

                        targetEl = evt.target;

                        var col = $(this).data('me');

                        col.triggerClick.call(col);

                    }
                    else {
                        me.head_click.call(this, this, evt, me);
                    }

                }


                //console.log("列头 up", $(this).find('.mi-column-header-text').text());


            }


            me.isDragCol = false;
            me.dragHoverEl = null;

            me.mouse_button = null;

        }).muBind('dblclick', function (sender, e) {

            me.head_dblclick.call(this, sender, e, me);

        });
    },




    //表头单击
    head_click: function (sender, e, owner) {
        "use strict";
        var me = owner,
            checkedCls = 'mi-grid-checkcolumn-checked',

            imgEl, check,

            col = $(this).data('me');

        if (!me.m_IsLineMove) {

            //console.log("col ", col);

            //console.log("点击表头, col.isCheckerHd = ", col.isCheckerHd);

            Mini2.ui.FocusMgr.setControl(this);

            if (col && col.isCheckerHd) {

                imgEl = $(this).find('img:first');
                imgEl.toggleClass(checkedCls);

                check = $(imgEl).hasClass(checkedCls);

                col.onHeaderClick(this, col, check);

                Mini2.EventManager.stopEvent(e);
            }
        }
    },

    //表头双击
    head_dblclick: function (sender, e, owner) {
        "use strict";
        var me = owner,
            grid = me.grid,
            baseCls = me.baseCls,
            headerSortCls = baseCls + '-sort',
            checkedCls = 'mi-grid-checkcolumn-checked',
            descCls = headerSortCls + '-DESC',
            ascCls = headerSortCls + '-ASC',

            col = $(this).data("me");

        if (!me.m_IsLineMove) {

            Mini2.ui.FocusMgr.setControl(this);


            if (!col.isCheckerHd) {
                //alert("排序字段" + me.m_IsLineMove);

                if (!grid.sortable || !col.sortable) {
                    return;
                }

                if (col.columns && col.columns.length) {
                    return;
                }

                if (me.lastSortColumn === this) {
                    col.doSort();
                }
                else {
                    col.doSort('ASC');
                }


                $(me.lastSortColumn).removeCls(descCls, ascCls);

                if ('ASC' == col.direction) {
                    $(this).addClass(descCls);
                }
                else if ('DESC' == col.direction) {
                    $(this).addClass(ascCls);
                }

                me.lastSortColumn = this;

                Mini2.EventManager.stopEvent(e);

            }
        }
    },


    setHeaderCheck: function (isChecked) {
        "use strict";
        var me = this,
            col,
            cols = me.checkColumns,
            i,
            imgEl, colEl,
            targetEl = me.targetEl,
            checkcolumnCheckedCls = 'mi-grid-checkcolumn-checked',
            checkcolumnUndefined = 'mi-grid-checkcolumn-undefined';

        for (i = 0; i < cols.length; i++) {

            col = cols[i];

            colEl = $(targetEl).children(":eq(" + col.index + ")");

            imgEl = $(colEl).find("img:first");

            if (undefined === isChecked) {
                $(imgEl).removeClass(checkcolumnCheckedCls);
                $(imgEl).addClass(checkcolumnUndefined);
            }
            else {
                $(imgEl).removeClass(checkcolumnUndefined);
                $(imgEl).toggleClass(checkcolumnCheckedCls, isChecked);
            }
        }

    },

    // 计算标题行数
    getRowCount: function (columns, rowIndex) {
        "use strict";
        var me = this,
            cols = columns,
            i,
            curDept,
            col;

        rowIndex = rowIndex || 1;

        if (cols && cols.length) {

            var count = 0;

            //计算标题行数
            for (i = 0; i < cols.length; i++) {
                col = cols[i];

                if (col.columns) {
                    curDept = me.getRowCount(col.columns, rowIndex + 1);

                    count = Math.max(count, curDept);
                }

            }


            rowIndex = Math.max(rowIndex, count);
        }

        return rowIndex;
    },


    //填充明细列的信息,也就是最底层的
    fullDetailCols: function (detailCols, cols, dept) {
        "use strict";
        var me = this,
            i,
            col,
            subColumns;

        if (cols && cols.length) {

            //计算标题行数
            for (i = 0; i < cols.length; i++) {
                col = cols[i];
                subColumns = col.columns;

                if (subColumns && subColumns.length) {
                    me.fullDetailCols(detailCols, subColumns, dept + 1);
                }
                else {
                    detailCols.push(col);
                }

            }
        }

        return detailCols.length;
    },


    createColumns: function (headerCtEl) {
        "use strict";

        var me = this,
            i,
            curColEl,
            col,
            baseCls = me.baseCls,
            cols = me.columns,
            headHtml,
            left = 0,
            width = 0;


        var boxInnerEl = headerCtEl.cFirst(".mi-box-inner");

        var boxTargetEl = $(boxInnerEl).cFirst(".mi-box-target");


        var rowCount = me.getRowCount(me.columns);

        me.fullDetailCols(me.detailCols, me.columns, 1);


        me.height = me.rowHeight * rowCount - rowCount - 1;

        headerCtEl.height(me.height);
        boxInnerEl.height(me.height);


        for (i = 0; i < cols.length; i++) {
            col = cols[i];
            col.ownerParent = me.grid;

            me.allCols.push(col);

            if (col.isCheckerHd) {
                me.checkColumns.push(col);
            }

            col.deptPath = i + '';

            if (col.columns) {
                curColEl = me.createGroupColumn(col, 1, rowCount);
            }
            else {
                curColEl = me.createColumn(col);

                $(curColEl).css({
                    'padding-top': parseInt((me.height - me.rowHeight) / 2)
                });
            }

            col.el = curColEl;

            boxTargetEl.append(curColEl);

            //log.debug('left = ' + left + ', 【width】 = ' + col.width + ', col.deptPath = ' + col.deptPath);

            $(curColEl).css({
                left: left + "px",
                height: me.height + "px",
                width: (col.width - 1) + "px"
            });

            if (col.visible) {
                left += col.width;
                width += col.width;
            }
        }

        me.width = width;


        return headerCtEl;
    },


    createMarker: function () {
        "use strict";
        /// <summary>创建拖动条</summary>
        var me = this,
            divHtml,
            dm1 = me.m_divMarker1,
            dm2 = me.m_divMarker2;

        if (dm1 != null && dm2 != null) {
            return;
        }

        divHtml = '<div class="mi-grid-resize-marker" style="top:0;right: auto; left: -9999px; pointer-events: none;" ></div>';

        me.m_divMarker1 = dm1 = $(divHtml);
        me.m_divMarker2 = dm2 = $(divHtml);


        $(me.renderTo).append([dm1, dm2]);
    },

    onInit: function () {
        "use strict";
        var me = this;

        me.detailCols = me.detailCols || [];
        me.allCols = me.allCols || [];

        me.headTpl = me.strJoin(me.headTpl);
    },

    render: function () {
        "use strict";
        var me = this,
            baseCls = me.baseCls,
            headGroupsEl,
            boxInnerEl,
            boxTargetEl;


        me.onInit();

        var headerCtEl = Mini2.$join([
            '<div unselectable="on" >',
                '<div class="mi-box-inner" role="presentation" style="width: 644px; height: 29px;  overflow: visible;">',
                    '<div class="mi-box-target" style="width: 644px; "></div>',
                '</div>',
            '</div>']);

        headerCtEl.addCls(
            baseCls + "-ct",
            baseCls + "-ct-docked-top",
            baseCls + "-ct-default-docked-top",
            baseCls + "-ct-default",
            "mi-box-layout-ct",
            "mi-noborder-rl");


        $(me.renderTo).append(headerCtEl);

        me.createColumns(headerCtEl);


        boxInnerEl = $(headerCtEl).cFirst(".mi-box-inner");
        boxTargetEl = $(boxInnerEl).cFirst(".mi-box-target");



        me.headerEl = headerCtEl;
        me.innerEl = boxInnerEl;
        me.targetEl = boxTargetEl;


        me.IntiHeaderInner();
        me.InitHeadGroups();

        me.resetWidth();
    },

    setWidth: function (value) {
        "use strict";
        var me = this;

        me.width = value;


        me.innerEl.css("width", value - 1 - 16);
        me.targetEl.css("width", value);

        return me;
    },

    scrollLeft: function (x) {

        var me = this;

        //me.innerEl.scrollLeft(x);
        me.innerEl.css('left',-x);

        return me;
    },

    preventDefault: function (e) {
        e = e || window.event;
        if (e.preventDefault) {
            e.preventDefault();

        } else {
            e.returnValue = false;
        }
    },



    StartHeadDrog: function (e) {
        "use strict";
        /// <summary>开始调整列宽度</summary>
        var me = this,
            curMoveHead = $(me.m_CurMoveHead),
            renderTo = me.renderTo,
            headerEl = me.headerEl,
            conWidth,
            conLeft,
            x,
            h;

        me.createMarker();

        me.m_IsLineMove = true;
        me.curIndex = curMoveHead.index();

        conWidth = curMoveHead.width();
        conLeft = curMoveHead.offset().left - renderTo.offset().left;

        x = e.clientX - conLeft;

        h = renderTo.height() + headerEl.height();


        me.m_divMarker1.css({
            left: conLeft,
            top: 0,
            height: h
        });

        me.m_divMarker2.css({
            left: conLeft + conWidth,
            top: 0,
            height: h
        });


        me.headerElMoveWidth = conWidth;

        Mini2.ui.FocusMgr.setControl(headerEl);
    },


    HeadDroging: function (e, objOver) {
        "use strict";
        /// <summary>拖放过程</summary>
        /// <param name="e" type="object">事件源</param>
        /// <param name="objOver" type="object">拖到到上面的对象</param>

        var me = this,
            overEl = $(objOver),
            conWidth = overEl.width(),
            conLeft = overEl.offset().left,
            x,
            x1, x2, span, div2Left;

        x = e.clientX + conLeft;

        x1 = me.m_divMarker1.offset().left;
        x2 = e.clientX;

        span = x2 - x1;

        if (span > 4) {

            div2Left = x2 - $(me.renderTo).offset().left;

            me.m_divMarker2.css({ left: div2Left });

            me.headerElMoveWidth = x2 - x1;
        }
    },

    EndHeadDrog: function (e, objOver) {
        "use strict";
        var me = this,
            col,
            dm1 = me.m_divMarker1,
            dm2 = me.m_divMarker2,
            curMoveHead = me.m_CurMoveHead,
            moveWidth = me.headerElMoveWidth;

        if (dm1 && dm2) {

            dm1.css({ left: -9999 });
            dm2.css({ left: -9999 });


            $(curMoveHead).css({ width: moveWidth });

            me.m_IsLineMove = false;
            me.m_CurMoveHead = null;

            col = $(curMoveHead).data('me');

            if (col) {
                col.width = moveWidth;


                me.resetSubColumnWidth(col);    //重置子节点的宽度

                if (col.parent) {
                    me.resetSubColumnWidth(col.parent);
                }

            }

            me.resetWidth(col);

            me.setWidth(me.grid.width);
        }


    },

    // 重置子节点的宽度
    resetSubColumnWidth: function (col) {
        "use strict";
        var me = this,
            cols = col.columns,
            subCol,
            len,
            width = col.width,
            el,
            left = 0,
            vWidth = width,
            i;



        if (cols && cols.length) {

            len = cols.length;


            for (i = 0; i < len - 1; i++) {

                subCol = cols[i];

                subCol.el.css({
                    left: left
                });

                if (subCol.visible) {
                    vWidth -= subCol.width;

                    left += subCol.width;
                }
            }

            subCol = cols[len - 1];

            subCol.width = vWidth;

            el = subCol.el;

            el.css({
                left: left,
                width: vWidth
            });

            me.resetSubColumnWidth(subCol);
        }

    },





    InitHeadGroups: function () {
        "use strict";
        var me = this;

        var boxInner = $(me.headerEl).find(".mi-box-inner:first");

        boxInner.mousemove(function (e) {

            if (me.m_IsLineMove) {

                me.HeadDroging(e, this);

                me.preventDefault(e);
            }
        })
        .mouseup(function (e) {

            if (me.m_IsLineMove) {
                me.EndHeadDrog(e, this);
            }

            me.preventDefault(e);
        });
    },

    IntiHeaderInner: function () {
        "use strict";
        var owner = this,
            log = owner.log,
            headerEl = $(owner.headerEl),
            baseCls = owner.baseCls,
            headerInnerEls = headerEl.find('.' + baseCls + '-inner'),
            headerList = headerEl.find('.' + baseCls);

        //log.debug('headerInnerEls = ' + headerInnerEls.length);
        //log.debug('baseCls = ' + baseCls);


        //菜单样式
        $(headerInnerEls).muBind('mouseenter', function () {
            $(this).addClass("mi-column-header-over");
        })
        .muBind('mouseleave', function () {
            $(this).removeClass("mi-column-header-over");
        });



        $(headerList)
        .muBind('mousedown', function (evt) {


            //console.log("header target 事件 ", evt);

            if (owner.m_CurMoveHead != null) {

                Mini2.EventManager.drag = true;

                owner.StartHeadDrog(evt);
            }

            Mini2.EventManager.stopEvent(evt);

        })
        .muBind('mouseup', function (sender, e) {

            owner.m_IsLineMove = false;

            Mini2.EventManager.drag = false;

            owner.EndHeadDrog(e, this);
        })
        .muBind('mousemove', function (sender, e) {

            var me = $(this);

            if (owner.m_IsLineMove) {

                owner.HeadDroging(e, this);
                Mini2.EventManager.stopEvent(e);
                return;
            }

            var downCon = me;

            var conWidth = me.width() + 20;
            var conLeft = me.offset().left;

            var x = e.clientX - conLeft;

            var curMove1 = false,
                curMove2 = false,
                col = me.data('me');

            var n = me.index();


            if (x <= 4) {

                if (n > 0) {
                    n = me.prev().index();
                    //col = owner.columns[n];

                    if (col && col.resizable && me.prev().index() > 0) {
                        me.css("cursor", "col-resize");

                        owner.m_CurMoveHead = me.prev();
                        curMove1 = true;
                    }
                }
            }
            else if (x >= 20) {
                //log.debug("x = " + x);
            }
            else {
                me.css("cursor", "");
            }

            //n = me.index();
            //col = owner.columns[n];

            if (col.resizable && x > conWidth - 24) {
                //me.css("cursor", "col-resize");
                me.find('.' + baseCls + '-trigger:first').css("cursor", "col-resize");

                owner.m_CurMoveHead = me;
                curMove2 = true;

            }
            else {
                //me.css("cursor", "");
                me.find('.' + baseCls + '-trigger:first').css("cursor", "");
            }


            if (curMove1 == false && curMove2 == false) {
                owner.m_CurMoveHead = null;
            }

            Mini2.EventManager.stopEvent(e);
        });



    },

    clear: function () {
        "use strict";
        var me = this,
            targetEl = me.targetEl;

        $(targetEl).children().remove();
    },

    /**
    * 添加列集合
    *
    * @param {Array} columns 列集合
    */
    add: function (columns) {
        "use strict";
        var me = this,
            log = me.log,
            i,
            left = 0,
            width = 0,
            cols = columns,
            headerEl = me.headerEl,
            innerEl = me.innerEl,
            targetEl = me.targetEl;



        me.columns = [];
        me.checkColumns = [];
        me.detailCols = [];

        me.columns = columns;


        me.createColumns(headerEl);



        me.IntiHeaderInner();
        me.InitHeadGroups();

        me.resetWidth();


        return me;
    },


    /**
    * 重新设置标头宽度
    */
    resetWidth: function (curCol) {
        "use strict";
        var me = this,
            x = 0,
            n = 0,
            i, col, headerEl,
            colEl,
            cols = me.columns,
            len = cols.length;


        for (i = 0; i < len; i++) {
            col = cols[i];

            if (col.visible) {
                colEl = col.el;

                colEl.css('left', x);

                x += col.width;
            }
        }

        if (curCol) {
            me.m_ColumnWidthResize(curCol);
        }


    }
})