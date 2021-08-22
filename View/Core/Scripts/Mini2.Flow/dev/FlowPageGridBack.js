

//网格背景
Mini2.define('Mini2.flow.FlowPageGridBack', {

    width: 100,

    height:100,

    gridSize: 20,

    colCount: 0,

    rowCount: 0,

    el:null,

    initComponent: function () {
        var me = this;

        me.el = new createjs.Shape();


    },

    setSize:function(w,h){
        var me = this;

        me.width = w;
        me.height = h;
    },

    setWidth: function(value){
        var me = this;

        me.width = value;
    },

    setHeight:function(value){
        var me = this;

        me.height = value;
    },

    setGridSize:function(value){
        var me = this;

        me.gridSize = value;
    },

    onResize:function(){
        var me = this;

        me.refresh();
    },


    //刷新
    refresh: function () {
        "use strict";
        var me = this,
            el = me.el,
            g = el.graphics,
            w = me.width,
            h = me.height,
            gridSz = me.gridSize,
            mCol,mRow,
            col, row,
            x,y,
            x0,y0, x1,y1;

        mCol = w % gridSz;
        mRow = h % gridSz;

        col = (w - mCol) / gridSz;
        row = (h - mRow) / gridSz;

        me.colCount = col;
        me.rowCount = row;

        //console.log("col=" + col + ", row=" + row);


        g.setStrokeStyle(1);
        g.beginStroke('#EEEEEE');

        for (x = 0; x < col; x++) {

            x0 = (x + 1) * gridSz + 0.5;
            y0 = 0;
            x1 = x0;
            y1 = h;

            g.moveTo(x0, y0);
            g.lineTo(x1, y1);

        }

        for (y = 0; y < row; y++) {
            x0 = 0;
            y0 = (1 + y ) * gridSz + 0.5;
            x1 = w;
            y1 = y0;

            g.moveTo(x0, y0);
            g.lineTo(x1, y1);

        }

    }


}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});