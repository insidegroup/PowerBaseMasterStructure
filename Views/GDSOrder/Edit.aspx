<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Orders
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Orders</div></div>
        <div id="content">
			<% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<%using (Html.BeginForm("Edit", null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
				<%= Html.AntiForgeryToken() %>
				<%= Html.ValidationSummary(true) %>
        
				<table cellpadding="0" cellspacing="0" width="100%"> 
					<tr> 
						<th class="row_header" colspan="3">Edit GDS Order</th> 
					</tr>  
					<tr>
						<td><label for="GDSOrderId">Order Number</label></td>
						<td colspan="3"><%= Html.Encode(Model.GDSOrder.GDSOrderId)%></td>
					</tr>
					<tr>
						<td><label for="GDSCode">GDS</label></td>
						<td><%= Html.Encode(Model.GDSOrder.GDS.GDSName)%></td>
						<td>
							<%= Html.HiddenFor(model => model.GDSOrder.GDSCode)%>
							<%= Html.ValidationMessageFor(model => model.GDSOrder.GDSCode)%>
						</td>
					</tr>
					<tr>
						<td><label for="GDSOrderDateTime">Order Date</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.GDSOrderDateTime, new { @Value = Model.GDSOrder.GDSOrderDateTime.ToString("MMM dd, yyyy")})%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.GDSOrderDateTime)%></td>
					</tr>
					<tr>
						<td><label for="GDSOrderTypeId">Order Type</label></td>
						<td><%= Html.Encode(Model.GDSOrder.GDSOrderType != null ? Model.GDSOrder.GDSOrderType.GDSOrderTypeName : "")%></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.GDSOrder.GDSOrderTypeId)%>
							<%= Html.HiddenFor(model => model.GDSOrder.GDSOrderTypeId)%>
							<%= Html.Hidden("GDSOrder_ShowDataFlag", Model.GDSOrder.GDSOrderType.ShowDataFlag)%>
						</td>
					</tr>
					<tr>
						<td><label for="GDSOrderStatusId">Order Status</label></td>
						<td><%= Html.DropDownListFor(model => model.GDSOrder.GDSOrderStatusId, Model.GDSOrderStatuses as SelectList, "Please Select...")%> <span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.GDSOrderStatusId)%></td>
					</tr>
					<tr>
						<td><label for="PseudoCityOrOfficeMaintenanceId">PCC/OID</label></td>
						<td><%= Html.Encode(Model.GDSOrder.PseudoCityOrOfficeId)%></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeId)%></td>
					</tr>
					<tr>
						<td><label for="PseudoCityOrOfficeAddressId">PCC/OID Address</label></td>
						<td><%= Html.Encode(Model.GDSOrder.PseudoCityOrOfficeAddress)%></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeAddress)%></td>
					</tr>
					<tr>
						<td><label for="OrderAnalyst">Order Analyst</label></td>
						<td><%= Html.Encode(Model.GDSOrder.OrderAnalystName)%></td>
						<td><%= Html.HiddenFor(model => model.GDSOrder.OrderAnalystName)%></td>
					</tr>
					<tr>
						<td><label for="OrderAnalystEmail">Order Analyst Email</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.OrderAnalystEmail, new { maxlength = "100" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.OrderAnalystEmail)%></td>
					</tr>
					<tr>
						<td><label for="OrderAnalystPhone">Order Analyst Phone</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.OrderAnalystPhone, new { maxlength = "20" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.OrderAnalystPhone)%></td>
					</tr>
					<tr>
						<td><label for="OrderAnalystCountryCode">Order Analyst Country</label></td>
						<td><%= Html.DropDownListFor(model => model.GDSOrder.OrderAnalystCountryCode, Model.OrderAnalystCountries as SelectList, "Please Select...")%> <span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.OrderAnalystCountryCode)%></td>
					</tr>
					<tr>
						<td><label for="TicketNumber">Ticket Number</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.TicketNumber, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.TicketNumber)%></td>
					</tr>
					<tr>
						<td><label for="ExpediteFlag">Expedite?</label></td>
						<td><%= Html.CheckBoxFor(model => model.GDSOrder.ExpediteFlagNullable)%></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.ExpediteFlagNullable)%></td>
					</tr>
					<tr>
						<td><label for="CWTCostCenterNumber">CWT Cost Center Number</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.CWTCostCenterNumber, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.CWTCostCenterNumber)%></td>
					</tr>
					<tr>
						<td><label for="RequesterFirstName">Requester First Name</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.RequesterFirstName, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.RequesterFirstName)%></td>
					</tr>
					<tr>
						<td><label for="RequesterLastName">Requester Last Name</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.RequesterLastName, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.RequesterLastName)%></td>
					</tr>
					<tr>
						<td><label for="RequesterEmail">Requester Email</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.RequesterEmail, new { maxlength = "100" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.RequesterEmail)%></td>
					</tr>
					<tr>
						<td><label for="RequesterPhone">Requester Phone</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.RequesterPhone, new { maxlength = "20" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.RequesterPhone)%></td>
					</tr>
					<tr>
						<td><label for="RequesterCountryCode">Requester Country</label></td>
						<td><%= Html.DropDownListFor(model => model.GDSOrder.RequesterCountryCode, Model.RequesterCountries as SelectList, "Please Select...")%> <span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.RequesterCountryCode)%></td>
					</tr>
					<tr>
						<td><label for="RequesterUID">Requester UID</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.RequesterUID, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.RequesterUID)%></td>
					</tr>
					<tr>
						<td><label for="ExternalRemarks">External Remarks</label></td>
						<td><%= Html.TextAreaFor(model => model.GDSOrder.ExternalRemarks, new { maxlength = "250" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.ExternalRemarks)%></td>
					</tr>
					<tr>
						<td><label for="DeactivationDateTime">Deactivation Date</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSOrder.DeactivationDateTime)%></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSOrder.DeactivationDateTime)%></td>
					</tr>
					<tr>
						<td><label for="GDSThirdPartyVendorIds">3<sup>rd</sup> Party Vendor(s)</label></td>
						<td>
							<%= Html.ListBoxFor(x => x.GDSThirdPartyVendorIds, Model.GDSThirdPartyVendorsList)%> 
							<% if(Model.GDSOrder.PseudoCityOrOfficeMaintenance != null && Model.GDSOrder.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlag == true) { %>
								<span class="error" id="GDSThirdPartyVendorIdRequired"> *</span>
							<% } %>
							<br />
							Hold CNTRL to select multiple<br />
							<span class="error" id="GDSThirdPartyVendorIdError">3rd Party Vendor(s) is required for the selected PCC/OID</span>
						</td>
						<td>
                            <% if(Model.GDSOrder.PseudoCityOrOfficeMaintenance != null && Model.GDSOrder.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlag == true) { %>
                                <%= Html.Hidden("IsGDSThirdPartyVendorIdRequired", "1")%>
                            <% } else { %>
                                <%= Html.Hidden("IsGDSThirdPartyVendorIdRequired", "")%>
                            <% } %>
						</td>
					</tr>
				   <tr>
						<td width="40%" class=""></td>
						<td width="40%" class=""></td>
						<td width="20%" class=""></td>
					</tr>
					<tr>
						<td colspan="3" style="padding: 0;">
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
										<th>&nbsp;</th>
									</tr>
								</thead>
								<tbody>
									<% int counter = 0;
									if (Model.GDSOrder.GDSOrderLineItems != null && Model.GDSOrder.GDSOrderLineItems.Count > 0){ %>
										<% foreach (CWTDesktopDatabase.Models.GDSOrderLineItem item in Model.GDSOrder.GDSOrderLineItems) { %>
											<tr class="gds-order-line-item">
												<td><%= Html.TextBox("GDSOrder.GDSOrderLineItem[" + counter + "].Quantity", item.Quantity, new { maxlength = "3", @Class = "Quantity" })%></td>
												<td><%= Html.DropDownList("GDSOrder.GDSOrderLineItem[" + counter + "].GDSOrderLineItemActionId", item.GDSOrderLineItemActions as SelectList, "Please Select...", new { @Class = "GDSOrderLineItemActionId" } )%></td>
												<td><%= Html.DropDownList("GDSOrder.GDSOrderLineItem[" + counter + "].GDSOrderDetailId", item.GDSOrderDetails as SelectList, "Please Select...", new { @Class = "GDSOrderDetailId" })%></td>
												<td><%= Html.TextArea("GDSOrder.GDSOrderLineItem[" + counter + "].Comment", item.Comment, new { maxlength = "250", @Class = "Comment" })%></td>
												<td>
													<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
													<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
												</td>
											</tr>
										<% counter++;
										} %>
									<% } else { %>
										<tr class="gds-order-line-item">
											<td><input type="text" name="GDSOrder.GDSOrderLineItem[0].Quantity" value="1" maxlength="3" class="Quantity" /></td>
											<td><%= Html.DropDownList("GDSOrder.GDSOrderLineItem[0].GDSOrderLineItemActionId", Model.GDSOrderLineItemActions as SelectList, "Please Select...", new { @Class = "GDSOrderLineItemActionId" })%></td>
											<td>
												<select name="GDSOrder.GDSOrderLineItem[0].GDSOrderDetailId" class="GDSOrderDetailId">
													<option value="">Please Select...</option>
												</select>
											</td>
											<td><textarea cols="10" rows="10" name="GDSOrder.GDSOrderLineItem[0].Comment" class="Comment"></textarea></td>
											<td>
												<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
												<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
											</td>
										</tr>
									<% } %>
								</tbody>
							</table>
						</td>
					</tr>
					<tr>
						<td width="40%" class=""></td>
						<td width="40%" class=""></td>
						<td width="20%" class=""></td>
					</tr>

					<% if(Model.GDSOrder.GDSOrderType != null && Model.GDSOrder.GDSOrderType.ShowDataFlag == true) { %>

						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.IATAId">IATA</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.IATAId, Model.IATAs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.IATAId)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.GDSCode">GDS</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GDSCode, Model.GDSs as SelectList, "Please Select...", new { @Class = "PseudoCityOrOfficeMaintenanceGDSCode" })%> <span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GDSCode)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId">PCC/OID Address</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId, Model.PseudoCityOrOfficeAddresses as SelectList, "Please Select...")%> <span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeAddressId)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.LocationContactName">Location Contact Name</label></td>
							<td><%= Html.TextBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.LocationContactName, new { maxlength = "255" })%><span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.LocationContactName)%></td>
						</tr>  
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.LocationContactName">Location Phone</label></td>
							<td><%= Html.TextBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.LocationPhone, new { maxlength = "20" })%><span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.LocationPhone)%></td>
						</tr> 
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.CountryName">Country</label></td>
							<td><%= Html.TextBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CountryName, new { @disabled = "disabled" })%><span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CountryName)%></td>
						</tr> 
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.GlobalRegionName">Global Region</label></td>
							<td>
								<%= Html.TextBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GlobalRegionName, new { @disabled = "disabled" })%><span class="error"> *</span>
								<%= Html.Hidden("PseudoCityOrOfficeMaintenance.GlobalRegionCode") %>
							</td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GlobalRegionName)%></td>
						</tr> 
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId">Pseudo City/Office ID Defined Region</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId, Model.PseudoCityOrOfficeDefinedRegions as SelectList, "Please Select...")%><span id="GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error" class="error"> *</span></td>
							<td>
								<%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeDefinedRegionId)%>
								<label id="GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label" class="error">The Pseudo City/Office ID Defined Region is required</label>
							</td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable">Active</label></td>
							<td><%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable)%></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ActiveFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.InternalSiteName">Internal Site Name</label></td>
							<td><%= Html.TextBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.InternalSiteName, new { maxlength = "50" })%><span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.InternalSiteName)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.ExternalNameId">External Name</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ExternalNameId , Model.ExternalNames as SelectList, "Please Select...")%> <span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ExternalNameId)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId">Pseudo City/Office ID Type</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId , Model.PseudoCityOrOfficeTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeTypeId)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId">Pseudo City/Office ID Location Type</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId , Model.PseudoCityOrOfficeLocationTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeLocationTypeId)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.FareRedistributionId">Fare Redistribution</label></td>
							<td><%= Html.DropDownListFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.FareRedistributionId, Model.FareRedistributions as SelectList, "Please Select...", new { @Class = "FareRedistributionId" } )%> <span id="GDSOrder_PseudoCityOrOfficeMaintenance.FareRedistributionId_Error" class="error"> *</span></td>
							<td>
								<%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.FareRedistributionId)%>
								<label id="GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Label" class="error">The Fare Redistribution is required</label>
							</td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable">Shared PCC/Office ID?</label></td>
							<td><%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable)%></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.SharedPseudoCityOrOfficeFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable">CWT Owned PCC/Office ID?</label></td>
							<td><%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable)%></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CWTOwnedPseudoCityOrOfficeFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable">Client Dedicated PCC/Office ID?</label></td>
							<td><%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable)%></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ClientDedicatedPseudoCityOrOfficeFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable">Client GDS Access?</label></td>
							<td><%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable)%></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.ClientGDSAccessFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable">Development/Internal PCC/Office ID?</label></td>
							<td><%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable)%></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.DevelopmentOrInternalPseudoCityOrOfficeFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable">Cuba PCC/Office ID?</label></td>
							<td>
								<%if (ViewData["ComplianceAdministratorAccess"] == "WriteAccess"){ %>
									<%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable)%>
								<% } else { %>
									<%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable, new { @disabled = "disabled" })%>
									<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable)%>
								<% } %>
							</td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.CubaPseudoCityOrOfficeFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable">Government PCC/Office ID?</label></td>
							<td>
								<%if (ViewData["GDSGovernmentAdministratorAccess"] == "WriteAccess"){ %>
									<%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable)%>
								<% } else { %>
									<%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable, new { @disabled = "disabled" })%>
									<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable)%>
								<% } %>
							</td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GovernmentPseudoCityOrOfficeFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable">3<sup>rd</sup> Party Vendor?</label></td>
							<td><%= Html.CheckBoxFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable)%></td>
							<td><%= Html.ValidationMessageFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorFlagNonNullable)%></td>
						</tr>
						<tr>
							<td><label for="PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorId">3<sup>rd</sup> Party Vendor(s)</label></td>
							<td>
								<%= Html.ListBoxFor(x => x.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorIds, Model.PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorsList)%> <span id="PseudoCityOrOfficeMaintenance.GDSThirdPartyVendorIds_Error" class="error"> *</span><br/>
								<label>Hold CNTRL to select multiple</label>
							</td>
							<td>
								<label id="PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Label" class="error">The 3rd Party Vendor(s) is required</label>
								<%= Html.ValidationMessageFor(model => model.GDSThirdPartyVendors)%>
							</td>
						</tr>
						<tr>
							<td width="40%" class="row_footer_left"></td>
							<td width="40%" class="row_footer_centre"></td>
							<td width="20%" class="row_footer_right"></td>
						</tr>
					
					<% } else { %>

						<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.LocationContactName)%>
						<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.LocationPhone)%>

					<% } %>
					
					<tr>
						<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
						<td class="row_footer_blank_right"><input type="submit" value="Edit GDS Order" title="Edit GDS Order" class="red"/></td>
					</tr>
				</table>

			<%= Html.HiddenFor(model => model.GDSOrder.GDSOrderId) %>
			<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeId)%>
			<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenanceId)%>

			<%= Html.HiddenFor(model => model.GDSOrder.GDSOrderTypeId)%>

			<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeId)%>
			<%= Html.HiddenFor(model => model.GDSOrder.PseudoCityOrOfficeMaintenance.PseudoCityOrOfficeMaintenanceId)%>
			
			<%= Html.HiddenFor(model => model.GDSOrder.VersionNumber) %>

			<%= Html.Hidden("PageType", "Edit") %>

		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/GDSOrder.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("GDS Orders", "Main", new { controller = "GDSOrder", action = "List", }, new { title = "GDS Orders" })%> &gt;
Edit &gt;
<% =Html.Encode(Model.GDSOrder.PseudoCityOrOfficeId) %>
</asp:Content>