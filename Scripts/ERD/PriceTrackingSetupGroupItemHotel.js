$(document).ready(function () {
    $("#form0 > table > tbody > tr:odd").not(":hidden").addClass("row_odd");
    $("#form0 > table > tbody > tr:even").not(":hidden").addClass("row_even");
    $('.full-width #search_wrapper').css('height', 'auto');

    //Show DatePickers
    $('.enabledDate input[type="text"]').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    $('.deactivationDate input[type="text"]').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('.enabledDate input[type="text"]').val() == "") {
        $('.enabledDate input[type="text"]').val("No Enabled Date");
    }

    if ($('.deactivationDate input[type="text"]').val() == "") {
        $('.deactivationDate input[type="text"]').val("No Deactivation Date");
    }

    //ExchangesAllowedFlag
    var exchangesAllowedFlag = $('#ExchangesAllowedFlag').find('option:selected').val();
    if (exchangesAllowedFlag === "false") {
        $('#ExchangePreviousExchangeAllowedFlag').val("false").attr("disabled", true);
    } else {
        $('#ExchangePreviousExchangeAllowedFlag').val("").attr("disabled", false);
    }
});

//Shared Savings
$('#SharedSavingsFlag').change(function () {
    var selectedValue = $(this).find('option:selected').val();
    if (selectedValue === "true") {
        $('#TransactionFeeFlag').val("false");
        $('#TransactionFeeAmount').val("");
    } else {
        $('#TransactionFeeFlag').val("true");
        $('#SharedSavingsAmount').val("");
    }
});

// Transaction Fee
$('#TransactionFeeFlag').change(function () {
    var selectedValue = $(this).find('option:selected').val();
    if (selectedValue === "true") {
        $('#SharedSavingsFlag').val("false");
        $('#SharedSavingsAmount').val("");
    } else {
        $('#SharedSavingsFlag').val("true");
        $('#TransactionFeeAmount').val("");
    }
});

//CalculatedTotalThresholdAmount
$('#EstimatedCWTRebookingFeeAmount').change(function () {
    updateCalculatedTotalThresholdAmount();
});
$('#CWTVoidRefundFeeAmount').change(function () {
    updateCalculatedTotalThresholdAmount();
});
$('#ThresholdAmount').change(function () {
    updateCalculatedTotalThresholdAmount();
});

function updateCalculatedTotalThresholdAmount() {

    var estimatedCWTRebookingFeeAmount = $('#EstimatedCWTRebookingFeeAmount').val();
    if (estimatedCWTRebookingFeeAmount === '') {
        estimatedCWTRebookingFeeAmount = 0;
    }

    var cwtVoidRefundFeeAmount = $('#CWTVoidRefundFeeAmount').val();
    if (cwtVoidRefundFeeAmount === '') {
        cwtVoidRefundFeeAmount = 0;
    }

    var thresholdAmount = $('#ThresholdAmount').val();
    if (thresholdAmount === '') {
        thresholdAmount = 0;
    }
    var total = parseFloat(estimatedCWTRebookingFeeAmount) + parseFloat(cwtVoidRefundFeeAmount) + parseFloat(thresholdAmount);

    total = parseFloat(total).toFixed(2);

    $('#lblCalculatedTotalThresholdAmount').text(total);

}

//Hotel Tracking Alerts Email Add button
$('.HotelTrackingAlertsEmailAddresses_Line_Item .btn-add').live('click', function (e) {

    e.preventDefault();

    //Clone last row
    var lastItem = $('.HotelTrackingAlertsEmailAddresses_Line_Item').last().clone();

    //If last item isn't filled in, prompt to complete
    var check_field = lastItem.find('.HotelTrackingAlertsEmailAddress');
    if (check_field.val() === '') {
        alert('Please complete last field before adding a new one');
        return false;
    }

    //Validate Duplicates
    var validateHotelTrackingAlertsEmailAddresses = true;
    var hotelTrackingAlertsEmailAddresses = [];
    $('.HotelTrackingAlertsEmailAddress').each(function () {
        if (jQuery.inArray($(this).val(), hotelTrackingAlertsEmailAddresses) === -1) {
            hotelTrackingAlertsEmailAddresses.push($(this).val());
        } else {
            alert('The Hotel Tracking Alerts Email Addresses must be unique');
            validateHotelTrackingAlertsEmailAddresses = false;
            return false;
        }
    });

    //EmailAddresses must contain @ symbol
    $('.HotelTrackingAlertsEmailAddress').each(function () {
        var emailAddress = $(this).val();
        if (emailAddress.toLowerCase().indexOf("@") === -1) {
            alert('The @ sign is a required for Hotel Tracking Alerts Email');
            validateHotelTrackingAlertsEmailAddresses = false;
            return false;
        }
    });

    if (!validateHotelTrackingAlertsEmailAddresses) {
        return false;
    }

    //Proceed to and cloned row to end
    $('.HotelTrackingAlertsEmailAddresses_Line_Item').last().after(lastItem);

    //Select the last item
    var newItem = $('.HotelTrackingAlertsEmailAddresses_Line_Item').last();

    //Increment Id
    var regExp = /\[([^\]]+)\]/;
    var first_field = newItem.find('.HotelTrackingAlertsEmailAddress');
    var first_field_name = first_field.attr('name');
    var first_field_id = regExp.exec(first_field_name);
    var new_id = Number(first_field_id[1]) + 1;

    //EmailAddress
    var travelerTypeGuid = newItem.find('.HotelTrackingAlertsEmailAddress');
    travelerTypeGuid.attr('name', 'PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress[' + new_id + '].EmailAddress').val('');
});

