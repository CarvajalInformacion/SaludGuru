/*profile slider render method*/
var ProfileSliderObject = {

    /*profile info*/
    JssorSlider: null,

    /*init meeting calendar variables*/
    Init: function (vInitObject) {
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
