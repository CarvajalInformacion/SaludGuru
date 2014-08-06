function CreatePatient() {
    debugger;
    //create ajax form object
    $("#CreatePatientForm").submit(function (e) {
        var postData = $(this).serializeArray();
        var formURL = $(this).attr("action");
        $.ajax(
        {
            url: formURL,
            type: "POST",
            data: postData,
            success: function (data, textStatus, jqXHR) {
                debugger;
                AddPatientToList(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //if fails      
            }
        });
        e.preventDefault(); //STOP default action
        e.unbind(); //unbind. to stop multiple form submit.
    });

    $("#CreatePatientForm").submit(); //Submit  the FORM
}

function AddPatientToList(vPatientModel) {
    var ApPatHtml = $('#ulPatientTemplate').html();
    ApPatHtml = ApPatHtml.replace(/{PatientPublicId}/gi, vPatientModel.PatientPublicId);
    ApPatHtml = ApPatHtml.replace(/{PatientName}/gi, vPatientModel.Name + " " + vPatientModel.LastName);
    $('#ulPatientList').append(ApPatHtml);
    $('#NewPatient').hide();
    $('#Name').val("");
    $('#LastName').val("");
    $('#Birthday').val("");
    $('#Identification').val("");
    $('#GenderFemale').prop("checked", false);
    $('#GenderMale').prop("checked", false);
    $('#SelectedItem').prop("checked", false);

}

/*profile office appointment render method*/
var AppointmentObject = {

    /*profile info*/
    DivId: '',
    lstOffice: new Array(),

    /*init meeting calendar variables*/
    APInit: function (vInitObject) {
        //init render info
        this.DivId = vInitObject.DivId;

        //init office info object array
        $.each(vInitObject.OfficeInfo, function (index, value) {
            //set key value pair for an office
            AppointmentObject.lstOffice[value.OfficePublicId] = value;
        });
    },

    APRenderAsync: function () {
        var oFirstRender = false;

        for (var item in this.lstOffice) {

            //get office public id
            this.lstOffice[item].OfficeDivId = this.DivId + '_' + this.lstOffice[item].OfficePublicId;

            //crete calendar menu item
            $('#' + this.DivId + '_Menu').append($('#' + this.DivId + '_Template_Menu').html().replace(/\${OfficePublicId}/gi, this.lstOffice[item].OfficePublicId).replace(/\${OfficeName}/gi, this.lstOffice[item].OfficeName));

            //create div to put a calendar
            $('#' + this.DivId).append($('#' + this.DivId + '_Template_Grid').html().replace(/\${OfficePublicId}/gi, this.lstOffice[item].OfficePublicId));

            if (oFirstRender == false) {
                this.APRenderOfficeSchedule(this.lstOffice[item].OfficePublicId);
                oFirstRender = true;
            }
        }
    },

    APRenderOfficeSchedule: function (vOfficePublicId) {
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
                                    url: '/api/ScheduleAvailableApi?ProfilePublicId=' + AppointmentObject.lstOffice[vOfficePublicId].ProfilePublicId + '&OfficePublicId=' + vOfficePublicId + '&TreatmentId=&NextAvailableDate=' + vNextAvailableDate + '&StartDateTime=' + vStartDate,
                                    dataType: "json",
                                    type: "POST",
                                    success: function (result) {
                                        options.success(result);
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
                        template: $('#' + AppointmentObject.DivId + '_Template_Grid_Event_Monday').html()
                    }, {
                        field: 'Tuesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivId + '_Template_Grid_Event_Tuesday').html()
                    }, {
                        field: 'Wednesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivId + '_Template_Grid_Event_Wednesday').html()
                    }, {
                        field: 'Thursday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivId + '_Template_Grid_Event_Thursday').html()
                    }, {
                        field: 'Friday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivId + '_Template_Grid_Event_Friday').html()
                    }, {
                        field: 'Saturday',
                        title: ' ',
                        width: 100,
                        template: $('#' + AppointmentObject.DivId + '_Template_Grid_Event_Saturday').html()
                    }, {

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
                                    AppointmentObject.PageMove(vOfficePublicId, lstData[0].AfterDate, 'false');
                                });
                                $('#aAfter_' + vOfficePublicId).show();
                            }
                            else {
                                $('#aAfter_' + vOfficePublicId).hide();
                            }

                            if (lstData[0].BeforeDate != null && lstData[0].BeforeDate.length > 0) {
                                $('#aBefore_' + vOfficePublicId).click(function () {
                                    AppointmentObject.PageMove(vOfficePublicId, lstData[0].BeforeDate, 'false');
                                });
                                $('#aBefore_' + vOfficePublicId).show();
                            }
                            else {
                                $('#aBefore_' + vOfficePublicId).hide();
                            }

                            if (lstData.length == 1 && lstData[0].CurrentDate != null && lstData[0].CurrentDate.length > 0) {

                                $('#divGrid_NotSchedule_' + vOfficePublicId).click(function () {
                                    AppointmentObject.PageMove(vOfficePublicId, lstData[0].CurrentDate, 'true');
                                });

                                $('#divGrid_NotSchedule_' + vOfficePublicId).show();
                            }

                            $('#spanHeader_' + vOfficePublicId).show();
                        }
                        else {

                            $('#divGrid_NotSchedule_' + vOfficePublicId).click(function () {
                                AppointmentObject.PageMove(vOfficePublicId, null, 'true');
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

