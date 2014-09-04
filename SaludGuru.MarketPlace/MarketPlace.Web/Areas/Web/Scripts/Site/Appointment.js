function ValidateCreatePatient() {

    var rules = {
        Name: {
            required: true,
        },
        LastName: {
            required: true,
        },
        Identification: {
            required: true,
            minlength: 5,
        },
        Birthday: {
            required: true
        }
    };

    $('#CreatePatientForm').validate({
        errorClass: 'error help-inline',
        validClass: 'success',
        errorElement: 'span',
        rules: rules,
    });

    if ($('#CreatePatientForm').valid()) {
        CreatePatient();
    }
    else
    {
        $('#NewPatientId').show();
    }
}


function CreatePatient() {
    //create ajax form object
    $("#CreatePatientForm").submit(function (e) {
        var postData = $(this).serializeArray();
        var formURL = $(this).attr("action");
        $.ajax(
        {
            url: formURL,
            type: "POST",
            data: postData,
            success: function (data, textStatus, jqXHR) {
                ;
                AddPatientToList(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //if fails      
            }
        });
        e.preventDefault(); //STOP default action
        e.unbind(); //unbind. to stop multiple form submit.
    });

    CloseNewPatient();
    $("#CreatePatientForm").submit(); //Submit  the FORM
    $("#CreatePatientForm input#Name, input#LastName, input#Identification, input#Birthday").val('');
}

function CloseNewPatient() {
    $("#NewPatientId").hide(800);    
}

function AddPatientToList(vPatientModel) {
    var ApPatHtml = $('#ulPatientTemplate').html();
    ApPatHtml = ApPatHtml.replace(/{PatientPublicId}/gi, vPatientModel.PatientPublicId);
    ApPatHtml = ApPatHtml.replace(/{PatientName}/gi, vPatientModel.Name + " " + vPatientModel.LastName);
    $('#ulPatientList').append(ApPatHtml);
    $('#NewPatient').hide();
    $('#Name').val("");
    $('#LastName').val("");
    $('#Birthday').val("");
    $('#Identification').val("");
    $('#GenderFemale').prop("checked", false);
    $('#GenderMale').prop("checked", false);
    $('#SelectedItem').prop("checked", false);
    $('#SelectedPatientItem').prop("checked", false);
}

var InitFunctionsNewPatient = {

    InitialLoad: function(){
        this.CalendarNewPatient();
        this.CopyDiv();
    },

    ChangeCheckNewPatient: function(){
        $('input#SelectedPatientItem').click(function () {
            var $this = $(this);
            if ($this.is(':checked')) {
                CloseNewPatient();
            }
            else { }
        });
    },
    
    ChangeCheck: function () {
        $('input#GenderMale').click(function () {
            var $this = $(this);
            // $this will contain a reference to the checkbox   
            if ($this.is(':checked')) {
                // the checkbox was checked
                $("#GenderFemale").prop("checked", false);
            }
            else {
                $("#GenderFemale").prop("checked", true);
            }
        });

        $('input#GenderFemale').click(function () {
            var $this = $(this);
            // $this will contain a reference to the checkbox   
            if ($this.is(':checked')) {
                // the checkbox was checked 
                $("#GenderMale").prop("checked", false);
            }
            else {
                $("#GenderMale").prop("checked", true);
            }
        });
    },

    CalendarNewPatient: function () {
        $(function () {
            var pickerOpts = {
                showAnim: "drop",
                duration: jQuery.support.boxModel ? 'normal' : 'slow',
                changeMonth: true,
                changeYear: true
            };
            $("#Birthday").datepicker(pickerOpts);
        });
    },

    CopyDiv: function () {
        var htmlDivShow = $('#NewPatient').html();
        $('#NewPatientId').append(htmlDivShow);
    }
}

/*profile office appointment render method*/
var AppointmentObject = {    
    /*profile info*/
    DivAppointmentId: '',
    selOfficeId: '',
    selTreatmentId: '',

    lstOffice: new Array(),

    ///*init meeting calendar variables*/
    Init: function (vInitObject) {
        debugger;
        //init render info
        this.DivAppointmentId = vInitObject.DivAppointmentId;
        this.selOfficeId = vInitObject.selOfficeId;
        this.selTreatmentId = vInitObject.selTreatmentId;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            //set key value pair for an office       
            ;
            AppointmentObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    RenderAsync: function () {
        //create office events
        if (this.lstOffice.length > 1) {
            $('#' + this.selOfficeId).change(function () {
                var selectedVal = $('#' + this.selOfficeId).val();
                AppointmentObject.ChangeOffice(selectedVal);
            });
        }

        //create treatment event


        //create init objects for appointment calendar
        for (var item in this.lstOffice) {

            //get office public id
            this.lstOffice[item].OfficeDivId = this.DivAppointmentId + '_' + this.lstOffice[item].OfficePublicId;

            //create div to put a calendar
            $('#' + this.DivAppointmentId).append($('#' + this.DivAppointmentId + '_Template_Grid').html().replace(/\${OfficePublicId}/gi, this.lstOffice[item].OfficePublicId));
        }
    },

    ChangeOffice: function (vOfficePublicId) {
        var selectOffice = $('#' + this.selOfficeId).val();
        //Remove and add the new items 
        //$('#' + AppointmentObject.selTreatmentId).empty();

        $.each(this.lstOffice[selectOffice].TreatmentList, function (index, value) {
            $('#' + AppointmentObject.selTreatmentId).append('<option value=' + value.CategoryId + '>' + value.Name + '</option>');
            $('#' + AppointmentObject.selTreatmentId).append('<input type="hidden" name="Slot" value=' + value + '>');
        });
        AppointmentObject.RenderOfficeSchedule(selectOffice);
    },

    PageAppointmentMove: function (vOfficePublicId, vNewDate, vNextAvailableDate) {
        $('#divGrid_' + vOfficePublicId).data("kendoGrid").dataSource.read({
            NewDate: vNewDate,
            NextAvailableDate: vNextAvailableDate,
        });
    },

    RenderOfficeSchedule: function (vOfficePublicId) {
        debugger;
        var CurrentOfficeDiv = $('#divGrid_' + vOfficePublicId);
        if (CurrentOfficeDiv.length == 1) {
            if (CurrentOfficeDiv.children().length == 0) {
                //startup grid
                CurrentOfficeDiv.kendoGrid({
                    pageable: false,
                    height: 350,
                    dataSource: {
                        transport: {
                            read: function (options) {

                                var vStartDate = '';
                                var vNextAvailableDate = 'true';
                                var vCategoryId = '';

                                if (options.data != null && options.data.NewDate != null) {
                                    vStartDate = options.data.NewDate;
                                }

                                if (options.data != null && options.data.NextAvailableDate != null) {
                                    vNextAvailableDate = options.data.NextAvailableDate;
                                }
                                if (options.data != null && options.data.CategoryId != null) {
                                    vCategoryId = options.data.CategoryId;
                                }

                                $.ajax({
                                    url: '/api/ScheduleAvailableApi?ProfilePublicId=' + AppointmentObject.lstOffice[vOfficePublicId].ProfilePublicId + '&OfficePublicId=' + vOfficePublicId + '&TreatmentId=' + vCategoryId + '&NextAvailableDate=' + vNextAvailableDate + '&StartDateTime=' + vStartDate,
                                    dataType: "json",
                                    type: "POST",
                                    success: function (result) {
                                 var NewResult = new Array();
                                        //set header titles
                                        $.each(result, function (item, value) {
                                            if (value.Monday.IsHeader == true) {
                                                var HeaderHtml = $('#divAppointmentSchedule_Template_Grid_Event_Header').html();
                                                $('#' + AppointmentObject.DivAppointmentId).find('[data-field=Monday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Monday.AvailableDateText));
                                                $('#' + AppointmentObject.DivAppointmentId).find('[data-field=Tuesday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Tuesday.AvailableDateText));
                                                $('#' + AppointmentObject.DivAppointmentId).find('[data-field=Wednesday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Wednesday.AvailableDateText));
                                                $('#' + AppointmentObject.DivAppointmentId).find('[data-field=Thursday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Thursday.AvailableDateText));
                                                $('#' + AppointmentObject.DivAppointmentId).find('[data-field=Friday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Friday.AvailableDateText));
                                                $('#' + AppointmentObject.DivAppointmentId).find('[data-field=Saturday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Saturday.AvailableDateText));
                                            }
                                            else {
                                                NewResult.push(value);
                                            }
                                        });
                                        options.success(NewResult);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                    }
                                });
                            }
                        },
                    },
                    columns: [{
                        field: 'Monday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivAppointmentId + '_Template_Grid_Event_Monday').html()
                    }, {
                        field: 'Tuesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivAppointmentId + '_Template_Grid_Event_Tuesday').html()
                    }, {
                        field: 'Wednesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivAppointmentId + '_Template_Grid_Event_Wednesday').html()
                    }, {
                        field: 'Thursday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivAppointmentId + '_Template_Grid_Event_Thursday').html()
                    }, {
                        field: 'Friday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivAppointmentId + '_Template_Grid_Event_Friday').html()
                    }, {
                        field: 'Saturday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivAppointmentId + '_Template_Grid_Event_Saturday').html()
                    }, {

                    }],
                    dataBound: function (e) {
                        //set header before render
                        var lstData = this.dataSource.view();

                        $('#spanHeader_' + vOfficePublicId).hide();
                        $('#divGrid_NotSchedule_' + vOfficePublicId).hide();

                        if (lstData != null && lstData.length > 0) {

                            $('#spanHeader_' + vOfficePublicId).html(lstData[0].HeaderTitle);

                            if (lstData[0].AfterDate != null && lstData[0].AfterDate.length > 0) {
                                $('#aAfter_' + vOfficePublicId).click(function () {
                                    AppointmentObject.PageMove(vOfficePublicId, lstData[0].AfterDate, 'false');
                                });
                                $('#aAfter_' + vOfficePublicId).show();
                            }
                            else {
                                $('#aAfter_' + vOfficePublicId).hide();
                            }

                            if (lstData[0].BeforeDate != null && lstData[0].BeforeDate.length > 0) {
                                $('#aBefore_' + vOfficePublicId).click(function () {
                                    AppointmentObject.PageMove(vOfficePublicId, lstData[0].BeforeDate, 'false');
                                });
                                $('#aBefore_' + vOfficePublicId).show();
                            }
                            else {
                                $('#aBefore_' + vOfficePublicId).hide();
                            }

                            if (lstData.length == 1 && lstData[0].CurrentDate != null && lstData[0].CurrentDate.length > 0) {

                                $('#divGrid_NotSchedule_' + vOfficePublicId).click(function () {
                                    AppointmentObject.PageMove(vOfficePublicId, lstData[0].CurrentDate, 'true');
                                });

                                $('#divGrid_NotSchedule_' + vOfficePublicId).show();
                            }

                            $('#spanHeader_' + vOfficePublicId).show();
                        }
                        else {

                            $('#divGrid_NotSchedule_' + vOfficePublicId).click(function () {
                                AppointmentObject.PageMove(vOfficePublicId, null, 'true');
                            });

                            $('#divGrid_NotSchedule_' + vOfficePublicId).show();
                        }
                        $("#SelectedTreatment").change(function () {
                            var selectedVal = $("#SelectedTreatment option:selected").val();
                            AppointmentObject.PageMove(vOfficePublicId, lstData[0].CurrentDate, 'true',
                                selectedVal);
                        });
                        e.preventDefault();
                    }
                });
            }

            //show grid
            $('.SelOfficeGrid').hide();
            $('#divScheduleContainer_' + vOfficePublicId).fadeIn('slow');

            //select de current menu
            $('.SelOfficeMenu').removeClass('MPProfileCallendarTabs');
            $('#li_' + vOfficePublicId).addClass('selected');
        }
    },

    PageMove: function (vOfficePublicId, vNewDate, vNextAvailableDate, vCategoryId) {

        $('#divGrid_' + vOfficePublicId).data("kendoGrid").dataSource.read({
            NewDate: vNewDate,
            NextAvailableDate: vNextAvailableDate,
            CategoryId: vCategoryId
        });
    },

    SetHour: function (vCurrentHour, vCurrentHourUnFormated) {

        $('#StartDate').val(vCurrentHourUnFormated);
        $('#DateMoreInfoIdN').val(vCurrentHour);
        $('#DateMoreInfoIdNN').val(vCurrentHour);
        $("#FreeScheduleId").hide(1000);
    },
};

$('#DateMoreInfoIdN').click(function () {

    var selectedOfficeVal = $("#officeSelectedId option:selected").val();

    AppointmentObject.RenderOfficeSchedule(selectedOfficeVal);
    $("#FreeScheduleId").show(1000);
    $("#DateMoreInfoId").hide();
});
$('#DateMoreInfoIdNN').click(function () {

    var selectedOfficeVal = $("#officeSelectedId option:selected").val();
    AppointmentObject.RenderOfficeSchedule(selectedOfficeVal);
    $("#FreeScheduleId").show(1000);
    $("#DateMoreInfoId").hide();
});
$('#SaveAppointmentId').click(function () {
    var startDate = $("#StartDate").val();
    var treatmentSelected = $("#SelectedTreatment").val();
    if (startDate == "") {
        $('#DateMoreInfoIdN').val("Aun no has seleccionado una hora para tu cita");
        return false;
    }
    if (treatmentSelected == null) {
        return false;
    }
    //Validate legal terms
    $('#AppointmentForm').validate({ // initialize the plugin
        errorClass: 'error help-inline',
        validClass: 'success',
        errorElement: 'span',
        rules: {
            'LegalTerms': {
                required: true,
            }
        },
        messages: {
            'LegalTerms': {
                required: "Debes aceptar los términos y condiciones.",
            }
        }
    });
});
$("#isSomeBody").click(function () {
    $('#NewPatientId').show(800);
});
$("#GenderFemale").click(function () {
    $("#GenderMale").attr('checked', false);
});
$("#GenderMale").click(function () {
    $("#GenderFemale").attr('checked', false);
});
$("#DateLessInfoId").click(function () {
    $("#FreeScheduleId").hide(1000);
    $("#DateMoreInfoId").show();
});
