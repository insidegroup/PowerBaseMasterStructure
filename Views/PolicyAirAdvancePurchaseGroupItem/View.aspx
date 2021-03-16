<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirParameterGroupItemVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Vendor Group Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Policy Air Vendor Group Item</th> 
		        </tr> 
                <tr>
                    <td>Advance Purchase Days</td>
                    <td><%= Html.Encode(Model.PolicyAirParameterGroupItem.PolicyAirParameterValue)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.PolicyAirParameterGroupItem.EnabledFlag)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.PolicyAirParameterGroupItem.EnabledDate.HasValue ? Model.PolicyAirParameterGroupItem.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.PolicyAirParameterGroupItem.ExpiryDate.HasValue ? Model.PolicyAirParameterGroupItem.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Travel Date Valid From</td>
                    <td><%= Html.Encode(Model.PolicyAirParameterGroupItem.TravelDateValidFrom.HasValue ? Model.PolicyAirParameterGroupItem.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No Travel Date Valid From")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Travel Date Valid To</td>
                    <td><%= Html.Encode(Model.PolicyAirParameterGroupItem.TravelDateValidTo.HasValue ? Model.PolicyAirParameterGroupItem.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No Travel Date Valid To")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td class="row_footer_blank" colspan="3"></td>
                </tr>
                <tr> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
                <tr>
                    <td>Name</td>
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
                    <td>
						<%if (Model.PolicyRouting.FromCode != null) { %>
							<%= Html.Encode(Model.PolicyRouting.FromCode) %>
						 <% } %>
                    </td>
					<td>
						<%if (Model.PolicyRouting.FromName != null) { %>
							<%= Html.Encode(string.Format("({0})", Model.PolicyRouting.FromName)) %>
						 <% } %>
					</td>
                </tr> 
                 <tr>
                    <td>To Global?</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToGlobalFlag)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>To</td>
                    <td>
						<%if (Model.PolicyRouting.ToCode != null) { %>
							<%= Html.Encode(Model.PolicyRouting.ToCode) %>
						 <% } %>
                    </td>
					<td>
						<%if (Model.PolicyRouting.ToName != null) { %>
							<%= Html.Encode(string.Format("({0})", Model.PolicyRouting.ToName)) %>
						 <% } %>
					</td>
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
<%=Html.RouteLink("Policy Air Advance Purchase Group Items", "Default", new { controller = "PolicyAirParameterGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Vendor Group Items" })%> &gt;
Item
</asp:Content>