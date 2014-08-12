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

/*manage date and time conversions*/
function serverDateToString(vDate) {
    return vDate.getFullYear() + '-' + ((new Number(vDate.getMonth())) + 1) + '-' + vDate.getDate();
}

function serverDateTimeToString(vDate) {
    return vDate.getFullYear() + '-' + ((new Number(vDate.getMonth())) + 1) + '-' + vDate.getDate() + 'T' + vDate.getHours() + ':' + vDate.getMinutes();
}
/*function start global pages controls*/
function InitGlobalPagesControls(InitParams) {

    InitSeachBox(InitParams.SearchBoxId, InitParams.CityId);

}

function InitSeachBox(SearchBoxId, CityId) {

    if ($('#' + SearchBoxId).lenght > 0) {

        $('#' + SearchBoxId).autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/api/SearchApi?IsAc=true&CityId=' + CityId + '&SearchParam=' + request.term,
                    dataType: "json",
                    success: function (data) {
                        response(data);
                    }
                });
            },
        });

        $('#' + SearchBoxId).autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/api/SearchApi?IsAc=true&CityId=' + CityId + '&SearchParam=' + request.term,
                    dataType: 'json',
                    success: function (data) {
                        response(data);
                    }
                });
            },
            minLength: 2,
            focus: function (event, ui) {
                $('#' + SearchBoxId).val(ui.item.MatchQuery);
                return false;
            },
            select: function (event, ui) {
                //UpsertAppointmentObject.AddPatientAppointment({
                //    ProfileImage: ui.item.ProfileImage,
                //    Name: ui.item.Name,
                //    IdentificationNumber: ui.item.IdentificationNumber,
                //    Mobile: ui.item.Mobile,
                //    Email: ui.item.Email,
                //    PatientPublicId: ui.item.PatientPublicId
                //});
                //Layout_SearchBoxAcTemplate
                //$('#getPatient').val('');
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

    }
}

/*show hide user menu*/
function Header_ShowHideUserMenu(divId) {

    $('#' + divId).toggle('slow');
}
