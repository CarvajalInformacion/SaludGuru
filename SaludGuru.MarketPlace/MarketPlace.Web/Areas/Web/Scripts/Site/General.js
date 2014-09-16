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

    //init search box 
    SearchBoxObject.Init({
        InputId: InitParams.SearchBoxId,
        CityId: InitParams.CityId,
    });
    SearchBoxObject.RenderAsync();


    //init schedule appointment object
    ScheduleAppointmentObject.Init({
        FBUrl: InitParams.FBUrl,
        GoogleUrl: InitParams.GoogleUrl,
        OutlookUrl: InitParams.OutlookUrl,
        IsLogin: InitParams.IsLogin,
    });

    //init change city action
    if ($('#' + InitParams.selCityId).length > 0) {
        $('#' + InitParams.selCityId).change(function () {
            if (InitParams.CityId != $(this).val()) {
                window.location = '/Home/ChangeCity?NewCityId=' + $(this).val();
            }
        });
    }
}

/*schedule appointment*/
var ScheduleAppointmentObject = {
    FBUrl: '',
    GoogleUrl: '',
    OutlookUrl: '',
    IsLogin: false,

    Init: function (vInitObject) {
        this.FBUrl = vInitObject.FBUrl;
        this.GoogleUrl = vInitObject.GoogleUrl;
        this.IsLogin = vInitObject.IsLogin;
        this.OutlookUrl = vInitObject.OutlookUrl;
    },

    ScheduleAppointment: function (vLink) {
        if (ScheduleAppointmentObject.IsLogin) {
            window.location = vLink;
        }
        else {

            $('#dialog_ScheduleAppointmentNotLogin .MPFacebookLogIn').attr('href', ScheduleAppointmentObject.FBUrl.replace(/{{UrlRetorno}}/gi, vLink));
            $('#dialog_ScheduleAppointmentNotLogin .MPGoogleLogIn').attr('href', ScheduleAppointmentObject.GoogleUrl.replace(/{{UrlRetorno}}/gi, vLink));
            $('#dialog_ScheduleAppointmentNotLogin .MPOutlookLogIn').attr('href', ScheduleAppointmentObject.OutlookUrl.replace(/{{UrlRetorno}}/gi, vLink));

            $('#dialog_ScheduleAppointmentNotLogin').dialog();
        }
    }
};

/*Searchbox objext*/
var SearchBoxObject = {

    /*profile info*/
    InputId: '',
    CityId: '1',

    /*init meeting calendar variables*/
    Init: function (vInitObject) {
        this.InputId = vInitObject.InputId;
        this.CityId = vInitObject.CityId;
    },

    RenderAsync: function () {

        if ($('#' + SearchBoxObject.InputId).length > 0) {
            //init on enter event
            $('#' + SearchBoxObject.InputId).keyup(function (e) {
                if (e.keyCode == 13) {
                    SearchBoxObject.SearchTerm();
                }
            });

            //init autocomplete
            $('#' + SearchBoxObject.InputId).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '/api/SearchApi?IsAc=true&CityId=' + SearchBoxObject.CityId + '&SearchParam=' + request.term,
                        dataType: 'json',
                        type: "POST",
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                minLength: 2,
                focus: function (event, ui) {
                    return false;
                },
                select: function (event, ui) {
                    if (ui.item != null && ui.item.CurrentAcItem != null) {
                        $('#' + SearchBoxObject.InputId).val(ui.item.CurrentAcItem.Node);
                        SearchBoxObject.SearchTerm();
                    }
                    return false;
                },
                messages: {
                    noResults: '',
                    results: '',
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {

                var RenderItem = $('#' + SearchBoxObject.InputId + '_AcTemplate').html();
                RenderItem = RenderItem.replace(/\${Type}/gi, item.Type);
                RenderItem = RenderItem.replace(/\${Node}/gi, item.NodeSelected);

                return $("<li></li>")
                    .data("ui-autocomplete-item", item)
                    .append("<a>" + RenderItem + "</a>")
                    .appendTo(ul);
            };
        }
    },

    SearchTerm: function () {
        //find url to redirect
        fnEventTrack({
            category: 'MP.Header',
            action: 'MPHeader.Search.Click',
            label: $('#' + SearchBoxObject.InputId).val(),
        });
        window.location = '/doctores-' + $("#selGlobalCity option:selected").attr('itemname') + '/' + encodeURIComponent($('#' + SearchBoxObject.InputId).val().replace(/\./gi, '').trim());
    }
};

/*show hide user menu*/
function Header_ShowHideUserMenu(divId) {
    $('#' + divId).toggle('slow');
}

/*show hide login menu*/
function Header_ShowHideLoginMenu(ulLoginId) {
    $('#' + ulLoginId).toggle('slow');
}

/*Site tooltips*/
function fnInitToolTip(vSelector) {

    $(vSelector).tooltip({
        show: {
            effect: "slideDown",
            delay: 250
        },
    });
}

/*Google Event Tracking*/
function fnEventTrack(vEventInfo) {
    try {        
        _gaq.push(['_trackEvent', vEventInfo.category, vEventInfo.action, vEventInfo.label]);
    }
    catch (err) { }
}

