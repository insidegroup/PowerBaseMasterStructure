$(document).ready(function () {

    //Navigation
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    
    //Submit Form Validation
    $('#form0').submit(function () {

        var validApprovalGroupApprovalTypeId = false;
        var validApprovalGroupApprovalTypeName = false;

        //Validate
        if(!$(this).valid()) {
            return false;
        }

        //Group Id
        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableApprovalGroupApprovalTypeId",
            data: {
                approvalGroupApprovalTypeId: $("#ApprovalGroupApprovalTypeId").val(),
                newApprovalGroupApprovalTypeId: $("#NewApprovalGroupApprovalTypeId").val() === undefined ? 0 : $("#NewApprovalGroupApprovalTypeId").val()
            },
            success: function(data) {
                validApprovalGroupApprovalTypeId = data;
            },
            dataType: "json",
            async: false
        });

        if(!validApprovalGroupApprovalTypeId) {
            $("#lblApprovalGroupIdMsg").removeClass('field-validation-valid');
            $("#lblApprovalGroupIdMsg").addClass('field-validation-error');
            $("#lblApprovalGroupIdMsg").text("This ID has already been used, please choose a different ID.");
            return false;
        } else {
            $("#lblApprovalGroupIdMsg").text("");
        }

        //Group Name
        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableApprovalGroupApprovalTypeName",
            data: {
                id: $("#ApprovalGroupApprovalTypeId").val(),
                groupName: $("#ApprovalGroupApprovalTypeDescription").val()
            },
            success: function(data) {
                validApprovalGroupApprovalTypeName = data;
            },
            dataType: "json",
            async: false
        });

        if(!validApprovalGroupApprovalTypeName) {
            $("#lblApprovalGroupNameMsg").removeClass('field-validation-valid');
            $("#lblApprovalGroupNameMsg").addClass('field-validation-error');
            $("#lblApprovalGroupNameMsg").text("This Approval Type has already been used, please enter a different name.");
            return false;
        } else {
            $("#lblApprovalGroupNameMsg").text("");
        }

        if(validApprovalGroupApprovalTypeId && validApprovalGroupApprovalTypeName) {
            return true;
        } else {
            return false;
        }
    });
});