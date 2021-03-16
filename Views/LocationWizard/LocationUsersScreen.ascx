<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.LocationSystemUsersVM>" %>



       
         <input type="hidden" id="locationUsersCount" value="<%=Model.SystemUsers.Count%>"  />
  	    <div id="divSearch">
     	<table class="tablesorter" cellspacing="0">
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
                <th><div style="float:right;"> <span id="locationUsersBackButton"><small><< Back</small></span>
         &nbsp;<span id="locationUsersNextButton"><small>Next >></small> </span></th>
    			</tr>
        </table>
    <%= Html.HiddenFor(model => model.Location.LocationId) %>

    
    </div>
    
	  
 
  
    <table class="tablesorter_other" cellspacing="0">
        <tr>
           
            <td>
                <div id="SystemUserSearchResults"></div>
            </td>
            
            <td>
                <table class="tablesorter_other2" cellspacing="0" id="locationCurrentUsers">
                	<thead>
                    <tr>
                        <td colspan="6">Current Location Members</td>
                    </tr>
                    <tr>
                    	<th>Name</th>
                        <th>Login</th>
						<th>Profile ID</th>
                        <th>Date added</th>
                        <th>Added By</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>                
                     <% 
				 	 foreach (var item in Model.SystemUsers) { 
					 %> 
                        <tr userStatus="Current" id="<%: item.SystemUserLoginIdentifier %>" userGUID="<%: item.SystemUserGuid%>">
                            <td><%: Html.Encode(item.LastName) %>,<%if(item.MiddleName!=""){%><% =item.MiddleName + " "%> <%}%><%: " " + item.FirstName %></td>
                            <td><%: Html.Encode(item.SystemUserLoginIdentifier)%></td>
							<td><%: Html.Encode(item.UserProfileIdentifier)%></td>
                            <td><%= Html.Encode(item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString("MMM dd, yyyy") : "")%></td>
                            <td><%: Html.Encode(item.LastUpdateUserIdentifier)%></td>
                            <td><img src="<%=Url.Content("~/Images/remove.png")%>" title="remove" border="0" alt="remove"/></td>
                        </tr>    
                    <%
                    }
                    %>
                    </tbody>
                </table>
            </td>
             
        </tr>
         </table>

         <div id="noLocationUsers"></div>






