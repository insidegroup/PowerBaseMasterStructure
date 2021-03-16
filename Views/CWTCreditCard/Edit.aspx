<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CreditCard>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Credit Cards
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">CWT Credit Cards</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm("Edit", "CWTCreditCard", new { id = Model.CreditCardId }, FormMethod.Post, new { autocomplete = "off" })) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Credit Card</th> 
		        </tr> 
                <tr>
                    <td>Client TopUnit</td>
                    <td><%= Html.Encode(Model.ClientTopUnitName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Credit Card Type</td>
                    <td><%= Html.Encode(Model.CreditCardTypeDescription)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.ProductName)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.SupplierName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Credit Card Holder Name</td>
                    <td><%= Html.Encode(Model.CreditCardHolderName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Credit Card Number</td>
                    <td><%= Html.Encode(Model.MaskedCreditCardNumber)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>CVV</td>
                    <td><%= Html.Encode(Model.CreditCardVendorName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Credit Card Valid From</td>
                    <td><%= Html.Encode(Model.CreditCardValidFrom.HasValue ? Model.CreditCardValidFrom.Value.ToString("MMM yyyy") : "No ValidFrom Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Credit Card Valid To</td>
                    <td><%= Html.Encode(Model.CreditCardValidTo.ToString("MMM yyyy"))%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Issue Number</td>
                    <td><%= Html.Encode(Model.CreditCardIssueNumber)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...", new { autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label id="lblHierarchyItem"/>HierarchyItem</td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled="disabled",  size = "30", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr style="display:none" id="TravelerType">
                    <td><%= Html.LabelFor(model => model.TravelerTypeGuid)%></td>
                    <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.TravelerTypeGuid)%>
                        <%= Html.Hidden("TravelerTypeGuid")%>
                        <label id="lblTravelerTypeMsg"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Credit Card" title="Edit Credit Card" class="red"/></td>
                </tr>
             
            </table>
            <%= Html.Hidden("ClientTopUnitGuid",Model.ClientTopUnitGuid) %>
            <%= Html.HiddenFor(model => model.CreditCardTypeId) %>
            <%= Html.HiddenFor(model => model.CreditCardId) %>
            <%= Html.HiddenFor(model => model.VersionNumber) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
             <%= Html.Hidden("OriginalCreditCardNumber", Model.CreditCardNumber)%>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
        <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/CreditCardCWT.js")%>" type="text/javascript"></script>
</asp:Content>


