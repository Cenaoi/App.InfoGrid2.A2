/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../define.js" />

//带按钮的文本框
Mini2.define('Mini2.ui.form.field.TextButton', {



    extend: 'Mini2.ui.form.field.Trigger',
    

    alias: 'widget.textbutton',

    //引发事件
    onButtonClick: function () {
        var me = this;

        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '文本框',
            buttons: [{
                text: '确定',
                width: 100
            }, {
                text: '取消',
                width: 100,
                click: Mini2.emptyFn
            }]
        });

        win.show();

    }

});