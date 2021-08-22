/// <reference path="../../Mini.js" />



Mini2.define('Mini2.ui.form.Labelable', {

    childEls: [
    /**
    * @property {Ext.Element} labelCell
    * The `<TD>` Element which contains the label Element for this component. Only available after the component has been rendered.
    */
        'labelCell',

    /**
    * @property {Ext.Element} labelEl
    * The label Element for this component. Only available after the component has been rendered.
    */
        'labelEl',

    /**
    * @property {Ext.Element} bodyEl
    * The div Element wrapping the component's contents. Only available after the component has been rendered.
    */
        'bodyEl',

    // private - the TD which contains the msgTarget: 'side' error icon
        'sideErrorCell',

    /**
    * @property {Ext.Element} errorEl
    * The div Element that will contain the component's error message(s). Note that depending on the configured
    * {@link #msgTarget}, this element may be hidden in favor of some other form of presentation, but will always
    * be present in the DOM for use by assistive technologies.
    */
        'errorEl',

        'inputRow',

        /**
        * @property 帮助模块
        */
        'helperEl',
    ],

    activeErrorsTpl: [
        '<td role="presentation" sid="sideErrorCell" valign="middle" style="" width="21">',
            '<div role="presentation" sid="textfield-XXX-errorEl" class="mi-form-error-msg mi-form-invalid-icon" title="" ',
                'style="" data-errorqtip="&lt;ul class=&quot;mi-list-plain&quot;&gt;&lt;li role=&quot;alert&quot;&gt;Name is required&lt;/li&gt;&lt;/ul&gt;">',
                '<ul class="mi-list-plain"><li role="alert"></li></ul>',
            '</div>',
        '</td>'
    ],


    /**
    * 帮助信息
    */
    helperTpl: [
        '<td role="helper" >',
            '<div role="helper-inner" style="mi-form-helper-msg mi-form-helper-icon" >',
                '<ul class="mi-list-plain"><li role="alert"></li></ul>',
            '</div>',
        '</td>'
    ],


    //activeError: null ,

    labelableRenderTpl: [
        '<table role="Labelable" class="mi-field mi-table-plain mi-form-item mi-field-default " ',
            'cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%; ">',
            '<tbody>',
                '<tr role="presentation" class="mi-form-item-input-row">',
                    '<td role="presentation" class="mi-form-item-body"></td>',
                '</tr>',
            '</tbody>',
        '</table>'
    ],


    requiredTpl: [
        '<span style="color:red;font-weight:bold" data-qtip="Required">*</span>'
    ],



    formItemCls: Mini2.baseCSSPrefix + 'form-item',

    labelCls: Mini2.baseCSSPrefix + 'form-item-label',

    helerCls : Mini2.baseCSSPrefix + 'form-helper-msg',

    errorMsgCls: Mini2.baseCSSPrefix + 'form-error-msg',

    baseBodyCls: Mini2.baseCSSPrefix + 'form-item-body',

    inputRowCls: Mini2.baseCSSPrefix + 'form-item-input-row',

    fieldBodyCls: '',

    mixinsPropertys: [
        'fieldLabel',
        'labelAlign',
        'labelWidth',
        'labelPad',
        'labelSeparator',
        'hideLabel',

        'hideHelper',
        'helperText',
        'helperLayout',
        'helperStyle'
    ],

    clearCls: Mini2.baseCSSPrefix + 'clear',

    invalidCls: Mini2.baseCSSPrefix + 'form-invalid',

    fieldLabel: undefined,


    //默认 left
    //bodyAlign :'left',

    labelAlign: 'left',

    labelWidth: 100,

    labelPad: 5,

    labelSeparator: ':',

    hideLabel: false,

    labelCell: undefined,


    getEl: function () {
        "use strict";
        var me = this,
            labEl,
            fieldLabel = me.fieldLabel;

        labEl = $('<label class="mi-form-item-label mi-unselectable" unselectable="on"></label>');
        labEl.html((fieldLabel ? fieldLabel : '标签') + me.labelSeparator);

        labEl.addClass("mi-form-item-label-" + me.labelAlign);
        labEl.css({
            "width": me.labelWidth + "px",
            "margin-right": me.labelPad + "px"
        });

        if (me.required) {
            labEl.append(Mini2.$join(me.requiredTpl));
        }

        return labEl;
    },

    setHideLabel: function (value) {
        "use strict";
        var me = this,
            labelCell = me.labelCell;

        me.hideLabel = value;

        if (value) {
            if (!labelCell) {
                me.renderLabelCell();
            }
        }
        else if (labelCell) {
            labelCell.hide();
        }
    },


    setLabelPad: function (value) {
        "use strict";
        var me = this,
            el,
            owner = me.owner,
            cellEl,
            labEl,
            labelCls = me.labelCls;

        me.labelPad = value;

        if (owner && owner.el) {
            el = owner.el;

            cellEl = el.find('td.mi-field-label-cell:first');
            labEl = cellEl.find('label:first');
        }

        labEl.css({ "margin-right": me.labelPad + "px" });
    },

    getLabelPad: function () {
        return this.labelPad;
    },

    setLabelAlign: function (value) {
        "use strict";
        var me = this, 
            owner = me.owner,
            el,
            cellEl,
            labEl,
            labelCls = me.labelCls;

        me.labelAlign = value;

        if (owner && owner.el) {
            el = owner.el;

            cellEl = el.find('td.mi-field-label-cell:first');
            labEl = cellEl.find('label:first');

            labEl.removeClass(labelCls + '-left');
            labEl.removeClass(labelCls + '-right');
        }

        labEl.addClass("mi-form-item-label-" + me.labelAlign);
    },

    getLabelAlign: function () {
        var me = this;

        return me.labelAlign;
    },

    setWidth: function (value) {
        "use strict";
        var me = this,
            owner = me.owner,
            el,
            labEl,
            cellEl,
            labelWidth;

        me.labelWidth = value;


        if (owner && owner.el) {
            el = owner.el;

            cellEl = el.find('td.mi-field-label-cell:first');
            labEl = cellEl.find('label:first');

            labelWidth = value + me.labelPad;

            $(cellEl).css('width', labelWidth);
            $(labEl).css('width', value);
        }
    },

    getWidth: function () {
        var me = this;
        return me.labelWidth;
    },

    renderLabelCell: function () {
        "use strict";
        var me = this,
            labelCell,
            bodyEl;

        bodyEl = me.labelableRenderTpl.find("td.mi-form-item-body:first");

        me.labelCell = labelCell = Mini2.$join(['<td role="presentation" valign="top" halign="left" class="mi-field-label-cell"></td>']);

        labelCell.css('width',me.labelWidth + me.labelPad);

        $(bodyEl).before(labelCell);

    },

    initLabelable: function () {
        "use strict";
        var me = this,
            labeEl = me.labelableRenderTpl;


        labeEl = Mini2.$join(labeEl);

        me.labelableRenderTpl = labeEl;

        /**
        *
        * 注意: 设计模式下, 外面套多一层 div
        *
        **/
        if (Mini2.designMode()) {
            var divEl = $('<div class="mi-form-item"></div>');

            labeEl.appendTo(divEl);

            me.labelableRenderTpl = divEl;
        }
        
        me.lastActiveError = '';

        if (!me.hideLabel) {
            me.renderLabelCell();
        }

        if (!me.hideHelper) {
            me.renderHelperCell();
        }

    },


    /**
    * 渲染帮助信息框
    */
    renderHelperCell:function(){
        "use strict";
        var me = this,
            helperCell,
            helperStyle,
            iconEl,
            bodyEl;


        if (!me.hideHelper && !Mini2.isBlank(me.helperText) ) {

            helperStyle = me.helperStyle;
            

            bodyEl = me.labelableRenderTpl.find("td.mi-form-item-body:first");

            me.helperCell = helperCell = Mini2.$join([
                '<td role="presentation" valign="top" halign="left" class="mi-field-helper-cell">',
                    '<ul>',
                        '<li>',
                            '<div class="mi-helper-icon"></div>',
                        '</li>',
                    '</ul>',
                '</td>'
            ]);

            iconEl = helperCell.find('.mi-helper-icon');

            if ('icontext' == helperStyle) {
                var textEl = Mini2.$join('<div class="mi-helper-text">', me.helperText, '</div>');

                iconEl.after(textEl);
            }
            else if('icon' == helperStyle) {

                iconEl.attr('data-original-title', me.helperText);
                
                if (iconEl.tooltip) {
                    iconEl.tooltip({
                        placement: 'right',
                        delay: 200,
                        container: Mini2.getBody()
                    });
                }
            }
            

            helperCell.css('width', 0);

            $(bodyEl).parent().append(helperCell);
        }
    },


    /**
    * 设置帮助信息
    */
    setHelper: function (heler, styles, layout) {

    },


    getActiveError: function () {
        return this.activeError || '';
    },

    hasActiveError: function () {
        return !!this.getActiveError();
    },

    setActiveError: function (msg) {

        this.setActiveErrors(msg);
    },

    //获取错误集合
    getActiveErrors: function () {
        return this.activeErrors || [];
    },

    //设置错误集合信息
    setActiveErrors: function (errors) {
        var me = this;

        if (!Mini2.isArray(errors)) {
            errors = [errors];
        }

        me.activeError = errors[0];
        me.activeErrors = errors;

        //        me.activeError = me.getTpl('activeErrorsTpl').apply({
        //            errors: errors,
        //            listCls: Ext.plainListCls 
        //        });

        me.renderActiveError();
    },

    //删除错误提示的 dom 元素
    unsetActiveError: function () {
        var me = this;
        delete me.activeError;
        delete me.activeErrors;
        me.renderActiveError();
    },


    renderActiveError: function () {
        "use strict";
        var me = this,
            activeError = me.getActiveError(),
            hasError = !!activeError,
            errorEl = me.errorEl;

        if (activeError !== me.lastActiveError) {
            //me.on('errorchange', me, activeError);
            me.lastActiveError = activeError;
        }

        if (hasError) {

            if (!errorEl) {

                me.errorEl = errorEl = Mini2.$joinStr(me.activeErrorsTpl);

                var bodyEl = me.labelableRenderTpl.find("td.mi-form-item-body:first");

                $(bodyEl).after(errorEl);
            }

            errorEl.show();

            var errMsgEl = errorEl.children('.mi-form-error-msg');
            $(errMsgEl).attr('title', activeError);
        }
        else {
            if (errorEl) {
                errorEl.hide();
            }
        }

        //        if (me.rendered && !me.isDestroyed && !me.preventMark) {
        //            // Add/remove invalid class
        //            me.el[hasError ? 'addCls' : 'removeCls'](me.invalidCls);

        //            // Update the aria-invalid attribute
        //            me.getActionEl().dom.setAttribute('aria-invalid', hasError);

        //            // Update the errorEl (There will only be one if msgTarget is 'side' or 'under') with the error message text
        //            if (me.errorEl) {
        //                me.errorEl.dom.innerHTML = activeError;
        //            }
        //        }

    }


}, function (args) {
    "use strict";
    var me = this;

    Mini2.apply(me, args);

    if (me.owner) {
        var owner = me.owner,
            i,
            mixinsPropertys = me.mixinsPropertys,
            len = mixinsPropertys.length,
            propName;

        for (i = 0; i < len; i++) {
            propName = mixinsPropertys[i];

            me[propName] = owner[propName] || me[propName];
        }
    }

    me.initLabelable();
});