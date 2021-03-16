<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MerchantFeeVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Merchant Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Mid Office Merchant Fees</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Mid Office Merchant Fee</th> 
		    </tr> 
            <tr>
                <td>Fee Description</td>
                 <td><%=CWTStringHelpers.WrapString(Model.MerchantFee.MerchantFeeDescription)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Country</td>
                <td><%= Html.Encode(Model.MerchantFee.CountryName)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Credit Card Vendor</td>
                <td><%= Html.Encode(Model.MerchantFee.CreditCardVendorName)%></td>
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
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.MerchantFee.EnabledDate.HasValue ? Model.MerchantFee.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.MerchantFee.ExpiryDate.HasValue ? Model.MerchantFee.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
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
                    <%= Html.HiddenFor(model => model.MerchantFee.VersionNumber)%>
                    <%= Html.HiddenFor(model => model.MerchantFee.MerchantFeeId)%>
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
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Mid Office Merchant Fees", "Main", new { controller = "MerchantFee", action = "List", }, new { title = "Mid Office Merchant Fees" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.MerchantFee.MerchantFeeDescription,37)) %>
</asp:Content>

