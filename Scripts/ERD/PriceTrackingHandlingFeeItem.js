
$(document).ready(function() {

    //Navigation
    $('#menu_pricetracking').click();
    $("tr:odd").not(":hidden").addClass("row_odd");
    $("tr:even").not(":hidden").addClass("row_even");

    $('#PriceTrackingHandlingFeeItem_HandlingFee_Error').hide();

});
    
//Submit Form Validation
$('#form0').submit(function () {

    var validItem = false;

    $('#PriceTrackingHandlingFeeItem_HandlingFee_Error').hide();

    /*
    The Saving Amount Percentage and Handling Fee fields are mutually exclusive so only one or the other can be filled in, but not both.
    Either Saving Amount Percentage or Handling Fee must have a value else the system should provide an error: "Please enter either a Saving Amount Percentage or Handling Fee"
    */

    var priceTrackingHandlingFeeItem_SavingAmountPercentage = $('#PriceTrackingHandlingFeeItem_SavingAmountPercentage').val();
    var priceTrackingHandlingFeeItem_HandlingFee = $('#PriceTrackingHandlingFeeItem_HandlingFee').val();

    if (
        (priceTrackingHandlingFeeItem_SavingAmountPercentage != '' && priceTrackingHandlingFeeItem_HandlingFee != '') ||
        (priceTrackingHandlingFeeItem_SavingAmountPercentage == '' && priceTrackingHandlingFeeItem_HandlingFee == '')
    ) {
        $('#PriceTrackingHandlingFeeItem_HandlingFee_Error').show();
        return false;
    } else {
        validItem = true;
    }

    if (!$(this).valid()) {
        return false;
    }

    if (validItem) {
        return true;
    } else {
        return false
    };
});