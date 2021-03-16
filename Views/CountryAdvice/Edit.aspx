<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyCountryGroupItemLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Country Advice</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Country Advice</th> 
		        </tr>  
                <tr>
                    <td><label for="LanguageName">Language</label></td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                </tr> 
            <tr>
                <td valign="top"><label for="CountryAdvice">Country Advice</label></td>
                <td valign="top"> <%= Html.TextAreaFor(model => model.CountryAdvice, new { maxlength = "255", style="width:320px;height:180px;" })%><span class="error" style="vertical-align:top"> *</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.CountryAdvice)%> </td>
            </tr> 
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>            
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Country Advice" title="Edit Country Advice" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.LanguageCode) %>
<%= Html.HiddenFor(model => model.PolicyCountryGroupItemId) %>
<%= Html.HiddenFor(model => model.VersionNumber) %>
    <% } %>
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
<%=Html.RouteLink(Model.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Country Group Items", "Default", new { controller = "PolicyCountryGroupItem", action = "List", id = ViewData["PolicyGroupId"] }, new { title = "Policy Country Group Items" })%> &gt;
<%=Html.RouteLink("Item", "Default", new { controller = "PolicyCountryGroupItem", action = "View", id = ViewData["PolicyCountryGroupItemId"] }, new { title = "Item" })%> &gt;
<%=Html.RouteLink("Country Advice", "Default", new { controller = "CountryAdvice", action = "List", id = Model.PolicyCountryGroupItemId }, new { title = "Country Advice" })%>
</asp:Content>