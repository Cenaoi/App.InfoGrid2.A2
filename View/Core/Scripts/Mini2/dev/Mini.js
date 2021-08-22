/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
//this.constructor.base.xxxXxx();

var Mini2 = Mini2 || {};
Mini2._startTime = new Date().getTime();

//对象定义
Mini2.classDefine = {};

//单件模式
Mini2.singleton = {};

Mini2.identity = 0;

//当前页面的配置信息
Mini2.onwerPage = Mini2.onwerPage || {};
Mini2.onwerPage.controls = Mini2.onwerPage.controls || {};
Mini2.find = function (conId) {
    return Mini2.onwerPage.controls[conId];
};

/**
* typeahead 输入提示框,显示的数量
*/
Mini2.onwerPage.typeahead = [];


enumerables = true;
enumerablesTest = { toString: 1 };

Mini2.getIdentity = function () {

    Mini2.identity++;

    return Mini2.identity;

};

Mini2.newId = Mini2.getIdentity;


Mini2.ClassType = function () {};


/**
 * 继承
 * @example 调用方法：  this.constructor.base.函数名.call(this);
 *                      this.constructor.base.setValue.call(this, valueStr);
 */
Mini2.extend = function (subClass, superClass) {
    "use strict";

    var F = function () { };
    F.prototype = superClass.prototype;
    subClass.prototype = new F();
    subClass.prototype.constructor = subClass;

    //subClass.superclass = superClass.prototype;
    subClass.base = superClass.prototype;

    if (superClass.prototype.constructor == Object.prototype.constructor) {
        superClass.prototype.constructor = superClass;
    }


    return subClass;
}


Mini2.overriden = function (origclass, overrides) {
    "use strict";
    if (overrides) {
        var p = origclass.prototype;
        for (var method in overrides) {
            p[method] = overrides[method];
        }
    }
}


Mini2.apply = function (obj, config, defaults) {
    "use strict";
    /// <summary>创建对象</summary>

    if (defaults) {
        Mini2.apply(obj, defaults);
    }

    if (obj && config && typeof config === 'object') {
        var i, j, k;

        for (i in config) {
            obj[i] = config[i];
        }
    }

    return obj;

}


Mini2.applyIf = function (o, c) {
    "use strict";
    if (o && c) {
        for (var p in c) {
            if (typeof o[p] == "undefined") { o[p] = c[p]; }
        }
    }
    return o;
}

Mini2.ns = function (namespace) {
    "use strict";
    var nn,strFList = namespace.split('.'),
        len = strFList.length,
        pNn = Mini2,
        i;

    for (i = 1; i < len ; i++) {

        nn = strFList[i];

        if (!pNn[nn]) {
            pNn[nn] = {};
        }

        pNn = pNn[nn];

    }
}

Mini2.defineClass = function (fullname, classT) {
    "use strict";
    if (!fullname) { return; }

    var strFList = fullname.split('.'),
        len = strFList.length,
        className = strFList[len - 1],
        pNn = Mini2,
        i,nn;

    if ('Mini2' != strFList[0]) {

        if (len >= 2) {
            var v = strFList[0];
            pNn = window[v];

            if (!pNn) {
                pNn = window[v] = {};
            }
        }
    }

    for (i = 1; i < len - 1; i++) {

        nn = strFList[i];

        if (!pNn[nn]) {
            pNn[nn] = {};
        }

        pNn = pNn[nn];

    }

    pNn[className] = classT;

    return className;
}

