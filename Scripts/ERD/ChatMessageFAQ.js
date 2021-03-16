

$(document).ready(function () {

    //Navigation
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Submit Form Validation
    $('#form0').submit(function () {

        var validGroupName = false;

        var chatMessageFAQName = $("#ChatMessageFAQName").val();
        var chatMessageFAQId = $("#ChatMessageFAQId").val() != "" ? $("#ChatMessageFAQId").val() : 0;

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableChatMessageFAQName",
            data: { groupName: chatMessageFAQName, id: chatMessageFAQId },
            success: function (data) {
                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblChatMessageFAQNameMsg").removeClass('field-validation-valid');
            $("#lblChatMessageFAQNameMsg").addClass('field-validation-error');
            if ($("#ChatMessageFAQId").val() == "0") {//Create
                $("#lblChatMessageFAQNameMsg").text("This name has already been used, please choose a different name.");
            } else {
                if ($("#ChatMessageFAQName").val() != "") {
                    $("#lblChatMessageFAQNameMsg").text("This name has already been used, please choose a different name.");
                }
            }
            return false;
        } else {
            $("#lblChatMessageFAQNameMsg").text("");
        }

        if (!$(this).valid()) {
            return false;
        }
    });

});