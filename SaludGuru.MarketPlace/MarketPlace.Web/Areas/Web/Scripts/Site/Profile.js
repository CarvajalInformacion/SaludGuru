/*profile office appointment render method*/
var ProfileAppointmentObject = {

    /*profile info*/
    DivId: '',
    lstOffice: new Array(),

    /*init meeting calendar variables*/
    Init: function (vInitObject) {
        //init render info
        this.DivId = vInitObject.DivId;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            //set key value pair for an office
            ProfileAppointmentObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    RenderAsync: function () {
        var oFirstRender = false;

        for (var item in this.lstOffice) {

            //get office public id
            this.lstOffice[item].OfficeDivId = this.DivId + '_' + this.lstOffice[item].OfficePublicId;
            //crete calendar menu item
            $('#' + this.DivId + '_Menu').append($('#' + this.DivId + '_Template_Menu').html().replace(/\${OfficePublicId}/gi, this.lstOffice[item].OfficePublicId).replace(/\${OfficeName}/gi, this.lstOffice[item].OfficeName));

            //create div to put a calendar
            $('#' + this.DivId).append($('#' + this.DivId + '_Template_Grid').html().replace(/\${OfficePublicId}/gi, this.lstOffice[item].OfficePublicId));
            //show office basic info
            var vOfficeInfo = $('#' + this.DivId + '_Template_OfficeInfo').html();
            vOfficeInfo = vOfficeInfo.replace(/\${Address}/gi, this.lstOffice[item].OfficeAddress);
            vOfficeInfo = vOfficeInfo.replace(/\${Telephone}/gi, this.lstOffice[item].OfficeTelephone);
            $('#divGrid_OfficeInfo_' + this.lstOffice[item].OfficePublicId).html(vOfficeInfo);

            if (oFirstRender == false) {
                this.RenderOfficeSchedule(this.lstOffice[item].OfficePublicId);
                oFirstRender = true;
            }
        }
    },

    RenderOfficeSchedule: function (vOfficePublicId) {
        var CurrentOfficeDiv = $('#divGrid_' + vOfficePublicId);
        if (CurrentOfficeDiv.length == 1) {
            if (CurrentOfficeDiv.children().length == 0) {
                //startup grid
                CurrentOfficeDiv.kendoGrid({
                    pageable: false,
                    height: 350,
                    dataSource: {
                        transport: {
                            read: function (options) {

                                var vStartDate = '';
                                var vNextAvailableDate = 'true';

                                if (options.data != null && options.data.NewDate != null) {
                                    vStartDate = options.data.NewDate;
                                }

                                if (options.data != null && options.data.NextAvailableDate != null) {
                                    vNextAvailableDate = options.data.NextAvailableDate;
                                }

                                $.ajax({
                                    url: '/api/ScheduleAvailableApi?ProfilePublicId=' + ProfileAppointmentObject.lstOffice[vOfficePublicId].ProfilePublicId + '&OfficePublicId=' + vOfficePublicId + '&TreatmentId=&NextAvailableDate=' + vNextAvailableDate + '&StartDateTime=' + vStartDate,
                                    dataType: "json",
                                    type: "POST",
                                    success: function (result) {
                                        var NewResult = new Array();
                                        //set header titles
                                        $.each(result, function (item, value) {
                                            if (value.Monday.IsHeader == true) {
                                                var HeaderHtml = $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Header').html();
                                                $('#' + ProfileAppointmentObject.DivId).find('[data-field=Monday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Monday.AvailableDateText));
                                                $('#' + ProfileAppointmentObject.DivId).find('[data-field=Tuesday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Tuesday.AvailableDateText));
                                                $('#' + ProfileAppointmentObject.DivId).find('[data-field=Wednesday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Wednesday.AvailableDateText));
                                                $('#' + ProfileAppointmentObject.DivId).find('[data-field=Thursday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Thursday.AvailableDateText));
                                                $('#' + ProfileAppointmentObject.DivId).find('[data-field=Friday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Friday.AvailableDateText));
                                                $('#' + ProfileAppointmentObject.DivId).find('[data-field=Saturday]').html(HeaderHtml.replace(/\${AvailableDateText}/, value.Saturday.AvailableDateText));
                                            }
                                            else {
                                                NewResult.push(value);
                                            }
                                        });
                                        options.success(NewResult);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                    }
                                });
                            }
                        },
                    },
                    columns: [{
                        field: 'Monday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Monday').html().replace(/{OfficePublicId}/gi, vOfficePublicId)
                    }, {
                        field: 'Tuesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Tuesday').html().replace(/{OfficePublicId}/gi, vOfficePublicId)
                    }, {
                        field: 'Wednesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Wednesday').html().replace(/{OfficePublicId}/gi, vOfficePublicId)
                    }, {
                        field: 'Thursday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Thursday').html().replace(/{OfficePublicId}/gi, vOfficePublicId)
                    }, {
                        field: 'Friday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Friday').html().replace(/{OfficePublicId}/gi, vOfficePublicId)
                    }, {
                        field: 'Saturday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Saturday').html().replace(/{OfficePublicId}/gi, vOfficePublicId)
                    }],
                    dataBound: function (e) {
                        //set header before render
                        var lstData = this.dataSource.view();

                        $('#spanHeader_' + vOfficePublicId).hide();
                        $('#divGrid_NotSchedule_' + vOfficePublicId).hide();

                        if (lstData != null && lstData.length > 0) {

                            $('#spanHeader_' + vOfficePublicId).html(lstData[0].HeaderTitle);

                            if (lstData[0].AfterDate != null && lstData[0].AfterDate.length > 0) {
                                $('#aAfter_' + vOfficePublicId).click(function () {
                                    ProfileAppointmentObject.PageMove(vOfficePublicId, lstData[0].AfterDate, 'false');
                                });
                                $('#aAfter_' + vOfficePublicId).show();
                            }
                            else {
                                $('#aAfter_' + vOfficePublicId).hide();
                            }

                            if (lstData[0].BeforeDate != null && lstData[0].BeforeDate.length > 0) {
                                $('#aBefore_' + vOfficePublicId).click(function () {
                                    ProfileAppointmentObject.PageMove(vOfficePublicId, lstData[0].BeforeDate, 'false');
                                });
                                $('#aBefore_' + vOfficePublicId).show();
                            }
                            else {
                                $('#aBefore_' + vOfficePublicId).hide();
                            }

                            if (lstData.length == 1 && lstData[0].CurrentDate != null && lstData[0].CurrentDate.length > 0) {

                                $('#divGrid_NotSchedule_' + vOfficePublicId).click(function () {
                                    ProfileAppointmentObject.PageMove(vOfficePublicId, lstData[0].CurrentDate, 'true');
                                });

                                $('#divGrid_NotSchedule_' + vOfficePublicId).show();
                            }

                            $('#spanHeader_' + vOfficePublicId).show();
                        }
                        else {

                            $('#divGrid_NotSchedule_' + vOfficePublicId).click(function () {
                                ProfileAppointmentObject.PageMove(vOfficePublicId, null, 'true');
                            });

                            $('#divGrid_NotSchedule_' + vOfficePublicId).show();
                        }
                        e.preventDefault();
                    }
                });
            }

            //show grid
            $('.SelOfficeGrid').hide();
            $('#divScheduleContainer_' + vOfficePublicId).fadeIn('slow');

            //select de current menu
            $('.SelOfficeMenu').removeClass('selected');
            $('#li_' + vOfficePublicId).addClass('selected');
        }
    },

    PageMove: function (vOfficePublicId, vNewDate, vNextAvailableDate) {
        $('#divGrid_' + vOfficePublicId).data("kendoGrid").dataSource.read({
            NewDate: vNewDate,
            NextAvailableDate: vNextAvailableDate,
        });
    },
};


