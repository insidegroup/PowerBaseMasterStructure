<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemAirVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Message Item</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%" border="0"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Policy Message Group Item - Air</th> 
		    </tr> 
            <tr>
                <td>Policy Message Name</td>
                <td><%=Model.PolicyMessageGroupItemAir.PolicyMessageGroupItemName%></td>
                <td></td>
            </tr>
              <tr>
                <td>Policy Routing</td>
                <td><%=Model.PolicyRouting.Name%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Supplier</td>
                <td><%=Model.PolicyMessageGroupItemAir.SupplierName%></td>
                <td></td>
            </tr>
            <tr>
                <td>Enabled</td>  
                <td><%=Model.PolicyMessageGroupItemAir.EnabledFlag%></td>
                <td></td>
            </tr>  
           <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemAir.EnabledDate.HasValue ? Model.PolicyMessageGroupItemAir.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemAir.ExpiryDate.HasValue ? Model.PolicyMessageGroupItemAir.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Travel Date Valid From</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemAir.TravelDateValidFrom.HasValue ? Model.PolicyMessageGroupItemAir.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No Travel Date Valid From")%></td>
                <td></td>
            </tr> 
            <tr>
               <td>Travel Date Valid To</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemAir.TravelDateValidTo.HasValue ? Model.PolicyMessageGroupItemAir.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No Travel Date Valid To")%></td>
                <td></td>
            </tr> 
              <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                 <tr>
                    <td class="row_footer_blank" colspan="3"></td>
                </tr>
                <tr> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
                <tr>
                    <td>Routing Name</td>
                    <td><%= Html.Encode(Model.PolicyRouting.Name)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>From Global?</td>
                    <td><%= Html.Encode(Model.PolicyRouting.FromGlobalFlag)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>From</td>
                    <td><%= Html.Encode(Model.PolicyRouting.FromName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>To Global?</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToGlobalFlag)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>To</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Routing Vice Versa?</td>
                    <td><%= Html.Encode(Model.PolicyRouting.RoutingViceVersaFlag)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
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
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Message Group Items", "Default", new { controller = "PolicyMessageGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Message Group Items" })%>
</asp:Content>