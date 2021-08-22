/// <reference path="In/in.js" />
//临时过渡到 in.js 模式的配置文件.

function GetRandomNum(Min, Max) {
    var Range = Max - Min;
    var Rand = Math.random();
    return (Min + Math.round(Rand * Range));
}

var rnum = "?_rnum=" + GetRandomNum(10000, 99999999);
var mu2Path = '/Core/Scripts/Mini2/dev'; 
var css2Path = '/Core/Scripts/Mini2/Themes';

//In.add('jquery', { path: '/Core/Scripts/jquery/jquery-1.8.3.js', rely: [] });
In.add('jquery', { path: '/Core/Scripts/In/In.Empty.js', rely: [] });


In.add('jq.ui.core', { path: '/Core/Scripts/jquery.ui/ui/jquery.ui.core.js', rely: ['jquery'] });
In.add('jq.ui.datepicker', { path: '/Core/Scripts/jquery.ui/ui/jquery.ui.datepicker.js', rely: ['jq.ui.core'] });

In.add("jq.ui.datepicker.zh-CN", { path: '/Core/Scripts/ui-lightness/jquery.ui.datepicker-zh-CN.js', rely: ['jq.ui.datepicker'] });



In.add('Mini2.template', { path: mu2Path + '/template.min.js', rely: [] });


In.add('Mini2.Mini', { path: mu2Path + '/Mini.js' + rnum, rely: ['jquery'] });


In.add('Mini2.lang.Function', { path: mu2Path + '/lang/Function.js' + rnum, rely: ['Mini2.Mini'] });
In.add('Mini2.lang.Json', { path: mu2Path + '/lang/Json.js' + rnum, rely: ['Mini2.Mini'] });
In.add('Mini2.lang.Array',       { path: mu2Path + '/lang/Array.js' + rnum, rely: ['Mini2.Mini'] });
In.add('Mini2.lang.Date', { path: mu2Path + '/lang/Date.js' + rnum, rely: ['Mini2.Mini'] });
In.add('Mini2.lang.String', { path: mu2Path + '/lang/String.js' + rnum, rely: ['Mini2.Mini'] });
In.add('Mini2.lang.Number', { path: mu2Path + '/lang/Number.js' + rnum, rely: ['Mini2.Mini'] });
In.add('Mini2.lang.log', { path: mu2Path + '/lang/log.js' + rnum, rely: ['Mini2.Mini'] });

In.add('Mini2.Mini-more', { path: mu2Path + '/Mini-more.js' + rnum,
    rely: [
        'Mini2.Mini',
        'Mini2.lang.Function',
        'Mini2.lang.Json',
        'Mini2.lang.Date',
        'Mini2.lang.Array',
        'Mini2.lang.String',
        'Mini2.lang.Number',
        'Mini2.lang.log']
});

In.add('Mini2.collection.ArrayList', { path: mu2Path + '/collection/ArrayList.js' + rnum, rely: ['Mini2.Mini-more'] });


In.add('Mini2.EventHandler', { path: mu2Path + '/EventHandler.js' + rnum, rely: ['Mini2.Mini-more', 'Mini2.collection.ArrayList'] });
In.add('Mini2.EventManager', { path: mu2Path + '/EventManager.js' + rnum, rely: ['Mini2.Mini-more', 'Mini2.collection.ArrayList'] });
In.add('Mini2.ModelManager', { path: mu2Path + '/ModelManager.js' + rnum, rely: ['Mini2.Mini-more', 'Mini2.collection.ArrayList'] });

In.add('Mini2.LoaderManager', { path: mu2Path + '/LoaderManager.js' + rnum, rely: ['Mini2.Mini-more'] });


In.add('Mini2.data.ResultSet', { path: mu2Path + '/data/ResultSet.js', rely: ['Mini2.Mini-more'] });
In.add('Mini2.data.Field', { path: mu2Path + '/data/Field.js' + rnum, rely: ['Mini2.Mini-more'] });
In.add('Mini2.data.Model', { path: mu2Path + '/data/Model.js' + rnum, rely: ['Mini2.ModelManager', 'Mini2.data.Field'] });

In.add('Mini2.data.StoreManager', { path: mu2Path + '/data/StoreManager.js' + rnum, rely: ['Mini2.Mini-more'] });
In.add('Mini2.data.Store', { path: mu2Path + '/data/Store.js' + rnum, rely: ['Mini2.data.StoreManager', 'Mini2.data.Model', 'Mini2.data.ResultSet'] });


