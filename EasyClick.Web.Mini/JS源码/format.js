Date.prototype.format = function (formatter) {
    if (!formatter || formatter == "") {
        formatter = "yyyy-MM-dd";
    }
    var year = this.getYear().toString();
    var month = (this.getMonth() + 1).toString();
    var day = this.getDate().toString();
    var yearMarker = formatter.replace(/[^y|Y]/g, '');
    if (yearMarker.length == 2) {
        year = year.substring(2, 4);
    }
    var monthMarker = formatter.replace(/[^m|M]/g, '');
    if (monthMarker.length > 1) {
        if (month.length == 1) {
            month = "0" + month;
        }
    }
    var dayMarker = formatter.replace(/[^d]/g, '');
    if (dayMarker.length > 1) {
        if (day.length == 1) {
            day = "0" + day;
        }
    }
    return formatter.replace(yearMarker, year).replace(monthMarker, month).replace(dayMarker, day);
}


