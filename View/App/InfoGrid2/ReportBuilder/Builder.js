Mini2.define('Mini2.report.Builder', {

    tableName: '',

    //存放编辑节点
    curEditCol: null,

    ///放整体数据
    init_json: '',

    ///大表的div
    fieldBoxId: '',

    ///行标签ID
    rowBoxId: '',

    ///列标签ID
    colBoxId: '',

    ///值标签ID
    valueBoxId: '',

    ///编辑事件
    editClick: Mini2.emptyFn,

    init: function () {
        var me = this;

        me.InitData("#" + me.fieldBoxId);

        me.init_droppable("#" + me.rowBoxId + " ol");

        me.init_droppable("#" + me.colBoxId + " ol");

        me.init_droppable("#" + me.valueBoxId + " ol");
    },

    ///返回所有数据
    toJson: function () {
        var me = this;
        var objJson = {
            table_name: me.tableName,
            col_value: [],
            row_value: [],
            values: []

        };

        me.getData("#" + me.colBoxId, objJson.col_value);

        me.getData("#" + me.rowBoxId, objJson.row_value);

        me.getData("#" + me.valueBoxId, objJson.values);


        return objJson;
    },

    ///拿到行、列、值标签下面的所有数据
    getData: function (divID, obj) {
        //拿到标签下面的li集合
        var cellList = $(divID).children("div").children("ol").children("li");

        ///循环li集合
        cellList.each(function () {

            var data = $(this).data("EX") || {};

            obj.push(data);


        });

    },



    crateCol:function( colObj){
        var me = this;

    },



    ///循环添加数据，传div的ID，和要添加进去的数据
    inputData: function (colsId, colsData) {
        var me = this;

        //拿到标签下面的ol节点
        var cellList = $(colsId).children("div").children("ol");

        for (var i = 0; i < colsData.length; i++) {
            var dataJson = colsData[i];

            var dragText = Mini2.$joinStr([
                '<li style="height:22px;" >',
                    '<a href="#" class="edit" style="width:50px;">编辑</a>',
                    '<span style="padding:0 6px;">',
                        dataJson.field + ' (' + dataJson.desc + ')',
                    '</span>',
                    '<a href="#" class="close" style="float:right;"><img src="/res/icon/close.png" border="0" /></a>',
                '</li>'
            ]);

            $(dragText).data("EX", dataJson);


            $(cellList).append(dragText);


            ///拿到编辑节点
            var aEl = $(dragText).children(".edit");

            ///这是点击编辑事件
            $(aEl).click(function () {

                me.editClick.call(me, aEl);

            });


            var imgEl = $(dragText).children('.close');

            ///这是删除列事件
            $(imgEl).click(function () {

                me.deleteCol(this);

            });


        }

    },

    ///删除节点事件
    deleteCol: function (btnEl) {
        ///拿到父节点
        var me = this;

        var liEl = $(btnEl).parent();

        $(liEl).remove();

    },

    ///加载数据
    InitData: function (divID) {
        var me = this;

        $(divID).accordion();

        $(divID).find("li").draggable({
            appendTo: "body",
            helper: "clone"
        });



        var json = me.init_json;

        var obj = null;

        if (json == '') {
            return;
        }
        json = '(' + json + ')';

        try {

            obj = eval(json);
        }
        catch (ex) {
            alert(ex.Message);
            return;
        }

        ///添加列数据

        me.inputData("#" + me.colBoxId, obj.col_value);

        ///添加行标签
        me.inputData("#" + me.rowBoxId, obj.row_value);

        ///添加值标签
        me.inputData("#" + me.valueBoxId, obj.values);



    },

    showDrop: function (boxEl, ui) {

        var me = this;

        $(boxEl).find(".placeholder").remove();

        ///从节点中把字段名拿出来
        var field = $(ui.draggable).attr('dbfield');

        ///从节点把描述拿出来
        var desc = $(ui.draggable).attr('dbdesc');

        var dragText = Mini2.$joinStr([
            '<li style="height:22px;" >',
                '<a href="#" class="edit" style="width:50px;">编辑</a>',
                '<span style="padding:0 6px;">',
                    field + ' (' + desc + ')',
                '</span>',
                '<a href="#" class="close" style="float:right;"><img src="/res/icon/close.png" border="0" /></a>',
            '</li>'
        ]);

        $(boxEl).append(dragText);

        ///初始化json数据
        var newData = {
            field: field,
            desc: desc,
            total: true,
            title: "",
            value_mode: "",
            width: "",
            fun_name: "SUM",
            format: "",
            db_value: "",
            style: "",
            fixed_values: []
        };

        $(dragText).data("EX", newData);

        var aEl = $(dragText).children('.edit');

        ///这是点击编辑按钮事件
        $(aEl).click(function () {
            me.editClick.call(me, aEl);
        });


        var imgEl = $(dragText).children('.close');

        ///这是删除列事件
        $(imgEl).click(function () {

            me.deleteCol(this);
        });


    },


    //初始化拖拉
    init_droppable: function (name) {
        var me = this;

        $(name).droppable({
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            accept: ":not(.ui-sortable-helper)",
            drop: function (event, ui) {

                me.showDrop.call(me, this, ui);

            }
        }).sortable({
            items: "li:not(.placeholder)",
            sort: function () {
                // gets added unintentionally by droppable interacting with sortable
                // using connectWithSortable fixes this, but doesn't allow you to customize active/hoverClass options
                $(this).removeClass("ui-state-default");
            }
        });
    }

});