In.add('Mini2.ui.toolbar.Toolbar', { path: mu2Path + '/ui/toolbar/Toolbar.js' + rnum, rely: ['Mini2.ui.panel.Panel'] });


In.add('Mini2.ui.FocusManager', { path: mu2Path + '/ui/FocusManager.js' + rnum, rely: ['Mini2.Mini-more', 'Mini2.collection.ArrayList', 'Mini2.LoaderManager'] });
In.add('Mini2.ui.Component', { path: mu2Path + '/ui/Component.js' + rnum, rely: ['Mini2.ui.FocusManager', 'Mini2.EventManager', 'Mini2.EventHandler'] });



In.add('Mini2.ui.layout.container.Form', { path: mu2Path + '/ui/layout/container/Form.js' + rnum, rely: ['Mini2.Mini-more'] });

In.add('Mini2.ui.container.DockingContainer', { path: mu2Path + '/ui/container/DockingContainer.js' + rnum, rely: ['Mini2.Mini-more'] });
In.add('Mini2.ui.container.Viewport', { path: mu2Path + '/ui/container/Viewport.js' + rnum, rely: ['Mini2.ui.panel.Panel'] });


In.add('Mini2.ui.panel.Panel', { path: mu2Path + '/ui/panel/Panel.js' + rnum,
    rely: [
        'Mini2.ui.Component',
        'Mini2.ui.container.DockingContainer',
        'Mini2.ui.layout.container.Form'] 
});



In.add('Mini2.ui.Editor', { path: mu2Path + '/ui/Editor.js' + rnum, rely: ['Mini2.ui.Component'] });

In.add('Mini2.ui.WindowManager', { path: mu2Path + '/ui/WindowManager.js' + rnum, rely: ['Mini2.Mini-more'] });

In.add('Mini2.ui.Window.css', { path: css2Path + '/theme-window.css' + rnum, rely: [] });
In.add('Mini2.ui.WindowHeader', { path: mu2Path + '/ui/WindowHeader.js' + rnum, rely: ['Mini2.Mini-more'] });
In.add('Mini2.ui.Window', { path: mu2Path + '/ui/Window.js' + rnum,
    rely: [
        'Mini2.ui.Component',
        'Mini2.ui.WindowHeader',
        'Mini2.ui.WindowManager',
        'Mini2.ui.button.Button',
        'Mini2.ui.panel.Panel',
        'Mini2.ui.Window.css']
});

In.add('Mini2.ui.MessageBox', { path: mu2Path + '/ui/MessageBox.js' + rnum, rely: ['Mini2.ui.Window','Mini2.ui.form.Label'] });



/** mu2.ui.form.field **/

In.add('Mini2.ui.button.Button',          { path: mu2Path + '/ui/button/Button.js' + rnum ,       rely: ['Mini2.ui.Component']});

In.add('Mini2.ui.form.Labelable',         { path: mu2Path + '/ui/form/Labelable.js' + rnum,       rely: ['Mini2.ui.Component'] });
In.add('Mini2.ui.form.CheckboxGroup',     { path: mu2Path + '/ui/form/CheckboxGroup.js' + rnum,   rely: ['Mini2.ui.form.field.Base','Mini2.ui.form.field.CheckBox'] });
In.add('Mini2.ui.form.RadioGroup',        { path: mu2Path + '/ui/form/RadioGroup.js' + rnum,      rely: ['Mini2.ui.form.CheckboxGroup', 'Mini2.ui.form.field.Radio'] });
In.add('Mini2.ui.form.Label',             { path: mu2Path + '/ui/form/Label.js' + rnum,           rely: ['Mini2.ui.form.field.Base'] });

