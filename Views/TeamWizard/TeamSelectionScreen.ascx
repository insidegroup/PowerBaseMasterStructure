<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectTeamsFiltered_v1Result>>" %> 
 <div id="divSearch">
     <table class="tablesorter" cellspacing="0">
        <thead>
            <tr>
                <th>Find Team: &nbsp; <input type="text" value="" id="TSFilter"/>&nbsp;<span id="SearchButton"><small>Search >> </small></span></th>
                <th id="lastSearchTS"></th>
		        <th><span id="createTeam" style="float:right"><small>Create New Team</small></span></th>
            </tr>
        </thead> 
    </table>
</div>
<div id="divTeamList">
    <% 
    var i = Model.Count();
	if(i>0){ 
	%>
    <table width="100%" id="teamTable" class="tablesorter_other2" cellspacing="0">
        <thead>
            <tr>
                <th>
                    Name
                </th>
                <th>
                    Phone Number
                </th>
                <th>
                    Email
                </th>
                <th>
                    Type
                </th>
                 <th></th>
            </tr>
        </thead>
        <tbody>
        <% foreach (var item in Model) { %>
            <tr>
                <td>
                    <%: item.TeamName %>
                </td>
                <td>
                    <%: item.TeamPhoneNumber %>
                </td>
                <td>
                    <%: item.TeamEmail %>
                </td>
                <td>
                    <%: item.TeamTypeDescription %>
                </td>
                 <td>
                    <a href="javascript:SetWizardTeam(<%: item.TeamId %>)" title="Select">Select</a>
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



