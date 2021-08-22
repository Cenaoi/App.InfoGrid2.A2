


Mini2.Array = new function () {
    "use strict";
    Mini2.apply(this, {

        clone: Mini2.clone,



        contains: function (srcArray, item, isValueType) {

            var isExist = false,
                i=0,
                len = srcArray.length;

            if (isValueType) {
                for (; i < len; i++) {
                    if (srcArray[i] == item) {
                        isExist = true;
                        break;
                    }
                }
            }
            else {
                for (; i < len; i++) {
                    if (srcArray[i] === item) {
                        isExist = true;
                        break;
                    }
                }
            }

            return isExist;
        },


        lastIndexOf: function (srcArray, item) {

            var n = -1,
                len = srcArray.length,
                index = len;

            while (index--) {
                if (srcArray[index] === item) {
                    n = index;
                    break;
                }
            }
            return n;
        },


        indexOf: function (srcArray, item) {

            var n = -1,
                i=0,
                len = srcArray.length;

            for (; i < len; i++) {
                if (srcArray[i] === item) {
                    n = i;
                    break;
                }
            }

            return n;
        },

        add: function (srcArray, item) {

            srcArray.push(item);

        },

        include: function (srcArray, item) {
            var c = [];
            if (this.contains(srcArray, item)) {
                return;
            }
            srcArray.push(item);
        },

        remove: function (srcArray, item) {
            var c = [],
                n = this.indexOf(srcArray, item);

            if (n > -1) {
                srcArray.splice(n, 1);
            }

            
        }

    });

};
