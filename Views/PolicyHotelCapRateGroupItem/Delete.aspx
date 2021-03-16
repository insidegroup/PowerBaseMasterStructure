<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyHotelCapRateGroupItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Hotel Cap Rate Group Item</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="2">Delete Policy Hotel Cap Rate Group Item</th> 
		        </tr> 
                <tr>
                   <td><label for="PolicyGroupName">Policy Group Name</label></td>
                    <td><%= Html.Encode(Model.PolicyGroupName) %></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.PolicyLocationId)%></td>
                    <td><%= Html.Encode(Model.PolicyLocation)%></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.CurrencyName)%></td>
                    <td><%= Html.Encode(Model.CurrencyName)%></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.CapRate)%></td>
                    <td><%= Html.Encode(Model.CapRate)%></td>
                </tr>  
                 <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%><//td>
                </tr>
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label></td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%><//td>
                </tr> 
                <tr>
                     <td><label for="TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.Encode(Model.TravelDateValidFrom.HasValue ? Model.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No TravelDateValidFrom Date")%><//td>
                </tr> 
                <tr>
                     <td><label for="TravelDateValidTo">Travel Date Valid To</label></td>
                    <td><%= Html.Encode(Model.TravelDateValidTo.HasValue ? Model.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No TravelDateValidTo Date")%><//td>
                </tr> 
                <tr>
                    <td><label for="TaxInclusiveFlag">Tax Inclusive?</label></td>
                    <td><%= Html.Encode(Model.TaxInclusiveFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="70%" class="row_footer_right" align="right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                     <td class="row_footer_blank_right"> 
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                         <%= Html.HiddenFor(model => model.VersionNumber)%>
                    <%}%></td>
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
<%=Html.RouteLink("Policy Hotel Cap Rate Group Items", "Default", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Hotel Cap Rate Group Items" })%> &gt;
Item
</asp:Content>
