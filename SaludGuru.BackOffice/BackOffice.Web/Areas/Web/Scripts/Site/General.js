/*change profile*/
function Header_ChangeProfile(option) {
    window.location = option.value;
}
/*show hide user menu*/
function Header_ShowHideUserMenu(divId) {

    $('#' + divId).toggle('slow');
}