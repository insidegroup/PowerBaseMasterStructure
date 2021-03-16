<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicySupplierServiceInformation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Supplier Service Information</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="2">Delete Policy Supplier Service Information</th> 
		        </tr> 
                <tr>
                    <td>Policy Group Name</td>
                    <td><%= Html.Encode(Model.PolicyGroupName) %></td>
                </tr>
                <tr>
                    <td>Value</td>
                    <td><%= Html.Encode(Model.PolicySupplierServiceInformationValue)%></td>
                </tr>
                <tr>
                    <td>Type</td>
                    <td><%= Html.Encode(Model.PolicySupplierServiceInformationTypeDescription)%></td>
                </tr>
                 <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.EnabledFlagNonNullable)%></td>
                </tr>  
                <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                </tr> 
                <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.SupplierName)%></td>
                </tr> 
                <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.ProductName)%></td>
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
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId}, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Supplier Service Information", "Default", new { controller = "PolicySupplierServiceInformation", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Supplier Service Information" })%>
</asp:Content>