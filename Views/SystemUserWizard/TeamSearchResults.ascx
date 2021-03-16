<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectTeamsFiltered_v1Result>>" %>
<table cellspacing="0" id="teamUsersSearchResultTable" class="tablesorter_other2">
    <thead>
        <tr>
            <td colspan="3">Available Teams</td>
        </tr>
        <tr>
    	    <th>Team Name</th>
            <th>Hierarchy</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
    
        <% 
		var i = Model.Count();
		if(i>0) {
			foreach (var item in Model) {
            %>
                <tr teamName="<%: item.TeamName %>" teamEmail="<%: item.TeamEmail %>" teamPhone="<%: item.TeamPhoneNumber %>" id="<%: item.TeamId %>"  teamHierarchy="<%: item.HierarchyInfo %>">
                    <td><%: Html.Encode(item.TeamName) %></td>
                    <td><%: Html.Encode(item.HierarchyInfo) %></td>
                    <td><img src="<%=Url.Content("~/Images/add.png")%>" alt="add"  /></td>
                </tr>
        <%
            } 
        }else {
        %>
                <tr>
                    <td colspan="3">No results matching your search criteria and/or user rights.</td>
                </tr>
        <%	
	    }
        %>
</tbody>
</table>