/**
* 定义对象
* 
* @param {String} fullname 对象名称
* @param {Object} codeObj 类的内容
* @param {Function} returnFn 实例化对象后, 执行的函数(构造函数)
*/
Mini2.define = function (fullname, codeObj, returnFn) {
    "use strict";
    /// <summary></summary>
    /// <param name="fullname" type="String">对象名称</param>
    /// <param name="codeObj" type="object">对象参数</param>
    var F, extend,mixins, extendObj, name, funType;

    if (codeObj instanceof Function) {
        //Mini2.overriden(F, codeObj);

        Mini2.classDefine[fullname] = codeObj;

        return codeObj;
    }

    //新建一个空的构造函数
    F = function () {
        return ((typeof this._returnFn == 'function') ?
             this._returnFn((arguments.length > 0 ? arguments[0] : false), (arguments.length > 1 ? arguments[1] : false)) : this._returnFn);
    };

    extend = codeObj.extend;
    mixins = codeObj.mixins;

    if (undefined != extend && extend != null && extend != '') {

        extendObj = Mini2.classDefine[extend];

        if (undefined === extendObj) {
            throw new Error(fullname + "继承对象 '" + extend + "' 不存在");
        }

        Mini2.extend(F, extendObj);
    }

    if (mixins ) {

        if (Mini2.isString(mixins)) {
            mixins = [mixins];
        }

        var mmm = null;

        for (var i = 0; i < mixins.length; i++) {

            mmm = mixins[i];
            extendObj = Mini2.classDefine[mmm];


            if (undefined === extendObj) {
                throw new Error(fullname + "继承对象 " + mmm + " 不存在");
            }

            Mini2.extend(F, extendObj);
        }
       
    }


    Mini2.overriden(F, codeObj);


    if (returnFn != undefined) {
        F.prototype._returnFn = returnFn;
    }

    Mini2.classDefine[fullname] = F;
    name = Mini2.defineClass(fullname, F);


    funType = new Mini2.ClassType();
    funType.fullName = fullname;
    funType.name = name;

    F.prototype.getType = function () {
        return eval("this.classType");
    };
    F.prototype.classType = funType;


    //别名的特殊处理
    if (codeObj.alternateClassName) {

        var i, altName,
            altNames = codeObj.alternateClassName,
            len ;

        if (Mini2.isString(altNames)) { altNames = [altNames]; }

        len = altNames.length;

        for (i = 0; i < len; i++) {

            altName = altNames[i];

            Mini2.classDefine[altName] = F;
            Mini2.defineClass(altName, F);
        }
    }

    //别名
    if (codeObj.alias) {
        Mini2.ClassManager.setAlias(fullname, codeObj.alias);
    }

    //如果是单件模式，就直接实例化处理
    if (codeObj.singleton) {
        return Mini2.create(fullname);
    }

    return F;
}

/**
* 创建单间类(静态类)
*
* @param {function} F 
* @param {String} fullname 全名
* @param {Object} ps 
* @return {Object} 静态类
*/
Mini2.createSingleton = function (F, fullname, ps) {
    "use strict";
    var obj = Mini2.singleton[fullname];


    if (obj != undefined) {
        return obj;
    }

    obj = new F(ps);
    obj.muid = "mu_" + Mini2.newId();

    for (var i in ps) {
        newObj[i] = ps[i];
    }

    Mini2.singleton[fullname] = obj;

    Mini2.defineClass(fullname, obj);


    if (F.prototype.alternateClassName) {

        var altNames = F.prototype.alternateClassName,
            altName,
            i,
            len;

        if (Mini2.isString(altNames)) { altNames = [altNames]; }

        len = altNames.length;

        for (i = 0; i < len; i++) {
            altName = altNames[i];
            Mini2.singleton[altName] = obj;
            Mini2.defineClass(altName, obj);
        }
    }

    return obj;
}

/**
 * 创建对象
 *
 */
Mini2.create = function (fullname, ps) {
    "use strict";

    var newObj = null,
        F;
    
    if (Mini2.String.startsWith(fullname, 'widget.')) {
        fullname = Mini2.ClassManager.getNameByAlias(fullname);
    }


    F = Mini2.classDefine[fullname];
    

    if (undefined === F) {
        throw new Error("对象 " + fullname + " 不存在, 无法创建实例.");
    }

    ps = ps || {};
        
    //单件模式
    if (F.prototype.singleton) {

        newObj = Mini2.createSingleton(F, fullname, ps);
    }
    else {

        ps.muid = "mu_" + Mini2.newId();

        newObj = new F(ps);
        
        if (undefined === F.prototype._returnFn) {
            Mini2.apply(newObj, ps);
        }
    }


    return newObj;
}


