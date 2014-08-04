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
    debugger;
    var ApPatHtml = $('#ulPatientTemplate').html();
    ApPatHtml = ApPatHtml.replace(/{PatientPublicId}/gi, vPatientModel.PatientPublicId);
    ApPatHtml = ApPatHtml.replace(/{PatientName}/gi, vPatientModel.Name + " " + vPatientModel.LastName );
    $('#ulPatientList').append(ApPatHtml);
    $('#NewPatient').hide();
    $('#Name').val("");
    $('#LastName').val("");
    $('#Birthday').val("");
    $('#Identification').val("");
    $('#GenderFemale').prop("checked", false);
    $('#GenderMale').prop("checked", false);
}