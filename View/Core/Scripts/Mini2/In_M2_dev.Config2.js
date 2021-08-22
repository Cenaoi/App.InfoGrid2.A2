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
