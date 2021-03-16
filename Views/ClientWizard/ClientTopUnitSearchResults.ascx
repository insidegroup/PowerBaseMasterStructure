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
            <th id="csTH1">
            </th>
            <th id="csTH2">
                
            </th>
            <th id="csTH3">
                
            </th>
            <th id="csTH4">
                
            </th>
             <th id="csTH5">
             </th>
             <td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
        </tr>
	</thead>
	<tbody>
    <% foreach (var item in Model) { %>
        <tr>
            <td <%= Html.Raw(item.ExpiryDate != null && item.ExpiryDate.Value <= DateTime.Now ? " class=\"expired\"" : String.Empty) %>>
                <%: item.ClientTopUnitName %>
            </td>
           
            <td>
                <%: item.ClientTopUnitGuid %>
            </td>
           
                
            <td>
            </td>
                
            <td>
            </td>
                
            <td>
            </td>
             <td>
                <a href="javascript:SetWizardClientTopUnit('<%: item.ClientTopUnitName.Replace("\\", "\\\\").Replace("'", "\\'")%>','<%: item.ClientTopUnitGuid %>')">Select</a>
            </td>
        </tr>   
    <% } %>
	
   
	</tbody>
    </table>
   <p>[Results: <%: Model.Count() %> ]</p> 
   <% } else { %>
       <h4>No results matching your search criteria and/or user rights.</h4>
       
       <% } %>

