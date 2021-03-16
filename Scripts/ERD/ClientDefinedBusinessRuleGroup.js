$(document).ready(function () {

	$("#content tr:odd").addClass("row_odd");
	$("#content tr:even").addClass("row_even");
	$('#search').hide();

	$('#SearchButton').button();

	//Breadcrumbs
	$('#breadcrumb').css('width', 'auto');
	$('.full-width #search_wrapper').css('height', '22px');

	//Default Value
	if ($('#Filter')[0].selectedIndex <= 0) {
		$('#Filter').val('Category');
	}

	//Show Search Rows
	LoadSearchRows();

	function LoadSearchRows() {

		$('.search-row').hide(); 

		var selected = $('#Filter option:selected').val();

		if (selected == "Category") {
			$('.find-category').show();
			$('.find-business-name').hide();
		} else if (selected == "Business Rules Group Name") {
			$('.find-business-name').show();
			$('.find-category').hide();
		}
	}

	$("#Filter").change(function () {
		$('.search-row input').val("");
		LoadSearchRows();
	});
	
	$("#SearchButton").click(function () {
		$('#form0').submit();
	});

	//Submit Form Validation
	$('#form0').submit(function () {

		var validItem = false;

		// Get Fields
		var selected = $('#Filter option:selected').val();
		var category = $('#Category').val();
		var groupName = $('#ClientDefinedRuleGroupName').val();

		var message = "";
		var message_minimum_chars = "Please enter at least 2 characters for each search field";

		if (selected != "") {
			if (selected == "Category") {
				if (category != "") { //Droplist
					validItem = true;
				}
			} else if (selected == "Business Rules Group Name") {
				if (groupName == "" || (groupName != "" && groupName.length < 2)) {
					message = message_minimum_chars;
				} else {
					validItem = true;
				}
			}
		} else {
			message = message_minimum_chars;
		}

		//Show alert on error
		if (!validItem && message != '') {
			alert(message);
		}

		return validItem;

	});
});
