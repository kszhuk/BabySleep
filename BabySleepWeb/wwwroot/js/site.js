function ShowBusyIndicator() {
    document.getElementById("loading").style.display = "block";
};

function HideBusyIndicator() {
    document.getElementById("loading").style.display = "none";
};

$(document).ready(function() {
    $('.dropdown-toggle').dropdown();
});

$(function() {
    $('.selectpicker').selectpicker();
});