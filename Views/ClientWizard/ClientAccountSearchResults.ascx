<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CWTDesktopDatabase.Models.spDDAWizard_SelectClientAccountsFiltered_v1Result>>" %>
<style>
tr.over td
  { background-color:#FFFF99;
	}


</style>


 <%if (Model.Count() > 0)
   { %>
 
    <table width="100%" id="searchResultClientTable" class="tablesorter_other2" cellspacing="0">
    <thead>
    <tr><td colspan="4">Available accounts</td></tr>
        <tr>
        	
            <th id="csTH1">Account Name</th>
            <th>Acc No / SSC</th>
           	<th>Country</th>
             <td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
        </tr>
	</thead>
	<tbody>
    <% foreach (var item in Model) { %>
    
   
        <tr id="<%: item.ClientAccountNumber %>" sourceSystemCode="<%: item.SourceSystemCode %>" combinedAccSSC="<%: item.ClientAccountNumber + item.SourceSystemCode %>" country="<%: item.CountryName %>" accountName="<%: item.ClientAccountName %>">
            <td>
                 <%: item.ClientAccountName %> 
            </td>
           <td><%: item.ClientAccountNumber %>/<%:item.SourceSystemCode %></td>
           <td><%: item.CountryName %></td>
             <td>
                <img src="<%=Url.Content("~/Images/add.png")%>" alt="add" border="0" />
            </td>
        </tr>
       
        
    <% } %>
	
   
	</tbody>
    </table>
   <p>[Results: <%: Model.Count() %> ]</p> 
   <% } else { %>
       <h4>No results matching your search criteria and/or user rights.</h4>
       
       <% } %>

