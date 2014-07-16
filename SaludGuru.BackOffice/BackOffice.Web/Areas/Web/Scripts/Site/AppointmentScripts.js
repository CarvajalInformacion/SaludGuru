/*calendar render method*/
var CalendarObject = {

    /*calendar info*/
    DivId: '',
    CountryId: '',
    ProfilePublicId: '',
    StartDate: new Date(),
    EndDate: new Date(),
    FirstDate: new Date(),
    SecondDate: new Date(),

    /*init meeting calendar variables*/
    Init: function (vInitObject) {
        this.DivId = vInitObject.DivId;
        this.CountryId = vInitObject.CountryId;
        this.ProfilePublicId = vInitObject.ProfilePublicId;
        this.StartDate = vInitObject.StartDate;
        this.EndDate = vInitObject.EndDate;
        this.FirstDate = vInitObject.FirstDate;
        this.SecondDate = vInitObject.SecondDate;
    },

    RenderAsync: function () {
        //make ajax for special days
        $.ajax({
            type: "POST",
            url: '/api/Calendar?CountryId=' + this.CountryId + '&ProfilePublicId=' + this.ProfilePublicId + '&StartDate=' + serverDateToString(this.StartDate) + '&EndDate=' + serverDateToString(this.EndDate)
        }).done(function (data) {

            //left date picker
            CalendarObject.RenderCalendar('-Left', data, CalendarObject.FirstDate);
            //right date picker
            CalendarObject.RenderCalendar('-Right', data, CalendarObject.SecondDate);

        }).fail(function () {
            alert("se ha generado un error en el calendario");
        });
    },

    RenderCalendar: function (vDivId, vlstSpecialDay, vCurrentDate) {

        //render calendar
        $('#' + this.DivId + vDivId).datepicker({
            dateFormat: 'yy-mm-dd',
            locale: $.datepicker.regional['es'],
            defaultDate: vCurrentDate,
            beforeShowDay: function (date) {

                var oReturn = [true, ''];

                //eval selected date
                if (date.getFullYear() >= CalendarObject.StartDate.getFullYear() && date.getMonth() >= CalendarObject.StartDate.getMonth() && date.getDate() >= CalendarObject.StartDate.getDate() && date.getFullYear() <= CalendarObject.EndDate.getFullYear() && date.getMonth() <= CalendarObject.EndDate.getMonth() && date.getDate() < CalendarObject.EndDate.getDate()) {
                    oReturn = [true, ' selected'];
                }

                //eval special day
                if (vlstSpecialDay != null) {
                    $(vlstSpecialDay).each(function (index, value) {
                        if (value.Year == date.getFullYear() && value.Month == (date.getMonth() + 1) && value.Day == date.getDate()) {
                            oReturn = [true, 'specialDay_' + value.SpecialDayType];
                        }
                    });
                }
                return oReturn;
            },
            onSelect: function (date) {
                //delete selected style in continuos calendar
                //$('#' + this.DivId + '-Right .ui-state-active').removeClass('ui-state-active ui-state-hover');
                //alert(date + 'jairo');
                //window.location = '/Appointment/Day?Date=' + date;
            },
        });

        //delete selected style on calendar
        $('#' + this.DivId + vDivId + ' .ui-datepicker-current-day').removeClass('ui-datepicker-days-cell-over ui-datepicker-current-day');
        $('#' + this.DivId + vDivId + ' .ui-state-active').removeClass('ui-state-active ui-state-hover');
    },
};