/**
 * 在浏览器的最外层实例化对象， 一般针对 Window 弹出窗口。
 */
Mini2.createTop = function (fullname, ps) {
    "use strict";

    var pMini2 = Mini2.getTopMini2();

    return pMini2.create(fullname, ps);
};


/**
 * 根据别名, 实例化组件
 */
Mini2.createByAlias = function (alias, ps) {
    "use strict";

    var newObj,
        fullname,
        F;
    
    fullname = Mini2.ClassManager.getNameByAlias(alias);

    F = Mini2.classDefine[fullname]

    ps.muid = "mu_" + Mini2.newId();

    newObj = new F(ps);


    if (undefined === F.prototype._returnFn) {
        Mini2.apply(newObj, ps);
    }

    return newObj;
};


Mini2.__isDisignMode = false;

/**
 * 设计模式
 */
Mini2.designMode = function (value) {

    if (arguments.length == 0) {
        return Mini2.__isDisignMode;
    }
    else {
        Mini2.__isDisignMode = !!value;
    }
};

Mini2.support = {


};

Mini2.support.mozilla = /firefox/.test(navigator.userAgent.toLowerCase()) || /mozilla/.test(navigator.userAgent.toLowerCase());
Mini2.support.webkit = /webkit/.test(navigator.userAgent.toLowerCase());
Mini2.support.opera = /opera/.test(navigator.userAgent.toLowerCase());
Mini2.support.msie = /msie/.test(navigator.userAgent.toLowerCase());

Mini2.support.ipad = /ipad/.test(navigator.userAgent.toLowerCase());
Mini2.support.iphone = /iphone os/.test(navigator.userAgent.toLowerCase());
Mini2.support.android = /android/.test(navigator.userAgent.toLowerCase());



//console.trace("Mini2.support", Mini2.support);

