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
    },

    RenderAsync: function () {
        //make ajax for special days
        $.ajax({
            type: "POST",
            url: '/api/Calendar?CountryId=' + this.CountryId + '&ProfilePublicId=' + this.ProfilePublicId,
        }).done(function (data) {

            //left date picker
            CalendarObject.RenderCalendar(data, CalendarObject.FirstDate);

        }).fail(function () {
            alert("se ha generado un error en el calendario");
        });
    },

    RenderCalendar: function (vlstSpecialDay, vCurrentDate) {

        //render calendar
        $('#' + this.DivId).datepicker({
            dateFormat: 'yy-mm-dd',
            locale: $.datepicker.regional['es'],
            defaultDate: vCurrentDate,
            numberOfMonths: [1, 2],
            beforeShowDay: function (date) {

                var oReturn = [true, ''];

                //eval selected date
                if (CalendarObject.GetNumberDate(date) >= CalendarObject.GetNumberDate(CalendarObject.StartDate) && CalendarObject.GetNumberDate(date) < CalendarObject.GetNumberDate(CalendarObject.EndDate)) {
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
                if (window.location.pathname.toLowerCase() == '/appointment/detail') {
                    var strUrl = window.location.pathname + '?Date=' + date;
                    $.each(window.location.search.replace('?', '').split('&'), function (item, value) {
                        if (value.toLowerCase().indexOf('appointmentpublicid') >= 0) {
                            strUrl = strUrl + '&' + value;
                        }
                    });
                    //appointment detail
                    window.location = strUrl;
                }
                else {
                    //appointment day, week, month
                    window.location = window.location.pathname + '?Date=' + date + '&SelectedOffice=' + MettingCalendarObject.CurrentPublicOfficeId;
                }
            },
        });

        //delete selected style on calendar
        $('#' + this.DivId + ' .ui-datepicker-current-day').removeClass('ui-datepicker-days-cell-over ui-datepicker-current-day');
        $('#' + this.DivId + ' .ui-state-active').removeClass('ui-state-active ui-state-hover');
    },

    GetNumberDate: function (vDateToEval) {

        var oReturn = '';

        oReturn = vDateToEval.getFullYear();

        if (vDateToEval.getMonth() < 10) {
            oReturn = oReturn + '0' + (vDateToEval.getMonth()).toString();
        }
        else {
            oReturn = oReturn + (vDateToEval.getMonth()).toString();
        }

        if (vDateToEval.getDate() < 10) {
            oReturn = oReturn + '0' + (vDateToEval.getDate()).toString();
        }
        else {
            oReturn = oReturn + (vDateToEval.getDate()).toString();
        }

        return oReturn;
    },

    Refresh: function () {

        $('#' + this.DivId).datepicker("refresh");

    },

};

/*render day calendar*/
var MettingCalendarObject = {

    /*meeting info*/
    DivId: '',
    CurrentAgentType: 'agendaDay',
    StartDateTime: new Date(),
    EndDateTime: new Date(),
    CurrentPublicOfficeId: '',

    /*full calendar info*/
    dayNamesSp: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
    dayNamesShortSp: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
    monthNamesSp: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],

    /*office info*/
    lstOffice: new Array(),
    optionOffice: '',

    /*init meeting variables*/
    Init: function (vInitObject) {

        //init meeting info
        this.DivId = vInitObject.DivId;
        this.CurrentAgentType = vInitObject.CurrentAgentType;
        this.StartDateTime = vInitObject.StartDateTime;
        this.EndDateTime = vInitObject.EndDateTime;

        //init office option list
        this.optionOffice = '<select id="selOffice_{OfficePublicId}">';

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            //set key value pair for an office
            MettingCalendarObject.lstOffice[value.OfficePublicId] = value;
            //add office option
            MettingCalendarObject.optionOffice = MettingCalendarObject.optionOffice + '<option value="' + value.OfficePublicId + '">' + value.OfficeName + '</option>';
        });

        this.optionOffice = this.optionOffice + '</select>';
    },

    /*init meeting calendar by day*/
    RenderAsync: function () {
        //render calendars by month
        for (var item in this.lstOffice) {
            //create div to put a calendar
            this.lstOffice[item].OfficeDivId = 'divMetting_' + this.lstOffice[item].OfficePublicId;
            $('#' + this.DivId).append($('#divMetting').html().replace(/divOfficePublicId/gi, this.lstOffice[item].OfficeDivId));

            if (this.lstOffice[item].IsSelected == true) {
                this.RenderMettingCalendar(this.lstOffice[item].OfficePublicId);
            }
        }
    },

    RenderMettingCalendar: function (vOfficePublicId) {
        if ($('#' + this.lstOffice[vOfficePublicId].OfficeDivId).length == 1) {

            //hide all calendars
            $('#' + this.DivId + ' .SelectorMettingCalendar').hide();

            //show new office
            $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fadeIn('slow');

            //set current office public id
            this.CurrentPublicOfficeId = vOfficePublicId;

            //eval if render calendar
            if ($('#' + this.lstOffice[vOfficePublicId].OfficeDivId).children().length == 0) {

                //get title
                var vTitle = $('#divMettingHeader').html();

                vTitle = vTitle.replace(/{OfficeList}/gi, this.optionOffice);
                vTitle = vTitle.replace(/{OfficeScheduleConfigUrl}/gi, this.lstOffice[vOfficePublicId].OfficeScheduleConfigUrl);
                vTitle = vTitle.replace(/{OfficePublicId}/gi, vOfficePublicId);

                //get parameters by calendar type
                var oEventUrl = '/api/AppointmentApi?OfficePublicId=' + vOfficePublicId + '&StartDateTime=' + serverDateTimeToString(this.StartDateTime) + '&EndDateTime=' + serverDateTimeToString(this.EndDateTime);
                var oEditable = true;
                var oLeft = '';
                var oRight = '';

                if (this.CurrentAgentType == 'month') {
                    oEventUrl = '/api/AppointmentApi?OfficePublicId=' + vOfficePublicId + '&StartDate=' + serverDateToString(this.StartDateTime);
                    oEditable = false;
                    var oLeft = 'prev';
                    var oRight = 'next';
                }


                //init office metting calendar
                $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fullCalendar({
                    dayNames: this.dayNamesSp,
                    dayNamesShort: this.dayNamesShortSp,
                    monthNames: this.monthNamesSp,
                    defaultView: this.CurrentAgentType,
                    allDaySlot: false,
                    allDayText: ' ',

                    contentHeight: this.CalendarHeight(),
                    slotMinutes: this.lstOffice[vOfficePublicId].SlotMinutes,
                    firstHour: 8,
                    minTime: 5,
                    maxTime: 20,
                    slotEventOverlap: true,

                    titleFormat: '\'' + vTitle + '\'',
                    weekNumbers: false,
                    editable: oEditable,
                    header: {
                        left: oLeft,
                        center: 'title',
                        right: oRight,
                    },
                    columnFormat: {
                        month: ' ddd ',
                        week: ' dddd ',
                        day: ' dddd '
                    },
                    viewRender: function (view, element) {

                        //select current office
                        $('#selOffice_' + vOfficePublicId).val(vOfficePublicId);

                        //init on change event
                        $('#selOffice_' + vOfficePublicId).unbind('change');
                        $('#selOffice_' + vOfficePublicId).change(function () {
                            //show new office
                            MettingCalendarObject.RenderMettingCalendar($(this).val());

                            //keep selected current office
                            $('#selOffice_' + vOfficePublicId).val(vOfficePublicId);
                        });
                    },
                    dayClick: function (date, jsEvent, view) {

                        if (MettingCalendarObject.CurrentAgentType == 'month') {
                            window.location = '/Appointment/Day?Date=' + serverDateToString(date) + '&SelectedOffice=' + MettingCalendarObject.CurrentPublicOfficeId;
                        }
                        else if (MettingCalendarObject.CurrentAgentType == 'basicDay' && window.location.pathname.toLowerCase() == '/appointment/detail') {

                        }
                        else {
                            UpsertAppointmentObject.RenderForm(date, vOfficePublicId, null);
                        }
                    },
                    eventClick: function (event, jsEvent, view) {
                        if (MettingCalendarObject.CurrentAgentType == 'month') {
                            window.location = '/Appointment/Day?Date=' + serverDateToString(event.start) + '&SelectedOffice=' + MettingCalendarObject.CurrentPublicOfficeId;
                        }
                        else if (MettingCalendarObject.CurrentAgentType == 'basicDay' && window.location.pathname.toLowerCase() == '/appointment/detail') {

                            var strUrl = window.location.pathname + '?AppointmentPublicId=' + event.id;

                            $.each(window.location.search.replace('?', '').split('&'), function (item, value) {
                                if (value.toLowerCase().indexOf('date') >= 0) {
                                    strUrl = strUrl + '&' + value;
                                }
                            });

                            //appointment detail
                            window.location = strUrl;

                        }
                        else {
                            UpsertAppointmentObject.RenderForm(null, null, event);
                        }
                    },
                    eventAfterRender: function (event, element, view) {
                        if (element.find('.fc-event-title').text().length > 0) {
                            element.find('.fc-event-title').html(element.find('.fc-event-title').text());
                        }
                        else {
                            element.find('.fc-event-time').html(element.find('.fc-event-time').text());
                        }
                    },
                    events: {
                        url: oEventUrl,
                    },
                });
                //set start date
                $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fullCalendar('gotoDate', this.StartDateTime);
            }
            else {
                //refresh calendar events
                this.RefreshMettingCalendar(vOfficePublicId);
            }
        }
    },

    CalendarHeight: function () {
        var contentHeightAux = $(window).height() - 170;

        if (contentHeightAux < 300) {
            contentHeightAux = 300;
        }
        return contentHeightAux;
    },

    Refresh: function () {

        this.RefreshMettingCalendar(this.CurrentPublicOfficeId);

    },

    RefreshMettingCalendar: function (vOfficePublicId) {

        $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fullCalendar('refetchEvents');

    },

    ToggleCancelAppointment: function () {
        $('.AppointmentStatus_1203').toggle('slow');
    }
};

