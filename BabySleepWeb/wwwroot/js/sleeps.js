$(function () {
    $('#datepicker').datepicker({
        autoclose: true,
        format: "MM dd, yyyy",
        immediateUpdates: true,
        startDate: new Date(new Date().setDate(new Date().getDate() - 30)),  
        endDate: new Date(new Date().setDate(new Date().getDate() + 1))
    }).datepicker("setDate", new Date());

});

$("#prev").click(function (e) {
    var dateNew = $('#datepicker').datepicker('getDate', '-1d');
    dateNew.setDate(dateNew.getDate() - 1);
    $('#datepicker').datepicker('setDate', dateNew);
    e.preventDefault();
});


$("#next").click(function (e) {
    var dateNew = $('#datepicker').datepicker('getDate', '+1d');
    dateNew.setDate(dateNew.getDate() + 1);
    if (cleanDate(dateNew) <= cleanDate(new Date())) {
        $('#datepicker').datepicker('setDate', dateNew);
    }
    e.preventDefault();
});

function cleanDate(date) {
    return date.setHours(0, 0, 0, 0);
}