Mini2.apply(Mini2, {

    emptyFn: function () { },

    baseCSSPrefix: 'mi-',

    toString: Object.prototype.toString,

    defaultEl: $('<div></div>'),

    isEmpty: function (obj) {

        if (obj == undefined || obj == null) {
            return true;
        }

        if (typeof obj == 'string') {
            if (obj == "") {
                return true;
            }
        }

        return false;

    },

    getBody: function () { return $(document.body); },

    iterableRe: /\[object\s*(?:Array|Arguments|\w*Collection|\w*List|HTML\s+document\.all\s+class)\]/,





    
    getParentWindow: function () {

        var lastParent = window;

        var i = 0;
        for (i = 0; i < 9; i++) {
            if (lastParent.parent == lastParent || lastParent.parent == undefined) {
                break;
            }

            lastParent = lastParent.parent;
        }

        return lastParent;
    },

    //获取最顶层的 Mini2 对象
    getTopMini2: function () {

        var lastParent = window;

        var tops = [window];

        var i = 0;
        for (i = 0; i < 9; i++) {

            if (lastParent.parent === lastParent || lastParent.parent === undefined) {
                break;
            }
            
            lastParent = lastParent.parent;

            tops.push(lastParent);
        }

        var n = tops.length;

        var mini2Obj = Mini2;

        while (n--) {
            var curWin = tops[n];

            if (curWin.Mini2) {
                mini2Obj = curWin.Mini2;
                break;
            }
        }

        return mini2Obj;
    },

    /**
    * 作废
    */
    $joinStr: function (array) {

        var str = array.join('');

        return $(str);
    },

    $join:function(array){
        var str = Mini2.join(array);
        return $(str);
    },

    join: function (array) {

        if (Mini2.isString(array)) { return array; }

        return array.join('');
    },


    typeOf: function (value) {
        "use strict";
        var type,
            typeToString;

        if (value === null) {
            return 'null';
        }

        type = typeof value;

        if (type === 'undefined' || type === 'string' || type === 'number' || type === 'boolean') {
            return type;
        }

        typeToString = Object.prototype.toString.call(value);

        switch (typeToString) {
            case '[object Array]':
                return 'array';
            case '[object Date]':
                return 'date';
            case '[object Boolean]':
                return 'boolean';
            case '[object Number]':
                return 'number';
            case '[object RegExp]':
                return 'regexp';
        }

        if (type === 'function') {
            return 'function';
        }

        if (type === 'object') {
            if (value.nodeType !== undefined) {
                if (value.nodeType === 3) {
                    return (nonWhitespaceRe).test(value.nodeValue) ? 'textnode' : 'whitespace';
                }
                else {
                    return 'element';
                }
            }
            return 'object';
        }
    },


    //克隆对象
    clone: function (item) {
        "use strict";
        var type,
                i,
                j,
                k,
                clone,
                key;

        if (item === null || item === undefined) {
            return item;
        }

        // DOM nodes
        // TODO proxy this to Ext.Element.clone to handle automatic id attribute changing
        // recursively
        if (item.nodeType && item.cloneNode) {
            return item.cloneNode(true);
        }

        type = Object.prototype.toString.call(item);

        // Date
        if (type === '[object Date]') {
            return new Date(item.getTime());
        }


        // Array
        if (type === '[object Array]') {
            i = item.length;
            clone = [];

            while (i--) {
                clone[i] = Mini2.clone(item[i]);
            }
        }
        // Object
        else if (type === '[object Object]' && item.constructor === Object) {
            clone = {};

            for (key in item) {
                clone[key] = Mini2.clone(item[key]);
            }

            if (enumerables) {
                for (j = enumerables.length; j--; ) {
                    k = enumerables[j];
                    if (item.hasOwnProperty(k)) {
                        clone[k] = item[k];
                    }
                }
            }
        }

        return clone || item;
    },

    isString: function (value) {
        if (value === false) { return; }
        return (typeof value == 'string') || (value instanceof String);
    },

    isDate: function (value) {
        if (value === false) { return; }

        return Object.prototype.toString.call(value) === '[object Date]';
    },

    isArray: function (value) {
        return value && (value.isArray || $.isArray(value));
    },

    isFunction: function (value) {
        return $.isFunction(value);
    },

    isBoolean: function (value) {
        return (typeof value === "boolean");
    },

    isNumber: function (value) {
        return (typeof value === 'number');
    },


    isDom: function (value) {
        /// <summary>是否为 文档对象 节点</summary>

        if (!value) {
            return false;
        }

        var typeToString = value.toString();

        var len = typeToString.length;

        if (len < 12) {
            return false;
        }

        var subTag = typeToString.substr(0, 12);

        return (subTag === '[object HTML');
    },

    type: function (value) {

    },

    isIterable: function (value) {
        "use strict";
        if (!value || typeof value.length !== 'number' || typeof value === 'string') {
            return false;
        }

        if (value.muid) {
            return true;
        }

        // Certain "standard" collections in IE (such as document.images) do not offer the correct
        // Javascript Object interface; specifically, they lack the propertyIsEnumerable method.
        // And the item property while it does exist is not typeof "function"
        if (!value.propertyIsEnumerable) {
            return !!value.item;
        }

        // If it is a regular, interrogatable JS object (not an IE ActiveX object), then...
        // If it has its own property called "length", but not enumerable, it's iterable
        if (value.hasOwnProperty('length') && !value.propertyIsEnumerable('length')) {
            return true;
        }


        // Test against whitelist which includes known iterable collection types
        return this.iterableRe.test(Object.prototype.toString.call(value));

    },

    functionFactory: function () {
        var me = this,
            args = Array.prototype.slice.call(arguments),
            ln;

        if (Mini2.isSandboxed) {
            ln = args.length;
            if (ln > 0) {
                ln--;
                args[ln] = 'var Mini=window.' + Mini2.name + ';' + args[ln];
            }
        }

        return Function.prototype.constructor.apply(Function.prototype, args);
    },


    isDefined: function (value) {
        return typeof value !== 'undefined';
    },




    //弹出当前对象的属性集合
    alertProps: function (obj) {

        var txt = "";

        for (var i in obj) {

//            if (Mini2.isFunction(obj[i])) {
//                continue;
//            }

            txt += i + " = " + obj[i] + "\n";
        }

        alert(txt);

    }

});


