//init office grid
function OfficeListGrid(vidDiv, vCreateUrl, vOfficeData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: '<a href="' + vCreateUrl + '">Nuevo consultorio</a>' }],
        dataSource: {
            type: "json",
            data: vOfficeData,
        },
        columns: [{
            field: "Name",
            title: "Nombre",
            template: '<a href="${UrlToUpdate}">${Name}</a>'
        }, {
            field: "CityName",
            title: "Ciudad"
        }, {
            field: "IsDefault",
            title: "Predeterminada"
        }, {
            field: "LastModify",
            title: "Modificación"
        }, {
            field: "CreateDate",
            title: "Creación"
        }]
    });
}

//init office treatment grid
function OfficeTreatmentListGrid(vidDiv, vCreateUrl, vTreatmentData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: '<a href="' + vCreateUrl + '">Asociar tratamiento</a>' }],
        dataSource: {
            type: "json",
            data: vTreatmentData,
        },
        columns: [{
            field: "Name",
            title: "Nombre",
            template: '<a href="${UrlToUpdate}">${Name}</a>'
        }, {
            field: "IsDefault",
            title: "Predeterminado"
        }, {
            field: "LastModify",
            title: "Modificación"
        }, {
            field: "CreateDate",
            title: "Creación"
        }]
    });
}

//init autocomplete treatment
function OfficeTreatmentAc(acId, acData) {

    $('#' + acId).autocomplete(
	{
	    source: acData,
	    minLength: 0,
	    focus: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        return false;
	    },
	    select: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        $('#' + acId + '-id').val(ui.item.value);
	        $('#' + acId + '-duration').val(ui.item.duration);
	        return false;
	    }
	}).data("ui-autocomplete")._renderItem = function (ul, item) {
	    return $("<li></li>")
			.data("ui-autocomplete-item", item)
			.append("<a><strong>" + item.label + "</strong></a>")
			.appendTo(ul);
	};
}

//init autocomplete specialty
function OfficeSpecialtyAc(acId, acData) {

    $('#' + acId).autocomplete(
	{
	    source: acData,
	    minLength: 0,
	    focus: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        return false;
	    },
	    select: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        $('#' + acId + '-id').val(ui.item.value);
	        $('#' + acId + '-duration').val(ui.item.duration);
	        return false;
	    }
	}).data("ui-autocomplete")._renderItem = function (ul, item) {
	    return $("<li></li>")
			.data("ui-autocomplete-item", item)
			.append("<a><strong>" + item.label + "</strong></a>")
			.appendTo(ul);
	};
}

//init office specialty grid
function OfficeSpecialtyListGrid(vidDiv, vCreateUrl, vSpecialtyData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: '<a href="' + vCreateUrl + '">Asociar especialidad</a>' }],
        dataSource: {
            type: "json",
            data: vTreatmentData,
        },
        columns: [{
            field: "Name",
            title: "Nombre",
            template: '<a href="${UrlToUpdate}">${Name}</a>'
        }, {
            field: "IsDefault",
            title: "Predeterminado"
        }, {
            field: "LastModify",
            title: "Modificación"
        }, {
            field: "CreateDate",
            title: "Creación"
        }]
    });
}

//init Schedule Available grid
function OfficeScheduleAvailableListGrid(vidDiv, vScheduleData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vScheduleData,
        },
        sortable: {
            mode: "multiple",
            allowUnsort: false
        },
        columns: [{
            field: "ScheduleDayName",
            title: "Día",
        }, {
            field: "StartTime",
            title: "Hora inicio"
        }, {
            field: "EndTime",
            title: "Hora fin"
        }, {
            field: "CreateDate",
            title: "Creación"
        }, {
            field: "ScheduleAvailableId",
            title: "&nbsp;",
            template: $("#templateDelete").html(),
            width: 300
        }],
    });
}

//init Schedule Available grid
function ProfileAutorizationListGrid(vidDiv, vProfileData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vProfileData,
        },
        columns: [{
            field: "RoleName",
            title: "Rol",
        }, {
            field: "UserEmail",
            title: "Correo electrónico"
        }, {
            field: "ProfileRoleId",
            title: "&nbsp;",
            template: $("#templateDelete").html()
        }],
    });
}

//init autocomplete Insurance
function OfficeInsuranceAc(acId, acData) {

    $('#' + acId).autocomplete(
	{
	    source: acData,
	    minLength: 0,
	    focus: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        return false;
	    },
	    select: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        $('#' + acId + '-id').val(ui.item.value);
	        return false;
	    }
	}).data("ui-autocomplete")._renderItem = function (ul, item) {
	    return $("<li></li>")
			.data("ui-autocomplete-item", item)
			.append("<a><strong>" + item.label + "</strong></a>")
			.appendTo(ul);
	};
}

//init InsuranceProfile grid
function ProfileInsuranceListGrid(vidDiv, vInsuranceData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vInsuranceData,
        },
        columns: [{
            field: "CategoryId",
            title: "Id",
        }, {
            field: "Name",
            title: "Seguro"
        }, {
            field: "CategoryId",
            title: "&nbsp;",
            template: $("#templateDelete").html()
        }],
    });
}


function ProfileTreatmentListGrid(vidDiv, vInsuranceData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vInsuranceData,
        },
        columns: [{
            field: "CategoryId",
            title: "Id",
        }, {
            field: "Name",
            title: "Tratamiento"
        }, {
            field: "CategoryId",
            title: "&nbsp;",
            template: $("#templateDelete").html()
        }],
    });
}

