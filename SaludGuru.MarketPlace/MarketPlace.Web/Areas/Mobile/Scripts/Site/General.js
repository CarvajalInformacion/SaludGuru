function InitGlobalPagesControlsMobile(InitParams) {
    //debugger;
    //$('#' + InitParams.MenuButton).click(function () {
    //    debugger;
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
