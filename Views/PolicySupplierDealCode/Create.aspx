<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicySupplierDealCode>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicySupplierDealCode.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Supplier Deal Codes</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm("Create", null, FormMethod.Post, new { id = "form0" })){%>
			<%= Html.AntiForgeryToken() %>
			<%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Policy Supplier Deal Code</th> 
		        </tr> 
                 <tr>
                    <td>Policy Group Name</td>
                    <td colspan="2"><strong><%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80))%></strong></td>
                </tr>
                <tr>
                    <td><label for="PolicySupplierDealCodeValue">Deal Code Value</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicySupplierDealCodeValue)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierDealCodeValue)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicySupplierDealCodeDescription">Deal Code Description</label></td>
                    <td><%= Html.TextAreaFor(model => model.PolicySupplierDealCodeDescription, new { maxlength = 100, cols = 30, rows = 4 } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierDealCodeDescription)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicySupplierDealCodeTypeId">Deal Code Type</label></td>
                    <td><%= Html.DropDownList("PolicySupplierDealCodeTypeId", ViewData["PolicySupplierDealCodeTypeList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierDealCodeTypeId)%></td>
                </tr> 
                <tr>
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.DropDownList("GDSCode", ViewData["GDSList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSCode)%></td>
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
                    <td><label for="PolicyLocationId">Policy Location</label></td>
                    <td><%= Html.DropDownList("PolicyLocationId", ViewData["PolicyLocationList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyLocationId)%></td>
                </tr> 
				<tr class="tourcode-row">
                    <td><label for="TravelIndicator">Travel Indicator</label></td>
                    <td><%= Html.DropDownList("TravelIndicator", ViewData["TravelIndicatorList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelIndicator)%></td>
                </tr>
				<tr class="tourcode-row">
                    <td><label for="TourCodeTypeId">Tour Code Type</label></td>
                    <td><%= Html.DropDownList("TourCodeTypeId", ViewData["TourCodeTypeList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TourCodeTypeId)%></td>
                </tr>
				<tr class="tourcode-row">
                    <td><label for="Endorsement">Endorsement</label></td>
                    <td><%= Html.TextAreaFor(model => model.Endorsement)%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.Endorsement)%>
						<span id="countEndorsement"></span> characters remaining
                    </td>
                </tr> 
				<tr class="tourcode-row">
                    <td><label for="EndorsementOverride">Endorsement Override</label></td>
                    <td><%= Html.TextAreaFor(model => model.EndorsementOverride)%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.EndorsementOverride)%>
						<span id="countEndorsementOverride"></span> characters remaining
                    </td>
                </tr>
				<tr class="tourcode-row">
                    <td><label for="OSIFlag">OSI</label></td>
                    <td><%= Html.CheckBoxFor(model => model.OSIFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OSIFlag)%></td>
                </tr>
				<tr class="PolicySupplierDealCodeOSILine" valign="top">
                    <td><label id="PolicySupplierDealCodeOSILabel_1"/>OSI 1</td>
                    <td>
						<%= Html.TextArea("PolicySupplierDealCodeOSI_1", null, new { @class = "osi-input" }) %>
						<span class="error"> *</span> 
						<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/delete.png" alt="Remove" /></a> 
						<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a>
                    </td>
                    <td><span id="PolicySupplierDealCodeOSI_Count_1" class="character_count"></span> characters remaining</td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Policy Supplier Deal Code" class="red" title="Create Policy Supplier Deal Code"/></td>
                </tr>
            </table>
           
             <%=Html.HiddenFor(model => model.PolicyGroupId)%>
    <% } %>

   </div>
    </div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId}, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Supplier Deal Codes", "Default", new { controller = "PolicySupplierDealCode", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Supplier Deal Codes" })%>
</asp:Content>