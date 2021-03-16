<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyCarTypeGroupItem>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyCarTypeGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Car Type Group Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Policy Car Type Group Item</th> 
		        </tr> 
                <tr>
                    <td>Policy Group Name</td>
                    <td colspan="2"><strong><%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80))%></strong></td>
                </tr>
                <tr>
                    <td> <label for="PolicyLocationId">Policy Location</label></td>
                    <td><%= Html.DropDownList("PolicyLocationId", ViewData["PolicyLocationList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyLocationId)%></td>
                </tr> 
               <tr>
                    <td> <label for="PolicyCarStatusId">Policy Car Status</label></td>
                    <td><%= Html.DropDownList("PolicyCarStatusId", ViewData["PolicyCarStatusList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCarStatusId)%></td>
                </tr> 
              
                <tr>
                    <td><label for="CarTypeCategoryId">Car Type Category</label></td>
                    <td><%= Html.DropDownList("CarTypeCategoryId", ViewData["CarTypeCategoryList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CarTypeCategoryId)%></td>
                </tr> 
                 <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBox("EnabledFlag", true)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlag)%></td>
                </tr> 
                <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.TextBoxFor(model => model.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="TravelDateValidFrom">Travel Date Valid From</label> </td>
                    <td><%= Html.TextBox("TravelDateValidFrom", "")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelDateValidFrom)%></td>
                </tr> 
                  <tr>
                    <td><label for="TravelDateValidTo">Travel Date Valid To</label></td>
                    <td><%= Html.TextBox("TravelDateValidTo", "")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelDateValidTo)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", new { id = Model.PolicyGroupId }, new { @class = "red" })%></td>
                    <td></td> 
                    <td class="row_footer_blank_right"><input type="submit" value="Create Policy CarType Group Item" class="red" title="Create PolicyCarTypeGroupItem"/></td>
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
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
Policy Car Type Group Items
</asp:Content>
