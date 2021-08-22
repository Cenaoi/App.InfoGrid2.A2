/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../../Mini.js" />

/// <reference path="../../define.js" />
/// <reference path="../../lang/Date.js" />



Mini2.define('Mini2.ui.form.field.Date', {

    extend: 'Mini2.ui.form.field.Trigger',

    alias: 'widget.date',

    log: Mini2.Logger.getLogger('Mini2.ui.form.field.Date', false),  //日志管理器

    //绑定显示的字段名
    displayField: 'text',

    //绑定值的字段名
    valueField: 'value',

    isLoaded: false,

    dateEl: null,

    defaultValue: null,

    useStrict: undefined,

    format: "Y-m-d",

    initTimeFormat: 'H',

    initTime: '24',

    showTime: false,

    timeEl: null,

    buttonCls: 'mi-icon-date-trigger',

    setWidth: function (value) {
        "use strict";
        var me = this,
            el = me.el;

        me.width = value,
        el.css('width', value);
    },

    getWidth: function () {
        return this.width;
    },

    getTriggerMarkup: function () {
        "use strict";
        var me = this,
            tpl = Mini2.$join(['<div class="mi-layer mi-border-box" style="z-index:39001;" >', '</div>']);

        return tpl;
    },


    focus: function () {

        var me = this,
            el = me.el,
            fieldEl = me.fieldEl;

        if (!el.hasClass('mi-form-trigger-wrap-focus')) {
            fieldEl.addClass('mi-form-focus');

            el.addClass('mi-form-trigger-wrap-focus');


            //特殊处理，延时获取焦点。不然无法及时得到焦点。
            if (!me._focusTimeout) {

                if (me.inputEl) {
                    me._focusTimeout = setTimeout(function () {

                        var inputEl = me.inputEl;

                        //如果焦点已经移除， 那么就作废
                        if (fieldEl.hasClass('mi-form-focus')) {

                            inputEl.focus();

                        }

                        me._focusTimeout = null;

                    }, 200);
                }
            }

            //console.log('me.scope', me.scope);

            //最后一个参数,强制设置
            Mini2.ui.FocusManager.setControl(me.scope || me, null, null, true);
        }
    },


    /**
     * 原生控件
     */
    getFieldEl_Native: function () {
        var me = this,
            $join = Mini2.$join,
            log = me.log,
            tableEl,
            inputEl,
            btnEl,
            sysInfo = Mini2.SystemInfo.formTrigger;

        if (me.showTime) {
            me.inputEl = inputEl = $join([
                '<input type="datetime-local" class="mi-form-field mi-form-text" ',
                    'autocomplete="off" aria-invalid="false" style="width: 100%; ">'
            ]);
        }
        else {
            me.inputEl = inputEl = $join([
                '<input type="date" class="mi-form-field mi-form-text" ',
                    'autocomplete="off" aria-invalid="false" style="width: 100%; ">'
            ]);
        }

        tableEl = me.inputEl;

        inputEl.attr("id", me.clientId);

        if (me.name) {
            inputEl.attr('name', me.name);
        }

        $(inputEl).change(function () {

        });

        $(inputEl).muBind('focus', function () {
            me.focus();
        });

        $(inputEl).muBind('click', function () {
            me.focus();
        });

        return tableEl;
    },



    getFieldEl_M2: function () {
        var me = this,
            $join = Mini2.$join,
            log = me.log,
            tableEl,
            inputEl,
            btnEl,
            sysInfo = Mini2.SystemInfo.formTrigger;

        tableEl = $join([
            '<table class="mi-form-trigger-wrap" cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%; ">',
                '<tbody>',
                    '<tr>',
                        '<td class="mi-form-trigger-input-cell" style="width: 100%; "></td>',
                        '<td valign="top" class="mi-trigger-cell mi-unselectable" style="width:22px;" role="button" ></td>',
                    '</tr>',
                '</tbody>',
            '</table>'
        ]);

        me.inputEl = inputEl = $join([
            '<input type="text" class="mi-form-field mi-form-text" ',
                'autocomplete="off" aria-invalid="false" style="width: 100%; ">'
        ]);

        if (!Mini2.isBlank(me.placeholder)) {
            inputEl.attr('placeholder', me.placeholder);
        }

        if (me.required) {
            inputEl.addClass('mi-form-required-field');
        }

        btnEl = $join(['<div class="mi-form-trigger mi-form-arrow-trigger " role="button"></div>']);

        me.buttonEl = btnEl;

        if (me.buttonCls) {
            btnEl.addCls(me.buttonCls);
        }

        inputEl.attr("id", me.clientId);

        if (me.name) {
            inputEl.attr('name', me.name);
        }

        me.appendCell(tableEl, inputEl, 'td.mi-form-trigger-input-cell');
        me.appendCell(tableEl, btnEl, 'td.mi-trigger-cell');


        tableEl.find('td.mi-trigger-cell').width(sysInfo.width);

        if (undefined != me.value) {
            me.setValue(me.value);

            me.srcValue = me.value;

            delete me.value;
        }

        me.bindEvent_Button(btnEl);

        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);

        me.bindInputEl_Mouse2(inputEl);


        $(me).on('keydown', function (data, ea) {

            if (13 == ea.keyCode && me.mapping) {
                me.onMaping();
                ea.cancel = true;
            }

        });

        return tableEl;
    },

    getFieldEl: function () {
        var me = this,
            $join = Mini2.$join,
            log = me.log,
            tableEl,
            inputEl,
            btnEl,
            sysInfo = Mini2.SystemInfo.formTrigger;


        if ('touch' == Mini2.SystemInfo.mode || Mini2.support.ipad || Mini2.support.iphone) {
            tableEl = me.getFieldEl_Native();
        }
        else {
            tableEl = me.getFieldEl_M2();
        }


        if ('none' == me.mode) {
            me.setMode(true);
        }

        return tableEl;
    },


    callDatepickerShow: function (divEl) {
        "use strict";
        var me = this;

        me.initLocalMessage();

        $.datepicker.formatDate('yy-mm-dd', new Date());

        $(divEl).datepicker({

            dateFormat: 'yy-mm-dd',

            changeMonth: true,
            changeYear: true,
            showOn: '',


            buttonImageOnly: false,

            showOptions: { direction: '' },

            showButtonPanel: true,

            inline: false,

            onSelect: function (dateText, inst) {


                if (me.showTime) {
                    var text = dateText + " " + me.timeEl.val();

                    me.inputEl.val(text);
                }
                else {
                    me.inputEl.val(dateText);
                }

                me.hideDropDownPanel();

                me.focus();

                me.clearInvalid();  //清理异常信息
            }

        });





    },

    //初始化时间控件
    initTimeBox: function () {
        "use strict";
        var me = this,
            panel = me.dropDownPanel;


        if (me.showTime && !me.timeEl) {

            var timeEl = Mini2.$joinStr([
                '<div class="ui-datepicker-time" style="padding: 3px 0px 0px 6px; background-color: #FFF;">',
                    '<span>时间：</span>',
                        '<input type="text" class="ui-time" style="width:80px;" value="12:59:59" />',
                '</div>']);

            //var buttons = $(me.dateEl).find('.ui-datepicker-buttonpane:first');

            $(panel).append(timeEl);

            me.timeEl = timeEl.find('.ui-time:first');
        }
    },


    //扩展关闭按钮
    initCloseBtn: function () {
        "use strict";
        var me = this;



        var buttons = $(me.dateEl).find('.ui-datepicker-buttonpane:first');


        if (buttons.children('.ui-datepicker-close').length > 0) {
            return;
        }


        var closeBtnEl = Mini2.$joinStr(["<button type='button' ",
            "class='ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all' ",
            "data-handler='hide' data-event='click'>关闭</button>"]);

        $(buttons).append(closeBtnEl);

        $(closeBtnEl).click(function () {
            me.hideDropDownPanel();
        });


    },

    getBoundPanel: function () {
        "use strict";
        var me = this;

        me.dateEl = Mini2.$joinStr(['<div style="font-size:12px; "></div>']);


        if ($.fn.datepicker) {
            me.callDatepickerShow(me.dateEl);
        }
        else {
            In('jq.ui.datepicker', function () {
                me.callDatepickerShow.call(me, me.dateEl);
            });
        }

        return me.dateEl;
    },

    beforeShowDropDownPanel: function () {
        "use strict";
        /// <summary>开始显示之前的</summary>

        var me = this;

        me.init_DropDwonPanel();

        if (me.isLoaded) {
            me.initCloseBtn();

            me.initTimeBox();

            console.debug("me.dateEl", me.dateEl);

            return;
        }

        var panel = me.dropDownPanel;


        var boundList = me.getBoundPanel();

        $(panel).children().remove();

        $(panel).append(boundList);

        me.initCloseBtn();

        me.initTimeBox();

        me.isLoaded = true;
    },


    loadDataPicker: function () {
        "use strict";
        var me = this,
            log = me.log,
            el = me.el,
            fieldEl = me.fieldEl,
            pos,
            dateEl = me.dateEl,
            panel = me.dropDownPanel,
            winH = $(window).height(),
            panelTop, buttomY, targetHeight,
            timeEl = me.timeEl;


        pos = fieldEl.offset();

        var dateStr = me.inputEl.val();

        if (me.pw == undefined || me.ph == undefined) {
            me.pw = dateEl.width();
            me.ph = dateEl.height();
        }


        //$(dateEl).datepicker({
        //    currentText: dateStr
        //});

        $(dateEl).datepicker('setDate', dateStr);


        if (timeEl) {

            if (dateStr == '') {
                timeEl.val('00:00:00');
            }
            else {

                /**
                * 注意: 针对 IE 旧版,火狐,进行替换
                */
                if (Mini2.support.msie || Mini2.support.mozilla) {
                    dateStr = dateStr.replace(/-/g, '/');
                }

                var tmpDate = new Date(dateStr);

                var timeStr = Mini2.Date.format(tmpDate, 'H:i:s');

                timeEl.val(timeStr);
            }
        }

        targetHeight = me.ph;

        if (me.showTime) {
            targetHeight += 30;
        }

        buttomY = (pos.top + el.height() - 2) + targetHeight;

        if (buttomY > winH) {
            panelTop = pos.top - targetHeight;
        }
        else {
            panelTop = pos.top + el.height() - 1;
        }

        var elWidth = el.width();

        var posX = pos.left + fieldEl.width() - $(panel).width() + 2;

        var posH = me.ph;

        if (me.showTime) {
            posH += 30; //加上 30像素时间像素
        }

        log.debug('posX', posX);

        panel.css({
            width: me.pw,
            height: posH,
            left: posX,
            top: panelTop
        });

        panel.show();

    },


    //初始化本土信息
    initLocalMessage: function () {
        "use strict";
        if ($.datepicker.regional['zh-CN']) {
            return;
        }

        $.datepicker.regional['zh-CN'] = {
            closeText: '关闭',
            prevText: '&#x3C;上月',
            nextText: '下月&#x3E;',
            currentText: '今天',
            monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
            monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
            dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
            dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
            dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
            weekHeader: '周',
            dateFormat: 'yy-mm-dd',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: true,
            yearSuffix: '年'
        };

        $.datepicker.setDefaults($.datepicker.regional['zh-CN']);

    },

    //显示下拉版面
    showDropDownPanel: function () {
        "use strict";
        /// <summary>显示下拉框</summary>

        var me = this;

        me.init_DropDwonPanel();

        if ($.fn.datepicker) {
            me.loadDataPicker();
        }
        else {
            In('jq.ui.datepicker', function () {

                me.loadDataPicker.call(me);

            });
        }
    },


    parseDate: function (value) {
        "use strict";
        if (!value || Mini2.isDate(value)) {
            return value;
        }

        var me = this,
            val = me.safeParse(value, me.format),
            altFormats = me.altFormats,
            altFormatsArray = me.altFormatsArray,
            i = 0,
            len;

        if (!val && altFormats) {
            altFormatsArray = altFormatsArray || altFormats.split('|');
            len = altFormatsArray.length;
            for (; i < len && !val; ++i) {
                val = me.safeParse(value, altFormatsArray[i]);
            }
        }

        return val;
    },

    safeParse: function (value, format) {
        "use strict";
        var me = this,
            log = me.log,
            utilDate = Mini2.Date,
            result = null,
            strict = me.useStrict,
            parsedDate;


        if (utilDate.formatContainsHourInfo(format)) {
            // if parse format contains hour information, no DST adjustment is necessary
            result = utilDate.parse(value, format, strict);

        }
        else {
            // set time to 12 noon, then clear the time
            parsedDate = utilDate.parse(value + ' ' + me.initTime, format + ' ' + me.initTimeFormat, strict);
            log.debug(format + ' ' + me.initTimeFormat);
            if (parsedDate) {
                result = utilDate.clearTime(parsedDate);
            }
        }

        return result;
    },





    setOldValue: function (value) {
        "use strict";
        var me = this,
            valueStr = value,
            support = Mini2.support,
            timeStr = '';

        if (!value) {
            me.oldValue = ''
            return;
        }


        if (Mini2.isDate(valueStr)) {
            if (me.showTime) {
                valueStr = Mini2.Date.format(value, 'Y-m-d\\TH:i:s');
            }
            else {
                valueStr = Mini2.Date.format(tmpDate, 'Y-m-d');
            }
            //valueStr = Mini2.Date.format(value, me.format);

            timeStr = Mini2.Date.format(value, 'H:i:s');
        }
        else if (Mini2.isString(valueStr)) {

            if (valueStr != "") {


                if ('touch' == Mini2.SystemInfo.mode || support.ipad || support.iphone) {

                    var tmpDate = new Date(valueStr);

                    //console.debug("转化类型前, ", tmpDate);


                    if (me.showTime) {
                        valueStr = Mini2.Date.format(tmpDate, 'Y-m-d\\TH:i:s');
                    }
                    else {
                        valueStr = Mini2.Date.format(tmpDate, 'Y-m-d');
                    }

                    //console.debug("转化类型后, ", valueStr);
                }
                else if (support.mozilla) {

                    //火狐浏览器特殊处理

                    var sssIndex = value.lastIndexOf("."),
                        dtStr, sssStr, dt;

                    if (sssIndex > 0) {
                        dtStr = value.substring(0, sssIndex);   //获取年月日时分秒
                        sssStr = value.substr(sssIndex + 1); //获取毫秒

                        dtStr = dtStr.replace(/-/g, "/");

                        dt = new Date(dtStr);

                        try {
                            dt.setMilliseconds(sssStr);
                        }
                        catch (ex) {
                            console.log("错误啦...." + ex.message);
                        }

                        value = dt;
                    }

                    valueStr = Mini2.Date.format(value, me.format);
                    //timeStr = Mini2.Date.format(value, 'H:i:s');

                }
                else {
                    var tmpDate = new Date(valueStr);

                    valueStr = Mini2.Date.format(tmpDate, me.format);
                    //timeStr = Mini2.Date.format(tmpDate, 'H:i:s');
                }
            }

        }




        me.oldValue = valueStr;
    },

    getValue: function () {
        "use strict";
        var me = this,
            support = Mini2.support,
            defaultValue = me.defaultValue,
            valueStr,
            value = me.constructor.base.getValue.call(me);

        if (value == '') {
            return me.defaultValue;
        }

        if ('touch' == Mini2.SystemInfo.mode || support.ipad || support.iphone) {

            value = me.inputEl.val()


            return value;
        }
        else {
            value = me.constructor.base.getValue.call(me)
            if (value == '') {
                return me.defaultValue;
            }

            value = new Date(value);

            value = Mini2.Date.format(value, me.format);
        }



        return value;
    },

    setValue: function (value) {
        "use strict";
        var me = this,
            support = Mini2.support,
            valueStr = value,
            timeStr = '';

        if (!value) {
            me.constructor.base.setValue.call(me, '');
            return;
        }

        if (Mini2.isDate(valueStr)) {
            if (me.showTime) {
                valueStr = Mini2.Date.format(value, 'Y-m-d\\TH:i:s');
            }
            else {
                valueStr = Mini2.Date.format(tmpDate, 'Y-m-d');
            }

            timeStr = Mini2.Date.format(value, 'H:i:s');

        }
        else if (Mini2.isString(valueStr)) {

            if (valueStr != "") {


                if ('touch' == Mini2.SystemInfo.mode || support.ipad || support.iphone) {

                    if (valueStr.indexOf('T') > 0) {

                        me.inputEl.val(valueStr);
                    }
                    else {
                        console.debug("转化类型前1, ", valueStr);
                        var tmpDate = new Date(valueStr);  //--就是这里出现问题, 多了 8 个小时

                        console.debug("转化类型后2, ", tmpDate);


                        if (me.showTime) {
                            valueStr = Mini2.Date.format(tmpDate, 'Y-m-d\\TH:i:s');
                        }
                        else {
                            valueStr = Mini2.Date.format(tmpDate, 'Y-m-d');
                        }

                        console.debug("转化类型后, ", valueStr);

                        me.inputEl.val(valueStr);
                    }



                    return;
                }
                else if (support.mozilla) {

                    //火狐浏览器特殊处理

                    var sssIndex = value.lastIndexOf("."),
                        dtStr, sssStr, dt;

                    if (sssIndex > 0) {
                        dtStr = value.substring(0, sssIndex);   //获取年月日时分秒
                        sssStr = value.substr(sssIndex + 1); //获取毫秒

                        dtStr = dtStr.replace(/-/g, "/");

                        dt = new Date(dtStr);

                        try {
                            dt.setMilliseconds(sssStr);
                        }
                        catch (ex) {
                            console.log("错误啦...." + ex.message);
                        }

                        value = dt;
                    }
                    else {
                        value = new Date(value);
                    }

                    valueStr = Mini2.Date.format(value, me.format);

                }
                else {
                    var tmpDate = new Date(valueStr);

                    valueStr = Mini2.Date.format(tmpDate, me.format);
                }
            }

        }

        me.constructor.base.setValue.call(me, valueStr);

    },



    isValueChanged: function () {
        var me = this,
            log = me.log,
            oldValue = me.oldValue,
            curValue = me.getValue();

        if (undefined === oldValue || '' == oldValue) {
            oldValue = null;
        }

        if (undefined === curValue || '' == curValue) {
            curValue = null;
        }

        return oldValue != curValue;
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
            targetHeight;

        panel = me.dropDownPanel;

        office = fieldEl.offset();

        fieldElWidth = fieldEl.width();
        width = me.dropDownWidth || fieldElWidth;

        if (width < fieldElWidth) {
            width = fieldElWidth;
        }


        itemCount = me.store.getCount();

        if (itemCount > 8) {
            itemCount = 8;
        }

        targetHeight = 24 * itemCount;

        buttomY = (office.top + el.height() - 2) + targetHeight;

        if (buttomY > winH) {
            panelTop = office.top - targetHeight;
        }
        else {
            panelTop = office.top + el.height() - 2
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