<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeMaintenanceVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Maintenance</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "PseudoCityOrOfficeMaintenance", action = "Edit", id = Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Pseudo City/Office ID Maintenance</th> 
				</tr> 
		        <tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeId">Pseudo City/Office ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId, new { maxlength = "9" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenanceIATAId">IATA</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.IATAId , Model.IATAs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.IATAId)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.GDSCode , Model.GDSs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_AmadeusId">Amadeus ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.AmadeusId, new { maxlength = "6", @disabled = "disabled" })%> <span id="PseudoCityOrOfficeMaintenance_AmadeusId_Error" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.AmadeusId)%>
						<label id="PseudoCityOrOfficeMaintenance_AmadeusId_Label" class="error">Amadeus ID is mandatory only when the GDS is Amadeus</label>
                    </td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId">Address</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId , Model.PseudoCityOrOfficeAddresses as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_LocationContactName">Location Contact Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.LocationContactName, new { maxlength = "255" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.LocationContactName)%></td>
                </tr>  
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_LocationContactName">Location Phone</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.LocationPhone, new { maxlength = "20" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.LocationPhone)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_CountryName">Country</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.CountryName, new { @disabled = "disabled" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.CountryName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GlobalRegionName">Global Region</label></td>
                    <td>
						<%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.GlobalRegionName, new { @disabled = "disabled" })%><span class="error"> *</span>
						<%= Html.Hidden("PseudoCityOrOfficeMaintenance_GlobalRegionCode") %>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.GlobalRegionName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId">Pseudo City/Office ID Defined Region</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId , Model.PseudoCityOrOfficeDefinedRegions as SelectList, "Please Select...")%><span id="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId)%>
						<label id="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label" class="error">The Pseudo City/Office ID Defined Region is required</label>
                    </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ActiveFlagNonNullable">Active</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_InternalSiteName">Internal Site Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.InternalSiteName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.InternalSiteName)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ExternalNameId">External Name</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.ExternalNameId , Model.ExternalNames as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.ExternalNameId)%></td>
                </tr>
				<tr class="ClientSubUnitGuidDisplay">
                    <td><label for="PseudoCityOrOfficeMaintenance_ClientSubUnitGuid">Client SubUnit</label></td>
                    <td>
						<% 
						if (Model.PseudoCityOrOfficeMaintenance.ClientSubUnitsList != null && Model.PseudoCityOrOfficeMaintenance.ClientSubUnitsList.Count > 0) {
							foreach (CWTDesktopDatabase.Models.ClientSubUnit clientSubUnit in Model.PseudoCityOrOfficeMaintenance.ClientSubUnitsList) { %>
							<div class="ClientSubUnitGuidRow">
								<%= Html.TextBox("ClientSubUnitGuidName", clientSubUnit.ClientSubUnitName, new  { @Class = "ClientSubUnitName", size = "30" })%>  <%= Html.Hidden("ClientSubUnitGuids", clientSubUnit.ClientSubUnitGuid, new  { @Class = "ClientSubUnitGuid" })%> 
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/delete.png" alt="Remove" /></a> 
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a>
							</div>
							<% } %>
						<% } else { %>
							<div class="ClientSubUnitGuidRow">
								<%= Html.TextBox("ClientSubUnitGuidName", null, new  { @Class = "ClientSubUnitName", size = "30" })%>  <%= Html.HiddenFor(model => model.ClientSubUnitGuids, new  { @Class = "ClientSubUnitGuid" })%> 
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/delete.png" alt="Remove" /></a> 
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a>
							</div>
						<% } %>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientSubUnitGuids)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeTypeId">Pseudo City/Office ID Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId , Model.PseudoCityOrOfficeTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeLocationTypeId">Pseudo City/Office ID Location Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId , Model.PseudoCityOrOfficeLocationTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_FareRedistributionId">Fare Redistribution</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeMaintenance.FareRedistributionId , Model.FareRedistributions as SelectList, "Please Select...")%> <span id="PseudoCityOrOfficeMaintenance_FareRedistributionId_Error" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.FareRedistributionId)%>
						<label id="PseudoCityOrOfficeMaintenance_FareRedistributionId_Label" class="error">The Fare Redistribution is required</label>
                    </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GovernmentContract">Government Contract</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.GovernmentContract, new { maxlength = "255" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.GovernmentContract)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_CIDBPIN">CIDB/PIN</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.CIDBPIN, new { maxlength = "128" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.CIDBPIN)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_SharedPseudoCityOrOfficeFlagNonNullable">Shared PCC/Office ID?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_CWTOwnedPseudoCityOrOfficeFlagNonNullable">CWT Owned PCC/Office ID?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ClientDedicatedPseudoCityOrOfficeFlagNonNullable">Client Dedicated PCC/Office ID?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ClientGDSAccessFlagNonNullable">Client GDS Access?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable">Development/Internal PCC/Office ID?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable)%></td>
                </tr>
				<tr>
					<td><label for="PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlagNonNullable">Cuba PCC/Office ID?</label></td>
					<td>
						<%if (ViewData["ComplianceAdministratorAccess"] == "WriteAccess"){ %>
							<%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable)%>
						<% } else { %>
							<%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable, new { @disabled = "disabled" })%>
							<%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable)%>
						<% } %>
					</td>
					<td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable)%></td>
				</tr>			
				<tr>
					<td><label for="PseudoCityOrOfficeMaintenance_GovernmentPseudoCityOrOfficeFlagNonNullable">Government PCC/Office ID?</label></td>
					<td>
						<%if (ViewData["GDSGovernmentAdministratorAccess"] == "WriteAccess"){ %>
							<%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable)%>
						<% } else { %>
							<%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable, new { @disabled = "disabled" })%>
							<%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable)%>
						<% } %>
					</td>
					<td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable)%></td>
				</tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable">3rd Party Vendor?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorId">3rd Party Vendor(s)</label></td>
                    <td><%= Html.ListBoxFor(x => x.GDSThirdPartyVendorIds, Model.GDSThirdPartyVendorsList)%> <span id="GDSThirdPartyVendorIds_Error" class="error"> *</span></td>
                    <td>
						<label>Hold CNTRL to select multiple</label><br /><br />
						<label id="GDSThirdPartyVendorIds_Label" class="error">The 3rd Party Vendor(s) is required</label>
						<%= Html.ValidationMessageFor(model => model.GDSThirdPartyVendors)%>
                    </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_VendorAssignedDate">Vendor Assigned Date</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeMaintenance.VendorAssignedDate, new { })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.VendorAssignedDate.Value)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_InternalRemarks">Internal Remarks</label></td>
                    <td><%= Html.TextAreaFor(model => model.PseudoCityOrOfficeMaintenance.InternalRemarks, new { maxlength = "1024" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeMaintenance.InternalRemarks)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit PCC/OID" title="Edit PCC/OID" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId) %>
			<%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.DeletedFlag) %>
			<%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.DeletedDateTime) %>
			<%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.VersionNumber) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/PseudoCityOrOfficeMaintenance.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Pseudo City/Office ID Maintenance", "Main", new { controller = "PseudoCityOrOfficeMaintenance", action = "ListUnDeleted", }, new { title = "Pseudo City/Office ID Maintenance" })%> &gt;
<%=Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId) %>
</asp:Content>