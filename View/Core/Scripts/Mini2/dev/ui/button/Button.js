/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini2.js" />
/// <reference path="../../lang/String.js" />
/// <reference path="../../lang/Number.js" />
/// <reference path="../../lang/log.js" />
/// <reference path="../../lang/Json.js" />
/// <reference path="../../lang/Function.js" />
/// <reference path="../../lang/Date.js" />
/// <reference path="../../lang/Array.js" />


Mini2.define('Mini2.ui.button.Button', {

    extend: 'Mini2.ui.Component',

    alias: 'widget.button',


    btnInner: null,

    baseCls: Mini2.baseCSSPrefix + 'btn',

    scale: 'small',

    allowedScales: ['small', 'medium', 'large'],

    dock: 'none',

    text: '按钮',

    icon: false,

    el: null,

    userCls: false,


    /**
    * 选中的样式
    */
    checkedCls : false,

    radioCls: false,

    isButton: true,

    //beforeClick:true,

    hrefTarget: '_blank',

    /**
    * 分组名称
    */
    group: false,

    
    /**
    * 按钮类型: button=普通按钮, check=复选框按钮, radio=单选框类型
    */
    buttonType : 'button',
    
    //当做复选框使用的时候
    checked: false,




    renderTpl: [
        '<a class="mi-btn ',
            'mi-unselectable ',
            '" ',
            'role="button" hidefocus="on" unselectable="on" tabindex="0" >',
            '<span class="mi-btn-wrap" unselectable="on">',
                '<span class="mi-btn-button">',
                    '<span class="mi-btn-inner mi-btn-inner-center" unselectable="on">',
                    '</span>',
                '</span>',
            '</span>',
        '</a>'
    ],



    //初始化组件
    initComponent: function () {
        var me = this;


    },

    /**
    * 复选框模式
    */
    setChekced: function (value, trigger) {
        var me = this,
            el = me.el,
            item,
            group = me.group,
            btnType = me.buttonType,
            checkCls = me.checkCls;

        me.checked = value;
        
        if (checkCls) {
            el.toggleClass(checkCls, value);
        }

               
        if (false !== trigger && 'radio' == btnType) {

            var items = Mini2.GroupManager.get(group);

            for (var i = 0; i < items.length; i++) {
                if (items[i] === me) {
                    continue;
                }

                item = items[i];

                item.setChekced(false, false);
            }
        }

        
        return me;
    },


    setTop: function (v) {
        var me = this,
            el = me.el;
        el.css('top', v);
        return me;
    },

    setLeft: function (v) {
        var me = this,
            el = me.el;
        el.css('left', v);
        return me;
    },

    focus: function () {
        var me = this;

        Mini2.ui.FocusMgr.setControl(me);
    },

    getInputEl: function () {
        "use strict";
        var me = this,
            support = Mini2.support,
            scale = me.scale,
            margin = me.margin,
            href = me.href,
            hrefTarget = me.hrefTarget,
            baseCls = me.baseCls,
            command = me.command,
            pressedTag = '-pressed',
            overTag = '-over',
            el = Mini2.$join(me.renderTpl);

        if (support.ipad || support.iphone) {
            el.attr('readonly', true);
        }

        me.overCls = [
            'mi' + overTag,
            baseCls + overTag,
            baseCls + '-default-' + scale + overTag];

        me.pressedCls = [
            'mi' + pressedTag,
            baseCls + pressedTag,
            baseCls + '-default-' + scale + pressedTag];


        if (!Mini2.isBlank(command)) {
            el.attr('command', command);
        }


        el.css({
            width: me.width,
            height: me.height,
            left: me.left
        });

        if (me.minWidth) { el.css('min-width', me.minWidth);}
        if (me.minHeight) { el.css('min-height', me.minHeight);}

        if (me.maxWidth) { el.css('max-width', me.maxWidth); }
        if (me.maxHeight) { el.css('max-height', me.maxHeight); }




        el.addCls(baseCls + '-default-' + scale);

        if (margin) {
            if (Mini2.isString(margin)) {
                el.css('margin', margin);
            }
            else {
                for (var i in margin) {
                    el.css('margin-' + i, margin[i]);
                }
            }
        }

        me.btnInner = el.find('.mi-btn-inner:first');
        me.btnInner.html(me.text);

        if (me.icon && '' != me.icon) {
            el.addCls(
                'mi-icon-text-left',
                baseCls + '-icon-text-left',
                baseCls + '-default-' + scale + '-icon-text-left'
            );

            me.imgEl = $('<img src="" class="mi-btn-icon-el mi-btn-glyph"  />');
            me.imgEl.attr('src', me.icon);

            el.mFind('.' + baseCls + '-button').append(me.imgEl);
        }



        el.on('mouseenter', function () {
            $(this).addCls(me.overCls);
        })
        .on('mouseleave', function () {
            $(this).removeCls(me.overCls);
        });

        if (href) {

            el.attr('href', href);

            if (href) {
                el.attr('target', hrefTarget);
            }
        }
        else {
            el.muBind('mousedown', function (e) {

                $(this).addCls(me.pressedCls);



                Mini2.ui.FocusMgr.setControl(me);

            })
            .muBind('mouseup', function (e,srcEvt) {
                "use strict";

                $(this).removeCls(me.overCls);
                $(this).removeCls(me.pressedCls);
                
                if (0 == srcEvt.button) {
                    if (me.beforeClick) {

                        var result = me.beforeClick.call(me, e);

                        if (false ===  result) {
                            return;
                        }
                    }
                    else if (!Mini2.isBlank( me.beforeAskText)) {

                        return me.showAskMsg.call(me);

                    }


                    var btnType = me.buttonType;

                    if ('radio' == btnType) {
                        me.setChekced(true);
                    }
                    else if ('check' == btnType) {
                        me.setChekced(!me.checked);
                    }

                    me.callback_click.call(me);
                }



            });
        }


        return el;
    },

    //显示询问弹出窗口
    showAskMsg: function () {
        "use strict";
        var me = this;

        Mini2.Msg.confirm("询问", me.beforeAskText, function () {
            me.click();
        });

        return false;
    },

    //扩展添加 class 
    expandCls: function (cls) {
        "use strict";
        var me = this,
            el = me.el,
            i, ns;

        if (cls) {
            ns = cls.split(' ');

            for (i = 0; i < ns.length; i++) {
                el.addClass(ns[i]);
            }
        }
    },

    //提交到服务器
    serverForSubmit: function () {
        "use strict";
        var me = this,
            widget = me.widget || window.widget1;

        if (widget) {
            widget.submit(me.el, {
                command: me.command,
                commandParam: me.commandParams
            });
        }
    },

    setHref: function (href) {
        "use strict";
        var me = this,
            el = me.el;

        me.href = href;

        el.attr('href', href);
    },

    getHref: function () {
        var me = this,
            el = me.el;

        return me.href;
    },

    render: function () {
        "use strict";
        var me = this,
            el,
            cId = me.clientId;


        me.el = el = me.getInputEl();

        el.attr('id', cId)
            .data('me', me);


        me.expandCls(me.userCls);

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }


        if (me.click) {
            me.userClick = me.click;

            delete me.click;
        }

        me.click = me.callback_click;


        if (me.group) {
            el.attr('data-group', me.group);

            Mini2.GroupManager.add(me.group, me);
        }


        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }

        return me;
    },


    callback_click: function () {
        "use strict";
        var me = this;

        if (me.userClick) {
            me.userClick.call(me);
        }
        else if (me.command) {
            me.serverForSubmit.call(me);
        }
    }

});

