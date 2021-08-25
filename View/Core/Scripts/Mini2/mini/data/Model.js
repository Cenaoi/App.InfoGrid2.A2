Mini2.define("Mini2.data.Model",{alternateClassName:["Mini2.data.Record"],persistenceProperty:"data",emptyData:[],idField:[],idProperty:"id",dataSave:{},evented:false,isModel:true,editing:false,fields:[],modified:{},statics:{PREFIX:"Mini-record",AUTO_ID:1,EDIT:"edit",REJECT:"reject",COMMIT:"commit",id:function(a){}},onInit:function(b,g,r,a){var m=this,p=(g||g===0),e,d,k,c,n,s,o,q,j=m.idProperty,h=m.constructor.idField,l=m.constructor.lockedField,f;m.raw=r||b;m.modified={};m.data=b;m.stores=[];m.id=m.muid;d=m.fields;for(f=0;f<d.length;f++){c=new Mini2.data.Field(d[f]);d[f]=c}m.fields=new Mini2.collection.ArrayList(d);m.idField=h;m.lockedField=l},inheritableStatics:{setFields:function(b,c,a){},getFields:function(){return this.prototype.fields.items}},_singleProp:{},isEqual:function(c,d){if(c instanceof Date&&d instanceof Date){return c.getTime()===d.getTime()}return c===d},setFields:function(b,f,a){var h=this,j,d,e=false,k=h.prototype,l=k.fields,m=k.superClass.fields,g,c;if(f){k.idProperty=f;d=f.isField?f:new Mini2.data.Field(f)}if(a){k.clientIdProperty=a}if(l){l.clear()}else{l=h.prototype.fields=new Mini2.collection.ArrayList()}if(m){b=m.items.concat(b)}for(c=0,g=b.length;c<g;c++){j=new Mini2.data.Field(b[c]);if(d&&((j.mapping&&(j.mapping===d.mapping))||(j.name===d.name))){e=true;j.defaultValue=undefined}l.add(j)}if(d&&!e){d.defaultValue=undefined;l.add(d)}h.fields=l;return l},getId:function(){if(!this.idField){return null}var a=this.get(this.idField.name);if(a==undefined){a=null}return a},get:function(a){return this[this.persistenceProperty][a]},set:function(d,m){var h=this,b=h[h.persistenceProperty],e=h.fields,i=h.modified,o=(typeof d=="string"),a,c,f,g,j,k,n,l,p,q;if(o){q=h._singleProp;q[d]=m}else{q=d}for(k in q){if(q.hasOwnProperty(k)){p=q[k];c=(e?e.getByProp("name",k):null);if(e&&c&&c.convert){p=c.convert(p,h)}a=b[k];if((a==""&&p==null)||(a==null&&p=="")){continue}if(h.isEqual(a,p)){continue}b[k]=p;(j||(j=[])).push(k);if(c&&c.persist){if(i.hasOwnProperty(k)){if(h.isEqual(i[k],p)){delete i[k];h.dirty=false;for(g in i){if(i.hasOwnProperty(g)){h.dirty=true;break}}}}else{h.dirty=true;i[k]=a}}}}if(o){delete q[d]}if(!h.editing&&j){h.afterEdit(j)}if(h.editing&&j){h.afterCommit(j)}return j||null},callStore:function(b){var a=[],g=this.stores,c,d,e=g.length,f;for(d=0;d<arguments.length;d++){a[d]=arguments[d]}a[0]=this;for(c=0;c<e;++c){f=g[c];if(f&&Mini2.isFunction(f[b])){f[b].apply(f,a)}}},afterInvalid:function(a){this.callStore("afterInvalid",a)},afterEdit:function(a){this.callStore("afterEdit",a)},afterReject:function(){this.callStore("afterReject")},afterCommit:function(a){this.callStore("afterCommit",a)},isLocked:function(){var b=this,d,a=b.lockedField,c=b.store;if(c&&c.readOnly){return true}if(a){d=b.get(a);return !!d}return false},isLockedForField:function(a){var b=this;return false},beginEdit:function(){var c=this,b,a,d;if(!c.editing){c.editing=true;c.dirtySave=c.dirty;d=c[c.persistenceProperty];a=c.dataSave={};for(b in d){if(d.hasOwnProperty(b)){a[b]=d[b]}}d=c.modified;a=c.modifiedSave={};for(b in d){if(d.hasOwnProperty(b)){a[b]=d[b]}}}},cancelEdit:function(){var a=this;if(a.editing){a.editing=false;a.modified=a.modifiedSave;a[a.persistenceProperty]=a.dataSave;a.dirty=a.dirtySave;a.modifiedSave=a.dataSave=a.dirtySave=null}},endEdit:function(e,d){var c=this,b,a;e=e===true;if(c.editing){c.editing=false;b=c.dataSave;c.modifiedSave=c.dataSave=c.dirtySave=null;if(!e){if(!d){d=c.getModifiedFieldNames(b)}a=c.dirty||d.length>0;if(a){c.afterEdit(d)}}}},getModifiedFieldNames:function(e){var c=this,a=c[c.persistenceProperty],d=[],b;e=e||c.dataSave;for(b in a){if(a.hasOwnProperty(b)){if(!c.isEqual(a[b],e[b])){d.push(b)}}}return d},getChanges:function(){var c=this.modified,a={},b;for(b in c){if(c.hasOwnProperty(b)){a[b]=this.get(b)}}return a},reject:function(d){var b=this,c=b.modified,a;for(a in c){if(c.hasOwnProperty(a)){if(typeof c[a]!="function"){b[b.persistenceProperty][a]=c[a]}}}b.dirty=false;b.editing=false;b.modified={};if(d!==true){b.afterReject()}},commit:function(c,b){var a=this;a.phantom=a.dirty=a.editing=false;a.modified={};if(c!==true){a.afterCommit(b)}},isModified:function(a){return this.modified.hasOwnProperty(a)},joinStore:function(b){var a=this;if(!a.stores.length){a.stores[0]=b}else{Mini2.Array.include(this.stores,b)}this.store=this.stores[0]},unjoinStore:function(a){Mini2.Array.remove(this.stores,a);this.store=this.stores[0]||null},errors:false,markInvalid:function(a){var h=this,b=h.errors,g,f,d=a.field,e=[d];if(!b){h.errors=b={length:0,items:{}}}g=b.items;f=g[d];if(!f){g[d]=f=[]}f.push(a);b.length++;try{h.afterInvalid(e)}catch(c){alert("���ô���: Model.afterInvalid(error)",c.Message)}},clearInvalidAll:function(){var a=this;if(a.errors){delete a.errors;a.afterInvalid(fields)}},clearInvalid:function(b){var c=this,a=c.errors;if(a){if(b){delete a.items[b];c.afterInvalid(fields)}else{delete c.errors;c.errors=false;c.afterInvalid(fields)}}},getErrors:function(d){var f=this,b=f.errors,e,a;try{if(b){e=b.items;a=e[d]}}catch(c){alert(c.Message)}return a},isValid:function(){return this.validate().isValid()},validate:function(){var e=this,a=new Mini2.data.Errors(),j=e.validations,k=Mini2.data.validations,d,h,b,g,f,c;if(j){d=j.length;for(c=0;c<d;c++){h=j[c];b=h.field||h.name;f=h.type;g=k[f](h,e.get(b));if(!g){a.add({field:b,message:h.message||k[f+"Message"]})}}}return a}},function(){var a=this;Mini2.apply(a,arguments[0]);a.onInit(arguments[0]);a.muid=Mini2.getIdentity()});