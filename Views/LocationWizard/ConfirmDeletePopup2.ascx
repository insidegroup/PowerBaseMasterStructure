<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.LocationTeamsVM>" %>


   <h2>Location Details</h2>
    <% bool hasAttachedItems = false; %>
 
    <div id="divLocationTeams">
    <table width="100%" class="tablesorter_other3">
        <tr>


            <td>
                <table width="100%">
                    <tr>
                        <th>
                            Attached Teams
                        </th>

                    </tr>
                    <% if(Model.Teams.Count>0) { 
                           hasAttachedItems = true;%>
                    
                     <% foreach (var item in Model.Teams) { %> 
                        <tr>
                            <td>
                                <%: item.TeamName %>
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
         <input type="hidden" id="LinkedItemsCount" name="LinkedItemsCount" value="<%: Model.LinkedItemsCount%>" />
    </div>
    


