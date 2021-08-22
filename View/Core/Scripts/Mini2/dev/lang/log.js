
Mini2.define('Mini2.log.MethodLogger', {

    owner: null,

    name: null,


    debugFormat: function (text, params) {
        var me = this,
                    console = window.console,
                    i,
                    d = new Date(),
                    strD,
                    spaceStr = "";

        if (arguments.length > 1) {
            text = $.format(text, params);
        }


        if (console && console.log) {
            console.log(me.now() + 'DEBUG ' + me.className + ' - ' + spaceStr + text);
        }
    },

    debug: function (text, optionParams) {
        var me = this,
                    console = window.console,
                    i,
                    d = new Date(),
                    strD,
                    spaceStr = "";

        strD = me.now();

        if (console && console.log) {
            console.log(strD + 'DEBUG ' + me.className + ' - ' + spaceStr + text, optionParams);
        }
    }

    


});

Mini2.define('Mini2.log.Logger',{
    
    className: '',

    enabled : true,

    now: function () {
        var d = new Date(),
            ttt = d.getTime() + '',
            strD = d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "." + d.getMilliseconds() ;

        return strD;
    },

    getMethod:function(methodName){
        var me = this,
            methodLog;

        methodLog = Mini2.create('Mini2.log.MethodLogger', {
            owner: me,
            name : methodName
        });

        return methodLog;
    },

    debugFormat: function (text, params) {
        var me = this,
                    console = window.console,
                    i,
                    d = new Date(),
                    strD,
                    spaceStr = "";

        if (!me.enabled) {
            return;
        }

        if (arguments.length > 1) {
            text = $.format(text, params);
        }


        if (console && console.debug) {
            var logText = '%c' + me.now() + ' %cDEBUG %c' + me.className + ' - ' + spaceStr + text;

            console.debug(logText, 'color:#0', 'color:#00A49D', 'color:#0');
        }
    },

    debug: function (text, optionParams) {
        var me = this,
                    console = window.console,
                    i,
                    d = new Date(),
                    strD,
                    spaceStr = "";

        if (!me.enabled) {
            return;
        }
        
        if (console && console.debug) {

            var logText = '%c' + me.now() + ' %cDEBUG %c' + me.className + ' - ' + spaceStr + text;
            
            if (arguments.length == 1) {
                console.debug(logText, 'color:#0', 'color:#00A49D', 'color:#0');
            }
            else {
                console.debug(logText, 'color:#0', 'color:#00A49D', 'color:#0', optionParams);
            }
        }
    },



    error: function (text, optionParams) {
        var me = this,
                    console = window.console,
                    i,
                    d = new Date(),
                    strD,
                    spaceStr = "";

        if (console && console.error) {

            var logText = '%c' + me.now() + ' %ERROR %c' + me.className + ' - ' + spaceStr + text;

            if (arguments.length == 1) {
                console.error(logText, 'color:#0', 'color:red', 'color:#0');
            }
            else {
                console.error(logText, 'color:#0', 'color:red', 'color:#0', optionParams);
            }

            console.trace();
        }
    },

    fatal: function (text, params) {

    },

    info: function (text, params) {

    },

    warn: function (text, params) {

    },

    trace: function (text, params) {

    }


});

window.log = Mini2.Logger = new function () {
    "use strict";
    Mini2.apply(this, {

        space: -4,
        
        begin: function (text) {

            var console = window.console;

            this.space += 4;

            if (!text) {
                return;
            }

            var spaceStr = "";

            for (var i = 0; i < this.space; i++) {
                spaceStr += " ";
            }

            var d = new Date();

            var strD = d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "  ";

            if (console && console.log) {
                console.log(strD + spaceStr + "【" + text + "】");
            }
        },

        end: function (text) {

            if (text) {
                this.debug('【end】:' + text);
            }
            else {
                this.debug('【end】');
            }

            this.space -= 4;
        },


        error:function(text){
            var me = this,
                console = window.console,
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

            strD = me.now() + "  ";

            if (console && console.log) {
                console.log(strD + spaceStr + "[ERROR]" + text);
            }
        },

        debug: function (text, optionParams) {
            var me = this, 
                console = window.console,
                i,
                d = new Date(),
                strD,
                spaceStr = "";

            for (i = 0; i < this.space; i++) {
                spaceStr += " ";
            }

            strD = me.now() + "  ";

            if (arguments.length > 1) {
                text = $.format(text, optionParams);
            }
            

            if (console && console.log) {

                if (arguments.length == 1) {
                    console.log(strD + spaceStr + text);
                }
                else {
                    console.log(strD + spaceStr + text, optionParams);
                }
            }
        },



        now: function () {
            var d = new Date(),
                ttt = d.getTime() + '',
                strD = d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "." + d.getMilliseconds() ;

            return strD;
        },

        d: function (text, optionParams) {
            var me = this,
                d = new Date(),
                strD,
                spaceStr = "";



            var logText = '%c' + me.now() + ' %cDEBUG %c' + me.className + ' - ' + spaceStr + text;

            return logText;
        },


        getLogger: function (className, enabled) {
            if (enabled === console) {
                return console;
            }

            if (undefined == enabled) {
                enabled = true;
            }

            
            var item = Mini2.create('Mini2.log.Logger', {
                className: className,
                enabled : enabled
            });

            return item;
        }



    });
};
