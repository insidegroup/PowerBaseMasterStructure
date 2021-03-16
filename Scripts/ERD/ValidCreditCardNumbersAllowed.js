$(function () {

    $.validator.addMethod("creditcard", function (value, element) {
        if (this.optional(element))
            return "dependency-mismatch";
        // accept only digits and dashes
        if (/[^0-9-]+/.test(value))
            return false;
        var nCheck = 0,
				nDigit = 0,
				bEven = false;

        value = value.replace(/\D/g, "");

        if (value.length < 6) {
            return false;
        }

        for (var n = value.length - 1; n >= 0; n--) {
            var cDigit = value.charAt(n);
            var nDigit = parseInt(cDigit, 10);
            if (bEven) {
                if ((nDigit *= 2) > 9)
                    nDigit -= 9;
            }
            nCheck += nDigit;
            bEven = !bEven;
        }

        if ($("#CreditCard_WarningShownFlag").val() == "False") {
            $("#CreditCard_WarningShownFlag").val("True");
            return (nCheck % 10) == 0;
        } else {
            return true;
        }
    });



    jQuery.validator.unobtrusive.adapters.addBool('creditcard');
    $.validator.unobtrusive.parse();
});

jQuery.extend(jQuery.validator.messages, {
    creditcard: "NOTE: You are adding an invalid credit card number, you may continue."
});

$(document).ready(function() {
    $('#CreditCard_CreditCardNumber').change(function () {
        $("#CreditCard_WarningShownFlag").val("False")
    });
});