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
var MettingCalendarObject = {

    /*calendar info*/
    DivId: '',
    CountryId: '',
    ProfilePublicId: '',
    CurrentDate: new Date(),

    /*init meeting calendar variables*/
    InitMettingCalendar: function (vInitObject) {
        this.DivId = vInitObject.DivId;
        this.CountryId = vInitObject.CountryId;
        this.ProfilePublicId = vInitObject.ProfilePublicId;
        this.CurrentDate = vInitObject.CurrentDate;
    },

    InitMettingCalendarDoubleByDayAsync: function (vSecondDate) {
        //make ajax for special days
        $.ajax({
            type: "POST",
            url: '/api/Calendar?CountryId=' + this.CountryId + '&ProfilePublicId=' + this.ProfilePublicId + '&Date=' + this.CurrentDate.getFullYear() + '-' + ((new Number(this.CurrentDate.getMonth())) + 1) + '-1'
        }).done(function (data) {
            //left date picker
            MettingCalendarObject.InitMettingCalendarDoubleByDay(vSecondDate, data);
        }).fail(function () {
            alert("se ha generado un error en el render del calendario");
        });
    },

    InitMettingCalendarDoubleByDay: function (vSecondDate, lstSpecialDay) {
        //load calendar by day

        //left
        $('#' + this.DivId + '-Left').datepicker({
            dateFormat: 'yy-mm-dd',
            locale: $.datepicker.regional['es'],
            defaultDate: this.CurrentDate,
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
                $('#' + this.DivId + '-Right .ui-state-active').removeClass('ui-state-active ui-state-hover');

                alert(date);
            },
        });

        //right
        $('#' + this.DivId + '-Right').datepicker({
            dateFormat: 'yy-mm-dd',
            locale: $.datepicker.regional['es'],
            defaultDate: vSecondDate,
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
                $('#' + this.DivId + '-Left .ui-state-active').removeClass('ui-state-active ui-state-hover');

                alert(date);
            },
        });

        //delete selected style on right calendar
        $('#' + this.DivId + '-Right .ui-state-active').removeClass('ui-state-active ui-state-hover');
    },
};

/*render day calendar*/
var MeetingObject = {

    /*meeting info*/
    StartDateTime: new Date(),
    EndDateTime: new Date(),

    /*office info*/
    lstOffice: new Array(),

    /*init meeting variables*/
    InitMeeting: function (vInitObject) {

        //init meeting info
        this.StartDateTime = vInitObject.StartDateTime;
        this.EndDateTime = vInitObject.EndDateTime;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            MeetingObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    /*init meeting calendar by day*/
    InitByDay: function (DivId) {

        var TotalCalendars = 0;

        for (var item in this.lstOffice) {
            //create div to put a calendar
            this.lstOffice[item].OfficeDivId = 'divMetting_' + this.lstOffice[item].OfficePublicId;
            $('#' + DivId).append($('#divMetting').html().replace(/divOfficePublicId/gi, this.lstOffice[item].OfficeDivId));

            //init calendar
            this.InitOfficeByDay(this.lstOffice[item].OfficePublicId);

            //add calendars count
            TotalCalendars = TotalCalendars + 1;
        }

        //recalc div width
        $('#' + DivId).width(($('#divOfficePublicId').width() * TotalCalendars) + 1);
    },

    InitOfficeByDay: function (vOfficePublicId) {

        //init one office calendar by day
        $('#' + this.lstOffice[vOfficePublicId].OfficeDivId).fullCalendar({
            defaultDate: this.CurrentDateTime,
            defaultView: 'agendaDay',
            allDaySlot: false,
            allDayText: '',
            titleFormat: '\'' + this.lstOffice[vOfficePublicId].OfficeName + '\'',
            weekNumbers: false,
            editable: true,
            header: {
                left: '',
                center: 'title',
                right: '',
            },
            columnFormat: {
                month: 'dddd',
                week: 'dddd',
                day: 'dddd'
            },
            dayClick: function (date, jsEvent, view) {
                MeetingObject.RenderCreateAppointment(date, vOfficePublicId);
            },
            eventClick: function (event, jsEvent, view) {
                alert(event);
            },
            eventRender: function (event, element) {
                element.find('.fc-event-title').html(element.find('.fc-event-title').text());
            },
            events: {
                url: '/api/AppointmentApi?OfficePublicId=' + vOfficePublicId + '&StartDateTime=' + serverDateTimeToString(this.StartDateTime) + '&EndDateTime=' + serverDateTimeToString(this.EndDateTime),
            },
        });
    },

    RenderCreateAppointment: function (vDate, vOfficePublicId) {

        //hidde create appointment form
        $('#AppointmentUpsert').hide();


        //load office
        $('#OfficePublicId').find('option').remove();
        $('#OfficePublicId').unbind('change');
        for (var item in this.lstOffice) {

            $('#OfficePublicId').append($('<option/>', {
                value: this.lstOffice[item].OfficePublicId,
                text: this.lstOffice[item].OfficeName,
                selected: (this.lstOffice[item].OfficePublicId == vOfficePublicId)
            }));
        }

        $('#OfficePublicId').change(function () {
            MeetingObject.RenderCreateAppointment(vDate, $(this).val());
        });

        //load treatment
        $('#TreatmentId').find('option').remove();
        $('#TreatmentId').unbind('change');
        $.each(this.lstOffice[vOfficePublicId].TreatmentList, function (index, value) {
            $('#TreatmentId').append($('<option/>', {
                value: value.TreatmentId,
                text: value.TreatmentName,
                selected: value.Default,
                officepublicid: vOfficePublicId,
            }));

            if (value.Default == true) {
                //set duration
                $('#Duration').val(value.Duration);
                //set aftercare
                $('#AfterCare').val(value.AfterCare);
                //set beforecare
                $('#BeforeCare').val(value.BeforeCare);
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
                    //set aftercare
                    $('#AfterCare').val(value.AfterCare);
                    //set beforecare
                    $('#BeforeCare').val(value.BeforeCare);
                }
            });
        });

        $('#CatId_TreatmentId').val('0');

        //init duration spinner
        $('#Duration').spinner({
            min: 10,
            step: 5,
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

        //load patient autocomplete
        $('#lstPatient').html('');
        $('#PatientAppointmentCreate').val('');
        $('#PatientAppointmentDelete').val('');
        $('#getPatient').autocomplete(
	    {
	        //source: acData,
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
	            MeetingObject.AddPatientAppointment({
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

        //set appointment id
        $('#AppointmentPublicId').val('');

        //display create appointment form
        $('#AppointmentUpsert').fadeIn('slow');
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

    SaveAppointment: function () {
        debugger;
        $("#frmAppointment").submit(function (e) {
            debugger;
            var postData = $(this).serializeArray();
            var formURL = $(this).attr("action");
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    debugger;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    debugger;
                    alert('Se ha generado un error creando la cita.');
                }
            });
            e.preventDefault(); //STOP default action
            e.unbind(); //unbind. to stop multiple form submit.
        });

        $("#frmAppointment").submit();
    }
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
