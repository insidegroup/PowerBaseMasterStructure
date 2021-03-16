<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.TeamSystemUsersVM>" %>

<div id="divSearch">
    <table class="" cellspacing="0" width="100%">
        <thead>
            <tr>            	
                <th>Search field:</th>
                <th><select id="FilterField">
                   <option value="LastName">Last Name</option>
                   <option value="NetworkLogin">Network Login</option>
                   <option value="Team">Team</option>
                   <option value="Location">Location</option>
                   <option value="CountryRegion">CountryRegion</option>
                   <option value="Country">Country</option>
                   </select></th>
                <th>Find SystemUser:</th>
                <th><input type="text" value="" id="Filter"/></th>            
                <th><span id="searchForSystemUserButton"><small>Search >> </small></span></th>
                <th id="lastSearchTR"></th>
                <th><div style="float:right;"><span id="teamUsersBackButton"><small><< Back</small></span>&nbsp;<span id="teamUsersNextButton"><small>Next >></small> </span></div></th>	
            </tr>
        </thead>
    </table>  
    </div> 
    <%= Html.HiddenFor(model => model.Team.TeamId) %>


    
<table class="tablesorter_other" cellspacing="0">
    <tr>
        <td><div id="SystemUserSearchResults"></div></td>          
        <td>
            <table class="tablesorter_other2" cellspacing="0" id="teamCurrentUsers">
                <thead>
                    <tr>
                        <td colspan="6">Current Team Members</td>
                    </tr>
                    <tr>
						<th>Name</th>
						<th>Login</th>
						<th>Profile ID</th>
						<th>Date added</th>
						<th>Added By</th>
						<td style="background-color: #007886; border: 0; font-size: 10pt; padding: 4px; color: #000000; color: #FFF;"></td>
                    </tr>
                </thead>
                <tbody>               
                <% 
				foreach (var item in Model.SystemUsers) { 					 
				%> 
                    <tr userStatus="Current" id="<%: item.SystemUserLoginIdentifier %>" userGUID="<%: item.SystemUserGuid%>">
                        <td>
                            <%=Server.HtmlEncode(item.LastName) %>,<%if(item.MiddleName!=""){%><%=Server.HtmlEncode(item.MiddleName) + " "%> <%}%><%: " " + Server.HtmlEncode(item.FirstName) %>
                        </td>
                        <td><%=Server.HtmlEncode(item.SystemUserLoginIdentifier)%></td>
						<td><%=Server.HtmlEncode(item.UserProfileIdentifier)%></td>
						<td><%= Html.Encode(item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString("MMM dd, yyyy") : "")%></td>
                        <td><%: item.CreationUserIdentifier %></td>
                        <td><img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" border="0" /></td>

                    </tr>
                <% }  %>
                </tbody>
            </table>
        </td>
    </tr>
</table>
    



