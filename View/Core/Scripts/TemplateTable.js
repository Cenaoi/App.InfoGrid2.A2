/// <reference path="jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="MiniHtml.js" />

/*


Builder.setData(json)     //这是初始化函数
json  = []        //只有标题
json  = [{“列名”：“列数据”}]    //对应列  给数据  不分前后顺序        



Builder.newItem(json)     这是新增一行
json  = []  //空数据一行
json  = [{“列名”：“列数据”}]  // 对应列 给数据  不分前后顺序


Builder.newSubItem(id,json)      这是新增行的子项
id = 1    //这是item的主键
json  = []  //空数据一行
json  = [{“列名”：“列数据”}]  // 对应列 给数据  不分前后顺序

Builder.deleteItem(id)    //删除一行  
id = 1    //数据库表格主键


Builder.deleteSubItem(id)     //删除行子项
id = 1       //数据库表格主键






*/




Mini2.define('Mini2.report.Builder', {

    debug: false,

    single_cols: [{ "HeaderText": "", "DBField": ""}],
    ma_cols: [{ "HeaderText": "", "DBField": ""}],
    ///item主键
    id_field: "",
    ///sub_item主键
    sub_id_field: "",
    ///sub_item的单元格宽度
    sub_td_width: "",
    ///放置表格的容器class属性的值
    div_class: "",
    ///删除item后台的函数名称（有一个参数）
    delete_item: "",
    ///删除sub_item后台的函数名称（有一个参数（id|sub_id））
    delete_sub_item: "",
    ///新增item后台的函数名称
    new_item: "",
    ///保存数据，后台的函数名称
    save_data: "",
    ///新增sub_item后台函数名称（有一个参数）
    new_sub_item: "",
    ///这是初始化函数
    initData: function () {


        var me = this;

        ///这是放置table的容器
        var content = "." + me.div_class;


        ///新建一个table
        var table = "<table class='table1 TemplateTable' cellpadding='0'  rules='all'  cellspacing='0'  ><thead>";

        table += "<tr></tr></thead><tbody></tbody></table>";

        table = $(table);

        ///把table追加到body里面去
        $(content).append(table);

        ///添加标题行
        me.addItem();

        var deleteEl = "<th style='width:100px;'>新增</th><th></th>";

        $(".TemplateTable").children("thead").children("tr").append($(deleteEl));



    },
    ///这是多选框选中的id
    checkboxID: function () {

        var me = this;

        var cbList = $("." + me.div_class).children(".TemplateTable").find(".checkbox_item");

        var id = "";


        for (var i = 0; i < cbList.length; i++) {


            var cbEl = cbList[i];


            if ($(cbEl).attr("checked") == "checked") {

                var value = $(cbEl).attr("value");

                id += value + "|";
            }



        }

        ///去掉最后面的|字符 
        if (id) {

            id = id.substring(0, id.length - 1);
        }

        return id;


    },
    ///添加标题行的函数
    addItem: function () {

        var me = this;

        ///拿到对象长度
        var len = me.single_cols.length;




        ///把多选列添加到表头里面去
        $(".TemplateTable").children("thead").children("tr").append("<th style='width:15px;'></th>");


        ///根据对象长度添加列到表格中去
        for (var i = 0; i < len; i++) {

            var item = me.single_cols[i];
            ///创建新的列
            var td = "<th style='width:140px;'>" + item.HeaderText + "</th>";


            ///把新建列添加到表格里面去
            $(".TemplateTable").children("thead").children("tr").append(td);

        }

        var lens = me.ma_cols.length;

        var thEl = $.format("<th style='width:{0}px;padding:0px;'></th>", (me.sub_td_width * 5));

        thEl = $(thEl);


        var tableEl = "<table class='tabel_Sub_Item' style='width:100%;' cellpadding='0' cellspacing='0'><thead><tr></tr></thead></table>";

        tableEl = $(tableEl);

        for (var i = 0; i < lens; i++) {
            var item = me.ma_cols[i];

            ///创建新的列
            var td = $("<th>" + item.HeaderText + "</th>");



            td.css({
                "width": me.sub_td_width,
                "padding": "0px",
                "border-bottom-style": "none"
            });



            $(tableEl).children("thead").children("tr").append(td);
        }




        $(tableEl).children("thead").children("tr").append("<th style='border-width:0px;'>删除</th>");


        thEl.append(tableEl);

        ///把新建列添加到表格里面去
        $(".TemplateTable").children("thead").children("tr").append(thEl);


    },
    ///添加一行
    newItem: function (json) {

        var me = this;

        ///新建一行
        var trEl = $("<tr></tr>");




        ///给每一行添加上多选框按钮
        var checkboxEl = $("<td><input type='checkbox'  class='checkbox_item'  /></td>");

        trEl.append(checkboxEl);


        ///根据对象长度添加列到表格中去
        for (var i = 0; i < me.single_cols.length; i++) {



            var item = me.single_cols[i];

            ///创建新的列
            var tdEl = $("<td style='padding:0px 4px 0px 4px;'></td>");


            


            ///判断控件类型
            if (item.ConType) {

                var seleteEl = $("<select class='item' DBField='" + item.DBField + "' style='width:100%;border:0px;padding:0px;'></select>");


                ///这是焦点事件，改变背景颜色
                seleteEl.focus(function () {

                    $(trEl).css({ "border": "2px solid #D4D4D4" });


                }).blur(function () {

                    $(trEl).css({ "border": "1px solid #F2F2F2" });

                });





                for (var j = 0; j < item.Items.length; j++) {

                    var optionEl = $("<option style='width:100%;' value='" + item.Items[j].value + "'>" + item.Items[j].text + "</option>");

                    seleteEl.append(optionEl);

                }

                tdEl.append(seleteEl);

            }
            else {



                var inputEl = $("<input type='text' DBField='" + item.DBField + "' class='item' style='width:100%;height:100%;border:0px;' />");



                ///这是焦点事件，改变背景颜色
                inputEl.focus(function () {

                    $(trEl).css({ "border": "2px solid #D4D4D4" });


                }).blur(function () {

                    $(trEl).css({ "border": "1px solid #F2F2F2" });

                });



                tdEl.append(inputEl);

            }



            ///把新建列添加到行里面去
            trEl.append(tdEl);

            //   alert(me.single_cols.length);
        }


        var tableEl = "<table  class='tabel_Sub_Item'  rules='all' cellspacing='0' style='width:100%'></table>";

        ///创建td
        var tdEll = $("<td style='padding:0px;'></td>");


        ///把表格添加进td中
        tdEll.append(tableEl);

        ///把子项表格添加进tr中
        trEl.append(tdEll);

       ///新建一个删除按钮和新增子项按钮
        var deleteEl = $("<td><center><a href='#'><img class='add_sub_item' src='/res/icon/add.png'  /></a></center></td><td></td>");

      // <input type='button' value='新增子项' class='add_sub_btn' />

       trEl.append(deleteEl);

        ///<input type='button' class='delete_Row' value='删除一行记录' />


        $(".TemplateTable").children("tbody").append(trEl);


        ///绑定新增子项事件
        deleteEl.find(".add_sub_item").click(function () {




            var pk = trEl.attr(me.id_field);

            widget1.submit('form:first', {
                action: me.new_sub_item,
                actionPs: pk
            });

        });


        //        ///绑定删除行事件
        //        deleteEl.find(".delete_Row").click(function () {
        //            if (confirm("你确定要删除吗？")) {

        //                var pk = trEl.attr(me.id_field);

        //                widget1.submit('form:first', {
        //                    action: me.delete_item,
        //                    actionPs: pk
        //                });

        //            }
        //        });


        ///有数据才调用函数
        if (json) {

            me.addData(json, trEl);
        }


    },
    ///添加数据进表格中
    addData: function (json, trEl) {

        var me = this;

        ///把ID给传到tr上面
        trEl.attr(me.id_field, json[me.id_field]);



        $(trEl).find(".checkbox_item").attr("value", json[me.id_field]);



        var inputListEl = trEl.find(".item");

        ///循环添加数据进文本框
        for (var i = 0; i < inputListEl.length; i++) {


            var inputEl = inputListEl[i];
            var db_field = $(inputEl).attr("dbfield");
            var db_value = json[db_field];
            $(inputEl).val(db_value);



        }
        var pk = trEl.attr(me.id_field);

        for (var j = 0; j < json.sub_items.length; j++) {

            var item = json.sub_items[j];
            me.newSubItem(pk, item);

        }

    },
    ///新建一行子项函数
    newSubItem: function (pk, sub_item) {


        var me = this;
        var json = me.ma_cols;
        var tableEl = $("tr[" + me.id_field + " = " + pk + "]").children("td").find(".tabel_Sub_Item");

        var trEl = $("<tr></tr>");

        ///拿到数组长度
        var len = json.length;



        ///循环把td添加进tr中
        for (var i = 0; i < len; i++) {
            var item = json[i];

            if (item.ConType == "COMBOBOX") {

                var tdEl = $("<td></td>");

                var seleteEl = $("<select  class='sub_item' DBField='" + item.DBField + "'></select>");

                tdEl.css({ "width": (me.sub_td_width - 4) });

                seleteEl.css({ "width": "100%", "border": "0px" });


                ///这是焦点事件，改变背景颜色
                seleteEl.focus(function () {

                    $("tr[" + me.id_field + " = " + pk + "]").css({ "border": "2px solid #D4D4D4" });


                }).blur(function () {

                    $("tr[" + me.id_field + " = " + pk + "]").css({ "border": "1px solid #F2F2F2" });

                });



                for (var j = 0; j < item.Items.length; j++) {

                    var optionEl = $("<option style='width:100%;' value='" + item.Items[j].value + "'>" + item.Items[j].text + "</option>");

                    seleteEl.append(optionEl);

                }

                tdEl.append(seleteEl);

                trEl.append(tdEl);

            }
            else {
                var tdEl = $("<td ><input type='text'  id='txt" + item.DBField + "' DBField='" + item.DBField + "' class='sub_item' /></td>");

                tdEl.css({ "width": (me.sub_td_width - 4) });

                tdEl.children(".sub_item").css({ "width": "100%", "border": "0px", "padding": "2px 0px" });


                ///这是焦点事件，改变背景颜色
                tdEl.children(".sub_item").focus(function () {

                    $("tr[" + me.id_field + " = " + pk + "]").css({ "border": "2px solid #D4D4D4" });


                }).blur(function () {

                    $("tr[" + me.id_field + " = " + pk + "]").css({ "border": "1px solid #F2F2F2" });

                });



                ///如果有此属性，值为true，将input设置为只读的
                if (item.ReadOnly) {
                    tdEl.children(".sub_item").attr("disabled", "true");
                }

                if (item.ConType == "NUMBER") {
                    tdEl.children(".sub_item").attr("onkeyup", "value=value.replace(/[^\\d\\.]/g,'') ");
                }

                trEl.append(tdEl);
            }

        }

        ///这是删除和新增子项按钮
        var deleteEl = $("<td><center><a href='#'><img class='delete_sub_item' src='/res/icon/close.png'  /></a></center></td>");



        deleteEl.find("a").css("width", "100%");


        trEl.append(deleteEl);


        ///把tr添加进table中
        tableEl.append(trEl);


        var inputList = trEl.find('input.default-new-row');



        ///删除子项事件
        deleteEl.find(".delete_sub_item").click(function () {

            var trList = $(tableEl).find("tr").length;

            if (trList == 1) {
                alert("只有一个子项不能删除！！！");
                return;
            }


            if (confirm("你确定要删除吗？")) {

                var sub_pk = trEl.attr(me.sub_id_field);


                var id = pk + "|" + sub_pk;


                widget1.submit('form:first', {
                    action: me.delete_sub_item,
                    actionPs: id
                });

            }

        });



       


        if (sub_item) {

            me.addSubItem(sub_item, trEl);
        }

    },
    ///删除行方法
    deleteItem: function (pk) {


        var me = this;

        ///拿到div下面的模板表格下tr属性pk = pk 的清除掉
        var trEl = $("." + me.div_class + " > .TemplateTable").children("tbody").children("tr[" + me.id_field + "=" + pk + "]");

        $(trEl).remove();


    },
    ///删除子项行方法
    deleteSubItem: function (pk) {

        var me = this;

        ///拿到div下面的模板表格下子项表格下的tr属性pk = pk 的清除掉
        var trEl = $("." + me.div_class + " > .TemplateTable").find(".tabel_Sub_Item").children("tbody").children("tr[" + me.sub_id_field + "=" + pk + "]");

        $(trEl).remove();

    },
    ///把数据添加进子项中
    addSubItem: function (json, trEl) {


        var me = this;

        var inputListEl = trEl.find(".sub_item");

        ///把ID给传到tr上面
        trEl.attr(me.sub_id_field, json[me.sub_id_field]);



        ///循环添加数据进文本框
        for (var i = 0; i < inputListEl.length; i++) {


            var inputEl = inputListEl[i];

            var db_field = $(inputEl).attr("dbfield");

            var db_value = json[db_field];

            $(inputEl).val(db_value);

        }

    },
    ///保存整个数据
    toJson: function () {
        var me = this;


        ///拿到所有行
        var tb_trList = $(".TemplateTable").children("tbody").children("tr");


        var json_data = [];

        ///循环所有行
        tb_trList.each(function () {

            var item = {};

            var inputEls = $(this).find('.item');

            ///拿到每一项的ID
            item[me.id_field] = $(this).attr(me.id_field);


            inputEls.each(function () {

                var db_field = $(this).attr("dbfield");
                var db_value = $(this).val();

                item[db_field] = db_value;

            });
            ///拿到子项table
            var tableEl = $(this).find("table");

            var sub_items = me.saveSubItem(tableEl);

            item.sub_items = sub_items;


            json_data.push(item);

        });


        var json = Mini2.Json.toJson(json_data);


        ///这是调试模式
        if (me.debug) {

            alert(json);
        }


        var str = {
            "json": json_data,
            "ids": me.checkboxID()
        };




        return Mini2.Json.toJson(str);


    },


    ///保存一行数据
    saveSubItem: function (talbeEl) {

        var me = this;

        ///拿到子项的所有tr
        var trListEl = $(talbeEl).children("tbody").children("tr");


        var sub_items = [];

        ///循环拿所有td
        trListEl.each(function () {

            ///新建一个对象
            var obj = {};

            ///拿到每一项的ID
            obj[me.sub_id_field] = $(this).attr(me.sub_id_field);



            ///拿到td的集合
            var tdEls = $(this).children("td");

            ///拿到td数量
            var tdLen = tdEls.length;

            ///循环添加数据进json里面
            for (var i = 0; i < tdLen - 1; i++) {
                var tdEl = tdEls[i];

                var inputEl = $(tdEl).find(".sub_item");

                var dbField = $(inputEl).attr('dbfield');


                var dbValue = $(inputEl).val();

                obj[dbField] = dbValue;

            }

            ///把对象添加进数组里面
            sub_items.push(obj);

        });

        return sub_items;
    },
    ///清除tbody下面所有数据
    clearAll: function () {

        var me = this;

        $("." + me.div_class).empty();



    },
    ///显示进来的数据
    setData: function (json) {



        var me = this;

        me.initData();

        ///拿到大的table
        var tableEl = $(".TemplateTable");

        ///先清空tbody里面的内容
        tableEl.children("tbody").empty();

        if (!json) { return; };

        for (var i = 0; i < json.length; i++) {
            var item = json[i];
            me.newItem(item);

        }






    }
});