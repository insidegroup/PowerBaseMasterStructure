function addRemoveCreditCard(guid) {
	$("#CreditCardId").val(guid)
    document.forms["hierarchyform"].submit();
}

function addRemoveClientAccount(CreditCardId, ClientAccountNumber, SourceSystemCode) {
	$("#CreditCardId").val(CreditCardId);
	$("#hierarchyform input[name='HierarchyType']").val("ClientAccount")
	$("#HierarchyCode").val(ClientAccountNumber)
	$("#SourceSystemCode").val(SourceSystemCode)
	document.forms["hierarchyform"].submit();
}

function addRemoveClientSubUnitTravelerType(CreditCardId, ClientSubUnitGuid, TravelerTypeGuid) {
	$("#CreditCardId").val(CreditCardId);
	$("#hierarchyform input[name='HierarchyType']").val("ClientSubUnitTravelerType")
	$("#ClientSubUnitGuid").val(ClientSubUnitGuid)
	$("#TravelerTypeGuid").val(TravelerTypeGuid)
	document.forms["hierarchyform"].submit();
}

function addRemoveTravelerType(CreditCardId, ClientSubUnitGuid, TravelerTypeGuid) {
	$("#CreditCardId").val(CreditCardId);
	$("#hierarchyform input[name='HierarchyType']").val("TravelerType")
	$("#ClientSubUnitGuid").val(ClientSubUnitGuid)
	$("#TravelerTypeGuid").val(TravelerTypeGuid)
	document.forms["hierarchyform"].submit();
}

function addRemoveClientTopUnit(CreditCardId, HierarchyType, HierarchyCode) {
	$("#CreditCardId").val(CreditCardId);
	$("#hierarchyform input[name='HierarchyType']").val(HierarchyType);
	$("#ClientTopUnitGuid").val(HierarchyCode)
	document.forms["hierarchyform"].submit();
}

function addRemoveClientSubUnit(CreditCardId, HierarchyType, HierarchyCode) {
	$("#CreditCardId").val(CreditCardId);
	$("#hierarchyform input[name='HierarchyType']").val(HierarchyType);
	$("#ClientSubUnitGuid").val(HierarchyCode)
	document.forms["hierarchyform"].submit();
}
