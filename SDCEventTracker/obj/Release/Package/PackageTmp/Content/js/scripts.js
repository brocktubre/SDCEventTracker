/*jQuery Datepikcer for choosing the date
//jqueryui.com/datepicker/*/
$(function () {
    $(".datepicker").datepicker();
});

/* Stripes the Competetions table 
http://stackoverflow.com/questions/4929538/c-sharp-mvc3-razor-alternating-items-in-a-foreach-list*/
$(function () {
    $("table > tbody tr:odd").css("background-color", "#D4A26A");
    $("table > tbody tr:even").css("background-color", "#FFD6AA");
    $("#top-row").css("background-color", "#AA7439");
    
});
