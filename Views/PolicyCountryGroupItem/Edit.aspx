<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyCountryGroupItem>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyCountryGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Country Group Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Policy Country Group Item</th> 
		        </tr> 
                 <tr>
                    <td>Policy Group Name</td>
                    <td colspan="2"><strong><%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80))%></strong></td>
                </tr>
                 <tr>
                    <td><label for="CountryCode">Country</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CountryName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.CountryCode)%>
                        <%= Html.Hidden("CountryCode")%>
                        <label id="lblCountryNameMsg"/>
                    </td>
                </tr> 
               <tr>
                    <td><label for="PolicyCountryStatusId">Country Status</label></td>
                    <td><%= Html.DropDownList("PolicyCountryStatusId", ViewData["PolicyCountryStatusList"] as SelectList, "None")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCountryStatusId)%></td>
                </tr> 
                <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate)%></td>
                </tr>  
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label></td>
                    <td><%= Html.EditorFor(model => model.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelDateValidFrom)%></td>
                </tr>  
                <tr>
                    <td><label for="TravelDateValidTo">Travel Date Valid To</label></td>
                    <td><%= Html.EditorFor(model => model.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelDateValidTo)%></td>
                </tr> 
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td><label for="InheritFromParentFlag">Inherit From Parent?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.InheritFromParentFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.InheritFromParentFlag)%></td>
                </tr>
             <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Policy Country Group Item" class="red" title="Edit Policy Country Group Item"/></td>
                </tr>
            </table>
             <%= Html.HiddenFor(model => model.VersionNumber) %>
             <%=Html.HiddenFor(model => model.PolicyGroupId)%>
    <% } %>

   </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/PolicyCountryGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Country Group Items", "Default", new { controller = "PolicyCountryGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Country Group Items" })%> &gt;
Item
</asp:Content>