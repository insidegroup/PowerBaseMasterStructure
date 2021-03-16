<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PriceTrackingSetupGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Setup Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
        <div id="banner"><div id="banner_text">Price Tracking Setup</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Price Tracking Setup</th> 
		        </tr> 
                <tr>
                    <td><label for="PriceTrackingSetupGroupName">Price Tracking Setup Name</label></td>
                    <td><%= Html.Encode(Model.PriceTrackingSetupGroupName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingSetupTypeId">Setup Type</label></td>
                    <td><%= Html.Encode(Model.PriceTrackingSetupType.PriceTrackingSetupTypeName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.Encode(Model.GDS.GDSName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="PseudoCityOrOfficeId">PCC/OID</label></td>
                    <td><%= Html.Encode(Model.PseudoCityOrOfficeId) %></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="SharedPseudoCityOrOfficeIdFlag">Shared PCC/OID?</label></td>
                    <td><%= Html.Encode(Model.SharedPseudoCityOrOfficeIdFlag.HasValue ? Model.SharedPseudoCityOrOfficeIdFlag.Value.ToString() : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Additional PCC/OID/Shared PCC/OID?</td>
                    <td>
                        <% if (Model.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds != null && Model.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds.Count > 0){ %>
                           <% =Html.Encode(String.Join(", ", Model.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeIds.Select(x => x.PseudoCityOrOfficeId + "/" + x.SharedPseudoCityOrOfficeIdFlag).ToList())) %>
                        <% } %>
                    </td>
                    <td></td>
				</tr>
                <tr>
                    <td><label for="FIQID">FIQID</label></td>
                    <td><%= Html.Encode(Model.FIQID)%></td>
                    <td></td>
                </tr>
                <% if (Model.IsMultipleHierarchy && Model.ClientSubUnitsHierarchy != null) { %>
                    <tr>
                        <td>Hierarchy Type</td>
                        <td colspan="2">ClientSubUnit</td>
                    </tr>
                    <tr>
                        <td>Client Top Unit Name</td>
                        <td colspan="2"><%= Html.Encode(!string.IsNullOrEmpty(Model.ClientTopUnitName) ? Model.ClientTopUnitName : "")%></td>
                    </tr> 
                    <tr>
                        <td>Client Sub Unit Names</td>
                        <td colspan="2"> 
                            <table cellpadding="4" cellspacing="0" border="0" width="100%" class="hierarchyTable">
                                <tbody>
                                    <% foreach (CWTDesktopDatabase.Models.ClientSubUnit clientSubUnit in Model.ClientSubUnitsHierarchy) {%>
                                        <tr>
                                            <td width="50%"><%= Html.Raw(clientSubUnit.ClientSubUnitName) %></td>
                                            <td width="50%"><%= Html.Raw(clientSubUnit.Country.CountryName) %></td>
                                        </tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                <% } else { %>
                    <tr>
                        <td><label for="HierarchyType">Hierarchy Type</label> </td>
                        <td colspan="2"><%= Html.Encode(Model.HierarchyType)%></td>
                    </tr>
                    <% if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                            <tr>
                                <td>Client Subunit Name</td>
                                <td colspan="2"><%= Html.Encode(Model.ClientSubUnitName)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                            </tr> 
                            <tr>
                                <td>Traveler Type Name</td>
                                <td colspan="2"><%= Html.Encode(Model.TravelerTypeName)%></td>
                            </tr> 
                   <% } else if (Model.HierarchyType == "TravelerType"){ %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td colspan="2"><%= Html.Encode(Model.TravelerTypeName)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                        </tr> 
                    <% } else if (Model.HierarchyType == "ClientSubUnit"){ %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td colspan="2"><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                        </tr> 
                    <% } else if (Model.HierarchyType == "ClientAccount"){ %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td colspan="2"><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.HierarchyCode)%>, <%= Html.Encode(Model.SourceSystemCode)%></td>
                        </tr> 
                    <%} else { %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td colspan="2"><%= Html.Encode(Model.HierarchyItem != null ? Model.HierarchyItem : "")%></td>
                        </tr> 
                    <% } %>
                <% } %>
                <tr>
                    <td><label for="DesktopUsedTypeId">GDS Counsellor Desktop</label></td>
                    <td><%= Html.Encode(Model.DesktopUsedType != null ? Model.DesktopUsedType.DesktopUsedTypeDescription : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingMidOfficePlatformId">Mid Office Platform</label></td>
                    <td><%= Html.Encode(Model.PriceTrackingMidOfficePlatform != null ? Model.PriceTrackingMidOfficePlatform.PriceTrackingMidOfficePlatformName : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="MidOfficeUsedForQCTicketingFlag">Mid Office used for QC and Ticketing?</label></td>
                    <td><%= Html.Encode(Model.MidOfficeUsedForQCTicketingFlag.HasValue ? Model.MidOfficeUsedForQCTicketingFlag.Value.ToString() : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingItinerarySolutionId">Itinerary Solution</label></td>
                    <td><%= Html.Encode(Model.PriceTrackingItinerarySolution != null ? Model.PriceTrackingItinerarySolution.PriceTrackingItinerarySolutionName : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingSystemRuleId">System Rules</label></td>
                    <td><%= Html.Encode(Model.PriceTrackingSystemRule != null ? Model.PriceTrackingSystemRule.PriceTrackingSystemRuleName : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="USGovernmentContractorFlag">US Government Contractor?</label></td>
                    <td><%= Html.Encode(Model.USGovernmentContractorFlag.HasValue ? Model.USGovernmentContractorFlag.Value.ToString() : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="BackOfficeSystemId">Back Office System</label></td>
                    <td><%= Html.Encode(Model.BackOfficeSystem != null ? Model.BackOfficeSystem.BackOfficeSystemDescription : "")%></td>
                    <td></td>
                </tr>
                <tr>
					<td><label>Traveler Types to Exclude</label></td>
                    <td>
					    <% if (Model.PriceTrackingSetupGroupExcludedTravelerTypes != null && Model.PriceTrackingSetupGroupExcludedTravelerTypes.Count > 0){ %>
                           <% =Html.Encode(String.Join(", ", Model.PriceTrackingSetupGroupExcludedTravelerTypes.Select(x => x.TravelerTypeName).ToList())) %>
					    <% } %>
					</td>
                    <td></td>
				</tr>
                <tr>
                    <td><label for="OtherExclusions">Other Exclusions</label><br /><br /></td>
                    <td><%= Html.Encode(Model.OtherExclusions)%></td>
                    <td></td>
                </tr>
                <tr> 
                    <td colspan="3"><strong>Billing Model Section</strong></td> 
                </tr>
                <tr>
                    <td><label for="AirPriceTrackingBillingModelId">Air</label></td>
                    <td><%= Html.Encode(Model.AirPriceTrackingBillingModel != null ? Model.AirPriceTrackingBillingModel.PriceTrackingBillingModelName : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="HotelPriceTrackingBillingModelId">Hotel</label></td>
                    <td><%= Html.Encode(Model.HotelPriceTrackingBillingModel != null ? Model.HotelPriceTrackingBillingModel.PriceTrackingBillingModelName : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="PreTicketPriceTrackingBillingModelId">Pre-Ticket</label></td>
                    <td><%= Html.Encode(Model.PreTicketPriceTrackingBillingModel != null ? Model.PreTicketPriceTrackingBillingModel.PriceTrackingBillingModelName : "")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="DeletedFlag">Deleted?</label></td>
                    <td><%= Html.Encode(Model.DeletedFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><label for="DeletedDateTime">Deleted Date/Time</label></td>
                    <td><%= Html.Encode(Model.DeletedDateTime.HasValue ? string.Format("{0} {1}", Model.DeletedDateTime.Value.ToShortDateString(), Model.DeletedDateTime.Value.ToLongTimeString()) : "No Deleted Date")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_pricetracking').click();
        $("#content > table > tbody > tr:odd").addClass("row_odd");
        $("#content > table > tbody > tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Setups", "Main", new { controller = "PriceTrackingSetupGroup", action = "ListUnDeleted", }, new { title = "Price Tracking Setups" })%> &gt;
<%=Html.Encode(Model.PriceTrackingSetupGroupName)%> &gt;
View
</asp:Content>