var UpsertAppointmentObject = {

    /*appointment info*/
    DivId: '',
    DivBlockId: '',

    /*init appointment variables*/
    Init: function (vInitObject) {

        //init meeting info
        this.DivId = vInitObject.DivId;
        this.DivBlockId = vInitObject.DivBlockId;
    },

    /*render appointment form*/
    RenderForm: function (vStartDate, vOfficeInfo, vAppointmentInfo) {

        //hidde create appointment form
        $('#' + this.DivId).hide();
        $('#' + this.DivBlockId).hide();

        //get current values

        //current appointment status
        var oCurrentAppointmentStatus = '1201';
        if (vAppointmentInfo != null) {
            oCurrentAppointmentStatus = vAppointmentInfo.AppointmentStatus;
        }

        //validate if is render appointment block
        if (oCurrentAppointmentStatus == 1207) {
            this.RenderBlockForm(vAppointmentInfo);
            return;
        }


        //set appointment id
        $('#AppointmentPublicId').val('');
        if (vAppointmentInfo != null) {
            $('#AppointmentPublicId').val(vAppointmentInfo.AppointmentPublicId);
        }

        //current office
        var oCurrentOfficePublicId = '';
        if (vOfficeInfo != null) {
            oCurrentOfficePublicId = vOfficeInfo;
        }
        else if (vAppointmentInfo != null) {
            oCurrentOfficePublicId = vAppointmentInfo.OfficePublicId;
        }

        //current treatment
        var oCurrentTreatmentId = 0;

        if (vAppointmentInfo != null) {
            oCurrentTreatmentId = vAppointmentInfo.TreatmentId;
        }

        //current start date and duration
        var oCurrentStartDate = new Date();
        var oCurrentStartTime = '';
        var oCurrentDuration = 0;

        if (vStartDate != null) {

            //get start date
            oCurrentStartDate = vStartDate;

            //get start time
            var vMin = vStartDate.getMinutes();
            if (vMin < 10) {
                vMin = '0' + vStartDate.getMinutes();
            }

            if (vStartDate.getHours() <= 12) {
                oCurrentStartTime = vStartDate.getHours() + ':' + vMin + ' AM';
            }
            else {
                oCurrentStartTime = (vStartDate.getHours() - 12) + ':' + vMin + ' PM';
            }
        }
        else if (vAppointmentInfo != null) {
            oCurrentStartDate = vAppointmentInfo.StartDate;
            oCurrentStartTime = vAppointmentInfo.StartTime;
            oCurrentDuration = vAppointmentInfo.Duration;
        }

        //render office
        this.RenderOffice(oCurrentOfficePublicId, vStartDate, vAppointmentInfo)

        //render treatment duration startdate and starttime
        this.RenderTreatment(oCurrentOfficePublicId, oCurrentDuration, oCurrentTreatmentId, oCurrentStartDate, oCurrentStartTime);

        //render patient appointment
        this.RenderPatient(vAppointmentInfo);

        //render appointment actions
        this.RenderActions(vAppointmentInfo, oCurrentAppointmentStatus);

        //display create appointment form
        $('#' + this.DivId).fadeIn('slow');
    },

    RenderOffice: function (vCurrentOfficePublicId, vStartDate, vAppointmentInfo) {

        //load office
        $('#OfficePublicId').find('option').remove();
        $('#OfficePublicId').unbind('change');

        for (var item in MettingCalendarObject.lstOffice) {
            $('#OfficePublicId').append($('<option/>', {
                value: MettingCalendarObject.lstOffice[item].OfficePublicId,
                text: MettingCalendarObject.lstOffice[item].OfficeName,
                selected: (MettingCalendarObject.lstOffice[item].OfficePublicId == vCurrentOfficePublicId)
            }));
        }

        $('#OfficePublicId').change(function () {
            UpsertAppointmentObject.RenderForm(vStartDate, $(this).val(), vAppointmentInfo);
        });
    },

    RenderTreatment: function (vCurrentOfficePublicId, vCurrentDuration, vCurrentTreatmentId, vCurrentStartDate, vCurrentStartTime) {

        //init duration spinner
        $('#Duration').spinner({
            min: 10,
            step: 5,
        });

        if (vCurrentDuration != null && vCurrentDuration > 0) {
            $('#Duration').val(vCurrentDuration);
        }

        //load treatment
        $('#TreatmentId').find('option').remove();
        $('#TreatmentId').unbind('change');
        $.each(MettingCalendarObject.lstOffice[vCurrentOfficePublicId].TreatmentList, function (index, value) {

            $('#TreatmentId').append($('<option/>', {
                value: value.TreatmentId,
                text: value.TreatmentName,
                selected: ((vCurrentTreatmentId > 0 && vCurrentTreatmentId == value.TreatmentId) || value.IsDefault),
                officepublicid: vCurrentOfficePublicId,
            }));

            //set input dependencies
            if ((vCurrentTreatmentId > 0 && vCurrentTreatmentId == value.TreatmentId) || value.IsDefault) {
                if (vCurrentDuration != null && vCurrentDuration > 0) { }
                else {
                    //set duration
                    $('#Duration').val(value.Duration);
                }
            }
        });

        $('#TreatmentId').change(function () {
            //get treatment id
            var ovTreatmentId = $(this).val();
            var ovOfficePublicId = $(this).find(':selected').attr('officepublicid');

            //set input dependencies
            $.each(MeetingObject.lstOffice[ovOfficePublicId].TreatmentList, function (index, value) {
                if (value.TreatmentId == ovTreatmentId) {
                    //set duration
                    $('#Duration').val(value.Duration);
                }
            });
        });

        //load start date
        $('#StartDate').datepicker();

        $('#StartDate').datepicker("setDate", vCurrentStartDate);

        //load start time
        $('#StartTime').ptTimeSelect({
            hoursLabel: 'Hora',
            minutesLabel: 'Minutos',
            setButtonLabel: 'Aceptar',
        });

        $('#StartTime').val(vCurrentStartTime);

    },

    RenderPatient: function (vAppointmentInfo) {

        //init patient hidden values
        $('#lstPatient').html('');
        $('#PatientAppointmentCreate').val('');
        $('#PatientAppointmentDelete').val('');

        if (vAppointmentInfo != null) {

            //render actual related patient
            $.each(vAppointmentInfo.CurrentPatientInfo, function (index, value) {

                UpsertAppointmentObject.AddPatientAppointment({
                    ProfileImage: value.ProfileImage,
                    Name: value.Name,
                    IdentificationNumber: value.IdentificationNumber,
                    Mobile: value.Mobile,
                    Email: value.Email,
                    PatientPublicId: value.PatientPublicId
                });

            });
        }

        //load patient autocomplete
        $('#getPatient').autocomplete(
        {
            source: function (request, response) {
                $.ajax({
                    url: '/api/PatientApi?SearchCriteria=' + request.term + '&PageNumber=0&RowCount=10',
                    dataType: 'json',
                    success: function (data) {
                        response(data);
                    }
                });
            },
            minLength: 0,
            focus: function (event, ui) {
                $('#getPatient').val(ui.item.Name);
                return false;
            },
            select: function (event, ui) {
                UpsertAppointmentObject.AddPatientAppointment({
                    ProfileImage: ui.item.ProfileImage,
                    Name: ui.item.Name,
                    IdentificationNumber: ui.item.IdentificationNumber,
                    Mobile: ui.item.Mobile,
                    Email: ui.item.Email,
                    PatientPublicId: ui.item.PatientPublicId
                });

                $('#getPatient').val('');
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {

            var RenderItem = $('#divPatientAcItem').html();
            RenderItem = RenderItem.replace(/{ProfileImage}/gi, item.ProfileImage);
            RenderItem = RenderItem.replace(/{Name}/gi, item.Name);
            RenderItem = RenderItem.replace(/{IdentificationNumber}/gi, item.IdentificationNumber);
            RenderItem = RenderItem.replace(/{Mobile}/gi, item.Mobile);

            return $("<li></li>")
                .data("ui-autocomplete-item", item)
                .append("<a><strong>" + RenderItem + "</strong></a>")
                .appendTo(ul);
        };
    },

    RenderActions: function (vAppointmentInfo, vCurrentAppointmentStatus) {

        //add style for specific appointment status
        $('#' + this.DivId).attr('class', '');
        $('#' + this.DivId).addClass('container2 AppointmentFormStatus_' + vCurrentAppointmentStatus);

        //get appointment header template
        var oHeaderTemplate = $('#AppointmentHeaderTemplate').html();

        //disable actions for status
        if (vCurrentAppointmentStatus == 1201) {
            //New
            $('#AppointmentUpsertActions .AppointmentActionsAccept').show();

            if (vAppointmentInfo != null) {
                $('#AppointmentUpsertActions .AppointmentActionsCancel').show();
                $('#AppointmentUpsertActions .AppointmentActionsConfirm').show();
                $('#AppointmentUpsertActions .AppointmentActionsNew').show();

                oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentStatus}/gi, '(Cita creada)');
            }
            else {
                $('#AppointmentUpsertActions .AppointmentActionsCancel').hide();
                $('#AppointmentUpsertActions .AppointmentActionsConfirm').hide();
                $('#AppointmentUpsertActions .AppointmentActionsNew').hide();

                oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentStatus}/gi, '(Cita nueva)');
            }
        }
        else if (vCurrentAppointmentStatus == 1202) {
            //Confirmed
            $('#AppointmentUpsertActions .AppointmentActionsCancel').show();
            $('#AppointmentUpsertActions .AppointmentActionsConfirm').hide();
            $('#AppointmentUpsertActions .AppointmentActionsNew').show();
            $('#AppointmentUpsertActions .AppointmentActionsAccept').show();

            oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentStatus}/gi, '(Cita confirmada)');
        }
        else if (vCurrentAppointmentStatus == 1203) {
            //Canceled
            $('#AppointmentUpsertActions .AppointmentActionsCancel').hide();
            $('#AppointmentUpsertActions .AppointmentActionsConfirm').hide();
            $('#AppointmentUpsertActions .AppointmentActionsNew').show();
            $('#AppointmentUpsertActions .AppointmentActionsAccept').hide();

            oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentStatus}/gi, '(Cita cancelada)');
        }
        else if (vCurrentAppointmentStatus == 1204) {
            //PendientAsistance
            $('#AppointmentUpsertActions .AppointmentActionsCancel').hide();
            $('#AppointmentUpsertActions .AppointmentActionsConfirm').show();
            $('#AppointmentUpsertActions .AppointmentActionsNew').show();
            $('#AppointmentUpsertActions .AppointmentActionsAccept').hide();

            oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentStatus}/gi, '(Cita pendiente por asistencia)');
        }
        else if (vCurrentAppointmentStatus == 1205) {
            //Attendance
            $('#AppointmentUpsertActions .AppointmentActionsCancel').hide();
            $('#AppointmentUpsertActions .AppointmentActionsConfirm').hide();
            $('#AppointmentUpsertActions .AppointmentActionsNew').show();
            $('#AppointmentUpsertActions .AppointmentActionsAccept').hide();

            oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentStatus}/gi, '(Cita confirmada asistencia)');
        }
        else if (vCurrentAppointmentStatus == 1206) {
            //NotAttendance
            $('#AppointmentUpsertActions .AppointmentActionsCancel').hide();
            $('#AppointmentUpsertActions .AppointmentActionsConfirm').hide();
            $('#AppointmentUpsertActions .AppointmentActionsNew').show();
            $('#AppointmentUpsertActions .AppointmentActionsAccept').hide();

            oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentStatus}/gi, '(Cita confirmada inasistencia)');
        }

        //set appointment header template
        if (vAppointmentInfo != null) {
            oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentPublicId}/gi, vAppointmentInfo.AppointmentPublicId);
        }
        else {
            oHeaderTemplate = oHeaderTemplate.replace(/{AppointmentPublicId}/gi, '');
        }

        $('#liAppointmentHeader').html(oHeaderTemplate);

        //set action events on click
        $('#AppointmentUpsertActions .AppointmentActionsCancel').unbind('click');
        $('#AppointmentUpsertActions .AppointmentActionsCancel').click(function () {
            $("#Dialog_CancelAppointment").dialog({
                buttons: {
                    "Si": function () {
                        $(this).dialog("close");
                        UpsertAppointmentObject.CancelAppointment(true);
                    },
                    "No": function () {
                        $(this).dialog("close");
                        UpsertAppointmentObject.CancelAppointment(false);
                    }
                }
            });
        });

        $('#AppointmentUpsertActions .AppointmentActionsConfirm').unbind('click');
        $('#AppointmentUpsertActions .AppointmentActionsConfirm').click(function () {

            //set radio button options
            $('#Dialog_ConfirmAppointment input:radio').prop('checked', false);

            $('#Dialog_ConfirmAppointment input:radio').unbind('change');
            $('#Dialog_ConfirmAppointment input:radio').change(function (eventData, handler) {
                if ($(this).val() == '1205') {
                    //attendance
                    $('#liRemindedFuture').fadeIn('slow');
                }
                else if ($(this).val() == '1206') {
                    //not attendance
                    $('#liRemindedFuture').fadeOut('slow');
                }
            });

            //set check box options
            $('#SendRemindedFuture').prop('checked', false);

            $('#SendRemindedFuture').unbind('change');
            $('#SendRemindedFuture').change(function (eventData, handler) {
                if ($(this).is(':checked')) {
                    //attendance
                    $('#divRemindedDate').fadeIn('slow');
                }
                else {
                    //not attendance
                    $('#divRemindedDate').fadeOut('slow');
                }
            });

            //start datepicker
            $('#RemindedDate').datepicker();

            //hide not started controls
            $('#liRemindedFuture').hide();
            $('#divRemindedDate').hide();

            //init dialog
            $("#Dialog_ConfirmAppointment").dialog({
                height: 200,
                width: 400,
                buttons: {
                    "Confirmar": function () {
                        $(this).dialog("close");
                        UpsertAppointmentObject.ConfirmAppointment();
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        });

        $('#AppointmentUpsertActions .AppointmentActionsNew').unbind('click');
        $('#AppointmentUpsertActions .AppointmentActionsNew').click(function () { UpsertAppointmentObject.DuplicateAppointment() });

        $('#AppointmentUpsertActions .AppointmentActionsAccept').unbind('click');
        if (vAppointmentInfo != null) {
            $('#AppointmentUpsertActions .AppointmentActionsAccept').click(function () {

                $("#Dialog_SaveAppointment").dialog({
                    buttons: {
                        "Si": function () {
                            $(this).dialog("close");
                            UpsertAppointmentObject.SaveAppointment(true);
                        },
                        "No": function () {
                            $(this).dialog("close");
                            UpsertAppointmentObject.SaveAppointment(false);
                        }
                    }
                });

            });
        }
        else {
            $('#AppointmentUpsertActions .AppointmentActionsAccept').click(function () { UpsertAppointmentObject.SaveAppointment(true) });
        }

        //Create patient        
        UpsertAppointmentObject.ValidateCreatePatient();
    },

    CreatePatient: function () {
        //create ajax form object
        $("#frmCreatePatient").submit(function (e) {
            var postData = $(this).serializeArray();
            var formURL = $(this).attr("action");
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    $("#Dialog_CreatePatient").dialog("close");
                    UpsertAppointmentObject.AddPatientAppointment(data);
                    $("#frmCreatePatient input").val('');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //show success message
                }
            });
            e.preventDefault(); //STOP default action
            e.unbind(); //unbind. to stop multiple form submit.
        });

        //submit ajax form        
        $("#frmCreatePatient").submit();
    },

    ValidateCreatePatient: function (vPatientModel) {

        var rules = {
            Name: {
                required: true,
            },
            IdentificationNumber: {
                required: true,
                minlength: 6,
            },
            Mobile: {
                required: true,
                maxlength: 10,
                minlength: 10
            }
        };

        var messages = {
            Name: {
                required: "Debe ingresar el nombre",
                return: false,
            },
            IdentificationNumber: {
                required: "Debe ingresar la identificación",
                minlength: "El número debe ser mayor a 6 dígitos",
                return: false,
            },
            Mobile: {
                required: "Debe ingresar un número móvil",
                maxlength: "El número debe contener 10 dígitos",
                minlength: "El número debe contener 10 dígitos",
                return: false,
            }
        };

        $('#frmCreatePatient').validate({
            errorClass: 'error help-inline',
            errorElement: 'span',
            validClass: 'success',
            rules: rules,
            messages: messages
        });

        //Dialog create patient
        $('#aCreatePatient').unbind('click');
        $('#aCreatePatient').click(function () {
            //init dialog
            $("#Dialog_CreatePatient").dialog({
                width: 800,
                show: "clip",
                hide: "blind",
                buttons: {
                    "Guardar": function () {
                        if ($('#frmCreatePatient').valid()) {
                            UpsertAppointmentObject.CreatePatient();
                        }
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                        $("#frmCreatePatient input").val('');
                        $("#frmCreatePatient span").empty();
                    }
                }
            });
        });
    },

    AddPatientAppointment: function (vPatientModel) {
        if ($('#PatientAppointmentCreate').val().indexOf(vPatientModel.PatientPublicId) == -1) {
            var ApPatHtml = $('#ulPatientAppointment').html();
            ApPatHtml = ApPatHtml.replace(/{ProfileImage}/gi, vPatientModel.ProfileImage);
            ApPatHtml = ApPatHtml.replace(/{Name}/gi, vPatientModel.Name);
            ApPatHtml = ApPatHtml.replace(/{IdentificationNumber}/gi, vPatientModel.IdentificationNumber);
            ApPatHtml = ApPatHtml.replace(/{Mobile}/gi, vPatientModel.Mobile);
            ApPatHtml = ApPatHtml.replace(/{Email}/gi, vPatientModel.Email);
            ApPatHtml = ApPatHtml.replace(/{PatientPublicId}/gi, vPatientModel.PatientPublicId);
            $('#lstPatient').append(ApPatHtml);
            $('#PatientAppointmentCreate').val($('#PatientAppointmentCreate').val() + ',' + vPatientModel.PatientPublicId);
            $('#PatientAppointmentDelete').val($('#PatientAppointmentDelete').val().replace(new RegExp(vPatientModel.PatientPublicId, 'gi'), ''));
        }
    },

    RemovePatientAppointment: function (vPatientPublicId) {
        $('#lstPatient').find('#' + vPatientPublicId).remove();
        $('#PatientAppointmentDelete').val($('#PatientAppointmentDelete').val() + ',' + vPatientPublicId);
        $('#PatientAppointmentCreate').val($('#PatientAppointmentCreate').val().replace(new RegExp(vPatientPublicId, 'gi'), ''));
    },

    SaveAppointment: function (vSendNotifications) {

        //set value for SendNotifications
        $('#SendNotifications').val(vSendNotifications);

        //create ajax form object
        $("#frmAppointment").submit(function (e) {
            var postData = $(this).serializeArray();
            var formURL = $(this).attr("action");
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    //refresh all controls
                    UpsertAppointmentObject.Refresh();

                    //show success message
                    var oMsj = $('#SaveResultTemplate').html();
                    oMsj = oMsj.replace(/{AppointmentPublicId}/gi, data);
                    oMsj = oMsj.replace(/{Status}/gi, 'correctamente');

                    $("#Dialog_SaveResult").html(oMsj);
                    $("#Dialog_SaveResult").dialog();

                    //hidde create appointment form
                    $('#' + UpsertAppointmentObject.DivId).hide();
                    $('#' + UpsertAppointmentObject.DivBlockId).hide();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //refresh all controls
                    UpsertAppointmentObject.Refresh();

                    //show success message
                    var oMsj = $('#SaveResultTemplate').html();
                    oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                    oMsj = oMsj.replace(/{Status}/gi, 'con errores');

                    $("#Dialog_SaveResult").html(oMsj);
                    $("#Dialog_SaveResult").dialog();

                    //hidde create appointment form
                    $('#' + UpsertAppointmentObject.DivId).hide();
                    $('#' + UpsertAppointmentObject.DivBlockId).hide();
                }
            });
            e.preventDefault(); //STOP default action
            e.unbind(); //unbind. to stop multiple form submit.
        });

        //hidde create appointment form
        $('#' + this.DivId).hide();

        //submit ajax form
        $("#frmAppointment").submit();
    },

    CancelAppointment: function (vSendNotifications) {

        $.ajax({
            url: '/api/AppointmentApi?CancelAppointment=true',
            type: "POST",
            data: {
                SendNotifications: vSendNotifications,
                AppointmentPublicId: $('#AppointmentPublicId').val(),
            },
            success: function (data, textStatus, jqXHR) {
                //refresh all controls
                UpsertAppointmentObject.Refresh();

                //show success message
                var oMsj = $('#SaveResultTemplate').html();
                oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                oMsj = oMsj.replace(/{Status}/gi, 'correctamente');

                $("#Dialog_SaveResult").html(oMsj);
                $("#Dialog_SaveResult").dialog();

                //hidde create appointment form
                $('#' + UpsertAppointmentObject.DivId).hide();
                $('#' + UpsertAppointmentObject.DivBlockId).hide();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //refresh all controls
                UpsertAppointmentObject.Refresh();

                //show success message
                var oMsj = $('#SaveResultTemplate').html();
                oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                oMsj = oMsj.replace(/{Status}/gi, 'con errores');

                $("#Dialog_SaveResult").html(oMsj);
                $("#Dialog_SaveResult").dialog();

                //hidde create appointment form
                $('#' + UpsertAppointmentObject.DivId).hide();
                $('#' + UpsertAppointmentObject.DivBlockId).hide();
            },
        });
    },

    DuplicateAppointment: function () {

        //hidde create appointment form
        $('#' + this.DivId).hide();
        $('#' + this.DivBlockId).hide();

        //get current values

        //set appointment id
        $('#AppointmentPublicId').val('');

        //current appointment status
        var oCurrentAppointmentStatus = '1201';

        //add style for specific appointment status
        $('#' + this.DivId).addClass('container2 AppointmentFormStatus_' + oCurrentAppointmentStatus);

        //render appointment actions
        this.RenderActions(null, oCurrentAppointmentStatus);

        //display create appointment form
        $('#' + this.DivId).fadeIn('slow');
    },

    ConfirmAppointment: function () {
        //set current appointment public id
        $('#R_AppointmentPublicId').val($('#AppointmentPublicId').val());

        //create ajax form object
        $("#frmConfirmAppointment").submit(function (e) {

            var postData = $(this).serializeArray();
            var formURL = $(this).attr("action");

            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    //refresh all controls
                    UpsertAppointmentObject.Refresh();

                    //show success message
                    var oMsj = $('#SaveResultTemplate').html();
                    oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                    oMsj = oMsj.replace(/{Status}/gi, 'correctamente');

                    $("#Dialog_SaveResult").html(oMsj);
                    $("#Dialog_SaveResult").dialog();

                    //hidde create appointment form
                    $('#' + UpsertAppointmentObject.DivId).hide();
                    $('#' + UpsertAppointmentObject.DivBlockId).hide();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //refresh all controls
                    UpsertAppointmentObject.Refresh();

                    //show success message
                    var oMsj = $('#SaveResultTemplate').html();
                    oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                    oMsj = oMsj.replace(/{Status}/gi, 'con errores');

                    $("#Dialog_SaveResult").html(oMsj);
                    $("#Dialog_SaveResult").dialog();

                    //hidde create appointment form
                    $('#' + UpsertAppointmentObject.DivId).hide();
                    $('#' + UpsertAppointmentObject.DivBlockId).hide();
                }
            });
            e.preventDefault(); //STOP default action
            e.unbind(); //unbind. to stop multiple form submit.
        });

        //submit ajax form
        $("#frmConfirmAppointment").submit();
    },

    /*render block appointment form*/
    RenderBlockForm: function (vAppointmentInfo) {

        //hidde create appointment form
        $('#' + this.DivId).hide();
        $('#' + this.DivBlockId).hide();

        //load appointment id
        if (vAppointmentInfo != null) {
            $('#BlockAppointmentPublicId').val(vAppointmentInfo.id);
        }

        //load office
        $('#BlockOfficePublicId').val(MettingCalendarObject.CurrentPublicOfficeId);

        //load block date
        $('#BlockDate').datepicker();

        if (vAppointmentInfo != null) {
            $('#BlockDate').val(vAppointmentInfo.StartDate);
        }
        else if (CalendarObject.StartDate != null) {
            var vDateAux = '';
            if (CalendarObject.StartDate.getDate() < 10) {
                vDateAux = vDateAux + '0';
            }
            vDateAux = vDateAux + CalendarObject.StartDate.getDate() + '/';

            if (CalendarObject.StartDate.getMonth() < 9) {
                vDateAux = vDateAux + '0';
            }
            vDateAux = vDateAux + ((new Number(CalendarObject.StartDate.getMonth())) + 1) + '/' + CalendarObject.StartDate.getFullYear();

            $('#BlockDate').val(vDateAux);
        }
        else {
            $('#BlockDate').val('');
        }

        //load block start time
        $('#BlockStartTime').ptTimeSelect({
            hoursLabel: 'Hora',
            minutesLabel: 'Minutos',
            setButtonLabel: 'Aceptar',
        });

        if (vAppointmentInfo != null) {
            $('#BlockStartTime').val(vAppointmentInfo.StartTime);
        }
        else {
            $('#BlockStartTime').val('');
        }

        //load block end time
        $('#BlockEndTime').ptTimeSelect({
            hoursLabel: 'Hora',
            minutesLabel: 'Minutos',
            setButtonLabel: 'Aceptar',
        });

        if (vAppointmentInfo != null) {
            $('#BlockEndTime').val(vAppointmentInfo.EndTime);
        }
        else {
            $('#BlockEndTime').val('');
        }

        //load actions
        if (vAppointmentInfo != null) {
            $('#BlockAppointmentContainer .AppointmentActionsCancel').show();
        }
        else {
            $('#BlockAppointmentContainer .AppointmentActionsCancel').hide();
        }

        //display create appointment form
        $('#' + this.DivBlockId).fadeIn('slow');
    },

    SaveBlockAppointment: function () {

        //create ajax form object
        $("#frmBlockAppointment").submit(function (e) {
            var postData = $(this).serializeArray();
            var formURL = $(this).attr("action");
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    //refresh all controls
                    UpsertAppointmentObject.Refresh();

                    //show success message
                    var oMsj = $('#SaveResultTemplate').html();
                    oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                    oMsj = oMsj.replace(/{Status}/gi, 'correctamente');

                    $("#Dialog_SaveResult").html(oMsj);
                    $("#Dialog_SaveResult").dialog();

                    //hidde create appointment form
                    $('#' + UpsertAppointmentObject.DivId).hide();
                    $('#' + UpsertAppointmentObject.DivBlockId).hide();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //refresh all controls
                    UpsertAppointmentObject.Refresh();

                    //show success message
                    var oMsj = $('#SaveResultTemplate').html();
                    oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                    oMsj = oMsj.replace(/{Status}/gi, 'con errores');

                    $("#Dialog_SaveResult").html(oMsj);
                    $("#Dialog_SaveResult").dialog();

                    //hidde create appointment form
                    $('#' + UpsertAppointmentObject.DivId).hide();
                    $('#' + UpsertAppointmentObject.DivBlockId).hide();
                }
            });
            e.preventDefault(); //STOP default action
            e.unbind(); //unbind. to stop multiple form submit.
        });

        //hidde create appointment form
        $('#' + this.DivBlockId).hide();

        //submit ajax form
        $("#frmBlockAppointment").submit();
    },

    SaveUnBlockAppointment: function () {
        
        //ajax for unblock appointment
        $.ajax(
        {
            url: '/api/AppointmentApi?BlockAppointmentPublicId=' + $('#BlockAppointmentPublicId').val(),
            type: "POST",
            success: function (data, textStatus, jqXHR) {
                //refresh all controls
                UpsertAppointmentObject.Refresh();

                //show success message
                var oMsj = $('#SaveResultTemplate').html();
                oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                oMsj = oMsj.replace(/{Status}/gi, 'correctamente');

                $("#Dialog_SaveResult").html(oMsj);
                $("#Dialog_SaveResult").dialog();

                //hidde create appointment form
                $('#' + UpsertAppointmentObject.DivId).hide();
                $('#' + UpsertAppointmentObject.DivBlockId).hide();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //refresh all controls
                UpsertAppointmentObject.Refresh();

                //show success message
                var oMsj = $('#SaveResultTemplate').html();
                oMsj = oMsj.replace(/{AppointmentPublicId}/gi, '');
                oMsj = oMsj.replace(/{Status}/gi, 'con errores');

                $("#Dialog_SaveResult").html(oMsj);
                $("#Dialog_SaveResult").dialog();

                //hidde create appointment form
                $('#' + UpsertAppointmentObject.DivId).hide();
                $('#' + UpsertAppointmentObject.DivBlockId).hide();
            }
        });

        //hidde create appointment form
        $('#' + this.DivBlockId).hide();
    },

    Refresh: function () {
        //refresh calendar 
        CalendarObject.Refresh();

        //refresh meeting
        MettingCalendarObject.Refresh();
    }
};

