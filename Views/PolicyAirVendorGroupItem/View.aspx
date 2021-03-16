<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirVendorGroupItemVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Vendor Group Item</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Policy Air Vendor Group Item</th> 
		        </tr> 
                <tr>
                    <td>Policy Group Name</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.PolicyGroupName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Air Status</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.PolicyAirStatus)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.EnabledFlag)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.EnabledDate.HasValue ? Model.PolicyAirVendorGroupItem.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.ExpiryDate.HasValue ? Model.PolicyAirVendorGroupItem.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Travel Date Valid From</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.TravelDateValidFrom.HasValue ? Model.PolicyAirVendorGroupItem.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No Travel Date Valid From")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Travel Date Valid To</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.TravelDateValidTo.HasValue ? Model.PolicyAirVendorGroupItem.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No Travel Date Valid To")%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.ProductName)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.SupplierName)%></td>
                    <td></td>
                </tr>   
                 <tr>
                    <td>Ranking</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.AirVendorRanking)%></td>
                    <td></td>
                </tr>                            
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                 <tr>
                    <td class="row_footer_blank" colspan="3"></td>
                </tr>
                <tr> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
                <tr>
                    <td>Routing Name</td>
                    <td><%= Html.Encode(Model.PolicyRouting.Name)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>From Global?</td>
                    <td><%= Html.Encode(Model.PolicyRouting.FromGlobalFlag)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>From</td>
                    <td><%= Html.Encode(Model.PolicyRouting.FromName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>To Global?</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToGlobalFlag)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>To</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Routing Vice Versa?</td>
                    <td><%= Html.Encode(Model.PolicyRouting.RoutingViceVersaFlag)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right"></td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_policies').click();
	    $("tr:odd").addClass("row_odd");
	    $("tr:even").addClass("row_even");
	    //for pages with long breadcrumb and no search box
	    $('#breadcrumb').css('width', '725px');
	    $('#search').css('width', '5px');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Vendor Group Items", "Default", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Vendor Group Items" })%> &gt;
Item
</asp:Content>