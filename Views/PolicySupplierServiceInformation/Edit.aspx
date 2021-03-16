<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicySupplierServiceInformation>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/policySupplierServiceInformation.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Supplier Service Information</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Policy Supplier Service Information</th> 
		        </tr> 
                <tr>
                    <td>Policy Group Name</td>
                    <td colspan="2"><strong><%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80))%></strong></td>
                </tr>
                 <tr>
                    <td><label for="PolicySupplierServiceInformationValue">Value</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicySupplierServiceInformationValue)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierServiceInformationValue)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicySupplierServiceInformationTypeId">Type</label></td>
                    <td><%= Html.DropDownList("PolicySupplierServiceInformationTypeId", ViewData["PolicySupplierServiceInformationList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierServiceInformationTypeId)%></td>
                </tr> 
                 <tr>
                    <td><label for="EnabledFlagNonNullable">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.EnabledFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlagNonNullable)%></td>
                </tr> 
                <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label></td>
                    <td> <%= Html.EditorFor(model => model.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate)%></td>
                </tr> 
                 <tr>
                    <td><label for="ProductId">Product</label></td>
                    <td><%= Html.DropDownList("ProductId", ViewData["ProductList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
                </tr> 
                 <tr>
                    <td><label for="SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.SupplierName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.SupplierName)%>
                        <%= Html.HiddenFor(model => model.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
             <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Policy Supplier Service Information" class="red" title="Edit Policy Supplier Service Information"/></td>
                </tr>
            </table>
           
             <%=Html.HiddenFor(model => model.PolicyGroupId)%>
             <%=Html.HiddenFor(model => model.VersionNumber)%>
    <% } %>

   </div>
</div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId}, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Supplier Service Information", "Default", new { controller = "PolicySupplierServiceInformation", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Supplier Service Information" })%>
</asp:Content>