/*render appointment detail*/
var AppointmentDetailObject = {

    StartDateTime: new Date(),

    CurrentAppointment: null,

    /*office info*/
    lstOffice: new Array(),

    /*init meeting variables*/
    Init: function (vInitObject) {

        this.StartDateTime = vInitObject.StartDateTime;

        this.CurrentAppointment = vInitObject.CurrentAppointment;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            //set key value pair for an office
            AppointmentDetailObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    /*init appointment info form*/
    RenderAsync: function () {

        //get current values

        //current appointment status
        var oCurrentAppointmentStatus = '1201';
        if (this.CurrentAppointment != null) {
            oCurrentAppointmentStatus = this.CurrentAppointment.AppointmentStatus;
        }

        //current office
        var oCurrentOfficePublicId = '';
        if (this.CurrentAppointment != null) {
            oCurrentOfficePublicId = this.CurrentAppointment.OfficePublicId;
        }
        else {
            for (ovOfficePublicId in this.lstOffice) {
                oCurrentOfficePublicId = ovOfficePublicId;
                break;
            }
        }

        //current treatment
        var oCurrentTreatmentId = 0;

        if (this.CurrentAppointment != null) {
            oCurrentTreatmentId = this.CurrentAppointment.TreatmentId;
        }

        //current start date and duration
        var oCurrentStartDate = new Date();
        var oCurrentStartTime = '';
        var oCurrentDuration = 0;

        if (this.CurrentAppointment != null) {
            oCurrentStartDate = this.CurrentAppointment.StartDate;
            oCurrentStartTime = this.CurrentAppointment.StartTime;
            oCurrentDuration = this.CurrentAppointment.Duration;
        }
        else if (this.StartDateTime != null) {

            //get start date
            oCurrentStartDate = this.StartDateTime;
        }

        //render office
        this.RenderOffice(oCurrentOfficePublicId, oCurrentStartDate, oCurrentStartTime)

        //render treatment duration startdate and starttime
        this.RenderTreatment(oCurrentOfficePublicId, oCurrentDuration, oCurrentTreatmentId, oCurrentStartDate, oCurrentStartTime);

        //render patient appointment
        this.RenderPatient();

        //render appointment actions
        this.RenderActions(oCurrentAppointmentStatus);
    },

    RenderOffice: function (vCurrentOfficePublicId, vCurrentStartDate, vCurrentStartTime) {

        //load office
        $('#OfficePublicId').find('option').remove();
        $('#OfficePublicId').unbind('change');

        for (var item in AppointmentDetailObject.lstOffice) {
            $('#OfficePublicId').append($('<option/>', {
                value: AppointmentDetailObject.lstOffice[item].OfficePublicId,
                text: AppointmentDetailObject.lstOffice[item].OfficeName,
                selected: (AppointmentDetailObject.lstOffice[item].OfficePublicId == vCurrentOfficePublicId)
            }));
        }

        $('#OfficePublicId').change(function () {
            AppointmentDetailObject.RenderTreatment($(this).val(), 0, 0, vCurrentStartDate, vCurrentStartTime);
        });
    },

    RenderTreatment: function (vCurrentOfficePublicId, vCurrentDuration, vCurrentTreatmentId, vCurrentStartDate, vCurrentStartTime) {

        //init duration spinner
        $('#Duration').spinner({
            min: 10,
            step: 5,
        });

        if (vCurrentDuration != null && vCurrentDuration > 0) {
            $('#Duration').val(vCurrentDuration);
        }

        //load treatment
        $('#TreatmentId').find('option').remove();
        $('#TreatmentId').unbind('change');
        $.each(AppointmentDetailObject.lstOffice[vCurrentOfficePublicId].TreatmentList, function (index, value) {

            $('#TreatmentId').append($('<option/>', {
                value: value.TreatmentId,
                text: value.TreatmentName,
                selected: ((vCurrentTreatmentId > 0 && vCurrentTreatmentId == value.TreatmentId) || value.IsDefault),
                officepublicid: vCurrentOfficePublicId,
            }));

            //set input dependencies
            if ((vCurrentTreatmentId > 0 && vCurrentTreatmentId == value.TreatmentId) || value.IsDefault) {
                if (vCurrentDuration != null && vCurrentDuration > 0) { }
                else {
                    //set duration
                    $('#Duration').val(value.Duration);
                    if (this.CurrentAppointment == null) {
                        //set after care 
                        $('#AfterCare').val(value.AfterCare);
                        //set before care
                        $('#BeforeCare').val(value.BeforeCare);
                    }
                }
            }
        });

        $('#TreatmentId').change(function () {
            //get treatment id
            var ovTreatmentId = $(this).val();
            var ovOfficePublicId = $(this).find(':selected').attr('officepublicid');

            //set input dependencies
            $.each(AppointmentDetailObject.lstOffice[ovOfficePublicId].TreatmentList, function (index, value) {
                if (value.TreatmentId == ovTreatmentId) {
                    //set duration
                    $('#Duration').val(value.Duration);
                    //set after care 
                    $('#AfterCare').val(value.AfterCare);
                    //set before care
                    $('#BeforeCare').val(value.BeforeCare);

                }
            });
        });

        //load start date
        $('#StartDate').datepicker();

        $('#StartDate').datepicker("setDate", vCurrentStartDate);

        //load start time
        $('#StartTime').ptTimeSelect({
            hoursLabel: 'Hora',
            minutesLabel: 'Minutos',
            setButtonLabel: 'Aceptar',
        });

        $('#StartTime').val(vCurrentStartTime);
    },

    RenderPatient: function () {

        //init patient hidden values
        $('#lstPatient').html('');
        $('#PatientAppointmentCreate').val('');
        $('#PatientAppointmentDelete').val('');

        if (this.CurrentAppointment != null) {

            //render actual related patient
            $.each(this.CurrentAppointment.CurrentPatientInfo, function (index, value) {

                AppointmentDetailObject.AddPatientAppointment({
                    ProfileImage: value.ProfileImage,
                    Name: value.Name,
                    IdentificationNumber: value.IdentificationNumber,
                    Mobile: value.Mobile,
                    Email: value.Email,
                    PatientPublicId: value.PatientPublicId,
                    Notes: value.Notes,
                });

            });
        }

        //load patient autocomplete
        $('#getPatient').autocomplete(
        {
            source: function (request, response) {
                $.ajax({
                    url: '/api/PatientApi?SearchCriteria=' + request.term + '&PageNumber=0&RowCount=10',
                    dataType: 'json',
                    success: function (data) {
                        response(data);
                    }
                });
            },
            minLength: 0,
            focus: function (event, ui) {
                $('#getPatient').val(ui.item.Name);
                return false;
            },
            select: function (event, ui) {
                AppointmentDetailObject.AddPatientAppointment({
                    ProfileImage: ui.item.ProfileImage,
                    Name: ui.item.Name,
                    IdentificationNumber: ui.item.IdentificationNumber,
                    Mobile: ui.item.Mobile,
                    Email: ui.item.Email,
                    PatientPublicId: ui.item.PatientPublicId,
                    Notes: value.Notes,
                });

                $('#getPatient').val('');
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {

            var RenderItem = $('#divPatientAcItem').html();
            RenderItem = RenderItem.replace(/{ProfileImage}/gi, item.ProfileImage);
            RenderItem = RenderItem.replace(/{Name}/gi, item.Name);
            RenderItem = RenderItem.replace(/{IdentificationNumber}/gi, item.IdentificationNumber);
            RenderItem = RenderItem.replace(/{Mobile}/gi, item.Mobile);

            return $("<li></li>")
                .data("ui-autocomplete-item", item)
                .append("<a><strong>" + RenderItem + "</strong></a>")
                .appendTo(ul);
        };
    },

    RenderActions: function () {

        //set action events on click
        if ($('#AppointmentUpsertActions .AppointmentActionsCancel').length > 0) {

            $('#AppointmentUpsertActions .AppointmentActionsCancel').unbind('click');
            $('#AppointmentUpsertActions .AppointmentActionsCancel').click(function () {
                $("#Dialog_CancelAppointment").dialog({
                    buttons: {
                        "Si": function () {
                            $(this).dialog("close");
                            AppointmentDetailObject.CancelAppointment(true);
                        },
                        "No": function () {
                            $(this).dialog("close");
                            AppointmentDetailObject.CancelAppointment(false);
                        }
                    }
                });
            });
        }

        if ($('#AppointmentUpsertActions .AppointmentActionsConfirm').length > 0) {

            $('#AppointmentUpsertActions .AppointmentActionsConfirm').unbind('click');
            $('#AppointmentUpsertActions .AppointmentActionsConfirm').click(function () {

                //set radio button options
                $('#Dialog_ConfirmAppointment input:radio').prop('checked', false);

                $('#Dialog_ConfirmAppointment input:radio').unbind('change');
                $('#Dialog_ConfirmAppointment input:radio').change(function (eventData, handler) {
                    if ($(this).val() == '1205') {
                        //attendance
                        $('#liRemindedFuture').fadeIn('slow');
                    }
                    else if ($(this).val() == '1206') {
                        //not attendance
                        $('#liRemindedFuture').fadeOut('slow');
                    }
                });

                //set check box options
                $('#SendRemindedFuture').prop('checked', false);

                $('#SendRemindedFuture').unbind('change');
                $('#SendRemindedFuture').change(function (eventData, handler) {
                    if ($(this).is(':checked')) {
                        //attendance
                        $('#divRemindedDate').fadeIn('slow');
                    }
                    else {
                        //not attendance
                        $('#divRemindedDate').fadeOut('slow');
                    }
                });

                //start datepicker
                $('#RemindedDate').datepicker();

                //hide not started controls
                $('#liRemindedFuture').hide();
                $('#divRemindedDate').hide();

                //init dialog
                $("#Dialog_ConfirmAppointment").dialog({
                    height: 200,
                    width: 400,
                    buttons: {
                        "Confirmar": function () {
                            $(this).dialog("close");
                            AppointmentDetailObject.ConfirmAppointment();
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });
        }

        if ($('#AppointmentUpsertActions .AppointmentActionsNew').length > 0) {

            $('#AppointmentUpsertActions .AppointmentActionsNew').unbind('click');
            $('#AppointmentUpsertActions .AppointmentActionsNew').click(function () { AppointmentDetailObject.DuplicateAppointment() });
        }

        if ($('#AppointmentUpsertActions .AppointmentActionsAccept').length > 0) {

            $('#AppointmentUpsertActions .AppointmentActionsAccept').unbind('click');
            if (this.CurrentAppointment != null) {
                $('#AppointmentUpsertActions .AppointmentActionsAccept').click(function () {

                    $("#Dialog_SaveAppointment").dialog({
                        buttons: {
                            "Si": function () {
                                $(this).dialog("close");
                                AppointmentDetailObject.SaveAppointment(true);
                            },
                            "No": function () {
                                $(this).dialog("close");
                                AppointmentDetailObject.SaveAppointment(false);
                            }
                        }
                    });

                });
            }
            else {
                $('#AppointmentUpsertActions .AppointmentActionsAccept').click(function () { AppointmentDetailObject.SaveAppointment(true) });
            }
        }
    },

    AddPatientAppointment: function (vPatientModel) {
        if ($('#PatientAppointmentCreate').val().indexOf(vPatientModel.PatientPublicId) == -1) {

            var ApPatHtml = $('#ulPatientAppointment').html();
            ApPatHtml = ApPatHtml.replace(/{ProfileImage}/gi, vPatientModel.ProfileImage);
            ApPatHtml = ApPatHtml.replace(/{Name}/gi, vPatientModel.Name);
            ApPatHtml = ApPatHtml.replace(/{IdentificationNumber}/gi, vPatientModel.IdentificationNumber);
            ApPatHtml = ApPatHtml.replace(/{Mobile}/gi, vPatientModel.Mobile);
            ApPatHtml = ApPatHtml.replace(/{Email}/gi, vPatientModel.Email);
            ApPatHtml = ApPatHtml.replace(/{PatientPublicId}/gi, vPatientModel.PatientPublicId);

            $('#lstPatient').append(ApPatHtml);
            $('#PatientAppointmentCreate').val($('#PatientAppointmentCreate').val() + ',' + vPatientModel.PatientPublicId);
            $('#PatientAppointmentDelete').val($('#PatientAppointmentDelete').val().replace(new RegExp(vPatientModel.PatientPublicId, 'gi'), ''));

            this.RenderDoctorNotes('ulDoctorNotes_' + vPatientModel.PatientPublicId, vPatientModel.Notes, vPatientModel.PatientPublicId);

            this.RenderPatientAppointment('divPatientAppointment_' + vPatientModel.PatientPublicId, vPatientModel.PatientPublicId);
        }
    },

    RemovePatientAppointment: function (vPatientPublicId) {
        $('#lstPatient').find('#' + vPatientPublicId).remove();
        $('#PatientAppointmentDelete').val($('#PatientAppointmentDelete').val() + ',' + vPatientPublicId);
        $('#PatientAppointmentCreate').val($('#PatientAppointmentCreate').val().replace(new RegExp(vPatientPublicId, 'gi'), ''));
    },

    RenderDoctorNotes: function (vUlId, vNotes, vPatientPublicId) {

        $('#' + vUlId).html('');

        var ApPatNoteHtml = $('#ulDoctorNotes').html();
        if (vNotes != null) {

            $.each(vNotes, function (index, value) {
                var DateAux = new Date();
                if (value.CreateDate.indexOf('Date') >= 0) {
                    DateAux = new Date(eval(value.CreateDate.replace(/\//gi, '')));
                }
                else {
                    DateAux = new Date(value.CreateDate.replace(/\//gi, ''));
                }

                var TmpApPatNoteBody = ApPatNoteHtml.replace(/{NoteDate}/gi, DateAux.getFullYear() + '/' + DateAux.getMonth() + '/' + DateAux.getDate());
                TmpApPatNoteBody = TmpApPatNoteBody.replace(/{NoteText}/gi, value.LargeValue);
                TmpApPatNoteBody = TmpApPatNoteBody.replace(/{PatientInfoId}/gi, value.PatientInfoId);
                TmpApPatNoteBody = TmpApPatNoteBody.replace(/{PatientPublicId}/gi, vPatientPublicId);

                $('#' + vUlId).append(TmpApPatNoteBody);
            });
        }
    },

    RenderPatientAppointment: function (vDivId, vPatientPublicId) {
        //init office metting calendar
        $('#' + vDivId).kendoGrid({
            pageable: false,
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: '/api/PatientApi?Quantity=5&PatientPublicId=' + vPatientPublicId,
                            dataType: "json",
                            type: "POST",
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
                field: 'id',
                title: ' ',
                template: $('#divPatientAppointmentTemplate').html()
            }]
        });
    },

    SaveAppointment: function (vSendNotifications) {
        //set value for SendNotifications
        $('#SendNotifications').val(vSendNotifications);
        //set action name
        $('#UpsertAction').val('SaveAppointment');

        //submit form
        $("#frmAppointment").submit();
    },

    CancelAppointment: function (vSendNotifications) {
        //set value for SendNotifications
        $('#SendNotifications').val(vSendNotifications);
        //set action name
        $('#UpsertAction').val('CancelAppointment');

        //submit form
        $("#frmAppointment").submit();
    },

    DuplicateAppointment: function () {
        window.localtion = '/Appointment/Detail?ReturnUrl=' + $('#ReturnUrl').val() + '&AppointmentPublicIdToDuplicate=' + this.CurrentAppointment.id;
    },

    ConfirmAppointment: function () {
        //submit form
        $("#frmConfirmAppointment").submit();
    },

    AddPatientNote: function (vPatientPublicId) {
        $.ajax({
            type: "POST",
            url: '/api/PatientApi?PatientPublicId=' + vPatientPublicId,
            data: {
                DoctorNote: $('#NewDoctorNote_' + vPatientPublicId).val(),
            },
        }).done(function (data) {
            if (data != null) {
                AppointmentDetailObject.RenderDoctorNotes('ulDoctorNotes_' + data.PatientPublicId, data.Notes, data.PatientPublicId);
            }
        });
    },

    RemovePatientNote: function (vPatientPublicId, vPatientInfoId) {

        $.ajax({
            type: "POST",
            url: '/api/PatientApi?PatientPublicId=' + vPatientPublicId + '&PatientInfoId=' + vPatientInfoId,
        }).done(function (data) {
            if (data != null) {
                AppointmentDetailObject.RenderDoctorNotes('ulDoctorNotes_' + data.PatientPublicId, data.Notes, data.PatientPublicId);
            }
        });
    },
};

