/*init function view contact*/
function fnInitContactOffice() {
    $('.OfficeSelector').click(function () {
        $(this).find('.OfficeTextSelector').hide();
        $(this).find('.OfficeContactSelector').fadeIn('slow');
    });
}
