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

//render day calendar
function renderDayCalendar(DivId) {
    $('#' + DivId).fullCalendar({
        defaultView: 'agendaDay',
        //defaultView: 'agendaWeek',
        //defaultView: 'month',
        header: {
            left: '',
            center: 'title',
            right: 'month,agendaWeek,agendaDay',
        },
        titleFormat: '\'Consultorío 1\'',
        weekNumbers: false,
        columnFormat: {
            month: 'dddd',
            week: 'dddd',
            day: 'dddd'
        },
        editable: true,
        dayClick: function (date, jsEvent, view) {
            alert(date);
        },
        eventClick: function (event, jsEvent, view) {
            alert(event);
        },
        events: [{
            id: 'ABCDEF01',
            title: '<div id=\'div1j\'><img src=\'https://lh6.googleusercontent.com/-8MajLkkygS0/AAAAAAAAAAI/AAAAAAAAADM/FBzd750qjbg/photo.jpg\'/><div>Mario Casallas Garcia</div><div>Cedula: 80456258</div></div>',
            start: '2014-07-01T10:30:00',
            end: '2014-07-01T11:30:00',
            allDay: false,
            durationEditable: true,
            className: 'claseEvento_1',
        }],
        eventRender: function (event, element) {
            debugger;
            element.find('.fc-event-title').html(element.find('.fc-event-title').text());
            //element.addClass('claseEvento_1');
        }
    });
}

//init appointment grid
function AppointmentListGrid(vidDiv) {

    //configure grid
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateHeader").html() }],
        pageable: true,
        dataSource: {
            pageSize: 2,
            serverPaging: true,
            schema: {
                total: function (data) {
                    if (data && data.length > 0) {
                        return data[0].SearchProfileCount;
                    }
                    return 0;
                }
            },
            transport: {
                read: function (options) {
                    var oProfilePublicId = $('#' + vidDiv + '-ProfilePublicId').val();
                    $ajax({
                        url: '/api/AppointmentApi?ProfilePublicId=' + oProfilePublicId + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                        dataType: "json",
                        success: function (result) {
                            options.success(result);
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                }
            },
        },
        columns: [{
            field: "AppointmentId",
            title: " ",
            template: $("#templateName").html()
        }, {
            field: "CreateDate",
            title: " ",
        }, {
            field: "Status",
            title: " ",
        }],
    });

    //add search button event
    $('#' + vidDiv + '-Search').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
}

