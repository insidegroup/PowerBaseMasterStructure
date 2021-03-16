<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldItemVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Groups</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/OptionalFieldItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Group Items</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Client Fee Item</th> 
		        </tr> 
                 <tr>
                    <td>Optional Field Group</td>
                    <td><%=Model.OptionalFieldItem.OptionalFieldGroup.OptionalFieldGroupName %></td>
                    <td></td>
                </tr>   
                <tr>
                    <td><label for="OptionalFieldItem_OptionalFieldId">Optional Field</label></td>
                    <td><%= Html.DropDownListFor(model => model.OptionalFieldItem.OptionalFieldId, Model.OptionalFields, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldItem.OptionalFieldId)%></td>
                </tr>  
				<tr>
                    <td><label for="OptionalFieldItem_ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.OptionalFieldItem.ProductId, ViewData["ProductList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldItem.ProductId)%></td>
                </tr> 
                <tr>
                    <td><label for="OptionalFieldItem_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalFieldItem.SupplierName)%></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.OptionalFieldItem.SupplierName)%>
                        <%= Html.HiddenFor(model => model.OptionalFieldItem.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
				<tr>
                    <td><label for="OptionalFieldGroup_Mandatory">Mandatory?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.OptionalFieldItem.Mandatory)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldItem.Mandatory)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Optional Field Group Item" title="Edit Optional Field Group Item" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.OptionalFieldItem.OptionalFieldItemId) %>
            <%= Html.HiddenFor(model => model.OptionalFieldItem.VersionNumber) %>
    <% } %>
    </div>
</div>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Optional Field Groups", "Main", new { controller = "OptionalFieldGroup", action = "ListUnDeleted" }, new { title = "Optional Field Groups" })%> &gt;
<%=Html.RouteLink(Model.OptionalFieldItem.OptionalFieldGroup.OptionalFieldGroupName, "Main", new { controller = "optionalFieldGroup", action = "View", id = Model.OptionalFieldItem.OptionalFieldGroupId }, new { title = Model.OptionalFieldItem.OptionalFieldGroup.OptionalFieldGroupName })%> &gt;
Optional Field Group Items
</asp:Content>

