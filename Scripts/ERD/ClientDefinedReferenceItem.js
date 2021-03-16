/*
OnReady
*/
$(document).ready(function() {

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");

	/*
    Submit Form Validation
    */
    $("form").submit(function() {
    
        var validItem = true;

        var minLength = parseInt($("#ClientDefinedReferenceItem_MinLength").val(), 10);
        var maxLength = parseInt($("#ClientDefinedReferenceItem_MaxLength").val(), 10);

        $("#ClientDefinedReferenceItem_MinLengthError").text("");
        $("#ClientDefinedReferenceItem_EntryFormatError").text("");

    	/*
		The Entry Format needs to have a number of characters that are less than or equal to the Max length.
        */

        var entryFormat = $("#ClientDefinedReferenceItem_EntryFormat").val();

        if (entryFormat != "" && maxLength != "" && entryFormat.length > maxLength) {
        	$("#ClientDefinedReferenceItem_EntryFormatError").text("The Entry Format cannot have more characters than the Max length.");
        	return false;
        }

    	/*
		If the user enters a number in the Min Length that is greater than the Max length, show error
		*/

        if (minLength != "" && maxLength != "" && minLength > maxLength) {
        	$("#ClientDefinedReferenceItem_MinLengthError").text("The Min Length cannot be higher than the Max Length");
        	return false;
        }

        return validItem;

    });
});
