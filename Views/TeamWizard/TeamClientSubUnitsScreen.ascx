<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.TeamClientSubUnitsVM>" %>



 
        
 <div id="divSearch">
     	<table class="" cellspacing="0" width="100%">
        <thead>
        	<tr>
            <th>Search field:</th>
                <th> 
					<select id="ClientFilterField">
					   <option value="ClientTopUnitName">TopUnit Name</option>
					   <option value="ClientTopUnitGuid">TopUnit Guid</option>
					   <option value="ClientSubUnitName" selected="selected">SubUnit Name</option>
					   <option value="ClientSubUnitGuid">SubUnit Guid</option>
					   <option value="Team">Team</option>
					   <option value="Country">Country</option>
					   <option value="GDSCompanyProfileName">GDSCompanyProfileName</option>
					   <option value="PseudoCityCode">PseudoCityCode or OfficeID</option>
				   </select>
                </th>
            	<th>Find Client:</th>
                <th><input type="text" value="" id="ClientFilter"/></th>
                
                <th><span id="ClientSearchButton"><small>Search >> </small></span></th>
    			<th id="lastSearchTRClient"></th>
                <th><div style="float:right;"><span id="clientsInTeamBackButton"><small><< Back</small></span>
        &nbsp;<span id="clientsInTeamNextButton"><small>Next >></small> </span></div> </th>
                </tr>
        </table>
    <%= Html.HiddenFor(model => model.Team.TeamId) %>
  
     </div>

    <table class="tablesorter_other" cellspacing="0">
        <tr>
           
            <td>
                <div id="ClientSubUnitSearchResults"></div>
            </td>


            <td>
                <table id="currentClientsTable" cellspacing="0" class="tablesorter_other2">
                     <thead><tr>
                        <td colspan="5">Current Team Clients</td>
                    </tr>
                   
                    <tr>
                    	<th>Name</th><th>Country</th><th>GUID</th><th title="Include in client drop list">ICDL</th><td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
                     </tr>
                     </thead>
                     <tbody>
                     <% foreach (var item in Model.ClientSubUnits) { 
					 
					 string oldClientGUID = item.ClientSubUnitGuid;

                     string[] newClientGUID = oldClientGUID.Split(':');
					 
					 %> 
                        <tr id="<%: newClientGUID[1] %>" clientSubUnitName="<%=Html.Encode(item.ClientSubUnitName) %>" clientCountry="<% =Html.Encode(item.CountryName) %>" clientSubUnitStatus="Current" inClientDropList="<%: item.IncludeInClientDroplistFlag %>" clientSubUnitGuid="<%=Html.Encode(item.ClientSubUnitGuid) %>" clientSubUnitVersion="<%: item.VersionNumber %>">
                        
                            <td><%=Html.Encode(item.ClientSubUnitName) %> </td><td><%=Html.Encode(item.CountryName) %></td><td><%=Html.Encode(item.ClientSubUnitGuid) %></td>
                            
                            <td id="testTD">
							<% if(item.IncludeInClientDroplistFlag==true) {
								%>
                                <input type="checkbox" checked="checked" id="IncludeInClientDroplist"/>
                                <% }
								
								else {%>
								<input type="checkbox" id="IncludeInClientDroplist" />
                                <% }%>
                            
                            </td>
                            <td><img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" border="0" /></td>
                        </tr>
                        
    
                    <% } %>
                    </tbody>
                </table>
            </td>

        </tr>
         </table>
 



