Mini.util.DateHelper = function () {

    this.StartByToday = function () {
        var d = new Date();

        return d.format("yyyy-MM-dd");
    }

    this.StartByMonth = function () {
        var d = new Date()
        var dd = new Date(d.getYear(), d.getMonth(), 1);
        return dd.format("yyyy-MM-dd");
    };

    this.StartByYear = function () {
        var d = new Date()
        var dd = new Date(d.getYear(), 0, 1);
        return dd.format("yyyy-MM-dd");
    };



    this.Today = function () {
        var d = new Date();
        return d.format("yyyy-MM-dd");
    };
};

Mini.DateHelper = new Mini.util.DateHelper();