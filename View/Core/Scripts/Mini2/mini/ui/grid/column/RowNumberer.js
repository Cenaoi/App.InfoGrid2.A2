Mini2.define("Mini2.ui.grid.column.RowNumberer",{extend:"Mini2.ui.grid.column.Colunm",headerText:"&#160",sortable:false,align:"center",width:23,dataIndex:"",rowspan:undefined,menuDisabled:true,autoLock:true,resizable:false,renderTpl:['<td role="gridcell" class="mi-grid-cell mi-grid-td mi-grid-cell-row-numberer ">','<div class="mi-grid-cell-inner mi-grid-cell-inner-row-numberer">',"</div>","</td>"],defaultRenderer:function(a){return a},renderCell:function(){var b=this,a=Mini2.$joinStr(b.renderTpl);if("left"!=b.align){a.css("text-align",b.align)}return a},renderer:function(j,c,a,e,g,b,i){var h=this.rowspan,d=i.currentPage,f=e.index;return g+1;if(h){c.tdAttr='rowspan="'+h+'"'}if(f==null){f=g;if(d>1){f+=(d-1)*i.pageSize}}return f+1}});