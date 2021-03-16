<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectSystemUsersFiltered_v1Result>>" %>
<style type="text/css">
tr.over td
  { background-color:#FFFF99;
	}
</style>

<table class="tablesorter_other2" cellspacing="0" id="locationUsersSearchResult">
	<thead>
        <tr>
            <td colspan="4">Available SystemUsers</td>
        </tr>
        <tr>
    	    <th>Name</th>
            <th>Login</th>
			<th>Profile ID</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
       
        <% 
            var i = Model.Count();
            if (i > 0) {
                foreach (var item in Model){ 
            %> 
        <tr userFirstName="<%: item.FirstName %>" userLastName="<%: item.LastName %>" networkLogin="<%: item.SystemUserLoginIdentifier %>" userGUID="<%: item.SystemUserGuid %>" userProfileIdentifier="<%: Server.HtmlEncode(item.UserProfileIdentifier) %>">
            <td><%: Server.HtmlEncode(item.LastName) %>,<%if(item.MiddleName!=""){%><% =Server.HtmlEncode(item.MiddleName) + " "%> <%}%><%: " " + Server.HtmlEncode(item.FirstName) %></td>
            <td><%: Server.HtmlEncode(item.SystemUserLoginIdentifier) %></td>
			<td><%: Server.HtmlEncode(item.UserProfileIdentifier) %></td>
            <td><img src="<%=Url.Content("~/Images/add.png")%>" alt="add" border="0" /></td>
         </tr>
            <%
                } 
            }else {
            %>
            <tr>
                <td colspan="4">No results matching your search criteria and/or user rights.</td>
            </tr>
            <%	
	        }
            %>
    </tbody>
</table>