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
        url: '/api/Calendar?CountryId=' + objCalendar.CountryId + '&ProfilePublicId=' + objCalendar.ProfilePublicId + '&Date=' + objCalendar.FirstDate.getFullYear() + '-' + ((new Number(objCalendar.FirstDate.getMonth())) + 1) + '-1'
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

/*render day calendar*/
var MeetingObject = {

    /*office info*/
    lstOffice: new Array(),

    /*init meeting variables*/
    InitMeeting: function (vlstOffice) {

        $.each(vlstOffice, function (index, value) {
            MeetingObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    /*init meeting calendar by day*/
    InitByDay: function (DivId) {
        for (var item in this.lstOffice) {
            //create div to put a calendar
            this.lstOffice[item].OfficeDivId = 'divMetting_' + this.lstOffice[item].OfficePublicId;
            $('#' + DivId).append($('#divMetting').html().replace('divOfficePublicId', this.lstOffice[item].OfficeDivId));

            //init calendar
            this.InitOfficeByDay(this.lstOffice[item].OfficePublicId);
        }
    },

    InitOfficeByDay: function (vOfficePublicId) {

        //init one office calendar by day
        $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fullCalendar({
            defaultView: 'agendaDay',
            header: {
                left: '',
                center: 'title',
                right: '',
            },
            titleFormat: '\'' + this.lstOffice[vOfficePublicId].OfficeName + '\'',
            weekNumbers: false,
            columnFormat: {
                month: 'dddd',
                week: 'dddd',
                day: 'dddd'
            },
            editable: true,
            dayClick: function (date, jsEvent, view) {
                MeetingObject.RenderCreateAppointment(date, vOfficePublicId);
            },
            eventClick: function (event, jsEvent, view) {
                alert(event);
            },
            //events: [{
            //    id: 'ABCDEF01',
            //    title: '<div id=\'div1j\'><img src=\'https://lh6.googleusercontent.com/-8MajLkkygS0/AAAAAAAAAAI/AAAAAAAAADM/FBzd750qjbg/photo.jpg\'/><div>Mario Casallas Garcia</div><div>Cedula: 80456258</div></div>',
            //    start: '2014-07-01T10:30:00',
            //    end: '2014-07-01T11:30:00',
            //    allDay: false,
            //    durationEditable: true,
            //    className: 'claseEvento_1',
            //}],
            //eventRender: function (event, element) {
            //    debugger;
            //    element.find('.fc-event-title').html(element.find('.fc-event-title').text());
            //    //element.addClass('claseEvento_1');
            //}
        });
    },

    RenderCreateAppointment: function (vDate, vOfficePublicId) {
        //load office
        $('#selOffice').find('option').remove();
        $('#selOffice').unbind('change');
        for (var item in this.lstOffice) {

            $('#selOffice').append($('<option/>', {
                value: this.lstOffice[item].OfficePublicId,
                text: this.lstOffice[item].OfficeName,
                selected: (this.lstOffice[item].OfficePublicId == vOfficePublicId)
            }));
        }

        $('#selOffice').change(function () {
            debugger;
            MeetingObject.RenderCreateAppointment(vDate, $(this).val());
        });

        //load treatment
        $('#selTreatment').find('option').remove();
        $('#selTreatment').unbind('change');
        $.each(this.lstOffice[vOfficePublicId].TreatmentList, function (index, value) {
            $('#selTreatment').append($('<option/>', {
                value: value.TreatmentId,
                text: value.TreatmentName,
                selected: value.Default,
                duration: value.Duration,
            }));

            //set default duration
            if (value.Default == true) {
                $('#Duration').val(value.Duration);
            }
        });

        $('#selTreatment').change(function () {
            $('#Duration').val($(this).find(':selected').attr('duration'));
        });

        //load start date
        $('#StartDate').datepicker({
            altFormat: "yy-mm-dd"
        });

        $('#StartDate').datepicker("setDate", vDate);

        //load start time
        $('#StartTime').ptTimeSelect({
            hoursLabel: 'Hora',
            minutesLabel: 'Minutos',
            setButtonLabel: 'Aceptar',
        });
        var vMin = vDate.getMinutes();
        if (vMin < 10) {
            vMin = '0' + vDate.getMinutes();
        }

        if (vDate.getHours() <= 12) {
            $('#StartTime').val(vDate.getHours() + ':' + vMin + 'AM');
        }
        else {
            $('#StartTime').val((vDate.getHours() - 12) + ':' + vMin + ' PM');
        }
    },
};



//init appointment grid
function PatientAppointmentListGrid(vidDiv, vDataAppointment) {

    //configure grid
    $('#' + vidDiv).kendoGrid({
        dataSource: {
            type: "json",
            data: vDataAppointment
        },
        columns: [{
            field: "AppointmentPublicId",
            title: "Appointment ",
            template: $("#templateName").html()
        }, {
            field: "CreateDate",
            title: "CreateDate",
        }, {
            field: "StatusName",
            title: "Status",
        }],
    });
}
