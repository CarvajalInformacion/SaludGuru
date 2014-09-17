
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

    $("#NewPatientId").hide(800);
    $("#CreatePatientForm").submit(); //Submit  the FORM
    $("#CreatePatientForm input#Name, input#LastName, input#Identification, input#Birthday, input#Mobile, input#Email").val('');
}

function CloseNewPatient() {
    $("#NewPatientId").hide(800);
    $("#CreatePatientForm input#Name, input#LastName, input#Identification, input#Birthday, input#Mobile, input#Email").val('');
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

    InitialLoad: function () {
        //this.CalendarNewPatient();
        this.CopyDiv();
    },

    ChangeCheckNewPatient: function () {
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
    NextSchedule: '',

    lstOffice: new Array(),

    ///*init meeting calendar variables*/
    Init: function (vInitObject) {
        //init render info
        this.DivAppointmentId = vInitObject.DivAppointmentId;
        this.selOfficeId = vInitObject.selOfficeId;
        this.selTreatmentId = vInitObject.selTreatmentId;
        this.NextSchedule = vInitObject.NextAvailableDate;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            //set key value pair for an office       
            ;
            AppointmentObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    RenderAsync: function () {
        //create office events
        if (this.lstOffice.length < 1) {
            $('#' + this.selOfficeId).change(function () {
                var selectedVal = $('#' + this.selOfficeId).val();
                AppointmentObject.ChangeOffice(selectedVal);
            });
        }

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
 
    RenderOfficeSchedule: function (vOfficePublicId, actualDate, vProPublicId, vNextSchedule, vTreatment, vIsPrevDay) {
        debugger;
        var CurrentOfficeDiv = $('#divGrid_' + vOfficePublicId);
        var vOPublicId = $('#' + vOfficePublicId).val();
        var Treatment = $('#' + this.selTreatmentId).val();
        var oTreatment = $('#' + vTreatment).val();
        if (actualDate == null) {
            var vNewDate = '';
        }
        else {
            var vNewDate = actualDate;
        }
        if (vIsPrevDay == 'true') {
            $.ajax(
               {
                   url: '/api/ScheduleAvailableApi/GetEventAvailableWeek?ProfilePublicId=' + vProPublicId + '&OfficePublicId=' + vOPublicId + '&TreatmentId=' + oTreatment + '&NextAvailableDate=' + 'true' + '&StartDateTime=' + vNewDate + '&Mobile=' + 'true' + '&IsPrevDay=' + 'true',
                   dataType: "json",
                   type: "POST",
                   success: function (data, textStatus, jqXHR) {
                       AppointmentObject.RenderHour(data);

                       //AddPatientToList(data);
                   },
                   error: function (jqXHR, textStatus, errorThrown) {
                       //if fails    
                   }
               });
        }

        if (vNextSchedule == 'true') {
            $.ajax(
          {
              url: '/api/ScheduleAvailableApi/GetEventAvailableWeek?ProfilePublicId=' + vProPublicId + '&OfficePublicId=' + vOPublicId + '&TreatmentId=' + oTreatment + '&NextAvailableDate=' + 'true' + '&StartDateTime=' + vNewDate + '&Mobile=' + 'true' + '&IsPrevDay=' + 'false',
              dataType: "json",
              type: "POST",
              success: function (data, textStatus, jqXHR) {
                  AppointmentObject.RenderHour(data);

                  //AddPatientToList(data);
              },
              error: function (jqXHR, textStatus, errorThrown) {
                  //if fails   
              }
          });
        }
        else {
            $.ajax(
          {
              url: '/api/ScheduleAvailableApi/GetEventAvailableWeek?ProfilePublicId=' + AppointmentObject.lstOffice[vOfficePublicId].ProfilePublicId + '&OfficePublicId=' + vOfficePublicId + '&TreatmentId=' + Treatment + '&NextAvailableDate=' + 'false' + '&StartDateTime=' + vNewDate + '&Mobile=' + 'false' + '&IsPrevDay=' + 'false',
              dataType: "json",
              type: "POST",
              success: function (data, textStatus, jqXHR) {
                  AppointmentObject.RenderHour(data);

                  //AddPatientToList(data);
              },
              error: function (jqXHR, textStatus, errorThrown) {
                  //if fails    
              }
          });
        }
        //show grid
        //$('.SelOfficeGrid').hide();
        //$('#divScheduleContainer_' + vOfficePublicId).fadeIn('slow');

        //select de current menu
        $('.SelOfficeMenu').removeClass('MPProfileCallendarTabs');
        $('#li_' + vOfficePublicId).addClass('selected');
    },

    RenderHour: function (Hours) {
        if (Hours != null) {
            $("#ul_GridFreeSchedule").html('')
            $.each(Hours, function (i, item) {
                //get html notification template                 
                var valSet = $('#ul_GridFreeSchedule').html();

                if (item.IsHeader) {
                    $("#ul_GridFreeSchedule").append('<li>' + item.AvailableDateTemplateText + '</li>');
                    $('#divGrid_Title').html('');
                    var valDiv = $('#divGrid_Title').html();

                    $('#divGrid_Title').append('<a id="aPrevDay" onclick="AppointmentObject.PrevAvalableDay(' + "'" + item.PublicProfileId + "','" + item.NextDate + "'" + ' );" ' + " href=" + ' "javascript:"' + ">Anterior             </a>");
                    $('#divGrid_Title').append('<a id="aNextDay" onclick="AppointmentObject.NextAndPrevDay(' + "'" + item.PublicProfileId + "','" + item.NextDate + "'" + ' );" ' + " href=" + ' "javascript:"' + ">Siguiente </a>");
                    $('#divGrid_Title').append('<label id="lblNextDay" style="display:none">' + item.NextDate + '</label>');

                    var gridSearchSh = $('#divGrid_NotSchedule').html('');


                    gridSearchSh.append('<a id="SearchNexAvailableDayId" onclick="AppointmentObject.NextAndPrevDay(' + "'" + item.PublicProfileId + "','" + item.NextDate + "'" + ' );" ' + " href=" + ' "javascript:"' + ">Buscar siguiente horario disponible </a>");

                }
                else {
                    $("#ul_GridFreeSchedule").append('<li class="MPFreeSchedule">' + '<a onclick="AppointmentObject.SetHour(' + "'" + item.AvailableDateTemplateText + "','" + item.AvailableDate + "'" + ' );"   href="javascript:;"> ' + item.AvailableDateText + ' </a></li>');
                }
            });
        }
    },

    NextAndPrevDay: function (vPublicProfileId, NextDay) {
        var actualDate = $('#lblNextDay').val();
        AppointmentObject.RenderOfficeSchedule('SelectedOffice', NextDay, vPublicProfileId, 'true', 'SelectedTreatment')
    },

    PrevAvalableDay: function (vPublicProfileId, NextDay) {
        var actualDate = $('#lblNextDay').val();
        AppointmentObject.RenderOfficeSchedule('SelectedOffice', NextDay, vPublicProfileId, 'false', 'SelectedTreatment', 'true')
    },

    RenderOfficeScheduleProfile: function (vOfficePublicId, actualDate, vProPublicId, vNextSchedule, vTreatment, vIsPrevDay) {
        debugger;
        var CurrentOfficeDiv = $('#divGrid_' + vOfficePublicId);
        var vOPublicId = $('#officeSelectedId').val();
        var Treatment = $('#RelatedTreatment').val();
        var oTreatment = $('#RelatedTreatment').val();
        if (actualDate == null) {
            var vNewDate = '';
        }
        else {
            var vNewDate = actualDate;
        }
        if (vIsPrevDay == 'true') {
            $.ajax(
               {
                   url: '/api/ScheduleAvailableApi/GetEventAvailableWeek?ProfilePublicId=' + vProPublicId + '&OfficePublicId=' + vOPublicId + '&TreatmentId=' + oTreatment + '&NextAvailableDate=' + 'true' + '&StartDateTime=' + vNewDate + '&Mobile=' + 'true' + '&IsPrevDay=' + 'true',
                   dataType: "json",
                   type: "POST",
                   success: function (data, textStatus, jqXHR) {
                       AppointmentObject.RenderHourProfile(data, vOfficePublicId);

                       //AddPatientToList(data);
                   },
                   error: function (jqXHR, textStatus, errorThrown) {
                       //if fails    
                   }
               });
        }

        if (vNextSchedule == 'true') {
            $.ajax(
          {
              url: '/api/ScheduleAvailableApi/GetEventAvailableWeek?ProfilePublicId=' + vProPublicId + '&OfficePublicId=' + vOPublicId + '&TreatmentId=' + oTreatment + '&NextAvailableDate=' + 'true' + '&StartDateTime=' + vNewDate + '&Mobile=' + 'true' + '&IsPrevDay=' + 'false',
              dataType: "json",
              type: "POST",
              success: function (data, textStatus, jqXHR) {
                  AppointmentObject.RenderHourProfile(data, vOfficePublicId);

                  //AddPatientToList(data);
              },
              error: function (jqXHR, textStatus, errorThrown) {
                  //if fails   
              }
          });
        }
        else {
            $.ajax(
          {
              url: '/api/ScheduleAvailableApi/GetEventAvailableWeek?ProfilePublicId=' + AppointmentObject.lstOffice[vOfficePublicId].ProfilePublicId + '&OfficePublicId=' + vOfficePublicId + '&TreatmentId=' + Treatment + '&NextAvailableDate=' + 'false' + '&StartDateTime=' + vNewDate + '&Mobile=' + 'false' + '&IsPrevDay=' + 'false',
              dataType: "json",
              type: "POST",
              success: function (data, textStatus, jqXHR) {
                  AppointmentObject.RenderHourProfile(data, vOfficePublicId);

                  //AddPatientToList(data);
              },
              error: function (jqXHR, textStatus, errorThrown) {
                  //if fails    
              }
          });
        }
        //show grid
        //$('.SelOfficeGrid').hide();
        //$('#divScheduleContainer_' + vOfficePublicId).fadeIn('slow');

        //select de current menu
        $('.SelOfficeMenu').removeClass('MPProfileCallendarTabs');
        $('#li_' + vOfficePublicId).addClass('selected');
    },

    RenderHourProfile: function (Hours, vOfficePublicId) {
        debugger;
        var UrlProfile = $('#UrlRedirect').val();
        if (Hours != null) {
            $("#ul_GridFreeSchedule").html('')
            $.each(Hours, function (i, item) {
                //get html notification template                 
                var valSet = $('#ul_GridFreeSchedule').html();

                if (item.IsHeader) {
                    $("#ul_GridFreeSchedule").append('<li>' + item.AvailableDateTemplateText + '</li>');
                    $('#divGrid_Title').html('');
                    var valDiv = $('#divGrid_Title').html();

                    $('#divGrid_Title').append('<a id="aPrevDay" onclick="AppointmentObject.PrevAvalableDayProfile(' + "'" + item.PublicProfileId + "','" + item.NextDate + "'" + ' );" ' + " href=" + ' "javascript:"' + ">Anterior             </a>");
                    $('#divGrid_Title').append('<a id="aNextDay" onclick="AppointmentObject.NextAndPrevDayProfile(' + "'" + item.PublicProfileId + "','" + item.NextDate + "'" + ' );" ' + " href=" + ' "javascript:"' + ">Siguiente </a>");
                    $('#divGrid_Title').append('<label id="lblNextDay" style="display:none">' + item.NextDate + '</label>');

                    var gridSearchSh = $('#divGrid_NotSchedule').html('');

                    
                    gridSearchSh.append('<a id="SearchNexAvailableDayId" onclick="AppointmentObject.NextAndPrevDayProfile(' + "'" + item.PublicProfileId + "','" + item.NextDate + "'" + ' );" ' + " href=" + ' "javascript:"' + ">Buscar siguiente horario disponible </a>");

                }
                else {
                    $("#ul_GridFreeSchedule").append('<li class="MPFreeSchedule">' + '<a href="'+ UrlProfile + '&OfficePublicId=' + vOfficePublicId + '&Date=' + item.AvailableDateText + '"> ' + item.AvailableDateText + ' </a></li>');
                }
            });
        }
    },
    
    NextAndPrevDayProfile: function (vPublicProfileId, NextDay) {
        debugger;
        var actualDate = $('#lblNextDay').val();
        AppointmentObject.RenderOfficeScheduleProfile('SelectedOffice', NextDay, vPublicProfileId, 'true', 'SelectedTreatment')
    },

    PrevAvalableDayProfile: function (vPublicProfileId, NextDay) {
        debugger;
        var actualDate = $('#lblNextDay').val();
        AppointmentObject.RenderOfficeScheduleProfile('SelectedOffice', NextDay, vPublicProfileId, 'false', 'SelectedTreatment', 'true')
    },

    PageMove: function (vOfficePublicId, vNewDate, vNextAvailableDate, vCategoryId) {

        $('#divGrid_' + vOfficePublicId).data("kendoGrid").dataSource.read({
            NewDate: vNewDate,
            NextAvailableDate: vNextAvailableDate,
            CategoryId: vCategoryId
        });
    },

    SetHour: function (vCurrentHour, vCurrentHourUnFormated) {
        debugger;
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
$('#DateInfoId').click(function () {
    debugger;
    var selectedOfficeVal = $("#officeSelectedId").val();

    AppointmentObject.RenderOfficeScheduleProfile(selectedOfficeVal);
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
    if ($('input#isSomeBody').is(':checked')) {
        $('#isSomeBodyValidate').show();
        return false;
    }
    else {
        $('#isSomeBodyValidate').hide();
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
    $("input#Name").focus();
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

