$(document).ready(function () {
    $('#menu_teams').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");


    /////////////////////////////////////////////////////////
    // BEGIN GDSCODE/PSEUDOCITYOROFFICEID SETUP
    /////////////////////////////////////////////////////////
    if ($("#GDSCode").val() == "ALL") {
        $("#PseudoCityOrOfficeId").val("DOES NOT APPLY");
        $("#PseudoCityOrOfficeId").attr("disabled", true);
        $("#errorPseudoCityOrOfficeId").text("");

        $("#GDSSignOn").val("DOES NOT APPLY");
        $("#GDSSignOn").attr("disabled", true);
    }
    if ($("#GDSSignOn").val() != "" && $("#GDSSignOn").val() != "DOES NOT APPLY") {
        $("#errorPseudoCityOrOfficeId").text(" *");
    }

    $("#GDSCode").change(function () {

        if ($("#GDSCode").val() == "ALL") {
            $("#PseudoCityOrOfficeId").val("DOES NOT APPLY");
            $("#PseudoCityOrOfficeId").attr("disabled", true);
            $("#errorPseudoCityOrOfficeId").text("");

            $("#GDSSignOn").val("DOES NOT APPLY");
            $("#GDSSignOn").attr("disabled", true);
        } else {
            if ($("#PseudoCityOrOfficeId").val() == "DOES NOT APPLY") {
                $("#PseudoCityOrOfficeId").val("");
            }
            $("#PseudoCityOrOfficeId").removeAttr("disabled");
            if ($("#GDSSignOn").val() == "DOES NOT APPLY") {
                $("#GDSSignOn").val("");
            }
            $("#GDSSignOn").removeAttr("disabled");
        }
    });

    $("#GDSSignOn").change(function () {
        if ($("#GDSSignOn").val() == "") {
            $("#errorPseudoCityOrOfficeId").text("");

        } else {
            $("#errorPseudoCityOrOfficeId").text(" *");
        }
    });

    $('input[id^="GDSSignOnId_"]').change(function () {
    alert("fsd")
    });

    /////////////////////////////////////////////////////////
    // END GDSCODE/PSEUDOCITYOROFFICEID SETUP
    /////////////////////////////////////////////////////////


    //Submit Form Validation
    $('#form0').submit(function () {
        var GDSSignOn = $("#GDSSignOn").val();
        var PseudoCityOrOfficeId = $("#PseudoCityOrOfficeId").val();

        if ((GDSSignOn != "" && GDSSignOn != "DOES NOT APPLY") && (PseudoCityOrOfficeId == "")) {
            $("#lblPseudoCityOrOfficeIdMsg").removeClass('field-validation-valid');
            $("#lblPseudoCityOrOfficeIdMsg").addClass('field-validation-error');
            $("#lblPseudoCityOrOfficeIdMsg").text("Home Pseudo City or Office ID is required.");
            return false;
        }

    });
});
