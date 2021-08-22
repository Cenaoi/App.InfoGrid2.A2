


Mini2.define('EC5.ui.OBox', {



});


//焦点管理
Mini2.define('EC5.ui.OPageBuilder', {


    savePage: function () {

        var headObj = {
            "ec-tag": "head",
            "childs": [{
                "ec-tag": "title",
                "value": "标题"
            }]
        };

        var bodyObj = {
            "ec-tag": "body",
            "childs": []
        };

        var obj = {
            childs: [headObj, bodyObj]
        };


        var frmEl = $(".ec-viewbox:first");

        GetPageForItem(frmEl, bodyObj);

        var json = Mini.JsonHelper.toJson(obj);


        //执行保存

        var id = $.query.get('id');

        $.post("Action.aspx?method=save&id=" + id, {
            'id': id,
            'data': json
        }, function (result) {

            Mini2.Msg.alert("提示", "保存成功!" + result);

        });
    },



    GetPageForItem: function (parentEl, parentObj) {

        var childEl = $(parentEl).children('.ec-con');

        if (childEl.size() == 0) {
            return;
        }

        parentObj.childs = [];

        $(childEl).each(function () {
            var el = $(this);
            var boxEl = el.children('.ec-box');

            var id = el.attr('id');
            var ecType = el.attr('ec-type');

            var mainView = el.attr('ec-main-view');
            var mainType = el.attr('ec-main-type');
            var mainName = el.attr('ec-main-name');

            var obj = {};


            obj['ec-tag'] = 'div';
            obj['ec-type'] = ecType;

            obj.id = id || '';
            obj.title = el.attr('title');


            if (mainView) {
                obj['ec-main-view'] = mainView;
                obj['ec-main-type'] = mainType;
                obj['ec-main-name'] = mainName;
            }

            parentObj.childs.push(obj);

            if (ecType == 'TABS') {
                GetPageForItem(el, obj);
            }
            else {
                GetPageForItem(boxEl, obj);
            }
        });


    },




    getPage: function () {

        var id = $.query.get('id');

        if (!id) {
            return;
        }

        $(".ec-viewbox:first").html('');

        $.post("Action.aspx?method=load&id=" + id, {

        }, function (result) {
            var jsonObj;

            try {
                jsonObj = eval("(" + result + ")");
            }
            catch (ex) {
                alert("解析 json 数据错误." + ex.Message);
                alert(result);
                return;
            }

            var frmEl = $(".ec-viewbox:first");

            pro_PageTemplate(jsonObj, frmEl);

        });

    },



    pro_PageTemplate: function (tItem, parentEl) {

        var bodyElem = findBodyItem(tItem, 'body');


        pro_PageTemplateItem(bodyElem, parentEl);
    },



    //查找 ec-tag = body 的节点
    findBodyItem: function (tItem, ecTag) {

        var item = null;
        var childs = tItem.childs;

        if (!childs) {
            return item;
        }

        for (var i = 0; i < childs.length; i++) {

            var cItem = childs[i];

            if (cItem['ec-tag'] == ecTag) {
                item = cItem;
                break;
            }
        }

        return item;
    },

    arrayToStr: function (array) {
        var str = '';

        for (var i = 0; i < array.length; i++) {
            str += array[i];
        }

        return str;
    },

    //获取当前页面的表名
    getTableAll: function () {

    },


    //绑定
    bindEvent_HtmlTabEl: function (el) {

        var tabDeleteBtn = el.find('.tab-delete');

        tabDeleteBtn.click(function () {




        });

    },

    bindEvent_HtmlEl: function (el) {

        el.mouseenter(function () {
            var exBox = $(this).children('.ec-box');

            $(this).children('.ec-con-title').addClass("ec-con-title-hover");
            $(this).children('.ec-tab-toolbar').addClass("ec-tab-toolbar-hover");

            if (exBox.size() == 0) {
                return;
            }



            if (exBox.children().size() == 0) {
                $(this).children('.ec-box-icon-add').show();

                var w = $(this).width();

                var l = (w - 64) / 2;

                var iconAddEl = $(this).children('.ec-box-icon-add');

                iconAddEl.css('left', l);
                iconAddEl.show();


            }
            else {
                $(this).children('.ec-con-button').show();
            }

        }).mouseleave(function () {
            $(this).children('.ec-con-button').hide();

            var iconAddEl = $(this).children('.ec-box-icon-add');

            iconAddEl.hide();


            $(this).children('.ec-con-title').removeClass("ec-con-title-hover");
            $(this).children('.ec-tab-toolbar').removeClass("ec-tab-toolbar-hover");


        });


        var editBtn = el.find('.edit:first');
        var editAreaBtn = el.find('.edit_area:first');
        var deleteBtn = el.find('.delete:first');

        var addBtn = el.find('.ec-box-icon-add a');

        editAreaBtn.click(function () {

            var pId = $.query.get('id');  //复杂页面ID
            var ecCon = $(this).closest('.ec-con');


            var areaId = $(ecCon).attr('id');
            var areaTypeId = $(ecCon).attr('ec-type');

            var mainView = $(ecCon).attr('ec-main-view');
            var mainType = $(ecCon).attr('ec-main-type');
            var mainName = $(ecCon).attr('ec-main-name') || '';

            var conList = $('.ec-con');

            var tables = '';
            var tableDict = {};

            var n = 0;

            $(conList).each(function () {

                var vName = $(this).attr('ec-main-name');

                if (vName && vName != '' && tableDict[vName] == undefined) {

                    if (n > 0) { tables += ","; }

                    tables += vName;
                    tableDict[vName] = true;
                    n++;
                }
            });


            var url = $.format('/App/InfoGrid2/View/OnePage/AreaLinkStepEdit1.aspx?' +
                    'page_Id={0}&view_Id={1}&tables={2}&self_table={3}',
                    pId, mainView, tables, mainName);

            var frm = Mini2.create('Mini2.ui.Window', {
                text: '修改域',
                width: 800,
                height: 500,
                mode: true,
                startPosition: 'center_screen',
                url: url
            });


            frm.show();


            frm.formClosed(function (e) {

                if (e.result != 'ok') { return; }

                var tab = e.tab;
                var col = e.col;

                //alert(tab.id + " = " + tab.name + "\n" + col.id + " = " + col.name);

            });
        });

        editBtn.click(function () {

            var pId = $.query.get('pId');  //复杂页面ID

            var ecCon = $(this).closest('.ec-con');


            var areaId = $(ecCon).attr('id');
            var areaTypeId = $(ecCon).attr('ec-type');

            var mainView = $(ecCon).attr('ec-main-view');
            var mainType = $(ecCon).attr('ec-main-type');
            var mainName = $(ecCon).attr('ec-main-name') || '';


            var url = $.format('/App/InfoGrid2/View/OnePage/AreaStepEdit2.aspx?' +
                    'page_Id={0}&Area_Id={1}&Area_Type_Id={2}&view_id={3}&table_names={4}',
                    pId, areaId, areaTypeId, mainView, mainName);

            var frm = Mini2.create('Mini2.ui.Window', {
                text: '修改工作表',
                width: 800,
                height: 600,
                mode: true,
                startPosition: 'center_screen',
                url: url
            });

            frm.show();


            var targetBox = $(el).children('.ec-box');


            frm.formClosed(function (e) {

                if (e.result != 'ok') { return; }

                var viewId = e.viewId;


                var targetBox = $(el).children('.ec-box');

                var areaId = $(el).attr('id');
                var areaTypeId = $(el).attr('ec-type');


                $(el).attr('ec-main-view', viewId);
                $(el).attr('ec-main-type', e.viewType);
                $(el).attr('ec-main-name', e.viewName);

                getActionSample(targetBox, areaTypeId, viewId);


            });


        });



        deleteBtn.click(function () {

            Mini2.Msg.prompt('询问', '您确认要删除吗？', function () {

                var targetBox = $(el).children('.ec-box');

                targetBox.empty();

                $(el).attr('ec-main-view', '');

                $(el).attr('ec-main-type', '');
                $(el).attr('ec-main-name', '');

            });
        });


        addBtn.click(function () {

            var ecCon = $(this).closest('.ec-con');

            var areaId = $(ecCon).attr('id');
            var areaTypeId = $(ecCon).attr('ec-type');

            //页面id
            var pageId = $.query.get('id');

            //操作的 Guid,主要是避免操作重复
            var opGuid = Mini2.Guid.newGuid();

            var url = $.format('/App/InfoGrid2/View/OnePage/AreaStepNew1.aspx?' +
                    'page_Id={0}&Area_Id={1}&Area_Type_Id={2}&TMP_GUID={3}',
                    pageId, areaId, areaTypeId, opGuid);



            var frm = Mini2.create('Mini2.ui.Window', {
                text: '选择工作表',
                width: 800,
                height: 600,
                mode: true,
                startPosition: 'center_screen',
                url: url
            });

            // EcView.showDialog(url, '选择工作表');

            frm.show();


            $(this).parent().hide();

            frm.formClosed(function (e) {

                if (e.result != 'ok') { return; }


                var viewId = e.viewId;


                var targetBox = $(el).children('.ec-box');

                var areaId = $(el).attr('id');
                var areaTypeId = $(el).attr('ec-type');

                Mini2.alertProps(e);

                $(el).attr('ec-main-view', viewId);
                $(el).attr('ec-main-type', e.viewType);
                $(el).attr('ec-main-name', e.viewName);

                getActionSample(targetBox, areaTypeId, viewId);


            });


        });


    },

    TItem_To_HtmlEl: function (tItem) {

        var ecTag = tItem['ec-tag'];
        var ecType = tItem['ec-type'];
        var ecMainView = tItem['ec-main-view'];
        var ecMainType = tItem['ec-main-type'];
        var ecMainName = tItem['ec-main-name'];


        var elTpl = $.format(arrayToStr([
                '<{0} class="ec-con">',
                    '<div class="ec-con-title" >',
                        tItem['title'],
                    '</div>',
                    '<div class="ec-tab-toolbar">',
                        '<a href="#" class="tab-move-up">上移</a>',
                        '<a href="#" class="tab-move-down">下移</a>',
                        '<a href="#" class="tab-insert-before">插入-上方</a>',
                        '<a href="#" class="tab-insert-after">插入-下方</a>',
                        '<a href="#" class="tab-delete">删除</a>',
                    '</div>',
                    '<div class="ec-con-button" style="display:none;">',
                        '<a href="#" class="edit">修改字段</a>',
                        '<a href="#" class="edit_area">修改域</a>',
                        '<a href="#" class="edit_area edit_area_v2">修改关联v2</a>',
                        '<a href="#" class="delete">删除</a>',
                    '</div>',
                '</{0}>'
            ]), ecTag);


        var el = $(elTpl);

        if (ecType != 'TABS') {
            el.append('<div class="ec-box"></div>');
            el.append(arrayToStr([
                    '<div class="ec-box-icon-add" style="display:none;" >',
                        '<a href="#"><img src="/res/page-builder/add_big.png" width="50" height="50" alt="添加" /></a>',
                    '</div>'
                ]));
        }


        $(el).attr('ec-type', ecType);

        if (ecMainView && ecMainView != '') {
            $(el).attr('ec-main-view', ecMainView);
            $(el).attr('ec-main-type', ecMainType);
            $(el).attr('ec-main-name', ecMainName);
        }

        for (var i in tItem) {

            if (i == 'ec-tag' ||
                   i == 'ec-type' ||
                   i == 'childs' ||
                   i == 'ec-main-view') {

                continue;
            }

            var attrValue = tItem[i];
            $(el).attr(i, attrValue);
        }

        return el;
    },


    pro_PageTemplateItem: function (tItem, parentEl) {

        var tItem;
        var itemEl;
        var childs = tItem['childs'];

        if (!childs) {
            return;
        }

        for (var i = 0; i < childs.length; i++) {

            tItem = childs[i];

            itemEl = TItem_To_HtmlEl(tItem);

            bindEvent_HtmlEl(itemEl);

            bindEvent_HtmlTabEl(itemEl);


            $(parentEl).append(itemEl);

            pro_PageTemplateItem(tItem, itemEl);


            var ecType = $(itemEl).attr('ec-type');
            var ecMainView = $(itemEl).attr('ec-main-view');
            var ecMainType = $(itemEl).attr('ec-main-type');
            var ecMainName = $(itemEl).attr('ec-main-name');

            if (ecMainView && ecMainView != '') {
                var boxEx = $(itemEl).children('.ec-box');

                getActionSample(boxEx, ecType, ecMainView);
            }
        }





    },


    getActionSample: function (targetBox, areaTypeId, pageViewId) {


        var url = '';

        console.log('areaTypeId = ' + areaTypeId);

        if ('SEARCH' == areaTypeId) {
            url = $.format("ActionSample_Search.aspx?pageViewId={0}", pageViewId);
        }
        else if ('FORM' == areaTypeId) {
            url = $.format("ActionSample_Form.aspx?pageViewId={0}", pageViewId);
        }
        else {
            url = $.format("ActionSample.aspx?pageViewId={0}", pageViewId);
        }

        $.post(url, {
            'op': 'load'

        }, function (result) {

            $(targetBox).empty();
            $(targetBox).append(result);

        });

    }

});