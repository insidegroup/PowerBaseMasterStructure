<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.GDSOrder>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Orders
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Orders</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="4">View GDS Order</th> 
		    </tr> 
			<tr>
				<td><label for="GDSOrderId">OrderNumber</label></td>
				<td colspan="3"><%= Html.Encode(Model.GDSOrderId)%></td>
			</tr>
			<tr>
				<td><label for="GDSName">GDS</label></td>
				<td colspan="3"><%= Html.Encode(Model.GDS.GDSName)%></td>
			</tr>
			<tr>
                <td><label for="GDSOrderDateTime">Order Date</label></td>
                <td><%= Html.Encode(Model.GDSOrderDateTime)%></td>
                <td colspan="2" style="padding-left: 0;">
					<table cellpadding="0" cellspacing="0" border="0" width="100%">
						<tr>
							<td width="40%"><label for="DeactivationDateTime">Deactivation Date</label></td>
							<td width="60%"><%= Html.Encode(Model.DeactivationDateTime.HasValue ? Model.DeactivationDateTime.Value.ToString("MMM dd, yyyy") : "No Deactivation Date")%></td>
						</tr>
					</table>
				</td>
            </tr>
			<tr>
                <td><label for="OrderTypeName">Order Type</label></td>
                <td><%= Html.Encode(Model.GDSOrderType != null ? Model.GDSOrderType.GDSOrderTypeName.ToString() : "")%></td>
                <td colspan="2" style="padding-left: 0;">
					<table cellpadding="0" cellspacing="0" border="0" width="100%">
						<tr>
							<td width="40%"><label for="OrderTypeName">3<sup>rd</sup> Party Vendor(s)</label></td>
							<td width="60%">
								<% if(Model.GDSThirdPartyVendors != null && Model.GDSThirdPartyVendors.Count > 0) { %>
									<% foreach(CWTDesktopDatabase.Models.GDSThirdPartyVendor item in  Model.GDSThirdPartyVendors) { %>
										<%= Html.Encode(item.GDSThirdPartyVendorName) %><br />
									<% } %>
								<% } %>
							</td>
						</tr>
					</table>
				</td>
            </tr>
			<tr>
				<td><label for="OrderStatusName">Order Status</label></td>
				<td colspan="3"><%= Html.Encode(Model.GDSOrderStatus.GDSOrderStatusName)%></td>
			</tr>
			<tr>
				<td><label for="PseudoCityOrOfficeId">PCC/OID</label></td>
				<td colspan="3"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId)%></td>
			</tr>
			<tr>
				<td><label for="FirstAddressLine">PCC/OID Address</label></td>
				<td colspan="3"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddress.FirstAddressLine)%></td>
			</tr>
			<tr>
                <td><label for="OrderAnalyst">Order Analyst</label></td>
                <td colspan="3"><%= Html.Encode(Model.OrderAnalystName)%></td>
            </tr>
			<tr>
				<td><label for="OrderAnalystEmail">Order Analyst Email</label></td>
				<td colspan="3"><%= Html.Encode(Model.OrderAnalystEmail)%></td>
			</tr>
			<tr>
				<td><label for="OrderAnalystPhone">Order Analyst Phone</label></td>
				<td colspan="3"><%= Html.Encode(Model.OrderAnalystPhone)%></td>
			</tr>
			<tr>
				<td><label for="OrderAnalystCountryCode">Order Analyst Country</label></td>
				<td colspan="3"><%= Html.Encode(Model.OrderAnalystCountry.CountryName)%></td>
			</tr>
			<tr>
				<td><label for="TicketNumber">Ticket Number</label></td>
				<td colspan="3"><%= Html.Encode(Model.TicketNumber)%></td>
			</tr>
			<tr>
				<td><label for="ExpediteFlag">Expedite?</label></td>
				<td colspan="3"><%= Html.Encode(Model.ExpediteFlag)%></td>
			</tr>
			<tr>
				<td><label for="CWTCostCenterNumber">CWT Cost Center Number</label></td>
				<td colspan="3"><%= Html.Encode(Model.CWTCostCenterNumber)%></td>
			</tr>
			<tr>
				<td><label for="RequesterFirstName">Requester First Name</label></td>
				<td colspan="3"><%= Html.Encode(Model.RequesterFirstName)%></td>
			</tr>
			<tr>
				<td><label for="RequesterFirstName">Requester Last Name</label></td>
				<td colspan="3"><%= Html.Encode(Model.RequesterLastName)%></td>
			</tr>
			<tr>
				<td><label for="RequesterEmail">Requester Email</label></td>
				<td colspan="3"><%= Html.Encode(Model.RequesterEmail)%></td>
			</tr>
			<tr>
				<td><label for="RequesterPhone">Requester Phone</label></td>
				<td colspan="3"><%= Html.Encode(Model.RequesterPhone)%></td>
			</tr>
			<tr>
				<td><label for="RequesterCountryCode">Requester Country</label></td>
				<td colspan="3"><%= Html.Encode(Model.RequesterCountry.CountryName)%></td>
			</tr>
			<tr>
				<td><label for="RequesterUID">Requester UID</label></td>
				<td colspan="3"><%= Html.Encode(Model.RequesterUID)%></td>
			</tr>
			<tr>
				<td><label for="ExternalRemarks">External Remarks</label></td>
				<td colspan="3" class="wrap-text"><%= Html.Encode(Model.ExternalRemarks)%></td>
			</tr>
			<tr>
				<td colspan="4" style="padding: 0;">
					<table cellpadding="0" cellspacing="0" border="0" width="100%">
						<thead>
							<tr>
								<td colspan="4"><strong style="line-height: 30px;">GDS Order Details</strong></td>
							</tr>
							<tr>
								<th>Quantity</th>
								<th>Add/Delete</th>
								<th>Additional Order Detail</th>
								<th>Comments</th>
							</tr>
						</thead>
						<tbody>
							<% int counter = 0;
							if (Model.GDSOrderLineItems != null && Model.GDSOrderLineItems.Count > 0){ %>
								<% foreach (CWTDesktopDatabase.Models.GDSOrderLineItem item in Model.GDSOrderLineItems) { %>
									<tr class="gds-order-line-item">
										<td><%= Html.Encode(item.Quantity)%></td>
										<td><%= Html.Encode(item.GDSOrderLineItemAction.GDSOrderLineItemActionName)%></td>
										<td><%= Html.Encode(item.GDSOrderDetail.GDSOrderDetailName)%></td>
										<td><%= Html.Encode(item.Comment)%></td>
									</tr>
								<% counter++;
								} %>
							<% } %>
						</tbody>
					</table>
				</td>
			</tr>
			<tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="25%" class="row_footer_centre"></td>
				<td width="25%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>
			<tr>
            <td>
				<label for="PseudoCityOrOfficeMaintenanceIATAId">IATA</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.IATA.IATANumber)%></td>
                <td><label for="PseudoCityOrOfficeMaintenance_SharedPseudoCityOrOfficeFlag">Shared PCC/Office ID?</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlag == true ? "True" : "False")%></td>
			</tr>
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_GDSCode">GDS</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GDSCode)%> </td>
                <td><label for="PseudoCityOrOfficeMaintenance_CWTOwnedPseudoCityOrOfficeFlag">CWT Owned PCC/Office ID?</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlag == true ? "True" : "False")%></td>
			</tr>
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId">Address</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId)%> </td>
                <td><label for="PseudoCityOrOfficeMaintenance_ClientDedicatedPseudoCityOrOfficeFlag">Client Dedicated PCC/Office ID?</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlag == true ? "True" : "False")%></td>
            </tr>
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_LocationContactName">Location Contact Name</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.LocationContactName)%></td>
                <td><label for="PseudoCityOrOfficeMaintenance_ClientGDSAccessFlag">Client GDS Access?</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlag== true ? "True" : "False")%></td>
            </tr>  
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_LocationContactName">Location Phone</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.LocationPhone)%></td>
                <td><label for="PseudoCityOrOfficeMaintenance_DevelopmentOrInternalPseudoCityOrOfficeFlag">Development/Internal PCC/Office ID?</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlag == true ? "True" : "False")%></td>
            </tr> 
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_CountryName">Country</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.CountryName)%></td>
 				<td><label for="PseudoCityOrOfficeMaintenance_CubaPseudoCityOrOfficeFlag">Cuba PCC/Office ID?</label></td>
				<td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlag == true ? "True" : "False")%></td>
           </tr> 
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_GlobalRegionName">Global Region</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GlobalRegionName)%></td>
 				<td><label for="PseudoCityOrOfficeMaintenance_GovernmentPseudoCityOrOfficeFlag">Government PCC/Office ID?</label></td>
				<td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlag == true ? "True" : "False")%></td>
           </tr> 
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_InternalSiteName">Internal Site Name</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.InternalSiteName)%></td>
                <td><label for="PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlag">3rd Party Vendor?</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlag == true ? "True" : "False")%></td>
            </tr>
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_ExternalNameId">External Name</label></td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.ExternalName != null ? Model.PseudoCityOrOfficeMaintenance.ExternalName.ExternalName1 : "")%> </td>
                <td><label for="PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorId">3rd Party Vendor(s)</label></td>
                <td>
					<% if (Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors != null && Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors.Count > 0){ %>
						<% foreach (CWTDesktopDatabase.Models.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendor item in Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendors){ %>
							<%= Html.Encode(item.GDSThirdPartyVendor.GDSThirdPartyVendorName) %><br />
						<% } %>
					<% } %>
				</td>
			</tr>
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeTypeId">Pseudo City/Office ID Type</label></td>
                <td colspan="3"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeType != null ? Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName : "")%> </td>
            </tr>
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeLocationTypeId">Pseudo City/Office ID Location Type</label></td>
                <td colspan="3"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationType != null ? Model.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName : "")%> </td>
            </tr>
			<tr>
                <td><label for="PseudoCityOrOfficeMaintenance_FareRedistributionId">Fare Redistribution</label></td>
                <td colspan="3"><%= Html.Encode(Model.PseudoCityOrOfficeMaintenance.FareRedistribution != null ? Model.PseudoCityOrOfficeMaintenance.FareRedistribution.FareRedistributionName : "")%></td>
            </tr>
            <tr>
                <td><label for="GDSOrderEmailLogs">Email Generated</label></td>
                <td colspan="3">
                    <% if (Model.GDSOrderEmailLogs != null && Model.GDSOrderEmailLogs.Count > 0){ %>
                        <% int log_counter = 1; %>
					    <% foreach (CWTDesktopDatabase.Models.GDSOrderEmailLog item in Model.GDSOrderEmailLogs){ %>
                                <%= Html.Encode(
                                    string.Format("{0} {1}{2}",
                                        string.Format("{0:yyyy-MM-dd H:mm:ss.fff }", item.CreationTimestamp),
                                        item.CreationUserIdentifier.Contains(" - ") ? item.CreationUserIdentifier.Substring(0, item.CreationUserIdentifier.IndexOf(" - ")) : item.CreationUserIdentifier,
                                        log_counter != Model.GDSOrderEmailLogs.Count ? ", " : ""
                                )
                           ) %>
                           <% log_counter++; %>
						<% } %>
					<% } %>    
                </td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
					<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right" colspan="2"></td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
$(document).ready(function() {
	$('#menu_gdsmanagement').click();
	$("#content > table > tbody > tr:odd").addClass("row_odd");
	$("#content > table > tbody > tr:even").addClass("row_even");
})
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("GDS Orders", "Main", new { controller = "GDSOrder", action = "List", }, new { title = "GDS Orders" })%> &gt;
View &gt;
<% =Html.Encode(Model.PseudoCityOrOfficeId) %>
</asp:Content>