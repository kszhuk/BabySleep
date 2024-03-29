﻿$(document).on('submit', '#partialform', function (e) {
    if (e.originalEvent.submitter.name == "Delete") {
        CallLambda("/Sleep/DeleteSleep", this.method, $(this).serialize());
    }

    var isValid = $('form').valid();
    e.preventDefault();
    if (isValid) {
        CallLambda(this.action, this.method, $(this).serialize());
    }
}); 

function CallLambda(urlAction, method, dataSerialized) {
    ShowBusyIndicator();
    $.ajax({
        url: urlAction,
        type: method,
        data: dataSerialized,
        success: function (result) {
            if (result.redirectToUrl != null) {
                window.location.href = result.redirectToUrl;
            }
            $("#modalSleepEntry").html(result);
            LoadDateTimePickers();
            HideBusyIndicator();
        },
        error: function (response) {
            HideBusyIndicator();
        }
    });
}

$(function () {
    $('#datepicker').datepicker({
        autoclose: true,
        format: "MM dd, yyyy",
        immediateUpdates: true,
        startDate: new Date(new Date().setDate(new Date().getDate() - 30)),  
        endDate: new Date(new Date().setDate(new Date().getDate() + 1))
    }).datepicker("setDate", new Date()).on('changeDate', function (e) {
        ChangeDate(e.date);
    });

    $('#modalSleepEntry').on('shown.bs.modal', function () {
        LoadDateTimePickers();
    });
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

function ChangeDate(date) {
    ShowBusyIndicator();

    $.ajax({
        url: "/Sleep/ChangeDate?date=" + date.toLocaleDateString("en-US"),
        type: 'GET',
        success: function (result) {
            $("#sleepsInfo").html(result);
            HideBusyIndicator();
        },
        error: function (result) {
            HideBusyIndicator();
        }
    });
};

function refreshModal() {
    $('#modalContent')[0].classList.remove('modal-blocked');
    $('#modalDelete').modal('hide');
}

function cleanDate(date) {
    return date.setHours(0, 0, 0, 0);
};

function AddEditSleep(sleepGuid) {
    ShowBusyIndicator();

    $.ajax({
        url: "/Sleep/AddEditSleep?sleepGuid=" + sleepGuid,
        type: 'GET',
        success: function (result) {
            $("#modalSleepEntry").html(result);
            $("#modalSleepEntry").modal("show");
            HideBusyIndicator();
        },
        error: function () {
            HideBusyIndicator();
        }
    }); 
};

function LoadDateTimePickers() {
    const linkedPicker1Element = document.getElementById('datetimepicker1');
    const linked1 = new tempusDominus.TempusDominus(linkedPicker1Element, {
        restrictions: {
            minDate: new Date(new Date().setDate(new Date().getDate() - 7)),
            maxDate: new Date(new Date().setDate(new Date().getDate() + 1))
        }
    });

    //using event listeners
    linkedPicker1Element.addEventListener(tempusDominus.Namespace.events.show, (e) => {
        $(".tempus-dominus-widget").css({ 'position': 'absolute', 'top': $('#datetimepicker1').offset().top + 30, 'left': $('#datetimepicker1').offset().left });
    });

    const linkedPicker2Element = document.getElementById('datetimepicker2');
    const linked2 = new tempusDominus.TempusDominus(linkedPicker2Element, {
        restrictions: {
            minDate: new Date(new Date().setDate(new Date().getDate() - 7)),
            maxDate: new Date(new Date().setDate(new Date().getDate() + 1))
        }
    });

    //using event listeners
    linkedPicker2Element.addEventListener(tempusDominus.Namespace.events.show, (e) => {
        $(".tempus-dominus-widget").css({ 'position': 'absolute', 'top': $('#datetimepicker2').offset().top + 30, 'left': $('#datetimepicker2').offset().left });
    });
}
