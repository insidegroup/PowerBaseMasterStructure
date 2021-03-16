<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyHotelVendorGroupItemLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Hotel Vendor Advice</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
        
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Edit Hotel Vendor Advice</th> 
		    </tr>  
            <tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.LanguageName)%></td>
                <td></td>
            </tr> 
            <tr>
                <td valign="top"><label for="HotelAdvice">Hotel Vendor Advice</label></td>
                <td valign="top"> <%= Html.TextAreaFor(model => model.HotelAdvice, new { maxlength = "255", style="width:320px;height:180px;" })%><span class="error" style="vertical-align:top"> *</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.HotelAdvice)%> </td>
            </tr> 
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>            
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Edit Hotel Vendor Advice" title="Edit Hotel Vendor Advice" class="red"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.LanguageCode) %>
        <%= Html.HiddenFor(model => model.PolicyHotelVendorGroupItemId) %>
        <%= Html.HiddenFor(model => model.VersionNumber) %>
    <% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_policies').click();
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
<%=Html.RouteLink("Item", "Default", new { controller = "PolicyHotelVendorGroupItem", action = "View", id = Model.PolicyHotelVendorGroupItemId }, new { title = "Item" })%> &gt;
<%=Html.RouteLink("Hotel Vendor Advice", "Default", new { controller = "HotelVendorAdvice", action = "List", id = Model.PolicyHotelVendorGroupItemId }, new { title = "Hotel Vendor Advice" })%> 
</asp:Content>