Mini2.define("Mini2.ui.grid.property.Grid",{extend:"Mini2.ui.panel.Table",checkedMode:"SINGLE",onInit:function(){var b=this;if(b.isInit){return}b.isInit=true;b.pager={visible:false};b.columns=[{miType:"col",dataIndex:"Display",headerText:"����",width:115},{miType:"propertyColumn",dataIndex:"Value",headerText:"ֵ",editor:{xtype:"property"},width:120}];var c=Mini2.create("Mini2.data.Store",{storeId:"MiniStore_"+Mini2.getIdentity(),idField:"Name",fields:["Name","Value","Display","Type","Extend"]});b.bindStore(c);delete b.m_Cols;b.m_Cols=[];var a=0;$(b.columns).each(function(){b.getColumnForConfig(a,this);a++});b.cellTpl=b.StrJoin(b.cellTpl)}},function(){var a=this;Mini2.apply(this,arguments[0]);a.onInit(arguments[0]);a.muid=Mini2.getIdentity()});