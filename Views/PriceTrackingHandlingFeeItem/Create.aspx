<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PriceTrackingHandlingFeeItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Item
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="contentarea">
         <div id="banner"><div id="banner_text">Price Tracking Item</div></div>
            <div id="content">
            <% Html.EnableClientValidation(); %>
		    <% Html.EnableUnobtrusiveJavaScript(); %>
            <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
                <table cellpadding="0" cellspacing="0" width="100%"> 
		            <tr> 
			            <th class="row_header" colspan="3">Create Price Tracking Item</th> 
		            </tr> 
                    <tr>
                        <td><label for="PriceTrackingSystem">Price Tracking System</label></td>
                        <td><%= Html.DropDownListFor(model => model.PriceTrackingHandlingFeeItem.PriceTrackingSystemId, ViewData["PriceTrackingSystemList"] as SelectList, "None", new { autocomplete="off" })%><span class="error"> *</span></td>
                        <td><%= Html.ValidationMessageFor(model => model.PriceTrackingHandlingFeeItem.PriceTrackingSystemId)%></td>
                    </tr> 
                    <tr>
                        <td><label for="Product">Product</label></td>
                        <td><%= Html.DropDownListFor(model => model.PriceTrackingHandlingFeeItem.ProductId, ViewData["ProductList"] as SelectList, "None", new { autocomplete="off" })%><span class="error"> *</span></td>
                        <td><%= Html.ValidationMessageFor(model => model.PriceTrackingHandlingFeeItem.ProductId)%></td>
                    </tr> 
				    <tr>
				        <td><label for="PriceTrackingHandlingFeeItem_SavingAmountPercentage">Saving Amount Percentage</label></td>
				        <td><%= Html.EditorFor(model => model.PriceTrackingHandlingFeeItem.SavingAmountPercentage, "numeric5.2", new { maxlength = "6" })%></td>
				        <td>
                            <%= Html.ValidationMessageFor(model => model.PriceTrackingHandlingFeeItem.SavingAmountPercentage)%>
                            <span id="PriceTrackingHandlingFeeItem_HandlingFee_Error" class="error">Please enter either a Saving Amount Percentage or Handling Fee</span>
				        </td>
			        </tr> 
			        <tr>
				        <td><label for="PriceTrackingHandlingFeeItem_HandlingFee">Handling Fee</label></td>
				        <td><%= Html.EditorFor(model => model.PriceTrackingHandlingFeeItem.HandlingFee, "numeric7.2", new { maxlength = "8" })%></td>
				        <td><%= Html.ValidationMessageFor(model => model.PriceTrackingHandlingFeeItem.HandlingFee)%></td>
			        </tr>
				    <tr>
                        <td width="30%" class="row_footer_left"></td>
                        <td width="40%" class="row_footer_centre"></td>
                        <td width="30%" class="row_footer_right"></td>
                    </tr>
                   <tr>
                        <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                        <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Price Tracking Item" title="Create Price Tracking Item" class="red"/></td>
                    </tr>
                </table>
               <%=Html.HiddenFor(model => model.PriceTrackingHandlingFeeItem.PriceTrackingHandlingFeeGroupId)%>

        <% } %>

       </div>
    </div>
    <script src="<%=Url.Content("~/Scripts/ERD/PriceTrackingHandlingFeeItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Groups", "Main", new { controller = "PriceTrackingHandlingFeeGroup", action = "ListUnDeleted", id = ViewData["PriceTrackingHandlingFeeGroupId"] }, new { title = "Price Tracking Groups" })%> &gt;
<%=ViewData["PriceTrackingHandlingFeeGroupName"]%> &gt;
Create
</asp:Content>