//Hotel Tracking Alerts Email Remove btn
$('.HotelTrackingAlertsEmailAddresses_Line_Item .btn-remove').live('click', function (e) {

    e.preventDefault();

    //Remove all items but clear last remaining ones
    var lineItemCount = $('.HotelTrackingAlertsEmailAddresses_Line_Item').length;
    if (lineItemCount > 1) {
        $(this).closest('.HotelTrackingAlertsEmailAddresses_Line_Item').remove();
    } else {
        $(this).closest('.HotelTrackingAlertsEmailAddresses_Line_Item').find('.EmailAddress').val('');
    }

    //If removed a middle one, update all numbers
    for (var i = 0; i < $('.HotelTrackingAlertsEmailAddresses_Line_Item').length; i++) {

        var item = $('.HotelTrackingAlertsEmailAddresses_Line_Item:eq(' + i + ')');

        var new_id = i + 1;

        //EmailAddress
        var emailAddress = item.find('.EmailAddress');
        emailAddress.attr('name', 'PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress[' + new_id + '].EmailAddress');
    }
});

//Submit Form Validation
$('#form0').submit(function () {

    var validItem = true;

    //Reset Errors
    $('#lblSharedSavingsFlag').hide();
    $('#lblTransactionFeeFlag').hide();

    //Shared Savings
    var sharedSavingsFlag = $('#SharedSavingsFlag').val();
    if (sharedSavingsFlag === "true") {
        var sharedSavingsAmount = $('#SharedSavingsAmount').val();
        if (sharedSavingsAmount === "") {
            $('#lblSharedSavingsFlag').text('Shared Savings is required').addClass('field-validation-error').show();
            return false;
        }
    }

    //Transaction Fee
    var transactionFeeFlag = $('#TransactionFeeFlag').val();
    if (transactionFeeFlag === "true") {
        var transactionFeeAmount = $('#TransactionFeeAmount').val();
        if (transactionFeeAmount === "") {
            $('#lblTransactionFeeFlag').text('Transaction Fee is required').addClass('field-validation-error').show();
            return false;
        }
    }

    //HotelTrackingAlertsEmailAddresses - Check for duplicates and valid entries
    var validateHotelTrackingAlertsEmailAddresses = true;
    var hotelTrackingAlertsEmailAddresses = [];

    $('.HotelTrackingAlertsEmailAddress').each(function () {

        var emailAddress = $(this).val();

        if (emailAddress !== '') {

            //Reset errors
            $('.HotelTrackingAlertsEmailAddress').removeClass('field-validation-error');

            //EmailAddresses must contain @ symbol
            if (emailAddress.toLowerCase().indexOf("@") === -1) {
                alert('The @ sign is a required for Hotel Tracking Alerts Email');
                validateHotelTrackingAlertsEmailAddresses = false;
                return false;
            }

            //Validate Duplicates
            if (jQuery.inArray(emailAddress, hotelTrackingAlertsEmailAddresses) === -1) {
                hotelTrackingAlertsEmailAddresses.push(emailAddress);
            } else {
                $(this).addClass('field-validation-error');
                alert('The Hotel Tracking Alerts Email Addresses must be unique');
                validateHotelTrackingAlertsEmailAddresses = false;
                return false;
            }
        }
    });

    if (!validateHotelTrackingAlertsEmailAddresses) {
        return false;
    }

    if (!$(this).valid()) {
        return false;
    }

    if (validItem) {

        //Reset Date Fields
        if ($('#EnabledDate').val() == "No Enabled Date") {
            $('#EnabledDate').val("");
        }
        if ($('#DeactivationDate').val() == "No Deactivation Date") {
            $('#DeactivationDate').val("");
        }

        return true;
    } else {
        return false;
    }
});
