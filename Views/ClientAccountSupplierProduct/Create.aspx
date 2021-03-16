<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientAccountSupplierProductVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Account - Client Detail Product Suppliers</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Product Supplier</th> 
		        </tr> 
                <tr>
                    <td><label for="SupplierProduct_ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.SupplierProduct.ProductId, Model.Products, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.SupplierProduct.ProductId)%></td>
                </tr>                  
                <tr>
                    <td><label for="SupplierProduct_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.SupplierProduct.SupplierName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.SupplierProduct.SupplierName)%>
                        <%= Html.HiddenFor(model => model.SupplierProduct.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Product Supplier" class="red" title="Create Product Supplier"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientDetail.ClientDetailId) %>
    <% } %>


    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/SupplierProduct.js")%>" type="text/javascript"></script>
</asp:Content>


