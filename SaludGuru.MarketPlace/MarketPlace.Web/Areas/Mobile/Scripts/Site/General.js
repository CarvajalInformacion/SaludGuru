function InitGlobalPagesControlsMobile(InitParams) {
    //$('#' + InitParams.MenuButton).click(function () {
    //    $('#' + InitParams.DivEmergentMenu).show();

    //});
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

function InitDialogControlMobile(InitParams) {
    ScheduleAppointmentObject.Init({
        FBUrl: InitParams.FBUrl,
        GoogleUrl: InitParams.GoogleUrl,
        OutlookUrl: InitParams.OutlookUrl,
    });
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

    ScheduleAppointment: function (vLink, officePublicId, Date) {
        if (ScheduleAppointmentObject.IsLogin) {
            var CurrentSite = vLink;
            window.location = CurrentSite;
        }
        else {
            $.mobile.changePage(vLink, { transition: "pop", role: "dialog", id: "LoginDialog" });
        }
    },

    UrlReturn: function (vLink) {
        $('#dialog_NotLogin .MPFacebookLogIn').attr('href', ScheduleAppointmentObject.FBUrl.replace(/{{UrlRetorno}}/gi, vLink));
        $('#dialog_NotLogin .MPGoogleLogIn').attr('href', ScheduleAppointmentObject.GoogleUrl.replace(/{{UrlRetorno}}/gi, vLink));
        //$('#dialog_NotLogin .MPOutlookLogIn').attr('href', ScheduleAppointmentObject.OutlookUrl.replace(/{{UrlRetorno}}/gi, vLink));
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
        }
    },

    SearchTerm: function () {
        //find url to redirect
        window.location = '/doctores-' + $("#selGlobalCity option:selected").attr('itemname') + '/' + encodeURIComponent($('#' + SearchBoxObject.InputId).val().replace(/\./gi, '').trim());
    }
};
