<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wizard.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TeamWizardVM>" %>





<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDatabaseAdmin - Client Wizard
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Wizard/AjaxAuthentication.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Wizard/Client.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Wizard/ClientPolicy.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/JSON/JSON2.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Wizard/jsDatePick.jquery.full.1.3.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/bgiframe/jquery.bgiframe.min.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/Style/jsDatePick_ltr.min.css")%>" rel="stylesheet" type="text/css" />

<script type="text/javascript">
$(function(){
	$('#dialog-delete').hide();
});
</script>
<style type="text/css">
.banner2ClientWizard
{
    margin-left:255px;
    height:22px;
    width:745px;
    -moz-border-radius: 5px;
    border-radius: 5px;
	color:#666;
	}

</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="contentarea">
 

    <div id="banner"><div id="banner_text">Client Maintenance &nbsp;<span id="currentClient"></span><span id="menuSelection"><select id="wizardMenu" class="ui-state-default">
            <option value="0" selected="selected">Change to  ...</option>
            <option value="Home.mvc">Main Menu / Home</option>
            <option value="SystemUserWizard.mvc">System User Wizard</option>
            <option value="TeamWizard.mvc">Team Wizard</option>
            <option value="LocationWizard.mvc">Location Wizard</option>
            </select></span></div></div>
            <div id="banner2ClientWizard"><span id="clientSubUnitName"></span></div>
  
    <div id="content">
    
        <div id="tabs">
	        <ul>
		        <li><a href="#tabs-1" id="selectAClientLink">Find</a></li>
	            <li><a href="#tabs-2" id="clientDetailsLink">Client Details</a></li>
		        <li><a href="#tabs-3" id="clientServicingTeamsLink">Client Servicing Teams</a></li>
		        <li><a href="#tabs-4" id="clientsAccountsLink">Client Accounts</a></li>
		        <li><a href="#tabs-5" id="clientServicingOptionsLink">Client Service Options</a></li>
		        <li><a href="#tabs-6" id="clientReasonCodesLink">Client Reason Codes</a></li>
		        <li><a href="#tabs-7" id="clientPolicyGroupsLink">Policies</a></li>
<%--                <li><a href="#tabs-8" id="finishLink">Finish</a></li>		            --%>
	        </ul>
	        <div id="tabs-1"><div id="tabs-1Content"></div></div>           
            <div id="tabs-2"><div id="tabs-2Content"></div></div>
            <div id="tabs-3"><div id="tabs-3Content"></div></div>
            <div id="tabs-4"><div id="tabs-4Content"></div></div>
            <div id="tabs-5"><div id="tabs-5Content"></div></div>
            <div id="tabs-6"><div id="tabs-6Content"></div></div>           
            <div id="tabs-7"><div id="tabs-7Content"></div></div>
<%--            <div id="tabs-8"><div id="tabs-8Content"></div></div>              --%>
        </div>    
    </div>
    <div id="dialog-confirm" title="Service Options"></div>
    <div id="dialog-delete" title="Delete this policy item?">
	<p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Are you sure you wish to remove this item from this policy group?</p>
    <div id="dialog-save-changes">
        <p>There are unsaved changes. Select OK to Save the changes or Cancel to ignore any changes.</p>
    </div>
    <div id="dialog-show-errors">
        <p>Save failed because of the following errors:</p>
        <p id="error-list"></p>
    </div>
</div>

 </div> 

