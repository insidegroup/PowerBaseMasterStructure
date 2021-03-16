<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemHotelVM>" %>
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
			    <th class="row_header" colspan="3">Delete Policy Message Group Item - Hotel</th> 
		    </tr> 
            <tr>
                <td>Policy Message Name</td>
                <td><%=Model.PolicyMessageGroupItemHotel.PolicyMessageGroupItemName%></td>
                <td></td>
            </tr>
            <tr>
                <td>Policy Location</td>
                <td><%=Model.PolicyMessageGroupItemHotel.PolicyLocationName%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Supplier</td>
                <td><%=Model.PolicyMessageGroupItemHotel.SupplierName%></td>
                <td></td>
            </tr>
            <tr>
                <td>Enabled</td>  
                <td><%=Model.PolicyMessageGroupItemHotel.EnabledFlag%></td>
                <td></td>
            </tr>  
           <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemHotel.EnabledDate.HasValue ? Model.PolicyMessageGroupItemHotel.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemHotel.ExpiryDate.HasValue ? Model.PolicyMessageGroupItemHotel.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Travel Date Valid From</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemHotel.TravelDateValidFrom.HasValue ? Model.PolicyMessageGroupItemHotel.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
               <td>Travel Date Valid To</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemHotel.TravelDateValidTo.HasValue ? Model.PolicyMessageGroupItemHotel.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
                <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.PolicyMessageGroupItemHotel.VersionNumber)%>
                    <%}%>
                    </td>                
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