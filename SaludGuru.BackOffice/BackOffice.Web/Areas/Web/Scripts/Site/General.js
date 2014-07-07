/*manage date and time conversions*/
function serverDateToString(vDate) {
    return vDate.getFullYear() + '-' + ((new Number(vDate.getMonth())) + 1) + '-' + vDate.getDate();
}

function serverDateTimeToString(vDate) {
    return vDate.getFullYear() + '-' + ((new Number(vDate.getMonth())) + 1) + '-' + vDate.getDate() + 'T' + vDate.getHours() + ':' + vDate.getMinutes();
}

/*function start global pages controls*/
function InitGlobalPagesControls() {

    //init hover menu
    $(".admin").hover(function () {
        $(".subMenuAdministrar").fadeIn();
        $(".subMenuAgenda").fadeOut();
    });
    $(".agenda").hover(function () {
        $(".subMenuAgenda").fadeIn();
        $(".subMenuAdministrar").fadeOut();
    });

    //init autorization options menu
    InitAutorizationMenu();

    //init user notifications
    NotificationObject.InitUserNotifications();
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

    NotificationList: new Array(),

    InitUserNotifications: function () {
        setInterval(function () { this.TimerEvent() }, 300000);
    },

    TimerEvent: function () {
        $.Ajax({
            url: "test.html",
            Type: "GET",
            dataType: "Json"
        }).done(function (data) {


            //var oReturn
        })

        //NotificationObject.NotificationList
    },

    RenderNotifications: function () {
        //this.NotificationList

        //$('#aNotifyCount').html('5');
        //$('#ulNotificationList').html('hola mundo');
        //NotificationTemplate
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