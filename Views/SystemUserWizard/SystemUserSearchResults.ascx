<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectSystemUsersFiltered_v1Result>>" %>
<style type="text/css">
tr.over td
  { background-color:#FFFF99;}
</style>
<%
var i = Model.Count();
if(i>0){
 %>
    <table class="tablesorter_other2" cellspacing="0" id="teamUsersSearchResult">
	    <thead>
            <tr>
    	        <th>Name</th><th>Login</th><th>Profile ID</th><td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
            </tr>
        </thead>
        <tbody>
       
            <% foreach (var item in Model) { %> 
            <tr userFirstName="<%: item.FirstName %>" userLastName="<%: item.LastName %>" networkLogin="<%: item.SystemUserLoginIdentifier %>" userGUID="<%: item.SystemUserGuid %>">
                <td><%: Server.HtmlEncode(item.LastName) %>,<%if(item.MiddleName!=""){%><% =Server.HtmlEncode(item.MiddleName) + " "%> <%}%><%: " " + Server.HtmlEncode(item.FirstName) %></td>
                <td><%: Server.HtmlEncode(item.SystemUserLoginIdentifier) %></td>
                <td><%: Server.HtmlEncode(item.UserProfileIdentifier)%></td>
                <td><a href="javascript:setWizardUser('<%: item.SystemUserGuid %>');">Select</a></td>
            </tr>
    
        <% } %>
    
        </tbody>
    </table>
 [Results: <%: i%> ]
<% }else {%>
       <h4>No results matching your search criteria and/or user rights.</h4>
<% } %>