/*init function view contact*/
function fnInitHome(vInitObject) {
    //init home image
    var count = vInitObject.CountMax;
    var img = Math.floor(Math.random() * count) + 1;
    var imageUrl = vInitObject.Baseurl.replace(/{{Count}}/gi, img);
    $(document).ready(function () {
        $('.SearchBox').css('background', 'url("' + imageUrl + '") no-repeat');
    });
}
