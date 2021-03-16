<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyAirCabinGroupItemLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Air Cabin Advice</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
        
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Create Air Cabin Advice</th> 
		    </tr>  
            <tr>
                <td><label for="LanguageCode">Language</label></td>
                <td><%= Html.DropDownList("LanguageCode", ViewData["Languages"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
            </tr> 
            <tr>
                <td valign="top"><label for="AirCabinAdvice">Air Cabin Advice</label></td>
                <td valign="top"> <%= Html.TextAreaFor(model => model.AirCabinAdvice, new { maxlength = "255", style="width:320px;height:180px;" })%><span class="error" style="vertical-align:top"> *</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.AirCabinAdvice)%> </td>
            </tr> 
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Create Air Cabin Advice" title="Create Air Cabin Advice" class="red"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.PolicyAirCabinGroupItemId) %>
    <% } %>
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
<%=Html.RouteLink(ViewData["PolicyGroupName"].ToString(), "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = ViewData["PolicyGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Cabin Group Items", "Default", new { controller = "PolicyAirCabinGroupItem", action = "List", id = ViewData["PolicyGroupId"] }, new { title = "Policy Air Cabin Group Items" })%> &gt;
<%=Html.RouteLink("Item", "Default", new { controller = "PolicyAirCabinGroupItem", action = "View", id = ViewData["PolicyAirCabinGroupItemId"] }, new { title = "Item" })%> &gt;
<%=Html.RouteLink("Air Cabin Advice", "Default", new { controller = "AirCabinAdvice", action = "List", id = Model.PolicyAirCabinGroupItemId }, new { title = "Air Cabin Advice" })%>
</asp:Content>

