<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectClientSubUnitsFiltered_v1Result>>" %>
<style type="text/css">
tr.over td
  { background-color:#FFFF99;
	}
</style>
<table cellspacing="0" id="clientSearchResultTable" class="tablesorter_other2">

    <thead>
        <tr>
            <td colspan="4">Available ClientSubUnits
            </td>
        </tr>
        <tr>
    	    <th>Subunit Name</th>
            <th>Country</th>
            <th>GUID</th>
            <th></th>
        </tr>
    </thead>
    <tbody>  
    <% 
    var i = Model.Count();
    if (i > 0) {
        foreach (var item in Model){
            
			string oldClientGUID = item.ClientSubUnitGuid;
            string[] newClientGUID = oldClientGUID.Split(':');
			 %> 
            <tr id="<%: item.ClientSubUnitGuid %>" clientSubUnitName="<%: item.ClientSubUnitName %>" clientCountryName="<%: item.CountryName %>" clientSubJSUnitGuid="<%: newClientGUID[1] %>" addStatus="New">
                <td><%: Server.HtmlEncode(item.ClientSubUnitName) %></td>
                <td><%: Server.HtmlEncode(item.CountryName) %></td>
                <td><%: Server.HtmlEncode(item.ClientSubUnitGuid) %></td>
                <td><img src="<%=Url.Content("~/Images/add.png")%>" alt="add" border="0" /></td>
            </tr>
            <%
        } 
    }else{
            %>
            <tr>
                <td colspan="3">No results matching your search criteria and/or user rights.</td>
            </tr>
    <%	
    }
    %>
</tbody>
</table>