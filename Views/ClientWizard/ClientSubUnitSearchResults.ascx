<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectClientTopUnitSubUnitFiltered_v1Result>>" %>
<style>
tr.over td
  { background-color:#FFFF99;
	}


</style>


 <%if (Model.Count() > 0)
   { %>
 
    <table width="100%" id="clientTable" class="tablesorter_other2" cellspacing="0">
    <thead>
        <tr>
            <th id="csTH1">Subunit Name
            </th>
            <th id="csTH2">GUID
                
            </th>
            <th id="csTH3">
                
            </th>
            <th id="csTH4">
                
            </th>
             <th id="csTH5">Country
             </th>
             <td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
        </tr>
	</thead>
	<tbody>
    <% foreach (var item in Model) { %>
    
   
        <tr>
			<td <%= Html.Raw(item.ExpiryDate != null && item.ExpiryDate.Value <= DateTime.Now ? " class=\"expired\"" : String.Empty) %>>
                 <%: item.ClientSubUnitName %>
            </td>
            <td>
                <%: item.ClientSubUnitGuid %>
            </td>
            <td>
      			
            </td>
            <td>
				
            </td>
            <td>
                <%: item.CountryName %>
            </td>
             <td>
                <a href="javascript:SetWizardClientSubUnit('<%: item.ClientSubUnitGuid %>')">Select</a>
            </td>
        </tr>
       
        
    <% } %>
	
   
	</tbody>
    </table>
   <p>[Results: <%: Model.Count() %> ]</p> 
   <% } else { %>
       <h4>No results matching your search criteria and/or user rights.</h4>
       
       <% } %>

