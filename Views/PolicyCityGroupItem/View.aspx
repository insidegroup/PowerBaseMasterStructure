<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyCityGroupItemVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
     <div id="banner"><div id="banner_text">Policy City Group Item</div></div>

        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="2">View Policy City Group Item</th> 
		        </tr> 
                <tr>
                    <td>City</td>
                    <td><%= Html.Encode(Model.PolicyCityGroupItem.City.Name)%></td>
                </tr>
                <tr>
                    <td>City Status</td>
                    <td><%= Html.Encode(Model.PolicyCityGroupItem.PolicyCityStatus.PolicyCityStatusDescription)%></td>
                </tr>
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.PolicyCityGroupItem.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td>Enabled Date</td>
                    <td><%if (Model.PolicyCityGroupItem.EnabledDate.HasValue)
                          { %><%= Html.Encode(Model.PolicyCityGroupItem.EnabledDate.Value.ToString("MMM dd, yyyy"))%><%} %></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%if (Model.PolicyCityGroupItem.ExpiryDate.HasValue)
                          { %><%= Html.Encode(Model.PolicyCityGroupItem.ExpiryDate.Value.ToString("MMM dd, yyyy"))%><%} %></td>
                </tr>  
                 <tr>
                    <td>Travel Date Valid From </td>
                    <td><%if (Model.PolicyCityGroupItem.TravelDateValidFrom.HasValue)
                          { %><%= Html.Encode(Model.PolicyCityGroupItem.TravelDateValidFrom.Value.ToString("MMM dd, yyyy"))%><%} %></td>
                </tr> 
                <tr>
                    <td>Travel Date Valid To</td>
                    <td><%if (Model.PolicyCityGroupItem.TravelDateValidTo.HasValue)
                          { %><%= Html.Encode(Model.PolicyCityGroupItem.TravelDateValidTo.Value.ToString("MMM dd, yyyy"))%><%} %></td>
                </tr>  
                 <tr>
                    <td>Inherit From Parent?</td>
                    <td><%= Html.Encode(Model.PolicyCityGroupItem.InheritFromParentFlag)%></td>
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
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy City Group Items", "Default", new { action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;

</asp:Content>