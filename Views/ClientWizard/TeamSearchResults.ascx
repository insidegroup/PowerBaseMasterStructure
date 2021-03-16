<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectTeamsFiltered_v1Result>>" %>
<table cellspacing="0" id="teamUsersSearchResultTable" class="tablesorter_other2">
	<thead>
		<tr>
			<td colspan="4">Available Teams</td>
		</tr>
		<tr>
			<th>Team Name</th><td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
		</tr>
	</thead>
	<tbody>
		<% 
		var i = 0;
		foreach (var item in Model) {
		i = i + 1;
		}
		if(i>0) {
		foreach (var item in Model) { %>
			<tr teamName="<%: item.TeamName %>" teamEmail="<%: item.TeamEmail %>" teamPhone="<%: item.TeamPhoneNumber %>" IsPrimaryTeamForSub="<%: item.IsPrimaryTeamForSub.ToString().ToLower() %>" id="<%: item.TeamId %>">
				<td><%: item.TeamName %></td>
				<td><img src="../../images/add.png"  /></td>
			</tr>
		<% } 
		} else { %>
			<tr>
				<td colspan="2">No teams found matching your search criteria.</td>
			</tr>
		<% } %>
	</tbody>
</table>