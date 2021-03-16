<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeMaintenanceVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Maintenance</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Pseudo City/Office ID Maintenance</th> 
		        </tr> 
                <tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeId">Pseudo City/Office ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenanceIATAId">IATA</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.IATA.IATANumber)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GDSCode">GDS</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GDS.GDSName)%> </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_AmadeusId">Amadeus ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.AmadeusId)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId">Address</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId)%> </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_LocationContactName">Location Contact Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.LocationContactName)%></td>
                </tr>  
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_LocationContactName">Location Phone</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.LocationPhone)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_CountryName">Country</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.CountryName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GlobalRegionName">Global Region</label></td>
                    <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GlobalRegionCode)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId">Pseudo City/Office ID Defined Region</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegion != null ? Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName: "") %></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ActiveFlagNonNullable">Active</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ActiveFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.ActiveFlag.Value == true ? "True" : "False")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_InternalSiteName">Internal Site Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.InternalSiteName)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ExternalNameId">External Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ExternalName != null ? Model.PseudoCityOrOfficeMaintenance.ExternalName.ExternalName1 : "")%> </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ClientSubUnitGuid">Client SubUnit</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ClientSubUnitsList != null ? string.Join(", ", Model.PseudoCityOrOfficeMaintenance.ClientSubUnitsList.Select(x => x.ClientSubUnitName)) : "")%> </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeTypeId">Pseudo City/Office ID Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeType != null ? Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName : "")%> </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeLocationTypeId">Pseudo City/Office ID Location Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationType != null ? Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName : "")%> </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_FareRedistributionId">Fare Redistribution</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.FareRedistribution != null ? Model.PseudoCityOrOfficeMaintenance.FareRedistribution.FareRedistributionName : "")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GovernmentContract">Government Contract</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GovernmentContract)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_CIDBPIN">CIDB/PIN</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.CIDBPIN)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_SharedPseudoCityOrOfficeFlag">Shared PCC/Office ID?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlag.Value == true ? "True" : "False")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_CWTOwnedPseudoCityOrOfficeFlag">CWT Owned PCC/Office ID?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlag.Value == true ? "True" : "False")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ClientDedicatedPseudoCityOrOfficeFlag">Client Dedicated PCC/Office ID?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlag.Value == true ? "True" : "False")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_ClientGDSAccessFlag">Client GDS Access?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlag.Value == true ? "True" : "False")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_DevelopmentOrInternalPseudoCityOrOfficeFlag">Development/Internal PCC/Office ID?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlag.Value == true ? "True" : "False")%></td>
                </tr>
				<tr>
					<td><label for="PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlag">Cuba PCC/Office ID?</label></td>
					<td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlag.Value == true ? "True" : "False")%></td>
				</tr>			
				<tr>
					<td><label for="PseudoCityOrOfficeMaintenance_GovernmentPseudoCityOrOfficeFlag">Government PCC/Office ID?</label></td>
					<td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlag.Value == true ? "True" : "False")%></td>
				</tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlag">3rd Party Vendor?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlag.HasValue && Model.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlag.Value == true ? "True" : "False")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorId">3rd Party Vendor(s)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorsList != null ? string.Join(", ", Model.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorsList.Select(x => x.GDSThirdPartyVendorName)) : "")%> </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_VendorAssignedDate">Vendor Assigned Date</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.VendorAssignedDate.HasValue ? Model.PseudoCityOrOfficeMaintenance.VendorAssignedDate.Value.ToString("MMM dd yyyy") : "")%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_InternalRemarks">Internal Remarks</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.InternalRemarks)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_DeletedFlag">Deleted?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.DeletedFlag)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeMaintenance_DeletedDateTime">Deleted Date/Time</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.DeletedDateTime)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
							<%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId) %>
							<%= Html.HiddenFor(model => model.PseudoCityOrOfficeMaintenance.VersionNumber) %>
						<%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_gdsmanagement').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Pseudo City/Office ID Maintenance", "Main", new { controller = "PseudoCityOrOfficeMaintenance", action = "ListUnDeleted", }, new { title = "Pseudo City/Office ID Maintenance" })%> &gt;
<%=Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId) %>
</asp:Content>