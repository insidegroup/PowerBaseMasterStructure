<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.CommissionableRouteItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Commissionable Route Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
     <div id="banner"><div id="banner_text">Commissionable Route Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Commissionable Route Items</th> 
		        </tr> 
                <tr>
                    <td>Group Name</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.CommissionableRouteGroup.CommissionableRouteGroupName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.Product.ProductName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.Supplier.SupplierName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Travel Indicator</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.TravelIndicator)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Class of Service</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.ClassOfTravel)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Commission Amount</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.CommissionAmount)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Commission Currency</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.CommissionAmountCurrencyCode)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Commission %</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.BSPCommission)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Commission % on Tax</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.CommissionOnTaxes)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Commission Tax Code</td>
                    <td style="word-wrap: break-word"><%= Html.Encode(Model.CommissionableRouteItem.CommissionableTaxCodes)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Include on Negotiated Fare?</td>
                    <td><%= Html.Encode(Model.CommissionableRouteItem.NegotiatedFareFlag)%></td>
                    <td></td>
                </tr>        
                <tr valign="top">
                    <td>Remarks</td>
                    <td style="word-wrap: break-word"><%= Html.Encode(Model.CommissionableRouteItem.RemarksOrRoute)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
                <tr>
                    <td>Policy Routing Name</td>
                    <td><%= Html.Encode(Model.PolicyRouting.Name)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>From Global?</td>
                    <td><%if (Model.PolicyRouting.PolicyRoutingId > 0) {  %><%= Html.Encode(Model.PolicyRouting.FromGlobalFlag)%><%} %></td>
                    <td></td>
                </tr>
                <tr>
                    <td>From</td>
                    <td><%= Html.Encode(Model.PolicyRouting.FromCode)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>To Global?</td>
                    <td> <%if (Model.PolicyRouting.PolicyRoutingId > 0){  %><%= Html.Encode(Model.PolicyRouting.ToGlobalFlag)%><%} %></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>To</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToCode)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Routing Vice Versa? </td>
                    <td><%if (Model.PolicyRouting.PolicyRoutingId > 0){  %><%= Html.Encode(Model.PolicyRouting.RoutingViceVersaFlag)%><%} %></td>
                    <td></td>
                </tr>
                 <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back", "List", new { id = Model.CommissionableRouteItem.CommissionableRouteGroupId }, new { @class = "red", title = "Back" })%></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.CommissionableRouteItem.CommissionableRouteItemId)%>
						<%= Html.HiddenFor(model => model.CommissionableRouteItem.CommissionableRouteGroupId)%>
						<%= Html.HiddenFor(model => model.CommissionableRouteItem.VersionNumber)%>
                    <%}%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_commissionableroutes').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#breadcrumb').css('width', 'auto');
	})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Commissionable Route Groups", "Main", new { controller = "CommissionableRouteGroup", action = "ListUnDeleted", id = ViewData["CommissionableRouteGroupId"] }, new { title = "Commissionable Route Groups" })%> &gt;
<%=ViewData["CommissionableRouteGroupName"]%> &gt;
Create Commissionable Route Items
</asp:Content>
