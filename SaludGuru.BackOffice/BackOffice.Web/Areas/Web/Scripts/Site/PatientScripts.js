//init patient search grid
function PatientListGrid(vidDiv) {

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
                        return data[0].SearchPatientCount;
                    }
                    return 0;
                }
            },
            transport: {
                read: function (options) {

                    var oSearchCriteria = $('#' + vidDiv + '-txtSearch').val();
                    var oPublicProfileId = $('#' + vidDiv + '-PublicProfileId').val();
                    
                    $.ajax({
                        url: '/api/PatientApi?PublicProfileId=' + oPublicProfileId + '&SearchCriteria=' + oSearchCriteria + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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
            field: "Identification",
            title: "Indentificación"
        }, {
            field: "Email",
            title: "Contacto"
        }, {
            field: "Telephone",
            title: "Teléfono"
        }],
    });

    //add search button event
    $('#' + vidDiv + '-Search').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
}


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
            title: "id Appointment ",
            template: $("#templateName").html()
        }, {
            field: "CreateDate",
            title: "Fecha Creación ",
        }, {
            field: "StatusName",
            title: "Estado Cita ",
        }],
    });
}
