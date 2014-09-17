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
    //ScheduleAppointmentObject.Init({
    //    FBUrl: InitParams.FBUrl,
    //    GoogleUrl: InitParams.GoogleUrl,
    //    OutlookUrl: InitParams.OutlookUrl,
    //    IsLogin: InitParams.IsLogin,
    //});

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
           
        }
    },

    SearchTerm: function () {
        //find url to redirect
        window.location = '/doctores-' + $("#selGlobalCity option:selected").attr('itemname') + '/' + encodeURIComponent($('#' + SearchBoxObject.InputId).val().replace(/\./gi, ''));
    }
};
