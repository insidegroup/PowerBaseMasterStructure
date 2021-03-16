<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientWizardReasonCodesVM>" %>

<script type="text/javascript">

	$(document).ready(function () {

		$("tr:even").addClass("row_odd");
		$("tr:odd").addClass("row_even");
		$("td").css("background", "transparent");

		$('#changesMade').val("No");

		//Add values to ReasonCode dropdown that have been removed
		$('#hiddenRemovedClientReasonCodesTable tr').each(function () {

			var reasonCode = $(this).attr("reasonCode");
			var reasonCodeTypeID = $(this).attr("reasonCodeTypeID");
			var reasonCodeDescription = $(this).attr("reasonCodeDescription");
			var reasonCodeTypeDescription = $(this).attr("reasonCodeTypeDescription");
			var reasonCodeGroupID = $('#reasonCodeGroupID').val();
			var productID = $(this).attr("productID");
			var currentProductID = $('#currentProductID').val();
			var currentReasonCodeTypeID = $('#currentReasonCodeTypeID').val();

			if (productID == currentProductID) {
				if (reasonCodeTypeID == currentReasonCodeTypeID) {

					var itemExists = false;

					//Prevent adding duplicates
					$("#ReasonCode option").each(function () {
						if ($(this).val() == $.trim(reasonCode)) {
							itemExists = true;
						}
					});

					if (!itemExists) {
						var text = reasonCode;
						if (reasonCodeDescription != '') {
							text = text + " - " + reasonCodeDescription;
						}
						$("#ReasonCode").append(
							$("<option></option>")
								.attr("value", reasonCode)
								.text(text)
							);

						//Sort Items ABC
						$("#ReasonCode").html($('#ReasonCode option').sort(function (x, y) {
							return $(x).val() < $(y).val() ? -1 : 1;
						}));

						$("#ReasonCode").get(0).selectedIndex = 0;
					}
				}
			}

		});

		// Remove values from ReasonCode Dropdown that have been added
		$('#hiddenAddedClientReasonCodesTable tr').each(function () {

			var reasonCode = $(this).attr("reasonCode");
			var productID = $(this).attr("productID");
			var reasonCodeTypeID = $(this).attr("reasonCodeTypeID");
			var reasonCodeGroupID = $(this).attr('#reasonCodeGroupID');
			var currentProductID = $('#currentProductID').val();
			var currentReasonCodeTypeID = $('#currentReasonCodeTypeID').val();

			if (productID == currentProductID) {
				if (reasonCodeTypeID == currentReasonCodeTypeID) {
					$("#ReasonCode > option").each(function () {
						if ($(this).val() == reasonCode) {
							$(this).remove();
						}
					});
				}
			}

		});

		// Remove values from ReasonCode Dropdown that are in table
		var productName = $('#currentProductName').val();
		var reasonCodeTypeDescription = $('#currentReasonCodeTypeDescripton').val();
		var productNameId = productName.replace(/ /g, '');
		var reasonCodeTypeDescriptionId = reasonCodeTypeDescription.replace(/ /g, '');

		//Present in Edit Screen
		var oldReasonCode = $('#oldReasonCode').val();

		$('#currentReasonCode' + productNameId + reasonCodeTypeDescriptionId + 'ItemsTable tr').each(function () {
			if ($(this).is(":visible")) {
				var reasonCode = $(this).find(".reason_code").html();
				$("#ReasonCode > option").each(function () {
					var optionText = $(this).val();
					if (oldReasonCode != null) {
						if (optionText == reasonCode && optionText != oldReasonCode) {
							$(this).remove();
						}
					} else {
						if (optionText == reasonCode) {
							$(this).remove();
						}
					}
				});
			}
		});
	});

	//Save Reason Code Item 
	function SaveReasonCodeItem() {

		$('#changesMade').val("Yes");

		//Get Values
		var productID = $('#currentProductID').val();
		var productName = $('#currentProductName').val();
		var reasonCode = $('#ReasonCode option:selected').val();
		var reasonCodeTypeID = $('#currentReasonCodeTypeID').val();
		var reasonCodeTypeDescription = $('#currentReasonCodeTypeDescripton').val();
		var reasonCodeGroupID = $('#reasonCodeGroupID').val();
		var reasonCodeGroupDescription = $('#reasonCodeGroupDescription').val();

		if (reasonCode != '') {

			var hasItemAlreadyBeenDeleted = false;

			//Item could have been re-added so check this
			$('#hiddenRemovedClientReasonCodesTable tr').each(function () {

				var reasonCodeItemId = $(this).attr("reasoncodeitemid");
				var reasonCodeDeleted = $(this).attr("reasonCode");
				var reasonCodeTypeIdDeleted = $(this).attr("reasonCodeTypeID");
				var productIdDeleted = $(this).attr("productID");

				if (productID == productIdDeleted && reasonCodeTypeID == reasonCodeTypeIdDeleted && reasonCode == reasonCodeDeleted) {

					hasItemAlreadyBeenDeleted = true;

					//Remove from Deleted Table
					$(this).remove();

					//Show in main table
					var productNameId = productName.replace(/ /g, '');
					var reasonCodeTypeDescriptionId = reasonCodeTypeDescription.replace(/ /g, '');

					$('#currentReasonCode' + productNameId + reasonCodeTypeDescriptionId + 'ItemsTable tr').each(function () {
						if ($(this).attr("id") == reasonCodeItemId) {
							$(this).show();
						}
					});

				}
			});

			//Add if doesnt exist
			if (!hasItemAlreadyBeenDeleted) {

				//Add new row into table
				var productNameId = productName.replace(/ /g, '');
				var reasonCodeTypeDescriptionId = reasonCodeTypeDescription.replace(/ /g, '');
				var tableId = '#currentReasonCode' + productNameId + reasonCodeTypeDescriptionId + 'ItemsTable';
				var targetTable = $(tableId);
				targetTable.find('tr:last')
					.after('<tr>' +
								'<td>&nbsp;</td>' +
								'<td>' + reasonCode + '</td>' +
								'<td>' + reasonCodeTypeDescription + '</td>' +
								'<td>&nbsp;</td>' +
								'<td>&nbsp;</td>' +
								'<td>Added</td>' +
							'</tr>');

				//Add new item into hidden table
				$('#hiddenAddedClientReasonCodesTable').append("<tr reasonCode='" + reasonCode + "' reasonCodeTypeDescription='" + reasonCodeTypeDescription + "' productName='" + productName + "' productID='" + productID + "' reasonCodeTypeID='" + reasonCodeTypeID + "' reasonCodeGroupID='" + reasonCodeGroupID + "'></tr>");
			}
		}
	}
	
	//Edit Reason Code Item 
	function EditReasonCodeItem(reasonCodeItemID) {

		$('#changesMade').val("Yes");

		//Get Values
		var productID = $('#currentProductID').val();
		var productName = $('#currentProductName').val();
		var oldReasonCode = $('#oldReasonCode').val();
		var reasonCode = $('#ReasonCode option:selected').val();
		var reasonCodeTypeID = $('#currentReasonCodeTypeID').val();
		var reasonCodeTypeDescription = $('#currentReasonCodeTypeDescripton').val();
		var reasonCodeGroupID = $('#reasonCodeGroupID').val();
		var reasonCodeGroupDescription = $('#reasonCodeGroupDescription').val();
		var versionNumber = '1'; //versionNumber

		//Check if the reasoncode is different to original
		if (reasonCode != '' && reasonCode != oldReasonCode) {

			//Update row in main table
			$('#' + reasonCodeItemID).find('td:last').html('Modified');
			$('#' + reasonCodeItemID).find('.reason_code').html(reasonCode);

			//Add item into hidden Changed table
			$('#hiddenChangedClientReasonCodesTable').append(
					"<tr reasonCodeItemID='" + reasonCodeItemID +
					"' reasonCode='" + reasonCode +
					"' reasonCodeTypeDescription='" + reasonCodeTypeDescription +
					"' productID='" + productID +
					"' reasonCodeTypeID='" + reasonCodeTypeID +
					"' reasonCodeGroupID='" + reasonCodeGroupID +
					"' versionNumber='" + versionNumber + "'></tr>");

		}
	}
