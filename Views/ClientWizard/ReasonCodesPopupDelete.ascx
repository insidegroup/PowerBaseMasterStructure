<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientWizardReasonCodesVM>" %>

<script type="text/javascript">
	
	/*
	Remove ReasonCode
	*/
	function RemoveReasonCodeItem(reasonCodeItemId) {

		$('#changesMade').val("Yes");

		//Get Values
		var productID = $('#currentProductID').val();
		var productName = $('#currentProductName').val();
		var reasonCode = $('#reasonCode').val();
		var reasonCodeDescription = $('reasonCodeDescription').text();
		var reasonCodeTypeID = $('#currentReasonCodeTypeID').val();
		var reasonCodeTypeDescription = $('#currentReasonCodeTypeDescripton').val();
		var reasonCodeGroupID = $('#reasonCodeGroupID').val();
		var reasonCodeGroupDescription = $('#reasonCodeGroupDescription').val();
		var versionNumber = '1'; //versionNumber

		//Remove row in main table
		$('#' + reasonCodeItemId).hide();

		//Remove any existing rows with same ID to avoid duplicates
		$('#hiddenRemovedClientReasonCodesTable tr').each(function () {
			var reasonCodeItemIdAttr = $(this).attr("reasonCodeItemID");
			if (reasonCodeItemIdAttr == reasonCodeItemId) {
				$(this).remove();
			}
		});

		//Add removed item into hidden Removed table
		$('#hiddenRemovedClientReasonCodesTable').append(
				"<tr reasonCodeItemID='" + reasonCodeItemId +
				"' reasonCode='" + reasonCode +
				"' reasonCodeDescription='" + reasonCodeDescription +
				"' reasonCodeTypeDescription='" + reasonCodeTypeDescription +
				"' productID='" + productID +
				"' reasonCodeTypeID='" + reasonCodeTypeID +
				"' reasonCodeGroupID='" + reasonCodeGroupID +
				"' versionNumber='" + versionNumber + "'></tr>");

	}

</script>
<div id='popupBody'>
	<p>
		<span style="float:left; margin:0 7px 20px 0;" class="ui-icon ui-icon-alert"></span>
		Are you sure you wish to remove this item from this Reason Code group?
	</p>
	<input type="hidden" id="changesMade" />
	<input type="hidden" id="reasonCode" value="<%=ViewData["ReasonCode"]%>" />
	<input type="hidden" id="currentProductID" value="<%=ViewData["ProductId"]%>" />
	<input type="hidden" id="currentProductName" value="<%=ViewData["ProductName"] %>" />	
	<input type="hidden" id="currentReasonCodeTypeID" value="<%=ViewData["ReasonCodeTypeId"]%>" />
	<input type="hidden" id="currentReasonCodeDescripton" value="<%=ViewData["ReasonCodeDescription"] %>" />
	<input type="hidden" id="currentReasonCodeTypeDescripton" value="<%=ViewData["ReasonCodeTypeDescription"] %>" />
	<input type="hidden" id="reasonCodeGroupID" value="<%=ViewData["ReasonCodeGroupID"]%>" />
	<input type="hidden" id="reasonCodeGroupDescription" value="<%=ViewData["ReasonCodeGroupDescription"] %>" />
</div>
