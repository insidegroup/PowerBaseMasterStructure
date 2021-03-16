<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyAirCabinGroupItemLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Air Cabin Advice</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Air Cabin Advice</th> 
		    </tr> 
            <tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.LanguageName)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Air Cabin Advice</td>
                <td><%= Html.Encode(Model.AirCabinAdvice)%></td>
                <td></td>
            </tr>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right"></td>
            </tr>             
        </table>
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
<%=Html.RouteLink(Model.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Cabin Group Items", "Default", new { controller = "PolicyAirCabinGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Air Cabin Group Items" })%> &gt;
<%=Html.RouteLink("Item", "Default", new { controller = "PolicyAirCabinGroupItem", action = "View", id = ViewData["PolicyAirCabinGroupItemId"] }, new { title = "Item" })%> &gt;
<%=Html.RouteLink("Air Cabin Advice", "Default", new { controller = "AirCabinAdvice", action = "List", id = Model.PolicyAirCabinGroupItemId }, new { title = "Air Cabin Advice" })%>
</asp:Content>

