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

        //hidde create appointment form
        $('#AppointmentUpsert').hide();


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

        $('#selTreatment').change(function () {
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
        $('#PatientAppointment').val('');
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
	        RenderItem = RenderItem.replace('{ProfileImage}', item.ProfileImage);
	        RenderItem = RenderItem.replace('{Name}', item.Name);
	        RenderItem = RenderItem.replace('{IdentificationNumber}', item.IdentificationNumber);
	        RenderItem = RenderItem.replace('{Mobile}', item.Mobile);

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
        var ApPatHtml = $('#ulPatientAppointment').html();
        ApPatHtml = ApPatHtml.replace('{ProfileImage}', vPatientModel.ProfileImage);
        ApPatHtml = ApPatHtml.replace('{Name}', vPatientModel.Name);
        ApPatHtml = ApPatHtml.replace('{IdentificationNumber}', vPatientModel.IdentificationNumber);
        ApPatHtml = ApPatHtml.replace('{Mobile}', vPatientModel.Mobile);
        ApPatHtml = ApPatHtml.replace('{Email}', vPatientModel.Email);
        ApPatHtml = ApPatHtml.replace('{PatientPublicId}', vPatientModel.PatientPublicId);

        $('#lstPatient').append(ApPatHtml);
        $('#PatientAppointment').val($('#PatientAppointment').val() + ',' + vPatientModel.PatientPublicId);
    },
    SaveAppointment: function () {

        $("#frmAppointment").submit(function (e) {
            var postData = $(this).serializeArray();
            var formURL = $(this).attr("action");
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    debugger;
                    //data: return data from server
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    debugger;
                    //if fails      
                }
            });
            e.preventDefault(); //STOP default action
            e.unbind(); //unbind. to stop multiple form submit.
        });

        $("#frmAppointment").submit();

        //$.ajax({
        //    url: '/api/AppointmentApi',
        //    dataType: 'json',
        //    success: function (data) {
        //        response(data);
        //    }
        //});

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