//init autocomplete Treatment
function ProfileTreatmentAc(acId, acData) {

    $('#' + acId).autocomplete(
	{
	    source: acData,
	    minLength: 0,
	    focus: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        return false;
	    },
	    select: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        $('#' + acId + '-id').val(ui.item.value);
	        return false;
	    }
	}).data("ui-autocomplete")._renderItem = function (ul, item) {
	    return $("<li></li>")
			.data("ui-autocomplete-item", item)
			.append("<a><strong>" + item.label + "</strong></a>")
			.appendTo(ul);
	};
}

function ProfileSpecialtyListGrid(vidDiv, vSpecialtyData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vSpecialtyData,
        },
        columns: [{
            field: "CategoryId",
            title: "Id",
        }, {
            field: "Name",
            title: "Especialidad"
        }, {
            field: "IsDefault",
            title: "Predeterminada"
        }, {
            field: "CategoryId",
            title: "&nbsp;",
            template: $("#templateDelete").html()
        }],
    });
}

//init autocomplete Treatment
function ProfileSpecialtyAc(acId, acData) {

    $('#' + acId).autocomplete(
	{
	    source: acData,
	    minLength: 0,
	    focus: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        return false;
	    },
	    select: function (event, ui) {
	        $('#' + acId).val(ui.item.label);
	        $('#' + acId + '-id').val(ui.item.value);
	        return false;
	    }
	}).data("ui-autocomplete")._renderItem = function (ul, item) {
	    return $("<li></li>")
			.data("ui-autocomplete-item", item)
			.append("<a><strong>" + item.label + "</strong></a>")
			.appendTo(ul);
	};
}

//init profile search grid
function ProfileSearchGrid(vidDiv) {

    //configure grid
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateHeader").html() }],
        pageable: true,
        dataSource: {
            pageSize: 20,
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

                    var oSearchCriteria = $('#' + vidDiv + '-txtSearch').val();

                    $.ajax({
                        url: '/api/ProfileApi?SearchCriteria=' + oSearchCriteria + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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
            field: "Name",
            title: "Nombre",
            template: $("#templateName").html()
        }, {
            field: "Certified",
            title: "Certificado"
        }, {
            field: "Email",
            title: "Email"
        }, {
            field: "ProfileStatus",
            title: "Estado"
        }],
    });

    //add search button event
    $('#' + vidDiv + '-Search').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
}

function RelatedProfileListGrid(vidDiv, vRelatedData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateHeader").html() }],
        dataSource: {
            type: "json",
            data: vRelatedData,
        },
        columns: [{
            field: "Name",
            title: "Nombre",
            template: $('#TemplateUserFrom').html()
        }, {
            field: "Email",
            title: "Correo Electrónico"
        }, {
            field: "CategoryId",
            title: "&nbsp;",
            template: $("#templateDelete").html()
        }],
    });
}

function ProfileReminderListGrid(vidDiv, vReminderData) {
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#template_Header").html() }],
        dataSource: {
            type: "json",
            data: vReminderData,
        },
        columns: [{
            field: "Name",
            title: "Tipo recordatorio",
            width: 200
        }, {
            field: "ValueEmail",
            title: "Email",
            template: $("#templateCheckMessageEmail").html(),
            width: 70
        }, {
            field: "ValueSms",
            title: "SMS",
            template: $("#templateCheckMessageSMS").html(),
            width: 70
        }, {
            field: "ValueNotify",
            title: "Notificaciones Gurú",
            template: $("#templateCheckMessageNotificationGuru").html(),
            width: 180
        },
        {
            field: "ProgramTime",
            title: "Tiempo previo (horas)",
            template: $("#templateHourTime").html()

        }],
    });

    //start all spinner
    $('.spinnerSelector').spinner({
        min: 1,
        step: 1,
    });

    //checked selected checkbox
    $('.SelectedChecked').attr('checked', 'checked');
}

function ProfileComunicationListGrid(vidDiv, vReminderData) {
    debugger;
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#template_Header").html() }],
        dataSource: {
            type: "json",
            data: vReminderData,
           
        },
        columns: [{
            field: "Name",
            title: "Tipo recordatorio",
            width: 200
        }, {
            field: "ValueEmail",
            title: "Email",
            template: $("#templateCheckMessageEmail").html(),
            width: 100
        }, {
            field: "ValueSms",
            title: "SMS",
            template: $("#templateCheckMessageSMS").html(),
            width: 100
        }, {
            field: "ValueNotify",
            title: "Notificaciones Gurú",
            template: $("#templateCheckMessageNotificationGuru").html(),
            width: 100
        }],
    });

    //start all spinner
    $('.spinnerSelector').spinner({
        min: 1,
        step: 1,
    });

    //checked selected checkbox
    $('.SelectedChecked').attr('checked', 'checked');
}

//Valitation, Email 
function ValidEmailAutorizationProfileList(ControlId )
{
    $('#' + ControlId).validate({
        //debug: true,
        errorClass: 'error help-inline',
        validClass: 'success',
        errorElement: 'span',
        highlight: function (element, errorClass, validClass) {
            $(element).parents("div.control-group").addClass(errorClass).removeClass(validClass);

        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".error").removeClass(errorClass).addClass(validClass);
        },
        rules: {
            UserEmail: {
                email: true
            }
        },
        messages: {
            UserEmail:
                {
                    required: " Debe ingresar una dirección de correo electrónico",
                    email: " Debe ingresar un email valido"
                }
        }
    });

}