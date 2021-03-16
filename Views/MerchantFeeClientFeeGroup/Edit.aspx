<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupMerchantFeeVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Merchant Fee Items</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Mid Office Merchant Fee Item</th> 
		        </tr>  
                <tr>
                    <td>Group Name</td>
                    <td> <%= Model.ClientFeeGroup.ClientFeeGroupName%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="MerchantFeeClientFeeGroup_MerchantFeeId">Fee Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.MerchantFeeClientFeeGroup.MerchantFeeId, Model.MerchantFees, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFeeClientFeeGroup.MerchantFeeId)%></td>
                </tr>  
                 <tr>
                    <td>Country</td>
                    <td><span id="CountryName"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Credit Card Vendor</td>
                    <td><span id="CreditCardVendor"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Merchant Fee Percent</td>
                    <td><span id="MerchantFeePercent"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Product</td>
                    <td><span id="ProductName"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Supplier</td>
                    <td><span id="SupplierName"></span></td>
                    <td></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Mid Office Merchant Fee Item" title="Edit Mid Office Merchant Fee Item" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.MerchantFeeClientFeeGroup.ClientFeeGroupId)%>
            <%= Html.HiddenFor(model => model.MerchantFeeClientFeeGroup.VersionNumber)%>
            <%= Html.HiddenFor(model => model.OriginalMerchantFeeId)%>
    <% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/MerchantFeeClientFeeGroup.js")%>" type="text/javascript"></script>
 </asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink(Model.FeeTypeDisplayName + "s", "Main", new { controller = "ClientFeeGroup", action = "ListUnDeleted", ft = Model.FeeTypeId }, new { title = "Client Fee Groups" })%> &gt;
<%=Html.RouteLink(Model.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeGroup.ClientFeeGroupName })%> &gt;
<%=Html.RouteLink("Mid Office Merchant Fee Items", "Main", new { controller = "MerchantFeeClientFeeGroup", action = "List", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = "Mid Office Merchant Fee Items" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.MerchantFee.MerchantFeeDescription,37)) %>
</asp:Content>

