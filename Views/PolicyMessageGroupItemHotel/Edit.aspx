<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemHotelVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyMessageGroupItemHotel.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Message Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%" border="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Policy Message Group Item - Hotel</th> 
		        </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemHotel_PolicyMessageGroupItemName">Policy Message Name</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemHotel.PolicyMessageGroupItemName, new { @maxlength = "50"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.PolicyMessageGroupItemName)%></td>
                </tr>
                <tr>
                    <td> <label for="PolicyMessageGroupItemHotel_PolicyLocationId">Policy Location</label></td>
                    <td><%= Html.DropDownList("PolicyMessageGroupItemHotel.PolicyLocationId", Model.PolicyLocations, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.PolicyLocationId)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemHotel_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyMessageGroupItemHotel.SupplierName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.SupplierName)%>
                        <%= Html.HiddenFor(model => model.PolicyMessageGroupItemHotel.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
                <tr>
                    <td><label for="PolicyMessageGroupItemHotel_EnabledFlag">Enabled</label></td>  
                    <td><%= Html.CheckBoxFor(model => model.PolicyMessageGroupItemHotel.EnabledFlag)%>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.EnabledFlag)%></td>
                </tr>  
                <tr>
                     <td><label for="PolicyMessageGroupItemHotel_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemHotel.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.EnabledDate)%></td>
                </tr> 
                <tr>
                     <td><label for="PolicyMessageGroupItemHotel_ExpiryDate">Expiry Date</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyMessageGroupItemHotel.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemHotel_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemHotel.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                     <td><label for="PolicyMessageGroupItemHotel_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyMessageGroupItemHotel.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemHotel.TravelDateValidTo)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Policy Message Group Item" class="red" title="Edit Policy Message Group Item"/></td>
                </tr>
            </table>
           
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemHotel.PolicyMessageGroupItemId)%>
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemHotel.PolicyGroupId)%>
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemHotel.ProductId)%>
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemHotel.VersionNumber)%>
    <% } %>

   </div>
    </div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Message Group Items", "Default", new { controller = "PolicyMessageGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Message Group Items" })%>
</asp:Content>