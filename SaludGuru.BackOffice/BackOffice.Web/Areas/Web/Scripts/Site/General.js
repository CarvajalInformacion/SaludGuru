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
function InitGlobalPagesControls(InitModel) {

    //init autorization options menu
    InitAutorizationMenu();

    //init user notifications
    NotificationObject.InitUserNotifications();

    //init search box
    InitGlobalSearchBox(InitModel.IsUserAdmin);

    InitSearchMagGlass(InitModel.IsUserAdmin);
}

/*init autorization menu*/
function InitAutorizationMenu() {

    //override render core item function
    $.widget("custom.iconselectmenu", $.ui.selectmenu, {
        _renderItem: function (ul, item) {
            var li = $("<li>", { text: item.label });
            if (item.disabled) {
                li.addClass("ui-state-disabled");
            }
            $("<span>", {
                style: item.element.attr("data-style"),
                "class": "ui-icon " + item.element.attr("data-class")
            }).appendTo(li);
            return li.appendTo(ul);
        }
    });

    //start selected custom menu
    $('#ddAutorizationProfiles').iconselectmenu({
        width: 200,
        select: function (event, ui) {
            Header_ChangeProfile(ui.item.value);
        }
    });

    //set selected item html
    $('#ddAutorizationProfiles-button .ui-selectmenu-text').html($('#SelAutorization').html());
}

/*init user notifications*/
var NotificationObject = {

    NotificationList: [],

    InitUserNotifications: function () {
        NotificationObject.TimerEvent();
        setInterval(function () { NotificationObject.TimerEvent() }, 50000);

        $('#notificationsWrapper').click(function () {
            $("#ulNotificationList").toggle("slow");
        });
    },

    TimerEvent: function () {
        $.ajax({
            url: "/api/NotificationApi",
            Type: "POST",
            dataType: "Json"
        }).done(function (data) {

            if (data != null && NotificationObject.NotificationList.length != data.length) {
                NotificationObject.NotificationList = data;
                NotificationObject.RenderNotifications();
            }

            //var oReturn
        }).error(function (jqXHR, textStatus, errorThrown) {
        });
    },

    RenderNotifications: function () {
        if (NotificationObject.NotificationList != null ) {

            //set notification count background-image
            $('#aNotifyCount').html(NotificationObject.NotificationList.length);

            $("#ulNotificationList").html('');

            $.each(NotificationObject.NotificationList, function (i, item) {
                debugger;
                //get html notification template                 
                var valSet = $("#NotificationTemplate").html();
                valSet = valSet.replace('{NotificationImage}', '/Areas/Web/Content/Images/NotificationType_' + item.NotificationType + '.png');
                valSet = valSet.replace('{NotificationText}', item.Title);
                valSet = valSet.replace('{NotificationId}', item.NotificationId);
                
                $("#ulNotificationList").append(valSet);

            });
        }
    },
};
function ReadNotification_OnMouseOver(NotificationId) {
    debugger;
    $.ajax({        
        url: '/api/NotificationApi?NotificationId=' + NotificationId,
        Type: "POST",
        dataType: "Json"
    }).done(function (data) {
        debugger;
        if (data != null && NotificationObject.NotificationList.length != data.length) {
            debugger;
            NotificationObject.NotificationList = data;
            NotificationObject.RenderNotifications();
        }
        else {
            debugger;
            NotificationObject.NotificationList = data;
            NotificationObject.RenderNotifications();
        }
        //var oReturn
    }).error(function (jqXHR, textStatus, errorThrown) {
    });    
}

/*change profile*/
function Header_ChangeProfile(urlToChange) {
    window.location = urlToChange;
}
/*show hide user menu*/
function Header_ShowHideUserMenu(divId) {
    
    $('#' + divId).toggle('slow');
}

/*init search box*/
function InitGlobalSearchBox(IsUserAdmin) {

    if (IsUserAdmin == true) {
        $('#ipGlobalSearchBox').keyup(function (e) {
            if (e.keyCode == 13) {
                window.location = '/Profile/ProfileSearch?SearchParam=' + $('#ipGlobalSearchBox').val();
            }
        });
    }
    else {
        $('#ipGlobalSearchBox').keyup(function (e) {
            if (e.keyCode == 13) {
                window.location = '/Patient/Search?SearchParam=' + $('#ipGlobalSearchBox').val();
            }
        });
    }
}

