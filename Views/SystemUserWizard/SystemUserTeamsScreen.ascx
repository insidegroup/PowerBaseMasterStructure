<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.SystemUserTeamsVM>" %>




        
 <div id="divSearch">
     	<table class="" cellspacing="0" width="100%">
        <thead>
        	<tr>
            <th>Search field:</th>
                <th> <select id="TeamFilterField">
       <option value="TeamName">Team Name</option>
       <option value="Global">Global Teams</option>
       <option value="GlobalRegion">GlobalRegion Teams</option>
       <option value="GlobalSubRegion">GlobalSubRegion Teams</option>
       <option value="Country">Country Teams</option>
       <option value="CountryRegion">CountryRegion Teams</option>
       <option value="Location">Location Teams</option>
       </select></th>
            	<th>Find Team:</th>
                <th><input type="text" value="" id="TeamFilter"/></th>
                
                <th><span id="TeamSearchButton"><small>Search >> </small></span></th>
    			<th id="lastSearchTRTeam"></th>
                <th><div style="float:right;"><span id="temasInSystemUserBackButton"><small><< Back</small></span>
        &nbsp;<span id="temasInSystemUserNextButton"><small>Next >></small> </span></div> </th>
                </tr>
        </table>
    <%= Html.HiddenFor(model => model.SystemUser.SystemUserGuid) %>

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
                        <td colspan="3">Current Teams</td>
                    </tr>
                   <tr>
                   	<th>Team Name</th>
                    <th>Hierarchy</th>
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
						foreach (var item in Model.Teams) {

				%>
                    <tr teamStatus="Current" teamName="<%: item.TeamName %>" teamEmail="<%: item.TeamEmail %>" teamPhone="<%: item.TeamPhoneNumber %>" id="<%: item.TeamId %>">
                        <td><%=Html.Encode(item.TeamName)%></td>
                        <td><%: item.HierarchyInfo %></td>
                        <td><%if((bool)item.HasWriteAccess){ %><img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" /><%} %></td>
                    </tr>
    
                    <% } }
					else {
					%>
                    <tr id="noTeams">
                        <td colspan="3">User not in any teams at present.</td>
                    </tr>
                <%	
						
						
					}%>
                 </tbody>   
                </table>
            </td>

        </tr>
         </table>
 



