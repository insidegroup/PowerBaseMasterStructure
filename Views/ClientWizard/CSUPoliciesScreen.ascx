<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientSubUnitPoliciesVM>" %>
 <div id="divSearch">
    <table class="" cellspacing="0" width="100%">
    <thead>
        <tr>
            <th><div style="float:left;"><span id="clientPoliciesSaveButton"><small>Save</small></span>&nbsp;<span id="clientPoliciesCancelButton"><small>Cancel</small> </span></div> </th>
            <th><div style="float:right;"><span id="clientPoliciesBackButton"><small><< Back</small></span>&nbsp;<span id="clientPoliciesNextButton"><small>Client Wizard Home</small> </span></div> </th>
        </tr>
    </thead>
    </table>
</div>

<input type="hidden" id="Inherited" <%if (Model.PolicyGroup.InheritFromParentFlag) { %> value="Yes" <%} else { %> value="No" <% } %>  />  
<%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupId)%>
<%= Html.HiddenFor(model => model.PolicyGroup.VersionNumber)%>
<table class="tablesorter_other" cellspacing="0" width="100%" border="1">
    <tr>
        <td>Policies</td>
    </tr>
    <tr>
        <td>Inherit from Parent: <%= Html.CheckBoxFor(model => model.PolicyGroup.InheritFromParentFlag)%><span id="inheritCBChange"><img src="<%=Url.Content("~/Images/Common/grid-loading.gif")%>"  title="Please Wait  alt="Please Wait"/> please wait</span></td>
    </tr>

	<% if (Model.PolicyAirAdvancePurchaseGroupItemDisplayFlag != null && Model.PolicyAirAdvancePurchaseGroupItemDisplayFlag == true)
	{ %>
		<tr>
			<td>
				<table id="currentPolicyAirAdvancePurchaseGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
				<thead>          
					<tr>
						<th colspan="4"><%=Model.PolicyAirAdvancePurchaseGroupItemDisplayTitle %></th>
						<th align="right"><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyAirParameterGroupItem('0', '2')"><img src="<%=Url.Content("~/images/Common/addButton.jpg")%>" title="Add new Air Advance Purchase Item" alt="add" border="0"  style="float:right" /></a><%}%></th>
					</tr>  
					<tr>
                		<td>Advance Purchase Days</td>
						<td>Routing</td>
						<td width="12%">Source</td>
<%--						<td colspan="2">Travel date valid from/to</td>--%>
                        <td>Policy Group Name</td>
						<td align="center" width="13%">Actions</td>
					</tr>				                
				</thead>
				<tbody>
				<% 
				if (Model.PolicyAirAdvancePurchaseGroupItems != null){
					foreach (var item in Model.PolicyAirAdvancePurchaseGroupItems){                 
				%>
					<tr PolicyAirParameterGroupItemId="<%=item.PolicyAirParameterGroupItemId%>" Source="<%:item.Source %>" PolicyGroupId="">
						<td><%: Html.Encode(item.PolicyAirParameterValue) %></td>
<%--						<td><%: Html.Encode(item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "")%></td>  
						<td><%: Html.Encode(item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "")%></td>  --%>
                        <td><%if (item.PolicyRoutingId != null) { %><%:Html.Encode(item.FromCode)%>  to  <%:Html.Encode(item.ToCode)%><%} %></td>
						<td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
						<td><%: Html.Encode(item.PolicyGroupName) %></td>
						<%if (item.Source != "Custom"){ %>
							<td>Inherited</td>
						<%}else{%>
							<%if (Model.HasPolicyGroupWriteAccess){%>
								  <td>
									  <a href="javascript:AddEditPolicyAirParameterGroupItem('<% =item.PolicyAirParameterGroupItemId%>', '<%:item.PolicyAirParameterTypeId %>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;
									  <a href="javascript:DeletePolicyAirParameterGroupItemPopup('<%:item.PolicyAirParameterGroupItemId%>', '<%:item.PolicyAirParameterTypeId %>', '<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
							<% }else { %>
								  <td>Read Only</td>
							<% }  %>
						<%}%> 
					</tr>
				<%        
					}
				}
				%>                  
                
				</tbody>   
				</table> 
			</td>
		</tr>
	<% } %>

	<% if (Model.PolicyAirCabinGroupItemDisplayFlag != null && Model.PolicyAirCabinGroupItemDisplayFlag == true) { %>
		<tr>
			<td>
				<table id="currentPolicyAirCabinGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
				<thead>          
					<tr>
						<th colspan="9"><%=Model.PolicyAirCabinGroupItemDisplayTitle %></th>
						<th align="right"><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyAirCabinGroupItem('0')"><img src="<%=Url.Content("~/images/Common/addButton.jpg")%>" title="Add new Air Cabin Policy Item" alt="add" border="0"  style="float:right" /></a><%}%></th>
					</tr>  
					<tr>
                		<td>Cabin</td>
						<td colspan="2">Duration Min/Max</td>
						<td colspan="2">Mileage Min/Max</td>
<%--						<td>Policy Prohibited</td>--%>
						<td>Routing</td>
						<td>Vice Versa</td>
						<td width="12%">Source</td>
                        <td>Policy Group Name</td>
						<td align="center" width="13%">Actions</td>
					</tr>				                
				</thead>
				<tbody>
				<% 
				if (Model.PolicyAirCabinGroupItems != null){
					foreach (var item in Model.PolicyAirCabinGroupItems){                 
				%>
					<tr PolicyAirCabinGroupItemId="<%=item.PolicyAirCabinGroupItemId%>" Source="<%:item.Source %>" PolicyGroupId="">
						<td><%: Html.Encode(item.AirlineCabinCode) %></td>
						<td><%: Html.Encode(item.FlightDurationAllowedMin) %></td>
						<td><%: Html.Encode(item.FlightDurationAllowedMax)%></td>
						<td><%: Html.Encode(item.FlightMileageAllowedMin) %></td>
						<td><%: Html.Encode(item.FlightMileageAllowedMax)%></td>
<%--						<td><%: Html.Encode(item.PolicyProhibitedFlag)%></td>--%>
						<td><%if (item.PolicyRoutingId != null)
							  { %><%:Html.Encode(item.FromCode)%>  to  <%:Html.Encode(item.ToCode)%><%} %></td>                 
						<td><%: Html.Encode(item.RoutingViceVersaFlag)%></td>
						<td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                        <td><%: Html.Encode(item.PolicyGroupName)%></td>
						<%if (item.Source != "Custom"){ %>
							<td>Inherited</td>
						<%}else{%>
							<%if (Model.HasPolicyGroupWriteAccess){%>
								  <td><a href="javascript:AddEditPolicyAirCabinGroupItem('<% =item.PolicyAirCabinGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyAirCabinGroupItemPopup('<%:item.PolicyAirCabinGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
							<% }else { %>
								  <td>Read Only</td>
							<% }  %>
						<%}%> 
					</tr>
				<%        
					}
				}
				%>                  
                
				</tbody>   
				</table> 
			</td>
		</tr>
	<% } %>
    
	<% if (Model.PolicyAirMissedSavingsThresholdGroupItemDisplayFlag != null && Model.PolicyAirMissedSavingsThresholdGroupItemDisplayFlag == true) { %>
		<tr>
        <td>
            <table id="currentPolicyAirMissedSavingsThresholdGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="5"><%=Model.PolicyAirMissedSavingsThresholdGroupItemDisplayTitle %></th>
                    <th align="right"><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyAirMissedSavingsThresholdGroupItem('0')"><img src="<%=Url.Content("~/images/Common/addButton.jpg")%>" title="Add new Air Cabin Policy Item" alt="add" border="0"  style="float:right" /></a><%}%></th>
                </tr>  
				<tr>
                	<td width="10%">Amount</td>
                    <td>Currency</td>
                    <td>Routing</td>
                    <td>Source</td>
                    <td>Policy Group Name</td>
<%--                    <td colspan="2">Travel date valid from/to</td>--%>
                    <td align="center" width="13%">Actions</td>
                </tr>				                
            </thead>
            <tbody>
            <% 
                if (Model.PolicyAirMissedSavingsThresholdGroupItems != null)
                {
                foreach (var item in Model.PolicyAirMissedSavingsThresholdGroupItems){                 
            %>
			    <tr PolicyAirMissedSavingsThresholdGroupItemId="<%=item.PolicyAirMissedSavingsThresholdGroupItemId%>" Source="<%:item.Source %>" PolicyGroupId="">
                    <td><%: Html.Encode(item.MissedThresholdAmount != null ? item.MissedThresholdAmount.Value.ToString("N2") : "")%></td>                   
                    <td><%: Html.Encode(item.CurrencyCode) %></td>
                    <td><%: Html.Encode(item.RoutingCode)%></td>
                    <td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%: Html.Encode(item.PolicyGroupName)%></td>
<%--                    <td width="10%"><%if (item.TravelDateValidFrom.HasValue){ %><%: Html.Encode(item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy"))%><%}%></td>
                    <td width="10%"><%if (item.TravelDateValidTo.HasValue){ %><%: Html.Encode(item.TravelDateValidTo.Value.ToString("MMM dd, yyyy"))%><%}%></td> --%>
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                              <td><a href="javascript:AddEditPolicyAirMissedSavingsThresholdGroupItem('<% =item.PolicyAirMissedSavingsThresholdGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyAirMissedSavingsThresholdGroupItemPopup('<%:item.PolicyAirMissedSavingsThresholdGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
                
            </tbody>   
            </table> 
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyAirTimeWindowGroupItemDisplayFlag != null && Model.PolicyAirTimeWindowGroupItemDisplayFlag == true)
	{ %>
		<tr>
			<td>
				<table id="currentPolicyAirTimeWindowGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
				<thead>          
					<tr>
						<th colspan="6"><%=Model.PolicyAirTimeWindowGroupItemDisplayTitle %></th>
						<th align="right"><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyAirParameterGroupItem('0', '1')"><img src="<%=Url.Content("~/images/Common/addButton.jpg")%>" title="Add new Air Advance Purchase Item" alt="add" border="0"  style="float:right" /></a><%}%></th>
					</tr>  
					<tr>
                		<td>Time Window Minutes</td>
						<td>Routing</td>
						<td colspan="2">Travel date valid from/to</td>
						<td width="12%">Source</td>
                        <td>Policy Group Name</td>
						<td align="center" width="13%">Actions</td>
					</tr>				                
				</thead>
				<tbody>
				<% 
				if (Model.PolicyAirTimeWindowGroupItems != null){
					foreach (var item in Model.PolicyAirTimeWindowGroupItems){                 
				%>
					<tr PolicyAirParameterGroupItemId="<%=item.PolicyAirParameterGroupItemId%>" Source="<%:item.Source %>" PolicyGroupId="">
						<td><%: Html.Encode(item.PolicyAirParameterValue) %></td>
						<td><%if (item.PolicyRoutingId != null) { %><%:Html.Encode(item.FromCode)%>  to  <%:Html.Encode(item.ToCode)%><%} %></td>
						<td><%: Html.Encode(item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "")%></td>  
						<td><%: Html.Encode(item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "")%></td>  
						<td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                        <td><%: Html.Encode(item.PolicyGroupName) %></td>
						<%if (item.Source != "Custom"){ %>
							<td>Inherited</td>
						<%}else{%>
							<%if (Model.HasPolicyGroupWriteAccess){%>
								  <td>
									  <a href="javascript:AddEditPolicyAirParameterGroupItem('<% =item.PolicyAirParameterGroupItemId%>', '<%:item.PolicyAirParameterTypeId %>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;
									  <a href="javascript:DeletePolicyAirParameterGroupItemPopup('<%:item.PolicyAirParameterGroupItemId%>', '<%:item.PolicyAirParameterTypeId %>', '<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
								  </td>
							<% }else { %>
								  <td>Read Only</td>
							<% }  %>
						<%}%> 
					</tr>
				<%        
					}
				}
				%>                  
                
				</tbody>   
				</table> 
			</td>
		</tr>
	<% } %>

	<% if (Model.PolicyAirVendorGroupItemDisplayFlag != null && Model.PolicyAirVendorGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyAirVendorGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="6"><%=Model.PolicyAirVendorGroupItemDisplayTitle %></th>
                    <th align="right"><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyAirVendorGroupItem('0');"><img src="<%=Url.Content("~/images/Common/addButton.jpg")%>" title="Add" border="0" style="float:right" alt="Add"/></a><%}%></th>
                </tr> 
                <tr>
                	<td>Vendor</td>
                    <td>Status</td>
                    <td>Ranking</td>
                    <td>Routing</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td> 
                </tr>               
            </thead>
            <tbody>
            <% 
            if (Model.PolicyAirVendorGroupItems != null){
                foreach (var item in Model.PolicyAirVendorGroupItems){                 
            %>
				<tr PolicyAirVendorGroupItemId = '<%: item.PolicyAirVendorGroupItemId %>' Source='<%:Html.Encode(item.Source)%>'>
                    <td><%:Html.Encode(item.SupplierName)%></td>
                    <td><%:Html.Encode(item.PolicyAirStatusDescription)%></td>
                    <td><%:Html.Encode(item.AirVendorRanking)%></td>
                    <td><%:Html.Encode(item.FromCode)%>  to  <%:Html.Encode(item.ToCode)%></td>
                    <td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%:Html.Encode(item.PolicyGroupName)%></td>    
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyAirVendorGroupItem('<% =item.PolicyAirVendorGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyAirVendorGroupItemPopup('<%:item.PolicyAirVendorGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
          
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyCarTypeGroupItemDisplayFlag != null && Model.PolicyCarTypeGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyCarTypeGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="5"><%=Model.PolicyCarTypeGroupItemDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyCarTypeGroupItem('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr> 
                <tr>
                	<td>Category</td>
                    <td>Status</td>
                    <td>Location</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>   
                </tr>             
            </thead>
            <tbody>
            <% 
            if (Model.PolicyCarTypeGroupItems != null){
                foreach (var item in Model.PolicyCarTypeGroupItems)
                {                 
            %>
				<tr PolicyCarTypeGroupItemId = '<%:item.PolicyCarTypeGroupItemId%>' Source = '<%:Html.Encode(item.Source)%>'>
                    <td><%: Html.Encode(item.CarTypeCategoryName)%></td>
                    <td><%: Html.Encode(item.PolicyCarStatusDescription)%></td>
                    <td><%: Html.Encode(item.PolicyLocationName)%></td>
                    <td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%: Html.Encode(item.PolicyGroupName)%></td>
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyCarTypeGroupItem('<% =item.PolicyCarTypeGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyCarTypeGroupItemPopup('<%:item.PolicyCarTypeGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
           
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyCarVendorGroupItemDisplayFlag != null && Model.PolicyCarVendorGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyCarVendorGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="5"><%=Model.PolicyCarVendorGroupItemDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyCarVendorGroupItem('0','NEW');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr>                 
                <tr>
                	<td>Vendor</td>
                    <td>Status</td>
                    <td>Location</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>
                </tr>
            </thead>
            <tbody>
            <% 
            if (Model.PolicyCarVendorGroupItems != null){
                foreach (var item in Model.PolicyCarVendorGroupItems){                 
            %>
				<tr PolicyCarVendorGroupItemId = '<%:item.PolicyCarVendorGroupItemId%>' Source='<%:item.Source%>'>
                    <td><%: Html.Encode(item.SupplierName)%></td>
                    <td><%: Html.Encode(item.PolicyCarStatusDescription)%></td>
                    <td><%: Html.Encode(item.PolicyLocationName)%></td>
                    <td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%: Html.Encode(item.PolicyGroupName)%></td>
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyCarVendorGroupItem('<% =item.PolicyCarVendorGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyCarVendorGroupItemPopup('<%:item.PolicyCarVendorGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyCityGroupItemDisplayFlag != null && Model.PolicyCityGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyCityGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="6"><%=Model.PolicyCityGroupItemDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyCityGroupItem('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr>  
                <tr>
                	<td>City</td>
                    <td>Status</td>
                    <td colspan="2">Travel date valid from/to</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>
                </tr>               
            </thead>
            <tbody>
            <% 
                if (Model.PolicyCityGroupItems != null)
                {
                    foreach (var item in Model.PolicyCityGroupItems)
                    {                 
            %>
				<tr PolicyCityGroupItemId='<%:item.PolicyCityGroupItemId%>' Source='<%:item.Source%>' id='<%: Html.Encode(item.CityName)%>' InheritFlag='<%=Html.Encode(item.InheritFromParentFlag)%>'>
                    <td><%: Html.Encode(item.CityName)%></td>
                    <td><%: Html.Encode(item.PolicyCityStatusDescription)%></td>
                    <td width="10%"><%if (item.TravelDateValidFrom.HasValue){ %><%: Html.Encode(item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy"))%><%}%></td>
                    <td width="10%"><%if (item.TravelDateValidTo.HasValue){ %><%: Html.Encode(item.TravelDateValidTo.Value.ToString("MMM dd, yyyy"))%><%}%></td>           
                    <td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%: Html.Encode(item.PolicyGroupName)%></td>
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyCityGroupItem('<% =item.PolicyCityGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyCityGroupItemPopup('<%:item.PolicyCityGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%>              
                </tr>
            <%        
                }
            }
            %>                  
        
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyCountryGroupItemDisplayFlag != null && Model.PolicyCountryGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyCountryGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="6"><%=Model.PolicyCountryGroupItemDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyCountryGroupItem('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr>  
                <tr>
                	<td>Country</td>
                    <td>Status</td>
                    <td colspan="2">Travel date valid from/to</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>
                </tr>               
            </thead>
            <tbody>
            <% 
                if (Model.PolicyCountryGroupItems != null)
                {
                    foreach (var item in Model.PolicyCountryGroupItems)
                    {                 
            %>
				<tr PolicyCountryGroupItemId='<%:item.PolicyCountryGroupItemId%>' Source='<%:item.Source%>' id='<%:item.CountryName%>' InheritFlag='<%=Html.Encode(item.InheritFromParentFlag)%>'>
                    <td><%: Html.Encode(item.CountryName)%></td>
                    <td><%: Html.Encode(item.PolicyCountryStatusDescription)%></td>
                    <td width="10%"><%if (item.TravelDateValidFrom.HasValue){ %><%: Html.Encode(item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy"))%><%}%></td>
                    <td width="10%"><%if (item.TravelDateValidTo.HasValue){ %><%: Html.Encode(item.TravelDateValidTo.Value.ToString("MMM dd, yyyy"))%><%}%></td>           
                    <td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%: Html.Encode(item.PolicyGroupName)%></td>
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyCountryGroupItem('<% =item.PolicyCountryGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyCountryGroupItemPopup('<%:item.PolicyCountryGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%>              
                </tr>
            <%        
                }
            }
            %>                  
        
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyHotelCapRateGroupItemDisplayFlag != null && Model.PolicyHotelCapRateGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyHotelCapRateGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="6"><%=Model.PolicyHotelCapRateGroupItemDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyHotelCapRateGroupItem('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr>
                <tr>
                    <td>Location</td>
                    <td>Cap rate</td>
                    <td>Currency</td>
                    <td>Tax Inclusive?</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>
                </tr>                 
            </thead>
            <tbody>
            <% 
                if (Model.PolicyHotelCapRateGroupItems != null)
                {
                    foreach (var item in Model.PolicyHotelCapRateGroupItems)
                    {                 
            %>
				<tr PolicyHotelCapRateItemId="<%:item.PolicyHotelCapRateItemId%>" Source="<%:item.Source%>">
                    <td><%: Html.Encode(item.PolicyLocationName)%></td>
                    <td><%: Html.Encode(item.CapRate)%></td>
                    <td><%: Html.Encode(item.Name)%></td>
                    <td><%: Html.Encode(item.TaxInclusiveFlag == true ? "Yes" : "No") %></td>
                    <td><%: Html.Encode(item.Source)%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%: Html.Encode(item.PolicyGroupName)%></td>
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyHotelCapRateGroupItem('<% =item.PolicyHotelCapRateItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyHotelCapRateGroupItemPopup('<%:item.PolicyHotelCapRateItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
           
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyHotelPropertyGroupItemDisplayFlag != null && Model.PolicyHotelPropertyGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyHotelPropertyGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="6"><%=Model.PolicyHotelPropertyGroupItemDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyHotelPropertyGroupItem('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr> 
                <tr>
                	<td>Harp hotel name</td>
                    <td>Status</td>
                    <td width="12%">Source</td>
                    <td colspan="2">Travel date valid from/to</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>
                </tr>                
            </thead>
            <tbody>
            <% 
                if (Model.PolicyHotelPropertyGroupItems != null)
                {
                    foreach (var item in Model.PolicyHotelPropertyGroupItems)
                    {                 
            %>
				<tr PolicyHotelPropertyGroupItemId = '<%:item.PolicyHotelPropertyGroupItemId%>' Source = '<%:Html.Encode(item.Source)%>'>
                    <td><%: item.HarpHotelName %></td>
                    <td><%: item.PolicyHotelStatusDescription%></td>
                    <td><%: item.Source%><%if (item.Source == "Country") {%> (<%: Html.Encode(item.SourceCode)%>)<%}%></td>
                    <td><%: item.PolicyGroupName%></td>
                    <td width="10%"><%if (item.TravelDateValidFrom.HasValue){ %><%: Html.Encode(item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy"))%><%}%></td>
                    <td width="10%"><%if (item.TravelDateValidTo.HasValue){ %><%: Html.Encode(item.TravelDateValidTo.Value.ToString("MMM dd, yyyy"))%><%}%></td>           
                    
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyHotelPropertyGroupItem('<% =item.PolicyHotelPropertyGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyHotelPropertyGroupItemPopup('<%:item.PolicyHotelPropertyGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
             
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicyHotelVendorGroupItemDisplayFlag != null && Model.PolicyHotelVendorGroupItemDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicyHotelVendorGroupItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="5"><%=Model.PolicyHotelVendorGroupItemDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicyHotelVendorGroupItem('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr>  
                <tr>
                	<td>Supplier</td>
                    <td>Status</td>
                    <td>Location</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>
                </tr>               
            </thead>
            <tbody>
            <% 
                if (Model.PolicyHotelVendorGroupItems != null)
                {
                    foreach (var item in Model.PolicyHotelVendorGroupItems)
                    {                 
            %>
				<tr PolicyHotelVendorGroupItemId = '<%:item.PolicyHotelVendorGroupItemId%>' Source = '<%:Html.Encode(item.Source)%>'>
                    <td><%: item.SupplierName%></td>
                    <td><%: item.PolicyHotelStatusDescription%></td>
                    <td><%: item.PolicyLocationName%></td>
                    <td><%: item.Source%><%if (item.Source == "Country") {%> (<%: item.SourceCode%>)<%}%></td>
                    <td><%: item.PolicyGroupName%></td>
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicyHotelVendorGroupItem('<% =item.PolicyHotelVendorGroupItemId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicyHotelVendorGroupItemPopup('<%:item.PolicyHotelVendorGroupItemId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
          
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicySupplierDealCodeDisplayFlag != null && Model.PolicySupplierDealCodeDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicySupplierDealCodesTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="8"><%=Model.PolicySupplierDealCodeDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicySupplierDealCode('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr>  
                <tr>
                	<td>Product</td>
                	<td>Supplier</td>
                    <td>Value</td>
                    <td>GDS</td>
                    <td>Type</td>
                    <td>Location</td>
                    <td width="12%">Source</td>
                    <td>Policy Group Name</td>
                    <td align="center" width="13%">Actions</td>         
                </tr>     
            </thead>
            <tbody>
            <% 
                if (Model.PolicySupplierDealCodes != null)
                {
                    foreach (var item in Model.PolicySupplierDealCodes)
                    {                 
            %>
				<tr PolicySupplierDealCodeId = '<%: item.PolicySupplierDealCodeId %>' Source='<%:Html.Encode(item.Source)%>'>
                    <td><%: item.ProductName%></td>                 
                    <td><%: item.SupplierName%></td>  
                    <td><%: item.PolicySupplierDealCodeValue%></td>
                    <td><%: item.GDSCode%></td>
                    <td><%: item.PolicySupplierDealCodeTypeDescription%></td>
                    <td><%: item.PolicyLocationName%></td>
                    <td><%: item.Source%><%if (item.Source == "Country") {%> (<%: item.SourceCode%>)<%}%></td>
                    <td><%: item.PolicyGroupName%></td>                                
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicySupplierDealCode('<% =item.PolicySupplierDealCodeId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicySupplierDealCodePopup('<%:item.PolicySupplierDealCodeId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%>  
                </tr>
            <%        
                }
            }
            %>                  
           
            </tbody>    
            </table>              
        </td>
    </tr>
    <% } %>
    
	<% if (Model.PolicySupplierServiceInformationDisplayFlag != null && Model.PolicySupplierServiceInformationDisplayFlag == true)
	{ %>
		<tr>
        <td>
            <table id="currentPolicySupplierServiceInformationsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
            <thead>          
                <tr>
                    <th colspan="5"><%=Model.PolicySupplierServiceInformationDisplayTitle %></th>
                    <th><%if (Model.HasPolicyGroupWriteAccess){%><a href="javascript:AddEditPolicySupplierServiceInformation('0');"><img src="<%=Url.Content("~/Images/Common/addButton.jpg")%>" border="0" style="float:right" title="Add" alt="Add"/></a><%}%></th>
                </tr>  
                <tr>
                    <td>Product</td>
                	<td>Supplier</td>
                    <td width="12%">Source</td>
                    <td>Service Info Type</td>
                    <td>Service Info Value</td>
                    <td align="center" width="13%">Actions</td>  
                </tr>             
            </thead>
            <tbody>
            <% 
                if (Model.PolicySupplierServiceInformations != null)
                {
                    foreach (var item in Model.PolicySupplierServiceInformations)
                    {                 
            %>
				<tr PolicySupplierServiceInformationId = '<%: item.PolicySupplierServiceInformationId %>' Source='<%:Html.Encode(item.Source)%>'>
                    <td><%: item.ProductName%></td>
                    <td><%: item.SupplierName%></td>
                    <td><%: item.Source%><%if (item.Source == "Country") {%> (<%: item.SourceCode%>)<%}%></td>
                    <td><%: item.PolicySupplierServiceInformationTypeDescription%></td>
                    <td><%: item.PolicySupplierServiceInformationValue%></td>                
                    <%if (item.Source != "Custom"){ %>
                        <td>Inherited</td>
                    <%}else{%>
                        <%if (Model.HasPolicyGroupWriteAccess){%>
                        <td><a href="javascript:AddEditPolicySupplierServiceInformation('<% =item.PolicySupplierServiceInformationId%>')"><img src="<%=Url.Content("~/images/Common/editButton2.jpg")%>" border="0" title="Edit" alt="Edit"/></a>&nbsp;<a href="javascript:DeletePolicySupplierServiceInformationPopup('<%:item.PolicySupplierServiceInformationId%>','<%:item.VersionNumber%>');"><img src="<%=Url.Content("~/Images/Common/removeButton.jpg")%>" border="0" title="Remove" alt="Remove"/></a></td>
                        <% }else { %>
                              <td>Read Only</td>
                        <% }  %>
                    <%}%> 
                </tr>
            <%        
                }
            }
            %>                  
             
            </tbody>    
            </table>              
        </td>
    </tr>
	<% } %>

</table>
<script type="text/javascript">
    $(document).ready(function () {
        //If we have just Created a PolicyGroup, we could have altered this value
        //and therefore overwrite loaded value with stored value from previous Screen
        if ($("#PolicyGroup").val() != "") {
            var PolicyGroup = $.parseJSON($("#PolicyGroup").val());
            if (PolicyGroup["InheritFromParentFlag"] == "true") {
                $("#PolicyGroup_InheritFromParentFlag").attr("checked", "checked");
            }
        }
        //disable inheritance checkbox if no WriteAcccess
        if ($('#ShowPolicyGroupScreen').val() == "False") {
            $("#PolicyGroup_InheritFromParentFlag").attr('disabled', 'disabled');
        }
    });
</script>

 


 