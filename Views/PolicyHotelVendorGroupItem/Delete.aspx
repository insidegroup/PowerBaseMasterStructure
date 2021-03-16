<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyHotelVendorGroupItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Hotel Vendor Group Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="2">Delete Policy Hotel Vendor Group Item</th> 
		        </tr> 
                <tr>
                    <td><%= Html.LabelFor(model => model.PolicyGroupId)%></td>
                    <td><%= Html.Encode(Model.PolicyGroupName) %></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.PolicyLocationId)%></td>
                    <td><%= Html.Encode(Model.PolicyLocationId)%></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.PolicyHotelStatusId)%></td>
                    <td><%= Html.Encode(Model.PolicyHotelStatus)%></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.EnabledFlag)%></td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td><%= Html.LabelFor(model => model.EnabledDate) %></td>
                    <td><%= Html.Encode(String.Format("{0:d}", Model.EnabledDate)) %></td>
                </tr> 
                <tr>
                    <td><%= Html.LabelFor(model => model.ExpiryDate)%> </td>
                    <td><%= Html.Encode(String.Format("{0:d}", Model.ExpiryDate))%></td>
                </tr> 
                 <tr>
                    <td><%= Html.LabelFor(model => model.TravelDateValidFrom)%></td>
                    <td><%= Html.Encode(String.Format("{0:d}", Model.TravelDateValidFrom))%></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.TravelDateValidTo)%> </td>
                    <td><%= Html.Encode(String.Format("{0:d}", Model.TravelDateValidTo))%></td>
                </tr> 
                 <tr>
                    <td><%= Html.LabelFor(model => model.SupplierCode)%> </td>
                    <td> <%= Html.Encode(Model.SupplierName)%></td>
                </tr>
                 <tr>
                    <td><%= Html.LabelFor(model => model.ProductId)%></td>
                    <td> <%= Html.Encode(Model.ProductName)%></td>
                </tr>
                <%if (!ViewData.ModelState.IsValid) { %>
                <%if (ViewData.ModelState["Exception"].Errors.Count > 0 ){ %>
                <tr>
                    <td></td>
                    <td><span class="error"><% =ViewData.ModelState["Exception"].Errors[0].ErrorMessage %></span></td>
                </tr> 
                <% } %>
                <% } %>

             <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="70%" class="row_footer_right" align="right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", new { id = Model.PolicyGroupId }, new { @class = "red" })%></td>
                    <td class="row_footer_blank_right"> 
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber)%>
                    <%}%></td>
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
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Hotel Vendor Group Items", "Default", new { controller = "PolicyHotelVendorGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Hotel Vendor Group Items" })%> &gt;
Item
</asp:Content>