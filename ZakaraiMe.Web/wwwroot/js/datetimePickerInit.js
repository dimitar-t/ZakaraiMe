function parseDatePicker() {
    let startDate = $("#datetimePicker").val();

    initDatepicker();

    let date = new Date(startDate);
    let year = date.getFullYear();

    if (year >= '2019') {
        let parsedStartDate = new Date(Date.parse(startDate));
        $('#datetimePicker').data('daterangepicker').setStartDate(parsedStartDate);
    }
}

function initDatepicker() {
    $("#datetimePicker").daterangepicker({
        singleDatePicker: true,
        timePicker: true,
        startDate: moment().startOf('hour').add(1, 'hour'),
        timePicker24Hour: true,
        opens: 'left',
        "locale": {
            "format": "DD-MM-YYYY HH:mm",
            "separator": " - ",
            "applyLabel": "Запази",
            "cancelLabel": "Откажи",
            "fromLabel": "От",
            "toLabel": "До",
            "customRangeLabel": "Custom",
            "weekLabel": "W",
            "daysOfWeek": [
                "Нд",
                "Пон",
                "Вт",
                "Ср",
                "Четв",
                "Пт",
                "Сб"
            ],
            "monthNames": [
                "Януари",
                "Февруари",
                "Март",
                "Април",
                "Май",
                "Юни",
                "Юли",
                "Август",
                "Септември",
                "Октомрви",
                "Ноември",
                "Декември"
            ],
            "firstDay": 1
        }
    });    
}