jQuery.fn.extend({

    cFirst: function (until) {
        return $(this).children(until + ":first");
    },


    mFind: function (until) {
        return $(this).find(until + ":first");
    },


    addCls: function (cls) {
        "use strict";
        var args = arguments,
            $el = $(this),
            i = 0,
            len;

        if (Mini2.isArray(cls)) {
            args = cls;
        }

        len = args.length;

        for (i = 0; i < len; i++) {            
            $el.addClass(args[i]);
        }

        return this;
    },


    removeCls: function (cls) {
        "use strict";
        var args = arguments,
            i;

        if (Mini2.isArray(cls)) {
            args = cls;
        }

        i = args.length;

        while (i--) {
            $(this).removeClass(args[i]);
        }

    },



    muBind: function (eventName, data, fn) {
        "use strict";
        if (arguments.length == 2) {
            fn = arguments[1];
            data = null;
        }


        var me = this;

        if (typeof fn != 'function') {
            throw new Error("参数必须是 function 对象");
        }

        switch (eventName) {
            case 'mousedown':


                //$(me).on('mu_' + eventName, fn);

                $(me).on(eventName, data, function (evt) {

                    //console.debug("鼠标按键: ", evt.button);

                    Mini2.EventManager.setMouseDown(this, data, null, evt);

                    fn.call(this, evt, this);

                    //$(this).triggerHandler('mu_mousedown', [evt]);
                });

                break;
            case 'mouseup':
                $(me).on('mu_' + eventName, fn);

                $(me).on(eventName, data, function (evt) {

                    Mini2.EventManager.setMouseUp(this, evt);
                });

                break;
            case 'mousemove':
                $(me).on('mu_' + eventName, fn);

                $(me).on(eventName, data, function (evt) {
                    Mini2.EventManager.setMouseMove(this, evt);
                });
                break;
            default:

                if (me.length == 0) {
                    console.error('对象不存在,无法绑定事件 "' + eventName + '";');
                }

                //针对 JQuery.3.0 做了特殊处理
                if (me[0].muid && ('focusout' == eventName || 'focusin' == eventName)) {
                    eventName = 'mu_focusout';
                }

                try {
                    me.on(eventName, data, fn);
                }
                catch (ex) {
                    console.error("这个错误肯定是 JQuery 3.0 产生的错误. 事件名:" + eventName);
                    throw ex;
                }


                break;
        }

        return me;
    },

    muTriggerHandler: function (eventName, fun) {

        var me = this;

        /*****注: 以下代码( 针对 JQuery.3.0 做了特殊处理 )  *******/
        if (me[0].muid && ('focusout' == eventName || 'focusin' == eventName)) {
            eventName = 'mu_focusout';
        }

        me.triggerHandler(eventName, fun);
    }

});

//数据格式化显示,支持多参数
//例子： result = $.format('你好!{0}先生.', '黄');
//result：你好!黄先生
$.format = function (source, params) {
    "use strict";
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
    });
    return source;
};

$(document).ready(function () {
    $(document.body).addClass("mi-webkit mi-border-box mi-body");
});
