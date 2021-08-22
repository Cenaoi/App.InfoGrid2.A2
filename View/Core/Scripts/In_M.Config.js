/// <reference path="In/in.js" />
//临时过渡到 in.js 模式的配置文件.


In.add("ckeditor", { path: '/Core/Scripts/CKEditor_3.6/ckeditor.js', rely: [] });

In.add("ueditor.config", { path: '/Core/Scripts/UEditor/1.2.6.0/ueditor.config.js?v=12', rely: [] });
In.add("ueditor", { path: '/Core/Scripts/UEditor/1.2.6.0/ueditor.all.js', rely: ['ueditor.config'] });



In.add("jq.cookie", { path: '/Core/Scripts/jstree_pre1.0_stable/_lib/jquery.cookie.js', rely: [] });
In.add("jq.jstree", { path: '/Core/Scripts/jstree_pre1.0_stable/jquery.jstree.js', rely: ['jq.cookie'] });

In.add("swfupload.default.css", { path: "/Core/Scripts/SWFUpload/default.css" });
In.add("swfupload", { path: "/Core/Scripts/SWFUpload/swfupload.js", rely: ["swfupload.default.css"] });
In.add("swfupload.fileprgress", { path: "/Core/Scripts/SWFUpload/fileprogress.js" });
In.add("swfupload.handlers", { path: "/Core/Scripts/SWFUpload/handlers.js", rely: ["swfupload", "swfupload.fileprgress"] });



/**** Mini 控件 ***/

In.add("mi.SWFUpload", { path: "/Core/Scripts/Mini/SWFUpload.js", rely: [ "swfupload", "swfupload.fileprgress", "swfupload.handlers"] });
