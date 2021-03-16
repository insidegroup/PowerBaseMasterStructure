/*
OnReady
*/
$(document).ready(function () {

    $("#hierarchysearchform").submit(function () {

		//Check Dropdown
    	if ($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val() == "") {
            $("#errorMessageSearchProperty").text("Please choose a Hierarchy Level");
            return false;
        }

		//Check Fields
        var value = $("#FilterHierarchySearchProperty").val();
        if (value == "ClientSubUnitTravelerType" || value == "TravelerType") {
        	if ($("#hierarchysearchform input[name='FilterHierarchyCSUSearchText']").val().trim().length < 2) {
        		$("#errorMessageSearchText").text("Please enter a minimum of 2 characters for Client SubUnit");
        		return false;
        	}
        	if ($("#hierarchysearchform input[name='FilterHierarchyTTSearchText']").val().trim().length < 2) {
        		$("#errorMessageSearchText").text("Please enter a minimum of 2 characters for TravelerType");
        		return false;
        	}
        } else {
        	if ($("#hierarchysearchform input[name='FilterHierarchySearchText']").val().trim().length < 2) {
        		$("#errorMessageSearchText").text("Please enter a minimum of 2 characters");
        		return false;
        	}
        }
    });

    $('.filterHierarchyCSUSearchText').hide();
    $('.filterHierarchyTTSearchText').hide();
    $('.csu-search').hide();
    $('.filter-search').show();

    LoadSearchBoxes();

    function LoadSearchBoxes() {

    	var value = $("#FilterHierarchySearchProperty").val();

    	if (value == "ClientSubUnitTravelerType" || value == "TravelerType") {
    		$('.csu-search').show();
    		$('.filter-search').hide(); 
			$('.filterHierarchyCSUSearchText').show();
    		$('.filterHierarchyTTSearchText').show();
    		$('.filterHierarchySearchText').val("").hide();
    	} else {
    		$('.csu-search').hide();
    		$('.filter-search').show();
    		$('.filterHierarchyCSUSearchText').val("").hide();
    		$('.filterHierarchyTTSearchText').val("").hide();
    		$('.filterHierarchySearchText').show();
    	}
    }

    $("#FilterHierarchySearchProperty").change(function () {
    	LoadSearchBoxes();
    });
});

function addRemoveClientAccount(ClientAccountNumber, SourceSystemCode) {
	$("#hierarchyform input[name='FilterHierarchySearchText']").val($("#hierarchysearchform input[name='FilterHierarchySearchText']").val())
	$("#hierarchyform input[name='FilterHierarchySearchProperty']").val($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val())
	$("#hierarchyform input[name='HierarchyType']").val("ClientAccount")
	$("#HierarchyCode").val(ClientAccountNumber)
	$("#SourceSystemCode").val(SourceSystemCode)
	document.forms["hierarchyform"].submit();
}

function addRemoveClientSubUnitTravelerType(ClientSubUnitGuid, TravelerTypeGuid) {
	$("#hierarchyform input[name='FilterHierarchySearchText']").val($("#hierarchysearchform input[name='FilterHierarchySearchText']").val())
	$("#hierarchyform input[name='FilterHierarchySearchProperty']").val($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val())
	$("#hierarchyform input[name='FilterHierarchyCSUSearchText']").val($("#hierarchysearchform input[name='FilterHierarchyCSUSearchText']").val())
	$("#hierarchyform input[name='FilterHierarchyTTSearchText']").val($("#hierarchysearchform input[name='FilterHierarchyTTSearchText']").val())
	$("#hierarchyform input[name='HierarchyType']").val("ClientSubUnitTravelerType")
	$("#ClientSubUnitGuid").val(ClientSubUnitGuid)
	$("#TravelerTypeGuid").val(TravelerTypeGuid)
	document.forms["hierarchyform"].submit();
}

function addRemoveTravelerType(ClientSubUnitGuid, TravelerTypeGuid) {
	$("#hierarchyform input[name='FilterHierarchySearchText']").val($("#hierarchysearchform input[name='FilterHierarchySearchText']").val())
	$("#hierarchyform input[name='FilterHierarchySearchProperty']").val($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val())
	$("#hierarchyform input[name='FilterHierarchyCSUSearchText']").val($("#hierarchysearchform input[name='FilterHierarchyCSUSearchText']").val())
	$("#hierarchyform input[name='FilterHierarchyTTSearchText']").val($("#hierarchysearchform input[name='FilterHierarchyTTSearchText']").val())
	$("#hierarchyform input[name='HierarchyType']").val("TravelerType")
	$("#ClientSubUnitGuid").val(ClientSubUnitGuid)
	$("#TravelerTypeGuid").val(TravelerTypeGuid)
	document.forms["hierarchyform"].submit();
}

function addRemoveHierarchy(HierarchyType, HierarchyCode) {
	$("#hierarchyform input[name='FilterHierarchySearchText']").val($("#hierarchysearchform input[name='FilterHierarchySearchText']").val())
	$("#hierarchyform input[name='FilterHierarchySearchProperty']").val($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val())
	$("#hierarchyform input[name='FilterHierarchyCSUSearchText']").val($("#hierarchysearchform input[name='FilterHierarchyCSUSearchText']").val())
	$("#hierarchyform input[name='FilterHierarchyTTSearchText']").val($("#hierarchysearchform input[name='FilterHierarchyTTSearchText']").val())
	$("#hierarchyform input[name='HierarchyType']").val(HierarchyType);
	$("#HierarchyCode").val(HierarchyCode)
	document.forms["hierarchyform"].submit();
}
