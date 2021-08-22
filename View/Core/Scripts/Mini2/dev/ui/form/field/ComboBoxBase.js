/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />
/// <reference path="../../../Mini.js" />


Mini2.define('Mini2.ui.form.field.ComboBoxBase', {

    extend: 'Mini2.ui.form.field.Trigger',

    alias: 'widget.comboboxbase',


    log: Mini2.Logger.getLogger('Mini2.ui.form.field.ComboBoxBase', true),

    //绑定显示的字段名
    displayField: 'text',

    //绑定显示的字段名
    itemDisplayField: 'textEx',


    //绑定值的字段名
    valueField: 'value',

    //当前选中的记录
    curRecord: null,


    //复选框模式: single=单选, multi=多选
    checkedMode: 'single',

    itemHeight : 24,

    // private
    // 字符最大长度.
    //maxItemTextLen:0,

    /**
    * 激活远程数据模式
    */
    remoteEnabled: false,

    /**
    * 远程地址
    */
    remoteUrl: null,

    /**
    * 获取远程数据,提交的 post 数据
    */
    remoteParams: null,

    /**
     * 空项目激活
     */
    emptyEnabled: true,

    /**
     * 空项目的值
     */
    emptyValue: '',

    /**
     * 空项目显示的内容
     */
    emptyText: '',

    emptyItemEl:null,
    emptyItemHeight :0,


    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this;


        $(me).muBind('focusout', function () {


            if (me._focusTimeout) {
                clearTimeout(me._focusTimeout);
                delete me._focusTimeout;
            }



            me.fieldEl.removeClass("mi-form-focus");
            me.el.removeClass("mi-form-trigger-wrap-focus");

            me.hideDropDownPanel();
        });

        me.initStore();

        me.initLabelable();

        me.curRecordEl = Mini2.$join(['<input type="hidden" name="', me.clientId , '_curRecord" />']);

    },

    //获取下拉条目显示的内容
    getItemDisplayText: function (record) {
        "use strict";
        var me = this,
            text;

        text = record.get(me.itemDisplayField);

        if (undefined == text || '' == text) {
            text = record.get(me.displayField);
        }

        return text;
    },





    getBoundPanel: function () {
        "use strict";
        var me = this,
            sysInfo = Mini2.SystemInfo,
            i,
            divEl,
            newItemEl,
            itemEls,
            text,
            store = me.store,
            records = store.data,
            len,
            record,
            itemCount = store.getCount(),
            cfg = Mini2.SystemInfo.list,
            itemHeight = cfg.itemHeight || me.itemHeight;

        divEl = Mini2.$joinStr([
            '<div class="mi-boundlist-list-ct mi-unselectable" style="overflow: auto; height: 100px; font-size:12px; ">',
                '<ul class="mi-list-plain"></ul>',
            '</div>'
        ]);



        var plainEl = divEl.children(".mi-list-plain");

        me.boundListEl = divEl;     //容器
        me.listPlainEl = plainEl;   //小容器

        records = store.data;

        if ('multi' == me.checkedMode) {

            var itemEl = Mini2.$joinStr([
                '<li role="option" unselectable="on" class="mi-boundlist-item">',
                    '<img class="mi-form-checkbox" src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" style="">',
                '</li>']);

            records = store.data;

            var values = me.value || '';

            var valueList = values.split(',');

            for (i = 0, len = valueList.length ; i < len; i++) {
                valueList[i] = Mini2.String.trim(valueList[i]);
            }

            for (i = 0, len = records.getCount() ; i < len ; i++) {

                record = records.get(i);

                newItemEl = itemEl.clone(); //克隆一个对象出来

                text = me.getItemDisplayText(record);

                newItemEl.append(text + '&nbsp;');


                newItemEl.data('record', record);

                if (Mini2.Array.contains(valueList, text, true)) {
                    newItemEl.addClass('mi-boundlist-item-selected');
                }

                $(plainEl).append(newItemEl);

            };

            itemEls = plainEl.children();
            me.bindItems_ForMulti(itemEls);
        }
        else {
            var itemEl = $('<li role="option" unselectable="on" class="mi-boundlist-item"></li>');

            var text2,
                len2,
                maxTextLen = 0;

            for (i = 0, len = records.getCount() ; i < len ; i++) {

                record = records.get(i);

                newItemEl = itemEl.clone();

                text = me.getItemDisplayText(record);

                newItemEl.html(text + '&nbsp;');

                text2 = newItemEl.text();
                len2 = me.getStrLength(text2);

                maxTextLen = Math.max(maxTextLen, len2);


                newItemEl.data('record', record);

                $(plainEl).append(newItemEl);

            };

            me.maxItemTextLen = maxTextLen * sysInfo.font.chatWidth + 16;   //36 = 2*8边距 + 20滚动条


            itemEls = plainEl.children();
            me.bindItems(itemEls);
        }

        itemCount = (itemCount > 8 ? 8 : itemCount);



        divEl.css({
            height: itemHeight * itemCount
        });


        

        return divEl;
    },


    itemTpl: ['<li role="option" unselectable="on" class="mi-boundlist-item"></li>'],



    /**
    * 创建关闭项目
    */
    createCloseItemEl:function(store){
        var me = this,
            record = {},
            emptyValue = me.emptyValue,
            itemEl = Mini2.$join(me.itemTpl);


        record[me.valueField] = me.emptyValue;
        record[me.displayField] = '';
        record[me.itemDisplayField] = '[ X ]';// me.emptyText;

        record = store.createModel(record);

        itemEl.addClass("mi-boundlist-empty").html('[ X ]').data('record', record);

        return itemEl;
    },


    //刷新
    refresh: function () {
        "use strict";
        var me = this,
            sysInfo = Mini2.SystemInfo,
            i,
            divEl,
            itemTpl = me.itemTpl,
            emptyItemEl,
            emptyItemHeight =0,
            newItemEl,
            itemEls,
            text,
            store = me.store,
            records = store.data,
            record,
            itemCount = store.getCount(),
            listPlainEl = me.listPlainEl,
            len,
            text2, len2, maxTextLen = 0,
            cfg = Mini2.SystemInfo.list,
            itemHeight = cfg.itemHeight || me.itemHeight;

        
        listPlainEl.html('');


        if (me.emptyEnabled) {
            
            me.emptyItemEl = emptyItemEl = me.createCloseItemEl(store);

            $(listPlainEl).append(emptyItemEl);

            me.emptyItemHeight = emptyItemHeight = $(emptyItemEl).outerHeight() + 2;

        }

        records = store.data;

        for (i = 0, len = records.getCount() ; i < len; i++) {

            record = records.get(i);

            newItemEl = Mini2.$join(itemTpl);
            
            text = me.getItemDisplayText(record);

            if (Mini2.isBlank(text)) { text = '&nbsp;'; }

            newItemEl.html(text);

            text2 = newItemEl.text();
            len2 = me.getStrLength(text2);

            maxTextLen = Math.max(maxTextLen, len2);

            newItemEl.data('record', record);

            $(listPlainEl).append(newItemEl);

        };

        me.maxItemTextLen = maxTextLen * sysInfo.font.chatWidth + 16;   //36 = 2*8边距 + 20滚动条

        itemEls = listPlainEl.children();
        me.bindItems(itemEls);

        itemCount = (itemCount > 8 ? 8 : itemCount);

        me.boundListEl.css({
            height: itemHeight * itemCount + emptyItemHeight
        });


        me.proDropDownPanel_SizeALocal();

    },

    getStrLength: function (str) {
        "use strict";
        var cArr = str.match(/[^\x00-\xff]/ig);
        return str.length + (cArr == null ? 0 : cArr.length);
    },


    //获取多选的值
    getMultiValues: function (listPlainEl) {
        "use strict";
        var me = this;
            
        var checkedItemEls = $(listPlainEl).children('.mi-boundlist-item-selected');

        var values = '';

        var j = 0;

        $(checkedItemEls).each(function () {

            var record = $(this).data("record");

            var value = record.get(me.valueField);
            var text = record.get(me.displayField);

            if (j++ > 0) { values += ',';}

            values += value;

        })
        
        return values;

    },


    bindItems_ForMulti: function (itemEls) {
        "use strict";
        var me = this;

        $(itemEls).mouseenter(function () {
            $(this).addClass("mi-boundlist-item-over");
        }).mouseleave(function () {
            $(this).removeClass("mi-boundlist-item-over");
        })
        .mouseup(function (e) {

            $(this).toggleClass('mi-boundlist-item-selected');

            var values = me.getMultiValues($(this).parent());
            
            if ('none' == me.mode) {
                me.hideEl.val(values);
                me.inputEl.val(values);
            }
            else {
                me.inputEl.val(values);
            }

            me.value = values;
            me.text = values;


            //me.hideDropDownPanel();

            me.clearInvalid();  //清理异常信息

            me.focus();


            if (me.changed) {
                me.changed.call(me);
            }

            me.trigger('changed_callback', { value: value, text: text });

        });


    },


    //选中的项目到值
    selectItemToValue: function (item) {
        "use strict";
        var me = this,
            value, text,
            record = $(item).data("record");

        if (record.isModel) {
            value = record.get(me.valueField);
            text = record.get(me.displayField);
        }
        else {
            value = record[me.valueField];
            text = record[me.displayField];
        }


        if ('none' == me.mode) {
            me.hideEl.val(value);
            me.inputEl.val(text);
        }
        else {
            me.inputEl.val(value);
        }

        me.curRecord = record;

        var jsonObj = me.store.getJsonForRecord(record);
        var jsonStr = Mini2.Json.toJson(jsonObj);

        me.curRecordEl.val(jsonStr);

        me.value = value;
        me.text = text;


        try{
            me.setMap(record);
        }
        catch (ex) {
            console.error('设置映射值错误', ex);
        }

        me.hideDropDownPanel();

        me.clearInvalid();  //清理异常信息

        me.focus();


        if (me.changed) {
            me.changed.call(me);
        }

        me.trigger('changed_callback', { value: value, text: text });

    },


    /**
     * 映射值
     */
    setMap:function(record){

        var me = this,
            mapItems = me.mapItems,
            ownerParent = me.ownerParent,
            store ;

        //没有映射就取消
        if (!mapItems) {
            return;
        }

        if (!ownerParent) {
            return;
        }

        store = ownerParent.store;

        if (!store) {
            return;
        }


        var curRect = store.getCurrent();

        for (var i = 0; i < mapItems.length; i++) {
            var mapItem = mapItems[i];
            
            if (mapItem && mapItem.srcField && mapItem.targetField) {

                var value = record.get(mapItem.srcField);

                curRect.set(mapItem.targetField, value);
            }
        }
    },

    bindItems: function (itemEls) {
        "use strict";
        var me = this;

        $(itemEls).mouseenter(function () {
            $(this).addClass("mi-boundlist-item-over");
        })
        .mouseleave( function () {
            $(this).removeClass("mi-boundlist-item-over");
        });


        $(itemEls).muBind('mousedown', function (e) {

        })
        .muBind('mouseup',function (e) {
            
            me.selectItemToValue.call(me,this);
        });


    },


    setValue: function (value) {
        "use strict";
        var me = this,
            record,
            valueField = me.valueField,
            displayField = me.displayField,
            hideEl = me.hideEl,
            store = me.store,
            text,
            inputEl = me.inputEl;

        me.value = value;

        //log.debug("setValue() = " + value);

        if ('none' == me.mode && store) {

            try {

                record = store.filterFirstBy(valueField, value);

                if (record) {

                    me.curRecord = record;

                    var jsonObj = store.getJsonForRecord(record);
                    var jsonStr = Mini2.Json.toJson(jsonObj);

                    me.curRecordEl.val(jsonStr);

                    text = record.get(displayField);

                    inputEl.val(text);
                }
                else {
                    inputEl.val(value);
                }
            }
            catch (ex) {
                alert("错误啦。" + ex.Message);
            }

        }
        else {
            inputEl.val(value);
        }

        if (hideEl) {
            hideEl.val(value);
        }
    },

    getValue: function () {
        "use strict";
        var me = this,
            log = me.log,
            hideEl = me.hideEl,
            value;


        if (me.readOnly || 'none' == me.mode) {
            value = hideEl.val();
        }
        else {
            value = me.inputEl.val();
        }

        log.debug('getValue() return ' + value);

        me.value = value;

        return value;
    },


    //处理下拉框的形状和位置
    proDropDownPanel_SizeALocal: function () {
        "use strict";

        var me = this,
            el = me.el,
            fieldEl = me.fieldEl,
            panel,
            office,
            width,
            itemCount,
            winH = $(window).height(),
            buttomY,
            panelTop,
            fieldElWidth,
            targetHeight,
            cfg = Mini2.SystemInfo.list,
            itemHeight = cfg.itemHeight || me.itemHeight;

        panel = me.dropDownPanel;

        office = fieldEl.offset();

        fieldElWidth = fieldEl.outerWidth() + 2;
        width = me.dropDownWidth || me.maxItemTextLen || fieldElWidth;

        width = Math.max(width, fieldElWidth);

        itemCount = me.store.getCount();
        
        if (itemCount > 8) {
            itemCount = 8;
            width += 20;    //出现滚动条..加上20像素
        }

        targetHeight = itemHeight * itemCount + me.emptyItemHeight;

        buttomY = (office.top + fieldEl.height() - 2) + targetHeight;

        if (buttomY > winH) {
            panelTop = office.top - targetHeight;
        }
        else {
            panelTop = office.top + fieldEl.height() + 1;
        }


        panel.css({
            'border-width': 1,
            'width': width,
            'height': targetHeight,
            'left': office.left,
            'top': panelTop
        }).show();


        return me;

    },



    //光标是否在开始位置
    isFirstCursorPos: function () {
        return true;
    },

    //光标是否在尾部
    isLastCursorPos: function () {
        return true;
    },


    proKeyEvent: function (e) {
        "use strict";
        var me = this,
            dropDownPanel = me.dropDownPanel,
            listPlainEl = $(me.listPlainEl),
            keyCode = e.keyCode,
            cls = 'mi-boundlist';

        if (Mini2.onwerPage.typeahead.length) {
            /* 如果出现自动补充提示框, 就忽略按键 */
            return;
        }



        if (40 == keyCode) {        //上

            if (!dropDownPanel || dropDownPanel.is(":hidden")) {

                me.beforeShowDropDownPanel();
                me.showDropDownPanel();
            }


            var sItem = $(listPlainEl).children(".mi-boundlist-selected");

            sItem.removeClass('mi-boundlist-selected');

            if (sItem.length == 0) {
                sItem = listPlainEl.children(".mi-boundlist-item:first");
            }
            else {
                sItem = sItem.next(".mi-boundlist-item");
            }

            sItem.addClass('mi-boundlist-selected');

            Mini2.EventManager.stopEvent(e);
            e.stoped = true;
            
        }
        else if (38 == keyCode) {   //下
            

            if (!dropDownPanel || dropDownPanel.is(":hidden")) {

                me.beforeShowDropDownPanel();
                me.showDropDownPanel();
            }


            var sItem = $(listPlainEl).children(".mi-boundlist-selected");

            sItem.removeClass('mi-boundlist-selected');

            if (sItem.length == 0) {
                sItem = listPlainEl.children(".mi-boundlist-item:last");
            }
            else {
                sItem = sItem.prev(".mi-boundlist-item");
            }

            sItem.addClass('mi-boundlist-selected');


            Mini2.EventManager.stopEvent(e);
            e.stoped = true;
        }
        else if (13 == keyCode) {   //回车

            if (!dropDownPanel || dropDownPanel.is(":hidden")) {
                return;
            }

            var sItem = $(listPlainEl).children(".mi-boundlist-selected");


            me.selectItemToValue.call(me, sItem);


            Mini2.EventManager.stopEvent(e);
            e.stoped = true;
        }
    },


    /**
    * 获取要发送的数据
    */
    getSentData:function(){
        var me = this,
            sentData = {},
            ps = me.remoteParams;
        
        if (ps && ps.length) {
            
            $(ps).each(function () {

                var p = this;

                if ('query_string' == p.role) {
                    var qValue = $.query.get(p.queryStringField);

                    sentData[p.name] = qValue;
                }
                else if ('control' == p.role) {

                    var con = Mini2.find(p.control);

                    var value = con.getValue();

                    sentData[p.name] = value;

                }
                else {

                    var pDef = p['default'];

                    if (pDef.indexOf('{{') >= 0 && pDef.indexOf('}}')) {

                        var artTemp = template.compile(pDef);
                        var valueStr = artTemp({
                            T: {'未定义':''}
                        });

                        sentData[p.name] = valueStr;
                    }
                    else {
                        sentData[p.name] = pDef;
                    }

                }

            });

        }

        return sentData;
    },


    showDropDownPanel: function () {
        "use strict";
        /// <summary>显示下拉框</summary>

        var me = this,
            el = me.el,
            fieldEl = me.fieldEl,
            panel,
            office,
            width,
            itemCount,
            winH = $(window).height(),
            buttomY,
            panelTop,
            fieldElWidth,
            targetHeight;

        me.init_DropDwonPanel();

        panel = me.dropDownPanel;

        // 激活 "获取远程数据"
        if (me.remoteEnabled) {

            var sentData = me.getSentData();
            
            Mini2.post(me.remoteUrl, sentData, function (data) {

                var store = me.store;

                store.removeAll();

                store.add(data);

                me.refresh();

            });
        }


        if (me.dropDown) {
            try {
                me.dropDown.call(me, me);
            }
            catch (ex) {
                throw new Error("执行 Trigger.dropDown(...) 事件发生错误");
            }
        }

        me.proDropDownPanel_SizeALocal();

        return me;
    },

    render: function () {
        "use strict";
        var me = this,
            bodyCls = 'td.mi-form-item-body:first';

        me.baseRender();


        me.appendCell(me.el, me.curRecordEl, bodyCls);



        me.el.data('me', me);
    }


});