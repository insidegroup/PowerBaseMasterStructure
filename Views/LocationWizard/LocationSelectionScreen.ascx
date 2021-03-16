<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectLocationsFiltered_v1Result>>" %>

        
<div id="divSearch">
    <table class="tablesorter" cellspacing="0">
        <thead>
            <tr>
                <th>Find Location: &nbsp; <input type="text" value="" id="LFilter"/></th>
                <th>Search field:</th>
                <th> <select id="LFilterField">
                    <option value="Location">Location</option>
                    <option value="CountryRegion">Country Region</option>
                    <option value="Country">Country</option>
                    </select>&nbsp;<span id="SearchButton"><small>Search >> </small></span></th>
                <th id="lastSearchL"></th>
                
		        <th><span id="createLocation" style="float:right"><small>Create New Location</small></span></th>
            </tr>
        </thead> 
    </table>
</div>
<div id="divLocationList">
    <% 
    var i = Model.Count();
	if(i>0){ 
	%>
    <table width="100%" id="locationTable" class="tablesorter_other2" cellspacing="0">
        <thead>
            <tr>
                <th>
                    Location
                </th>
                <th>
                    Country Region
                </th>
                <th>
                    Country
                </th>
                 <th></th>
            </tr>
        </thead>
        <tbody>
        <% foreach (var item in Model) { %>
            <tr>
                <td>
                    <%: item.LocationName %>
                </td>
                <td>
                    <%: item.CountryRegionName %>
                </td>
                <td>
                    <%: item.CountryName %>
                </td>
                 <td>
                    <a href="javascript:SetWizardLocation(<%: item.LocationId %>)" title="Select">Select</a>
                </td>
            </tr>  
        <% } %>
	    </tbody>
    </table>
     [Results: <%: i%> ]
    <%} else {%>
        <h4>No results for your search criteria and/or user rights.</h4>
    <%} %>
</div>
   



