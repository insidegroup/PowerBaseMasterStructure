<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.TeamClientSubUnitsAndSystemUsersVM>" %>

   <h2>Team Details</h2>
    
 
    <div id="divClientSubUnit">
    <table width="100%" class="tablesorter_other3">
        <tr>

            <td>
                <table width="100%">
                    <tr>
                        <th>
                            Attached ClientSubUnits
                        </th>

                    </tr>
                     <% 
					 var csu = 0;
					 
					 foreach (var item in Model.ClientSubUnits) { %> 
                        <tr>
                            <td>
                                <%: item.ClientSubUnitName%>
                            </td>
                        </tr>
    
                    <% csu = csu + 1;
					 } %>
                     <% if(csu>0) {
						 %>
						 
                     <tr><td>[Total: <%: csu %>]</td></tr>
                    <% }
					
					else {  %>
                    	<tr><td>None</td></tr>
                        <% } %>
                </table>
            </td>

            <td>
                <table width="100%">
                    <tr>
                        <th>
                            Attached SystemUsers
                        </th>

                    </tr>
                     <% 
					 var asu = 0;
					 foreach (var item in Model.SystemUsers) { %> 
                        <tr>
                            <td>
                                <%: item.LastName %>,<%if(item.MiddleName!=""){%><% =item.MiddleName + " "%> <%}%><%: " " + item.FirstName %>
                            </td>
                        </tr>
    
                    <% asu = asu + 1; 
					} %>
                     <% if(asu>0) {
						 %>
						 
                     <tr><td>[Total: <%: asu %>]</td></tr>
                    <% }
					
					else {  %>
                    	<tr><td>None</td></tr>
                        <% } %>
                </table>
            </td>
         
        </tr>
         </table>

    </div>
    


