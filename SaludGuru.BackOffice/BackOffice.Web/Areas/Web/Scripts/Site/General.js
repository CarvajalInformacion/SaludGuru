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
            $("#ulNotificationList").toggle();
        });
    },
    TimerEvent: function () {
        $.ajax({
            url: "/api/NotificationApi",
            Type: "GET",
            dataType: "Json"
        }).done(function (data) {
            this.NotificationList = data;
            if (this.NotificationList != null && data != null && this.NotificationList.length != data.length) {
                this.NotificationList = data;
            }
            NotificationObject.RenderNotifications(this.NotificationList)
            //var oReturn
        });
    },

    RenderNotifications: function (NotificationList) {
        //set notification count background-image
        $('#aNotifyCount').html(NotificationList.length);
        if (NotificationList.length > 0) {
            $('#aNotifyCount').css('background-image', 'url(/Areas/Web/Content/Images/icono campana sonando.png)');
            //set notification alert icon
        }
        $("#ulNotificationList").html('');

        $.each(NotificationList, function (i, item) {
            //get html notification template                 
            var valSet = $("#NotificationTemplate").html();
            valSet = valSet.replace('{NotificationImage}', '/Areas/Web/Content/Images/NotificationType_' + item.NotificationType + '.png');
            valSet = valSet.replace('{NotificationText}', item.Body);
            //ulNotificationList
            $("#ulNotificationList").append(valSet);
        })
    },
};

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