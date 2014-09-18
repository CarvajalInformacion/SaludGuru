/*init function view contact*/
function fnInitContactOffice() {
    $('.OfficeSelector').click(function () {
        $(this).find('.OfficeTextSelector').hide();
        $(this).find('.OfficeContactSelector').fadeIn('slow');
    });
}

/*filter functions*/
function fnViewMoreLessFilter(vSelector) {
    if ($('#' + vSelector + '_a').length > 0) {

        if ($('#' + vSelector + '_a').text().trim() == 'Ver más') {
            $('.' + vSelector).fadeIn('slow');
            $('#' + vSelector + '_a').text('Ver menos');
        }
        else {
            $('.' + vSelector).fadeOut('slow');
            $('#' + vSelector + '_a').text('Ver más');
        }
    }
}

function fnFilter(vFilterUrl) {
    window.location = vFilterUrl;
}




