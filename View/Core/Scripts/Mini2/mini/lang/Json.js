Mini2.Json=new function(){Mini2.apply(this,{toJson:function(b){if(b==undefined){return'""'}var c=[];if(typeof b=="string"){return'"'+b.replace(/([\"\\])/g,"\\$1").replace(/(\n)/g,"\\n").replace(/(\r)/g,"\\r").replace(/(\t)/g,"\\t").replace(/\+/g,"%2B")+'"'}if(Mini2.isDate(b)){return'"'+Mini2.Date.format(b,"Y-m-d H:i:s.u")+'"'}if(typeof b=="object"){if(!b.sort){for(var a in b){c.push('"'+a+'":'+Mini2.Json.toJson(b[a]))}if(!!document.all&&!/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(b.toString)){c.push("toString:"+b.toString.toString())}c="{"+c.join()+"}"}else{for(var a=0;a<b.length;a++){c.push(Mini2.Json.toJson(b[a]))}c="["+c.join()+"]"}return c}return b.toString().replace(/\"\:/g,'":""')}})};