</script>
<div id='popupBody'>
<table class="tablesorter_other2" cellspacing="0" id="currentReasonCodesTable">
	<tbody>
		<tr>
			<td>Product</td>
			<td><%=ViewData["ProductName"] %></td>
		</tr>
		<tr>
			<td>Reason Code Type</td>
			<td><%=ViewData["ReasonCodeTypeDescription"] %></td>
		</tr>
		<tr>
			<td>Reason Code</td>
			<td>
				<%=Html.DropDownList("ReasonCode", ViewData["AvailableReasonCodesList"] as SelectList, "Please Select")%>
				<%=Html.ValidationMessage("ReasonCode")%>
			</td>
		</tr>
	</tbody>
 </table>
<input type="hidden" id="changesMade" />
	<input type="hidden" id="oldReasonCode" value="<%=ViewData["ReasonCode"]%>" />
	<input type="hidden" id="currentProductID" value="<%=ViewData["ProductId"]%>" />
	<input type="hidden" id="currentProductName" value="<%=ViewData["ProductName"] %>" />	
	<input type="hidden" id="currentReasonCodeTypeID" value="<%=ViewData["ReasonCodeTypeId"]%>" />
	<input type="hidden" id="currentReasonCodeTypeDescripton" value="<%=ViewData["ReasonCodeTypeDescription"] %>" />
	<input type="hidden" id="reasonCodeGroupID" value="<%=ViewData["ReasonCodeGroupID"]%>" />
	<input type="hidden" id="reasonCodeGroupDescription" value="<%=ViewData["ReasonCodeGroupDescription"] %>" />
</div>
