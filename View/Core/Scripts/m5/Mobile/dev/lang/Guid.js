

Mini2.Guid = new function () {
    "use strict";
    Mini2.apply(this, {

        newGuid: function () {

            var guid = "",
                i,
                n;
            
            for (i = 1; i <= 32; i++) {
                n = Math.floor(Math.random() * 16.0).toString(16);
                
                guid += n;
                
                if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) {
                    guid += "-";
                }
            }

            return guid;

        }

    });
};


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