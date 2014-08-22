/*init function view contact*/
function fnInitContactOffice() {
    $('.OfficeSelector').click(function () {
        $(this).find('.OfficeTextSelector').hide();
        $(this).find('.OfficeContactSelector').fadeIn('slow');
    });
}

function fnInitCertifiedToolTip() {

    $('.SelCertifiedImage').tooltip({
        show: {
            effect: "slideDown",
            delay: 250
        },
    });
}


