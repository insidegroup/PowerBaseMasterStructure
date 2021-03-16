$(document).ready(function () {
	$('#breadcrumb').css('width', 'auto');

	//buttons
	$('#clientAccountsSearchButton').button();
	$('#clientAccountsSearchButton').click(function () {
		DoClientAccountSearch();
	});

	/*
	Search ClientAccounts
	*/
	function DoClientAccountSearch() {

		$("#lastSearchAccount").html("<img src='/images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");

		var url = '/ClientWizard.mvc/ClientAccountSearch?id=' + Math.random();

		var filterField1, filterField2, filterField3;
		if ($('#ClientAccountFilter1').val() != "") {
			if ($('#ClientAccountFilterField1').val() != "PleaseSelect") {
				filterField1 = $('#ClientAccountFilterField1').val();
			} else {
				alert("Please select a filter field for your first filter!");
				$('#lastSearchAccount').html("");
				return;
			}
		} else {
			alert("Please provide at least one filter");
			$('#lastSearchAccount').html("");
			return;
		}

		if ($('#ClientAccountFilter2').val() != "") {
			if ($('#ClientAccountFilterField2').val() != "PleaseSelect") {
				filterField2 = $('#ClientAccountFilterField2').val();
			} else {
				alert("Please select a filter field for your second filter!");
				$('#lastSearchAccount').html("");
				return;
			}
		}
		if ($('#ClientAccountFilter3').val() != "") {
			if ($('#ClientAccountFilterField3').val() != "PleaseSelect") {
				filterField3 = $('#ClientAccountFilterField3').val();
			} else {
				alert("Please select a filter field for your third filter!");
				$('#lastSearchAccount').html("");
				return;
			}
		}

		var filterField1Value, filterField2Value, filterField3Value;

		filterField1Value = escape($('#ClientAccountFilter1').val());
		filterField2Value = escape($('#ClientAccountFilter2').val());
		filterField3Value = escape($('#ClientAccountFilter3').val());

		//PCI = Enforce minimum length to reduce timeouts
		if ((filterField1Value != "" && filterField1Value.length < 2) ||
			(filterField2Value != "" && filterField2Value.length < 2) ||
			(filterField3Value != "" && filterField3Value.length < 2)) {
			alert("Please ensure a minimum of 2 characters per filter!");
			$('#lastSearchAccount').html("");
			return;
		}

		$.ajax({
			type: "POST",
			data: {
				filter1: filterField1Value,
				filterField1: filterField1,

				filter2: filterField2Value,
				filterField2: filterField2,

				filter3: filterField3Value,
				filterField3: filterField3
			},
			url: url,
			timeout: 30000, //Add a timeout of 30 seconds
			error: function (objAJAXRequest, errorMessage) {
				if (errorMessage == "timeout") {
					$("#ClientAccountSearchResults").html("Apologies but the search query timed out.");
				} else {
					$("#ClientAccountSearchResults").html("Apologies but an error occurred");
				}
				$('#lastSearchAccount').html("");
			},
			success: function (result) {

				$("#ClientAccountSearchResults").html(result);
				$("#searchResultClientTable").tablesorter({ widgets: ['zebra'] });
				$('#lastSearchAccount').html("");

				$("#currentAccountsTable").tablesorter({
					headers: {
						0: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						1: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						2: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						3: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						4: {
							// disable it by setting the property sorter to false
							sorter: false
						}
					},
					widgets: ['zebra']
				});
			}
		});
	}

	$('#searchResultClientTable img').live("click", function () {

		var currentAccount = 0;
		var accountNum = $(this).parent().parent().attr("id");
		var combinedAccSSC = $(this).parent().parent().attr("combinedAccSSC");
		var accountCountry = $(this).parent().parent().attr("country");
		var sourceSystemCode = $(this).parent().parent().attr("sourceSystemCode");
		var accountName = $(this).parent().parent().attr("accountName");

		$('#currentAccountsTable tbody tr').each(function () {
			var currentItemAccountSSCNumber = $(this).attr("combinedAccSSC");
			if (combinedAccSSC == currentItemAccountSSCNumber) {
				alert("Account already in current list.");
				currentAccount = 1;
			}
		});

		if (currentAccount == 0) {
			$('#currentAccountsTable').append("<tr accountStatus='NotCurrent' id='" + accountNum + "' combinedAccSSC='" + accountNum + sourceSystemCode + "' country='" + accountCountry + "' sourceSystemCode='" + sourceSystemCode + "'><td>" + accountName + "</td><td>" + accountNum + "</td><td>" + sourceSystemCode + "</td><td>" + accountCountry + "</td> <td><img src='../../images/remove.png' /></td></tr>");
			$('#hiddenAddedClientAccountsTable').append("<tr clientAccount='" + accountNum + "' SSC='" + sourceSystemCode + "'></tr>");
			UpdateClientAccounts();
		}
	});

	$('#currentAccountsTable img').live("click", function () {

		if ($(this).parent().parent().attr("accountStatus") == "Current") {

			//current, therefore add to removedAccounts hidden table
			var clientAccount = $(this).parent().parent().attr("id");
			var SSC = $(this).parent().parent().attr("SSC");
			var versionNumber = $(this).parent().parent().attr("versionNumber");
			$('#hiddenRemovedClientAccountsTable').append("<tr clientAccount='" + clientAccount + "' SSC='" + SSC + "' versionNumber='" + versionNumber + "'></tr>");

		} else {
			//it's not current so remove from addedAccounts hidden table
			var clientAccount = $(this).parent().parent().attr("id");
			var SSC = $(this).parent().parent().attr("sourcesystemcode");
			$('#hiddenAddedClientAccountsTable tr').each(function () {

				var clientAccount2 = $(this).attr("clientAccount");
				var SSC2 = $(this).attr("SSC");
				if ((clientAccount2 == clientAccount) && (SSC2 == SSC)) {
					$(this).remove();
				}
			});

		}
		$(this).parent().parent().remove();
		UpdateClientAccounts();
	});

	function UpdateClientAccounts() {

		//added client accounts
		$('#AddedClientAccounts').val("");
		var addedClientAccounts = [];
		$('#hiddenAddedClientAccountsTable tr').each(function () {
			var SourceSystemCode = $(this).attr("SSC");
			var ClientAccountNumber = $(this).attr("clientAccount");
			var ConfidenceLevelForLoadId = '0';
			addedClientAccounts.push({ SourceSystemCode: SourceSystemCode, ClientAccountNumber: ClientAccountNumber, ConfidenceLevelForLoadId: ConfidenceLevelForLoadId })

		});
		$('#AddedClientAccounts').val(JSON.stringify(addedClientAccounts));

		//removed client accounts
		$('#RemovedClientAccounts').val("");
		var removedClientAccounts = [];
		$('#hiddenRemovedClientAccountsTable tr').each(function () {
			var SourceSystemCode = $(this).attr("SSC");
			var ClientAccountNumber = $(this).attr("clientAccount");
			var VersionNumber = $(this).attr("versionNumber");
			removedClientAccounts.push({ ClientAccountNumber: ClientAccountNumber, SourceSystemCode: SourceSystemCode, VersionNumber: VersionNumber });
		});
		$('#RemovedClientAccounts').val(JSON.stringify(removedClientAccounts));

		$("#lastSearchAccount").html("<img src='/images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");

		//Build Object to Store All Items
		var clientChanges = {
			ClientAccountsAdded: addedClientAccounts,
			ClientAccountsRemoved: removedClientAccounts,
		};

		//AJAX (JSON) POST of Team Changes Object
		$.ajax({
			type: "POST",
			data: JSON.stringify({ "id": $('#ClientSubUnit_ClientSubUnitGuid').val(), "updatedClient": clientChanges }),
			url: '/ClientSubUnitClientAccount.mvc/CreateAccount',
			dataType: "json",
			async: false,
			contentType: "application/json; charset=utf-8",
			success: function (result) {

				if (result.success) {
					$("#lastSearchAccount").html("");
				}
			}
		});
	}
});