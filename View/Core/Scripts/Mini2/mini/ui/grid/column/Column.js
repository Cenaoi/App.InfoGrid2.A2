Mini2.define("Mini2.ui.grid.column.Colunm",{extend:"Mini2.ui.Component",width:120,headerWidth:120,handleWidth:4,headerAlign:"left",noWrap:true,emptyCellText:"&#160;",miType:"col",dataIndex:"",headerText:"&#160",renderer:false,editor:false,autoLock:false,resizable:true,hasCustomRenderer:false,lockedHeader:false,rowspan:undefined,menuDisabled:false,align:"left",isHeader:true,sortAsc:false,sortDesc:false,sortable:true,tdCls:"",innerCls:"",renderTpl:['<td role="gridcell" class="mi-grid-cell mi-grid-td ">','<div class="mi-grid-cell-inner">',"</div>","</td>"],initComponent:function(){var b=this,c,a;c=b.renderer;if(c){if(typeof c=="string"){}b.hasCustomRenderer=true}else{if(b.defaultRenderer){b.scope=b;b.renderer=b.defaultRenderer}}},hasLockedHeader:function(){return this.lockedHeader},defaultRenderer:function(a){return a},renderCell:function(){var b=this,a=Mini2.$joinStr(b.renderTpl);a.children(".mi-grid-cell-inner").css("text-align",b.align);return a},renderer:function(r,i,a,m,n,d,o,p){var l=this,b,q,f=l.editor,g=l.editorMode,c=l.displayStore,e=l.displayField,k=l.itemValueField||"value",j=l.itemDisplayField||"text";if(e){try{q=m.get(e)}catch(h){throw new Error("��ȡ�ֶΡ�"+e+"����ֵ����")}if(q&&q!=""){return q}}if(c&&("none"==g||(f&&"none"==f.mode))){b=c.filterFirstBy(k,r);if(b){q=b.get(j);return q}}return r},clearInvalid:function(e,a,b){var d=this;if($(a).hasClass("mi-grid-cell-invalid")){var c=$(a).children(".mi-grid-cell-invalid-icon");if(c.size()){$(c).remove()}$(a).removeClass("mi-grid-cell-invalid")}},markInvalid:function(i,a,b,g){var h=this,e=i.errors;if(g){e=g;if(e){c=e[0]}var d=$(a).children(".mi-grid-cell-invalid-icon");if(d.size()){$(d).remove()}var f=c.type.toLowerCase();d=Mini2.$joinStr(['<div role="presentation" class="mi-form-',f,'-msg mi-form-invalid-icon mi-grid-cell-invalid-icon" >',"</div>"]);d.attr("title",c.message);d.attr("data-errorqtip","");$(a).addClass("mi-grid-cell-invalid");$(a).append(d)}else{if(e&&e.length){var c=i.getErrors(b);if(c){c=c[0]}if(c){var d=$(a).children(".mi-grid-cell-invalid-icon");if(d.size()){$(d).remove()}var f=c.type.toLowerCase();d=Mini2.$joinStr(['<div role="presentation" class="mi-form-',f,'-msg mi-form-invalid-icon mi-grid-cell-invalid-icon" >',"</div>"]);d.attr("title",c.message);d.attr("data-errorqtip","");$(a).addClass("mi-grid-cell-invalid");$(a).append(d)}}}},getSortParam:function(){return this.dataIndex},doSort:function(g){var b=this,f="",a=b.direction,c=b.sortAsc,d=b.sortDesc,e=b.sortExpression,h=b.grid.getStore();if(!g){g=b.direction=("ASC"==a?"DESC":"ASC")}else{g=b.direction=g.toUpperCase()}if("ASC"==g&&c){f=c}else{if("DESC"==g&&d){f=d}else{if(e){f=e+" "+g}else{f=b.dataIndex+" "+g}}}h.sort(f)}});