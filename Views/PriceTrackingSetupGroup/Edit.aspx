<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PriceTrackingSetupGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Setup Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Setup</div></div>
        <div id="content">       
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "PriceTrackingSetupGroup", action = "Edit",id = Model.PriceTrackingSetupGroupId }, FormMethod.Post, new { id = "form0", autocomplete="off" })) {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Price Tracking Setup</th> 
		        </tr> 
                <tr>
                    <td>Price Tracking Setup Name</td>
                    <td><%= Html.Raw(Model.PriceTrackingSetupGroupName) %></td>
                    <td><%= Html.HiddenFor(model => model.PriceTrackingSetupGroupName) %><label id="lblPriceTrackingSetupGroupNameMsg"/></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingSetupTypeId">Setup Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingSetupTypeId, ViewData["PriceTrackingSetupTypes"] as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PriceTrackingSetupTypeId)%></td>
                </tr>
                <tr>
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.GDSCode, ViewData["GDSs"] as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSCode)%></td>
                </tr>
                <tr>
                    <td><label for="PseudoCityOrOfficeId">PCC/OID</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeId, new { size = "9" })%> <span class="error"> *</span></td>
                    <td>
                        <label id="lblValidBookingPseudoCityOrOfficeIdMessage"></label>
                        <%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeId)%></td>
                </tr>
                <tr>
                    <td><label for="SharedPseudoCityOrOfficeIdFlag">Shared PCC/OID?</label></td>
                    <td><%= Html.DropDownListFor(model => model.SharedPseudoCityOrOfficeIdFlagSelectedValue, ViewData["SharedPseudoCityOrOfficeList"] as SelectList)%></td>
                    <td>
                        <%= Html.HiddenFor(model => model.SharedPseudoCityOrOfficeIdFlag)%>
                        <%= Html.ValidationMessageFor(model => model.SharedPseudoCityOrOfficeIdFlag)%>
                    </td>
                </tr>
                <tr>
					<td colspan="3" style="padding: 0;">
						<table cellpadding="0" cellspacing="0" border="0" width="100%">
							<tbody>
								<% int counter = 0;
								if (Model.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds != null && Model.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds.Count > 0){ %>
									<% foreach (CWTDesktopDatabase.Models.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId item in Model.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds) { %>
										<tr class="PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item">
											<td width="30%">Additional PCC/OID</td>
											<td width="25%"><%= Html.TextBox("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[" + counter + "].PseudoCityOrOfficeId", item.PseudoCityOrOfficeId, new { maxlength = "9", @Class = "PseudoCityOrOfficeId" })%></td>
											<td width="15%">Shared PCC/OID?</td>
											<td width="10%">
                                                <%= Html.DropDownList("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[" + counter + "].SharedPseudoCityOrOfficeIdFlagSelectedValue", item.SharedPseudoCityOrOfficeIdFlagList as SelectList, new { @Class = "SharedPseudoCityOrOfficeList" } )%>
                                                <%= Html.Hidden("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[" + counter + "].SharedPseudoCityOrOfficeIdFlag", item.SharedPseudoCityOrOfficeIdFlag, new { @Class = "SharedPseudoCityOrOfficeIdFlag"})%>
											</td>
											<td width="30%">
												<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
												<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
											</td>
										</tr>
									<% counter++;
									} %>
								<% } else { %>
									<tr class="PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item">
										<td width="30%">Additional PCC/OID</td>
                                        <td width="25%"><input type="text" name="PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[0].PseudoCityOrOfficeId" value="" maxlength="9" class="PseudoCityOrOfficeId" /></td>
										<td width="15%">Shared PCC/OID?</td>	
                                        <td width="10%">
                                            <%= Html.DropDownList("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[0].SharedPseudoCityOrOfficeIdFlagSelectedValue", ViewData["PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIdList"] as SelectList, new { @Class = "SharedPseudoCityOrOfficeList" } )%>
                                            <%= Html.Hidden("PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[0].SharedPseudoCityOrOfficeIdFlag", "false", new { @Class = "SharedPseudoCityOrOfficeIdFlag"})%>
                                        </td>
										<td width="30%">
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
                    <td><label for="FIQID">FIQID</label></td>
                    <td>
                        <%if (ViewData["FIQIDAccess"] == "WriteAccess") { %>
                            <%= Html.TextBoxFor(model => model.FIQID, new { size = "30", autocomplete="off" })%><span class="error"> *</span>
                        <% } else { %>
                            <%= Html.Encode(Model.FIQID)%>
                            <%= Html.HiddenFor(model => model.FIQID)%>
                        <% } %>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.FIQID)%></td>
                </tr>
                <% if(!Model.IsMultipleHierarchy){ %>
                    <tr>
                        <td><label for="HierarchyType">Hierarchy Type</label></td>
                        <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                        <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                    </tr>               
                    <tr>
                        <td><label id="lblHierarchyItem"/>HierarchyItem</td>
                        <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled="disabled",  size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                        <td>
                            <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                            <%= Html.Hidden("HierarchyCode")%>
						    <%= Html.Hidden("TT_CSU")%>
                            <label id="lblHierarchyItemMsg"/>
                        </td>
                    </tr> 
                    <tr style="display:none" id="TravelerType">
                        <td><label for="TravelerTypeName">Traveler Type</label></td>
                        <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                        <td>
                            <%= Html.ValidationMessageFor(model => model.TravelerTypeGuid)%>
                            <%= Html.Hidden("TravelerTypeGuid")%>
                            <label id="lblTravelerTypeMsg"/>
                        </td>
                    </tr>
                <% } %>
                <tr>
                    <td><label for="DesktopUsedTypeId">GDS Counsellor Desktop</label></td>
                    <td><%= Html.DropDownListFor(model => model.DesktopUsedTypeId, ViewData["DesktopUsedTypes"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.DesktopUsedTypeId)%></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingMidOfficePlatformId">Mid Office Platform</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingMidOfficePlatformId, ViewData["PriceTrackingMidOfficePlatforms"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PriceTrackingMidOfficePlatformId)%></td>
                </tr>
                <tr>
                    <td><label for="MidOfficeUsedForQCTicketingFlag">Mid Office used for QC and Ticketing?</label></td>
                    <td><%= Html.DropDownListFor(model => model.MidOfficeUsedForQCTicketingFlagSelectedValue, ViewData["MidOfficeUsedForQCTicketingList"] as SelectList)%></td>
                    <td>
                        <%= Html.HiddenFor(model => model.MidOfficeUsedForQCTicketingFlag)%>
                        <%= Html.ValidationMessageFor(model => model.MidOfficeUsedForQCTicketingFlag)%>
                    </td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingItinerarySolutionId">Itinerary Solution</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingItinerarySolutionId, ViewData["PriceTrackingItinerarySolutions"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PriceTrackingItinerarySolutionId)%></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingSystemRuleId">System Rules</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingSystemRuleId, ViewData["PriceTrackingSystemRules"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PriceTrackingSystemRuleId)%></td>
                </tr>
                <tr>
                    <td><label for="USGovernmentContractorFlag">US Government Contractor?</label></td>
                    <td><%= Html.DropDownListFor(model => model.USGovernmentContractorFlagSelectedValue, ViewData["USGovernmentContractorList"] as SelectList)%></td>
                    <td>
                        <%= Html.HiddenFor(model => model.USGovernmentContractorFlag)%>
                        <%= Html.ValidationMessageFor(model => model.USGovernmentContractorFlag)%>
                    </td>
                </tr>
                <tr>
                    <td><label for="BackOfficeSystemId">Back Office System</label></td>
                    <td><%= Html.DropDownListFor(model => model.BackOfficeSystemId, ViewData["BackOfficeSystems"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.BackOfficeSystemId)%></td>
                </tr>
                <tr>
					<td><label>Traveler Types to Exclude</label></td>
                    <td colspan="2" style="padding: 0;">
						<table cellpadding="0" cellspacing="0" border="0" width="100%">
							<tbody>
								<% int traveler_counter = 0;
								if (Model.PriceTrackingSetupGroupExcludedTravelerTypes != null && Model.PriceTrackingSetupGroupExcludedTravelerTypes.Count > 0){ %>
									<% foreach (CWTDesktopDatabase.Models.PriceTrackingSetupGroupExcludedTravelerType item in Model.PriceTrackingSetupGroupExcludedTravelerTypes) { %>
										<tr class="PriceTrackingSetupGroupExcludedTravelerType_Line_Item">
											<td width="70%">
                                                <%= Html.TextBox("PriceTrackingSetupGroupExcludedTravelerType[" + traveler_counter + "].TravelerTypeName", item.TravelerTypeName, new { @Class = "TravelerTypeName" })%>
                                                <%= Html.Hidden("PriceTrackingSetupGroupExcludedTravelerType[" + traveler_counter + "].TravelerTypeGuid", item.TravelerTypeGuid, new { @Class = "TravelerTypeGuid" })%>
											</td>
                                            <td width="30%">
												<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
												<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
											</td>
										</tr>
									<% traveler_counter++;
									} %>
								<% } else { %>
									<tr class="PriceTrackingSetupGroupExcludedTravelerType_Line_Item">
										<td width="70%">
                                            <%= Html.TextBox("PriceTrackingSetupGroupExcludedTravelerType[0].TravelerTypeName", "", new { @Class = "TravelerTypeName" })%>
                                            <%= Html.Hidden("PriceTrackingSetupGroupExcludedTravelerType[0].TravelerTypeGuid", "", new { @Class = "TravelerTypeGuid" })%>
                                        </td>
										<td width="30%">
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
                    <td><label for="OtherExclusions">Other Exclusions</label><br /><br /></td>
                    <td><%= Html.TextAreaFor(model => model.OtherExclusions, new { maxlength = "240" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OtherExclusions)%></td>
                </tr>
                <tr> 
                    <td colspan="3"><strong>Billing Model Section</strong></td> 
                </tr>
                <tr>
                    <td><label for="AirPriceTrackingBillingModelId">Air</label></td>
                    <td><%= Html.DropDownListFor(model => model.AirPriceTrackingBillingModelId, ViewData["AirPriceTrackingBillingModels"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.AirPriceTrackingBillingModelId)%></td>
                </tr>
                <tr>
                    <td><label for="HotelPriceTrackingBillingModelId">Hotel</label></td>
                    <td><%= Html.DropDownListFor(model => model.HotelPriceTrackingBillingModelId, ViewData["HotelPriceTrackingBillingModels"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.HotelPriceTrackingBillingModelId)%></td>
                </tr>
                <tr>
                    <td><label for="PreTicketPriceTrackingBillingModelId">Pre-Ticket</label></td>
                    <td><%= Html.DropDownListFor(model => model.PreTicketPriceTrackingBillingModelId, ViewData["PreTicketPriceTrackingBillingModels"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PreTicketPriceTrackingBillingModelId)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Price Tracking Setup" title="Edit Price Tracking Setup" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.VersionNumber)%>
            <%= Html.HiddenFor(model => model.PriceTrackingSetupGroupId) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
            <%if(Model.IsMultipleHierarchy){ %>
                <%= Html.HiddenFor(model => model.HierarchyType)%>
                <%= Html.Hidden("HierarchyCode", Model.HierarchyCode)%>
                <%= Html.HiddenFor(model => model.HierarchyItem)%>
                <%= Html.HiddenFor(model => model.TravelerTypeGuid)%>
                <%= Html.HiddenFor(model => model.IsMultipleHierarchy)%>
            <% } %>
        <% } %>
        </div>
    </div>

<script src="<%=Url.Content("~/Scripts/ERD/PriceTrackingSetupGroup.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Setups", "Main", new { controller = "PriceTrackingSetupGroup", action = "ListUnDeleted", }, new { title = "Price Tracking Setups" })%> &gt;
<%=Html.Encode(Model.PriceTrackingSetupGroupName)%> &gt;
Edit
</asp:Content>