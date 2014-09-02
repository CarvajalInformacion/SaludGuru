//init office grid
function TreatmentListGrid(vidDiv, oCreateUrl, vTreatmentData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: '<a href="'+ oCreateUrl + '">Nuevo Tratamiento</a>' }],
        dataSource: {
            type: "json",
            data: vTreatmentData,
        },
        columns: [{
            field: "Name",
            title: "Nombre",
            template: '<a href="${UrlToUpdate}">${Name}</a>'
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
        toolbar: [{ template: '<a href="' + vCreateUrl + '">Asociar Tratamiento</a>' }],
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

    //Duration
    //
    //$('#' + divId).autocomplete({
    //    surce: acData,
    //    minLength: 0,
    //    //_renderItem: function (ul, item) {
    //    //    return $("<li>")
    //    //      .attr("data-value", item.Id)
    //    //      .append($("<a>").text(item.Name))
    //    //      .appendTo(ul);
    //    //}
    //}).data("autocomplete")._renderItem = function (ul, item) {
    //    
    //    return $("<li>").attr("data-value", item.value).append("<a>" + item.label + "</a>").appendTo(ul);
    //};
}