<form id="frmClientWizard" action="" method="post">
    <input type="hidden" id="ClientSubUnit" value=""/>
    <input type="hidden" id="ClientTopUnit" value=""/>
    <input type="hidden" id="RemovedTeams"/>
    <input type="hidden" id="AddedTeams"/>   
    <input type="hidden" id="AlteredTeams"/>   
    <input type="hidden" id="RemovedClientAccounts"/>
    <input type="hidden" id="AddedClientAccounts"/>
    <input type="hidden" id="AlteredClientAccounts"/>
    <input type="hidden" id="AddedServicingOptions" />
    <input type="hidden" id="RemovedServicingOptions" />
    <input type="hidden" id="AlteredServicingOptions" />
    <input type="hidden" id="RemovedReasonCodes" />
    <input type="hidden" id="AddedReasonCodes" />   
	<input type="hidden" id="AlteredReasonCodes" />
	<input type="hidden" id="AlteredPolicyAirParameterGroupItems" />
    <input type="hidden" id="AddedPolicyAirParameterGroupItems" />
    <input type="hidden" id="RemovedPolicyAirParameterGroupItems" />
	<input type="hidden" id="AlteredPolicyAirCabinGroupItems" />
    <input type="hidden" id="AddedPolicyAirCabinGroupItems" />
    <input type="hidden" id="RemovedPolicyAirCabinGroupItems" /> 
	<input type="hidden" id="AlteredPolicyAirMissedSavingsThresholdGroupItems" />
    <input type="hidden" id="AddedPolicyAirMissedSavingsThresholdGroupItems" />
    <input type="hidden" id="RemovedPolicyAirMissedSavingsThresholdGroupItems" /> 
    <input type="hidden" id="AlteredPolicyAirVendorGroupItems" />
	<input type="hidden" id="AddedPolicyAirVendorGroupItems" />
    <input type="hidden" id="RemovedPolicyAirVendorGroupItems" />   
    <input type="hidden" id="AlteredPolicyCarTypeGroupItems" />
	<input type="hidden" id="AddedPolicyCarTypeGroupItems" />
    <input type="hidden" id="RemovedPolicyCarTypeGroupItems" />   
    <input type="hidden" id="AlteredPolicyCarVendorGroupItems" />
	<input type="hidden" id="AddedPolicyCarVendorGroupItems" />
    <input type="hidden" id="RemovedPolicyCarVendorGroupItems" />  
    <input type="hidden" id="AlteredPolicyCityGroupItems" />
	<input type="hidden" id="AddedPolicyCityGroupItems" />
    <input type="hidden" id="RemovedPolicyCityGroupItems" />   
    <input type="hidden" id="AlteredPolicyCountryGroupItems" />
	<input type="hidden" id="AddedPolicyCountryGroupItems" />
    <input type="hidden" id="RemovedPolicyCountryGroupItems" />  
    <input type="hidden" id="AlteredPolicyHotelCapRateGroupItems" />
	<input type="hidden" id="AddedPolicyHotelCapRateGroupItems" />
    <input type="hidden" id="RemovedPolicyHotelCapRateGroupItems" />   
    <input type="hidden" id="AlteredPolicyHotelPropertyGroupItems" />
	<input type="hidden" id="AddedPolicyHotelPropertyGroupItems" />
    <input type="hidden" id="RemovedPolicyHotelPropertyGroupItems" />    
    <input type="hidden" id="AlteredPolicyHotelVendorGroupItems" />
	<input type="hidden" id="AddedPolicyHotelVendorGroupItems" />
    <input type="hidden" id="RemovedPolicyHotelVendorGroupItems" />   
    <input type="hidden" id="AlteredPolicySupplierDealCodes" />
	<input type="hidden" id="AddedPolicySupplierDealCodes" />
    <input type="hidden" id="RemovedPolicySupplierDealCodes" />   
    <input type="hidden" id="AlteredPolicySupplierServiceInformations" />
	<input type="hidden" id="AddedPolicySupplierServiceInformations" />
    <input type="hidden" id="RemovedPolicySupplierServiceInformations" /> 
	<input type="hidden" id="AddedTelephonies" />
    <input type="hidden" id="RemovedTelephonies" />   
    <input type="hidden" id="ClientTopUnitGuid" value=""/>
    <input type="hidden" id="ClientSubUnitGuid" value=""/>
    <input type="hidden" id="ClientType" value=""/> 
    <input type="hidden" id="PolicyInheritCheckChange" />
    <input type="hidden" id="PolicyGroup" />
</form>

