Mini2.define("Mini2.ModelManager",{alternateClassName:"Mini2.ModelMgr",typeName:"mtype",singleton:true,types:{},create:function(b,e,d){var a=(typeof e=="function")?e:this.types[e||b.name];var c=new a(b,d);return c},registerType:function(c,a){var d=a.prototype,b;if(d&&d.isModel){b=a}else{if(!a.extend){a.extend="Mini2.data.Model"}b=Mini2.define(c,a)}this.types[c]=b;return b},getModel:function(a){var b=a;if(typeof b=="string"){b=this.types[b]}return b}});