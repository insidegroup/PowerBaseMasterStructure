<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicySupplierDealCode>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Supplier Deal Codes</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="2">View Policy Supplier Deal Code</th> 
		    </tr> 
            <tr>
                <td>Deal Code Value</td>
                <td><%= Html.Encode(Model.PolicySupplierDealCodeValue)%></td>
            </tr>
            <tr>
                <td>Deal Code Description</td>
                <td class="wrap-text"><%= Html.Encode(Model.PolicySupplierDealCodeDescription)%></td>
            </tr>
                <tr>
                <td>Deal Code Type</td>
                <td><%= Html.Encode(Model.PolicySupplierDealCodeTypeDescription)%></td>
            </tr>
            <tr>
                <td>GDS</td>
                <td><%= Html.Encode(Model.GDSName)%></td>
            </tr>  
                <tr>
                <td>Enabled?</td>
                <td><%= Html.Encode(Model.EnabledFlagNonNullable)%></td>
            </tr>  
            <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
            </tr> 
            <tr>
                <td>Supplier</td>
                <td><%= Html.Encode(Model.SupplierName)%></td>
            </tr> 
            <tr>
                <td>Product</td>
                <td><%= Html.Encode(Model.ProductName)%></td>
            </tr> 
            <tr>
                <td>Policy Location</td>
                <td><%= Html.Encode(Model.PolicyLocationName)%></td>
            </tr> 
			<% if (Model.PolicySupplierDealCodeTypeDescription == "Tour Code") { %>
				<tr>
					<td>Travel Indicator</td>
					<td><%= Html.Encode(Model.TravelIndicator)%></td>
				</tr>
				<tr>
					<td>Tour Code Type</td>
					<td><%= Html.Encode(Model.TourCodeType != null ? Model.TourCodeType.TourCodeTypeDescription : "")%></td>
				</tr>
				<tr>
					<td>Endorsement</td>
					<td><%= Html.Encode(Model.Endorsement)%></td>
				</tr> 
				<tr>
					<td>Endorsement Override</td>
					<td><%= Html.Encode(Model.EndorsementOverride)%></td>
				</tr>
				<tr>
					<td>OSI</td>
					<td><%= Html.Encode(Model.OSIFlagNonNullable)%></td>
				</tr>
				<% if (Model.OSIFlagNonNullable) { %>
					<% foreach (var policySupplierDealCodeOSI in Model.PolicySupplierDealCodeOSIs) { %>
						<tr>
							<td>OSI <%= policySupplierDealCodeOSI.PolicySupplierDealCodeOSISequenceNumber %></td>
							<td><%= Html.Encode(policySupplierDealCodeOSI.PolicySupplierDealCodeOSIDescription)%></td>
						</tr>
					<% } %>
				<% } %>
			<% } %>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="70%" class="row_footer_right"></td>
            </tr>
            <tr>
            <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
            <td class="row_footer_blank_right"></td>
        </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
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
<%=Html.RouteLink(Model.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId}, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Supplier Deal Codes", "Default", new { controller = "PolicySupplierDealCode", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Supplier Deal Codes" })%>
</asp:Content>