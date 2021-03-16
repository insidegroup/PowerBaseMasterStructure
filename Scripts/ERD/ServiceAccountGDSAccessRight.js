$(document).ready(function () {

    //Navigation
    $('#menu_gdsmanagement').click();

    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    $('#breadcrumb').css('width', 'auto');

    //Show DatePickers
    $('#ServiceAccountGDSAccessRight_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#ServiceAccountGDSAccessRight_EnabledDate').val() == "" || $('#ServiceAccountGDSAccessRight_EnabledDate').val() == "1/1/0001 12:00:00 AM") {
        $('#ServiceAccountGDSAccessRight_EnabledDate').val("No Enabled Date")
    }

    //The Home PCC/Office ID is not active until a GDS has been selected, unless the value selected is “All GDS Systems”, in which case the Home PCC/Office ID drop list remains inactive.

    if ($('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId').val() == "") {
    	updatePseudoCityOrOfficeId();
    }

    $('#ServiceAccountGDSAccessRight_GDSCode').change(function () {
        updatePseudoCityOrOfficeId();
    });

    function updatePseudoCityOrOfficeId() {
        var value = $('#ServiceAccountGDSAccessRight_GDSCode').val();
        if (value != "" && value != "ALL") {
            $('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId').attr('disabled', false);
            $('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId_Error').show();            
            $('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId').empty().append('<option value="">Please Select...</option>');
            LoadPseudoCityOrOfficeIds();
        } else {
            $('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId').attr('disabled', true);
            $('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId').empty().append('<option value="">Please Select...</option>');
            $('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId_Error').hide();
        }
    }

    function LoadPseudoCityOrOfficeIds() {

    	var gdsCode = $('#ServiceAccountGDSAccessRight_GDSCode').val();
    	var serviceAccountId = $('#ServiceAccountGDSAccessRight_ServiceAccountId').val();

    	jQuery.ajax({
            type: "POST",
            url: "/ServiceAccount.mvc/GetServiceAccountPseudoCityOrOfficeIdsByGDSCode",
            data: {
            	gdsCode: gdsCode,
            	serviceAccountId : serviceAccountId
			},
            success: function (data) {
                $.each(data, function (index, item) {
                    $("#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId").append(
						$("<option></option>")
							.text(item.PseudoCityOrOfficeId)
							.val(item.PseudoCityOrOfficeId)
					);
                });

            },
            dataType: "json",
            async: false
        });
    }

	//GDS Access Type is not active until a GDS has been selected, unless the value selected is "All GDS Systems",
	// in which case the GDS Access Type drop list remains inactive.
    $('#lblServiceAccountGDSAccessRightPseudoCityOrOfficeIdMsg').hide();

    //Edit
	if ($('#ServiceAccountGDSAccessRight_GDSAccessTypeId').val() == "") {
    updateGDSAccessTypeId();
	}

    $('#ServiceAccountGDSAccessRight_GDSCode').change(function () {
        updateGDSAccessTypeId();
    });

    function updateGDSAccessTypeId() {
        var value = $('#ServiceAccountGDSAccessRight_GDSCode').val();
        if (value != "" && value != "ALL") {
            $('#ServiceAccountGDSAccessRight_GDSAccessTypeId').attr('disabled', false);
                $('#ServiceAccountGDSAccessRight_GDSAccessTypeId').empty().append('<option value="">Please Select...</option>');
                LoadGDSAccessTypeIds();
        } else {
            $('#ServiceAccountGDSAccessRight_GDSAccessTypeId').empty().append('<option value="">Please Select...</option>');
            $('#ServiceAccountGDSAccessRight_GDSAccessTypeId').attr('disabled', true);
        }
    }

    function LoadGDSAccessTypeIds() {
        var gdsCode = $('#ServiceAccountGDSAccessRight_GDSCode').val();
        jQuery.ajax({
            type: "POST",
            url: "/GDSAccessType.mvc/GetGDSAccessTypesByGDSCode",
            data: { gdsCode: gdsCode },
            success: function (data) {
                $.each(data, function (index, item) {
                    $("#ServiceAccountGDSAccessRight_GDSAccessTypeId").append(
						$("<option></option>")
							.text(item.GDSAccessTypeName)
							.val(item.GDSAccessTypeId)
					);
                });

            },
            dataType: "json",
            async: false
        });
    }


});

//Submit Form Validation
$('#form0').submit(function () {

    var valid = false;

    //IsAvailableServiceAccountGDSAccessRightGDSSignOnID
    jQuery.ajax({
    	type: "POST",
        url: "/GroupNameBuilder.mvc/IsAvailableGDSAccessRightGDSSignOnID",
    	data: {
    		gdsSignOnID: $("#ServiceAccountGDSAccessRight_GDSSignOnID").val(),
    		pseudoCityOrOfficeId: $("#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId").val(),
            id: $("#ServiceAccountGDSAccessRight_ServiceAccountGDSAccessRightId").length > 0 ? $("#ServiceAccountGDSAccessRight_ServiceAccountGDSAccessRightId").val() : 0,
            groupName: "ServiceAccount"
    	},
    	success: function (data) {
    		valid = data;
    	},
    	dataType: "json",
    	async: false
    });

    if (!valid) {
    	$("#lblServiceAccountGDSAccessRightMsg").removeClass('field-validation-valid');
    	$("#lblServiceAccountGDSAccessRightMsg").addClass('field-validation-error');
    	$("#lblServiceAccountGDSAccessRightMsg").text("This GDS Sign On ID is already in use for the selected Home PCC/Office ID.");
    	return false;
    } else {
    	$("#lblServiceAccountGDSAccessRightMsg").text("");
    }

    if (!$(this).valid()) {
        return false;
    }

    //PCC
    var selected_GDS = $('#ServiceAccountGDSAccessRight_GDSCode').val();
    if (selected_GDS != "" && selected_GDS != "ALL") {
        var selected_PCC = $('#ServiceAccountGDSAccessRight_PseudoCityOrOfficeId').val();
        if (selected_PCC == '') {
            $('#lblServiceAccountGDSAccessRightPseudoCityOrOfficeIdMsg').show();
            return false;
        }
    }

    if (valid) {
        if ($('#ServiceAccountGDSAccessRight_EnabledDate').val() == "No Enabled Date") {
            $('#ServiceAccountGDSAccessRight_EnabledDate').val("");
        }
        return true;
    }
});