In.add('Mini2.ui.form.field.Base',        { path: mu2Path + '/ui/form/field/Base.js' + rnum,      rely: ['Mini2.ui.Component','Mini2.ui.form.Labelable'] }); ;
In.add('Mini2.ui.form.field.Hidden',      { path: mu2Path + '/ui/form/field/Hidden.js',           rely: ['Mini2.ui.form.field.Base'] });
In.add('Mini2.ui.form.field.CheckBox',    { path: mu2Path + '/ui/form/field/CheckBox.js' + rnum,  rely: ['Mini2.ui.form.field.Base'] });
In.add('Mini2.ui.form.field.Text',        { path: mu2Path + '/ui/form/field/Text.js' + rnum,      rely: ['Mini2.ui.form.field.Base'] });
In.add('Mini2.ui.form.field.TextArea',    { path: mu2Path + '/ui/form/field/TextArea.js' + rnum,  rely: ['Mini2.ui.form.field.Text'] });
In.add('Mini2.ui.form.field.Trigger',     { path: mu2Path + '/ui/form/field/Trigger.js' + rnum,   rely: ['Mini2.ui.form.field.Text'] });
In.add('Mini2.ui.form.field.ComboBox',    { path: mu2Path + '/ui/form/field/ComboBox.js' + rnum,  rely: ['Mini2.ui.form.field.Trigger','Mini2.data.Store'] });
In.add('Mini2.ui.form.field.Date', { path: mu2Path + '/ui/form/field/Date.js' + rnum, rely: ['Mini2.ui.form.field.Trigger', 'jq.ui.datepicker.zh-CN'] });
In.add('Mini2.ui.form.field.Number',      { path: mu2Path + '/ui/form/field/Number.js' + rnum,    rely: ['Mini2.ui.form.field.Text'] });
In.add('Mini2.ui.form.field.Radio', { path: mu2Path + '/ui/form/field/Radio.js' + rnum, rely: ['Mini2.ui.form.field.CheckBox'] });



/** mu2.ui.Pagination **/

In.add('Mini2.ui.Pagination', { path: mu2Path + '/ui/Pagination.js' + rnum, rely: ['Mini2.Mini-more','Mini2.data.Store'] });


/** mu2.ui.grid.field **/

In.add('Mini2.ui.grid.header.Container', { path: mu2Path + '/ui/grid/header/Container.js' + rnum, rely: ['Mini2.Mini-more'] });

In.add('Mini2.ui.grid.column.Column', { path: mu2Path + '/ui/grid/column/Column.js' + rnum, rely: ['Mini2.ui.Component'] });
In.add('Mini2.ui.grid.column.Action', { path: mu2Path + '/ui/grid/column/Action.js' + rnum, rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.Boolean', { path: mu2Path + '/ui/grid/column/Boolean.js', rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.Date', { path: mu2Path + '/ui/grid/column/Date.js', rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.CheckColumn',   { path: mu2Path + '/ui/grid/column/CheckColumn.js', rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.RowNumberer',   { path: mu2Path + '/ui/grid/column/RowNumberer.js', rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.RowCheckColumn', { path: mu2Path + '/ui/grid/column/RowCheckColumn.js' + rnum, rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.Number', { path: mu2Path + '/ui/grid/column/Number.js', rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.Template', { path: mu2Path + '/ui/grid/column/Template.js', rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.grid.column.PropertyColumn', { path: mu2Path + '/ui/grid/column/PropertyColumn.js', rely: ['Mini2.ui.grid.column.Column'] });


In.add('Mini2.ui.grid.CellEditor', { path: mu2Path + '/ui/grid/CellEditor.js' + rnum,
    rely: [
        'Mini2.ui.Editor',
        'Mini2.ui.form.field.Base',
        'Mini2.ui.form.field.Text',
        'Mini2.ui.form.field.Trigger',
        'Mini2.ui.form.field.ComboBox',
        'Mini2.ui.form.field.TextArea',
        'Mini2.ui.form.field.CheckBox',
        'Mini2.ui.form.field.Date',
        'Mini2.ui.form.field.Number']
});


In.add('Mini2.ui.panel.AbstractTable', { path: mu2Path + '/ui/panel/AbstractTable.js', rely: ['Mini2.ui.grid.column.Column'] });
In.add('Mini2.ui.panel.Table', { path: mu2Path + '/ui/panel/Table.js' + rnum,
    rely: [
        'Mini2.ui.panel.AbstractTable',

        'Mini2.template',
        'Mini2.data.Model',
        'Mini2.data.Store',

        //'Mini2.ui.grid.CellEditor',
        'Mini2.ui.Pagination',

        'Mini2.ui.grid.header.Container',


        

        'Mini2.ui.grid.column.Column',
        'Mini2.ui.grid.column.Action',
        'Mini2.ui.grid.column.Boolean',
        'Mini2.ui.grid.column.CheckColumn',
        'Mini2.ui.grid.column.Date',
        'Mini2.ui.grid.column.RowNumberer',
        'Mini2.ui.grid.column.RowCheckColumn',
        'Mini2.ui.grid.column.Number',
        'Mini2.ui.grid.column.Template']
});

In.add('Mini2.ui.grid.property.Grid', { path: mu2Path + '/ui/grid/property/Grid.js' + rnum, 
    rely: [
        'Mini2.ui.panel.Table',
        'Mini2.ui.grid.column.PropertyColumn'] 
});