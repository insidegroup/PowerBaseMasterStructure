$(document).ready(function () {
    $("tr:odd").not(":hidden").addClass("row_odd");
    $("tr:even").not(":hidden").addClass("row_even");
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

//RefundableToRefundablePreDepartureDayAmount
$('#RefundableToRefundablePreDepartureDayAmount').change(function () {
    var value = $(this).val();
    if (value !== "") {
        $('#RefundableToNonRefundablePreDepartureDayAmount').val(value);
    }
});

//ExchangesAllowedFlag
$('#ExchangesAllowedFlag').change(function () {
    var selectedValue = $(this).find('option:selected').val();
    if (selectedValue === "false") {
        $('#ExchangePreviousExchangeAllowedFlag').val("false").attr("disabled", true);
    } else {
        $('#ExchangePreviousExchangeAllowedFlag').val("").attr("disabled", false);
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

    if (!$(this).valid()) {
        return false;
    }

    if (validItem) {

        //Ensure ExchangePreviousExchangeAllowedFlag is submitted
        $('#ExchangePreviousExchangeAllowedFlag').attr("disabled", false);

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
    };
});
