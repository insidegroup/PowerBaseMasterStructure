<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.SystemUserWizardVM>" %>
<div id="divTeam">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm("SystemUserDetailsScreen","SystemUserWizard",FormMethod.Post,new{id="frmSystemUserDetails"}))
           {%>
            <%= Html.ValidationSummary(true)%>
            <div style="float:right">
	        <span id="systemUserDetailsBackButton"><small><< Back</small></span>
              &nbsp;<span id="systemUserDetailsNextButton"><small>Next >></small></span>
            </div>
            <input type="hidden" id="systemUserHeader" value="<%= Html.Encode(Model.SystemUser.LastName)%>, <%= Html.Encode(Model.SystemUser.FirstName)%> <%= Html.Encode(Model.SystemUser.MiddleName)%>" />
            <div id="systemUserDetailsDiv">
            <table cellpadding="0" cellspacing="0" width="100%" class="tablesorter_other2" id='systemUserEditTable'> 
            	<thead>
		        <tr> 
			        <th colspan="3">System User</th> 
		        </tr> 
                </thead>
                <tbody>
                 <tr>
                    <td width="30%">Name</td>
                    <td width="40%"><%= Html.Encode(Model.SystemUser.LastName)%>, <%= Html.Encode(Model.SystemUser.FirstName)%> <%= Html.Encode(Model.SystemUser.MiddleName)%></td>
                    <td width="30%"></td>
                </tr> 
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.SystemUser.LanguageName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>IsActive?</td>
                    <td><%= Html.Encode(Model.SystemUser.IsActiveFlag)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Login</td>
                    <td><%= Html.Encode(Model.SystemUser.SystemUserLoginIdentifier)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td>Profile ID</td>
                    <td><%= Html.Encode(Model.SystemUser.UserProfileIdentifier)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="SystemUser_LocationName"/>Location</td>
                    <td> <%
                    if (Model.HasWriteAccessToLocation)
                    { %>
                    <%=Html.TextBoxFor(model => model.SystemUser.LocationName, new { maxlength="50" })%><span class="error"> *</span>
                    <%}else{%>
                        <%=Model.SystemUser.LocationName%>
                    <%
                     }
                    %></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.SystemUser.LocationName)%>
                        <%= Html.HiddenFor(model => model.SystemUser.LocationId)%>
                        <label id="lblLocationMsg"/>
                    </td>
                </tr>
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td colspan="2">
						<% if(ViewData["ComplianceAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("CubaBookingAllowanceIndicator", Model.SystemUser.CubaBookingAllowanceIndicator ?? false, new { @checked = (Model.SystemUser.CubaBookingAllowanceIndicator.HasValue && Model.SystemUser.CubaBookingAllowanceIndicator.Value == true) ? "checked" : "" })%>
						<% } else { %>
							<%=Html.CheckBox("CubaBookingAllowanceIndicator", Model.SystemUser.CubaBookingAllowanceIndicator ?? false, new { disabled = true, @checked = (Model.SystemUser.CubaBookingAllowanceIndicator.HasValue && Model.SystemUser.CubaBookingAllowanceIndicator.Value == true) ? "checked" : "" })%>
						<% } %>
						<%= Html.ValidationMessageFor(model => model.SystemUser.CubaBookingAllowanceIndicator)%>
						<label id="lblCubaBookingAllowanceIndicatorMsg" for="CubaBookingAllowanceIndicator" class="error"/>
                    </td>
                </tr> 
				<tr>
                    <td>Default Profile?</td>
                    <td colspan="2">
						<%=Html.CheckBox("DefaultProfileIdentifier", Model.SystemUser.DefaultProfileIdentifier ?? false, new { @checked = (Model.SystemUser.DefaultProfileIdentifier.HasValue && Model.SystemUser.DefaultProfileIdentifier.Value == true) ? "checked" : "" })%>
						<%= Html.ValidationMessageFor(model => model.SystemUser.DefaultProfileIdentifier)%>
						<label id="lblDefaultProfileIdentifierMsg" for="DefaultProfileIdentifier" class="error"/>
                    </td>
                </tr> 
                <tr>
                    <td>Restricted?</td>
                    <td colspan="2">
						<% if(ViewData["RestrictedSystemUserAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("RestrictedFlag", Model.SystemUser.RestrictedFlag ?? false, new { @checked = (Model.SystemUser.RestrictedFlag.HasValue && Model.SystemUser.RestrictedFlag.Value == true) ? "checked" : "" })%>
						<% } else { %>
							<%=Html.CheckBox("RestrictedFlag", Model.SystemUser.RestrictedFlag ?? false, new { disabled = true, @checked = (Model.SystemUser.RestrictedFlag.HasValue && Model.SystemUser.RestrictedFlag.Value == true) ? "checked" : "" })%>
						<% } %>
						<%= Html.ValidationMessageFor(model => model.SystemUser.RestrictedFlag)%>
						<label id="lblRestrictedFlagMsg" for="RestrictedFlag" class="error"/>
                    </td>
                </tr> 
            </tbody>
        </table>
        </div>
		<div id="ExternalSystemLoginSection">
			<table id="ExternalSystemLoginTable" class="tablesorter_other2" cellspacing="0">
				<thead>
					<tr>
						<th>Back Office Counselor Identifier</th>
						<th>Country</th>
						<th>Default</th>
						<th width="30%">&nbsp;</th>
					</tr>
				</thead>
				<tbody>
					<% if (Model.ExternalSystemLoginSystemUserCountries != null && Model.ExternalSystemLoginSystemUserCountries.Count > 0)
					{
						foreach (var item in Model.ExternalSystemLoginSystemUserCountries)
						{ %>                  
							<tr>
                    			<td>
									<%=Html.TextBox("ExternalSystemLoginName", item.ExternalSystemLoginName, new { @Class ="externalSystemLoginName", maxlength = "50" } ) %>
									<%=Html.Hidden("VersionNumber", item.VersionNumber, new { @Class = "versionNumber" } )%>
                    			</td>
								<td><%=Html.DropDownList("CountryCode", item.Countries, "Please Select...", new { @Class = "countryCode", @Value = item.CountryCode } )%></td>
								<td>
									<% if(item.IsDefaultFlag != null && item.IsDefaultFlag == true) { %>
										<%=Html.RadioButton("IsDefaultFlag", "IsDefaultFlag", new { @Class = "isDefaultFlag", @checked = "checked" } )%>
									<% } else {%>
										<%=Html.RadioButton("IsDefaultFlag", "IsDefaultFlag", new { @Class = "isDefaultFlag" } )%>
									<% } %>
								</td>
								<td>
									<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/delete.png" alt="Remove"></a> 
									<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add"></a> 
								</td>
							</tr>
						<% } %>
					<% } else { %>
						<tr>
                    		<td>
								<%=Html.TextBox("ExternalSystemLoginName", "", new { @Class ="externalSystemLoginName", maxlength = "50" } ) %>
								<%=Html.Hidden("VersionNumber", "1", new { @Class = "versionNumber" } )%>
                    		</td>
							<td><%=Html.DropDownList("CountryCode", Model.Countries, "Please Select...", new { @Class = "countryCode" } )%></td>
							<td><%=Html.RadioButton("IsDefaultFlag", "IsDefaultFlag", new { @Class = "isDefaultFlag" } )%></td>
							<td>
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/delete.png" alt="Remove"></a> 
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add"></a> 
							</td>
						</tr>
					<% } %>
				</tbody>
			</table>
			<span class="error" id="ExternalSystemLoginMessage" style="display: none;">
				Please provide both a Back Office Counselor Identifier and Country for each row or leave one row blank.<br />
			</span>
			<span class="error" id="ExternalSystemLoginDuplicateMessage" style="display: none;">
				Please complete fields before adding new one.<br />
			</span>
			<span class="error" id="ExternalSystemLoginRegexMessage" style="display: none;">
				Back Office Counselor Identifier must be alphanumeric.
			</span>
		</div>
		<div id="userGDSData"><span id="userGDSChecks"></span>
       	<table id="allGDSs" class="tablesorter_other2" cellspacing="0">
            <thead>
                <tr>
                    <th>GDS</th>
                    <th>Select</th>
                    <th>Home Pseudo or Office ID</th>
                    <th>GDS Sign On ID</th>
                    <th>Default</th>
                </tr>
            </thead>
            <tbody>
                <%
                if (Model.GDSs != null)
                {
                    foreach (var item in Model.GDSs)
                    {
                    %>                  
                    <tr>
                    	<td><%: item.GDSName%></td>
                        <td><input type="checkbox" id="<%: item.GDSCode %>" gdsCode="<%: item.GDSCode %>" /></td>
                        <td><%if(item.GDSCode !="ALL"){%><input type="text" id="PseudoCityOrOfficeId_<%:item.GDSCode%>" size="4" value="" maxlength="9"/><%}%>&nbsp;<span class="field-validation-error" id="lblPseudoCityOrOfficeId_<%:item.GDSCode%>"></span></td>
                        <td><%if(item.GDSCode !="ALL"){%><input type="text" id="GDSSignOnId_<%:item.GDSCode%>" size="7" value="" maxlength="10"/><%}%>&nbsp;<span class="field-validation-error" id="lblGDSSignOnId_<%:item.GDSCode%>"></span></td>
                        <td><input type="radio" id="radio_<%: item.GDSCode %>" radiogdsCode="<%: item.GDSCode %>" name="GDSDefault" /></td>
                    </tr>
                    <%
                    }
                }
                %>
            </tbody>
                
            </table>
            <div>
                <table id="userCurrentGDSs">
                <% 
            if (Model.SystemUserGDSs !=null)
            {
                    foreach (var item in Model.SystemUserGDSs)
                {
                        %>
                <tr systemUserGUID="<%: item.SystemUserGuid %>" id="<%: item.GDSCode %>" gdsName="<%: item.GDSName %>" isDefault="<%: item.DefaultGDS %>" gdssignonid="<%: item.GDSSignOn %>" pseudocityorofficeid="<%: item.PseudoCityOrOfficeId %>"></tr>
		        <%
                }
                } 
            %>                    
                </table>
                
            </div>
        </div>
            
            
        <div id="userBackOfficeData"></div>
            <%= Html.HiddenFor(model => model.SystemUser.VersionNumber)%>
            <%= Html.HiddenFor(model => model.SystemUserLocation.VersionNumber)%>
			<%--<%= Html.HiddenFor(model => model.ExternalSystemLogin.VersionNumber)%>--%>
    <% } %>

</div>