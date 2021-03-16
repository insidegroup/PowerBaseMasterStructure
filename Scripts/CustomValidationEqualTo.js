/*
This file is no longer used - DMc 23 Mar 2013 - To remove
*/

/* Client-side validation for equaltoproperty rule */
jQuery.validator.addMethod("equaltoproperty", function (value, element, params) {
    if (this.optional(element)) {
        return true;
    }

    var otherPropertyControl = $("#" + params.otherProperty);
    if (otherPropertyControl == null) {
        return false;
    }

    var otherPropertyValue = otherPropertyControl[0].value;
    return otherPropertyValue == value;

    
});


/* Utility method to test if element matches the value of the control passed in 
   as params.otherProperty */
function testConditionEqual(element, params) {
    /* Find control for other property */
    var otherPropertyControl = $("#" + params.otherProperty);
    if (otherPropertyControl == null) {
        return false;
    }

    var otherPropertyValue;
    if (otherPropertyControl[0].type == "checkbox") {
        otherPropertyValue = (otherPropertyControl[0].checked) ? "True" : "False";
    } else {
        otherPropertyValue = otherPropertyControl[0].value;
    }

    return otherPropertyValue == params.comparand;
}
