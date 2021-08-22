/// <reference path="/Core/Scripts/Zepto/zepto.1.1.6.js" />


Mini2.define('Mini2.ui.Picker', {
    extend: 'Mini2.ui.Component',

    title: '请选择',

    initComponent: function () {
        var me = this,
            el = me.el;



    },


    /**
    * columns: [{
    *    field:'add_1'
    * },{
    *    field:'add_2'
    * },{
    *    field:'add_3'
    * }
    *
    */
    cols: [{
        values: ['广东省']
    },{
        values: ['广州市']
    }, {
        values: ['天河区','海珠区','荔湾区','越秀区','番禺区','花都区','萝岗区','白云区','南沙区','黄埔区', '增城市','从化市','广州大学城']
    }],


    /**
    * 设置值
    *
    */
    setValue:function(value){
        var me = this,
            values,
            col,
            cols = me.cols;

        values = value.split(' ');

        for (var i = 0; i < values.length; i++) {

            var vStr = values[i];

            col = cols[i];
            col.value = vStr;

            me.selectItem(i, vStr);
            
        }

        return me;
    },

    getValue:function(){
        var me = this,
            valueStr = '',
            col,
            cols = me.cols || [],
            len = cols.length;


        for (var i = 0; i < len; i++) {

            if (i > 0) {
                valueStr += ' ';
            }

            col = cols[i];
            valueStr += col.value;

        }

        return valueStr;

    },
    
    /*
    * 当前移动的焦点列
    */
    curCol:null,


    onInit: function () {
        var me = this;

        //me.columns = [];

    },



    isPreLoaded: false,

    onPreLoad: function () {
        var me = this;
        if (!me.isPreLoaded) {
            me.isPreLoaded = true;

            if (me.onLoad) {
                me.onLoad();
            }
        }
    },


    /**
    * 根据 value 设置行焦点
    * 
    * @param {Number} colIndex 列索引
    * @param {Number} colEl 列对象
    * @param {Number} itemIndex 条目索引
    */
    selectItem:function(colIndex, itemValue){

        var me = this,
            cols = me.cols,
            col = cols[colIndex],
            innerEl = me.innerEl,
            colEl ;


        var colEls = $(innerEl).children('.picker-items-col')


        //transform: 
        colEl = colEls[colIndex];

        var wrapperEl = $(colEl).children('.picker-items-col-wrapper');

        var itemEls = $(wrapperEl).children('.picker-item');

        var curItemEl = null;
        var curIndex = -1;

        for (var i = 0; i < itemEls.length; i++) {

            var itemEl = itemEls[i];

            var v = $(itemEl).attr('data-picker-value');

            if (v == itemValue) {
                curIndex = i;
                curItemEl = itemEl;
                break;
            }
        }
        
        curIndex = me.selectItemForIndex(colIndex, colEl, curIndex);

        var newY = curIndex * 36;

        col.local.y = -newY;

        $(wrapperEl).css('transform', 'translate3d(0px, ' + (col.local.y + 72 + 7) + 'px, 0px)')

    },

    /**
    * 根据 ItemId 设置行焦点
    * 
    * @param {Number} colIndex 列索引
    * @param {Number} colEl 列对象
    * @param {Number} itemIndex 条目索引
    */
    selectItemForIndex:function(colIndex,colEl, itemIndex){
        var me = this,
            col,
            cols = me.cols;


        //transform: 
        var wrapperEl = $(colEl).children('.picker-items-col-wrapper');

        col = cols[colIndex];

        var itemEls = $(wrapperEl).children('.picker-item');

        if (itemIndex < 0) {
            itemIndex = 0;
        }
        else if (itemIndex >= itemEls.length) {
            itemIndex = itemEls.length - 1;
        }

        var lastItemIndex = col.item_index;
        col.item_index = itemIndex;

        var itemEl = itemEls[itemIndex];
        var lastItemEl = itemEls[lastItemIndex];

        $(lastItemEl).removeClass('picker-selected');
        $(itemEl).addClass('picker-selected');

        col.value = $(itemEl).attr('data-picker-value');   //value

        console.debug("itemEl index = ", itemEls.length);

        return itemIndex;
    },


    baseRender: function () {
        "use strict";
        var me = this,
            el ,
            headerEl,
            closeBtnEl, //关闭按钮
            innerEl,
            cols = me.cols || [];

        for (var i = 0; i < cols.length; i++) {

            var col = cols[i];

            Mini2.apply(col, {

                index : i,
                item_index: -1,

                mouse_down: { x: 0, y: 0 },
                mouse_move: { x: 0, y: 0 },
                src_mouse_move: { x: 0, y: 0 },
                src_local: { x: 0, y: 0 },
                local: { x: 0, y: 0 }

            });

        }



        me.el = el = me.$join([
            '<div class="picker-modal picker-columns  remove-on-close" style="display:none;">',
                '<header class="bar bar-nav">',
                    '<button class="button button-link pull-left close-picker" data-cmd="cancel">取消</button>',
                    '<button class="button button-link pull-right close-picker" data-cmd="ok" >确定</button>',
                    '<h1 class="title">', me.title, '</h1>',
                '</header>',

                '<div class="picker-modal-inner picker-items">',
                    //'<div class="picker-items-col picker-items-col-center ">',
                    //    '<div class="picker-items-col-wrapper" style="transform: translate3d(0px, 72px, 0px);">',
                    //        '<div class="picker-item " data-picker-value="iPhone 4">iPhone 4</div>',
                    //        '<div class="picker-item" data-picker-value="iPhone 4S">iPhone 4S</div>',
                    //        '<div class="picker-item" data-picker-value="iPhone 5">iPhone 5</div><div class="picker-item" data-picker-value="iPhone 5S">iPhone 5S</div><div class="picker-item" data-picker-value="iPhone 6">iPhone 6</div><div class="picker-item" data-picker-value="iPhone 6 Plus">iPhone 6 Plus</div><div class="picker-item" data-picker-value="iPad 2">iPad 2</div><div class="picker-item" data-picker-value="iPad Retina">iPad Retina</div><div class="picker-item" data-picker-value="iPad Air">iPad Air</div><div class="picker-item" data-picker-value="iPad mini">iPad mini</div><div class="picker-item" data-picker-value="iPad mini 2">iPad mini 2</div><div class="picker-item" data-picker-value="iPad mini 3">iPad mini 3</div>',
                    //    '</div>',
                    //'</div>',
                    //'<div class="picker-items-col picker-items-col-center ">',
                    //    '<div class="picker-items-col-wrapper" style="transform: translate3d(0px, 72px, 0px);">',
                    //        '<div class="picker-item" data-picker-value="iPhone 4">iPhone 4</div>',
                    //        '<div class="picker-item" data-picker-value="iPhone 4S">iPhone 4S</div>',
                    //        '<div class="picker-item" data-picker-value="iPhone 5">iPhone 5</div><div class="picker-item" data-picker-value="iPhone 5S">iPhone 5S</div><div class="picker-item" data-picker-value="iPhone 6">iPhone 6</div><div class="picker-item" data-picker-value="iPhone 6 Plus">iPhone 6 Plus</div><div class="picker-item" data-picker-value="iPad 2">iPad 2</div><div class="picker-item" data-picker-value="iPad Retina">iPad Retina</div><div class="picker-item" data-picker-value="iPad Air">iPad Air</div><div class="picker-item" data-picker-value="iPad mini">iPad mini</div><div class="picker-item" data-picker-value="iPad mini 2">iPad mini 2</div><div class="picker-item" data-picker-value="iPad mini 3">iPad mini 3</div>',
                    //    '</div>',
                    //'</div>',
                '</div>',
                '<div class="picker-center-highlight"></div>',
                '</div>',
            '</div>'
        ]);

        headerEl = el.children('header.bar')
        closeBtnEl = headerEl.children('.close-picker');




        innerEl = el.children('.picker-modal-inner');

        for (var i = 0; i < cols.length; i++) {

            var col = cols[i];

            var values = col.values;

            var html = '<div class="picker-items-col picker-items-col-center ">';
            html += '<div class="picker-items-col-wrapper" style="transform: translate3d(0px, 72px, 0px);">';

            for (var j = 0; j < values.length; j++) {

                var v = values[j];

                html += '<div class="picker-item " data-picker-value="' + v + '">' + v + '</div>';

            }

            html += '</div>';
            html += '</div>';

            innerEl.append(html);
        }





        me.headerEl = headerEl;
        me.innerEl = innerEl;

        $(closeBtnEl).on('click', function (evt) {

            var cmd = $(this).attr('data-cmd');

            if ('ok' == cmd && me.closed) {
                me.closed.call(me);
            }

            me.close();
        });

        if (me.renderTo) {
            $(me.renderTo).append(el);
        }


        var colEls = $(innerEl).children('.picker-items-col')

        $(colEls).on('touchstart', function (evt) {

            var touche0 = evt.touches[0];

            var cx = touche0.clientX;
            var cy = touche0.clientY;

            var index = $(this).index();

            var col = me.cols[index];

            me.curCol = col;

            col.mouse_down = {
                x: cx,
                y: cy
            };

            col.mouse_move = { x: 0, y: 0 };
            col.src_mouse_move = { x: 0, y: 0 };
            
            col.curStep = { x: 0, y: 0 };

            console.log("Touch started (%d, %d)", cx, cy);

        });


        $(colEls).on('touchmove',  function (evt) {

            var col = me.curCol;

            var touche0 = evt.touches[0];

            //var cx = touche0.clientX;
            var cy = touche0.clientY;


            //从点击,到现在的移动的范围
            var my = cy - col.mouse_down.y;           

            //计算出步骤
            var sy = my - col.src_mouse_move.y;

            col.src_mouse_move.y = my;


            col.curStep.y = sy;

            col.local.y += sy;

            
            var myy = col.local.y % 36;
            console.log("y = %d, m = %d", col.local.y, myy);

            //console.log("Touch move (%d, %d) -> 距离=(%d, %d), 上一次移动距离=(%d, %d) local.y = %d  ---- y=", 0, cy, 0, my, 0, sy, col.local.y);
            

            //transform: 
            var wrapperEl = $(this).children('.picker-items-col-wrapper');



            var my = col.local.y % 36;

            var curIndex = (col.local.y - my) / 36;

            if (my > 18) {
                curIndex++;
            }
            else if (my < -18) {
                curIndex--;
            }


            me.selectItemForIndex(col.index, this, -curIndex);



            //var itemEls = $(wrapperEl).children();

            //if (curIndex > 0) {
            //    curIndex = 0;
            //}
            //else if (curIndex <= -itemEls.length) {
            //    curIndex = -(itemEls.length - 1);
            //}

            //var lastItemIndex = col.item_index;
            //col.item_index = -curIndex;

            //var itemEl = itemEls[col.item_index];
            //var lastItemEl = itemEls[lastItemIndex];

            //$(lastItemEl).removeClass('picker-selected');
            //$(itemEl).addClass('picker-selected');


            $(wrapperEl).css('transform', 'translate3d(0px, ' + ( col.local.y + 72 + 7) + 'px, 0px)')

            evt.preventDefault();

        });

        $(colEls).on('touchend', function (evt) {

            //var cx = evt.touches[0].clientX;
            //var cy = evt.touches[0].clientY;
            if (!me.curCol) {
                return;
            }


            var col = me.curCol;

            var y = col.local.y;

            var my = y % 36;
            
            var curIndex = (y - my) / 36;

            if (my > 18) {
                curIndex++;
            }
            else if (my < -18) {
                curIndex--;
            }
            

            curIndex = me.selectItemForIndex(col.index, this, -curIndex);

            var newY = curIndex * 36 ;

            col.local.y = -newY;

            var wrapperEl = $(this).children('.picker-items-col-wrapper');
            $(wrapperEl).css('transform', 'translate3d(0px, ' + (col.local.y + 72 + 7) + 'px, 0px)')


            console.log("end y = %d", col.local.y);

            me.curCol = null;

            evt.preventDefault();

        });

    },



    setVisible: function (value) {
        var me = this,
            el = me.el;

        el.switchClass('modal-in', 'modal-out', value);

        return me;
    },

    /**
    * 打开窗体
    */
    open: function () {
        "use strict";
        var me = this,
            el = me.el;

        $(document.body).addClass('with-picker-modal');

        $(el).show();

        setTimeout(function () {
            me.setVisible(true);
        }, 100);

        //me.setVisible(true);

        if (me.value) {
            me.setValue(me.value);
        }

        return me;
    },


    /**
    * 关闭窗体
    */
    close: function () {
        "use strict";
        var me = this,
            el = me.el;

        me.setVisible(false);

        el.transitionEnd(function (e) {
            console.debug('关闭 Picker.');
            el.remove();
        });

        $(document.body).removeClass('with-picker-modal');

        return me;
    },

    render: function () {
        "use strict";
        var me = this,
            el;

        me.baseRender();

        me.open();
    }



});




(function ($) {
    "use strict";

    /**
    * 切换样式
    *
    */
    $.fn.Picker = function (config) {

        var me = this;


        $(me).on('click', function () {

            var cfg = Mini2.apply({

                value: $(me).val(),   // '广东省 广州市 荔湾区',

                closed: function () {

                    var vStr = this.getValue();

                    $(me).val(vStr);
                }

            }, config);

            var picker = Mini2.create('Mini2.ui.Picker', cfg);

            picker.render();

        });

    };

})(Zepto);