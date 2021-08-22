


Mini2.Logger = new function () {
    "use strict";
    Mini2.apply(this, {

        space: -4,

        begin: function (text) {
            var me = this,
                console = window.console,
                spaceStr = '',
                d = new Date(),
                strD = '[' + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "] ";

            me.space += 4;

            if (!text) {
                return;
            }
            
            for (var i = 0; i < me.space; i++) {
                spaceStr += " ";
            }

            if (console && console.log) {
                console.log(strD + spaceStr + "【" + text + "】");
            }
        },

        end: function (text) {
            var me = this;

            if (text) {
                me.debug('【end】:' + text);
            }
            else {
                me.debug('【end】');
            }

            me.space -= 4;
        },


        error:function(text){
            var console = window.console,
                i,
                d = new Date(),
                strD,
                spaceStr = "";


            if (arguments.length > 1) {

                var args = [];

                for (var i = 1; i < arguments.length; i++) {
                    args.push(arguments[i - 1]);
                }

                //text = $.format(text, args);
            }


            for (i = 0; i < this.space; i++) {
                spaceStr += " ";
            }

            strD = d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "  ";

            if (console && console.log) {
                console.log(strD + spaceStr + "[ERROR]" + text);
            }
        },

        debug: function (text, params) {
            var me = this,
                console = window.console,
                i,
                d = new Date(),
                strD = "[" + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "] ",
                spaceStr = "";

            for (i = 0; i < me.space; i++) {
                spaceStr += " ";
            }
            
            if (arguments.length > 1) {
                text = $.format(text, params);
            }
            

            if (console && console.log) {
                console.log(strD + spaceStr + text);
            }
        }

    });
};
