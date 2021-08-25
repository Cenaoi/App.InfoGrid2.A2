Mini2.define("Mini2.ui.form.CheckboxGroup",{extend:"Mini2.ui.form.field.Base",mixins:{labelable:"Mini2.ui.form.Labelable"},labeldable:null,columns:"auto",vertical:false,blankText:"You must select at least one item in this group",defaultType:"checkboxfield",defaultItemType:"Mini2.ui.form.field.CheckBox",groupCls:Mini2.baseCSSPrefix+"form-check-group",extraFieldBodyCls:Mini2.baseCSSPrefix+"form-checkboxgroup-body",layout:"checkboxgroup",componentCls:Mini2.baseCSSPrefix+"form-checkboxgroup",items:[],initComponent:function(){var f=this,b,d=f.items,e=d.length,c,a;$(f).muBind("focusout",function(){f.fieldEl.removeClass("mi-form-focus");f.el.removeClass("mi-form-trigger-wrap-focus");f.on("hided")});f.initLabelable();for(b=0;b<e;b++){c=Mini2.apply({hideLabel:true,name:f.name,width:""},d[b]);a=Mini2.create(f.defaultItemType,c);d[b]=a}},initValue:function(){var a=this,b=a.value;a.originalValue=a.lastValue=b||a.getValue();if(b){a.setValue(b)}},getValue:function(){},setValue:function(a){},setHeight:function(a){},createGroupCell:function(e,d,a){var b,c,f,g;f=e.find("tbody:first");for(b=0;b<d;b++){g=$("<tr></tr>");f.append(g);for(c=0;c<a;c++){g.append('<td class="mi-form-radio-group" valign="top"></td>')}}},getGroupTable:function(){var e=this,c=e.items,d=c.length,b=e.columns,a,i,g,h,f;h=Mini2.$joinStr(["<div>",'<table id="radiogroup-',e.muid,'-innerCt" class="mi-table-plain" cellpadding="0" role="presentation" style="table-layout: auto;">',"<tbody>","</tbody>","</table>","</div>"]);if(b=="auto"||b<=1){e.createGroupCell(h,1,d);f={el:h,row:1,col:d}}else{a=parseInt(b);i=d%a;g=d/(d-i);if(i>0){g++}e.createGroupCell(h,g,a);f={el:h,row:g,col:a}}return f},getFieldEl:function(){var k=this,d,h,e,a,g=k.items;var c=k.getGroupTable();var l=c.row;var b=c.col;c=c.el;var m=c.children("table");var f=0;for(d=0;d<l;d++){for(h=0;h<b;h++){a=k.getCell(m,d,h);e=k.items[f++];e.renderTo=a;e.render();k.bindItem(e);e.el.css("margin","0px 15px 0px 0px")}}return c},bindItem:function(a){},render:function(){var c=this,a="td.mi-form-item-body:first",d,b;d=c.getTableContainer();if(c.applyTo){$(c.applyTo).replaceWith(d)}else{$(c.renderTo).append(d)}c.fieldBodyEl=d.find(a);b=c.getFieldEl();c.renderForLableable(d);c.appendCell(d,b,a);c.renderForBoxLabel(d);c.fieldEl=b;c.el=d;c.el.data("me",c);c.el.addClass(c.componentCls);c.fieldBodyEl.addClass(c.extraFieldBodyCls);c.el.css({height:"24px"})}});