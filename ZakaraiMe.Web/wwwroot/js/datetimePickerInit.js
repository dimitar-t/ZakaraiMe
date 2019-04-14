function parseDatePicker() {
    let startDate = $("#datetimePicker").val();
    
    initDatepicker();

    let dateArray = startDate.split(/[\s,\-:]+/).map(Number);

    let date = new Date(dateArray[2], dateArray[1]-1, dateArray[0], dateArray[3], dateArray[4]);
    let year = date.getFullYear();

    // if the datepicker receives first date (01-01-0001) or an already inputed valid date (01-05-2019)
    if (year >= '2019') {
        $('#datetimePicker').data('daterangepicker').setStartDate(date);
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
            "culture": "en-GB",
            "separator": " - ",
            "autoUpdateInput": true,
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