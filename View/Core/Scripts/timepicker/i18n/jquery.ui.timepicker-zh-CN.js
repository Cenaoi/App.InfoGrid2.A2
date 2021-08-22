/* Japanese initialisation for the jQuery time picker plugin. */
/* Written by Bernd Plagge (bplagge@choicenet.ne.jp). */
jQuery(function($){
    $.timepicker.regional['ja'] = {
                hourText: '时',
                minuteText: '分',
                amPmText: ['上午', '下午'] }
    $.timepicker.setDefaults($.timepicker.regional['zh-CN']);
});
