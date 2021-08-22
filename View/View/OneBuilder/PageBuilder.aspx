<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageBuilder.aspx.cs" Inherits="App.InfoGrid2.View.OneBuilder.PageBuilder" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>

    <link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />

    <link href="/View/OneBuilder/page-builder.css" rel="stylesheet" type="text/css" />

    <script src="/Core/Scripts/jquery/jquery-1.8.3.js" type="text/javascript"></script>

    <script src="/Core/Scripts/jquery.ui/ui/jquery.ui.datepicker.js" type="text/javascript"></script>

    <script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>

    <script src="/Core/Scripts/jtemplates/jquery-jtemplates.js" type="text/javascript"></script>

    <script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>

    <link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" type="text/css" />
    <link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" type="text/css" />
    <link href="/Core/Scripts/Mini2/Themes/Win8/theme-win8.css" rel="stylesheet" type="text/css" />

    <%
        Response.WriteFile("~/Core/Scripts/Mini2/Mini2.script.txt");
    %>
     
    <script type="text/javascript" >


        function savePage() {

            var headObj = {
		        "ec-tag" : "head",
		        "childs" : [{
			        "ec-tag" : "title",
			        "value" : "标题"
                }]
            };

            var bodyObj = {
                "ec-tag" : "body",
                "childs":[]
            };

            var obj = {
                childs: [headObj,bodyObj]
            };


            var frmEl = $(".ec-viewbox:first");

            GetPageForItem(frmEl, bodyObj);

            var json = Mini.JsonHelper.toJson(obj);


            //执行保存

            var id = $.query.get('id');

            $.post("Action.aspx?method=save&id=" + id, {
                'id': id,
                'ser_class': $('#ser_class').val(),
                'data': json
            },
            function (result) {

                Mini2.toast("提示", "保存成功!" + result);

            });
        }


        
        function GetPageForItem(parentEl, parentObj) {

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


        }




        function getPage() {

            var id = $.query.get('id');
            var page_type = $.query.get('page_type');

            if (!id) {
                return;
            }

            $(".ec-viewbox:first").html('');



            var url = $.format("Action.aspx?method=load&id={0}&page_type={1}", id, page_type);

            $.post(url, {

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

            $.post("Action.aspx?method=load_params&id=" + id, {

            }, function (result) {
                
                var jsonObj;


                try {
                    jsonObj = eval('(' + result + ')');
                } catch (ex) {
                    alert('解析 json 数据错误:' + ex.message);
                    alert(result);
                    return;
                }

                $('#ser_class').val(jsonObj.SERVER_CLASS);

            });

        }



        function pro_PageTemplate(tItem, parentEl) {

            var bodyElem = findBodyItem(tItem, 'body');
            

            pro_PageTemplateItem(bodyElem, parentEl);
        }



        //查找 ec-tag = body 的节点
        function findBodyItem(tItem,ecTag) {

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
        }

        function arrayToStr(array) {
            var str = '';

            for (var i = 0; i < array.length; i++) {
                str += array[i];
            }

            return str;
        }

        //获取当前页面的表名
        function getTableAll() {

        }


        //绑定
        function bindEvent_HtmlTabEl(el) {


        }



        function bindEvent_HtmlEl(el) {


            var tabDeleteBtn = el.find('.tab-delete');

            tabDeleteBtn.click(function () {

                var me = this,
                    tabEl = $(me).parents('.ec-con:first');

                Mini2.Msg.confirm('提示', '确定删除标签?', function () {

                    $(tabEl).fadeOut(500, function () {
                        $(tabEl).remove();
                    });
                });

                return false;
            });


            var tabAddBtn = el.find('.tab-add');

            tabAddBtn.click(function () {

                var me = this,
                    parentCon = $(me).parents('.ec-con:first');

                var tabId = "";

                for (var i = 0; i < 9999; i++) {

                    tabId = "TAB_" + Mini2.newId();

                    var tabEl = $('#' + tabId);

                    if (tabEl.length == 0) {
                        break;
                    }

                }

                var itemCfg = { "ec-tag": "div", "ec-type": "TAB", "title": "Tab-1", "id": tabId };

                itemEl = TItem_To_HtmlEl(itemCfg);


                bindEvent_HtmlEl(itemEl);
                bindEvent_HtmlTabEl(itemEl);

                itemEl.hide();

                $(parentCon).append(itemEl);

                $(itemEl).fadeIn(1000);

                return false;
            });

            var tabMoveupBtn = el.find('.tab-move-up');

            tabMoveupBtn.click(function () {
                var me = this,
                    parentCon = $(me).parents('.ec-con:first'),
                    nextCon = parentCon.prev('.ec-con:first');

                if (nextCon.size()) {
                    nextCon.insertAfter(parentCon);
                }

                return false;
            });


            var tabMovedownBtn = el.find('.tab-move-down');

            tabMovedownBtn.click(function () {
                var me = this,
                    parentCon = $(me).parents('.ec-con:first'),
                    nextCon = parentCon.next('.ec-con:first');

                if (nextCon.size()) {
                    nextCon.insertBefore(parentCon);
                }

                return false;
            });



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


            var editToolbarBtn = el.find('.edit_toolbar');

            var editBtn = el.find('.edit:first');
            var editAreaBtn = el.find('.edit_area');

            
            var deleteBtn = el.find('.delete:first');

            var addBtn = el.find('.ec-box-icon-add a');


            editToolbarBtn.click(function () {


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


                $.get('/app/infogrid2/view/OneToolbar/ToolbarForTable.aspx',{
                    "table_id":mainView
                },
                function (result) {
                   
                    console.log(result);

                    var resultObj = JSON.parse(result);

                    if (resultObj.result != 'ok') {
                        return;
                    }

                    var data = resultObj.data;
                    
                    var url = $.format('/app/infogrid2/view/OneToolbar/SetToolbar.aspx?' + 
                        'id=161&table_id={1}', data.toolbar_id, mainView);

                    var frm = Mini2.createTop('Mini2.ui.Window', {
                        text: '修改工具栏',
                        width: 800,
                        height: 500,
                        mode: true,
                        state:'max',
                        startPosition: 'center_screen',
                        url: url
                    });


                    frm.show();
                });


                return false;
            });

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

                    if (vName && vName != '' && undefined == tableDict[vName]) {
                        
                        if (n > 0) { tables += ","; }

                        tables += vName;
                        tableDict[vName] = true;
                        n++;
                    }
                });

                var isV2 = $(this).hasClass('edit_area_v2');


                var url;
                
                if (isV2) {
                    url = $.format('/App/InfoGrid2/View/OnePage/AreaLinkStepEdit2.aspx?' +
                        'page_Id={0}&view_Id={1}&tables={2}&self_table={3}',
                        pId, mainView, tables, mainName);
                }
                else {
                    url = $.format('/App/InfoGrid2/View/OnePage/AreaLinkStepEdit1.aspx?' +
                        'page_Id={0}&view_Id={1}&tables={2}&self_table={3}',
                        pId, mainView, tables, mainName);

                }


                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: '修改域',
                    width: 800,
                    height: 500,
                    mode: true,
                    url: url
                });


                frm.show();


                frm.formClosed(function (e) {

                    if (e.result != 'ok') { return; }

                    var tab = e.tab;
                    var col = e.col;

                    //alert(tab.id + " = " + tab.name + "\n" + col.id + " = " + col.name);

                });

                return false;
            });

            editBtn.click(function () {

                var pId = $.query.get('id');  //复杂页面ID

                var ecCon = $(this).closest('.ec-con');


                var areaId = $(ecCon).attr('id');
                var areaTypeId = $(ecCon).attr('ec-type');

                var mainView = $(ecCon).attr('ec-main-view');
                var mainType = $(ecCon).attr('ec-main-type');
                var mainName = $(ecCon).attr('ec-main-name') || '';


                var url = $.format('/App/InfoGrid2/View/OnePage/AreaStepEdit2.aspx?' + 
                    'page_Id={0}&Area_Id={1}&Area_Type_Id={2}&view_id={3}&table_names={4}',
                    pId, areaId, areaTypeId, mainView, mainName);

                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: '修改工作表',
                    width: 800,
                    height: 600,
                    mode: true,
                    startPosition: 'center_screen', 
                    shadowVisible: false,
                    state:'max',
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


                return false;
            });



            deleteBtn.click(function () {

                Mini2.Msg.confirm('询问', '您确认要删除吗？', function () {

                    var targetBox = $(el).children('.ec-box');

                    targetBox.empty();

                    $(el).attr('ec-main-view', '');

                    $(el).attr('ec-main-type', '');
                    $(el).attr('ec-main-name', '');

                });

                return false;
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



                var frm = Mini2.createTop('Mini2.ui.Window', {
                    text: '选择工作表',
                    width: 800,
                    height: 600,
                    mode: true,
                    startPosition: 'center_screen', 
                    shadowVisible:false,
                    state: 'max',
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


                    $(el).attr('ec-main-view', viewId);
                    $(el).attr('ec-main-type', e.viewType);
                    $(el).attr('ec-main-name', e.viewName);

                    getActionSample(targetBox, areaTypeId, viewId);


                });


                return false;
            });


        }

        function TItem_To_HtmlEl(tItem) {

            var ecTag = tItem['ec-tag'];
            var ecType = tItem['ec-type'];
            var ecMainView = tItem['ec-main-view'];
            var ecMainType = tItem['ec-main-type'];
            var ecMainName = tItem['ec-main-name'];


            var elTpl = $.format(arrayToStr([
                '<{0} class="ec-con">',
                    '<div class="ec-con-title" >',
                        '<span>',
                            tItem['title'],
                        '</span>',
                        '<span style="margin-left:12px;">',
                            '<a href="#" style="width:16px;height:16px;" class="tab-delete">X</a>',
                        '</span>',
                    '</div>',
                    '<div class="ec-con-button" style="display:none;">',
                        '<a href="#" class="edit_toolbar">工具条</a>',
                        '<a href="#" class="edit">修改字段</a>',
                        '<a href="#" class="edit_area">修改域</a>',
                        '<a href="#" class="edit_area edit_area_v2">修改关联v2</a>',
                        '<a href="#" class="delete">删除</a>',
                    '</div>',
                '</{0}>'
            ]), ecTag);


            var el = $(elTpl);

            if (ecType == 'TABS') {

                el.append(arrayToStr([
                    '<div class="ec-tab-toolbar">',
                        '<a href="#" class="tab-add">添加子Tab</a>',
                    '</div>'
                ]));


            }
            else if (ecType == 'VIEWPORT') {

            }
            else if (ecType == 'PANEL') {

            }
            else {

                el.append(arrayToStr([
                    '<div class="ec-tab-toolbar">',
                        '<a href="#" class="tab-move-up">上移</a>',
                        '<a href="#" class="tab-move-down">下移</a>',
                    '</div>'
                ]));

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

                $(el).attr('id', tItem.id);
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
        }


        function pro_PageTemplateItem(tItem,parentEl) {

            var tItem;
            var itemEl;
            var childs = tItem['childs'];

            if (!childs) {
                return;
            }

            for (var i = 0; i < childs.length; i++) {

                tItem = childs[i];

                log.debug(Mini2.Json.toJson(tItem));

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





        }



        $(document).ready(function () {
            getPage();
        });

        function getActionSample(targetBox,areaTypeId,pageViewId) {


            var url = '';

            if ('SEARCH' == areaTypeId) {
                url = $.format("ActionSample_Search.aspx?pageViewId={0}",pageViewId);
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


        function GoReturn() {

             var urlStr = '/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=' + $.query.get('id');

             window.location.href = urlStr;
        }

    </script>

</head>
<body style="background-image: url('/res/page-builder/builder_bg_texture_tile.png')">

    <table style="">
        <tr>
            <td>
                <button onclick="savePage()" type="button" style="width:80px;height:30px;">保存</button>
                <button onclick="GoReturn()" type="button" style="width:80px;height:30px;">返回</button>
            </td>
            <td style="text-align:right; width:100px;">类路径</td>
            <td><input type="text" id="ser_class" style="width:500px;" /></td>
        </tr>
    </table>

    <form id="form1" action="">
<%--<button onclick="getPage()" type="button" style="width:120px;height:30px;">打开复杂表</button>--%>

        <div class="ec-box ec-viewbox">
        
            
        
        </div>


    </form>
</body>
</html>
