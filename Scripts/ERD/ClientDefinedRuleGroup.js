$(document).ready(function () {

	$("#content tr:odd").addClass("row_odd");
	$("#content tr:even").addClass("row_even");
	$('#search').hide();

	$('#SearchButton').button();

	//Breadcrumbs
	$('#breadcrumb').css('width', 'auto');
	$('.full-width #search_wrapper').css('height', '22px');

	//Default Value
	if ($('#HierarchyType')[0].selectedIndex <= 0) {
		$('#HierarchyType').val('ClientSubUnit');
	}

	$(function () {

		$("#HierarchyItem").autocomplete({
			source: function (request, response) {
				if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
					$.ajax({
						url: "/PolicyGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
						data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#TravelerTypeGuid").val() },
						success: function (data) {
							response($.map(data, function (item) {
								return {
									label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
									value: item.HierarchyName,
									id: item.HierarchyCode,
									text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
								}
							}))
						}
					})
				} else if ($("#HierarchyType").val() == "TravelerType") {
					$.ajax({
						url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
						data: { searchText: request.term, hierarchyItem: "TravelerType", domainName: 'Policy Hierarchy' },
						success: function (data) {
							response($.map(data, function (item) {
								return {
									label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>" + (item.ClientSubUnitName == "" ? "" : "<span class=\"tt-csu-name\">" + item.ClientSubUnitName + "</span>"),
									value: item.HierarchyName,
									id: item.HierarchyCode,
									text: item.HierarchyName
								}
							}))
						}
					});
				} else {
					$.ajax({
						url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
						data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Policy Hierarchy' },
						success: function (data) {
							response($.map(data, function (item) {
								if ($("#HierarchyType").val() == "ClientAccount") {
									return {
										label: item.HierarchyName,
										value: item.HierarchyName,
										id: item.ClientAccountNumber,
										ssc: item.SourceSystemCode,
										text: ""
									}
								} else {
									return {
										label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
										value: item.HierarchyName,
										id: item.HierarchyCode,
										text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
									}
								}

							}))
						}
					});
				}
			},
			select: function (event, ui) {
				//ui.item.text
				//ui.item.value
				//ui.item.id
				//ui.item.ssc			

				$("#HierarchyCode").val(ui.item.id);

				var hierarchyItem = $("#HierarchyType").val();
				
				switch (hierarchyItem) {
					case "ClientTopUnit":
						$('#ClientTopUnitName').val(ui.item.value);
						break;
					case "ClientSubUnit":
						$('#ClientSubUnitName').val(ui.item.value);
						break;
					case "ClientAccount":
						$('#ClientAccountName').val(ui.item.value);
						$("#SourceSystemCode").val(ui.item.ssc);
						break;
					case "TravelerType":
						$('#TravelerTypeName').val(ui.item.value);
						break;
					case "ClientSubUnitTravelerType":
						$('#ClientSubUnitName').val(ui.item.value);
						break;
				}
			}
		});

		$("#TravelerTypeName").autocomplete({
			source: function (request, response) {
				$.ajax({
					url: "/PolicyGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
					data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#HierarchyCode").val() },
					success: function (data) {
						response($.map(data, function (item) {
							return {
								label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
								value: item.HierarchyName,
								id: item.HierarchyCode,
								text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
							}
						}))
					}
				})
			},
			select: function (event, ui) {
				//ui.item.text
				//ui.item.value
				//ui.item.id

				$('#ClientSubUnitTravelerTypeName').val(ui.item.text);
			}
		});
	});

	//Show Search Rows
	LoadSearchRows();

	function LoadSearchRows() {

		$('#TravelerTypeRow, #AccountRow, .client-row, .hierarchy-row').hide();
		$('.client-label').text('Find Client');
		$('#HierarchyCode').val('');

		var selected = $('#HierarchyType option:selected').text();

		if (selected == "Client Traveler Type") {
			$('#TravelerTypeRow').show();

		} else if (selected == "Client SubUnit Traveler Type") {
			$('.client-label').text('Client SubUnit');
			$('.client-row').show();
			$('#TravelerTypeRow').show();

		} else if (selected == "Client Account") {
			$('#AccountRow').show();

		} else {
			$('.client-row').show();
		}
	}

	$("#HierarchyType").change(function () {
		LoadSearchRows();
	});
	
	$("#SearchButton").click(function () {
		$('#form0').submit();
	});

	//Submit Form Validation
	$('#form0').submit(function () {

		var validItem = true;
		var validTravelerType = true;

		if ($("#HierarchyType").val() != "" && ($("#HierarchyType").val() == "ClientTopUnit" || $("#HierarchyType").val() == "ClientSubUnit" || $("#HierarchyType").val() == "ClientSubUnitTravelerType")) {
			jQuery.ajax({
				type: "POST",
				url: "/Hierarchy.mvc/IsValid" + $("#HierarchyType").val(),
				data: { searchText: encodeURIComponent($("#HierarchyItem").val()) },
				success: function (data) {

					if (jQuery.isEmptyObject(data)) {
						validItem = false;
					}
				},
				dataType: "json",
				async: false
			});
			if (!validItem) {
				$("#lblHierarchyItemMsg").removeClass('field-validation-valid');
				$("#lblHierarchyItemMsg").addClass('field-validation-error');
				$("#lblHierarchyItemMsg").text("This is not a valid entry.");
				if ($("#lblAuto").length) { $("#lblAuto").text("") };
			} else {
				$("#lblHierarchyItemMsg").text("");
			}
		}

		if ($("#TravelerTypeName").is(":visible")) {

			if ($("#HierarchyType").val() != "") {
				jQuery.ajax({
					type: "POST",
					url: "/Hierarchy.mvc/IsValidTravelerType",
					data: { searchText: $("#TravelerTypeName").val() },
					success: function (data) {

						if (jQuery.isEmptyObject(data)) {
							validTravelerType = false;
						}
					},
					dataType: "json",
					async: false
				});
				if (!validTravelerType) {
					$("#lblTravelerTypeMsg").removeClass('field-validation-valid');
					$("#lblTravelerTypeMsg").addClass('field-validation-error');
					$("#lblTravelerTypeMsg").text("This is not a valid entry.");
					if ($("#lblAuto").length) { $("#lblAuto").text("") };
				} else {
					$("#lblTravelerTypeMsg").text("");
				}
			}
		}

		if (validItem && validTravelerType) {
			return true;
		} else {
			return false
		};

	});
});