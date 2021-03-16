<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientSubUnitTeamsVM>" %>

<div style="float:left"><span id="clientTeamsCancelButton"><small>Cancel</small></span>&nbsp;<span id="clientTeamsSaveButton"><small>Save</small></span></div>    
<div style="float:right;"><span id="clientTeamsBackButton"><small><< Back</small></span>&nbsp;<span id="clientTeamsNextButton"><small>Next >></small> </span></div>

<br />
<br />

 <div id="divSearch" style="margin-top:20px;">
     	<table class="" cellspacing="0" width="100%">
			<thead>
        		<tr>
					<th>Search field:</th>
					<th> 
						<select id="TeamFilterField">
							<option value="TeamName">Team Name</option>
							<option value="Location">Location Teams</option>
						</select>
					</th>
            		<th>Find Team:</th>
					<th><input type="text" value="" id="TeamFilter"/></th>
					<th><span id="clientTeamsSearchButton"><small>Search >> </small></span></th>
    				<th id="lastSearchTRTeam"></th>
                </tr>
			</thead>
        </table>
		<%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid)%>
	</div>

	<table class="tablesorter_other" cellspacing="0">
        <tr>
            <td>
                <div id="TeamSearchResults"></div>
            </td>
            <td>
                
				<table id="currentTeamsTable" cellspacing="0" class="tablesorter_other2">
					<thead>
						<tr>
							<td>Current Teams</td>
							<td colspan="4" align="right"><span id="IsPrimaryTeamForSubError" class="error" style="display:none;">There is more than one Team for this SubUnit, please indicate which is Primary.</span></td>
						</tr>
						<tr>
							<th>Team Name</th>
							<th>Team eMail</th>
							<th>Team Phone</th>
							<th>Primary</th>
							<td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
						</tr>
					</thead>
					<tbody>
						<% 
						var t = 0;
						foreach (var item in Model.Teams) {
							t = t + 1;
						}
						if(t>0) {
							foreach (var item in Model.Teams) { %>
								<tr teamStatus="Current" teamName="<%: item.TeamName %>" teamEmail="<%: item.TeamEmail %>" teamPhone="<%: item.TeamPhoneNumber %>" IsPrimaryTeamForSub="<%: item.IsPrimaryTeamForSub.ToString().ToLower() %>" id="<%: item.TeamId %>" includeInClientDroplistFlag='<%: item.IncludeInClientDroplistFlag%>' versionNumber='<%: item.VersionNumber%>'>
									<td><%: item.TeamName%></td>
									<td><%: item.TeamEmail %></td>
									<td><%: item.TeamPhoneNumber %></td>
									<td><input type="checkbox" value="true" <%if(item.IsPrimaryTeamForSub == true) { %>checked="checked"<% } %> class="UpdateIsPrimaryTeamForSub" /></td>
									<td><img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" /></td>
								</tr>
							<% } 
						} else { %>
							<tr id="noTeams">
								<td colspan="5">Client not in any teams at present.</td>
							</tr>
						<% } %>
					</tbody>   
                </table>
            </td>
        </tr>
	</table>