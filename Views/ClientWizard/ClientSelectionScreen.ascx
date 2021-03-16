<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="divSearch">
    <table class="tablesorter" cellspacing="0">
        <thead>
            <tr>
                <th>Search field:</th>
                <th><select id="CFilterField" class="ui-state-active">
                    <option value="ClientTopUnitName">Client TopUnit Name</option>
                    <option value="ClientTopUnitGuid">Client TopUnit GUID</option>
                    <option value="ClientSubUnitName" selected="selected">Client SubUnit Name</option>
                    <option value="ClientSubUnitGuid">Client SubUnit GUID</option>
					<option value="ClientSubUnitDisplayName">Client SubUnit Display Name</option>
                    <option value="Team">Team</option>
                    <option value="Country">Country</option>
                    <option value="GDSCompanyProfileName">GDS Company Profile Name</option>
                    <option value="PseudoCityCode">PseudoCityCode or OfficeID</option>
                    </select></th>
                <th>Find Client: &nbsp; <input type="text" value="" id="CFilter"/>&nbsp;<span id="SearchButton"><small>Search >> </small></span></th>
                <th id="lastSearchTS"></th>
		        <th></th>
            </tr>
        </thead> 
    </table>
</div>
<div id="ClientTopUnitSearchResults">
<p>Please complete the form above to do a search for clients.</p>
</div>
<div id="ClientSubUnitSearchResults"></div>
   