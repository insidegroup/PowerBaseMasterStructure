<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

       


  <div id="divSearch">
   <table class="tablesorter" cellspacing="0">
        <thead>
        	<tr><th>Search field:</th>
                <th><select id="FilterField">
                   <option value="LastName">Last Name</option>
                   <option value="NetworkLogin">Network Login</option>
                   <option value="Team">Team</option>
                   <option value="Location">Location</option>
                   <option value="CountryRegion">CountryRegion</option>
                   <option value="Country">Country</option>
                   </select></th>
                <th>Find System User: &nbsp; <input type="text" value="" id="Filter"/>&nbsp;<span id="SearchButton"><small>Search >> </small></span></th>
                <th id="lastSearchTS"></th>
		        <th></th>
            </tr>
         </thead> 
         </table>
    </div>
 
    <div id="SystemUserSearchResults">

    <p>Please complete the form above to do a search for system users.</p>
       
   
    </div>
   



