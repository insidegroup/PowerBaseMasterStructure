    $(document).ready(function() {
        $('#menu_admin').click();
	    $("tr:odd").addClass("row_odd");
	    $("tr:even").addClass("row_even");
	})


	$(function() {
	    $("#TravelPortTypeId").change(function() {

	        if ($("#LanguageCode").val() != "") {
	            $("#LanguageCode").val("");
	        }
	        if ($("#TravelPortTypeId").val() != "") {

	            jQuery.ajax({
	                type: "POST",
	                url: "/TravelPortType.mvc/GetLanguages",
	                data: { travelPortTypeId: $("#TravelPortTypeId").val(), travelPortCode: $("#TravelPortCode").val() },
	                success: function(data) {
	                    $("#LanguageCode").get(0).options.length = 0;
	                    $("#LanguageCode").get(0).options[0] = new Option("Please Select...", "");

	                    $.each(data, function(index, item) {
	                        $("#LanguageCode").get(0).options[$("#LanguageCode").get(0).options.length] = new Option(item.LanguageName, item.LanguageCode);
	                    });
	                },
	                dataType: "json",
	                async: false
	            });
	        };
	    });
	});
