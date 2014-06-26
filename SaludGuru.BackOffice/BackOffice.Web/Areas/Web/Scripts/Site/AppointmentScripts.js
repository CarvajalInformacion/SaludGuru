/* Inicialización en español para la extensión 'UI date picker' para jQuery. */
jQuery(function ($) {
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: '&#x3c;Ant',
        nextText: 'Sig&#x3e;',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);
});


/*calendar render method*/

function renderAsyncCalendar(objCalendar) {
    //make ajax for special days
    $.ajax({
        type: "POST",
        url: '/api/Calendar?CountryId=' + objCalendar.CountryId + '&ProfilePublicId=' + objCalendar.ProfilePublicId + '&OfficePublicId=' + objCalendar.OfficePublicId + '&Date=' + objCalendar.FirstDate.getFullYear() + '-' + ((new Number(objCalendar.FirstDate.getMonth())) + 1) + '-1'
    }).done(function (data) {
        //left date picker
        setCalendarOptions(objCalendar, data);
    }).fail(function () {
        alert("error");
    });
}

function setCalendarOptions(objCalendar, lstSpecialDay) {
    
    if (objCalendar.Type == 'day') {

        //load calendar by day

        //left
        $('#' + objCalendar.DivId + '-Left').datepicker({
            dateFormat: 'yy-mm-dd',
            locale: $.datepicker.regional['es'],
            defaultDate: objCalendar.FirstDate,
            beforeShowDay: function (date) {
                //eval special day
                var oReturn = [true, ''];
                if (lstSpecialDay != null) {
                    $(lstSpecialDay).each(function (index, value) {
                        if (value.Year == date.getFullYear() && value.Month == (date.getMonth() + 1) && value.Day == date.getDate()) {
                            oReturn = [true, 'specialDay_' + value.SpecialDayType];
                        }
                    });
                }
                return oReturn;
            },
            onSelect: function (date) {
                //delete selected style in continuos calendar
                $('#' + objCalendar.DivId + '-Right .ui-state-active').removeClass('ui-state-active ui-state-hover');

                alert(date);
            },
        });

        //right
        $('#' + objCalendar.DivId + '-Right').datepicker({
            dateFormat: 'yy-mm-dd',
            locale: $.datepicker.regional['es'],
            defaultDate: objCalendar.SecondDate,
            beforeShowDay: function (date) {
                //eval special day
                var oReturn = [true, ''];
                if (lstSpecialDay != null) {
                    $(lstSpecialDay).each(function (index, value) {
                        if (value.Year == date.getFullYear() && value.Month == (date.getMonth() + 1) && value.Day == date.getDate()) {
                            oReturn = [true, 'specialDay_' + value.SpecialDayType];
                        }
                    });
                }
                return oReturn;
            },
            onSelect: function (date) {
                //delete selected style in continuos calendar
                $('#' + objCalendar.DivId + '-Left .ui-state-active').removeClass('ui-state-active ui-state-hover');

                alert(date);
            },
        });

        //delete selected style on right calendar
        $('#' + objCalendar.DivId + '-Right .ui-state-active').removeClass('ui-state-active ui-state-hover');
    }
}



