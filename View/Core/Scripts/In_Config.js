/// <reference path="In/in.js" />

var mV = "?v=1.3";

//In.add("jq", { path: "/Core/Scripts/jquery/jquery-1.7.2.min.js" });

In.add("layer", { path: '/Core/Scripts/layer/jquery.layout-latest.min.js', rely: [], type: 'js', charset: 'utf-8' });
In.add("jq.tools", { path: '/Core/Scripts/JQuery.Tools/jquery.tools.min.js', rely: [], type: 'js', charset: 'utf-8' });
In.add("jq.query", { path: '/Core/Scripts/JQuery.Query/jquery.query-2.1.7.min.js', rely: [], type: 'js', charset: 'utf-8' });

In.add("jq.metadata", { path: '/Core/Scripts/jquery.metadata.js', rely: [] });

In.add("jq.form", { path: '/Core/Scripts/jquery.form.js', rely: ['jq.metadata', "jq.validate"] });
In.add("jq.validate", { path: '/Core/Scripts/validate/jquery.validate-1.10.0.js', rely: ["jq.metadata"] });
In.add("ckeditor", { path: '/Core/Scripts/CKEditor_3.6/ckeditor.js', rely: [] });

//In.add("jq.ui.css", { path: "/Core/Scripts/ui-lightness/redmond/jquery-ui-1.8.18.custom.css" });
//In.add("jq.ui", { path: '/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js', rely: [ 'jq.ui.css'] });
//In.add("jq.ui.zh-CN", { path: '/Core/Scripts/ui-lightness/jquery.ui.datepicker-zh-CN.js', rely: ['jq.ui.css'] });

In.add("jq.ui.css", { path: "/Core/Scripts/In/In.Empty.js" });
In.add("jq.ui", { path: '/Core/Scripts/In/In.Empty.js', rely: ['jq.ui.css'] });
In.add("jq.ui.zh-CN", { path: '/Core/Scripts/In/In.Empty.js', rely: ['jq.ui.css'] });

In.add("jq.cookie", { path: '/Core/Scripts/jstree_pre1.0_stable/_lib/jquery.cookie.js', rely: [] });
In.add("jq.jstree", { path: '/Core/Scripts/jstree_pre1.0_stable/jquery.jstree.js', rely: ['jq.cookie'] });

In.add("jq.cleverTabs", { path: '/Core/Scripts/cleverTabs/jquery.cleverTabs.js', rely: [] });

In.add("jq.jtemplates", { path: '/Core/Scripts/jtemplates/jquery-jtemplates.js', rely: [] });


In.add('jq.sticky.css', { path: '/Core/Scripts/sticky/sticky.css' });
In.add("jq.sticky", { path: '/Core/Scripts/sticky/sticky.js', rely: ['jq.sticky.css'] });

In.add("jq.powerFloat", { path: '/Core/Scripts/jquery.powerFloat/jquery-powerFloat-min.js', rely: [] });

In.add("swfupload.default.css", { path: "/Core/Scripts/SWFUpload/default.css" });
In.add("swfupload", { path: "/Core/Scripts/SWFUpload/swfupload.js", rely: ["swfupload.default.css"] });
In.add("swfupload.fileprgress", { path: "/Core/Scripts/SWFUpload/fileprogress.js" });
In.add("swfupload.handlers", { path: "/Core/Scripts/SWFUpload/handlers.js", rely: ["swfupload", "swfupload.fileprgress"] });



/**** Mini 控件 ***/


In.add("mi._Default", { path: "/Core/Scripts/Mini/_Default.js" + mV });
In.add("mi.Widget", { path: "/Core/Scripts/Mini/Widget.js" + mV, rely: ["jq.ui.zh-CN", "mi._Default"] });


In.add("mi.Template", { path: "/Core/Scripts/Mini/Template.js" + mV, rely: ["mi._Default", "jq.jtemplates"] });

In.add("mi.DataGrid", { path: "/Core/Scripts/Mini/DataGrid.js" + mV, rely: ["mi._Default", "mi.Widget", "mi.Template", "mi.Pagination"] });
In.add("mi.DataGridView", { path: "/Core/Scripts/Mini/DataGridView.js" + mV, rely: ["mi._Default", "mi.Widget", "mi.Template", "mi.Pagination"] });
In.add("mi.DropDownText", { path: "/Core/Scripts/Mini/DropDownText.js" + mV, rely: ["mi._Default", "mi.Widget"] });
In.add("mi.EcView", { path: "/Core/Scripts/Mini/EcView.js" + mV, rely: ["mi._Default", "mi.Window"] });

In.add("mi.SelectItemField", { path: "/Core/Scripts/Mini/SelectItemField.js" + mV, rely: ["mi.DataGridView"] });
In.add("mi.EditorCheckBoxCell", { path: "/Core/Scripts/Mini/EditorCheckBoxCell.js" + mV, rely: ["mi.DataGridView"] });
In.add("mi.EditorDateCell", { path: "/Core/Scripts/Mini/EditorDateCell.js" + mV, rely: ["mi.DataGridView"] });
In.add("mi.EditorHtmlCell", { path: "/Core/Scripts/Mini/EditorHtmlCell.js" + mV, rely: ["mi.DataGridView"] });
In.add("mi.EditorSelect2Cell", { path: "/Core/Scripts/Mini/EditorSelect2Cell.js" + mV, rely: ["mi.DataGridView"] });
In.add("mi.EditorSelectCell", { path: "/Core/Scripts/Mini/EditorSelectCell.js" + mV, rely: ["mi.DataGridView"] });
In.add("mi.EditorTextButtonCell", { path: "/Core/Scripts/Mini/EditorTextButtonCell.js" + mV, rely: ["mi.DataGridView"] });
In.add("mi.EditorTextCell", { path: "/Core/Scripts/Mini/EditorTextCell.js" + mV, rely: ["mi.DataGridView"] });

In.add("mi.Pagination", { path: "/Core/Scripts/Mini/Pagination.js" + mV, rely: ["mi._Default"] });
In.add("mi.SWFUpload", { path: "/Core/Scripts/Mini/SWFUpload.js" + mV, rely: ["mi._Default", "swfupload", "swfupload.fileprgress", "swfupload.handlers"] });

In.add("mi.Tooltip", { path: "/Core/Scripts/Mini/Tooltip.js" + mV, rely: ["mi._Default", "jq.sticky"] });
In.add("mi.Window", { path: "/Core/Scripts/Mini/Window.js" + mV, rely: ["mi._Default", "jq.ui.zh-CN"] });
