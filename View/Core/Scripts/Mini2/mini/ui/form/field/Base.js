Mini2.define("Mini2.ui.form.field.Base",{extend:"Mini2.ui.Component",mixins:{labelable:"Mini2.ui.form.Labelable"},invalidCls:"mi-form-invalid",labelable:null,preventMark:false,disabled:false,renderTo:Mini2.getBody(),el:null,name:"",oldValue:"",newValue:"",autoComplete:"off",allowBlank:true,readOnly:false,align:"left",width:"100%",height:"",inputDom:null,initComponent:function(){var a=this;$(a).muBind("focusout",function(){if(a.fieldEl){a.fieldEl.removeClass("mi-form-focus")}a.el.removeClass("mi-form-trigger-wrap-focus");a.on("hided");a.clearInvalid()});a.initLabelable()},getRawValue:function(){var a=this,b;if(a.readOnly&&a.hideEl){b=a.hideEl.val()}else{b=a.inputEl.val()}return b},processRawValue:function(a){return a},isValid:function(){var b=this,a=b.disabled,c=b.forceValidation||!a;return c?b.validateValue(b.processRawValue(b.getRawValue())):a},getErrors:function(b){var a=this;if(b==""||b==null){return"����"}return""},validateValue:function(d){var c=this,a=c.getErrors(d),b=Mini2.isEmpty(a);if(!c.preventMark){if(b){c.clearInvalid()}else{c.markInvalid(a)}}return b},getActiveError:function(){var b=this,a=b.labelable;return a.getActiveError()},hasActiveError:function(){var b=this,a=b.labelable;return a.hasActiveError()},unsetActiveError:function(){var b=this,a=b.labelable;a.unsetActiveError()},clearInvalid:function(){var e=this,a=e.el,b=e.hasActiveError(),c=e.inputEl,d=e.invalidCls;a.removeClass(d);c.removeClass(d+"-field");e.unsetActiveError();if(b){e.setError("")}},markInvalid:function(b){var e=this,a=e.el,f=e.getActiveError(),c=e.inputEl,d=e.invalidCls;a.addClass(d);c.addClass(d+"-field");if(f!==b){e.setError(b)}return e},setError:function(a){var c=this,d=c.msgTarget,e,b=c.labelable;if(a==undefined||a==""||a==null){c.unsetActiveError()}else{if(b){b.setActiveError(a)}}},insertText:function(c,f){if(document.selection){var d=document.selection.createRange();d.text=f}else{if(typeof c.selectionStart==="number"&&typeof c.selectionEnd==="number"){var e=c.selectionStart,b=c.selectionEnd,a=e,g=c.value;c.value=g.substring(0,e)+f+g.substring(b,g.length);a+=f.length;c.selectionStart=c.selectionEnd=a}else{c.value+=f}}},moveEnd:function(b){b.focus();var a=b.value.length;if(document.selection){var c=b.createTextRange();c.moveStart("character",a);c.collapse();c.select()}else{if(typeof b.selectionStart=="number"&&typeof b.selectionEnd=="number"){b.selectionStart=b.selectionEnd=a}}},layoutReset:function(){},onPositionChanged:Mini2.emptyFn,isValueChanged:function(){var a=this;return a.oldValue!=a.getValue()},getLeft:function(){var a=this;return a.el.css("left")},setLeft:function(c){var b=this,a=b.el;a.css("left",c);return b},getTop:function(){var b=this,a=b.el;return a.css("top")},setTop:function(c){var b=this,a=b.el;a.css("top",c);return b},setWidth:function(c){var b=this,a=b.el;b.width=c;a.css("width",c);return b},getWidth:function(){return this.width},setHeight:function(b){var a=this;a.height=b;a.el.css("height",b);return a},getHeight:function(){return this.height},setValue:function(c){var b=this,a=b.hideEl;b.inputEl.val(c);if(a){a.val(c)}return b},getValue:function(){var b=this,a=b.hideEl,c;if(b.readOnly&&a){c=a.val()}else{c=b.inputEl.val()}return c},hide:function(){var a=this;a.el.hide();return a},focus:function(){var c=this,a=c.fieldEl,b=c.inputEl;Mini2.ui.FocusMgr.setControl(c.scope||c);if(a){a.addCls("mi-form-focus",c.focusCls)}c.el.addClass("mi-form-trigger-wrap-focus");if(!c.readOnly&&b&&b.focus){setTimeout(function(){b.focus()},200)}return c},select:function(){var a=this;a.inputEl.select()},onLostFocus:function(a){var b=this;b.inputEl.removeClass("mi-form-focus");b.on("hided")},show:function(){var a=this;a.el.show();return a},fieldCls:"mi-form-type-text",getTableContainer:function(){var b=this,c=b.mixins,a=c.labelable,d;if(b.labelable){d=b.labelable.labelableRenderTpl}else{d=Mini2.$joinStr(['<table class="mi-field mi-table-plain mi-form-item  mi-field-default mi-anchor-form-item" ','cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 100%; ">',"<tbody>",'<tr role="presentation" class="mi-form-item-input-row">','<td class="mi-form-item-body"></td>',"</tr>","</tbody>","</table>"])}d.addClass(b.fieldCls);d.css("width",b.width);return d},bindInputEl_Key:function(a){var b=this;$(a).bind("keydown",function(c){b.on("keydown",c)}).bind("keyup",function(c){b.on("keyup",c)}).bind("keypress",function(c){b.on("keypress",c)})},bindInputEl_Mouse:function(a){var b=this;$(a).muBind("mousedown",function(c){b.focus();b.on("mousedown",c)}).muBind("mouseup",function(c){b.on("mouseup",c);Mini2.EventManager.stopEvent(c)})},getCell:function(d,c,a){var e,b,f;e=$(d).children("tbody:first");if(e.size()==0){e=d}b=$(e).children("tr:eq("+c+")");f=$(b).children(":eq("+a+")");return f},appendCell:function(f,g,e,c){var d=this,b,a=arguments;if(!g){return}if(a.length==4){b=d.getCell(f,e,c)}else{if(Mini2.isString(a[2])){b=f.find(a[2])}}$(b).append(g);return d},initLabelable:function(){var d=this,e=d.mixins,c=e.labelable,b=d.labelable||{},a;if(b&&b.hideLable){return}b=Mini2.apply(b,{owner:d});a=Mini2.create(c,b);delete d.labelable;d.labelable=a},hideEl:null,createHideEl:function(){var b=this,c=b.name,a=$('<input type="hidden" value="" />');if(c){a.attr("name",c)}a.val(b.value||b.srcValue);return a},setReadOnly:function(f){var d=this,c=d.inputEl,b=d.hideEl;d.readOnly=f;if(f){c.attr("readonly","readonly")}else{c.removeAttr("readonly")}if(f){if(!b){c.attr("name","");d.hideEl=b=d.createHideEl();var a="td.mi-form-item-body:first";var e=d.getTableContainer();d.appendCell(e,b,a)}}else{if(b){c.attr("name",d.name);$(b).remove();delete d.hideEl}}$(c).change(function(){d.clearInvalid()});$(c).keydown(function(){d.clearInvalid()});return d},getReadOnly:function(){var a=this;return a.readOnly},getFieldEl:function(){var b=this,a=Mini2.$joinStr(['<input type="text" class="mi-form-field mi-form-text" ','aria-invalid="false" style="width: 100%; ">']);if(b.name){a.attr("name",b.name)}a.attr("id",b.clientId).attr("placeholder",b.placeholder).css("text-align",b.align).attr("autocomplete",b.autoComplete);b.bindInputEl_Mouse(a);b.bindInputEl_Key(a);b.inputEl=a;return a},renderForLableable:function(d){var c=this,b,a=c.labelable;if(a&&!a.hideLabel){b=a.getEl();if(c.clientId){b.attr("for",c.clientId)}b.muBind("mousedown",function(f){c.focus()});b.muBind("mouseup",function(f){Mini2.EventManager.stopEvent(f)});c.appendCell(d,b,"td.mi-field-label-cell:first")}return c},renderForBoxLabel:function(c){var b=this,a;if(b.boxLabel){a=Mini2.$joinStr(['<label class="mi-form-cb-label mi-form-cb-label-after" for="',b.clientId,'">',b.boxLabel,"</label>"]);a.muBind("mousedown",function(d){b.focus()});a.muBind("mouseup",function(d){Mini2.EventManager.stopEvent(d)});b.boxLableEl=a;b.appendCell(c,a,"td.mi-form-item-body:first")}return b},baseRender:function(){var c=this,a="td.mi-form-item-body:first";var d=c.getTableContainer();if(c.applyTo){$(c.applyTo).replaceWith(d)}else{$(c.renderTo).append(d)}c.fieldBodyEl=d.find(a);var b=c.getFieldEl();c.renderForLableable(d);c.appendCell(d,c.srcInputEl,a);c.appendCell(d,b,a);c.renderForBoxLabel(d);c.fieldEl=b;c.el=d;c.setReadOnly(c.readOnly);c.initValue();if(c.value!=undefined){c.setValue(c.value);delete c.value}return c},initValue:Mini2.emptyFn,render:function(){var a=this;a.baseRender();a.el.data("me",a)}});