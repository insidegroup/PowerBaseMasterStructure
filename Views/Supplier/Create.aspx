<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.Supplier>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Supplier</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Supplier</th> 
		        </tr>  
                <tr>
					<td><label for="SupplierCode">Supplier Code</label></td>
					<td><%= Html.TextBoxFor(model => model.SupplierCode, new { maxlength = "50" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.SupplierCode)%>
						<label id="lblSupplierCodeMsg"/>
					</td>
				</tr>   
				<tr>
					<td><label for="SupplierName">Supplier Name</label></td>
					<td><%= Html.TextBoxFor(model => model.SupplierName, new { maxlength = "100" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.SupplierName)%>
						<label id="lblSupplierNameMsg"/>
                    </td>
				</tr>
				<tr>
                    <td><label for="ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.ProductId, ViewData["ProductList"] as SelectList, "Please Select...")%><span class="stateProvinceCodeError error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ProductId)%>
						 <label id="lblProductMsg"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Supplier" title="Create Supplier" class="red"/></td>
                </tr>
            </table>
		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/Supplier.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Supplier", "Main", new { controller = "Supplier", action = "List", }, new { title = "Supplier" })%> &gt;
Create Supplier
</asp:Content>