/*render day calendar*/
var MettingCalendarObject = {

    /*meeting info*/
    DivId: '',
    CurrentAgentType: 'agendaDay',
    StartDateTime: new Date(),
    EndDateTime: new Date(),

    /*full calendar info*/
    dayNamesSp: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
    dayNamesShortSp: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],

    /*office info*/
    lstOffice: new Array(),

    /*init meeting variables*/
    Init: function (vInitObject) {

        //init meeting info
        this.DivId = vInitObject.DivId;
        this.CurrentAgentType = vInitObject.CurrentAgentType;
        this.StartDateTime = vInitObject.StartDateTime;
        this.EndDateTime = vInitObject.EndDateTime;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            MettingCalendarObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    /*init meeting calendar by day*/
    RenderAsync: function () {

        var TotalCalendars = 0;

        for (var item in this.lstOffice) {
            //create div to put a calendar
            this.lstOffice[item].OfficeDivId = 'divMetting_' + this.lstOffice[item].OfficePublicId;
            $('#' + this.DivId).append($('#divMetting').html().replace(/divOfficePublicId/gi, this.lstOffice[item].OfficeDivId));

            //init calendar
            this.RenderMettingCalendar(this.lstOffice[item].OfficePublicId);

            //add calendars count
            TotalCalendars = TotalCalendars + 1;
        }

        //TODO: Recalc dimension with bootstrap
        $('#' + this.DivId).width(($('#divOfficePublicId').width() * TotalCalendars) + 1);
    },

    RenderMettingCalendar: function (vOfficePublicId) {

        //get title
        var vTitle = $('#divMettingHeader').html();
        vTitle = vTitle.replace(/{{OfficeScheduleConfigUrl}}/gi, this.lstOffice[vOfficePublicId].OfficeScheduleConfigUrl);
        vTitle = vTitle.replace(/{{OfficeName}}/gi, this.lstOffice[vOfficePublicId].OfficeName);

        //init office metting calendar
        $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fullCalendar({
            dayNames: this.dayNamesSp,
            dayNamesShort: this.dayNamesShortSp,
            defaultView: this.CurrentAgentType,
            allDaySlot: false,
            allDayText: ' ',
            titleFormat: '\'' + vTitle + '\'',
            weekNumbers: false,
            editable: true,
            header: {
                left: '',
                center: 'title',
                right: '',
            },
            columnFormat: {
                month: ' ',
                week: 'ddd M/d',
                day: 'ddd M/d'
            },
            //dayClick: function (date, jsEvent, view) {
            //    //MeetingObject.RenderCreateAppointment(date, vOfficePublicId);
            //},
            //eventClick: function (event, jsEvent, view) {
            //    alert(event);
            //},
            //eventRender: function (event, element) {
            //    //debugger;
            //    element.find('.fc-event-title').html(element.find('.fc-event-title').text());
            //},
            //events: {
            //    url: '/api/AppointmentApi?OfficePublicId=' + vOfficePublicId + '&StartDateTime=' + serverDateTimeToString(this.StartDateTime) + '&EndDateTime=' + serverDateTimeToString(this.EndDateTime),
            //},
        });

        $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fullCalendar('gotoDate', this.StartDateTime);
    },
};


//RenderCreateAppointment: function (vDate, vOfficePublicId) {

//    //hidde create appointment form
//    $('#AppointmentUpsert').hide();
//    $('#AppointmentUpsertActions').hide();


//    //load office
//    $('#OfficePublicId').find('option').remove();
//    $('#OfficePublicId').unbind('change');
//    for (var item in this.lstOffice) {

//        $('#OfficePublicId').append($('<option/>', {
//            value: this.lstOffice[item].OfficePublicId,
//            text: this.lstOffice[item].OfficeName,
//            selected: (this.lstOffice[item].OfficePublicId == vOfficePublicId)
//        }));
//    }

//    $('#OfficePublicId').change(function () {
//        MeetingObject.RenderCreateAppointment(vDate, $(this).val());
//    });

//    //load treatment
//    $('#TreatmentId').find('option').remove();
//    $('#TreatmentId').unbind('change');
//    $.each(this.lstOffice[vOfficePublicId].TreatmentList, function (index, value) {
//        $('#TreatmentId').append($('<option/>', {
//            value: value.TreatmentId,
//            text: value.TreatmentName,
//            selected: value.Default,
//            officepublicid: vOfficePublicId,
//        }));

//        if (value.Default == true) {
//            //set duration
//            $('#Duration').val(value.Duration);
//            //set aftercare
//            $('#AfterCare').val(value.AfterCare);
//            //set beforecare
//            $('#BeforeCare').val(value.BeforeCare);
//        }
//    });

//    $('#TreatmentId').change(function () {
//        //get treatment id
//        var ovTreatmentId = $(this).val();
//        var ovOfficePublicId = $(this).find(':selected').attr('officepublicid');

//        //set input dependencies
//        $.each(MeetingObject.lstOffice[ovOfficePublicId].TreatmentList, function (index, value) {
//            if (value.TreatmentId == ovTreatmentId) {
//                //set duration
//                $('#Duration').val(value.Duration);
//                //set aftercare
//                $('#AfterCare').val(value.AfterCare);
//                //set beforecare
//                $('#BeforeCare').val(value.BeforeCare);
//            }
//        });
//    });

//    $('#CatId_TreatmentId').val('0');

//    //init duration spinner
//    $('#Duration').spinner({
//        min: 10,
//        step: 5,
//    });

//    //load start date
//    $('#StartDate').datepicker({
//        altFormat: "yy-mm-dd"
//    });

//    $('#StartDate').datepicker("setDate", vDate);

//    //load start time
//    $('#StartTime').ptTimeSelect({
//        hoursLabel: 'Hora',
//        minutesLabel: 'Minutos',
//        setButtonLabel: 'Aceptar',
//    });
//    var vMin = vDate.getMinutes();
//    if (vMin < 10) {
//        vMin = '0' + vDate.getMinutes();
//    }

