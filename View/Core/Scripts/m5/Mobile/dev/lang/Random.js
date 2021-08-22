
Mini2.Random = new function () {
    "use strict";

    Mini2.apply(this, {

        /**
        * 默认产生 8 位随机数
        */
        newNum: function (maxValue) {

            var max = maxValue || 10000000,
                randomNum = Math.random() * max;

            return Math.ceil(randomNum);
        }


    });


}