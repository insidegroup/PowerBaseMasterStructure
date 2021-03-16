<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupMerchantFeeVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Merchant Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Mid Office Merchant Fee Items</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Mid Office Merchant Fee Item</th> 
		    </tr> 
            <tr>
                <td>Group Name</td>
                <td><%= Html.Encode(CWTStringHelpers.TrimString(Model.ClientFeeGroup.ClientFeeGroupName, 57))%></td>
                <td></td>
            </tr>
              <tr>
                <td>Fee Description</td>
                <td><%= Html.Encode(CWTStringHelpers.TrimString(Model.MerchantFee.MerchantFeeDescription, 57))%></td>
                <td></td>
            </tr>
            <tr>
                <td>Country</td>
                <td><%= Html.Encode(Model.MerchantFee.Country != null ? Model.MerchantFee.Country.CountryName : "")%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Credit Card Vendor</td>
                <td><%= Html.Encode(Model.MerchantFee.CreditCardVendor != null ? Model.MerchantFee.CreditCardVendor.CreditCardVendorName : "")%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Merchant Fee Percent</td>
                <td><%= Html.Encode(Model.MerchantFee.MerchantFeePercent)%></td>
                <td></td>
            </tr>   
            <tr>
                <td>Product</td>
                <td><%= Html.Encode(Model.MerchantFee.ProductName)%></td>
                <td></td>
            </tr>     
             <tr>
                <td>Supplier</td>
                <td><%= Html.Encode(Model.MerchantFee.SupplierName)%></td>
                <td></td>
            </tr>           
              <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                <td class="row_footer_blank_right">
                <% using (Html.BeginForm()) { %>
                    <%= Html.AntiForgeryToken() %>
                    <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                    <%= Html.HiddenFor(model => model.MerchantFeeClientFeeGroup.VersionNumber)%>
                    <%= Html.HiddenFor(model => model.MerchantFeeClientFeeGroup.ClientFeeGroupId)%>
                    <%= Html.HiddenFor(model => model.MerchantFeeClientFeeGroup.MerchantFeeId)%>
                <%}%>
                </td>  
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink(Model.FeeTypeDisplayName + "s", "Main", new { controller = "ClientFeeGroup", action = "ListUnDeleted", ft = Model.FeeTypeId }, new { title = "Client Fee Groups" })%> &gt;
<%=Html.RouteLink(Model.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeGroup.ClientFeeGroupName })%> &gt;
<%=Html.RouteLink("Mid Office Merchant Fee Items", "Main", new { controller = "MerchantFeeClientFeeGroup", action = "List", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = "Mid Office Merchant Fee Items" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.MerchantFee.MerchantFeeDescription,37)) %>
</asp:Content>

