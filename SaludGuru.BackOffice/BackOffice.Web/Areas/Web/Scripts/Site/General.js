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
    
    NotificationList:[],   

    InitUserNotifications: function () {
            setInterval(function () { NotificationObject.TimerEvent() }, 10000);        

        $('#aNotifyCount').click(function () {
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
            //debugger;
            if (this.NotificationList != null && data != null && this.NotificationList.length != data. length) {
                this.NotificationList = data;
            }           
            NotificationObject.RenderNotifications(this.NotificationList)
            //var oReturn
        });
        //NotificationObject.NotificationList
    },

    RenderNotifications: function (NotificationList) {
        //set notification count
        $('#aNotifyCount').html(NotificationList.length);
        if (NotificationList.length > 0) {
            //set notification alert icon
        }

        //set notification content
        
        //delete all current notifications
        //ulNotificationList
        $("#ulNotificationList").html('');

        $.each(NotificationList, function (i, item) {
            if (NotificationList != null) {
                //get html notification template                 
                var valSet = $("#NotificationTemplate").html();
                valSet = valSet.replace('{NotificationImage}', '~/Content/Images/facebookIconSmall.png');
                valSet = valSet.replace('{NotificationText}', NotificationList[i].Body);
                //ulNotificationList
                $("#ulNotificationList").append(valSet);      
            }
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