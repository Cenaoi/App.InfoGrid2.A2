/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />


Mini2.define('Mini2.ui.form.field.ComboBox', {

    extend: 'Mini2.ui.form.field.Trigger',

    alias: 'widget.combobox',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.ComboBox', console),

    //绑定显示的字段名
    displayField: 'text',

    //绑定显示的字段名
    itemDisplayField: 'textEx',


    //绑定值的字段名
    valueField: 'value',

    //复选框模式: single=单选, multi=多选
    checkedMode: 'single',

    //
    //mode: 'user-input'

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

    /**
    * 创建关闭项目
    */
    createCloseItemEl: function (store) {
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



    //初始化组件
    initComponent: function () {

        "use strict";
        var me = this,
            log = me.log;


        $(me).muBind('focusout', function () {

            if (me._focusTimeout) {
                clearTimeout(me._focusTimeout);
                delete me._focusTimeout;
            }

            //log.debug("失去焦点啦。。。");


            me.fieldEl.removeClass("mi-form-focus");
            me.el.removeClass("mi-form-trigger-wrap-focus");

            me.hideDropDownPanel();
        });

        me.initStore();

        me.initLabelable();

    },

    //获取下拉条目显示的内容
    getItemDisplayText: function (record) {
        "use strict";
        var me = this,
            text ;

        text = record.get(me.itemDisplayField);

        if (Mini2.isBlank(text)) {
            text = record.get(me.displayField);
        }

        return text;
    },


    add: function (item) {


    },


    getBoundPanel: function () {
        "use strict";
        var me = this,
            i,
            divEl,
            newItemEl,
            itemEls,
            text,
            store = me.store,
            records = store.data,
            record,
            itemCount = store.getCount(),
            cfg = Mini2.SystemInfo.list,
            itemHeight = cfg.itemHeight || 24;

        divEl = Mini2.$join([
            '<div class="mi-boundlist-list-ct mi-unselectable" style="overflow: auto; height: 100px; font-size:12px; ">',
                '<ul class="mi-list-plain"></ul>',
            '</div>'
        ]);



        var plainEl = divEl.children(".mi-list-plain");

        me.boundListEl = divEl;     //容器
        me.listPlainEl = plainEl;   //小容器


        if (me.checkedMode == 'multi') {

            var itemEl = Mini2.$joinStr([
                '<li role="option" unselectable="on" class="mi-boundlist-item">',
                    '<img class="mi-form-checkbox" src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" style="">',
                '</li>']);

            records = store.data;

            var values = me.value || '';

            var valueList = values.split(',');

            for (var i = 0; i < valueList.length; i++) {
                valueList[i] = Mini2.String.trim(valueList[i]);
            }

            for (i = 0; i < records.getCount() ; i++) {

                record = records.get(i);

                newItemEl = itemEl.clone(); //克隆一个对象出来

                text = me.getItemDisplayText(record);

                newItemEl.append(text);


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

            records = store.data;

            for (i = 0; i < records.getCount() ; i++) {

                record = records.get(i);

                newItemEl = itemEl.clone();

                text = me.getItemDisplayText(record);

                newItemEl.html(text);


                newItemEl.data('record', record);

                $(plainEl).append(newItemEl);

            };


            itemEls = plainEl.children();

            me.bindItems(itemEls);  //绑定每个项目 单选事件
        }


        itemCount = (itemCount > 8 ? 8 : itemCount);

        divEl.css('height', itemHeight * itemCount);

        return divEl;
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

            if (j++ > 0) { values += ','; }

            values += value;

        })

        return values;

    },

    //多选模式的事件...暂时只支持字符串
    bindItems_ForMulti: function (itemEls) {
        "use strict";
        var me = this;

        $(itemEls).on('mouseenter', function () {
            $(this).addClass("mi-boundlist-item-over");
        }).on('mouseleave', function () {
            $(this).removeClass("mi-boundlist-item-over");
        }).on('mouseup', function (e) {

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

            me.trigger('changed', { value: value, text: text });

            me.trigger('changed_callback', { values: values });

        });


    },

    bindItems: function (itemEls) {
        "use strict";
        var me = this;

        $(itemEls).on('mouseenter', function () {
            $(this).addClass("mi-boundlist-item-over");
        }).on('mouseleave', function () {
            $(this).removeClass("mi-boundlist-item-over");
        })
        .on('mouseup', function (e) {
            var record = $(this).data("record");

            var value = record.get(me.valueField);
            var text = record.get(me.displayField);

            //me.log.debug($.format('value={0} ,text={1}', value,text));
            //me.log.debug('me.mode = ', me.mode);

            if ('none' == me.mode) {
                me.hideEl.val(value);
                me.inputEl.val(text);
            }
            else {
                me.inputEl.val(value);
            }


            me.curRecord = record;

            me.value = value;
            me.text = text;


            me.hideDropDownPanel();

            me.clearInvalid();  //清理异常信息

            me.focus();


            if (me.changed) {
                me.changed.call(me);
            }

            me.trigger('changed', { value: value, text: text });

            me.trigger('changed_callback', { value: value, text: text });


            Mini2.EventManager.stopPropagation(e);
        });

    },


    setValue: function (value) {
        "use strict";
        var me = this,
            log = me.log,
            record,
            hideEl = me.hideEl,
            store = me.store,
            text,
            inputEl = me.inputEl,
            valueField = me.valueField,
            displayField = me.displayField;

        me.value = value;


        if ('none' == me.mode && store) {


            try {

                var tmpValue;


                if (true === value) {
                    tmpValue = "true";
                }
                else if (false === value) {
                    tmpValue = "false";
                }
                else {
                    tmpValue = value;
                }


                record = store.filterFirstBy(valueField, tmpValue);

                //if (me.fieldLabel == '是否会签') {
                //    log.debug(me.fieldLabel + '  setValue = ', tmpValue);
                //    log.debug(typeof value);
                //    log.debug("record ", record);

                //    console.trace();
                //}


                if (record) {
                    me.curRecord = record;
                    text = record.get(displayField);

                    me.text = text;

                    inputEl.val(text);
                }
                else {
                    me.text = '';
                    inputEl.val('');
                }
            }
            catch (ex) {
                log.error("ComboBox.setValue(...) 赋值错误啦。", ex);
            }

        }
        else {

            record = store.filterFirstBy(valueField, value);

            me.curRecord = record;

            if (record) {
                me.text = record.get(me.displayField);
            }
            else {
                me.text = '';
            }

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
            record,
            valueField = me.valueField,
            hideEl = me.hideEl,
            value;




        if (me.readOnly || 'none' == me.mode) {
            value = hideEl.val();
        }
        else {
            value = me.inputEl.val();
        }


        record = me.store.filterFirstBy(valueField, value);

        me.curRecord = record;

        if (record) {
            me.text = record.get(me.displayField);
        }
        else {
            me.text = '';
        }


        //log.debug('getValue() return ' + value);

        me.value = value;

        return value;
    },



    /**
    * 获取要发送的数据
    */
    getSentData: function () {
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

                    //发现有这个 {{ }} , 进行编译
                    if (pDef.indexOf('{{') >= 0 && pDef.indexOf('}}')) {

                        var artTemp = template.compile(pDef);
                        var valueStr = artTemp({
                            T: { '未定义': '' }
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
            el = me.el;

        me.init_DropDwonPanel();
        

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
                throw new Error("执行 Trigger.dropDown(...) 事件发生错误." + ex.Message);
            }
        }


        me.proDropDownPanel_SizeALocal();

        return me;
    },


    itemTpl: ['<li role="option" unselectable="on" class="mi-boundlist-item"></li>'],



    /**
    * 创建关闭项目
    */
    createCloseItemEl: function (store) {
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
            i,
            divEl,
            itemTpl = me.itemTpl,
            newItemEl,
            itemEls,
            text,
            store = me.store,
            records = store.data,
            record,
            itemCount = store.getCount(),
            cfg = Mini2.SystemInfo.list,
            itemHeight = cfg.itemHeight || 24;

        var listPlainEl = me.listPlainEl;

        listPlainEl.html('');

        if (me.emptyEnabled) {

            var emptyItemEl = me.createCloseItemEl(store);

            $(listPlainEl).append(emptyItemEl);
        }

        records = store.data;

        for (i = 0; i < records.getCount() ; i++) {

            record = records.get(i);

            text = me.getItemDisplayText(record);

            if (Mini2.isBlank(text)) { text = '&nbsp;'; }

            newItemEl = Mini2.$join(itemTpl);
            newItemEl.html(text);


            newItemEl.data('record', record);

            $(listPlainEl).append(newItemEl);

        };


        itemEls = listPlainEl.children();
        me.bindItems(itemEls);

        itemCount = (itemCount > 8 ? 8 : itemCount);


        me.boundListEl.css('height', itemHeight * itemCount);


        me.proDropDownPanel_SizeALocal();

    },

    getStrLength: function (str) {
        var cArr = str.match(/[^\x00-\xff]/ig);
        return str.length + (cArr == null ? 0 : cArr.length);
    },

    //处理下拉框的形状和位置
    proDropDownPanel_SizeALocal: function () {
        "use strict";

        var me = this,
            el = me.el,
            fieldEl = me.fieldEl,
            panel = me.dropDownPanel,
            office,
            width,
            itemCount,
            winH = $(window).height(),
            buttomY,
            panelTop,
            fieldElWidth,
            targetHeight,
            cfg = Mini2.SystemInfo.list,
            itemHeight = cfg.itemHeight || 24;


        office = fieldEl.offset();

        fieldElWidth = fieldEl.outerWidth();
        width = me.dropDownWidth || fieldElWidth;

        if (width < fieldElWidth) {
            width = fieldElWidth;
        }
        
        itemCount = me.store.getCount();

        if (itemCount > 8) {
            itemCount = 8;
        }

        targetHeight = itemHeight * itemCount;

        buttomY = (office.top + fieldEl.height() + 2) + targetHeight;

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

    }

});