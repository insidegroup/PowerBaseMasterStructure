<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemCarVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyMessageGroupItemCar.js")%>" type="text/javascript"></script>
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
			        <th class="row_header" colspan="3">Edit Policy Message Group Item - Car</th> 
		        </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemCar_PolicyMessageGroupItemName">Policy Message Name</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemCar.PolicyMessageGroupItemName, new { @maxlength = "50"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.PolicyMessageGroupItemName)%></td>
                </tr>
                <tr>
                    <td> <label for="PolicyMessageGroupItemCar_PolicyLocationId">Policy Location</label></td>
                    <td><%= Html.DropDownList("PolicyMessageGroupItemCar.PolicyLocationId", Model.PolicyLocations, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.PolicyLocationId)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemCar_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyMessageGroupItemCar.SupplierName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.SupplierName)%>
                        <%= Html.HiddenFor(model => model.PolicyMessageGroupItemCar.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
                <tr>
                    <td><label for="PolicyMessageGroupItemCar_EnabledFlag">Enabled</label></td>  
                    <td><%= Html.CheckBoxFor(model => model.PolicyMessageGroupItemCar.EnabledFlag)%>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.EnabledFlag)%></td>
                </tr>  
                <tr>
                     <td><label for="PolicyMessageGroupItemCar_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemCar.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.EnabledDate)%></td>
                </tr> 
                <tr>
                     <td><label for="PolicyMessageGroupItemCar_ExpiryDate">Expiry Date</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyMessageGroupItemCar.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemCar_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemCar.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                     <td><label for="PolicyMessageGroupItemCar_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyMessageGroupItemCar.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemCar.TravelDateValidTo)%></td>
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
           
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemCar.PolicyMessageGroupItemId)%>
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemCar.PolicyGroupId)%>
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemCar.ProductId)%>
             <%=Html.HiddenFor(model => model.PolicyMessageGroupItemCar.VersionNumber)%>
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