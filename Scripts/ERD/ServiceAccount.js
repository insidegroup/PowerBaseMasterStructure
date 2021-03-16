$(document).ready(function() {
    //Navigation
	$('#menu_gdsmanagement').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

});

//Submit Form Validation
$('#form0').submit(function () {

    var valid = false;

    //ServiceAccountName
    var serviceAccount_ServiceAccountId = $("#ServiceAccount_ServiceAccountId").val() != "" ? $("#ServiceAccount_ServiceAccountId").val() : 0;
    var serviceAccount_ServiceAccountName = $("#ServiceAccount_ServiceAccountName").val();

    if (serviceAccount_ServiceAccountName != "") {
        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableServiceAccountName",
            data: {
                groupName: serviceAccount_ServiceAccountName,
                id: serviceAccount_ServiceAccountId
            },
            success: function (data) {
                valid = data;
            },
            dataType: "json",
            async: false
        });
        if (!valid) {
            $("#lblServiceAccountMsg").removeClass('field-validation-valid');
            $("#lblServiceAccountMsg").addClass('field-validation-error');
            if ($("#ServiceAccount_ServiceAccountId").val() == "0") {//Create
                $("#lblServiceAccountMsg").text("This name has already been used, please choose a different name.");
            } else {
                if ($("#ServiceAccount_ServiceAccountName").val() != "") {
                    $("#lblServiceAccountMsg").text("This name has already been used, please choose a different name.");
                }
            }
            return false;
        } else {
            $("#lblServiceAccountMsg").text("");
        }
    }

    //GroupName End
    if (!$(this).valid()) {
        return false;
    }

    if (valid) {
        return true;
    }
});