/*profile office appointment render method*/
var ProfileMapObject = {

    /*profile info*/
    DivId: '',
    CenterMap: '0,0',
    lstOffice: new Array(),

    /*init variables*/
    Init: function (vInitObject) {
        //init render info
        this.DivId = vInitObject.DivId;
        this.CenterMap = vInitObject.CenterMap;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            //set key value pair for an office
            ProfileMapObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    RenderAsync: function () {
        //start map
        $('#' + ProfileMapObject.DivId).gmap({
            'center': ProfileMapObject.CenterMap,
            'zoom': 12,
            'disableDefaultUI': true,
        });

        for (var item in this.lstOffice) {
            //get tool tip for office
            var oToolTip = $('#OfficeToolTip_' + ProfileMapObject.DivId).html();
            oToolTip = oToolTip.replace(/<img src=""/gi, '<img src="' + ProfileMapObject.lstOffice[item].ProfileImage + '"');
            oToolTip = oToolTip.replace(/\${OfficeName}/gi, ProfileMapObject.lstOffice[item].OfficeName);
            oToolTip = oToolTip.replace(/\${Address}/gi, ProfileMapObject.lstOffice[item].Address);
            oToolTip = oToolTip.replace(/\${Telephone}/gi, ProfileMapObject.lstOffice[item].Telephone);

            $('#' + ProfileMapObject.DivId).gmap('addMarker', {
                'position': ProfileMapObject.lstOffice[item].Geolocation,
                //'height': '10px'
            }).click(function () {
                $('#' + ProfileMapObject.DivId).gmap('openInfoWindow',
                    {
                        'content': oToolTip
                    }, this);
            });
        }
    },
};