<div style="visibility:hidden" id="hiddenTables">
    <table id="hiddenRemovedServicingOptionsTable"></table>
    <table id="hiddenAlteredServicingOptionsTable"></table>
    <table id="hiddenAddedServicingOptionsTable"></table>
    <table id="hiddenAddedClientTeamsTable"></table>
    <table id="hiddenRemovedClientTeamsTable"></table>
    <table id="hiddenAlteredClientTeamsTable"></table>
    <table id="hiddenRemovedClientAccountsTable"></table>
    <table id="hiddenAddedClientAccountsTable"></table>
    <table id="hiddenAddedClientReasonCodesTable"></table>
    <table id="hiddenRemovedClientReasonCodesTable"></table>
	<table id="hiddenChangedClientReasonCodesTable"></table>
    <table id="hiddenChangedPolicyAirCabinGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyAirCabinGroupItemsTable"></table>
 	<table id="hiddenAddedPolicyAirCabinGroupItemsTable"></table>
    <table id="hiddenChangedPolicyAirMissedSavingsThresholdGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyAirMissedSavingsThresholdGroupItemsTable"></table>
 	<table id="hiddenAddedPolicyAirMissedSavingsThresholdGroupItemsTable"></table>
  	<table id="hiddenChangedPolicyAirParameterGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyAirParameterGroupItemsTable"></table>
    <table id="hiddenAddedPolicyAirParameterGroupItemsTable"></table>  
	<table id="hiddenChangedPolicyAirVendorGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyAirVendorGroupItemsTable"></table>
    <table id="hiddenAddedPolicyAirVendorGroupItemsTable"></table>  
    <table id="hiddenAddedPolicyCarTypeGroupItemsTable"></table>
    <table id="hiddenChangedPolicyCarTypeGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyCarTypeGroupItemsTable"></table>  
    <table id="hiddenAddedPolicyCarVendorGroupItemsTable"></table>
    <table id="hiddenChangedPolicyCarVendorGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyCarVendorGroupItemsTable"></table>
    <table id="hiddenAddedPolicyHotelCapRateGroupItemsTable"></table>
    <table id="hiddenChangedPolicyHotelCapRateGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyHotelCapRateGroupItemsTable"></table>
    <table id="hiddenChangedPolicyCityGroupItemsTable"></table>
    <table id="hiddenAddedPolicyCityGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyCityGroupItemsTable"></table>
    <table id="hiddenChangedPolicyCountryGroupItemsTable"></table>
    <table id="hiddenAddedPolicyCountryGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyCountryGroupItemsTable"></table>
    <table id="hiddenAddedPolicyHotelPropertyGroupItemsTable"></table>
    <table id="hiddenChangedPolicyHotelPropertyGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyHotelPropertyGroupItemsTable"></table>
    <table id="hiddenAddedPolicyHotelVendorGroupItemsTable"></table>
    <table id="hiddenChangedPolicyHotelVendorGroupItemsTable"></table>
    <table id="hiddenRemovedPolicyHotelVendorGroupItemsTable"></table>
    <table id="hiddenAddedPolicySupplierDealCodesTable"></table>
    <table id="hiddenChangedPolicySupplierDealCodesTable"></table>
    <table id="hiddenRemovedPolicySupplierDealCodesTable"></table>
    <table id="hiddenAddedPolicySupplierServiceInformationsTable"></table>
    <table id="hiddenChangedPolicySupplierServiceInformationsTable"></table>
    <table id="hiddenRemovedPolicySupplierServiceInformationsTable"></table>

    <table id="hiddenAddedTelephoniesTable"></table>
    <table id="hiddenRemovedTelephoniesTable"></table>
</div>

<!--Used For ReasonCodes-->
<input type="hidden" id="source" />
<input type="hidden" id="groupCount" />

<!--Used For Policies  -->
<input type="hidden" id="UnAvailableRoutingNames" />
<input type="hidden" id="ShowPolicyGroupScreen" value="" />
</asp:Content>
