<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyCarTypeGroupItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Car Type Group Item</div></div>

        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="2">View Policy Car Type Group Item</th> 
		        </tr> 
                <tr>
                    <td><label for="PolicyLocationId">Policy Location</label></td>
                    <td><%= Html.Encode(Model.PolicyLocation)%></td>
                </tr>
                <tr>
                    <td><label for="PolicyCarStatusDescription">Car Status</label></td>
                    <td><%= Html.Encode(Model.PolicyCarStatusDescription)%></td>
                </tr>
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                </tr>  
                 <tr>
                    <td><label for="CarTypeCategoryName">Car Type</label></td>
                    <td><%= Html.Encode(Model.CarTypeCategoryName)%></td>
                </tr>
                <tr>
                    <td><label for="TravelDateValidFrom">Applicable Date Valid From</label></td>
                    <td><%if (Model.TravelDateValidFrom .HasValue)
                          { %><%= Html.Encode(Model.TravelDateValidFrom.Value.ToString("MMM dd, yyyy"))%><%} %></td>
                </tr> 
                <tr>
                    <td><label for="TravelDateValidTo">Applicable Date Valid To</label></td>
                    <td><%if (Model.TravelDateValidTo.HasValue)
                          { %><%= Html.Encode(Model.TravelDateValidTo.Value.ToString("MMM dd, yyyy"))%><%} %></td>
                </tr>  

                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="70%" class="row_footer_right"></td>
                </tr>
                 <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr>
            </table>
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
<%=Html.RouteLink(Model.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Car Type Group Items", "Default", new { controller = "PolicyCarTypeGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Car Type Group Items" })%> &gt;
Item
</asp:Content>