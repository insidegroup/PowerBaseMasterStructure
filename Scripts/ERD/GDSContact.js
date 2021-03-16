$(document).ready(function () {
	$('#menu_gdsmanagement').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//Disabled Linked Dropdowns
	$('#GDSContact_GlobalRegionCode, #GDSContact_PseudoCityOrOfficeDefinedRegionId').attr('disabled', true);

	//Enabled for Edit Page
	var country = $('#GDSContact_CountryCode option:selected').val();
	if(country != '') {
		$('#GDSContact_GlobalRegionCode').attr('disabled', false);
		var globalRegionCode = $('#GDSContact_GlobalRegionCode option:selected').val();
		if (globalRegionCode != '') {
			$('#GDSContact_PseudoCityOrOfficeDefinedRegionId').attr('disabled', false);
		}
	}

	//GDSContact_CountryCode Change
	$('#GDSContact_CountryCode').change(function () {
		var selectedCountry = $('#GDSContact_CountryCode option:selected').val();
		if (selectedCountry != '') {
			$('#GDSContact_GlobalRegionCode').attr('disabled', false);
			SelectGlobalRegionForCountry(selectedCountry);
		} else {
			$('#GDSContact_GlobalRegionCode, #GDSContact_PseudoCityOrOfficeDefinedRegionId').val('').attr('disabled', true);
		}
	});

	//GDSContact_GlobalRegionCode Change
	$('#GDSContact_GlobalRegionCode').change(function () {
		var globalRegionCode = $('#GDSContact_GlobalRegionCode option:selected').val();
		LoadPseudoCityOrOfficeDefinedRegionsForGlobalRegion(globalRegionCode);
	});

	//Choose GlobalRegionCode based on Country
	function SelectGlobalRegionForCountry(selectedCountry) {
		jQuery.ajax({
			type: "POST",
			url: "/Country.mvc/GetCountryGlobalRegions",
			data: { countryCode: selectedCountry },
			success: function (data) {
				if (!jQuery.isEmptyObject(data)) {
					var globalRegionCode = data[0].HierarchyCode;
					if (globalRegionCode != '') {
						$('#GDSContact_GlobalRegionCode').val(globalRegionCode).attr('disabled', false);
						LoadPseudoCityOrOfficeDefinedRegionsForGlobalRegion(globalRegionCode);
					}
				}
			},
			dataType: "json",
			async: false
		});
	}

	//Load PseudoCityOrOfficeDefinedRegions based on GlobalRegion
	function LoadPseudoCityOrOfficeDefinedRegionsForGlobalRegion(globalRegionCode) {
		jQuery.ajax({
			type: "POST",
			url: "/PseudoCityOrOfficeDefinedRegion.mvc/GetPseudoCityOrOfficeDefinedRegions",
			data: { globalRegionCode: globalRegionCode },
			success: function (data) {
				if (!jQuery.isEmptyObject(data)) {
					$("#GDSContact_PseudoCityOrOfficeDefinedRegionId").find('option').remove();
					$("<option value=''>Please Select...</option>").appendTo($("#GDSContact_PseudoCityOrOfficeDefinedRegionId"));
					$(data).each(function () {
						$("<option value=" + this.PseudoCityOrOfficeDefinedRegionId + ">" + this.PseudoCityOrOfficeDefinedRegionName + "</option>").appendTo($("#GDSContact_PseudoCityOrOfficeDefinedRegionId"));
					});
					$("#GDSContact_PseudoCityOrOfficeDefinedRegionId").attr('disabled', false);
				}
			},
			dataType: "json",
			async: false
		});
	}

});


$('#form0').submit(function () {

	
});