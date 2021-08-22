

Mini2.Function = {

    bind: function (fn, scope, args, appendArgs) {
        "use strict";
        if (arguments.length === 2) {
            return function() {
                return fn.apply(scope, arguments);
            };
        }

        var method = fn,
            slice = Array.prototype.slice;

        return function() {
            var callArgs = args || arguments;

            if (appendArgs === true) {
                callArgs = slice.call(arguments, 0);
                callArgs = callArgs.concat(args);
            }
            else if (typeof appendArgs == 'number') {
                callArgs = slice.call(arguments, 0); // copy arguments first
                Mini2.Array.insert(callArgs, appendArgs, args);
            }

            return method.apply(scope || Ext.global, callArgs);
        };
    }

}