//    if (vDate.getHours() <= 12) {
//        $('#StartTime').val(vDate.getHours() + ':' + vMin + 'AM');
//    }
//    else {
//        $('#StartTime').val((vDate.getHours() - 12) + ':' + vMin + ' PM');
//    }

//    //load patient autocomplete
//    $('#lstPatient').html('');
//    $('#PatientAppointmentCreate').val('');
//    $('#PatientAppointmentDelete').val('');
//    $('#getPatient').autocomplete(
//    {
//        //source: acData,
//        source: function (request, response) {
//            $.ajax({
//                url: '/api/PatientApi?SearchCriteria=' + request.term + '&PageNumber=0&RowCount=10',
//                dataType: 'json',
//                success: function (data) {
//                    response(data);
//                }
//            });
//        },
//        minLength: 0,
//        focus: function (event, ui) {
//            $('#getPatient').val(ui.item.Name);
//            return false;
//        },
//        select: function (event, ui) {
//            MeetingObject.AddPatientAppointment({
//                ProfileImage: ui.item.ProfileImage,
//                Name: ui.item.Name,
//                IdentificationNumber: ui.item.IdentificationNumber,
//                Mobile: ui.item.Mobile,
//                Email: ui.item.Email,
//                PatientPublicId: ui.item.PatientPublicId
//            });

//            $('#getPatient').val('');
//            return false;
//        }
//    }).data("ui-autocomplete")._renderItem = function (ul, item) {

//        var RenderItem = $('#divPatientAcItem').html();
//        RenderItem = RenderItem.replace(/{ProfileImage}/gi, item.ProfileImage);
//        RenderItem = RenderItem.replace(/{Name}/gi, item.Name);
//        RenderItem = RenderItem.replace(/{IdentificationNumber}/gi, item.IdentificationNumber);
//        RenderItem = RenderItem.replace(/{Mobile}/gi, item.Mobile);

//        return $("<li></li>")
//		    .data("ui-autocomplete-item", item)
//		    .append("<a><strong>" + RenderItem + "</strong></a>")
//		    .appendTo(ul);
//    };

//    //set appointment id
//    $('#AppointmentPublicId').val('');

//    //display create appointment form
//    $('#AppointmentUpsert').fadeIn('slow');
//    $('#AppointmentUpsertActions').fadeIn('slow');
//},

//AddPatientAppointment: function (vPatientModel) {
//    if ($('#PatientAppointmentCreate').val().indexOf(vPatientModel.PatientPublicId) == -1) {
//        var ApPatHtml = $('#ulPatientAppointment').html();
//        ApPatHtml = ApPatHtml.replace(/{ProfileImage}/gi, vPatientModel.ProfileImage);
//        ApPatHtml = ApPatHtml.replace(/{Name}/gi, vPatientModel.Name);
//        ApPatHtml = ApPatHtml.replace(/{IdentificationNumber}/gi, vPatientModel.IdentificationNumber);
//        ApPatHtml = ApPatHtml.replace(/{Mobile}/gi, vPatientModel.Mobile);
//        ApPatHtml = ApPatHtml.replace(/{Email}/gi, vPatientModel.Email);
//        ApPatHtml = ApPatHtml.replace(/{PatientPublicId}/gi, vPatientModel.PatientPublicId);
//        $('#lstPatient').append(ApPatHtml);
//        $('#PatientAppointmentCreate').val($('#PatientAppointmentCreate').val() + ',' + vPatientModel.PatientPublicId);
//        $('#PatientAppointmentDelete').val($('#PatientAppointmentDelete').val().replace(new RegExp(vPatientModel.PatientPublicId, 'gi'), ''));
//    }
//},

//RemovePatientAppointment: function (vPatientPublicId) {
//    $('#lstPatient').find('#' + vPatientPublicId).remove();
//    $('#PatientAppointmentDelete').val($('#PatientAppointmentDelete').val() + ',' + vPatientPublicId);
//    $('#PatientAppointmentCreate').val($('#PatientAppointmentCreate').val().replace(new RegExp(vPatientPublicId, 'gi'), ''));
//},

//SaveAppointment: function () {
//    $("#frmAppointment").submit(function (e) {
//        var postData = $(this).serializeArray();
//        var formURL = $(this).attr("action");
//        $.ajax(
//        {
//            url: formURL,
//            type: "POST",
//            data: postData,
//            success: function (data, textStatus, jqXHR) {
//                window.location.reload();
//            },
//            error: function (jqXHR, textStatus, errorThrown) {
//                alert('Se ha generado un error creando la cita.');
//            }
//        });
//        e.preventDefault(); //STOP default action
//        e.unbind(); //unbind. to stop multiple form submit.
//    });

//    $("#frmAppointment").submit();
//}
