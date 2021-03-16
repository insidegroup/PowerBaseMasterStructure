<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientSubUnitClientAccountsVM>" %>

        
 <div id="divSearch">

        <div style="float:left"><span id="clientAccountsSaveButton"><small>Save</small></span>&nbsp;<span id="clientAccountsCancelButton"><small>Cancel</small></span></div>    
        <div style="float:right;"><span id="clientAccountsBackButton"><small><< Back</small></span>&nbsp;<span id="clientAccountsNextButton"><small>Next >></small> </span></div>

     	<table class="" cellspacing="0" width="100%" style="padding-top:20px;">
        <thead>
        	<tr>
            <th>Search field:</th>
                <th> 
                    <select id="ClientAccountFilterField1">
                    <option value="PleaseSelect">Please select...</option>
                       <option value="ClientAccountName">Client Account Name</option>
                       <option value="ClientAccountNumber">Client Account Number</option>
                       <option value="CountryName">Country</option>
                       <option value="SourceSystemCode">Source System Code</option>
                    </select>
                    <br /><input type="text" value="" id="ClientAccountFilter1"/>
                    </th>
                    <th>
                    <select id="ClientAccountFilterField2">
                     <option value="PleaseSelect">Please select...</option>
                       <option value="ClientAccountName">Client Account Name</option>
                       <option value="ClientAccountNumber">Client Account Number</option>
                       <option value="CountryName">Country</option>
                       <option value="SourceSystemCode">Source System Code</option>
                    </select>
                    <br /><input type="text" value="" id="ClientAccountFilter2"/>
                    </th>
                    <th>
                    <select id="ClientAccountFilterField3">
                     <option value="PleaseSelect">Please select...</option>
                       <option value="ClientAccountName">Client Account Name</option>
                       <option value="ClientAccountNumber">Client Account Number</option>
                       <option value="CountryName">Country</option>
                       <option value="SourceSystemCode">Source System Code</option>
                    </select>
                    <br /><input type="text" value="" id="ClientAccountFilter3"/>
                </th>
                
                <th><span id="clientAccountsSearchButton"><small>Search >> </small></span></th>
    			<th><span id="lastSearchAccount"></span></th>
                </tr>
                </thead>
        </table>

     </div>

    <table class="tablesorter_other" cellspacing="0">
        <tr>
           
            <td>
                <div id="ClientAccountSearchResults"></div>
            </td>


            <td>
                <table id="currentAccountsTable" cellspacing="0" class="tablesorter_other2">
                <thead>
                     <tr>
                        <td colspan="6">Current Accounts</td>
                    </tr>
                   <tr>
                   	<th>Account Name</th><th>Acc No</th><th title="System Source Code">SSC</th><th>Country</th><th>Default?</th><td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
                   </tr>
                   
                  </thead>
                  <tbody>
                    <% 
					var count = Model.ClientAccounts.Count;
					foreach (var item in Model.ClientAccounts) {
						%>
                        
                        <tr id="<%: item.ClientAccountNumber %>" SSC="<%: item.SourceSystemCode %>" combinedAccSSC="<%: item.ClientAccountNumber + item.SourceSystemCode %>" accountStatus="Current" versionNumber="<%: item.VersionNumber %>">
                        	<td><%: item.ClientAccountName %></td>
                            <td><%: item.ClientAccountNumber %></td>
                            <td><%: item.SourceSystemCode %></td>
                            <td><%: item.CountryName %></td>
                            <td><%: item.DefaultFlag ? "True" : "False" %></td>
                            <td><img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" border="0" /></td>
                         </tr>
				<%
					}
					%>
                   
                   
                 </tbody>   
                </table>
                
            </td>

        </tr>
         </table>
 



