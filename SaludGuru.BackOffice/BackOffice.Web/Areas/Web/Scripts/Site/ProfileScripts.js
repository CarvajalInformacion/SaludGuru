function OfficeListGrid(vidDiv, vCreateUrl, vOfficeData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: '<a href="' + vCreateUrl + '">Nueva oficina</a>' }],
        dataSource: {
            type: "json",
            data: vOfficeData,
        },

        columns: [{
                field: "Name",
                title: "Nombre"
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