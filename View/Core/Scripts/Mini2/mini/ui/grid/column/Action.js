Mini2.define("Mini2.ui.grid.column.Action",{extend:"Mini2.ui.grid.column.Colunm",sortable:false,menuDisabled:true,innerCls:function(){var b=this,a="mi-grid-cell-inner-action-col";if(b.autoHide){a+=" mi-grid-cell-autohide"}return a},tdCls:"mi-action-col-cell",renderItem_ForIcon:function(a,b){var c=$('<img role="button" alt="" src="" class="mi-action-col-icon" data-qtip="" style="margin-left:3px;margin-right:3px; " click="alert(\'dddddddddd\')" />');c.addClass("mi-action-col-"+a).attr("src",b.icon).attr("data-qtip",b.tooltip).attr("itemId",a).attr("title",b.text);return c},renderItem_ForText:function(a,b){var c=$('<a role="button" alt="" href="#" class="mi-action-col-icon" data-qtip="" style="margin-left:3px;margin-right:3px; "></a>');c.attr("itemId",a).html(b.text);return c},renderItem_ForIconText:function(a,b){var c=Mini2.$joinStr(['<a role="button" alt="" href="#" class="mi-action-col" data-qtip="" ','style="margin-left:3px;margin-right:3px; ">','<img src="" class="mi-action-col-icon" data-qtip="" />','<span class="mi-action-col-text">',b.text,"</span>","</a>"]);c.cFirst(".mi-action-col-icon").attr("src",b.icon).attr("itemId",a);return c},renderer:function(p,j,a,k,m,b,n){var h=this,c,d,e,f=h.items,g=f.length,l;l='<table border="0" cellpadding="0" cellspacing="0" align="center" ><tr>';for(c=0;c<g;c++){d=f[c];d.displayMode=d.displayMode||"icon";switch(d.displayMode){case"icontext":e=h.renderItem_ForIconText(c,d);break;case"icon":e=h.renderItem_ForIcon(c,d);break;case"text":e=h.renderItem_ForText(c,d);break}var o=$("<div></div>").append(e);l+="<td>"+o.html()+"</td>"}l+="</tr></table>";return l},bindEvent:function(j,b,h,c,d,g,i){var f=this,a=b.find("[role='button']");$(a).muBind("mousedown",Mini2.emptyFn).muBind("mouseup",function(e){f.processEvent("mouseup",j,b,h,c,e,g,i);Mini2.EventManager.stopEvent(e)})},processEvent:function(l,m,a,j,b,c,i,k){var h=c.currentTarget,d,g=this.items,f=parseInt($(h).attr("itemId"));d=g[f];if(Mini2.isFunction(d.handler)){d.handler.call(h,m,a,j,b,c,i,k)}}});