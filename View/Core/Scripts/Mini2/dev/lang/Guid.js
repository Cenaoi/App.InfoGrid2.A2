

Mini2.Guid = new function () {
    "use strict";
    Mini2.apply(this, {

        newGuid: function (formatParam) {

            var guid = "",
                i,
                n;

            formatParam = formatParam || 'D';
            
            if (formatParam == 'D') {

                for (i = 1; i <= 32; i++) {
                    n = Math.floor(Math.random() * 16.0).toString(16);

                    guid += n;

                    if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) {
                        guid += "-";
                    }
                }
            }
            else if(formatParam == 'N'){
                for (i = 1; i <= 32; i++) {
                    n = Math.floor(Math.random() * 16.0).toString(16);

                    guid += n;

                }
            }

            return guid;

        }

    });
};
