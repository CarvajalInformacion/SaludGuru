//init office grid
function OfficeListGrid(vidDiv, vCreateUrl, vOfficeData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: '<a href="' + vCreateUrl + '">Nueva oficina</a>' }],
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

//init Schedule Available grid
function OfficeScheduleAvailableListGrid(vidDiv, vScheduleData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vScheduleData,
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
            template: $("#templateDelete").html()        
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
            field: "Role",
            title: "Role",
        }, {
            field: "UserEmail",
            title: "Email"
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