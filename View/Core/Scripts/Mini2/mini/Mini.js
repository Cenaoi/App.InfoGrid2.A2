var Mini2=Mini2||{};Mini2._startTime=new Date().getTime();Mini2.classDefine={};Mini2.singleton={};Mini2.identity=0;enumerables=true;enumerablesTest={toString:1};Mini2.getIdentity=function(){Mini2.identity++;return Mini2.identity};Mini2.newId=Mini2.getIdentity;Mini2.ClassType=function(){};Mini2.extend=function(b,c){var a=function(){};a.prototype=c.prototype;b.prototype=new a();b.prototype.constructor=b;b.base=c.prototype;if(c.prototype.constructor==Object.prototype.constructor){c.prototype.constructor=c}return b};Mini2.overriden=function(b,c){if(c){var d=b.prototype;for(var a in c){d[a]=c[a]}}};Mini2.apply=function(f,a,b){if(b){Mini2.apply(f,b)}if(f&&a&&typeof a==="object"){var c,d,e;for(c in a){f[c]=a[c]}}return f};Mini2.applyIf=function(b,a){if(b&&a){for(var d in a){if(typeof b[d]=="undefined"){b[d]=a[d]}}}return b};Mini2.defineClass=function(c,b){if(!c){return}var h=c.split("."),e=h.length,a=h[e-1],g=Mini2,d,f;for(d=1;d<e-1;d++){f=h[d];if(g[f]==undefined){g[f]={}}g=g[f]}g[a]=b;return a};Mini2.define=function(fullname,codeObj,returnFn){if(codeObj instanceof Function){Mini2.classDefine[fullname]=codeObj;return codeObj}var F=function(){return((typeof this._returnFn=="function")?this._returnFn((arguments.length>0?arguments[0]:false),(arguments.length>1?arguments[1]:false)):this._returnFn)};var extend=codeObj.extend;if(extend!=undefined&&extend!=null&&extend!=""){var extendObj=Mini2.classDefine[extend];if(extendObj==undefined){throw new Error(fullname+"�̳ж��� "+extend+" ������")}Mini2.extend(F,extendObj)}Mini2.overriden(F,codeObj);if(returnFn!=undefined){F.prototype._returnFn=returnFn}Mini2.classDefine[fullname]=F;var name=Mini2.defineClass(fullname,F);var funType=new Mini2.ClassType();funType.fullName=fullname;funType.name=name;F.prototype.classType=funType;F.prototype.getType=function(){return eval("this.classType")};if(codeObj.alternateClassName){var i,altNames=codeObj.alternateClassName;for(i=0;i<altNames.length;i++){var altName=altNames[i];Mini2.classDefine[altName]=F;Mini2.defineClass(altName,F)}}if(codeObj.singleton){Mini2.create(fullname)}return F};Mini2.createSingleton=function(c,d,g){var f=Mini2.singleton[d];if(f!=undefined){return f}f=new c(g);f.muid="mu_"+Mini2.getIdentity();for(var e in g){newObj[e]=g[e]}Mini2.singleton[d]=f;Mini2.defineClass(d,f);if(c.prototype.alternateClassName){var b=c.prototype.alternateClassName;for(var e=0;e<b.length;e++){var a=b[e];Mini2.singleton[a]=f;Mini2.defineClass(a,f)}}return f};Mini2.create=function(b,d){var a=Mini2.classDefine[b];if(a==undefined){throw new Error("���� "+b+" ������")}if(d==undefined){d={}}var c=null;if(a.prototype.singleton){c=Mini2.createSingleton(a,b,d)}else{d.muid="mu_"+Mini2.getIdentity();c=new a(d);if(a.prototype._returnFn==undefined){Mini2.apply(c,d)}}return c};Mini2.apply(Mini2,{emptyFn:function(){},baseCSSPrefix:"mi-",toString:Object.prototype.toString,defaultEl:$("<div></div>"),isEmpty:function(a){if(a==undefined||a==null){return true}if(typeof a=="string"){if(a==""){return true}}return false},getBody:function(){return $(document.body)},iterableRe:/\[object\s*(?:Array|Arguments|\w*Collection|\w*List|HTML\s+document\.all\s+class)\]/,$joinStr:function(a){var b,c="";for(b=0;b<a.length;b++){c+=a[b]}return $(c)},typeOf:function(c){var a,b;if(c===null){return"null"}a=typeof c;if(a==="undefined"||a==="string"||a==="number"||a==="boolean"){return a}b=Object.prototype.toString.call(c);switch(b){case"[object Array]":return"array";case"[object Date]":return"date";case"[object Boolean]":return"boolean";case"[object Number]":return"number";case"[object RegExp]":return"regexp"}if(a==="function"){return"function"}if(a==="object"){if(c.nodeType!==undefined){if(c.nodeType===3){return(nonWhitespaceRe).test(c.nodeValue)?"textnode":"whitespace"}else{return"element"}}return"object"}},clone:function(c){var g,b,d,e,a,f;if(c===null||c===undefined){return c}if(c.nodeType&&c.cloneNode){return c.cloneNode(true)}g=Object.prototype.toString.call(c);if(g==="[object Date]"){return new Date(c.getTime())}if(g==="[object Array]"){b=c.length;a=[];while(b--){a[b]=Mini2.clone(c[b])}}else{if(g==="[object Object]"&&c.constructor===Object){a={};for(f in c){a[f]=Mini2.clone(c[f])}if(enumerables){for(d=enumerables.length;d--;){e=enumerables[d];if(c.hasOwnProperty(e)){a[e]=c[e]}}}}}return a||c},isString:function(a){if(a===false){return}return(typeof a=="string")||(a instanceof String)},isDate:function(a){if(a===false){return}return Object.prototype.toString.call(a)==="[object Date]"},isArray:function(a){return $.isArray(a)},isFunction:function(a){return $.isFunction(a)},isBoolean:function(a){return(typeof a==="boolean")},isNumber:function(a){return(typeof a==="number")},isDom:function(d){if(!d){return false}var c=d.toString();var a=c.length;if(a<12){return false}var b=c.substr(0,12);return(b==="[object HTML")},type:function(a){},isIterable:function(a){if(!a||typeof a.length!=="number"||typeof a==="string"){return false}if(a.muid){return true}if(!a.propertyIsEnumerable){return !!a.item}if(a.hasOwnProperty("length")&&!a.propertyIsEnumerable("length")){return true}return this.iterableRe.test(Object.prototype.toString.call(a))},functionFactory:function(){var c=this,a=Array.prototype.slice.call(arguments),b;if(Mini2.isSandboxed){b=a.length;if(b>0){b--;a[b]="var Mini=window."+Mini2.name+";"+a[b]}}return Function.prototype.constructor.apply(Function.prototype,a)},isDefined:function(a){return typeof a!=="undefined"},alertProps:function(b){var c="";for(var a in b){c+=a+" = "+b[a]+"\n"}alert(c)}});jQuery.fn.extend({cFirst:function(a){return $(this).children(a+":first")},mFind:function(a){return $(this).find(a+":first")},addCls:function(b){var a=arguments,c;if(Mini2.isArray(b)){a=b}c=a.length;while(c--){$(this).addClass(a[c])}},removeCls:function(b){var a=arguments,c;if(Mini2.isArray(b)){a=b}c=a.length;while(c--){$(this).removeClass(a[c])}},muBind:function(b,a,c){if(arguments.length==2){c=arguments[1];a=null}var d=this;if(typeof c!="function"){throw new Error("���������� function ����")}switch(b){case"mousedown":$(d).bind("mu_"+b,c);$(d).bind(b,a,function(f){Mini2.EventManager.setMouseDown(this,null,null,f);$(this).triggerHandler("mu_mousedown",a)});break;case"mouseup":$(d).bind("mu_"+b,c);$(d).bind(b,a,function(f){Mini2.EventManager.setMouseUp(this,f)});break;case"mousemove":$(d).bind("mu_"+b,c);$(d).bind(b,a,function(f){Mini2.EventManager.setMouseMove(this,f)});break;default:$(d).bind(b,a,c);break}return d}});$(document).ready(function(){$(document.body).addCls("mi-webkit","mi-border-box")});