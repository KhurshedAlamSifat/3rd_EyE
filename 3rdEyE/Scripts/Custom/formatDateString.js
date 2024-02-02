//Format: 2021-04-05 2:00:00 PM
function formatDateString_t1(inputString) {
    var inputDate = new Date(parseInt(inputString.replace("/Date(", "").replace(")/", "")));
    var outputString = "";
    var _year = inputDate.getFullYear();
    outputString = outputString + _year;
    var _month = inputDate.getMonth() + 1;
    if (_month < 10) {
        outputString = outputString + "-0" + _month;
    } else {
        outputString = outputString + "-" + _month;
    }
    var _date = inputDate.getDate();
    if (_date < 10) {
        outputString = outputString + "-0" + _date;
    } else {
        outputString = outputString + "-0" + _date;
    }
    //var _hour = inputDate.getHours();
    //if (_hour < 10) {
    //    outputString = outputString + " " + "0" + _hour;
    //} else {
    //    outputString = outputString + " " + (_hour % 12);
    //}
    var _hour = inputDate.getHours();
    outputString = outputString + " " + (_hour % 12);
    var _minute = inputDate.getMinutes();
    if (_minute < 10) {
        outputString = outputString + ":" + "0" + _minute;
    } else {
        outputString = outputString + ":" + _minute;
    }
    var _second = inputDate.getSeconds();
    if (_second < 10) {
        outputString = outputString + ":" + "0" + _second;
    } else {
        outputString = outputString + ":" + _second;
    }
    if (_hour < 12) {
        outputString = outputString + " " + "AM";
    } else {
        outputString = outputString + " " + "PM";
    }
    return outputString;
}