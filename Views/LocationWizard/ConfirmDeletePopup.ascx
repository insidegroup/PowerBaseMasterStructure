<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.LocationSystemUsersVM>" %>


   <h2>Location Details</h2>
    
 
    <div id="divClientSubUnit">
    <table width="100%" class="tablesorter_other3">
        <tr>


            <td>
                <table width="100%">
                    <tr>
                        <th>
                            Attached SystemUsers
                        </th>

                    </tr>
                    <% if(Model.SystemUsers.Count>0) { %>
                    
                     <% foreach (var item in Model.SystemUsers) { %> 
                        <tr>
                            <td>
                                <%: item.LastName %>,<%if(item.MiddleName!=""){%><% =item.MiddleName + " "%> <%}%><%: " " + item.FirstName %>
                            </td>
                        </tr>
    
                    <% } } 
					
					else { %>
                    <tr>
                    	<td> None </td>
                     </tr>
                     <% } %>
                </table>
            </td>
         
        </tr>
         </table>
         <input type="hidden" id="LocationTeamCount" name="LocationTeamCount" value="<%: Model.LocationTeamCount%>" />
    </div>
    


