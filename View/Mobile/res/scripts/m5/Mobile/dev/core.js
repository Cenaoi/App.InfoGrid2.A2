/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
//this.constructor.base.xxxXxx();

var Mini2 = Mini2 || {};


Mini2._startTime = new Date().getTime();

//对象定义
Mini2.classDefine = {};

//单件模式
Mini2.singleton = {};

Mini2.identity = 0;

enumerables = true;
enumerablesTest = { toString: 1 };

Mini2.getIdentity = function () {

    Mini2.identity++;

    return Mini2.identity;

};

Mini2.newId = Mini2.getIdentity;


Mini2.ClassType = function () { };


Mini2.extend = function (subClass, superClass) {
    "use strict";
    /// <summary>继承</summary>
    /// <code>
    ///    调用方法：this.constructor.base.函数名.call(this);
    ///              this.constructor.base.setValue.call(this, valueStr);
    /// </code>

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
    var nn, strFList = namespace.split('.'),
        len = strFList.length,
        pNn = Mini2,
        i;

    for (i = 1; i < len ; i++) {

        nn = strFList[i];

        if (pNn[nn] == undefined) {
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
        i, nn;


    for (i = 1; i < len - 1; i++) {

        nn = strFList[i];

        if (pNn[nn] == undefined) {
            pNn[nn] = {};
        }

        pNn = pNn[nn];

    }

    pNn[className] = classT;

    return className;
}

Mini2.define = function (fullname, codeObj, returnFn) {
    "use strict";
    /// <summary>定义对象</summary>
    /// <param name="fullname" type="String">对象名称</param>
    /// <param name="codeObj" type="object">对象参数</param>
    var F, extend, extendObj, name, funType;

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

    if (extend != undefined && extend != null && extend != '') {

        extendObj = Mini2.classDefine[extend];

        if (extendObj == undefined) {
            throw new Error(fullname + "继承对象 " + extend + " 不存在");
        }

        Mini2.extend(F, extendObj);
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
            altNames = codeObj.alternateClassName;


        for (i = 0; i < altNames.length; i++) {

            altName = altNames[i];

            Mini2.classDefine[altName] = F;
            Mini2.defineClass(altName, F);
        }
    }

    //如果是单件模式，就直接实例化处理
    if (codeObj.singleton) {
        Mini2.create(fullname);
    }

    return F;
}

Mini2.createSingleton = function (F, fullname, ps) {
    "use strict";
    var obj = Mini2.singleton[fullname];


    if (obj != undefined) {
        return obj;
    }

    obj = new F(ps);
    obj.muid = "mu_" + Mini2.getIdentity();

    for (var i in ps) {
        newObj[i] = ps[i];
    }

    Mini2.singleton[fullname] = obj;

    Mini2.defineClass(fullname, obj);


    if (F.prototype.alternateClassName) {

        var altNames = F.prototype.alternateClassName;

        for (var i = 0; i < altNames.length; i++) {
            var altName = altNames[i];
            Mini2.singleton[altName] = obj;
            Mini2.defineClass(altName, obj);
        }
    }

    return obj;
}

Mini2.create = function (fullname, ps) {
    "use strict";
    /// <summary>创建对象</summary>

    var F = Mini2.classDefine[fullname];

    if (undefined == F) {
        throw new Error("对象 " + fullname + " 不存在, 无法创建.");
    }

    ps = ps || {};

    var newObj = null;

    //单件模式
    if (F.prototype.singleton) {

        newObj = Mini2.createSingleton(F, fullname, ps);
    }
    else {

        ps.muid = "mu_" + Mini2.getIdentity();

        newObj = new F(ps);

        if (F.prototype._returnFn == undefined) {
            Mini2.apply(newObj, ps);
        }
    }


    return newObj;
}


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

    $joinStr: function (array) {

        var i = 0, str = "";
        for (; i < array.length; i++) {
            str += array[i];
        }

        return $(str);
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
                for (j = enumerables.length; j--;) {
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
        return $.isArray(value);
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







//数据格式化显示,支持多参数
//例子： result = $.format('你好!{0}先生.', '黄');
//result：你好!黄先生
Mini2.format = function (source, params) {
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