function InitSearchMagGlass(IsUserAdmin) {
    if (IsUserAdmin == true) {
        $('#searchMagGlass').click(function () {
            window.location = '/Profile/ProfileSearch?SearchParam=' + $('#ipGlobalSearchBox').val();
        });
    }
    else {
        $('#searchMagGlass').click(function () {
            window.location = '/Patient/Search?SearchParam=' + $('#ipGlobalSearchBox').val();
        });
    }
}

/*Error popup validation*/
function ValidatePopUp(controlName, message, NameSubmit) {
    //
    $("#dialogError").dialog({
        show: {
            effect: "clip",
            duration: 500,
            title: "Error"
        },
        hide: {
            effect: "blind",
            duration: 500,
            title: "Error"
        }
    });
    $("#dialogError").text(message);
    $("#dialogError").dialog("open");
}


/*calendar render method*/
var GeneralCalendarObject = {
    /*calendar info*/
    DivId: '',
    CountryId: '',
    ProfilePublicId: '',
    StartDate: new Date(),
    EndDate: new Date(),
    FirstDate: new Date(),
    SecondDate: new Date(),

    /*init meeting calendar variables*/
    Init: function (vInitObject) {

        this.DivId = vInitObject.DivId;
        this.CountryId = vInitObject.CountryId;
        this.ProfilePublicId = vInitObject.ProfilePublicId;
        this.StartDate = vInitObject.StartDate;
        this.EndDate = vInitObject.EndDate;
        this.FirstDate = vInitObject.FirstDate;
    },

    RenderAsync: function () {
        //make ajax for special days
        $.ajax({
            type: "POST",
            url: '/api/Calendar?CountryId=' + this.CountryId + '&ProfilePublicId=' + this.ProfilePublicId,
        }).done(function (data) {
            //left date picker

            GeneralCalendarObject.RenderCalendar(data, GeneralCalendarObject.FirstDate);

        }).fail(function () {
            alert("se ha generado un error en el calendario");
        });
    },

    RenderCalendar: function (vlstSpecialDay, vCurrentDate) {

        //render calendar
        $('#' + this.DivId).datepicker({
            dateFormat: 'yy-mm-dd',
            locale: $.datepicker.regional['es'],
            defaultDate: vCurrentDate,
            //numberOfMonths: [1,2],
            beforeShowDay: function (date) {

                var oReturn = [true, ''];

                //eval selected date
                if (GeneralCalendarObject.GetNumberDate(date) >= GeneralCalendarObject.GetNumberDate(GeneralCalendarObject.StartDate) && GeneralCalendarObject.GetNumberDate(date) < GeneralCalendarObject.GetNumberDate(GeneralCalendarObject.EndDate)) {
                    oReturn = [true, ' selected'];
                }

                //eval special day
                if (vlstSpecialDay != null) {
                    $(vlstSpecialDay).each(function (index, value) {
                        if (value.Year == date.getFullYear() && value.Month == (date.getMonth() + 1) && value.Day == date.getDate()) {
                            oReturn = [true, 'specialDay_' + value.SpecialDayType];
                        }
                    });
                }
                return oReturn;
            },           
        });
        //delete selected style on calendar
        $('#' + this.DivId + ' .ui-datepicker-current-day').removeClass('ui-datepicker-days-cell-over ui-datepicker-current-day');
        $('#' + this.DivId + ' .ui-state-active').removeClass('ui-state-active ui-state-hover');
    },

    GetNumberDate: function (vDateToEval) {

        var oReturn = '';

        oReturn = vDateToEval.getFullYear();

        if (vDateToEval.getMonth() < 10) {
            oReturn = oReturn + '0' + (vDateToEval.getMonth()).toString();
        }
        else {
            oReturn = oReturn + (vDateToEval.getMonth()).toString();
        }

        if (vDateToEval.getDate() < 10) {
            oReturn = oReturn + '0' + (vDateToEval.getDate()).toString();
        }
        else {
            oReturn = oReturn + (vDateToEval.getDate()).toString();
        }

        return oReturn;
    },

    Refresh: function () {

        $('#' + this.DivId).datepicker("refresh");

    },

};