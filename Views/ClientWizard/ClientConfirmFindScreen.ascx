<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientTopUnitAndClientSubUnitsVM>" %>

<% 
    if(Model.ClientTopUnit != null){
		
		%>

		<h4>Please confirm your selection for: <%: Model.ClientTopUnit.ClientTopUnitName %> </h4>
        
       <table id="clientConfirmSelection" class="tablesorter_other2" cellspacing="0">
       	<thead>
        	<tr>
            	<th>ClientSubUnit Name</th><th>ClientSubUnitGuid</th><th>Country</th><td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
            </tr>
        </thead>
		<tbody>        
    <% 

    
    foreach (var clientSubUnit in Model.ClientSubUnits) {
		%> 
     <tr>
     	<td><%: clientSubUnit.ClientSubUnitName %></td><td><%: clientSubUnit.ClientSubUnitGuid %></td><td><%: clientSubUnit.CountryName %></td><td><a href="javascript:ShowClientDetailsScreen('<%: clientSubUnit.ClientSubUnitGuid %>');">Select</a></td>
     </tr>
       
   <% } 
%>
</tbody>
</table>
<% } %>
