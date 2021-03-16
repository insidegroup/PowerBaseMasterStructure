<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CreditCard>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Credit Cards
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">CWT Credit Cards</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(true); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete = "off" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
    		<table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Add Credit Card</th> 
		        </tr>           
                 <tr>
                    <td><label for="ClientTopUnitName">Client TopUnit</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ClientTopUnitName, new { maxlength = "50", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitName)%><%= Html.Hidden("ClientTopUnitGuid")%>
                        <label id="lblClientTopUnitNameMsg"/></td>
                </tr> 
                <tr>
                    <td><label for="CreditCardTypeId">Credit Card Type</label></td>
                    <td><%= Html.DropDownList("CreditCardTypeId", ViewData["CreditCardTypeList"] as SelectList, "Please Select...", new { autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCardTypeId)%></td>
                </tr>  
                <tr>
                    <td><label for="CreditCardHolderName">Credit Card Holder Name</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCardHolderName, new { maxlength = "100", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCardHolderName)%></td>
                </tr> 
                 <tr>
                    <td><label for="CreditCardNumber">Credit Card Number</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCardNumber, new { maxlength = "24", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardNumber)%> </td>
                </tr> 
                <tr>
                    <td><label for="CreditCardVendorCode">Credit Card Vendor</label></td>
                     <td><%= Html.DropDownList("CreditCardVendorCode", ViewData["CreditCardVendors"] as SelectList, "Please Select...", new { autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCardVendorCode)%></td>
                </tr>   
                <tr>
                    <td><label for="CreditCardValidFrom">Credit Card Valid From</label></td>
                    <td> <%= Html.EditorFor(model => model.CreditCardValidFrom, new { maxlength = "8", autocomplete = "off" })%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardValidFrom)%> </td>
                </tr>
                  <tr>
                    <td><label for="CreditCardValidTo">Credit Card Valid To </label></td>
                    <td><%= Html.TextBox("CreditCardValidTo", "", new { maxlength = "8", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardValidTo)%> </td>
                </tr>
               <tr>
                    <td><label for="CreditCardIssueNumber">Issue Number</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCardIssueNumber, new { maxlength = "3", autocomplete = "off" })%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardIssueNumber)%> </td>
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
                    <td class="row_footer_blank_right"><input type="submit" value="Create Credit Card" title="Create Credit Card" class="red"/></td>
                </tr>
            </table>        
            <%= Html.HiddenFor(model => model.CreditCardId) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
            <%= Html.Hidden("OriginalCreditCardNumber", "")%>
            <%= Html.Hidden("CanHaveRealCreditCardsFlag", Model.CanHaveRealCreditCardsFlag)%>
            <%= Html.Hidden("WarningShownFlag", Model.WarningShownFlag)%>
    <% } %>


        </div>
    </div>

<%if (Model.CanHaveRealCreditCardsFlag)
  {%>
    <script src="<%=Url.Content("~/Scripts/ERD/CreditCardCWT.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/ERD/ValidCreditCardNumbersAllowed2.js")%>" type="text/javascript"></script>
<%}else{ %>
    <script src="<%=Url.Content("~/Scripts/ERD/CreditCardCWT.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/ERD/ValidCreditCardNumbersNotAllowed2.js")%>" type="text/javascript"></script>
<%} %>
</asp:Content>

