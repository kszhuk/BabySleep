$(function () {
    var weekDate = new Date(new Date().setDate(new Date().getDate() - 7));
    var date = new Date();
    var endDatePicker = new Date(date.getFullYear(), date.getMonth(), date.getDate());

    $('#datepicker1').datepicker({
        autoclose: true,
        format: "MM dd, yyyy",
        immediateUpdates: true,
        startDate: weekDate,
        endDate: endDatePicker
    }).datepicker("setDate", weekDate).on('changeDate', function (e) {
        ReloadChart();
    });

    $('#datepicker2').datepicker({
        autoclose: true,
        format: "MM dd, yyyy",
        immediateUpdates: true,
        startDate: weekDate,
        endDate: endDatePicker
    }).datepicker("setDate", endDatePicker).on('changeDate', function (e) {
        ReloadChart();
    });

    ReloadChart();
});

function GetDate(dateControl) {
    return dateControl.data('datepicker').getFormattedDate('yyyy-mm-dd');
}

function ReloadChart() {
    ShowBusyIndicator();

    var startDate = GetDate($('#datepicker1'));
    var endDate = GetDate($('#datepicker2'));

    $.ajax({
        url: "/Statistics/GetStatistics",
        type: 'GET',
        data: {
            'startDate': startDate,
            'endDate': endDate
        },
        contentType: 'application/json',
        success: function (result) {
            CreateChart(result);
            HideBusyIndicator();
        },
        error: function (result) {
            HideBusyIndicator();
        }
    });
}

function CreateChart(dataJson) {
    var chartData = JSON.parse(dataJson);

    let chartStatus = Chart.getChart("statisticsChart");
    if (chartStatus !== undefined) {
        chartStatus.destroy();
    }

    var ctx = document.getElementById("statisticsChart").getContext("2d");
    var myChart = new Chart(ctx, {
        type: "line",
        data: {
            datasets: [
                {
                    label: chartData.NightHoursLabel,
                    data: chartData.NightHoursStatistics,
                    backgroundColor: "rgba(47,164,231,0.6)",
                    fill: true
                },
                {
                    label: chartData.DayHoursLabel,
                    data: chartData.DayHoursStatistics,
                    backgroundColor: "rgba(38,131,185,0.6)",
                    fill: true
                },
                {
                    label: chartData.TotalHoursLabel,
                    data: chartData.TotalHoursStatistics,
                    backgroundColor: "rgba(234,246,253,0.6)",
                    fill: true
                }
            ],
        },
        options: {
            elements: {
                line: {
                    tension: 0.4  // smooth lines
                },
            },
            scales: {
                y: {
                    //display: false,
                    type: 'time',
                    time: {
                        parser: 'HH:mm',
                        unit: 'hour',
                        displayFormats: {
                            hour: 'HH:mm'
                        },
                        tooltipFormat: 'HH:mm'
                    }
                }
            },
            parsing: {
                xAxisKey: 'Label',
                yAxisKey: 'ValueLabel'
            }
        }
    });
}