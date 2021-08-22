Mini2.String=(function(){var l=/^[\x09\x0a\x0b\x0c\x0d\x20\xa0\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u2028\u2029\u202f\u205f\u3000]+|[\x09\x0a\x0b\x0c\x0d\x20\xa0\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u2028\u2029\u202f\u205f\u3000]+$/g,g=/('|\\)/g,i=/\{(\d+)\}/g,h=/([-.*+?\^${}()|\[\]\/\\])/g,a=/^\s+|\s+$/g,n=/\s+/,m=/(^[^a-z]*|[^\w])/gi,c,e,d,f,k=function(p,o){return c[o]},j=function(p,o){return(o in e)?e[o]:String.fromCharCode(parseInt(o.substr(2),10))},b=function(p,o){if(p===null||p===undefined||o===null||o===undefined){return false}return o.length<=p.length};return{insert:function(q,r,o){if(!q){return r}if(!r){return q}var p=q.length;if(!o&&o!==0){o=p}if(o<0){o*=-1;if(o>=p){o=0}else{o=p-o}}if(o===0){q=r+q}else{if(o>=q.length){q+=r}else{q=q.substr(0,o)+r+q.substr(o)}}return q},startsWith:function(q,r,o){var p=b(q,r);if(p){if(o){q=q.toLowerCase();r=r.toLowerCase()}p=q.lastIndexOf(r,0)===0}return p},endsWith:function(r,o,p){var q=b(r,o);if(q){if(p){r=r.toLowerCase();o=o.toLowerCase()}q=r.indexOf(o,r.length-o.length)!==-1}return q},createVarName:function(o){return o.replace(m,"")},htmlEncode:function(o){return(!o)?o:String(o).replace(d,k)},htmlDecode:function(o){return(!o)?o:String(o).replace(f,j)},addCharacterEntities:function(s){var o=[],q=[],r,p;for(r in s){p=s[r];e[r]=p;c[p]=r;o.push(p);q.push(r)}d=new RegExp("("+o.join("|")+")","g");f=new RegExp("("+q.join("|")+"|&#[0-9]{1,5};)","g")},resetCharacterEntities:function(){c={};e={};this.addCharacterEntities({"&amp;":"&","&gt;":">","&lt;":"<","&quot;":'"',"&#39;":"'"})},urlAppend:function(p,o){if(!Mini2.isEmpty(o)){return p+(p.indexOf("?")===-1?"?":"&")+o}return p},trim:function(o){return o.replace(l,"")},capitalize:function(o){return o.charAt(0).toUpperCase()+o.substr(1)},uncapitalize:function(o){return o.charAt(0).toLowerCase()+o.substr(1)},ellipsis:function(q,p,s){if(q&&q.length>p){if(s){var r=q.substr(0,p-2),o=Math.max(r.lastIndexOf(" "),r.lastIndexOf("."),r.lastIndexOf("!"),r.lastIndexOf("?"));if(o!==-1&&o>=(p-15)){return r.substr(0,o)+"..."}}return q.substr(0,p-3)+"..."}return q},escapeRegex:function(o){return o.replace(h,"\\$1")},escape:function(o){return o.replace(g,"\\$1")},toggle:function(p,q,o){return p===q?o:q},leftPad:function(r,q,o){var p=String(r);o=o||" ";while(p.length<q){p=o+p}return p},format:function(p){var o=Mini2.Array.toArray(arguments,1);return p.replace(i,function(r,q){return o[q]})},repeat:function(r,p,s){if(p<1){p=0}for(var o=[],q=p;q--;){o.push(r)}return o.join(s||"")},splitWords:function(o){if(o&&typeof o=="string"){return o.replace(a,"").split(n)}return o||[]}}}());Mini2.String.resetCharacterEntities();Mini2.htmlEncode=Mini2.String.htmlEncode;Mini2.htmlDecode=Mini2.String.htmlDecode;Mini2.urlAppend=Mini2.String.urlAppend;