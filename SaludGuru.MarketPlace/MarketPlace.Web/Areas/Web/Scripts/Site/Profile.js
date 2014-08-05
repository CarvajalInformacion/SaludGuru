/*profile slider render method*/
var ProfileSliderObject = {

    /*profile info*/
    JssorSlider: null,

    /*init meeting calendar variables*/
    Init: function () {
    },

    RenderAsync: function () {

        var options = {
            $AutoPlay: true,
            $AutoPlaySteps: 1,
            $AutoPlayInterval: 10000,
            $PauseOnHover: 3,

            $ArrowKeyNavigation: true,
            $SlideDuration: 750,
            $MinDragOffsetToSlide: 20,
            $SlideSpacing: 3,
            $DisplayPieces: 1,
            $ParkingPosition: 0,
            $UISearchMode: 1,
            $PlayOrientation: 1,
            $DragOrientation: 1,

            $ThumbnailNavigatorOptions: {
                $Class: $JssorThumbnailNavigator$,
                $ChanceToShow: 2,

                $Loop: 2,
                $AutoCenter: 3,
                $Lanes: 1,
                $SpacingX: 2,
                $SpacingY: 2,
                $DisplayPieces: 4,
                $ParkingPosition: 0,
                $Orientation: 2,
                $DisableDrag: false
            }
        };

        //start slider
        this.JssorSlider = new $JssorSlider$('divProfileSlide', options);

        //scale slider
        this.ScaleSlider();

        //register responsive function
        if (!navigator.userAgent.match(/(iPhone|iPod|iPad|BlackBerry|IEMobile)/)) {
            $(window).bind('resize', ProfileSliderObject.ScaleSlider);
        }
    },

    //responsive code begin
    ScaleSlider: function () {
        var parentWidth = ProfileSliderObject.JssorSlider.$Elmt.parentNode.clientWidth;
        if (parentWidth) {
            var sliderWidth = parentWidth;

            //keep the slider width no more than 810
            sliderWidth = Math.min(sliderWidth, 810);

            ProfileSliderObject.JssorSlider.$SetScaleWidth(sliderWidth);
        }
        else {
            window.setTimeout(ScaleSlider, 500);
        }
    },
};

/*profile detail render method*/
var ProfileDetailObject = {

    /*profile info*/
    ProfileTextId: '',
    ProfileEducationId: '',
    ProfileCertificationId: '',

    /*init meeting calendar variables*/
    Init: function (vInitObject) {
        this.ProfileTextId = vInitObject.ProfileTextId;
        this.ProfileEducationId = vInitObject.ProfileEducationId;
        this.ProfileCertificationId = vInitObject.ProfileCertificationId;
    },

    RenderAsync: function () {

        var IsRender = false;

        if ($('#' + ProfileDetailObject.ProfileTextId).length > 0) {

            $('#' + ProfileDetailObject.ProfileTextId + '_sel').click(function () {

                $('#' + ProfileDetailObject.ProfileEducationId).hide();
                $('#' + ProfileDetailObject.ProfileCertificationId).hide();
                $('#' + ProfileDetailObject.ProfileTextId).fadeIn('slow');
            });

            $('#' + ProfileDetailObject.ProfileTextId + '_a').html('Ver más');

            $('#' + ProfileDetailObject.ProfileTextId + '_a').click(function () {

                if ($('#' + ProfileDetailObject.ProfileTextId).height() == 90) {

                    $('#' + ProfileDetailObject.ProfileTextId).animate({ height: '100%' });
                    $(this).html('Ver menos');
                }
                else {
                    $('#' + ProfileDetailObject.ProfileTextId).animate({ height: '90px' });
                    $(this).html('Ver más');
                }
            });

            if (IsRender == false) {
                $('#' + ProfileDetailObject.ProfileTextId).show();
                IsRender = true;
            }
            else {
                $('#' + ProfileDetailObject.ProfileTextId).hide();
            }
        }

        if ($('#' + ProfileDetailObject.ProfileEducationId).length > 0) {

            $('#' + ProfileDetailObject.ProfileEducationId + '_sel').click(function () {

                $('#' + ProfileDetailObject.ProfileTextId).hide();
                $('#' + ProfileDetailObject.ProfileCertificationId).hide();
                $('#' + ProfileDetailObject.ProfileEducationId).fadeIn('slow');
            });

            $('#' + ProfileDetailObject.ProfileEducationId + '_a').html('Ver más');

            $('#' + ProfileDetailObject.ProfileEducationId + '_a').click(function () {

                if ($('#' + ProfileDetailObject.ProfileEducationId).height() == 90) {

                    $('#' + ProfileDetailObject.ProfileEducationId).animate({ height: '100%' });
                    $(this).html('Ver menos');
                }
                else {
                    $('#' + ProfileDetailObject.ProfileEducationId).animate({ height: '90px' });
                    $(this).html('Ver más');
                }
            });

            if (IsRender == false) {
                $('#' + ProfileDetailObject.ProfileEducationId).show();
                IsRender = true;
            }
            else {
                $('#' + ProfileDetailObject.ProfileEducationId).hide();
            }
        }

        if ($('#' + ProfileDetailObject.ProfileCertificationId).length > 0) {

            $('#' + ProfileDetailObject.ProfileCertificationId + '_sel').click(function () {
                $('#' + ProfileDetailObject.ProfileTextId).hide();
                $('#' + ProfileDetailObject.ProfileEducationId).hide();
                $('#' + ProfileDetailObject.ProfileCertificationId).fadeIn('slow');
            });

            $('#' + ProfileDetailObject.ProfileCertificationId + '_a').html('Ver más');

            $('#' + ProfileDetailObject.ProfileCertificationId + '_a').click(function () {

                if ($('#' + ProfileDetailObject.ProfileCertificationId).height() == 90) {

                    $('#' + ProfileDetailObject.ProfileCertificationId).animate({ height: '100%' });
                    $(this).html('Ver menos');
                }
                else {
                    $('#' + ProfileDetailObject.ProfileCertificationId).animate({ height: '90px' });
                    $(this).html('Ver más');
                }
            });

            if (IsRender == false) {
                $('#' + ProfileDetailObject.ProfileCertificationId).show();
                IsRender = true;
            }
            else {
                $('#' + ProfileDetailObject.ProfileCertificationId).hide();
            }
        }
    },
};


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
                                $.ajax({
                                    url: '/api/ScheduleAvailableApi?ProfilePublicId=' + ProfileAppointmentObject.lstOffice[vOfficePublicId].ProfilePublicId + '&OfficePublicId=' + vOfficePublicId + '&TreatmentId=&NextAvailableDate=true&StartDateTime=',
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
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Monday').html()
                    }, {
                        field: 'Tuesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Tuesday').html()
                    }, {
                        field: 'Wednesday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Wednesday').html()
                    }, {
                        field: 'Thursday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Thursday').html()
                    }, {
                        field: 'Friday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Friday').html()
                    }, {
                        field: 'Saturday',
                        title: ' ',
                        width: 100,
                        template: $('#' + ProfileAppointmentObject.DivId + '_Template_Grid_Event_Saturday').html()
                    }, {

                    }]
                });
            }

            //show grid
            $('.SelOfficeGrid').hide();
            CurrentOfficeDiv.fadeIn('slow');

            //select de current menu
            $('.SelOfficeMenu').removeClass('selected');
            $('#li_' + vOfficePublicId).addClass('selected');
        }
    },
};