Mini2.define("Mini2.ui.WindowManager",{singleton:true,items:[],defaultZIndex:19000,zIndex:19000,isUpdateLayout:false,regWin:function(d){var c=this,a=c.items,b=a.length;Mini2.Array.add(a,d);d.setZIndex(c.zIndex+b*20)},moveFirst:function(g){var f=this,c,a,e,b,d=f.items;b=Mini2.Array.lastIndexOf(d,g);e=d.length;if(b==e-1){return}for(a=b;a<e-1;a++){d[a]=d[a+1]}d[e-1]=g;for(a=b;a<e;a++){c=d[a];c.setZIndex(f.zIndex+a*20)}},removeWin:function(c){var b=this,a=b.items;Mini2.Array.remove(a,c);if(!a.length){b.zIndex=b.defaultZIndex}}});