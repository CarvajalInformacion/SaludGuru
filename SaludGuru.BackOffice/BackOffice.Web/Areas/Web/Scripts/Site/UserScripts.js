//init Notifications grid
function UserNotificationsListGrid(vidDiv, vNotificationData) {
    debugger;
    $('#' + vidDiv).kendoGrid({
        dataSource: {
            type: "json",
            data: vNotificationData,
        },
        columns: [{
            field: "CreateDate",
            title: "Fecha",
            template: $('#TemplateNotificationType').html(),
        }, {
            field: "UserFromName",
            title: "Enviado Por",
            template: $('#TemplateUserFrom').html(),
        }, {
            field: "Body",
            title: "Notificación"
        }],
    });
}