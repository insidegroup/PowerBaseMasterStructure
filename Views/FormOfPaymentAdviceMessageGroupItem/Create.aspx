<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.FormOfPaymentAdviceMessageGroupItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - FOP Advice Message Group Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">FOP Advice Message Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>

            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create FOP Advice Message Item</th> 
		        </tr> 
				<tr>
                    <td><label for="FormOfPaymentAdviceMessageGroupItem_ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.FormOfPaymentAdviceMessageGroupItem.ProductId, Model.Products, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageGroupItem.ProductId)%></td>
                </tr>  
                <tr>
                    <td><label for="FormOfPaymentAdviceMessageGroupItem_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.FormOfPaymentAdviceMessageGroupItem.SupplierName, new { maxlength = "100", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageGroupItem.SupplierCode)%>
                        <%= Html.HiddenFor(model => model.FormOfPaymentAdviceMessageGroupItem.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr> 
				<tr>
                    <td><label for="FormOfPaymentAdviceMessageGroupItem_CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.FormOfPaymentAdviceMessageGroupItem.CountryCode, Model.Countries, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageGroupItem.CountryCode)%></td>
                </tr>  
				<tr>
                    <td><label for="FormOfPaymentAdviceMessageGroupItem_TravelIndicator">Travel Indicator</label></td>
                    <td><%= Html.DropDownListFor(model => model.FormOfPaymentAdviceMessageGroupItem.TravelIndicator, Model.TravelIndicators, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageGroupItem.TravelIndicator)%></td>
                </tr>
				<tr>
                    <td><label for="FormOfPaymentAdviceMessageGroupItem_LanguageCode">Language</label></td>
                    <td><%= Html.Raw(Model.FormOfPaymentAdviceMessageGroupItem.LanguageName) %></td>
                    <td><%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageGroupItem.LanguageCode)%></td>
                </tr>
				<tr>
                    <td><label for="FormOfPaymentAdviceMessageGroupItem_FormofPaymentTypeID">FOP Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.FormOfPaymentAdviceMessageGroupItem.FormofPaymentTypeID, Model.FormOfPaymentTypes, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageGroupItem.FormofPaymentTypeID)%></td>
                </tr>
				<tr>
                    <td><label for="FormOfPaymentAdviceMessageGroupItem_FormOfPaymentAdviceMessage">FOP Advice Message</label></td>
                    <td><%= Html.TextAreaFor(model => model.FormOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessage, new { maxlength = "255" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessage)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Confirm Create" title="Confirm Create" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.FormOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessageGroupID) %>
            <%= Html.HiddenFor(model => model.FormOfPaymentAdviceMessageGroupItem.LanguageCode) %>
		<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/FormOfPaymentAdviceMessageGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("FOP Advice Message Groups", "Main", new { controller = "FormOfPaymentAdviceMessageGroup", action = "ListUnDeleted", }, new { title = "FOP Advice Message Groups" })%> &gt;
<%=Html.RouteLink(Model.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName, "Default", new { controller = "FormOfPaymentAdviceMessageGroup", action = "View", id = Model.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID }, new { title = Model.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName })%> &gt;
Items &gt;
Create
</asp:Content>