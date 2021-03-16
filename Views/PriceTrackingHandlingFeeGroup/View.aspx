<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PriceTrackingHandlingFeeGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Group</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Price Tracking Group</th> 
		        </tr> 
                <tr>
                    <td><label for="PriceTrackingHandlingFeeGroupName">Group Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PriceTrackingHandlingFeeGroupName)%></td>
                </tr> 
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label></td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="DeletedFlag">Deleted?</label></td>
                    <td><%= Html.Encode(Model.DeletedFlag == true ? "True" : "False")%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><label for="DeletedDateTime">Deleted Date/Time</label></td>
                    <td><%= Html.Encode(Model.DeletedDateTime.HasValue ? Model.DeletedDateTime.Value.ToShortDateString() : "No Deleted Date")%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label> </td>
                    <td><%= Html.Encode(Model.HierarchyType)%></td>
                    <td></td>
                </tr>
                <% if (Model.HierarchyType == "ClientSubUnit" || Model.HierarchyType == "TravelerType")
                    { %>
                   
                    <tr>
                        <td>Hierarchy Item</td>
                        <td><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.ClientTopUnitName)%></td>
                        <td></td>
                    </tr>

                <% } else if (Model.HierarchyType == "ClientAccount") { %>
                   
                    <tr>
                        <td>Hierarchy Item</td>
                        <td><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.HierarchyCode)%>,  <%= Html.Encode(Model.SourceSystemCode)%></td>
                        <td></td>
                    </tr>

                <% } else if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                   
                    <tr>
                        <td><label for="ClientSubUnitName">Client Sub Unit Name</label> </td>
                        <td colspan="2"><%= Html.Encode(Model.ClientSubUnitName)%><%= Html.Encode(!string.IsNullOrEmpty(Model.ClientTopUnitName) ? string.Format(", {0}", Model.ClientTopUnitName) : "") %></td>
                    </tr> 
                    <tr>
                        <td><label for="TravelerTypeName">Traveler Type</label> </td>
                        <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                        <td></td>
                    </tr> 

                 <%}else{ %>

                    <% if (Model.HierarchyType != null){ %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td><%= Html.Encode(Model.HierarchyItem)%></td>
                            <td></td>
                        </tr> 
                   <%}else{%>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td></td>
                            <td></td>
                        </tr> 
                   <%}%>
                <% }%>
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
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Groups", "Main", new { controller = "PriceTrackingHandlingFeeGroup", action = "ListUnDeleted", }, new { title = "Price Tracking Groups" })%> &gt;
<%=Model.PriceTrackingHandlingFeeGroupName%>
</asp:Content>