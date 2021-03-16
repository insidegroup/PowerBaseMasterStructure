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

        return !(nCheck % 10) == 0;
    });



    jQuery.validator.unobtrusive.adapters.addBool('creditcard');
    $.validator.unobtrusive.parse();
});
jQuery.extend(jQuery.validator.messages, { creditcard: "Valid Credit Card Numbers are not allowed." });