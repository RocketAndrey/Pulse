// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function SetClock()
{
    dayarray = new Array("воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота");
    montharray = new Array("января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря");
    ndata = new Date();
    var time = [ndata.getHours(), ndata.getMinutes(), ndata.getSeconds()];
    // |[0] = Hours| |[1] = Minutes| |[2] = Seconds|
    day = dayarray[ndata.getDay()];
    month = montharray[ndata.getMonth()];
    date = ndata.getDate();
    year = ndata.getYear();
    

    if (time[0] < 10) { time[0] = "0" + time[0]; }
    if (time[1] < 10) { time[1] = "0" + time[1]; }
    if (time[2] < 10) { time[2] = "0" + time[2]; }

    var current_time = [time[0], time[1], time[2]].join(':');

    

    datastr = (day + " " + date + " " + month + " " + current_time);
    var clock = document.getElementById("clock");

    clock.innerHTML = "<h2>"+ datastr + "</h2>";
 

    setTimeout("SetClock()", 1000);
}